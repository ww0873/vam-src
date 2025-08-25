using System;
using UnityEngine;

// Token: 0x020008BE RID: 2238
public abstract class OVRComposition
{
	// Token: 0x06003850 RID: 14416 RVA: 0x00111160 File Offset: 0x0010F560
	protected OVRComposition()
	{
	}

	// Token: 0x06003851 RID: 14417
	public abstract OVRManager.CompositionMethod CompositionMethod();

	// Token: 0x06003852 RID: 14418
	public abstract void Update(Camera mainCamera);

	// Token: 0x06003853 RID: 14419
	public abstract void Cleanup();

	// Token: 0x06003854 RID: 14420 RVA: 0x00111182 File Offset: 0x0010F582
	public virtual void RecenterPose()
	{
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x00111184 File Offset: 0x0010F584
	internal OVRPose ComputeCameraWorldSpacePose(OVRPlugin.CameraExtrinsics extrinsics)
	{
		OVRPose ovrpose = default(OVRPose);
		OVRPose ovrpose2 = default(OVRPose);
		OVRPose ovrpose3 = extrinsics.RelativePose.ToOVRPose();
		ovrpose2 = ovrpose3;
		if (extrinsics.AttachedToNode != OVRPlugin.Node.None && OVRPlugin.GetNodePresent(extrinsics.AttachedToNode))
		{
			if (this.usingLastAttachedNodePose)
			{
				Debug.Log("The camera attached node get tracked");
				this.usingLastAttachedNodePose = false;
			}
			OVRPose lhs = OVRPlugin.GetNodePose(extrinsics.AttachedToNode, OVRPlugin.Step.Render).ToOVRPose();
			this.lastAttachedNodePose = lhs;
			ovrpose2 = lhs * ovrpose2;
		}
		else if (extrinsics.AttachedToNode != OVRPlugin.Node.None)
		{
			if (!this.usingLastAttachedNodePose)
			{
				Debug.LogWarning("The camera attached node could not be tracked, using the last pose");
				this.usingLastAttachedNodePose = true;
			}
			ovrpose2 = this.lastAttachedNodePose * ovrpose2;
		}
		return OVRExtensions.ToWorldSpacePose(ovrpose2);
	}

	// Token: 0x040029A1 RID: 10657
	protected bool usingLastAttachedNodePose;

	// Token: 0x040029A2 RID: 10658
	protected OVRPose lastAttachedNodePose = default(OVRPose);
}
