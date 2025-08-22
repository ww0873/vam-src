using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000B4 RID: 180
	[ExecuteInEditMode]
	public class GLCamera : MonoBehaviour
	{
		// Token: 0x060002ED RID: 749 RVA: 0x00013B74 File Offset: 0x00011F74
		public GLCamera()
		{
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00013B83 File Offset: 0x00011F83
		private void OnPostRender()
		{
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Draw(this.CullingMask);
			}
		}

		// Token: 0x040003A5 RID: 933
		public int CullingMask = -1;
	}
}
