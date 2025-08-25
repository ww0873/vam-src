using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Weelco.VRInput
{
	// Token: 0x0200058E RID: 1422
	public class VRGazeInputModule : VRInputModule
	{
		// Token: 0x060023C7 RID: 9159 RVA: 0x000CF32D File Offset: 0x000CD72D
		public VRGazeInputModule()
		{
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x000CF340 File Offset: 0x000CD740
		public override void AddController(IUIPointer controller)
		{
			if (controller is UIGazePointer)
			{
				this.controllerData.Add(controller as UIGazePointer, new VRInputControllerData());
			}
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000CF363 File Offset: 0x000CD763
		public override void RemoveController(IUIPointer controller)
		{
			if (controller is UIGazePointer)
			{
				this.controllerData.Remove(controller as UIGazePointer);
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x000CF384 File Offset: 0x000CD784
		public override void Process()
		{
			foreach (KeyValuePair<UIGazePointer, VRInputControllerData> keyValuePair in this.controllerData)
			{
				UIGazePointer key = keyValuePair.Key;
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
						ExecuteEvents.ExecuteHierarchy<IPointerUpHandler>(value.currentPressed, value.pointerEvent, ExecuteEvents.pointerUpHandler);
						key.OnExitControl(value.currentPoint);
					}
					if (gameObject != null)
					{
						key.OnEnterControl(gameObject);
					}
				}
				value.currentPoint = gameObject;
				this.currentLook = gameObject;
				if (gameObject == null)
				{
					base.ClearSelection();
				}
				base.HandlePointerExitAndEnter(value.pointerEvent, value.currentPoint);
				Image gazeProgressBar = key.GazeProgressBar;
				float gazeClickTimer = key.GazeClickTimer;
				float gazeClickTimerDelay = key.GazeClickTimerDelay;
				if (this.currentLook != null && gazeClickTimer > 0f)
				{
					bool flag = false;
					if (this.currentLook.transform.gameObject.GetComponent<Button>() != null)
					{
						flag = true;
					}
					if (this.currentLook.transform.parent != null)
					{
						if (this.currentLook.transform.parent.gameObject.GetComponent<Button>() != null)
						{
							flag = true;
						}
						if (this.currentLook.transform.parent.gameObject.GetComponent<Toggle>() != null)
						{
							flag = true;
						}
						if (this.currentLook.transform.parent.gameObject.GetComponent<Slider>() != null)
						{
							flag = true;
						}
						if (this.currentLook.transform.parent.parent != null)
						{
							if (this.currentLook.transform.parent.parent.gameObject.GetComponent<Slider>() != null && this.currentLook.name != "Handle")
							{
								flag = true;
							}
							if (this.currentLook.transform.parent.parent.gameObject.GetComponent<Toggle>() != null)
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						if (this.lastActiveButton == this.currentLook)
						{
							if (gazeProgressBar)
							{
								if (gazeProgressBar.isActiveAndEnabled)
								{
									gazeProgressBar.fillAmount = (Time.realtimeSinceStartup - this.lookTimer) / gazeClickTimer;
								}
								else if (Time.realtimeSinceStartup - this.lookTimer > 0f)
								{
									gazeProgressBar.fillAmount = 0f;
									gazeProgressBar.gameObject.SetActive(true);
									ExecuteEvents.ExecuteHierarchy<IPointerUpHandler>(value.currentPressed, value.pointerEvent, ExecuteEvents.pointerUpHandler);
								}
							}
							if (Time.realtimeSinceStartup - this.lookTimer > gazeClickTimer)
							{
								if (gazeProgressBar)
								{
									gazeProgressBar.gameObject.SetActive(false);
								}
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
								}
								this.lookTimer = Time.realtimeSinceStartup + gazeClickTimer * gazeClickTimerDelay;
							}
						}
						else
						{
							this.lastActiveButton = this.currentLook;
							this.lookTimer = Time.realtimeSinceStartup;
							if (gazeProgressBar && gazeProgressBar.isActiveAndEnabled)
							{
								gazeProgressBar.gameObject.SetActive(false);
							}
						}
					}
					else
					{
						this.lastActiveButton = null;
						if (gazeProgressBar && gazeProgressBar.isActiveAndEnabled)
						{
							gazeProgressBar.gameObject.SetActive(false);
						}
						base.ClearSelection();
					}
				}
				else
				{
					if (gazeProgressBar)
					{
						gazeProgressBar.gameObject.SetActive(false);
					}
					this.lastActiveButton = null;
					base.ClearSelection();
				}
			}
		}

		// Token: 0x04001E28 RID: 7720
		private GameObject lastActiveButton;

		// Token: 0x04001E29 RID: 7721
		private GameObject currentLook;

		// Token: 0x04001E2A RID: 7722
		private float lookTimer;

		// Token: 0x04001E2B RID: 7723
		private Dictionary<UIGazePointer, VRInputControllerData> controllerData = new Dictionary<UIGazePointer, VRInputControllerData>();
	}
}
