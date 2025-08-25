using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000268 RID: 616
	public class TreeViewDemo : MonoBehaviour
	{
		// Token: 0x06000D1E RID: 3358 RVA: 0x0004E6BD File Offset: 0x0004CABD
		public TreeViewDemo()
		{
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0004E6C8 File Offset: 0x0004CAC8
		public static bool IsPrefab(Transform This)
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				throw new InvalidOperationException("Does not work in edit mode");
			}
			return This.gameObject.scene.buildIndex < 0;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0004E70C File Offset: 0x0004CB0C
		private void Start()
		{
			if (!this.TreeView)
			{
				Debug.LogError("Set TreeView field");
				return;
			}
			IEnumerable<GameObject> source = Resources.FindObjectsOfTypeAll<GameObject>();
			if (TreeViewDemo.<>f__am$cache0 == null)
			{
				TreeViewDemo.<>f__am$cache0 = new Func<GameObject, bool>(TreeViewDemo.<Start>m__0);
			}
			IEnumerable<GameObject> source2 = source.Where(TreeViewDemo.<>f__am$cache0);
			if (TreeViewDemo.<>f__am$cache1 == null)
			{
				TreeViewDemo.<>f__am$cache1 = new Func<GameObject, int>(TreeViewDemo.<Start>m__1);
			}
			IEnumerable<GameObject> items = source2.OrderBy(TreeViewDemo.<>f__am$cache1);
			this.TreeView.ItemDataBinding += this.OnItemDataBinding;
			this.TreeView.SelectionChanged += this.OnSelectionChanged;
			this.TreeView.ItemsRemoved += this.OnItemsRemoved;
			this.TreeView.ItemExpanding += this.OnItemExpanding;
			this.TreeView.ItemBeginDrag += this.OnItemBeginDrag;
			this.TreeView.ItemDrop += this.OnItemDrop;
			this.TreeView.ItemBeginDrop += this.OnItemBeginDrop;
			this.TreeView.ItemEndDrag += this.OnItemEndDrag;
			this.TreeView.Items = items;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0004E842 File Offset: 0x0004CC42
		private void OnItemBeginDrop(object sender, ItemDropCancelArgs e)
		{
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0004E844 File Offset: 0x0004CC44
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

		// Token: 0x06000D23 RID: 3363 RVA: 0x0004E91C File Offset: 0x0004CD1C
		private void OnItemExpanding(object sender, ItemExpandingArgs e)
		{
			GameObject gameObject = (GameObject)e.Item;
			if (gameObject.transform.childCount > 0)
			{
				GameObject[] array = new GameObject[gameObject.transform.childCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = gameObject.transform.GetChild(i).gameObject;
				}
				e.Children = array;
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0004E986 File Offset: 0x0004CD86
		private void OnSelectionChanged(object sender, SelectionChangedArgs e)
		{
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0004E988 File Offset: 0x0004CD88
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

		// Token: 0x06000D26 RID: 3366 RVA: 0x0004E9D0 File Offset: 0x0004CDD0
		private void OnItemDataBinding(object sender, TreeViewItemDataBindingArgs e)
		{
			GameObject gameObject = e.Item as GameObject;
			if (gameObject != null)
			{
				Text componentInChildren = e.ItemPresenter.GetComponentInChildren<Text>(true);
				componentInChildren.text = gameObject.name;
				Image image = e.ItemPresenter.GetComponentsInChildren<Image>()[4];
				image.sprite = Resources.Load<Sprite>("cube");
				if (gameObject.name != "TreeView")
				{
					e.HasChildren = (gameObject.transform.childCount > 0);
				}
			}
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0004EA55 File Offset: 0x0004CE55
		private void OnItemBeginDrag(object sender, ItemArgs e)
		{
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0004EA58 File Offset: 0x0004CE58
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

		// Token: 0x06000D29 RID: 3369 RVA: 0x0004EC0B File Offset: 0x0004D00B
		private void OnItemEndDrag(object sender, ItemArgs e)
		{
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0004EC10 File Offset: 0x0004D010
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.J))
			{
				this.TreeView.SelectedItems = this.TreeView.Items.OfType<object>().Take(5).ToArray<object>();
			}
			else if (Input.GetKeyDown(KeyCode.K))
			{
				this.TreeView.SelectedItem = null;
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0004EC6C File Offset: 0x0004D06C
		[CompilerGenerated]
		private static bool <Start>m__0(GameObject go)
		{
			return !TreeViewDemo.IsPrefab(go.transform) && go.transform.parent == null;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0004EC92 File Offset: 0x0004D092
		[CompilerGenerated]
		private static int <Start>m__1(GameObject t)
		{
			return t.transform.GetSiblingIndex();
		}

		// Token: 0x04000D23 RID: 3363
		public TreeView TreeView;

		// Token: 0x04000D24 RID: 3364
		[CompilerGenerated]
		private static Func<GameObject, bool> <>f__am$cache0;

		// Token: 0x04000D25 RID: 3365
		[CompilerGenerated]
		private static Func<GameObject, int> <>f__am$cache1;
	}
}
