using System;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class ButtonControl : MonoBehaviour
{
	// Token: 0x06001040 RID: 4160 RVA: 0x0005B9B4 File Offset: 0x00059DB4
	public ButtonControl()
	{
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x0005B9BC File Offset: 0x00059DBC
	private void Start()
	{
		for (int i = 0; i < this.lamps.Length; i++)
		{
			this.lamps[i].GetComponent<Renderer>().material = this.lamps[i].GetComponent<LightScript>().offMat;
			this.lamps[i].GetComponent<LightScript>().isOn = false;
		}
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0005BA1C File Offset: 0x00059E1C
	public void turn()
	{
		this.resetAnimator();
		this.anim.SetTrigger(this.downTrigger);
		for (int i = 0; i < this.lamps.Length; i++)
		{
			if (this.lamps[i].GetComponent<LightScript>().isOn)
			{
				this.lamps[i].GetComponent<Renderer>().material = this.lamps[i].GetComponent<LightScript>().offMat;
				this.lamps[i].GetComponent<LightScript>().isOn = false;
			}
			else
			{
				this.lamps[i].GetComponent<Renderer>().material = this.lamps[i].GetComponent<LightScript>().onMat;
				this.lamps[i].GetComponent<LightScript>().isOn = true;
			}
		}
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0005BAE3 File Offset: 0x00059EE3
	private void resetAnimator()
	{
		this.anim.ResetTrigger(this.downTrigger);
	}

	// Token: 0x04000E6B RID: 3691
	public Animator anim;

	// Token: 0x04000E6C RID: 3692
	public GameObject[] lamps;

	// Token: 0x04000E6D RID: 3693
	public string downTrigger;
}
