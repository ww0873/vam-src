using System;
using UnityEngine;

// Token: 0x02000959 RID: 2393
public class OVRTracker
{
	// Token: 0x06003B5F RID: 15199 RVA: 0x0011E2F1 File Offset: 0x0011C6F1
	public OVRTracker()
	{
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06003B60 RID: 15200 RVA: 0x0011E2F9 File Offset: 0x0011C6F9
	public bool isPresent
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.positionSupported;
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06003B61 RID: 15201 RVA: 0x0011E30C File Offset: 0x0011C70C
	public bool isPositionTracked
	{
		get
		{
			return OVRPlugin.positionTracked;
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06003B62 RID: 15202 RVA: 0x0011E313 File Offset: 0x0011C713
	// (set) Token: 0x06003B63 RID: 15203 RVA: 0x0011E326 File Offset: 0x0011C726
	public bool isEnabled
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.position;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.position = value;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06003B64 RID: 15204 RVA: 0x0011E33C File Offset: 0x0011C73C
	public int count
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if (this.GetPresent(i))
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x0011E370 File Offset: 0x0011C770
	public OVRTracker.Frustum GetFrustum(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return default(OVRTracker.Frustum);
		}
		return OVRPlugin.GetTrackerFrustum((OVRPlugin.Tracker)tracker).ToFrustum();
	}

	// Token: 0x06003B66 RID: 15206 RVA: 0x0011E39C File Offset: 0x0011C79C
	public OVRPose GetPose(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return OVRPose.identity;
		}
		OVRPose ovrpose;
		switch (tracker)
		{
		case 0:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerZero, OVRPlugin.Step.Render).ToOVRPose();
			break;
		case 1:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerOne, OVRPlugin.Step.Render).ToOVRPose();
			break;
		case 2:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerTwo, OVRPlugin.Step.Render).ToOVRPose();
			break;
		case 3:
			ovrpose = OVRPlugin.GetNodePose(OVRPlugin.Node.TrackerThree, OVRPlugin.Step.Render).ToOVRPose();
			break;
		default:
			return OVRPose.identity;
		}
		return new OVRPose
		{
			position = ovrpose.position,
			orientation = ovrpose.orientation * Quaternion.Euler(0f, 180f, 0f)
		};
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x0011E460 File Offset: 0x0011C860
	public bool GetPoseValid(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return false;
		}
		switch (tracker)
		{
		case 0:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerZero);
		case 1:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerOne);
		case 2:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerTwo);
		case 3:
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.TrackerThree);
		default:
			return false;
		}
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x0011E4B4 File Offset: 0x0011C8B4
	public bool GetPresent(int tracker = 0)
	{
		if (!OVRManager.isHmdPresent)
		{
			return false;
		}
		switch (tracker)
		{
		case 0:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerZero);
		case 1:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerOne);
		case 2:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerTwo);
		case 3:
			return OVRPlugin.GetNodePresent(OVRPlugin.Node.TrackerThree);
		default:
			return false;
		}
	}

	// Token: 0x0200095A RID: 2394
	public struct Frustum
	{
		// Token: 0x04002D4C RID: 11596
		public float nearZ;

		// Token: 0x04002D4D RID: 11597
		public float farZ;

		// Token: 0x04002D4E RID: 11598
		public Vector2 fov;
	}
}
