using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x0200026A RID: 618
	public class VirtualizingTreeViewDemo : MonoBehaviour
	{
		// Token: 0x06000D32 RID: 3378 RVA: 0x0004EE6E File Offset: 0x0004D26E
		public VirtualizingTreeViewDemo()
		{
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0004EE80 File Offset: 0x0004D280
		public static bool IsPrefab(Transform This)
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				throw new InvalidOperationException("Does not work in edit mode");
			}
			return This.gameObject.scene.buildIndex < 0;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0004EEC4 File Offset: 0x0004D2C4
		private void Start()
		{
			if (!this.TreeView)
			{
				Debug.LogError("Set TreeView field");
				return;
			}
			this.TreeView.ItemDataBinding += this.OnItemDataBinding;
			this.TreeView.SelectionChanged += this.OnSelectionChanged;
			this.TreeView.ItemsRemoved += this.OnItemsRemoved;
			this.TreeView.ItemExpanding += this.OnItemExpanding;
			this.TreeView.ItemBeginDrag += this.OnItemBeginDrag;
			this.TreeView.ItemDrop += this.OnItemDrop;
			this.TreeView.ItemBeginDrop += this.OnItemBeginDrop;
			this.TreeView.ItemEndDrag += this.OnItemEndDrag;
			VirtualizingItemsControl treeView = this.TreeView;
			IEnumerable<GameObject> source = Resources.FindObjectsOfTypeAll<GameObject>();
			if (VirtualizingTreeViewDemo.<>f__am$cache0 == null)
			{
				VirtualizingTreeViewDemo.<>f__am$cache0 = new Func<GameObject, bool>(VirtualizingTreeViewDemo.<Start>m__0);
			}
			IEnumerable<GameObject> source2 = source.Where(VirtualizingTreeViewDemo.<>f__am$cache0);
			if (VirtualizingTreeViewDemo.<>f__am$cache1 == null)
			{
				VirtualizingTreeViewDemo.<>f__am$cache1 = new Func<GameObject, int>(VirtualizingTreeViewDemo.<Start>m__1);
			}
			treeView.Items = source2.OrderBy(VirtualizingTreeViewDemo.<>f__am$cache1).ToArray<GameObject>();
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0004EFFD File Offset: 0x0004D3FD
		private void OnItemBeginDrop(object sender, ItemDropCancelArgs e)
		{
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0004F000 File Offset: 0x0004D400
		private void OnDestroy()
		{
			if (!this.TreeView)
			{
				return;
			}
			this.TreeView.ItemDataBinding -= this.OnItemDataBinding;
			this.TreeView.SelectionChanged -= this.OnSelectionChanged;
			this.TreeView.ItemsRemoved -= this.OnItemsRemoved;
			this.TreeView.ItemExpanding -= this.OnItemExpanding;
			this.TreeView.ItemBeginDrag -= this.OnItemBeginDrag;
			this.TreeView.ItemBeginDrop -= this.OnItemBeginDrop;
			this.TreeView.ItemDrop -= this.OnItemDrop;
			this.TreeView.ItemEndDrag -= this.OnItemEndDrag;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0004F0D8 File Offset: 0x0004D4D8
		private void OnItemExpanding(object sender, ItemExpandingArgs e)
		{
			GameObject gameObject = (GameObject)e.Item;
			if (gameObject.transform.childCount > 0)
			{
				List<GameObject> list = new List<GameObject>();
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
					list.Add(gameObject2);
				}
				e.Children = list;
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0004F144 File Offset: 0x0004D544
		private void OnSelectionChanged(object sender, SelectionChangedArgs e)
		{
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0004F148 File Offset: 0x0004D548
		private void OnItemsRemoved(object sender, ItemsRemovedArgs e)
		{
			for (int i = 0; i < e.Items.Length; i++)
			{
				GameObject gameObject = (GameObject)e.Items[i];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0004F190 File Offset: 0x0004D590
		private void OnItemDataBinding(object sender, TreeViewItemDataBindingArgs e)
		{
			GameObject gameObject = e.Item as GameObject;
			if (gameObject != null)
			{
				Text componentInChildren = e.ItemPresenter.GetComponentInChildren<Text>(true);
				componentInChildren.text = gameObject.name;
				Image image = e.ItemPresenter.GetComponentsInChildren<Image>()[4];
				image.sprite = Resources.Load<Sprite>("cube");
				e.HasChildren = (gameObject.transform.childCount > 0);
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0004F200 File Offset: 0x0004D600
		private void OnItemBeginDrag(object sender, ItemArgs e)
		{
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0004F204 File Offset: 0x0004D604
		private void OnItemDrop(object sender, ItemDropArgs e)
		{
			if (e.DropTarget == null)
			{
				return;
			}
			Transform transform = ((GameObject)e.DropTarget).transform;
			if (e.Action == ItemDropAction.SetLastChild)
			{
				for (int i = 0; i < e.DragItems.Length; i++)
				{
					Transform transform2 = ((GameObject)e.DragItems[i]).transform;
					transform2.SetParent(transform, true);
					transform2.SetAsLastSibling();
				}
			}
			else if (e.Action == ItemDropAction.SetNextSibling)
			{
				for (int j = e.DragItems.Length - 1; j >= 0; j--)
				{
					Transform transform3 = ((GameObject)e.DragItems[j]).transform;
					int siblingIndex = transform.GetSiblingIndex();
					if (transform3.parent != transform.parent)
					{
						transform3.SetParent(transform.parent, true);
						transform3.SetSiblingIndex(siblingIndex + 1);
					}
					else
					{
						int siblingIndex2 = transform3.GetSiblingIndex();
						if (siblingIndex < siblingIndex2)
						{
							transform3.SetSiblingIndex(siblingIndex + 1);
						}
						else
						{
							transform3.SetSiblingIndex(siblingIndex);
						}
					}
				}
			}
			else if (e.Action == ItemDropAction.SetPrevSibling)
			{
				for (int k = 0; k < e.DragItems.Length; k++)
				{
					Transform transform4 = ((GameObject)e.DragItems[k]).transform;
					if (transform4.parent != transform.parent)
					{
						transform4.SetParent(transform.parent, true);
					}
					int siblingIndex3 = transform.GetSiblingIndex();
					int siblingIndex4 = transform4.GetSiblingIndex();
					if (siblingIndex3 > siblingIndex4)
					{
						transform4.SetSiblingIndex(siblingIndex3 - 1);
					}
					else
					{
						transform4.SetSiblingIndex(siblingIndex3);
					}
				}
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0004F3B7 File Offset: 0x0004D7B7
		private void OnItemEndDrag(object sender, ItemArgs e)
		{
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0004F3BC File Offset: 0x0004D7BC
		public void AddItem()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "GameObject " + this.m_counter;
			this.m_counter++;
			this.TreeView.Add(gameObject);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0004F405 File Offset: 0x0004D805
		[CompilerGenerated]
		private static bool <Start>m__0(GameObject go)
		{
			return !VirtualizingTreeViewDemo.IsPrefab(go.transform) && go.transform.parent == null && go.name != "VirtualizingTreeViewDemo";
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0004F440 File Offset: 0x0004D840
		[CompilerGenerated]
		private static int <Start>m__1(GameObject t)
		{
			return t.transform.GetSiblingIndex();
		}

		// Token: 0x04000D27 RID: 3367
		public VirtualizingTreeView TreeView;

		// Token: 0x04000D28 RID: 3368
		private GameObject[] m_dataItems;

		// Token: 0x04000D29 RID: 3369
		private int m_counter = 1;

		// Token: 0x04000D2A RID: 3370
		[CompilerGenerated]
		private static Func<GameObject, bool> <>f__am$cache0;

		// Token: 0x04000D2B RID: 3371
		[CompilerGenerated]
		private static Func<GameObject, int> <>f__am$cache1;
	}
}
