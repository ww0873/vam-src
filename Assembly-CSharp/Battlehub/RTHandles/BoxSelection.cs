using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Battlehub.RTCommon;
using Battlehub.RTSaveLoad;
using Battlehub.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTHandles
{
	// Token: 0x020000FA RID: 250
	public class BoxSelection : MonoBehaviour
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x0001F528 File Offset: 0x0001D928
		public BoxSelection()
		{
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060005B0 RID: 1456 RVA: 0x0001F530 File Offset: 0x0001D930
		// (remove) Token: 0x060005B1 RID: 1457 RVA: 0x0001F564 File Offset: 0x0001D964
		public static event EventHandler<FilteringArgs> Filtering
		{
			add
			{
				EventHandler<FilteringArgs> eventHandler = BoxSelection.Filtering;
				EventHandler<FilteringArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<FilteringArgs>>(ref BoxSelection.Filtering, (EventHandler<FilteringArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<FilteringArgs> eventHandler = BoxSelection.Filtering;
				EventHandler<FilteringArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<FilteringArgs>>(ref BoxSelection.Filtering, (EventHandler<FilteringArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0001F598 File Offset: 0x0001D998
		public bool IsDragging
		{
			get
			{
				return this.m_isDragging;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0001F5A0 File Offset: 0x0001D9A0
		public static BoxSelection Current
		{
			get
			{
				return BoxSelection.m_current;
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001F5A8 File Offset: 0x0001D9A8
		private void Awake()
		{
			if (BoxSelection.m_current != null)
			{
				UnityEngine.Debug.LogWarning("Another instance of BoxSelection exists");
			}
			if (!base.GetComponent<PersistentIgnore>())
			{
				base.gameObject.AddComponent<PersistentIgnore>();
			}
			BoxSelection.m_current = this;
			this.m_image = base.gameObject.AddComponent<Image>();
			this.m_image.type = Image.Type.Sliced;
			if (this.Graphics == null)
			{
				this.Graphics = Resources.Load<Sprite>("BoxSelection");
			}
			this.m_image.sprite = this.Graphics;
			this.m_image.raycastTarget = false;
			this.m_rectTransform = base.GetComponent<RectTransform>();
			this.m_rectTransform.sizeDelta = new Vector2(0f, 0f);
			this.m_rectTransform.pivot = new Vector2(0f, 0f);
			this.m_rectTransform.anchoredPosition = new Vector3(0f, 0f);
			if (this.SceneCamera == null)
			{
				this.SceneCamera = Camera.main;
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001F6C8 File Offset: 0x0001DAC8
		private void OnEnable()
		{
			this.m_canvas = base.GetComponentInParent<Canvas>();
			if (this.SceneCamera == null)
			{
				return;
			}
			if (this.m_canvas == null)
			{
				this.m_canvas = new GameObject
				{
					name = "BoxSelectionCanvas"
				}.AddComponent<Canvas>();
			}
			CanvasScaler canvasScaler = this.m_canvas.GetComponent<CanvasScaler>();
			if (canvasScaler == null)
			{
				canvasScaler = this.m_canvas.gameObject.AddComponent<CanvasScaler>();
			}
			if (this.UseCameraSpace)
			{
				this.m_canvas.worldCamera = this.SceneCamera;
				this.m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
				this.m_canvas.planeDistance = this.SceneCamera.nearClipPlane + 0.05f;
			}
			else
			{
				this.m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			}
			canvasScaler.referencePixelsPerUnit = 1f;
			base.transform.SetParent(this.m_canvas.gameObject.transform);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001F7C5 File Offset: 0x0001DBC5
		public void SetSceneCamera(Camera camera)
		{
			this.SceneCamera = camera;
			if (this.m_canvas != null && this.UseCameraSpace)
			{
				this.m_canvas.worldCamera = camera;
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001F7F6 File Offset: 0x0001DBF6
		private void OnDestroy()
		{
			if (RuntimeTools.ActiveTool == this)
			{
				RuntimeTools.ActiveTool = null;
			}
			if (BoxSelection.m_current == this)
			{
				BoxSelection.m_current = null;
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001F824 File Offset: 0x0001DC24
		private void LateUpdate()
		{
			if (RuntimeTools.ActiveTool != null && RuntimeTools.ActiveTool != this)
			{
				return;
			}
			if (RuntimeTools.IsViewing)
			{
				return;
			}
			if (this.KeyCode == KeyCode.None || InputController.GetKeyDown(this.KeyCode))
			{
				this.m_active = true;
			}
			if (this.m_active)
			{
				if (Input.GetMouseButtonDown(this.MouseButton))
				{
					this.m_startMousePosition = Input.mousePosition;
					this.m_isDragging = (this.GetPoint(out this.m_startPt) && (!RuntimeEditorApplication.IsOpened || (RuntimeEditorApplication.IsPointerOverWindow(RuntimeWindowType.SceneView) && !RuntimeTools.IsPointerOverGameObject())));
					if (this.m_isDragging)
					{
						this.m_rectTransform.anchoredPosition = this.m_startPt;
						this.m_rectTransform.sizeDelta = new Vector2(0f, 0f);
						CursorHelper.SetCursor(this, null, Vector3.zero, CursorMode.Auto);
					}
					else
					{
						RuntimeTools.ActiveTool = null;
					}
				}
				else if (Input.GetMouseButtonUp(this.MouseButton))
				{
					if (this.m_isDragging)
					{
						this.m_isDragging = false;
						this.HitTest();
						this.m_rectTransform.sizeDelta = new Vector2(0f, 0f);
						CursorHelper.ResetCursor(this);
					}
					RuntimeTools.ActiveTool = null;
					this.m_active = false;
				}
				else if (this.m_isDragging)
				{
					this.GetPoint(out this.m_endPt);
					Vector2 lhs = this.m_endPt - this.m_startPt;
					if (lhs != Vector2.zero)
					{
						RuntimeTools.ActiveTool = this;
					}
					this.m_rectTransform.sizeDelta = new Vector2(Mathf.Abs(lhs.x), Mathf.Abs(lhs.y));
					this.m_rectTransform.localScale = new Vector3(Mathf.Sign(lhs.x), Mathf.Sign(lhs.y), 1f);
				}
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001FA2C File Offset: 0x0001DE2C
		private void HitTest()
		{
			if (this.m_rectTransform.sizeDelta.magnitude < 5f)
			{
				return;
			}
			Vector3 center = (this.m_startMousePosition + Input.mousePosition) / 2f;
			center.z = 0f;
			Bounds bounds = new Bounds(center, this.m_rectTransform.sizeDelta);
			Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(this.SceneCamera);
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			Renderer[] array = UnityEngine.Object.FindObjectsOfType<Renderer>();
			Collider[] array2 = UnityEngine.Object.FindObjectsOfType<Collider>();
			FilteringArgs args = new FilteringArgs();
			foreach (Renderer renderer in array)
			{
				Bounds bounds2 = renderer.bounds;
				GameObject gameObject = renderer.gameObject;
				this.TrySelect(ref bounds, hashSet, args, ref bounds2, gameObject, frustumPlanes);
			}
			foreach (Collider collider in array2)
			{
				Bounds bounds3 = collider.bounds;
				GameObject gameObject2 = collider.gameObject;
				this.TrySelect(ref bounds, hashSet, args, ref bounds3, gameObject2, frustumPlanes);
			}
			RuntimeSelection.objects = hashSet.ToArray<GameObject>();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001FB50 File Offset: 0x0001DF50
		private void TrySelect(ref Bounds selectionBounds, HashSet<GameObject> selection, FilteringArgs args, ref Bounds bounds, GameObject go, Plane[] frustumPlanes)
		{
			bool flag;
			if (this.Method == BoxSelectionMethod.LooseFitting)
			{
				flag = this.LooseFitting(ref selectionBounds, ref bounds);
			}
			else if (this.Method == BoxSelectionMethod.BoundsCenter)
			{
				flag = this.BoundsCenter(ref selectionBounds, ref bounds);
			}
			else
			{
				flag = this.TransformCenter(ref selectionBounds, go.transform);
			}
			if (!GeometryUtility.TestPlanesAABB(frustumPlanes, bounds))
			{
				flag = false;
			}
			if (flag && !selection.Contains(go))
			{
				if (BoxSelection.Filtering != null)
				{
					args.Object = go;
					BoxSelection.Filtering(this, args);
					if (!args.Cancel)
					{
						selection.Add(go);
					}
					args.Reset();
				}
				else
				{
					selection.Add(go);
				}
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001FC10 File Offset: 0x0001E010
		private bool TransformCenter(ref Bounds selectionBounds, Transform tr)
		{
			Vector3 point = this.SceneCamera.WorldToScreenPoint(tr.position);
			point.z = 0f;
			return selectionBounds.Contains(point);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001FC44 File Offset: 0x0001E044
		private bool BoundsCenter(ref Bounds selectionBounds, ref Bounds bounds)
		{
			Vector3 point = this.SceneCamera.WorldToScreenPoint(bounds.center);
			point.z = 0f;
			return selectionBounds.Contains(point);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001FC78 File Offset: 0x0001E078
		private bool LooseFitting(ref Bounds selectionBounds, ref Bounds bounds)
		{
			Vector3 position = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
			Vector3 position2 = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
			Vector3 position3 = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
			Vector3 position4 = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
			Vector3 position5 = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
			Vector3 position6 = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
			Vector3 position7 = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
			Vector3 position8 = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
			position = this.SceneCamera.WorldToScreenPoint(position);
			position2 = this.SceneCamera.WorldToScreenPoint(position2);
			position3 = this.SceneCamera.WorldToScreenPoint(position3);
			position4 = this.SceneCamera.WorldToScreenPoint(position4);
			position5 = this.SceneCamera.WorldToScreenPoint(position5);
			position6 = this.SceneCamera.WorldToScreenPoint(position6);
			position7 = this.SceneCamera.WorldToScreenPoint(position7);
			position8 = this.SceneCamera.WorldToScreenPoint(position8);
			float x = Mathf.Min(new float[]
			{
				position.x,
				position2.x,
				position3.x,
				position4.x,
				position5.x,
				position6.x,
				position7.x,
				position8.x
			});
			float x2 = Mathf.Max(new float[]
			{
				position.x,
				position2.x,
				position3.x,
				position4.x,
				position5.x,
				position6.x,
				position7.x,
				position8.x
			});
			float y = Mathf.Min(new float[]
			{
				position.y,
				position2.y,
				position3.y,
				position4.y,
				position5.y,
				position6.y,
				position7.y,
				position8.y
			});
			float y2 = Mathf.Max(new float[]
			{
				position.y,
				position2.y,
				position3.y,
				position4.y,
				position5.y,
				position6.y,
				position7.y,
				position8.y
			});
			Vector3 vector = new Vector2(x, y);
			Vector3 vector2 = new Vector2(x2, y2);
			Bounds bounds2 = new Bounds((vector + vector2) / 2f, vector2 - vector);
			return selectionBounds.Intersects(bounds2);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000200BC File Offset: 0x0001E4BC
		private bool GetPoint(out Vector2 localPoint)
		{
			Camera cam = null;
			if (this.m_canvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				cam = this.m_canvas.worldCamera;
			}
			return RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_canvas.GetComponent<RectTransform>(), Input.mousePosition, cam, out localPoint);
		}

		// Token: 0x04000519 RID: 1305
		public Sprite Graphics;

		// Token: 0x0400051A RID: 1306
		protected Image m_image;

		// Token: 0x0400051B RID: 1307
		protected RectTransform m_rectTransform;

		// Token: 0x0400051C RID: 1308
		protected Canvas m_canvas;

		// Token: 0x0400051D RID: 1309
		protected bool m_isDragging;

		// Token: 0x0400051E RID: 1310
		protected Vector3 m_startMousePosition;

		// Token: 0x0400051F RID: 1311
		protected Vector2 m_startPt;

		// Token: 0x04000520 RID: 1312
		protected Vector2 m_endPt;

		// Token: 0x04000521 RID: 1313
		public bool UseCameraSpace;

		// Token: 0x04000522 RID: 1314
		public Camera SceneCamera;

		// Token: 0x04000523 RID: 1315
		public BoxSelectionMethod Method;

		// Token: 0x04000524 RID: 1316
		public int MouseButton;

		// Token: 0x04000525 RID: 1317
		public KeyCode KeyCode;

		// Token: 0x04000526 RID: 1318
		protected bool m_active;

		// Token: 0x04000527 RID: 1319
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler<FilteringArgs> Filtering;

		// Token: 0x04000528 RID: 1320
		private static BoxSelection m_current;
	}
}
