using System;
using UnityEngine;

// Token: 0x02000301 RID: 769
public class BoatSimulator : MonoBehaviour
{
	// Token: 0x06001231 RID: 4657 RVA: 0x0006461C File Offset: 0x00062A1C
	public BoatSimulator()
	{
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00064624 File Offset: 0x00062A24
	private void Start()
	{
		this.rigid = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00064634 File Offset: 0x00062A34
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			this.keyPressedW = true;
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			this.keyPressedA = true;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			this.keyPressedS = true;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			this.keyPressedD = true;
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			this.keyPressedW = false;
		}
		if (Input.GetKeyUp(KeyCode.A))
		{
			this.keyPressedA = false;
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			this.keyPressedS = false;
		}
		if (Input.GetKeyUp(KeyCode.D))
		{
			this.keyPressedD = false;
		}
		if (this.keyPressedW)
		{
			this.rigid.AddForce(base.transform.right * 500f * Time.deltaTime);
		}
		if (this.keyPressedS)
		{
			this.rigid.AddForce(-base.transform.right * 500f * Time.deltaTime);
		}
		if (this.keyPressedD)
		{
			this.rigid.AddTorque(base.transform.up * 200f * Time.deltaTime);
		}
		if (this.keyPressedA)
		{
			this.rigid.AddTorque(-base.transform.up * 200f * Time.deltaTime);
		}
	}

	// Token: 0x04000F79 RID: 3961
	private Rigidbody rigid;

	// Token: 0x04000F7A RID: 3962
	private bool keyPressedW;

	// Token: 0x04000F7B RID: 3963
	private bool keyPressedA;

	// Token: 0x04000F7C RID: 3964
	private bool keyPressedS;

	// Token: 0x04000F7D RID: 3965
	private bool keyPressedD;
}
