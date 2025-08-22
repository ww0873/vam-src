using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Hair.Scripts.Settings;
using GPUTools.Hair.Scripts.Settings.Colors;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
	// Token: 0x02000A17 RID: 2583
	public class BuildParticlesData : IBuildCommand
	{
		// Token: 0x06004179 RID: 16761 RVA: 0x0013796D File Offset: 0x00135D6D
		public BuildParticlesData(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x0013797C File Offset: 0x00135D7C
		public void Build()
		{
			RenderParticle[] array;
			if (this.settings.RuntimeData.Particles != null)
			{
				array = new RenderParticle[this.settings.RuntimeData.Particles.Count];
			}
			else
			{
				array = new RenderParticle[0];
			}
			this.UpdateBodies(array);
			List<Vector3> list = this.GenRandoms();
			if (this.settings.RuntimeData.RenderParticles != null)
			{
				this.settings.RuntimeData.RenderParticles.Dispose();
			}
			if (this.settings.RuntimeData.RandomsPerStrand != null)
			{
				this.settings.RuntimeData.RandomsPerStrand.Dispose();
			}
			if (array.Length > 0)
			{
				this.settings.RuntimeData.RenderParticles = new GpuBuffer<RenderParticle>(array, RenderParticle.Size());
				this.settings.RuntimeData.RandomsPerStrand = new GpuBuffer<Vector3>(list.ToArray(), 12);
			}
			else
			{
				this.settings.RuntimeData.RenderParticles = null;
				this.settings.RuntimeData.RandomsPerStrand = null;
			}
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x00137A90 File Offset: 0x00135E90
		public void UpdateSettings()
		{
			if (this.settings.RuntimeData.RenderParticles != null)
			{
				this.UpdateBodies(this.settings.RuntimeData.RenderParticles.Data);
				this.settings.RuntimeData.RenderParticles.PushData();
			}
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x00137AE4 File Offset: 0x00135EE4
		private List<Vector3> GenRandoms()
		{
			List<Vector3> list = new List<Vector3>();
			UnityEngine.Random.InitState(5);
			for (int i = 0; i < this.settings.StandsSettings.Provider.GetStandsNum(); i++)
			{
				Vector3 item;
				item.x = UnityEngine.Random.Range(0f, 1f);
				item.y = UnityEngine.Random.Range(0f, 1f);
				item.z = UnityEngine.Random.Range(0f, 1f);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x00137B70 File Offset: 0x00135F70
		private void UpdateBodies(RenderParticle[] renderParticles)
		{
			HairRenderSettings renderSettings = this.settings.RenderSettings;
			int segmentsNum = this.settings.StandsSettings.Provider.GetSegmentsNum();
			RootTipColorProvider rootTipColorProvider = null;
			if (renderSettings.ColorProvider is RootTipColorProvider)
			{
				rootTipColorProvider = (renderSettings.ColorProvider as RootTipColorProvider);
			}
			float[] particleRootToTipRatios = this.settings.RuntimeData.ParticleRootToTipRatios;
			float num = Mathf.Clamp(renderSettings.InterpolationMidpoint, 0.001f, 1f);
			float num2 = Mathf.Clamp(renderSettings.WavinessMidpoint, 0.001f, 1f);
			for (int i = 0; i < renderParticles.Length; i++)
			{
				int num3 = i / segmentsNum;
				int num4 = i % segmentsNum;
				float num5 = (float)num4 / (float)(segmentsNum - 1);
				Vector3 color;
				if (rootTipColorProvider != null)
				{
					float y = particleRootToTipRatios[i];
					color = this.ColorToVector(rootTipColorProvider.GetColor(this.settings, y));
				}
				else
				{
					color = this.ColorToVector(renderSettings.ColorProvider.GetColor(this.settings, num3, num4, segmentsNum));
				}
				float interpolation;
				if (this.settings.PhysicsSettings.StyleMode)
				{
					interpolation = 0f;
				}
				else if (renderSettings.UseInterpolationCurves)
				{
					interpolation = Mathf.Clamp01(renderSettings.InterpolationCurve.Evaluate(num5));
				}
				else if (num5 <= renderSettings.InterpolationMidpoint)
				{
					float num6 = num5 / num;
					float f = 1f - num6;
					float t = Mathf.Pow(f, renderSettings.InterpolationCurvePower);
					interpolation = 1f - Mathf.Lerp(renderSettings.InterpolationMid, renderSettings.InterpolationRoot, t);
				}
				else
				{
					float f2 = (num5 - num) / (1f - num);
					float t2 = Mathf.Pow(f2, renderSettings.InterpolationCurvePower);
					interpolation = 1f - Mathf.Lerp(renderSettings.InterpolationMid, renderSettings.InterpolationTip, t2);
				}
				float num7;
				float num8;
				if (this.settings.PhysicsSettings.StyleMode)
				{
					num7 = 0f;
					num8 = 1f;
				}
				else if (renderSettings.UseWavinessCurves)
				{
					num7 = Mathf.Clamp01(renderSettings.WavinessScaleCurve.Evaluate(num5));
					num8 = Mathf.Clamp01(renderSettings.WavinessFrequencyCurve.Evaluate(num5));
				}
				else
				{
					if (num5 <= renderSettings.WavinessMidpoint)
					{
						float num9 = num5 / num2;
						float f3 = 1f - num9;
						float t3 = Mathf.Pow(f3, renderSettings.WavinessCurvePower);
						num7 = Mathf.Lerp(renderSettings.WavinessMid, renderSettings.WavinessRoot, t3);
					}
					else
					{
						float f4 = (num5 - num2) / (1f - num2);
						float t4 = Mathf.Pow(f4, renderSettings.WavinessCurvePower);
						num7 = Mathf.Lerp(renderSettings.WavinessMid, renderSettings.WavinessTip, t4);
					}
					num8 = 1f;
				}
				RenderParticle renderParticle = new RenderParticle
				{
					RootIndex = num3,
					Color = color,
					Interpolation = interpolation,
					WavinessScale = num7 * renderSettings.WavinessScale,
					WavinessFrequency = num8 * renderSettings.WavinessFrequency
				};
				renderParticles[i] = renderParticle;
			}
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x00137E81 File Offset: 0x00136281
		public Vector3 ColorToVector(Color color)
		{
			return new Vector3(color.r, color.g, color.b);
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x00137E9D File Offset: 0x0013629D
		public void Dispatch()
		{
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x00137E9F File Offset: 0x0013629F
		public void FixedDispatch()
		{
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00137EA4 File Offset: 0x001362A4
		public void Dispose()
		{
			if (this.settings.RuntimeData.RenderParticles != null)
			{
				this.settings.RuntimeData.RenderParticles.Dispose();
			}
			if (this.settings.RuntimeData.RandomsPerStrand != null)
			{
				this.settings.RuntimeData.RandomsPerStrand.Dispose();
			}
		}

		// Token: 0x04003115 RID: 12565
		private readonly HairSettings settings;
	}
}
