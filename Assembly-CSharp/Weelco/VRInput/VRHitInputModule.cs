using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Weelco.VRInput
{
	// Token: 0x0200058F RID: 1423
	public class VRHitInputModule : VRInputModule
	{
		// Token: 0x060023CB RID: 9163 RVA: 0x000CF9C0 File Offset: 0x000CDDC0
		public VRHitInputModule()
		{
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000CF9D3 File Offset: 0x000CDDD3
		public override void AddController(IUIPointer controller)
		{
			if (controller is IUIHitPointer)
			{
				this.controllerData.Add(controller as IUIHitPointer, new VRInputControllerData());
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000CF9F6 File Offset: 0x000CDDF6
		public override void RemoveController(IUIPointer controller)
		{
			if (controller is IUIHitPointer)
			{
				this.controllerData.Remove(controller as IUIHitPointer);
			}
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000CFA18 File Offset: 0x000CDE18
		public override void Process()
		{
			foreach (KeyValuePair<IUIHitPointer, VRInputControllerData> keyValuePair in this.controllerData)
			{
				IUIHitPointer key = keyValuePair.Key;
				VRInputControllerData value = keyValuePair.Value;
				base.UpdateCameraPosition(key);
				if (value.pointerEvent == null)
				{
					value.pointerEvent = new VRInputEventData(base.eventSystem);
				}
				else
				{
					value.pointerEvent.Reset();
				}
				value.pointerEvent.controller = key;
				value.pointerEvent.delta = Vector2.zero;
				value.pointerEvent.position = new Vector2(base.GetCameraSize().x * 0.5f, base.GetCameraSize().y * 0.5f);
				base.eventSystem.RaycastAll(value.pointerEvent, this.m_RaycastResultCache);
				value.pointerEvent.pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
				this.m_RaycastResultCache.Clear();
				if (value.pointerEvent.pointerCurrentRaycast.distance > 0f)
				{
					key.LimitLaserDistance(value.pointerEvent.pointerCurrentRaycast.distance + 0.01f);
				}
				GameObject gameObject = value.pointerEvent.pointerCurrentRaycast.gameObject;
				if (value.currentPoint != gameObject)
				{
					if (value.currentPoint != null)
					{
						key.OnExitControl(value.currentPoint);
					}
					if (gameObject != null)
					{
						key.OnEnterControl(gameObject);
					}
				}
				value.currentPoint = gameObject;
				base.HandlePointerExitAndEnter(value.pointerEvent, value.currentPoint);
				if (key.ButtonDown())
				{
					base.ClearSelection();
					value.pointerEvent.pressPosition = value.pointerEvent.position;
					value.pointerEvent.pointerPressRaycast = value.pointerEvent.pointerCurrentRaycast;
					value.pointerEvent.pointerPress = null;
					if (value.currentPoint != null)
					{
						value.currentPressed = value.currentPoint;
						value.pointerEvent.current = value.currentPressed;
						GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(value.currentPressed, value.pointerEvent, ExecuteEvents.pointerDownHandler);
						ExecuteEvents.Execute<IPointerDownHandler>(key.target.gameObject, value.pointerEvent, ExecuteEvents.pointerDownHandler);
						if (gameObject2 == null)
						{
							gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerClickHandler>(value.currentPressed, value.pointerEvent, ExecuteEvents.pointerClickHandler);
							ExecuteEvents.Execute<IPointerClickHandler>(key.target.gameObject, value.pointerEvent, ExecuteEvents.pointerClickHandler);
							if (gameObject2 != null)
							{
								value.currentPressed = gameObject2;
							}
						}
						else
						{
							value.currentPressed = gameObject2;
							ExecuteEvents.Execute<IPointerClickHandler>(gameObject2, value.pointerEvent, ExecuteEvents.pointerClickHandler);
							ExecuteEvents.Execute<IPointerClickHandler>(key.target.gameObject, value.pointerEvent, ExecuteEvents.pointerClickHandler);
						}
						if (gameObject2 != null)
						{
							value.pointerEvent.pointerPress = gameObject2;
							value.currentPressed = gameObject2;
							this.Select(value.currentPressed);
						}
						ExecuteEvents.Execute<IBeginDragHandler>(value.currentPressed, value.pointerEvent, ExecuteEvents.beginDragHandler);
						ExecuteEvents.Execute<IBeginDragHandler>(key.target.gameObject, value.pointerEvent, ExecuteEvents.beginDragHandler);
						value.pointerEvent.pointerDrag = value.currentPressed;
					}
				}
				if (key.ButtonUp())
				{
					if (value.currentPressed)
					{
						value.pointerEvent.current = value.currentPressed;
						ExecuteEvents.Execute<IPointerUpHandler>(value.currentPressed, value.pointerEvent, ExecuteEvents.pointerUpHandler);
						ExecuteEvents.Execute<IPointerUpHandler>(key.target.gameObject, value.pointerEvent, ExecuteEvents.pointerUpHandler);
						value.pointerEvent.rawPointerPress = null;
						value.pointerEvent.pointerPress = null;
						value.currentPressed = null;
					}
					base.ClearSelection();
				}
				if (base.eventSystem.currentSelectedGameObject != null)
				{
					value.pointerEvent.current = base.eventSystem.currentSelectedGameObject;
					ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, this.GetBaseEventData(), ExecuteEvents.updateSelectedHandler);
				}
			}
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000CFE78 File Offset: 0x000CE278
		private void Select(GameObject go)
		{
			base.ClearSelection();
			if (ExecuteEvents.GetEventHandler<ISelectHandler>(go))
			{
				base.eventSystem.SetSelectedGameObject(go);
			}
		}

		// Token: 0x04001E2C RID: 7724
		private Dictionary<IUIHitPointer, VRInputControllerData> controllerData = new Dictionary<IUIHitPointer, VRInputControllerData>();
	}
}
