using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MeshVR;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C03 RID: 3075
public class Atom : JSONStorable
{
	// Token: 0x060058C2 RID: 22722 RVA: 0x00208614 File Offset: 0x00206A14
	public Atom()
	{
	}

	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x060058C3 RID: 22723 RVA: 0x0020866B File Offset: 0x00206A6B
	// (set) Token: 0x060058C4 RID: 22724 RVA: 0x00208673 File Offset: 0x00206A73
	public SubScene subSceneComponent
	{
		[CompilerGenerated]
		get
		{
			return this.<subSceneComponent>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<subSceneComponent>k__BackingField = value;
		}
	}

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x060058C5 RID: 22725 RVA: 0x0020867C File Offset: 0x00206A7C
	// (set) Token: 0x060058C6 RID: 22726 RVA: 0x00208684 File Offset: 0x00206A84
	public bool isSubSceneRestore
	{
		[CompilerGenerated]
		get
		{
			return this.<isSubSceneRestore>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<isSubSceneRestore>k__BackingField = value;
		}
	}

	// Token: 0x060058C7 RID: 22727 RVA: 0x00208690 File Offset: 0x00206A90
	protected void SyncSelectContainingSubSceneButton()
	{
		if (this.SelectContainingSubSceneAction != null && this.SelectContainingSubSceneAction.dynamicButton != null)
		{
			this.SelectContainingSubSceneAction.dynamicButton.gameObject.SetActive(this._containingSubScene != null);
		}
	}

	// Token: 0x17000D0C RID: 3340
	// (get) Token: 0x060058C8 RID: 22728 RVA: 0x002086DF File Offset: 0x00206ADF
	// (set) Token: 0x060058C9 RID: 22729 RVA: 0x002086E7 File Offset: 0x00206AE7
	public SubScene containingSubScene
	{
		get
		{
			return this._containingSubScene;
		}
		protected set
		{
			if (this._containingSubScene != value)
			{
				this._containingSubScene = value;
				this.SyncSelectContainingSubSceneButton();
			}
		}
	}

	// Token: 0x060058CA RID: 22730 RVA: 0x00208708 File Offset: 0x00206B08
	protected void SelectContainingSubScene()
	{
		if (this._containingSubScene != null && this._containingSubScene.containingAtom != null && this._containingSubScene.containingAtom.mainController != null)
		{
			SuperController.singleton.SelectController(this._containingSubScene.containingAtom.mainController, false, true, true, true);
		}
	}

	// Token: 0x17000D0D RID: 3341
	// (get) Token: 0x060058CB RID: 22731 RVA: 0x00208775 File Offset: 0x00206B75
	// (set) Token: 0x060058CC RID: 22732 RVA: 0x0020877D File Offset: 0x00206B7D
	public bool useRigidbodyInterpolation
	{
		get
		{
			return this._useRigidbodyInterpolation;
		}
		set
		{
			if (this._useRigidbodyInterpolation != value)
			{
				this._useRigidbodyInterpolation = value;
				if (Application.isPlaying)
				{
					this.SyncRigidbodyInterpolation();
				}
			}
		}
	}

	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x060058CD RID: 22733 RVA: 0x002087A2 File Offset: 0x00206BA2
	// (set) Token: 0x060058CE RID: 22734 RVA: 0x002087AA File Offset: 0x00206BAA
	protected bool resetSimulation
	{
		get
		{
			return this._resetSimulation;
		}
		set
		{
			if (this._resetSimulation != value)
			{
				this._resetSimulation = value;
			}
		}
	}

	// Token: 0x060058CF RID: 22735 RVA: 0x002087C0 File Offset: 0x00206BC0
	protected void CheckResumeSimulation()
	{
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		bool flag = false;
		if (this.waitResumeSimulationFlags.Count > 0)
		{
			List<AsyncFlag> list = new List<AsyncFlag>();
			foreach (AsyncFlag asyncFlag in this.waitResumeSimulationFlags)
			{
				if (asyncFlag.Raised)
				{
					list.Add(asyncFlag);
					flag = true;
				}
			}
			foreach (AsyncFlag item in list)
			{
				this.waitResumeSimulationFlags.Remove(item);
			}
		}
		if (this.waitResumeSimulationFlags.Count > 0)
		{
			this.resetSimulation = true;
		}
		else if (flag)
		{
			this.resetSimulation = false;
		}
	}

	// Token: 0x060058D0 RID: 22736 RVA: 0x002088D4 File Offset: 0x00206CD4
	public void ResetSimulation(AsyncFlag af)
	{
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		this.waitResumeSimulationFlags.Add(af);
		this.resetSimulation = true;
		if (this._autoColliderBatchUpdaters != null)
		{
			foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this._autoColliderBatchUpdaters)
			{
				autoColliderBatchUpdater.ResetSimulation(af);
			}
		}
		if (this._physicsSimulators != null)
		{
			foreach (PhysicsSimulator physicsSimulator in this._physicsSimulators)
			{
				if (physicsSimulator.enabled)
				{
					physicsSimulator.ResetSimulation(af);
				}
			}
		}
		if (this._physicsSimulatorsStorable != null)
		{
			foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable in this._physicsSimulatorsStorable)
			{
				if (physicsSimulatorJSONStorable.enabled)
				{
					physicsSimulatorJSONStorable.ResetSimulation(af);
				}
			}
		}
		if (this._dynamicPhysicsSimulators != null)
		{
			foreach (PhysicsSimulator physicsSimulator2 in this._dynamicPhysicsSimulators)
			{
				physicsSimulator2.ResetSimulation(af);
			}
		}
		if (this._dynamicPhysicsSimulatorsStorable != null)
		{
			foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable2 in this._dynamicPhysicsSimulatorsStorable)
			{
				physicsSimulatorJSONStorable2.ResetSimulation(af);
			}
		}
	}

	// Token: 0x060058D1 RID: 22737 RVA: 0x00208A7C File Offset: 0x00206E7C
	protected IEnumerator ResetPhysicsCo(bool full = true, bool fullResetControls = true)
	{
		this._isResettingPhysics = true;
		this._isResettingPhysicsFull = full;
		this.resetPhysicsFlag = new AsyncFlag("Atom Reset Physics");
		this.ResetSimulation(this.resetPhysicsFlag);
		if (full)
		{
			this.resetPhysicsProgressJSON.val = 0f;
			float increment = 0.1f;
			this.resetPhysicsDisableCollision = true;
			this.ClearPauseAutoSimulationFlag();
			this.pauseAutoSimulationFlag = new AsyncFlag("Atom Pause Auto Simulation");
			SuperController.singleton.PauseAutoSimulation(this.pauseAutoSimulationFlag);
			for (int i = 0; i < 5; i++)
			{
				this.resetPhysicsProgressJSON.val += increment;
				if (fullResetControls)
				{
					foreach (FreeControllerV3 freeControllerV in this.containingAtom.freeControllers)
					{
						freeControllerV.SelectLinkToRigidbody(null);
					}
					foreach (FreeControllerV3 freeControllerV2 in this.containingAtom.freeControllers)
					{
						freeControllerV2.ResetControl();
					}
				}
				if (this._physicsResetters != null)
				{
					foreach (PhysicsResetter physicsResetter in this._physicsResetters)
					{
						physicsResetter.ResetPhysics();
					}
				}
				yield return null;
			}
			this.ClearPauseAutoSimulationFlag();
			this.resetPhysicsDisableCollision = false;
			for (int j = 0; j < 5; j++)
			{
				this.resetPhysicsProgressJSON.val += increment;
				yield return null;
			}
		}
		else
		{
			for (int k = 0; k < 5; k++)
			{
				yield return null;
			}
		}
		this.resetPhysicsFlag.Raise();
		this.resetPhysicsFlag = null;
		this.resetRoutine = null;
		this._isResettingPhysics = false;
		this._isResettingPhysicsFull = false;
		yield break;
	}

	// Token: 0x060058D2 RID: 22738 RVA: 0x00208AA5 File Offset: 0x00206EA5
	protected void ClearPauseAutoSimulationFlag()
	{
		if (this.pauseAutoSimulationFlag != null)
		{
			this.pauseAutoSimulationFlag.Raise();
			this.pauseAutoSimulationFlag = null;
		}
	}

	// Token: 0x060058D3 RID: 22739 RVA: 0x00208AC4 File Offset: 0x00206EC4
	protected void ClearResetPhysics()
	{
		if (this.resetRoutine != null)
		{
			base.StopCoroutine(this.resetRoutine);
			this.resetRoutine = null;
		}
		if (this.resetPhysicsFlag != null)
		{
			this.resetPhysicsFlag.Raise();
			this.resetPhysicsFlag = null;
		}
		this.resetPhysicsDisableCollision = false;
		this._isResettingPhysics = false;
		this._isResettingPhysicsFull = false;
	}

	// Token: 0x060058D4 RID: 22740 RVA: 0x00208B24 File Offset: 0x00206F24
	public void AlertPhysicsCorruption(string type)
	{
		if (!this._isResettingPhysicsFull)
		{
			this.ClearResetPhysics();
			SuperController.LogError("Detected physics corruption on Atom " + this.uid + ". Disabling collision on atom and force resetting physics to prevent game crash. Move atom to different location, then reenable collision. Type of corruption: " + type);
			this.collisionEnabledJSON.val = false;
			this.ResetPhysics(true, false);
		}
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x00208B71 File Offset: 0x00206F71
	public void ResetPhysics(bool fullReset = true, bool fullResetControls = true)
	{
		if (this.resetPhysicsFlag == null)
		{
			this.resetRoutine = base.StartCoroutine(this.ResetPhysicsCo(fullReset, fullResetControls));
		}
	}

	// Token: 0x060058D6 RID: 22742 RVA: 0x00208B92 File Offset: 0x00206F92
	protected void ResetPhysics()
	{
		this.ResetPhysics(true, true);
	}

	// Token: 0x060058D7 RID: 22743 RVA: 0x00208B9C File Offset: 0x00206F9C
	protected void SyncRigidbodyInterpolation()
	{
		if (this._realRigidbodies != null)
		{
			foreach (Rigidbody rigidbody in this._realRigidbodies)
			{
				RigidbodyAttributes component = rigidbody.GetComponent<RigidbodyAttributes>();
				if (component != null)
				{
					component.useInterpolation = (this._on && !this._tempOff && this._useRigidbodyInterpolation);
				}
				else if (this._on && !this._tempOff && this._useRigidbodyInterpolation)
				{
					if (!rigidbody.isKinematic)
					{
						rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
					}
				}
				else
				{
					rigidbody.interpolation = RigidbodyInterpolation.None;
				}
			}
		}
		if (this._physicsSimulators != null)
		{
			foreach (PhysicsSimulator physicsSimulator in this._physicsSimulators)
			{
				physicsSimulator.useInterpolation = this._useRigidbodyInterpolation;
			}
		}
		if (this._physicsSimulatorsStorable != null)
		{
			foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable in this._physicsSimulatorsStorable)
			{
				physicsSimulatorJSONStorable.useInterpolation = this._useRigidbodyInterpolation;
			}
		}
		if (this._dynamicPhysicsSimulators != null)
		{
			foreach (PhysicsSimulator physicsSimulator2 in this._dynamicPhysicsSimulators)
			{
				physicsSimulator2.useInterpolation = this._useRigidbodyInterpolation;
			}
		}
		if (this._dynamicPhysicsSimulatorsStorable != null)
		{
			foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable2 in this._dynamicPhysicsSimulatorsStorable)
			{
				physicsSimulatorJSONStorable2.useInterpolation = this._useRigidbodyInterpolation;
			}
		}
	}

	// Token: 0x060058D8 RID: 22744 RVA: 0x00208D90 File Offset: 0x00207190
	protected bool IsOnInAtomHierarchy(Atom currentAtom)
	{
		if (!(currentAtom != null))
		{
			return true;
		}
		if (currentAtom == this)
		{
			return this.on;
		}
		return this.IsOnInAtomHierarchy(currentAtom.parentAtom) && currentAtom.on;
	}

	// Token: 0x060058D9 RID: 22745 RVA: 0x00208DD0 File Offset: 0x002071D0
	protected void SyncOnToggleObjects()
	{
		if (!this._insideSyncOnToggleObjects)
		{
			this._insideSyncOnToggleObjects = true;
			bool active = this._on && !this._tempOff && this.IsOnInAtomHierarchy(this.parentAtom);
			if (this.onToggleObjects != null)
			{
				foreach (Transform transform in this.onToggleObjects)
				{
					if (transform != null)
					{
						transform.gameObject.SetActive(active);
					}
				}
			}
			foreach (Atom atom in this.childAtoms)
			{
				atom.SyncOnToggleObjects();
			}
			this._insideSyncOnToggleObjects = false;
		}
	}

	// Token: 0x060058DA RID: 22746 RVA: 0x00208EB0 File Offset: 0x002072B0
	protected void SyncOn(bool b)
	{
		this._on = b;
		this.SyncOnToggleObjects();
		this.SyncRigidbodyInterpolation();
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x00208EC5 File Offset: 0x002072C5
	public void SetOn(bool b)
	{
		this.onJSON.val = b;
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x00208ED3 File Offset: 0x002072D3
	public void ToggleOn()
	{
		if (this.onJSON != null)
		{
			this.onJSON.val = !this.onJSON.val;
		}
	}

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x060058DD RID: 22749 RVA: 0x00208EF9 File Offset: 0x002072F9
	public bool on
	{
		get
		{
			return this._on;
		}
	}

	// Token: 0x060058DE RID: 22750 RVA: 0x00208F01 File Offset: 0x00207301
	protected void SyncHidden(bool b)
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SyncHiddenAtoms();
		}
	}

	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x060058DF RID: 22751 RVA: 0x00208F1D File Offset: 0x0020731D
	// (set) Token: 0x060058E0 RID: 22752 RVA: 0x00208F3C File Offset: 0x0020733C
	public bool hidden
	{
		get
		{
			if (this.hiddenJSON != null)
			{
				return this.hiddenJSON.val;
			}
			return this._hidden;
		}
		set
		{
			if (this.hiddenJSON != null)
			{
				this.hiddenJSON.val = value;
			}
			else if (this._hidden != value)
			{
				this._hidden = value;
				this.SyncHidden(value);
			}
		}
	}

	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x060058E1 RID: 22753 RVA: 0x00208F74 File Offset: 0x00207374
	// (set) Token: 0x060058E2 RID: 22754 RVA: 0x00208F93 File Offset: 0x00207393
	public bool hiddenNoCallback
	{
		get
		{
			if (this.hiddenJSON != null)
			{
				return this.hiddenJSON.val;
			}
			return this._hidden;
		}
		set
		{
			if (this.hiddenJSON != null)
			{
				this.hiddenJSON.valNoCallback = value;
			}
			else if (this._hidden != value)
			{
				this._hidden = value;
				this.SyncHidden(value);
			}
		}
	}

	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x060058E3 RID: 22755 RVA: 0x00208FCB File Offset: 0x002073CB
	// (set) Token: 0x060058E4 RID: 22756 RVA: 0x00208FD3 File Offset: 0x002073D3
	public bool tempHidden
	{
		get
		{
			return this._tempHidden;
		}
		set
		{
			if (this._tempHidden != value)
			{
				this._tempHidden = value;
			}
		}
	}

	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x060058E5 RID: 22757 RVA: 0x00208FE8 File Offset: 0x002073E8
	// (set) Token: 0x060058E6 RID: 22758 RVA: 0x00208FF0 File Offset: 0x002073F0
	public bool tempOff
	{
		get
		{
			return this._tempOff;
		}
		set
		{
			if (this._tempOff != value)
			{
				this._tempOff = value;
			}
		}
	}

	// Token: 0x060058E7 RID: 22759 RVA: 0x00209005 File Offset: 0x00207405
	protected void SyncKeepParamLocksWhenPuttingBackInPool(bool b)
	{
		this._keepParamLocksWhenPuttingBackInPool = b;
	}

	// Token: 0x17000D14 RID: 3348
	// (get) Token: 0x060058E8 RID: 22760 RVA: 0x0020900E File Offset: 0x0020740E
	public bool keepParamLocksWhenPuttingBackInPool
	{
		get
		{
			return this._keepParamLocksWhenPuttingBackInPool;
		}
	}

	// Token: 0x17000D15 RID: 3349
	// (get) Token: 0x060058E9 RID: 22761 RVA: 0x00209016 File Offset: 0x00207416
	// (set) Token: 0x060058EA RID: 22762 RVA: 0x0020901E File Offset: 0x0020741E
	public bool tempFreezePhysics
	{
		get
		{
			return this._tempFreezePhysics;
		}
		set
		{
			if (this._tempFreezePhysics != value)
			{
				this._tempFreezePhysics = value;
				this.SyncFreezePhysics();
			}
		}
	}

	// Token: 0x17000D16 RID: 3350
	// (get) Token: 0x060058EB RID: 22763 RVA: 0x00209039 File Offset: 0x00207439
	// (set) Token: 0x060058EC RID: 22764 RVA: 0x00209044 File Offset: 0x00207444
	public bool grabFreezePhysics
	{
		get
		{
			return this._grabFreezePhysics;
		}
		set
		{
			if (this._grabFreezePhysics != value)
			{
				this._grabFreezePhysics = value;
				if (this._grabFreezePhysics)
				{
					this.grabFreezePhysicsFlag = new AsyncFlag("Grab Freeze Physics");
					if (SuperController.singleton != null)
					{
						SuperController.singleton.PauseAutoSimulation(this.grabFreezePhysicsFlag);
					}
				}
				else
				{
					this.grabFreezePhysicsFlag.Raise();
					this.grabFreezePhysicsFlag = null;
				}
				this.SyncFreezePhysics();
				if (this.subSceneComponent != null)
				{
					foreach (Atom atom in this.subSceneComponent.atomsInSubScene)
					{
						atom.grabFreezePhysics = this._grabFreezePhysics;
					}
				}
			}
		}
	}

	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x060058ED RID: 22765 RVA: 0x00209124 File Offset: 0x00207524
	// (set) Token: 0x060058EE RID: 22766 RVA: 0x0020912C File Offset: 0x0020752C
	public bool tempDisableCollision
	{
		get
		{
			return this._tempDisableCollision;
		}
		set
		{
			if (this._tempDisableCollision != value)
			{
				this._tempDisableCollision = value;
				this.SyncCollisionEnabled(this._collisionEnabled);
			}
		}
	}

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x060058EF RID: 22767 RVA: 0x0020914D File Offset: 0x0020754D
	// (set) Token: 0x060058F0 RID: 22768 RVA: 0x00209155 File Offset: 0x00207555
	protected bool resetPhysicsDisableCollision
	{
		get
		{
			return this._resetPhysicsDisableCollision;
		}
		set
		{
			if (this._resetPhysicsDisableCollision != value)
			{
				this._resetPhysicsDisableCollision = value;
				this.SyncCollisionEnabled(this._collisionEnabled);
			}
		}
	}

	// Token: 0x060058F1 RID: 22769 RVA: 0x00209178 File Offset: 0x00207578
	public void ScaleChanged(float sc)
	{
		this._currentScale = sc;
		if (this._scaleChangeReceivers != null)
		{
			foreach (ScaleChangeReceiver scaleChangeReceiver in this._scaleChangeReceivers)
			{
				scaleChangeReceiver.ScaleChanged(sc);
			}
		}
		if (this._scaleChangeReceiverJSONStorables != null)
		{
			foreach (ScaleChangeReceiverJSONStorable scaleChangeReceiverJSONStorable in this._scaleChangeReceiverJSONStorables)
			{
				scaleChangeReceiverJSONStorable.ScaleChanged(sc);
			}
		}
		if (this._dynamicScaleChangeReceivers != null)
		{
			foreach (ScaleChangeReceiver scaleChangeReceiver2 in this._dynamicScaleChangeReceivers)
			{
				scaleChangeReceiver2.ScaleChanged(sc);
			}
		}
		if (this._dynamicScaleChangeReceiverJSONStorables != null)
		{
			foreach (ScaleChangeReceiverJSONStorable scaleChangeReceiverJSONStorable2 in this._dynamicScaleChangeReceiverJSONStorables)
			{
				scaleChangeReceiverJSONStorable2.ScaleChanged(sc);
			}
		}
	}

	// Token: 0x060058F2 RID: 22770 RVA: 0x002092AC File Offset: 0x002076AC
	protected void SyncCollisionEnabled(bool b)
	{
		this._collisionEnabled = b;
		bool flag = this._collisionEnabled && (this.excludeFromTempDisableCollision || !this._tempDisableCollision) && !this._resetPhysicsDisableCollision;
		if (this._freeControllers != null)
		{
			foreach (FreeControllerV3 freeControllerV in this._freeControllers)
			{
				if (freeControllerV.controlsCollisionEnabled)
				{
					freeControllerV.globalCollisionEnabled = flag;
				}
			}
		}
		if (this._realRigidbodies != null)
		{
			foreach (Rigidbody rigidbody in this._realRigidbodies)
			{
				if (!this._collisionExemptRigidbodies.ContainsKey(rigidbody))
				{
					rigidbody.detectCollisions = flag;
				}
			}
		}
		if (this._physicsSimulators != null)
		{
			foreach (PhysicsSimulator physicsSimulator in this._physicsSimulators)
			{
				physicsSimulator.collisionEnabled = flag;
			}
		}
		if (this._physicsSimulatorsStorable != null)
		{
			foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable in this._physicsSimulatorsStorable)
			{
				physicsSimulatorJSONStorable.collisionEnabled = flag;
			}
		}
		if (this._dynamicPhysicsSimulators != null)
		{
			foreach (PhysicsSimulator physicsSimulator2 in this._dynamicPhysicsSimulators)
			{
				physicsSimulator2.collisionEnabled = flag;
			}
		}
		if (this._dynamicPhysicsSimulatorsStorable != null)
		{
			foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable2 in this._dynamicPhysicsSimulatorsStorable)
			{
				physicsSimulatorJSONStorable2.collisionEnabled = flag;
			}
		}
	}

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x060058F3 RID: 22771 RVA: 0x002094A4 File Offset: 0x002078A4
	// (set) Token: 0x060058F4 RID: 22772 RVA: 0x002094AC File Offset: 0x002078AC
	public bool collisionEnabled
	{
		get
		{
			return this._collisionEnabled;
		}
		set
		{
			if (this.collisionEnabledJSON != null)
			{
				this.collisionEnabledJSON.val = value;
			}
			else if (this._collisionEnabled != value)
			{
				this.SyncCollisionEnabled(value);
			}
		}
	}

	// Token: 0x060058F5 RID: 22773 RVA: 0x002094DD File Offset: 0x002078DD
	public void SetCollisionEnabled(bool b)
	{
		this.collisionEnabledJSON.val = b;
	}

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x060058F6 RID: 22774 RVA: 0x002094EB File Offset: 0x002078EB
	public bool isPhysicsFrozen
	{
		get
		{
			return this._isPhysicsFrozen;
		}
	}

	// Token: 0x060058F7 RID: 22775 RVA: 0x002094F4 File Offset: 0x002078F4
	protected void SyncFreezePhysics()
	{
		if ((!this.excludeFromTempFreezePhysics && this._tempFreezePhysics) || this._freezePhysics || this._grabFreezePhysics)
		{
			if (!this._isPhysicsFrozen)
			{
				if (this.saveRigidbodyContraints == null)
				{
					this.saveRigidbodyContraints = new Dictionary<Rigidbody, RigidbodyConstraints>();
				}
				else
				{
					this.saveRigidbodyContraints.Clear();
				}
				foreach (Rigidbody rigidbody in this._rigidbodies)
				{
					this.saveRigidbodyContraints.Add(rigidbody, rigidbody.constraints);
					rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				}
				this._isPhysicsFrozen = true;
				if (this._physicsSimulators != null)
				{
					foreach (PhysicsSimulator physicsSimulator in this._physicsSimulators)
					{
						physicsSimulator.freezeSimulation = true;
					}
				}
				if (this._physicsSimulatorsStorable != null)
				{
					foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable in this._physicsSimulatorsStorable)
					{
						physicsSimulatorJSONStorable.freezeSimulation = true;
					}
				}
				if (this._dynamicPhysicsSimulators != null)
				{
					foreach (PhysicsSimulator physicsSimulator2 in this._dynamicPhysicsSimulators)
					{
						physicsSimulator2.freezeSimulation = true;
					}
				}
				if (this._dynamicPhysicsSimulatorsStorable != null)
				{
					foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable2 in this._dynamicPhysicsSimulatorsStorable)
					{
						physicsSimulatorJSONStorable2.freezeSimulation = true;
					}
				}
				if (this.subSceneComponent != null && this.subSceneComponent.motionAnimationMaster != null)
				{
					this.subSceneComponent.motionAnimationMaster.freeze = true;
				}
			}
		}
		else if (this._isPhysicsFrozen)
		{
			foreach (Rigidbody rigidbody2 in this._rigidbodies)
			{
				RigidbodyConstraints constraints;
				if (this.saveRigidbodyContraints.TryGetValue(rigidbody2, out constraints))
				{
					rigidbody2.constraints = constraints;
				}
			}
			if (this._physicsSimulators != null)
			{
				foreach (PhysicsSimulator physicsSimulator3 in this._physicsSimulators)
				{
					physicsSimulator3.freezeSimulation = false;
				}
			}
			if (this._physicsSimulatorsStorable != null)
			{
				foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable3 in this._physicsSimulatorsStorable)
				{
					physicsSimulatorJSONStorable3.freezeSimulation = false;
				}
			}
			if (this._dynamicPhysicsSimulators != null)
			{
				foreach (PhysicsSimulator physicsSimulator4 in this._dynamicPhysicsSimulators)
				{
					physicsSimulator4.freezeSimulation = false;
				}
			}
			if (this._dynamicPhysicsSimulatorsStorable != null)
			{
				foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable4 in this._dynamicPhysicsSimulatorsStorable)
				{
					physicsSimulatorJSONStorable4.freezeSimulation = false;
				}
			}
			foreach (RigidbodyAttributes rigidbodyAttributes2 in this._rigidbodyAttributes)
			{
				rigidbodyAttributes2.SyncTensor();
			}
			if (this.subSceneComponent != null && this.subSceneComponent.motionAnimationMaster != null)
			{
				this.subSceneComponent.motionAnimationMaster.freeze = false;
			}
			this.ResetPhysics(false, true);
			this._isPhysicsFrozen = false;
		}
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x002098F4 File Offset: 0x00207CF4
	protected void SyncFreezePhysics(bool b)
	{
		this._freezePhysics = b;
		this.SyncFreezePhysics();
	}

	// Token: 0x060058F9 RID: 22777 RVA: 0x00209903 File Offset: 0x00207D03
	public void SetFreezePhysics(bool b)
	{
		this.freezePhysicsJSON.val = b;
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x00209914 File Offset: 0x00207D14
	public void ResetRigidbodies()
	{
		foreach (Rigidbody rigidbody in this._rigidbodies)
		{
			if (rigidbody.isKinematic)
			{
				rigidbody.isKinematic = false;
				rigidbody.isKinematic = true;
			}
		}
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x00209959 File Offset: 0x00207D59
	protected void IsolateEditAtom()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.StartIsolateEditAtom(this);
		}
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x00209978 File Offset: 0x00207D78
	protected void ZeroReParentObjectTransform()
	{
		if (this.reParentObject != null && !this.doNotZeroReParentObject)
		{
			List<Vector3> list = new List<Vector3>();
			List<Quaternion> list2 = new List<Quaternion>();
			IEnumerator enumerator = this.reParentObject.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					list.Add(transform.position);
					list2.Add(transform.rotation);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.reParentObject.localPosition = Vector3.zero;
			this.reParentObject.localRotation = Quaternion.identity;
			int num = 0;
			IEnumerator enumerator2 = this.reParentObject.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					Transform transform2 = (Transform)obj2;
					transform2.position = list[num];
					transform2.rotation = list2[num];
					num++;
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}
	}

	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x060058FD RID: 22781 RVA: 0x00209AB4 File Offset: 0x00207EB4
	// (set) Token: 0x060058FE RID: 22782 RVA: 0x00209ABC File Offset: 0x00207EBC
	public Atom parentAtom
	{
		get
		{
			return this._parentAtom;
		}
		set
		{
			if (this._parentAtom != value && this._parentAtom != this)
			{
				if (this._parentAtom != null)
				{
					this._parentAtom.RemoveChild(this);
				}
				this._parentAtom = value;
				if (this.reParentObject != null)
				{
					if (this._parentAtom != null)
					{
						if (this._parentAtom.childAtomContainer != null)
						{
							this.reParentObject.parent = this._parentAtom.childAtomContainer;
						}
						else
						{
							this.reParentObject.parent = this._parentAtom.transform;
						}
					}
					else
					{
						this.reParentObject.parent = base.transform;
					}
				}
				if (this._parentAtom != null)
				{
					this._parentAtom.AddChild(this);
				}
				if (SuperController.singleton != null)
				{
					SuperController.singleton.AtomParentChanged(this, value);
				}
				this.SyncOnToggleObjects();
			}
		}
	}

	// Token: 0x060058FF RID: 22783 RVA: 0x00209BCD File Offset: 0x00207FCD
	public HashSet<Atom> GetChildren()
	{
		return this.childAtoms;
	}

	// Token: 0x06005900 RID: 22784 RVA: 0x00209BD5 File Offset: 0x00207FD5
	public void AddChild(Atom a)
	{
		this.childAtoms.Add(a);
	}

	// Token: 0x06005901 RID: 22785 RVA: 0x00209BE4 File Offset: 0x00207FE4
	public void RemoveChild(Atom a)
	{
		this.childAtoms.Remove(a);
	}

	// Token: 0x06005902 RID: 22786 RVA: 0x00209BF4 File Offset: 0x00207FF4
	public void RemoveAllChildAtoms()
	{
		List<Atom> list = new List<Atom>();
		foreach (Atom item in this.childAtoms)
		{
			list.Add(item);
		}
		foreach (Atom atom in list)
		{
			atom.SelectAtomParent(null);
		}
	}

	// Token: 0x06005903 RID: 22787 RVA: 0x00209CA0 File Offset: 0x002080A0
	public void RecalculateSubScenePath()
	{
		string text = string.Empty;
		Atom parentAtom = this.parentAtom;
		while (parentAtom != null)
		{
			if (parentAtom.isSubSceneType)
			{
				text = parentAtom.uidWithoutSubScenePath + "/" + text;
			}
			parentAtom = parentAtom.parentAtom;
		}
		this.subScenePath = text;
	}

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x06005904 RID: 22788 RVA: 0x00209CF6 File Offset: 0x002080F6
	// (set) Token: 0x06005905 RID: 22789 RVA: 0x00209CFE File Offset: 0x002080FE
	public string subScenePath
	{
		get
		{
			return this._subScenePath;
		}
		protected set
		{
			if (this._subScenePath != value)
			{
				this._subScenePath = value;
				this.SetUID(this._uid);
			}
		}
	}

	// Token: 0x06005906 RID: 22790 RVA: 0x00209D24 File Offset: 0x00208124
	protected void SyncIdText()
	{
		if (this.idText != null)
		{
			this.idText.text = this._uid;
		}
		if (this.idTextAlt != null)
		{
			this.idTextAlt.text = this._uid;
		}
	}

	// Token: 0x06005907 RID: 22791 RVA: 0x00209D78 File Offset: 0x00208178
	public void SetUID(string val)
	{
		string text = Regex.Replace(val, ".*/", string.Empty);
		text = this._subScenePath + text;
		if (SuperController.singleton != null)
		{
			SuperController.singleton.RenameAtom(this, text);
		}
	}

	// Token: 0x06005908 RID: 22792 RVA: 0x00209DBF File Offset: 0x002081BF
	protected void SetUIDToInputField()
	{
		if (this.idText != null)
		{
			this.SetUID(this.idText.text);
		}
	}

	// Token: 0x06005909 RID: 22793 RVA: 0x00209DE3 File Offset: 0x002081E3
	protected void SetUIDToInputFieldAlt()
	{
		if (this.idTextAlt != null)
		{
			this.SetUID(this.idTextAlt.text);
		}
	}

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x0600590A RID: 22794 RVA: 0x00209E07 File Offset: 0x00208207
	// (set) Token: 0x0600590B RID: 22795 RVA: 0x00209E0F File Offset: 0x0020820F
	public string uid
	{
		get
		{
			return this._uid;
		}
		set
		{
			this._uid = value;
			this.SyncIdText();
		}
	}

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x0600590C RID: 22796 RVA: 0x00209E1E File Offset: 0x0020821E
	public string uidWithoutSubScenePath
	{
		get
		{
			return Regex.Replace(this._uid, this._subScenePath, string.Empty);
		}
	}

	// Token: 0x0600590D RID: 22797 RVA: 0x00209E38 File Offset: 0x00208238
	protected void SyncDescriptionText()
	{
		if (this.descriptionText != null)
		{
			this.descriptionText.text = this._description;
		}
		if (this.descriptionTextAlt != null)
		{
			this.descriptionTextAlt.text = this._description;
		}
	}

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x0600590E RID: 22798 RVA: 0x00209E89 File Offset: 0x00208289
	// (set) Token: 0x0600590F RID: 22799 RVA: 0x00209E91 File Offset: 0x00208291
	public string description
	{
		get
		{
			return this._description;
		}
		set
		{
			this._description = value;
			this.SyncDescriptionText();
		}
	}

	// Token: 0x06005910 RID: 22800 RVA: 0x00209EA0 File Offset: 0x002082A0
	private void SyncMasterControllerCorners()
	{
		List<Transform> list = new List<Transform>();
		if (this._masterController != null)
		{
			IEnumerator enumerator = this._masterController.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform item = (Transform)obj;
					list.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
		this.masterControllerCorners = list.ToArray();
	}

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x06005911 RID: 22801 RVA: 0x00209F30 File Offset: 0x00208330
	// (set) Token: 0x06005912 RID: 22802 RVA: 0x00209F38 File Offset: 0x00208338
	public FreeControllerV3 masterController
	{
		get
		{
			return this._masterController;
		}
		set
		{
			if (this._masterController != value)
			{
				this._masterController = value;
				this.SyncMasterControllerCorners();
			}
		}
	}

	// Token: 0x06005913 RID: 22803 RVA: 0x00209F58 File Offset: 0x00208358
	private void walkAndGetComponents(Transform t, List<ForceReceiver> receivers, List<ForceProducerV2> producers, List<GrabPoint> gpoints, List<FreeControllerV3> controllers, List<Rigidbody> rbs, List<Rigidbody> linkablerbs, List<Rigidbody> realrbs, List<RigidbodyAttributes> rbattrs, List<AnimationPattern> ans, List<AnimationStep> asts, List<Animator> anms, List<JSONStorable> jss, List<Canvas> cvs, List<AutoColliderBatchUpdater> acbus, List<PhysicsSimulator> psms, List<PhysicsSimulatorJSONStorable> psmjss, List<MotionAnimationControl> macs, List<PlayerNavCollider> pncs, List<JSONStorableDynamic> jsds, List<RhythmController> rcs, List<AudioSourceControl> ascs, List<ScaleChangeReceiver> scrs, List<ScaleChangeReceiverJSONStorable> scrjss, List<RenderSuspend> rss, List<MeshRenderer> mrs, List<PresetManagerControl> pmcs, Dictionary<ParticleSystemRenderer, ParticleSystemRenderMode> psrm, List<PhysicsResetter> prs, bool insidePhysicsSimulator)
	{
		Rigidbody component = t.GetComponent<Rigidbody>();
		if (component != null)
		{
			rbs.Add(component);
		}
		RigidbodyAttributes component2 = t.GetComponent<RigidbodyAttributes>();
		if (component2 != null)
		{
			rbattrs.Add(component2);
		}
		ForceReceiver component3 = t.GetComponent<ForceReceiver>();
		if (component3 != null)
		{
			component3.containingAtom = this;
			receivers.Add(component3);
		}
		ForceProducerV2 component4 = t.GetComponent<ForceProducerV2>();
		if (component4 != null)
		{
			component4.containingAtom = this;
			producers.Add(component4);
		}
		GrabPoint component5 = t.GetComponent<GrabPoint>();
		if (component5 != null)
		{
			component5.containingAtom = this;
			gpoints.Add(component5);
		}
		FreeControllerV3 component6 = t.GetComponent<FreeControllerV3>();
		if (component6 != null)
		{
			component6.containingAtom = this;
			controllers.Add(component6);
			if (component6.controlsCollisionEnabled && component6.followWhenOffRB != null)
			{
				this._collisionExemptRigidbodies.Add(component6.followWhenOffRB, true);
			}
		}
		MotionAnimationControl component7 = t.GetComponent<MotionAnimationControl>();
		if (component7 != null)
		{
			component7.containingAtom = this;
			macs.Add(component7);
		}
		PhysicsSimulator component8 = t.GetComponent<PhysicsSimulator>();
		if (component8 != null)
		{
			insidePhysicsSimulator = true;
			psms.Add(component8);
		}
		PhysicsResetter component9 = t.GetComponent<PhysicsResetter>();
		if (component9 != null)
		{
			prs.Add(component9);
		}
		PhysicsSimulatorJSONStorable component10 = t.GetComponent<PhysicsSimulatorJSONStorable>();
		if (component10 != null)
		{
			insidePhysicsSimulator = true;
			psmjss.Add(component10);
		}
		if (component != null && component6 == null && !insidePhysicsSimulator)
		{
			realrbs.Add(component);
		}
		if (component != null && (component3 != null || component6 != null))
		{
			linkablerbs.Add(component);
		}
		AnimationPattern component11 = t.GetComponent<AnimationPattern>();
		if (component11 != null)
		{
			component11.containingAtom = this;
			ans.Add(component11);
		}
		AnimationStep component12 = t.GetComponent<AnimationStep>();
		if (component12 != null)
		{
			component12.containingAtom = this;
			asts.Add(component12);
		}
		Animator component13 = t.GetComponent<Animator>();
		if (component13 != null)
		{
			anms.Add(component13);
		}
		Canvas component14 = t.GetComponent<Canvas>();
		if (component14 != null)
		{
			cvs.Add(component14);
		}
		JSONStorable[] components = t.GetComponents<JSONStorable>();
		if (components != null)
		{
			foreach (JSONStorable jsonstorable in components)
			{
				jsonstorable.containingAtom = this;
				jss.Add(jsonstorable);
			}
		}
		JSONStorableDynamic[] components2 = t.GetComponents<JSONStorableDynamic>();
		if (components2 != null)
		{
			foreach (JSONStorableDynamic jsonstorableDynamic in components2)
			{
				jsonstorableDynamic.containingAtom = this;
				jsds.Add(jsonstorableDynamic);
			}
		}
		PlayerNavCollider component15 = t.GetComponent<PlayerNavCollider>();
		if (component15 != null)
		{
			component15.containingAtom = this;
			pncs.Add(component15);
		}
		AutoColliderBatchUpdater component16 = t.GetComponent<AutoColliderBatchUpdater>();
		if (component16 != null)
		{
			acbus.Add(component16);
		}
		RhythmController component17 = t.GetComponent<RhythmController>();
		if (component17 != null)
		{
			component17.containingAtom = this;
			rcs.Add(component17);
		}
		AudioSourceControl component18 = t.GetComponent<AudioSourceControl>();
		if (component18 != null)
		{
			component18.containingAtom = this;
			ascs.Add(component18);
		}
		UIConnectorMaster component19 = t.GetComponent<UIConnectorMaster>();
		if (component19 != null)
		{
			component19.containingAtom = this;
		}
		SetTransformScale component20 = t.GetComponent<SetTransformScale>();
		if (component20 != null)
		{
			component20.containingAtom = this;
		}
		ScaleChangeReceiver[] components3 = t.GetComponents<ScaleChangeReceiver>();
		foreach (ScaleChangeReceiver scaleChangeReceiver in components3)
		{
			if (scaleChangeReceiver != null)
			{
				scrs.Add(scaleChangeReceiver);
			}
		}
		ScaleChangeReceiverJSONStorable[] components4 = t.GetComponents<ScaleChangeReceiverJSONStorable>();
		foreach (ScaleChangeReceiverJSONStorable scaleChangeReceiverJSONStorable in components4)
		{
			if (scaleChangeReceiverJSONStorable != null)
			{
				scrjss.Add(scaleChangeReceiverJSONStorable);
			}
		}
		RenderSuspend[] components5 = t.GetComponents<RenderSuspend>();
		foreach (RenderSuspend renderSuspend in components5)
		{
			if (renderSuspend != null)
			{
				rss.Add(renderSuspend);
			}
		}
		MeshRenderer[] components6 = t.GetComponents<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in components6)
		{
			if (meshRenderer != null && meshRenderer.enabled)
			{
				mrs.Add(meshRenderer);
			}
		}
		ParticleSystemRenderer[] components7 = t.GetComponents<ParticleSystemRenderer>();
		foreach (ParticleSystemRenderer particleSystemRenderer in components7)
		{
			psrm.Add(particleSystemRenderer, particleSystemRenderer.renderMode);
		}
		PresetManagerControl component21 = t.GetComponent<PresetManagerControl>();
		if (component21 != null)
		{
			pmcs.Add(component21);
		}
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (!transform.GetComponent<Atom>())
				{
					this.walkAndGetComponents(transform, receivers, producers, gpoints, controllers, rbs, linkablerbs, realrbs, rbattrs, ans, asts, anms, jss, cvs, acbus, psms, psmjss, macs, pncs, jsds, rcs, ascs, scrs, scrjss, rss, mrs, pmcs, psrm, prs, insidePhysicsSimulator);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06005914 RID: 22804 RVA: 0x0020A518 File Offset: 0x00208918
	private void Init()
	{
		if (this._wasInit)
		{
			UnityEngine.Debug.LogError("Init was already called");
		}
		this._wasInit = true;
		this.onJSON = new JSONStorableBool("on", this._on, new JSONStorableBool.SetBoolCallback(this.SyncOn));
		base.RegisterBool(this.onJSON);
		this.onJSON.isStorable = false;
		this.onJSON.isRestorable = false;
		this.toggleOnJSON = new JSONStorableAction("ToggleOn", new JSONStorableAction.ActionCallback(this.ToggleOn));
		base.RegisterAction(this.toggleOnJSON);
		this.hiddenJSON = new JSONStorableBool("hidden", this._hidden, new JSONStorableBool.SetBoolCallback(this.SyncHidden));
		this.hiddenJSON.storeType = JSONStorableParam.StoreType.Full;
		base.RegisterBool(this.hiddenJSON);
		this.keepParamLocksWhenPuttingBackInPoolJSON = new JSONStorableBool("keepParamLocksWhenPuttingBackInPool", this._keepParamLocksWhenPuttingBackInPool, new JSONStorableBool.SetBoolCallback(this.SyncKeepParamLocksWhenPuttingBackInPool));
		this.keepParamLocksWhenPuttingBackInPoolJSON.isStorable = false;
		this.keepParamLocksWhenPuttingBackInPoolJSON.isRestorable = false;
		base.RegisterBool(this.keepParamLocksWhenPuttingBackInPoolJSON);
		this.collisionEnabledJSON = new JSONStorableBool("collisionEnabled", this._collisionEnabled, new JSONStorableBool.SetBoolCallback(this.SyncCollisionEnabled));
		base.RegisterBool(this.collisionEnabledJSON);
		this.collisionEnabledJSON.isStorable = false;
		this.collisionEnabledJSON.isRestorable = false;
		this.freezePhysicsJSON = new JSONStorableBool("freezePhysics", this._freezePhysics, new JSONStorableBool.SetBoolCallback(this.SyncFreezePhysics));
		base.RegisterBool(this.freezePhysicsJSON);
		this.freezePhysicsJSON.isStorable = false;
		this.freezePhysicsJSON.isRestorable = false;
		this.IsolateEditAtomAction = new JSONStorableAction("IsolateEditAtom", new JSONStorableAction.ActionCallback(this.IsolateEditAtom));
		base.RegisterAction(this.IsolateEditAtomAction);
		this.SelectContainingSubSceneAction = new JSONStorableAction("SelectContainingSubScene", new JSONStorableAction.ActionCallback(this.SelectContainingSubScene));
		base.RegisterAction(this.SelectContainingSubSceneAction);
		this.ResetPhysicsJSONAction = new JSONStorableAction("ResetPhysics", new JSONStorableAction.ActionCallback(this.ResetPhysics));
		base.RegisterAction(this.ResetPhysicsJSONAction);
		this.resetPhysicsProgressJSON = new JSONStorableFloat("resetPhysicsProgress", 0f, 0f, 1f, true, true);
		if (this.isSubSceneType)
		{
			this.subSceneComponent = base.GetComponentInChildren<SubScene>(true);
		}
		this._collisionExemptRigidbodies = new Dictionary<Rigidbody, bool>();
		this.childAtoms = new HashSet<Atom>();
		List<ForceReceiver> list = new List<ForceReceiver>();
		List<ForceProducerV2> list2 = new List<ForceProducerV2>();
		List<GrabPoint> list3 = new List<GrabPoint>();
		List<FreeControllerV3> list4 = new List<FreeControllerV3>();
		List<Rigidbody> list5 = new List<Rigidbody>();
		List<Rigidbody> list6 = new List<Rigidbody>();
		List<Rigidbody> list7 = new List<Rigidbody>();
		List<RigidbodyAttributes> list8 = new List<RigidbodyAttributes>();
		List<AnimationPattern> list9 = new List<AnimationPattern>();
		List<AnimationStep> list10 = new List<AnimationStep>();
		List<Animator> list11 = new List<Animator>();
		List<Canvas> list12 = new List<Canvas>();
		List<JSONStorable> list13 = new List<JSONStorable>();
		List<AutoColliderBatchUpdater> list14 = new List<AutoColliderBatchUpdater>();
		List<PhysicsResetter> list15 = new List<PhysicsResetter>();
		List<PhysicsSimulator> list16 = new List<PhysicsSimulator>();
		List<PhysicsSimulatorJSONStorable> list17 = new List<PhysicsSimulatorJSONStorable>();
		List<MotionAnimationControl> list18 = new List<MotionAnimationControl>();
		List<PlayerNavCollider> list19 = new List<PlayerNavCollider>();
		List<JSONStorableDynamic> jsds = new List<JSONStorableDynamic>();
		List<RhythmController> list20 = new List<RhythmController>();
		List<AudioSourceControl> list21 = new List<AudioSourceControl>();
		List<ScaleChangeReceiver> list22 = new List<ScaleChangeReceiver>();
		List<ScaleChangeReceiverJSONStorable> list23 = new List<ScaleChangeReceiverJSONStorable>();
		List<RenderSuspend> list24 = new List<RenderSuspend>();
		List<MeshRenderer> list25 = new List<MeshRenderer>();
		Dictionary<ParticleSystemRenderer, ParticleSystemRenderMode> dictionary = new Dictionary<ParticleSystemRenderer, ParticleSystemRenderMode>();
		List<PresetManagerControl> list26 = new List<PresetManagerControl>();
		this.walkAndGetComponents(base.transform, list, list2, list3, list4, list5, list6, list7, list8, list9, list10, list11, list13, list12, list14, list16, list17, list18, list19, jsds, list20, list21, list22, list23, list24, list25, list26, dictionary, list15, false);
		this._forceReceivers = list.ToArray();
		this._forceProducers = list2.ToArray();
		this._rhythmControllers = list20.ToArray();
		this._audioSourceControls = list21.ToArray();
		this._grabPoints = list3.ToArray();
		this._freeControllers = list4.ToArray();
		this._rigidbodies = list5.ToArray();
		this._linkableRigidbodies = list6.ToArray();
		this._realRigidbodies = list7.ToArray();
		this._rigidbodyAttributes = list8.ToArray();
		this._animationPatterns = list9.ToArray();
		this._animationSteps = list10.ToArray();
		this._animators = list11.ToArray();
		this._motionAnimationsControls = list18.ToArray();
		this._playerNavColliders = list19.ToArray();
		this._canvases = list12;
		this._storables = list13;
		this._physicsResetters = list15.ToArray();
		this._physicsSimulators = list16.ToArray();
		this._physicsSimulatorsStorable = list17.ToArray();
		this._autoColliderBatchUpdaters = list14.ToArray();
		this._scaleChangeReceivers = list22.ToArray();
		this._scaleChangeReceiverJSONStorables = list23.ToArray();
		this._renderSuspends = list24;
		this._meshRenderers = list25;
		this._particleSystemRenderers = dictionary;
		this.presetManagerControls = list26;
		this._storableById = new Dictionary<string, JSONStorable>();
		foreach (JSONStorable jsonstorable in this._storables)
		{
			if (!jsonstorable.exclude)
			{
				if (this._storableById.ContainsKey(jsonstorable.storeId))
				{
					UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"Found duplicate storable uid ",
						jsonstorable.storeId,
						" in atom ",
						this.uid,
						" of type ",
						this.type
					}));
				}
				else
				{
					this._storableById.Add(jsonstorable.storeId, jsonstorable);
				}
			}
		}
		this.SyncOnToggleObjects();
		this.SyncRigidbodyInterpolation();
		this.SyncCollisionEnabled(this._collisionEnabled);
		if (SuperController.singleton != null)
		{
			this._callbackRegistered = true;
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRenamed));
			SuperController singleton2 = SuperController.singleton;
			singleton2.onAtomSubSceneChangedHandlers = (SuperController.OnAtomSubSceneChanged)Delegate.Combine(singleton2.onAtomSubSceneChangedHandlers, new SuperController.OnAtomSubSceneChanged(this.OnAtomSubSceneChanged));
		}
	}

	// Token: 0x06005915 RID: 22805 RVA: 0x0020AB10 File Offset: 0x00208F10
	public List<string> GetStorableIDs()
	{
		List<string> list = new List<string>();
		foreach (string text in this._storableById.Keys)
		{
			JSONStorable jsonstorable;
			if (this._storableById.TryGetValue(text, out jsonstorable) && !jsonstorable.exclude && (!jsonstorable.onlyStoreIfActive || jsonstorable.gameObject.activeInHierarchy))
			{
				list.Add(text);
			}
		}
		list.Sort();
		return list;
	}

	// Token: 0x06005916 RID: 22806 RVA: 0x0020ABB8 File Offset: 0x00208FB8
	public JSONStorable GetStorableByID(string storeid)
	{
		JSONStorable result = null;
		this._storableById.TryGetValue(storeid, out result);
		return result;
	}

	// Token: 0x06005917 RID: 22807 RVA: 0x0020ABD8 File Offset: 0x00208FD8
	public void Store(JSONArray atoms, bool includePhysical = true, bool includeAppearance = true)
	{
		JSONClass jsonclass = new JSONClass();
		jsonclass["id"] = this.uid;
		if (includePhysical)
		{
			jsonclass["on"].AsBool = this._on;
		}
		if (includePhysical && this.collisionEnabledJSON.val != this.collisionEnabledJSON.defaultVal)
		{
			jsonclass["collisionEnabled"].AsBool = this._collisionEnabled;
		}
		if (includePhysical && this.freezePhysicsJSON.val != this.freezePhysicsJSON.defaultVal)
		{
			jsonclass["freezePhysics"].AsBool = this._freezePhysics;
		}
		if (this.type != null)
		{
			jsonclass["type"] = this.type;
		}
		else
		{
			UnityEngine.Debug.LogWarning("Atom " + this.uid + " does not have a type set");
		}
		if (this.parentAtom != null)
		{
			jsonclass["parentAtom"] = this.parentAtom.uid;
		}
		if (this.reParentObject != null && includePhysical)
		{
			Vector3 position = this.reParentObject.position;
			jsonclass["position"]["x"].AsFloat = position.x;
			jsonclass["position"]["y"].AsFloat = position.y;
			jsonclass["position"]["z"].AsFloat = position.z;
			Vector3 eulerAngles = this.reParentObject.eulerAngles;
			jsonclass["rotation"]["x"].AsFloat = eulerAngles.x;
			jsonclass["rotation"]["y"].AsFloat = eulerAngles.y;
			jsonclass["rotation"]["z"].AsFloat = eulerAngles.z;
		}
		if (this.childAtomContainer != null && includePhysical)
		{
			Vector3 position2 = this.childAtomContainer.position;
			jsonclass["containerPosition"]["x"].AsFloat = position2.x;
			jsonclass["containerPosition"]["y"].AsFloat = position2.y;
			jsonclass["containerPosition"]["z"].AsFloat = position2.z;
			Vector3 eulerAngles2 = this.childAtomContainer.eulerAngles;
			jsonclass["containerRotation"]["x"].AsFloat = eulerAngles2.x;
			jsonclass["containerRotation"]["y"].AsFloat = eulerAngles2.y;
			jsonclass["containerRotation"]["z"].AsFloat = eulerAngles2.z;
		}
		atoms.Add(jsonclass);
		JSONArray jsonarray = new JSONArray();
		jsonclass["storables"] = jsonarray;
		foreach (JSONStorable jsonstorable in this._storables)
		{
			if (jsonstorable != null && !jsonstorable.exclude)
			{
				if (jsonstorable.onlyStoreIfActive)
				{
					if (!jsonstorable.gameObject.activeInHierarchy)
					{
						continue;
					}
				}
				try
				{
					JSONClass json = jsonstorable.GetJSON(includePhysical, includeAppearance, false);
					if (jsonstorable.needsStore)
					{
						jsonarray.Add(json);
					}
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during Store of ",
						jsonstorable.storeId,
						": ",
						ex
					}));
				}
			}
		}
	}

	// Token: 0x06005918 RID: 22808 RVA: 0x0020AFE8 File Offset: 0x002093E8
	public void StoreForSubScene(JSONClass jc, bool isTheSubSceneAtom = false)
	{
		if (!isTheSubSceneAtom)
		{
			jc["id"] = this.uidWithoutSubScenePath;
			jc["type"] = this.type;
			jc["on"].AsBool = this._on;
			jc["collisionEnabled"].AsBool = this._collisionEnabled;
			if (this.parentAtom != null && !this.parentAtom.isSubSceneType)
			{
				jc["parentAtom"] = this.parentAtom.uidWithoutSubScenePath;
			}
			if (this.reParentObject != null)
			{
				Vector3 localPosition = this.reParentObject.localPosition;
				jc["localPosition"]["x"].AsFloat = localPosition.x;
				jc["localPosition"]["y"].AsFloat = localPosition.y;
				jc["localPosition"]["z"].AsFloat = localPosition.z;
				Vector3 localEulerAngles = this.reParentObject.localEulerAngles;
				jc["localRotation"]["x"].AsFloat = localEulerAngles.x;
				jc["localRotation"]["y"].AsFloat = localEulerAngles.y;
				jc["localRotation"]["z"].AsFloat = localEulerAngles.z;
			}
			if (this.childAtomContainer != null)
			{
				Vector3 localPosition2 = this.childAtomContainer.localPosition;
				jc["containerLocalPosition"]["x"].AsFloat = localPosition2.x;
				jc["containerLocalPosition"]["y"].AsFloat = localPosition2.y;
				jc["containerLocalPosition"]["z"].AsFloat = localPosition2.z;
				Vector3 localEulerAngles2 = this.childAtomContainer.localEulerAngles;
				jc["containerLocalRotation"]["x"].AsFloat = localEulerAngles2.x;
				jc["containerLocalRotation"]["y"].AsFloat = localEulerAngles2.y;
				jc["containerLocalRotation"]["z"].AsFloat = localEulerAngles2.z;
			}
		}
		JSONArray jsonarray = new JSONArray();
		jc["storables"] = jsonarray;
		foreach (JSONStorable jsonstorable in this._storables)
		{
			if (jsonstorable != null && !jsonstorable.exclude)
			{
				if (!isTheSubSceneAtom || (!(jsonstorable.gameObject == this.mainController.gameObject) && !(jsonstorable.gameObject == this.childAtomContainer.gameObject) && !(jsonstorable is Atom) && !(jsonstorable is SubScene)))
				{
					if (jsonstorable.onlyStoreIfActive)
					{
						if (!jsonstorable.gameObject.activeInHierarchy)
						{
							continue;
						}
					}
					try
					{
						if (isTheSubSceneAtom)
						{
							jsonstorable.subScenePrefix = this.uid + "/";
						}
						else
						{
							jsonstorable.subScenePrefix = this.subScenePath;
						}
						JSONClass json = jsonstorable.GetJSON(true, true, true);
						jsonstorable.subScenePrefix = null;
						jsonarray.Add(json);
					}
					catch (Exception ex)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception during SubScene Store of ",
							jsonstorable.storeId,
							": ",
							ex
						}));
					}
				}
			}
		}
	}

	// Token: 0x06005919 RID: 22809 RVA: 0x0020B408 File Offset: 0x00209808
	public void RestoreForceInitialize()
	{
		this.onJSON.val = true;
	}

	// Token: 0x0600591A RID: 22810 RVA: 0x0020B416 File Offset: 0x00209816
	public void RestoreStartingOn()
	{
		this.onJSON.val = this.onJSON.defaultVal;
	}

	// Token: 0x0600591B RID: 22811 RVA: 0x0020B430 File Offset: 0x00209830
	public void RestoreTransform(JSONClass jc, bool setUnlistedToDefault = true)
	{
		if (this.reParentObject != null)
		{
			if (jc["position"] != null)
			{
				Vector3 position = this.reParentObject.position;
				if (jc["position"]["x"] != null)
				{
					position.x = jc["position"]["x"].AsFloat;
				}
				if (jc["position"]["y"] != null)
				{
					position.y = jc["position"]["y"].AsFloat;
				}
				if (jc["position"]["z"] != null)
				{
					position.z = jc["position"]["z"].AsFloat;
				}
				this.reParentObject.position = position;
			}
			else if (jc["localPosition"] != null)
			{
				Vector3 localPosition = this.reParentObject.localPosition;
				if (jc["localPosition"]["x"] != null)
				{
					localPosition.x = jc["localPosition"]["x"].AsFloat;
				}
				if (jc["localPosition"]["y"] != null)
				{
					localPosition.y = jc["localPosition"]["y"].AsFloat;
				}
				if (jc["localPosition"]["z"] != null)
				{
					localPosition.z = jc["localPosition"]["z"].AsFloat;
				}
				this.reParentObject.localPosition = localPosition;
			}
			else if (setUnlistedToDefault)
			{
				this.reParentObject.position = this.reParentObjectStartingPosition;
			}
			if (jc["rotation"] != null)
			{
				Vector3 eulerAngles = this.reParentObject.eulerAngles;
				if (jc["rotation"]["x"] != null)
				{
					eulerAngles.x = jc["rotation"]["x"].AsFloat;
				}
				if (jc["rotation"]["y"] != null)
				{
					eulerAngles.y = jc["rotation"]["y"].AsFloat;
				}
				if (jc["rotation"]["z"] != null)
				{
					eulerAngles.z = jc["rotation"]["z"].AsFloat;
				}
				this.reParentObject.eulerAngles = eulerAngles;
			}
			else if (jc["localRotation"] != null)
			{
				Vector3 localEulerAngles = this.reParentObject.localEulerAngles;
				if (jc["localRotation"]["x"] != null)
				{
					localEulerAngles.x = jc["localRotation"]["x"].AsFloat;
				}
				if (jc["localRotation"]["y"] != null)
				{
					localEulerAngles.y = jc["localRotation"]["y"].AsFloat;
				}
				if (jc["localRotation"]["z"] != null)
				{
					localEulerAngles.z = jc["localRotation"]["z"].AsFloat;
				}
				this.reParentObject.localEulerAngles = localEulerAngles;
			}
			else if (setUnlistedToDefault)
			{
				this.reParentObject.rotation = this.reParentObjectStartingRotation;
			}
		}
		if (this.childAtomContainer != null)
		{
			if (jc["containerPosition"] != null)
			{
				Vector3 position2 = this.childAtomContainer.position;
				if (jc["containerPosition"]["x"] != null)
				{
					position2.x = jc["containerPosition"]["x"].AsFloat;
				}
				if (jc["containerPosition"]["y"] != null)
				{
					position2.y = jc["containerPosition"]["y"].AsFloat;
				}
				if (jc["containerPosition"]["z"] != null)
				{
					position2.z = jc["containerPosition"]["z"].AsFloat;
				}
				this.childAtomContainer.position = position2;
			}
			else if (jc["containerLocalPosition"] != null)
			{
				Vector3 localPosition2 = this.childAtomContainer.localPosition;
				if (jc["containerLocalPosition"]["x"] != null)
				{
					localPosition2.x = jc["containerLocalPosition"]["x"].AsFloat;
				}
				if (jc["containerLocalPosition"]["y"] != null)
				{
					localPosition2.y = jc["containerLocalPosition"]["y"].AsFloat;
				}
				if (jc["containerLocalPosition"]["z"] != null)
				{
					localPosition2.z = jc["containerLocalPosition"]["z"].AsFloat;
				}
				this.childAtomContainer.localPosition = localPosition2;
			}
			else if (setUnlistedToDefault)
			{
				this.childAtomContainer.position = this.childAtomContainerStartingPosition;
			}
			if (jc["containerRotation"] != null)
			{
				Vector3 eulerAngles2 = this.childAtomContainer.eulerAngles;
				if (jc["containerRotation"]["x"] != null)
				{
					eulerAngles2.x = jc["containerRotation"]["x"].AsFloat;
				}
				if (jc["containerRotation"]["y"] != null)
				{
					eulerAngles2.y = jc["containerRotation"]["y"].AsFloat;
				}
				if (jc["containerRotation"]["z"] != null)
				{
					eulerAngles2.z = jc["containerRotation"]["z"].AsFloat;
				}
				this.childAtomContainer.eulerAngles = eulerAngles2;
			}
			else if (jc["containerLocalRotation"] != null)
			{
				Vector3 localEulerAngles2 = this.childAtomContainer.localEulerAngles;
				if (jc["containerLocalRotation"]["x"] != null)
				{
					localEulerAngles2.x = jc["containerLocalRotation"]["x"].AsFloat;
				}
				if (jc["containerLocalRotation"]["y"] != null)
				{
					localEulerAngles2.y = jc["containerLocalRotation"]["y"].AsFloat;
				}
				if (jc["containerLocalRotation"]["z"] != null)
				{
					localEulerAngles2.z = jc["containerLocalRotation"]["z"].AsFloat;
				}
				this.childAtomContainer.localEulerAngles = localEulerAngles2;
			}
			else if (setUnlistedToDefault)
			{
				this.childAtomContainer.rotation = this.childAtomContainerStartingRotation;
			}
		}
	}

	// Token: 0x0600591C RID: 22812 RVA: 0x0020BC73 File Offset: 0x0020A073
	public void ClearParentAtom()
	{
		this.SelectAtomParent(null);
	}

	// Token: 0x0600591D RID: 22813 RVA: 0x0020BC7C File Offset: 0x0020A07C
	public void RestoreParentAtom(JSONClass jc)
	{
		if (jc["parentAtom"] != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(jc["parentAtom"]);
			this.SelectAtomParent(atomByUid);
		}
		else
		{
			this.SelectAtomParent(null);
		}
	}

	// Token: 0x0600591E RID: 22814 RVA: 0x0020BCCD File Offset: 0x0020A0CD
	public void SetLastRestoredData(JSONClass jc, bool isAppearance = true, bool isPhysical = true)
	{
		this.lastRestoredData = jc;
		this.lastRestorePhysical = isPhysical;
		this.lastRestoreAppearance = isAppearance;
	}

	// Token: 0x0600591F RID: 22815 RVA: 0x0020BCE4 File Offset: 0x0020A0E4
	public void RestoreFromLast(JSONStorable js)
	{
		if (this.lastRestoredData != null)
		{
			bool flag = false;
			IEnumerator enumerator = this.lastRestoredData["storables"].AsArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					JSONClass jsonclass = (JSONClass)obj;
					string a = jsonclass["id"];
					if (a == js.storeId)
					{
						flag = true;
						try
						{
							js.RestoreFromJSON(jsonclass, this.lastRestorePhysical, this.lastRestoreAppearance, null, true);
							js.LateRestoreFromJSON(jsonclass, this.lastRestorePhysical, this.lastRestoreAppearance, true);
						}
						catch (Exception ex)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during RestoreFromLast of ",
								js.storeId,
								": ",
								ex
							}));
						}
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (!flag)
			{
				JSONClass jc = new JSONClass();
				try
				{
					js.RestoreFromJSON(jc, this.lastRestorePhysical, this.lastRestoreAppearance, null, true);
				}
				catch (Exception ex2)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during RestoreFromLast of ",
						js.storeId,
						": ",
						ex2
					}));
				}
			}
		}
	}

	// Token: 0x06005920 RID: 22816 RVA: 0x0020BE68 File Offset: 0x0020A268
	public void Restore(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool restoreCore = true, JSONArray presetAtoms = null, bool isClear = false, bool isSubSceneRestore = false, bool setMissingToDefault = true, bool isTheSubSceneAtom = false)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		this.lastRestoredData = jc;
		this.lastRestoreAppearance = restoreAppearance;
		this.lastRestorePhysical = restorePhysical;
		if (restoreCore)
		{
			if (jc["on"] != null)
			{
				this.onJSON.val = jc["on"].AsBool;
			}
			else if (setMissingToDefault)
			{
				this.onJSON.val = this.onJSON.defaultVal;
			}
		}
		IEnumerator enumerator = jc["storables"].AsArray.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				JSONClass jsonclass = (JSONClass)obj;
				JSONStorable jsonstorable;
				if (this._storableById.TryGetValue(jsonclass["id"], out jsonstorable) && jsonstorable != null && !jsonstorable.exclude)
				{
					if (!isTheSubSceneAtom || (!(jsonstorable.gameObject == this.mainController.gameObject) && !(jsonstorable.gameObject == this.childAtomContainer.gameObject) && !(jsonstorable is Atom) && !(jsonstorable is SubScene)))
					{
						if (isSubSceneRestore)
						{
							if (isTheSubSceneAtom)
							{
								jsonstorable.subScenePrefix = this.uid + "/";
							}
							else
							{
								jsonstorable.subScenePrefix = this.subScenePath;
							}
						}
						try
						{
							jsonstorable.RestoreFromJSON(jsonclass, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
						}
						catch (Exception ex)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during Restore of ",
								jsonstorable.storeId,
								": ",
								ex
							}));
						}
						if (isSubSceneRestore)
						{
							jsonstorable.subScenePrefix = null;
						}
						if (!dictionary.ContainsKey(jsonclass["id"]))
						{
							dictionary.Add(jsonclass["id"], true);
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (setMissingToDefault)
		{
			JSONStorable[] array = this._storables.ToArray();
			foreach (JSONStorable jsonstorable2 in array)
			{
				if (jsonstorable2 != null && !jsonstorable2.exclude)
				{
					if (!isTheSubSceneAtom || (!(jsonstorable2 == null) && !(jsonstorable2.gameObject == this.mainController.gameObject) && !(jsonstorable2.gameObject == this.childAtomContainer.gameObject) && !(jsonstorable2 is Atom) && !(jsonstorable2 is SubScene)))
					{
						if (!dictionary.ContainsKey(jsonstorable2.storeId))
						{
							if (!isClear && jsonstorable2.onlyStoreIfActive)
							{
								if (!jsonstorable2.gameObject.activeInHierarchy)
								{
									goto IL_33D;
								}
							}
							try
							{
								JSONClass jc2 = new JSONClass();
								jsonstorable2.RestoreFromJSON(jc2, restorePhysical, restoreAppearance, null, true);
							}
							catch (Exception ex2)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during Restore of ",
									jsonstorable2.storeId,
									": ",
									ex2
								}));
							}
						}
					}
				}
				IL_33D:;
			}
		}
	}

	// Token: 0x06005921 RID: 22817 RVA: 0x0020C210 File Offset: 0x0020A610
	public void LateRestore(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool restoreCore = true, bool isSubSceneRestore = false, bool setMissingToDefault = true, bool isTheSubSceneAtom = false)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		if (restoreCore)
		{
			if (jc["collisionEnabled"] != null)
			{
				this.collisionEnabledJSON.val = jc["collisionEnabled"].AsBool;
			}
			else if (setMissingToDefault)
			{
				this.collisionEnabledJSON.val = this.collisionEnabledJSON.defaultVal;
			}
			if (jc["freezePhysics"] != null)
			{
				this.freezePhysicsJSON.val = jc["freezePhysics"].AsBool;
			}
			else if (setMissingToDefault)
			{
				this.freezePhysicsJSON.val = this.freezePhysicsJSON.defaultVal;
			}
		}
		IEnumerator enumerator = jc["storables"].AsArray.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				JSONClass jsonclass = (JSONClass)obj;
				JSONStorable jsonstorable;
				if (this._storableById.TryGetValue(jsonclass["id"], out jsonstorable) && jsonstorable != null && !jsonstorable.exclude)
				{
					if (!isTheSubSceneAtom || (!(jsonstorable == null) && !(jsonstorable.gameObject == this.mainController.gameObject) && !(jsonstorable.gameObject == this.childAtomContainer.gameObject) && !(jsonstorable is Atom) && !(jsonstorable is SubScene)))
					{
						if (isSubSceneRestore)
						{
							if (isTheSubSceneAtom)
							{
								jsonstorable.subScenePrefix = this.uid + "/";
							}
							else
							{
								jsonstorable.subScenePrefix = this.subScenePath;
							}
						}
						try
						{
							jsonstorable.LateRestoreFromJSON(jsonclass, restorePhysical, restoreAppearance, setMissingToDefault);
						}
						catch (Exception ex)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during LateRestore of ",
								jsonstorable.storeId,
								": ",
								ex
							}));
						}
						if (isSubSceneRestore)
						{
							jsonstorable.subScenePrefix = null;
						}
						if (!dictionary.ContainsKey(jsonclass["id"]))
						{
							dictionary.Add(jsonclass["id"], true);
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (setMissingToDefault)
		{
			JSONStorable[] array = this._storables.ToArray();
			foreach (JSONStorable jsonstorable2 in array)
			{
				if (jsonstorable2 != null && !jsonstorable2.exclude)
				{
					if (!isTheSubSceneAtom || (!(jsonstorable2.gameObject == this.mainController.gameObject) && !(jsonstorable2.gameObject == this.childAtomContainer.gameObject) && !(jsonstorable2 is Atom) && !(jsonstorable2 is SubScene)))
					{
						if (!dictionary.ContainsKey(jsonstorable2.storeId))
						{
							try
							{
								JSONClass jc2 = new JSONClass();
								jsonstorable2.LateRestoreFromJSON(jc2, restorePhysical, restoreAppearance, true);
							}
							catch (Exception ex2)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during LateRestore of ",
									jsonstorable2.storeId,
									": ",
									ex2
								}));
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06005922 RID: 22818 RVA: 0x0020C5D0 File Offset: 0x0020A9D0
	public new void Validate()
	{
		foreach (JSONStorable jsonstorable in this._storables)
		{
			jsonstorable.Validate();
		}
	}

	// Token: 0x06005923 RID: 22819 RVA: 0x0020C62C File Offset: 0x0020AA2C
	public void PreRestoreForSubScene()
	{
		JSONStorable[] array = this._storables.ToArray();
		foreach (JSONStorable jsonstorable in array)
		{
			if (jsonstorable != null && !jsonstorable.exclude)
			{
				if (!(jsonstorable.gameObject == this.mainController.gameObject) && !(jsonstorable.gameObject == this.childAtomContainer.gameObject) && !(jsonstorable is Atom) && !(jsonstorable is SubScene))
				{
					try
					{
						jsonstorable.PreRestore();
						jsonstorable.PreRestore(true, true);
					}
					catch (Exception ex)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception during PreRestore of ",
							jsonstorable.storeId,
							": ",
							ex
						}));
					}
				}
			}
		}
	}

	// Token: 0x06005924 RID: 22820 RVA: 0x0020C720 File Offset: 0x0020AB20
	public new void PreRestore()
	{
		this.PreRestore(true, true);
	}

	// Token: 0x06005925 RID: 22821 RVA: 0x0020C72C File Offset: 0x0020AB2C
	public new void PreRestore(bool restorePhysical, bool restoreAppearance)
	{
		JSONStorable[] array = this._storables.ToArray();
		foreach (JSONStorable jsonstorable in array)
		{
			if (jsonstorable != null && !jsonstorable.exclude)
			{
				try
				{
					jsonstorable.PreRestore();
					jsonstorable.PreRestore(restorePhysical, restoreAppearance);
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during PreRestore of ",
						jsonstorable.storeId,
						": ",
						ex
					}));
				}
			}
		}
	}

	// Token: 0x06005926 RID: 22822 RVA: 0x0020C7D0 File Offset: 0x0020ABD0
	public new void PostRestore()
	{
		this.PostRestore(true, true);
	}

	// Token: 0x06005927 RID: 22823 RVA: 0x0020C7DC File Offset: 0x0020ABDC
	public new void PostRestore(bool restorePhysical, bool restoreAppearance)
	{
		JSONStorable[] array = this._storables.ToArray();
		foreach (JSONStorable jsonstorable in array)
		{
			if (jsonstorable != null && !jsonstorable.exclude)
			{
				try
				{
					jsonstorable.PostRestore();
					jsonstorable.PostRestore(restorePhysical, restoreAppearance);
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during PostRestore of ",
						jsonstorable.storeId,
						": ",
						ex
					}));
				}
			}
		}
	}

	// Token: 0x06005928 RID: 22824 RVA: 0x0020C880 File Offset: 0x0020AC80
	public void OnPreRemove()
	{
		JSONStorable[] array = this._storables.ToArray();
		foreach (JSONStorable jsonstorable in array)
		{
			try
			{
				jsonstorable.PreRemove();
			}
			catch (Exception ex)
			{
				SuperController.LogError(string.Concat(new object[]
				{
					"Exception during PreRemove of ",
					jsonstorable.storeId,
					": ",
					ex
				}));
			}
		}
	}

	// Token: 0x06005929 RID: 22825 RVA: 0x0020C904 File Offset: 0x0020AD04
	public new void Remove()
	{
		SuperController.singleton.RemoveAtom(this);
	}

	// Token: 0x0600592A RID: 22826 RVA: 0x0020C914 File Offset: 0x0020AD14
	public void OnRemove()
	{
		JSONStorable[] array = this._storables.ToArray();
		foreach (JSONStorable jsonstorable in array)
		{
			if (jsonstorable != null)
			{
				try
				{
					jsonstorable.Remove();
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during Remove of ",
						jsonstorable.storeId,
						": ",
						ex
					}));
				}
			}
		}
	}

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x0600592B RID: 22827 RVA: 0x0020C9A4 File Offset: 0x0020ADA4
	// (set) Token: 0x0600592C RID: 22828 RVA: 0x0020C9AC File Offset: 0x0020ADAC
	public bool isPreparingToPutBackInPool
	{
		[CompilerGenerated]
		get
		{
			return this.<isPreparingToPutBackInPool>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<isPreparingToPutBackInPool>k__BackingField = value;
		}
	}

	// Token: 0x0600592D RID: 22829 RVA: 0x0020C9B8 File Offset: 0x0020ADB8
	public void PrepareToPutBackInPool()
	{
		if (this.isPoolable)
		{
			if (!this._keepParamLocksWhenPuttingBackInPool)
			{
				foreach (PresetManagerControl presetManagerControl in this.presetManagerControls)
				{
					presetManagerControl.lockParams = false;
				}
			}
			this.isPreparingToPutBackInPool = true;
			this.Reset(false);
			this.isPreparingToPutBackInPool = false;
		}
	}

	// Token: 0x0600592E RID: 22830 RVA: 0x0020CA40 File Offset: 0x0020AE40
	public void Reset(bool resetSimulation = true)
	{
		JSONClass jc = new JSONClass();
		this.loadedName = null;
		this.loadedPhysicalName = null;
		this.loadedAppearanceName = null;
		this.PreRestore(true, true);
		this.RestoreTransform(jc, true);
		this.RestoreParentAtom(jc);
		this.Restore(jc, true, true, true, null, true, false, true, false);
		this.Restore(jc, true, true, true, null, false, false, true, false);
		this.LateRestore(jc, true, true, true, false, true, false);
		this.PostRestore(true, true);
		if (resetSimulation)
		{
			this.ResetPhysics(false, true);
		}
	}

	// Token: 0x0600592F RID: 22831 RVA: 0x0020CAC0 File Offset: 0x0020AEC0
	public void ResetPhysical()
	{
		JSONClass jc = new JSONClass();
		this.loadedName = null;
		this.loadedPhysicalName = null;
		this.loadedAppearanceName = null;
		this.PreRestore(true, false);
		this.RestoreTransform(jc, true);
		this.RestoreParentAtom(jc);
		this.Restore(jc, true, false, false, null, true, false, true, false);
		this.Restore(jc, true, false, false, null, false, false, true, false);
		this.LateRestore(jc, true, false, false, false, true, false);
		this.PostRestore(true, false);
		this.ResetPhysics(false, true);
	}

	// Token: 0x06005930 RID: 22832 RVA: 0x0020CB3C File Offset: 0x0020AF3C
	public void ResetAppearance()
	{
		JSONClass jc = new JSONClass();
		this.loadedName = null;
		this.loadedPhysicalName = null;
		this.loadedAppearanceName = null;
		this.PreRestore(false, true);
		this.Restore(jc, false, true, false, null, true, false, true, false);
		this.Restore(jc, false, true, false, null, false, false, true, false);
		this.LateRestore(jc, false, true, false, false, true, false);
		this.PostRestore(false, true);
	}

	// Token: 0x06005931 RID: 22833 RVA: 0x0020CBA0 File Offset: 0x0020AFA0
	public void SavePresetDialog(bool includePhysical = false, bool includeAppearance = false)
	{
		if (SuperController.singleton != null && SuperController.singleton.fileBrowserUI != null)
		{
			this.saveIncludePhysical = includePhysical;
			this.saveIncludeAppearance = includeAppearance;
			string text = SuperController.singleton.savesDir + this.type;
			if (this.saveIncludePhysical && this.saveIncludeAppearance)
			{
				text += "\\full";
				if (this.lastLoadPresetDir != string.Empty && FileManager.DirectoryExists(this.lastLoadPresetDir, false, false))
				{
					string suggestedBrowserDirectoryFromDirectoryPath = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text, this.lastLoadPresetDir, false);
					if (suggestedBrowserDirectoryFromDirectoryPath != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath, false, false))
					{
						text = suggestedBrowserDirectoryFromDirectoryPath;
					}
				}
				else if (!FileManager.DirectoryExists(text, false, false))
				{
					FileManager.CreateDirectory(text);
				}
			}
			else if (this.saveIncludePhysical)
			{
				text += "\\pose";
				if (this.lastLoadPhysicalDir != string.Empty && FileManager.DirectoryExists(this.lastLoadPhysicalDir, false, false))
				{
					string suggestedBrowserDirectoryFromDirectoryPath2 = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text, this.lastLoadPhysicalDir, false);
					if (suggestedBrowserDirectoryFromDirectoryPath2 != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath2, false, false))
					{
						text = suggestedBrowserDirectoryFromDirectoryPath2;
					}
				}
				else if (!FileManager.DirectoryExists(text, false, false))
				{
					FileManager.CreateDirectory(text);
				}
			}
			else if (this.saveIncludeAppearance)
			{
				text += "\\appearance";
				if (this.lastLoadAppearanceDir != string.Empty && FileManager.DirectoryExists(this.lastLoadAppearanceDir, false, false))
				{
					string suggestedBrowserDirectoryFromDirectoryPath3 = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text, this.lastLoadAppearanceDir, false);
					if (suggestedBrowserDirectoryFromDirectoryPath3 != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath3, false, false))
					{
						text = suggestedBrowserDirectoryFromDirectoryPath3;
					}
				}
				else if (!FileManager.DirectoryExists(text, false, false))
				{
					FileManager.CreateDirectory(text);
				}
			}
			SuperController.singleton.fileBrowserUI.SetShortCuts(null, false);
			SuperController.singleton.fileBrowserUI.keepOpen = false;
			SuperController.singleton.fileBrowserUI.SetTitle("Select Save Preset File");
			SuperController.singleton.fileBrowserUI.defaultPath = text;
			SuperController.singleton.fileBrowserUI.SetTextEntry(true);
			SuperController.singleton.fileBrowserUI.Show(new FileBrowserCallback(this.SavePreset), true);
			if (SuperController.singleton.fileBrowserUI.fileEntryField != null)
			{
				string text2 = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
				SuperController.singleton.fileBrowserUI.fileEntryField.text = text2;
				SuperController.singleton.fileBrowserUI.ActivateFileNameField();
			}
		}
	}

	// Token: 0x06005932 RID: 22834 RVA: 0x0020CE58 File Offset: 0x0020B258
	public void SavePreset(string saveName)
	{
		if (saveName != string.Empty)
		{
			if (this.saveIncludePhysical && this.saveIncludeAppearance)
			{
				this.loadedName = saveName;
				this.lastLoadPresetDir = FileManager.GetDirectoryName(saveName, true);
			}
			else if (this.saveIncludePhysical)
			{
				this.loadedPhysicalName = saveName;
				this.lastLoadPhysicalDir = FileManager.GetDirectoryName(saveName, true);
			}
			else if (this.saveIncludeAppearance)
			{
				this.loadedAppearanceName = saveName;
				this.lastLoadAppearanceDir = FileManager.GetDirectoryName(saveName, true);
			}
			SuperController.singleton.SaveFromAtom(saveName, this, this.saveIncludePhysical, this.saveIncludeAppearance, null, false);
		}
	}

	// Token: 0x06005933 RID: 22835 RVA: 0x0020CF04 File Offset: 0x0020B304
	public void LoadPresetDialog()
	{
		string text = SuperController.singleton.savesDir + this.type + "\\full";
		string text2 = text;
		if (this.lastLoadPresetDir != string.Empty && FileManager.DirectoryExists(this.lastLoadPresetDir, false, false))
		{
			string suggestedBrowserDirectoryFromDirectoryPath = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text2, this.lastLoadPresetDir, true);
			if (suggestedBrowserDirectoryFromDirectoryPath != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath, false, false))
			{
				text2 = suggestedBrowserDirectoryFromDirectoryPath;
			}
		}
		else if (!FileManager.DirectoryExists(text2, false, false))
		{
			FileManager.CreateDirectory(text2);
		}
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(text, true, false, true, true);
		SuperController.singleton.fileBrowserUI.SetShortCuts(shortCutsForDirectory, false);
		SuperController.singleton.fileBrowserUI.keepOpen = false;
		SuperController.singleton.fileBrowserUI.defaultPath = text2;
		SuperController.singleton.fileBrowserUI.SetTitle("Select Preset File");
		SuperController.singleton.fileBrowserUI.SetTextEntry(false);
		SuperController.singleton.fileBrowserUI.Show(new FileBrowserCallback(this.LoadPreset), true);
	}

	// Token: 0x06005934 RID: 22836 RVA: 0x0020D00C File Offset: 0x0020B40C
	public void LoadPreset(string saveName = "savefile")
	{
		if (saveName != string.Empty)
		{
			try
			{
				using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(saveName, true))
				{
					this.lastLoadPresetDir = FileManager.GetDirectoryName(saveName, false);
					FileManager.PushLoadDirFromFilePath(saveName, false);
					string aJSON = fileEntryStreamReader.ReadToEnd();
					JSONNode jsonnode = JSON.Parse(aJSON);
					JSONArray asArray = jsonnode["atoms"].AsArray;
					this.loadedName = saveName;
					JSONClass asObject = asArray[0].AsObject;
					if (asObject != null)
					{
						string a = asObject["type"];
						if (a == this.type)
						{
							this.PreRestore(true, true);
							this.RestoreTransform(asObject, true);
							this.Restore(asObject, true, true, false, asArray, false, false, true, false);
							this.LateRestore(asObject, true, true, false, false, true, false);
							this.PostRestore(true, true);
							if (SuperController.singleton != null)
							{
								if (asObject["id"] != null)
								{
									SuperController.singleton.RenameAtom(this, asObject["id"]);
								}
								this.ResetPhysics(false, true);
							}
						}
					}
				}
			}
			catch (Exception arg)
			{
				SuperController.LogError("Exception during LoadPreset " + arg);
			}
		}
	}

	// Token: 0x06005935 RID: 22837 RVA: 0x0020D190 File Offset: 0x0020B590
	public void LoadPhysicalPresetDialog()
	{
		string text = SuperController.singleton.savesDir + this.type + "\\pose";
		string text2 = text;
		if (this.lastLoadPhysicalDir != string.Empty && FileManager.DirectoryExists(this.lastLoadPhysicalDir, false, false))
		{
			string suggestedBrowserDirectoryFromDirectoryPath = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text2, this.lastLoadPhysicalDir, true);
			if (suggestedBrowserDirectoryFromDirectoryPath != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath, false, false))
			{
				text2 = suggestedBrowserDirectoryFromDirectoryPath;
			}
		}
		else if (!FileManager.DirectoryExists(text2, false, false))
		{
			FileManager.CreateDirectory(text2);
		}
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(text, true, false, true, true);
		SuperController.singleton.fileBrowserUI.SetShortCuts(shortCutsForDirectory, false);
		SuperController.singleton.fileBrowserUI.keepOpen = false;
		SuperController.singleton.fileBrowserUI.defaultPath = text2;
		SuperController.singleton.fileBrowserUI.SetTitle("Select Preset File");
		SuperController.singleton.fileBrowserUI.SetTextEntry(false);
		SuperController.singleton.fileBrowserUI.Show(new FileBrowserCallback(this.LoadPhysicalPreset), true);
	}

	// Token: 0x06005936 RID: 22838 RVA: 0x0020D298 File Offset: 0x0020B698
	public void LoadPhysicalPreset(string saveName = "savefile")
	{
		if (saveName != string.Empty)
		{
			try
			{
				using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(saveName, true))
				{
					this.lastLoadPhysicalDir = FileManager.GetDirectoryName(saveName, true);
					FileManager.PushLoadDirFromFilePath(saveName, false);
					string aJSON = fileEntryStreamReader.ReadToEnd();
					JSONNode jsonnode = JSON.Parse(aJSON);
					JSONArray asArray = jsonnode["atoms"].AsArray;
					this.loadedPhysicalName = saveName;
					JSONClass asObject = asArray[0].AsObject;
					if (asObject != null)
					{
						string a = asObject["type"];
						if (a == this.type)
						{
							this.PreRestore(true, false);
							this.RestoreTransform(asObject, true);
							this.Restore(asObject, true, false, false, asArray, false, false, true, false);
							this.LateRestore(asObject, true, false, false, false, true, false);
							this.PostRestore(true, false);
							this.ResetPhysics(false, true);
						}
					}
				}
			}
			catch (Exception arg)
			{
				SuperController.LogError("Error during LoadPhysicalPreset " + arg);
			}
		}
	}

	// Token: 0x06005937 RID: 22839 RVA: 0x0020D3C4 File Offset: 0x0020B7C4
	public void LoadAppearancePresetDialog()
	{
		string text = SuperController.singleton.savesDir + this.type + "\\appearance";
		string text2 = text;
		if (this.lastLoadAppearanceDir != string.Empty && FileManager.DirectoryExists(this.lastLoadAppearanceDir, false, false))
		{
			string suggestedBrowserDirectoryFromDirectoryPath = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text2, this.lastLoadAppearanceDir, true);
			if (suggestedBrowserDirectoryFromDirectoryPath != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath, false, false))
			{
				text2 = suggestedBrowserDirectoryFromDirectoryPath;
			}
		}
		else if (!FileManager.DirectoryExists(text2, false, false))
		{
			FileManager.CreateDirectory(text2);
		}
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(text, true, false, true, true);
		SuperController.singleton.fileBrowserUI.SetShortCuts(shortCutsForDirectory, false);
		SuperController.singleton.fileBrowserUI.keepOpen = false;
		SuperController.singleton.fileBrowserUI.defaultPath = text2;
		SuperController.singleton.fileBrowserUI.SetTitle("Select Preset File");
		SuperController.singleton.fileBrowserUI.SetTextEntry(false);
		SuperController.singleton.fileBrowserUI.Show(new FileBrowserCallback(this.LoadAppearancePreset), true);
	}

	// Token: 0x06005938 RID: 22840 RVA: 0x0020D4CC File Offset: 0x0020B8CC
	public void LoadAppearancePreset(string saveName = "savefile")
	{
		if (saveName != string.Empty)
		{
			try
			{
				using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(saveName, true))
				{
					this.lastLoadAppearanceDir = FileManager.GetDirectoryName(saveName, true);
					FileManager.PushLoadDirFromFilePath(saveName, false);
					string aJSON = fileEntryStreamReader.ReadToEnd();
					JSONNode jsonnode = JSON.Parse(aJSON);
					JSONArray asArray = jsonnode["atoms"].AsArray;
					this.loadedAppearanceName = saveName;
					JSONClass asObject = asArray[0].AsObject;
					if (asObject != null)
					{
						string a = asObject["type"];
						if (a == this.type)
						{
							this.PreRestore(false, true);
							this.Restore(asObject, false, true, false, null, false, false, true, false);
							this.LateRestore(asObject, false, true, false, false, true, false);
							this.PostRestore(false, true);
							if (SuperController.singleton != null && asObject["id"] != null)
							{
								SuperController.singleton.RenameAtom(this, asObject["id"]);
							}
						}
					}
				}
			}
			catch (Exception arg)
			{
				SuperController.LogError("Exception during LoadAppearancePreset " + arg);
			}
		}
	}

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x06005939 RID: 22841 RVA: 0x0020D640 File Offset: 0x0020BA40
	public ForceReceiver[] forceReceivers
	{
		get
		{
			return this._forceReceivers;
		}
	}

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x0600593A RID: 22842 RVA: 0x0020D648 File Offset: 0x0020BA48
	public ForceProducerV2[] forceProducers
	{
		get
		{
			return this._forceProducers;
		}
	}

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x0600593B RID: 22843 RVA: 0x0020D650 File Offset: 0x0020BA50
	public GrabPoint[] grabPoints
	{
		get
		{
			return this._grabPoints;
		}
	}

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x0600593C RID: 22844 RVA: 0x0020D658 File Offset: 0x0020BA58
	public ScaleChangeReceiver[] scaleChangeReceivers
	{
		get
		{
			return this._scaleChangeReceivers;
		}
	}

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x0600593D RID: 22845 RVA: 0x0020D660 File Offset: 0x0020BA60
	public ScaleChangeReceiverJSONStorable[] scaleChangeReceiverJSONStorables
	{
		get
		{
			return this._scaleChangeReceiverJSONStorables;
		}
	}

	// Token: 0x17000D27 RID: 3367
	// (get) Token: 0x0600593E RID: 22846 RVA: 0x0020D668 File Offset: 0x0020BA68
	public FreeControllerV3[] freeControllers
	{
		get
		{
			return this._freeControllers;
		}
	}

	// Token: 0x17000D28 RID: 3368
	// (get) Token: 0x0600593F RID: 22847 RVA: 0x0020D670 File Offset: 0x0020BA70
	public Rigidbody[] rigidbodies
	{
		get
		{
			return this._rigidbodies;
		}
	}

	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x06005940 RID: 22848 RVA: 0x0020D678 File Offset: 0x0020BA78
	public Rigidbody[] linkableRigidbodies
	{
		get
		{
			return this._linkableRigidbodies;
		}
	}

	// Token: 0x17000D2A RID: 3370
	// (get) Token: 0x06005941 RID: 22849 RVA: 0x0020D680 File Offset: 0x0020BA80
	public Rigidbody[] realRigidbodies
	{
		get
		{
			return this._realRigidbodies;
		}
	}

	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x06005942 RID: 22850 RVA: 0x0020D688 File Offset: 0x0020BA88
	public AnimationPattern[] animationPatterns
	{
		get
		{
			return this._animationPatterns;
		}
	}

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x06005943 RID: 22851 RVA: 0x0020D690 File Offset: 0x0020BA90
	public AnimationStep[] animationSteps
	{
		get
		{
			return this._animationSteps;
		}
	}

	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x06005944 RID: 22852 RVA: 0x0020D698 File Offset: 0x0020BA98
	public Animator[] animators
	{
		get
		{
			return this._animators;
		}
	}

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06005945 RID: 22853 RVA: 0x0020D6A0 File Offset: 0x0020BAA0
	public MotionAnimationControl[] motionAnimationControls
	{
		get
		{
			return this._motionAnimationsControls;
		}
	}

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06005946 RID: 22854 RVA: 0x0020D6A8 File Offset: 0x0020BAA8
	public PlayerNavCollider[] playerNavColliders
	{
		get
		{
			return this._playerNavColliders;
		}
	}

	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x06005947 RID: 22855 RVA: 0x0020D6B0 File Offset: 0x0020BAB0
	public List<Canvas> canvases
	{
		get
		{
			return this._canvases;
		}
	}

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x06005948 RID: 22856 RVA: 0x0020D6B8 File Offset: 0x0020BAB8
	// (set) Token: 0x06005949 RID: 22857 RVA: 0x0020D6C0 File Offset: 0x0020BAC0
	public List<PresetManagerControl> presetManagerControls
	{
		[CompilerGenerated]
		get
		{
			return this.<presetManagerControls>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<presetManagerControls>k__BackingField = value;
		}
	}

	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x0600594A RID: 22858 RVA: 0x0020D6C9 File Offset: 0x0020BAC9
	public PhysicsSimulator[] physicsSimulators
	{
		get
		{
			return this._physicsSimulators;
		}
	}

	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x0600594B RID: 22859 RVA: 0x0020D6D1 File Offset: 0x0020BAD1
	public PhysicsSimulatorJSONStorable[] physicsSimulatorsStorable
	{
		get
		{
			return this._physicsSimulatorsStorable;
		}
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x0020D6DC File Offset: 0x0020BADC
	public void RegisterDynamicPhysicsSimulator(PhysicsSimulator ps)
	{
		if (this._dynamicPhysicsSimulators == null)
		{
			this._dynamicPhysicsSimulators = new List<PhysicsSimulator>();
		}
		ps.useInterpolation = this._useRigidbodyInterpolation;
		ps.collisionEnabled = this._collisionEnabled;
		ps.freezeSimulation = this._isPhysicsFrozen;
		if (this.waitResumeSimulationFlags != null)
		{
			foreach (AsyncFlag waitFor in this.waitResumeSimulationFlags)
			{
				ps.ResetSimulation(waitFor);
			}
		}
		this._dynamicPhysicsSimulators.Add(ps);
	}

	// Token: 0x0600594D RID: 22861 RVA: 0x0020D78C File Offset: 0x0020BB8C
	public void DeregisterDynamicPhysicsSimulator(PhysicsSimulator ps)
	{
		this._dynamicPhysicsSimulators.Remove(ps);
	}

	// Token: 0x0600594E RID: 22862 RVA: 0x0020D79C File Offset: 0x0020BB9C
	public void RegisterDynamicPhysicsSimulatorJSONStorable(PhysicsSimulatorJSONStorable ps)
	{
		if (this._dynamicPhysicsSimulatorsStorable == null)
		{
			this._dynamicPhysicsSimulatorsStorable = new List<PhysicsSimulatorJSONStorable>();
		}
		ps.useInterpolation = this._useRigidbodyInterpolation;
		ps.collisionEnabled = this._collisionEnabled;
		ps.freezeSimulation = this._isPhysicsFrozen;
		if (this.waitResumeSimulationFlags != null)
		{
			foreach (AsyncFlag waitFor in this.waitResumeSimulationFlags)
			{
				ps.ResetSimulation(waitFor);
			}
		}
		this._dynamicPhysicsSimulatorsStorable.Add(ps);
	}

	// Token: 0x0600594F RID: 22863 RVA: 0x0020D84C File Offset: 0x0020BC4C
	public void DeregisterDynamicPhysicsSimulatorJSONStorable(PhysicsSimulatorJSONStorable ps)
	{
		this._dynamicPhysicsSimulatorsStorable.Remove(ps);
	}

	// Token: 0x06005950 RID: 22864 RVA: 0x0020D85B File Offset: 0x0020BC5B
	public void RegisterDynamicScaleChangeReceiver(ScaleChangeReceiver scr)
	{
		if (this._dynamicScaleChangeReceivers == null)
		{
			this._dynamicScaleChangeReceivers = new List<ScaleChangeReceiver>();
		}
		scr.ScaleChanged(this._currentScale);
		this._dynamicScaleChangeReceivers.Add(scr);
	}

	// Token: 0x06005951 RID: 22865 RVA: 0x0020D88B File Offset: 0x0020BC8B
	public void DeregisterDynamicScaleChangeReceiver(ScaleChangeReceiver scr)
	{
		this._dynamicScaleChangeReceivers.Remove(scr);
	}

	// Token: 0x06005952 RID: 22866 RVA: 0x0020D89A File Offset: 0x0020BC9A
	public void RegisterDynamicScaleChangeReceiverJSONStorable(ScaleChangeReceiverJSONStorable scr)
	{
		if (this._dynamicScaleChangeReceiverJSONStorables == null)
		{
			this._dynamicScaleChangeReceiverJSONStorables = new List<ScaleChangeReceiverJSONStorable>();
		}
		scr.ScaleChanged(this._currentScale);
		this._dynamicScaleChangeReceiverJSONStorables.Add(scr);
	}

	// Token: 0x06005953 RID: 22867 RVA: 0x0020D8CA File Offset: 0x0020BCCA
	public void DeregisterDynamicScaleChangeReceiverJSONStorable(ScaleChangeReceiverJSONStorable scr)
	{
		this._dynamicScaleChangeReceiverJSONStorables.Remove(scr);
	}

	// Token: 0x06005954 RID: 22868 RVA: 0x0020D8DC File Offset: 0x0020BCDC
	protected void SyncRenderSuspend()
	{
		bool flag = (!this.excludeFromTempDisableRender && this._tempDisableRender) || (!this.excludeFromGlobalDisableRender && this._globalDisableRender);
		if (this._renderSuspends != null)
		{
			foreach (RenderSuspend renderSuspend in this._renderSuspends)
			{
				renderSuspend.renderSuspend = flag;
			}
		}
		if (this._dynamicRenderSuspends != null)
		{
			foreach (RenderSuspend renderSuspend2 in this._dynamicRenderSuspends)
			{
				renderSuspend2.renderSuspend = flag;
			}
		}
		if (this._meshRenderers != null)
		{
			foreach (MeshRenderer meshRenderer in this._meshRenderers)
			{
				meshRenderer.enabled = !flag;
			}
		}
		if (this._dynamicMeshRenderers != null)
		{
			foreach (MeshRenderer meshRenderer2 in this._dynamicMeshRenderers)
			{
				meshRenderer2.enabled = !flag;
			}
		}
		if (this._particleSystemRenderers != null)
		{
			foreach (KeyValuePair<ParticleSystemRenderer, ParticleSystemRenderMode> keyValuePair in this._particleSystemRenderers)
			{
				if (flag)
				{
					keyValuePair.Key.renderMode = ParticleSystemRenderMode.None;
				}
				else
				{
					keyValuePair.Key.renderMode = keyValuePair.Value;
				}
			}
		}
	}

	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x06005955 RID: 22869 RVA: 0x0020DAFC File Offset: 0x0020BEFC
	// (set) Token: 0x06005956 RID: 22870 RVA: 0x0020DB04 File Offset: 0x0020BF04
	public bool tempDisableRender
	{
		get
		{
			return this._tempDisableRender;
		}
		set
		{
			if (this._tempDisableRender != value)
			{
				this._tempDisableRender = value;
				this.SyncRenderSuspend();
			}
		}
	}

	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x06005957 RID: 22871 RVA: 0x0020DB1F File Offset: 0x0020BF1F
	// (set) Token: 0x06005958 RID: 22872 RVA: 0x0020DB27 File Offset: 0x0020BF27
	public bool globalDisableRender
	{
		get
		{
			return this._globalDisableRender;
		}
		set
		{
			if (this._globalDisableRender != value)
			{
				this._globalDisableRender = value;
				this.SyncRenderSuspend();
			}
		}
	}

	// Token: 0x06005959 RID: 22873 RVA: 0x0020DB42 File Offset: 0x0020BF42
	public void RegisterDynamicMeshRenderer(MeshRenderer mr)
	{
		if (mr.enabled)
		{
			if (this._dynamicMeshRenderers == null)
			{
				this._dynamicMeshRenderers = new List<MeshRenderer>();
			}
			this._dynamicMeshRenderers.Add(mr);
			this.SyncRenderSuspend();
		}
	}

	// Token: 0x0600595A RID: 22874 RVA: 0x0020DB77 File Offset: 0x0020BF77
	public void DeregisterDynamicMeshRenderer(MeshRenderer mr)
	{
		this._dynamicMeshRenderers.Remove(mr);
	}

	// Token: 0x0600595B RID: 22875 RVA: 0x0020DB86 File Offset: 0x0020BF86
	public void RegisterDynamicRenderSuspend(RenderSuspend rs)
	{
		if (this._dynamicRenderSuspends == null)
		{
			this._dynamicRenderSuspends = new List<RenderSuspend>();
		}
		this._dynamicRenderSuspends.Add(rs);
		this.SyncRenderSuspend();
	}

	// Token: 0x0600595C RID: 22876 RVA: 0x0020DBB0 File Offset: 0x0020BFB0
	public void DeregisterDynamicRenderSuspend(RenderSuspend rs)
	{
		rs.renderSuspend = false;
		this._dynamicRenderSuspends.Remove(rs);
	}

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x0600595D RID: 22877 RVA: 0x0020DBC6 File Offset: 0x0020BFC6
	public AutoColliderBatchUpdater[] autoColliderBatchUpdaters
	{
		get
		{
			return this._autoColliderBatchUpdaters;
		}
	}

	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x0600595E RID: 22878 RVA: 0x0020DBCE File Offset: 0x0020BFCE
	public RhythmController[] rhythmControllers
	{
		get
		{
			return this._rhythmControllers;
		}
	}

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x0600595F RID: 22879 RVA: 0x0020DBD6 File Offset: 0x0020BFD6
	public AudioSourceControl[] audioSourceControls
	{
		get
		{
			return this._audioSourceControls;
		}
	}

	// Token: 0x06005960 RID: 22880 RVA: 0x0020DBE0 File Offset: 0x0020BFE0
	public void SetParentAtomSelectPopupValues()
	{
		if (this.parentAtomSelectionPopup != null && SuperController.singleton != null)
		{
			List<string> visibleAtomUIDs = SuperController.singleton.GetVisibleAtomUIDs();
			if (visibleAtomUIDs == null)
			{
				this.parentAtomSelectionPopup.numPopupValues = 1;
				this.parentAtomSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.parentAtomSelectionPopup.numPopupValues = visibleAtomUIDs.Count + 1;
				this.parentAtomSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < visibleAtomUIDs.Count; i++)
				{
					this.parentAtomSelectionPopup.setPopupValue(i + 1, visibleAtomUIDs[i]);
				}
			}
		}
	}

	// Token: 0x06005961 RID: 22881 RVA: 0x0020DC94 File Offset: 0x0020C094
	public bool RegisterAdditionalStorable(JSONStorable js)
	{
		if (js != null)
		{
			if (!this._storableById.ContainsKey(js.storeId))
			{
				js.containingAtom = this;
				this._storables.Add(js);
				this._storableById.Add(js.storeId, js);
				return true;
			}
			UnityEngine.Debug.LogError("Found duplicate storable uid " + js.storeId + " in atom " + this.uid);
		}
		return false;
	}

	// Token: 0x06005962 RID: 22882 RVA: 0x0020DD10 File Offset: 0x0020C110
	public void AddCanvas(Canvas c)
	{
		this._canvases.Add(c);
		if (SuperController.singleton != null)
		{
			SuperController.singleton.AddCanvas(c);
		}
	}

	// Token: 0x06005963 RID: 22883 RVA: 0x0020DD39 File Offset: 0x0020C139
	public void RemoveCanvas(Canvas c)
	{
		this._canvases.Remove(c);
		if (SuperController.singleton != null)
		{
			SuperController.singleton.RemoveCanvas(c);
		}
	}

	// Token: 0x06005964 RID: 22884 RVA: 0x0020DD64 File Offset: 0x0020C164
	public void UnregisterAdditionalStorable(JSONStorable js)
	{
		if (js != null)
		{
			if (this._storableById.ContainsKey(js.storeId))
			{
				this._storableById.Remove(js.storeId);
			}
			js.containingAtom = null;
			this._storables.Remove(js);
		}
	}

	// Token: 0x06005965 RID: 22885 RVA: 0x0020DDBC File Offset: 0x0020C1BC
	public void SetParentAtom(string atomUID)
	{
		if (SuperController.singleton != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(atomUID);
			this.SetParentAtom(atomByUid);
		}
	}

	// Token: 0x06005966 RID: 22886 RVA: 0x0020DDEC File Offset: 0x0020C1EC
	public void SetParentAtom(Atom a)
	{
		if (a == this)
		{
			this.SelectAtomParent(this.parentAtom);
		}
		else
		{
			this.parentAtom = a;
		}
	}

	// Token: 0x06005967 RID: 22887 RVA: 0x0020DE14 File Offset: 0x0020C214
	protected void WalkAndRecalculateSubScenePath(Atom atom)
	{
		atom.RecalculateSubScenePath();
		foreach (Atom atom2 in atom.GetChildren())
		{
			this.WalkAndRecalculateSubScenePath(atom2);
		}
	}

	// Token: 0x06005968 RID: 22888 RVA: 0x0020DE78 File Offset: 0x0020C278
	protected void OnAtomRenamed(string oldid, string newid)
	{
		if (this.parentAtom != null && this.parentAtomSelectionPopup != null)
		{
			this.parentAtomSelectionPopup.currentValueNoCallback = this.parentAtom.uid;
		}
		if (newid == this.uid && this.isSubSceneType)
		{
			this.WalkAndRecalculateSubScenePath(this);
		}
	}

	// Token: 0x06005969 RID: 22889 RVA: 0x0020DEE0 File Offset: 0x0020C2E0
	protected void OnAtomSubSceneChanged(Atom atom, SubScene subScene)
	{
		if (atom == this)
		{
			this.containingSubScene = subScene;
			this.RecalculateSubScenePath();
		}
	}

	// Token: 0x0600596A RID: 22890 RVA: 0x0020DEFC File Offset: 0x0020C2FC
	public void SelectAtomParent(Atom a)
	{
		if (a == this)
		{
			a = this.parentAtom;
		}
		if (this.parentAtomSelectionPopup != null)
		{
			if (a == null)
			{
				this.parentAtomSelectionPopup.currentValue = "None";
			}
			else
			{
				this.parentAtomSelectionPopup.currentValue = a.uid;
			}
		}
		this.parentAtom = a;
	}

	// Token: 0x0600596B RID: 22891 RVA: 0x0020DF67 File Offset: 0x0020C367
	public void SelectAtomParentFromScene()
	{
		this.SetParentAtomSelectPopupValues();
		SuperController.singleton.SelectModeAtom(new SuperController.SelectAtomCallback(this.SelectAtomParent));
	}

	// Token: 0x0600596C RID: 22892 RVA: 0x0020DF88 File Offset: 0x0020C388
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			AtomUI componentInChildren = this.UITransform.GetComponentInChildren<AtomUI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggle = componentInChildren.onToggle;
				this.hiddenJSON.toggle = componentInChildren.hiddenToggle;
				this.keepParamLocksWhenPuttingBackInPoolJSON.toggle = componentInChildren.keepParamLocksWhenPuttingBackInPoolToggle;
				this.collisionEnabledJSON.toggle = componentInChildren.collisionEnabledToggle;
				this.freezePhysicsJSON.toggle = componentInChildren.freezePhysicsToggle;
				this.ResetPhysicsJSONAction.button = componentInChildren.resetPhysicsButton;
				this.resetPhysicsProgressJSON.slider = componentInChildren.resetPhysicsProgressSlider;
				Canvas componentInChildren2 = this.UITransform.GetComponentInChildren<Canvas>();
				if (componentInChildren2 != null)
				{
					RectTransform component = componentInChildren2.GetComponent<RectTransform>();
					if (component != null)
					{
						UIDynamicButton uidynamicButton = SuperController.singleton.CreateDynamicButton(component);
						if (uidynamicButton != null)
						{
							RectTransform component2 = uidynamicButton.GetComponent<RectTransform>();
							component2.anchorMin = new Vector2(0.5f, 1f);
							component2.anchorMax = new Vector2(1f, 1f);
							component2.anchoredPosition = new Vector2(5f, 45f);
							component2.sizeDelta = new Vector2(-10f, 70f);
							uidynamicButton.label = "Select Containing SubScene";
							this.SelectContainingSubSceneAction.dynamicButton = uidynamicButton;
							this.SyncSelectContainingSubSceneButton();
						}
						if (!this.isSubSceneType)
						{
							uidynamicButton = SuperController.singleton.CreateDynamicButton(component);
							if (uidynamicButton != null)
							{
								RectTransform component3 = uidynamicButton.GetComponent<RectTransform>();
								component3.anchorMin = new Vector2(0f, 1f);
								component3.anchorMax = new Vector2(0.5f, 1f);
								component3.anchoredPosition = new Vector2(-5f, 45f);
								component3.sizeDelta = new Vector2(-10f, 70f);
								uidynamicButton.label = "Isolate Edit This Atom";
								this.IsolateEditAtomAction.dynamicButton = uidynamicButton;
							}
						}
					}
				}
				this.parentAtomSelectionPopup = componentInChildren.parentAtomSelectionPopup;
				if (this.parentAtomSelectionPopup != null)
				{
					this.parentAtomSelectionPopup.numPopupValues = 1;
					this.parentAtomSelectionPopup.setPopupValue(0, "None");
					if (this.parentAtom != null)
					{
						this.parentAtomSelectionPopup.currentValue = this.parentAtom.uid;
					}
					else
					{
						this.parentAtomSelectionPopup.currentValue = "None";
					}
					UIPopup uipopup = this.parentAtomSelectionPopup;
					uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetParentAtomSelectPopupValues));
					UIPopup uipopup2 = this.parentAtomSelectionPopup;
					uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetParentAtom));
				}
				if (componentInChildren.selectAtomParentFromSceneButton != null)
				{
					componentInChildren.selectAtomParentFromSceneButton.onClick.AddListener(new UnityAction(this.SelectAtomParentFromScene));
				}
				if (componentInChildren.resetButton != null)
				{
					componentInChildren.resetButton.onClick.AddListener(new UnityAction(this.<InitUI>m__0));
				}
				if (componentInChildren.resetPhysicalButton != null)
				{
					componentInChildren.resetPhysicalButton.onClick.AddListener(new UnityAction(this.<InitUI>m__1));
				}
				if (componentInChildren.resetAppearanceButton != null)
				{
					componentInChildren.resetAppearanceButton.onClick.AddListener(new UnityAction(this.<InitUI>m__2));
				}
				if (componentInChildren.removeButton != null)
				{
					componentInChildren.removeButton.onClick.AddListener(new UnityAction(this.<InitUI>m__3));
				}
				if (componentInChildren.savePresetButton != null)
				{
					componentInChildren.savePresetButton.onClick.AddListener(new UnityAction(this.<InitUI>m__4));
				}
				if (componentInChildren.saveAppearancePresetButton != null)
				{
					componentInChildren.saveAppearancePresetButton.onClick.AddListener(new UnityAction(this.<InitUI>m__5));
				}
				if (componentInChildren.savePhysicalPresetButton != null)
				{
					componentInChildren.savePhysicalPresetButton.onClick.AddListener(new UnityAction(this.<InitUI>m__6));
				}
				if (componentInChildren.loadPresetButton != null)
				{
					componentInChildren.loadPresetButton.onClick.AddListener(new UnityAction(this.<InitUI>m__7));
				}
				if (componentInChildren.loadAppearancePresetButton != null)
				{
					componentInChildren.loadAppearancePresetButton.onClick.AddListener(new UnityAction(this.<InitUI>m__8));
				}
				if (componentInChildren.loadPhysicalPresetButton != null)
				{
					componentInChildren.loadPhysicalPresetButton.onClick.AddListener(new UnityAction(this.<InitUI>m__9));
				}
				this.idText = componentInChildren.idText;
				if (this.idText != null)
				{
					this.idText.onEndEdit.AddListener(new UnityAction<string>(this.SetUID));
				}
				this.idTextAction = componentInChildren.idTextAction;
				if (this.idTextAction != null)
				{
					InputFieldAction inputFieldAction = this.idTextAction;
					inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetUIDToInputField));
				}
				this.descriptionText = componentInChildren.descriptionText;
				this.SyncIdText();
				this.SyncDescriptionText();
			}
		}
	}

	// Token: 0x0600596D RID: 22893 RVA: 0x0020E4D8 File Offset: 0x0020C8D8
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			AtomUI componentInChildren = this.UITransformAlt.GetComponentInChildren<AtomUI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggleAlt = componentInChildren.onToggle;
				this.hiddenJSON.toggleAlt = componentInChildren.hiddenToggle;
				this.collisionEnabledJSON.toggleAlt = componentInChildren.collisionEnabledToggle;
				this.freezePhysicsJSON.toggleAlt = componentInChildren.freezePhysicsToggle;
				this.ResetPhysicsJSONAction.buttonAlt = componentInChildren.resetPhysicsButton;
				this.resetPhysicsProgressJSON.sliderAlt = componentInChildren.resetPhysicsProgressSlider;
				this.SelectContainingSubSceneAction.buttonAlt = componentInChildren.selectAtomParentFromSceneButton;
				if (componentInChildren.selectAtomParentFromSceneButton != null)
				{
					componentInChildren.selectAtomParentFromSceneButton.onClick.AddListener(new UnityAction(this.SelectAtomParentFromScene));
				}
				if (componentInChildren.resetButton != null)
				{
					componentInChildren.resetButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__A));
				}
				if (componentInChildren.resetPhysicalButton != null)
				{
					componentInChildren.resetPhysicalButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__B));
				}
				if (componentInChildren.resetAppearanceButton != null)
				{
					componentInChildren.resetAppearanceButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__C));
				}
				if (componentInChildren.removeButton != null)
				{
					componentInChildren.removeButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__D));
				}
				if (componentInChildren.savePresetButton != null)
				{
					componentInChildren.savePresetButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__E));
				}
				if (componentInChildren.saveAppearancePresetButton != null)
				{
					componentInChildren.saveAppearancePresetButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__F));
				}
				if (componentInChildren.savePhysicalPresetButton != null)
				{
					componentInChildren.savePhysicalPresetButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__10));
				}
				if (componentInChildren.loadPresetButton != null)
				{
					componentInChildren.loadPresetButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__11));
				}
				if (componentInChildren.loadAppearancePresetButton != null)
				{
					componentInChildren.loadAppearancePresetButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__12));
				}
				if (componentInChildren.loadPhysicalPresetButton != null)
				{
					componentInChildren.loadPhysicalPresetButton.onClick.AddListener(new UnityAction(this.<InitUIAlt>m__13));
				}
				this.idTextAlt = componentInChildren.idText;
				if (this.idTextAlt != null)
				{
					this.idTextAlt.onEndEdit.AddListener(new UnityAction<string>(this.SetUID));
				}
				this.idTextActionAlt = componentInChildren.idTextAction;
				if (this.idTextActionAlt != null)
				{
					InputFieldAction inputFieldAction = this.idTextActionAlt;
					inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetUIDToInputField));
				}
				this.descriptionTextAlt = componentInChildren.descriptionText;
				this.SyncIdText();
				this.SyncDescriptionText();
			}
		}
	}

	// Token: 0x0600596E RID: 22894 RVA: 0x0020E80A File Offset: 0x0020CC0A
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

	// Token: 0x0600596F RID: 22895 RVA: 0x0020E830 File Offset: 0x0020CC30
	private void Start()
	{
		if (this.reParentObject != null)
		{
			this.reParentObjectStartingPosition = this.reParentObject.position;
			this.reParentObjectStartingRotation = this.reParentObject.rotation;
		}
		if (this.childAtomContainer != null)
		{
			this.childAtomContainerStartingPosition = this.childAtomContainer.position;
			this.childAtomContainerStartingRotation = this.childAtomContainer.rotation;
		}
		this.SyncMasterControllerCorners();
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x0020E8AC File Offset: 0x0020CCAC
	private void Update()
	{
		if (Application.isPlaying)
		{
			this.CheckResumeSimulation();
		}
		if (this._masterController != null && this.masterControllerCorners != null && this.masterControllerCorners.Length >= 8)
		{
			if (this._freeControllers.Length > 1 || (this.alwaysShowExtents && this._freeControllers.Length > 0))
			{
				Vector3 position = this._freeControllers[0].transform.position;
				this.extentLowX = position.x;
				this.extentHighX = position.x;
				this.extentLowY = position.y;
				this.extentHighY = position.y;
				this.extentLowZ = position.z;
				this.extentHighZ = position.z;
				foreach (FreeControllerV3 freeControllerV in this._freeControllers)
				{
					if (freeControllerV != this._masterController)
					{
						position = freeControllerV.transform.position;
						if (position.x > this.extentHighX)
						{
							this.extentHighX = position.x;
						}
						else if (position.x < this.extentLowX)
						{
							this.extentLowX = position.x;
						}
						if (position.y > this.extentHighY)
						{
							this.extentHighY = position.y;
						}
						else if (position.y < this.extentLowY)
						{
							this.extentLowY = position.y;
						}
						if (position.z > this.extentHighZ)
						{
							this.extentHighZ = position.z;
						}
						else if (position.z < this.extentLowZ)
						{
							this.extentLowZ = position.z;
						}
					}
				}
				this.extentLowX -= this.extentPadding;
				this.extentLowY -= this.extentPadding;
				this.extentLowZ -= this.extentPadding;
				this.extentHighX += this.extentPadding;
				this.extentHighY += this.extentPadding;
				this.extentHighZ += this.extentPadding;
				this.extentlll.x = this.extentLowX;
				this.extentlll.y = this.extentLowY;
				this.extentlll.z = this.extentLowZ;
				this.extentllh.x = this.extentLowX;
				this.extentllh.y = this.extentLowY;
				this.extentllh.z = this.extentHighZ;
				this.extentlhl.x = this.extentLowX;
				this.extentlhl.y = this.extentHighY;
				this.extentlhl.z = this.extentLowZ;
				this.extentlhh.x = this.extentLowX;
				this.extentlhh.y = this.extentHighY;
				this.extentlhh.z = this.extentHighZ;
				this.extenthll.x = this.extentHighX;
				this.extenthll.y = this.extentLowY;
				this.extenthll.z = this.extentLowZ;
				this.extenthlh.x = this.extentHighX;
				this.extenthlh.y = this.extentLowY;
				this.extenthlh.z = this.extentHighZ;
				this.extenthhl.x = this.extentHighX;
				this.extenthhl.y = this.extentHighY;
				this.extenthhl.z = this.extentLowZ;
				this.extenthhh.x = this.extentHighX;
				this.extenthhh.y = this.extentHighY;
				this.extenthhh.z = this.extentHighZ;
				this.masterControllerCorners[0].position = this.extentlll;
				this.masterControllerCorners[1].position = this.extentllh;
				this.masterControllerCorners[2].position = this.extentlhl;
				this.masterControllerCorners[3].position = this.extentlhh;
				this.masterControllerCorners[4].position = this.extenthll;
				this.masterControllerCorners[5].position = this.extenthlh;
				this.masterControllerCorners[6].position = this.extenthhl;
				this.masterControllerCorners[7].position = this.extenthhh;
				foreach (Transform transform in this.masterControllerCorners)
				{
					transform.gameObject.SetActive(true);
				}
			}
			else
			{
				foreach (Transform transform2 in this.masterControllerCorners)
				{
					transform2.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x0020ED91 File Offset: 0x0020D191
	private void OnEnable()
	{
		this.ClearResetPhysics();
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x0020ED99 File Offset: 0x0020D199
	private void OnDisable()
	{
		this.ClearResetPhysics();
		this.ClearPauseAutoSimulationFlag();
		this.grabFreezePhysics = false;
		this.tempOff = false;
		this.tempHidden = false;
		this.tempFreezePhysics = false;
		this.tempDisableRender = false;
		this.tempDisableCollision = false;
		this.globalDisableRender = false;
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x0020EDD8 File Offset: 0x0020D1D8
	private void OnDestroy()
	{
		if (this._callbackRegistered && SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRenamed));
			SuperController singleton2 = SuperController.singleton;
			singleton2.onAtomSubSceneChangedHandlers = (SuperController.OnAtomSubSceneChanged)Delegate.Remove(singleton2.onAtomSubSceneChangedHandlers, new SuperController.OnAtomSubSceneChanged(this.OnAtomSubSceneChanged));
		}
	}

	// Token: 0x06005974 RID: 22900 RVA: 0x0020EE4C File Offset: 0x0020D24C
	[CompilerGenerated]
	private void <InitUI>m__0()
	{
		this.Reset(true);
	}

	// Token: 0x06005975 RID: 22901 RVA: 0x0020EE55 File Offset: 0x0020D255
	[CompilerGenerated]
	private void <InitUI>m__1()
	{
		this.ResetPhysical();
	}

	// Token: 0x06005976 RID: 22902 RVA: 0x0020EE5D File Offset: 0x0020D25D
	[CompilerGenerated]
	private void <InitUI>m__2()
	{
		this.ResetAppearance();
	}

	// Token: 0x06005977 RID: 22903 RVA: 0x0020EE65 File Offset: 0x0020D265
	[CompilerGenerated]
	private void <InitUI>m__3()
	{
		this.Remove();
	}

	// Token: 0x06005978 RID: 22904 RVA: 0x0020EE6D File Offset: 0x0020D26D
	[CompilerGenerated]
	private void <InitUI>m__4()
	{
		this.SavePresetDialog(true, true);
	}

	// Token: 0x06005979 RID: 22905 RVA: 0x0020EE77 File Offset: 0x0020D277
	[CompilerGenerated]
	private void <InitUI>m__5()
	{
		this.SavePresetDialog(false, true);
	}

	// Token: 0x0600597A RID: 22906 RVA: 0x0020EE81 File Offset: 0x0020D281
	[CompilerGenerated]
	private void <InitUI>m__6()
	{
		this.SavePresetDialog(true, false);
	}

	// Token: 0x0600597B RID: 22907 RVA: 0x0020EE8B File Offset: 0x0020D28B
	[CompilerGenerated]
	private void <InitUI>m__7()
	{
		this.LoadPresetDialog();
	}

	// Token: 0x0600597C RID: 22908 RVA: 0x0020EE93 File Offset: 0x0020D293
	[CompilerGenerated]
	private void <InitUI>m__8()
	{
		this.LoadAppearancePresetDialog();
	}

	// Token: 0x0600597D RID: 22909 RVA: 0x0020EE9B File Offset: 0x0020D29B
	[CompilerGenerated]
	private void <InitUI>m__9()
	{
		this.LoadPhysicalPresetDialog();
	}

	// Token: 0x0600597E RID: 22910 RVA: 0x0020EEA3 File Offset: 0x0020D2A3
	[CompilerGenerated]
	private void <InitUIAlt>m__A()
	{
		this.Reset(true);
	}

	// Token: 0x0600597F RID: 22911 RVA: 0x0020EEAC File Offset: 0x0020D2AC
	[CompilerGenerated]
	private void <InitUIAlt>m__B()
	{
		this.ResetPhysical();
	}

	// Token: 0x06005980 RID: 22912 RVA: 0x0020EEB4 File Offset: 0x0020D2B4
	[CompilerGenerated]
	private void <InitUIAlt>m__C()
	{
		this.ResetAppearance();
	}

	// Token: 0x06005981 RID: 22913 RVA: 0x0020EEBC File Offset: 0x0020D2BC
	[CompilerGenerated]
	private void <InitUIAlt>m__D()
	{
		this.Remove();
	}

	// Token: 0x06005982 RID: 22914 RVA: 0x0020EEC4 File Offset: 0x0020D2C4
	[CompilerGenerated]
	private void <InitUIAlt>m__E()
	{
		this.SavePresetDialog(true, true);
	}

	// Token: 0x06005983 RID: 22915 RVA: 0x0020EECE File Offset: 0x0020D2CE
	[CompilerGenerated]
	private void <InitUIAlt>m__F()
	{
		this.SavePresetDialog(false, true);
	}

	// Token: 0x06005984 RID: 22916 RVA: 0x0020EED8 File Offset: 0x0020D2D8
	[CompilerGenerated]
	private void <InitUIAlt>m__10()
	{
		this.SavePresetDialog(true, false);
	}

	// Token: 0x06005985 RID: 22917 RVA: 0x0020EEE2 File Offset: 0x0020D2E2
	[CompilerGenerated]
	private void <InitUIAlt>m__11()
	{
		this.LoadPresetDialog();
	}

	// Token: 0x06005986 RID: 22918 RVA: 0x0020EEEA File Offset: 0x0020D2EA
	[CompilerGenerated]
	private void <InitUIAlt>m__12()
	{
		this.LoadAppearancePresetDialog();
	}

	// Token: 0x06005987 RID: 22919 RVA: 0x0020EEF2 File Offset: 0x0020D2F2
	[CompilerGenerated]
	private void <InitUIAlt>m__13()
	{
		this.LoadPhysicalPresetDialog();
	}

	// Token: 0x0400490D RID: 18701
	public string category;

	// Token: 0x0400490E RID: 18702
	public string type;

	// Token: 0x0400490F RID: 18703
	public bool isSubSceneType;

	// Token: 0x04004910 RID: 18704
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private SubScene <subSceneComponent>k__BackingField;

	// Token: 0x04004911 RID: 18705
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <isSubSceneRestore>k__BackingField;

	// Token: 0x04004912 RID: 18706
	protected SubScene _containingSubScene;

	// Token: 0x04004913 RID: 18707
	protected JSONStorableAction SelectContainingSubSceneAction;

	// Token: 0x04004914 RID: 18708
	[NonSerialized]
	public bool destroyed;

	// Token: 0x04004915 RID: 18709
	[NonSerialized]
	public bool loadedFromBundle;

	// Token: 0x04004916 RID: 18710
	public bool isPoolable;

	// Token: 0x04004917 RID: 18711
	[NonSerialized]
	public bool inPool;

	// Token: 0x04004918 RID: 18712
	public Transform[] onToggleObjects;

	// Token: 0x04004919 RID: 18713
	[SerializeField]
	protected bool _useRigidbodyInterpolation = true;

	// Token: 0x0400491A RID: 18714
	protected List<AsyncFlag> waitResumeSimulationFlags;

	// Token: 0x0400491B RID: 18715
	protected bool _resetSimulation;

	// Token: 0x0400491C RID: 18716
	protected AsyncFlag resetPhysicsFlag;

	// Token: 0x0400491D RID: 18717
	protected AsyncFlag pauseAutoSimulationFlag;

	// Token: 0x0400491E RID: 18718
	protected bool _isResettingPhysics;

	// Token: 0x0400491F RID: 18719
	protected bool _isResettingPhysicsFull;

	// Token: 0x04004920 RID: 18720
	protected Coroutine resetRoutine;

	// Token: 0x04004921 RID: 18721
	protected JSONStorableAction ResetPhysicsJSONAction;

	// Token: 0x04004922 RID: 18722
	protected JSONStorableFloat resetPhysicsProgressJSON;

	// Token: 0x04004923 RID: 18723
	protected bool _insideSyncOnToggleObjects;

	// Token: 0x04004924 RID: 18724
	protected JSONStorableAction toggleOnJSON;

	// Token: 0x04004925 RID: 18725
	protected JSONStorableBool onJSON;

	// Token: 0x04004926 RID: 18726
	[SerializeField]
	protected bool _on = true;

	// Token: 0x04004927 RID: 18727
	protected JSONStorableBool hiddenJSON;

	// Token: 0x04004928 RID: 18728
	protected bool _hidden;

	// Token: 0x04004929 RID: 18729
	protected bool _tempHidden;

	// Token: 0x0400492A RID: 18730
	protected bool _tempOff;

	// Token: 0x0400492B RID: 18731
	protected bool _keepParamLocksWhenPuttingBackInPool;

	// Token: 0x0400492C RID: 18732
	public JSONStorableBool keepParamLocksWhenPuttingBackInPoolJSON;

	// Token: 0x0400492D RID: 18733
	protected Dictionary<Rigidbody, RigidbodyConstraints> saveRigidbodyContraints;

	// Token: 0x0400492E RID: 18734
	public bool excludeFromTempFreezePhysics;

	// Token: 0x0400492F RID: 18735
	protected bool _tempFreezePhysics;

	// Token: 0x04004930 RID: 18736
	protected AsyncFlag grabFreezePhysicsFlag;

	// Token: 0x04004931 RID: 18737
	protected bool _grabFreezePhysics;

	// Token: 0x04004932 RID: 18738
	public bool excludeFromTempDisableCollision;

	// Token: 0x04004933 RID: 18739
	protected bool _tempDisableCollision;

	// Token: 0x04004934 RID: 18740
	protected bool _resetPhysicsDisableCollision;

	// Token: 0x04004935 RID: 18741
	protected float _currentScale = 1f;

	// Token: 0x04004936 RID: 18742
	public JSONStorableBool collisionEnabledJSON;

	// Token: 0x04004937 RID: 18743
	[SerializeField]
	protected bool _collisionEnabled = true;

	// Token: 0x04004938 RID: 18744
	protected bool _isPhysicsFrozen;

	// Token: 0x04004939 RID: 18745
	protected bool _freezePhysics;

	// Token: 0x0400493A RID: 18746
	public JSONStorableBool freezePhysicsJSON;

	// Token: 0x0400493B RID: 18747
	protected JSONStorableAction IsolateEditAtomAction;

	// Token: 0x0400493C RID: 18748
	[SerializeField]
	private Atom _parentAtom;

	// Token: 0x0400493D RID: 18749
	private HashSet<Atom> childAtoms;

	// Token: 0x0400493E RID: 18750
	public UIPopup parentAtomSelectionPopup;

	// Token: 0x0400493F RID: 18751
	public Transform reParentObject;

	// Token: 0x04004940 RID: 18752
	public bool doNotZeroReParentObject;

	// Token: 0x04004941 RID: 18753
	public Transform childAtomContainer;

	// Token: 0x04004942 RID: 18754
	protected string _subScenePath = string.Empty;

	// Token: 0x04004943 RID: 18755
	public InputField idText;

	// Token: 0x04004944 RID: 18756
	public InputFieldAction idTextAction;

	// Token: 0x04004945 RID: 18757
	public InputField idTextAlt;

	// Token: 0x04004946 RID: 18758
	public InputFieldAction idTextActionAlt;

	// Token: 0x04004947 RID: 18759
	[SerializeField]
	private string _uid;

	// Token: 0x04004948 RID: 18760
	public Text descriptionText;

	// Token: 0x04004949 RID: 18761
	public Text descriptionTextAlt;

	// Token: 0x0400494A RID: 18762
	[SerializeField]
	private string _description;

	// Token: 0x0400494B RID: 18763
	private Transform[] masterControllerCorners;

	// Token: 0x0400494C RID: 18764
	[SerializeField]
	private FreeControllerV3 _masterController;

	// Token: 0x0400494D RID: 18765
	public FreeControllerV3 mainController;

	// Token: 0x0400494E RID: 18766
	public PresetManagerControl mainPresetControl;

	// Token: 0x0400494F RID: 18767
	public float extentPadding = 0.3f;

	// Token: 0x04004950 RID: 18768
	public bool alwaysShowExtents = true;

	// Token: 0x04004951 RID: 18769
	private float extentLowX;

	// Token: 0x04004952 RID: 18770
	private float extentHighX;

	// Token: 0x04004953 RID: 18771
	private float extentLowY;

	// Token: 0x04004954 RID: 18772
	private float extentHighY;

	// Token: 0x04004955 RID: 18773
	private float extentLowZ;

	// Token: 0x04004956 RID: 18774
	private float extentHighZ;

	// Token: 0x04004957 RID: 18775
	private Vector3 extentlll;

	// Token: 0x04004958 RID: 18776
	private Vector3 extentllh;

	// Token: 0x04004959 RID: 18777
	private Vector3 extentlhl;

	// Token: 0x0400495A RID: 18778
	private Vector3 extentlhh;

	// Token: 0x0400495B RID: 18779
	private Vector3 extenthll;

	// Token: 0x0400495C RID: 18780
	private Vector3 extenthlh;

	// Token: 0x0400495D RID: 18781
	private Vector3 extenthhl;

	// Token: 0x0400495E RID: 18782
	private Vector3 extenthhh;

	// Token: 0x0400495F RID: 18783
	private bool _wasInit;

	// Token: 0x04004960 RID: 18784
	private bool _callbackRegistered;

	// Token: 0x04004961 RID: 18785
	private Vector3 reParentObjectStartingPosition;

	// Token: 0x04004962 RID: 18786
	private Quaternion reParentObjectStartingRotation;

	// Token: 0x04004963 RID: 18787
	private Vector3 childAtomContainerStartingPosition;

	// Token: 0x04004964 RID: 18788
	private Quaternion childAtomContainerStartingRotation;

	// Token: 0x04004965 RID: 18789
	private List<JSONStorable> _storables;

	// Token: 0x04004966 RID: 18790
	private Dictionary<string, JSONStorable> _storableById;

	// Token: 0x04004967 RID: 18791
	protected JSONClass lastRestoredData;

	// Token: 0x04004968 RID: 18792
	protected bool lastRestorePhysical;

	// Token: 0x04004969 RID: 18793
	protected bool lastRestoreAppearance;

	// Token: 0x0400496A RID: 18794
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <isPreparingToPutBackInPool>k__BackingField;

	// Token: 0x0400496B RID: 18795
	protected bool saveIncludePhysical;

	// Token: 0x0400496C RID: 18796
	protected bool saveIncludeAppearance;

	// Token: 0x0400496D RID: 18797
	protected string loadedName;

	// Token: 0x0400496E RID: 18798
	protected string loadedPhysicalName;

	// Token: 0x0400496F RID: 18799
	protected string loadedAppearanceName;

	// Token: 0x04004970 RID: 18800
	protected string lastLoadPresetDir;

	// Token: 0x04004971 RID: 18801
	protected string lastLoadAppearanceDir;

	// Token: 0x04004972 RID: 18802
	protected string lastLoadPhysicalDir;

	// Token: 0x04004973 RID: 18803
	private ForceReceiver[] _forceReceivers;

	// Token: 0x04004974 RID: 18804
	private ForceProducerV2[] _forceProducers;

	// Token: 0x04004975 RID: 18805
	private GrabPoint[] _grabPoints;

	// Token: 0x04004976 RID: 18806
	private ScaleChangeReceiver[] _scaleChangeReceivers;

	// Token: 0x04004977 RID: 18807
	private ScaleChangeReceiverJSONStorable[] _scaleChangeReceiverJSONStorables;

	// Token: 0x04004978 RID: 18808
	private FreeControllerV3[] _freeControllers;

	// Token: 0x04004979 RID: 18809
	private Rigidbody[] _rigidbodies;

	// Token: 0x0400497A RID: 18810
	private Rigidbody[] _linkableRigidbodies;

	// Token: 0x0400497B RID: 18811
	private Dictionary<Rigidbody, bool> _collisionExemptRigidbodies;

	// Token: 0x0400497C RID: 18812
	private Rigidbody[] _realRigidbodies;

	// Token: 0x0400497D RID: 18813
	private RigidbodyAttributes[] _rigidbodyAttributes;

	// Token: 0x0400497E RID: 18814
	private AnimationPattern[] _animationPatterns;

	// Token: 0x0400497F RID: 18815
	private AnimationStep[] _animationSteps;

	// Token: 0x04004980 RID: 18816
	private Animator[] _animators;

	// Token: 0x04004981 RID: 18817
	private MotionAnimationControl[] _motionAnimationsControls;

	// Token: 0x04004982 RID: 18818
	private PlayerNavCollider[] _playerNavColliders;

	// Token: 0x04004983 RID: 18819
	private List<Canvas> _canvases;

	// Token: 0x04004984 RID: 18820
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private List<PresetManagerControl> <presetManagerControls>k__BackingField;

	// Token: 0x04004985 RID: 18821
	private PhysicsResetter[] _physicsResetters;

	// Token: 0x04004986 RID: 18822
	private PhysicsSimulator[] _physicsSimulators;

	// Token: 0x04004987 RID: 18823
	private PhysicsSimulatorJSONStorable[] _physicsSimulatorsStorable;

	// Token: 0x04004988 RID: 18824
	private List<PhysicsSimulator> _dynamicPhysicsSimulators;

	// Token: 0x04004989 RID: 18825
	private List<PhysicsSimulatorJSONStorable> _dynamicPhysicsSimulatorsStorable;

	// Token: 0x0400498A RID: 18826
	private List<ScaleChangeReceiver> _dynamicScaleChangeReceivers;

	// Token: 0x0400498B RID: 18827
	private List<ScaleChangeReceiverJSONStorable> _dynamicScaleChangeReceiverJSONStorables;

	// Token: 0x0400498C RID: 18828
	public bool excludeFromTempDisableRender;

	// Token: 0x0400498D RID: 18829
	protected bool _tempDisableRender;

	// Token: 0x0400498E RID: 18830
	public bool excludeFromGlobalDisableRender;

	// Token: 0x0400498F RID: 18831
	protected bool _globalDisableRender;

	// Token: 0x04004990 RID: 18832
	private Dictionary<ParticleSystemRenderer, ParticleSystemRenderMode> _particleSystemRenderers;

	// Token: 0x04004991 RID: 18833
	private List<MeshRenderer> _meshRenderers;

	// Token: 0x04004992 RID: 18834
	private List<MeshRenderer> _dynamicMeshRenderers;

	// Token: 0x04004993 RID: 18835
	private List<RenderSuspend> _renderSuspends;

	// Token: 0x04004994 RID: 18836
	private List<RenderSuspend> _dynamicRenderSuspends;

	// Token: 0x04004995 RID: 18837
	private AutoColliderBatchUpdater[] _autoColliderBatchUpdaters;

	// Token: 0x04004996 RID: 18838
	private RhythmController[] _rhythmControllers;

	// Token: 0x04004997 RID: 18839
	private AudioSourceControl[] _audioSourceControls;

	// Token: 0x04004998 RID: 18840
	public bool canBeParented = true;

	// Token: 0x04004999 RID: 18841
	public Button selectAtomParentFromSceneButton;

	// Token: 0x02000FFB RID: 4091
	[CompilerGenerated]
	private sealed class <ResetPhysicsCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600764F RID: 30287 RVA: 0x0020EEFA File Offset: 0x0020D2FA
		[DebuggerHidden]
		public <ResetPhysicsCo>c__Iterator0()
		{
		}

		// Token: 0x06007650 RID: 30288 RVA: 0x0020EF04 File Offset: 0x0020D304
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this._isResettingPhysics = true;
				this._isResettingPhysicsFull = full;
				this.resetPhysicsFlag = new AsyncFlag("Atom Reset Physics");
				base.ResetSimulation(this.resetPhysicsFlag);
				if (!full)
				{
					k = 0;
					goto IL_2A2;
				}
				this.resetPhysicsProgressJSON.val = 0f;
				increment = 0.1f;
				base.resetPhysicsDisableCollision = true;
				base.ClearPauseAutoSimulationFlag();
				this.pauseAutoSimulationFlag = new AsyncFlag("Atom Pause Auto Simulation");
				SuperController.singleton.PauseAutoSimulation(this.pauseAutoSimulationFlag);
				i = 0;
				break;
			case 1U:
				i++;
				break;
			case 2U:
				j++;
				goto IL_25C;
			case 3U:
				k++;
				goto IL_2A2;
			default:
				return false;
			}
			if (i < 5)
			{
				this.resetPhysicsProgressJSON.val += increment;
				if (fullResetControls)
				{
					foreach (FreeControllerV3 freeControllerV in this.containingAtom.freeControllers)
					{
						freeControllerV.SelectLinkToRigidbody(null);
					}
					foreach (FreeControllerV3 freeControllerV2 in this.containingAtom.freeControllers)
					{
						freeControllerV2.ResetControl();
					}
				}
				if (this._physicsResetters != null)
				{
					foreach (PhysicsResetter physicsResetter in this._physicsResetters)
					{
						physicsResetter.ResetPhysics();
					}
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			base.ClearPauseAutoSimulationFlag();
			base.resetPhysicsDisableCollision = false;
			j = 0;
			IL_25C:
			if (j >= 5)
			{
				goto IL_2AE;
			}
			this.resetPhysicsProgressJSON.val += increment;
			this.$current = null;
			if (!this.$disposing)
			{
				this.$PC = 2;
			}
			return true;
			IL_2A2:
			if (k < 5)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
			}
			IL_2AE:
			this.resetPhysicsFlag.Raise();
			this.resetPhysicsFlag = null;
			this.resetRoutine = null;
			this._isResettingPhysics = false;
			this._isResettingPhysicsFull = false;
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06007651 RID: 30289 RVA: 0x0020F209 File Offset: 0x0020D609
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x06007652 RID: 30290 RVA: 0x0020F211 File Offset: 0x0020D611
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007653 RID: 30291 RVA: 0x0020F219 File Offset: 0x0020D619
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007654 RID: 30292 RVA: 0x0020F229 File Offset: 0x0020D629
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A21 RID: 27169
		internal bool full;

		// Token: 0x04006A22 RID: 27170
		internal float <increment>__1;

		// Token: 0x04006A23 RID: 27171
		internal int <i>__2;

		// Token: 0x04006A24 RID: 27172
		internal bool fullResetControls;

		// Token: 0x04006A25 RID: 27173
		internal int <i>__3;

		// Token: 0x04006A26 RID: 27174
		internal int <i>__4;

		// Token: 0x04006A27 RID: 27175
		internal Atom $this;

		// Token: 0x04006A28 RID: 27176
		internal object $current;

		// Token: 0x04006A29 RID: 27177
		internal bool $disposing;

		// Token: 0x04006A2A RID: 27178
		internal int $PC;
	}
}
