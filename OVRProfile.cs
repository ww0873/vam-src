using System;
using UnityEngine;

// Token: 0x02000957 RID: 2391
public class OVRProfile : UnityEngine.Object
{
	// Token: 0x06003B56 RID: 15190 RVA: 0x0011E27A File Offset: 0x0011C67A
	public OVRProfile()
	{
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06003B57 RID: 15191 RVA: 0x0011E282 File Offset: 0x0011C682
	[Obsolete]
	public string id
	{
		get
		{
			return "000abc123def";
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06003B58 RID: 15192 RVA: 0x0011E289 File Offset: 0x0011C689
	[Obsolete]
	public string userName
	{
		get
		{
			return "Oculus User";
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06003B59 RID: 15193 RVA: 0x0011E290 File Offset: 0x0011C690
	[Obsolete]
	public string locale
	{
		get
		{
			return "en_US";
		}
	}

	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x06003B5A RID: 15194 RVA: 0x0011E298 File Offset: 0x0011C698
	public float ipd
	{
		get
		{
			return Vector3.Distance(OVRPlugin.GetNodePose(OVRPlugin.Node.EyeLeft, OVRPlugin.Step.Render).ToOVRPose().position, OVRPlugin.GetNodePose(OVRPlugin.Node.EyeRight, OVRPlugin.Step.Render).ToOVRPose().position);
		}
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x06003B5B RID: 15195 RVA: 0x0011E2D2 File Offset: 0x0011C6D2
	public float eyeHeight
	{
		get
		{
			return OVRPlugin.eyeHeight;
		}
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06003B5C RID: 15196 RVA: 0x0011E2D9 File Offset: 0x0011C6D9
	public float eyeDepth
	{
		get
		{
			return OVRPlugin.eyeDepth;
		}
	}

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06003B5D RID: 15197 RVA: 0x0011E2E0 File Offset: 0x0011C6E0
	public float neckHeight
	{
		get
		{
			return this.eyeHeight - 0.075f;
		}
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06003B5E RID: 15198 RVA: 0x0011E2EE File Offset: 0x0011C6EE
	[Obsolete]
	public OVRProfile.State state
	{
		get
		{
			return OVRProfile.State.READY;
		}
	}

	// Token: 0x02000958 RID: 2392
	[Obsolete]
	public enum State
	{
		// Token: 0x04002D48 RID: 11592
		NOT_TRIGGERED,
		// Token: 0x04002D49 RID: 11593
		LOADING,
		// Token: 0x04002D4A RID: 11594
		READY,
		// Token: 0x04002D4B RID: 11595
		ERROR
	}
}
