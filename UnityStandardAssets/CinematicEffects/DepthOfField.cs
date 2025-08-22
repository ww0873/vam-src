using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x02000E3B RID: 3643
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Other/DepthOfField")]
	[RequireComponent(typeof(Camera))]
	public class DepthOfField : MonoBehaviour
	{
		// Token: 0x06007034 RID: 28724 RVA: 0x002A37C0 File Offset: 0x002A1BC0
		public DepthOfField()
		{
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06007035 RID: 28725 RVA: 0x002A3889 File Offset: 0x002A1C89
		public Material filmicDepthOfFieldMaterial
		{
			get
			{
				if (this.m_FilmicDepthOfFieldMaterial == null)
				{
					this.m_FilmicDepthOfFieldMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(this.filmicDepthOfFieldShader);
				}
				return this.m_FilmicDepthOfFieldMaterial;
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06007036 RID: 28726 RVA: 0x002A38B3 File Offset: 0x002A1CB3
		public Material medianFilterMaterial
		{
			get
			{
				if (this.m_MedianFilterMaterial == null)
				{
					this.m_MedianFilterMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(this.medianFilterShader);
				}
				return this.m_MedianFilterMaterial;
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06007037 RID: 28727 RVA: 0x002A38DD File Offset: 0x002A1CDD
		public Material textureBokehMaterial
		{
			get
			{
				if (this.m_TextureBokehMaterial == null)
				{
					this.m_TextureBokehMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(this.textureBokehShader);
				}
				return this.m_TextureBokehMaterial;
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06007038 RID: 28728 RVA: 0x002A3908 File Offset: 0x002A1D08
		public ComputeBuffer computeBufferDrawArgs
		{
			get
			{
				if (this.m_ComputeBufferDrawArgs == null)
				{
					this.m_ComputeBufferDrawArgs = new ComputeBuffer(1, 16, ComputeBufferType.DrawIndirect);
					int[] data = new int[]
					{
						0,
						1,
						0,
						0
					};
					this.m_ComputeBufferDrawArgs.SetData(data);
				}
				return this.m_ComputeBufferDrawArgs;
			}
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06007039 RID: 28729 RVA: 0x002A395C File Offset: 0x002A1D5C
		public ComputeBuffer computeBufferPoints
		{
			get
			{
				if (this.m_ComputeBufferPoints == null)
				{
					this.m_ComputeBufferPoints = new ComputeBuffer(90000, 28, ComputeBufferType.Append);
				}
				return this.m_ComputeBufferPoints;
			}
		}

		// Token: 0x0600703A RID: 28730 RVA: 0x002A3984 File Offset: 0x002A1D84
		protected void OnEnable()
		{
			if (this.filmicDepthOfFieldShader == null)
			{
				this.filmicDepthOfFieldShader = Shader.Find("Hidden/DepthOfField/DepthOfField");
			}
			if (this.medianFilterShader == null)
			{
				this.medianFilterShader = Shader.Find("Hidden/DepthOfField/MedianFilter");
			}
			if (this.textureBokehShader == null)
			{
				this.textureBokehShader = Shader.Find("Hidden/DepthOfField/BokehSplatting");
			}
			if (!ImageEffectHelper.IsSupported(this.filmicDepthOfFieldShader, true, true, this) || !ImageEffectHelper.IsSupported(this.medianFilterShader, true, true, this))
			{
				base.enabled = false;
				Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
				return;
			}
			if (ImageEffectHelper.supportsDX11 && !ImageEffectHelper.IsSupported(this.textureBokehShader, true, true, this))
			{
				base.enabled = false;
				Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
				return;
			}
			this.ComputeBlurDirections(true);
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600703B RID: 28731 RVA: 0x002A3A98 File Offset: 0x002A1E98
		protected void OnDisable()
		{
			this.ReleaseComputeResources();
			if (this.m_FilmicDepthOfFieldMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_FilmicDepthOfFieldMaterial);
			}
			if (this.m_TextureBokehMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_TextureBokehMaterial);
			}
			if (this.m_MedianFilterMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MedianFilterMaterial);
			}
			this.m_TextureBokehMaterial = null;
			this.m_FilmicDepthOfFieldMaterial = null;
			this.m_MedianFilterMaterial = null;
			this.m_RTU.ReleaseAllTemporyRenderTexutres();
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
		}

		// Token: 0x0600703C RID: 28732 RVA: 0x002A3B28 File Offset: 0x002A1F28
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.skipEffect || this.medianFilterMaterial == null || this.filmicDepthOfFieldMaterial == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.visualizeBluriness)
			{
				Vector4 value;
				Vector4 value2;
				this.ComputeCocParameters(out value, out value2);
				this.filmicDepthOfFieldMaterial.SetVector("_BlurParams", value);
				this.filmicDepthOfFieldMaterial.SetVector("_BlurCoe", value2);
				Graphics.Blit(null, destination, this.filmicDepthOfFieldMaterial, (this.uiMode != DepthOfField.UIMode.Explicit) ? 6 : 7);
			}
			else
			{
				this.DoDepthOfField(source, destination);
			}
			this.m_RTU.ReleaseAllTemporyRenderTexutres();
		}

		// Token: 0x0600703D RID: 28733 RVA: 0x002A3BDC File Offset: 0x002A1FDC
		private void DoDepthOfField(RenderTexture source, RenderTexture destination)
		{
			float num = (float)source.height / 720f;
			float num2 = num;
			float num3 = Mathf.Max(this.nearRadius, this.farRadius) * num2 * 0.75f;
			float num4 = this.nearRadius * num;
			float num5 = this.farRadius * num;
			float num6 = Mathf.Max(num4, num5);
			DepthOfField.ApertureShape apertureShape = this.apertureShape;
			if (apertureShape != DepthOfField.ApertureShape.Hexagonal)
			{
				if (apertureShape == DepthOfField.ApertureShape.Octogonal)
				{
					num6 *= 1.15f;
				}
			}
			else
			{
				num6 *= 1.2f;
			}
			if (num6 < 0.5f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width / 2;
			int height = source.height / 2;
			Vector4 value = new Vector4(num4 * 0.5f, num5 * 0.5f, 0f, 0f);
			RenderTexture temporaryRenderTexture = this.m_RTU.GetTemporaryRenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
			RenderTexture temporaryRenderTexture2 = this.m_RTU.GetTemporaryRenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
			Vector4 value2;
			Vector4 value3;
			this.ComputeCocParameters(out value2, out value3);
			this.filmicDepthOfFieldMaterial.SetVector("_BlurParams", value2);
			this.filmicDepthOfFieldMaterial.SetVector("_BlurCoe", value3);
			this.filmicDepthOfFieldMaterial.SetVector("_BoostParams", new Vector4(num4 * this.nearBoostAmount * -0.5f, num5 * this.farBoostAmount * 0.5f, this.boostPoint, 0f));
			Graphics.Blit(source, temporaryRenderTexture2, this.filmicDepthOfFieldMaterial, (this.uiMode != DepthOfField.UIMode.Explicit) ? 4 : 5);
			RenderTexture renderTexture = temporaryRenderTexture2;
			RenderTexture renderTexture2 = temporaryRenderTexture;
			if (this.shouldPerformBokeh)
			{
				RenderTexture temporaryRenderTexture3 = this.m_RTU.GetTemporaryRenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
				Graphics.Blit(renderTexture, temporaryRenderTexture3, this.filmicDepthOfFieldMaterial, 1);
				this.filmicDepthOfFieldMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f, 0f, 1.5f));
				Graphics.Blit(temporaryRenderTexture3, renderTexture2, this.filmicDepthOfFieldMaterial, 0);
				this.filmicDepthOfFieldMaterial.SetVector("_Offsets", new Vector4(1.5f, 0f, 0f, 1.5f));
				Graphics.Blit(renderTexture2, temporaryRenderTexture3, this.filmicDepthOfFieldMaterial, 0);
				this.textureBokehMaterial.SetTexture("_BlurredColor", temporaryRenderTexture3);
				this.textureBokehMaterial.SetFloat("_SpawnHeuristic", this.textureBokehSpawnHeuristic);
				this.textureBokehMaterial.SetVector("_BokehParams", new Vector4(this.textureBokehScale * num2, this.textureBokehIntensity, this.textureBokehThreshold, num3));
				Graphics.SetRandomWriteTarget(1, this.computeBufferPoints);
				Graphics.Blit(renderTexture, renderTexture2, this.textureBokehMaterial, 1);
				Graphics.ClearRandomWriteTargets();
				DepthOfField.SwapRenderTexture(ref renderTexture, ref renderTexture2);
				this.m_RTU.ReleaseTemporaryRenderTexture(temporaryRenderTexture3);
			}
			this.filmicDepthOfFieldMaterial.SetVector("_BlurParams", value2);
			this.filmicDepthOfFieldMaterial.SetVector("_BlurCoe", value);
			this.filmicDepthOfFieldMaterial.SetVector("_BoostParams", new Vector4(num4 * this.nearBoostAmount * -0.5f, num5 * this.farBoostAmount * 0.5f, this.boostPoint, 0f));
			RenderTexture renderTexture3 = null;
			if (this.dilateNearBlur)
			{
				RenderTexture temporaryRenderTexture4 = this.m_RTU.GetTemporaryRenderTexture(width, height, 0, RenderTextureFormat.RGHalf, FilterMode.Bilinear);
				renderTexture3 = this.m_RTU.GetTemporaryRenderTexture(width, height, 0, RenderTextureFormat.RGHalf, FilterMode.Bilinear);
				this.filmicDepthOfFieldMaterial.SetVector("_Offsets", new Vector4(0f, num4 * 0.75f, 0f, 0f));
				Graphics.Blit(renderTexture, temporaryRenderTexture4, this.filmicDepthOfFieldMaterial, 2);
				this.filmicDepthOfFieldMaterial.SetVector("_Offsets", new Vector4(num4 * 0.75f, 0f, 0f, 0f));
				Graphics.Blit(temporaryRenderTexture4, renderTexture3, this.filmicDepthOfFieldMaterial, 3);
				this.m_RTU.ReleaseTemporaryRenderTexture(temporaryRenderTexture4);
			}
			if (this.prefilterBlur)
			{
				Graphics.Blit(renderTexture, renderTexture2, this.filmicDepthOfFieldMaterial, 8);
				DepthOfField.SwapRenderTexture(ref renderTexture, ref renderTexture2);
			}
			DepthOfField.ApertureShape apertureShape2 = this.apertureShape;
			if (apertureShape2 != DepthOfField.ApertureShape.Circular)
			{
				if (apertureShape2 != DepthOfField.ApertureShape.Hexagonal)
				{
					if (apertureShape2 == DepthOfField.ApertureShape.Octogonal)
					{
						this.DoOctogonalBlur(renderTexture3, ref renderTexture, ref renderTexture2, num6);
					}
				}
				else
				{
					this.DoHexagonalBlur(renderTexture3, ref renderTexture, ref renderTexture2, num6);
				}
			}
			else
			{
				this.DoCircularBlur(renderTexture3, ref renderTexture, ref renderTexture2, num6);
			}
			DepthOfField.FilterQuality filterQuality = this.medianFilter;
			if (filterQuality != DepthOfField.FilterQuality.Normal)
			{
				if (filterQuality == DepthOfField.FilterQuality.High)
				{
					Graphics.Blit(renderTexture, renderTexture2, this.medianFilterMaterial, 1);
					DepthOfField.SwapRenderTexture(ref renderTexture, ref renderTexture2);
				}
			}
			else
			{
				this.medianFilterMaterial.SetVector("_Offsets", new Vector4(1f, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, renderTexture2, this.medianFilterMaterial, 0);
				DepthOfField.SwapRenderTexture(ref renderTexture, ref renderTexture2);
				this.medianFilterMaterial.SetVector("_Offsets", new Vector4(0f, 1f, 0f, 0f));
				Graphics.Blit(renderTexture, renderTexture2, this.medianFilterMaterial, 0);
				DepthOfField.SwapRenderTexture(ref renderTexture, ref renderTexture2);
			}
			this.filmicDepthOfFieldMaterial.SetVector("_BlurCoe", value);
			this.filmicDepthOfFieldMaterial.SetVector("_Convolved_TexelSize", new Vector4((float)renderTexture.width, (float)renderTexture.height, 1f / (float)renderTexture.width, 1f / (float)renderTexture.height));
			this.filmicDepthOfFieldMaterial.SetTexture("_SecondTex", renderTexture);
			int pass = (this.uiMode != DepthOfField.UIMode.Explicit) ? 13 : 14;
			if (this.highQualityUpsampling)
			{
				pass = ((this.uiMode != DepthOfField.UIMode.Explicit) ? 15 : 16);
			}
			if (this.shouldPerformBokeh)
			{
				RenderTexture temporaryRenderTexture5 = this.m_RTU.GetTemporaryRenderTexture(source.height, source.width, 0, source.format, FilterMode.Bilinear);
				Graphics.Blit(source, temporaryRenderTexture5, this.filmicDepthOfFieldMaterial, pass);
				Graphics.SetRenderTarget(temporaryRenderTexture5);
				ComputeBuffer.CopyCount(this.computeBufferPoints, this.computeBufferDrawArgs, 0);
				this.textureBokehMaterial.SetBuffer("pointBuffer", this.computeBufferPoints);
				this.textureBokehMaterial.SetTexture("_MainTex", this.bokehTexture);
				this.textureBokehMaterial.SetVector("_Screen", new Vector3(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), num3));
				this.textureBokehMaterial.SetPass(0);
				Graphics.DrawProceduralIndirect(MeshTopology.Points, this.computeBufferDrawArgs, 0);
				Graphics.Blit(temporaryRenderTexture5, destination);
			}
			else
			{
				Graphics.Blit(source, destination, this.filmicDepthOfFieldMaterial, pass);
			}
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x002A4290 File Offset: 0x002A2690
		private void DoHexagonalBlur(RenderTexture blurredFgCoc, ref RenderTexture src, ref RenderTexture dst, float maxRadius)
		{
			this.ComputeBlurDirections(false);
			int pass;
			int pass2;
			DepthOfField.GetDirectionalBlurPassesFromRadius(blurredFgCoc, maxRadius, out pass, out pass2);
			this.filmicDepthOfFieldMaterial.SetTexture("_SecondTex", blurredFgCoc);
			RenderTexture temporaryRenderTexture = this.m_RTU.GetTemporaryRenderTexture(src.width, src.height, 0, src.format, FilterMode.Bilinear);
			this.filmicDepthOfFieldMaterial.SetVector("_Offsets", this.m_HexagonalBokehDirection1);
			Graphics.Blit(src, temporaryRenderTexture, this.filmicDepthOfFieldMaterial, pass);
			this.filmicDepthOfFieldMaterial.SetVector("_Offsets", this.m_HexagonalBokehDirection2);
			Graphics.Blit(temporaryRenderTexture, src, this.filmicDepthOfFieldMaterial, pass);
			this.filmicDepthOfFieldMaterial.SetVector("_Offsets", this.m_HexagonalBokehDirection3);
			this.filmicDepthOfFieldMaterial.SetTexture("_ThirdTex", src);
			Graphics.Blit(temporaryRenderTexture, dst, this.filmicDepthOfFieldMaterial, pass2);
			this.m_RTU.ReleaseTemporaryRenderTexture(temporaryRenderTexture);
			DepthOfField.SwapRenderTexture(ref src, ref dst);
		}

		// Token: 0x0600703F RID: 28735 RVA: 0x002A4378 File Offset: 0x002A2778
		private void DoOctogonalBlur(RenderTexture blurredFgCoc, ref RenderTexture src, ref RenderTexture dst, float maxRadius)
		{
			this.ComputeBlurDirections(false);
			int pass;
			int pass2;
			DepthOfField.GetDirectionalBlurPassesFromRadius(blurredFgCoc, maxRadius, out pass, out pass2);
			this.filmicDepthOfFieldMaterial.SetTexture("_SecondTex", blurredFgCoc);
			RenderTexture temporaryRenderTexture = this.m_RTU.GetTemporaryRenderTexture(src.width, src.height, 0, src.format, FilterMode.Bilinear);
			this.filmicDepthOfFieldMaterial.SetVector("_Offsets", this.m_OctogonalBokehDirection1);
			Graphics.Blit(src, temporaryRenderTexture, this.filmicDepthOfFieldMaterial, pass);
			this.filmicDepthOfFieldMaterial.SetVector("_Offsets", this.m_OctogonalBokehDirection2);
			Graphics.Blit(temporaryRenderTexture, dst, this.filmicDepthOfFieldMaterial, pass);
			this.filmicDepthOfFieldMaterial.SetVector("_Offsets", this.m_OctogonalBokehDirection3);
			Graphics.Blit(src, temporaryRenderTexture, this.filmicDepthOfFieldMaterial, pass);
			this.filmicDepthOfFieldMaterial.SetVector("_Offsets", this.m_OctogonalBokehDirection4);
			this.filmicDepthOfFieldMaterial.SetTexture("_ThirdTex", dst);
			Graphics.Blit(temporaryRenderTexture, src, this.filmicDepthOfFieldMaterial, pass2);
			this.m_RTU.ReleaseTemporaryRenderTexture(temporaryRenderTexture);
		}

		// Token: 0x06007040 RID: 28736 RVA: 0x002A4480 File Offset: 0x002A2880
		private void DoCircularBlur(RenderTexture blurredFgCoc, ref RenderTexture src, ref RenderTexture dst, float maxRadius)
		{
			int pass;
			if (blurredFgCoc != null)
			{
				this.filmicDepthOfFieldMaterial.SetTexture("_SecondTex", blurredFgCoc);
				pass = ((maxRadius <= 10f) ? 12 : 10);
			}
			else
			{
				pass = ((maxRadius <= 10f) ? 11 : 9);
			}
			Graphics.Blit(src, dst, this.filmicDepthOfFieldMaterial, pass);
			DepthOfField.SwapRenderTexture(ref src, ref dst);
		}

		// Token: 0x06007041 RID: 28737 RVA: 0x002A44F4 File Offset: 0x002A28F4
		private void ComputeCocParameters(out Vector4 blurParams, out Vector4 blurCoe)
		{
			Camera component = base.GetComponent<Camera>();
			float num = (!this.focusTransform) ? (this.focusPlane * this.focusPlane * this.focusPlane * this.focusPlane) : (component.WorldToViewportPoint(this.focusTransform.position).z / component.farClipPlane);
			if (this.uiMode == DepthOfField.UIMode.Basic || this.uiMode == DepthOfField.UIMode.Advanced)
			{
				float w = this.focusRange * this.focusRange * this.focusRange * this.focusRange;
				float num2 = 4f / Mathf.Tan(0.5f * component.fieldOfView * 0.017453292f);
				float x = num2 / this.fStops;
				blurCoe = new Vector4(0f, 0f, 1f, 1f);
				blurParams = new Vector4(x, num2, num, w);
			}
			else
			{
				float num3 = this.nearPlane * this.nearPlane * this.nearPlane * this.nearPlane;
				float num4 = this.farPlane * this.farPlane * this.farPlane * this.farPlane;
				float num5 = this.focusRange * this.focusRange * this.focusRange * this.focusRange;
				float num6 = num5;
				if (num <= num3)
				{
					num = num3 + 1E-07f;
				}
				if (num >= num4)
				{
					num = num4 - 1E-07f;
				}
				if (num - num5 <= num3)
				{
					num5 = num - num3 - 1E-07f;
				}
				if (num + num6 >= num4)
				{
					num6 = num4 - num - 1E-07f;
				}
				float num7 = 1f / (num3 - num + num5);
				float num8 = 1f / (num4 - num - num6);
				float num9 = 1f - num7 * num3;
				float num10 = 1f - num8 * num4;
				blurParams = new Vector4(-1f * num7, -1f * num9, 1f * num8, 1f * num10);
				blurCoe = new Vector4(0f, 0f, (num10 - num9) / (num7 - num8), 0f);
			}
		}

		// Token: 0x06007042 RID: 28738 RVA: 0x002A470A File Offset: 0x002A2B0A
		private void ReleaseComputeResources()
		{
			if (this.m_ComputeBufferDrawArgs != null)
			{
				this.m_ComputeBufferDrawArgs.Release();
			}
			this.m_ComputeBufferDrawArgs = null;
			if (this.m_ComputeBufferPoints != null)
			{
				this.m_ComputeBufferPoints.Release();
			}
			this.m_ComputeBufferPoints = null;
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x002A4748 File Offset: 0x002A2B48
		private void ComputeBlurDirections(bool force)
		{
			if (!force && Math.Abs(this.m_LastApertureOrientation - this.apertureOrientation) < 1E-45f)
			{
				return;
			}
			this.m_LastApertureOrientation = this.apertureOrientation;
			float num = this.apertureOrientation * 0.017453292f;
			float cosinus = Mathf.Cos(num);
			float sinus = Mathf.Sin(num);
			this.m_OctogonalBokehDirection1 = new Vector4(0.5f, 0f, 0f, 0f);
			this.m_OctogonalBokehDirection2 = new Vector4(0f, 0.5f, 1f, 0f);
			this.m_OctogonalBokehDirection3 = new Vector4(-0.353553f, 0.353553f, 1f, 0f);
			this.m_OctogonalBokehDirection4 = new Vector4(0.353553f, 0.353553f, 1f, 0f);
			this.m_HexagonalBokehDirection1 = new Vector4(0.5f, 0f, 0f, 0f);
			this.m_HexagonalBokehDirection2 = new Vector4(0.25f, 0.433013f, 1f, 0f);
			this.m_HexagonalBokehDirection3 = new Vector4(0.25f, -0.433013f, 1f, 0f);
			if (num > 1E-45f)
			{
				DepthOfField.Rotate2D(ref this.m_OctogonalBokehDirection1, cosinus, sinus);
				DepthOfField.Rotate2D(ref this.m_OctogonalBokehDirection2, cosinus, sinus);
				DepthOfField.Rotate2D(ref this.m_OctogonalBokehDirection3, cosinus, sinus);
				DepthOfField.Rotate2D(ref this.m_OctogonalBokehDirection4, cosinus, sinus);
				DepthOfField.Rotate2D(ref this.m_HexagonalBokehDirection1, cosinus, sinus);
				DepthOfField.Rotate2D(ref this.m_HexagonalBokehDirection2, cosinus, sinus);
				DepthOfField.Rotate2D(ref this.m_HexagonalBokehDirection3, cosinus, sinus);
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06007044 RID: 28740 RVA: 0x002A48DE File Offset: 0x002A2CDE
		private bool shouldPerformBokeh
		{
			get
			{
				return ImageEffectHelper.supportsDX11 && this.useBokehTexture && this.textureBokehMaterial;
			}
		}

		// Token: 0x06007045 RID: 28741 RVA: 0x002A4904 File Offset: 0x002A2D04
		private static void Rotate2D(ref Vector4 direction, float cosinus, float sinus)
		{
			Vector4 vector = direction;
			direction.x = vector.x * cosinus - vector.y * sinus;
			direction.y = vector.x * sinus + vector.y * cosinus;
		}

		// Token: 0x06007046 RID: 28742 RVA: 0x002A494C File Offset: 0x002A2D4C
		private static void SwapRenderTexture(ref RenderTexture src, ref RenderTexture dst)
		{
			RenderTexture renderTexture = dst;
			dst = src;
			src = renderTexture;
		}

		// Token: 0x06007047 RID: 28743 RVA: 0x002A4964 File Offset: 0x002A2D64
		private static void GetDirectionalBlurPassesFromRadius(RenderTexture blurredFgCoc, float maxRadius, out int blurPass, out int blurAndMergePass)
		{
			if (blurredFgCoc == null)
			{
				if (maxRadius > 10f)
				{
					blurPass = 25;
					blurAndMergePass = 27;
				}
				else if (maxRadius > 5f)
				{
					blurPass = 21;
					blurAndMergePass = 23;
				}
				else
				{
					blurPass = 17;
					blurAndMergePass = 19;
				}
			}
			else if (maxRadius > 10f)
			{
				blurPass = 26;
				blurAndMergePass = 28;
			}
			else if (maxRadius > 5f)
			{
				blurPass = 22;
				blurAndMergePass = 24;
			}
			else
			{
				blurPass = 18;
				blurAndMergePass = 20;
			}
		}

		// Token: 0x040061F9 RID: 25081
		private const float kMaxBlur = 35f;

		// Token: 0x040061FA RID: 25082
		[Tooltip("Allow to view where the blur will be applied. Yellow for near blur, Blue for far blur.")]
		public bool visualizeBluriness;

		// Token: 0x040061FB RID: 25083
		[Tooltip("When enabled quality settings can be hand picked, rather than being driven by the quality slider.")]
		public bool customizeQualitySettings;

		// Token: 0x040061FC RID: 25084
		public bool skipEffect;

		// Token: 0x040061FD RID: 25085
		public bool prefilterBlur = true;

		// Token: 0x040061FE RID: 25086
		public DepthOfField.FilterQuality medianFilter = DepthOfField.FilterQuality.High;

		// Token: 0x040061FF RID: 25087
		public bool dilateNearBlur = true;

		// Token: 0x04006200 RID: 25088
		public bool highQualityUpsampling = true;

		// Token: 0x04006201 RID: 25089
		[DepthOfField.GradientRangeAttribute(0f, 100f)]
		[Tooltip("Color represent relative performance. From green (faster) to yellow (slower).")]
		public float quality = 100f;

		// Token: 0x04006202 RID: 25090
		[Range(0f, 1f)]
		public float focusPlane = 0.225f;

		// Token: 0x04006203 RID: 25091
		[Range(0f, 1f)]
		public float focusRange = 0.9f;

		// Token: 0x04006204 RID: 25092
		[Range(0f, 1f)]
		public float nearPlane;

		// Token: 0x04006205 RID: 25093
		[Range(0f, 35f)]
		public float nearRadius = 20f;

		// Token: 0x04006206 RID: 25094
		[Range(0f, 1f)]
		public float farPlane = 1f;

		// Token: 0x04006207 RID: 25095
		[Range(0f, 35f)]
		public float farRadius = 20f;

		// Token: 0x04006208 RID: 25096
		[Range(0f, 35f)]
		public float radius = 20f;

		// Token: 0x04006209 RID: 25097
		[Range(0.5f, 4f)]
		public float boostPoint = 0.75f;

		// Token: 0x0400620A RID: 25098
		[Range(0f, 1f)]
		public float nearBoostAmount;

		// Token: 0x0400620B RID: 25099
		[Range(0f, 1f)]
		public float farBoostAmount;

		// Token: 0x0400620C RID: 25100
		[Range(0f, 32f)]
		public float fStops = 5f;

		// Token: 0x0400620D RID: 25101
		[Range(0.01f, 5f)]
		public float textureBokehScale = 1f;

		// Token: 0x0400620E RID: 25102
		[Range(0.01f, 100f)]
		public float textureBokehIntensity = 50f;

		// Token: 0x0400620F RID: 25103
		[Range(0.01f, 50f)]
		public float textureBokehThreshold = 2f;

		// Token: 0x04006210 RID: 25104
		[Range(0.01f, 1f)]
		public float textureBokehSpawnHeuristic = 0.15f;

		// Token: 0x04006211 RID: 25105
		public Transform focusTransform;

		// Token: 0x04006212 RID: 25106
		public Texture2D bokehTexture;

		// Token: 0x04006213 RID: 25107
		public DepthOfField.ApertureShape apertureShape;

		// Token: 0x04006214 RID: 25108
		[Range(0f, 179f)]
		public float apertureOrientation;

		// Token: 0x04006215 RID: 25109
		[Tooltip("Use with care Bokeh texture are only available on shader model 5, and performance scale with the number of bokehs.")]
		public bool useBokehTexture;

		// Token: 0x04006216 RID: 25110
		public DepthOfField.UIMode uiMode;

		// Token: 0x04006217 RID: 25111
		public Shader filmicDepthOfFieldShader;

		// Token: 0x04006218 RID: 25112
		public Shader medianFilterShader;

		// Token: 0x04006219 RID: 25113
		public Shader textureBokehShader;

		// Token: 0x0400621A RID: 25114
		[NonSerialized]
		private RenderTexureUtility m_RTU = new RenderTexureUtility();

		// Token: 0x0400621B RID: 25115
		private ComputeBuffer m_ComputeBufferDrawArgs;

		// Token: 0x0400621C RID: 25116
		private ComputeBuffer m_ComputeBufferPoints;

		// Token: 0x0400621D RID: 25117
		private Material m_FilmicDepthOfFieldMaterial;

		// Token: 0x0400621E RID: 25118
		private Material m_MedianFilterMaterial;

		// Token: 0x0400621F RID: 25119
		private Material m_TextureBokehMaterial;

		// Token: 0x04006220 RID: 25120
		private float m_LastApertureOrientation;

		// Token: 0x04006221 RID: 25121
		private Vector4 m_OctogonalBokehDirection1;

		// Token: 0x04006222 RID: 25122
		private Vector4 m_OctogonalBokehDirection2;

		// Token: 0x04006223 RID: 25123
		private Vector4 m_OctogonalBokehDirection3;

		// Token: 0x04006224 RID: 25124
		private Vector4 m_OctogonalBokehDirection4;

		// Token: 0x04006225 RID: 25125
		private Vector4 m_HexagonalBokehDirection1;

		// Token: 0x04006226 RID: 25126
		private Vector4 m_HexagonalBokehDirection2;

		// Token: 0x04006227 RID: 25127
		private Vector4 m_HexagonalBokehDirection3;

		// Token: 0x02000E3C RID: 3644
		[AttributeUsage(AttributeTargets.Field)]
		public sealed class GradientRangeAttribute : PropertyAttribute
		{
			// Token: 0x06007048 RID: 28744 RVA: 0x002A49F2 File Offset: 0x002A2DF2
			public GradientRangeAttribute(float min, float max)
			{
				this.min = min;
				this.max = max;
			}

			// Token: 0x04006228 RID: 25128
			public readonly float max;

			// Token: 0x04006229 RID: 25129
			public readonly float min;
		}

		// Token: 0x02000E3D RID: 3645
		private enum Passes
		{
			// Token: 0x0400622B RID: 25131
			BlurAlphaWeighted,
			// Token: 0x0400622C RID: 25132
			BoxBlur,
			// Token: 0x0400622D RID: 25133
			DilateFgCocFromColor,
			// Token: 0x0400622E RID: 25134
			DilateFgCoc,
			// Token: 0x0400622F RID: 25135
			CaptureCoc,
			// Token: 0x04006230 RID: 25136
			CaptureCocExplicit,
			// Token: 0x04006231 RID: 25137
			VisualizeCoc,
			// Token: 0x04006232 RID: 25138
			VisualizeCocExplicit,
			// Token: 0x04006233 RID: 25139
			CocPrefilter,
			// Token: 0x04006234 RID: 25140
			CircleBlur,
			// Token: 0x04006235 RID: 25141
			CircleBlurWithDilatedFg,
			// Token: 0x04006236 RID: 25142
			CircleBlurLowQuality,
			// Token: 0x04006237 RID: 25143
			CircleBlowLowQualityWithDilatedFg,
			// Token: 0x04006238 RID: 25144
			Merge,
			// Token: 0x04006239 RID: 25145
			MergeExplicit,
			// Token: 0x0400623A RID: 25146
			MergeBicubic,
			// Token: 0x0400623B RID: 25147
			MergeExplicitBicubic,
			// Token: 0x0400623C RID: 25148
			ShapeLowQuality,
			// Token: 0x0400623D RID: 25149
			ShapeLowQualityDilateFg,
			// Token: 0x0400623E RID: 25150
			ShapeLowQualityMerge,
			// Token: 0x0400623F RID: 25151
			ShapeLowQualityMergeDilateFg,
			// Token: 0x04006240 RID: 25152
			ShapeMediumQuality,
			// Token: 0x04006241 RID: 25153
			ShapeMediumQualityDilateFg,
			// Token: 0x04006242 RID: 25154
			ShapeMediumQualityMerge,
			// Token: 0x04006243 RID: 25155
			ShapeMediumQualityMergeDilateFg,
			// Token: 0x04006244 RID: 25156
			ShapeHighQuality,
			// Token: 0x04006245 RID: 25157
			ShapeHighQualityDilateFg,
			// Token: 0x04006246 RID: 25158
			ShapeHighQualityMerge,
			// Token: 0x04006247 RID: 25159
			ShapeHighQualityMergeDilateFg
		}

		// Token: 0x02000E3E RID: 3646
		public enum MedianPasses
		{
			// Token: 0x04006249 RID: 25161
			Median3,
			// Token: 0x0400624A RID: 25162
			Median3X3
		}

		// Token: 0x02000E3F RID: 3647
		public enum BokehTexturesPasses
		{
			// Token: 0x0400624C RID: 25164
			Apply,
			// Token: 0x0400624D RID: 25165
			Collect
		}

		// Token: 0x02000E40 RID: 3648
		public enum UIMode
		{
			// Token: 0x0400624F RID: 25167
			Basic,
			// Token: 0x04006250 RID: 25168
			Advanced,
			// Token: 0x04006251 RID: 25169
			Explicit
		}

		// Token: 0x02000E41 RID: 3649
		public enum ApertureShape
		{
			// Token: 0x04006253 RID: 25171
			Circular,
			// Token: 0x04006254 RID: 25172
			Hexagonal,
			// Token: 0x04006255 RID: 25173
			Octogonal
		}

		// Token: 0x02000E42 RID: 3650
		public enum FilterQuality
		{
			// Token: 0x04006257 RID: 25175
			None,
			// Token: 0x04006258 RID: 25176
			Normal,
			// Token: 0x04006259 RID: 25177
			High
		}
	}
}
