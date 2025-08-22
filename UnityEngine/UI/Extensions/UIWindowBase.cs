using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200053C RID: 1340
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/UI Window Base")]
	public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x0600221C RID: 8732 RVA: 0x000C364E File Offset: 0x000C1A4E
		public UIWindowBase()
		{
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000C3668 File Offset: 0x000C1A68
		private void Start()
		{
			this.m_transform = base.GetComponent<RectTransform>();
			this.m_originalCoods = this.m_transform.position;
			this.m_canvas = base.GetComponentInParent<Canvas>();
			this.m_canvasRectTransform = this.m_canvas.GetComponent<RectTransform>();
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000C36A4 File Offset: 0x000C1AA4
		private void Update()
		{
			if (UIWindowBase.ResetCoords)
			{
				this.resetCoordinatePosition();
			}
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000C36B8 File Offset: 0x000C1AB8
		public void OnDrag(PointerEventData eventData)
		{
			if (this._isDragging)
			{
				Vector3 b = this.ScreenToCanvas(eventData.position) - this.ScreenToCanvas(eventData.position - eventData.delta);
				this.m_transform.localPosition += b;
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000C371C File Offset: 0x000C1B1C
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == null)
			{
				return;
			}
			if (eventData.pointerCurrentRaycast.gameObject.name == base.name)
			{
				this._isDragging = true;
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000C376D File Offset: 0x000C1B6D
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isDragging = false;
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000C3776 File Offset: 0x000C1B76
		private void resetCoordinatePosition()
		{
			this.m_transform.position = this.m_originalCoods;
			UIWindowBase.ResetCoords = false;
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000C3790 File Offset: 0x000C1B90
		private Vector3 ScreenToCanvas(Vector3 screenPosition)
		{
			Vector2 sizeDelta = this.m_canvasRectTransform.sizeDelta;
			Vector3 result;
			Vector2 vector;
			Vector2 vector2;
			if (this.m_canvas.renderMode == RenderMode.ScreenSpaceOverlay || (this.m_canvas.renderMode == RenderMode.ScreenSpaceCamera && this.m_canvas.worldCamera == null))
			{
				result = screenPosition;
				vector = Vector2.zero;
				vector2 = sizeDelta;
			}
			else
			{
				Ray ray = this.m_canvas.worldCamera.ScreenPointToRay(screenPosition);
				Plane plane = new Plane(this.m_canvasRectTransform.forward, this.m_canvasRectTransform.position);
				float d;
				if (!plane.Raycast(ray, out d))
				{
					throw new Exception("Is it practically possible?");
				}
				Vector3 position = ray.origin + ray.direction * d;
				result = this.m_canvasRectTransform.InverseTransformPoint(position);
				vector = -Vector2.Scale(sizeDelta, this.m_canvasRectTransform.pivot);
				vector2 = Vector2.Scale(sizeDelta, Vector2.one - this.m_canvasRectTransform.pivot);
			}
			result.x = Mathf.Clamp(result.x, vector.x + (float)this.KeepWindowInCanvas, vector2.x - (float)this.KeepWindowInCanvas);
			result.y = Mathf.Clamp(result.y, vector.y + (float)this.KeepWindowInCanvas, vector2.y - (float)this.KeepWindowInCanvas);
			return result;
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x000C38FA File Offset: 0x000C1CFA
		// Note: this type is marked as 'beforefieldinit'.
		static UIWindowBase()
		{
		}

		// Token: 0x04001C65 RID: 7269
		private RectTransform m_transform;

		// Token: 0x04001C66 RID: 7270
		private bool _isDragging;

		// Token: 0x04001C67 RID: 7271
		public static bool ResetCoords;

		// Token: 0x04001C68 RID: 7272
		private Vector3 m_originalCoods = Vector3.zero;

		// Token: 0x04001C69 RID: 7273
		private Canvas m_canvas;

		// Token: 0x04001C6A RID: 7274
		private RectTransform m_canvasRectTransform;

		// Token: 0x04001C6B RID: 7275
		public int KeepWindowInCanvas = 5;
	}
}
