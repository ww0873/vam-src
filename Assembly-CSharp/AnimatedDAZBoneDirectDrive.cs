using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000A97 RID: 2711
public class AnimatedDAZBoneDirectDrive : MonoBehaviour
{
	// Token: 0x06004663 RID: 18019 RVA: 0x001411AB File Offset: 0x0013F5AB
	public AnimatedDAZBoneDirectDrive()
	{
	}

	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x06004664 RID: 18020 RVA: 0x001411D4 File Offset: 0x0013F5D4
	// (set) Token: 0x06004665 RID: 18021 RVA: 0x001411DC File Offset: 0x0013F5DC
	public Transform orientationTransform
	{
		[CompilerGenerated]
		get
		{
			return this.<orientationTransform>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<orientationTransform>k__BackingField = value;
		}
	}

	// Token: 0x06004666 RID: 18022 RVA: 0x001411E8 File Offset: 0x0013F5E8
	public void Init()
	{
		if (this.dazBone != null)
		{
			this.cj = this.dazBone.GetComponent<ConfigurableJoint>();
			if (this.cj != null)
			{
				this.jointAxis = this.cj.axis;
				this.jointSecondaryAxis = this.cj.secondaryAxis;
			}
			GameObject gameObject = new GameObject(base.name + "_orientation");
			this.orientationTransform = gameObject.transform;
			this.orientationTransform.SetParent(base.transform);
			this.orientationTransform.localPosition = Vector3.zero;
			this.dazBone.Init();
			Quaternion rotation = base.transform.rotation;
			base.transform.rotation = Quaternion.identity;
			this.orientationTransform.rotation = this.dazBone.startingRotationRelativeToRoot;
			base.transform.rotation = rotation;
		}
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x001412D8 File Offset: 0x0013F6D8
	public void InitParent()
	{
		AnimatedDAZBoneDirectDrive component = base.transform.parent.GetComponent<AnimatedDAZBoneDirectDrive>();
		if (component != null)
		{
			this.parentOrientationTransform = component.orientationTransform;
		}
		else
		{
			AnimatedDAZBoneMoveProducer component2 = base.transform.parent.GetComponent<AnimatedDAZBoneMoveProducer>();
			if (component2 != null)
			{
				this.parentOrientationTransform = component2.orientationTransform;
			}
		}
		if (this.parentOrientationTransform == null)
		{
			UnityEngine.Debug.LogError("Could not find parent orientation transform for " + base.name);
			this.parentOrientationTransform = base.transform.parent;
		}
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x00141373 File Offset: 0x0013F773
	public void RestoreBoneControl()
	{
		if (this.dazBoneJointRotationDisabled)
		{
			this.dazBoneJointRotationDisabled = false;
			this.dazBone.jointRotationDisabled = false;
		}
	}

	// Token: 0x06004669 RID: 18025 RVA: 0x00141394 File Offset: 0x0013F794
	public void Prep()
	{
		if (this.orientationTransform != null)
		{
			this.orientationTransformRotation = this.orientationTransform.rotation;
		}
		if (this.parentOrientationTransform != null)
		{
			this.parentOrientationTransformRotation = this.parentOrientationTransform.rotation;
		}
	}

	// Token: 0x0600466A RID: 18026 RVA: 0x001413E8 File Offset: 0x0013F7E8
	public void ThreadedUpdate()
	{
		Quaternion q = this.dazBone.inverseStartingLocalRotation * Quaternion.Inverse(this.parentOrientationTransformRotation) * this.orientationTransformRotation;
		this.localRotationEuler = Quaternion2Angles.GetAngles(q, Quaternion2Angles.RotationOrder.XYZ) * 57.29578f;
		this.localRotationEuler.x = this.localRotationEuler.x * this.multiplier.x;
		this.localRotationEuler.y = this.localRotationEuler.y * this.multiplier.y;
		this.localRotationEuler.z = this.localRotationEuler.z * this.multiplier.z;
		if (this.jointAxis.x == 1f)
		{
			this.applyRotation.x = this.localRotationEuler.x;
			if (this.jointSecondaryAxis.y == 1f)
			{
				this.applyRotation.y = this.localRotationEuler.y;
				this.applyRotation.z = this.localRotationEuler.z;
			}
			else
			{
				this.applyRotation.y = this.localRotationEuler.z;
				this.applyRotation.z = this.localRotationEuler.y;
			}
		}
		else if (this.jointAxis.y == 1f)
		{
			this.applyRotation.x = this.localRotationEuler.y;
			if (this.jointSecondaryAxis.x == 1f)
			{
				this.applyRotation.y = this.localRotationEuler.x;
				this.applyRotation.z = this.localRotationEuler.z;
			}
			else
			{
				this.applyRotation.y = this.localRotationEuler.z;
				this.applyRotation.z = -this.localRotationEuler.x;
			}
		}
		else
		{
			this.applyRotation.x = this.localRotationEuler.z;
			if (this.jointSecondaryAxis.x == 1f)
			{
				this.applyRotation.y = this.localRotationEuler.x;
				this.applyRotation.z = this.localRotationEuler.y;
			}
			else
			{
				this.applyRotation.y = this.localRotationEuler.y;
				this.applyRotation.z = this.localRotationEuler.x;
			}
		}
		this.jointTargetRotation = Quaternion2Angles.EulerToQuaternion(this.applyRotation, this.outputRotationOrder);
	}

	// Token: 0x0600466B RID: 18027 RVA: 0x00141670 File Offset: 0x0013FA70
	public void Finish()
	{
		if (this.dazBone != null && this.cj != null)
		{
			this.dazBoneJointRotationDisabled = true;
			this.dazBone.jointRotationDisabled = true;
			this.cj.targetRotation = this.jointTargetRotation;
		}
	}

	// Token: 0x040033C5 RID: 13253
	public DAZBone dazBone;

	// Token: 0x040033C6 RID: 13254
	public Quaternion2Angles.RotationOrder outputRotationOrder;

	// Token: 0x040033C7 RID: 13255
	public Vector3 multiplier = Vector3.one;

	// Token: 0x040033C8 RID: 13256
	public Vector3 localRotationEuler;

	// Token: 0x040033C9 RID: 13257
	public Vector3 applyRotation;

	// Token: 0x040033CA RID: 13258
	protected Transform parentOrientationTransform;

	// Token: 0x040033CB RID: 13259
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <orientationTransform>k__BackingField;

	// Token: 0x040033CC RID: 13260
	protected ConfigurableJoint cj;

	// Token: 0x040033CD RID: 13261
	protected Vector3 jointAxis;

	// Token: 0x040033CE RID: 13262
	protected Vector3 jointSecondaryAxis;

	// Token: 0x040033CF RID: 13263
	protected bool dazBoneJointRotationDisabled;

	// Token: 0x040033D0 RID: 13264
	protected Quaternion orientationTransformRotation = Quaternion.identity;

	// Token: 0x040033D1 RID: 13265
	protected Quaternion parentOrientationTransformRotation = Quaternion.identity;

	// Token: 0x040033D2 RID: 13266
	protected Quaternion jointTargetRotation;
}
