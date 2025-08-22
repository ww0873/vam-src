using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200096F RID: 2415
[RequireComponent(typeof(Canvas))]
public class OVRRaycaster : GraphicRaycaster, IPointerEnterHandler, IEventSystemHandler
{
	// Token: 0x06003C56 RID: 15446 RVA: 0x00124443 File Offset: 0x00122843
	protected OVRRaycaster()
	{
	}

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06003C57 RID: 15447 RVA: 0x00124456 File Offset: 0x00122856
	private Canvas canvas
	{
		get
		{
			if (this.m_Canvas != null)
			{
				return this.m_Canvas;
			}
			this.m_Canvas = base.GetComponent<Canvas>();
			return this.m_Canvas;
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06003C58 RID: 15448 RVA: 0x00124482 File Offset: 0x00122882
	public override Camera eventCamera
	{
		get
		{
			return this.canvas.worldCamera;
		}
	}

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x06003C59 RID: 15449 RVA: 0x0012448F File Offset: 0x0012288F
	public override int sortOrderPriority
	{
		get
		{
			return this.sortOrder;
		}
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00124498 File Offset: 0x00122898
	private void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList, Ray ray, bool checkForBlocking)
	{
		if (this.canvas == null)
		{
			return;
		}
		float num = float.MaxValue;
		if (checkForBlocking && base.blockingObjects != GraphicRaycaster.BlockingObjects.None)
		{
			float farClipPlane = this.eventCamera.farClipPlane;
			if (base.blockingObjects == GraphicRaycaster.BlockingObjects.ThreeD || base.blockingObjects == GraphicRaycaster.BlockingObjects.All)
			{
				UnityEngine.RaycastHit[] array = Physics.RaycastAll(ray, farClipPlane, this.m_BlockingMask);
				if (array.Length > 0 && array[0].distance < num)
				{
					num = array[0].distance;
				}
			}
			if (base.blockingObjects == GraphicRaycaster.BlockingObjects.TwoD || base.blockingObjects == GraphicRaycaster.BlockingObjects.All)
			{
				RaycastHit2D[] rayIntersectionAll = Physics2D.GetRayIntersectionAll(ray, farClipPlane, this.m_BlockingMask);
				if (rayIntersectionAll.Length > 0 && rayIntersectionAll[0].fraction * farClipPlane < num)
				{
					num = rayIntersectionAll[0].fraction * farClipPlane;
				}
			}
		}
		this.m_RaycastResults.Clear();
		this.GraphicRaycast(this.canvas, ray, this.m_RaycastResults);
		for (int i = 0; i < this.m_RaycastResults.Count; i++)
		{
			GameObject gameObject = this.m_RaycastResults[i].graphic.gameObject;
			bool flag = true;
			if (base.ignoreReversedGraphics)
			{
				Vector3 direction = ray.direction;
				Vector3 rhs = gameObject.transform.rotation * Vector3.forward;
				flag = (Vector3.Dot(direction, rhs) > 0f);
			}
			if (this.eventCamera.transform.InverseTransformPoint(this.m_RaycastResults[i].worldPos).z <= 0f)
			{
				flag = false;
			}
			if (flag)
			{
				float num2 = Vector3.Distance(ray.origin, this.m_RaycastResults[i].worldPos);
				if (num2 < num)
				{
					RaycastResult item = new RaycastResult
					{
						gameObject = gameObject,
						module = this,
						distance = num2,
						index = (float)resultAppendList.Count,
						depth = this.m_RaycastResults[i].graphic.depth,
						worldPosition = this.m_RaycastResults[i].worldPos
					};
					resultAppendList.Add(item);
				}
			}
		}
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00124710 File Offset: 0x00122B10
	public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
	{
		if (eventData.IsVRPointer())
		{
			this.Raycast(eventData, resultAppendList, eventData.GetRay(), true);
		}
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x0012472C File Offset: 0x00122B2C
	public void RaycastPointer(PointerEventData eventData, List<RaycastResult> resultAppendList)
	{
		if (this.pointer != null && this.pointer.activeInHierarchy)
		{
			this.Raycast(eventData, resultAppendList, new Ray(this.eventCamera.transform.position, (this.pointer.transform.position - this.eventCamera.transform.position).normalized), false);
		}
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x001247A8 File Offset: 0x00122BA8
	private void GraphicRaycast(Canvas canvas, Ray ray, List<OVRRaycaster.RaycastHit> results)
	{
		IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
		OVRRaycaster.s_SortedGraphics.Clear();
		for (int i = 0; i < graphicsForCanvas.Count; i++)
		{
			Graphic graphic = graphicsForCanvas[i];
			if (graphic.depth != -1 && !(this.pointer == graphic.gameObject))
			{
				Vector3 vector;
				if (OVRRaycaster.RayIntersectsRectTransform(graphic.rectTransform, ray, out vector))
				{
					Vector2 sp = this.eventCamera.WorldToScreenPoint(vector);
					if (graphic.Raycast(sp, this.eventCamera))
					{
						OVRRaycaster.RaycastHit item;
						item.graphic = graphic;
						item.worldPos = vector;
						item.fromMouse = false;
						OVRRaycaster.s_SortedGraphics.Add(item);
					}
				}
			}
		}
		List<OVRRaycaster.RaycastHit> list = OVRRaycaster.s_SortedGraphics;
		if (OVRRaycaster.<>f__am$cache0 == null)
		{
			OVRRaycaster.<>f__am$cache0 = new Comparison<OVRRaycaster.RaycastHit>(OVRRaycaster.<GraphicRaycast>m__0);
		}
		list.Sort(OVRRaycaster.<>f__am$cache0);
		for (int j = 0; j < OVRRaycaster.s_SortedGraphics.Count; j++)
		{
			results.Add(OVRRaycaster.s_SortedGraphics[j]);
		}
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x001248C1 File Offset: 0x00122CC1
	public Vector2 GetScreenPosition(RaycastResult raycastResult)
	{
		return this.eventCamera.WorldToScreenPoint(raycastResult.worldPosition);
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x001248DC File Offset: 0x00122CDC
	private static bool RayIntersectsRectTransform(RectTransform rectTransform, Ray ray, out Vector3 worldPos)
	{
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		Plane plane = new Plane(array[0], array[1], array[2]);
		float distance;
		if (!plane.Raycast(ray, out distance))
		{
			worldPos = Vector3.zero;
			return false;
		}
		Vector3 point = ray.GetPoint(distance);
		Vector3 vector = array[3] - array[0];
		Vector3 vector2 = array[1] - array[0];
		float num = Vector3.Dot(point - array[0], vector);
		float num2 = Vector3.Dot(point - array[0], vector2);
		if (num < vector.sqrMagnitude && num2 < vector2.sqrMagnitude && num >= 0f && num2 >= 0f)
		{
			worldPos = array[0] + num2 * vector2 / vector2.sqrMagnitude + num * vector / vector.sqrMagnitude;
			return true;
		}
		worldPos = Vector3.zero;
		return false;
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x00124A44 File Offset: 0x00122E44
	public bool IsFocussed()
	{
		OVRInputModule ovrinputModule = EventSystem.current.currentInputModule as OVRInputModule;
		return ovrinputModule && ovrinputModule.activeGraphicRaycaster == this;
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x00124A7C File Offset: 0x00122E7C
	public void OnPointerEnter(PointerEventData e)
	{
		if (e.IsVRPointer())
		{
			OVRInputModule ovrinputModule = EventSystem.current.currentInputModule as OVRInputModule;
			ovrinputModule.activeGraphicRaycaster = this;
		}
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x00124AAB File Offset: 0x00122EAB
	// Note: this type is marked as 'beforefieldinit'.
	static OVRRaycaster()
	{
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00124AB8 File Offset: 0x00122EB8
	[CompilerGenerated]
	private static int <GraphicRaycast>m__0(OVRRaycaster.RaycastHit g1, OVRRaycaster.RaycastHit g2)
	{
		return g2.graphic.depth.CompareTo(g1.graphic.depth);
	}

	// Token: 0x04002E3E RID: 11838
	[Tooltip("A world space pointer for this canvas")]
	public GameObject pointer;

	// Token: 0x04002E3F RID: 11839
	public int sortOrder;

	// Token: 0x04002E40 RID: 11840
	[NonSerialized]
	private Canvas m_Canvas;

	// Token: 0x04002E41 RID: 11841
	[NonSerialized]
	private List<OVRRaycaster.RaycastHit> m_RaycastResults = new List<OVRRaycaster.RaycastHit>();

	// Token: 0x04002E42 RID: 11842
	[NonSerialized]
	private static readonly List<OVRRaycaster.RaycastHit> s_SortedGraphics = new List<OVRRaycaster.RaycastHit>();

	// Token: 0x04002E43 RID: 11843
	[CompilerGenerated]
	private static Comparison<OVRRaycaster.RaycastHit> <>f__am$cache0;

	// Token: 0x02000970 RID: 2416
	private struct RaycastHit
	{
		// Token: 0x04002E44 RID: 11844
		public Graphic graphic;

		// Token: 0x04002E45 RID: 11845
		public Vector3 worldPos;

		// Token: 0x04002E46 RID: 11846
		public bool fromMouse;
	}
}
