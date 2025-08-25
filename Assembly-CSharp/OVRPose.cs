using System;
using UnityEngine;

// Token: 0x020008CD RID: 2253
[Serializable]
public struct OVRPose
{
	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x060038AF RID: 14511 RVA: 0x0011441C File Offset: 0x0011281C
	public static OVRPose identity
	{
		get
		{
			return new OVRPose
			{
				position = Vector3.zero,
				orientation = Quaternion.identity
			};
		}
	}

	// Token: 0x060038B0 RID: 14512 RVA: 0x0011444A File Offset: 0x0011284A
	public override bool Equals(object obj)
	{
		return obj is OVRPose && this == (OVRPose)obj;
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x0011446B File Offset: 0x0011286B
	public override int GetHashCode()
	{
		return this.position.GetHashCode() ^ this.orientation.GetHashCode();
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x00114490 File Offset: 0x00112890
	public static bool operator ==(OVRPose x, OVRPose y)
	{
		return x.position == y.position && x.orientation == y.orientation;
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x001144C0 File Offset: 0x001128C0
	public static bool operator !=(OVRPose x, OVRPose y)
	{
		return !(x == y);
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x001144CC File Offset: 0x001128CC
	public static OVRPose operator *(OVRPose lhs, OVRPose rhs)
	{
		return new OVRPose
		{
			position = lhs.position + lhs.orientation * rhs.position,
			orientation = lhs.orientation * rhs.orientation
		};
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x00114524 File Offset: 0x00112924
	public OVRPose Inverse()
	{
		OVRPose result;
		result.orientation = Quaternion.Inverse(this.orientation);
		result.position = result.orientation * -this.position;
		return result;
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x00114564 File Offset: 0x00112964
	internal OVRPose flipZ()
	{
		OVRPose result = this;
		result.position.z = -result.position.z;
		result.orientation.z = -result.orientation.z;
		result.orientation.w = -result.orientation.w;
		return result;
	}

	// Token: 0x060038B7 RID: 14519 RVA: 0x001145C4 File Offset: 0x001129C4
	internal OVRPlugin.Posef ToPosef()
	{
		return new OVRPlugin.Posef
		{
			Position = this.position.ToVector3f(),
			Orientation = this.orientation.ToQuatf()
		};
	}

	// Token: 0x040029E2 RID: 10722
	public Vector3 position;

	// Token: 0x040029E3 RID: 10723
	public Quaternion orientation;
}
