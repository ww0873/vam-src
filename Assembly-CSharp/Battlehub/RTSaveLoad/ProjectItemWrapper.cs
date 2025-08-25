using System;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200025C RID: 604
	public class ProjectItemWrapper : ScriptableObject
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x0004D4F0 File Offset: 0x0004B8F0
		public ProjectItemWrapper()
		{
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0004D4F8 File Offset: 0x0004B8F8
		// (set) Token: 0x06000CB5 RID: 3253 RVA: 0x0004D500 File Offset: 0x0004B900
		public ProjectItem ProjectItem
		{
			get
			{
				return this.m_projectItem;
			}
			set
			{
				this.m_projectItem = value;
				if (this.m_projectItem == null)
				{
					base.name = "None";
				}
				else
				{
					base.name = this.m_projectItem.Name;
				}
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0004D535 File Offset: 0x0004B935
		public bool IsNone
		{
			get
			{
				return this.ProjectItem == null;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0004D540 File Offset: 0x0004B940
		public bool IsScene
		{
			get
			{
				return !this.IsNone && this.ProjectItem.IsScene;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0004D55B File Offset: 0x0004B95B
		public bool IsFolder
		{
			get
			{
				return !this.IsNone && this.ProjectItem.IsFolder;
			}
		}

		// Token: 0x04000D15 RID: 3349
		[NonSerialized]
		private ProjectItem m_projectItem;
	}
}
