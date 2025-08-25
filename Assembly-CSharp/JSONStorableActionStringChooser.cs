using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CD6 RID: 3286
public class JSONStorableActionStringChooser
{
	// Token: 0x06006348 RID: 25416 RVA: 0x0025D105 File Offset: 0x0025B505
	public JSONStorableActionStringChooser(string n, JSONStorableActionStringChooser.StringChoiceActionCallback callback, JSONStorableStringChooser chooser)
	{
		this.name = n;
		this.actionCallback = callback;
		this._chooser = chooser;
	}

	// Token: 0x17000E8B RID: 3723
	// (get) Token: 0x06006349 RID: 25417 RVA: 0x0025D122 File Offset: 0x0025B522
	public List<string> choices
	{
		get
		{
			if (this._chooser != null)
			{
				return this._chooser.choices;
			}
			return null;
		}
	}

	// Token: 0x17000E8C RID: 3724
	// (get) Token: 0x0600634A RID: 25418 RVA: 0x0025D13C File Offset: 0x0025B53C
	public List<string> displayChoices
	{
		get
		{
			if (this._chooser != null)
			{
				return this._chooser.displayChoices;
			}
			return null;
		}
	}

	// Token: 0x17000E8D RID: 3725
	// (get) Token: 0x0600634B RID: 25419 RVA: 0x0025D156 File Offset: 0x0025B556
	public string choice
	{
		get
		{
			if (this._chooser != null)
			{
				return this._chooser.val;
			}
			return null;
		}
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x0025D170 File Offset: 0x0025B570
	protected void DoCallback()
	{
		this.actionCallback(this.choice);
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x0025D183 File Offset: 0x0025B583
	public void RegisterButton(UIDynamicButton b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.dynamicButtonAlt = b;
		}
		else
		{
			this.dynamicButton = b;
		}
	}

	// Token: 0x17000E8E RID: 3726
	// (get) Token: 0x0600634E RID: 25422 RVA: 0x0025D19E File Offset: 0x0025B59E
	// (set) Token: 0x0600634F RID: 25423 RVA: 0x0025D1A8 File Offset: 0x0025B5A8
	public UIDynamicButton dynamicButton
	{
		get
		{
			return this._dynamicButton;
		}
		set
		{
			if (this._dynamicButton != value)
			{
				this._dynamicButton = value;
				if (this._dynamicButton != null)
				{
					this.button = this._dynamicButton.button;
				}
				else
				{
					this.button = null;
				}
			}
		}
	}

	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x06006350 RID: 25424 RVA: 0x0025D1FB File Offset: 0x0025B5FB
	// (set) Token: 0x06006351 RID: 25425 RVA: 0x0025D204 File Offset: 0x0025B604
	public UIDynamicButton dynamicButtonAlt
	{
		get
		{
			return this._dynamicButtonAlt;
		}
		set
		{
			if (this._dynamicButtonAlt != value)
			{
				this._dynamicButtonAlt = value;
				if (this._dynamicButtonAlt != null)
				{
					this.buttonAlt = this._dynamicButtonAlt.button;
				}
				else
				{
					this.buttonAlt = null;
				}
			}
		}
	}

	// Token: 0x06006352 RID: 25426 RVA: 0x0025D257 File Offset: 0x0025B657
	public void RegisterButton(Button b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.buttonAlt = b;
		}
		else
		{
			this.button = b;
		}
	}

	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x06006353 RID: 25427 RVA: 0x0025D272 File Offset: 0x0025B672
	// (set) Token: 0x06006354 RID: 25428 RVA: 0x0025D27C File Offset: 0x0025B67C
	public Button button
	{
		get
		{
			return this._button;
		}
		set
		{
			if (this._button != value)
			{
				if (this._button != null)
				{
					this._button.onClick.RemoveListener(new UnityAction(this.DoCallback));
				}
				this._button = value;
				if (this._button != null)
				{
					this._button.onClick.AddListener(new UnityAction(this.DoCallback));
				}
			}
		}
	}

	// Token: 0x17000E91 RID: 3729
	// (get) Token: 0x06006355 RID: 25429 RVA: 0x0025D2FB File Offset: 0x0025B6FB
	// (set) Token: 0x06006356 RID: 25430 RVA: 0x0025D304 File Offset: 0x0025B704
	public Button buttonAlt
	{
		get
		{
			return this._buttonAlt;
		}
		set
		{
			if (this._buttonAlt != value)
			{
				if (this._buttonAlt != null)
				{
					this._buttonAlt.onClick.RemoveListener(new UnityAction(this.DoCallback));
				}
				this._buttonAlt = value;
				if (this._buttonAlt != null)
				{
					this._buttonAlt.onClick.AddListener(new UnityAction(this.DoCallback));
				}
			}
		}
	}

	// Token: 0x040053D0 RID: 21456
	protected JSONStorableStringChooser _chooser;

	// Token: 0x040053D1 RID: 21457
	public string name;

	// Token: 0x040053D2 RID: 21458
	public JSONStorableActionStringChooser.StringChoiceActionCallback actionCallback;

	// Token: 0x040053D3 RID: 21459
	public JSONStorable storable;

	// Token: 0x040053D4 RID: 21460
	protected UIDynamicButton _dynamicButton;

	// Token: 0x040053D5 RID: 21461
	protected UIDynamicButton _dynamicButtonAlt;

	// Token: 0x040053D6 RID: 21462
	protected Button _button;

	// Token: 0x040053D7 RID: 21463
	protected Button _buttonAlt;

	// Token: 0x02000CD7 RID: 3287
	// (Invoke) Token: 0x06006358 RID: 25432
	public delegate void StringChoiceActionCallback(string choice);
}
