using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DD4 RID: 3540
public class MultiButtonPanel : MonoBehaviour
{
	// Token: 0x06006DA7 RID: 28071 RVA: 0x00293A88 File Offset: 0x00291E88
	public MultiButtonPanel()
	{
	}

	// Token: 0x1700100A RID: 4106
	// (get) Token: 0x06006DA8 RID: 28072 RVA: 0x00293A90 File Offset: 0x00291E90
	// (set) Token: 0x06006DA9 RID: 28073 RVA: 0x00293A98 File Offset: 0x00291E98
	public bool showButton1
	{
		get
		{
			return this._showButton1;
		}
		set
		{
			this._showButton1 = value;
			if (this.button1 != null)
			{
				this.button1.gameObject.SetActive(this._showButton1);
			}
		}
	}

	// Token: 0x1700100B RID: 4107
	// (get) Token: 0x06006DAA RID: 28074 RVA: 0x00293AC8 File Offset: 0x00291EC8
	// (set) Token: 0x06006DAB RID: 28075 RVA: 0x00293AD0 File Offset: 0x00291ED0
	public bool showButton2
	{
		get
		{
			return this._showButton2;
		}
		set
		{
			this._showButton2 = value;
			if (this.button2 != null)
			{
				this.button2.gameObject.SetActive(this._showButton2);
			}
		}
	}

	// Token: 0x1700100C RID: 4108
	// (get) Token: 0x06006DAC RID: 28076 RVA: 0x00293B00 File Offset: 0x00291F00
	// (set) Token: 0x06006DAD RID: 28077 RVA: 0x00293B08 File Offset: 0x00291F08
	public bool showButton3
	{
		get
		{
			return this._showButton3;
		}
		set
		{
			this._showButton3 = value;
			if (this.button3 != null)
			{
				this.button3.gameObject.SetActive(this._showButton3);
			}
		}
	}

	// Token: 0x06006DAE RID: 28078 RVA: 0x00293B38 File Offset: 0x00291F38
	protected void Button1Click()
	{
		if (this.button1Text != null && this.buttonCallback != null)
		{
			this.buttonCallback(this.button1Text.text);
		}
	}

	// Token: 0x06006DAF RID: 28079 RVA: 0x00293B6C File Offset: 0x00291F6C
	protected void Button2Click()
	{
		if (this.button2Text != null && this.buttonCallback != null)
		{
			this.buttonCallback(this.button2Text.text);
		}
	}

	// Token: 0x06006DB0 RID: 28080 RVA: 0x00293BA0 File Offset: 0x00291FA0
	protected void Button3Click()
	{
		if (this.button3Text != null && this.buttonCallback != null)
		{
			this.buttonCallback(this.button3Text.text);
		}
	}

	// Token: 0x06006DB1 RID: 28081 RVA: 0x00293BD4 File Offset: 0x00291FD4
	public void SetButton1Text(string t)
	{
		if (this.button1Text != null)
		{
			this.button1Text.text = t;
		}
	}

	// Token: 0x06006DB2 RID: 28082 RVA: 0x00293BF3 File Offset: 0x00291FF3
	public void SetButton2Text(string t)
	{
		if (this.button2Text != null)
		{
			this.button2Text.text = t;
		}
	}

	// Token: 0x06006DB3 RID: 28083 RVA: 0x00293C12 File Offset: 0x00292012
	public void SetButton3Text(string t)
	{
		if (this.button3Text != null)
		{
			this.button3Text.text = t;
		}
	}

	// Token: 0x06006DB4 RID: 28084 RVA: 0x00293C34 File Offset: 0x00292034
	private void OnEnable()
	{
		this.showButton1 = this._showButton1;
		if (this.button1 != null)
		{
			this.button1.onClick.AddListener(new UnityAction(this.Button1Click));
		}
		this.showButton2 = this._showButton2;
		if (this.button2 != null)
		{
			this.button2.onClick.AddListener(new UnityAction(this.Button2Click));
		}
		this.showButton3 = this._showButton3;
		if (this.button3 != null)
		{
			this.button3.onClick.AddListener(new UnityAction(this.Button3Click));
		}
	}

	// Token: 0x06006DB5 RID: 28085 RVA: 0x00293CEC File Offset: 0x002920EC
	private void OnDisable()
	{
		if (this.button1 != null)
		{
			this.button1.onClick.RemoveListener(new UnityAction(this.Button1Click));
		}
		if (this.button2 != null)
		{
			this.button2.onClick.RemoveListener(new UnityAction(this.Button2Click));
		}
		if (this.button3 != null)
		{
			this.button3.onClick.RemoveListener(new UnityAction(this.Button3Click));
		}
	}

	// Token: 0x04005EF7 RID: 24311
	public Text headerText;

	// Token: 0x04005EF8 RID: 24312
	public Button button1;

	// Token: 0x04005EF9 RID: 24313
	public Text button1Text;

	// Token: 0x04005EFA RID: 24314
	public Button button2;

	// Token: 0x04005EFB RID: 24315
	public Text button2Text;

	// Token: 0x04005EFC RID: 24316
	public Button button3;

	// Token: 0x04005EFD RID: 24317
	public Text button3Text;

	// Token: 0x04005EFE RID: 24318
	[SerializeField]
	protected bool _showButton1;

	// Token: 0x04005EFF RID: 24319
	[SerializeField]
	protected bool _showButton2;

	// Token: 0x04005F00 RID: 24320
	[SerializeField]
	protected bool _showButton3;

	// Token: 0x04005F01 RID: 24321
	public MultiButtonPanel.ButtonCallback buttonCallback;

	// Token: 0x02000DD5 RID: 3541
	// (Invoke) Token: 0x06006DB7 RID: 28087
	public delegate void ButtonCallback(string name);
}
