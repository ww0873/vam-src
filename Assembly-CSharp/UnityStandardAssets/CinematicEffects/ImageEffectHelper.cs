using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x02000E39 RID: 3641
	public static class ImageEffectHelper
	{
		// Token: 0x0600702D RID: 28717 RVA: 0x002A3614 File Offset: 0x002A1A14
		public static bool IsSupported(Shader s, bool needDepth, bool needHdr, MonoBehaviour effect)
		{
			if (s == null || !s.isSupported)
			{
				Debug.LogWarningFormat("Missing shader for image effect {0}", new object[]
				{
					effect
				});
				return false;
			}
			return SystemInfo.supportsImageEffects && (!needDepth || SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth)) && (!needHdr || SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf));
		}

		// Token: 0x0600702E RID: 28718 RVA: 0x002A3684 File Offset: 0x002A1A84
		public static Material CheckShaderAndCreateMaterial(Shader s)
		{
			if (s == null || !s.isSupported)
			{
				return null;
			}
			return new Material(s)
			{
				hideFlags = HideFlags.DontSave
			};
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x0600702F RID: 28719 RVA: 0x002A36BA File Offset: 0x002A1ABA
		public static bool supportsDX11
		{
			get
			{
				return SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			}
		}
	}
}
