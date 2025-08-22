using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000094 RID: 148
	public sealed class RenderTextureFactory : IDisposable
	{
		// Token: 0x0600021D RID: 541 RVA: 0x0001020F File Offset: 0x0000E60F
		public RenderTextureFactory()
		{
			this.m_TemporaryRTs = new HashSet<RenderTexture>();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00010224 File Offset: 0x0000E624
		public RenderTexture Get(RenderTexture baseRenderTexture)
		{
			return this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, (!baseRenderTexture.sRGB) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00010274 File Offset: 0x0000E674
		public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf, RenderTextureReadWrite rw = RenderTextureReadWrite.Default, FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, string name = "FactoryTempTexture")
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format, rw);
			temporary.filterMode = filterMode;
			temporary.wrapMode = wrapMode;
			temporary.name = name;
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000102B4 File Offset: 0x0000E6B4
		public void Release(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				throw new ArgumentException(string.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt));
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00010304 File Offset: 0x0000E704
		public void ReleaseAll()
		{
			foreach (RenderTexture temp in this.m_TemporaryRTs)
			{
				RenderTexture.ReleaseTemporary(temp);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00010345 File Offset: 0x0000E745
		public void Dispose()
		{
			this.ReleaseAll();
		}

		// Token: 0x04000313 RID: 787
		private HashSet<RenderTexture> m_TemporaryRTs;
	}
}
