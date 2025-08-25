using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200002B RID: 43
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00008E83 File Offset: 0x00007283
		public BuiltinDebugViewsComponent()
		{
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00008E8B File Offset: 0x0000728B
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00008EC0 File Offset: 0x000072C0
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (mode != BuiltinDebugViewsModel.Mode.Normals)
			{
				if (mode != BuiltinDebugViewsModel.Mode.MotionVectors)
				{
					if (mode == BuiltinDebugViewsModel.Mode.Depth)
					{
						depthTextureMode |= DepthTextureMode.Depth;
					}
				}
				else
				{
					depthTextureMode |= (DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
				}
			}
			else
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00008F1C File Offset: 0x0000731C
		public override CameraEvent GetCameraEvent()
		{
			return (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeImageEffects;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008F4B File Offset: 0x0000734B
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008F54 File Offset: 0x00007354
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			if (mode != BuiltinDebugViewsModel.Mode.Depth)
			{
				if (mode != BuiltinDebugViewsModel.Mode.Normals)
				{
					if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
					{
						this.MotionVectorsPass(cb);
					}
				}
				else
				{
					this.DepthNormalsPass(cb);
				}
			}
			else
			{
				this.DepthPass(cb);
			}
			this.context.Interrupt();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008FF8 File Offset: 0x000073F8
		private void DepthPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 0);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00009058 File Offset: 0x00007458
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 1);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000908C File Offset: 0x0000748C
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int nameID = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(nameID, this.context.width, this.context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, nameID, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
				cb.Blit(nameID, tempRT, material, 3);
				cb.ReleaseTemporaryRT(nameID);
				nameID = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num = 1f / (float)motionVectors.motionVectorsResolution;
				float x = num * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(x, num));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
			cb.Blit(nameID, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(nameID);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00009294 File Offset: 0x00007694
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00009318 File Offset: 0x00007718
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x04000134 RID: 308
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x04000135 RID: 309
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x0200002C RID: 44
		private static class Uniforms
		{
			// Token: 0x060000F0 RID: 240 RVA: 0x00009338 File Offset: 0x00007738
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x04000136 RID: 310
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x04000137 RID: 311
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04000138 RID: 312
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x04000139 RID: 313
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x0400013A RID: 314
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x0400013B RID: 315
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x0400013C RID: 316
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x0200002D RID: 45
		private enum Pass
		{
			// Token: 0x0400013E RID: 318
			Depth,
			// Token: 0x0400013F RID: 319
			Normals,
			// Token: 0x04000140 RID: 320
			MovecOpacity,
			// Token: 0x04000141 RID: 321
			MovecImaging,
			// Token: 0x04000142 RID: 322
			MovecArrows
		}

		// Token: 0x0200002E RID: 46
		private class ArrowArray
		{
			// Token: 0x060000F1 RID: 241 RVA: 0x000093AE File Offset: 0x000077AE
			public ArrowArray()
			{
			}

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000F2 RID: 242 RVA: 0x000093B6 File Offset: 0x000077B6
			// (set) Token: 0x060000F3 RID: 243 RVA: 0x000093BE File Offset: 0x000077BE
			public Mesh mesh
			{
				[CompilerGenerated]
				get
				{
					return this.<mesh>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<mesh>k__BackingField = value;
				}
			}

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000F4 RID: 244 RVA: 0x000093C7 File Offset: 0x000077C7
			// (set) Token: 0x060000F5 RID: 245 RVA: 0x000093CF File Offset: 0x000077CF
			public int columnCount
			{
				[CompilerGenerated]
				get
				{
					return this.<columnCount>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<columnCount>k__BackingField = value;
				}
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000F6 RID: 246 RVA: 0x000093D8 File Offset: 0x000077D8
			// (set) Token: 0x060000F7 RID: 247 RVA: 0x000093E0 File Offset: 0x000077E0
			public int rowCount
			{
				[CompilerGenerated]
				get
				{
					return this.<rowCount>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<rowCount>k__BackingField = value;
				}
			}

			// Token: 0x060000F8 RID: 248 RVA: 0x000093EC File Offset: 0x000077EC
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, MeshTopology.Lines, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x060000F9 RID: 249 RVA: 0x000095CF File Offset: 0x000079CF
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}

			// Token: 0x04000143 RID: 323
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private Mesh <mesh>k__BackingField;

			// Token: 0x04000144 RID: 324
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private int <columnCount>k__BackingField;

			// Token: 0x04000145 RID: 325
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private int <rowCount>k__BackingField;
		}
	}
}
