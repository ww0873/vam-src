using System;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class RobotArmController : MonoBehaviour
{
	// Token: 0x06001690 RID: 5776 RVA: 0x0007EC54 File Offset: 0x0007D054
	public RobotArmController()
	{
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x0007EC68 File Offset: 0x0007D068
	private void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			this.section1.Rotate(0f, this.speed * Time.deltaTime, 0f, Space.World);
		}
		if (Input.GetKey(KeyCode.D))
		{
			this.section1.Rotate(0f, -this.speed * Time.deltaTime, 0f, Space.World);
		}
		if (Input.GetKey(KeyCode.W))
		{
			this.section1.Rotate(0f, this.speed * Time.deltaTime, 0f, Space.Self);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.section1.Rotate(0f, -this.speed * Time.deltaTime, 0f, Space.Self);
		}
		if (Input.GetKey(KeyCode.T))
		{
			this.section2.Rotate(0f, this.speed * Time.deltaTime, 0f, Space.Self);
		}
		if (Input.GetKey(KeyCode.G))
		{
			this.section2.Rotate(0f, -this.speed * Time.deltaTime, 0f, Space.Self);
		}
		if (Input.GetKey(KeyCode.Y))
		{
			this.actuator.Rotate(0f, this.speed * Time.deltaTime, 0f, Space.Self);
		}
		if (Input.GetKey(KeyCode.H))
		{
			this.actuator.Rotate(0f, -this.speed * Time.deltaTime, 0f, Space.Self);
		}
	}

	// Token: 0x040012B7 RID: 4791
	public Transform section1;

	// Token: 0x040012B8 RID: 4792
	public Transform section2;

	// Token: 0x040012B9 RID: 4793
	public Transform actuator;

	// Token: 0x040012BA RID: 4794
	public float speed = 40f;
}
