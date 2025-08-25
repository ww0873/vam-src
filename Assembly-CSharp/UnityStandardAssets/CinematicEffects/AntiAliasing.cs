using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x02000E34 RID: 3636
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/SMAA")]
	public class AntiAliasing : MonoBehaviour
	{
		// Token: 0x0600701B RID: 28699 RVA: 0x002A2C74 File Offset: 0x002A1074
		public AntiAliasing()
		{
		}

		// Token: 0x0600701C RID: 28700 RVA: 0x002A2CA4 File Offset: 0x002A10A4
		private static Matrix4x4 CalculateViewProjection(Camera camera, Matrix4x4 prjMatrix)
		{
			Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
			Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(prjMatrix, true);
			return gpuprojectionMatrix * worldToCameraMatrix;
		}

		// Token: 0x0600701D RID: 28701 RVA: 0x002A2CC7 File Offset: 0x002A10C7
		private void StoreBaseProjectionMatrix(Matrix4x4 prjMatrix)
		{
			this.m_BaseProjectionMatrix = prjMatrix;
		}

		// Token: 0x0600701E RID: 28702 RVA: 0x002A2CD0 File Offset: 0x002A10D0
		private void StorePreviousViewProjMatrix(Matrix4x4 viewPrjMatrix)
		{
			this.m_PrevViewProjMat = viewPrjMatrix;
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x0600701F RID: 28703 RVA: 0x002A2CD9 File Offset: 0x002A10D9
		private Camera aaCamera
		{
			get
			{
				if (this.m_AACamera == null)
				{
					this.m_AACamera = base.GetComponent<Camera>();
				}
				return this.m_AACamera;
			}
		}

		// Token: 0x06007020 RID: 28704 RVA: 0x002A2D00 File Offset: 0x002A1100
		public void UpdateSampleIndex()
		{
			int num = 1;
			if (this.temporalType == AntiAliasing.TemporalType.SMAA_2x || this.temporalType == AntiAliasing.TemporalType.Standard_2x)
			{
				num = 2;
			}
			else if (this.temporalType == AntiAliasing.TemporalType.Standard_4x)
			{
				num = 4;
			}
			else if (this.temporalType == AntiAliasing.TemporalType.Standard_8x)
			{
				num = 8;
			}
			else if (this.temporalType == AntiAliasing.TemporalType.Standard_16x)
			{
				num = 16;
			}
			this.m_SampleIndex = (this.m_SampleIndex + 1) % num;
		}

		// Token: 0x06007021 RID: 28705 RVA: 0x002A2D74 File Offset: 0x002A1174
		private Vector2 GetJitterStandard2X()
		{
			int[,] array = new int[,]
			{
				{
					4,
					4
				},
				{
					-4,
					-4
				}
			};
			int num = array[this.m_SampleIndex, 0];
			int num2 = array[this.m_SampleIndex, 1];
			float x = (float)num / 16f;
			float y = (float)num2 / 16f;
			return new Vector2(x, y);
		}

		// Token: 0x06007022 RID: 28706 RVA: 0x002A2DCC File Offset: 0x002A11CC
		private Vector2 GetJitterStandard4X()
		{
			int[,] array = new int[,]
			{
				{
					-2,
					-6
				},
				{
					6,
					-2
				},
				{
					-6,
					2
				},
				{
					2,
					6
				}
			};
			int num = array[this.m_SampleIndex, 0];
			int num2 = array[this.m_SampleIndex, 1];
			float x = (float)num / 16f;
			float y = (float)num2 / 16f;
			return new Vector2(x, y);
		}

		// Token: 0x06007023 RID: 28707 RVA: 0x002A2E24 File Offset: 0x002A1224
		private Vector2 GetJitterStandard8X()
		{
			int[,] array = new int[,]
			{
				{
					7,
					-7
				},
				{
					-3,
					-5
				},
				{
					3,
					7
				},
				{
					-7,
					-1
				},
				{
					5,
					1
				},
				{
					-1,
					3
				},
				{
					1,
					-3
				},
				{
					-5,
					5
				}
			};
			int num = array[this.m_SampleIndex, 0];
			int num2 = array[this.m_SampleIndex, 1];
			float x = (float)num / 16f;
			float y = (float)num2 / 16f;
			return new Vector2(x, y);
		}

		// Token: 0x06007024 RID: 28708 RVA: 0x002A2E7C File Offset: 0x002A127C
		private Vector2 GetJitterStandard16X()
		{
			int[,] array = new int[,]
			{
				{
					7,
					-4
				},
				{
					-1,
					-3
				},
				{
					3,
					-5
				},
				{
					-5,
					-2
				},
				{
					6,
					7
				},
				{
					-2,
					6
				},
				{
					2,
					5
				},
				{
					-6,
					-4
				},
				{
					4,
					-1
				},
				{
					-3,
					2
				},
				{
					1,
					1
				},
				{
					-8,
					0
				},
				{
					5,
					3
				},
				{
					-4,
					-6
				},
				{
					0,
					-7
				},
				{
					-7,
					-8
				}
			};
			int num = array[this.m_SampleIndex, 0];
			int num2 = array[this.m_SampleIndex, 1];
			float x = ((float)num + 0.5f) / 16f;
			float y = ((float)num2 + 0.5f) / 16f;
			return new Vector2(x, y);
		}

		// Token: 0x06007025 RID: 28709 RVA: 0x002A2EE0 File Offset: 0x002A12E0
		private Vector2 GetJitterSMAAX2()
		{
			float num = 0.25f;
			num *= ((this.m_SampleIndex != 0) ? 1f : -1f);
			float x = num;
			float y = -num;
			return new Vector2(x, y);
		}

		// Token: 0x06007026 RID: 28710 RVA: 0x002A2F1C File Offset: 0x002A131C
		private Vector2 GetCurrentJitter()
		{
			Vector2 result = new Vector2(0f, 0f);
			if (this.temporalType == AntiAliasing.TemporalType.SMAA_2x)
			{
				result = this.GetJitterSMAAX2();
			}
			else if (this.temporalType == AntiAliasing.TemporalType.Standard_2x)
			{
				result = this.GetJitterStandard2X();
			}
			else if (this.temporalType == AntiAliasing.TemporalType.Standard_4x)
			{
				result = this.GetJitterStandard4X();
			}
			else if (this.temporalType == AntiAliasing.TemporalType.Standard_8x)
			{
				result = this.GetJitterStandard8X();
			}
			else if (this.temporalType == AntiAliasing.TemporalType.Standard_16x)
			{
				result = this.GetJitterStandard16X();
			}
			return result;
		}

		// Token: 0x06007027 RID: 28711 RVA: 0x002A2FB0 File Offset: 0x002A13B0
		private void OnPreCull()
		{
			this.StoreBaseProjectionMatrix(this.aaCamera.projectionMatrix);
			if (this.temporalType != AntiAliasing.TemporalType.Off)
			{
				this.UpdateSampleIndex();
				Vector2 currentJitter = this.GetCurrentJitter();
				Matrix4x4 identity = Matrix4x4.identity;
				identity.m03 = currentJitter.x * 2f / (float)this.aaCamera.pixelWidth;
				identity.m13 = currentJitter.y * 2f / (float)this.aaCamera.pixelHeight;
				Matrix4x4 projectionMatrix = identity * this.m_BaseProjectionMatrix;
				this.aaCamera.projectionMatrix = projectionMatrix;
			}
		}

		// Token: 0x06007028 RID: 28712 RVA: 0x002A3047 File Offset: 0x002A1447
		private void OnPostRender()
		{
			this.aaCamera.ResetProjectionMatrix();
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06007029 RID: 28713 RVA: 0x002A3054 File Offset: 0x002A1454
		public Material smaaMaterial
		{
			get
			{
				if (this.m_SmaaMaterial == null)
				{
					this.m_SmaaMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(this.smaaShader);
				}
				return this.m_SmaaMaterial;
			}
		}

		// Token: 0x0600702A RID: 28714 RVA: 0x002A3080 File Offset: 0x002A1480
		protected void OnEnable()
		{
			if (this.smaaShader == null)
			{
				this.smaaShader = Shader.Find("Hidden/SMAA");
			}
			if (!ImageEffectHelper.IsSupported(this.smaaShader, true, true, this))
			{
				base.enabled = false;
				Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
				return;
			}
			this.aaCamera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600702B RID: 28715 RVA: 0x002A30F8 File Offset: 0x002A14F8
		private void OnDisable()
		{
			this.aaCamera.ResetProjectionMatrix();
			if (this.m_SmaaMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_SmaaMaterial);
				this.m_SmaaMaterial = null;
			}
			if (this.m_RtAccum)
			{
				UnityEngine.Object.DestroyImmediate(this.m_RtAccum);
				this.m_RtAccum = null;
			}
		}

		// Token: 0x0600702C RID: 28716 RVA: 0x002A3154 File Offset: 0x002A1554
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.smaaMaterial == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			bool flag = false;
			if (this.m_RtAccum == null || this.m_RtAccum.width != source.width || this.m_RtAccum.height != source.height)
			{
				if (this.m_RtAccum != null)
				{
					RenderTexture.ReleaseTemporary(this.m_RtAccum);
				}
				this.m_RtAccum = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				this.m_RtAccum.hideFlags = HideFlags.DontSave;
				flag = true;
			}
			int value = 0;
			if (this.temporalType == AntiAliasing.TemporalType.SMAA_2x)
			{
				value = ((this.m_SampleIndex >= 1) ? 2 : 1);
			}
			int width = source.width;
			int height = source.height;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
			Matrix4x4 matrix4x = AntiAliasing.CalculateViewProjection(this.aaCamera, this.m_BaseProjectionMatrix);
			Matrix4x4 rhs = Matrix4x4.Inverse(matrix4x);
			this.smaaMaterial.SetMatrix("_ToPrevViewProjCombined", this.m_PrevViewProjMat * rhs);
			this.smaaMaterial.SetInt("_JitterOffset", value);
			this.smaaMaterial.SetTexture("areaTex", this.areaTex);
			this.smaaMaterial.SetTexture("searchTex", this.searchTex);
			this.smaaMaterial.SetTexture("colorTex", source);
			this.smaaMaterial.SetVector("_PixelSize", new Vector4(1f / (float)source.width, 1f / (float)source.height, 0f, 0f));
			Vector2 currentJitter = this.GetCurrentJitter();
			this.smaaMaterial.SetVector("_PixelOffset", new Vector4(currentJitter.x / (float)source.width, currentJitter.y / (float)source.height, 0f, 0f));
			this.smaaMaterial.SetTexture("edgesTex", temporary);
			this.smaaMaterial.SetTexture("blendTex", temporary2);
			this.smaaMaterial.SetFloat("K", this.K);
			this.smaaMaterial.SetFloat("_TemporalAccum", this.temporalAccumulationWeight);
			Graphics.Blit(source, temporary, this.smaaMaterial, 2);
			if (this.edgeType == AntiAliasing.EdgeType.Luminance)
			{
				Graphics.Blit(source, temporary, this.smaaMaterial, 1);
			}
			else if (this.edgeType == AntiAliasing.EdgeType.Color)
			{
				Graphics.Blit(source, temporary, this.smaaMaterial, 6);
			}
			else
			{
				this.smaaMaterial.SetFloat("_DepthThreshold", 0.01f * this.depthThreshold);
				Graphics.Blit(source, temporary, this.smaaMaterial, 8);
			}
			Graphics.Blit(temporary, temporary2, this.smaaMaterial, 3);
			if (this.temporalType == AntiAliasing.TemporalType.Off)
			{
				Graphics.Blit(source, destination, this.smaaMaterial, 4);
			}
			else
			{
				RenderTexture temporary3 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				if (this.temporalType == AntiAliasing.TemporalType.SMAA_2x)
				{
					this.smaaMaterial.SetTexture("accumTex", this.m_RtAccum);
					if (flag)
					{
						Graphics.Blit(source, temporary3, this.smaaMaterial, 4);
					}
					else
					{
						Graphics.Blit(source, temporary3, this.smaaMaterial, 5);
					}
					Graphics.Blit(temporary3, this.m_RtAccum, this.smaaMaterial, 0);
					Graphics.Blit(temporary3, destination, this.smaaMaterial, 0);
				}
				else
				{
					Graphics.Blit(source, temporary3, this.smaaMaterial, 4);
					if (flag)
					{
						Graphics.Blit(temporary3, this.m_RtAccum, this.smaaMaterial, 0);
					}
					this.smaaMaterial.SetTexture("accumTex", this.m_RtAccum);
					this.smaaMaterial.SetTexture("smaaTex", temporary3);
					temporary3.filterMode = FilterMode.Bilinear;
					RenderTexture temporary4 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
					Graphics.Blit(temporary3, temporary4, this.smaaMaterial, 7);
					Graphics.Blit(temporary4, this.m_RtAccum, this.smaaMaterial, 0);
					Graphics.Blit(temporary4, destination, this.smaaMaterial, 0);
					RenderTexture.ReleaseTemporary(temporary4);
				}
				RenderTexture.ReleaseTemporary(temporary3);
			}
			if (this.displayType == AntiAliasing.DebugDisplay.Edges)
			{
				Graphics.Blit(temporary, destination, this.smaaMaterial, 0);
			}
			else if (this.displayType == AntiAliasing.DebugDisplay.Weights)
			{
				Graphics.Blit(temporary2, destination, this.smaaMaterial, 0);
			}
			else if (this.displayType == AntiAliasing.DebugDisplay.Depth)
			{
				Graphics.Blit(null, destination, this.smaaMaterial, 9);
			}
			else if (this.displayType == AntiAliasing.DebugDisplay.Accumulation)
			{
				Graphics.Blit(this.m_RtAccum, destination);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			this.StorePreviousViewProjMatrix(matrix4x);
		}

		// Token: 0x040061CD RID: 25037
		public AntiAliasing.DebugDisplay displayType;

		// Token: 0x040061CE RID: 25038
		public AntiAliasing.EdgeType edgeType = AntiAliasing.EdgeType.Depth;

		// Token: 0x040061CF RID: 25039
		public Texture2D areaTex;

		// Token: 0x040061D0 RID: 25040
		public Texture2D searchTex;

		// Token: 0x040061D1 RID: 25041
		private Matrix4x4 m_BaseProjectionMatrix;

		// Token: 0x040061D2 RID: 25042
		private Matrix4x4 m_PrevViewProjMat;

		// Token: 0x040061D3 RID: 25043
		private Camera m_AACamera;

		// Token: 0x040061D4 RID: 25044
		private int m_SampleIndex;

		// Token: 0x040061D5 RID: 25045
		[Range(0f, 80f)]
		public float K = 1f;

		// Token: 0x040061D6 RID: 25046
		public AntiAliasing.TemporalType temporalType;

		// Token: 0x040061D7 RID: 25047
		[Range(0f, 1f)]
		public float temporalAccumulationWeight = 0.3f;

		// Token: 0x040061D8 RID: 25048
		[Range(0.01f, 1f)]
		public float depthThreshold = 0.1f;

		// Token: 0x040061D9 RID: 25049
		public Shader smaaShader;

		// Token: 0x040061DA RID: 25050
		private Material m_SmaaMaterial;

		// Token: 0x040061DB RID: 25051
		private RenderTexture m_RtAccum;

		// Token: 0x02000E35 RID: 3637
		public enum DebugDisplay
		{
			// Token: 0x040061DD RID: 25053
			Off,
			// Token: 0x040061DE RID: 25054
			Edges,
			// Token: 0x040061DF RID: 25055
			Weights,
			// Token: 0x040061E0 RID: 25056
			Depth,
			// Token: 0x040061E1 RID: 25057
			Accumulation
		}

		// Token: 0x02000E36 RID: 3638
		public enum EdgeType
		{
			// Token: 0x040061E3 RID: 25059
			Luminance,
			// Token: 0x040061E4 RID: 25060
			Color,
			// Token: 0x040061E5 RID: 25061
			Depth
		}

		// Token: 0x02000E37 RID: 3639
		public enum TemporalType
		{
			// Token: 0x040061E7 RID: 25063
			Off,
			// Token: 0x040061E8 RID: 25064
			SMAA_2x,
			// Token: 0x040061E9 RID: 25065
			Standard_2x,
			// Token: 0x040061EA RID: 25066
			Standard_4x,
			// Token: 0x040061EB RID: 25067
			Standard_8x,
			// Token: 0x040061EC RID: 25068
			Standard_16x
		}

		// Token: 0x02000E38 RID: 3640
		private enum Passes
		{
			// Token: 0x040061EE RID: 25070
			Copy,
			// Token: 0x040061EF RID: 25071
			LumaDetection,
			// Token: 0x040061F0 RID: 25072
			ClearToBlack,
			// Token: 0x040061F1 RID: 25073
			WeightCalculation,
			// Token: 0x040061F2 RID: 25074
			WeightsAndBlend1,
			// Token: 0x040061F3 RID: 25075
			WeightsAndBlend2,
			// Token: 0x040061F4 RID: 25076
			ColorDetection,
			// Token: 0x040061F5 RID: 25077
			MergeFrames,
			// Token: 0x040061F6 RID: 25078
			DepthDetection,
			// Token: 0x040061F7 RID: 25079
			DebugDepth
		}
	}
}
