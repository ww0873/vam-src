using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000B67 RID: 2919
public class MotionAnimationControl : JSONStorable
{
	// Token: 0x06005177 RID: 20855 RVA: 0x001D6A0F File Offset: 0x001D4E0F
	public MotionAnimationControl()
	{
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x06005178 RID: 20856 RVA: 0x001D6A2B File Offset: 0x001D4E2B
	// (set) Token: 0x06005179 RID: 20857 RVA: 0x001D6A33 File Offset: 0x001D4E33
	public MotionAnimationMaster animationMaster
	{
		get
		{
			return this._animationMaster;
		}
		set
		{
			if (this._animationMaster != value)
			{
				this._animationMaster = value;
			}
		}
	}

	// Token: 0x0600517A RID: 20858 RVA: 0x001D6A4D File Offset: 0x001D4E4D
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x0600517B RID: 20859 RVA: 0x001D6A58 File Offset: 0x001D4E58
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this.clip != null && this.clip.SaveToJSON(json))
		{
			this.needsStore = true;
		}
		return json;
	}

	// Token: 0x0600517C RID: 20860 RVA: 0x001D6AA0 File Offset: 0x001D4EA0
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("steps") && this.clip != null)
		{
			this.clip.RestoreFromJSON(jc, setMissingToDefault);
		}
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x0600517D RID: 20861 RVA: 0x001D6AF4 File Offset: 0x001D4EF4
	// (set) Token: 0x0600517E RID: 20862 RVA: 0x001D6B0E File Offset: 0x001D4F0E
	public bool armedForRecord
	{
		get
		{
			return this.armedForRecordJSON != null && this.armedForRecordJSON.val;
		}
		set
		{
			if (this.armedForRecordJSON != null)
			{
				this.armedForRecordJSON.val = value;
			}
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x0600517F RID: 20863 RVA: 0x001D6B27 File Offset: 0x001D4F27
	// (set) Token: 0x06005180 RID: 20864 RVA: 0x001D6B41 File Offset: 0x001D4F41
	public bool playbackEnabled
	{
		get
		{
			return this.playbackEnabledJSON != null && this.playbackEnabledJSON.val;
		}
		set
		{
			if (this.playbackEnabledJSON != null)
			{
				this.playbackEnabledJSON.val = value;
			}
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06005181 RID: 20865 RVA: 0x001D6B5A File Offset: 0x001D4F5A
	// (set) Token: 0x06005182 RID: 20866 RVA: 0x001D6B74 File Offset: 0x001D4F74
	public bool drawPath
	{
		get
		{
			return this.drawPathJSON != null && this.drawPathJSON.val;
		}
		set
		{
			if (this.drawPathJSON != null)
			{
				this.drawPathJSON.val = value;
			}
		}
	}

	// Token: 0x06005183 RID: 20867 RVA: 0x001D6B90 File Offset: 0x001D4F90
	protected void Init()
	{
		this.armedForRecordJSON = new JSONStorableBool("armedForRecord", false);
		this.armedForRecordJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.armedForRecordJSON);
		this.playbackEnabledJSON = new JSONStorableBool("playbackEnabled", true);
		this.playbackEnabledJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.playbackEnabledJSON);
		this.drawPathJSON = new JSONStorableBool("drawPath", false);
		this.drawPathJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.drawPathJSON);
		this.clip = new MotionAnimationClip();
	}

	// Token: 0x06005184 RID: 20868 RVA: 0x001D6C24 File Offset: 0x001D5024
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			MotionAnimationControlUI componentInChildren = this.UITransform.GetComponentInChildren<MotionAnimationControlUI>(true);
			if (componentInChildren != null)
			{
				this.armedForRecordJSON.toggle = componentInChildren.armedForRecordToggle;
				this.playbackEnabledJSON.toggle = componentInChildren.playbackEnabledToggle;
				this.drawPathJSON.toggle = componentInChildren.drawPathToggle;
				if (componentInChildren.clearAnimationButton != null)
				{
					componentInChildren.clearAnimationButton.onClick.AddListener(new UnityAction(this.ClearAnimation));
				}
			}
		}
	}

	// Token: 0x06005185 RID: 20869 RVA: 0x001D6CBC File Offset: 0x001D50BC
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			MotionAnimationControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<MotionAnimationControlUI>(true);
			if (componentInChildren != null)
			{
				this.armedForRecordJSON.toggleAlt = componentInChildren.armedForRecordToggle;
				this.playbackEnabledJSON.toggleAlt = componentInChildren.playbackEnabledToggle;
				this.drawPathJSON.toggleAlt = componentInChildren.drawPathToggle;
				if (componentInChildren.clearAnimationButton)
				{
					componentInChildren.clearAnimationButton.onClick.AddListener(new UnityAction(this.ClearAnimation));
				}
			}
		}
	}

	// Token: 0x06005186 RID: 20870 RVA: 0x001D6D52 File Offset: 0x001D5152
	public void ClearAnimation()
	{
		if (this.clip != null)
		{
			this.clip.ClearAllSteps();
		}
	}

	// Token: 0x06005187 RID: 20871 RVA: 0x001D6D6A File Offset: 0x001D516A
	public void PrepareRecord(int recordCounter)
	{
		if (this.armedForRecordJSON != null && this.armedForRecordJSON.val)
		{
			this.clip.PrepareRecord((float)recordCounter);
		}
	}

	// Token: 0x06005188 RID: 20872 RVA: 0x001D6D94 File Offset: 0x001D5194
	public void RecordStep(float recordCounter, bool forceRecord = false)
	{
		if (this.armedForRecordJSON != null && this.armedForRecordJSON.val && this.controller != null)
		{
			this.clip.RecordStep(this.controller.transform, recordCounter, this.controller.currentPositionState != FreeControllerV3.PositionState.Off, this.controller.currentRotationState != FreeControllerV3.RotationState.Off, forceRecord);
		}
	}

	// Token: 0x06005189 RID: 20873 RVA: 0x001D6E07 File Offset: 0x001D5207
	public void FinalizeRecord()
	{
		if (this.armedForRecordJSON != null && this.armedForRecordJSON.val)
		{
			this.clip.FinalizeRecord();
		}
	}

	// Token: 0x0600518A RID: 20874 RVA: 0x001D6E2F File Offset: 0x001D522F
	public void TrimClip(float startStep, float stopStep)
	{
		if (this.clip != null && this.clip.clipLength > 0f)
		{
			this.clip.ClearAllStepsStartingAt(stopStep);
			this.clip.ShiftAllSteps(-startStep);
		}
	}

	// Token: 0x0600518B RID: 20875 RVA: 0x001D6E6C File Offset: 0x001D526C
	protected void ApplyStep(MotionAnimationStep step, bool forceAlignControlled = false)
	{
		if (this.controller != null)
		{
			if (!this.suspendPositionPlayback)
			{
				if (step.positionOn)
				{
					this.controller.currentPositionState = FreeControllerV3.PositionState.On;
				}
				else
				{
					this.controller.currentPositionState = FreeControllerV3.PositionState.Off;
				}
				this.controller.transform.localPosition = step.position;
				if (forceAlignControlled && this.controller.followWhenOff != null)
				{
					this.controller.followWhenOff.position = this.controller.transform.position;
				}
			}
			if (!this.suspendRotationPlayback)
			{
				if (step.rotationOn)
				{
					this.controller.currentRotationState = FreeControllerV3.RotationState.On;
				}
				else
				{
					this.controller.currentRotationState = FreeControllerV3.RotationState.Off;
				}
				this.controller.transform.localRotation = step.rotation;
				if (forceAlignControlled && this.controller.followWhenOff != null)
				{
					this.controller.followWhenOff.rotation = this.controller.transform.rotation;
				}
			}
		}
	}

	// Token: 0x0600518C RID: 20876 RVA: 0x001D6F94 File Offset: 0x001D5394
	public void PlaybackStepForceAlign(float playbackCounter)
	{
		if (this.playbackEnabledJSON != null && this.playbackEnabledJSON.val && this.clip != null && this.clip.clipLength > 0f)
		{
			MotionAnimationStep step = this.clip.PlaybackStep(playbackCounter);
			this.ApplyStep(step, true);
		}
	}

	// Token: 0x0600518D RID: 20877 RVA: 0x001D6FF4 File Offset: 0x001D53F4
	public void PlaybackStep(float playbackCounter)
	{
		if (this.playbackEnabledJSON != null && this.playbackEnabledJSON.val && this.clip != null && this.clip.clipLength > 0f)
		{
			MotionAnimationStep step = this.clip.PlaybackStep(playbackCounter);
			this.ApplyStep(step, false);
		}
	}

	// Token: 0x0600518E RID: 20878 RVA: 0x001D7054 File Offset: 0x001D5454
	public void LoopbackStep(float percent, float toTimeStep)
	{
		if (this.playbackEnabledJSON != null && this.playbackEnabledJSON.val && this.clip != null && this.clip.clipLength > 0f)
		{
			MotionAnimationStep step = this.clip.LoopbackStep(percent, toTimeStep);
			this.ApplyStep(step, false);
		}
	}

	// Token: 0x0600518F RID: 20879 RVA: 0x001D70B2 File Offset: 0x001D54B2
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x06005190 RID: 20880 RVA: 0x001D70D8 File Offset: 0x001D54D8
	protected void Update()
	{
		if ((this.drawPathJSON.val || this.drawPathOpt) && this.clip != null && this.material != null)
		{
			Mesh mesh = this.clip.GetMesh();
			Graphics.DrawMesh(mesh, base.transform.parent.localToWorldMatrix, this.material, base.gameObject.layer, null, 0, null, false, false);
		}
	}

	// Token: 0x04004131 RID: 16689
	protected MotionAnimationMaster _animationMaster;

	// Token: 0x04004132 RID: 16690
	protected string[] customParamNames = new string[]
	{
		"steps"
	};

	// Token: 0x04004133 RID: 16691
	public bool suspendPositionPlayback;

	// Token: 0x04004134 RID: 16692
	public bool suspendRotationPlayback;

	// Token: 0x04004135 RID: 16693
	protected JSONStorableBool armedForRecordJSON;

	// Token: 0x04004136 RID: 16694
	protected JSONStorableBool playbackEnabledJSON;

	// Token: 0x04004137 RID: 16695
	protected JSONStorableBool drawPathJSON;

	// Token: 0x04004138 RID: 16696
	public FreeControllerV3 controller;

	// Token: 0x04004139 RID: 16697
	[NonSerialized]
	public MotionAnimationClip clip;

	// Token: 0x0400413A RID: 16698
	public Material material;

	// Token: 0x0400413B RID: 16699
	protected Mesh mesh;

	// Token: 0x0400413C RID: 16700
	public bool drawPathOpt;
}
