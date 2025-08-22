using System;

// Token: 0x02000C1F RID: 3103
public class InteractionControl : JSONStorable
{
	// Token: 0x06005A43 RID: 23107 RVA: 0x00212B9A File Offset: 0x00210F9A
	public InteractionControl()
	{
	}

	// Token: 0x06005A44 RID: 23108 RVA: 0x00212BA4 File Offset: 0x00210FA4
	protected void SelectModeFreeMoveMouse()
	{
		if (SuperController.singleton != null)
		{
			if (SuperController.singleton.gameMode == SuperController.GameMode.Play)
			{
				SuperController.singleton.SelectModeFreeMoveMouse();
			}
			else
			{
				SuperController.singleton.Error("Trigger tried to set mode to free mouse look while in edit game mode. This trigger only works in play mode", true, true);
			}
		}
	}

	// Token: 0x06005A45 RID: 23109 RVA: 0x00212BF4 File Offset: 0x00210FF4
	protected void SelectModeOff()
	{
		if (SuperController.singleton != null)
		{
			if (SuperController.singleton.gameMode == SuperController.GameMode.Play)
			{
				SuperController.singleton.SelectModeOff();
			}
			else
			{
				SuperController.singleton.Error("Trigger tried to set mode to off while in edit game mode. This trigger only works in play mode", true, true);
			}
		}
	}

	// Token: 0x06005A46 RID: 23110 RVA: 0x00212C41 File Offset: 0x00211041
	protected void ResetNavigationRigPositionRotation()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ResetNavigationRigPositionRotation();
		}
	}

	// Token: 0x06005A47 RID: 23111 RVA: 0x00212C5D File Offset: 0x0021105D
	protected void MoveToSceneLoadPosition()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.MoveToSceneLoadPosition();
		}
	}

	// Token: 0x06005A48 RID: 23112 RVA: 0x00212C7C File Offset: 0x0021107C
	protected void Init()
	{
		this.selectModeFreeMoveMouseAction = new JSONStorableAction("SelectModeFreeMoveMouse", new JSONStorableAction.ActionCallback(this.SelectModeFreeMoveMouse));
		base.RegisterAction(this.selectModeFreeMoveMouseAction);
		this.selectModeOffAction = new JSONStorableAction("SelectModeOff", new JSONStorableAction.ActionCallback(this.SelectModeOff));
		base.RegisterAction(this.selectModeOffAction);
	}

	// Token: 0x06005A49 RID: 23113 RVA: 0x00212CD9 File Offset: 0x002110D9
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x04004A78 RID: 19064
	protected JSONStorableAction selectModeFreeMoveMouseAction;

	// Token: 0x04004A79 RID: 19065
	protected JSONStorableAction selectModeOffAction;

	// Token: 0x04004A7A RID: 19066
	protected JSONStorableAction resetNavigationRigPositionRotationAction;

	// Token: 0x04004A7B RID: 19067
	protected JSONStorableAction moveToSceneLoadPositionAction;
}
