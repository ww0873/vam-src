using System;

// Token: 0x02000A96 RID: 2710
public class AdjustJointPositionTargetsAndSetDAZMorph : AdjustJointPositionTargets
{
	// Token: 0x0600465F RID: 18015 RVA: 0x00141136 File Offset: 0x0013F536
	public AdjustJointPositionTargetsAndSetDAZMorph()
	{
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x0014113E File Offset: 0x0013F53E
	protected void Init()
	{
		this.setDAZMorph = base.GetComponent<SetDAZMorph>();
	}

	// Token: 0x06004661 RID: 18017 RVA: 0x0014114C File Offset: 0x0013F54C
	protected override void Adjust()
	{
		base.Adjust();
		if (this.setDAZMorph == null)
		{
			this.Init();
		}
		if (this.on && this.setDAZMorph != null)
		{
			this.setDAZMorph.morphPercent = this._percent;
		}
	}

	// Token: 0x06004662 RID: 18018 RVA: 0x001411A3 File Offset: 0x0013F5A3
	public void OnEnable()
	{
		this.Adjust();
	}

	// Token: 0x040033C4 RID: 13252
	protected SetDAZMorph setDAZMorph;
}
