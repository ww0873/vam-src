using System;

// Token: 0x02000B2D RID: 2861
public class SetDAZMorphControl : JSONStorable
{
	// Token: 0x06004E59 RID: 20057 RVA: 0x001B9735 File Offset: 0x001B7B35
	public SetDAZMorphControl()
	{
	}

	// Token: 0x06004E5A RID: 20058 RVA: 0x001B9740 File Offset: 0x001B7B40
	protected void SyncEnabled(bool b)
	{
		SetDAZMorph[] components = base.GetComponents<SetDAZMorph>();
		foreach (SetDAZMorph setDAZMorph in components)
		{
			setDAZMorph.enabled = b;
		}
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001B9775 File Offset: 0x001B7B75
	protected void Init()
	{
		this.enabledJSON = new JSONStorableBool("enabled", true, new JSONStorableBool.SetBoolCallback(this.SyncEnabled));
		base.RegisterBool(this.enabledJSON);
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001B97A0 File Offset: 0x001B7BA0
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			SetDAZMorphControlUI componentInChildren = this.UITransform.GetComponentInChildren<SetDAZMorphControlUI>(true);
			if (componentInChildren != null && this.enabledJSON != null)
			{
				this.enabledJSON.toggle = componentInChildren.enabledToggle;
			}
		}
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x001B97F4 File Offset: 0x001B7BF4
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			SetDAZMorphControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<SetDAZMorphControlUI>(true);
			if (componentInChildren != null && this.enabledJSON != null)
			{
				this.enabledJSON.toggleAlt = componentInChildren.enabledToggle;
			}
		}
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001B9847 File Offset: 0x001B7C47
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x04003E09 RID: 15881
	public JSONStorableBool enabledJSON;
}
