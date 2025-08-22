using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020008CC RID: 2252
public static class OVRExtensions
{
	// Token: 0x0600389E RID: 14494 RVA: 0x00113EF0 File Offset: 0x001122F0
	public static OVRPose ToTrackingSpacePose(this Transform transform, Camera camera)
	{
		OVRPose lhs;
		lhs.position = InputTracking.GetLocalPosition(XRNode.Head);
		lhs.orientation = InputTracking.GetLocalRotation(XRNode.Head);
		return lhs * transform.ToHeadSpacePose(camera);
	}

	// Token: 0x0600389F RID: 14495 RVA: 0x00113F28 File Offset: 0x00112328
	public static OVRPose ToWorldSpacePose(OVRPose trackingSpacePose)
	{
		OVRPose ovrpose;
		ovrpose.position = InputTracking.GetLocalPosition(XRNode.Head);
		ovrpose.orientation = InputTracking.GetLocalRotation(XRNode.Head);
		OVRPose rhs = ovrpose.Inverse() * trackingSpacePose;
		return Camera.main.transform.ToOVRPose(false) * rhs;
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x00113F78 File Offset: 0x00112378
	public static OVRPose ToHeadSpacePose(this Transform transform, Camera camera)
	{
		return camera.transform.ToOVRPose(false).Inverse() * transform.ToOVRPose(false);
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x00113FA8 File Offset: 0x001123A8
	internal static OVRPose ToOVRPose(this Transform t, bool isLocal = false)
	{
		OVRPose result;
		result.orientation = ((!isLocal) ? t.rotation : t.localRotation);
		result.position = ((!isLocal) ? t.position : t.localPosition);
		return result;
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x00113FF4 File Offset: 0x001123F4
	internal static void FromOVRPose(this Transform t, OVRPose pose, bool isLocal = false)
	{
		if (isLocal)
		{
			t.localRotation = pose.orientation;
			t.localPosition = pose.position;
		}
		else
		{
			t.rotation = pose.orientation;
			t.position = pose.position;
		}
	}

	// Token: 0x060038A3 RID: 14499 RVA: 0x00114040 File Offset: 0x00112440
	internal static OVRPose ToOVRPose(this OVRPlugin.Posef p)
	{
		return new OVRPose
		{
			position = new Vector3(p.Position.x, p.Position.y, -p.Position.z),
			orientation = new Quaternion(-p.Orientation.x, -p.Orientation.y, p.Orientation.z, p.Orientation.w)
		};
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x001140C8 File Offset: 0x001124C8
	internal static OVRTracker.Frustum ToFrustum(this OVRPlugin.Frustumf f)
	{
		return new OVRTracker.Frustum
		{
			nearZ = f.zNear,
			farZ = f.zFar,
			fov = new Vector2
			{
				x = 57.29578f * f.fovX,
				y = 57.29578f * f.fovY
			}
		};
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x00114134 File Offset: 0x00112534
	internal static Color FromColorf(this OVRPlugin.Colorf c)
	{
		return new Color
		{
			r = c.r,
			g = c.g,
			b = c.b,
			a = c.a
		};
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x00114184 File Offset: 0x00112584
	internal static OVRPlugin.Colorf ToColorf(this Color c)
	{
		return new OVRPlugin.Colorf
		{
			r = c.r,
			g = c.g,
			b = c.b,
			a = c.a
		};
	}

	// Token: 0x060038A7 RID: 14503 RVA: 0x001141D4 File Offset: 0x001125D4
	internal static Vector3 FromVector3f(this OVRPlugin.Vector3f v)
	{
		return new Vector3
		{
			x = v.x,
			y = v.y,
			z = v.z
		};
	}

	// Token: 0x060038A8 RID: 14504 RVA: 0x00114214 File Offset: 0x00112614
	internal static Vector3 FromFlippedZVector3f(this OVRPlugin.Vector3f v)
	{
		return new Vector3
		{
			x = v.x,
			y = v.y,
			z = -v.z
		};
	}

	// Token: 0x060038A9 RID: 14505 RVA: 0x00114258 File Offset: 0x00112658
	internal static OVRPlugin.Vector3f ToVector3f(this Vector3 v)
	{
		return new OVRPlugin.Vector3f
		{
			x = v.x,
			y = v.y,
			z = v.z
		};
	}

	// Token: 0x060038AA RID: 14506 RVA: 0x00114298 File Offset: 0x00112698
	internal static OVRPlugin.Vector3f ToFlippedZVector3f(this Vector3 v)
	{
		return new OVRPlugin.Vector3f
		{
			x = v.x,
			y = v.y,
			z = -v.z
		};
	}

	// Token: 0x060038AB RID: 14507 RVA: 0x001142DC File Offset: 0x001126DC
	internal static Quaternion FromQuatf(this OVRPlugin.Quatf q)
	{
		return new Quaternion
		{
			x = q.x,
			y = q.y,
			z = q.z,
			w = q.w
		};
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x0011432C File Offset: 0x0011272C
	internal static Quaternion FromFlippedZQuatf(this OVRPlugin.Quatf q)
	{
		return new Quaternion
		{
			x = -q.x,
			y = -q.y,
			z = q.z,
			w = q.w
		};
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x0011437C File Offset: 0x0011277C
	internal static OVRPlugin.Quatf ToQuatf(this Quaternion q)
	{
		return new OVRPlugin.Quatf
		{
			x = q.x,
			y = q.y,
			z = q.z,
			w = q.w
		};
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x001143CC File Offset: 0x001127CC
	internal static OVRPlugin.Quatf ToFlippedZQuatf(this Quaternion q)
	{
		return new OVRPlugin.Quatf
		{
			x = -q.x,
			y = -q.y,
			z = q.z,
			w = q.w
		};
	}
}
