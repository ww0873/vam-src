using System;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CCE RID: 3278
public class JSONStorableAction
{
	// Token: 0x06006326 RID: 25382 RVA: 0x0025CDE1 File Offset: 0x0025B1E1
	public JSONStorableAction(string n, JSONStorableAction.ActionCallback callback)
	{
		this.name = n;
		this.actionCallback = callback;
	}

	// Token: 0x06006327 RID: 25383 RVA: 0x0025CDFE File Offset: 0x0025B1FE
	protected void DoCallback()
	{
		this.actionCallback();
	}

	// Token: 0x06006328 RID: 25384 RVA: 0x0025CE0B File Offset: 0x0025B20B
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

	// Token: 0x17000E86 RID: 3718
	// (get) Token: 0x06006329 RID: 25385 RVA: 0x0025CE26 File Offset: 0x0025B226
	// (set) Token: 0x0600632A RID: 25386 RVA: 0x0025CE30 File Offset: 0x0025B230
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

	// Token: 0x17000E87 RID: 3719
	// (get) Token: 0x0600632B RID: 25387 RVA: 0x0025CE83 File Offset: 0x0025B283
	// (set) Token: 0x0600632C RID: 25388 RVA: 0x0025CE8C File Offset: 0x0025B28C
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

	// Token: 0x0600632D RID: 25389 RVA: 0x0025CEDF File Offset: 0x0025B2DF
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

	// Token: 0x17000E88 RID: 3720
	// (get) Token: 0x0600632E RID: 25390 RVA: 0x0025CEFA File Offset: 0x0025B2FA
	// (set) Token: 0x0600632F RID: 25391 RVA: 0x0025CF04 File Offset: 0x0025B304
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
					this._button.interactable = this._interactable;
					this._button.onClick.AddListener(new UnityAction(this.DoCallback));
				}
			}
		}
	}

	// Token: 0x17000E89 RID: 3721
	// (get) Token: 0x06006330 RID: 25392 RVA: 0x0025CF94 File Offset: 0x0025B394
	// (set) Token: 0x06006331 RID: 25393 RVA: 0x0025CF9C File Offset: 0x0025B39C
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
					this._buttonAlt.interactable = this._interactable;
					this._buttonAlt.onClick.AddListener(new UnityAction(this.DoCallback));
				}
			}
		}
	}

	// Token: 0x17000E8A RID: 3722
	// (get) Token: 0x06006332 RID: 25394 RVA: 0x0025D02C File Offset: 0x0025B42C
	// (set) Token: 0x06006333 RID: 25395 RVA: 0x0025D034 File Offset: 0x0025B434
	public bool interactable
	{
		get
		{
			return this._interactable;
		}
		set
		{
			if (this._interactable != value)
			{
				this._interactable = value;
				if (this._button != null)
				{
					this._button.interactable = this._interactable;
				}
				if (this._buttonAlt != null)
				{
					this._buttonAlt.interactable = this._interactable;
				}
			}
		}
	}

	// Token: 0x040053BE RID: 21438
	public string name;

	// Token: 0x040053BF RID: 21439
	public JSONStorableAction.ActionCallback actionCallback;

	// Token: 0x040053C0 RID: 21440
	public JSONStorable storable;

	// Token: 0x040053C1 RID: 21441
	protected UIDynamicButton _dynamicButton;

	// Token: 0x040053C2 RID: 21442
	protected UIDynamicButton _dynamicButtonAlt;

	// Token: 0x040053C3 RID: 21443
	protected Button _button;

	// Token: 0x040053C4 RID: 21444
	protected Button _buttonAlt;

	// Token: 0x040053C5 RID: 21445
	protected bool _interactable = true;

	// Token: 0x02000CCF RID: 3279
	// (Invoke) Token: 0x06006335 RID: 25397
	public delegate void ActionCallback();
}
