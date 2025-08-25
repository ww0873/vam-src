using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D0 RID: 1232
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Extensions/Re-orderable list")]
	public class ReorderableList : MonoBehaviour
	{
		// Token: 0x06001F22 RID: 7970 RVA: 0x000B0BD8 File Offset: 0x000AEFD8
		public ReorderableList()
		{
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x000B0C25 File Offset: 0x000AF025
		public RectTransform Content
		{
			get
			{
				if (this._content == null)
				{
					this._content = this.ContentLayout.GetComponent<RectTransform>();
				}
				return this._content;
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000B0C50 File Offset: 0x000AF050
		private Canvas GetCanvas()
		{
			Transform transform = base.transform;
			Canvas canvas = null;
			int num = 100;
			int num2 = 0;
			while (canvas == null && num2 < num)
			{
				canvas = transform.gameObject.GetComponent<Canvas>();
				if (canvas == null)
				{
					transform = transform.parent;
				}
				num2++;
			}
			return canvas;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000B0CA8 File Offset: 0x000AF0A8
		private void Awake()
		{
			if (this.ContentLayout == null)
			{
				Debug.LogError("You need to have a child LayoutGroup content set for the list: " + base.name, base.gameObject);
				return;
			}
			if (this.DraggableArea == null)
			{
				this.DraggableArea = base.transform.root.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
			}
			if (this.IsDropable && !base.GetComponent<Graphic>())
			{
				Debug.LogError("You need to have a Graphic control (such as an Image) for the list [" + base.name + "] to be droppable", base.gameObject);
				return;
			}
			this._listContent = this.ContentLayout.gameObject.AddComponent<ReorderableListContent>();
			this._listContent.Init(this);
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000B0D6C File Offset: 0x000AF16C
		public void TestReOrderableListTarget(ReorderableList.ReorderableListEventStruct item)
		{
			Debug.Log("Event Received");
			Debug.Log("Hello World, is my item a clone? [" + item.IsAClone + "]");
		}

		// Token: 0x04001A40 RID: 6720
		[Tooltip("Child container with re-orderable items in a layout group")]
		public LayoutGroup ContentLayout;

		// Token: 0x04001A41 RID: 6721
		[Tooltip("Parent area to draw the dragged element on top of containers. Defaults to the root Canvas")]
		public RectTransform DraggableArea;

		// Token: 0x04001A42 RID: 6722
		[Tooltip("Can items be dragged from the container?")]
		public bool IsDraggable = true;

		// Token: 0x04001A43 RID: 6723
		[Tooltip("Should the draggable components be removed or cloned?")]
		public bool CloneDraggedObject;

		// Token: 0x04001A44 RID: 6724
		[Tooltip("Can new draggable items be dropped in to the container?")]
		public bool IsDropable = true;

		// Token: 0x04001A45 RID: 6725
		[Header("UI Re-orderable Events")]
		public ReorderableList.ReorderableListHandler OnElementDropped = new ReorderableList.ReorderableListHandler();

		// Token: 0x04001A46 RID: 6726
		public ReorderableList.ReorderableListHandler OnElementGrabbed = new ReorderableList.ReorderableListHandler();

		// Token: 0x04001A47 RID: 6727
		public ReorderableList.ReorderableListHandler OnElementRemoved = new ReorderableList.ReorderableListHandler();

		// Token: 0x04001A48 RID: 6728
		public ReorderableList.ReorderableListHandler OnElementAdded = new ReorderableList.ReorderableListHandler();

		// Token: 0x04001A49 RID: 6729
		private RectTransform _content;

		// Token: 0x04001A4A RID: 6730
		private ReorderableListContent _listContent;

		// Token: 0x020004D1 RID: 1233
		[Serializable]
		public struct ReorderableListEventStruct
		{
			// Token: 0x06001F27 RID: 7975 RVA: 0x000B0D98 File Offset: 0x000AF198
			public void Cancel()
			{
				this.SourceObject.GetComponent<ReorderableListElement>().isValid = false;
			}

			// Token: 0x04001A4B RID: 6731
			public GameObject DroppedObject;

			// Token: 0x04001A4C RID: 6732
			public int FromIndex;

			// Token: 0x04001A4D RID: 6733
			public ReorderableList FromList;

			// Token: 0x04001A4E RID: 6734
			public bool IsAClone;

			// Token: 0x04001A4F RID: 6735
			public GameObject SourceObject;

			// Token: 0x04001A50 RID: 6736
			public int ToIndex;

			// Token: 0x04001A51 RID: 6737
			public ReorderableList ToList;
		}

		// Token: 0x020004D2 RID: 1234
		[Serializable]
		public class ReorderableListHandler : UnityEvent<ReorderableList.ReorderableListEventStruct>
		{
			// Token: 0x06001F28 RID: 7976 RVA: 0x000B0DAB File Offset: 0x000AF1AB
			public ReorderableListHandler()
			{
			}
		}
	}
}
