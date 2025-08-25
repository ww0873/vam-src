using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200007D RID: 125
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x060001CE RID: 462 RVA: 0x0000EBAF File Offset: 0x0000CFAF
		public ScreenSpaceReflectionModel()
		{
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000EBC2 File Offset: 0x0000CFC2
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000EBCA File Offset: 0x0000CFCA
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000EBD3 File Offset: 0x0000CFD3
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x040002B0 RID: 688
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x0200007E RID: 126
		public enum SSRResolution
		{
			// Token: 0x040002B2 RID: 690
			High,
			// Token: 0x040002B3 RID: 691
			Low = 2
		}

		// Token: 0x0200007F RID: 127
		public enum SSRReflectionBlendType
		{
			// Token: 0x040002B5 RID: 693
			PhysicallyBased,
			// Token: 0x040002B6 RID: 694
			Additive
		}

		// Token: 0x02000080 RID: 128
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x040002B7 RID: 695
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x040002B8 RID: 696
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x040002B9 RID: 697
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x040002BA RID: 698
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x02000081 RID: 129
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x040002BB RID: 699
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x040002BC RID: 700
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x040002BD RID: 701
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x040002BE RID: 702
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x040002BF RID: 703
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x040002C0 RID: 704
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x040002C1 RID: 705
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x040002C2 RID: 706
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x02000082 RID: 130
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x040002C3 RID: 707
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x02000083 RID: 131
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005E RID: 94
			// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000EBE0 File Offset: 0x0000CFE0
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x040002C4 RID: 708
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x040002C5 RID: 709
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x040002C6 RID: 710
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
