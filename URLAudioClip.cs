using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B88 RID: 2952
public class URLAudioClip : NamedAudioClip
{
	// Token: 0x06005321 RID: 21281 RVA: 0x001E0DFD File Offset: 0x001DF1FD
	public URLAudioClip()
	{
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06005322 RID: 21282 RVA: 0x001E0E05 File Offset: 0x001DF205
	// (set) Token: 0x06005323 RID: 21283 RVA: 0x001E0E0D File Offset: 0x001DF20D
	public bool ready
	{
		get
		{
			return this._ready;
		}
		set
		{
			if (this._ready != value)
			{
				this._ready = value;
				if (this.readyToggle != null)
				{
					this.readyToggle.isOn = this._ready;
				}
			}
		}
	}

	// Token: 0x17000C1D RID: 3101
	// (get) Token: 0x06005324 RID: 21284 RVA: 0x001E0E44 File Offset: 0x001DF244
	// (set) Token: 0x06005325 RID: 21285 RVA: 0x001E0E4C File Offset: 0x001DF24C
	public string errorMsg
	{
		get
		{
			return this._errorMsg;
		}
		set
		{
			if (this._errorMsg != value)
			{
				this._errorMsg = value;
				if (this._errorMsg != string.Empty && this.uidText != null)
				{
					this.uidText.text = this._errorMsg;
				}
			}
		}
	}

	// Token: 0x17000C1E RID: 3102
	// (get) Token: 0x06005326 RID: 21286 RVA: 0x001E0EA8 File Offset: 0x001DF2A8
	public bool removed
	{
		get
		{
			return this._removed;
		}
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x001E0EB0 File Offset: 0x001DF2B0
	public void Remove()
	{
		this._removed = true;
		if (this.manager != null)
		{
			this.manager.RemoveClip(this);
		}
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x001E0ED8 File Offset: 0x001DF2D8
	public override void InitUI()
	{
		base.InitUI();
		if (this.removeButton != null)
		{
			this.removeButton.onClick.AddListener(new UnityAction(this.Remove));
		}
		if (this.readyToggle != null)
		{
			this.readyToggle.isOn = this._ready;
		}
	}

	// Token: 0x04004308 RID: 17160
	public string url;

	// Token: 0x04004309 RID: 17161
	public bool fromRestore;

	// Token: 0x0400430A RID: 17162
	public Toggle readyToggle;

	// Token: 0x0400430B RID: 17163
	protected bool _ready;

	// Token: 0x0400430C RID: 17164
	public Text sizeText;

	// Token: 0x0400430D RID: 17165
	public bool error;

	// Token: 0x0400430E RID: 17166
	protected string _errorMsg;

	// Token: 0x0400430F RID: 17167
	protected bool _removed;

	// Token: 0x04004310 RID: 17168
	public RectTransform UIpanel;

	// Token: 0x04004311 RID: 17169
	public Button removeButton;

	// Token: 0x04004312 RID: 17170
	public Slider loadProgressSlider;
}
