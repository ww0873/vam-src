using System;
using MVR;
using UnityEngine;

// Token: 0x02000BA9 RID: 2985
public class FreeControllerV3Link : MonoBehaviour
{
	// Token: 0x0600554A RID: 21834 RVA: 0x001F3521 File Offset: 0x001F1921
	public FreeControllerV3Link()
	{
	}

	// Token: 0x0600554B RID: 21835 RVA: 0x001F352C File Offset: 0x001F192C
	private void Update()
	{
		if (this.linkedController && this.linkedController.followWhenOff != null)
		{
			Vector3 position = this.linkedController.followWhenOff.position;
			if (NaNUtils.IsVector3Valid(position))
			{
				base.transform.position = position;
			}
		}
	}

	// Token: 0x04004571 RID: 17777
	public FreeControllerV3 linkedController;
}
