using System;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class LightMapData : MonoBehaviour
{
	// Token: 0x060011E2 RID: 4578 RVA: 0x00062DFB File Offset: 0x000611FB
	public LightMapData()
	{
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x00062E04 File Offset: 0x00061204
	public void GetInfo()
	{
		this.m_RendererInfo.renderer = base.GetComponent<Renderer>();
		if (this.m_RendererInfo.renderer)
		{
			this.m_RendererInfo.lightmapIndex = this.m_RendererInfo.renderer.lightmapIndex;
			this.m_RendererInfo.lightmapOffsetScale = this.m_RendererInfo.renderer.lightmapScaleOffset;
		}
	}

	// Token: 0x04000F5B RID: 3931
	[SerializeField]
	public LightMapData.RendererInfo m_RendererInfo;

	// Token: 0x020002F6 RID: 758
	[Serializable]
	public struct RendererInfo
	{
		// Token: 0x04000F5C RID: 3932
		public Renderer renderer;

		// Token: 0x04000F5D RID: 3933
		public int lightmapIndex;

		// Token: 0x04000F5E RID: 3934
		public Vector4 lightmapOffsetScale;
	}
}
