using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000090 RID: 144
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x0600020E RID: 526 RVA: 0x0000FB78 File Offset: 0x0000DF78
		public PostProcessingProfile()
		{
		}

		// Token: 0x040002FC RID: 764
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x040002FD RID: 765
		public FogModel fog = new FogModel();

		// Token: 0x040002FE RID: 766
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x040002FF RID: 767
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x04000300 RID: 768
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x04000301 RID: 769
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x04000302 RID: 770
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04000303 RID: 771
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04000304 RID: 772
		public BloomModel bloom = new BloomModel();

		// Token: 0x04000305 RID: 773
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04000306 RID: 774
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x04000307 RID: 775
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x04000308 RID: 776
		public GrainModel grain = new GrainModel();

		// Token: 0x04000309 RID: 777
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x0400030A RID: 778
		public DitheringModel dithering = new DitheringModel();
	}
}
