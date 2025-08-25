using System;
using UnityEngine;

// Token: 0x02000BAD RID: 2989
public class LookAtCamera : MonoBehaviour
{
	// Token: 0x06005551 RID: 21841 RVA: 0x001F3691 File Offset: 0x001F1A91
	public LookAtCamera()
	{
	}

	// Token: 0x06005552 RID: 21842 RVA: 0x001F36A0 File Offset: 0x001F1AA0
	private void LookAt()
	{
		CameraTarget cameraTarget = null;
		Transform transform = null;
		if (this.lookAtCameraLocation != CameraTarget.CameraLocation.None)
		{
			CameraTarget.CameraLocation cameraLocation = this.lookAtCameraLocation;
			if (cameraLocation != CameraTarget.CameraLocation.Center)
			{
				if (cameraLocation != CameraTarget.CameraLocation.Left)
				{
					if (cameraLocation == CameraTarget.CameraLocation.Right)
					{
						if (CameraTarget.rightTarget != null)
						{
							cameraTarget = CameraTarget.rightTarget;
							transform = CameraTarget.rightTarget.transform;
						}
					}
				}
				else if (CameraTarget.leftTarget != null)
				{
					cameraTarget = CameraTarget.leftTarget;
					transform = CameraTarget.leftTarget.transform;
				}
			}
			else if (CameraTarget.centerTarget != null)
			{
				cameraTarget = CameraTarget.centerTarget;
				transform = CameraTarget.centerTarget.transform;
			}
		}
		if (cameraTarget != null)
		{
			if (this.lockZLocalPositionIfMonitor && cameraTarget.isMonitorCamera)
			{
				Vector3 localPosition = base.transform.localPosition;
				localPosition.z = 0f;
				base.transform.localPosition = localPosition;
			}
			if (this.useCameraRotationIfMonitor && cameraTarget.isMonitorCamera)
			{
				base.transform.rotation = transform.rotation;
				base.transform.Rotate(0f, 180f, 0f);
			}
			else if (this.lockXZ || (this.lockXZIfMonitor && cameraTarget.isMonitorCamera))
			{
				Vector3 position = transform.position;
				position.y = base.transform.position.y;
				if (this.useTargetUp)
				{
					base.transform.LookAt(position, transform.up);
				}
				else
				{
					base.transform.LookAt(position);
				}
			}
			else
			{
				base.transform.LookAt(transform);
			}
		}
	}

	// Token: 0x06005553 RID: 21843 RVA: 0x001F3860 File Offset: 0x001F1C60
	private void Update()
	{
		this.LookAt();
	}

	// Token: 0x0400463E RID: 17982
	public bool on = true;

	// Token: 0x0400463F RID: 17983
	public CameraTarget.CameraLocation lookAtCameraLocation;

	// Token: 0x04004640 RID: 17984
	public bool lockXZ;

	// Token: 0x04004641 RID: 17985
	public bool useTargetUp;

	// Token: 0x04004642 RID: 17986
	public bool useCameraRotationIfMonitor;

	// Token: 0x04004643 RID: 17987
	public bool lockXZIfMonitor;

	// Token: 0x04004644 RID: 17988
	public bool lockZLocalPositionIfMonitor;
}
