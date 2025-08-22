using System;
using UnityEngine;
using UnityEngine.UI;

namespace uFileBrowser
{
	// Token: 0x02000467 RID: 1127
	public class DirectoryButton : MonoBehaviour
	{
		// Token: 0x06001C2B RID: 7211 RVA: 0x0009FAB3 File Offset: 0x0009DEB3
		public DirectoryButton()
		{
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0009FABB File Offset: 0x0009DEBB
		public void OnClick()
		{
			if (this.browser)
			{
				this.browser.OnDirectoryClick(this);
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0009FADC File Offset: 0x0009DEDC
		public void Set(FileBrowser b, string pkg, string pkgFilter, string txt, string path)
		{
			this.browser = b;
			this.package = pkg;
			this.packageFilter = pkgFilter;
			this.text = txt;
			this.fullPath = path;
			if (this.packageLabel != null)
			{
				if (this.package == string.Empty)
				{
					this.packageLabel.gameObject.SetActive(false);
				}
				else
				{
					this.packageLabel.gameObject.SetActive(true);
				}
				this.packageLabel.text = this.package;
			}
			if (this.label != null)
			{
				this.label.text = this.text;
			}
		}

		// Token: 0x040017D7 RID: 6103
		public Text packageLabel;

		// Token: 0x040017D8 RID: 6104
		public Text label;

		// Token: 0x040017D9 RID: 6105
		[HideInInspector]
		public string package;

		// Token: 0x040017DA RID: 6106
		[HideInInspector]
		public string packageFilter;

		// Token: 0x040017DB RID: 6107
		[HideInInspector]
		public string text;

		// Token: 0x040017DC RID: 6108
		[HideInInspector]
		public string fullPath;

		// Token: 0x040017DD RID: 6109
		private FileBrowser browser;
	}
}
