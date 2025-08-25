using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B7F RID: 2943
[Serializable]
public class NamedAudioClip
{
	// Token: 0x060052C2 RID: 21186 RVA: 0x001DEC48 File Offset: 0x001DD048
	public NamedAudioClip()
	{
	}

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x060052C3 RID: 21187 RVA: 0x001DEC50 File Offset: 0x001DD050
	public AudioClip clipToPlay
	{
		get
		{
			return this.sourceClip;
		}
	}

	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x060052C4 RID: 21188 RVA: 0x001DEC58 File Offset: 0x001DD058
	// (set) Token: 0x060052C5 RID: 21189 RVA: 0x001DEC60 File Offset: 0x001DD060
	public string uid
	{
		get
		{
			return this._uid;
		}
		set
		{
			if (this._uid != value)
			{
				this._uid = value;
				if (this.uidText != null)
				{
					this.uidText.text = this._uid;
				}
			}
		}
	}

	// Token: 0x060052C6 RID: 21190 RVA: 0x001DEC9C File Offset: 0x001DD09C
	protected void SetDisplayName(string v)
	{
		this.displayName = v;
	}

	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x060052C7 RID: 21191 RVA: 0x001DECA5 File Offset: 0x001DD0A5
	// (set) Token: 0x060052C8 RID: 21192 RVA: 0x001DECAD File Offset: 0x001DD0AD
	public string displayName
	{
		get
		{
			return this._displayName;
		}
		set
		{
			if (this._displayName != value)
			{
				this._displayName = value;
				if (this.displayNameField != null)
				{
					this.displayNameField.text = this._displayName;
				}
			}
		}
	}

	// Token: 0x060052C9 RID: 21193 RVA: 0x001DECE9 File Offset: 0x001DD0E9
	protected void SetCategory(string c)
	{
		this.category = c;
	}

	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x060052CA RID: 21194 RVA: 0x001DECF2 File Offset: 0x001DD0F2
	// (set) Token: 0x060052CB RID: 21195 RVA: 0x001DECFA File Offset: 0x001DD0FA
	public string category
	{
		get
		{
			return this._category;
		}
		set
		{
			if (this._category != value)
			{
				this._category = value;
				if (this.categoryField != null)
				{
					this.categoryField.text = this._category;
				}
			}
		}
	}

	// Token: 0x060052CC RID: 21196 RVA: 0x001DED36 File Offset: 0x001DD136
	public void Test()
	{
		if (this.manager != null)
		{
			this.manager.TestClip(this);
		}
	}

	// Token: 0x060052CD RID: 21197 RVA: 0x001DED58 File Offset: 0x001DD158
	public virtual void InitUI()
	{
		if (this.uidText != null)
		{
			this.uidText.text = this._uid;
		}
		if (this.displayNameField != null)
		{
			this.displayNameField.text = this._displayName;
			this.displayNameField.onEndEdit.AddListener(new UnityAction<string>(this.SetDisplayName));
		}
		if (this.categoryField != null)
		{
			this.categoryField.text = this._category;
			this.categoryField.onEndEdit.AddListener(new UnityAction<string>(this.SetCategory));
		}
		if (this.testButton != null)
		{
			this.testButton.onClick.AddListener(new UnityAction(this.Test));
		}
	}

	// Token: 0x0400429B RID: 17051
	public AudioClipManager manager;

	// Token: 0x0400429C RID: 17052
	public AudioClip sourceClip;

	// Token: 0x0400429D RID: 17053
	public bool destroyed;

	// Token: 0x0400429E RID: 17054
	public Text uidText;

	// Token: 0x0400429F RID: 17055
	[SerializeField]
	protected string _uid;

	// Token: 0x040042A0 RID: 17056
	public InputField displayNameField;

	// Token: 0x040042A1 RID: 17057
	[SerializeField]
	protected string _displayName;

	// Token: 0x040042A2 RID: 17058
	public InputField categoryField;

	// Token: 0x040042A3 RID: 17059
	[SerializeField]
	protected string _category;

	// Token: 0x040042A4 RID: 17060
	public Button testButton;

	// Token: 0x040042A5 RID: 17061
	public Text testButtonText;
}
