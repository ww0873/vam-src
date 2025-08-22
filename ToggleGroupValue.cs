using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE7 RID: 3559
public class ToggleGroupValue : MonoBehaviour
{
	// Token: 0x06006E0A RID: 28170 RVA: 0x002955FE File Offset: 0x002939FE
	public ToggleGroupValue()
	{
	}

	// Token: 0x17001012 RID: 4114
	// (get) Token: 0x06006E0B RID: 28171 RVA: 0x00295606 File Offset: 0x00293A06
	// (set) Token: 0x06006E0C RID: 28172 RVA: 0x00295610 File Offset: 0x00293A10
	public string activeToggleName
	{
		get
		{
			return this._activeToggleName;
		}
		set
		{
			if (this._activeToggleName != value)
			{
				this.Init();
				bool flag = false;
				this.disableCallback = true;
				foreach (Toggle toggle in this.toggles)
				{
					if (toggle.name == value)
					{
						flag = true;
						toggle.isOn = true;
						this._activeToggleName = value;
					}
					else
					{
						toggle.isOn = false;
					}
				}
				if (flag && this.onToggleChangedHandlers != null)
				{
					this.onToggleChangedHandlers(this._activeToggleName);
				}
				this.disableCallback = false;
			}
		}
	}

	// Token: 0x17001013 RID: 4115
	// (get) Token: 0x06006E0D RID: 28173 RVA: 0x002956B2 File Offset: 0x00293AB2
	// (set) Token: 0x06006E0E RID: 28174 RVA: 0x002956BC File Offset: 0x00293ABC
	public string activeToggleNameNoCallback
	{
		get
		{
			return this._activeToggleName;
		}
		set
		{
			if (this._activeToggleName != value)
			{
				this.Init();
				this.disableCallback = true;
				foreach (Toggle toggle in this.toggles)
				{
					if (toggle.name == value)
					{
						toggle.isOn = true;
						this._activeToggleName = value;
					}
					else
					{
						toggle.isOn = false;
					}
				}
				this.disableCallback = false;
			}
		}
	}

	// Token: 0x06006E0F RID: 28175 RVA: 0x00295738 File Offset: 0x00293B38
	public void Init()
	{
		if (this.toggles == null)
		{
			ToggleGroup component = base.GetComponent<ToggleGroup>();
			this.toggles = base.GetComponentsInChildren<Toggle>(true);
			if (component != null && this.toggles != null)
			{
				foreach (Toggle toggle in this.toggles)
				{
					if (toggle.group != component)
					{
						toggle.group = component;
					}
				}
			}
		}
	}

	// Token: 0x06006E10 RID: 28176 RVA: 0x002957B2 File Offset: 0x00293BB2
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06006E11 RID: 28177 RVA: 0x002957BA File Offset: 0x00293BBA
	public void ToggleChanged(bool b)
	{
		this.ToggleChanged();
	}

	// Token: 0x06006E12 RID: 28178 RVA: 0x002957C4 File Offset: 0x00293BC4
	public void ToggleChanged()
	{
		this.Init();
		if (this.toggles != null && !this.disableCallback)
		{
			foreach (Toggle toggle in this.toggles)
			{
				if (toggle.isOn && this._activeToggleName != toggle.name)
				{
					this._activeToggleName = toggle.name;
					if (this.onToggleChangedHandlers != null)
					{
						this.onToggleChangedHandlers(this._activeToggleName);
					}
				}
			}
		}
	}

	// Token: 0x04005F48 RID: 24392
	private string _activeToggleName;

	// Token: 0x04005F49 RID: 24393
	private bool disableCallback;

	// Token: 0x04005F4A RID: 24394
	private Toggle[] toggles;

	// Token: 0x04005F4B RID: 24395
	public ToggleGroupValue.OnToggleChanged onToggleChangedHandlers;

	// Token: 0x02000DE8 RID: 3560
	// (Invoke) Token: 0x06006E14 RID: 28180
	public delegate void OnToggleChanged(string activeToggle);
}
