using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BE7 RID: 3047
	public class PackageBuilderContentItem : MonoBehaviour
	{
		// Token: 0x060057A4 RID: 22436 RVA: 0x002038F0 File Offset: 0x00201CF0
		public PackageBuilderContentItem()
		{
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060057A5 RID: 22437 RVA: 0x002038F8 File Offset: 0x00201CF8
		public bool IsSelected
		{
			get
			{
				return this.toggle != null && this.toggle.isOn;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060057A6 RID: 22438 RVA: 0x00203918 File Offset: 0x00201D18
		// (set) Token: 0x060057A7 RID: 22439 RVA: 0x00203920 File Offset: 0x00201D20
		public string ItemPath
		{
			get
			{
				return this._itemPath;
			}
			set
			{
				if (this._itemPath != value)
				{
					this._itemPath = value;
					if (this.text != null)
					{
						this.text.text = this._itemPath;
					}
				}
			}
		}

		// Token: 0x0400481D RID: 18461
		public Toggle toggle;

		// Token: 0x0400481E RID: 18462
		public Text text;

		// Token: 0x0400481F RID: 18463
		protected string _itemPath;
	}
}
