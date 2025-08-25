using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x0200029B RID: 667
	public class VirtualizingTreeViewItem : VirtualizingItemContainer
	{
		// Token: 0x06000FC0 RID: 4032 RVA: 0x0005A664 File Offset: 0x00058A64
		public VirtualizingTreeViewItem()
		{
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0005A66C File Offset: 0x00058A6C
		private VirtualizingTreeView TreeView
		{
			get
			{
				if (this.m_treeView == null)
				{
					this.m_treeView = base.GetComponentInParent<VirtualizingTreeView>();
				}
				return this.m_treeView;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0005A691 File Offset: 0x00058A91
		public float Indent
		{
			get
			{
				return (float)this.m_treeViewItemData.Indent;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0005A69F File Offset: 0x00058A9F
		// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x0005A6A8 File Offset: 0x00058AA8
		public override object Item
		{
			get
			{
				return base.Item;
			}
			set
			{
				if (base.Item != value)
				{
					base.Item = value;
					this.m_treeViewItemData = (TreeViewItemContainerData)this.TreeView.GetItemContainerData(value);
					if (this.m_treeViewItemData == null)
					{
						base.name = "Null";
						return;
					}
					this.UpdateIndent();
					this.m_expander.CanExpand = this.m_treeViewItemData.CanExpand;
					this.m_expander.IsOn = (this.m_treeViewItemData.IsExpanded && this.m_treeViewItemData.CanExpand);
					base.name = base.Item.ToString() + " " + this.m_treeViewItemData.ToString();
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0005A761 File Offset: 0x00058B61
		public TreeViewItemContainerData TreeViewItemData
		{
			get
			{
				return this.m_treeViewItemData;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0005A769 File Offset: 0x00058B69
		// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x0005A776 File Offset: 0x00058B76
		public TreeViewItemContainerData Parent
		{
			get
			{
				return this.m_treeViewItemData.Parent;
			}
			set
			{
				if (this.m_treeViewItemData == null)
				{
					return;
				}
				if (this.m_treeViewItemData.Parent == value)
				{
					return;
				}
				this.m_treeViewItemData.Parent = value;
				this.UpdateIndent();
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0005A7A8 File Offset: 0x00058BA8
		public void UpdateIndent()
		{
			if (this.Parent != null && this.TreeView != null && this.m_itemLayout != null)
			{
				this.m_treeViewItemData.Indent = this.Parent.Indent + this.TreeView.Indent;
				this.m_itemLayout.padding = new RectOffset(this.m_treeViewItemData.Indent, this.m_itemLayout.padding.right, this.m_itemLayout.padding.top, this.m_itemLayout.padding.bottom);
				int num = this.TreeView.IndexOf(this.Item);
				this.SetIndent(this, ref num);
			}
			else
			{
				this.ZeroIndent();
				int num2 = this.TreeView.IndexOf(this.Item);
				if (this.HasChildren)
				{
					this.SetIndent(this, ref num2);
				}
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0005A89C File Offset: 0x00058C9C
		private void ZeroIndent()
		{
			this.m_treeViewItemData.Indent = 0;
			if (this.m_itemLayout != null)
			{
				this.m_itemLayout.padding = new RectOffset(this.m_treeViewItemData.Indent, this.m_itemLayout.padding.right, this.m_itemLayout.padding.top, this.m_itemLayout.padding.bottom);
			}
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0005A914 File Offset: 0x00058D14
		private void SetIndent(VirtualizingTreeViewItem parent, ref int itemIndex)
		{
			for (;;)
			{
				object itemAt = this.TreeView.GetItemAt(itemIndex + 1);
				VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)this.TreeView.GetItemContainer(itemAt);
				if (virtualizingTreeViewItem == null)
				{
					break;
				}
				if (virtualizingTreeViewItem.Item == null)
				{
					return;
				}
				if (virtualizingTreeViewItem.Parent != parent.m_treeViewItemData)
				{
					return;
				}
				virtualizingTreeViewItem.m_treeViewItemData.Indent = parent.m_treeViewItemData.Indent + this.TreeView.Indent;
				virtualizingTreeViewItem.m_itemLayout.padding.left = virtualizingTreeViewItem.m_treeViewItemData.Indent;
				itemIndex++;
				this.SetIndent(virtualizingTreeViewItem, ref itemIndex);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x0005A9BD File Offset: 0x00058DBD
		// (set) Token: 0x06000FCC RID: 4044 RVA: 0x0005A9C5 File Offset: 0x00058DC5
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

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x0005A9E6 File Offset: 0x00058DE6
		// (set) Token: 0x06000FCE RID: 4046 RVA: 0x0005AA04 File Offset: 0x00058E04
		public bool CanExpand
		{
			get
			{
				return this.m_treeViewItemData != null && this.m_treeViewItemData.CanExpand;
			}
			set
			{
				if (this.m_treeViewItemData.CanExpand != value)
				{
					this.m_treeViewItemData.CanExpand = value;
					if (this.m_expander != null)
					{
						this.m_expander.CanExpand = this.m_treeViewItemData.CanExpand;
					}
					if (!this.m_treeViewItemData.CanExpand)
					{
						this.IsExpanded = false;
					}
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x0005AA6C File Offset: 0x00058E6C
		// (set) Token: 0x06000FD0 RID: 4048 RVA: 0x0005AA7C File Offset: 0x00058E7C
		public bool IsExpanded
		{
			get
			{
				return this.m_treeViewItemData.IsExpanded;
			}
			set
			{
				if (this.m_treeViewItemData == null)
				{
					return;
				}
				if (this.m_treeViewItemData.IsExpanded != value)
				{
					this.m_treeViewItemData.IsExpanded = (value && this.CanExpand);
					if (this.m_expander != null)
					{
						this.m_expander.IsOn = (value && this.CanExpand);
					}
					if (this.TreeView != null)
					{
						if (this.m_treeViewItemData.IsExpanded)
						{
							this.TreeView.Expand(this.m_treeViewItemData.Item);
						}
						else
						{
							this.TreeView.Collapse(this.m_treeViewItemData.Item);
						}
					}
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0005AB3D File Offset: 0x00058F3D
		public bool HasChildren
		{
			get
			{
				return this.m_treeViewItemData.HasChildren(this.TreeView);
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0005AB50 File Offset: 0x00058F50
		public TreeViewItemContainerData FirstChild()
		{
			return this.m_treeViewItemData.FirstChild(this.TreeView);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0005AB63 File Offset: 0x00058F63
		public TreeViewItemContainerData NextChild(TreeViewItemContainerData currentChild)
		{
			return this.m_treeViewItemData.NextChild(this.TreeView, currentChild);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0005AB77 File Offset: 0x00058F77
		public TreeViewItemContainerData LastChild()
		{
			return this.m_treeViewItemData.LastChild(this.TreeView);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0005AB8A File Offset: 0x00058F8A
		public TreeViewItemContainerData LastDescendant()
		{
			return this.m_treeViewItemData.LastDescendant(this.TreeView);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0005ABA0 File Offset: 0x00058FA0
		protected override void AwakeOverride()
		{
			this.m_toggle = base.GetComponent<Toggle>();
			this.m_toggle.interactable = false;
			this.m_toggle.isOn = this.IsSelected;
			this.m_expander = base.GetComponentInChildren<TreeViewExpander>();
			if (this.m_expander != null)
			{
				this.m_expander.CanExpand = this.CanExpand;
			}
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0005AC04 File Offset: 0x00059004
		protected override void StartOverride()
		{
			if (this.TreeView != null)
			{
				this.m_toggle.isOn = this.TreeView.IsItemSelected(this.Item);
				this.m_isSelected = this.m_toggle.isOn;
			}
			if (this.Parent != null)
			{
				this.m_treeViewItemData.Indent = this.Parent.Indent + this.TreeView.Indent;
				this.m_itemLayout.padding = new RectOffset(this.m_treeViewItemData.Indent, this.m_itemLayout.padding.right, this.m_itemLayout.padding.top, this.m_itemLayout.padding.bottom);
			}
			if (this.CanExpand && this.TreeView.AutoExpand)
			{
				this.IsExpanded = true;
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0005ACE9 File Offset: 0x000590E9
		public override void Clear()
		{
			base.Clear();
		}

		// Token: 0x04000E4A RID: 3658
		private TreeViewExpander m_expander;

		// Token: 0x04000E4B RID: 3659
		[SerializeField]
		private HorizontalLayoutGroup m_itemLayout;

		// Token: 0x04000E4C RID: 3660
		private Toggle m_toggle;

		// Token: 0x04000E4D RID: 3661
		private VirtualizingTreeView m_treeView;

		// Token: 0x04000E4E RID: 3662
		private TreeViewItemContainerData m_treeViewItemData;
	}
}
