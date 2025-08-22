using System;
using UnityEngine;

// Token: 0x02000D8E RID: 3470
public class SetCenterOfGravity : MonoBehaviour
{
	// Token: 0x06006B03 RID: 27395 RVA: 0x002847E9 File Offset: 0x00282BE9
	public SetCenterOfGravity()
	{
	}

	// Token: 0x06006B04 RID: 27396 RVA: 0x002847F1 File Offset: 0x00282BF1
	private void setCOG()
	{
		this.rb.centerOfMass = this.centerOfGravity;
	}

	// Token: 0x06006B05 RID: 27397 RVA: 0x00284804 File Offset: 0x00282C04
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		if (this.rb)
		{
			this.setCOG();
		}
	}

	// Token: 0x06006B06 RID: 27398 RVA: 0x00284828 File Offset: 0x00282C28
	private void Update()
	{
		if (this.liveUpdate && this.rb && this.rb.centerOfMass != this.centerOfGravity)
		{
			this.setCOG();
		}
	}

	// Token: 0x04005CE2 RID: 23778
	public Vector3 centerOfGravity;

	// Token: 0x04005CE3 RID: 23779
	public bool liveUpdate;

	// Token: 0x04005CE4 RID: 23780
	private Rigidbody rb;
}
