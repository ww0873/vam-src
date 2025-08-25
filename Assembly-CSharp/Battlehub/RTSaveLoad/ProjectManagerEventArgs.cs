using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000241 RID: 577
	public class ProjectManagerEventArgs : EventArgs
	{
		// Token: 0x06000BF4 RID: 3060 RVA: 0x0004AAA5 File Offset: 0x00048EA5
		public ProjectManagerEventArgs(ProjectItem[] items)
		{
			this.ProjectItems = items;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0004AAB4 File Offset: 0x00048EB4
		public ProjectManagerEventArgs(ProjectItem item)
		{
			this.ProjectItems = new ProjectItem[]
			{
				item
			};
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0004AACC File Offset: 0x00048ECC
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x0004AAD4 File Offset: 0x00048ED4
		public ProjectItem[] ProjectItems
		{
			[CompilerGenerated]
			get
			{
				return this.<ProjectItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ProjectItems>k__BackingField = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0004AADD File Offset: 0x00048EDD
		public ProjectItem ProjectItem
		{
			get
			{
				if (this.ProjectItems == null || this.ProjectItems.Length == 0)
				{
					return null;
				}
				return this.ProjectItems[0];
			}
		}

		// Token: 0x04000CBE RID: 3262
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ProjectItem[] <ProjectItems>k__BackingField;
	}
}
