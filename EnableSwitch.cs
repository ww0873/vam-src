using System;
using UnityEngine;

// Token: 0x020007C1 RID: 1985
public class EnableSwitch : MonoBehaviour
{
	// Token: 0x06003268 RID: 12904 RVA: 0x00106DD4 File Offset: 0x001051D4
	public EnableSwitch()
	{
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x00106DDC File Offset: 0x001051DC
	public bool SetActive(int target)
	{
		if (target < 0 || target >= this.SwitchTargets.Length)
		{
			return false;
		}
		for (int i = 0; i < this.SwitchTargets.Length; i++)
		{
			this.SwitchTargets[i].SetActive(false);
		}
		this.SwitchTargets[target].SetActive(true);
		return true;
	}

	// Token: 0x0400268C RID: 9868
	public GameObject[] SwitchTargets;
}
