using System;
using UnityEngine;

// Token: 0x02000C20 RID: 3104
public class JoystickControl : MonoBehaviour
{
	// Token: 0x06005A4A RID: 23114 RVA: 0x00212CF4 File Offset: 0x002110F4
	public JoystickControl()
	{
	}

	// Token: 0x06005A4B RID: 23115 RVA: 0x00212D7C File Offset: 0x0021117C
	public static float GetAxis(JoystickControl.Axis axis)
	{
		if (JoystickControl.singleton != null && JoystickControl.singleton.on)
		{
			switch (axis)
			{
			case JoystickControl.Axis.LeftStickX:
				if (OVRManager.isHmdPresent)
				{
					return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active).x;
				}
				return Input.GetAxis(JoystickControl.singleton.leftStickXAxisName);
			case JoystickControl.Axis.LeftStickY:
				if (OVRManager.isHmdPresent)
				{
					return -OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active).y;
				}
				return Input.GetAxis(JoystickControl.singleton.leftStickYAxisName);
			case JoystickControl.Axis.RightStickX:
				if (OVRManager.isHmdPresent)
				{
					return OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active).x;
				}
				return Input.GetAxis(JoystickControl.singleton.rightStickXAxisName);
			case JoystickControl.Axis.RightStickY:
				if (OVRManager.isHmdPresent)
				{
					return -OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active).y;
				}
				return Input.GetAxis(JoystickControl.singleton.rightStickYAxisName);
			case JoystickControl.Axis.Triggers:
				if (JoystickControl.singleton.joystickType == JoystickControl.JoystickType.X360)
				{
					return Input.GetAxis(JoystickControl.singleton.triggersAxisName);
				}
				if (JoystickControl.singleton.joystickType == JoystickControl.JoystickType.XBONE)
				{
					float num;
					if (OVRManager.isHmdPresent)
					{
						num = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger, OVRInput.Controller.Active);
						float num2 = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, OVRInput.Controller.Active);
						num -= num2;
					}
					else
					{
						num = 0f;
					}
					return num * 0.5f;
				}
				break;
			case JoystickControl.Axis.DPadX:
				if (JoystickControl.singleton.joystickType == JoystickControl.JoystickType.X360)
				{
					return Input.GetAxis(JoystickControl.singleton.x360DPadXAxisName);
				}
				if (JoystickControl.singleton.joystickType == JoystickControl.JoystickType.XBONE)
				{
					return Input.GetAxis(JoystickControl.singleton.xboneDPadXAxisName);
				}
				break;
			case JoystickControl.Axis.DPadY:
				if (JoystickControl.singleton.joystickType == JoystickControl.JoystickType.X360)
				{
					return Input.GetAxis(JoystickControl.singleton.x360DPadYAxisName);
				}
				if (JoystickControl.singleton.joystickType == JoystickControl.JoystickType.XBONE)
				{
					return Input.GetAxis(JoystickControl.singleton.xboneDPadYAxisName);
				}
				break;
			}
		}
		return 0f;
	}

	// Token: 0x06005A4C RID: 23116 RVA: 0x00212F90 File Offset: 0x00211390
	public static bool GetButtonDown(string buttonName)
	{
		return JoystickControl.singleton != null && JoystickControl.singleton.on && Input.GetButtonDown(buttonName);
	}

	// Token: 0x06005A4D RID: 23117 RVA: 0x00212FB9 File Offset: 0x002113B9
	public static bool GetButtonUp(string buttonName)
	{
		return JoystickControl.singleton != null && JoystickControl.singleton.on && Input.GetButtonUp(buttonName);
	}

	// Token: 0x06005A4E RID: 23118 RVA: 0x00212FE2 File Offset: 0x002113E2
	public static bool GetButton(string buttonName)
	{
		return JoystickControl.singleton != null && JoystickControl.singleton.on && Input.GetButton(buttonName);
	}

	// Token: 0x06005A4F RID: 23119 RVA: 0x0021300B File Offset: 0x0021140B
	private void Awake()
	{
		JoystickControl.singleton = this;
	}

	// Token: 0x04004A7C RID: 19068
	public static JoystickControl singleton;

	// Token: 0x04004A7D RID: 19069
	public JoystickControl.JoystickType joystickType;

	// Token: 0x04004A7E RID: 19070
	public bool on = true;

	// Token: 0x04004A7F RID: 19071
	public string leftStickXAxisName = "LeftStickX";

	// Token: 0x04004A80 RID: 19072
	public string leftStickYAxisName = "LeftStickY";

	// Token: 0x04004A81 RID: 19073
	public string rightStickXAxisName = "RightStickX";

	// Token: 0x04004A82 RID: 19074
	public string rightStickYAxisName = "RightStickY";

	// Token: 0x04004A83 RID: 19075
	public string triggersAxisName = "Triggers";

	// Token: 0x04004A84 RID: 19076
	public string xboneTriggerRightAxisName = "Axis6";

	// Token: 0x04004A85 RID: 19077
	public string xboneDPadXAxisName = "Axis7";

	// Token: 0x04004A86 RID: 19078
	public string xboneDPadYAxisName = "Axis8";

	// Token: 0x04004A87 RID: 19079
	public string x360DPadXAxisName = "Axis6";

	// Token: 0x04004A88 RID: 19080
	public string x360DPadYAxisName = "Axis7";

	// Token: 0x02000C21 RID: 3105
	public enum JoystickType
	{
		// Token: 0x04004A8A RID: 19082
		X360,
		// Token: 0x04004A8B RID: 19083
		XBONE
	}

	// Token: 0x02000C22 RID: 3106
	public enum Axis
	{
		// Token: 0x04004A8D RID: 19085
		None,
		// Token: 0x04004A8E RID: 19086
		LeftStickX,
		// Token: 0x04004A8F RID: 19087
		LeftStickY,
		// Token: 0x04004A90 RID: 19088
		RightStickX,
		// Token: 0x04004A91 RID: 19089
		RightStickY,
		// Token: 0x04004A92 RID: 19090
		Triggers,
		// Token: 0x04004A93 RID: 19091
		DPadX,
		// Token: 0x04004A94 RID: 19092
		DPadY
	}
}
