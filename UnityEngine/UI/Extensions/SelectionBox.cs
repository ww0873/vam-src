using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004DE RID: 1246
	[RequireComponent(typeof(Canvas))]
	[AddComponentMenu("UI/Extensions/Selection Box")]
	public class SelectionBox : MonoBehaviour
	{
		// Token: 0x06001F7C RID: 8060 RVA: 0x000B2B95 File Offset: 0x000B0F95
		public SelectionBox()
		{
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x000B2BA8 File Offset: 0x000B0FA8
		private void ValidateCanvas()
		{
			Canvas component = base.gameObject.GetComponent<Canvas>();
			if (component.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				throw new Exception("SelectionBox component must be placed on a canvas in Screen Space Overlay mode.");
			}
			CanvasScaler component2 = base.gameObject.GetComponent<CanvasScaler>();
			if (component2 && component2.enabled && (!Mathf.Approximately(component2.scaleFactor, 1f) || component2.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize))
			{
				Object.Destroy(component2);
				Debug.LogWarning("SelectionBox component is on a gameObject with a Canvas Scaler component. As of now, Canvas Scalers without the default settings throw off the coordinates of the selection box. Canvas Scaler has been removed.");
			}
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x000B2C2C File Offset: 0x000B102C
		private void SetSelectableGroup(IEnumerable<MonoBehaviour> behaviourCollection)
		{
			if (behaviourCollection == null)
			{
				this.selectableGroup = null;
				return;
			}
			List<MonoBehaviour> list = new List<MonoBehaviour>();
			foreach (MonoBehaviour monoBehaviour in behaviourCollection)
			{
				if (monoBehaviour is IBoxSelectable)
				{
					list.Add(monoBehaviour);
				}
			}
			this.selectableGroup = list.ToArray();
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000B2CAC File Offset: 0x000B10AC
		private void CreateBoxRect()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "Selection Box";
			gameObject.transform.parent = base.transform;
			gameObject.AddComponent<Image>();
			this.boxRect = (gameObject.transform as RectTransform);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000B2CF4 File Offset: 0x000B10F4
		private void ResetBoxRect()
		{
			Image component = this.boxRect.GetComponent<Image>();
			component.color = this.color;
			component.sprite = this.art;
			this.origin = Vector2.zero;
			this.boxRect.anchoredPosition = Vector2.zero;
			this.boxRect.sizeDelta = Vector2.zero;
			this.boxRect.anchorMax = Vector2.zero;
			this.boxRect.anchorMin = Vector2.zero;
			this.boxRect.pivot = Vector2.zero;
			this.boxRect.gameObject.SetActive(false);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x000B2D94 File Offset: 0x000B1194
		private void BeginSelection()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			this.boxRect.gameObject.SetActive(true);
			this.origin = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if (!this.PointIsValidAgainstSelectionMask(this.origin))
			{
				this.ResetBoxRect();
				return;
			}
			this.boxRect.anchoredPosition = this.origin;
			MonoBehaviour[] array;
			if (this.selectableGroup == null)
			{
				array = Object.FindObjectsOfType<MonoBehaviour>();
			}
			else
			{
				array = this.selectableGroup;
			}
			List<IBoxSelectable> list = new List<IBoxSelectable>();
			foreach (MonoBehaviour monoBehaviour in array)
			{
				IBoxSelectable boxSelectable = monoBehaviour as IBoxSelectable;
				if (boxSelectable != null)
				{
					list.Add(boxSelectable);
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						boxSelectable.selected = false;
					}
				}
			}
			this.selectables = list.ToArray();
			this.clickedBeforeDrag = this.GetSelectableAtMousePosition();
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000B2E9C File Offset: 0x000B129C
		private bool PointIsValidAgainstSelectionMask(Vector2 screenPoint)
		{
			if (!this.selectionMask)
			{
				return true;
			}
			Camera screenPointCamera = this.GetScreenPointCamera(this.selectionMask);
			return RectTransformUtility.RectangleContainsScreenPoint(this.selectionMask, screenPoint, screenPointCamera);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000B2ED8 File Offset: 0x000B12D8
		private IBoxSelectable GetSelectableAtMousePosition()
		{
			if (!this.PointIsValidAgainstSelectionMask(Input.mousePosition))
			{
				return null;
			}
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				RectTransform rectTransform = boxSelectable.transform as RectTransform;
				if (rectTransform)
				{
					Camera screenPointCamera = this.GetScreenPointCamera(rectTransform);
					if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, screenPointCamera))
					{
						return boxSelectable;
					}
				}
				else
				{
					float magnitude = boxSelectable.transform.GetComponent<Renderer>().bounds.extents.magnitude;
					Vector2 screenPointOfSelectable = this.GetScreenPointOfSelectable(boxSelectable);
					if (Vector2.Distance(screenPointOfSelectable, Input.mousePosition) <= magnitude)
					{
						return boxSelectable;
					}
				}
			}
			return null;
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x000B2FA4 File Offset: 0x000B13A4
		private void DragSelection()
		{
			if (!Input.GetMouseButton(0) || !this.boxRect.gameObject.activeSelf)
			{
				return;
			}
			Vector2 a = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector2 sizeDelta = a - this.origin;
			Vector2 anchoredPosition = this.origin;
			if (sizeDelta.x < 0f)
			{
				anchoredPosition.x = a.x;
				sizeDelta.x = -sizeDelta.x;
			}
			if (sizeDelta.y < 0f)
			{
				anchoredPosition.y = a.y;
				sizeDelta.y = -sizeDelta.y;
			}
			this.boxRect.anchoredPosition = anchoredPosition;
			this.boxRect.sizeDelta = sizeDelta;
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				Vector3 v = this.GetScreenPointOfSelectable(boxSelectable);
				boxSelectable.preSelected = (RectTransformUtility.RectangleContainsScreenPoint(this.boxRect, v, null) && this.PointIsValidAgainstSelectionMask(v));
			}
			if (this.clickedBeforeDrag != null)
			{
				this.clickedBeforeDrag.preSelected = true;
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x000B30FC File Offset: 0x000B14FC
		private void ApplySingleClickDeselection()
		{
			if (this.clickedBeforeDrag == null)
			{
				return;
			}
			if (this.clickedAfterDrag != null && this.clickedBeforeDrag.selected && this.clickedBeforeDrag.transform == this.clickedAfterDrag.transform)
			{
				this.clickedBeforeDrag.selected = false;
				this.clickedBeforeDrag.preSelected = false;
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x000B3168 File Offset: 0x000B1568
		private void ApplyPreSelections()
		{
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				if (boxSelectable.preSelected)
				{
					boxSelectable.selected = true;
					boxSelectable.preSelected = false;
				}
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x000B31B0 File Offset: 0x000B15B0
		private Vector2 GetScreenPointOfSelectable(IBoxSelectable selectable)
		{
			RectTransform rectTransform = selectable.transform as RectTransform;
			if (rectTransform)
			{
				Camera screenPointCamera = this.GetScreenPointCamera(rectTransform);
				return RectTransformUtility.WorldToScreenPoint(screenPointCamera, selectable.transform.position);
			}
			return Camera.main.WorldToScreenPoint(selectable.transform.position);
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000B3208 File Offset: 0x000B1608
		private Camera GetScreenPointCamera(RectTransform rectTransform)
		{
			RectTransform rectTransform2 = rectTransform;
			Canvas canvas;
			do
			{
				canvas = rectTransform2.GetComponent<Canvas>();
				if (canvas && !canvas.isRootCanvas)
				{
					canvas = null;
				}
				rectTransform2 = (RectTransform)rectTransform2.parent;
			}
			while (canvas == null);
			switch (canvas.renderMode)
			{
			case RenderMode.ScreenSpaceOverlay:
				return null;
			case RenderMode.ScreenSpaceCamera:
				return (!canvas.worldCamera) ? Camera.main : canvas.worldCamera;
			}
			return Camera.main;
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000B3298 File Offset: 0x000B1698
		public IBoxSelectable[] GetAllSelected()
		{
			if (this.selectables == null)
			{
				return new IBoxSelectable[0];
			}
			List<IBoxSelectable> list = new List<IBoxSelectable>();
			foreach (IBoxSelectable boxSelectable in this.selectables)
			{
				if (boxSelectable.selected)
				{
					list.Add(boxSelectable);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x000B32F4 File Offset: 0x000B16F4
		private void EndSelection()
		{
			if (!Input.GetMouseButtonUp(0) || !this.boxRect.gameObject.activeSelf)
			{
				return;
			}
			this.clickedAfterDrag = this.GetSelectableAtMousePosition();
			this.ApplySingleClickDeselection();
			this.ApplyPreSelections();
			this.ResetBoxRect();
			this.onSelectionChange.Invoke(this.GetAllSelected());
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000B3351 File Offset: 0x000B1751
		private void Start()
		{
			this.ValidateCanvas();
			this.CreateBoxRect();
			this.ResetBoxRect();
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000B3365 File Offset: 0x000B1765
		private void Update()
		{
			this.BeginSelection();
			this.DragSelection();
			this.EndSelection();
		}

		// Token: 0x04001A88 RID: 6792
		public Color color;

		// Token: 0x04001A89 RID: 6793
		public Sprite art;

		// Token: 0x04001A8A RID: 6794
		private Vector2 origin;

		// Token: 0x04001A8B RID: 6795
		public RectTransform selectionMask;

		// Token: 0x04001A8C RID: 6796
		private RectTransform boxRect;

		// Token: 0x04001A8D RID: 6797
		private IBoxSelectable[] selectables;

		// Token: 0x04001A8E RID: 6798
		private MonoBehaviour[] selectableGroup;

		// Token: 0x04001A8F RID: 6799
		private IBoxSelectable clickedBeforeDrag;

		// Token: 0x04001A90 RID: 6800
		private IBoxSelectable clickedAfterDrag;

		// Token: 0x04001A91 RID: 6801
		public SelectionBox.SelectionEvent onSelectionChange = new SelectionBox.SelectionEvent();

		// Token: 0x020004DF RID: 1247
		public class SelectionEvent : UnityEvent<IBoxSelectable[]>
		{
			// Token: 0x06001F8D RID: 8077 RVA: 0x000B3379 File Offset: 0x000B1779
			public SelectionEvent()
			{
			}
		}
	}
}
