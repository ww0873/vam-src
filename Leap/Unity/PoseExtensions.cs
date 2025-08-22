using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006B7 RID: 1719
	public static class PoseExtensions
	{
		// Token: 0x0600296B RID: 10603 RVA: 0x000E1632 File Offset: 0x000DFA32
		public static Pose ToLocalPose(this Transform t)
		{
			return new Pose(t.localPosition, t.localRotation);
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x000E1645 File Offset: 0x000DFA45
		public static Pose ToPose(this Transform t)
		{
			return new Pose(t.position, t.rotation);
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000E1658 File Offset: 0x000DFA58
		public static Pose ToWorldPose(this Transform t)
		{
			return t.ToPose();
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000E1660 File Offset: 0x000DFA60
		public static void SetLocalPose(this Transform t, Pose localPose)
		{
			t.localPosition = localPose.position;
			t.localRotation = localPose.rotation;
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000E167C File Offset: 0x000DFA7C
		public static void SetPose(this Transform t, Pose worldPose)
		{
			t.position = worldPose.position;
			t.rotation = worldPose.rotation;
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000E1698 File Offset: 0x000DFA98
		public static void SetWorldPose(this Transform t, Pose worldPose)
		{
			t.SetPose(worldPose);
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000E16A4 File Offset: 0x000DFAA4
		public static Pose GetPose(this Matrix4x4 m)
		{
			return new Pose(m.GetColumn(3), (!(m.GetColumn(2) == m.GetColumn(1))) ? Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1)) : Quaternion.identity);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000E1706 File Offset: 0x000DFB06
		public static Pose WithRotation(this Pose pose, Quaternion newRotation)
		{
			return new Pose(pose.position, newRotation);
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000E1715 File Offset: 0x000DFB15
		public static Pose WithPosition(this Pose pose, Vector3 newPosition)
		{
			return new Pose(newPosition, pose.rotation);
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000E1724 File Offset: 0x000DFB24
		public static Vector3 GetVector3(this Matrix4x4 m)
		{
			return m.GetColumn(3);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000E1734 File Offset: 0x000DFB34
		public static Quaternion GetQuaternion(this Matrix4x4 m)
		{
			if (m.GetColumn(2) == m.GetColumn(1))
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000E1780 File Offset: 0x000DFB80
		public static bool ApproxEquals(this Vector3 v0, Vector3 v1)
		{
			return (v0 - v1).magnitude < 0.0001f;
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000E17A4 File Offset: 0x000DFBA4
		public static bool ApproxEquals(this Quaternion q0, Quaternion q1)
		{
			return (q0.ToAngleAxisVector() - q1.ToAngleAxisVector()).magnitude < 0.0001f;
		}

		// Token: 0x040021EF RID: 8687
		public const float EPSILON = 0.0001f;
	}
}
