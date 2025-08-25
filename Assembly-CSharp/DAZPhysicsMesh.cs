using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B3E RID: 2878
[ExecuteInEditMode]
public class DAZPhysicsMesh : PhysicsSimulatorJSONStorable, ISerializationCallbackReceiver
{
	// Token: 0x06004EC2 RID: 20162 RVA: 0x001C1A18 File Offset: 0x001BFE18
	public DAZPhysicsMesh()
	{
	}

	// Token: 0x06004EC3 RID: 20163 RVA: 0x001C1B1E File Offset: 0x001BFF1E
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06004EC4 RID: 20164 RVA: 0x001C1B20 File Offset: 0x001BFF20
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x001C1B24 File Offset: 0x001BFF24
	protected override void SyncResetSimulation()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.resetSimulation = this._resetSimulation;
		}
	}

	// Token: 0x06004EC6 RID: 20166 RVA: 0x001C1B88 File Offset: 0x001BFF88
	protected override void SyncFreezeSimulation()
	{
		this.SyncOn(true);
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001C1B91 File Offset: 0x001BFF91
	protected void SyncOnCallback(bool b)
	{
		this._on = b;
		this.SyncOn(true);
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001C1BA4 File Offset: 0x001BFFA4
	protected void SyncOn(bool resetSim = true)
	{
		this._globalOn = DAZPhysicsMesh.globalEnable;
		bool flag = this._globalOn && this._on && this._alternateOn;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.on = flag;
			dazphysicsMeshSoftVerticesGroup.freeze = this._freezeSimulation;
		}
		if (Application.isPlaying)
		{
			if (flag)
			{
				if (this.transformToEnableWhenOn != null)
				{
					this.transformToEnableWhenOn.gameObject.SetActive(true);
				}
				if (this.transformsToEnableWhenOn != null)
				{
					foreach (Transform transform in this.transformsToEnableWhenOn)
					{
						if (transform != null)
						{
							transform.gameObject.SetActive(true);
						}
					}
				}
				if (this.transformToEnableWhenOff != null)
				{
					this.transformToEnableWhenOff.gameObject.SetActive(false);
				}
				if (this.transformsToEnableWhenOff != null)
				{
					foreach (Transform transform2 in this.transformsToEnableWhenOff)
					{
						if (transform2 != null)
						{
							transform2.gameObject.SetActive(false);
						}
					}
				}
				bool flag2 = false;
				if (this.autoColliderGroupsToEnableWhenOff != null)
				{
					foreach (AutoColliderGroup autoColliderGroup in this.autoColliderGroupsToEnableWhenOff)
					{
						if (autoColliderGroup.enabled)
						{
							autoColliderGroup.enabled = false;
							flag2 = true;
						}
					}
				}
				if (this.autoColliderGroupsToEnableWhenOn != null)
				{
					foreach (AutoColliderGroup autoColliderGroup2 in this.autoColliderGroupsToEnableWhenOn)
					{
						if (!autoColliderGroup2.enabled)
						{
							autoColliderGroup2.enabled = true;
							flag2 = true;
						}
					}
				}
				if (flag2 && this.characterRun != null)
				{
					this.characterRun.SetNeedsAutoColliderUpdate();
				}
				base.gameObject.SetActive(false);
				base.gameObject.SetActive(true);
				if (!this._freezeSimulation && resetSim)
				{
					this.containingAtom.ResetPhysics(false, true);
				}
			}
			else
			{
				if (this.transformToEnableWhenOn != null)
				{
					this.transformToEnableWhenOn.gameObject.SetActive(false);
				}
				if (this.transformsToEnableWhenOn != null)
				{
					foreach (Transform transform3 in this.transformsToEnableWhenOn)
					{
						if (transform3 != null)
						{
							transform3.gameObject.SetActive(false);
						}
					}
				}
				if (this.transformToEnableWhenOff != null)
				{
					this.transformToEnableWhenOff.gameObject.SetActive(true);
				}
				if (this.transformsToEnableWhenOff != null)
				{
					foreach (Transform transform4 in this.transformsToEnableWhenOff)
					{
						if (transform4 != null)
						{
							transform4.gameObject.SetActive(true);
						}
					}
				}
				bool flag3 = false;
				if (this.autoColliderGroupsToEnableWhenOff != null)
				{
					foreach (AutoColliderGroup autoColliderGroup3 in this.autoColliderGroupsToEnableWhenOff)
					{
						if (!autoColliderGroup3.enabled)
						{
							autoColliderGroup3.enabled = true;
							flag3 = true;
						}
					}
				}
				if (this.autoColliderGroupsToEnableWhenOn != null)
				{
					foreach (AutoColliderGroup autoColliderGroup4 in this.autoColliderGroupsToEnableWhenOn)
					{
						if (autoColliderGroup4.enabled)
						{
							autoColliderGroup4.enabled = false;
							flag3 = true;
						}
					}
				}
				if (flag3 && this.characterRun != null)
				{
					this.characterRun.SetNeedsAutoColliderUpdate();
				}
			}
		}
	}

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x06004EC9 RID: 20169 RVA: 0x001C1FB0 File Offset: 0x001C03B0
	// (set) Token: 0x06004ECA RID: 20170 RVA: 0x001C1FB8 File Offset: 0x001C03B8
	public bool on
	{
		get
		{
			return this._on;
		}
		set
		{
			if (this.onJSON != null)
			{
				this.onJSON.val = value;
			}
			else if (this._on != value)
			{
				this._on = value;
				this.SyncOn(true);
			}
		}
	}

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x06004ECB RID: 20171 RVA: 0x001C1FF0 File Offset: 0x001C03F0
	// (set) Token: 0x06004ECC RID: 20172 RVA: 0x001C1FF8 File Offset: 0x001C03F8
	public bool alternateOn
	{
		get
		{
			return this._alternateOn;
		}
		set
		{
			if (this._alternateOn != value)
			{
				this._alternateOn = value;
				this.SyncOn(true);
			}
		}
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x001C2014 File Offset: 0x001C0414
	protected void MTTask(object info)
	{
		DAZPhysicsMesh.DAZPhysicsMeshTaskInfo dazphysicsMeshTaskInfo = (DAZPhysicsMesh.DAZPhysicsMeshTaskInfo)info;
		while (this._threadsRunning)
		{
			dazphysicsMeshTaskInfo.resetEvent.WaitOne(-1, true);
			if (dazphysicsMeshTaskInfo.kill)
			{
				break;
			}
			if (dazphysicsMeshTaskInfo.taskType == DAZPhysicsMesh.DAZPhysicsMeshTaskType.UpdateSoftJointTargets)
			{
				Thread.Sleep(0);
				this.UpdateSoftJointTargetsThreaded();
			}
			else if (dazphysicsMeshTaskInfo.taskType == DAZPhysicsMesh.DAZPhysicsMeshTaskType.MorphVertices)
			{
				Thread.Sleep(0);
				this.MorphSoftVerticesThreaded();
			}
			dazphysicsMeshTaskInfo.working = false;
		}
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x001C2098 File Offset: 0x001C0498
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.physicsMeshTask != null)
		{
			this.physicsMeshTask.kill = true;
			this.physicsMeshTask.resetEvent.Set();
			while (this.physicsMeshTask.thread.IsAlive)
			{
			}
			this.physicsMeshTask = null;
		}
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001C20F8 File Offset: 0x001C04F8
	protected void StartThreads()
	{
		if (this.useThreading && !this._threadsRunning)
		{
			this._threadsRunning = true;
			this.physicsMeshTask = new DAZPhysicsMesh.DAZPhysicsMeshTaskInfo();
			this.physicsMeshTask.threadIndex = 0;
			this.physicsMeshTask.name = "UpdateSoftJointTargetsTask";
			this.physicsMeshTask.resetEvent = new AutoResetEvent(false);
			this.physicsMeshTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.physicsMeshTask.thread.Priority = System.Threading.ThreadPriority.BelowNormal;
			this.physicsMeshTask.taskType = DAZPhysicsMesh.DAZPhysicsMeshTaskType.UpdateSoftJointTargets;
			this.physicsMeshTask.thread.Start(this.physicsMeshTask);
		}
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001C21AC File Offset: 0x001C05AC
	protected override void SyncCollisionEnabled()
	{
		base.SyncCollisionEnabled();
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.collisionEnabled = this._collisionEnabled;
		}
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x001C2214 File Offset: 0x001C0614
	protected override void SyncUseInterpolation()
	{
		base.SyncUseInterpolation();
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.useInterpolation = this._useInterpolation;
		}
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x001C227C File Offset: 0x001C067C
	protected override void SyncSolverIterations()
	{
		base.SyncSolverIterations();
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.solverIterations = this._solverIterations;
		}
	}

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x06004ED3 RID: 20179 RVA: 0x001C22E4 File Offset: 0x001C06E4
	// (set) Token: 0x06004ED4 RID: 20180 RVA: 0x001C22EC File Offset: 0x001C06EC
	public Transform skinTransform
	{
		get
		{
			return this._skinTransform;
		}
		set
		{
			if (this._skinTransform != value)
			{
				this._skinTransform = value;
			}
		}
	}

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x06004ED5 RID: 20181 RVA: 0x001C2306 File Offset: 0x001C0706
	// (set) Token: 0x06004ED6 RID: 20182 RVA: 0x001C2310 File Offset: 0x001C0710
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			if (this._skin != value)
			{
				this._skin = value;
				if (this._skin != null)
				{
					this.Init();
					this._skin.Init();
					foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
					{
						dazphysicsMeshSoftVerticesGroup.skin = this._skin;
					}
					if (this._skin.dazMesh != null)
					{
						this._baseVertToUVVertFullMap = this._skin.dazMesh.baseVertToUVVertFullMap;
					}
				}
			}
		}
	}

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x06004ED7 RID: 20183 RVA: 0x001C23D8 File Offset: 0x001C07D8
	// (set) Token: 0x06004ED8 RID: 20184 RVA: 0x001C23E0 File Offset: 0x001C07E0
	public bool showHandles
	{
		get
		{
			return this._showHandles;
		}
		set
		{
			if (this._showHandles != value)
			{
				this._showHandles = value;
			}
		}
	}

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x06004ED9 RID: 20185 RVA: 0x001C23F5 File Offset: 0x001C07F5
	// (set) Token: 0x06004EDA RID: 20186 RVA: 0x001C23FD File Offset: 0x001C07FD
	public bool showBackfaceHandles
	{
		get
		{
			return this._showBackfaceHandles;
		}
		set
		{
			if (this._showBackfaceHandles != value)
			{
				this._showBackfaceHandles = value;
			}
		}
	}

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x06004EDB RID: 20187 RVA: 0x001C2412 File Offset: 0x001C0812
	// (set) Token: 0x06004EDC RID: 20188 RVA: 0x001C241A File Offset: 0x001C081A
	public bool showLinkLines
	{
		get
		{
			return this._showLinkLines;
		}
		set
		{
			if (this._showLinkLines != value)
			{
				this._showLinkLines = value;
			}
		}
	}

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x06004EDD RID: 20189 RVA: 0x001C242F File Offset: 0x001C082F
	// (set) Token: 0x06004EDE RID: 20190 RVA: 0x001C2437 File Offset: 0x001C0837
	public bool showColliders
	{
		get
		{
			return this._showColliders;
		}
		set
		{
			if (this._showColliders != value)
			{
				this._showColliders = value;
			}
		}
	}

	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x06004EDF RID: 20191 RVA: 0x001C244C File Offset: 0x001C084C
	// (set) Token: 0x06004EE0 RID: 20192 RVA: 0x001C2454 File Offset: 0x001C0854
	public bool showCurrentSoftGroupOnly
	{
		get
		{
			return this._showCurrentSoftGroupOnly;
		}
		set
		{
			if (this._showCurrentSoftGroupOnly != value)
			{
				this._showCurrentSoftGroupOnly = value;
			}
		}
	}

	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x06004EE1 RID: 20193 RVA: 0x001C2469 File Offset: 0x001C0869
	// (set) Token: 0x06004EE2 RID: 20194 RVA: 0x001C2471 File Offset: 0x001C0871
	public bool showCurrentSoftSetOnly
	{
		get
		{
			return this._showCurrentSoftSetOnly;
		}
		set
		{
			if (this._showCurrentSoftSetOnly != value)
			{
				this._showCurrentSoftSetOnly = value;
			}
		}
	}

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x06004EE3 RID: 20195 RVA: 0x001C2486 File Offset: 0x001C0886
	// (set) Token: 0x06004EE4 RID: 20196 RVA: 0x001C248E File Offset: 0x001C088E
	public float handleSize
	{
		get
		{
			return this._handleSize;
		}
		set
		{
			if (this._handleSize != value)
			{
				this._handleSize = value;
			}
		}
	}

	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x06004EE5 RID: 20197 RVA: 0x001C24A3 File Offset: 0x001C08A3
	// (set) Token: 0x06004EE6 RID: 20198 RVA: 0x001C24AB File Offset: 0x001C08AB
	public float softSpringMultiplierSetValue
	{
		get
		{
			return this._softSpringMultiplierSetValue;
		}
		set
		{
			if (this._softSpringMultiplierSetValue != value)
			{
				this._softSpringMultiplierSetValue = value;
			}
		}
	}

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x06004EE7 RID: 20199 RVA: 0x001C24C0 File Offset: 0x001C08C0
	// (set) Token: 0x06004EE8 RID: 20200 RVA: 0x001C24C8 File Offset: 0x001C08C8
	public float softSizeMultiplierSetValue
	{
		get
		{
			return this._softSizeMultiplierSetValue;
		}
		set
		{
			if (this._softSizeMultiplierSetValue != value)
			{
				this._softSizeMultiplierSetValue = value;
			}
		}
	}

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x06004EE9 RID: 20201 RVA: 0x001C24DD File Offset: 0x001C08DD
	// (set) Token: 0x06004EEA RID: 20202 RVA: 0x001C24E5 File Offset: 0x001C08E5
	public float softLimitMultiplierSetValue
	{
		get
		{
			return this._softLimitMultiplierSetValue;
		}
		set
		{
			if (this._softLimitMultiplierSetValue != value)
			{
				this._softLimitMultiplierSetValue = value;
			}
		}
	}

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x06004EEB RID: 20203 RVA: 0x001C24FA File Offset: 0x001C08FA
	// (set) Token: 0x06004EEC RID: 20204 RVA: 0x001C2502 File Offset: 0x001C0902
	public DAZPhysicsMesh.SelectionMode selectionMode
	{
		get
		{
			return this._selectionMode;
		}
		set
		{
			if (this._selectionMode != value)
			{
				this._selectionMode = value;
			}
		}
	}

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x06004EED RID: 20205 RVA: 0x001C2517 File Offset: 0x001C0917
	// (set) Token: 0x06004EEE RID: 20206 RVA: 0x001C251F File Offset: 0x001C091F
	public int subMeshSelection
	{
		get
		{
			return this._subMeshSelection;
		}
		set
		{
			if (value != this._subMeshSelection)
			{
				this._subMeshSelection = value;
			}
		}
	}

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06004EEF RID: 20207 RVA: 0x001C2534 File Offset: 0x001C0934
	// (set) Token: 0x06004EF0 RID: 20208 RVA: 0x001C253C File Offset: 0x001C093C
	public int subMeshSelection2
	{
		get
		{
			return this._subMeshSelection2;
		}
		set
		{
			if (value != this._subMeshSelection2)
			{
				this._subMeshSelection2 = value;
			}
		}
	}

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x001C2551 File Offset: 0x001C0951
	// (set) Token: 0x06004EF2 RID: 20210 RVA: 0x001C2559 File Offset: 0x001C0959
	public bool showHardGroups
	{
		get
		{
			return this._showHardGroups;
		}
		set
		{
			if (this._showHardGroups != value)
			{
				this._showHardGroups = value;
			}
		}
	}

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x06004EF3 RID: 20211 RVA: 0x001C256E File Offset: 0x001C096E
	// (set) Token: 0x06004EF4 RID: 20212 RVA: 0x001C2576 File Offset: 0x001C0976
	public int currentHardVerticesGroupIndex
	{
		get
		{
			return this._currentHardVerticesGroupIndex;
		}
		set
		{
			if (this._currentHardVerticesGroupIndex != value)
			{
				this._currentHardVerticesGroupIndex = value;
			}
		}
	}

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x06004EF5 RID: 20213 RVA: 0x001C258B File Offset: 0x001C098B
	public DAZPhysicsMeshHardVerticesGroup currentHardVerticesGroup
	{
		get
		{
			if (this._currentHardVerticesGroupIndex >= 0 && this._currentHardVerticesGroupIndex < this._hardVerticesGroups.Count)
			{
				return this._hardVerticesGroups[this._currentHardVerticesGroupIndex];
			}
			return null;
		}
	}

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x001C25C2 File Offset: 0x001C09C2
	// (set) Token: 0x06004EF7 RID: 20215 RVA: 0x001C25CA File Offset: 0x001C09CA
	public bool showSoftGroups
	{
		get
		{
			return this._showSoftGroups;
		}
		set
		{
			if (this._showSoftGroups != value)
			{
				this._showSoftGroups = value;
			}
		}
	}

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x06004EF8 RID: 20216 RVA: 0x001C25DF File Offset: 0x001C09DF
	// (set) Token: 0x06004EF9 RID: 20217 RVA: 0x001C25E7 File Offset: 0x001C09E7
	public int currentSoftVerticesGroupIndex
	{
		get
		{
			return this._currentSoftVerticesGroupIndex;
		}
		set
		{
			if (this._currentSoftVerticesGroupIndex != value)
			{
				this._currentSoftVerticesGroupIndex = value;
			}
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06004EFA RID: 20218 RVA: 0x001C25FC File Offset: 0x001C09FC
	// (set) Token: 0x06004EFB RID: 20219 RVA: 0x001C2634 File Offset: 0x001C0A34
	public DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup
	{
		get
		{
			if (this._currentSoftVerticesGroupIndex >= 0 && this._currentSoftVerticesGroupIndex < this._softVerticesGroups.Count)
			{
				return this._softVerticesGroups[this._currentSoftVerticesGroupIndex];
			}
			return null;
		}
		set
		{
			for (int i = 0; i < this._softVerticesGroups.Count; i++)
			{
				if (this._softVerticesGroups[i] == value)
				{
					this._currentSoftVerticesGroupIndex = i;
				}
			}
		}
	}

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x06004EFC RID: 20220 RVA: 0x001C2676 File Offset: 0x001C0A76
	// (set) Token: 0x06004EFD RID: 20221 RVA: 0x001C267E File Offset: 0x001C0A7E
	public bool showColliderGroups
	{
		get
		{
			return this._showColliderGroups;
		}
		set
		{
			if (this._showColliderGroups != value)
			{
				this._showColliderGroups = value;
			}
		}
	}

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06004EFE RID: 20222 RVA: 0x001C2693 File Offset: 0x001C0A93
	// (set) Token: 0x06004EFF RID: 20223 RVA: 0x001C269B File Offset: 0x001C0A9B
	public int currentColliderGroupIndex
	{
		get
		{
			return this._currentColliderGroupIndex;
		}
		set
		{
			if (this._currentColliderGroupIndex != value)
			{
				this._currentColliderGroupIndex = value;
			}
		}
	}

	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x06004F00 RID: 20224 RVA: 0x001C26B0 File Offset: 0x001C0AB0
	public List<DAZPhysicsMeshHardVerticesGroup> hardVerticesGroups
	{
		get
		{
			return this._hardVerticesGroups;
		}
	}

	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x06004F01 RID: 20225 RVA: 0x001C26B8 File Offset: 0x001C0AB8
	public List<DAZPhysicsMeshSoftVerticesGroup> softVerticesGroups
	{
		get
		{
			return this._softVerticesGroups;
		}
	}

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x06004F02 RID: 20226 RVA: 0x001C26C0 File Offset: 0x001C0AC0
	public List<DAZPhysicsMeshColliderGroup> colliderGroups
	{
		get
		{
			return this._colliderGroups;
		}
	}

	// Token: 0x06004F03 RID: 20227 RVA: 0x001C26C8 File Offset: 0x001C0AC8
	protected void SyncGroupASpringMultiplier(float f)
	{
		for (int i = 0; i < this.groupASlots.Length; i++)
		{
			int num = this.groupASlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedSpring(this._softVerticesCombinedSpring);
	}

	// Token: 0x06004F04 RID: 20228 RVA: 0x001C2730 File Offset: 0x001C0B30
	protected void SyncGroupADamperMultiplier(float f)
	{
		for (int i = 0; i < this.groupASlots.Length; i++)
		{
			int num = this.groupASlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedDamper(this._softVerticesCombinedDamper);
	}

	// Token: 0x06004F05 RID: 20229 RVA: 0x001C2798 File Offset: 0x001C0B98
	protected void SyncGroupBSpringMultiplier(float f)
	{
		for (int i = 0; i < this.groupBSlots.Length; i++)
		{
			int num = this.groupBSlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedSpring(this._softVerticesCombinedSpring);
	}

	// Token: 0x06004F06 RID: 20230 RVA: 0x001C2800 File Offset: 0x001C0C00
	protected void SyncGroupBDamperMultiplier(float f)
	{
		for (int i = 0; i < this.groupBSlots.Length; i++)
		{
			int num = this.groupBSlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedDamper(this._softVerticesCombinedDamper);
	}

	// Token: 0x06004F07 RID: 20231 RVA: 0x001C2868 File Offset: 0x001C0C68
	protected void SyncGroupCSpringMultiplier(float f)
	{
		for (int i = 0; i < this.groupCSlots.Length; i++)
		{
			int num = this.groupCSlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedSpring(this._softVerticesCombinedSpring);
	}

	// Token: 0x06004F08 RID: 20232 RVA: 0x001C28D0 File Offset: 0x001C0CD0
	protected void SyncGroupCDamperMultiplier(float f)
	{
		for (int i = 0; i < this.groupCSlots.Length; i++)
		{
			int num = this.groupCSlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedDamper(this._softVerticesCombinedDamper);
	}

	// Token: 0x06004F09 RID: 20233 RVA: 0x001C2938 File Offset: 0x001C0D38
	protected void SyncGroupDSpringMultiplier(float f)
	{
		for (int i = 0; i < this.groupDSlots.Length; i++)
		{
			int num = this.groupDSlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedSpring(this._softVerticesCombinedSpring);
	}

	// Token: 0x06004F0A RID: 20234 RVA: 0x001C29A0 File Offset: 0x001C0DA0
	protected void SyncGroupDDamperMultiplier(float f)
	{
		for (int i = 0; i < this.groupDSlots.Length; i++)
		{
			int num = this.groupDSlots[i];
			if (num < this.softVerticesGroups.Count)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this.softVerticesGroups[num];
				if (dazphysicsMeshSoftVerticesGroup != null)
				{
					dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier = f;
				}
			}
		}
		this.SyncSoftVerticesCombinedDamper(this._softVerticesCombinedDamper);
	}

	// Token: 0x06004F0B RID: 20235 RVA: 0x001C2A08 File Offset: 0x001C0E08
	protected void SyncSoftVerticesCombinedSpring(float f)
	{
		this._softVerticesCombinedSpring = f;
		if (this.useCombinedSpringAndDamper)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
				{
					float num = this._softVerticesCombinedSpring * dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier;
					dazphysicsMeshSoftVerticesGroup.jointSpringNormal = num;
					dazphysicsMeshSoftVerticesGroup.jointSpringTangent = num;
					dazphysicsMeshSoftVerticesGroup.jointSpringTangent2 = num;
					if (dazphysicsMeshSoftVerticesGroup.tieLinkJointSpringAndDamperToNormalSpringAndDamper)
					{
						dazphysicsMeshSoftVerticesGroup.linkSpring = num;
					}
				}
			}
		}
	}

	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x06004F0C RID: 20236 RVA: 0x001C2AB0 File Offset: 0x001C0EB0
	// (set) Token: 0x06004F0D RID: 20237 RVA: 0x001C2AB8 File Offset: 0x001C0EB8
	public float softVerticesCombinedSpring
	{
		get
		{
			return this._softVerticesCombinedSpring;
		}
		set
		{
			if (this.softVerticesCombinedSpringJSON != null)
			{
				this.softVerticesCombinedSpringJSON.val = value;
			}
			else if (this._softVerticesCombinedSpring != value)
			{
				this.SyncSoftVerticesCombinedSpring(value);
			}
		}
	}

	// Token: 0x06004F0E RID: 20238 RVA: 0x001C2AEC File Offset: 0x001C0EEC
	protected void SyncSoftVerticesCombinedDamper(float f)
	{
		this._softVerticesCombinedDamper = f;
		if (this.useCombinedSpringAndDamper)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
				{
					float num = this._softVerticesCombinedDamper * dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier;
					dazphysicsMeshSoftVerticesGroup.jointDamperNormal = num;
					dazphysicsMeshSoftVerticesGroup.jointDamperTangent = num;
					dazphysicsMeshSoftVerticesGroup.jointDamperTangent2 = num;
					if (dazphysicsMeshSoftVerticesGroup.tieLinkJointSpringAndDamperToNormalSpringAndDamper)
					{
						dazphysicsMeshSoftVerticesGroup.linkDamper = num;
					}
				}
			}
		}
	}

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x06004F0F RID: 20239 RVA: 0x001C2B94 File Offset: 0x001C0F94
	// (set) Token: 0x06004F10 RID: 20240 RVA: 0x001C2B9C File Offset: 0x001C0F9C
	public float softVerticesCombinedDamper
	{
		get
		{
			return this._softVerticesCombinedDamper;
		}
		set
		{
			if (this.softVerticesCombinedDamperJSON != null)
			{
				this.softVerticesCombinedDamperJSON.val = value;
			}
			else if (this._softVerticesCombinedDamper != value)
			{
				this.SyncSoftVerticesCombinedDamper(value);
			}
		}
	}

	// Token: 0x06004F11 RID: 20241 RVA: 0x001C2BD0 File Offset: 0x001C0FD0
	protected void SyncSoftVerticesNormalSpring()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.jointSpringNormal = this._softVerticesNormalSpring * dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier;
			}
		}
	}

	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x06004F12 RID: 20242 RVA: 0x001C2C44 File Offset: 0x001C1044
	// (set) Token: 0x06004F13 RID: 20243 RVA: 0x001C2C4C File Offset: 0x001C104C
	public float softVerticesNormalSpring
	{
		get
		{
			return this._softVerticesNormalSpring;
		}
		set
		{
			if (!this.useCombinedSpringAndDamper && this._softVerticesNormalSpring != value)
			{
				this._softVerticesNormalSpring = value;
				if (this.softVerticesNormalSpringSlider != null)
				{
					this.softVerticesNormalSpringSlider.value = this._softVerticesNormalSpring;
				}
				this.SyncSoftVerticesNormalSpring();
			}
		}
	}

	// Token: 0x06004F14 RID: 20244 RVA: 0x001C2CA0 File Offset: 0x001C10A0
	protected void SyncSoftVerticesNormalDamper()
	{
		if (!this.useCombinedSpringAndDamper)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
				{
					dazphysicsMeshSoftVerticesGroup.jointDamperNormal = this._softVerticesNormalDamper * dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier;
				}
			}
		}
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x06004F15 RID: 20245 RVA: 0x001C2D20 File Offset: 0x001C1120
	// (set) Token: 0x06004F16 RID: 20246 RVA: 0x001C2D28 File Offset: 0x001C1128
	public float softVerticesNormalDamper
	{
		get
		{
			return this._softVerticesNormalDamper;
		}
		set
		{
			if (!this.useCombinedSpringAndDamper && this._softVerticesNormalDamper != value)
			{
				this._softVerticesNormalDamper = value;
				if (this.softVerticesNormalDamperSlider != null)
				{
					this.softVerticesNormalDamperSlider.value = this._softVerticesNormalDamper;
				}
				this.SyncSoftVerticesNormalDamper();
			}
		}
	}

	// Token: 0x06004F17 RID: 20247 RVA: 0x001C2D7C File Offset: 0x001C117C
	protected void SyncSoftVerticesTangentSpring()
	{
		if (!this.useCombinedSpringAndDamper)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
				{
					dazphysicsMeshSoftVerticesGroup.jointSpringTangent = this._softVerticesTangentSpring * dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier;
					dazphysicsMeshSoftVerticesGroup.jointSpringTangent2 = this._softVerticesTangentSpring * dazphysicsMeshSoftVerticesGroup.parentSettingSpringMultiplier;
				}
			}
		}
	}

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x06004F18 RID: 20248 RVA: 0x001C2E10 File Offset: 0x001C1210
	// (set) Token: 0x06004F19 RID: 20249 RVA: 0x001C2E18 File Offset: 0x001C1218
	public float softVerticesTangentSpring
	{
		get
		{
			return this._softVerticesTangentSpring;
		}
		set
		{
			if (!this.useCombinedSpringAndDamper && this._softVerticesTangentSpring != value)
			{
				this._softVerticesTangentSpring = value;
				if (this.softVerticesTangentSpringSlider != null)
				{
					this.softVerticesTangentSpringSlider.value = this._softVerticesTangentSpring;
				}
				this.SyncSoftVerticesTangentSpring();
			}
		}
	}

	// Token: 0x06004F1A RID: 20250 RVA: 0x001C2E6C File Offset: 0x001C126C
	protected void SyncSoftVerticesTangentDamper()
	{
		if (!this.useCombinedSpringAndDamper)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
				{
					dazphysicsMeshSoftVerticesGroup.jointDamperTangent = this._softVerticesTangentDamper * dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier;
					dazphysicsMeshSoftVerticesGroup.jointDamperTangent2 = this._softVerticesTangentDamper * dazphysicsMeshSoftVerticesGroup.parentSettingDamperMultiplier;
				}
			}
		}
	}

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x06004F1B RID: 20251 RVA: 0x001C2F00 File Offset: 0x001C1300
	// (set) Token: 0x06004F1C RID: 20252 RVA: 0x001C2F08 File Offset: 0x001C1308
	public float softVerticesTangentDamper
	{
		get
		{
			return this._softVerticesTangentDamper;
		}
		set
		{
			if (!this.useCombinedSpringAndDamper && this._softVerticesTangentDamper != value)
			{
				this._softVerticesTangentDamper = value;
				if (this.softVerticesTangentDamperSlider != null)
				{
					this.softVerticesTangentDamperSlider.value = this._softVerticesTangentDamper;
				}
				this.SyncSoftVerticesTangentDamper();
			}
		}
	}

	// Token: 0x06004F1D RID: 20253 RVA: 0x001C2F5C File Offset: 0x001C135C
	protected void SyncSoftVerticesSpringMaxForce()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.jointSpringMaxForce = this._softVerticesSpringMaxForce;
			}
		}
	}

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x06004F1E RID: 20254 RVA: 0x001C2FC8 File Offset: 0x001C13C8
	// (set) Token: 0x06004F1F RID: 20255 RVA: 0x001C2FD0 File Offset: 0x001C13D0
	public float softVerticesSpringMaxForce
	{
		get
		{
			return this._softVerticesSpringMaxForce;
		}
		set
		{
			if (this._softVerticesSpringMaxForce != value)
			{
				this._softVerticesSpringMaxForce = value;
				if (this.softVerticesSpringMaxForceSlider != null)
				{
					this.softVerticesSpringMaxForceSlider.value = this._softVerticesSpringMaxForce;
				}
				this.SyncSoftVerticesSpringMaxForce();
			}
		}
	}

	// Token: 0x06004F20 RID: 20256 RVA: 0x001C3010 File Offset: 0x001C1410
	protected void SyncSoftVerticesMass(float f)
	{
		this._softVerticesMass = f;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.jointMass = this._softVerticesMass;
			}
		}
	}

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x06004F21 RID: 20257 RVA: 0x001C3084 File Offset: 0x001C1484
	// (set) Token: 0x06004F22 RID: 20258 RVA: 0x001C308C File Offset: 0x001C148C
	public float softVerticesMass
	{
		get
		{
			return this._softVerticesMass;
		}
		set
		{
			if (this.softVerticesMassJSON != null)
			{
				this.softVerticesMassJSON.val = value;
			}
			else if (this._softVerticesMass != value)
			{
				this.SyncSoftVerticesMass(value);
			}
		}
	}

	// Token: 0x06004F23 RID: 20259 RVA: 0x001C30C0 File Offset: 0x001C14C0
	protected void SyncSoftVerticesBackForce(float f)
	{
		this._softVerticesBackForce = f;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.jointBackForce = this._softVerticesBackForce;
			}
		}
	}

	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x06004F24 RID: 20260 RVA: 0x001C3134 File Offset: 0x001C1534
	// (set) Token: 0x06004F25 RID: 20261 RVA: 0x001C313C File Offset: 0x001C153C
	public float softVerticesBackForce
	{
		get
		{
			return this._softVerticesBackForce;
		}
		set
		{
			if (this.softVerticesBackForceJSON != null)
			{
				this.softVerticesBackForceJSON.val = value;
			}
			else if (this._softVerticesBackForce != value)
			{
				this.SyncSoftVerticesBackForce(value);
			}
		}
	}

	// Token: 0x06004F26 RID: 20262 RVA: 0x001C3170 File Offset: 0x001C1570
	protected void SyncSoftVerticesBackForceThresholdDistance(float f)
	{
		this._softVerticesBackForceThresholdDistance = f;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.jointBackForceThresholdDistance = this._softVerticesBackForceThresholdDistance;
			}
		}
	}

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x06004F27 RID: 20263 RVA: 0x001C31E4 File Offset: 0x001C15E4
	// (set) Token: 0x06004F28 RID: 20264 RVA: 0x001C31EC File Offset: 0x001C15EC
	public float softVerticesBackForceThresholdDistance
	{
		get
		{
			return this._softVerticesBackForceThresholdDistance;
		}
		set
		{
			if (this.softVerticesBackForceThresholdDistanceJSON != null)
			{
				this.softVerticesBackForceThresholdDistanceJSON.val = value;
			}
			else if (this._softVerticesBackForceThresholdDistance != value)
			{
				this.SyncSoftVerticesBackForceThresholdDistance(value);
			}
		}
	}

	// Token: 0x06004F29 RID: 20265 RVA: 0x001C3220 File Offset: 0x001C1620
	protected void SyncSoftVerticesBackForceMaxForce(float f)
	{
		this._softVerticesBackForceMaxForce = f;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.jointBackForceMaxForce = this._softVerticesBackForceMaxForce;
			}
		}
	}

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x06004F2A RID: 20266 RVA: 0x001C3294 File Offset: 0x001C1694
	// (set) Token: 0x06004F2B RID: 20267 RVA: 0x001C329C File Offset: 0x001C169C
	public float softVerticesBackForceMaxForce
	{
		get
		{
			return this._softVerticesBackForceMaxForce;
		}
		set
		{
			if (this.softVerticesBackForceMaxForceJSON != null)
			{
				this.softVerticesBackForceMaxForceJSON.val = value;
			}
			else if (this._softVerticesBackForceMaxForce != value)
			{
				this.SyncSoftVerticesBackForceMaxForce(value);
			}
		}
	}

	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x06004F2C RID: 20268 RVA: 0x001C32CD File Offset: 0x001C16CD
	// (set) Token: 0x06004F2D RID: 20269 RVA: 0x001C32D8 File Offset: 0x001C16D8
	public bool multiplyMassByLimitMultiplier
	{
		get
		{
			return this._multiplyMassByLimitMultiplier;
		}
		set
		{
			this._multiplyMassByLimitMultiplier = value;
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				dazphysicsMeshSoftVerticesGroup.multiplyMassByLimitMultiplier = this._multiplyMassByLimitMultiplier;
			}
		}
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x001C3340 File Offset: 0x001C1740
	protected void SyncSoftVerticesUseUniformLimit()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.useUniformLimit = this._softVerticesUseUniformLimit;
			}
		}
	}

	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x06004F2F RID: 20271 RVA: 0x001C33AC File Offset: 0x001C17AC
	// (set) Token: 0x06004F30 RID: 20272 RVA: 0x001C33B4 File Offset: 0x001C17B4
	public bool softVerticesUseUniformLimit
	{
		get
		{
			return this._softVerticesUseUniformLimit;
		}
		set
		{
			if (this._softVerticesUseUniformLimit != value)
			{
				this._softVerticesUseUniformLimit = value;
				if (this.softVerticesUseUniformLimitToggle != null)
				{
					this.softVerticesUseUniformLimitToggle.isOn = this._softVerticesUseUniformLimit;
				}
				this.SyncSoftVerticesUseUniformLimit();
			}
		}
	}

	// Token: 0x06004F31 RID: 20273 RVA: 0x001C33F4 File Offset: 0x001C17F4
	protected void SyncSoftVerticesTangentLimit()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.tangentDistanceLimit = this._softVerticesTangentLimit;
				dazphysicsMeshSoftVerticesGroup.tangentNegativeDistanceLimit = this._softVerticesTangentLimit;
				dazphysicsMeshSoftVerticesGroup.tangent2DistanceLimit = this._softVerticesTangentLimit;
				dazphysicsMeshSoftVerticesGroup.tangent2NegativeDistanceLimit = this._softVerticesTangentLimit;
			}
		}
	}

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x06004F32 RID: 20274 RVA: 0x001C3484 File Offset: 0x001C1884
	// (set) Token: 0x06004F33 RID: 20275 RVA: 0x001C348C File Offset: 0x001C188C
	public float softVerticesTangentLimit
	{
		get
		{
			return this._softVerticesTangentLimit;
		}
		set
		{
			if (this._softVerticesTangentLimit != value)
			{
				this._softVerticesTangentLimit = value;
				if (this.softVerticesTangentLimitSlider != null)
				{
					this.softVerticesTangentLimitSlider.value = this._softVerticesTangentLimit;
				}
				this.SyncSoftVerticesTangentLimit();
			}
		}
	}

	// Token: 0x06004F34 RID: 20276 RVA: 0x001C34CC File Offset: 0x001C18CC
	protected void SyncSoftVerticesNormalLimit(float f)
	{
		this._softVerticesNormalLimit = f;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.normalDistanceLimit = this._softVerticesNormalLimit;
			}
		}
	}

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x06004F35 RID: 20277 RVA: 0x001C3540 File Offset: 0x001C1940
	// (set) Token: 0x06004F36 RID: 20278 RVA: 0x001C3548 File Offset: 0x001C1948
	public float softVerticesNormalLimit
	{
		get
		{
			return this._softVerticesNormalLimit;
		}
		set
		{
			if (this.softVerticesNormalLimitJSON != null)
			{
				this.softVerticesNormalLimitJSON.val = value;
			}
			else if (this._softVerticesNormalLimit != value)
			{
				this.SyncSoftVerticesNormalLimit(value);
			}
		}
	}

	// Token: 0x06004F37 RID: 20279 RVA: 0x001C357C File Offset: 0x001C197C
	protected void SyncSoftVerticesNegativeNormalLimit()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				dazphysicsMeshSoftVerticesGroup.normalNegativeDistanceLimit = this._softVerticesNegativeNormalLimit;
			}
		}
	}

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x06004F38 RID: 20280 RVA: 0x001C35E8 File Offset: 0x001C19E8
	// (set) Token: 0x06004F39 RID: 20281 RVA: 0x001C35F0 File Offset: 0x001C19F0
	public float softVerticesNegativeNormalLimit
	{
		get
		{
			return this._softVerticesNegativeNormalLimit;
		}
		set
		{
			if (this._softVerticesNegativeNormalLimit != value)
			{
				this._softVerticesNegativeNormalLimit = value;
				if (this.softVerticesNegativeNormalLimitSlider != null)
				{
					this.softVerticesNegativeNormalLimitSlider.value = this._softVerticesNegativeNormalLimit;
				}
				this.SyncSoftVerticesNegativeNormalLimit();
			}
		}
	}

	// Token: 0x06004F3A RID: 20282 RVA: 0x001C362D File Offset: 0x001C1A2D
	protected void SyncSoftVerticesUseAutoColliderRadius(bool b)
	{
		this._softVerticesUseAutoColliderRadius = b;
		if (this.updateEnabled)
		{
			this.SoftVerticesSetAutoRadius();
		}
	}

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x06004F3B RID: 20283 RVA: 0x001C3647 File Offset: 0x001C1A47
	// (set) Token: 0x06004F3C RID: 20284 RVA: 0x001C364F File Offset: 0x001C1A4F
	public bool softVerticesUseAutoColliderRadius
	{
		get
		{
			return this._softVerticesUseAutoColliderRadius;
		}
		set
		{
			if (this.softVerticesUseAutoColliderRadiusJSON != null)
			{
				this.softVerticesUseAutoColliderRadiusJSON.val = value;
			}
			else if (this._softVerticesUseAutoColliderRadius != value)
			{
				this.SyncSoftVerticesUseAutoColliderRadius(value);
			}
		}
	}

	// Token: 0x06004F3D RID: 20285 RVA: 0x001C3680 File Offset: 0x001C1A80
	public void SoftVerticesSetAutoRadiusFast(Vector3[] verts)
	{
		if (this.skin != null && this._softVerticesUseAutoColliderRadius && this.softVerticesAutoColliderVertex1 != -1 && this.softVerticesAutoColliderVertex2 != -1)
		{
			float num = (verts[this.softVerticesAutoColliderVertex1] - verts[this.softVerticesAutoColliderVertex2]).magnitude * this.softVerticesAutoColliderRadiusMultiplier + this.softVerticesAutoColliderRadiusOffset;
			if (num < this.softVerticesAutoColliderMinRadius)
			{
				num = this.softVerticesAutoColliderMinRadius;
			}
			if (num > this.softVerticesAutoColliderMaxRadius)
			{
				num = this.softVerticesAutoColliderMaxRadius;
			}
			int num2 = Mathf.RoundToInt(num * 1000f);
			int num3 = Mathf.RoundToInt(this._softVerticesColliderRadius * 1000f);
			if (num2 != num3)
			{
				this._softVerticesColliderRadius = (float)num2 / 1000f;
				foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
				{
					if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
					{
						if (dazphysicsMeshSoftVerticesGroup.useParentColliderSettings)
						{
							dazphysicsMeshSoftVerticesGroup.colliderRadiusNoSync = this._softVerticesColliderRadius;
							dazphysicsMeshSoftVerticesGroup.colliderNormalOffsetNoSync = this._softVerticesColliderRadius;
						}
						if (dazphysicsMeshSoftVerticesGroup.useParentColliderSettingsForSecondCollider)
						{
							dazphysicsMeshSoftVerticesGroup.secondColliderRadiusNoSync = this._softVerticesColliderRadius;
							dazphysicsMeshSoftVerticesGroup.secondColliderNormalOffsetNoSync = this._softVerticesColliderRadius;
						}
						if (dazphysicsMeshSoftVerticesGroup.colliderSyncDirty)
						{
							dazphysicsMeshSoftVerticesGroup.SyncCollidersThreaded();
						}
					}
				}
			}
		}
	}

	// Token: 0x06004F3E RID: 20286 RVA: 0x001C3810 File Offset: 0x001C1C10
	public void SoftVerticesSetAutoRadiusFinishFast()
	{
		if (this._softVerticesUseAutoColliderRadius && this.softVerticesColliderRadiusJSON != null)
		{
			this.softVerticesColliderRadiusJSON.valNoCallback = this._softVerticesColliderRadius;
		}
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings && dazphysicsMeshSoftVerticesGroup.colliderSyncDirty)
			{
				dazphysicsMeshSoftVerticesGroup.SyncCollidersThreadedFinish();
			}
		}
	}

	// Token: 0x06004F3F RID: 20287 RVA: 0x001C38A8 File Offset: 0x001C1CA8
	protected void SoftVerticesSetAutoRadius()
	{
		if (this.skin != null && this.softVerticesUseAutoColliderRadius && this.softVerticesAutoColliderVertex1 != -1 && this.softVerticesAutoColliderVertex2 != -1)
		{
			Vector3[] visibleMorphedUVVertices = this.skin.dazMesh.visibleMorphedUVVertices;
			float num = (visibleMorphedUVVertices[this.softVerticesAutoColliderVertex1] - visibleMorphedUVVertices[this.softVerticesAutoColliderVertex2]).magnitude * this.softVerticesAutoColliderRadiusMultiplier + this.softVerticesAutoColliderRadiusOffset;
			if (num < this.softVerticesAutoColliderMinRadius)
			{
				num = this.softVerticesAutoColliderMinRadius;
			}
			if (num > this.softVerticesAutoColliderMaxRadius)
			{
				num = this.softVerticesAutoColliderMaxRadius;
			}
			this.softVerticesColliderRadius = num;
		}
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x001C3968 File Offset: 0x001C1D68
	protected void SyncSoftVerticesColliderRadius(float f)
	{
		if (!this._softVerticesUseAutoColliderRadius)
		{
			this._softVerticesColliderRadius = f;
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
				{
					if (dazphysicsMeshSoftVerticesGroup.useParentColliderSettings)
					{
						dazphysicsMeshSoftVerticesGroup.colliderRadiusNoSync = this._softVerticesColliderRadius;
						dazphysicsMeshSoftVerticesGroup.colliderNormalOffsetNoSync = this._softVerticesColliderRadius;
					}
					if (dazphysicsMeshSoftVerticesGroup.useParentColliderSettingsForSecondCollider)
					{
						dazphysicsMeshSoftVerticesGroup.secondColliderRadiusNoSync = this._softVerticesColliderRadius;
						dazphysicsMeshSoftVerticesGroup.secondColliderNormalOffsetNoSync = this._softVerticesColliderRadius;
					}
					if (dazphysicsMeshSoftVerticesGroup.colliderSyncDirty)
					{
						dazphysicsMeshSoftVerticesGroup.SyncColliders();
					}
				}
			}
		}
	}

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x06004F41 RID: 20289 RVA: 0x001C3A34 File Offset: 0x001C1E34
	// (set) Token: 0x06004F42 RID: 20290 RVA: 0x001C3A3C File Offset: 0x001C1E3C
	public float softVerticesColliderRadius
	{
		get
		{
			return this._softVerticesColliderRadius;
		}
		set
		{
			if (this.softVerticesColliderRadiusJSON != null)
			{
				this.softVerticesColliderRadiusJSON.val = value;
			}
			else if (this._softVerticesColliderRadius != value)
			{
				this.SyncSoftVerticesColliderRadius(value);
			}
		}
	}

	// Token: 0x06004F43 RID: 20291 RVA: 0x001C3A70 File Offset: 0x001C1E70
	protected void SyncSoftVerticesColliderAdditionalNormalOffset(float f)
	{
		this._softVerticesColliderAdditionalNormalOffset = f;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useParentSettings)
			{
				if (dazphysicsMeshSoftVerticesGroup.useParentColliderSettings)
				{
					dazphysicsMeshSoftVerticesGroup.colliderAdditionalNormalOffset = this._softVerticesColliderAdditionalNormalOffset;
				}
				if (dazphysicsMeshSoftVerticesGroup.useParentColliderSettingsForSecondCollider)
				{
					dazphysicsMeshSoftVerticesGroup.secondColliderAdditionalNormalOffset = this._softVerticesColliderAdditionalNormalOffset;
				}
			}
		}
	}

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x06004F44 RID: 20292 RVA: 0x001C3B08 File Offset: 0x001C1F08
	// (set) Token: 0x06004F45 RID: 20293 RVA: 0x001C3B10 File Offset: 0x001C1F10
	public float softVerticesColliderAdditionalNormalOffset
	{
		get
		{
			return this._softVerticesColliderAdditionalNormalOffset;
		}
		set
		{
			if (this.softVerticesColliderAdditionalNormalOffsetJSON != null)
			{
				this.softVerticesColliderAdditionalNormalOffsetJSON.val = value;
			}
			else if (this._softVerticesColliderAdditionalNormalOffset != value)
			{
				this.SyncSoftVerticesColliderAdditionalNormalOffset(value);
			}
		}
	}

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x06004F46 RID: 20294 RVA: 0x001C3B41 File Offset: 0x001C1F41
	// (set) Token: 0x06004F47 RID: 20295 RVA: 0x001C3B4C File Offset: 0x001C1F4C
	public int numPredictionFrames
	{
		get
		{
			return this._numPredictionFrames;
		}
		set
		{
			if (this._numPredictionFrames != value)
			{
				this._numPredictionFrames = value;
				foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
				{
					dazphysicsMeshSoftVerticesGroup.numPredictionFrames = this._numPredictionFrames;
				}
			}
		}
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x001C3BC0 File Offset: 0x001C1FC0
	public int AddHardVerticesGroup()
	{
		DAZPhysicsMeshHardVerticesGroup item = new DAZPhysicsMeshHardVerticesGroup();
		this._hardVerticesGroups.Add(item);
		int num = this._hardVerticesGroups.Count - 1;
		this._currentHardVerticesGroupIndex = num;
		return num;
	}

	// Token: 0x06004F49 RID: 20297 RVA: 0x001C3BF8 File Offset: 0x001C1FF8
	public void RemoveHardVerticesGroup(int index)
	{
		DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup = this._hardVerticesGroups[index];
		for (int i = 0; i < dazphysicsMeshHardVerticesGroup.vertices.Length; i++)
		{
			int key = dazphysicsMeshHardVerticesGroup.vertices[i];
			this._hardTargetVerticesDict.Remove(key);
		}
		this._hardVerticesGroups.RemoveAt(index);
		if (this._currentHardVerticesGroupIndex >= this._hardVerticesGroups.Count)
		{
			this._currentHardVerticesGroupIndex = this._hardVerticesGroups.Count - 1;
		}
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x001C3C78 File Offset: 0x001C2078
	public void MoveHardVerticesGroup(int fromindex, int toindex)
	{
		if (toindex >= 0 && toindex < this._hardVerticesGroups.Count)
		{
			DAZPhysicsMeshHardVerticesGroup item = this._hardVerticesGroups[fromindex];
			this._hardVerticesGroups.RemoveAt(fromindex);
			this._hardVerticesGroups.Insert(toindex, item);
			if (this._currentHardVerticesGroupIndex == fromindex)
			{
				this._currentHardVerticesGroupIndex = toindex;
			}
		}
	}

	// Token: 0x06004F4B RID: 20299 RVA: 0x001C3CD8 File Offset: 0x001C20D8
	public int AddSoftVerticesGroup()
	{
		DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = new DAZPhysicsMeshSoftVerticesGroup();
		this._softVerticesGroups.Add(dazphysicsMeshSoftVerticesGroup);
		dazphysicsMeshSoftVerticesGroup.parent = this;
		int num = this._softVerticesGroups.Count - 1;
		this._currentSoftVerticesGroupIndex = num;
		return num;
	}

	// Token: 0x06004F4C RID: 20300 RVA: 0x001C3D14 File Offset: 0x001C2114
	public void RemoveSoftVerticesGroup(int index)
	{
		DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this._softVerticesGroups[index];
		for (int i = 0; i < dazphysicsMeshSoftVerticesGroup.softVerticesSets.Count; i++)
		{
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = dazphysicsMeshSoftVerticesGroup.softVerticesSets[i];
			if (dazphysicsMeshSoftVerticesSet.targetVertex != -1)
			{
				this._softTargetVerticesDict.Remove(dazphysicsMeshSoftVerticesSet.targetVertex);
			}
			List<DAZPhysicsMeshSoftVerticesSet> ssl;
			if (dazphysicsMeshSoftVerticesSet.anchorVertex != -1 && this._softAnchorVerticesDict.TryGetValue(dazphysicsMeshSoftVerticesSet.anchorVertex, out ssl))
			{
				this.RemoveSoftAnchor(ssl, dazphysicsMeshSoftVerticesSet);
			}
			for (int j = 0; j < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; j++)
			{
				this._softInfluenceVerticesDict.Remove(dazphysicsMeshSoftVerticesSet.influenceVertices[j]);
			}
		}
		this._softVerticesGroups.RemoveAt(index);
		if (this._currentSoftVerticesGroupIndex >= this._softVerticesGroups.Count)
		{
			this._currentSoftVerticesGroupIndex = this._softVerticesGroups.Count - 1;
		}
	}

	// Token: 0x06004F4D RID: 20301 RVA: 0x001C3E0C File Offset: 0x001C220C
	public void RemoveSoftVerticesSet(int groupIndex, int setIndex)
	{
		DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this._softVerticesGroups[groupIndex];
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = dazphysicsMeshSoftVerticesGroup.softVerticesSets[setIndex];
		if (dazphysicsMeshSoftVerticesSet.targetVertex != -1)
		{
			this._softTargetVerticesDict.Remove(dazphysicsMeshSoftVerticesSet.targetVertex);
		}
		List<DAZPhysicsMeshSoftVerticesSet> ssl;
		if (dazphysicsMeshSoftVerticesSet.anchorVertex != -1 && this._softAnchorVerticesDict.TryGetValue(dazphysicsMeshSoftVerticesSet.anchorVertex, out ssl))
		{
			this.RemoveSoftAnchor(ssl, dazphysicsMeshSoftVerticesSet);
		}
		for (int i = 0; i < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; i++)
		{
			this._softInfluenceVerticesDict.Remove(dazphysicsMeshSoftVerticesSet.influenceVertices[i]);
		}
		if (this._softSetToGroupDict.ContainsKey(dazphysicsMeshSoftVerticesSet))
		{
			this._softSetToGroupDict.Remove(dazphysicsMeshSoftVerticesSet);
		}
		dazphysicsMeshSoftVerticesGroup.RemoveSet(setIndex);
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001C3ED0 File Offset: 0x001C22D0
	public void MoveSoftVerticesGroup(int fromindex, int toindex)
	{
		if (toindex >= 0 && toindex < this._softVerticesGroups.Count)
		{
			DAZPhysicsMeshSoftVerticesGroup item = this._softVerticesGroups[fromindex];
			this._softVerticesGroups.RemoveAt(fromindex);
			this._softVerticesGroups.Insert(toindex, item);
			if (this._currentSoftVerticesGroupIndex == fromindex)
			{
				this._currentSoftVerticesGroupIndex = toindex;
			}
		}
	}

	// Token: 0x06004F4F RID: 20303 RVA: 0x001C3F30 File Offset: 0x001C2330
	public int AddColliderGroup()
	{
		DAZPhysicsMeshColliderGroup item = new DAZPhysicsMeshColliderGroup();
		this._colliderGroups.Add(item);
		int num = this._colliderGroups.Count - 1;
		this._currentColliderGroupIndex = num;
		return num;
	}

	// Token: 0x06004F50 RID: 20304 RVA: 0x001C3F65 File Offset: 0x001C2365
	public void RemoveColliderGroup(int index)
	{
		this._colliderGroups.RemoveAt(index);
		if (this._currentColliderGroupIndex >= this._colliderGroups.Count)
		{
			this._currentColliderGroupIndex = this._colliderGroups.Count - 1;
		}
	}

	// Token: 0x06004F51 RID: 20305 RVA: 0x001C3F9C File Offset: 0x001C239C
	public void MoveColliderGroup(int fromindex, int toindex)
	{
		if (toindex >= 0 && toindex < this._colliderGroups.Count)
		{
			DAZPhysicsMeshColliderGroup item = this._colliderGroups[fromindex];
			this._colliderGroups.RemoveAt(fromindex);
			this._colliderGroups.Insert(toindex, item);
			if (this._currentColliderGroupIndex == fromindex)
			{
				this._currentColliderGroupIndex = toindex;
			}
		}
	}

	// Token: 0x06004F52 RID: 20306 RVA: 0x001C3FFC File Offset: 0x001C23FC
	public DAZPhysicsMeshSoftVerticesSet GetSoftSetByID(string uid)
	{
		if (this._softVerticesGroups != null)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
			{
				DAZPhysicsMeshSoftVerticesSet setByID = dazphysicsMeshSoftVerticesGroup.GetSetByID(uid, true);
				if (setByID != null)
				{
					return setByID;
				}
			}
		}
		return null;
	}

	// Token: 0x06004F53 RID: 20307 RVA: 0x001C4078 File Offset: 0x001C2478
	protected void InitCaches(bool force = false)
	{
		if (this._hardVerticesGroups == null)
		{
			this._hardVerticesGroups = new List<DAZPhysicsMeshHardVerticesGroup>();
		}
		if (this._softVerticesGroups == null)
		{
			this._softVerticesGroups = new List<DAZPhysicsMeshSoftVerticesGroup>();
		}
		if (this._colliderGroups == null)
		{
			this._colliderGroups = new List<DAZPhysicsMeshColliderGroup>();
		}
		if (this._hardTargetVerticesDict == null || force)
		{
			this._hardTargetVerticesDict = new Dictionary<int, DAZPhysicsMeshHardVerticesGroup>();
			if (this._hardVerticesGroups != null)
			{
				foreach (DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup in this._hardVerticesGroups)
				{
					foreach (int key in dazphysicsMeshHardVerticesGroup.vertices)
					{
						this._hardTargetVerticesDict.Add(key, dazphysicsMeshHardVerticesGroup);
					}
				}
			}
		}
		if (this.skin != null && this.skin.dazMesh != null)
		{
			this._uvVertToBaseVertDict = this.skin.dazMesh.uvVertToBaseVert;
		}
		else
		{
			this._uvVertToBaseVertDict = new Dictionary<int, int>();
		}
		if (this._softSetToGroupDict == null || force)
		{
			this._softSetToGroupDict = new Dictionary<DAZPhysicsMeshSoftVerticesSet, DAZPhysicsMeshSoftVerticesGroup>();
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
			{
				foreach (DAZPhysicsMeshSoftVerticesSet key2 in dazphysicsMeshSoftVerticesGroup.softVerticesSets)
				{
					this._softSetToGroupDict.Add(key2, dazphysicsMeshSoftVerticesGroup);
				}
			}
		}
		if (this._softTargetVerticesDict == null || force)
		{
			this._softTargetVerticesDict = new Dictionary<int, DAZPhysicsMeshSoftVerticesSet>();
			if (this._softVerticesGroups != null)
			{
				foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup2 in this._softVerticesGroups)
				{
					dazphysicsMeshSoftVerticesGroup2.parent = this;
					foreach (DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet in dazphysicsMeshSoftVerticesGroup2.softVerticesSets)
					{
						if (dazphysicsMeshSoftVerticesSet.targetVertex != -1 && !this._softTargetVerticesDict.ContainsKey(dazphysicsMeshSoftVerticesSet.targetVertex))
						{
							this._softTargetVerticesDict.Add(dazphysicsMeshSoftVerticesSet.targetVertex, dazphysicsMeshSoftVerticesSet);
						}
					}
				}
			}
		}
		if (this._softAnchorVerticesDict == null || force)
		{
			this._softAnchorVerticesDict = new Dictionary<int, List<DAZPhysicsMeshSoftVerticesSet>>();
			if (this._softVerticesGroups != null)
			{
				foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup3 in this._softVerticesGroups)
				{
					foreach (DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet2 in dazphysicsMeshSoftVerticesGroup3.softVerticesSets)
					{
						if (dazphysicsMeshSoftVerticesSet2.anchorVertex != -1)
						{
							List<DAZPhysicsMeshSoftVerticesSet> list;
							if (this._softAnchorVerticesDict.TryGetValue(dazphysicsMeshSoftVerticesSet2.anchorVertex, out list))
							{
								list.Add(dazphysicsMeshSoftVerticesSet2);
							}
							else
							{
								list = new List<DAZPhysicsMeshSoftVerticesSet>();
								list.Add(dazphysicsMeshSoftVerticesSet2);
								this._softAnchorVerticesDict.Add(dazphysicsMeshSoftVerticesSet2.anchorVertex, list);
							}
						}
					}
				}
			}
		}
		if (this._softInfluenceVerticesDict == null || force)
		{
			this._softInfluenceVerticesDict = new Dictionary<int, DAZPhysicsMeshSoftVerticesSet>();
			if (this._softVerticesGroups != null)
			{
				foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup4 in this._softVerticesGroups)
				{
					foreach (DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet3 in dazphysicsMeshSoftVerticesGroup4.softVerticesSets)
					{
						foreach (int key3 in dazphysicsMeshSoftVerticesSet3.influenceVertices)
						{
							if (!this._softInfluenceVerticesDict.ContainsKey(key3))
							{
								this._softInfluenceVerticesDict.Add(key3, dazphysicsMeshSoftVerticesSet3);
							}
						}
					}
				}
			}
		}
		if (this._softVerticesInGroupDict == null || force)
		{
			this._softVerticesInGroupDict = new Dictionary<string, bool>();
			if (this._softVerticesGroups != null)
			{
				foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup5 in this._softVerticesGroups)
				{
					foreach (DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet4 in dazphysicsMeshSoftVerticesGroup5.softVerticesSets)
					{
						if (dazphysicsMeshSoftVerticesSet4.targetVertex != -1)
						{
							string key4 = dazphysicsMeshSoftVerticesGroup5.name + ":" + dazphysicsMeshSoftVerticesSet4.targetVertex.ToString();
							if (!this._softVerticesInGroupDict.ContainsKey(key4))
							{
								this._softVerticesInGroupDict.Add(key4, true);
							}
						}
						if (dazphysicsMeshSoftVerticesSet4.anchorVertex != -1)
						{
							string key5 = dazphysicsMeshSoftVerticesGroup5.name + ":" + dazphysicsMeshSoftVerticesSet4.anchorVertex.ToString();
							if (!this._softVerticesInGroupDict.ContainsKey(key5))
							{
								this._softVerticesInGroupDict.Add(key5, true);
							}
						}
						foreach (int num in dazphysicsMeshSoftVerticesSet4.influenceVertices)
						{
							string key6 = dazphysicsMeshSoftVerticesGroup5.name + ":" + num.ToString();
							if (!this._softVerticesInGroupDict.ContainsKey(key6))
							{
								this._softVerticesInGroupDict.Add(key6, true);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06004F54 RID: 20308 RVA: 0x001C47B0 File Offset: 0x001C2BB0
	public bool IsHardTargetVertex(int vid)
	{
		return this._hardTargetVerticesDict.ContainsKey(vid);
	}

	// Token: 0x06004F55 RID: 20309 RVA: 0x001C47C0 File Offset: 0x001C2BC0
	public DAZPhysicsMeshHardVerticesGroup GetHardVertexGroup(int vid)
	{
		DAZPhysicsMeshHardVerticesGroup result;
		if (this._hardTargetVerticesDict.TryGetValue(vid, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004F56 RID: 20310 RVA: 0x001C47E3 File Offset: 0x001C2BE3
	public bool IsSoftTargetVertex(int vid)
	{
		return this._softTargetVerticesDict.ContainsKey(vid);
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x001C47F4 File Offset: 0x001C2BF4
	public float GetSoftTargetVertexSpringMultipler(int vid)
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			return dazphysicsMeshSoftVerticesSet.springMultiplier;
		}
		return 0f;
	}

	// Token: 0x06004F58 RID: 20312 RVA: 0x001C4820 File Offset: 0x001C2C20
	public float GetSoftTargetVertexSizeMultipler(int vid)
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			return dazphysicsMeshSoftVerticesSet.sizeMultiplier;
		}
		return 0f;
	}

	// Token: 0x06004F59 RID: 20313 RVA: 0x001C484C File Offset: 0x001C2C4C
	public float GetSoftTargetVertexLimitMultipler(int vid)
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			return dazphysicsMeshSoftVerticesSet.limitMultiplier;
		}
		return 0f;
	}

	// Token: 0x06004F5A RID: 20314 RVA: 0x001C4878 File Offset: 0x001C2C78
	public bool IsSoftAnchorVertex(int vid)
	{
		return this._softAnchorVerticesDict.ContainsKey(vid);
	}

	// Token: 0x06004F5B RID: 20315 RVA: 0x001C4888 File Offset: 0x001C2C88
	public bool IsSoftInfluenceVertex(int vid)
	{
		bool result = false;
		List<DAZPhysicsMeshSoftVerticesSet> list;
		if (this._softInfluenceVerticesDict.ContainsKey(vid))
		{
			result = true;
		}
		else if (this._softAnchorVerticesDict.TryGetValue(vid, out list))
		{
			foreach (DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet in list)
			{
				if (dazphysicsMeshSoftVerticesSet.autoInfluenceAnchor)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06004F5C RID: 20316 RVA: 0x001C4918 File Offset: 0x001C2D18
	public bool IsVertexInCurrentSoftSet(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null)
		{
			DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
			List<DAZPhysicsMeshSoftVerticesSet> list;
			if (this._softInfluenceVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
			{
				if (dazphysicsMeshSoftVerticesSet == currentSet)
				{
					return true;
				}
			}
			else if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
			{
				if (dazphysicsMeshSoftVerticesSet == currentSet)
				{
					return true;
				}
			}
			else if (this._softAnchorVerticesDict.TryGetValue(vid, out list) && list.Contains(currentSet))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004F5D RID: 20317 RVA: 0x001C49A0 File Offset: 0x001C2DA0
	public bool IsVertexInCurrentSoftGroup(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null)
		{
			string key = currentSoftVerticesGroup.name + ":" + vid.ToString();
			if (this._softVerticesInGroupDict.ContainsKey(key))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004F5E RID: 20318 RVA: 0x001C49EC File Offset: 0x001C2DEC
	public bool IsVertexCurrentSoftSetAnchor(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null)
		{
			DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
			List<DAZPhysicsMeshSoftVerticesSet> list;
			if (this._softAnchorVerticesDict.TryGetValue(vid, out list) && list.Contains(currentSet))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004F5F RID: 20319 RVA: 0x001C4A30 File Offset: 0x001C2E30
	public bool IsVertexInCurrentHardGroup(int vid)
	{
		DAZPhysicsMeshHardVerticesGroup currentHardVerticesGroup = this.currentHardVerticesGroup;
		DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup;
		return currentHardVerticesGroup != null && this._hardTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshHardVerticesGroup) && currentHardVerticesGroup == dazphysicsMeshHardVerticesGroup;
	}

	// Token: 0x06004F60 RID: 20320 RVA: 0x001C4A68 File Offset: 0x001C2E68
	public void DrawLinkLines()
	{
		if (this._softVerticesGroups != null && this.skin != null && this.skin.dazMesh != null)
		{
			Vector3[] array;
			Vector3[] array2;
			if (Application.isPlaying)
			{
				array = this.skin.drawVerts;
				array2 = this.skin.drawNormals;
			}
			else
			{
				array = this.skin.dazMesh.morphedUVVertices;
				array2 = this.skin.dazMesh.morphedUVNormals;
			}
			for (int i = 0; i < this._softVerticesGroups.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup = this._softVerticesGroups[i];
				if ((!this._showCurrentSoftGroupOnly && !this._showCurrentSoftSetOnly) || i == this._currentSoftVerticesGroupIndex)
				{
					if (dazphysicsMeshSoftVerticesGroup.useLinkJoints)
					{
						foreach (DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet in dazphysicsMeshSoftVerticesGroup.softVerticesSets)
						{
							if (!this._showCurrentSoftSetOnly || dazphysicsMeshSoftVerticesSet == dazphysicsMeshSoftVerticesGroup.currentSet)
							{
								if (dazphysicsMeshSoftVerticesSet.targetVertex != -1 && dazphysicsMeshSoftVerticesSet.anchorVertex != -1)
								{
									Vector3 vector = array[dazphysicsMeshSoftVerticesSet.targetVertex];
									if (dazphysicsMeshSoftVerticesSet.links != null)
									{
										for (int j = 0; j < dazphysicsMeshSoftVerticesSet.links.Count; j++)
										{
											DAZPhysicsMeshSoftVerticesSet softSetByID = this.GetSoftSetByID(dazphysicsMeshSoftVerticesSet.links[j]);
											if (softSetByID == null || softSetByID.targetVertex == -1)
											{
												Debug.LogError(string.Concat(new string[]
												{
													"Soft vertices set ",
													dazphysicsMeshSoftVerticesSet.uid,
													" has broken link to ",
													dazphysicsMeshSoftVerticesSet.links[j],
													" Removing."
												}));
												dazphysicsMeshSoftVerticesSet.links.RemoveAt(j);
												break;
											}
											Debug.DrawLine(vector, array[softSetByID.targetVertex], Color.yellow);
											Debug.DrawLine((vector + 3f * array[softSetByID.targetVertex]) * 0.25f + array2[dazphysicsMeshSoftVerticesSet.targetVertex] * 0.003f, array[softSetByID.targetVertex], Color.green);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06004F61 RID: 20321 RVA: 0x001C4D2C File Offset: 0x001C312C
	protected int FindOrCreateHardGroup(DAZBone db)
	{
		DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup;
		for (int i = 0; i < this._hardVerticesGroups.Count; i++)
		{
			dazphysicsMeshHardVerticesGroup = this._hardVerticesGroups[i];
			if (dazphysicsMeshHardVerticesGroup.bone == db)
			{
				return i;
			}
		}
		int num = this.AddHardVerticesGroup();
		dazphysicsMeshHardVerticesGroup = this._hardVerticesGroups[num];
		dazphysicsMeshHardVerticesGroup.bone = db;
		dazphysicsMeshHardVerticesGroup.name = db.name;
		return num;
	}

	// Token: 0x06004F62 RID: 20322 RVA: 0x001C4DA0 File Offset: 0x001C31A0
	public void ToggleHardVertices(int vid, bool auto = false)
	{
		if (auto)
		{
			DAZBone dazbone = this.skin.strongestDAZBone[vid];
			if (dazbone == null)
			{
				Debug.LogError("Could not find DAZBone for vertex " + vid);
				return;
			}
			this._currentHardVerticesGroupIndex = this.FindOrCreateHardGroup(dazbone);
		}
		DAZPhysicsMeshHardVerticesGroup currentHardVerticesGroup = this.currentHardVerticesGroup;
		if (currentHardVerticesGroup != null)
		{
			DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup;
			if (this._hardTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshHardVerticesGroup))
			{
				if (dazphysicsMeshHardVerticesGroup != currentHardVerticesGroup)
				{
					dazphysicsMeshHardVerticesGroup.RemoveVertex(vid);
					this._hardTargetVerticesDict.Remove(vid);
					currentHardVerticesGroup.AddVertex(vid);
					this._hardTargetVerticesDict.Add(vid, currentHardVerticesGroup);
				}
				else
				{
					dazphysicsMeshHardVerticesGroup.RemoveVertex(vid);
					this._hardTargetVerticesDict.Remove(vid);
				}
			}
			else
			{
				currentHardVerticesGroup.AddVertex(vid);
				this._hardTargetVerticesDict.Add(vid, currentHardVerticesGroup);
			}
		}
	}

	// Token: 0x06004F63 RID: 20323 RVA: 0x001C4E74 File Offset: 0x001C3274
	public void OnHardVertices(int vid, bool auto = false)
	{
		if (auto)
		{
			DAZBone dazbone = this.skin.strongestDAZBone[vid];
			if (dazbone == null)
			{
				Debug.LogError("Could not find DAZBone for vertex " + vid);
				return;
			}
			this._currentHardVerticesGroupIndex = this.FindOrCreateHardGroup(dazbone);
		}
		DAZPhysicsMeshHardVerticesGroup currentHardVerticesGroup = this.currentHardVerticesGroup;
		if (currentHardVerticesGroup != null)
		{
			DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup;
			if (this._hardTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshHardVerticesGroup))
			{
				if (dazphysicsMeshHardVerticesGroup != currentHardVerticesGroup)
				{
					dazphysicsMeshHardVerticesGroup.RemoveVertex(vid);
					this._hardTargetVerticesDict.Remove(vid);
					currentHardVerticesGroup.AddVertex(vid);
					this._hardTargetVerticesDict.Add(vid, currentHardVerticesGroup);
				}
			}
			else
			{
				currentHardVerticesGroup.AddVertex(vid);
				this._hardTargetVerticesDict.Add(vid, currentHardVerticesGroup);
			}
		}
	}

	// Token: 0x06004F64 RID: 20324 RVA: 0x001C4F2C File Offset: 0x001C332C
	public void OffHardVertices(int vid, bool auto = false)
	{
		DAZPhysicsMeshHardVerticesGroup currentHardVerticesGroup = this.currentHardVerticesGroup;
		DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup;
		if (currentHardVerticesGroup != null && this._hardTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshHardVerticesGroup))
		{
			if (auto)
			{
				dazphysicsMeshHardVerticesGroup.RemoveVertex(vid);
				this._hardTargetVerticesDict.Remove(vid);
			}
			else if (dazphysicsMeshHardVerticesGroup == currentHardVerticesGroup)
			{
				dazphysicsMeshHardVerticesGroup.RemoveVertex(vid);
				this._hardTargetVerticesDict.Remove(vid);
			}
		}
	}

	// Token: 0x06004F65 RID: 20325 RVA: 0x001C4F94 File Offset: 0x001C3394
	public void ToggleSoftTargetVertex(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null)
		{
			DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
			if (currentSet != null)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
				if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
				{
					if (dazphysicsMeshSoftVerticesSet != currentSet)
					{
						dazphysicsMeshSoftVerticesSet.targetVertex = -1;
						this._softTargetVerticesDict.Remove(vid);
						if (currentSet.targetVertex != -1)
						{
							this._softTargetVerticesDict.Remove(currentSet.targetVertex);
						}
						currentSet.targetVertex = vid;
						this._softTargetVerticesDict.Add(vid, currentSet);
					}
					else
					{
						dazphysicsMeshSoftVerticesSet.targetVertex = -1;
						this._softTargetVerticesDict.Remove(vid);
					}
				}
				else
				{
					if (currentSet.targetVertex != -1)
					{
						this._softTargetVerticesDict.Remove(currentSet.targetVertex);
					}
					currentSet.targetVertex = vid;
					this._softTargetVerticesDict.Add(vid, currentSet);
				}
			}
		}
	}

	// Token: 0x06004F66 RID: 20326 RVA: 0x001C506C File Offset: 0x001C346C
	protected bool RemoveSoftAnchor(List<DAZPhysicsMeshSoftVerticesSet> ssl, DAZPhysicsMeshSoftVerticesSet ss)
	{
		if (ssl.Contains(ss))
		{
			ssl.Remove(ss);
			int anchorVertex = ss.anchorVertex;
			ss.anchorVertex = -1;
			if (ssl.Count == 0)
			{
				this._softAnchorVerticesDict.Remove(anchorVertex);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06004F67 RID: 20327 RVA: 0x001C50B8 File Offset: 0x001C34B8
	protected void AddSoftAnchor(int vid, DAZPhysicsMeshSoftVerticesSet ss)
	{
		List<DAZPhysicsMeshSoftVerticesSet> list;
		if (ss.anchorVertex != -1 && this._softAnchorVerticesDict.TryGetValue(ss.anchorVertex, out list))
		{
			this.RemoveSoftAnchor(list, ss);
		}
		ss.anchorVertex = vid;
		if (this._softAnchorVerticesDict.TryGetValue(vid, out list))
		{
			if (!list.Contains(ss))
			{
				list.Add(ss);
			}
		}
		else
		{
			list = new List<DAZPhysicsMeshSoftVerticesSet>();
			list.Add(ss);
			this._softAnchorVerticesDict.Add(vid, list);
		}
	}

	// Token: 0x06004F68 RID: 20328 RVA: 0x001C5140 File Offset: 0x001C3540
	public void ToggleSoftAnchorVertex(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null)
		{
			DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
			if (currentSet != null)
			{
				List<DAZPhysicsMeshSoftVerticesSet> ssl;
				if (this._softAnchorVerticesDict.TryGetValue(vid, out ssl))
				{
					if (!this.RemoveSoftAnchor(ssl, currentSet))
					{
						this.AddSoftAnchor(vid, currentSet);
					}
				}
				else
				{
					this.AddSoftAnchor(vid, currentSet);
				}
			}
		}
	}

	// Token: 0x06004F69 RID: 20329 RVA: 0x001C519C File Offset: 0x001C359C
	public void ToggleSoftInfluenceVertices(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null)
		{
			DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
			if (currentSet != null)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
				if (this._softInfluenceVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
				{
					if (dazphysicsMeshSoftVerticesSet != currentSet)
					{
						dazphysicsMeshSoftVerticesSet.RemoveInfluenceVertex(vid);
						this._softInfluenceVerticesDict.Remove(vid);
						currentSet.AddInfluenceVertex(vid);
						this._softInfluenceVerticesDict.Add(vid, currentSet);
					}
					else
					{
						dazphysicsMeshSoftVerticesSet.RemoveInfluenceVertex(vid);
						this._softInfluenceVerticesDict.Remove(vid);
					}
				}
				else
				{
					currentSet.AddInfluenceVertex(vid);
					this._softInfluenceVerticesDict.Add(vid, currentSet);
				}
			}
		}
	}

	// Token: 0x06004F6A RID: 20330 RVA: 0x001C5238 File Offset: 0x001C3638
	public void OnSoftInfluenceVertices(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null && !this._softTargetVerticesDict.ContainsKey(vid))
		{
			DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
			if (currentSet != null)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
				if (this._softInfluenceVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
				{
					if (dazphysicsMeshSoftVerticesSet != currentSet)
					{
						dazphysicsMeshSoftVerticesSet.RemoveInfluenceVertex(vid);
						this._softInfluenceVerticesDict.Remove(vid);
						currentSet.AddInfluenceVertex(vid);
						this._softInfluenceVerticesDict.Add(vid, currentSet);
					}
				}
				else
				{
					currentSet.AddInfluenceVertex(vid);
					this._softInfluenceVerticesDict.Add(vid, currentSet);
				}
			}
		}
	}

	// Token: 0x06004F6B RID: 20331 RVA: 0x001C52CC File Offset: 0x001C36CC
	public void OffSoftInfluenceVertices(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup != null)
		{
			DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
			if (currentSet != null && this._softInfluenceVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet) && dazphysicsMeshSoftVerticesSet == currentSet)
			{
				dazphysicsMeshSoftVerticesSet.RemoveInfluenceVertex(vid);
				this._softInfluenceVerticesDict.Remove(vid);
			}
		}
	}

	// Token: 0x06004F6C RID: 20332 RVA: 0x001C5324 File Offset: 0x001C3724
	public void SoftAutoRadius(int vid)
	{
		if (this.softVerticesUseAutoColliderRadius)
		{
			if (this.softVerticesAutoColliderVertex1 == vid)
			{
				this.softVerticesAutoColliderVertex1 = -1;
			}
			else if (this.softVerticesAutoColliderVertex2 == vid)
			{
				this.softVerticesAutoColliderVertex2 = -1;
			}
			else if (this.softVerticesAutoColliderVertex1 == -1)
			{
				this.softVerticesAutoColliderVertex1 = vid;
			}
			else if (this.softVerticesAutoColliderVertex2 == -1)
			{
				this.softVerticesAutoColliderVertex2 = vid;
			}
			this.SoftVerticesSetAutoRadius();
		}
	}

	// Token: 0x06004F6D RID: 20333 RVA: 0x001C53A0 File Offset: 0x001C37A0
	public void SoftSelect(int vid)
	{
		Debug.Log("SoftSelect " + vid);
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup;
		if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet) && this._softSetToGroupDict.TryGetValue(dazphysicsMeshSoftVerticesSet, out dazphysicsMeshSoftVerticesGroup))
		{
			Debug.Log("Got set " + dazphysicsMeshSoftVerticesSet.uid + " in group " + dazphysicsMeshSoftVerticesGroup.name);
			this.currentSoftVerticesGroup = dazphysicsMeshSoftVerticesGroup;
			this.currentSoftVerticesGroup.currentSet = dazphysicsMeshSoftVerticesSet;
		}
	}

	// Token: 0x06004F6E RID: 20334 RVA: 0x001C541C File Offset: 0x001C381C
	public void SoftSpringSet(int vid)
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			dazphysicsMeshSoftVerticesSet.springMultiplier = this._softSpringMultiplierSetValue;
			if (this.currentSoftVerticesGroup != null)
			{
				this.currentSoftVerticesGroup.currentSet = dazphysicsMeshSoftVerticesSet;
			}
		}
	}

	// Token: 0x06004F6F RID: 20335 RVA: 0x001C5460 File Offset: 0x001C3860
	public void SoftSizeSet(int vid)
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			dazphysicsMeshSoftVerticesSet.sizeMultiplier = this._softSizeMultiplierSetValue;
			if (this.currentSoftVerticesGroup != null)
			{
				this.currentSoftVerticesGroup.currentSet = dazphysicsMeshSoftVerticesSet;
			}
		}
	}

	// Token: 0x06004F70 RID: 20336 RVA: 0x001C54A4 File Offset: 0x001C38A4
	public void SoftLimitSet(int vid)
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			dazphysicsMeshSoftVerticesSet.limitMultiplier = this._softLimitMultiplierSetValue;
			if (this.currentSoftVerticesGroup != null)
			{
				this.currentSoftVerticesGroup.currentSet = dazphysicsMeshSoftVerticesSet;
			}
		}
	}

	// Token: 0x06004F71 RID: 20337 RVA: 0x001C54E8 File Offset: 0x001C38E8
	public void AddSoftSet(DAZPhysicsMeshSoftVerticesGroup sg)
	{
		int currentSetIndex = sg.AddSet();
		sg.currentSetIndex = currentSetIndex;
		this._softSetToGroupDict.Add(sg.currentSet, sg);
	}

	// Token: 0x06004F72 RID: 20338 RVA: 0x001C5518 File Offset: 0x001C3918
	public void AutoSoftVertex(int vid)
	{
		DAZPhysicsMeshSoftVerticesGroup currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		if (currentSoftVerticesGroup == null)
		{
			this.AddSoftVerticesGroup();
			currentSoftVerticesGroup = this.currentSoftVerticesGroup;
		}
		DAZPhysicsMeshSoftVerticesSet currentSet = currentSoftVerticesGroup.currentSet;
		if (currentSet == null)
		{
			currentSoftVerticesGroup.AddSet();
			currentSet = currentSoftVerticesGroup.currentSet;
			this._softSetToGroupDict.Add(currentSet, currentSoftVerticesGroup);
		}
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		List<DAZPhysicsMeshSoftVerticesSet> list;
		if (currentSet.targetVertex == -1)
		{
			if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
			{
				dazphysicsMeshSoftVerticesSet.targetVertex = -1;
				this._softTargetVerticesDict.Remove(vid);
			}
			currentSet.targetVertex = vid;
			this._softTargetVerticesDict.Add(vid, currentSet);
		}
		else if (currentSet.anchorVertex == -1)
		{
			this.AddSoftAnchor(vid, currentSet);
		}
		else if (this._softAnchorVerticesDict.TryGetValue(vid, out list) && list.Contains(currentSet))
		{
			this.RemoveSoftAnchor(list, currentSet);
		}
		else if (this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			if (currentSoftVerticesGroup.currentSet != dazphysicsMeshSoftVerticesSet)
			{
				currentSoftVerticesGroup.currentSet = dazphysicsMeshSoftVerticesSet;
			}
			else
			{
				dazphysicsMeshSoftVerticesSet.targetVertex = -1;
				this._softTargetVerticesDict.Remove(vid);
			}
		}
		else if (this._softInfluenceVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			currentSoftVerticesGroup.currentSet = dazphysicsMeshSoftVerticesSet;
			dazphysicsMeshSoftVerticesSet.RemoveInfluenceVertex(vid);
			this._softInfluenceVerticesDict.Remove(vid);
		}
		else
		{
			currentSet.AddInfluenceVertex(vid);
			this._softInfluenceVerticesDict.Add(vid, currentSet);
		}
	}

	// Token: 0x06004F73 RID: 20339 RVA: 0x001C5685 File Offset: 0x001C3A85
	public void StartSoftLink(int vid)
	{
		if (this._softTargetVerticesDict.TryGetValue(vid, out this.startSoftLinkSet))
		{
			Debug.Log("Start Soft Link " + vid);
		}
	}

	// Token: 0x06004F74 RID: 20340 RVA: 0x001C56B4 File Offset: 0x001C3AB4
	public void EndSoftLink(int vid)
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet;
		if (this.startSoftLinkSet != null && this._softTargetVerticesDict.TryGetValue(vid, out dazphysicsMeshSoftVerticesSet))
		{
			Debug.Log("End Soft Link " + vid);
			if (this.startSoftLinkSet.links.Remove(dazphysicsMeshSoftVerticesSet.uid))
			{
				Debug.Log("Remove");
			}
			else if (this.startSoftLinkSet.uid != dazphysicsMeshSoftVerticesSet.uid)
			{
				Debug.Log("Add " + this.startSoftLinkSet.uid);
				this.startSoftLinkSet.links.Add(dazphysicsMeshSoftVerticesSet.uid);
			}
		}
	}

	// Token: 0x06004F75 RID: 20341 RVA: 0x001C5769 File Offset: 0x001C3B69
	public void ClearLinks(DAZPhysicsMeshSoftVerticesSet ss)
	{
		this.currentSoftVerticesGroup.ClearLinks(ss);
	}

	// Token: 0x06004F76 RID: 20342 RVA: 0x001C5778 File Offset: 0x001C3B78
	public void ClickVertex(int vid)
	{
		int num;
		if (this._uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		switch (this._selectionMode)
		{
		case DAZPhysicsMesh.SelectionMode.HardTarget:
			this.ToggleHardVertices(vid, false);
			break;
		case DAZPhysicsMesh.SelectionMode.HardTargetAuto:
			this.ToggleHardVertices(vid, true);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftTarget:
			this.ToggleSoftTargetVertex(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftAnchor:
			this.ToggleSoftAnchorVertex(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftInfluenced:
			this.ToggleSoftInfluenceVertices(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftAuto:
			this.AutoSoftVertex(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftLink:
			this.StartSoftLink(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftSelect:
			this.SoftSelect(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftSpringSet:
			this.SoftSpringSet(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftSizeSet:
			this.SoftSizeSet(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftLimitSet:
			this.SoftLimitSet(vid);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftAutoRadius:
			this.SoftAutoRadius(vid);
			break;
		}
	}

	// Token: 0x06004F77 RID: 20343 RVA: 0x001C587C File Offset: 0x001C3C7C
	public void UpclickVertex(int vid)
	{
		int num;
		if (this._uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		DAZPhysicsMesh.SelectionMode selectionMode = this._selectionMode;
		if (selectionMode == DAZPhysicsMesh.SelectionMode.SoftLink)
		{
			this.EndSoftLink(vid);
		}
	}

	// Token: 0x06004F78 RID: 20344 RVA: 0x001C58C0 File Offset: 0x001C3CC0
	public void OnVertex(int vid)
	{
		int num;
		if (this._uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		switch (this._selectionMode)
		{
		case DAZPhysicsMesh.SelectionMode.HardTarget:
			this.OnHardVertices(vid, false);
			break;
		case DAZPhysicsMesh.SelectionMode.HardTargetAuto:
			this.OnHardVertices(vid, true);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftInfluenced:
		case DAZPhysicsMesh.SelectionMode.SoftAuto:
			this.OnSoftInfluenceVertices(vid);
			break;
		}
	}

	// Token: 0x06004F79 RID: 20345 RVA: 0x001C5940 File Offset: 0x001C3D40
	public void OffVertex(int vid)
	{
		int num;
		if (this._uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		switch (this._selectionMode)
		{
		case DAZPhysicsMesh.SelectionMode.HardTarget:
			this.OffHardVertices(vid, false);
			break;
		case DAZPhysicsMesh.SelectionMode.HardTargetAuto:
			this.OffHardVertices(vid, true);
			break;
		case DAZPhysicsMesh.SelectionMode.SoftInfluenced:
		case DAZPhysicsMesh.SelectionMode.SoftAuto:
			this.OffSoftInfluenceVertices(vid);
			break;
		}
	}

	// Token: 0x06004F7A RID: 20346 RVA: 0x001C59C0 File Offset: 0x001C3DC0
	public int GetBaseVertex(int vid)
	{
		int num;
		if (this._uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		return vid;
	}

	// Token: 0x06004F7B RID: 20347 RVA: 0x001C59E4 File Offset: 0x001C3DE4
	public bool IsBaseVertex(int vid)
	{
		return this._uvVertToBaseVertDict == null || !this._uvVertToBaseVertDict.ContainsKey(vid);
	}

	// Token: 0x06004F7C RID: 20348 RVA: 0x001C5A04 File Offset: 0x001C3E04
	protected void CreateHardVerticesColliders()
	{
		foreach (DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup in this._hardVerticesGroups)
		{
			dazphysicsMeshHardVerticesGroup.CreateColliders(base.transform, this._skin);
		}
	}

	// Token: 0x06004F7D RID: 20349 RVA: 0x001C5A6C File Offset: 0x001C3E6C
	protected void UpdateHardVerticesColliders()
	{
		foreach (DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup in this._hardVerticesGroups)
		{
			dazphysicsMeshHardVerticesGroup.UpdateColliders();
		}
	}

	// Token: 0x06004F7E RID: 20350 RVA: 0x001C5AC8 File Offset: 0x001C3EC8
	protected void InitSoftJoints()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.Init(base.transform, this.skin);
		}
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup2 in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup2.InitPass2();
		}
	}

	// Token: 0x06004F7F RID: 20351 RVA: 0x001C5B7C File Offset: 0x001C3F7C
	public void ResetSoftJoints()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.ResetJoints();
		}
	}

	// Token: 0x06004F80 RID: 20352 RVA: 0x001C5BD8 File Offset: 0x001C3FD8
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		float oneoversc = 1f / this._scale;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.ScaleChanged(this._scale, oneoversc);
		}
	}

	// Token: 0x06004F81 RID: 20353 RVA: 0x001C5C50 File Offset: 0x001C4050
	public void UpdateSoftJointsFast(bool predictOnly = false)
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.UpdateJointsFast(predictOnly);
		}
	}

	// Token: 0x06004F82 RID: 20354 RVA: 0x001C5CAC File Offset: 0x001C40AC
	protected void UpdateSimulationSoftJoints()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			if (dazphysicsMeshSoftVerticesGroup.useSimulation)
			{
				dazphysicsMeshSoftVerticesGroup.UpdateJoints();
			}
		}
	}

	// Token: 0x06004F83 RID: 20355 RVA: 0x001C5D14 File Offset: 0x001C4114
	protected void UpdateNonSimulationSoftJoints()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			if (!dazphysicsMeshSoftVerticesGroup.useSimulation)
			{
				dazphysicsMeshSoftVerticesGroup.UpdateJoints();
			}
		}
	}

	// Token: 0x06004F84 RID: 20356 RVA: 0x001C5D7C File Offset: 0x001C417C
	public void PrepareSoftUpdateJointsThreaded()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.PrepareUpdateJointsThreaded();
		}
	}

	// Token: 0x06004F85 RID: 20357 RVA: 0x001C5DD8 File Offset: 0x001C41D8
	public void UpdateSoftJointTargetsThreadedFast(Vector3[] verts, Vector3[] normals)
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.UpdateJointTargetsThreadedFast(verts, normals);
		}
	}

	// Token: 0x06004F86 RID: 20358 RVA: 0x001C5E38 File Offset: 0x001C4238
	protected void UpdateSoftJointTargetsThreaded()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.UpdateJointTargetsThreaded();
		}
	}

	// Token: 0x06004F87 RID: 20359 RVA: 0x001C5E94 File Offset: 0x001C4294
	public void ApplySoftJointBackForces()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.ResetAdjustJoints();
		}
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup2 in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup2.ApplyBackForce();
		}
	}

	// Token: 0x06004F88 RID: 20360 RVA: 0x001C5F3C File Offset: 0x001C433C
	protected void MorphSoftVertices()
	{
		float num = Time.time - Time.fixedTime;
		float interpFactor = num / Time.fixedDeltaTime;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.MorphVertices(interpFactor);
		}
	}

	// Token: 0x06004F89 RID: 20361 RVA: 0x001C5FAC File Offset: 0x001C43AC
	public void PrepareSoftMorphVerticesThreadedFast()
	{
		float num = Time.time - Time.fixedTime;
		float interpFactor = num / Time.fixedDeltaTime;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.PrepareMorphVerticesThreadedFast(interpFactor);
		}
	}

	// Token: 0x06004F8A RID: 20362 RVA: 0x001C601C File Offset: 0x001C441C
	protected void PrepareSoftMorphVerticesThreaded()
	{
		float num = Time.time - Time.fixedTime;
		float interpFactor = num / Time.fixedDeltaTime;
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.PrepareMorphVerticesThreaded(interpFactor);
		}
	}

	// Token: 0x06004F8B RID: 20363 RVA: 0x001C608C File Offset: 0x001C448C
	public void MorphSoftVerticesThreadedFast(Vector3[] verts)
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.MorphVerticesThreadedFast(verts);
		}
	}

	// Token: 0x06004F8C RID: 20364 RVA: 0x001C60E8 File Offset: 0x001C44E8
	protected void MorphSoftVerticesThreaded()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.MorphVerticesThreaded();
		}
	}

	// Token: 0x06004F8D RID: 20365 RVA: 0x001C6144 File Offset: 0x001C4544
	public void RecalculateLinkJointsFast(Vector3[] verts)
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.AdjustInitialTargetPositionsFast(verts);
		}
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup2 in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup2.AdjustLinkJointDistancesFast();
		}
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x001C61EC File Offset: 0x001C45EC
	public void RecalculateLinkJointsFinishFast()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.AdjustLinkJointDistancesFinishFast();
		}
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x001C6248 File Offset: 0x001C4648
	protected void RecalculateLinkJoints()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.AdjustInitialTargetPositions();
		}
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup2 in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup2.AdjustLinkJointDistances(false);
		}
	}

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x06004F90 RID: 20368 RVA: 0x001C62F0 File Offset: 0x001C46F0
	// (set) Token: 0x06004F91 RID: 20369 RVA: 0x001C62F8 File Offset: 0x001C46F8
	public bool allowSelfCollision
	{
		get
		{
			return this._allowSelfCollision;
		}
		set
		{
			if (this.allowSelfCollisionJSON != null)
			{
				this.allowSelfCollisionJSON.val = value;
			}
			else if (this._allowSelfCollision != value)
			{
				this.SyncAllowSelfCollision(value);
			}
		}
	}

	// Token: 0x06004F92 RID: 20370 RVA: 0x001C6329 File Offset: 0x001C4729
	protected void SyncAllowSelfCollision(bool b)
	{
		this._allowSelfCollision = b;
		if (Application.isPlaying)
		{
			this.InitColliders();
		}
	}

	// Token: 0x06004F93 RID: 20371 RVA: 0x001C6344 File Offset: 0x001C4744
	public void InitColliders()
	{
		foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
		{
			dazphysicsMeshSoftVerticesGroup.InitColliders();
		}
		if (this._softVerticesGroups.Count > 1)
		{
			for (int i = 0; i < this._softVerticesGroups.Count - 1; i++)
			{
				DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup2 = this._softVerticesGroups[i];
				for (int j = i + 1; j < this._softVerticesGroups.Count; j++)
				{
					DAZPhysicsMeshSoftVerticesGroup otherGroup = this._softVerticesGroups[j];
					dazphysicsMeshSoftVerticesGroup2.IgnoreOtherSoftGroupColliders(otherGroup, !dazphysicsMeshSoftVerticesGroup2.IsAllowedToCollideWithGroup(j) || !this._allowSelfCollision);
				}
			}
		}
		foreach (DAZPhysicsMesh dazphysicsMesh in this.ignorePhysicsMeshes)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup3 in this._softVerticesGroups)
			{
				foreach (DAZPhysicsMeshSoftVerticesGroup otherGroup2 in dazphysicsMesh.softVerticesGroups)
				{
					dazphysicsMeshSoftVerticesGroup3.IgnoreOtherSoftGroupColliders(otherGroup2, true);
				}
			}
		}
		foreach (DAZPhysicsMeshHardVerticesGroup dazphysicsMeshHardVerticesGroup in this._hardVerticesGroups)
		{
			dazphysicsMeshHardVerticesGroup.InitColliders();
		}
	}

	// Token: 0x06004F94 RID: 20372 RVA: 0x001C653C File Offset: 0x001C493C
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			DAZPhysicsMeshUI componentInChildren = this.UITransform.GetComponentInChildren<DAZPhysicsMeshUI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggle = componentInChildren.onToggle;
				this.allowSelfCollisionJSON.toggle = componentInChildren.allowSelfCollisionToggle;
				this.softVerticesCombinedSpringJSON.slider = componentInChildren.softVerticesCombinedSpringSlider;
				this.softVerticesCombinedDamperJSON.slider = componentInChildren.softVerticesCombinedDamperSlider;
				this.softVerticesMassJSON.slider = componentInChildren.softVerticesMassSlider;
				this.softVerticesBackForceJSON.slider = componentInChildren.softVerticesBackForceSlider;
				this.softVerticesBackForceThresholdDistanceJSON.slider = componentInChildren.softVerticesBackForceThresholdDistanceSlider;
				this.softVerticesBackForceMaxForceJSON.slider = componentInChildren.softVerticesBackForceMaxForceSlider;
				this.softVerticesUseAutoColliderRadiusJSON.toggle = componentInChildren.softVerticesUseAutoColliderRadiusToggle;
				this.softVerticesColliderRadiusJSON.slider = componentInChildren.softVerticesColliderRadiusSlider;
				this.softVerticesColliderAdditionalNormalOffsetJSON.slider = componentInChildren.softVerticesColliderAdditionalNormalOffsetSlider;
				this.softVerticesNormalLimitJSON.slider = componentInChildren.softVerticesNormalLimitSlider;
				if (this.groupASpringMultiplierJSON != null)
				{
					this.groupASpringMultiplierJSON.slider = componentInChildren.groupASpringMultplierSlider;
				}
				else if (componentInChildren.groupASpringMultplierSlider != null)
				{
					componentInChildren.groupASpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupADamperMultiplierJSON != null)
				{
					this.groupADamperMultiplierJSON.slider = componentInChildren.groupADamperMultplierSlider;
				}
				else if (componentInChildren.groupADamperMultplierSlider != null)
				{
					componentInChildren.groupADamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupBSpringMultiplierJSON != null)
				{
					this.groupBSpringMultiplierJSON.slider = componentInChildren.groupBSpringMultplierSlider;
				}
				else if (componentInChildren.groupBSpringMultplierSlider != null)
				{
					componentInChildren.groupBSpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupBDamperMultiplierJSON != null)
				{
					this.groupBDamperMultiplierJSON.slider = componentInChildren.groupBDamperMultplierSlider;
				}
				else if (componentInChildren.groupBDamperMultplierSlider != null)
				{
					componentInChildren.groupBDamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupCSpringMultiplierJSON != null)
				{
					this.groupCSpringMultiplierJSON.slider = componentInChildren.groupCSpringMultplierSlider;
				}
				else if (componentInChildren.groupCSpringMultplierSlider != null)
				{
					componentInChildren.groupCSpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupCDamperMultiplierJSON != null)
				{
					this.groupCDamperMultiplierJSON.slider = componentInChildren.groupCDamperMultplierSlider;
				}
				else if (componentInChildren.groupCDamperMultplierSlider != null)
				{
					componentInChildren.groupCDamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupDSpringMultiplierJSON != null)
				{
					this.groupDSpringMultiplierJSON.slider = componentInChildren.groupDSpringMultplierSlider;
				}
				else if (componentInChildren.groupDSpringMultplierSlider != null)
				{
					componentInChildren.groupDSpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupDDamperMultiplierJSON != null)
				{
					this.groupDDamperMultiplierJSON.slider = componentInChildren.groupDDamperMultplierSlider;
				}
				else if (componentInChildren.groupDDamperMultplierSlider != null)
				{
					componentInChildren.groupDDamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
			}
		}
		if (!this.useCombinedSpringAndDamper)
		{
			if (this.softVerticesNormalSpringSlider != null)
			{
				this.softVerticesNormalSpringSlider.value = this._softVerticesNormalSpring;
				this.softVerticesNormalSpringSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__0));
				SliderControl component = this.softVerticesNormalSpringSlider.GetComponent<SliderControl>();
				if (component != null)
				{
					component.defaultValue = this._softVerticesNormalSpring;
				}
				this.SyncSoftVerticesNormalSpring();
			}
			if (this.softVerticesNormalDamperSlider != null)
			{
				this.softVerticesNormalDamperSlider.value = this._softVerticesNormalDamper;
				this.softVerticesNormalDamperSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1));
				SliderControl component2 = this.softVerticesNormalDamperSlider.GetComponent<SliderControl>();
				if (component2 != null)
				{
					component2.defaultValue = this._softVerticesNormalDamper;
				}
				this.SyncSoftVerticesNormalDamper();
			}
			if (this.softVerticesTangentSpringSlider != null)
			{
				this.softVerticesTangentSpringSlider.value = this._softVerticesTangentSpring;
				this.softVerticesTangentSpringSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__2));
				SliderControl component3 = this.softVerticesTangentSpringSlider.GetComponent<SliderControl>();
				if (component3 != null)
				{
					component3.defaultValue = this._softVerticesTangentSpring;
				}
				this.SyncSoftVerticesTangentSpring();
			}
			if (this.softVerticesTangentDamperSlider != null)
			{
				this.softVerticesTangentDamperSlider.value = this._softVerticesTangentDamper;
				this.softVerticesTangentDamperSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__3));
				SliderControl component4 = this.softVerticesTangentDamperSlider.GetComponent<SliderControl>();
				if (component4 != null)
				{
					component4.defaultValue = this._softVerticesTangentDamper;
				}
				this.SyncSoftVerticesTangentDamper();
			}
		}
		if (this.softVerticesSpringMaxForceSlider != null)
		{
			this.softVerticesSpringMaxForceSlider.value = this._softVerticesSpringMaxForce;
			this.softVerticesSpringMaxForceSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__4));
			SliderControl component5 = this.softVerticesSpringMaxForceSlider.GetComponent<SliderControl>();
			if (component5 != null)
			{
				component5.defaultValue = this._softVerticesSpringMaxForce;
			}
			this.SyncSoftVerticesSpringMaxForce();
		}
		if (this.softVerticesUseUniformLimitToggle != null)
		{
			this.softVerticesUseUniformLimitToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__5));
			this.softVerticesUseUniformLimitToggle.isOn = this._softVerticesUseUniformLimit;
			this.SyncSoftVerticesUseUniformLimit();
		}
		if (this.softVerticesNegativeNormalLimitSlider != null)
		{
			this.softVerticesNegativeNormalLimitSlider.value = this._softVerticesNegativeNormalLimit;
			this.softVerticesNegativeNormalLimitSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__6));
			SliderControl component6 = this.softVerticesNegativeNormalLimitSlider.GetComponent<SliderControl>();
			if (component6 != null)
			{
				component6.defaultValue = this._softVerticesNegativeNormalLimit;
			}
			this.SyncSoftVerticesNegativeNormalLimit();
		}
		if (this.softVerticesTangentLimitSlider != null)
		{
			this.softVerticesTangentLimitSlider.value = this._softVerticesTangentLimit;
			this.softVerticesTangentLimitSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__7));
			SliderControl component7 = this.softVerticesTangentLimitSlider.GetComponent<SliderControl>();
			if (component7 != null)
			{
				component7.defaultValue = this._softVerticesTangentLimit;
			}
			this.SyncSoftVerticesTangentLimit();
		}
	}

	// Token: 0x06004F95 RID: 20373 RVA: 0x001C6BDC File Offset: 0x001C4FDC
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			DAZPhysicsMeshUI componentInChildren = this.UITransformAlt.GetComponentInChildren<DAZPhysicsMeshUI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggleAlt = componentInChildren.onToggle;
				this.allowSelfCollisionJSON.toggleAlt = componentInChildren.allowSelfCollisionToggle;
				this.softVerticesCombinedSpringJSON.sliderAlt = componentInChildren.softVerticesCombinedSpringSlider;
				this.softVerticesCombinedDamperJSON.sliderAlt = componentInChildren.softVerticesCombinedDamperSlider;
				this.softVerticesMassJSON.sliderAlt = componentInChildren.softVerticesMassSlider;
				this.softVerticesBackForceJSON.sliderAlt = componentInChildren.softVerticesBackForceSlider;
				this.softVerticesBackForceThresholdDistanceJSON.sliderAlt = componentInChildren.softVerticesBackForceThresholdDistanceSlider;
				this.softVerticesBackForceMaxForceJSON.sliderAlt = componentInChildren.softVerticesBackForceMaxForceSlider;
				this.softVerticesUseAutoColliderRadiusJSON.toggleAlt = componentInChildren.softVerticesUseAutoColliderRadiusToggle;
				this.softVerticesColliderRadiusJSON.sliderAlt = componentInChildren.softVerticesColliderRadiusSlider;
				this.softVerticesColliderAdditionalNormalOffsetJSON.sliderAlt = componentInChildren.softVerticesColliderAdditionalNormalOffsetSlider;
				this.softVerticesNormalLimitJSON.sliderAlt = componentInChildren.softVerticesNormalLimitSlider;
				if (this.groupASpringMultiplierJSON != null)
				{
					this.groupASpringMultiplierJSON.sliderAlt = componentInChildren.groupASpringMultplierSlider;
				}
				else if (componentInChildren.groupASpringMultplierSlider != null)
				{
					componentInChildren.groupASpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupADamperMultiplierJSON != null)
				{
					this.groupADamperMultiplierJSON.sliderAlt = componentInChildren.groupADamperMultplierSlider;
				}
				else if (componentInChildren.groupADamperMultplierSlider != null)
				{
					componentInChildren.groupADamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupBSpringMultiplierJSON != null)
				{
					this.groupBSpringMultiplierJSON.sliderAlt = componentInChildren.groupBSpringMultplierSlider;
				}
				else if (componentInChildren.groupBSpringMultplierSlider != null)
				{
					componentInChildren.groupBSpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupBDamperMultiplierJSON != null)
				{
					this.groupBDamperMultiplierJSON.sliderAlt = componentInChildren.groupBDamperMultplierSlider;
				}
				else if (componentInChildren.groupBDamperMultplierSlider != null)
				{
					componentInChildren.groupBDamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupCSpringMultiplierJSON != null)
				{
					this.groupCSpringMultiplierJSON.sliderAlt = componentInChildren.groupCSpringMultplierSlider;
				}
				else if (componentInChildren.groupCSpringMultplierSlider != null)
				{
					componentInChildren.groupCSpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupCDamperMultiplierJSON != null)
				{
					this.groupCDamperMultiplierJSON.sliderAlt = componentInChildren.groupCDamperMultplierSlider;
				}
				else if (componentInChildren.groupCDamperMultplierSlider != null)
				{
					componentInChildren.groupCDamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupDSpringMultiplierJSON != null)
				{
					this.groupDSpringMultiplierJSON.sliderAlt = componentInChildren.groupDSpringMultplierSlider;
				}
				else if (componentInChildren.groupDSpringMultplierSlider != null)
				{
					componentInChildren.groupDSpringMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.groupDDamperMultiplierJSON != null)
				{
					this.groupDDamperMultiplierJSON.sliderAlt = componentInChildren.groupDDamperMultplierSlider;
				}
				else if (componentInChildren.groupDDamperMultplierSlider != null)
				{
					componentInChildren.groupDDamperMultplierSlider.transform.parent.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x06004F96 RID: 20374 RVA: 0x001C6F47 File Offset: 0x001C5347
	public Mesh editorMeshForFocus
	{
		get
		{
			return this._editorMeshForFocus;
		}
	}

	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x06004F97 RID: 20375 RVA: 0x001C6F4F File Offset: 0x001C534F
	public bool wasInit
	{
		get
		{
			return this._wasInit;
		}
	}

	// Token: 0x06004F98 RID: 20376 RVA: 0x001C6F58 File Offset: 0x001C5358
	public void Init()
	{
		if (!this._wasInit && this._skin != null)
		{
			this._wasInit = true;
			if (Application.isPlaying)
			{
				DAZPhysicsMeshEarlyUpdate dazphysicsMeshEarlyUpdate = base.GetComponent<DAZPhysicsMeshEarlyUpdate>();
				if (dazphysicsMeshEarlyUpdate == null)
				{
					dazphysicsMeshEarlyUpdate = base.gameObject.AddComponent<DAZPhysicsMeshEarlyUpdate>();
					dazphysicsMeshEarlyUpdate.dazPhysicsMesh = this;
				}
				this._skin.Init();
				this.InitSoftJoints();
				this.CreateHardVerticesColliders();
				this.InitColliders();
				foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this._softVerticesGroups)
				{
					dazphysicsMeshSoftVerticesGroup.SyncCollisionEnabled();
				}
			}
			else if (this._skin.dazMesh != null)
			{
				this._skin.Init();
				this._baseVertToUVVertFullMap = this._skin.dazMesh.baseVertToUVVertFullMap;
			}
		}
	}

	// Token: 0x06004F99 RID: 20377 RVA: 0x001C7060 File Offset: 0x001C5460
	protected void InitJSONParams()
	{
		if (Application.isPlaying)
		{
			this.onJSON = new JSONStorableBool("on", this._on, new JSONStorableBool.SetBoolCallback(this.SyncOnCallback));
			this.SyncOn(false);
			base.RegisterBool(this.onJSON);
			this.allowSelfCollisionJSON = new JSONStorableBool("allowSelfCollision", this._allowSelfCollision, new JSONStorableBool.SetBoolCallback(this.SyncAllowSelfCollision));
			base.RegisterBool(this.allowSelfCollisionJSON);
			this.softVerticesCombinedSpringJSON = new JSONStorableFloat("softVerticesCombinedSpring", this._softVerticesCombinedSpring, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesCombinedSpring), 0f, 500f, true, true);
			base.RegisterFloat(this.softVerticesCombinedSpringJSON);
			this.softVerticesCombinedDamperJSON = new JSONStorableFloat("softVerticesCombinedDamper", this._softVerticesCombinedDamper, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesCombinedDamper), 0f, 10f, true, true);
			base.RegisterFloat(this.softVerticesCombinedDamperJSON);
			this.softVerticesMassJSON = new JSONStorableFloat("softVerticesMass", this._softVerticesMass, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesMass), 0.05f, 0.5f, true, true);
			base.RegisterFloat(this.softVerticesMassJSON);
			this.softVerticesBackForceJSON = new JSONStorableFloat("softVerticesBackForce", this._softVerticesBackForce, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesBackForce), 0f, 50f, true, true);
			base.RegisterFloat(this.softVerticesBackForceJSON);
			this.softVerticesBackForceThresholdDistanceJSON = new JSONStorableFloat("softVerticesBackForceThresholdDistance", this._softVerticesBackForceThresholdDistance, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesBackForceThresholdDistance), 0f, 0.03f, true, true);
			base.RegisterFloat(this.softVerticesBackForceThresholdDistanceJSON);
			this.softVerticesBackForceMaxForceJSON = new JSONStorableFloat("softVerticesBackForceMaxForce", this._softVerticesBackForceMaxForce, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesBackForceMaxForce), 0f, 50f, true, true);
			base.RegisterFloat(this.softVerticesBackForceMaxForceJSON);
			this.softVerticesUseAutoColliderRadiusJSON = new JSONStorableBool("softVerticesUseAutoColliderRadius", this._softVerticesUseAutoColliderRadius, new JSONStorableBool.SetBoolCallback(this.SyncSoftVerticesUseAutoColliderRadius));
			base.RegisterBool(this.softVerticesUseAutoColliderRadiusJSON);
			int num = Mathf.RoundToInt(this._softVerticesColliderRadius * 1000f);
			this._softVerticesColliderRadius = (float)num / 1000f;
			this.softVerticesColliderRadiusJSON = new JSONStorableFloat("softVerticesColliderRadius", this._softVerticesColliderRadius, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesColliderRadius), 0f, 0.07f, true, true);
			base.RegisterFloat(this.softVerticesColliderRadiusJSON);
			this.softVerticesColliderAdditionalNormalOffsetJSON = new JSONStorableFloat("softVerticesColliderAdditionalNormalOffset", this._softVerticesColliderAdditionalNormalOffset, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesColliderAdditionalNormalOffset), -0.01f, 0.01f, true, true);
			base.RegisterFloat(this.softVerticesColliderAdditionalNormalOffsetJSON);
			this.softVerticesNormalLimitJSON = new JSONStorableFloat("softVerticesDistanceLimit", this._softVerticesNormalLimit, new JSONStorableFloat.SetFloatCallback(this.SyncSoftVerticesNormalLimit), 0f, 0.1f, false, true);
			base.RegisterFloat(this.softVerticesNormalLimitJSON);
			if (this.groupASlots != null && this.groupASlots.Length > 0)
			{
				float parentSettingSpringMultiplier = this.softVerticesGroups[this.groupASlots[0]].parentSettingSpringMultiplier;
				this.groupASpringMultiplierJSON = new JSONStorableFloat("groupASpringMultiplier", parentSettingSpringMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncGroupASpringMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupASpringMultiplierJSON);
				float parentSettingDamperMultiplier = this.softVerticesGroups[this.groupASlots[0]].parentSettingDamperMultiplier;
				this.groupADamperMultiplierJSON = new JSONStorableFloat("groupADamperMultiplier", parentSettingDamperMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncGroupADamperMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupADamperMultiplierJSON);
			}
			if (this.groupBSlots != null && this.groupBSlots.Length > 0)
			{
				float parentSettingSpringMultiplier2 = this.softVerticesGroups[this.groupBSlots[0]].parentSettingSpringMultiplier;
				this.groupBSpringMultiplierJSON = new JSONStorableFloat("groupBSpringMultiplier", parentSettingSpringMultiplier2, new JSONStorableFloat.SetFloatCallback(this.SyncGroupBSpringMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupBSpringMultiplierJSON);
				float parentSettingDamperMultiplier2 = this.softVerticesGroups[this.groupBSlots[0]].parentSettingDamperMultiplier;
				this.groupBDamperMultiplierJSON = new JSONStorableFloat("groupBDamperMultiplier", parentSettingDamperMultiplier2, new JSONStorableFloat.SetFloatCallback(this.SyncGroupBDamperMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupBDamperMultiplierJSON);
			}
			if (this.groupCSlots != null && this.groupCSlots.Length > 0)
			{
				float parentSettingSpringMultiplier3 = this.softVerticesGroups[this.groupCSlots[0]].parentSettingSpringMultiplier;
				this.groupCSpringMultiplierJSON = new JSONStorableFloat("groupCSpringMultiplier", parentSettingSpringMultiplier3, new JSONStorableFloat.SetFloatCallback(this.SyncGroupCSpringMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupCSpringMultiplierJSON);
				float parentSettingDamperMultiplier3 = this.softVerticesGroups[this.groupCSlots[0]].parentSettingDamperMultiplier;
				this.groupCDamperMultiplierJSON = new JSONStorableFloat("groupCDamperMultiplier", parentSettingDamperMultiplier3, new JSONStorableFloat.SetFloatCallback(this.SyncGroupCDamperMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupCDamperMultiplierJSON);
			}
			if (this.groupDSlots != null && this.groupDSlots.Length > 0)
			{
				float parentSettingSpringMultiplier4 = this.softVerticesGroups[this.groupDSlots[0]].parentSettingSpringMultiplier;
				this.groupDSpringMultiplierJSON = new JSONStorableFloat("groupDSpringMultiplier", parentSettingSpringMultiplier4, new JSONStorableFloat.SetFloatCallback(this.SyncGroupDSpringMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupDSpringMultiplierJSON);
				float parentSettingDamperMultiplier4 = this.softVerticesGroups[this.groupDSlots[0]].parentSettingDamperMultiplier;
				this.groupDDamperMultiplierJSON = new JSONStorableFloat("groupDDamperMultiplier", parentSettingDamperMultiplier4, new JSONStorableFloat.SetFloatCallback(this.SyncGroupDDamperMultiplier), 0f, 5f, false, true);
				base.RegisterFloat(this.groupDDamperMultiplierJSON);
			}
		}
	}

	// Token: 0x06004F9A RID: 20378 RVA: 0x001C7610 File Offset: 0x001C5A10
	private void OnEnable()
	{
		this.Init();
		if (Application.isPlaying)
		{
			this.InitColliders();
		}
		else if (Application.isEditor)
		{
			this.InitCaches(true);
		}
		this.isEnabled = true;
	}

	// Token: 0x06004F9B RID: 20379 RVA: 0x001C7645 File Offset: 0x001C5A45
	private void OnDisable()
	{
		this.isEnabled = false;
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x001C764E File Offset: 0x001C5A4E
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.InitJSONParams();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x06004F9D RID: 20381 RVA: 0x001C7674 File Offset: 0x001C5A74
	private void LateUpdate()
	{
		if (this._wasInit && Application.isPlaying && this.updateEnabled)
		{
			foreach (DAZPhysicsMeshSoftVerticesGroup dazphysicsMeshSoftVerticesGroup in this.softVerticesGroups)
			{
				dazphysicsMeshSoftVerticesGroup.useThreading = this.useThreading;
			}
			if (this.useThreading)
			{
				this.StartThreads();
				while (this.physicsMeshTask.working)
				{
					Thread.Sleep(0);
				}
				this.PrepareSoftUpdateJointsThreaded();
				this.physicsMeshTask.taskType = DAZPhysicsMesh.DAZPhysicsMeshTaskType.UpdateSoftJointTargets;
				this.physicsMeshTask.working = true;
				this.physicsMeshTask.resetEvent.Set();
			}
		}
	}

	// Token: 0x06004F9E RID: 20382 RVA: 0x001C7754 File Offset: 0x001C5B54
	private void FixedUpdate()
	{
		if (this._wasInit && Application.isPlaying && this.updateEnabled)
		{
			if (this._globalOn != DAZPhysicsMesh.globalEnable)
			{
				this.SyncOn(true);
			}
			this.UpdateHardVerticesColliders();
			if (this.useThreading)
			{
				if (this.physicsMeshTask != null)
				{
					while (this.physicsMeshTask.working)
					{
						Thread.Sleep(0);
					}
					this.UpdateSimulationSoftJoints();
				}
			}
			else
			{
				this.UpdateSimulationSoftJoints();
			}
			this.ApplySoftJointBackForces();
		}
	}

	// Token: 0x06004F9F RID: 20383 RVA: 0x001C77E8 File Offset: 0x001C5BE8
	public void EarlyUpdate()
	{
		if (this.useThreading && this.updateEnabled)
		{
			this.StartThreads();
			while (this.physicsMeshTask.working)
			{
				Thread.Sleep(0);
			}
			this.UpdateNonSimulationSoftJoints();
			this.PrepareSoftMorphVerticesThreaded();
			this.physicsMeshTask.taskType = DAZPhysicsMesh.DAZPhysicsMeshTaskType.MorphVertices;
			this.physicsMeshTask.working = true;
			this.physicsMeshTask.resetEvent.Set();
		}
	}

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001C7868 File Offset: 0x001C5C68
	protected override void Update()
	{
		if (Application.isPlaying)
		{
			if (this._globalOn != DAZPhysicsMesh.globalEnable)
			{
				this.SyncOn(true);
			}
			this.CheckResumeSimulation();
			if (this.updateEnabled && this._wasInit)
			{
				if (!this.useThreading)
				{
					this.UpdateNonSimulationSoftJoints();
				}
				if (this.skin != null && (this.skin.dazMesh.visibleNonPoseVerticesChangedThisFrame || this.skin.dazMesh.visibleNonPoseVerticesChangedLastFrame))
				{
					this.SoftVerticesSetAutoRadius();
					this.RecalculateLinkJoints();
				}
				if (this.useThreading)
				{
					while (this.physicsMeshTask.working)
					{
						Thread.Sleep(0);
					}
				}
				else
				{
					this.MorphSoftVertices();
				}
			}
		}
		else
		{
			MeshFilter meshFilter = base.GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			MeshRenderer x = base.GetComponent<MeshRenderer>();
			if (x == null)
			{
				x = base.gameObject.AddComponent<MeshRenderer>();
			}
			if (this._editorMeshForFocus == null)
			{
				this._editorMeshForFocus = new Mesh();
			}
			meshFilter.mesh = this._editorMeshForFocus;
			if (this.skin != null && this.skin.dazMesh != null && this.skin.dazMesh.morphedUVVertices != null && this.skin.dazMesh.morphedUVVertices.Length > 0 && this.currentSoftVerticesGroup != null && this.currentSoftVerticesGroup.currentSet != null && this.currentSoftVerticesGroup.currentSet.targetVertex != -1)
			{
				Vector3 center = this.skin.dazMesh.morphedUVVertices[this.currentSoftVerticesGroup.currentSet.targetVertex];
				Vector3 size;
				size.x = this._handleSize * 50f;
				size.y = size.x;
				size.z = size.x;
				Bounds bounds = new Bounds(center, size);
				this._editorMeshForFocus.bounds = bounds;
			}
		}
	}

	// Token: 0x06004FA1 RID: 20385 RVA: 0x001C7A9D File Offset: 0x001C5E9D
	protected void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
			base.StopAllCoroutines();
		}
	}

	// Token: 0x06004FA2 RID: 20386 RVA: 0x001C7AB5 File Offset: 0x001C5EB5
	protected void OnApplicationQuit()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
			base.StopAllCoroutines();
		}
	}

	// Token: 0x06004FA3 RID: 20387 RVA: 0x001C7ACD File Offset: 0x001C5ECD
	// Note: this type is marked as 'beforefieldinit'.
	static DAZPhysicsMesh()
	{
	}

	// Token: 0x06004FA4 RID: 20388 RVA: 0x001C7AD5 File Offset: 0x001C5ED5
	[CompilerGenerated]
	private void <InitUI>m__0(float A_1)
	{
		this.softVerticesNormalSpring = this.softVerticesNormalSpringSlider.value;
	}

	// Token: 0x06004FA5 RID: 20389 RVA: 0x001C7AE8 File Offset: 0x001C5EE8
	[CompilerGenerated]
	private void <InitUI>m__1(float A_1)
	{
		this.softVerticesNormalDamper = this.softVerticesNormalDamperSlider.value;
	}

	// Token: 0x06004FA6 RID: 20390 RVA: 0x001C7AFB File Offset: 0x001C5EFB
	[CompilerGenerated]
	private void <InitUI>m__2(float A_1)
	{
		this.softVerticesTangentSpring = this.softVerticesTangentSpringSlider.value;
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x001C7B0E File Offset: 0x001C5F0E
	[CompilerGenerated]
	private void <InitUI>m__3(float A_1)
	{
		this.softVerticesTangentDamper = this.softVerticesTangentDamperSlider.value;
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001C7B21 File Offset: 0x001C5F21
	[CompilerGenerated]
	private void <InitUI>m__4(float A_1)
	{
		this.softVerticesSpringMaxForce = this.softVerticesSpringMaxForceSlider.value;
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x001C7B34 File Offset: 0x001C5F34
	[CompilerGenerated]
	private void <InitUI>m__5(bool A_1)
	{
		this.softVerticesUseUniformLimit = this.softVerticesUseUniformLimitToggle.isOn;
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x001C7B47 File Offset: 0x001C5F47
	[CompilerGenerated]
	private void <InitUI>m__6(float A_1)
	{
		this.softVerticesNegativeNormalLimit = this.softVerticesNegativeNormalLimitSlider.value;
	}

	// Token: 0x06004FAB RID: 20395 RVA: 0x001C7B5A File Offset: 0x001C5F5A
	[CompilerGenerated]
	private void <InitUI>m__7(float A_1)
	{
		this.softVerticesTangentLimit = this.softVerticesTangentLimitSlider.value;
	}

	// Token: 0x04003EFD RID: 16125
	public static bool globalEnable = true;

	// Token: 0x04003EFE RID: 16126
	public bool editorDirty;

	// Token: 0x04003EFF RID: 16127
	public Transform transformToEnableWhenOn;

	// Token: 0x04003F00 RID: 16128
	public Transform transformToEnableWhenOff;

	// Token: 0x04003F01 RID: 16129
	public Transform[] transformsToEnableWhenOn;

	// Token: 0x04003F02 RID: 16130
	public Transform[] transformsToEnableWhenOff;

	// Token: 0x04003F03 RID: 16131
	public DAZCharacterRun characterRun;

	// Token: 0x04003F04 RID: 16132
	public AutoColliderGroup[] autoColliderGroupsToEnableWhenOff;

	// Token: 0x04003F05 RID: 16133
	public AutoColliderGroup[] autoColliderGroupsToEnableWhenOn;

	// Token: 0x04003F06 RID: 16134
	protected bool _globalOn;

	// Token: 0x04003F07 RID: 16135
	protected JSONStorableBool onJSON;

	// Token: 0x04003F08 RID: 16136
	[SerializeField]
	protected bool _on = true;

	// Token: 0x04003F09 RID: 16137
	protected bool _alternateOn = true;

	// Token: 0x04003F0A RID: 16138
	protected bool morphsChanged;

	// Token: 0x04003F0B RID: 16139
	public bool useThreading;

	// Token: 0x04003F0C RID: 16140
	protected DAZPhysicsMesh.DAZPhysicsMeshTaskInfo physicsMeshTask;

	// Token: 0x04003F0D RID: 16141
	protected bool _threadsRunning;

	// Token: 0x04003F0E RID: 16142
	[SerializeField]
	protected Transform _skinTransform;

	// Token: 0x04003F0F RID: 16143
	protected Dictionary<int, List<int>> _baseVertToUVVertFullMap;

	// Token: 0x04003F10 RID: 16144
	[SerializeField]
	protected DAZSkinV2 _skin;

	// Token: 0x04003F11 RID: 16145
	[SerializeField]
	protected bool _showHandles;

	// Token: 0x04003F12 RID: 16146
	[SerializeField]
	protected bool _showBackfaceHandles;

	// Token: 0x04003F13 RID: 16147
	[SerializeField]
	protected bool _showLinkLines;

	// Token: 0x04003F14 RID: 16148
	[SerializeField]
	protected bool _showColliders;

	// Token: 0x04003F15 RID: 16149
	[SerializeField]
	protected bool _showCurrentSoftGroupOnly;

	// Token: 0x04003F16 RID: 16150
	[SerializeField]
	protected bool _showCurrentSoftSetOnly;

	// Token: 0x04003F17 RID: 16151
	[SerializeField]
	protected float _handleSize = 0.0002f;

	// Token: 0x04003F18 RID: 16152
	[SerializeField]
	protected float _softSpringMultiplierSetValue = 1f;

	// Token: 0x04003F19 RID: 16153
	[SerializeField]
	protected float _softSizeMultiplierSetValue = 1f;

	// Token: 0x04003F1A RID: 16154
	[SerializeField]
	protected float _softLimitMultiplierSetValue = 1f;

	// Token: 0x04003F1B RID: 16155
	[SerializeField]
	protected DAZPhysicsMesh.SelectionMode _selectionMode;

	// Token: 0x04003F1C RID: 16156
	[SerializeField]
	protected int _subMeshSelection = -1;

	// Token: 0x04003F1D RID: 16157
	[SerializeField]
	protected int _subMeshSelection2 = -1;

	// Token: 0x04003F1E RID: 16158
	[SerializeField]
	protected bool _showHardGroups;

	// Token: 0x04003F1F RID: 16159
	[SerializeField]
	protected int _currentHardVerticesGroupIndex;

	// Token: 0x04003F20 RID: 16160
	[SerializeField]
	protected bool _showSoftGroups;

	// Token: 0x04003F21 RID: 16161
	[SerializeField]
	protected int _currentSoftVerticesGroupIndex;

	// Token: 0x04003F22 RID: 16162
	[SerializeField]
	protected bool _showColliderGroups;

	// Token: 0x04003F23 RID: 16163
	[SerializeField]
	protected int _currentColliderGroupIndex;

	// Token: 0x04003F24 RID: 16164
	[SerializeField]
	protected List<DAZPhysicsMeshHardVerticesGroup> _hardVerticesGroups;

	// Token: 0x04003F25 RID: 16165
	[SerializeField]
	protected List<DAZPhysicsMeshSoftVerticesGroup> _softVerticesGroups;

	// Token: 0x04003F26 RID: 16166
	[SerializeField]
	protected List<DAZPhysicsMeshColliderGroup> _colliderGroups;

	// Token: 0x04003F27 RID: 16167
	public int[] groupASlots;

	// Token: 0x04003F28 RID: 16168
	public int[] groupBSlots;

	// Token: 0x04003F29 RID: 16169
	public int[] groupCSlots;

	// Token: 0x04003F2A RID: 16170
	public int[] groupDSlots;

	// Token: 0x04003F2B RID: 16171
	protected JSONStorableFloat groupASpringMultiplierJSON;

	// Token: 0x04003F2C RID: 16172
	protected JSONStorableFloat groupADamperMultiplierJSON;

	// Token: 0x04003F2D RID: 16173
	protected JSONStorableFloat groupBSpringMultiplierJSON;

	// Token: 0x04003F2E RID: 16174
	protected JSONStorableFloat groupBDamperMultiplierJSON;

	// Token: 0x04003F2F RID: 16175
	protected JSONStorableFloat groupCSpringMultiplierJSON;

	// Token: 0x04003F30 RID: 16176
	protected JSONStorableFloat groupCDamperMultiplierJSON;

	// Token: 0x04003F31 RID: 16177
	protected JSONStorableFloat groupDSpringMultiplierJSON;

	// Token: 0x04003F32 RID: 16178
	protected JSONStorableFloat groupDDamperMultiplierJSON;

	// Token: 0x04003F33 RID: 16179
	public bool useCombinedSpringAndDamper;

	// Token: 0x04003F34 RID: 16180
	protected JSONStorableFloat softVerticesCombinedSpringJSON;

	// Token: 0x04003F35 RID: 16181
	[SerializeField]
	protected float _softVerticesCombinedSpring = 80f;

	// Token: 0x04003F36 RID: 16182
	protected JSONStorableFloat softVerticesCombinedDamperJSON;

	// Token: 0x04003F37 RID: 16183
	[SerializeField]
	protected float _softVerticesCombinedDamper = 1f;

	// Token: 0x04003F38 RID: 16184
	public Slider softVerticesNormalSpringSlider;

	// Token: 0x04003F39 RID: 16185
	[SerializeField]
	protected float _softVerticesNormalSpring = 10f;

	// Token: 0x04003F3A RID: 16186
	public Slider softVerticesNormalDamperSlider;

	// Token: 0x04003F3B RID: 16187
	[SerializeField]
	protected float _softVerticesNormalDamper = 1f;

	// Token: 0x04003F3C RID: 16188
	public Slider softVerticesTangentSpringSlider;

	// Token: 0x04003F3D RID: 16189
	[SerializeField]
	protected float _softVerticesTangentSpring = 10f;

	// Token: 0x04003F3E RID: 16190
	public Slider softVerticesTangentDamperSlider;

	// Token: 0x04003F3F RID: 16191
	[SerializeField]
	protected float _softVerticesTangentDamper = 1f;

	// Token: 0x04003F40 RID: 16192
	public Slider softVerticesSpringMaxForceSlider;

	// Token: 0x04003F41 RID: 16193
	[SerializeField]
	protected float _softVerticesSpringMaxForce = 10f;

	// Token: 0x04003F42 RID: 16194
	protected JSONStorableFloat softVerticesMassJSON;

	// Token: 0x04003F43 RID: 16195
	[SerializeField]
	protected float _softVerticesMass = 0.1f;

	// Token: 0x04003F44 RID: 16196
	protected JSONStorableFloat softVerticesBackForceJSON;

	// Token: 0x04003F45 RID: 16197
	[SerializeField]
	protected float _softVerticesBackForce;

	// Token: 0x04003F46 RID: 16198
	protected JSONStorableFloat softVerticesBackForceThresholdDistanceJSON;

	// Token: 0x04003F47 RID: 16199
	[SerializeField]
	protected float _softVerticesBackForceThresholdDistance = 0.01f;

	// Token: 0x04003F48 RID: 16200
	protected JSONStorableFloat softVerticesBackForceMaxForceJSON;

	// Token: 0x04003F49 RID: 16201
	[SerializeField]
	protected float _softVerticesBackForceMaxForce;

	// Token: 0x04003F4A RID: 16202
	[SerializeField]
	protected bool _multiplyMassByLimitMultiplier = true;

	// Token: 0x04003F4B RID: 16203
	public Toggle softVerticesUseUniformLimitToggle;

	// Token: 0x04003F4C RID: 16204
	[SerializeField]
	protected bool _softVerticesUseUniformLimit;

	// Token: 0x04003F4D RID: 16205
	public Slider softVerticesTangentLimitSlider;

	// Token: 0x04003F4E RID: 16206
	[SerializeField]
	protected float _softVerticesTangentLimit;

	// Token: 0x04003F4F RID: 16207
	protected JSONStorableFloat softVerticesNormalLimitJSON;

	// Token: 0x04003F50 RID: 16208
	[SerializeField]
	protected float _softVerticesNormalLimit;

	// Token: 0x04003F51 RID: 16209
	public Slider softVerticesNegativeNormalLimitSlider;

	// Token: 0x04003F52 RID: 16210
	[SerializeField]
	protected float _softVerticesNegativeNormalLimit;

	// Token: 0x04003F53 RID: 16211
	public int softVerticesAutoColliderVertex1 = -1;

	// Token: 0x04003F54 RID: 16212
	public int softVerticesAutoColliderVertex2 = -1;

	// Token: 0x04003F55 RID: 16213
	public float softVerticesAutoColliderMinRadius = 0.02f;

	// Token: 0x04003F56 RID: 16214
	public float softVerticesAutoColliderMaxRadius = 0.05f;

	// Token: 0x04003F57 RID: 16215
	public float softVerticesAutoColliderRadiusMultiplier = 1f;

	// Token: 0x04003F58 RID: 16216
	public float softVerticesAutoColliderRadiusOffset;

	// Token: 0x04003F59 RID: 16217
	public JSONStorableBool softVerticesUseAutoColliderRadiusJSON;

	// Token: 0x04003F5A RID: 16218
	[SerializeField]
	protected bool _softVerticesUseAutoColliderRadius;

	// Token: 0x04003F5B RID: 16219
	protected const float radiusAdjustThreshold = 1000f;

	// Token: 0x04003F5C RID: 16220
	public JSONStorableFloat softVerticesColliderRadiusJSON;

	// Token: 0x04003F5D RID: 16221
	protected float _softVerticesColliderRadiusThreaded;

	// Token: 0x04003F5E RID: 16222
	[SerializeField]
	protected float _softVerticesColliderRadius;

	// Token: 0x04003F5F RID: 16223
	public JSONStorableFloat softVerticesColliderAdditionalNormalOffsetJSON;

	// Token: 0x04003F60 RID: 16224
	[SerializeField]
	protected float _softVerticesColliderAdditionalNormalOffset;

	// Token: 0x04003F61 RID: 16225
	[SerializeField]
	protected int _numPredictionFrames;

	// Token: 0x04003F62 RID: 16226
	protected Dictionary<int, DAZPhysicsMeshHardVerticesGroup> _hardTargetVerticesDict;

	// Token: 0x04003F63 RID: 16227
	protected Dictionary<int, DAZPhysicsMeshSoftVerticesSet> _softTargetVerticesDict;

	// Token: 0x04003F64 RID: 16228
	protected Dictionary<int, List<DAZPhysicsMeshSoftVerticesSet>> _softAnchorVerticesDict;

	// Token: 0x04003F65 RID: 16229
	protected Dictionary<int, DAZPhysicsMeshSoftVerticesSet> _softInfluenceVerticesDict;

	// Token: 0x04003F66 RID: 16230
	protected Dictionary<string, bool> _softVerticesInGroupDict;

	// Token: 0x04003F67 RID: 16231
	protected Dictionary<int, int> _uvVertToBaseVertDict;

	// Token: 0x04003F68 RID: 16232
	protected Dictionary<DAZPhysicsMeshSoftVerticesSet, DAZPhysicsMeshSoftVerticesGroup> _softSetToGroupDict;

	// Token: 0x04003F69 RID: 16233
	protected DAZPhysicsMeshSoftVerticesSet startSoftLinkSet;

	// Token: 0x04003F6A RID: 16234
	protected Vector3 zeroVector = Vector3.zero;

	// Token: 0x04003F6B RID: 16235
	[SerializeField]
	protected bool _allowSelfCollision;

	// Token: 0x04003F6C RID: 16236
	protected JSONStorableBool allowSelfCollisionJSON;

	// Token: 0x04003F6D RID: 16237
	public DAZPhysicsMesh[] ignorePhysicsMeshes;

	// Token: 0x04003F6E RID: 16238
	protected Mesh _editorMeshForFocus;

	// Token: 0x04003F6F RID: 16239
	protected bool _wasInit;

	// Token: 0x04003F70 RID: 16240
	public bool isEnabled;

	// Token: 0x04003F71 RID: 16241
	public bool updateEnabled = true;

	// Token: 0x02000B3F RID: 2879
	public enum DAZPhysicsMeshTaskType
	{
		// Token: 0x04003F73 RID: 16243
		UpdateSoftJointTargets,
		// Token: 0x04003F74 RID: 16244
		MorphVertices
	}

	// Token: 0x02000B40 RID: 2880
	public class DAZPhysicsMeshTaskInfo
	{
		// Token: 0x06004FAC RID: 20396 RVA: 0x001C7B6D File Offset: 0x001C5F6D
		public DAZPhysicsMeshTaskInfo()
		{
		}

		// Token: 0x04003F75 RID: 16245
		public DAZPhysicsMesh.DAZPhysicsMeshTaskType taskType;

		// Token: 0x04003F76 RID: 16246
		public int threadIndex;

		// Token: 0x04003F77 RID: 16247
		public string name;

		// Token: 0x04003F78 RID: 16248
		public AutoResetEvent resetEvent;

		// Token: 0x04003F79 RID: 16249
		public Thread thread;

		// Token: 0x04003F7A RID: 16250
		public volatile bool working;

		// Token: 0x04003F7B RID: 16251
		public volatile bool kill;

		// Token: 0x04003F7C RID: 16252
		public int index1;

		// Token: 0x04003F7D RID: 16253
		public int index2;
	}

	// Token: 0x02000B41 RID: 2881
	public enum SelectionMode
	{
		// Token: 0x04003F7F RID: 16255
		HardTarget,
		// Token: 0x04003F80 RID: 16256
		HardTargetAuto,
		// Token: 0x04003F81 RID: 16257
		SoftTarget,
		// Token: 0x04003F82 RID: 16258
		SoftAnchor,
		// Token: 0x04003F83 RID: 16259
		SoftInfluenced,
		// Token: 0x04003F84 RID: 16260
		SoftAuto,
		// Token: 0x04003F85 RID: 16261
		SoftLink,
		// Token: 0x04003F86 RID: 16262
		SoftSelect,
		// Token: 0x04003F87 RID: 16263
		SoftSpringSet,
		// Token: 0x04003F88 RID: 16264
		ColliderEditEnd1,
		// Token: 0x04003F89 RID: 16265
		ColliderEditEnd2,
		// Token: 0x04003F8A RID: 16266
		ColliderEditFront,
		// Token: 0x04003F8B RID: 16267
		SoftSizeSet,
		// Token: 0x04003F8C RID: 16268
		SoftLimitSet,
		// Token: 0x04003F8D RID: 16269
		SoftAutoRadius
	}
}
