using System;
using UnityEngine;

// Token: 0x020009EA RID: 2538
public class DemoControl : MonoBehaviour
{
	// Token: 0x06003FE9 RID: 16361 RVA: 0x00130AF3 File Offset: 0x0012EEF3
	public DemoControl()
	{
	}

	// Token: 0x06003FEA RID: 16362 RVA: 0x00130AFB File Offset: 0x0012EEFB
	private void Start()
	{
	}

	// Token: 0x06003FEB RID: 16363 RVA: 0x00130AFD File Offset: 0x0012EEFD
	private void Update()
	{
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x00130AFF File Offset: 0x0012EEFF
	public void ToggleAnimation()
	{
		if (this.anim.isActiveAndEnabled)
		{
			this.anim.enabled = false;
		}
		else
		{
			this.anim.enabled = true;
		}
	}

	// Token: 0x06003FED RID: 16365 RVA: 0x00130B30 File Offset: 0x0012EF30
	public void NextStyle()
	{
		int num = 0;
		for (int i = 0; i < this.styles.childCount; i++)
		{
			Transform child = this.styles.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				num = i;
				break;
			}
		}
		int num2 = num + 1;
		if (num2 >= this.styles.childCount)
		{
			num2 = 0;
		}
		for (int j = 0; j < this.styles.childCount; j++)
		{
			Transform child2 = this.styles.GetChild(j);
			child2.gameObject.SetActive(false);
		}
		Transform child3 = this.styles.GetChild(num2);
		child3.gameObject.SetActive(true);
	}

	// Token: 0x04003049 RID: 12361
	public Animator anim;

	// Token: 0x0400304A RID: 12362
	public Transform styles;
}
