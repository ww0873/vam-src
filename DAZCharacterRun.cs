using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using MeshVR;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x02000AAD RID: 2733
public class DAZCharacterRun : PhysicsSimulator, RenderSuspend
{
	// Token: 0x060047AE RID: 18350 RVA: 0x0015D4B0 File Offset: 0x0015B8B0
	public DAZCharacterRun()
	{
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x0015D50C File Offset: 0x0015B90C
	protected override void SyncResetSimulation()
	{
		if (this.setAnchorFromVertexObjects != null)
		{
			foreach (SetAnchorFromVertex setAnchorFromVertex in this.setAnchorFromVertexObjects)
			{
				setAnchorFromVertex.resetSimulation = this._resetSimulation;
			}
		}
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x0015D54F File Offset: 0x0015B94F
	protected override void SyncFreezeSimulation()
	{
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x0015D551 File Offset: 0x0015B951
	protected override void SyncCollisionEnabled()
	{
		if (this._collisionEnabled)
		{
			SuperController.singleton.ResetSimulation(5, "Character Run Resume Collision", true);
		}
	}

	// Token: 0x060047B2 RID: 18354 RVA: 0x0015D570 File Offset: 0x0015B970
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.characterRunTask != null)
		{
			this.characterRunTask.kill = true;
			this.characterRunTask.resetEvent.Set();
			while (this.characterRunTask.thread.IsAlive)
			{
			}
			this.characterRunTask = null;
		}
		if (this.characterRunTask2 != null)
		{
			this.characterRunTask2.kill = true;
			this.characterRunTask2.resetEvent.Set();
			while (this.characterRunTask2.thread.IsAlive)
			{
			}
			this.characterRunTask2 = null;
		}
		if (this.characterRunTask3 != null)
		{
			this.characterRunTask3.kill = true;
			this.characterRunTask3.resetEvent.Set();
			while (this.characterRunTask3.thread.IsAlive)
			{
			}
			this.characterRunTask3 = null;
		}
		if (this.characterRunTask4 != null)
		{
			this.characterRunTask4.kill = true;
			this.characterRunTask4.resetEvent.Set();
			while (this.characterRunTask4.thread.IsAlive)
			{
			}
			this.characterRunTask4 = null;
		}
		if (this.characterRunTask5 != null)
		{
			this.characterRunTask5.kill = true;
			this.characterRunTask5.resetEvent.Set();
			while (this.characterRunTask5.thread.IsAlive)
			{
			}
			this.characterRunTask5 = null;
		}
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x0015D6FC File Offset: 0x0015BAFC
	protected void StartThreads()
	{
		if (!this._threadsRunning)
		{
			this._threadsRunning = true;
			this.characterRunTask = new DAZCharacterRun.DAZCharacterRunTaskInfo();
			this.characterRunTask.name = "characterRunTask";
			this.characterRunTask.resetEvent = new AutoResetEvent(false);
			this.characterRunTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.characterRunTask.thread.Priority = System.Threading.ThreadPriority.AboveNormal;
			this.characterRunTask.taskType = DAZCharacterRun.DAZCharacterRunTaskType.Run;
			this.characterRunTask.thread.Start(this.characterRunTask);
			this.characterRunTask2 = new DAZCharacterRun.DAZCharacterRunTaskInfo();
			this.characterRunTask2.name = "characterRunTask2";
			this.characterRunTask2.resetEvent = new AutoResetEvent(false);
			this.characterRunTask2.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.characterRunTask2.thread.Priority = System.Threading.ThreadPriority.Normal;
			this.characterRunTask2.taskType = DAZCharacterRun.DAZCharacterRunTaskType.SecondaryMergeVerts;
			this.characterRunTask2.thread.Start(this.characterRunTask2);
			this.characterRunTask3 = new DAZCharacterRun.DAZCharacterRunTaskInfo();
			this.characterRunTask3.name = "characterRunTask3";
			this.characterRunTask3.resetEvent = new AutoResetEvent(false);
			this.characterRunTask3.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.characterRunTask3.thread.Priority = System.Threading.ThreadPriority.AboveNormal;
			this.characterRunTask3.taskType = DAZCharacterRun.DAZCharacterRunTaskType.MergeVertsRun2;
			this.characterRunTask3.thread.Start(this.characterRunTask3);
			this.characterRunTask4 = new DAZCharacterRun.DAZCharacterRunTaskInfo();
			this.characterRunTask4.name = "characterRunTask4";
			this.characterRunTask4.resetEvent = new AutoResetEvent(false);
			this.characterRunTask4.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.characterRunTask4.thread.Priority = System.Threading.ThreadPriority.AboveNormal;
			this.characterRunTask4.taskType = DAZCharacterRun.DAZCharacterRunTaskType.MergeVertsRun3;
			this.characterRunTask4.thread.Start(this.characterRunTask4);
			this.characterRunTask5 = new DAZCharacterRun.DAZCharacterRunTaskInfo();
			this.characterRunTask5.name = "characterRunTask5";
			this.characterRunTask5.resetEvent = new AutoResetEvent(false);
			this.characterRunTask5.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.characterRunTask5.thread.Priority = System.Threading.ThreadPriority.AboveNormal;
			this.characterRunTask5.taskType = DAZCharacterRun.DAZCharacterRunTaskType.MergeVertsRun4;
			this.characterRunTask5.thread.Start(this.characterRunTask5);
		}
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x0015D984 File Offset: 0x0015BD84
	protected void MTTask(object info)
	{
		DAZCharacterRun.DAZCharacterRunTaskInfo dazcharacterRunTaskInfo = (DAZCharacterRun.DAZCharacterRunTaskInfo)info;
		while (this._threadsRunning)
		{
			dazcharacterRunTaskInfo.resetEvent.WaitOne(-1, true);
			if (dazcharacterRunTaskInfo.kill)
			{
				break;
			}
			if (dazcharacterRunTaskInfo.taskType == DAZCharacterRun.DAZCharacterRunTaskType.Run)
			{
				this.RunThreaded(false);
			}
			else if (dazcharacterRunTaskInfo.taskType == DAZCharacterRun.DAZCharacterRunTaskType.SecondaryMergeVerts)
			{
				Thread.Sleep(0);
				this.SecondaryMergeVerts();
			}
			else if (dazcharacterRunTaskInfo.taskType == DAZCharacterRun.DAZCharacterRunTaskType.MergeVertsRun2)
			{
				this.MergeRun2();
			}
			else if (dazcharacterRunTaskInfo.taskType == DAZCharacterRun.DAZCharacterRunTaskType.MergeVertsRun3)
			{
				this.MergeRun3();
			}
			else if (dazcharacterRunTaskInfo.taskType == DAZCharacterRun.DAZCharacterRunTaskType.MergeVertsRun4)
			{
				this.MergeRun4();
			}
			dazcharacterRunTaskInfo.working = false;
		}
	}

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x060047B5 RID: 18357 RVA: 0x0015DA45 File Offset: 0x0015BE45
	// (set) Token: 0x060047B6 RID: 18358 RVA: 0x0015DA4D File Offset: 0x0015BE4D
	public bool doSnap
	{
		get
		{
			return this._doSnap;
		}
		set
		{
			this._doSnap = value;
		}
	}

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x060047B7 RID: 18359 RVA: 0x0015DA56 File Offset: 0x0015BE56
	public Vector3[] snappedMorphedUVVertices
	{
		get
		{
			return this._snappedMorphedUVVertices;
		}
	}

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x060047B8 RID: 18360 RVA: 0x0015DA5E File Offset: 0x0015BE5E
	public Vector3[] snappedMorphedUVNormals
	{
		get
		{
			return this._snappedMorphedUVNormals;
		}
	}

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x060047B9 RID: 18361 RVA: 0x0015DA66 File Offset: 0x0015BE66
	public Vector3[] snappedSkinnedVertices
	{
		get
		{
			return this._snappedSkinnedVertices;
		}
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x060047BA RID: 18362 RVA: 0x0015DA6E File Offset: 0x0015BE6E
	public Vector3[] snappedSkinnedNormals
	{
		get
		{
			return this._snappedSkinnedNormals;
		}
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x0015DA76 File Offset: 0x0015BE76
	public void Disconnect()
	{
		this.skin = null;
		this.skinForThread = null;
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x0015DA88 File Offset: 0x0015BE88
	public void Connect(bool genderChange)
	{
		if (Application.isPlaying && base.enabled && this.skin != null && this.morphBank1 != null && this.morphBank2 != null && this.bones != null)
		{
			this.WaitForRunTask();
			this.skinForThread = this.skin;
			if (this.setDazMorphContainer != null)
			{
				this.sdmFromAverageBoneAngleComps = this.setDazMorphContainer.GetComponentsInChildren<SetDAZMorphFromAverageBoneAngle>(true);
				foreach (SetDAZMorphFromAverageBoneAngle setDAZMorphFromAverageBoneAngle in this.sdmFromAverageBoneAngleComps)
				{
					setDAZMorphFromAverageBoneAngle.updateEnabled = false;
				}
				this.sdmFromBoneAngleComps = this.setDazMorphContainer.GetComponentsInChildren<SetDAZMorphFromBoneAngle>(true);
				foreach (SetDAZMorphFromBoneAngle setDAZMorphFromBoneAngle in this.sdmFromBoneAngleComps)
				{
					setDAZMorphFromBoneAngle.updateEnabled = false;
				}
				this.sdmFromDistanceComps = this.setDazMorphContainer.GetComponentsInChildren<SetDAZMorphFromDistance>(true);
				foreach (SetDAZMorphFromDistance setDAZMorphFromDistance in this.sdmFromDistanceComps)
				{
					setDAZMorphFromDistance.updateEnabled = false;
				}
				this.sdmFromLocalDistanceComps = this.setDazMorphContainer.GetComponentsInChildren<SetDAZMorphFromLocalDistance>(true);
				foreach (SetDAZMorphFromLocalDistance setDAZMorphFromLocalDistance in this.sdmFromLocalDistanceComps)
				{
					setDAZMorphFromLocalDistance.updateEnabled = false;
				}
			}
			this.mergedMesh = this.skin.GetComponent<DAZMergedMesh>();
			this.mergedMesh.Init();
			this.mergedMeshMorphedUVVertices = (Vector3[])this.mergedMesh.morphedUVVertices.Clone();
			this.mergedMeshMorphedUVVerticesCopy = (Vector3[])this.mergedMesh.morphedUVVertices.Clone();
			this.mergedMeshMorphedUVVerticesMergedOnly = (Vector3[])this.mergedMesh.morphedUVVertices.Clone();
			this.mergedMeshVisibleMorphedUVVertices = (Vector3[])this.mergedMesh.morphedUVVertices.Clone();
			this.mesh1 = this.mergedMesh.targetMesh;
			this.mesh2 = this.mergedMesh.graftMesh;
			this.mesh3 = this.mergedMesh.graft2Mesh;
			this.morphBank1.Init();
			this.morphBank1.updateEnabled = false;
			if (this.morphBank1OtherGender != null)
			{
				this.morphBank1OtherGender.Init();
				this.morphBank1OtherGender.updateEnabled = false;
			}
			this.morphBank2.Init();
			this.morphBank2.updateEnabled = false;
			if (this.morphBank3 != null)
			{
				this.morphBank3.Init();
				this.morphBank3.updateEnabled = false;
			}
			this.mergedMesh.staticMesh = true;
			this.skin.Init();
			this.skin.skin = false;
			this.skin.draw = false;
			if (this.autoColliderUpdaters != null)
			{
				foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this.autoColliderUpdaters)
				{
					autoColliderBatchUpdater.UpdateAutoColliders();
					autoColliderBatchUpdater.clumpUpdate = false;
				}
			}
			if (this.physicsMeshes != null)
			{
				foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshes)
				{
					dazphysicsMesh.updateEnabled = false;
				}
			}
			if (this.setAnchorFromVertexObjects != null)
			{
				foreach (SetAnchorFromVertex setAnchorFromVertex in this.setAnchorFromVertexObjects)
				{
					setAnchorFromVertex.doUpdate = false;
					setAnchorFromVertex.resetSimulation = this._resetSimulation;
				}
			}
			foreach (DAZBone dazbone in this.bones.dazBones)
			{
				dazbone.enabled = false;
			}
			this.ResetMorphsInternal(true);
			this.PrepRun(false);
			this.RunThreaded(!this._threadsRunning);
			this.FinishRun();
			this.PrepRun(false);
			this.RunThreaded(!this._threadsRunning);
			this.FinishRun();
			if (this.physicsMeshes != null)
			{
				foreach (DAZPhysicsMesh dazphysicsMesh2 in this.physicsMeshes)
				{
					dazphysicsMesh2.ResetSoftJoints();
				}
			}
			if (genderChange)
			{
				this.ResetMorphs();
			}
		}
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x0015DEF4 File Offset: 0x0015C2F4
	protected IEnumerator RefreshWhenHubClosed()
	{
		while (SuperController.singleton.HubOpen || SuperController.singleton.activeUI == SuperController.ActiveUI.PackageDownloader)
		{
			yield return null;
		}
		this.WaitForRunTask();
		float startt = GlobalStopwatch.GetElapsedMilliseconds();
		if (this.characterSelector != null && this.characterSelector.RefreshPackageMorphs() && this.skinForThread != null)
		{
			this.SmoothResetMorphs();
		}
		float stopt = GlobalStopwatch.GetElapsedMilliseconds();
		UnityEngine.Debug.Log("Deferred refresh package morphs took " + (stopt - startt).ToString("F1") + " ms");
		this.refreshCoroutine = null;
		yield break;
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x0015DF10 File Offset: 0x0015C310
	protected void RefreshPackageMorphs()
	{
		if (SuperController.singleton.HubOpen || SuperController.singleton.activeUI == SuperController.ActiveUI.PackageDownloader)
		{
			if (this.refreshCoroutine == null)
			{
				this.refreshCoroutine = SuperController.singleton.StartCoroutine(this.RefreshWhenHubClosed());
			}
		}
		else
		{
			this.WaitForRunTask();
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			if (this.characterSelector != null && this.characterSelector.RefreshPackageMorphs() && this.skinForThread != null)
			{
				this.SmoothResetMorphs();
			}
			float elapsedMilliseconds2 = GlobalStopwatch.GetElapsedMilliseconds();
			UnityEngine.Debug.Log("Refresh package morphs took " + (elapsedMilliseconds2 - elapsedMilliseconds).ToString("F1") + " ms");
		}
	}

	// Token: 0x060047BF RID: 18367 RVA: 0x0015DFD1 File Offset: 0x0015C3D1
	public void RefreshRuntimeMorphs()
	{
		this.WaitForRunTask();
		if (this.characterSelector != null && this.characterSelector.RefreshRuntimeMorphs() && this.skinForThread != null)
		{
			this.SmoothResetMorphs();
		}
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x0015E014 File Offset: 0x0015C414
	protected void ResetMorphsInternal(bool resetBones = true)
	{
		this.mesh1MorphedUVVertices = (Vector3[])this.mesh1.UVVertices.Clone();
		this.mesh2MorphedUVVertices = (Vector3[])this.mesh2.UVVertices.Clone();
		this.mesh1VisibleMorphedUVVertices = (Vector3[])this.mesh1.UVVertices.Clone();
		this.mesh2VisibleMorphedUVVertices = (Vector3[])this.mesh2.UVVertices.Clone();
		if (this.mesh3 != null)
		{
			this.mesh3MorphedUVVertices = (Vector3[])this.mesh3.UVVertices.Clone();
			this.mesh3VisibleMorphedUVVertices = (Vector3[])this.mesh3.UVVertices.Clone();
		}
		this.morphBank1.ResetMorphsFast(resetBones);
		if (this.morphBank1OtherGender != null)
		{
			this.morphBank1OtherGender.ResetMorphsFast(resetBones);
		}
		this.morphBank2.ResetMorphsFast(resetBones);
		if (this.morphBank3 != null)
		{
			this.morphBank3.ResetMorphsFast(resetBones);
		}
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x0015E126 File Offset: 0x0015C526
	public void ResetMorphs()
	{
		this.triggerReset = true;
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x0015E12F File Offset: 0x0015C52F
	public void WaitForRunTask()
	{
		if (this._threadsRunning)
		{
			while (this.characterRunTask.working)
			{
				Thread.Sleep(0);
			}
		}
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x0015E159 File Offset: 0x0015C559
	protected void SmoothResetMorphs()
	{
		this.WaitForRunTask();
		this.ResetMorphsInternal(true);
		this.SmoothApplyMorphs();
	}

	// Token: 0x060047C4 RID: 18372 RVA: 0x0015E16E File Offset: 0x0015C56E
	public void SmoothApplyMorphsLite()
	{
		this.WaitForRunTask();
		this.PrepRun(false);
		this.RunThreaded(!this._threadsRunning);
		this.FinishRun();
	}

	// Token: 0x060047C5 RID: 18373 RVA: 0x0015E194 File Offset: 0x0015C594
	public void SmoothApplyMorphs()
	{
		this.WaitForRunTask();
		this.PrepRun(false);
		this.RunThreaded(!this._threadsRunning);
		this.FinishRun();
		this.PrepRun(false);
		this.RunThreaded(!this._threadsRunning);
		this.FinishRun();
		if (this.physicsMeshes != null)
		{
			foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshes)
			{
				dazphysicsMesh.ResetSoftJoints();
			}
		}
	}

	// Token: 0x060047C6 RID: 18374 RVA: 0x0015E210 File Offset: 0x0015C610
	protected void PrepRun(bool skipPhysics = false)
	{
		if (this.skin == null)
		{
			return;
		}
		float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		this.prepTimeStart = elapsedMilliseconds;
		this.skinPrepStartTime = elapsedMilliseconds;
		this.skin.SkinMeshCPUandGPUStartFrameFast();
		elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		this.MAIN_skinPrepTime = elapsedMilliseconds - this.skinPrepStartTime;
		this.physicsMeshPrepStartTime = elapsedMilliseconds;
		if (!skipPhysics)
		{
			foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshes)
			{
				if (dazphysicsMesh.isEnabled && dazphysicsMesh.wasInit)
				{
					dazphysicsMesh.PrepareSoftUpdateJointsThreaded();
					dazphysicsMesh.PrepareSoftMorphVerticesThreadedFast();
				}
			}
		}
		elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		this.MAIN_physicsMeshPrepTime = elapsedMilliseconds - this.physicsMeshPrepStartTime;
		this.morphPrepStartTime = elapsedMilliseconds;
		this.morphBank1.PrepMorphsThreadedFast();
		if (this.useOtherGenderMorphs && this.morphBank1OtherGender != null)
		{
			this.morphBank1OtherGender.PrepMorphsThreadedFast();
		}
		this.morphBank2.PrepMorphsThreadedFast();
		if (this.morphBank3 != null)
		{
			this.morphBank3.PrepMorphsThreadedFast();
		}
		elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		this.MAIN_morphPrepTime = elapsedMilliseconds - this.morphPrepStartTime;
		elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		this.bonePrepStartTime = elapsedMilliseconds;
		if (this.bones != null)
		{
			foreach (DAZBone dazbone in this.bones.dazBones)
			{
				dazbone.PrepThreadUpdate();
			}
		}
		elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		this.MAIN_bonePrepTime = elapsedMilliseconds - this.bonePrepStartTime;
		this.otherPrepStartTime = elapsedMilliseconds;
		if (this.sdmFromDistanceComps != null)
		{
			foreach (SetDAZMorphFromDistance setDAZMorphFromDistance in this.sdmFromDistanceComps)
			{
				setDAZMorphFromDistance.DoUpdate();
			}
		}
		if (this.sdmFromLocalDistanceComps != null)
		{
			foreach (SetDAZMorphFromLocalDistance setDAZMorphFromLocalDistance in this.sdmFromLocalDistanceComps)
			{
				setDAZMorphFromLocalDistance.DoUpdate();
			}
		}
		if (this.setAnchorFromVertexObjects != null)
		{
			foreach (SetAnchorFromVertex setAnchorFromVertex in this.setAnchorFromVertexObjects)
			{
				if (setAnchorFromVertex.isEnabled)
				{
					setAnchorFromVertex.PrepThreadUpdate(true);
				}
			}
		}
		elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		this.MAIN_otherPrepTime = elapsedMilliseconds - this.otherPrepStartTime;
		this.MAIN_prepTime = elapsedMilliseconds - this.prepTimeStart;
	}

	// Token: 0x060047C7 RID: 18375 RVA: 0x0015E484 File Offset: 0x0015C884
	protected void SecondaryMergeVerts()
	{
		try
		{
			this.mergedMesh.UpdateVerticesPrepThreadedFast(this.mesh1VisibleMorphedUVVertices, this.mergedMeshVisibleMorphedUVVertices, true);
			this.mergedMesh.UpdateVerticesThreadedFast(this.mesh1VisibleMorphedUVVertices, this.mesh2VisibleMorphedUVVertices, this.mesh3VisibleMorphedUVVertices, this.mergedMeshVisibleMorphedUVVertices, 0, this.mergedMesh.numGraftBaseVertices, true);
			this.mergedMesh.UpdateVerticesFinishThreadedFast(this.mesh1VisibleMorphedUVVertices, this.mesh2VisibleMorphedUVVertices, this.mesh3VisibleMorphedUVVertices, this.mergedMeshVisibleMorphedUVVertices, true);
			if (this.autoColliderUpdaters != null && this.visibleNonPoseChangedThreaded)
			{
				foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this.autoColliderUpdaters)
				{
					if (autoColliderBatchUpdater.isEnabled)
					{
						autoColliderBatchUpdater.UpdateSizeThreadedFast(this.mergedMeshVisibleMorphedUVVertices, this.mergedMesh.morphedUVNormals);
					}
				}
			}
			foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshes)
			{
				if (dazphysicsMesh.isEnabled && dazphysicsMesh.wasInit)
				{
					if (this.visibleNonPoseChangedThreaded)
					{
						dazphysicsMesh.RecalculateLinkJointsFast(this.mergedMeshVisibleMorphedUVVertices);
					}
					dazphysicsMesh.SoftVerticesSetAutoRadiusFast(this.mergedMeshVisibleMorphedUVVertices);
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in thread caught " + ex.ToString());
		}
	}

	// Token: 0x060047C8 RID: 18376 RVA: 0x0015E5F4 File Offset: 0x0015C9F4
	protected void MergeRun2()
	{
		try
		{
			this.mergedMesh.UpdateVerticesThreadedFast(this.mesh1MorphedUVVertices, this.mesh2MorphedUVVertices, this.mesh3MorphedUVVertices, this.mergedMeshMorphedUVVertices, this.mergeRun2mini, this.mergeRun2maxi, false);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in thread caught " + ex.ToString());
		}
	}

	// Token: 0x060047C9 RID: 18377 RVA: 0x0015E664 File Offset: 0x0015CA64
	protected void MergeRun3()
	{
		try
		{
			this.mergedMesh.UpdateVerticesThreadedFast(this.mesh1MorphedUVVertices, this.mesh2MorphedUVVertices, this.mesh3MorphedUVVertices, this.mergedMeshMorphedUVVertices, this.mergeRun3mini, this.mergeRun3maxi, false);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in thread caught " + ex.ToString());
		}
	}

	// Token: 0x060047CA RID: 18378 RVA: 0x0015E6D4 File Offset: 0x0015CAD4
	protected void MergeRun4()
	{
		try
		{
			this.mergedMesh.UpdateVerticesThreadedFast(this.mesh1MorphedUVVertices, this.mesh2MorphedUVVertices, this.mesh3MorphedUVVertices, this.mergedMeshMorphedUVVertices, this.mergeRun4mini, this.mergeRun4maxi, false);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in thread caught " + ex.ToString());
		}
	}

	// Token: 0x060047CB RID: 18379 RVA: 0x0015E744 File Offset: 0x0015CB44
	protected void RunThreaded(bool noThreading = false)
	{
		if (this.skin == null)
		{
			return;
		}
		try
		{
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.boneUpdateStartTime = elapsedMilliseconds;
			this.threadStartTime = elapsedMilliseconds;
			foreach (DAZBone dazbone in this.bones.dazBones)
			{
				dazbone.ThreadsafeUpdate();
			}
			if (this.skinForThread != null)
			{
				this.skinForThread.RecalcBones();
			}
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.THREAD_boneUpdateTime = elapsedMilliseconds - this.boneUpdateStartTime;
			if (this.sdmFromAverageBoneAngleComps != null)
			{
				foreach (SetDAZMorphFromAverageBoneAngle setDAZMorphFromAverageBoneAngle in this.sdmFromAverageBoneAngleComps)
				{
					setDAZMorphFromAverageBoneAngle.DoUpdate();
				}
			}
			if (this.sdmFromBoneAngleComps != null)
			{
				foreach (SetDAZMorphFromBoneAngle setDAZMorphFromBoneAngle in this.sdmFromBoneAngleComps)
				{
					setDAZMorphFromBoneAngle.DoUpdate();
				}
			}
			float num = 600000f;
			if (elapsedMilliseconds - this.lastMorphResetTime > num)
			{
				this.lastMorphResetTime = elapsedMilliseconds;
				this.ResetMorphsInternal(false);
			}
			this.morphTime1Start = elapsedMilliseconds;
			bool flag = this.morphBank1.ApplyMorphsThreadedFast(this.mesh1MorphedUVVertices, this.mesh1VisibleMorphedUVVertices, this.bones);
			bool flag2 = false;
			if (this.useOtherGenderMorphs && this.morphBank1OtherGender != null)
			{
				flag2 = this.morphBank1OtherGender.ApplyMorphsThreadedFast(this.mesh1MorphedUVVertices, this.mesh1VisibleMorphedUVVertices, this.bones);
			}
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.THREAD_morphTime1 = elapsedMilliseconds - this.morphTime1Start;
			this.morphTime2Start = elapsedMilliseconds;
			bool flag3 = this.morphBank2.ApplyMorphsThreadedFast(this.mesh2MorphedUVVertices, this.mesh2VisibleMorphedUVVertices, this.bones);
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.THREAD_morphTime2 = elapsedMilliseconds - this.morphTime2Start;
			bool flag4 = false;
			if (this.morphBank3 != null && this.mesh3 != null)
			{
				this.morphTime3Start = elapsedMilliseconds;
				flag4 = this.morphBank3.ApplyMorphsThreadedFast(this.mesh3MorphedUVVertices, this.mesh3VisibleMorphedUVVertices, this.bones);
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_morphTime3 = elapsedMilliseconds - this.morphTime3Start;
			}
			this.visibleNonPoseChangedThreaded = (flag || flag2 || flag3 || flag4);
			this.mergeTimeStart = elapsedMilliseconds;
			if (noThreading)
			{
				this.SecondaryMergeVerts();
			}
			else
			{
				this.characterRunTask2.working = true;
				this.characterRunTask2.resetEvent.Set();
			}
			int numGraftBaseVertices = this.mergedMesh.numGraftBaseVertices;
			int mini = 0;
			int num2 = numGraftBaseVertices / 4;
			this.mergeRun2mini = num2;
			this.mergeRun2maxi = this.mergeRun2mini + num2;
			this.mergeRun3mini = this.mergeRun2maxi;
			this.mergeRun3maxi = this.mergeRun3mini + num2;
			this.mergeRun4mini = this.mergeRun3maxi;
			this.mergeRun4maxi = numGraftBaseVertices;
			this.mergedMesh.UpdateVerticesPrepThreadedFast(this.mesh1MorphedUVVertices, this.mergedMeshMorphedUVVertices, false);
			if (noThreading)
			{
				this.MergeRun2();
				this.MergeRun3();
				this.MergeRun4();
			}
			else
			{
				this.characterRunTask3.working = true;
				this.characterRunTask3.resetEvent.Set();
				this.characterRunTask4.working = true;
				this.characterRunTask4.resetEvent.Set();
				this.characterRunTask5.working = true;
				this.characterRunTask5.resetEvent.Set();
			}
			this.mergedMesh.UpdateVerticesThreadedFast(this.mesh1MorphedUVVertices, this.mesh2MorphedUVVertices, this.mesh3MorphedUVVertices, this.mergedMeshMorphedUVVertices, mini, num2, false);
			if (!noThreading)
			{
				while (this.characterRunTask3.working)
				{
					Thread.Sleep(0);
				}
				while (this.characterRunTask4.working)
				{
					Thread.Sleep(0);
				}
				while (this.characterRunTask5.working)
				{
					Thread.Sleep(0);
				}
			}
			this.mergedMesh.UpdateVerticesFinishThreadedFast(this.mesh1MorphedUVVertices, this.mesh2MorphedUVVertices, this.mesh3MorphedUVVertices, this.mergedMeshMorphedUVVertices, false);
			Array.Copy(this.mergedMeshMorphedUVVertices, this.mergedMeshMorphedUVVerticesCopy, this.mergedMeshMorphedUVVertices.Length);
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.THREAD_mergeTime = elapsedMilliseconds - this.mergeTimeStart;
			if (this._doSnap)
			{
				this._snappedMorphedUVVertices = (Vector3[])this.mergedMeshMorphedUVVertices.Clone();
			}
			if (this.doSetMergedVerts)
			{
				Array.Copy(this.mergedMeshMorphedUVVertices, this.mergedMeshMorphedUVVerticesMergedOnly, this.mergedMeshMorphedUVVertices.Length);
			}
			if (this.doSkin && this.skinForThread != null)
			{
				this.skinPostTimeStart = elapsedMilliseconds;
				this.skinForThread.SkinMeshPostVertsThreadedFast(this.mergedMeshMorphedUVVerticesCopy);
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_skinPostTime = elapsedMilliseconds - this.skinPostTimeStart;
				this.physicsMeshUpdateTargetsStartTime = elapsedMilliseconds;
				foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshes)
				{
					if (dazphysicsMesh.isEnabled && dazphysicsMesh.wasInit)
					{
						dazphysicsMesh.UpdateSoftJointTargetsThreadedFast(this.skinForThread.rawSkinnedWorkingVerts, this.skinForThread.postSkinNormalsThreaded);
					}
				}
				this.newJointTargets = true;
				this.pmJointTargetsUpdatingOnThread = false;
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_physicsMeshUpdateTargetsTime = elapsedMilliseconds - this.physicsMeshUpdateTargetsStartTime;
				this.skinTimeStart = elapsedMilliseconds;
				this.skinForThread.SkinMeshThreadedFast(this.mergedMeshMorphedUVVertices, false);
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_skinTime = elapsedMilliseconds - this.skinTimeStart;
				this.physicsMeshMorphStartTime = elapsedMilliseconds;
				foreach (DAZPhysicsMesh dazphysicsMesh2 in this.physicsMeshes)
				{
					if (dazphysicsMesh2.isEnabled && dazphysicsMesh2.wasInit)
					{
						dazphysicsMesh2.MorphSoftVerticesThreadedFast(this.skinForThread.rawSkinnedWorkingVerts);
					}
				}
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_physicsMeshMorphTime = elapsedMilliseconds - this.physicsMeshMorphStartTime;
				this.autoColliderUpdateAnchorsTimeStart = elapsedMilliseconds;
				if (this.autoColliderUpdaters != null)
				{
					foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this.autoColliderUpdaters)
					{
						if (autoColliderBatchUpdater.isEnabled)
						{
							autoColliderBatchUpdater.UpdateAnchorsThreadedFast(this.mergedMeshMorphedUVVertices, this.skinForThread.postSkinNormalsThreaded);
						}
					}
				}
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_autoColliderUpdateAnchorsTime = elapsedMilliseconds - this.autoColliderUpdateAnchorsTimeStart;
				this.setAnchorFromVertexUpdateTimeStart = elapsedMilliseconds;
				if (this.setAnchorFromVertexObjects != null)
				{
					foreach (SetAnchorFromVertex setAnchorFromVertex in this.setAnchorFromVertexObjects)
					{
						if (setAnchorFromVertex.isEnabled)
						{
							setAnchorFromVertex.DoThreadUpdate(this.mergedMeshMorphedUVVertices, true);
						}
					}
				}
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_setAnchorFromVertexUpdateTime = elapsedMilliseconds - this.setAnchorFromVertexUpdateTimeStart;
				this.postSkinMorphTimeStart = elapsedMilliseconds;
				this.skinForThread.SkinMeshCPUandGPUApplyPostSkinMorphsFast();
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.THREAD_postSkinMorphTime = elapsedMilliseconds - this.postSkinMorphTimeStart;
			}
			if (!noThreading)
			{
				while (this.characterRunTask2.working)
				{
					Thread.Sleep(0);
				}
			}
			this.THREAD_time = elapsedMilliseconds - this.threadStartTime;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in thread caught " + ex.ToString());
		}
		this.pmJointTargetsUpdatingOnThread = false;
	}

	// Token: 0x060047CC RID: 18380 RVA: 0x0015EEB8 File Offset: 0x0015D2B8
	public void SetNeedsAutoColliderUpdate()
	{
		this.needsAutoColliderUpdate = true;
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x0015EEC4 File Offset: 0x0015D2C4
	protected void FinishRun()
	{
		if (this.skin == null)
		{
			return;
		}
		if (this.triggerReset)
		{
			this.ResetMorphsInternal(true);
			this.triggerReset = false;
		}
		else
		{
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.morphFinishStartTime = elapsedMilliseconds;
			this.finishTimeStart = elapsedMilliseconds;
			this.morphBank1.ApplyMorphsThreadedFastFinish();
			if (this.useOtherGenderMorphs && this.morphBank1OtherGender != null)
			{
				this.morphBank1OtherGender.ApplyMorphsThreadedFastFinish();
			}
			this.morphBank2.ApplyMorphsThreadedFastFinish();
			if (this.morphBank3 != null)
			{
				this.morphBank3.ApplyMorphsThreadedFastFinish();
			}
			bool flag = false;
			if (this.useOtherGenderMorphs && this.morphBank1OtherGender != null)
			{
				flag = this.morphBank1OtherGender.bonesDirty;
			}
			if (flag || this.morphBank1.bonesDirty || this.morphBank2.bonesDirty)
			{
				this.bones.SetMorphedTransform(false);
				this.morphBank1.bonesDirty = false;
				if (this.useOtherGenderMorphs && this.morphBank1OtherGender != null)
				{
					this.morphBank1OtherGender.bonesDirty = false;
				}
				this.morphBank2.bonesDirty = false;
			}
			if (this.morphBank1.boneRotationsDirty != null && this.morphBank1.boneRotationsDirty.Count > 0)
			{
				foreach (DAZBone dazbone in this.morphBank1.boneRotationsDirty.Keys)
				{
					dazbone.SyncMorphBoneRotations(false);
				}
				this.morphBank1.boneRotationsDirty.Clear();
			}
			if (this.useOtherGenderMorphs && this.morphBank1OtherGender != null && this.morphBank1OtherGender.boneRotationsDirty != null)
			{
				foreach (DAZBone dazbone2 in this.morphBank1OtherGender.boneRotationsDirty.Keys)
				{
					dazbone2.SyncMorphBoneRotations(false);
				}
				this.morphBank1OtherGender.boneRotationsDirty.Clear();
			}
			if (this.morphBank2.boneRotationsDirty != null && this.morphBank2.boneRotationsDirty.Count > 0)
			{
				foreach (DAZBone dazbone3 in this.morphBank2.boneRotationsDirty.Keys)
				{
					dazbone3.SyncMorphBoneRotations(false);
				}
				this.morphBank2.boneRotationsDirty.Clear();
			}
			if (this.doSetMorphedMeshVerts)
			{
				this.mesh1.SetMorphedUVMeshVertices(this.mesh1MorphedUVVertices);
				this.mesh2.SetMorphedUVMeshVertices(this.mesh2MorphedUVVertices);
				if (this.mesh3 != null)
				{
					this.mesh3.SetMorphedUVMeshVertices(this.mesh3MorphedUVVertices);
				}
			}
			if (this.doSetMergedVerts)
			{
				Vector3[] morphedUVVertices = this.mergedMesh.morphedUVVertices;
				Array.Copy(this.mergedMeshMorphedUVVerticesMergedOnly, morphedUVVertices, this.mergedMeshMorphedUVVerticesMergedOnly.Length);
				if (this.doSetMergedMeshVerts)
				{
					this.mergedMesh.SetMorphedUVMeshVertices(morphedUVVertices);
				}
			}
			if (this.characterSelector != null)
			{
				this.characterSelector.CleanDemandActivatedMorphs();
			}
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.MAIN_morphFinishTime = elapsedMilliseconds - this.morphFinishStartTime;
			foreach (DAZBone dazbone4 in this.bones.dazBones)
			{
				dazbone4.FinishThreadUpdate();
			}
			foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this.autoColliderUpdaters)
			{
				autoColliderBatchUpdater.CheckPhysicsCorruption();
			}
			if (this.skin != null)
			{
				if (this._doSnap)
				{
					if (this._snappedMorphedUVVertices == null)
					{
						this._snappedMorphedUVVertices = (Vector3[])this.mergedMeshMorphedUVVertices.Clone();
					}
					this._snappedMorphedUVNormals = this.skin.RecalcAndGetAllNormals(this._snappedMorphedUVVertices);
					Vector3[] array2 = (Vector3[])this.mergedMeshMorphedUVVertices.Clone();
					this._snappedSkinnedVertices = (Vector3[])array2.Clone();
					this.skin.SmoothAllVertices(array2, this._snappedSkinnedVertices);
					this._snappedSkinnedNormals = this.skin.RecalcAndGetAllNormals(this._snappedSkinnedVertices);
				}
				if (this.doSkin)
				{
					this.autoColliderFinishTimeStart = elapsedMilliseconds;
					if (this.autoColliderUpdaters != null)
					{
						if (!this._resetSimulation && !this._freezeSimulation)
						{
							foreach (AutoColliderBatchUpdater autoColliderBatchUpdater2 in this.autoColliderUpdaters)
							{
								autoColliderBatchUpdater2.UpdateThreadedFinish(this.mergedMeshMorphedUVVertices, this.skin.postSkinNormalsThreaded);
							}
						}
						if (this.needsAutoColliderUpdate)
						{
							foreach (AutoColliderBatchUpdater autoColliderBatchUpdater3 in this.autoColliderUpdaters)
							{
								autoColliderBatchUpdater3.UpdateAutoColliders();
							}
							this.needsAutoColliderUpdate = false;
						}
					}
					elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
					this.MAIN_autoColliderFinishTime = elapsedMilliseconds - this.autoColliderFinishTimeStart;
					this.physicsMeshFinishStartTime = elapsedMilliseconds;
					foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshes)
					{
						if (dazphysicsMesh.isEnabled && dazphysicsMesh.wasInit)
						{
							dazphysicsMesh.SoftVerticesSetAutoRadiusFinishFast();
							dazphysicsMesh.RecalculateLinkJointsFinishFast();
						}
					}
					elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
					this.MAIN_physicsMeshFinishTime = elapsedMilliseconds - this.physicsMeshFinishStartTime;
					this.setAnchorFromVertexFinishStartTime = elapsedMilliseconds;
					if (this.setAnchorFromVertexObjects != null)
					{
						foreach (SetAnchorFromVertex setAnchorFromVertex in this.setAnchorFromVertexObjects)
						{
							if (setAnchorFromVertex.isEnabled)
							{
								setAnchorFromVertex.FinishThreadUpdate();
							}
						}
					}
					elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
					this.MAIN_setAnchorFromVertexFinishTime = elapsedMilliseconds - this.setAnchorFromVertexFinishStartTime;
					this.skinFinishStartTime = elapsedMilliseconds;
					this.skin.SkinMeshCPUandGPUFinishFrameFast();
					elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
					this.MAIN_skinFinishTime = elapsedMilliseconds - this.skinFinishStartTime;
				}
			}
			this.MAIN_finishTime = elapsedMilliseconds - this.finishTimeStart;
		}
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x0015F54C File Offset: 0x0015D94C
	protected void OnEnable()
	{
		if (Application.isPlaying && SuperController.singleton != null)
		{
			SuperController.singleton.ResetSimulation(5, "Character Thread Enable", true);
		}
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x0015F57C File Offset: 0x0015D97C
	private void FixedUpdate()
	{
		if (Application.isPlaying)
		{
			foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this.autoColliderUpdaters)
			{
				autoColliderBatchUpdater.CheckPhysicsCorruption();
			}
		}
		if (Application.isPlaying && this.skin != null && this.mergedMeshMorphedUVVertices != null && this.threadWasRun && this.doSkin && this.doUpdate && (SuperController.singleton.autoSimulation || this._resetSimulation))
		{
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.fixedStartTime = elapsedMilliseconds;
			if (this.useThreading && this.fixedUpdateWaitForThread && DAZPhysicsMesh.globalEnable)
			{
				this.fixedThreadWaitTimeStart = elapsedMilliseconds;
				while (this.pmJointTargetsUpdatingOnThread)
				{
					Thread.Sleep(0);
				}
				if (this.newJointTargets)
				{
					elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
					this.MAIN_fixedThreadWaitTime = elapsedMilliseconds - this.fixedThreadWaitTimeStart;
					PerfMon.ReportWaitTime(this.MAIN_fixedThreadWaitTime);
				}
			}
			this.physicsMeshFixedUpdateStartTime = elapsedMilliseconds;
			foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshes)
			{
				if (dazphysicsMesh.isEnabled && dazphysicsMesh.wasInit)
				{
					dazphysicsMesh.UpdateSoftJointsFast(this.waitForNewTargets && !this.newJointTargets);
					dazphysicsMesh.ApplySoftJointBackForces();
				}
			}
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.MAIN_physicsMeshFixedUpdateTime = elapsedMilliseconds - this.physicsMeshFixedUpdateStartTime;
			this.FIXED_time = elapsedMilliseconds - this.fixedStartTime;
			this.newJointTargets = false;
			if (this._resetSimulation)
			{
				if (this.bones != null)
				{
					foreach (DAZBone dazbone in this.bones.dazBones)
					{
						dazbone.SetResetVelocity();
					}
				}
				if (this.autoColliderUpdaters != null)
				{
					foreach (AutoColliderBatchUpdater autoColliderBatchUpdater2 in this.autoColliderUpdaters)
					{
						autoColliderBatchUpdater2.UpdateThreadedFinish(this.mergedMeshMorphedUVVertices, this.skin.postSkinNormalsThreaded);
					}
				}
			}
		}
		else
		{
			this.MAIN_fixedThreadWaitTime = 0f;
			this.MAIN_physicsMeshFixedUpdateTime = 0f;
			this.FIXED_time = 0f;
		}
	}

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x060047D0 RID: 18384 RVA: 0x0015F7E3 File Offset: 0x0015DBE3
	// (set) Token: 0x060047D1 RID: 18385 RVA: 0x0015F7EB File Offset: 0x0015DBEB
	public bool renderSuspend
	{
		get
		{
			return this._renderSuspend;
		}
		set
		{
			this._renderSuspend = value;
		}
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x0015F7F4 File Offset: 0x0015DBF4
	protected override void Update()
	{
		base.Update();
		if (Application.isPlaying && this.skin != null && this.mergedMeshMorphedUVVertices != null && this.doUpdate)
		{
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.updateTimeStart = elapsedMilliseconds;
			if (this.useThreading)
			{
				this.StartThreads();
				this.updateThreadWaitTimeStart = elapsedMilliseconds;
				if (this.waitForThread)
				{
					while (this.characterRunTask.working)
					{
						Thread.Sleep(0);
					}
				}
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				float num = elapsedMilliseconds - this.updateThreadWaitTimeStart;
				PerfMon.ReportWaitTime(num);
				this.MAIN_updateThreadWaitTime = num;
				if (!this.characterRunTask.working)
				{
					this.threadWasRun = true;
					this.FinishRun();
					this.PrepRun(!SuperController.singleton.autoSimulation);
					this.pmJointTargetsUpdatingOnThread = true;
					this.characterRunTask.working = true;
					this.characterRunTask.resetEvent.Set();
				}
			}
			else
			{
				this.threadWasRun = true;
				this.RunThreaded(true);
				this.FinishRun();
				this.PrepRun(!SuperController.singleton.autoSimulation);
			}
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.UPDATE_time = elapsedMilliseconds - this.updateTimeStart;
		}
		else
		{
			this.UPDATE_time = 0f;
		}
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x0015F94C File Offset: 0x0015DD4C
	private void LateUpdate()
	{
		if (Application.isPlaying && this.skin != null && this.mergedMeshMorphedUVVertices != null)
		{
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.lateTimeStart = elapsedMilliseconds;
			if (this.doDraw && !this._renderSuspend)
			{
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.skinDrawStartTime = elapsedMilliseconds;
				this.skin.DrawMeshGPU();
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				this.MAIN_skinDrawTime = elapsedMilliseconds - this.skinDrawStartTime;
			}
			this.LATE_time = elapsedMilliseconds - this.lateTimeStart;
		}
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x0015F9DC File Offset: 0x0015DDDC
	protected void Start()
	{
		if (Application.isPlaying)
		{
			FileManager.RegisterRefreshHandler(new OnRefresh(this.RefreshPackageMorphs));
		}
	}

	// Token: 0x060047D5 RID: 18389 RVA: 0x0015F9F9 File Offset: 0x0015DDF9
	protected void OnDestroy()
	{
		if (Application.isPlaying)
		{
			FileManager.UnregisterRefreshHandler(new OnRefresh(this.RefreshPackageMorphs));
			this.StopThreads();
		}
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x0015FA1C File Offset: 0x0015DE1C
	protected void OnApplicationQuit()
	{
		this.StopThreads();
	}

	// Token: 0x040034ED RID: 13549
	public bool waitForThread = true;

	// Token: 0x040034EE RID: 13550
	private CustomSampler runTask1Sampler = CustomSampler.Create("RunThreaded");

	// Token: 0x040034EF RID: 13551
	protected DAZCharacterRun.DAZCharacterRunTaskInfo characterRunTask;

	// Token: 0x040034F0 RID: 13552
	protected DAZCharacterRun.DAZCharacterRunTaskInfo characterRunTask2;

	// Token: 0x040034F1 RID: 13553
	protected DAZCharacterRun.DAZCharacterRunTaskInfo characterRunTask3;

	// Token: 0x040034F2 RID: 13554
	protected DAZCharacterRun.DAZCharacterRunTaskInfo characterRunTask4;

	// Token: 0x040034F3 RID: 13555
	protected DAZCharacterRun.DAZCharacterRunTaskInfo characterRunTask5;

	// Token: 0x040034F4 RID: 13556
	protected bool _threadsRunning;

	// Token: 0x040034F5 RID: 13557
	public DAZCharacterSelector characterSelector;

	// Token: 0x040034F6 RID: 13558
	public DAZMorphBank morphBank1;

	// Token: 0x040034F7 RID: 13559
	public bool useOtherGenderMorphs;

	// Token: 0x040034F8 RID: 13560
	public DAZMorphBank morphBank1OtherGender;

	// Token: 0x040034F9 RID: 13561
	public DAZMorphBank morphBank2;

	// Token: 0x040034FA RID: 13562
	public DAZMorphBank morphBank3;

	// Token: 0x040034FB RID: 13563
	public DAZBones bones;

	// Token: 0x040034FC RID: 13564
	public DAZMergedSkinV2 skin;

	// Token: 0x040034FD RID: 13565
	protected DAZMergedSkinV2 skinForThread;

	// Token: 0x040034FE RID: 13566
	protected DAZMergedMesh mergedMesh;

	// Token: 0x040034FF RID: 13567
	protected DAZMesh mesh1;

	// Token: 0x04003500 RID: 13568
	protected DAZMesh mesh2;

	// Token: 0x04003501 RID: 13569
	protected DAZMesh mesh3;

	// Token: 0x04003502 RID: 13570
	public AutoColliderBatchUpdater[] autoColliderUpdaters;

	// Token: 0x04003503 RID: 13571
	public DAZPhysicsMesh[] physicsMeshes;

	// Token: 0x04003504 RID: 13572
	public SetAnchorFromVertex[] setAnchorFromVertexObjects;

	// Token: 0x04003505 RID: 13573
	public Transform setDazMorphContainer;

	// Token: 0x04003506 RID: 13574
	protected SetDAZMorphFromBoneAngle[] sdmFromBoneAngleComps;

	// Token: 0x04003507 RID: 13575
	protected SetDAZMorphFromAverageBoneAngle[] sdmFromAverageBoneAngleComps;

	// Token: 0x04003508 RID: 13576
	protected SetDAZMorphFromDistance[] sdmFromDistanceComps;

	// Token: 0x04003509 RID: 13577
	protected SetDAZMorphFromLocalDistance[] sdmFromLocalDistanceComps;

	// Token: 0x0400350A RID: 13578
	protected Vector3[] mesh1MorphedUVVertices;

	// Token: 0x0400350B RID: 13579
	protected Vector3[] mesh1VisibleMorphedUVVertices;

	// Token: 0x0400350C RID: 13580
	protected Vector3[] mesh2MorphedUVVertices;

	// Token: 0x0400350D RID: 13581
	protected Vector3[] mesh2VisibleMorphedUVVertices;

	// Token: 0x0400350E RID: 13582
	protected Vector3[] mesh3MorphedUVVertices;

	// Token: 0x0400350F RID: 13583
	protected Vector3[] mesh3VisibleMorphedUVVertices;

	// Token: 0x04003510 RID: 13584
	protected Vector3[] mergedMeshMorphedUVVertices;

	// Token: 0x04003511 RID: 13585
	protected Vector3[] mergedMeshMorphedUVVerticesCopy;

	// Token: 0x04003512 RID: 13586
	protected Vector3[] mergedMeshMorphedUVVerticesMergedOnly;

	// Token: 0x04003513 RID: 13587
	protected Vector3[] mergedMeshVisibleMorphedUVVertices;

	// Token: 0x04003514 RID: 13588
	protected bool _doSnap;

	// Token: 0x04003515 RID: 13589
	protected Vector3[] _snappedMorphedUVVertices;

	// Token: 0x04003516 RID: 13590
	protected Vector3[] _snappedMorphedUVNormals;

	// Token: 0x04003517 RID: 13591
	protected Vector3[] _snappedSkinnedVertices;

	// Token: 0x04003518 RID: 13592
	protected Vector3[] _snappedSkinnedNormals;

	// Token: 0x04003519 RID: 13593
	protected float fixedStartTime;

	// Token: 0x0400351A RID: 13594
	public float FIXED_time;

	// Token: 0x0400351B RID: 13595
	protected float updateTimeStart;

	// Token: 0x0400351C RID: 13596
	public float UPDATE_time;

	// Token: 0x0400351D RID: 13597
	protected float lateTimeStart;

	// Token: 0x0400351E RID: 13598
	public float LATE_time;

	// Token: 0x0400351F RID: 13599
	protected float fixedThreadWaitTimeStart;

	// Token: 0x04003520 RID: 13600
	public float MAIN_fixedThreadWaitTime;

	// Token: 0x04003521 RID: 13601
	protected float updateThreadWaitTimeStart;

	// Token: 0x04003522 RID: 13602
	public float MAIN_updateThreadWaitTime;

	// Token: 0x04003523 RID: 13603
	protected float finishTimeStart;

	// Token: 0x04003524 RID: 13604
	public float MAIN_finishTime;

	// Token: 0x04003525 RID: 13605
	protected float prepTimeStart;

	// Token: 0x04003526 RID: 13606
	public float MAIN_prepTime;

	// Token: 0x04003527 RID: 13607
	protected float autoColliderFinishTimeStart;

	// Token: 0x04003528 RID: 13608
	public float MAIN_autoColliderFinishTime;

	// Token: 0x04003529 RID: 13609
	protected float skinPrepStartTime;

	// Token: 0x0400352A RID: 13610
	public float MAIN_skinPrepTime;

	// Token: 0x0400352B RID: 13611
	protected float skinFinishStartTime;

	// Token: 0x0400352C RID: 13612
	public float MAIN_skinFinishTime;

	// Token: 0x0400352D RID: 13613
	protected float skinDrawStartTime;

	// Token: 0x0400352E RID: 13614
	public float MAIN_skinDrawTime;

	// Token: 0x0400352F RID: 13615
	protected float physicsMeshPrepStartTime;

	// Token: 0x04003530 RID: 13616
	public float MAIN_physicsMeshPrepTime;

	// Token: 0x04003531 RID: 13617
	protected float physicsMeshFixedUpdateStartTime;

	// Token: 0x04003532 RID: 13618
	public float MAIN_physicsMeshFixedUpdateTime;

	// Token: 0x04003533 RID: 13619
	protected float physicsMeshFinishStartTime;

	// Token: 0x04003534 RID: 13620
	public float MAIN_physicsMeshFinishTime;

	// Token: 0x04003535 RID: 13621
	protected float setAnchorFromVertexFinishStartTime;

	// Token: 0x04003536 RID: 13622
	public float MAIN_setAnchorFromVertexFinishTime;

	// Token: 0x04003537 RID: 13623
	protected float morphPrepStartTime;

	// Token: 0x04003538 RID: 13624
	public float MAIN_morphPrepTime;

	// Token: 0x04003539 RID: 13625
	protected float morphFinishStartTime;

	// Token: 0x0400353A RID: 13626
	public float MAIN_morphFinishTime;

	// Token: 0x0400353B RID: 13627
	protected float bonePrepStartTime;

	// Token: 0x0400353C RID: 13628
	public float MAIN_bonePrepTime;

	// Token: 0x0400353D RID: 13629
	protected float otherPrepStartTime;

	// Token: 0x0400353E RID: 13630
	public float MAIN_otherPrepTime;

	// Token: 0x0400353F RID: 13631
	protected float threadStartTime;

	// Token: 0x04003540 RID: 13632
	public float THREAD_time;

	// Token: 0x04003541 RID: 13633
	protected float morphTime1Start;

	// Token: 0x04003542 RID: 13634
	public float THREAD_morphTime1;

	// Token: 0x04003543 RID: 13635
	protected float morphTime2Start;

	// Token: 0x04003544 RID: 13636
	public float THREAD_morphTime2;

	// Token: 0x04003545 RID: 13637
	protected float morphTime3Start;

	// Token: 0x04003546 RID: 13638
	public float THREAD_morphTime3;

	// Token: 0x04003547 RID: 13639
	protected float mergeTimeStart;

	// Token: 0x04003548 RID: 13640
	public float THREAD_mergeTime;

	// Token: 0x04003549 RID: 13641
	protected float skinPostTimeStart;

	// Token: 0x0400354A RID: 13642
	public float THREAD_skinPostTime;

	// Token: 0x0400354B RID: 13643
	protected float skinTimeStart;

	// Token: 0x0400354C RID: 13644
	public float THREAD_skinTime;

	// Token: 0x0400354D RID: 13645
	protected float postSkinMorphTimeStart;

	// Token: 0x0400354E RID: 13646
	public float THREAD_postSkinMorphTime;

	// Token: 0x0400354F RID: 13647
	protected float autoColliderUpdateSizeTimeStart;

	// Token: 0x04003550 RID: 13648
	public float THREAD_autoColliderUpdateSizeTime;

	// Token: 0x04003551 RID: 13649
	protected float autoColliderUpdateAnchorsTimeStart;

	// Token: 0x04003552 RID: 13650
	public float THREAD_autoColliderUpdateAnchorsTime;

	// Token: 0x04003553 RID: 13651
	protected float setAnchorFromVertexUpdateTimeStart;

	// Token: 0x04003554 RID: 13652
	public float THREAD_setAnchorFromVertexUpdateTime;

	// Token: 0x04003555 RID: 13653
	protected float physicsMeshUpdateTargetsStartTime;

	// Token: 0x04003556 RID: 13654
	public float THREAD_physicsMeshUpdateTargetsTime;

	// Token: 0x04003557 RID: 13655
	protected float physicsMeshMorphStartTime;

	// Token: 0x04003558 RID: 13656
	public float THREAD_physicsMeshMorphTime;

	// Token: 0x04003559 RID: 13657
	protected float boneUpdateStartTime;

	// Token: 0x0400355A RID: 13658
	public float THREAD_boneUpdateTime;

	// Token: 0x0400355B RID: 13659
	protected Coroutine refreshCoroutine;

	// Token: 0x0400355C RID: 13660
	protected int mergeRun2mini;

	// Token: 0x0400355D RID: 13661
	protected int mergeRun2maxi;

	// Token: 0x0400355E RID: 13662
	protected int mergeRun3mini;

	// Token: 0x0400355F RID: 13663
	protected int mergeRun3maxi;

	// Token: 0x04003560 RID: 13664
	protected int mergeRun4mini;

	// Token: 0x04003561 RID: 13665
	protected int mergeRun4maxi;

	// Token: 0x04003562 RID: 13666
	public bool visibleNonPoseChangedThreaded;

	// Token: 0x04003563 RID: 13667
	protected float lastMorphResetTime;

	// Token: 0x04003564 RID: 13668
	protected bool needsAutoColliderUpdate;

	// Token: 0x04003565 RID: 13669
	protected bool triggerReset;

	// Token: 0x04003566 RID: 13670
	protected bool threadWasRun;

	// Token: 0x04003567 RID: 13671
	protected bool pmJointTargetsUpdatingOnThread;

	// Token: 0x04003568 RID: 13672
	public bool waitForNewTargets = true;

	// Token: 0x04003569 RID: 13673
	public bool fixedUpdateWaitForThread = true;

	// Token: 0x0400356A RID: 13674
	protected bool newJointTargets = true;

	// Token: 0x0400356B RID: 13675
	public bool doUpdate = true;

	// Token: 0x0400356C RID: 13676
	public bool doSkin = true;

	// Token: 0x0400356D RID: 13677
	public bool doDraw = true;

	// Token: 0x0400356E RID: 13678
	protected bool _renderSuspend;

	// Token: 0x0400356F RID: 13679
	public bool doSetMergedVerts;

	// Token: 0x04003570 RID: 13680
	public bool doSetMergedMeshVerts;

	// Token: 0x04003571 RID: 13681
	public bool doSetMorphedMeshVerts;

	// Token: 0x04003572 RID: 13682
	public bool useThreading = true;

	// Token: 0x02000AAE RID: 2734
	public enum DAZCharacterRunTaskType
	{
		// Token: 0x04003574 RID: 13684
		Run,
		// Token: 0x04003575 RID: 13685
		SecondaryMergeVerts,
		// Token: 0x04003576 RID: 13686
		MergeVertsRun2,
		// Token: 0x04003577 RID: 13687
		MergeVertsRun3,
		// Token: 0x04003578 RID: 13688
		MergeVertsRun4
	}

	// Token: 0x02000AAF RID: 2735
	public class DAZCharacterRunTaskInfo
	{
		// Token: 0x060047D7 RID: 18391 RVA: 0x0015FA24 File Offset: 0x0015DE24
		public DAZCharacterRunTaskInfo()
		{
		}

		// Token: 0x04003579 RID: 13689
		public DAZCharacterRun.DAZCharacterRunTaskType taskType;

		// Token: 0x0400357A RID: 13690
		public string name;

		// Token: 0x0400357B RID: 13691
		public AutoResetEvent resetEvent;

		// Token: 0x0400357C RID: 13692
		public Thread thread;

		// Token: 0x0400357D RID: 13693
		public volatile bool working;

		// Token: 0x0400357E RID: 13694
		public volatile bool kill;

		// Token: 0x0400357F RID: 13695
		public int index1;

		// Token: 0x04003580 RID: 13696
		public int index2;
	}

	// Token: 0x02000FBE RID: 4030
	[CompilerGenerated]
	private sealed class <RefreshWhenHubClosed>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060074FE RID: 29950 RVA: 0x0015FA2C File Offset: 0x0015DE2C
		[DebuggerHidden]
		public <RefreshWhenHubClosed>c__Iterator0()
		{
		}

		// Token: 0x060074FF RID: 29951 RVA: 0x0015FA34 File Offset: 0x0015DE34
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				break;
			case 1U:
				break;
			default:
				return false;
			}
			if (SuperController.singleton.HubOpen || SuperController.singleton.activeUI == SuperController.ActiveUI.PackageDownloader)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			base.WaitForRunTask();
			startt = GlobalStopwatch.GetElapsedMilliseconds();
			if (this.characterSelector != null && this.characterSelector.RefreshPackageMorphs() && this.skinForThread != null)
			{
				base.SmoothResetMorphs();
			}
			stopt = GlobalStopwatch.GetElapsedMilliseconds();
			float t = stopt - startt;
			UnityEngine.Debug.Log("Deferred refresh package morphs took " + t.ToString("F1") + " ms");
			this.refreshCoroutine = null;
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06007500 RID: 29952 RVA: 0x0015FB5C File Offset: 0x0015DF5C
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06007501 RID: 29953 RVA: 0x0015FB64 File Offset: 0x0015DF64
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007502 RID: 29954 RVA: 0x0015FB6C File Offset: 0x0015DF6C
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007503 RID: 29955 RVA: 0x0015FB7C File Offset: 0x0015DF7C
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006916 RID: 26902
		internal float <startt>__0;

		// Token: 0x04006917 RID: 26903
		internal float <stopt>__0;

		// Token: 0x04006918 RID: 26904
		internal float <t>__0;

		// Token: 0x04006919 RID: 26905
		internal DAZCharacterRun $this;

		// Token: 0x0400691A RID: 26906
		internal object $current;

		// Token: 0x0400691B RID: 26907
		internal bool $disposing;

		// Token: 0x0400691C RID: 26908
		internal int $PC;
	}
}
