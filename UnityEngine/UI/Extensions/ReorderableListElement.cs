using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D5 RID: 1237
	[RequireComponent(typeof(RectTransform))]
	public class ReorderableListElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06001F31 RID: 7985 RVA: 0x000B1199 File Offset: 0x000AF599
		public ReorderableListElement()
		{
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x000B11BC File Offset: 0x000AF5BC
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.isValid = true;
			if (this._reorderableList == null)
			{
				return;
			}
			if (!this._reorderableList.IsDraggable || !this.IsGrabbable)
			{
				this._draggingObject = null;
				return;
			}
			if (!this._reorderableList.CloneDraggedObject)
			{
				this._draggingObject = this._rect;
				this._fromIndex = this._rect.GetSiblingIndex();
				if (this._reorderableList.OnElementRemoved != null)
				{
					this._reorderableList.OnElementRemoved.Invoke(new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex
					});
				}
				if (!this.isValid)
				{
					this._draggingObject = null;
					return;
				}
			}
			else
			{
				GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
				this._draggingObject = gameObject.GetComponent<RectTransform>();
			}
			this._draggingObjectOriginalSize = base.gameObject.GetComponent<RectTransform>().rect.size;
			this._draggingObjectLE = this._draggingObject.GetComponent<LayoutElement>();
			this._draggingObject.SetParent(this._reorderableList.DraggableArea, true);
			this._draggingObject.SetAsLastSibling();
			this._fakeElement = new GameObject("Fake").AddComponent<RectTransform>();
			this._fakeElementLE = this._fakeElement.gameObject.AddComponent<LayoutElement>();
			this.RefreshSizes();
			if (this._reorderableList.OnElementGrabbed != null)
			{
				this._reorderableList.OnElementGrabbed.Invoke(new ReorderableList.ReorderableListEventStruct
				{
					DroppedObject = this._draggingObject.gameObject,
					IsAClone = this._reorderableList.CloneDraggedObject,
					SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
					FromList = this._reorderableList,
					FromIndex = this._fromIndex
				});
				if (!this.isValid)
				{
					this.CancelDrag();
					return;
				}
			}
			this._isDragging = true;
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x000B1428 File Offset: 0x000AF828
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._isDragging)
			{
				return;
			}
			if (!this.isValid)
			{
				this.CancelDrag();
				return;
			}
			Canvas componentInParent = this._draggingObject.GetComponentInParent<Canvas>();
			Vector3 position;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(componentInParent.GetComponent<RectTransform>(), eventData.position, componentInParent.worldCamera, out position);
			this._draggingObject.position = position;
			EventSystem.current.RaycastAll(eventData, this._raycastResults);
			for (int i = 0; i < this._raycastResults.Count; i++)
			{
				this._currentReorderableListRaycasted = this._raycastResults[i].gameObject.GetComponent<ReorderableList>();
				if (this._currentReorderableListRaycasted != null)
				{
					break;
				}
			}
			if (this._currentReorderableListRaycasted == null || !this._currentReorderableListRaycasted.IsDropable)
			{
				this.RefreshSizes();
				this._fakeElement.transform.SetParent(this._reorderableList.DraggableArea, false);
			}
			else
			{
				if (this._fakeElement.parent != this._currentReorderableListRaycasted)
				{
					this._fakeElement.SetParent(this._currentReorderableListRaycasted.Content, false);
				}
				float num = float.PositiveInfinity;
				int siblingIndex = 0;
				float num2 = 0f;
				for (int j = 0; j < this._currentReorderableListRaycasted.Content.childCount; j++)
				{
					RectTransform component = this._currentReorderableListRaycasted.Content.GetChild(j).GetComponent<RectTransform>();
					if (this._currentReorderableListRaycasted.ContentLayout is VerticalLayoutGroup)
					{
						num2 = Mathf.Abs(component.position.y - position.y);
					}
					else if (this._currentReorderableListRaycasted.ContentLayout is HorizontalLayoutGroup)
					{
						num2 = Mathf.Abs(component.position.x - position.x);
					}
					else if (this._currentReorderableListRaycasted.ContentLayout is GridLayoutGroup)
					{
						num2 = Mathf.Abs(component.position.x - position.x) + Mathf.Abs(component.position.y - position.y);
					}
					if (num2 < num)
					{
						num = num2;
						siblingIndex = j;
					}
				}
				this.RefreshSizes();
				this._fakeElement.SetSiblingIndex(siblingIndex);
				this._fakeElement.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000B16AC File Offset: 0x000AFAAC
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isDragging = false;
			if (this._draggingObject != null)
			{
				if (this._currentReorderableListRaycasted != null && this._currentReorderableListRaycasted.IsDropable && (this.IsTransferable || this._currentReorderableListRaycasted == this._reorderableList))
				{
					ReorderableList.ReorderableListEventStruct reorderableListEventStruct = new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex,
						ToList = this._currentReorderableListRaycasted,
						ToIndex = this._fakeElement.GetSiblingIndex()
					};
					ReorderableList.ReorderableListEventStruct arg = reorderableListEventStruct;
					if (this._reorderableList && this._reorderableList.OnElementDropped != null)
					{
						this._reorderableList.OnElementDropped.Invoke(arg);
					}
					if (!this.isValid)
					{
						this.CancelDrag();
						return;
					}
					this.RefreshSizes();
					this._draggingObject.SetParent(this._currentReorderableListRaycasted.Content, false);
					this._draggingObject.rotation = this._currentReorderableListRaycasted.transform.rotation;
					this._draggingObject.SetSiblingIndex(this._fakeElement.GetSiblingIndex());
					this._reorderableList.OnElementAdded.Invoke(arg);
					if (!this.isValid)
					{
						throw new Exception("It's too late to cancel the Transfer! Do so in OnElementDropped!");
					}
				}
				else if (this.isDroppableInSpace)
				{
					UnityEvent<ReorderableList.ReorderableListEventStruct> onElementDropped = this._reorderableList.OnElementDropped;
					ReorderableList.ReorderableListEventStruct reorderableListEventStruct = new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex
					};
					onElementDropped.Invoke(reorderableListEventStruct);
				}
				else
				{
					this.CancelDrag();
				}
			}
			if (this._fakeElement != null)
			{
				Object.Destroy(this._fakeElement.gameObject);
			}
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000B1920 File Offset: 0x000AFD20
		private void CancelDrag()
		{
			this._isDragging = false;
			if (this._reorderableList.CloneDraggedObject)
			{
				Object.Destroy(this._draggingObject.gameObject);
			}
			else
			{
				this.RefreshSizes();
				this._draggingObject.SetParent(this._reorderableList.Content, false);
				this._draggingObject.rotation = this._reorderableList.Content.transform.rotation;
				this._draggingObject.SetSiblingIndex(this._fromIndex);
				ReorderableList.ReorderableListEventStruct arg = new ReorderableList.ReorderableListEventStruct
				{
					DroppedObject = this._draggingObject.gameObject,
					IsAClone = this._reorderableList.CloneDraggedObject,
					SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
					FromList = this._reorderableList,
					FromIndex = this._fromIndex,
					ToList = this._reorderableList,
					ToIndex = this._fromIndex
				};
				this._reorderableList.OnElementAdded.Invoke(arg);
				if (!this.isValid)
				{
					throw new Exception("Transfer is already Cancelled.");
				}
			}
			if (this._fakeElement != null)
			{
				Object.Destroy(this._fakeElement.gameObject);
			}
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000B1A80 File Offset: 0x000AFE80
		private void RefreshSizes()
		{
			Vector2 sizeDelta = this._draggingObjectOriginalSize;
			if (this._currentReorderableListRaycasted != null && this._currentReorderableListRaycasted.IsDropable && this._currentReorderableListRaycasted.Content.childCount > 0)
			{
				Transform child = this._currentReorderableListRaycasted.Content.GetChild(0);
				if (child != null)
				{
					sizeDelta = child.GetComponent<RectTransform>().rect.size;
				}
			}
			this._draggingObject.sizeDelta = sizeDelta;
			LayoutElement fakeElementLE = this._fakeElementLE;
			float num = sizeDelta.y;
			this._draggingObjectLE.preferredHeight = num;
			fakeElementLE.preferredHeight = num;
			LayoutElement fakeElementLE2 = this._fakeElementLE;
			num = sizeDelta.x;
			this._draggingObjectLE.preferredWidth = num;
			fakeElementLE2.preferredWidth = num;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000B1B49 File Offset: 0x000AFF49
		public void Init(ReorderableList reorderableList)
		{
			this._reorderableList = reorderableList;
			this._rect = base.GetComponent<RectTransform>();
		}

		// Token: 0x04001A58 RID: 6744
		[Tooltip("Can this element be dragged?")]
		public bool IsGrabbable = true;

		// Token: 0x04001A59 RID: 6745
		[Tooltip("Can this element be transfered to another list")]
		public bool IsTransferable = true;

		// Token: 0x04001A5A RID: 6746
		[Tooltip("Can this element be dropped in space?")]
		public bool isDroppableInSpace;

		// Token: 0x04001A5B RID: 6747
		private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

		// Token: 0x04001A5C RID: 6748
		private ReorderableList _currentReorderableListRaycasted;

		// Token: 0x04001A5D RID: 6749
		private RectTransform _draggingObject;

		// Token: 0x04001A5E RID: 6750
		private LayoutElement _draggingObjectLE;

		// Token: 0x04001A5F RID: 6751
		private Vector2 _draggingObjectOriginalSize;

		// Token: 0x04001A60 RID: 6752
		private RectTransform _fakeElement;

		// Token: 0x04001A61 RID: 6753
		private LayoutElement _fakeElementLE;

		// Token: 0x04001A62 RID: 6754
		private int _fromIndex;

		// Token: 0x04001A63 RID: 6755
		private bool _isDragging;

		// Token: 0x04001A64 RID: 6756
		private RectTransform _rect;

		// Token: 0x04001A65 RID: 6757
		private ReorderableList _reorderableList;

		// Token: 0x04001A66 RID: 6758
		internal bool isValid;
	}
}
