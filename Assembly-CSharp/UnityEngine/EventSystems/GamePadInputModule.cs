using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000508 RID: 1288
	[AddComponentMenu("Event/Extensions/GamePad Input Module")]
	public class GamePadInputModule : BaseInputModule
	{
		// Token: 0x06002078 RID: 8312 RVA: 0x000BA5D0 File Offset: 0x000B89D0
		protected GamePadInputModule()
		{
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x000BA625 File Offset: 0x000B8A25
		// (set) Token: 0x0600207A RID: 8314 RVA: 0x000BA62D File Offset: 0x000B8A2D
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

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600207B RID: 8315 RVA: 0x000BA636 File Offset: 0x000B8A36
		// (set) Token: 0x0600207C RID: 8316 RVA: 0x000BA63E File Offset: 0x000B8A3E
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x000BA647 File Offset: 0x000B8A47
		// (set) Token: 0x0600207E RID: 8318 RVA: 0x000BA64F File Offset: 0x000B8A4F
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

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x000BA658 File Offset: 0x000B8A58
		// (set) Token: 0x06002080 RID: 8320 RVA: 0x000BA660 File Offset: 0x000B8A60
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

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06002081 RID: 8321 RVA: 0x000BA669 File Offset: 0x000B8A69
		// (set) Token: 0x06002082 RID: 8322 RVA: 0x000BA671 File Offset: 0x000B8A71
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

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x000BA67A File Offset: 0x000B8A7A
		// (set) Token: 0x06002084 RID: 8324 RVA: 0x000BA682 File Offset: 0x000B8A82
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

		// Token: 0x06002085 RID: 8325 RVA: 0x000BA68C File Offset: 0x000B8A8C
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			bool flag = true;
			flag |= Input.GetButtonDown(this.m_SubmitButton);
			flag |= Input.GetButtonDown(this.m_CancelButton);
			flag |= !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f);
			return flag | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000BA6FC File Offset: 0x000B8AFC
		public override void ActivateModule()
		{
			StandaloneInputModule component = base.GetComponent<StandaloneInputModule>();
			if (component && component.enabled)
			{
				Debug.LogError("StandAloneInputSystem should not be used with the GamePadInputModule, please remove it from the Event System in this scene or disable it when this module is in use");
			}
			base.ActivateModule();
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000BA76C File Offset: 0x000B8B6C
		public override void DeactivateModule()
		{
			base.DeactivateModule();
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000BA774 File Offset: 0x000B8B74
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
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000BA7B4 File Offset: 0x000B8BB4
		protected bool SendSubmitEventToSelectedObject()
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

		// Token: 0x0600208A RID: 8330 RVA: 0x000BA834 File Offset: 0x000B8C34
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

		// Token: 0x0600208B RID: 8331 RVA: 0x000BA900 File Offset: 0x000B8D00
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000BAA34 File Offset: 0x000B8E34
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x04001B3B RID: 6971
		private float m_PrevActionTime;

		// Token: 0x04001B3C RID: 6972
		private Vector2 m_LastMoveVector;

		// Token: 0x04001B3D RID: 6973
		private int m_ConsecutiveMoveCount;

		// Token: 0x04001B3E RID: 6974
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04001B3F RID: 6975
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04001B40 RID: 6976
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04001B41 RID: 6977
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04001B42 RID: 6978
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04001B43 RID: 6979
		[SerializeField]
		private float m_RepeatDelay = 0.1f;
	}
}
