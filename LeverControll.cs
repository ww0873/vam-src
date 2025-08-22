using System;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class LeverControll : MonoBehaviour
{
	// Token: 0x06001044 RID: 4164 RVA: 0x0005BAF6 File Offset: 0x00059EF6
	public LeverControll()
	{
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0005BB08 File Offset: 0x00059F08
	private void Start()
	{
		this.anim.SetTrigger(this.upTrigger);
		for (int i = 0; i < this.lamps.Length; i++)
		{
			this.lamps[i].GetComponent<Renderer>().material = this.lamps[i].GetComponent<LightScript>().offMat;
			this.lamps[i].GetComponent<LightScript>().isOn = false;
		}
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x0005BB78 File Offset: 0x00059F78
	public void turn()
	{
		this.resetAnimator();
		if (this.up)
		{
			this.anim.SetTrigger(this.downTrigger);
			this.up = !this.up;
		}
		else
		{
			this.anim.SetTrigger(this.upTrigger);
			this.up = !this.up;
		}
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

	// Token: 0x06001047 RID: 4167 RVA: 0x0005BC7E File Offset: 0x0005A07E
	private void resetAnimator()
	{
		this.anim.ResetTrigger(this.upTrigger);
		this.anim.ResetTrigger(this.downTrigger);
	}

	// Token: 0x04000E6E RID: 3694
	public Animator anim;

	// Token: 0x04000E6F RID: 3695
	public GameObject[] lamps;

	// Token: 0x04000E70 RID: 3696
	private bool up = true;

	// Token: 0x04000E71 RID: 3697
	public string upTrigger;

	// Token: 0x04000E72 RID: 3698
	public string downTrigger;
}
