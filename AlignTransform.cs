using System;
using UnityEngine;

// Token: 0x02000A36 RID: 2614
public class AlignTransform : MonoBehaviour
{
	// Token: 0x06004373 RID: 17267 RVA: 0x0013C350 File Offset: 0x0013A750
	public AlignTransform()
	{
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x0013C358 File Offset: 0x0013A758
	private void Update()
	{
		if (this.alignTo != null)
		{
			if (this.delayFrame)
			{
				base.transform.position = this.lastPosition;
				base.transform.rotation = this.lastRotation;
				this.lastPosition = this.alignTo.position;
				this.lastRotation = this.alignTo.rotation;
			}
			else
			{
				base.transform.position = this.alignTo.position;
				base.transform.rotation = this.alignTo.rotation;
			}
		}
	}

	// Token: 0x0400326B RID: 12907
	public Transform alignTo;

	// Token: 0x0400326C RID: 12908
	public bool delayFrame;

	// Token: 0x0400326D RID: 12909
	protected Vector3 lastPosition;

	// Token: 0x0400326E RID: 12910
	protected Quaternion lastRotation;
}
