﻿using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020008CF RID: 2255
public class OVRDebugHeadController : MonoBehaviour
{
	// Token: 0x060038C1 RID: 14529 RVA: 0x00114735 File Offset: 0x00112B35
	public OVRDebugHeadController()
	{
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x00114770 File Offset: 0x00112B70
	private void Awake()
	{
		OVRCameraRig[] componentsInChildren = base.gameObject.GetComponentsInChildren<OVRCameraRig>();
		if (componentsInChildren.Length == 0)
		{
			Debug.LogWarning("OVRCamParent: No OVRCameraRig attached.");
		}
		else if (componentsInChildren.Length > 1)
		{
			Debug.LogWarning("OVRCamParent: More then 1 OVRCameraRig attached.");
		}
		else
		{
			this.CameraRig = componentsInChildren[0];
		}
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x001147C1 File Offset: 0x00112BC1
	private void Start()
	{
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x001147C4 File Offset: 0x00112BC4
	private void Update()
	{
		if (this.AllowMovement)
		{
			float y = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Active).y;
			float x = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.Active).x;
			Vector3 a = this.CameraRig.centerEyeAnchor.rotation * Vector3.forward * y * Time.deltaTime * this.ForwardSpeed;
			Vector3 b = this.CameraRig.centerEyeAnchor.rotation * Vector3.right * x * Time.deltaTime * this.StrafeSpeed;
			base.transform.position += a + b;
		}
		if (!XRDevice.isPresent && (this.AllowYawLook || this.AllowPitchLook))
		{
			Quaternion quaternion = base.transform.rotation;
			if (this.AllowYawLook)
			{
				float x2 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.Active).x;
				float angle = x2 * Time.deltaTime * this.GamePad_YawDegreesPerSec;
				Quaternion lhs = Quaternion.AngleAxis(angle, Vector3.up);
				quaternion = lhs * quaternion;
			}
			if (this.AllowPitchLook)
			{
				float num = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.Active).y;
				if (Mathf.Abs(num) > 0.0001f)
				{
					if (this.InvertPitch)
					{
						num *= -1f;
					}
					float angle2 = num * Time.deltaTime * this.GamePad_PitchDegreesPerSec;
					Quaternion rhs = Quaternion.AngleAxis(angle2, Vector3.left);
					quaternion *= rhs;
				}
			}
			base.transform.rotation = quaternion;
		}
	}

	// Token: 0x040029E7 RID: 10727
	[SerializeField]
	public bool AllowPitchLook;

	// Token: 0x040029E8 RID: 10728
	[SerializeField]
	public bool AllowYawLook = true;

	// Token: 0x040029E9 RID: 10729
	[SerializeField]
	public bool InvertPitch;

	// Token: 0x040029EA RID: 10730
	[SerializeField]
	public float GamePad_PitchDegreesPerSec = 90f;

	// Token: 0x040029EB RID: 10731
	[SerializeField]
	public float GamePad_YawDegreesPerSec = 90f;

	// Token: 0x040029EC RID: 10732
	[SerializeField]
	public bool AllowMovement;

	// Token: 0x040029ED RID: 10733
	[SerializeField]
	public float ForwardSpeed = 2f;

	// Token: 0x040029EE RID: 10734
	[SerializeField]
	public float StrafeSpeed = 2f;

	// Token: 0x040029EF RID: 10735
	protected OVRCameraRig CameraRig;
}
