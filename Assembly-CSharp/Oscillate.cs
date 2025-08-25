using System;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class Oscillate : MonoBehaviour
{
	// Token: 0x060012AF RID: 4783 RVA: 0x0006A809 File Offset: 0x00068C09
	public Oscillate()
	{
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x0006A811 File Offset: 0x00068C11
	private void Start()
	{
		this.basePosition = base.transform.position;
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x0006A824 File Offset: 0x00068C24
	private void LateUpdate()
	{
		Vector3 vector = Time.time * this.speed * 2f * 3.1415927f;
		Vector3 b = new Vector3(this.amplitude.x * Mathf.Sin(vector.x), this.amplitude.y * Mathf.Sin(vector.y), this.amplitude.z * Mathf.Sin(vector.z));
		b.x = Mathf.Clamp(b.x, -this.clamp.x, this.clamp.x);
		b.y = Mathf.Clamp(b.y, -this.clamp.y, this.clamp.y);
		b.z = Mathf.Clamp(b.z, -this.clamp.z, this.clamp.z);
		base.transform.position = this.basePosition + b;
	}

	// Token: 0x04001066 RID: 4198
	public Vector3 amplitude;

	// Token: 0x04001067 RID: 4199
	public Vector3 speed;

	// Token: 0x04001068 RID: 4200
	public Vector3 clamp;

	// Token: 0x04001069 RID: 4201
	public Vector3 basePosition;
}
