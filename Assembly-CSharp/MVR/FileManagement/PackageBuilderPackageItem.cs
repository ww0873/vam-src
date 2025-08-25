using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BE8 RID: 3048
	public class PackageBuilderPackageItem : MonoBehaviour
	{
		// Token: 0x060057A8 RID: 22440 RVA: 0x0020395C File Offset: 0x00201D5C
		public PackageBuilderPackageItem()
		{
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060057A9 RID: 22441 RVA: 0x00203964 File Offset: 0x00201D64
		// (set) Token: 0x060057AA RID: 22442 RVA: 0x0020396C File Offset: 0x00201D6C
		public string Package
		{
			get
			{
				return this._package;
			}
			set
			{
				if (this._package != value)
				{
					this._package = value;
					if (this.button != null)
					{
						this.text.text = this._package;
					}
				}
			}
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x002039A8 File Offset: 0x00201DA8
		public void SetColor(Color c)
		{
			if (this.image != null)
			{
				this.image.color = c;
			}
		}

		// Token: 0x04004820 RID: 18464
		public Button button;

		// Token: 0x04004821 RID: 18465
		public Text text;

		// Token: 0x04004822 RID: 18466
		protected string _package;

		// Token: 0x04004823 RID: 18467
		public Image image;
	}
}
