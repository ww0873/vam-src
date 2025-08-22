using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Battlehub.UIControls
{
	// Token: 0x02000298 RID: 664
	public class TreeViewItemContainerData : ItemContainerData
	{
		// Token: 0x06000F8B RID: 3979 RVA: 0x000594BB File Offset: 0x000578BB
		public TreeViewItemContainerData()
		{
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06000F8C RID: 3980 RVA: 0x000594C4 File Offset: 0x000578C4
		// (remove) Token: 0x06000F8D RID: 3981 RVA: 0x000594F8 File Offset: 0x000578F8
		public static event EventHandler<VirtualizingParentChangedEventArgs> ParentChanged
		{
			add
			{
				EventHandler<VirtualizingParentChangedEventArgs> eventHandler = TreeViewItemContainerData.ParentChanged;
				EventHandler<VirtualizingParentChangedEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<VirtualizingParentChangedEventArgs>>(ref TreeViewItemContainerData.ParentChanged, (EventHandler<VirtualizingParentChangedEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<VirtualizingParentChangedEventArgs> eventHandler = TreeViewItemContainerData.ParentChanged;
				EventHandler<VirtualizingParentChangedEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<VirtualizingParentChangedEventArgs>>(ref TreeViewItemContainerData.ParentChanged, (EventHandler<VirtualizingParentChangedEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x0005952C File Offset: 0x0005792C
		// (set) Token: 0x06000F8F RID: 3983 RVA: 0x00059534 File Offset: 0x00057934
		public TreeViewItemContainerData Parent
		{
			get
			{
				return this.m_parent;
			}
			set
			{
				if (this.m_parent == value)
				{
					return;
				}
				TreeViewItemContainerData parent = this.m_parent;
				this.m_parent = value;
				if (TreeViewItemContainerData.ParentChanged != null)
				{
					TreeViewItemContainerData.ParentChanged(this, new VirtualizingParentChangedEventArgs(parent, this.m_parent));
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0005957D File Offset: 0x0005797D
		// (set) Token: 0x06000F91 RID: 3985 RVA: 0x00059585 File Offset: 0x00057985
		public int Indent
		{
			[CompilerGenerated]
			get
			{
				return this.<Indent>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Indent>k__BackingField = value;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0005958E File Offset: 0x0005798E
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x00059596 File Offset: 0x00057996
		public bool CanExpand
		{
			[CompilerGenerated]
			get
			{
				return this.<CanExpand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CanExpand>k__BackingField = value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0005959F File Offset: 0x0005799F
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x000595A7 File Offset: 0x000579A7
		public bool IsExpanded
		{
			[CompilerGenerated]
			get
			{
				return this.<IsExpanded>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsExpanded>k__BackingField = value;
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x000595B0 File Offset: 0x000579B0
		public bool IsDescendantOf(TreeViewItemContainerData ancestor)
		{
			if (ancestor == null)
			{
				return true;
			}
			if (ancestor == this)
			{
				return false;
			}
			for (TreeViewItemContainerData treeViewItemContainerData = this; treeViewItemContainerData != null; treeViewItemContainerData = treeViewItemContainerData.Parent)
			{
				if (ancestor == treeViewItemContainerData)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x000595EC File Offset: 0x000579EC
		public bool HasChildren(VirtualizingTreeView treeView)
		{
			if (treeView == null)
			{
				return false;
			}
			int num = treeView.IndexOf(base.Item);
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)treeView.GetItemContainerData(num + 1);
			return treeViewItemContainerData != null && treeViewItemContainerData.Parent == this;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00059638 File Offset: 0x00057A38
		public TreeViewItemContainerData FirstChild(VirtualizingTreeView treeView)
		{
			if (!this.HasChildren(treeView))
			{
				return null;
			}
			int num = treeView.IndexOf(base.Item);
			num++;
			return (TreeViewItemContainerData)treeView.GetItemContainerData(num);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00059674 File Offset: 0x00057A74
		public TreeViewItemContainerData NextChild(VirtualizingTreeView treeView, TreeViewItemContainerData currentChild)
		{
			if (currentChild == null)
			{
				throw new ArgumentNullException("currentChild");
			}
			int num = treeView.IndexOf(currentChild.Item);
			num++;
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)treeView.GetItemContainerData(num);
			while (treeViewItemContainerData != null && treeViewItemContainerData.IsDescendantOf(this))
			{
				if (treeViewItemContainerData.Parent == this)
				{
					return treeViewItemContainerData;
				}
				num++;
				treeViewItemContainerData = (TreeViewItemContainerData)treeView.GetItemContainerData(num);
			}
			return null;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x000596E8 File Offset: 0x00057AE8
		public TreeViewItemContainerData LastChild(VirtualizingTreeView treeView)
		{
			if (!this.HasChildren(treeView))
			{
				return null;
			}
			int num = treeView.IndexOf(base.Item);
			TreeViewItemContainerData result = null;
			for (;;)
			{
				num++;
				TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)treeView.GetItemContainerData(num);
				if (treeViewItemContainerData == null || !treeViewItemContainerData.IsDescendantOf(this))
				{
					break;
				}
				if (treeViewItemContainerData.Parent == this)
				{
					result = treeViewItemContainerData;
				}
			}
			return result;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0005974C File Offset: 0x00057B4C
		public TreeViewItemContainerData LastDescendant(VirtualizingTreeView treeView)
		{
			if (!this.HasChildren(treeView))
			{
				return null;
			}
			int num = treeView.IndexOf(base.Item);
			TreeViewItemContainerData result = null;
			for (;;)
			{
				num++;
				TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)treeView.GetItemContainerData(num);
				if (treeViewItemContainerData == null || !treeViewItemContainerData.IsDescendantOf(this))
				{
					break;
				}
				result = treeViewItemContainerData;
			}
			return result;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x000597A1 File Offset: 0x00057BA1
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x04000E3B RID: 3643
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler<VirtualizingParentChangedEventArgs> ParentChanged;

		// Token: 0x04000E3C RID: 3644
		private TreeViewItemContainerData m_parent;

		// Token: 0x04000E3D RID: 3645
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Indent>k__BackingField;

		// Token: 0x04000E3E RID: 3646
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <CanExpand>k__BackingField;

		// Token: 0x04000E3F RID: 3647
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsExpanded>k__BackingField;
	}
}
