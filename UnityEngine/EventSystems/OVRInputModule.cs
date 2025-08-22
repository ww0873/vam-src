using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000965 RID: 2405
	public class OVRInputModule : PointerInputModule
	{
		// Token: 0x06003BF1 RID: 15345 RVA: 0x0012197C File Offset: 0x0011FD7C
		protected OVRInputModule()
		{
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x00121A25 File Offset: 0x0011FE25
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public OVRInputModule.InputMode inputMode
		{
			get
			{
				return OVRInputModule.InputMode.Mouse;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x00121A28 File Offset: 0x0011FE28
		// (set) Token: 0x06003BF4 RID: 15348 RVA: 0x00121A30 File Offset: 0x0011FE30
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_AllowActivationOnMobileDevice;
			}
			set
			{
				this.m_AllowActivationOnMobileDevice = value;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06003BF5 RID: 15349 RVA: 0x00121A39 File Offset: 0x0011FE39
		// (set) Token: 0x06003BF6 RID: 15350 RVA: 0x00121A41 File Offset: 0x0011FE41
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06003BF7 RID: 15351 RVA: 0x00121A4A File Offset: 0x0011FE4A
		// (set) Token: 0x06003BF8 RID: 15352 RVA: 0x00121A52 File Offset: 0x0011FE52
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06003BF9 RID: 15353 RVA: 0x00121A5B File Offset: 0x0011FE5B
		// (set) Token: 0x06003BFA RID: 15354 RVA: 0x00121A63 File Offset: 0x0011FE63
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06003BFB RID: 15355 RVA: 0x00121A6C File Offset: 0x0011FE6C
		// (set) Token: 0x06003BFC RID: 15356 RVA: 0x00121A74 File Offset: 0x0011FE74
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06003BFD RID: 15357 RVA: 0x00121A7D File Offset: 0x0011FE7D
		// (set) Token: 0x06003BFE RID: 15358 RVA: 0x00121A85 File Offset: 0x0011FE85
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x00121A8E File Offset: 0x0011FE8E
		public override void UpdateModule()
		{
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_MousePosition = Input.mousePosition;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00121AAC File Offset: 0x0011FEAC
		public override bool IsModuleSupported()
		{
			return this.m_AllowActivationOnMobileDevice || Input.mousePresent;
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x00121AC4 File Offset: 0x0011FEC4
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_SubmitButton);
			flag |= Input.GetButtonDown(this.m_CancelButton);
			flag |= !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f);
			flag |= !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f);
			flag |= ((this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f);
			return flag | Input.GetMouseButtonDown(0);
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x00121B5C File Offset: 0x0011FF5C
		public override void ActivateModule()
		{
			base.ActivateModule();
			this.m_MousePosition = Input.mousePosition;
			this.m_LastMousePosition = Input.mousePosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x00121BC5 File Offset: 0x0011FFC5
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			this.ClearSelection();
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x00121BD4 File Offset: 0x0011FFD4
		private bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (Input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00121C54 File Offset: 0x00120054
		private bool AllowMoveEventProcessing(float time)
		{
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis);
			flag |= Input.GetButtonDown(this.m_VerticalAxis);
			return flag | time > this.m_NextAction;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x00121C88 File Offset: 0x00120088
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
			if (Input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x00121D54 File Offset: 0x00120154
		private bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			if (!this.AllowMoveEventProcessing(unscaledTime))
			{
				return false;
			}
			Vector2 rawMoveVector = this.GetRawMoveVector();
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			if (!Mathf.Approximately(axisEventData.moveVector.x, 0f) || !Mathf.Approximately(axisEventData.moveVector.y, 0f))
			{
				ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			}
			this.m_NextAction = unscaledTime + 1f / this.m_InputActionsPerSecond;
			return axisEventData.used;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x00121E04 File Offset: 0x00120204
		private bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x00121E50 File Offset: 0x00120250
		private void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				if (buttonData.IsVRPointer())
				{
					buttonData.SetSwipeStart(Input.mousePosition);
				}
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					float num = unscaledTime - buttonData.clickTime;
					if (num < 0.3f)
					{
						buttonData.clickCount++;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x00122084 File Offset: 0x00120484
		private void ProcessMouseEvent(PointerInputModule.MouseState mouseData)
		{
			bool pressed = mouseData.AnyPressesThisFrame();
			bool released = mouseData.AnyReleasesThisFrame();
			PointerInputModule.MouseButtonEventData eventData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			if (!OVRInputModule.UseMouse(pressed, released, eventData.buttonData))
			{
				return;
			}
			this.ProcessMousePress(eventData);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			this.ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessMousePress(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			this.ProcessDrag(mouseData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x00122180 File Offset: 0x00120580
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			this.ProcessMouseEvent(this.GetGazePointerData());
			this.ProcessMouseEvent(this.GetCanvasPointerData());
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x001221D8 File Offset: 0x001205D8
		private static bool UseMouse(bool pressed, bool released, PointerEventData pointerData)
		{
			return pressed || released || OVRInputModule.IsPointerMoving(pointerData) || pointerData.IsScrolling();
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x00122200 File Offset: 0x00120600
		protected void CopyFromTo(OVRPointerEventData from, OVRPointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
			to.worldSpaceRay = from.worldSpaceRay;
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00122255 File Offset: 0x00120655
		protected new void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x00122294 File Offset: 0x00120694
		protected bool GetPointerData(int id, out OVRPointerEventData data, bool create)
		{
			if (!this.m_VRRayPointerData.TryGetValue(id, out data) && create)
			{
				data = new OVRPointerEventData(base.eventSystem)
				{
					pointerId = id
				};
				this.m_VRRayPointerData.Add(id, data);
				return true;
			}
			return false;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x001222E0 File Offset: 0x001206E0
		protected new void ClearSelection()
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			foreach (PointerEventData currentPointerData in this.m_PointerData.Values)
			{
				base.HandlePointerExitAndEnter(currentPointerData, null);
			}
			foreach (OVRPointerEventData currentPointerData2 in this.m_VRRayPointerData.Values)
			{
				base.HandlePointerExitAndEnter(currentPointerData2, null);
			}
			this.m_PointerData.Clear();
			base.eventSystem.SetSelectedGameObject(null, baseEventData);
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x001223B4 File Offset: 0x001207B4
		private static Vector3 GetRectTransformNormal(RectTransform rectTransform)
		{
			Vector3[] array = new Vector3[4];
			rectTransform.GetWorldCorners(array);
			Vector3 lhs = array[3] - array[0];
			Vector3 rhs = array[1] - array[0];
			rectTransform.GetWorldCorners(array);
			return Vector3.Cross(lhs, rhs).normalized;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x00122424 File Offset: 0x00120824
		protected virtual PointerInputModule.MouseState GetGazePointerData()
		{
			OVRPointerEventData ovrpointerEventData;
			this.GetPointerData(-1, out ovrpointerEventData, true);
			ovrpointerEventData.Reset();
			ovrpointerEventData.worldSpaceRay = new Ray(this.rayTransform.position, this.rayTransform.forward);
			ovrpointerEventData.scrollDelta = this.GetExtraScrollDelta();
			ovrpointerEventData.button = PointerEventData.InputButton.Left;
			ovrpointerEventData.useDragThreshold = true;
			base.eventSystem.RaycastAll(ovrpointerEventData, this.m_RaycastResultCache);
			RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			ovrpointerEventData.pointerCurrentRaycast = raycastResult;
			this.m_RaycastResultCache.Clear();
			OVRRaycaster ovrraycaster = raycastResult.module as OVRRaycaster;
			if (ovrraycaster)
			{
				ovrpointerEventData.position = ovrraycaster.GetScreenPosition(raycastResult);
				RectTransform component = raycastResult.gameObject.GetComponent<RectTransform>();
				if (component != null)
				{
					Vector3 worldPosition = raycastResult.worldPosition;
					Vector3 rectTransformNormal = OVRInputModule.GetRectTransformNormal(component);
					OVRGazePointer.instance.SetPosition(worldPosition, rectTransformNormal);
					OVRGazePointer.instance.RequestShow();
				}
			}
			OVRPhysicsRaycaster ovrphysicsRaycaster = raycastResult.module as OVRPhysicsRaycaster;
			if (ovrphysicsRaycaster)
			{
				Vector3 worldPosition2 = raycastResult.worldPosition;
				if (this.performSphereCastForGazepointer)
				{
					List<RaycastResult> list = new List<RaycastResult>();
					ovrphysicsRaycaster.Spherecast(ovrpointerEventData, list, OVRGazePointer.instance.GetCurrentRadius());
					if (list.Count > 0 && list[0].distance < raycastResult.distance)
					{
						worldPosition2 = list[0].worldPosition;
					}
				}
				ovrpointerEventData.position = ovrphysicsRaycaster.GetScreenPos(raycastResult.worldPosition);
				OVRGazePointer.instance.RequestShow();
				if (this.matchNormalOnPhysicsColliders)
				{
					OVRGazePointer.instance.SetPosition(worldPosition2, raycastResult.worldNormal);
				}
				else
				{
					OVRGazePointer.instance.SetPosition(worldPosition2);
				}
			}
			OVRPointerEventData ovrpointerEventData2;
			this.GetPointerData(-2, out ovrpointerEventData2, true);
			this.CopyFromTo(ovrpointerEventData, ovrpointerEventData2);
			ovrpointerEventData2.button = PointerEventData.InputButton.Right;
			OVRPointerEventData ovrpointerEventData3;
			this.GetPointerData(-3, out ovrpointerEventData3, true);
			this.CopyFromTo(ovrpointerEventData, ovrpointerEventData3);
			ovrpointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, this.GetGazeButtonState(), ovrpointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, PointerEventData.FramePressState.NotChanged, ovrpointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, PointerEventData.FramePressState.NotChanged, ovrpointerEventData3);
			return this.m_MouseState;
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x00122658 File Offset: 0x00120A58
		protected PointerInputModule.MouseState GetCanvasPointerData()
		{
			PointerEventData pointerEventData;
			base.GetPointerData(-1, out pointerEventData, true);
			pointerEventData.Reset();
			pointerEventData.position = Vector2.zero;
			pointerEventData.scrollDelta = Input.mouseScrollDelta;
			pointerEventData.button = PointerEventData.InputButton.Left;
			if (this.activeGraphicRaycaster)
			{
				this.activeGraphicRaycaster.RaycastPointer(pointerEventData, this.m_RaycastResultCache);
				RaycastResult raycastResult = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
				pointerEventData.pointerCurrentRaycast = raycastResult;
				this.m_RaycastResultCache.Clear();
				OVRRaycaster ovrraycaster = raycastResult.module as OVRRaycaster;
				if (ovrraycaster)
				{
					Vector2 screenPosition = ovrraycaster.GetScreenPosition(raycastResult);
					pointerEventData.delta = screenPosition - pointerEventData.position;
					pointerEventData.position = screenPosition;
				}
			}
			PointerEventData pointerEventData2;
			base.GetPointerData(-2, out pointerEventData2, true);
			this.CopyFromTo(pointerEventData, pointerEventData2);
			pointerEventData2.button = PointerEventData.InputButton.Right;
			PointerEventData pointerEventData3;
			base.GetPointerData(-3, out pointerEventData3, true);
			this.CopyFromTo(pointerEventData, pointerEventData3);
			pointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, base.StateForMouseButton(0), pointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, base.StateForMouseButton(1), pointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, base.StateForMouseButton(2), pointerEventData3);
			return this.m_MouseState;
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x0012278C File Offset: 0x00120B8C
		private bool ShouldStartDrag(PointerEventData pointerEvent)
		{
			if (!pointerEvent.useDragThreshold)
			{
				return true;
			}
			if (!pointerEvent.IsVRPointer())
			{
				return (pointerEvent.pressPosition - pointerEvent.position).sqrMagnitude >= (float)(base.eventSystem.pixelDragThreshold * base.eventSystem.pixelDragThreshold);
			}
			Vector3 position = pointerEvent.pressEventCamera.transform.position;
			Vector3 normalized = (pointerEvent.pointerPressRaycast.worldPosition - position).normalized;
			Vector3 normalized2 = (pointerEvent.pointerCurrentRaycast.worldPosition - position).normalized;
			return Vector3.Dot(normalized, normalized2) < Mathf.Cos(0.017453292f * this.angleDragThreshold);
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x00122852 File Offset: 0x00120C52
		private static bool IsPointerMoving(PointerEventData pointerEvent)
		{
			return pointerEvent.IsVRPointer() || pointerEvent.IsPointerMoving();
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x00122867 File Offset: 0x00120C67
		protected Vector2 SwipeAdjustedPosition(Vector2 originalPosition, PointerEventData pointerEvent)
		{
			return originalPosition;
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x0012286C File Offset: 0x00120C6C
		protected override void ProcessDrag(PointerEventData pointerEvent)
		{
			Vector2 position = pointerEvent.position;
			bool flag = OVRInputModule.IsPointerMoving(pointerEvent);
			if (flag && pointerEvent.pointerDrag != null && !pointerEvent.dragging && this.ShouldStartDrag(pointerEvent))
			{
				if (pointerEvent.IsVRPointer())
				{
					pointerEvent.position = this.SwipeAdjustedPosition(position, pointerEvent);
				}
				ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (pointerEvent.dragging && flag && pointerEvent.pointerDrag != null)
			{
				if (pointerEvent.IsVRPointer())
				{
					pointerEvent.position = this.SwipeAdjustedPosition(position, pointerEvent);
				}
				if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
				{
					ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
					pointerEvent.eligibleForClick = false;
					pointerEvent.pointerPress = null;
					pointerEvent.rawPointerPress = null;
				}
				ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
			}
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00122974 File Offset: 0x00120D74
		protected virtual PointerEventData.FramePressState GetGazeButtonState()
		{
			bool flag = Input.GetKeyDown(this.gazeClickKey) || OVRInput.GetDown(this.joyPadClickButton, OVRInput.Controller.Active);
			bool flag2 = Input.GetKeyUp(this.gazeClickKey) || OVRInput.GetUp(this.joyPadClickButton, OVRInput.Controller.Active);
			if (flag && flag2)
			{
				return PointerEventData.FramePressState.PressedAndReleased;
			}
			if (flag)
			{
				return PointerEventData.FramePressState.Pressed;
			}
			if (flag2)
			{
				return PointerEventData.FramePressState.Released;
			}
			return PointerEventData.FramePressState.NotChanged;
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x001229E8 File Offset: 0x00120DE8
		protected Vector2 GetExtraScrollDelta()
		{
			Vector2 result = default(Vector2);
			if (this.useRightStickScroll)
			{
				Vector2 vector = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active);
				if (Mathf.Abs(vector.x) < this.rightStickDeadZone)
				{
					vector.x = 0f;
				}
				if (Mathf.Abs(vector.y) < this.rightStickDeadZone)
				{
					vector.y = 0f;
				}
				result = vector;
			}
			return result;
		}

		// Token: 0x04002DDD RID: 11741
		[Tooltip("Object which points with Z axis. E.g. CentreEyeAnchor from OVRCameraRig")]
		public Transform rayTransform;

		// Token: 0x04002DDE RID: 11742
		[Tooltip("Gamepad button to act as gaze click")]
		public OVRInput.Button joyPadClickButton = OVRInput.Button.One;

		// Token: 0x04002DDF RID: 11743
		[Tooltip("Keyboard button to act as gaze click")]
		public KeyCode gazeClickKey = KeyCode.Space;

		// Token: 0x04002DE0 RID: 11744
		[Header("Physics")]
		[Tooltip("Perform an sphere cast to determine correct depth for gaze pointer")]
		public bool performSphereCastForGazepointer;

		// Token: 0x04002DE1 RID: 11745
		[Tooltip("Match the gaze pointer normal to geometry normal for physics colliders")]
		public bool matchNormalOnPhysicsColliders;

		// Token: 0x04002DE2 RID: 11746
		[Header("Gamepad Stick Scroll")]
		[Tooltip("Enable scrolling with the right stick on a gamepad")]
		public bool useRightStickScroll = true;

		// Token: 0x04002DE3 RID: 11747
		[Tooltip("Deadzone for right stick to prevent accidental scrolling")]
		public float rightStickDeadZone = 0.15f;

		// Token: 0x04002DE4 RID: 11748
		[Header("Touchpad Swipe Scroll")]
		[Tooltip("Enable scrolling by swiping the GearVR touchpad")]
		public bool useSwipeScroll = true;

		// Token: 0x04002DE5 RID: 11749
		[Tooltip("Minimum trackpad movement in pixels to start swiping")]
		public float swipeDragThreshold = 2f;

		// Token: 0x04002DE6 RID: 11750
		[Tooltip("Distance scrolled when swipe scroll occurs")]
		public float swipeDragScale = 1f;

		// Token: 0x04002DE7 RID: 11751
		[Tooltip("Invert X axis on touchpad")]
		public bool InvertSwipeXAxis;

		// Token: 0x04002DE8 RID: 11752
		[NonSerialized]
		public OVRRaycaster activeGraphicRaycaster;

		// Token: 0x04002DE9 RID: 11753
		[Header("Dragging")]
		[Tooltip("Minimum pointer movement in degrees to start dragging")]
		public float angleDragThreshold = 1f;

		// Token: 0x04002DEA RID: 11754
		private float m_NextAction;

		// Token: 0x04002DEB RID: 11755
		private Vector2 m_LastMousePosition;

		// Token: 0x04002DEC RID: 11756
		private Vector2 m_MousePosition;

		// Token: 0x04002DED RID: 11757
		[Header("Standalone Input Module")]
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04002DEE RID: 11758
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04002DEF RID: 11759
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04002DF0 RID: 11760
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04002DF1 RID: 11761
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04002DF2 RID: 11762
		[SerializeField]
		private bool m_AllowActivationOnMobileDevice;

		// Token: 0x04002DF3 RID: 11763
		protected Dictionary<int, OVRPointerEventData> m_VRRayPointerData = new Dictionary<int, OVRPointerEventData>();

		// Token: 0x04002DF4 RID: 11764
		private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

		// Token: 0x02000966 RID: 2406
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public enum InputMode
		{
			// Token: 0x04002DF6 RID: 11766
			Mouse,
			// Token: 0x04002DF7 RID: 11767
			Buttons
		}
	}
}
