using System;
using UnityEngine;

// Token: 0x02000770 RID: 1904
public class PoseEditHelper : MonoBehaviour
{
	// Token: 0x06003123 RID: 12579 RVA: 0x000FF79D File Offset: 0x000FDB9D
	public PoseEditHelper()
	{
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x000FF7A5 File Offset: 0x000FDBA5
	private void OnDrawGizmos()
	{
		if (this.poseRoot != null)
		{
			this.DrawJoints(this.poseRoot);
		}
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x000FF7C4 File Offset: 0x000FDBC4
	private void DrawJoints(Transform joint)
	{
		Gizmos.DrawWireSphere(joint.position, 0.005f);
		for (int i = 0; i < joint.childCount; i++)
		{
			Transform child = joint.GetChild(i);
			if (!child.name.EndsWith("_grip") && !child.name.EndsWith("hand_ignore"))
			{
				Gizmos.DrawLine(joint.position, child.position);
				this.DrawJoints(child);
			}
		}
	}

	// Token: 0x040024E4 RID: 9444
	public Transform poseRoot;
}
