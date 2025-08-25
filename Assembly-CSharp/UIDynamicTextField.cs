using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF0 RID: 3568
public class UIDynamicTextField : UIDynamic
{
	// Token: 0x06006E5B RID: 28251 RVA: 0x00296EF3 File Offset: 0x002952F3
	public UIDynamicTextField()
	{
	}

	// Token: 0x17001027 RID: 4135
	// (get) Token: 0x06006E5C RID: 28252 RVA: 0x00296EFB File Offset: 0x002952FB
	// (set) Token: 0x06006E5D RID: 28253 RVA: 0x00296F1F File Offset: 0x0029531F
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

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x06006E5E RID: 28254 RVA: 0x00296F3E File Offset: 0x0029533E
	// (set) Token: 0x06006E5F RID: 28255 RVA: 0x00296F62 File Offset: 0x00295362
	public Color textColor
	{
		get
		{
			if (this.UItext != null)
			{
				return this.UItext.color;
			}
			return Color.black;
		}
		set
		{
			if (this.UItext != null)
			{
				this.UItext.color = value;
			}
		}
	}

	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x06006E60 RID: 28256 RVA: 0x00296F81 File Offset: 0x00295381
	// (set) Token: 0x06006E61 RID: 28257 RVA: 0x00296FA1 File Offset: 0x002953A1
	public string text
	{
		get
		{
			if (this.UItext != null)
			{
				return this.UItext.text;
			}
			return null;
		}
		set
		{
			if (this.UItext != null)
			{
				this.UItext.text = value;
			}
		}
	}

	// Token: 0x04005F80 RID: 24448
	public Text UItext;

	// Token: 0x04005F81 RID: 24449
	public Image backgroundImage;
}
