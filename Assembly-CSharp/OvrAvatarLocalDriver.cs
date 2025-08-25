using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200078D RID: 1933
public class OvrAvatarLocalDriver : OvrAvatarDriver
{
	// Token: 0x060031B1 RID: 12721 RVA: 0x001041AD File Offset: 0x001025AD
	public OvrAvatarLocalDriver()
	{
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x001041B8 File Offset: 0x001025B8
	private OvrAvatarDriver.ControllerPose GetMalibuControllerPose(OVRInput.Controller controller)
	{
		ovrAvatarButton ovrAvatarButton = (ovrAvatarButton)0;
		if (OVRInput.Get(OVRInput.Button.One, controller))
		{
			ovrAvatarButton |= ovrAvatarButton.One;
		}
		return new OvrAvatarDriver.ControllerPose
		{
			buttons = ovrAvatarButton,
			touches = ((!OVRInput.Get(OVRInput.Touch.PrimaryTouchpad, OVRInput.Controller.Active)) ? ((ovrAvatarTouch)0) : ovrAvatarTouch.One),
			joystickPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, controller),
			indexTrigger = 0f,
			handTrigger = 0f,
			isActive = ((OVRInput.GetActiveController() & controller) != OVRInput.Controller.None)
		};
	}

	// Token: 0x060031B3 RID: 12723 RVA: 0x00104244 File Offset: 0x00102644
	private OvrAvatarDriver.ControllerPose GetControllerPose(OVRInput.Controller controller)
	{
		ovrAvatarButton ovrAvatarButton = (ovrAvatarButton)0;
		if (OVRInput.Get(OVRInput.Button.One, controller))
		{
			ovrAvatarButton |= ovrAvatarButton.One;
		}
		if (OVRInput.Get(OVRInput.Button.Two, controller))
		{
			ovrAvatarButton |= ovrAvatarButton.Two;
		}
		if (OVRInput.Get(OVRInput.Button.Start, controller))
		{
			ovrAvatarButton |= ovrAvatarButton.Three;
		}
		if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, controller))
		{
			ovrAvatarButton |= ovrAvatarButton.Joystick;
		}
		ovrAvatarTouch ovrAvatarTouch = (ovrAvatarTouch)0;
		if (OVRInput.Get(OVRInput.Touch.One, controller))
		{
			ovrAvatarTouch |= ovrAvatarTouch.One;
		}
		if (OVRInput.Get(OVRInput.Touch.Two, controller))
		{
			ovrAvatarTouch |= ovrAvatarTouch.Two;
		}
		if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, controller))
		{
			ovrAvatarTouch |= ovrAvatarTouch.Joystick;
		}
		if (OVRInput.Get(OVRInput.Touch.PrimaryThumbRest, controller))
		{
			ovrAvatarTouch |= ovrAvatarTouch.ThumbRest;
		}
		if (OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, controller))
		{
			ovrAvatarTouch |= ovrAvatarTouch.Index;
		}
		if (!OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, controller))
		{
			ovrAvatarTouch |= ovrAvatarTouch.Pointing;
		}
		if (!OVRInput.Get(OVRInput.NearTouch.PrimaryThumbButtons, controller))
		{
			ovrAvatarTouch |= ovrAvatarTouch.ThumbUp;
		}
		return new OvrAvatarDriver.ControllerPose
		{
			buttons = ovrAvatarButton,
			touches = ovrAvatarTouch,
			joystickPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller),
			indexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller),
			handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller),
			isActive = ((OVRInput.GetActiveController() & controller) != OVRInput.Controller.None)
		};
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x00104378 File Offset: 0x00102778
	private void CalculateCurrentPose()
	{
		Vector3 localPosition = InputTracking.GetLocalPosition(XRNode.CenterEye);
		if (OvrAvatarDriver.GetIsTrackedRemote())
		{
			this.CurrentPose = new OvrAvatarDriver.PoseFrame
			{
				voiceAmplitude = this.voiceAmplitude,
				headPosition = localPosition,
				headRotation = InputTracking.GetLocalRotation(XRNode.CenterEye),
				handLeftPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote),
				handLeftRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTrackedRemote),
				handRightPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote),
				handRightRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote),
				controllerLeftPose = this.GetMalibuControllerPose(OVRInput.Controller.LTrackedRemote),
				controllerRightPose = this.GetMalibuControllerPose(OVRInput.Controller.RTrackedRemote)
			};
		}
		else
		{
			this.CurrentPose = new OvrAvatarDriver.PoseFrame
			{
				voiceAmplitude = this.voiceAmplitude,
				headPosition = localPosition,
				headRotation = InputTracking.GetLocalRotation(XRNode.CenterEye),
				handLeftPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch),
				handLeftRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch),
				handRightPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),
				handRightRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch),
				controllerLeftPose = this.GetControllerPose(OVRInput.Controller.LTouch),
				controllerRightPose = this.GetControllerPose(OVRInput.Controller.RTouch)
			};
		}
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x001044B5 File Offset: 0x001028B5
	public override void UpdateTransforms(IntPtr sdkAvatar)
	{
		this.CalculateCurrentPose();
		base.UpdateTransformsFromPose(sdkAvatar);
	}

	// Token: 0x040025A0 RID: 9632
	private const float mobileBaseHeadHeight = 1.7f;

	// Token: 0x040025A1 RID: 9633
	private float voiceAmplitude;
}
