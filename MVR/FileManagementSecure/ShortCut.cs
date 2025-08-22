using System;
using MVR.FileManagement;

namespace MVR.FileManagementSecure
{
	// Token: 0x02000BDF RID: 3039
	[Serializable]
	public class ShortCut
	{
		// Token: 0x060056E8 RID: 22248 RVA: 0x001FD078 File Offset: 0x001FB478
		public ShortCut()
		{
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x060056E9 RID: 22249 RVA: 0x001FD09D File Offset: 0x001FB49D
		public bool isHidden
		{
			get
			{
				return this.directoryEntry != null && this.directoryEntry.IsHidden();
			}
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x001FD0B8 File Offset: 0x001FB4B8
		public bool IsSameAs(ShortCut otherShortCut)
		{
			return !(otherShortCut.path != this.path) && !(otherShortCut.package != this.package) && !(otherShortCut.creator != this.creator) && !(otherShortCut.packageFilter != this.packageFilter) && !(otherShortCut.displayName != this.displayName) && otherShortCut.flatten == this.flatten;
		}

		// Token: 0x04004779 RID: 18297
		public string path;

		// Token: 0x0400477A RID: 18298
		public string package = string.Empty;

		// Token: 0x0400477B RID: 18299
		public string creator = string.Empty;

		// Token: 0x0400477C RID: 18300
		public string packageFilter;

		// Token: 0x0400477D RID: 18301
		public string displayName;

		// Token: 0x0400477E RID: 18302
		public bool isLatest = true;

		// Token: 0x0400477F RID: 18303
		public bool flatten;

		// Token: 0x04004780 RID: 18304
		public bool includeRegularDirsInFlatten;

		// Token: 0x04004781 RID: 18305
		public DirectoryEntry directoryEntry;
	}
}
