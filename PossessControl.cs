using System;

// Token: 0x02000C43 RID: 3139
public class PossessControl : JSONStorable
{
	// Token: 0x06005B4F RID: 23375 RVA: 0x00218467 File Offset: 0x00216867
	public PossessControl()
	{
	}

	// Token: 0x06005B50 RID: 23376 RVA: 0x0021846F File Offset: 0x0021686F
	protected void StartPossessAndAlign()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectModePossessAndAlign();
		}
	}

	// Token: 0x06005B51 RID: 23377 RVA: 0x0021848B File Offset: 0x0021688B
	protected void StartTwoStagePossess()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectModeTwoStagePossess();
		}
	}

	// Token: 0x06005B52 RID: 23378 RVA: 0x002184A7 File Offset: 0x002168A7
	protected void StartTwoStagePossessNoClear()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectModeTwoStagePossessNoClear();
		}
	}

	// Token: 0x06005B53 RID: 23379 RVA: 0x002184C3 File Offset: 0x002168C3
	protected void StartHandPossess()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectModePossess(true);
		}
	}

	// Token: 0x06005B54 RID: 23380 RVA: 0x002184E0 File Offset: 0x002168E0
	protected void StopHandPossess()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ClearPossess(true);
		}
	}

	// Token: 0x06005B55 RID: 23381 RVA: 0x002184FD File Offset: 0x002168FD
	protected void StopAllPossess()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ClearPossess();
		}
	}

	// Token: 0x06005B56 RID: 23382 RVA: 0x00218519 File Offset: 0x00216919
	protected void StartUnpossess()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectModeUnpossess();
		}
	}

	// Token: 0x06005B57 RID: 23383 RVA: 0x00218538 File Offset: 0x00216938
	protected void Init()
	{
		this.startPossessAndAlignJSON = new JSONStorableAction("StartPossessAndAlign", new JSONStorableAction.ActionCallback(this.StartPossessAndAlign));
		base.RegisterAction(this.startPossessAndAlignJSON);
		this.startTwoStagePossessJSON = new JSONStorableAction("StartTwoStagePossess", new JSONStorableAction.ActionCallback(this.StartTwoStagePossess));
		base.RegisterAction(this.startTwoStagePossessJSON);
		this.startTwoStagePossessNoClearJSON = new JSONStorableAction("StartTwoStagePossessNoClear", new JSONStorableAction.ActionCallback(this.StartTwoStagePossessNoClear));
		base.RegisterAction(this.startTwoStagePossessNoClearJSON);
		this.startHandPossessJSON = new JSONStorableAction("StartHandPossess", new JSONStorableAction.ActionCallback(this.StartHandPossess));
		base.RegisterAction(this.startHandPossessJSON);
		this.stopHandPossessJSON = new JSONStorableAction("StopHandPossess", new JSONStorableAction.ActionCallback(this.StopHandPossess));
		base.RegisterAction(this.stopHandPossessJSON);
		this.stopAllPossessJSON = new JSONStorableAction("StopAllPossess", new JSONStorableAction.ActionCallback(this.StopAllPossess));
		base.RegisterAction(this.stopAllPossessJSON);
		this.startUnpossessJSON = new JSONStorableAction("StartUnpossess", new JSONStorableAction.ActionCallback(this.StartUnpossess));
		base.RegisterAction(this.startUnpossessJSON);
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x0021865D File Offset: 0x00216A5D
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x04004B47 RID: 19271
	protected JSONStorableAction startPossessAndAlignJSON;

	// Token: 0x04004B48 RID: 19272
	protected JSONStorableAction startTwoStagePossessJSON;

	// Token: 0x04004B49 RID: 19273
	protected JSONStorableAction startTwoStagePossessNoClearJSON;

	// Token: 0x04004B4A RID: 19274
	protected JSONStorableAction startHandPossessJSON;

	// Token: 0x04004B4B RID: 19275
	protected JSONStorableAction stopHandPossessJSON;

	// Token: 0x04004B4C RID: 19276
	protected JSONStorableAction stopAllPossessJSON;

	// Token: 0x04004B4D RID: 19277
	protected JSONStorableAction startUnpossessJSON;
}
