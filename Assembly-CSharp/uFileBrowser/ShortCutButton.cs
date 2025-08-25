using System;
using UnityEngine;
using UnityEngine.UI;

namespace uFileBrowser
{
	// Token: 0x02000472 RID: 1138
	public class ShortCutButton : MonoBehaviour
	{
		// Token: 0x06001CDB RID: 7387 RVA: 0x000A49A2 File Offset: 0x000A2DA2
		public ShortCutButton()
		{
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000A49AA File Offset: 0x000A2DAA
		public void OnClick()
		{
			if (this.browser)
			{
				this.browser.OnShortCutClick(this.id);
			}
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000A49D0 File Offset: 0x000A2DD0
		public void Set(FileBrowser b, string pkg, string pkgFilter, bool flat, bool includeRegDirsInFlat, string txt, string path, int i)
		{
			this.browser = b;
			this.package = pkg;
			this.packageFilter = pkgFilter;
			this.flatten = flat;
			this.includeRegularDirsInFlatten = includeRegDirsInFlat;
			this.text = txt;
			this.fullPath = path;
			this.id = i;
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

		// Token: 0x04001880 RID: 6272
		public Text packageLabel;

		// Token: 0x04001881 RID: 6273
		public Text label;

		// Token: 0x04001882 RID: 6274
		[HideInInspector]
		public string package;

		// Token: 0x04001883 RID: 6275
		[HideInInspector]
		public string packageFilter;

		// Token: 0x04001884 RID: 6276
		[HideInInspector]
		public bool flatten;

		// Token: 0x04001885 RID: 6277
		[HideInInspector]
		public bool includeRegularDirsInFlatten;

		// Token: 0x04001886 RID: 6278
		[HideInInspector]
		public string text;

		// Token: 0x04001887 RID: 6279
		[HideInInspector]
		public string fullPath;

		// Token: 0x04001888 RID: 6280
		[HideInInspector]
		public int id;

		// Token: 0x04001889 RID: 6281
		private FileBrowser browser;
	}
}
