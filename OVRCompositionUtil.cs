using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BF RID: 2239
internal class OVRCompositionUtil
{
	// Token: 0x06003856 RID: 14422 RVA: 0x00111B1C File Offset: 0x0010FF1C
	public OVRCompositionUtil()
	{
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x00111B24 File Offset: 0x0010FF24
	public static void SafeDestroy(GameObject obj)
	{
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(obj);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x00111B41 File Offset: 0x0010FF41
	public static void SafeDestroy(ref GameObject obj)
	{
		OVRCompositionUtil.SafeDestroy(obj);
		obj = null;
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x00111B4D File Offset: 0x0010FF4D
	public static OVRPlugin.CameraDevice ConvertCameraDevice(OVRManager.CameraDevice cameraDevice)
	{
		if (cameraDevice == OVRManager.CameraDevice.WebCamera0)
		{
			return OVRPlugin.CameraDevice.WebCamera0;
		}
		if (cameraDevice == OVRManager.CameraDevice.WebCamera1)
		{
			return OVRPlugin.CameraDevice.WebCamera1;
		}
		if (cameraDevice == OVRManager.CameraDevice.ZEDCamera)
		{
			return OVRPlugin.CameraDevice.ZEDCamera;
		}
		return OVRPlugin.CameraDevice.None;
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x00111B70 File Offset: 0x0010FF70
	public static OVRBoundary.BoundaryType ToBoundaryType(OVRManager.VirtualGreenScreenType type)
	{
		if (type == OVRManager.VirtualGreenScreenType.OuterBoundary)
		{
			return OVRBoundary.BoundaryType.OuterBoundary;
		}
		if (type == OVRManager.VirtualGreenScreenType.PlayArea)
		{
			return OVRBoundary.BoundaryType.PlayArea;
		}
		Debug.LogWarning("Unmatched VirtualGreenScreenType");
		return OVRBoundary.BoundaryType.OuterBoundary;
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x00111B94 File Offset: 0x0010FF94
	public static Vector3 GetWorldPosition(Vector3 trackingSpacePosition)
	{
		OVRPose trackingSpacePose;
		trackingSpacePose.position = trackingSpacePosition;
		trackingSpacePose.orientation = Quaternion.identity;
		return OVRExtensions.ToWorldSpacePose(trackingSpacePose).position;
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x00111BC8 File Offset: 0x0010FFC8
	public static float GetMaximumBoundaryDistance(Camera camera, OVRBoundary.BoundaryType boundaryType)
	{
		if (!OVRManager.boundary.GetConfigured())
		{
			return float.MaxValue;
		}
		Vector3[] geometry = OVRManager.boundary.GetGeometry(boundaryType);
		if (geometry.Length == 0)
		{
			return float.MaxValue;
		}
		float num = float.MinValue;
		foreach (Vector3 trackingSpacePosition in geometry)
		{
			Vector3 worldPosition = OVRCompositionUtil.GetWorldPosition(trackingSpacePosition);
			float num2 = Vector3.Dot(camera.transform.forward, worldPosition);
			if (num < num2)
			{
				num = num2;
			}
		}
		return num;
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x00111C5C File Offset: 0x0011005C
	public static Mesh BuildBoundaryMesh(OVRBoundary.BoundaryType boundaryType, float topY, float bottomY)
	{
		if (!OVRManager.boundary.GetConfigured())
		{
			return null;
		}
		List<Vector3> list = new List<Vector3>(OVRManager.boundary.GetGeometry(boundaryType));
		if (list.Count == 0)
		{
			return null;
		}
		list.Add(list[0]);
		int count = list.Count;
		Vector3[] array = new Vector3[count * 2];
		Vector2[] array2 = new Vector2[count * 2];
		for (int i = 0; i < count; i++)
		{
			Vector3 vector = list[i];
			array[i] = new Vector3(vector.x, bottomY, vector.z);
			array[i + count] = new Vector3(vector.x, topY, vector.z);
			array2[i] = new Vector2((float)i / (float)(count - 1), 0f);
			array2[i + count] = new Vector2(array2[i].x, 1f);
		}
		int[] array3 = new int[(count - 1) * 2 * 3];
		for (int j = 0; j < count - 1; j++)
		{
			array3[j * 6] = j;
			array3[j * 6 + 1] = j + count;
			array3[j * 6 + 2] = j + 1 + count;
			array3[j * 6 + 3] = j;
			array3[j * 6 + 4] = j + 1 + count;
			array3[j * 6 + 5] = j + 1;
		}
		return new Mesh
		{
			vertices = array,
			uv = array2,
			triangles = array3
		};
	}
}
