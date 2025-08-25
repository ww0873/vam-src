using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200025D RID: 605
	public class ProjectItemObjectPair
	{
		// Token: 0x06000CB9 RID: 3257 RVA: 0x0004D57E File Offset: 0x0004B97E
		public ProjectItemObjectPair(ProjectItem projectItem, UnityEngine.Object obj)
		{
			this.ProjectItem = projectItem;
			this.Object = obj;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0004D594 File Offset: 0x0004B994
		public bool IsNone
		{
			get
			{
				return this.ProjectItem == null && this.Object is NoneItem;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0004D5B2 File Offset: 0x0004B9B2
		public bool IsSceneObject
		{
			get
			{
				return this.ProjectItem == null && !(this.Object is NoneItem) && this.Object != null;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0004D5DE File Offset: 0x0004B9DE
		public bool IsScene
		{
			get
			{
				return !this.IsNone && this.ProjectItem != null && this.ProjectItem.IsScene;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0004D604 File Offset: 0x0004BA04
		public bool IsFolder
		{
			get
			{
				return !this.IsNone && this.ProjectItem != null && this.ProjectItem.IsFolder;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0004D62A File Offset: 0x0004BA2A
		public bool IsResource
		{
			get
			{
				return !this.IsNone && this.ProjectItem != null && !this.ProjectItem.IsFolder && !this.ProjectItem.IsScene;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0004D663 File Offset: 0x0004BA63
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x0004D66B File Offset: 0x0004BA6B
		public ProjectItem ProjectItem
		{
			[CompilerGenerated]
			get
			{
				return this.<ProjectItem>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ProjectItem>k__BackingField = value;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0004D674 File Offset: 0x0004BA74
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x0004D67C File Offset: 0x0004BA7C
		public UnityEngine.Object Object
		{
			[CompilerGenerated]
			get
			{
				return this.<Object>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Object>k__BackingField = value;
			}
		}

		// Token: 0x04000D16 RID: 3350
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ProjectItem <ProjectItem>k__BackingField;

		// Token: 0x04000D17 RID: 3351
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private UnityEngine.Object <Object>k__BackingField;
	}
}
