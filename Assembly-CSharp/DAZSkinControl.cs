using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B03 RID: 2819
public class DAZSkinControl : MonoBehaviour
{
	// Token: 0x06004CA5 RID: 19621 RVA: 0x001AE4B1 File Offset: 0x001AC8B1
	public DAZSkinControl()
	{
	}

	// Token: 0x17000ADC RID: 2780
	// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x001AE4B9 File Offset: 0x001AC8B9
	// (set) Token: 0x06004CA7 RID: 19623 RVA: 0x001AE4C1 File Offset: 0x001AC8C1
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			if (this._skin != value)
			{
				this._skin = value;
				this.SyncUseEarlyFinish();
			}
		}
	}

	// Token: 0x06004CA8 RID: 19624 RVA: 0x001AE4E1 File Offset: 0x001AC8E1
	protected void SyncUseEarlyFinish()
	{
		if (this.skin != null)
		{
			this.skin.useEarlyFinish = this._useEarlyFinish;
		}
	}

	// Token: 0x17000ADD RID: 2781
	// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x001AE505 File Offset: 0x001AC905
	// (set) Token: 0x06004CAA RID: 19626 RVA: 0x001AE50D File Offset: 0x001AC90D
	public bool useEarlyFinish
	{
		get
		{
			return this._useEarlyFinish;
		}
		set
		{
			if (this._useEarlyFinish != value)
			{
				this._useEarlyFinish = value;
				if (this.useEarlyFinishToggle != null)
				{
					this.useEarlyFinishToggle.isOn = value;
				}
				this.SyncUseEarlyFinish();
			}
		}
	}

	// Token: 0x06004CAB RID: 19627 RVA: 0x001AE548 File Offset: 0x001AC948
	protected void InitUI()
	{
		if (this.useEarlyFinishToggle != null)
		{
			this.useEarlyFinishToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__0));
			this.useEarlyFinishToggle.isOn = this._useEarlyFinish;
			this.SyncUseEarlyFinish();
		}
	}

	// Token: 0x06004CAC RID: 19628 RVA: 0x001AE599 File Offset: 0x001AC999
	private void Start()
	{
		this.InitUI();
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x001AE5A1 File Offset: 0x001AC9A1
	[CompilerGenerated]
	private void <InitUI>m__0(bool A_1)
	{
		this.useEarlyFinish = this.useEarlyFinishToggle.isOn;
	}

	// Token: 0x04003B97 RID: 15255
	protected DAZSkinV2 _skin;

	// Token: 0x04003B98 RID: 15256
	public Toggle useEarlyFinishToggle;

	// Token: 0x04003B99 RID: 15257
	[SerializeField]
	protected bool _useEarlyFinish;
}
