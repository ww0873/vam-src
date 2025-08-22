using System;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class SpinPlanet : MonoBehaviour
{
	// Token: 0x0600119D RID: 4509 RVA: 0x00061748 File Offset: 0x0005FB48
	public SpinPlanet()
	{
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x0006175B File Offset: 0x0005FB5B
	private void Update()
	{
		base.transform.Rotate(Vector3.up, this.speed * Time.deltaTime);
	}

	// Token: 0x04000F3A RID: 3898
	public float speed = 4f;
}
