using System;
using UnityEngine;

// Token: 0x02000C96 RID: 3222
public class LinkedPoint
{
	// Token: 0x06006129 RID: 24873 RVA: 0x0024FB2C File Offset: 0x0024DF2C
	public LinkedPoint(Vector3 pt)
	{
		this.position = pt;
		this.previous_position = pt;
		this.velocity = Vector3.zero;
		this.force = Vector3.zero;
	}

	// Token: 0x04005103 RID: 20739
	public LinkedPoint previous;

	// Token: 0x04005104 RID: 20740
	public LinkedPoint next;

	// Token: 0x04005105 RID: 20741
	public Vector3 previous_position;

	// Token: 0x04005106 RID: 20742
	public Vector3 stiff_position;

	// Token: 0x04005107 RID: 20743
	public float stiffness;

	// Token: 0x04005108 RID: 20744
	public Vector3 unconstrained_position;

	// Token: 0x04005109 RID: 20745
	public Vector3 position;

	// Token: 0x0400510A RID: 20746
	public Vector3 delta_position;

	// Token: 0x0400510B RID: 20747
	public Vector3 velocity;

	// Token: 0x0400510C RID: 20748
	public Vector3 force;

	// Token: 0x0400510D RID: 20749
	public bool collided;

	// Token: 0x0400510E RID: 20750
	public bool had_collided;
}
