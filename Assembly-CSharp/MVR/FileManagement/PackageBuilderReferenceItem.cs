using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BE9 RID: 3049
	public class PackageBuilderReferenceItem : MonoBehaviour
	{
		// Token: 0x060057AC RID: 22444 RVA: 0x002039C7 File Offset: 0x00201DC7
		public PackageBuilderReferenceItem()
		{
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060057AD RID: 22445 RVA: 0x002039CF File Offset: 0x00201DCF
		// (set) Token: 0x060057AE RID: 22446 RVA: 0x002039D7 File Offset: 0x00201DD7
		public string Reference
		{
			get
			{
				return this._reference;
			}
			set
			{
				if (this._reference != value)
				{
					this._reference = value;
					if (this.text != null)
					{
						this.text.text = this._reference;
					}
				}
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060057AF RID: 22447 RVA: 0x00203A13 File Offset: 0x00201E13
		// (set) Token: 0x060057B0 RID: 22448 RVA: 0x00203A1C File Offset: 0x00201E1C
		public string Issue
		{
			get
			{
				return this._issue;
			}
			set
			{
				if (this._issue != value)
				{
					this._issue = value;
					if (this.issueText != null)
					{
						if (this._issue == string.Empty)
						{
							this.issueText.gameObject.SetActive(false);
						}
						else
						{
							this.issueText.gameObject.SetActive(true);
						}
						this.issueText.text = this._issue;
					}
				}
			}
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x00203A9F File Offset: 0x00201E9F
		public void SetColor(Color c)
		{
			if (this.image != null)
			{
				this.image.color = c;
			}
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x00203ABE File Offset: 0x00201EBE
		public void SetIssueColor(Color c)
		{
			if (this.issueText != null)
			{
				this.issueText.color = c;
			}
		}

		// Token: 0x04004824 RID: 18468
		public Text text;

		// Token: 0x04004825 RID: 18469
		protected string _reference;

		// Token: 0x04004826 RID: 18470
		public Text issueText;

		// Token: 0x04004827 RID: 18471
		protected string _issue;

		// Token: 0x04004828 RID: 18472
		public Image image;
	}
}
