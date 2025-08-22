using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF1 RID: 3569
public class UIDynamicToggle : UIDynamic
{
	// Token: 0x06006E62 RID: 28258 RVA: 0x00296FC0 File Offset: 0x002953C0
	public UIDynamicToggle()
	{
	}

	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x06006E63 RID: 28259 RVA: 0x00296FC8 File Offset: 0x002953C8
	// (set) Token: 0x06006E64 RID: 28260 RVA: 0x00296FEC File Offset: 0x002953EC
	public Color backgroundColor
	{
		get
		{
			if (this.backgroundImage != null)
			{
				return this.backgroundImage.color;
			}
			return Color.black;
		}
		set
		{
			if (this.backgroundImage != null)
			{
				this.backgroundImage.color = value;
			}
		}
	}

	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x06006E65 RID: 28261 RVA: 0x0029700B File Offset: 0x0029540B
	// (set) Token: 0x06006E66 RID: 28262 RVA: 0x0029702F File Offset: 0x0029542F
	public Color textColor
	{
		get
		{
			if (this.labelText != null)
			{
				return this.labelText.color;
			}
			return Color.black;
		}
		set
		{
			if (this.labelText != null)
			{
				this.labelText.color = value;
			}
		}
	}

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x06006E67 RID: 28263 RVA: 0x0029704E File Offset: 0x0029544E
	// (set) Token: 0x06006E68 RID: 28264 RVA: 0x0029706E File Offset: 0x0029546E
	public string label
	{
		get
		{
			if (this.labelText != null)
			{
				return this.labelText.text;
			}
			return null;
		}
		set
		{
			if (this.labelText != null)
			{
				this.labelText.text = value;
			}
		}
	}

	// Token: 0x04005F82 RID: 24450
	public Toggle toggle;

	// Token: 0x04005F83 RID: 24451
	public Text labelText;

	// Token: 0x04005F84 RID: 24452
	public Image backgroundImage;
}
