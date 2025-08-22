using System;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
public class CameraTarget : MonoBehaviour
{
	// Token: 0x06005352 RID: 21330 RVA: 0x001E2F1A File Offset: 0x001E131A
	public CameraTarget()
	{
	}

	// Token: 0x06005353 RID: 21331 RVA: 0x001E2F22 File Offset: 0x001E1322
	private void OnPreRender()
	{
		if (this.targetCamera != null)
		{
			this.worldToCameraMatrix = this.targetCamera.worldToCameraMatrix;
			this.projectionMatrix = this.targetCamera.projectionMatrix;
		}
	}

	// Token: 0x06005354 RID: 21332 RVA: 0x001E2F57 File Offset: 0x001E1357
	public void FindCamera()
	{
		this.targetCamera = base.GetComponent<Camera>();
		if (this.targetCamera == null)
		{
			this.targetCamera = base.transform.parent.GetComponent<Camera>();
		}
	}

	// Token: 0x06005355 RID: 21333 RVA: 0x001E2F8C File Offset: 0x001E138C
	private void Update()
	{
		CameraTarget.CameraLocation cameraLocation = this.cameraLocation;
		if (cameraLocation != CameraTarget.CameraLocation.Center)
		{
			if (cameraLocation != CameraTarget.CameraLocation.Left)
			{
				if (cameraLocation == CameraTarget.CameraLocation.Right)
				{
					CameraTarget.rightTarget = this;
				}
			}
			else
			{
				CameraTarget.leftTarget = this;
			}
		}
		else
		{
			CameraTarget.centerTarget = this;
		}
	}

	// Token: 0x0400434B RID: 17227
	public static CameraTarget centerTarget;

	// Token: 0x0400434C RID: 17228
	public static CameraTarget leftTarget;

	// Token: 0x0400434D RID: 17229
	public static CameraTarget rightTarget;

	// Token: 0x0400434E RID: 17230
	public CameraTarget.CameraLocation cameraLocation;

	// Token: 0x0400434F RID: 17231
	public Camera targetCamera;

	// Token: 0x04004350 RID: 17232
	public Matrix4x4 worldToCameraMatrix;

	// Token: 0x04004351 RID: 17233
	public Matrix4x4 projectionMatrix;

	// Token: 0x04004352 RID: 17234
	public bool isMonitorCamera;

	// Token: 0x02000B8F RID: 2959
	public enum CameraLocation
	{
		// Token: 0x04004354 RID: 17236
		None,
		// Token: 0x04004355 RID: 17237
		Left,
		// Token: 0x04004356 RID: 17238
		Right,
		// Token: 0x04004357 RID: 17239
		Center
	}
}
