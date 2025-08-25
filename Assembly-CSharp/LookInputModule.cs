using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using Weelco.VRKeyboard;

// Token: 0x02000E25 RID: 3621
public class LookInputModule : StandaloneInputModule
{
	// Token: 0x06006F68 RID: 28520 RVA: 0x0029E3C4 File Offset: 0x0029C7C4
	public LookInputModule()
	{
	}

	// Token: 0x06006F69 RID: 28521 RVA: 0x0029E48C File Offset: 0x0029C88C
	private static void Execute(LookInputModule.IPointerMoveHandler handler, BaseEventData eventData)
	{
		handler.OnPointerMove(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
	}

	// Token: 0x17001045 RID: 4165
	// (get) Token: 0x06006F6A RID: 28522 RVA: 0x0029E49A File Offset: 0x0029C89A
	public static ExecuteEvents.EventFunction<LookInputModule.IPointerMoveHandler> pointerMoveHandler
	{
		get
		{
			if (LookInputModule.<>f__mg$cache0 == null)
			{
				LookInputModule.<>f__mg$cache0 = new ExecuteEvents.EventFunction<LookInputModule.IPointerMoveHandler>(LookInputModule.Execute);
			}
			return LookInputModule.<>f__mg$cache0;
		}
	}

	// Token: 0x17001046 RID: 4166
	// (get) Token: 0x06006F6B RID: 28523 RVA: 0x0029E4B9 File Offset: 0x0029C8B9
	public static LookInputModule singleton
	{
		get
		{
			return LookInputModule._singleton;
		}
	}

	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x06006F6C RID: 28524 RVA: 0x0029E4C0 File Offset: 0x0029C8C0
	public bool guiRaycastHit
	{
		get
		{
			return this._guiRaycastHit;
		}
	}

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x06006F6D RID: 28525 RVA: 0x0029E4C8 File Offset: 0x0029C8C8
	public bool mouseRaycastHit
	{
		get
		{
			return this._mouseRaycastHit;
		}
	}

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x06006F6E RID: 28526 RVA: 0x0029E4D0 File Offset: 0x0029C8D0
	public bool controlAxisUsed
	{
		get
		{
			return this._controlAxisUsed;
		}
	}

	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x06006F6F RID: 28527 RVA: 0x0029E4D8 File Offset: 0x0029C8D8
	public bool inputFieldActive
	{
		get
		{
			if (this.currentInputField != null)
			{
				return true;
			}
			if (this.currentKeyEventHandler != null)
			{
				return true;
			}
			if (this.cachedCurrentSelectedGameObject != base.eventSystem.currentSelectedGameObject)
			{
				this.cachedCurrentSelectedGameObject = base.eventSystem.currentSelectedGameObject;
				if (this.cachedCurrentSelectedGameObject != null)
				{
					this.cachedCurrentSelectedGameObjectInputField = this.cachedCurrentSelectedGameObject.GetComponent<InputField>();
				}
				else
				{
					this.cachedCurrentSelectedGameObjectInputField = null;
				}
			}
			return this.cachedCurrentSelectedGameObjectInputField != null;
		}
	}

	// Token: 0x06006F70 RID: 28528 RVA: 0x0029E574 File Offset: 0x0029C974
	private PointerEventData GetLookPointerEventData(bool isRight = false)
	{
		Vector2 position;
		if (this.referenceCamera != null)
		{
			position.x = (float)(this.referenceCamera.pixelWidth / 2);
			position.y = (float)(this.referenceCamera.pixelHeight / 2);
		}
		else
		{
			position.x = (float)(XRSettings.eyeTextureWidth / 2);
			position.y = (float)(XRSettings.eyeTextureHeight / 2);
		}
		if (isRight)
		{
			if (this.lookDataRight == null)
			{
				this.lookDataRight = new PointerEventData(base.eventSystem);
			}
			this.lookDataRight.Reset();
			this.lookDataRight.delta = Vector2.zero;
			this.lookDataRight.scrollDelta = Vector2.zero;
			this.lookDataRight.position = position;
			this.m_RaycastResultCacheRight.Clear();
			base.eventSystem.RaycastAll(this.lookDataRight, this.m_RaycastResultCacheRight);
			this.lookDataRight.pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCacheRight);
			if (this.lookDataRight.pointerCurrentRaycast.gameObject != null)
			{
				this._guiRaycastHit = true;
			}
			else
			{
				this._guiRaycastHit = false;
			}
			this.m_RaycastResultCacheRight.Clear();
			return this.lookDataRight;
		}
		if (this.lookData == null)
		{
			this.lookData = new PointerEventData(base.eventSystem);
		}
		this.lookData.Reset();
		this.lookData.delta = Vector2.zero;
		this.lookData.scrollDelta = Vector2.zero;
		this.lookData.position = position;
		this.m_RaycastResultCache.Clear();
		base.eventSystem.RaycastAll(this.lookData, this.m_RaycastResultCache);
		this.lookData.pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
		if (this.lookData.pointerCurrentRaycast.gameObject != null)
		{
			this._guiRaycastHit = true;
		}
		else
		{
			this._guiRaycastHit = false;
		}
		this.m_RaycastResultCache.Clear();
		return this.lookData;
	}

	// Token: 0x06006F71 RID: 28529 RVA: 0x0029E780 File Offset: 0x0029CB80
	private void UpdateCursor(PointerEventData lookData, RectTransform curs)
	{
		if (curs != null)
		{
			if (this.useCursor)
			{
				if (lookData.pointerEnter != null)
				{
					RectTransform component = lookData.pointerEnter.GetComponent<RectTransform>();
					Vector3 vector;
					if (RectTransformUtility.ScreenPointToWorldPointInRectangle(component, lookData.position, lookData.enterEventCamera, out vector))
					{
						curs.gameObject.SetActive(true);
						curs.position = vector;
						curs.rotation = component.rotation;
						if (this.scaleCursorWithDistance)
						{
							float num = (vector - lookData.enterEventCamera.transform.position).magnitude / this.worldScale;
							float num2 = num * this.normalCursorScale;
							if (num2 < this.normalCursorScale)
							{
								num2 = this.normalCursorScale;
							}
							Vector3 localScale;
							localScale.x = num2;
							localScale.y = num2;
							localScale.z = num2;
							curs.localScale = localScale;
						}
					}
					else
					{
						curs.gameObject.SetActive(false);
					}
				}
				else
				{
					curs.gameObject.SetActive(false);
				}
			}
			else
			{
				curs.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06006F72 RID: 28530 RVA: 0x0029E8A0 File Offset: 0x0029CCA0
	private void SetSelectedColor(GameObject go)
	{
		if (this.useSelectColor)
		{
			if (!this.useSelectColorOnButton && go.GetComponent<Button>())
			{
				this.currentSelectedNormalColorValid = false;
				return;
			}
			if (!this.useSelectColorOnToggle && go.GetComponent<Toggle>())
			{
				this.currentSelectedNormalColorValid = false;
				return;
			}
			Selectable component = go.GetComponent<Selectable>();
			if (component != null)
			{
				ColorBlock colors = component.colors;
				this.currentSelectedNormalColor = colors.normalColor;
				this.currentSelectedNormalColorValid = true;
				this.currentSelectedHighlightedColor = colors.highlightedColor;
				colors.normalColor = this.selectColor;
				colors.highlightedColor = this.selectColor;
				component.colors = colors;
			}
		}
	}

	// Token: 0x06006F73 RID: 28531 RVA: 0x0029E95C File Offset: 0x0029CD5C
	private void RestoreColor(GameObject go)
	{
		if (this.useSelectColor && this.currentSelectedNormalColorValid)
		{
			Selectable component = go.GetComponent<Selectable>();
			if (component != null)
			{
				ColorBlock colors = component.colors;
				colors.normalColor = this.currentSelectedNormalColor;
				colors.highlightedColor = this.currentSelectedHighlightedColor;
				component.colors = colors;
			}
		}
	}

	// Token: 0x06006F74 RID: 28532 RVA: 0x0029E9BC File Offset: 0x0029CDBC
	private void ClearKeyboardInput()
	{
		this.currentInputField = null;
		this.currentKeyEventHandler = null;
		if (this.keyboardTransform != null)
		{
			this.keyboardTransform.gameObject.SetActive(false);
		}
		if (this.currentKeyboardTransform != null)
		{
			this.currentKeyboardTransform.gameObject.SetActive(false);
			this.currentKeyboardTransform = null;
		}
	}

	// Token: 0x06006F75 RID: 28533 RVA: 0x0029EA24 File Offset: 0x0029CE24
	public new void ClearSelection()
	{
		if (base.eventSystem.currentSelectedGameObject)
		{
			if (this.controlCopy != null)
			{
				UnityEngine.Object.Destroy(this.controlCopy);
				this.controlCopy = null;
			}
			this.RestoreColor(base.eventSystem.currentSelectedGameObject);
			base.eventSystem.SetSelectedGameObject(null);
		}
		this.ClearKeyboardInput();
	}

	// Token: 0x06006F76 RID: 28534 RVA: 0x0029EA8C File Offset: 0x0029CE8C
	public void Select(GameObject go)
	{
		if (go != null)
		{
			if (ExecuteEvents.GetEventHandler<ISelectHandler>(go) && !go.GetComponent<IgnoreSelect>())
			{
				this.ClearKeyboardInput();
				if (this.anchorForControlCopy != null)
				{
					Slider component = go.GetComponent<Slider>();
					UIPopup component2 = go.GetComponent<UIPopup>();
					Transform transform = null;
					if (component != null)
					{
						transform = component.transform.parent;
					}
					else if (component2 != null)
					{
						transform = component2.transform;
					}
					if (transform != null)
					{
						RectTransform component3 = transform.GetComponent<RectTransform>();
						Rect rect = component3.rect;
						Vector2 sizeDelta;
						sizeDelta.x = rect.width;
						sizeDelta.y = rect.height;
						this.anchorForControlCopy.sizeDelta = sizeDelta;
						this.controlCopy = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject);
						this.controlCopy.transform.SetParent(this.anchorForControlCopy);
						this.controlCopy.transform.localRotation = Quaternion.identity;
						this.controlCopy.transform.localPosition = Vector3.zero;
						this.controlCopy.transform.localScale = Vector3.one;
						RectTransform component4 = this.controlCopy.GetComponent<RectTransform>();
						component4.offsetMin = component3.offsetMin;
						component4.offsetMax = component3.offsetMax;
						component4.anchoredPosition = Vector3.zero;
						if (component != null)
						{
							Slider componentInChildren = this.controlCopy.GetComponentInChildren<Slider>();
							if (componentInChildren != null)
							{
								SliderTrack sliderTrack = componentInChildren.gameObject.AddComponent<SliderTrack>();
								sliderTrack.master = component;
							}
						}
						else if (component2 != null)
						{
							UIPopup component5 = this.controlCopy.GetComponent<UIPopup>();
							if (component5 != null)
							{
								UIPopupTrack uipopupTrack = component5.gameObject.AddComponent<UIPopupTrack>();
								uipopupTrack.master = component2;
							}
						}
					}
				}
				this.SetSelectedColor(go);
				if (base.eventSystem.currentSelectedGameObject == null || base.eventSystem.currentSelectedGameObject != go)
				{
					base.eventSystem.SetSelectedGameObject(go);
				}
				this.currentInputField = go.GetComponent<InputField>();
				if (this.currentInputField != null)
				{
					VRKeyboardLink componentInParent = this.currentInputField.GetComponentInParent<VRKeyboardLink>();
					if (componentInParent != null)
					{
						this.currentKeyboardTransform = componentInParent.VRKeyboardTransformToUse;
					}
					if (this.currentKeyboardTransform != null)
					{
						this.currentKeyboardTransform.gameObject.SetActive(true);
					}
					else if (this.keyboardTransform != null)
					{
						this.keyboardTransform.gameObject.SetActive(true);
					}
				}
				Component[] components = go.GetComponents<Component>();
				foreach (Component component6 in components)
				{
					if (component6 is KeyEventHandler)
					{
						this.currentKeyEventHandler = (component6 as KeyEventHandler);
						break;
					}
				}
				if (this.currentKeyEventHandler != null && this.keyboardTransform != null)
				{
					this.keyboardTransform.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			this.ClearSelection();
		}
	}

	// Token: 0x06006F77 RID: 28535 RVA: 0x0029EDC9 File Offset: 0x0029D1C9
	public static void SelectGameObject(GameObject go)
	{
		if (LookInputModule._singleton != null)
		{
			LookInputModule._singleton.Select(go);
		}
	}

	// Token: 0x06006F78 RID: 28536 RVA: 0x0029EDE8 File Offset: 0x0029D1E8
	private new bool SendUpdateEventToSelectedObject()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return false;
		}
		BaseEventData baseEventData = this.GetBaseEventData();
		ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
		return baseEventData.used;
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x0029EE34 File Offset: 0x0029D234
	public bool AxisControllerSelected()
	{
		if (base.eventSystem.currentSelectedGameObject == null)
		{
			return false;
		}
		Slider component = base.eventSystem.currentSelectedGameObject.GetComponent<Slider>();
		if (component != null)
		{
			return true;
		}
		Scrollbar component2 = base.eventSystem.currentSelectedGameObject.GetComponent<Scrollbar>();
		if (component2 != null)
		{
			return true;
		}
		UIPopupButton component3 = base.eventSystem.currentSelectedGameObject.GetComponent<UIPopupButton>();
		UIPopup x;
		if (component3 != null)
		{
			x = component3.popupParent;
		}
		else
		{
			x = base.eventSystem.currentSelectedGameObject.GetComponent<UIPopup>();
		}
		return x != null;
	}

	// Token: 0x06006F7A RID: 28538 RVA: 0x0029EEE0 File Offset: 0x0029D2E0
	protected void HandleAxis(float newVal)
	{
		if (base.eventSystem.currentSelectedGameObject != null)
		{
			if (this.useSmoothAxis)
			{
				Slider component = base.eventSystem.currentSelectedGameObject.GetComponent<Slider>();
				if (component != null)
				{
					float num = component.maxValue - component.minValue;
					component.value += newVal * this.smoothAxisMultiplier * num;
					this._controlAxisUsed = true;
				}
				else
				{
					Scrollbar component2 = base.eventSystem.currentSelectedGameObject.GetComponent<Scrollbar>();
					if (component2 != null)
					{
						component2.value += newVal * this.smoothAxisMultiplier;
						this._controlAxisUsed = true;
					}
					else
					{
						UIPopupButton component3 = base.eventSystem.currentSelectedGameObject.GetComponent<UIPopupButton>();
						UIPopup uipopup;
						if (component3 != null)
						{
							uipopup = component3.popupParent;
						}
						else
						{
							uipopup = base.eventSystem.currentSelectedGameObject.GetComponent<UIPopup>();
						}
						if (uipopup != null)
						{
							this._controlAxisUsed = true;
							if (this.axisAccumulation > 0.3f)
							{
								uipopup.SetNextValue();
								this.axisAccumulation = 0f;
							}
							else if (this.axisAccumulation < -0.3f)
							{
								uipopup.SetPreviousValue();
								this.axisAccumulation = 0f;
							}
						}
						else
						{
							this._controlAxisUsed = false;
						}
					}
				}
			}
			else
			{
				this._controlAxisUsed = true;
				float unscaledTime = Time.unscaledTime;
				if (unscaledTime > this.nextAxisActionTime)
				{
					this.nextAxisActionTime = unscaledTime + 1f / this.steppedAxisStepsPerSecond;
					AxisEventData axisEventData = this.GetAxisEventData(newVal, 0f, 0f);
					if (!ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler))
					{
						this._controlAxisUsed = false;
					}
				}
			}
		}
		if (this.useSmoothAxis && !this._controlAxisUsed && UITabSelector.activeTabSelector != null)
		{
			this._controlAxisUsed = true;
			if (this.axisAccumulation > 0.6f)
			{
				UITabSelector.activeTabSelector.SelectNextTab();
				this.axisAccumulation = 0f;
			}
			else if (this.axisAccumulation < -0.6f)
			{
				UITabSelector.activeTabSelector.SelectPreviousTab();
				this.axisAccumulation = 0f;
			}
		}
	}

	// Token: 0x06006F7B RID: 28539 RVA: 0x0029F126 File Offset: 0x0029D526
	protected bool GetSubmitLeftButtonDown()
	{
		if (OVRManager.isHmdPresent)
		{
			return OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.Touch);
		}
		return this.interactAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06006F7C RID: 28540 RVA: 0x0029F146 File Offset: 0x0029D546
	protected bool GetSubmitLeftButtonUp()
	{
		if (OVRManager.isHmdPresent)
		{
			return OVRInput.GetUp(OVRInput.Button.Three, OVRInput.Controller.Touch);
		}
		return this.interactAction.GetStateUp(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06006F7D RID: 28541 RVA: 0x0029F166 File Offset: 0x0029D566
	protected bool GetSubmitRightButtonDown()
	{
		if (OVRManager.isHmdPresent)
		{
			return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch);
		}
		return this.interactAction.GetStateDown(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06006F7E RID: 28542 RVA: 0x0029F186 File Offset: 0x0029D586
	protected bool GetSubmitRightButtonUp()
	{
		if (OVRManager.isHmdPresent)
		{
			return OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.Touch);
		}
		return this.interactAction.GetStateUp(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06006F7F RID: 28543 RVA: 0x0029F1A8 File Offset: 0x0029D5A8
	public void ProcessMain()
	{
		this.SendUpdateEventToSelectedObject();
		PointerEventData lookPointerEventData = this.GetLookPointerEventData(false);
		this.currentLook = lookPointerEventData.pointerCurrentRaycast.gameObject;
		if (this.deselectWhenLookAway && this.currentLook == null && this.currentLookRight == null)
		{
			this.ClearSelection();
		}
		base.HandlePointerExitAndEnter(lookPointerEventData, this.currentLook);
		this.UpdateCursor(lookPointerEventData, this.cursor);
		if ((!this.ignoreInputsWhenLookAway || (this.ignoreInputsWhenLookAway && this.currentLook != null)) && (this.GetSubmitLeftButtonDown() || (this.useEitherControllerButtons && this.GetSubmitRightButtonDown())))
		{
			lookPointerEventData.pressPosition = lookPointerEventData.position;
			lookPointerEventData.pointerPressRaycast = lookPointerEventData.pointerCurrentRaycast;
			lookPointerEventData.pointerPress = null;
			if (this.currentLook != null)
			{
				if (this.mode == LookInputModule.Mode.Pointer)
				{
					GameObject gameObject = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(this.currentLook, lookPointerEventData, ExecuteEvents.pointerDownHandler);
					if (gameObject == null)
					{
						gameObject = ExecuteEvents.ExecuteHierarchy<IPointerClickHandler>(this.currentLook, lookPointerEventData, ExecuteEvents.pointerClickHandler);
						if (gameObject == null)
						{
							GameObject pointerDrag = ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(this.currentLook, lookPointerEventData, ExecuteEvents.beginDragHandler);
							lookPointerEventData.pointerDrag = pointerDrag;
							this.currentDragging = pointerDrag;
						}
					}
					else
					{
						this.currentPressed = gameObject;
						ExecuteEvents.Execute<IPointerClickHandler>(gameObject, lookPointerEventData, ExecuteEvents.pointerClickHandler);
					}
					if (gameObject != null)
					{
						lookPointerEventData.pointerPress = gameObject;
						this.Select(gameObject);
						SliderControl component = gameObject.GetComponent<SliderControl>();
						if (component == null || !component.disableLookDrag)
						{
							ExecuteEvents.Execute<IBeginDragHandler>(gameObject, lookPointerEventData, ExecuteEvents.beginDragHandler);
							lookPointerEventData.pointerDrag = gameObject;
							this.currentDragging = gameObject;
						}
					}
				}
				else if (this.mode == LookInputModule.Mode.Submit)
				{
					GameObject gameObject = ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(this.currentPressed, lookPointerEventData, ExecuteEvents.submitHandler);
					if (gameObject == null)
					{
						gameObject = ExecuteEvents.ExecuteHierarchy<ISelectHandler>(this.currentPressed, lookPointerEventData, ExecuteEvents.selectHandler);
						if (gameObject != null)
						{
							lookPointerEventData.pointerPress = gameObject;
							this.currentPressed = gameObject;
							this.Select(gameObject);
						}
					}
				}
			}
		}
		if (this.GetSubmitLeftButtonUp() || (this.useEitherControllerButtons && this.GetSubmitRightButtonUp()))
		{
			if (this.currentDragging)
			{
				ExecuteEvents.Execute<IEndDragHandler>(this.currentDragging, lookPointerEventData, ExecuteEvents.endDragHandler);
				if (this.currentLook != null)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(this.currentLook, lookPointerEventData, ExecuteEvents.dropHandler);
				}
				lookPointerEventData.pointerDrag = null;
				this.currentDragging = null;
			}
			if (this.currentPressed)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(this.currentPressed, lookPointerEventData, ExecuteEvents.pointerUpHandler);
				lookPointerEventData.rawPointerPress = null;
				lookPointerEventData.pointerPress = null;
				this.currentPressed = null;
			}
		}
		if (this.currentDragging != null)
		{
			ExecuteEvents.Execute<IDragHandler>(this.currentDragging, lookPointerEventData, ExecuteEvents.dragHandler);
		}
		if (this.currentLook != null)
		{
			ExecuteEvents.Execute<LookInputModule.IPointerMoveHandler>(this.currentLook, lookPointerEventData, LookInputModule.pointerMoveHandler);
		}
		if (!OVRManager.isHmdPresent)
		{
			Vector2 axis = this.scrollAction.GetAxis(SteamVR_Input_Sources.LeftHand);
			if (this.currentLook != null && !Mathf.Approximately(axis.sqrMagnitude, 0f))
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(this.currentLook);
				if (float.IsNaN(axis.x))
				{
					axis.x = 0f;
				}
				if (float.IsNaN(axis.y))
				{
					axis.y = 0f;
				}
				axis.x = Mathf.Clamp(axis.x, -100f, 100f) * 100f;
				axis.y = Mathf.Clamp(axis.y, -100f, 100f) * 100f;
				lookPointerEventData.scrollDelta = axis;
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, lookPointerEventData, ExecuteEvents.scrollHandler);
			}
		}
		if (!this.ignoreInputsWhenLookAway || (this.ignoreInputsWhenLookAway && this.currentLook != null))
		{
			this._controlAxisUsed = false;
			if (base.eventSystem.currentSelectedGameObject || UITabSelector.activeTabSelector)
			{
				this._controlAxisUsed = true;
				float num = JoystickControl.GetAxis(this.controlAxis);
				if (num > 0.01f || num < -0.01f)
				{
					if (!this.axisOn)
					{
						this.axisAccumulation = Mathf.Sign(num);
					}
					else
					{
						this.axisAccumulation += num * Time.deltaTime;
					}
					this.axisOn = true;
					this.HandleAxis(num);
				}
				else
				{
					this.axisOn = false;
				}
				num = JoystickControl.GetAxis(this.discreteControlAxis);
				if (num > 0.01f || num < -0.01f)
				{
					if (this.invertDiscreteControlAxis)
					{
						num = -num;
					}
					if (!this.discreteAxisOn)
					{
						this.axisAccumulation = Mathf.Sign(num);
					}
					else
					{
						this.axisAccumulation += num * Time.deltaTime;
					}
					this.discreteAxisOn = true;
					this.HandleAxis(num);
				}
				else
				{
					this.discreteAxisOn = false;
				}
			}
			else
			{
				this.axisAccumulation = 0f;
			}
		}
	}

	// Token: 0x06006F80 RID: 28544 RVA: 0x0029F708 File Offset: 0x0029DB08
	public void ProcessMouse()
	{
		this.SendUpdateEventToSelectedObject();
		PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData();
		PointerEventData buttonData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData.buttonData;
		this.currentLookMouse = buttonData.pointerCurrentRaycast.gameObject;
		if (this.currentLookMouse != null)
		{
			this._mouseRaycastHit = true;
		}
		else
		{
			this._mouseRaycastHit = false;
		}
		if (this.deselectWhenLookAway && this.currentLookMouse == null && this.currentLook == null && this.currentLookRight == null)
		{
			this.ClearSelection();
		}
		base.HandlePointerExitAndEnter(buttonData, this.currentLookMouse);
		if ((!this.ignoreInputsWhenLookAway || (this.ignoreInputsWhenLookAway && this.currentLookMouse != null)) && Input.GetMouseButtonDown(0))
		{
			buttonData.pressPosition = buttonData.position;
			buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
			buttonData.pointerPress = null;
			if (this.currentLookMouse != null)
			{
				GameObject gameObject = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(this.currentLookMouse, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject == null)
				{
					gameObject = ExecuteEvents.ExecuteHierarchy<IPointerClickHandler>(this.currentLookMouse, buttonData, ExecuteEvents.pointerClickHandler);
					if (gameObject == null)
					{
						GameObject pointerDrag = ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(this.currentLookMouse, buttonData, ExecuteEvents.beginDragHandler);
						buttonData.pointerDrag = pointerDrag;
						this.currentDraggingMouse = pointerDrag;
					}
				}
				else
				{
					this.currentPressedMouse = gameObject;
				}
				if (gameObject != null)
				{
					buttonData.pointerPress = gameObject;
					this.Select(gameObject);
					SliderControl component = gameObject.GetComponent<SliderControl>();
					if (component == null || !component.disableLookDrag)
					{
						ExecuteEvents.Execute<IBeginDragHandler>(gameObject, buttonData, ExecuteEvents.beginDragHandler);
						buttonData.pointerDrag = gameObject;
						this.currentDraggingMouse = gameObject;
					}
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (this.currentDraggingMouse)
			{
				ExecuteEvents.Execute<IEndDragHandler>(this.currentDraggingMouse, buttonData, ExecuteEvents.endDragHandler);
				if (this.currentLookMouse != null)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(this.currentLookMouse, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.pointerDrag = null;
				this.currentDraggingMouse = null;
			}
			if (this.currentPressedMouse)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(this.currentPressedMouse, buttonData, ExecuteEvents.pointerUpHandler);
				ExecuteEvents.Execute<IPointerClickHandler>(this.currentPressedMouse, buttonData, ExecuteEvents.pointerClickHandler);
				buttonData.rawPointerPress = null;
				buttonData.pointerPress = null;
				this.currentPressedMouse = null;
			}
		}
		if (this.currentDraggingMouse != null)
		{
			ExecuteEvents.Execute<IDragHandler>(this.currentDraggingMouse, buttonData, ExecuteEvents.dragHandler);
		}
		if (this.currentLookMouse != null)
		{
			ExecuteEvents.Execute<LookInputModule.IPointerMoveHandler>(this.currentLookMouse, buttonData, LookInputModule.pointerMoveHandler);
		}
		if (buttonData.pointerCurrentRaycast.gameObject != null && !Mathf.Approximately(buttonData.scrollDelta.sqrMagnitude, 0f))
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(buttonData.pointerCurrentRaycast.gameObject);
			Vector2 scrollDelta = buttonData.scrollDelta * 100f;
			buttonData.scrollDelta = scrollDelta;
			ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, buttonData, ExecuteEvents.scrollHandler);
		}
	}

	// Token: 0x06006F81 RID: 28545 RVA: 0x0029FA44 File Offset: 0x0029DE44
	private void ProcessMousePressAlt(PointerInputModule.MouseButtonEventData data)
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
			buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
			if (gameObject != base.eventSystem.currentSelectedGameObject)
			{
				this.ClearKeyboardInput();
			}
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
				this.Select(buttonData.pointerPress);
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

	// Token: 0x06006F82 RID: 28546 RVA: 0x0029FC88 File Offset: 0x0029E088
	public void ProcessMouseAlt(bool useCenterOfCamera = false)
	{
		this.SendUpdateEventToSelectedObject();
		PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData();
		PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
		PointerEventData buttonData = eventData.buttonData;
		if (useCenterOfCamera)
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector2 position;
			position.x = mousePosition.x;
			position.y = mousePosition.y;
			buttonData.delta = Vector2.zero;
			buttonData.position = position;
			this.m_RaycastResultCache.Clear();
			base.eventSystem.RaycastAll(buttonData, this.m_RaycastResultCache);
			buttonData.pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			this.m_RaycastResultCache.Clear();
			eventData.buttonData = buttonData;
			this.currentLookMouse = buttonData.pointerCurrentRaycast.gameObject;
			base.HandlePointerExitAndEnter(buttonData, this.currentLookMouse);
			this.UpdateCursor(buttonData, this.cursorMouse);
		}
		else
		{
			if (this.cursorMouse != null)
			{
				this.cursorMouse.gameObject.SetActive(false);
			}
			this.currentLookMouse = buttonData.pointerCurrentRaycast.gameObject;
		}
		if (this.currentLookMouse != null)
		{
			this._mouseRaycastHit = true;
		}
		else
		{
			this._mouseRaycastHit = false;
		}
		bool pressed = mousePointerEventData.AnyPressesThisFrame();
		bool released = mousePointerEventData.AnyReleasesThisFrame();
		if (!LookInputModule.UseMouse(pressed, released, buttonData) && !useCenterOfCamera)
		{
			return;
		}
		this.ProcessMousePressAlt(eventData);
		this.ProcessMove(buttonData);
		if (this.currentLookMouse != null)
		{
			ExecuteEvents.Execute<LookInputModule.IPointerMoveHandler>(this.currentLookMouse, buttonData, LookInputModule.pointerMoveHandler);
		}
		this.ProcessDrag(buttonData);
		base.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
		this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
		base.ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
		this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
		if (eventData.buttonData.pointerCurrentRaycast.gameObject != null && !Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
			buttonData.scrollDelta *= 100f;
			ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, buttonData, ExecuteEvents.scrollHandler);
		}
	}

	// Token: 0x06006F83 RID: 28547 RVA: 0x0029FEF2 File Offset: 0x0029E2F2
	private static bool UseMouse(bool pressed, bool released, PointerEventData pointerData)
	{
		return pressed || released || pointerData.IsPointerMoving() || pointerData.IsScrolling();
	}

	// Token: 0x06006F84 RID: 28548 RVA: 0x0029FF1C File Offset: 0x0029E31C
	public override void Process()
	{
		LookInputModule._singleton = this;
		if (this.currentInputField != null && !this.currentInputField.gameObject.activeInHierarchy)
		{
			this.ClearKeyboardInput();
		}
		if (!this.disableStandaloneProcess)
		{
			base.Process();
		}
	}

	// Token: 0x06006F85 RID: 28549 RVA: 0x0029FF6C File Offset: 0x0029E36C
	public void HideCursors()
	{
		if (this.cursor != null)
		{
			this.cursor.gameObject.SetActive(false);
		}
		if (this.cursorRight != null)
		{
			this.cursorRight.gameObject.SetActive(false);
		}
		if (this.cursorMouse != null)
		{
			this.cursorMouse.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006F86 RID: 28550 RVA: 0x0029FFE0 File Offset: 0x0029E3E0
	public void ProcessRight()
	{
		LookInputModule._singleton = this;
		this.SendUpdateEventToSelectedObject();
		PointerEventData lookPointerEventData = this.GetLookPointerEventData(true);
		this.currentLookRight = lookPointerEventData.pointerCurrentRaycast.gameObject;
		base.HandlePointerExitAndEnter(lookPointerEventData, this.currentLookRight);
		this.UpdateCursor(lookPointerEventData, this.cursorRight);
		if ((!this.ignoreInputsWhenLookAway || (this.ignoreInputsWhenLookAway && this.currentLookRight != null)) && this.GetSubmitRightButtonDown())
		{
			lookPointerEventData.pressPosition = lookPointerEventData.position;
			lookPointerEventData.pointerPressRaycast = lookPointerEventData.pointerCurrentRaycast;
			lookPointerEventData.pointerPress = null;
			if (this.currentLookRight != null)
			{
				if (this.mode == LookInputModule.Mode.Pointer)
				{
					GameObject gameObject = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(this.currentLookRight, lookPointerEventData, ExecuteEvents.pointerDownHandler);
					if (gameObject == null)
					{
						gameObject = ExecuteEvents.ExecuteHierarchy<IPointerClickHandler>(this.currentLookRight, lookPointerEventData, ExecuteEvents.pointerClickHandler);
						if (gameObject == null)
						{
							GameObject pointerDrag = ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(this.currentLookRight, lookPointerEventData, ExecuteEvents.beginDragHandler);
							lookPointerEventData.pointerDrag = pointerDrag;
							this.currentDraggingRight = pointerDrag;
						}
					}
					else
					{
						this.currentPressedRight = gameObject;
						ExecuteEvents.Execute<IPointerClickHandler>(gameObject, lookPointerEventData, ExecuteEvents.pointerClickHandler);
					}
					if (gameObject != null)
					{
						lookPointerEventData.pointerPress = gameObject;
						this.Select(gameObject);
						SliderControl component = gameObject.GetComponent<SliderControl>();
						if (component == null || !component.disableLookDrag)
						{
							ExecuteEvents.Execute<IBeginDragHandler>(gameObject, lookPointerEventData, ExecuteEvents.beginDragHandler);
							lookPointerEventData.pointerDrag = gameObject;
							this.currentDraggingRight = gameObject;
						}
					}
				}
				else if (this.mode == LookInputModule.Mode.Submit)
				{
					GameObject gameObject = ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(this.currentLookRight, lookPointerEventData, ExecuteEvents.submitHandler);
					if (gameObject == null)
					{
						gameObject = ExecuteEvents.ExecuteHierarchy<ISelectHandler>(this.currentLookRight, lookPointerEventData, ExecuteEvents.selectHandler);
					}
					if (gameObject != null)
					{
						lookPointerEventData.pointerPress = gameObject;
						this.currentPressedRight = gameObject;
						this.Select(gameObject);
					}
				}
			}
		}
		if (this.GetSubmitRightButtonUp())
		{
			if (this.currentDraggingRight)
			{
				ExecuteEvents.Execute<IEndDragHandler>(this.currentDraggingRight, lookPointerEventData, ExecuteEvents.endDragHandler);
				if (this.currentLookRight != null)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(this.currentLookRight, lookPointerEventData, ExecuteEvents.dropHandler);
				}
				lookPointerEventData.pointerDrag = null;
				this.currentDraggingRight = null;
			}
			if (this.currentPressedRight)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(this.currentPressedRight, lookPointerEventData, ExecuteEvents.pointerUpHandler);
				lookPointerEventData.rawPointerPress = null;
				lookPointerEventData.pointerPress = null;
				this.currentPressedRight = null;
			}
		}
		if (this.currentDraggingRight != null)
		{
			ExecuteEvents.Execute<IDragHandler>(this.currentDraggingRight, lookPointerEventData, ExecuteEvents.dragHandler);
		}
		if (this.currentLookRight != null)
		{
			ExecuteEvents.Execute<LookInputModule.IPointerMoveHandler>(this.currentLookRight, lookPointerEventData, LookInputModule.pointerMoveHandler);
		}
		if (!OVRManager.isHmdPresent)
		{
			Vector2 axis = this.scrollAction.GetAxis(SteamVR_Input_Sources.RightHand);
			if (this.currentLookRight != null && !Mathf.Approximately(axis.sqrMagnitude, 0f))
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(this.currentLookRight);
				if (float.IsNaN(axis.x))
				{
					axis.x = 0f;
				}
				if (float.IsNaN(axis.y))
				{
					axis.y = 0f;
				}
				axis.x = Mathf.Clamp(axis.x, -100f, 100f) * 100f;
				axis.y = Mathf.Clamp(axis.y, -100f, 100f) * 100f;
				lookPointerEventData.scrollDelta = axis;
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(eventHandler, lookPointerEventData, ExecuteEvents.scrollHandler);
			}
		}
	}

	// Token: 0x06006F87 RID: 28551 RVA: 0x002A038C File Offset: 0x0029E78C
	protected void AddCharacterEventToList(List<Event> elist, char c)
	{
		elist.Add(new Event
		{
			keyCode = KeyCode.None,
			type = EventType.KeyDown,
			character = c
		});
	}

	// Token: 0x06006F88 RID: 28552 RVA: 0x002A03BC File Offset: 0x0029E7BC
	protected void AddKeyboardEventToList(List<Event> elist, string s)
	{
		Event @event = Event.KeyboardEvent(s);
		@event.type = EventType.KeyDown;
		@event.character = '\0';
		elist.Add(@event);
		@event = Event.KeyboardEvent(s);
		@event.type = EventType.KeyUp;
		@event.character = '\0';
		elist.Add(@event);
	}

	// Token: 0x06006F89 RID: 28553 RVA: 0x002A0404 File Offset: 0x0029E804
	public void OnKeyClick(string value)
	{
		List<Event> list = new List<Event>();
		if (value == "BACK")
		{
			this.AddKeyboardEventToList(list, "\b");
		}
		else if (value == "ENTER")
		{
			this.AddCharacterEventToList(list, '\n');
		}
		else if (value == ".com")
		{
			this.AddCharacterEventToList(list, '.');
			this.AddCharacterEventToList(list, 'c');
			this.AddCharacterEventToList(list, 'o');
			this.AddCharacterEventToList(list, 'm');
		}
		else
		{
			this.AddCharacterEventToList(list, value[0]);
		}
		if (this.currentInputField != null)
		{
			foreach (Event @event in list)
			{
				if (@event.type != EventType.KeyUp)
				{
					if (@event.character == '\n' && this.currentInputField.lineType != InputField.LineType.MultiLineNewline)
					{
						InputFieldAction component = this.currentInputField.GetComponent<InputFieldAction>();
						if (component != null)
						{
							component.Submit();
						}
						else
						{
							GameObject eventHandler = ExecuteEvents.GetEventHandler<ISubmitHandler>(this.currentInputField.gameObject);
							if (eventHandler != null)
							{
								ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(eventHandler, null, ExecuteEvents.submitHandler);
							}
						}
					}
					else
					{
						this.currentInputField.ProcessEvent(@event);
					}
				}
			}
			this.currentInputField.ForceLabelUpdate();
		}
		if (this.currentKeyEventHandler != null)
		{
			foreach (Event ev in list)
			{
				this.currentKeyEventHandler.AddKeyEvent(ev);
			}
		}
	}

	// Token: 0x06006F8A RID: 28554 RVA: 0x002A05E0 File Offset: 0x0029E9E0
	protected override void Awake()
	{
		LookInputModule._singleton = this;
	}

	// Token: 0x06006F8B RID: 28555 RVA: 0x002A05E8 File Offset: 0x0029E9E8
	protected override void Start()
	{
		if (this.keyboardTransform != null)
		{
			this.keyboardTransform.gameObject.SetActive(false);
		}
		foreach (VRKeyboardFull vrkeyboardFull in this.vrKeyboards)
		{
			if (vrkeyboardFull != null)
			{
				vrkeyboardFull.Init();
				VRKeyboardFull vrkeyboardFull2 = vrkeyboardFull;
				vrkeyboardFull2.OnVRKeyboardBtnClick = (VRKeyboardBase.VRKeyboardBtnClick)Delegate.Combine(vrkeyboardFull2.OnVRKeyboardBtnClick, new VRKeyboardBase.VRKeyboardBtnClick(this.OnKeyClick));
			}
		}
		this.HideCursors();
	}

	// Token: 0x06006F8C RID: 28556 RVA: 0x002A0670 File Offset: 0x0029EA70
	protected override void OnDestroy()
	{
		foreach (VRKeyboardFull vrkeyboardFull in this.vrKeyboards)
		{
			if (vrkeyboardFull != null)
			{
				VRKeyboardFull vrkeyboardFull2 = vrkeyboardFull;
				vrkeyboardFull2.OnVRKeyboardBtnClick = (VRKeyboardBase.VRKeyboardBtnClick)Delegate.Remove(vrkeyboardFull2.OnVRKeyboardBtnClick, new VRKeyboardBase.VRKeyboardBtnClick(this.OnKeyClick));
			}
		}
	}

	// Token: 0x040060F7 RID: 24823
	protected List<RaycastResult> m_RaycastResultCacheRight = new List<RaycastResult>();

	// Token: 0x040060F8 RID: 24824
	private static LookInputModule _singleton;

	// Token: 0x040060F9 RID: 24825
	public bool disableStandaloneProcess;

	// Token: 0x040060FA RID: 24826
	public string submitButtonName = "Fire1";

	// Token: 0x040060FB RID: 24827
	public bool useEitherControllerButtons;

	// Token: 0x040060FC RID: 24828
	public SteamVR_Action_Boolean interactAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("UIInteract", false);

	// Token: 0x040060FD RID: 24829
	public SteamVR_Action_Vector2 scrollAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("UIScroll", false);

	// Token: 0x040060FE RID: 24830
	public JoystickControl.Axis controlAxis = JoystickControl.Axis.Triggers;

	// Token: 0x040060FF RID: 24831
	public JoystickControl.Axis discreteControlAxis = JoystickControl.Axis.DPadY;

	// Token: 0x04006100 RID: 24832
	public bool invertDiscreteControlAxis = true;

	// Token: 0x04006101 RID: 24833
	public bool useSmoothAxis = true;

	// Token: 0x04006102 RID: 24834
	public float smoothAxisMultiplier = 0.01f;

	// Token: 0x04006103 RID: 24835
	public float steppedAxisStepsPerSecond = 10f;

	// Token: 0x04006104 RID: 24836
	public float worldScale = 1f;

	// Token: 0x04006105 RID: 24837
	private bool _guiRaycastHit;

	// Token: 0x04006106 RID: 24838
	private bool _mouseRaycastHit;

	// Token: 0x04006107 RID: 24839
	private bool _controlAxisUsed;

	// Token: 0x04006108 RID: 24840
	public LookInputModule.Mode mode;

	// Token: 0x04006109 RID: 24841
	public bool useLookDrag = true;

	// Token: 0x0400610A RID: 24842
	public bool useLookDragSlider = true;

	// Token: 0x0400610B RID: 24843
	public bool useLookDragScrollbar;

	// Token: 0x0400610C RID: 24844
	public bool useCursor = true;

	// Token: 0x0400610D RID: 24845
	public float normalCursorScale = 0.0005f;

	// Token: 0x0400610E RID: 24846
	public bool scaleCursorWithDistance = true;

	// Token: 0x0400610F RID: 24847
	public RectTransform cursor;

	// Token: 0x04006110 RID: 24848
	public RectTransform cursorRight;

	// Token: 0x04006111 RID: 24849
	public RectTransform cursorMouse;

	// Token: 0x04006112 RID: 24850
	public Transform keyboardTransform;

	// Token: 0x04006113 RID: 24851
	public VRKeyboardFull[] vrKeyboards;

	// Token: 0x04006114 RID: 24852
	protected Transform currentKeyboardTransform;

	// Token: 0x04006115 RID: 24853
	public bool useSelectColor = true;

	// Token: 0x04006116 RID: 24854
	public bool useSelectColorOnButton;

	// Token: 0x04006117 RID: 24855
	public bool useSelectColorOnToggle;

	// Token: 0x04006118 RID: 24856
	public Color selectColor = Color.blue;

	// Token: 0x04006119 RID: 24857
	public bool ignoreInputsWhenLookAway = true;

	// Token: 0x0400611A RID: 24858
	public bool deselectWhenLookAway;

	// Token: 0x0400611B RID: 24859
	public RectTransform anchorForControlCopy;

	// Token: 0x0400611C RID: 24860
	protected GameObject controlCopy;

	// Token: 0x0400611D RID: 24861
	public Camera referenceCamera;

	// Token: 0x0400611E RID: 24862
	private PointerEventData lookData;

	// Token: 0x0400611F RID: 24863
	private PointerEventData lookDataRight;

	// Token: 0x04006120 RID: 24864
	private Color currentSelectedNormalColor;

	// Token: 0x04006121 RID: 24865
	private bool currentSelectedNormalColorValid;

	// Token: 0x04006122 RID: 24866
	private Color currentSelectedHighlightedColor;

	// Token: 0x04006123 RID: 24867
	private GameObject currentLook;

	// Token: 0x04006124 RID: 24868
	private GameObject currentLookRight;

	// Token: 0x04006125 RID: 24869
	private GameObject currentLookMouse;

	// Token: 0x04006126 RID: 24870
	private GameObject currentPressed;

	// Token: 0x04006127 RID: 24871
	private GameObject currentPressedRight;

	// Token: 0x04006128 RID: 24872
	private GameObject currentPressedMouse;

	// Token: 0x04006129 RID: 24873
	private GameObject currentDragging;

	// Token: 0x0400612A RID: 24874
	private GameObject currentDraggingRight;

	// Token: 0x0400612B RID: 24875
	private GameObject currentDraggingMouse;

	// Token: 0x0400612C RID: 24876
	private InputField currentInputField;

	// Token: 0x0400612D RID: 24877
	private GameObject cachedCurrentSelectedGameObject;

	// Token: 0x0400612E RID: 24878
	private InputField cachedCurrentSelectedGameObjectInputField;

	// Token: 0x0400612F RID: 24879
	private KeyEventHandler currentKeyEventHandler;

	// Token: 0x04006130 RID: 24880
	private float nextAxisActionTime;

	// Token: 0x04006131 RID: 24881
	protected float axisAccumulation;

	// Token: 0x04006132 RID: 24882
	protected bool axisOn;

	// Token: 0x04006133 RID: 24883
	protected bool discreteAxisOn;

	// Token: 0x04006134 RID: 24884
	[CompilerGenerated]
	private static ExecuteEvents.EventFunction<LookInputModule.IPointerMoveHandler> <>f__mg$cache0;

	// Token: 0x02000E26 RID: 3622
	public interface IPointerMoveHandler : IEventSystemHandler
	{
		// Token: 0x06006F8D RID: 28557
		void OnPointerMove(PointerEventData eventData);
	}

	// Token: 0x02000E27 RID: 3623
	public enum Mode
	{
		// Token: 0x04006136 RID: 24886
		Pointer,
		// Token: 0x04006137 RID: 24887
		Submit
	}
}
