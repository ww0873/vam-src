using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x02000E3A RID: 3642
	public class RenderTexureUtility
	{
		// Token: 0x06007030 RID: 28720 RVA: 0x002A36D0 File Offset: 0x002A1AD0
		public RenderTexureUtility()
		{
		}

		// Token: 0x06007031 RID: 28721 RVA: 0x002A36E4 File Offset: 0x002A1AE4
		public RenderTexture GetTemporaryRenderTexture(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf, FilterMode filterMode = FilterMode.Bilinear)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format);
			temporary.filterMode = filterMode;
			temporary.wrapMode = TextureWrapMode.Clamp;
			temporary.name = "RenderTextureUtilityTempTexture";
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x06007032 RID: 28722 RVA: 0x002A3724 File Offset: 0x002A1B24
		public void ReleaseTemporaryRenderTexture(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				Debug.LogErrorFormat("Attempting to remove texture that was not allocated: {0}", new object[]
				{
					rt
				});
				return;
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x06007033 RID: 28723 RVA: 0x002A3778 File Offset: 0x002A1B78
		public void ReleaseAllTemporyRenderTexutres()
		{
			for (int i = 0; i < this.m_TemporaryRTs.Count; i++)
			{
				RenderTexture.ReleaseTemporary(this.m_TemporaryRTs[i]);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x040061F8 RID: 25080
		private List<RenderTexture> m_TemporaryRTs = new List<RenderTexture>();
	}
}
