using System;
using UnityEngine;

// Token: 0x02000DFC RID: 3580
public class UIPopupTrack : MonoBehaviour
{
	// Token: 0x06006EB7 RID: 28343 RVA: 0x00298B31 File Offset: 0x00296F31
	public UIPopupTrack()
	{
	}

	// Token: 0x06006EB8 RID: 28344 RVA: 0x00298B39 File Offset: 0x00296F39
	private void Update()
	{
		if (this.master != null && this.slave != null)
		{
			this.slave.currentValue = this.master.currentValue;
		}
	}

	// Token: 0x06006EB9 RID: 28345 RVA: 0x00298B73 File Offset: 0x00296F73
	private void Start()
	{
		this.slave = base.GetComponent<UIPopup>();
	}

	// Token: 0x04005FC5 RID: 24517
	public UIPopup master;

	// Token: 0x04005FC6 RID: 24518
	protected UIPopup slave;
}
