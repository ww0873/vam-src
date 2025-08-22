using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000726 RID: 1830
	[ExecuteInEditMode]
	public class EnableDepthBuffer : MonoBehaviour
	{
		// Token: 0x06002CA3 RID: 11427 RVA: 0x000EF888 File Offset: 0x000EDC88
		public EnableDepthBuffer()
		{
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000EF898 File Offset: 0x000EDC98
		private void Awake()
		{
			base.GetComponent<Camera>().depthTextureMode = this._depthTextureMode;
			if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) && this._depthTextureMode != DepthTextureMode.None)
			{
				Shader.EnableKeyword("USE_DEPTH_TEXTURE");
			}
			else
			{
				Shader.DisableKeyword("USE_DEPTH_TEXTURE");
			}
		}

		// Token: 0x0400239C RID: 9116
		public const string DEPTH_TEXTURE_VARIANT_NAME = "USE_DEPTH_TEXTURE";

		// Token: 0x0400239D RID: 9117
		[SerializeField]
		private DepthTextureMode _depthTextureMode = DepthTextureMode.Depth;
	}
}
