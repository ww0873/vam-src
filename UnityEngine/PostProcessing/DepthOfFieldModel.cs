using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x0000E902 File Offset: 0x0000CD02
		public DepthOfFieldModel()
		{
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000E915 File Offset: 0x0000CD15
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000E91D File Offset: 0x0000CD1D
		public DepthOfFieldModel.Settings settings
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

		// Token: 0x060001B3 RID: 435 RVA: 0x0000E926 File Offset: 0x0000CD26
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x0400028A RID: 650
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x02000070 RID: 112
		public enum KernelSize
		{
			// Token: 0x0400028C RID: 652
			Small,
			// Token: 0x0400028D RID: 653
			Medium,
			// Token: 0x0400028E RID: 654
			Large,
			// Token: 0x0400028F RID: 655
			VeryLarge
		}

		// Token: 0x02000071 RID: 113
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000E934 File Offset: 0x0000CD34
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x04000290 RID: 656
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x04000291 RID: 657
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x04000292 RID: 658
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04000293 RID: 659
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x04000294 RID: 660
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
