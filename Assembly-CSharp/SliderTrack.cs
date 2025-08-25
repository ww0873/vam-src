using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE2 RID: 3554
public class SliderTrack : MonoBehaviour
{
	// Token: 0x06006DF3 RID: 28147 RVA: 0x002949A5 File Offset: 0x00292DA5
	public SliderTrack()
	{
	}

	// Token: 0x06006DF4 RID: 28148 RVA: 0x002949AD File Offset: 0x00292DAD
	private void Update()
	{
		if (this.master != null && this.slave != null)
		{
			this.slave.value = this.master.value;
		}
	}

	// Token: 0x06006DF5 RID: 28149 RVA: 0x002949E7 File Offset: 0x00292DE7
	private void Start()
	{
		this.slave = base.GetComponent<Slider>();
	}

	// Token: 0x04005F2F RID: 24367
	public Slider master;

	// Token: 0x04005F30 RID: 24368
	protected Slider slave;
}
