using System;

namespace UnityEngine.EventSystems.Extensions
{
	// Token: 0x02000507 RID: 1287
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("Event/Extensions/Aimer Input Module")]
	public class AimerInputModule : PointerInputModule
	{
		// Token: 0x06002072 RID: 8306 RVA: 0x000BA2B3 File Offset: 0x000B86B3
		protected AimerInputModule()
		{
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000BA2DC File Offset: 0x000B86DC
		public override void ActivateModule()
		{
			StandaloneInputModule component = base.GetComponent<StandaloneInputModule>();
			if (component != null && component.enabled)
			{
				Debug.LogError("Aimer Input Module is incompatible with the StandAloneInputSystem, please remove it from the Event System in this scene or disable it when this module is in use");
			}
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000BA314 File Offset: 0x000B8714
		public override void Process()
		{
			bool buttonDown = Input.GetButtonDown(this.activateAxis);
			bool buttonUp = Input.GetButtonUp(this.activateAxis);
			PointerEventData aimerPointerEventData = this.GetAimerPointerEventData();
			this.ProcessInteraction(aimerPointerEventData, buttonDown, buttonUp);
			if (!buttonUp)
			{
				this.ProcessMove(aimerPointerEventData);
			}
			else
			{
				base.RemovePointerData(aimerPointerEventData);
			}
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000BA364 File Offset: 0x000B8764
		protected virtual PointerEventData GetAimerPointerEventData()
		{
			PointerEventData pointerEventData;
			base.GetPointerData(-2, out pointerEventData, true);
			pointerEventData.Reset();
			pointerEventData.position = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f) + this.aimerOffset;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			return pointerEventData;
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000BA3E4 File Offset: 0x000B87E4
		private void ProcessInteraction(PointerEventData pointer, bool pressed, bool released)
		{
			GameObject gameObject = pointer.pointerCurrentRaycast.gameObject;
			AimerInputModule.objectUnderAimer = ExecuteEvents.GetEventHandler<ISubmitHandler>(gameObject);
			if (pressed)
			{
				pointer.eligibleForClick = true;
				pointer.delta = Vector2.zero;
				pointer.pressPosition = pointer.position;
				pointer.pointerPressRaycast = pointer.pointerCurrentRaycast;
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(gameObject, pointer, ExecuteEvents.submitHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, pointer, ExecuteEvents.pointerDownHandler);
					if (gameObject2 == null)
					{
						gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					}
				}
				else
				{
					pointer.eligibleForClick = false;
				}
				if (gameObject2 != pointer.pointerPress)
				{
					pointer.pointerPress = gameObject2;
					pointer.rawPointerPress = gameObject;
					pointer.clickCount = 0;
				}
				pointer.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointer.pointerDrag != null)
				{
					ExecuteEvents.Execute<IBeginDragHandler>(pointer.pointerDrag, pointer, ExecuteEvents.beginDragHandler);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute<IPointerUpHandler>(pointer.pointerPress, pointer, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointer.pointerPress == eventHandler && pointer.eligibleForClick)
				{
					float unscaledTime = Time.unscaledTime;
					if (unscaledTime - pointer.clickTime < 0.3f)
					{
						pointer.clickCount++;
					}
					else
					{
						pointer.clickCount = 1;
					}
					pointer.clickTime = unscaledTime;
					ExecuteEvents.Execute<IPointerClickHandler>(pointer.pointerPress, pointer, ExecuteEvents.pointerClickHandler);
				}
				else if (pointer.pointerDrag != null)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, pointer, ExecuteEvents.dropHandler);
				}
				pointer.eligibleForClick = false;
				pointer.pointerPress = null;
				pointer.rawPointerPress = null;
				if (pointer.pointerDrag != null)
				{
					ExecuteEvents.Execute<IEndDragHandler>(pointer.pointerDrag, pointer, ExecuteEvents.endDragHandler);
				}
				pointer.pointerDrag = null;
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000BA5C0 File Offset: 0x000B89C0
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
		}

		// Token: 0x04001B38 RID: 6968
		public string activateAxis = "Submit";

		// Token: 0x04001B39 RID: 6969
		public Vector2 aimerOffset = new Vector2(0f, 0f);

		// Token: 0x04001B3A RID: 6970
		public static GameObject objectUnderAimer;
	}
}
