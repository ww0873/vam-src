using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DBA RID: 3514
public class BasicLookInputModule : BaseInputModule
{
	// Token: 0x06006CF2 RID: 27890 RVA: 0x00290FB8 File Offset: 0x0028F3B8
	public BasicLookInputModule()
	{
	}

	// Token: 0x06006CF3 RID: 27891 RVA: 0x00290FD8 File Offset: 0x0028F3D8
	private PointerEventData GetLookPointerEventData()
	{
		Vector2 position;
		position.x = (float)(Screen.width / 2);
		position.y = (float)(Screen.height / 2);
		if (this.lookData == null)
		{
			this.lookData = new PointerEventData(base.eventSystem);
		}
		this.lookData.Reset();
		this.lookData.delta = Vector2.zero;
		this.lookData.position = position;
		this.lookData.scrollDelta = Vector2.zero;
		base.eventSystem.RaycastAll(this.lookData, this.m_RaycastResultCache);
		this.lookData.pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
		this.m_RaycastResultCache.Clear();
		return this.lookData;
	}

	// Token: 0x06006CF4 RID: 27892 RVA: 0x00291094 File Offset: 0x0028F494
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

	// Token: 0x06006CF5 RID: 27893 RVA: 0x002910E0 File Offset: 0x0028F4E0
	public override void Process()
	{
		this.SendUpdateEventToSelectedObject();
		PointerEventData lookPointerEventData = this.GetLookPointerEventData();
		base.HandlePointerExitAndEnter(lookPointerEventData, lookPointerEventData.pointerCurrentRaycast.gameObject);
		if (Input.GetButtonDown(this.submitButtonName))
		{
			base.eventSystem.SetSelectedGameObject(null);
			if (lookPointerEventData.pointerCurrentRaycast.gameObject != null)
			{
				GameObject gameObject = lookPointerEventData.pointerCurrentRaycast.gameObject;
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(gameObject, lookPointerEventData, ExecuteEvents.submitHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.ExecuteHierarchy<ISelectHandler>(gameObject, lookPointerEventData, ExecuteEvents.selectHandler);
				}
				if (gameObject2 != null)
				{
					Debug.Log("Selected " + gameObject2.name);
					base.eventSystem.SetSelectedGameObject(gameObject2);
				}
			}
		}
		if (base.eventSystem.currentSelectedGameObject && this.controlAxisName != null && this.controlAxisName != string.Empty)
		{
			float axis = Input.GetAxis(this.controlAxisName);
			if (axis > 0.01f || axis < -0.01f)
			{
				AxisEventData axisEventData = this.GetAxisEventData(axis, 0f, 0f);
				ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			}
		}
	}

	// Token: 0x04005E70 RID: 24176
	public const int kLookId = -3;

	// Token: 0x04005E71 RID: 24177
	public string submitButtonName = "ButtonA";

	// Token: 0x04005E72 RID: 24178
	public string controlAxisName = "RightStickX";

	// Token: 0x04005E73 RID: 24179
	private PointerEventData lookData;
}
