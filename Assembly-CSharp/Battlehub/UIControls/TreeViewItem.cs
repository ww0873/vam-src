using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x0200028A RID: 650
	public class TreeViewItem : ItemContainer
	{
		// Token: 0x06000E87 RID: 3719 RVA: 0x00054833 File Offset: 0x00052C33
		public TreeViewItem()
		{
		}

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06000E88 RID: 3720 RVA: 0x0005483C File Offset: 0x00052C3C
		// (remove) Token: 0x06000E89 RID: 3721 RVA: 0x00054870 File Offset: 0x00052C70
		public static event EventHandler<ParentChangedEventArgs> ParentChanged
		{
			add
			{
				EventHandler<ParentChangedEventArgs> eventHandler = TreeViewItem.ParentChanged;
				EventHandler<ParentChangedEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ParentChangedEventArgs>>(ref TreeViewItem.ParentChanged, (EventHandler<ParentChangedEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ParentChangedEventArgs> eventHandler = TreeViewItem.ParentChanged;
				EventHandler<ParentChangedEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ParentChangedEventArgs>>(ref TreeViewItem.ParentChanged, (EventHandler<ParentChangedEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x000548A4 File Offset: 0x00052CA4
		private TreeView TreeView
		{
			get
			{
				if (this.m_treeView == null)
				{
					this.m_treeView = base.GetComponentInParent<TreeView>();
				}
				return this.m_treeView;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x000548C9 File Offset: 0x00052CC9
		public int Indent
		{
			get
			{
				return this.m_indent;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x000548D1 File Offset: 0x00052CD1
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x000548DC File Offset: 0x00052CDC
		public TreeViewItem Parent
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
				TreeViewItem parent = this.m_parent;
				this.m_parent = value;
				this.UpdateIndent();
				if (this.TreeView != null && TreeViewItem.ParentChanged != null)
				{
					TreeViewItem.ParentChanged(this, new ParentChangedEventArgs(parent, this.m_parent));
				}
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00054944 File Offset: 0x00052D44
		public void UpdateIndent()
		{
			if (this.m_parent != null && this.TreeView != null && this.m_itemLayout != null)
			{
				this.m_indent = this.m_parent.m_indent + this.TreeView.Indent;
				this.m_itemLayout.padding = new RectOffset(this.m_indent, this.m_itemLayout.padding.right, this.m_itemLayout.padding.top, this.m_itemLayout.padding.bottom);
				int siblingIndex = base.transform.GetSiblingIndex();
				this.SetIndent(this, ref siblingIndex);
			}
			else
			{
				this.ZeroIndent();
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00054A08 File Offset: 0x00052E08
		private void ZeroIndent()
		{
			this.m_indent = 0;
			if (this.m_itemLayout != null)
			{
				this.m_itemLayout.padding = new RectOffset(this.m_indent, this.m_itemLayout.padding.right, this.m_itemLayout.padding.top, this.m_itemLayout.padding.bottom);
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00054A74 File Offset: 0x00052E74
		private void SetIndent(TreeViewItem parent, ref int siblingIndex)
		{
			for (;;)
			{
				TreeViewItem treeViewItem = (TreeViewItem)this.TreeView.GetItemContainer(siblingIndex + 1);
				if (treeViewItem == null)
				{
					break;
				}
				if (treeViewItem.Parent != parent)
				{
					return;
				}
				treeViewItem.m_indent = parent.m_indent + this.TreeView.Indent;
				treeViewItem.m_itemLayout.padding.left = treeViewItem.m_indent;
				siblingIndex++;
				this.SetIndent(treeViewItem, ref siblingIndex);
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x00054AF5 File Offset: 0x00052EF5
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x00054AFD File Offset: 0x00052EFD
		public override bool IsSelected
		{
			get
			{
				return base.IsSelected;
			}
			set
			{
				if (base.IsSelected != value)
				{
					this.m_toggle.isOn = value;
					base.IsSelected = value;
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x00054B1E File Offset: 0x00052F1E
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x00054B28 File Offset: 0x00052F28
		public bool CanExpand
		{
			get
			{
				return this.m_canExpand;
			}
			set
			{
				if (this.m_canExpand != value)
				{
					this.m_canExpand = value;
					if (this.m_expander != null)
					{
						this.m_expander.CanExpand = this.m_canExpand;
					}
					if (!this.m_canExpand)
					{
						this.IsExpanded = false;
					}
				}
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x00054B7C File Offset: 0x00052F7C
		// (set) Token: 0x06000E96 RID: 3734 RVA: 0x00054B84 File Offset: 0x00052F84
		public bool IsExpanded
		{
			get
			{
				return this.m_isExpanded;
			}
			set
			{
				if (this.m_isExpanded != value)
				{
					this.m_isExpanded = (value && this.m_canExpand);
					if (this.m_expander != null)
					{
						this.m_expander.IsOn = (value && this.m_canExpand);
					}
					if (this.TreeView != null)
					{
						if (this.m_isExpanded)
						{
							this.TreeView.Expand(this);
						}
						else
						{
							this.TreeView.Collapse(this);
						}
					}
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00054C18 File Offset: 0x00053018
		public bool HasChildren
		{
			get
			{
				int siblingIndex = base.transform.GetSiblingIndex();
				if (this.TreeView == null)
				{
					return false;
				}
				TreeViewItem treeViewItem = (TreeViewItem)this.TreeView.GetItemContainer(siblingIndex + 1);
				return treeViewItem != null && treeViewItem.Parent == this;
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00054C74 File Offset: 0x00053074
		public bool IsDescendantOf(TreeViewItem ancestor)
		{
			if (ancestor == null)
			{
				return true;
			}
			if (ancestor == this)
			{
				return false;
			}
			TreeViewItem treeViewItem = this;
			while (treeViewItem != null)
			{
				if (ancestor == treeViewItem)
				{
					return true;
				}
				treeViewItem = treeViewItem.Parent;
			}
			return false;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00054CC8 File Offset: 0x000530C8
		public TreeViewItem FirstChild()
		{
			if (!this.HasChildren)
			{
				return null;
			}
			int num = base.transform.GetSiblingIndex();
			num++;
			return (TreeViewItem)this.TreeView.GetItemContainer(num);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00054D08 File Offset: 0x00053108
		public TreeViewItem NextChild(TreeViewItem currentChild)
		{
			if (currentChild == null)
			{
				throw new ArgumentNullException("currentChild");
			}
			int num = currentChild.transform.GetSiblingIndex();
			num++;
			TreeViewItem treeViewItem = (TreeViewItem)this.TreeView.GetItemContainer(num);
			while (treeViewItem != null && treeViewItem.IsDescendantOf(this))
			{
				if (treeViewItem.Parent == this)
				{
					return treeViewItem;
				}
				num++;
				treeViewItem = (TreeViewItem)this.TreeView.GetItemContainer(num);
			}
			return null;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00054D98 File Offset: 0x00053198
		public TreeViewItem LastChild()
		{
			if (!this.HasChildren)
			{
				return null;
			}
			int num = base.transform.GetSiblingIndex();
			TreeViewItem result = null;
			for (;;)
			{
				num++;
				TreeViewItem treeViewItem = (TreeViewItem)this.TreeView.GetItemContainer(num);
				if (treeViewItem == null || !treeViewItem.IsDescendantOf(this))
				{
					break;
				}
				if (treeViewItem.Parent == this)
				{
					result = treeViewItem;
				}
			}
			return result;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00054E08 File Offset: 0x00053208
		public TreeViewItem LastDescendant()
		{
			if (!this.HasChildren)
			{
				return null;
			}
			int num = base.transform.GetSiblingIndex();
			TreeViewItem result = null;
			for (;;)
			{
				num++;
				TreeViewItem treeViewItem = (TreeViewItem)this.TreeView.GetItemContainer(num);
				if (treeViewItem == null || !treeViewItem.IsDescendantOf(this))
				{
					break;
				}
				result = treeViewItem;
			}
			return result;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00054E68 File Offset: 0x00053268
		protected override void AwakeOverride()
		{
			this.m_toggle = base.GetComponent<Toggle>();
			this.m_toggle.interactable = false;
			this.m_toggle.isOn = this.IsSelected;
			this.m_expander = base.GetComponentInChildren<TreeViewExpander>();
			if (this.m_expander != null)
			{
				this.m_expander.CanExpand = this.m_canExpand;
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00054ECC File Offset: 0x000532CC
		protected override void StartOverride()
		{
			if (this.TreeView != null)
			{
				this.m_toggle.isOn = this.TreeView.IsItemSelected(base.Item);
				this.m_isSelected = this.m_toggle.isOn;
			}
			if (this.Parent != null)
			{
				this.m_indent = this.Parent.m_indent + this.TreeView.Indent;
				this.m_itemLayout.padding = new RectOffset(this.m_indent, this.m_itemLayout.padding.right, this.m_itemLayout.padding.top, this.m_itemLayout.padding.bottom);
			}
			if (this.CanExpand && this.TreeView.AutoExpand)
			{
				this.IsExpanded = true;
			}
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00054FB0 File Offset: 0x000533B0
		public override void Clear()
		{
			base.Clear();
			this.m_parent = null;
			this.ZeroIndent();
			this.m_isSelected = false;
			this.m_toggle.isOn = this.m_isSelected;
			this.m_isExpanded = false;
			this.m_canExpand = false;
			this.m_expander.IsOn = false;
			this.m_expander.CanExpand = false;
		}

		// Token: 0x04000DCD RID: 3533
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler<ParentChangedEventArgs> ParentChanged;

		// Token: 0x04000DCE RID: 3534
		private TreeViewExpander m_expander;

		// Token: 0x04000DCF RID: 3535
		[SerializeField]
		private HorizontalLayoutGroup m_itemLayout;

		// Token: 0x04000DD0 RID: 3536
		private Toggle m_toggle;

		// Token: 0x04000DD1 RID: 3537
		private TreeView m_treeView;

		// Token: 0x04000DD2 RID: 3538
		private int m_indent;

		// Token: 0x04000DD3 RID: 3539
		private TreeViewItem m_parent;

		// Token: 0x04000DD4 RID: 3540
		private bool m_canExpand;

		// Token: 0x04000DD5 RID: 3541
		private bool m_isExpanded;
	}
}
