using System;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x02000788 RID: 1928
public abstract class OvrAvatarDriver : MonoBehaviour
{
	// Token: 0x060031A8 RID: 12712 RVA: 0x00103EBB File Offset: 0x001022BB
	protected OvrAvatarDriver()
	{
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x00103EC3 File Offset: 0x001022C3
	public OvrAvatarDriver.PoseFrame GetCurrentPose()
	{
		return this.CurrentPose;
	}

	// Token: 0x060031AA RID: 12714
	public abstract void UpdateTransforms(IntPtr sdkAvatar);

	// Token: 0x060031AB RID: 12715 RVA: 0x00103ECC File Offset: 0x001022CC
	protected void UpdateTransformsFromPose(IntPtr sdkAvatar)
	{
		if (sdkAvatar != IntPtr.Zero)
		{
			ovrAvatarTransform headPose = OvrAvatar.CreateOvrAvatarTransform(this.CurrentPose.headPosition, this.CurrentPose.headRotation);
			ovrAvatarHandInputState inputStateLeft = OvrAvatar.CreateInputState(OvrAvatar.CreateOvrAvatarTransform(this.CurrentPose.handLeftPosition, this.CurrentPose.handLeftRotation), this.CurrentPose.controllerLeftPose);
			ovrAvatarHandInputState inputStateRight = OvrAvatar.CreateInputState(OvrAvatar.CreateOvrAvatarTransform(this.CurrentPose.handRightPosition, this.CurrentPose.handRightRotation), this.CurrentPose.controllerRightPose);
			CAPI.ovrAvatarPose_UpdateBody(sdkAvatar, headPose);
			if (OvrAvatarDriver.GetIsTrackedRemote())
			{
				CAPI.ovrAvatarPose_UpdateSDK3DofHands(sdkAvatar, inputStateLeft, inputStateRight, this.GetRemoteControllerType());
			}
			else
			{
				CAPI.ovrAvatarPose_UpdateHands(sdkAvatar, inputStateLeft, inputStateRight);
			}
		}
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x00103F89 File Offset: 0x00102389
	public static bool GetIsTrackedRemote()
	{
		return OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote) || OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote);
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x00103FA7 File Offset: 0x001023A7
	private ovrAvatarControllerType GetRemoteControllerType()
	{
		return (!(OVRPlugin.productName == "Oculus Go")) ? ovrAvatarControllerType.Malibu : ovrAvatarControllerType.Go;
	}

	// Token: 0x0400258C RID: 9612
	public OvrAvatarDriver.PacketMode Mode;

	// Token: 0x0400258D RID: 9613
	protected OvrAvatarDriver.PoseFrame CurrentPose;

	// Token: 0x02000789 RID: 1929
	public enum PacketMode
	{
		// Token: 0x0400258F RID: 9615
		SDK,
		// Token: 0x04002590 RID: 9616
		Unity
	}

	// Token: 0x0200078A RID: 1930
	public struct ControllerPose
	{
		// Token: 0x060031AE RID: 12718 RVA: 0x00103FC4 File Offset: 0x001023C4
		public static OvrAvatarDriver.ControllerPose Interpolate(OvrAvatarDriver.ControllerPose a, OvrAvatarDriver.ControllerPose b, float t)
		{
			return new OvrAvatarDriver.ControllerPose
			{
				buttons = ((t >= 0.5f) ? b.buttons : a.buttons),
				touches = ((t >= 0.5f) ? b.touches : a.touches),
				joystickPosition = Vector2.Lerp(a.joystickPosition, b.joystickPosition, t),
				indexTrigger = Mathf.Lerp(a.indexTrigger, b.indexTrigger, t),
				handTrigger = Mathf.Lerp(a.handTrigger, b.handTrigger, t),
				isActive = ((t >= 0.5f) ? b.isActive : a.isActive)
			};
		}

		// Token: 0x04002591 RID: 9617
		public ovrAvatarButton buttons;

		// Token: 0x04002592 RID: 9618
		public ovrAvatarTouch touches;

		// Token: 0x04002593 RID: 9619
		public Vector2 joystickPosition;

		// Token: 0x04002594 RID: 9620
		public float indexTrigger;

		// Token: 0x04002595 RID: 9621
		public float handTrigger;

		// Token: 0x04002596 RID: 9622
		public bool isActive;
	}

	// Token: 0x0200078B RID: 1931
	public struct PoseFrame
	{
		// Token: 0x060031AF RID: 12719 RVA: 0x0010409C File Offset: 0x0010249C
		public static OvrAvatarDriver.PoseFrame Interpolate(OvrAvatarDriver.PoseFrame a, OvrAvatarDriver.PoseFrame b, float t)
		{
			return new OvrAvatarDriver.PoseFrame
			{
				headPosition = Vector3.Lerp(a.headPosition, b.headPosition, t),
				headRotation = Quaternion.Slerp(a.headRotation, b.headRotation, t),
				handLeftPosition = Vector3.Lerp(a.handLeftPosition, b.handLeftPosition, t),
				handLeftRotation = Quaternion.Slerp(a.handLeftRotation, b.handLeftRotation, t),
				handRightPosition = Vector3.Lerp(a.handRightPosition, b.handRightPosition, t),
				handRightRotation = Quaternion.Slerp(a.handRightRotation, b.handRightRotation, t),
				voiceAmplitude = Mathf.Lerp(a.voiceAmplitude, b.voiceAmplitude, t),
				controllerLeftPose = OvrAvatarDriver.ControllerPose.Interpolate(a.controllerLeftPose, b.controllerLeftPose, t),
				controllerRightPose = OvrAvatarDriver.ControllerPose.Interpolate(a.controllerRightPose, b.controllerRightPose, t)
			};
		}

		// Token: 0x04002597 RID: 9623
		public Vector3 headPosition;

		// Token: 0x04002598 RID: 9624
		public Quaternion headRotation;

		// Token: 0x04002599 RID: 9625
		public Vector3 handLeftPosition;

		// Token: 0x0400259A RID: 9626
		public Quaternion handLeftRotation;

		// Token: 0x0400259B RID: 9627
		public Vector3 handRightPosition;

		// Token: 0x0400259C RID: 9628
		public Quaternion handRightRotation;

		// Token: 0x0400259D RID: 9629
		public float voiceAmplitude;

		// Token: 0x0400259E RID: 9630
		public OvrAvatarDriver.ControllerPose controllerLeftPose;

		// Token: 0x0400259F RID: 9631
		public OvrAvatarDriver.ControllerPose controllerRightPose;
	}
}
