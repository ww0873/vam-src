using System;
using System.Collections.Generic;
using MVR;
using UnityEngine;

// Token: 0x02000AA3 RID: 2723
public class AutoColliderBatchUpdater : JSONStorable
{
	// Token: 0x060046F3 RID: 18163 RVA: 0x0014A878 File Offset: 0x00148C78
	public AutoColliderBatchUpdater()
	{
	}

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x060046F4 RID: 18164 RVA: 0x0014A887 File Offset: 0x00148C87
	public AutoCollider[] autoColliders
	{
		get
		{
			return this._autoColliders;
		}
	}

	// Token: 0x060046F5 RID: 18165 RVA: 0x0014A890 File Offset: 0x00148C90
	public void UpdateAutoColliders()
	{
		AutoCollider[] componentsInChildren = base.GetComponentsInChildren<AutoCollider>();
		List<AutoCollider> list = new List<AutoCollider>();
		List<AutoCollider> list2 = new List<AutoCollider>();
		List<AutoCollider> list3 = new List<AutoCollider>();
		List<AutoCollider> list4 = new List<AutoCollider>();
		foreach (AutoCollider autoCollider in componentsInChildren)
		{
			if (autoCollider.allowBatchUpdate)
			{
				autoCollider.enabled = false;
				autoCollider.resetSimulation = this._resetSimulation;
				list.Add(autoCollider);
				if (autoCollider.joint != null)
				{
					if (autoCollider.bone != null)
					{
						list3.Add(autoCollider);
					}
					else
					{
						list2.Add(autoCollider);
					}
				}
				else
				{
					list4.Add(autoCollider);
				}
			}
		}
		this._autoColliders = list.ToArray();
		this._autoCollidersWithJointsAndBones = list3.ToArray();
		this._autoCollidersWithJoints = list2.ToArray();
		this._autoCollidersWithoutJoints = list4.ToArray();
		this.numControlledColliders = this._autoColliders.Length;
	}

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x060046F6 RID: 18166 RVA: 0x0014A98F File Offset: 0x00148D8F
	// (set) Token: 0x060046F7 RID: 18167 RVA: 0x0014A997 File Offset: 0x00148D97
	public bool on
	{
		get
		{
			return this._on;
		}
		set
		{
			if (this._on != value)
			{
				this._on = value;
			}
		}
	}

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x060046F8 RID: 18168 RVA: 0x0014A9AC File Offset: 0x00148DAC
	// (set) Token: 0x060046F9 RID: 18169 RVA: 0x0014A9B4 File Offset: 0x00148DB4
	public bool resetSimulation
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
				if (this._autoColliders == null)
				{
					this.UpdateAutoColliders();
				}
				foreach (AutoCollider autoCollider in this._autoColliders)
				{
					autoCollider.resetSimulation = value;
				}
			}
		}
	}

	// Token: 0x060046FA RID: 18170 RVA: 0x0014AA0C File Offset: 0x00148E0C
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

	// Token: 0x060046FB RID: 18171 RVA: 0x0014AB20 File Offset: 0x00148F20
	public void ResetSimulation(AsyncFlag waitFor)
	{
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		this.waitResumeSimulationFlags.Add(waitFor);
		this.resetSimulation = true;
	}

	// Token: 0x060046FC RID: 18172 RVA: 0x0014AB4C File Offset: 0x00148F4C
	public void UpdateSizeThreadedFast(Vector3[] verts, Vector3[] norms)
	{
		if (this._autoColliders != null && this._autoColliders.Length > 0)
		{
			foreach (AutoCollider autoCollider in this._autoColliders)
			{
				autoCollider.AutoColliderSizeSetFast(verts);
				autoCollider.UpdateHardTransformPositionFast(verts, norms);
			}
		}
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x0014ABA0 File Offset: 0x00148FA0
	public void UpdateAnchorsThreadedFast(Vector3[] verts, Vector3[] norms)
	{
		if (this._autoColliders != null && this._autoColliders.Length > 0)
		{
			foreach (AutoCollider autoCollider in this._autoColliders)
			{
				if (autoCollider.centerJoint)
				{
					if (autoCollider.lookAtOption == AutoCollider.LookAtOption.Opposite && autoCollider.oppositeVertex != -1)
					{
						autoCollider.anchorTarget = (verts[autoCollider.targetVertex] + verts[autoCollider.oppositeVertex]) * 0.5f;
					}
					else if (autoCollider.lookAtOption == AutoCollider.LookAtOption.AnchorCenters && autoCollider.anchorVertex1 != -1 && autoCollider.anchorVertex2 != -1)
					{
						autoCollider.anchorTarget = (verts[autoCollider.anchorVertex1] + verts[autoCollider.anchorVertex2]) * 0.5f;
					}
					else if (autoCollider.lookAtOption == AutoCollider.LookAtOption.VertexNormal)
					{
						if (autoCollider.colliderOrient == AutoCollider.ColliderOrient.Look)
						{
							float num = autoCollider.colliderLength * 0.5f * autoCollider.scale;
							if (num < autoCollider.colliderRadius * autoCollider.scale)
							{
								num = autoCollider.colliderRadius * autoCollider.scale;
							}
							autoCollider.anchorTarget = verts[autoCollider.targetVertex] + norms[autoCollider.targetVertex] * -num;
						}
						else
						{
							autoCollider.anchorTarget = verts[autoCollider.targetVertex] + norms[autoCollider.targetVertex] * -autoCollider.colliderRadius * autoCollider.scale;
						}
					}
				}
				else
				{
					autoCollider.anchorTarget = verts[autoCollider.targetVertex];
				}
				if (autoCollider.bone != null)
				{
					Vector3 a = autoCollider.bone.worldToLocalMatrix.MultiplyPoint3x4(autoCollider.anchorTarget);
					float sqrMagnitude = (a - autoCollider.transformedAnchorTarget).sqrMagnitude;
					if (sqrMagnitude >= 4E-06f)
					{
						autoCollider.transformedAnchorTarget = autoCollider.bone.worldToLocalMatrix.MultiplyPoint3x4(autoCollider.anchorTarget);
						autoCollider.transformedAnchorTargetDirty = true;
					}
					else
					{
						autoCollider.transformedAnchorTargetDirty = false;
					}
				}
			}
		}
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x0014AE04 File Offset: 0x00149204
	public void CheckPhysicsCorruption()
	{
		if (this._autoCollidersWithJointsAndBones != null && this._autoCollidersWithJointsAndBones.Length > 0)
		{
			AutoCollider autoCollider = this._autoCollidersWithJointsAndBones[0];
			Vector3 position = autoCollider.jointTransform.position;
			if (!NaNUtils.IsVector3Valid(position) && this.containingAtom != null)
			{
				this.containingAtom.AlertPhysicsCorruption("AutoCollider invalid joint position");
			}
		}
	}

	// Token: 0x060046FF RID: 18175 RVA: 0x0014AE6C File Offset: 0x0014926C
	public void UpdateThreadedFinish(Vector3[] verts, Vector3[] norms)
	{
		if (this._resetSimulation)
		{
			foreach (AutoCollider autoCollider in this._autoColliders)
			{
				if (autoCollider.joint != null)
				{
					if (autoCollider.bone != null)
					{
						autoCollider.joint.connectedAnchor = autoCollider.transformedAnchorTarget;
					}
					else
					{
						autoCollider.joint.connectedAnchor = autoCollider.backForceRigidbody.transform.InverseTransformPoint(autoCollider.anchorTarget);
					}
					autoCollider.ResetJointPhysics();
				}
				if (autoCollider.colliderDirty)
				{
					autoCollider.AutoColliderSizeSetFinishFast();
				}
			}
		}
		else
		{
			foreach (AutoCollider autoCollider2 in this._autoCollidersWithJointsAndBones)
			{
				if (autoCollider2.transformedAnchorTargetDirty)
				{
					autoCollider2.joint.connectedAnchor = autoCollider2.transformedAnchorTarget;
				}
				if (autoCollider2.colliderDirty)
				{
					autoCollider2.AutoColliderSizeSetFinishFast();
				}
			}
			foreach (AutoCollider autoCollider3 in this._autoCollidersWithJoints)
			{
				autoCollider3.joint.connectedAnchor = autoCollider3.backForceRigidbody.transform.InverseTransformPoint(autoCollider3.anchorTarget);
				if (autoCollider3.colliderDirty)
				{
					autoCollider3.AutoColliderSizeSetFinishFast();
				}
			}
			foreach (AutoCollider autoCollider4 in this._autoCollidersWithoutJoints)
			{
				if (autoCollider4.colliderDirty)
				{
					autoCollider4.AutoColliderSizeSetFinishFast();
				}
			}
		}
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x0014B008 File Offset: 0x00149408
	protected void UpdateAnchors()
	{
		if (this._autoColliders != null && this._autoColliders.Length > 0 && this.skin != null)
		{
			Vector3[] rawSkinnedVerts = this.skin.rawSkinnedVerts;
			Vector3[] postSkinNormals = this.skin.postSkinNormals;
			foreach (AutoCollider autoCollider in this._autoColliders)
			{
				if (autoCollider.joint != null && !this._resetSimulation)
				{
					if (autoCollider.centerJoint)
					{
						if (autoCollider.lookAtOption == AutoCollider.LookAtOption.Opposite && autoCollider.oppositeVertex != -1)
						{
							autoCollider.anchorTarget = (rawSkinnedVerts[autoCollider.targetVertex] + rawSkinnedVerts[autoCollider.oppositeVertex]) * 0.5f;
							autoCollider.joint.connectedAnchor = autoCollider.backForceRigidbody.transform.InverseTransformPoint(autoCollider.anchorTarget);
						}
						else if (autoCollider.lookAtOption == AutoCollider.LookAtOption.AnchorCenters && autoCollider.anchorVertex1 != -1 && autoCollider.anchorVertex2 != -1)
						{
							autoCollider.anchorTarget = (rawSkinnedVerts[autoCollider.anchorVertex1] + rawSkinnedVerts[autoCollider.anchorVertex2]) * 0.5f;
							autoCollider.joint.connectedAnchor = autoCollider.backForceRigidbody.transform.InverseTransformPoint(autoCollider.anchorTarget);
						}
						else if (autoCollider.lookAtOption == AutoCollider.LookAtOption.VertexNormal)
						{
							if (autoCollider.colliderOrient == AutoCollider.ColliderOrient.Look)
							{
								float num = autoCollider.colliderLength * 0.5f;
								if (num < autoCollider.colliderRadius)
								{
									num = autoCollider.colliderRadius;
								}
								autoCollider.anchorTarget = rawSkinnedVerts[autoCollider.targetVertex] + postSkinNormals[autoCollider.targetVertex] * -num;
								autoCollider.joint.connectedAnchor = autoCollider.backForceRigidbody.transform.InverseTransformPoint(autoCollider.anchorTarget);
							}
							else
							{
								autoCollider.anchorTarget = rawSkinnedVerts[autoCollider.targetVertex] + postSkinNormals[autoCollider.targetVertex] * -autoCollider.colliderRadius;
								autoCollider.joint.connectedAnchor = autoCollider.backForceRigidbody.transform.InverseTransformPoint(autoCollider.anchorTarget);
							}
						}
					}
					else
					{
						autoCollider.anchorTarget = rawSkinnedVerts[autoCollider.targetVertex];
						autoCollider.joint.connectedAnchor = autoCollider.backForceRigidbody.transform.InverseTransformPoint(autoCollider.anchorTarget);
					}
				}
				if (autoCollider.debug)
				{
					MyDebug.DrawWireCube(autoCollider.anchorTarget, 0.005f, Color.blue);
				}
			}
		}
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x0014B2E4 File Offset: 0x001496E4
	protected void ResetJointPhysics()
	{
		this.UpdateAnchors();
		if (this._autoColliders != null && this._autoColliders.Length > 0)
		{
			foreach (AutoCollider autoCollider in this._autoColliders)
			{
				autoCollider.ResetJointPhysics();
			}
		}
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x0014B338 File Offset: 0x00149738
	private void OnEnable()
	{
		this.UpdateAutoColliders();
		foreach (AutoCollider autoCollider in this._autoColliders)
		{
			autoCollider.enabled = false;
			autoCollider.Init();
		}
		this.isEnabled = true;
		this.enableResetFlag = new AsyncFlag("EnableResetFlag");
		this.ResetSimulation(this.enableResetFlag);
		this.pauseCountdown = 10;
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x0014B3A4 File Offset: 0x001497A4
	private void OnDisable()
	{
		this.isEnabled = false;
		foreach (AutoCollider autoCollider in this._autoColliders)
		{
			autoCollider.enabled = true;
		}
		if (this.enableResetFlag != null)
		{
			this.enableResetFlag.Raise();
			this.pauseCountdown = 0;
		}
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x0014B3FC File Offset: 0x001497FC
	private void Update()
	{
		if (Application.isPlaying)
		{
			if (this.enableResetFlag != null && !this.enableResetFlag.Raised)
			{
				this.pauseCountdown--;
				if (this.pauseCountdown <= 0)
				{
					this.enableResetFlag.Raise();
				}
			}
			this.CheckResumeSimulation();
		}
		if (this.skin != null && (this.skin.dazMesh.visibleNonPoseVerticesChangedLastFrame || this.skin.dazMesh.visibleNonPoseVerticesChangedThisFrame))
		{
			this.morphsChanged = true;
		}
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x0014B49C File Offset: 0x0014989C
	private void FixedUpdate()
	{
		if (this._on && this.clumpUpdate)
		{
			if (this.resetSimulation)
			{
				this.ResetJointPhysics();
			}
			else
			{
				this.UpdateAnchors();
				if (this.morphsChanged)
				{
					this.morphsChanged = false;
					foreach (AutoCollider autoCollider in this._autoColliders)
					{
						autoCollider.AutoColliderSizeSet(false);
					}
				}
			}
		}
	}

	// Token: 0x04003455 RID: 13397
	protected AutoCollider[] _autoColliders;

	// Token: 0x04003456 RID: 13398
	protected AutoCollider[] _autoCollidersWithJointsAndBones;

	// Token: 0x04003457 RID: 13399
	protected AutoCollider[] _autoCollidersWithJoints;

	// Token: 0x04003458 RID: 13400
	protected AutoCollider[] _autoCollidersWithoutJoints;

	// Token: 0x04003459 RID: 13401
	public bool clumpUpdate;

	// Token: 0x0400345A RID: 13402
	public DAZSkinV2 skin;

	// Token: 0x0400345B RID: 13403
	public int numControlledColliders;

	// Token: 0x0400345C RID: 13404
	protected bool _on = true;

	// Token: 0x0400345D RID: 13405
	private bool _resetSimulation;

	// Token: 0x0400345E RID: 13406
	protected List<AsyncFlag> waitResumeSimulationFlags;

	// Token: 0x0400345F RID: 13407
	protected const float squaredThreshold = 4E-06f;

	// Token: 0x04003460 RID: 13408
	protected bool morphsChanged;

	// Token: 0x04003461 RID: 13409
	public bool isEnabled;

	// Token: 0x04003462 RID: 13410
	protected AsyncFlag enableResetFlag;

	// Token: 0x04003463 RID: 13411
	protected int pauseCountdown;
}
