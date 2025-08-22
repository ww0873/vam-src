using System;
using UnityEngine;

// Token: 0x02000BD0 RID: 3024
public class DebugQuaternion2Angles : MonoBehaviour
{
	// Token: 0x060055E3 RID: 21987 RVA: 0x001F666A File Offset: 0x001F4A6A
	public DebugQuaternion2Angles()
	{
	}

	// Token: 0x060055E4 RID: 21988 RVA: 0x001F667C File Offset: 0x001F4A7C
	private void rotateTransform(Transform t, float xr, float yr, float zr)
	{
		t.localRotation = Quaternion.identity;
		switch (this.rotationOrder)
		{
		case Quaternion2Angles.RotationOrder.XYZ:
			t.Rotate(xr, 0f, 0f);
			t.Rotate(0f, yr, 0f);
			t.Rotate(0f, 0f, zr);
			break;
		case Quaternion2Angles.RotationOrder.XZY:
			t.Rotate(xr, 0f, 0f);
			t.Rotate(0f, 0f, zr);
			t.Rotate(0f, yr, 0f);
			break;
		case Quaternion2Angles.RotationOrder.YXZ:
			t.Rotate(0f, yr, 0f);
			t.Rotate(xr, 0f, 0f);
			t.Rotate(0f, 0f, zr);
			break;
		case Quaternion2Angles.RotationOrder.YZX:
			t.Rotate(0f, yr, 0f);
			t.Rotate(0f, 0f, zr);
			t.Rotate(xr, 0f, 0f);
			break;
		case Quaternion2Angles.RotationOrder.ZXY:
			t.Rotate(0f, 0f, zr);
			t.Rotate(xr, 0f, 0f);
			t.Rotate(0f, yr, 0f);
			break;
		case Quaternion2Angles.RotationOrder.ZYX:
			t.Rotate(0f, 0f, zr);
			t.Rotate(0f, yr, 0f);
			t.Rotate(xr, 0f, 0f);
			break;
		}
	}

	// Token: 0x060055E5 RID: 21989 RVA: 0x001F681C File Offset: 0x001F4C1C
	private void LateUpdate()
	{
		if (this.set)
		{
			this.set = false;
			this.rotateTransform(base.transform, this.xrotset, this.yrotset, this.zrotset);
		}
		Quaternion localRotation = base.transform.localRotation;
		this.qx = localRotation.x;
		this.qy = localRotation.y;
		this.qz = localRotation.z;
		this.qw = localRotation.w;
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		if (localEulerAngles.x > 180f)
		{
			localEulerAngles.x -= 360f;
		}
		if (localEulerAngles.y > 180f)
		{
			localEulerAngles.y -= 360f;
		}
		if (localEulerAngles.z > 180f)
		{
			localEulerAngles.z -= 360f;
		}
		this.ex = localEulerAngles.x;
		this.ey = localEulerAngles.y;
		this.ez = localEulerAngles.z;
		Vector3 a = Quaternion2Angles.GetAngles(localRotation, this.rotationOrder);
		if (this.displayType == DebugQuaternion2Angles.DisplayType.Degrees)
		{
			a *= 57.29578f;
		}
		this.xrot = a.x;
		this.yrot = a.y;
		this.zrot = a.z;
		if (this.setOther)
		{
			this.rotateTransform(this.setOther, this.xrot, this.yrot, this.zrot);
		}
	}

	// Token: 0x04004701 RID: 18177
	public Quaternion2Angles.RotationOrder rotationOrder;

	// Token: 0x04004702 RID: 18178
	public DebugQuaternion2Angles.DisplayType displayType = DebugQuaternion2Angles.DisplayType.Degrees;

	// Token: 0x04004703 RID: 18179
	public float xrotset;

	// Token: 0x04004704 RID: 18180
	public float yrotset;

	// Token: 0x04004705 RID: 18181
	public float zrotset;

	// Token: 0x04004706 RID: 18182
	public bool set;

	// Token: 0x04004707 RID: 18183
	public Transform setOther;

	// Token: 0x04004708 RID: 18184
	public float xrot;

	// Token: 0x04004709 RID: 18185
	public float yrot;

	// Token: 0x0400470A RID: 18186
	public float zrot;

	// Token: 0x0400470B RID: 18187
	public float qx;

	// Token: 0x0400470C RID: 18188
	public float qy;

	// Token: 0x0400470D RID: 18189
	public float qz;

	// Token: 0x0400470E RID: 18190
	public float qw;

	// Token: 0x0400470F RID: 18191
	public float ex;

	// Token: 0x04004710 RID: 18192
	public float ey;

	// Token: 0x04004711 RID: 18193
	public float ez;

	// Token: 0x02000BD1 RID: 3025
	public enum DisplayType
	{
		// Token: 0x04004713 RID: 18195
		Radians,
		// Token: 0x04004714 RID: 18196
		Degrees
	}
}
