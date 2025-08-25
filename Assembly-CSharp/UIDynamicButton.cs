using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DEC RID: 3564
public class UIDynamicButton : UIDynamic
{
	// Token: 0x06006E29 RID: 28201 RVA: 0x00295D6D File Offset: 0x0029416D
	public UIDynamicButton()
	{
	}

	// Token: 0x17001017 RID: 4119
	// (get) Token: 0x06006E2A RID: 28202 RVA: 0x00295D75 File Offset: 0x00294175
	// (set) Token: 0x06006E2B RID: 28203 RVA: 0x00295D99 File Offset: 0x00294199
	public Color buttonColor
	{
		get
		{
			if (this.buttonImage != null)
			{
				return this.buttonImage.color;
			}
			return Color.black;
		}
		set
		{
			if (this.buttonImage != null)
			{
				this.buttonImage.color = value;
			}
		}
	}

	// Token: 0x17001018 RID: 4120
	// (get) Token: 0x06006E2C RID: 28204 RVA: 0x00295DB8 File Offset: 0x002941B8
	// (set) Token: 0x06006E2D RID: 28205 RVA: 0x00295DDC File Offset: 0x002941DC
	public Color textColor
	{
		get
		{
			if (this.buttonText != null)
			{
				return this.buttonText.color;
			}
			return Color.black;
		}
		set
		{
			if (this.buttonText != null)
			{
				this.buttonText.color = value;
			}
		}
	}

	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x06006E2E RID: 28206 RVA: 0x00295DFB File Offset: 0x002941FB
	// (set) Token: 0x06006E2F RID: 28207 RVA: 0x00295E1B File Offset: 0x0029421B
	public string label
	{
		get
		{
			if (this.buttonText != null)
			{
				return this.buttonText.text;
			}
			return null;
		}
		set
		{
			if (this.buttonText != null)
			{
				this.buttonText.text = value;
			}
		}
	}

	// Token: 0x04005F63 RID: 24419
	public Button button;

	// Token: 0x04005F64 RID: 24420
	public Text buttonText;

	// Token: 0x04005F65 RID: 24421
	public Image buttonImage;
}
