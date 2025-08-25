using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000266 RID: 614
	public class ListBoxDemo : MonoBehaviour
	{
		// Token: 0x06000D0E RID: 3342 RVA: 0x0004E1C8 File Offset: 0x0004C5C8
		public ListBoxDemo()
		{
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0004E1D0 File Offset: 0x0004C5D0
		public static bool IsPrefab(Transform This)
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				throw new InvalidOperationException("Does not work in edit mode");
			}
			return This.gameObject.scene.buildIndex < 0;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0004E214 File Offset: 0x0004C614
		private void Start()
		{
			if (!this.ListBox)
			{
				Debug.LogError("Set ListBox field");
				return;
			}
			this.ListBox.ItemDataBinding += this.OnItemDataBinding;
			this.ListBox.SelectionChanged += this.OnSelectionChanged;
			this.ListBox.ItemsRemoved += this.OnItemsRemoved;
			this.ListBox.ItemBeginDrag += this.OnItemBeginDrag;
			this.ListBox.ItemDrop += this.OnItemDrop;
			this.ListBox.ItemEndDrag += this.OnItemEndDrag;
			IEnumerable<GameObject> source = Resources.FindObjectsOfTypeAll<GameObject>();
			if (ListBoxDemo.<>f__am$cache0 == null)
			{
				ListBoxDemo.<>f__am$cache0 = new Func<GameObject, bool>(ListBoxDemo.<Start>m__0);
			}
			IEnumerable<GameObject> enumerable = source.Where(ListBoxDemo.<>f__am$cache0);
			ItemsControl listBox = this.ListBox;
			IEnumerable<GameObject> source2 = enumerable;
			if (ListBoxDemo.<>f__am$cache1 == null)
			{
				ListBoxDemo.<>f__am$cache1 = new Func<GameObject, int>(ListBoxDemo.<Start>m__1);
			}
			listBox.Items = source2.OrderBy(ListBoxDemo.<>f__am$cache1);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0004E31C File Offset: 0x0004C71C
		private void OnDestroy()
		{
			if (!this.ListBox)
			{
				return;
			}
			this.ListBox.ItemDataBinding -= this.OnItemDataBinding;
			this.ListBox.SelectionChanged -= this.OnSelectionChanged;
			this.ListBox.ItemsRemoved -= this.OnItemsRemoved;
			this.ListBox.ItemBeginDrag -= this.OnItemBeginDrag;
			this.ListBox.ItemDrop -= this.OnItemDrop;
			this.ListBox.ItemEndDrag -= this.OnItemEndDrag;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0004E3C4 File Offset: 0x0004C7C4
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

		// Token: 0x06000D13 RID: 3347 RVA: 0x0004E42E File Offset: 0x0004C82E
		private void OnSelectionChanged(object sender, SelectionChangedArgs e)
		{
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0004E430 File Offset: 0x0004C830
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

		// Token: 0x06000D15 RID: 3349 RVA: 0x0004E478 File Offset: 0x0004C878
		private void OnItemDataBinding(object sender, ItemDataBindingArgs e)
		{
			GameObject gameObject = e.Item as GameObject;
			if (gameObject != null)
			{
				Text componentInChildren = e.ItemPresenter.GetComponentInChildren<Text>(true);
				componentInChildren.text = gameObject.name;
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0004E4B6 File Offset: 0x0004C8B6
		private void OnItemBeginDrag(object sender, ItemArgs e)
		{
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0004E4B8 File Offset: 0x0004C8B8
		private void OnItemDrop(object sender, ItemDropArgs e)
		{
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
				for (int j = 0; j < e.DragItems.Length; j++)
				{
					Transform transform3 = ((GameObject)e.DragItems[j]).transform;
					if (transform3.parent != transform.parent)
					{
						transform3.SetParent(transform.parent, true);
					}
					int siblingIndex = transform.GetSiblingIndex();
					transform3.SetSiblingIndex(siblingIndex + 1);
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
					int siblingIndex2 = transform.GetSiblingIndex();
					transform4.SetSiblingIndex(siblingIndex2);
				}
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0004E60B File Offset: 0x0004CA0B
		private void OnItemEndDrag(object sender, ItemArgs e)
		{
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0004E60D File Offset: 0x0004CA0D
		[CompilerGenerated]
		private static bool <Start>m__0(GameObject go)
		{
			return !ListBoxDemo.IsPrefab(go.transform) && go.transform.parent == null;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0004E633 File Offset: 0x0004CA33
		[CompilerGenerated]
		private static int <Start>m__1(GameObject t)
		{
			return t.transform.GetSiblingIndex();
		}

		// Token: 0x04000D1E RID: 3358
		public ListBox ListBox;

		// Token: 0x04000D1F RID: 3359
		[CompilerGenerated]
		private static Func<GameObject, bool> <>f__am$cache0;

		// Token: 0x04000D20 RID: 3360
		[CompilerGenerated]
		private static Func<GameObject, int> <>f__am$cache1;
	}
}
