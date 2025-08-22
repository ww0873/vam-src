using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020008D9 RID: 2265
public static class OVRInput
{
	// Token: 0x06003905 RID: 14597 RVA: 0x00115784 File Offset: 0x00113B84
	static OVRInput()
	{
		OVRInput.controllers = new List<OVRInput.OVRControllerBase>
		{
			new OVRInput.OVRControllerGamepadPC(),
			new OVRInput.OVRControllerTouch(),
			new OVRInput.OVRControllerLTouch(),
			new OVRInput.OVRControllerRTouch(),
			new OVRInput.OVRControllerRemote()
		};
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06003906 RID: 14598 RVA: 0x0011581C File Offset: 0x00113C1C
	private static bool pluginSupportsActiveController
	{
		get
		{
			if (!OVRInput._pluginSupportsActiveControllerCached)
			{
				bool flag = true;
				OVRInput._pluginSupportsActiveController = (flag && OVRPlugin.version >= OVRInput._pluginSupportsActiveControllerMinVersion);
				OVRInput._pluginSupportsActiveControllerCached = true;
			}
			return OVRInput._pluginSupportsActiveController;
		}
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x00115860 File Offset: 0x00113C60
	public static void Update()
	{
		OVRInput.connectedControllerTypes = OVRInput.Controller.None;
		OVRInput.stepType = OVRPlugin.Step.Render;
		OVRInput.fixedUpdateCount = 0;
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			OVRInput.connectedControllerTypes |= ovrcontrollerBase.Update();
			if ((OVRInput.connectedControllerTypes & ovrcontrollerBase.controllerType) != OVRInput.Controller.None)
			{
				OVRInput.RawButton rawMask = OVRInput.RawButton.Any;
				OVRInput.RawTouch rawMask2 = OVRInput.RawTouch.Any;
				if (OVRInput.Get(rawMask, ovrcontrollerBase.controllerType) || OVRInput.Get(rawMask2, ovrcontrollerBase.controllerType))
				{
					OVRInput.activeControllerType = ovrcontrollerBase.controllerType;
				}
			}
		}
		if (OVRInput.activeControllerType == OVRInput.Controller.LTouch || OVRInput.activeControllerType == OVRInput.Controller.RTouch)
		{
			OVRInput.activeControllerType = OVRInput.Controller.Touch;
		}
		if ((OVRInput.connectedControllerTypes & OVRInput.activeControllerType) == OVRInput.Controller.None)
		{
			OVRInput.activeControllerType = OVRInput.Controller.None;
		}
		if (OVRInput.activeControllerType == OVRInput.Controller.None)
		{
			if ((OVRInput.connectedControllerTypes & OVRInput.Controller.RTrackedRemote) != OVRInput.Controller.None)
			{
				OVRInput.activeControllerType = OVRInput.Controller.RTrackedRemote;
			}
			else if ((OVRInput.connectedControllerTypes & OVRInput.Controller.LTrackedRemote) != OVRInput.Controller.None)
			{
				OVRInput.activeControllerType = OVRInput.Controller.LTrackedRemote;
			}
		}
		if (OVRInput.pluginSupportsActiveController)
		{
			OVRInput.connectedControllerTypes = (OVRInput.Controller)OVRPlugin.GetConnectedControllers();
			OVRInput.activeControllerType = (OVRInput.Controller)OVRPlugin.GetActiveController();
		}
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x0011598C File Offset: 0x00113D8C
	public static void FixedUpdate()
	{
		OVRInput.stepType = OVRPlugin.Step.Physics;
		double predictionSeconds = (double)OVRInput.fixedUpdateCount * (double)Time.fixedDeltaTime / (double)Mathf.Max(Time.timeScale, 1E-06f);
		OVRInput.fixedUpdateCount++;
		OVRPlugin.UpdateNodePhysicsPoses(0, predictionSeconds);
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x001159D2 File Offset: 0x00113DD2
	public static bool GetControllerOrientationTracked(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return false;
				}
			}
			return OVRPlugin.GetNodeOrientationTracked(OVRPlugin.Node.HandRight);
		}
		IL_29:
		return OVRPlugin.GetNodeOrientationTracked(OVRPlugin.Node.HandLeft);
	}

	// Token: 0x0600390A RID: 14602 RVA: 0x00115A0C File Offset: 0x00113E0C
	public static bool GetControllerPositionTracked(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return false;
				}
			}
			return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.HandRight);
		}
		IL_29:
		return OVRPlugin.GetNodePositionTracked(OVRPlugin.Node.HandLeft);
	}

	// Token: 0x0600390B RID: 14603 RVA: 0x00115A48 File Offset: 0x00113E48
	public static Vector3 GetLocalControllerPosition(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return Vector3.zero;
				}
			}
			return OVRPlugin.GetNodePose(OVRPlugin.Node.HandRight, OVRInput.stepType).ToOVRPose().position;
		}
		IL_29:
		return OVRPlugin.GetNodePose(OVRPlugin.Node.HandLeft, OVRInput.stepType).ToOVRPose().position;
	}

	// Token: 0x0600390C RID: 14604 RVA: 0x00115AB8 File Offset: 0x00113EB8
	public static Vector3 GetLocalControllerVelocity(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return Vector3.zero;
				}
			}
			return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
		}
		IL_29:
		return OVRPlugin.GetNodeVelocity(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
	}

	// Token: 0x0600390D RID: 14605 RVA: 0x00115B18 File Offset: 0x00113F18
	public static Vector3 GetLocalControllerAcceleration(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return Vector3.zero;
				}
			}
			return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
		}
		IL_29:
		return OVRPlugin.GetNodeAcceleration(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
	}

	// Token: 0x0600390E RID: 14606 RVA: 0x00115B78 File Offset: 0x00113F78
	public static Quaternion GetLocalControllerRotation(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return Quaternion.identity;
				}
			}
			return OVRPlugin.GetNodePose(OVRPlugin.Node.HandRight, OVRInput.stepType).ToOVRPose().orientation;
		}
		IL_29:
		return OVRPlugin.GetNodePose(OVRPlugin.Node.HandLeft, OVRInput.stepType).ToOVRPose().orientation;
	}

	// Token: 0x0600390F RID: 14607 RVA: 0x00115BE8 File Offset: 0x00113FE8
	public static Vector3 GetLocalControllerAngularVelocity(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return Vector3.zero;
				}
			}
			return OVRPlugin.GetNodeAngularVelocity(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
		}
		IL_29:
		return OVRPlugin.GetNodeAngularVelocity(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
	}

	// Token: 0x06003910 RID: 14608 RVA: 0x00115C48 File Offset: 0x00114048
	public static Vector3 GetLocalControllerAngularAcceleration(OVRInput.Controller controllerType)
	{
		if (controllerType != OVRInput.Controller.LTouch)
		{
			if (controllerType != OVRInput.Controller.RTouch)
			{
				if (controllerType == OVRInput.Controller.LTrackedRemote)
				{
					goto IL_29;
				}
				if (controllerType != OVRInput.Controller.RTrackedRemote)
				{
					return Vector3.zero;
				}
			}
			return OVRPlugin.GetNodeAngularAcceleration(OVRPlugin.Node.HandRight, OVRInput.stepType).FromFlippedZVector3f();
		}
		IL_29:
		return OVRPlugin.GetNodeAngularAcceleration(OVRPlugin.Node.HandLeft, OVRInput.stepType).FromFlippedZVector3f();
	}

	// Token: 0x06003911 RID: 14609 RVA: 0x00115CA5 File Offset: 0x001140A5
	public static bool Get(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButton(virtualMask, OVRInput.RawButton.None, controllerMask);
	}

	// Token: 0x06003912 RID: 14610 RVA: 0x00115CAF File Offset: 0x001140AF
	public static bool Get(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButton(OVRInput.Button.None, rawMask, controllerMask);
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x00115CBC File Offset: 0x001140BC
	private static bool GetResolvedButton(OVRInput.Button virtualMask, OVRInput.RawButton rawMask, OVRInput.Controller controllerMask)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawButton rawButton = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) != 0U)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003914 RID: 14612 RVA: 0x00115D35 File Offset: 0x00114135
	public static bool GetDown(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonDown(virtualMask, OVRInput.RawButton.None, controllerMask);
	}

	// Token: 0x06003915 RID: 14613 RVA: 0x00115D3F File Offset: 0x0011413F
	public static bool GetDown(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonDown(OVRInput.Button.None, rawMask, controllerMask);
	}

	// Token: 0x06003916 RID: 14614 RVA: 0x00115D4C File Offset: 0x0011414C
	private static bool GetResolvedButtonDown(OVRInput.Button virtualMask, OVRInput.RawButton rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawButton rawButton = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.previousState.Buttons & (uint)rawButton) != 0U)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) != 0U && (ovrcontrollerBase.previousState.Buttons & (uint)rawButton) == 0U)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003917 RID: 14615 RVA: 0x00115DED File Offset: 0x001141ED
	public static bool GetUp(OVRInput.Button virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonUp(virtualMask, OVRInput.RawButton.None, controllerMask);
	}

	// Token: 0x06003918 RID: 14616 RVA: 0x00115DF7 File Offset: 0x001141F7
	public static bool GetUp(OVRInput.RawButton rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedButtonUp(OVRInput.Button.None, rawMask, controllerMask);
	}

	// Token: 0x06003919 RID: 14617 RVA: 0x00115E04 File Offset: 0x00114204
	private static bool GetResolvedButtonUp(OVRInput.Button virtualMask, OVRInput.RawButton rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawButton rawButton = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) != 0U)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Buttons & (uint)rawButton) == 0U && (ovrcontrollerBase.previousState.Buttons & (uint)rawButton) != 0U)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x0600391A RID: 14618 RVA: 0x00115EA5 File Offset: 0x001142A5
	public static bool Get(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouch(virtualMask, OVRInput.RawTouch.None, controllerMask);
	}

	// Token: 0x0600391B RID: 14619 RVA: 0x00115EAF File Offset: 0x001142AF
	public static bool Get(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouch(OVRInput.Touch.None, rawMask, controllerMask);
	}

	// Token: 0x0600391C RID: 14620 RVA: 0x00115EBC File Offset: 0x001142BC
	private static bool GetResolvedTouch(OVRInput.Touch virtualMask, OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawTouch rawTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) != 0U)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600391D RID: 14621 RVA: 0x00115F35 File Offset: 0x00114335
	public static bool GetDown(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchDown(virtualMask, OVRInput.RawTouch.None, controllerMask);
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x00115F3F File Offset: 0x0011433F
	public static bool GetDown(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchDown(OVRInput.Touch.None, rawMask, controllerMask);
	}

	// Token: 0x0600391F RID: 14623 RVA: 0x00115F4C File Offset: 0x0011434C
	private static bool GetResolvedTouchDown(OVRInput.Touch virtualMask, OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawTouch rawTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.previousState.Touches & (uint)rawTouch) != 0U)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) != 0U && (ovrcontrollerBase.previousState.Touches & (uint)rawTouch) == 0U)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003920 RID: 14624 RVA: 0x00115FED File Offset: 0x001143ED
	public static bool GetUp(OVRInput.Touch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchUp(virtualMask, OVRInput.RawTouch.None, controllerMask);
	}

	// Token: 0x06003921 RID: 14625 RVA: 0x00115FF7 File Offset: 0x001143F7
	public static bool GetUp(OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedTouchUp(OVRInput.Touch.None, rawMask, controllerMask);
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x00116004 File Offset: 0x00114404
	private static bool GetResolvedTouchUp(OVRInput.Touch virtualMask, OVRInput.RawTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawTouch rawTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) != 0U)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.Touches & (uint)rawTouch) == 0U && (ovrcontrollerBase.previousState.Touches & (uint)rawTouch) != 0U)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x001160A5 File Offset: 0x001144A5
	public static bool Get(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouch(virtualMask, OVRInput.RawNearTouch.None, controllerMask);
	}

	// Token: 0x06003924 RID: 14628 RVA: 0x001160AF File Offset: 0x001144AF
	public static bool Get(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouch(OVRInput.NearTouch.None, rawMask, controllerMask);
	}

	// Token: 0x06003925 RID: 14629 RVA: 0x001160BC File Offset: 0x001144BC
	private static bool GetResolvedNearTouch(OVRInput.NearTouch virtualMask, OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawNearTouch rawNearTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) != 0U)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003926 RID: 14630 RVA: 0x00116135 File Offset: 0x00114535
	public static bool GetDown(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchDown(virtualMask, OVRInput.RawNearTouch.None, controllerMask);
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x0011613F File Offset: 0x0011453F
	public static bool GetDown(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchDown(OVRInput.NearTouch.None, rawMask, controllerMask);
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x0011614C File Offset: 0x0011454C
	private static bool GetResolvedNearTouchDown(OVRInput.NearTouch virtualMask, OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawNearTouch rawNearTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.previousState.NearTouches & (uint)rawNearTouch) != 0U)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) != 0U && (ovrcontrollerBase.previousState.NearTouches & (uint)rawNearTouch) == 0U)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x001161ED File Offset: 0x001145ED
	public static bool GetUp(OVRInput.NearTouch virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchUp(virtualMask, OVRInput.RawNearTouch.None, controllerMask);
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x001161F7 File Offset: 0x001145F7
	public static bool GetUp(OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedNearTouchUp(OVRInput.NearTouch.None, rawMask, controllerMask);
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x00116204 File Offset: 0x00114604
	private static bool GetResolvedNearTouchUp(OVRInput.NearTouch virtualMask, OVRInput.RawNearTouch rawMask, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawNearTouch rawNearTouch = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) != 0U)
				{
					return false;
				}
				if ((ovrcontrollerBase.currentState.NearTouches & (uint)rawNearTouch) == 0U && (ovrcontrollerBase.previousState.NearTouches & (uint)rawNearTouch) != 0U)
				{
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x0600392C RID: 14636 RVA: 0x001162A5 File Offset: 0x001146A5
	public static float Get(OVRInput.Axis1D virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis1D(virtualMask, OVRInput.RawAxis1D.None, controllerMask);
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x001162AF File Offset: 0x001146AF
	public static float Get(OVRInput.RawAxis1D rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis1D(OVRInput.Axis1D.None, rawMask, controllerMask);
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x001162BC File Offset: 0x001146BC
	private static float GetResolvedAxis1D(OVRInput.Axis1D virtualMask, OVRInput.RawAxis1D rawMask, OVRInput.Controller controllerMask)
	{
		float num = 0f;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawAxis1D rawAxis1D = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((OVRInput.RawAxis1D.LIndexTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					float num2 = ovrcontrollerBase.currentState.LIndexTrigger;
					if (ovrcontrollerBase.shouldApplyDeadzone)
					{
						num2 = OVRInput.CalculateDeadzone(num2, OVRInput.AXIS_DEADZONE_THRESHOLD);
					}
					num = OVRInput.CalculateAbsMax(num, num2);
				}
				if ((OVRInput.RawAxis1D.RIndexTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					float num3 = ovrcontrollerBase.currentState.RIndexTrigger;
					if (ovrcontrollerBase.shouldApplyDeadzone)
					{
						num3 = OVRInput.CalculateDeadzone(num3, OVRInput.AXIS_DEADZONE_THRESHOLD);
					}
					num = OVRInput.CalculateAbsMax(num, num3);
				}
				if ((OVRInput.RawAxis1D.LHandTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					float num4 = ovrcontrollerBase.currentState.LHandTrigger;
					if (ovrcontrollerBase.shouldApplyDeadzone)
					{
						num4 = OVRInput.CalculateDeadzone(num4, OVRInput.AXIS_DEADZONE_THRESHOLD);
					}
					num = OVRInput.CalculateAbsMax(num, num4);
				}
				if ((OVRInput.RawAxis1D.RHandTrigger & rawAxis1D) != OVRInput.RawAxis1D.None)
				{
					float num5 = ovrcontrollerBase.currentState.RHandTrigger;
					if (ovrcontrollerBase.shouldApplyDeadzone)
					{
						num5 = OVRInput.CalculateDeadzone(num5, OVRInput.AXIS_DEADZONE_THRESHOLD);
					}
					num = OVRInput.CalculateAbsMax(num, num5);
				}
			}
		}
		return num;
	}

	// Token: 0x0600392F RID: 14639 RVA: 0x00116403 File Offset: 0x00114803
	public static Vector2 Get(OVRInput.Axis2D virtualMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis2D(virtualMask, OVRInput.RawAxis2D.None, controllerMask);
	}

	// Token: 0x06003930 RID: 14640 RVA: 0x0011640D File Offset: 0x0011480D
	public static Vector2 Get(OVRInput.RawAxis2D rawMask, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		return OVRInput.GetResolvedAxis2D(OVRInput.Axis2D.None, rawMask, controllerMask);
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x00116418 File Offset: 0x00114818
	private static Vector2 GetResolvedAxis2D(OVRInput.Axis2D virtualMask, OVRInput.RawAxis2D rawMask, OVRInput.Controller controllerMask)
	{
		Vector2 vector = Vector2.zero;
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				OVRInput.RawAxis2D rawAxis2D = rawMask | ovrcontrollerBase.ResolveToRawMask(virtualMask);
				if ((OVRInput.RawAxis2D.LThumbstick & rawAxis2D) != OVRInput.RawAxis2D.None)
				{
					Vector2 vector2 = new Vector2(ovrcontrollerBase.currentState.LThumbstick.x, ovrcontrollerBase.currentState.LThumbstick.y);
					if (ovrcontrollerBase.shouldApplyDeadzone)
					{
						vector2 = OVRInput.CalculateDeadzone(vector2, OVRInput.AXIS_DEADZONE_THRESHOLD);
					}
					vector = OVRInput.CalculateAbsMax(vector, vector2);
				}
				if ((OVRInput.RawAxis2D.LTouchpad & rawAxis2D) != OVRInput.RawAxis2D.None)
				{
					Vector2 b = new Vector2(ovrcontrollerBase.currentState.LTouchpad.x, ovrcontrollerBase.currentState.LTouchpad.y);
					vector = OVRInput.CalculateAbsMax(vector, b);
				}
				if ((OVRInput.RawAxis2D.RThumbstick & rawAxis2D) != OVRInput.RawAxis2D.None)
				{
					Vector2 vector3 = new Vector2(ovrcontrollerBase.currentState.RThumbstick.x, ovrcontrollerBase.currentState.RThumbstick.y);
					if (ovrcontrollerBase.shouldApplyDeadzone)
					{
						vector3 = OVRInput.CalculateDeadzone(vector3, OVRInput.AXIS_DEADZONE_THRESHOLD);
					}
					vector = OVRInput.CalculateAbsMax(vector, vector3);
				}
				if ((OVRInput.RawAxis2D.RTouchpad & rawAxis2D) != OVRInput.RawAxis2D.None)
				{
					Vector2 b2 = new Vector2(ovrcontrollerBase.currentState.RTouchpad.x, ovrcontrollerBase.currentState.RTouchpad.y);
					vector = OVRInput.CalculateAbsMax(vector, b2);
				}
			}
		}
		return vector;
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x00116595 File Offset: 0x00114995
	public static OVRInput.Controller GetConnectedControllers()
	{
		return OVRInput.connectedControllerTypes;
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x0011659C File Offset: 0x0011499C
	public static bool IsControllerConnected(OVRInput.Controller controller)
	{
		return (OVRInput.connectedControllerTypes & controller) == controller;
	}

	// Token: 0x06003934 RID: 14644 RVA: 0x001165A8 File Offset: 0x001149A8
	public static OVRInput.Controller GetActiveController()
	{
		return OVRInput.activeControllerType;
	}

	// Token: 0x06003935 RID: 14645 RVA: 0x001165B0 File Offset: 0x001149B0
	public static void SetControllerVibration(float frequency, float amplitude, OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				ovrcontrollerBase.SetControllerVibration(frequency, amplitude);
			}
		}
	}

	// Token: 0x06003936 RID: 14646 RVA: 0x00116614 File Offset: 0x00114A14
	public static void RecenterController(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				ovrcontrollerBase.RecenterController();
			}
		}
	}

	// Token: 0x06003937 RID: 14647 RVA: 0x00116674 File Offset: 0x00114A74
	public static bool GetControllerWasRecentered(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		bool flag = false;
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				flag |= ovrcontrollerBase.WasRecentered();
			}
		}
		return flag;
	}

	// Token: 0x06003938 RID: 14648 RVA: 0x001166DC File Offset: 0x00114ADC
	public static byte GetControllerRecenterCount(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		byte result = 0;
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				result = ovrcontrollerBase.GetRecenterCount();
				break;
			}
		}
		return result;
	}

	// Token: 0x06003939 RID: 14649 RVA: 0x00116748 File Offset: 0x00114B48
	public static byte GetControllerBatteryPercentRemaining(OVRInput.Controller controllerMask = OVRInput.Controller.Active)
	{
		if ((controllerMask & OVRInput.Controller.Active) != OVRInput.Controller.None)
		{
			controllerMask |= OVRInput.activeControllerType;
		}
		byte result = 0;
		for (int i = 0; i < OVRInput.controllers.Count; i++)
		{
			OVRInput.OVRControllerBase ovrcontrollerBase = OVRInput.controllers[i];
			if (OVRInput.ShouldResolveController(ovrcontrollerBase.controllerType, controllerMask))
			{
				result = ovrcontrollerBase.GetBatteryPercentRemaining();
				break;
			}
		}
		return result;
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x001167B4 File Offset: 0x00114BB4
	private static Vector2 CalculateAbsMax(Vector2 a, Vector2 b)
	{
		float sqrMagnitude = a.sqrMagnitude;
		float sqrMagnitude2 = b.sqrMagnitude;
		if (sqrMagnitude >= sqrMagnitude2)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600393B RID: 14651 RVA: 0x001167DC File Offset: 0x00114BDC
	private static float CalculateAbsMax(float a, float b)
	{
		float num = (a < 0f) ? (-a) : a;
		float num2 = (b < 0f) ? (-b) : b;
		if (num >= num2)
		{
			return a;
		}
		return b;
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x0011681C File Offset: 0x00114C1C
	private static Vector2 CalculateDeadzone(Vector2 a, float deadzone)
	{
		if (a.sqrMagnitude <= deadzone * deadzone)
		{
			return Vector2.zero;
		}
		a *= (a.magnitude - deadzone) / (1f - deadzone);
		if (a.sqrMagnitude > 1f)
		{
			return a.normalized;
		}
		return a;
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x00116874 File Offset: 0x00114C74
	private static float CalculateDeadzone(float a, float deadzone)
	{
		float num = (a < 0f) ? (-a) : a;
		if (num <= deadzone)
		{
			return 0f;
		}
		a *= (num - deadzone) / (1f - deadzone);
		if (a * a > 1f)
		{
			return (a < 0f) ? -1f : 1f;
		}
		return a;
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x001168DC File Offset: 0x00114CDC
	private static bool ShouldResolveController(OVRInput.Controller controllerType, OVRInput.Controller controllerMask)
	{
		bool result = false;
		if ((controllerType & controllerMask) == controllerType)
		{
			result = true;
		}
		if ((controllerMask & OVRInput.Controller.Touch) == OVRInput.Controller.Touch && (controllerType & OVRInput.Controller.Touch) != OVRInput.Controller.None && (controllerType & OVRInput.Controller.Touch) != OVRInput.Controller.Touch)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x04002A16 RID: 10774
	private static readonly float AXIS_AS_BUTTON_THRESHOLD = 0.5f;

	// Token: 0x04002A17 RID: 10775
	private static readonly float AXIS_DEADZONE_THRESHOLD = 0.2f;

	// Token: 0x04002A18 RID: 10776
	private static List<OVRInput.OVRControllerBase> controllers;

	// Token: 0x04002A19 RID: 10777
	private static OVRInput.Controller activeControllerType = OVRInput.Controller.None;

	// Token: 0x04002A1A RID: 10778
	private static OVRInput.Controller connectedControllerTypes = OVRInput.Controller.None;

	// Token: 0x04002A1B RID: 10779
	private static OVRPlugin.Step stepType = OVRPlugin.Step.Render;

	// Token: 0x04002A1C RID: 10780
	private static int fixedUpdateCount = 0;

	// Token: 0x04002A1D RID: 10781
	private static bool _pluginSupportsActiveController = false;

	// Token: 0x04002A1E RID: 10782
	private static bool _pluginSupportsActiveControllerCached = false;

	// Token: 0x04002A1F RID: 10783
	private static Version _pluginSupportsActiveControllerMinVersion = new Version(1, 9, 0);

	// Token: 0x020008DA RID: 2266
	[Flags]
	public enum Button
	{
		// Token: 0x04002A21 RID: 10785
		None = 0,
		// Token: 0x04002A22 RID: 10786
		One = 1,
		// Token: 0x04002A23 RID: 10787
		Two = 2,
		// Token: 0x04002A24 RID: 10788
		Three = 4,
		// Token: 0x04002A25 RID: 10789
		Four = 8,
		// Token: 0x04002A26 RID: 10790
		Start = 256,
		// Token: 0x04002A27 RID: 10791
		Back = 512,
		// Token: 0x04002A28 RID: 10792
		PrimaryShoulder = 4096,
		// Token: 0x04002A29 RID: 10793
		PrimaryIndexTrigger = 8192,
		// Token: 0x04002A2A RID: 10794
		PrimaryHandTrigger = 16384,
		// Token: 0x04002A2B RID: 10795
		PrimaryThumbstick = 32768,
		// Token: 0x04002A2C RID: 10796
		PrimaryThumbstickUp = 65536,
		// Token: 0x04002A2D RID: 10797
		PrimaryThumbstickDown = 131072,
		// Token: 0x04002A2E RID: 10798
		PrimaryThumbstickLeft = 262144,
		// Token: 0x04002A2F RID: 10799
		PrimaryThumbstickRight = 524288,
		// Token: 0x04002A30 RID: 10800
		PrimaryTouchpad = 1024,
		// Token: 0x04002A31 RID: 10801
		SecondaryShoulder = 1048576,
		// Token: 0x04002A32 RID: 10802
		SecondaryIndexTrigger = 2097152,
		// Token: 0x04002A33 RID: 10803
		SecondaryHandTrigger = 4194304,
		// Token: 0x04002A34 RID: 10804
		SecondaryThumbstick = 8388608,
		// Token: 0x04002A35 RID: 10805
		SecondaryThumbstickUp = 16777216,
		// Token: 0x04002A36 RID: 10806
		SecondaryThumbstickDown = 33554432,
		// Token: 0x04002A37 RID: 10807
		SecondaryThumbstickLeft = 67108864,
		// Token: 0x04002A38 RID: 10808
		SecondaryThumbstickRight = 134217728,
		// Token: 0x04002A39 RID: 10809
		SecondaryTouchpad = 2048,
		// Token: 0x04002A3A RID: 10810
		DpadUp = 16,
		// Token: 0x04002A3B RID: 10811
		DpadDown = 32,
		// Token: 0x04002A3C RID: 10812
		DpadLeft = 64,
		// Token: 0x04002A3D RID: 10813
		DpadRight = 128,
		// Token: 0x04002A3E RID: 10814
		Up = 268435456,
		// Token: 0x04002A3F RID: 10815
		Down = 536870912,
		// Token: 0x04002A40 RID: 10816
		Left = 1073741824,
		// Token: 0x04002A41 RID: 10817
		Right = -2147483648,
		// Token: 0x04002A42 RID: 10818
		Any = -1
	}

	// Token: 0x020008DB RID: 2267
	[Flags]
	public enum RawButton
	{
		// Token: 0x04002A44 RID: 10820
		None = 0,
		// Token: 0x04002A45 RID: 10821
		A = 1,
		// Token: 0x04002A46 RID: 10822
		B = 2,
		// Token: 0x04002A47 RID: 10823
		X = 256,
		// Token: 0x04002A48 RID: 10824
		Y = 512,
		// Token: 0x04002A49 RID: 10825
		Start = 1048576,
		// Token: 0x04002A4A RID: 10826
		Back = 2097152,
		// Token: 0x04002A4B RID: 10827
		LShoulder = 2048,
		// Token: 0x04002A4C RID: 10828
		LIndexTrigger = 268435456,
		// Token: 0x04002A4D RID: 10829
		LHandTrigger = 536870912,
		// Token: 0x04002A4E RID: 10830
		LThumbstick = 1024,
		// Token: 0x04002A4F RID: 10831
		LThumbstickUp = 16,
		// Token: 0x04002A50 RID: 10832
		LThumbstickDown = 32,
		// Token: 0x04002A51 RID: 10833
		LThumbstickLeft = 64,
		// Token: 0x04002A52 RID: 10834
		LThumbstickRight = 128,
		// Token: 0x04002A53 RID: 10835
		LTouchpad = 1073741824,
		// Token: 0x04002A54 RID: 10836
		RShoulder = 8,
		// Token: 0x04002A55 RID: 10837
		RIndexTrigger = 67108864,
		// Token: 0x04002A56 RID: 10838
		RHandTrigger = 134217728,
		// Token: 0x04002A57 RID: 10839
		RThumbstick = 4,
		// Token: 0x04002A58 RID: 10840
		RThumbstickUp = 4096,
		// Token: 0x04002A59 RID: 10841
		RThumbstickDown = 8192,
		// Token: 0x04002A5A RID: 10842
		RThumbstickLeft = 16384,
		// Token: 0x04002A5B RID: 10843
		RThumbstickRight = 32768,
		// Token: 0x04002A5C RID: 10844
		RTouchpad = -2147483648,
		// Token: 0x04002A5D RID: 10845
		DpadUp = 65536,
		// Token: 0x04002A5E RID: 10846
		DpadDown = 131072,
		// Token: 0x04002A5F RID: 10847
		DpadLeft = 262144,
		// Token: 0x04002A60 RID: 10848
		DpadRight = 524288,
		// Token: 0x04002A61 RID: 10849
		Any = -1
	}

	// Token: 0x020008DC RID: 2268
	[Flags]
	public enum Touch
	{
		// Token: 0x04002A63 RID: 10851
		None = 0,
		// Token: 0x04002A64 RID: 10852
		One = 1,
		// Token: 0x04002A65 RID: 10853
		Two = 2,
		// Token: 0x04002A66 RID: 10854
		Three = 4,
		// Token: 0x04002A67 RID: 10855
		Four = 8,
		// Token: 0x04002A68 RID: 10856
		PrimaryIndexTrigger = 8192,
		// Token: 0x04002A69 RID: 10857
		PrimaryThumbstick = 32768,
		// Token: 0x04002A6A RID: 10858
		PrimaryThumbRest = 4096,
		// Token: 0x04002A6B RID: 10859
		PrimaryTouchpad = 1024,
		// Token: 0x04002A6C RID: 10860
		SecondaryIndexTrigger = 2097152,
		// Token: 0x04002A6D RID: 10861
		SecondaryThumbstick = 8388608,
		// Token: 0x04002A6E RID: 10862
		SecondaryThumbRest = 1048576,
		// Token: 0x04002A6F RID: 10863
		SecondaryTouchpad = 2048,
		// Token: 0x04002A70 RID: 10864
		Any = -1
	}

	// Token: 0x020008DD RID: 2269
	[Flags]
	public enum RawTouch
	{
		// Token: 0x04002A72 RID: 10866
		None = 0,
		// Token: 0x04002A73 RID: 10867
		A = 1,
		// Token: 0x04002A74 RID: 10868
		B = 2,
		// Token: 0x04002A75 RID: 10869
		X = 256,
		// Token: 0x04002A76 RID: 10870
		Y = 512,
		// Token: 0x04002A77 RID: 10871
		LIndexTrigger = 4096,
		// Token: 0x04002A78 RID: 10872
		LThumbstick = 1024,
		// Token: 0x04002A79 RID: 10873
		LThumbRest = 2048,
		// Token: 0x04002A7A RID: 10874
		LTouchpad = 1073741824,
		// Token: 0x04002A7B RID: 10875
		RIndexTrigger = 16,
		// Token: 0x04002A7C RID: 10876
		RThumbstick = 4,
		// Token: 0x04002A7D RID: 10877
		RThumbRest = 8,
		// Token: 0x04002A7E RID: 10878
		RTouchpad = -2147483648,
		// Token: 0x04002A7F RID: 10879
		Any = -1
	}

	// Token: 0x020008DE RID: 2270
	[Flags]
	public enum NearTouch
	{
		// Token: 0x04002A81 RID: 10881
		None = 0,
		// Token: 0x04002A82 RID: 10882
		PrimaryIndexTrigger = 1,
		// Token: 0x04002A83 RID: 10883
		PrimaryThumbButtons = 2,
		// Token: 0x04002A84 RID: 10884
		SecondaryIndexTrigger = 4,
		// Token: 0x04002A85 RID: 10885
		SecondaryThumbButtons = 8,
		// Token: 0x04002A86 RID: 10886
		Any = -1
	}

	// Token: 0x020008DF RID: 2271
	[Flags]
	public enum RawNearTouch
	{
		// Token: 0x04002A88 RID: 10888
		None = 0,
		// Token: 0x04002A89 RID: 10889
		LIndexTrigger = 1,
		// Token: 0x04002A8A RID: 10890
		LThumbButtons = 2,
		// Token: 0x04002A8B RID: 10891
		RIndexTrigger = 4,
		// Token: 0x04002A8C RID: 10892
		RThumbButtons = 8,
		// Token: 0x04002A8D RID: 10893
		Any = -1
	}

	// Token: 0x020008E0 RID: 2272
	[Flags]
	public enum Axis1D
	{
		// Token: 0x04002A8F RID: 10895
		None = 0,
		// Token: 0x04002A90 RID: 10896
		PrimaryIndexTrigger = 1,
		// Token: 0x04002A91 RID: 10897
		PrimaryHandTrigger = 4,
		// Token: 0x04002A92 RID: 10898
		SecondaryIndexTrigger = 2,
		// Token: 0x04002A93 RID: 10899
		SecondaryHandTrigger = 8,
		// Token: 0x04002A94 RID: 10900
		Any = -1
	}

	// Token: 0x020008E1 RID: 2273
	[Flags]
	public enum RawAxis1D
	{
		// Token: 0x04002A96 RID: 10902
		None = 0,
		// Token: 0x04002A97 RID: 10903
		LIndexTrigger = 1,
		// Token: 0x04002A98 RID: 10904
		LHandTrigger = 4,
		// Token: 0x04002A99 RID: 10905
		RIndexTrigger = 2,
		// Token: 0x04002A9A RID: 10906
		RHandTrigger = 8,
		// Token: 0x04002A9B RID: 10907
		Any = -1
	}

	// Token: 0x020008E2 RID: 2274
	[Flags]
	public enum Axis2D
	{
		// Token: 0x04002A9D RID: 10909
		None = 0,
		// Token: 0x04002A9E RID: 10910
		PrimaryThumbstick = 1,
		// Token: 0x04002A9F RID: 10911
		PrimaryTouchpad = 4,
		// Token: 0x04002AA0 RID: 10912
		SecondaryThumbstick = 2,
		// Token: 0x04002AA1 RID: 10913
		SecondaryTouchpad = 8,
		// Token: 0x04002AA2 RID: 10914
		Any = -1
	}

	// Token: 0x020008E3 RID: 2275
	[Flags]
	public enum RawAxis2D
	{
		// Token: 0x04002AA4 RID: 10916
		None = 0,
		// Token: 0x04002AA5 RID: 10917
		LThumbstick = 1,
		// Token: 0x04002AA6 RID: 10918
		LTouchpad = 4,
		// Token: 0x04002AA7 RID: 10919
		RThumbstick = 2,
		// Token: 0x04002AA8 RID: 10920
		RTouchpad = 8,
		// Token: 0x04002AA9 RID: 10921
		Any = -1
	}

	// Token: 0x020008E4 RID: 2276
	[Flags]
	public enum Controller
	{
		// Token: 0x04002AAB RID: 10923
		None = 0,
		// Token: 0x04002AAC RID: 10924
		LTouch = 1,
		// Token: 0x04002AAD RID: 10925
		RTouch = 2,
		// Token: 0x04002AAE RID: 10926
		Touch = 3,
		// Token: 0x04002AAF RID: 10927
		Remote = 4,
		// Token: 0x04002AB0 RID: 10928
		Gamepad = 16,
		// Token: 0x04002AB1 RID: 10929
		Touchpad = 134217728,
		// Token: 0x04002AB2 RID: 10930
		LTrackedRemote = 16777216,
		// Token: 0x04002AB3 RID: 10931
		RTrackedRemote = 33554432,
		// Token: 0x04002AB4 RID: 10932
		Active = -2147483648,
		// Token: 0x04002AB5 RID: 10933
		All = -1
	}

	// Token: 0x020008E5 RID: 2277
	private abstract class OVRControllerBase
	{
		// Token: 0x0600393F RID: 14655 RVA: 0x00116914 File Offset: 0x00114D14
		public OVRControllerBase()
		{
			this.ConfigureButtonMap();
			this.ConfigureTouchMap();
			this.ConfigureNearTouchMap();
			this.ConfigureAxis1DMap();
			this.ConfigureAxis2DMap();
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x001169A4 File Offset: 0x00114DA4
		public virtual OVRInput.Controller Update()
		{
			OVRPlugin.ControllerState4 controllerState = OVRPlugin.GetControllerState4((uint)this.controllerType);
			if (controllerState.LIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 268435456U;
			}
			if (controllerState.LHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 536870912U;
			}
			if (controllerState.LThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 16U;
			}
			if (controllerState.LThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 32U;
			}
			if (controllerState.LThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 64U;
			}
			if (controllerState.LThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 128U;
			}
			if (controllerState.RIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 67108864U;
			}
			if (controllerState.RHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 134217728U;
			}
			if (controllerState.RThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 4096U;
			}
			if (controllerState.RThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 8192U;
			}
			if (controllerState.RThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 16384U;
			}
			if (controllerState.RThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				controllerState.Buttons |= 32768U;
			}
			this.previousState = this.currentState;
			this.currentState = controllerState;
			return (OVRInput.Controller)(this.currentState.ConnectedControllers & (uint)this.controllerType);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x00116BB5 File Offset: 0x00114FB5
		public virtual void SetControllerVibration(float frequency, float amplitude)
		{
			OVRPlugin.SetControllerVibration((uint)this.controllerType, frequency, amplitude);
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x00116BC5 File Offset: 0x00114FC5
		public virtual void RecenterController()
		{
			OVRPlugin.RecenterTrackingOrigin(OVRPlugin.RecenterFlags.Controllers);
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x00116BD2 File Offset: 0x00114FD2
		public virtual bool WasRecentered()
		{
			return false;
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x00116BD5 File Offset: 0x00114FD5
		public virtual byte GetRecenterCount()
		{
			return 0;
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x00116BD8 File Offset: 0x00114FD8
		public virtual byte GetBatteryPercentRemaining()
		{
			return 0;
		}

		// Token: 0x06003946 RID: 14662
		public abstract void ConfigureButtonMap();

		// Token: 0x06003947 RID: 14663
		public abstract void ConfigureTouchMap();

		// Token: 0x06003948 RID: 14664
		public abstract void ConfigureNearTouchMap();

		// Token: 0x06003949 RID: 14665
		public abstract void ConfigureAxis1DMap();

		// Token: 0x0600394A RID: 14666
		public abstract void ConfigureAxis2DMap();

		// Token: 0x0600394B RID: 14667 RVA: 0x00116BDB File Offset: 0x00114FDB
		public OVRInput.RawButton ResolveToRawMask(OVRInput.Button virtualMask)
		{
			return this.buttonMap.ToRawMask(virtualMask);
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x00116BE9 File Offset: 0x00114FE9
		public OVRInput.RawTouch ResolveToRawMask(OVRInput.Touch virtualMask)
		{
			return this.touchMap.ToRawMask(virtualMask);
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x00116BF7 File Offset: 0x00114FF7
		public OVRInput.RawNearTouch ResolveToRawMask(OVRInput.NearTouch virtualMask)
		{
			return this.nearTouchMap.ToRawMask(virtualMask);
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x00116C05 File Offset: 0x00115005
		public OVRInput.RawAxis1D ResolveToRawMask(OVRInput.Axis1D virtualMask)
		{
			return this.axis1DMap.ToRawMask(virtualMask);
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x00116C13 File Offset: 0x00115013
		public OVRInput.RawAxis2D ResolveToRawMask(OVRInput.Axis2D virtualMask)
		{
			return this.axis2DMap.ToRawMask(virtualMask);
		}

		// Token: 0x04002AB6 RID: 10934
		public OVRInput.Controller controllerType;

		// Token: 0x04002AB7 RID: 10935
		public OVRInput.OVRControllerBase.VirtualButtonMap buttonMap = new OVRInput.OVRControllerBase.VirtualButtonMap();

		// Token: 0x04002AB8 RID: 10936
		public OVRInput.OVRControllerBase.VirtualTouchMap touchMap = new OVRInput.OVRControllerBase.VirtualTouchMap();

		// Token: 0x04002AB9 RID: 10937
		public OVRInput.OVRControllerBase.VirtualNearTouchMap nearTouchMap = new OVRInput.OVRControllerBase.VirtualNearTouchMap();

		// Token: 0x04002ABA RID: 10938
		public OVRInput.OVRControllerBase.VirtualAxis1DMap axis1DMap = new OVRInput.OVRControllerBase.VirtualAxis1DMap();

		// Token: 0x04002ABB RID: 10939
		public OVRInput.OVRControllerBase.VirtualAxis2DMap axis2DMap = new OVRInput.OVRControllerBase.VirtualAxis2DMap();

		// Token: 0x04002ABC RID: 10940
		public OVRPlugin.ControllerState4 previousState = default(OVRPlugin.ControllerState4);

		// Token: 0x04002ABD RID: 10941
		public OVRPlugin.ControllerState4 currentState = default(OVRPlugin.ControllerState4);

		// Token: 0x04002ABE RID: 10942
		public bool shouldApplyDeadzone = true;

		// Token: 0x020008E6 RID: 2278
		public class VirtualButtonMap
		{
			// Token: 0x06003950 RID: 14672 RVA: 0x00116C21 File Offset: 0x00115021
			public VirtualButtonMap()
			{
			}

			// Token: 0x06003951 RID: 14673 RVA: 0x00116C2C File Offset: 0x0011502C
			public OVRInput.RawButton ToRawMask(OVRInput.Button virtualMask)
			{
				OVRInput.RawButton rawButton = OVRInput.RawButton.None;
				if (virtualMask == OVRInput.Button.None)
				{
					return OVRInput.RawButton.None;
				}
				if ((virtualMask & OVRInput.Button.One) != OVRInput.Button.None)
				{
					rawButton |= this.One;
				}
				if ((virtualMask & OVRInput.Button.Two) != OVRInput.Button.None)
				{
					rawButton |= this.Two;
				}
				if ((virtualMask & OVRInput.Button.Three) != OVRInput.Button.None)
				{
					rawButton |= this.Three;
				}
				if ((virtualMask & OVRInput.Button.Four) != OVRInput.Button.None)
				{
					rawButton |= this.Four;
				}
				if ((virtualMask & OVRInput.Button.Start) != OVRInput.Button.None)
				{
					rawButton |= this.Start;
				}
				if ((virtualMask & OVRInput.Button.Back) != OVRInput.Button.None)
				{
					rawButton |= this.Back;
				}
				if ((virtualMask & OVRInput.Button.PrimaryShoulder) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryShoulder;
				}
				if ((virtualMask & OVRInput.Button.PrimaryIndexTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Button.PrimaryHandTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryHandTrigger;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstick) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstick;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickUp) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickUp;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickDown) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickDown;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickLeft) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickLeft;
				}
				if ((virtualMask & OVRInput.Button.PrimaryThumbstickRight) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryThumbstickRight;
				}
				if ((virtualMask & OVRInput.Button.PrimaryTouchpad) != OVRInput.Button.None)
				{
					rawButton |= this.PrimaryTouchpad;
				}
				if ((virtualMask & OVRInput.Button.SecondaryShoulder) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryShoulder;
				}
				if ((virtualMask & OVRInput.Button.SecondaryIndexTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Button.SecondaryHandTrigger) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryHandTrigger;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstick) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstick;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickUp) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickUp;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickDown) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickDown;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickLeft) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickLeft;
				}
				if ((virtualMask & OVRInput.Button.SecondaryThumbstickRight) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryThumbstickRight;
				}
				if ((virtualMask & OVRInput.Button.SecondaryTouchpad) != OVRInput.Button.None)
				{
					rawButton |= this.SecondaryTouchpad;
				}
				if ((virtualMask & OVRInput.Button.DpadUp) != OVRInput.Button.None)
				{
					rawButton |= this.DpadUp;
				}
				if ((virtualMask & OVRInput.Button.DpadDown) != OVRInput.Button.None)
				{
					rawButton |= this.DpadDown;
				}
				if ((virtualMask & OVRInput.Button.DpadLeft) != OVRInput.Button.None)
				{
					rawButton |= this.DpadLeft;
				}
				if ((virtualMask & OVRInput.Button.DpadRight) != OVRInput.Button.None)
				{
					rawButton |= this.DpadRight;
				}
				if ((virtualMask & OVRInput.Button.Up) != OVRInput.Button.None)
				{
					rawButton |= this.Up;
				}
				if ((virtualMask & OVRInput.Button.Down) != OVRInput.Button.None)
				{
					rawButton |= this.Down;
				}
				if ((virtualMask & OVRInput.Button.Left) != OVRInput.Button.None)
				{
					rawButton |= this.Left;
				}
				if ((virtualMask & OVRInput.Button.Right) != OVRInput.Button.None)
				{
					rawButton |= this.Right;
				}
				return rawButton;
			}

			// Token: 0x04002ABF RID: 10943
			public OVRInput.RawButton None;

			// Token: 0x04002AC0 RID: 10944
			public OVRInput.RawButton One;

			// Token: 0x04002AC1 RID: 10945
			public OVRInput.RawButton Two;

			// Token: 0x04002AC2 RID: 10946
			public OVRInput.RawButton Three;

			// Token: 0x04002AC3 RID: 10947
			public OVRInput.RawButton Four;

			// Token: 0x04002AC4 RID: 10948
			public OVRInput.RawButton Start;

			// Token: 0x04002AC5 RID: 10949
			public OVRInput.RawButton Back;

			// Token: 0x04002AC6 RID: 10950
			public OVRInput.RawButton PrimaryShoulder;

			// Token: 0x04002AC7 RID: 10951
			public OVRInput.RawButton PrimaryIndexTrigger;

			// Token: 0x04002AC8 RID: 10952
			public OVRInput.RawButton PrimaryHandTrigger;

			// Token: 0x04002AC9 RID: 10953
			public OVRInput.RawButton PrimaryThumbstick;

			// Token: 0x04002ACA RID: 10954
			public OVRInput.RawButton PrimaryThumbstickUp;

			// Token: 0x04002ACB RID: 10955
			public OVRInput.RawButton PrimaryThumbstickDown;

			// Token: 0x04002ACC RID: 10956
			public OVRInput.RawButton PrimaryThumbstickLeft;

			// Token: 0x04002ACD RID: 10957
			public OVRInput.RawButton PrimaryThumbstickRight;

			// Token: 0x04002ACE RID: 10958
			public OVRInput.RawButton PrimaryTouchpad;

			// Token: 0x04002ACF RID: 10959
			public OVRInput.RawButton SecondaryShoulder;

			// Token: 0x04002AD0 RID: 10960
			public OVRInput.RawButton SecondaryIndexTrigger;

			// Token: 0x04002AD1 RID: 10961
			public OVRInput.RawButton SecondaryHandTrigger;

			// Token: 0x04002AD2 RID: 10962
			public OVRInput.RawButton SecondaryThumbstick;

			// Token: 0x04002AD3 RID: 10963
			public OVRInput.RawButton SecondaryThumbstickUp;

			// Token: 0x04002AD4 RID: 10964
			public OVRInput.RawButton SecondaryThumbstickDown;

			// Token: 0x04002AD5 RID: 10965
			public OVRInput.RawButton SecondaryThumbstickLeft;

			// Token: 0x04002AD6 RID: 10966
			public OVRInput.RawButton SecondaryThumbstickRight;

			// Token: 0x04002AD7 RID: 10967
			public OVRInput.RawButton SecondaryTouchpad;

			// Token: 0x04002AD8 RID: 10968
			public OVRInput.RawButton DpadUp;

			// Token: 0x04002AD9 RID: 10969
			public OVRInput.RawButton DpadDown;

			// Token: 0x04002ADA RID: 10970
			public OVRInput.RawButton DpadLeft;

			// Token: 0x04002ADB RID: 10971
			public OVRInput.RawButton DpadRight;

			// Token: 0x04002ADC RID: 10972
			public OVRInput.RawButton Up;

			// Token: 0x04002ADD RID: 10973
			public OVRInput.RawButton Down;

			// Token: 0x04002ADE RID: 10974
			public OVRInput.RawButton Left;

			// Token: 0x04002ADF RID: 10975
			public OVRInput.RawButton Right;
		}

		// Token: 0x020008E7 RID: 2279
		public class VirtualTouchMap
		{
			// Token: 0x06003952 RID: 14674 RVA: 0x00116ECB File Offset: 0x001152CB
			public VirtualTouchMap()
			{
			}

			// Token: 0x06003953 RID: 14675 RVA: 0x00116ED4 File Offset: 0x001152D4
			public OVRInput.RawTouch ToRawMask(OVRInput.Touch virtualMask)
			{
				OVRInput.RawTouch rawTouch = OVRInput.RawTouch.None;
				if (virtualMask == OVRInput.Touch.None)
				{
					return OVRInput.RawTouch.None;
				}
				if ((virtualMask & OVRInput.Touch.One) != OVRInput.Touch.None)
				{
					rawTouch |= this.One;
				}
				if ((virtualMask & OVRInput.Touch.Two) != OVRInput.Touch.None)
				{
					rawTouch |= this.Two;
				}
				if ((virtualMask & OVRInput.Touch.Three) != OVRInput.Touch.None)
				{
					rawTouch |= this.Three;
				}
				if ((virtualMask & OVRInput.Touch.Four) != OVRInput.Touch.None)
				{
					rawTouch |= this.Four;
				}
				if ((virtualMask & OVRInput.Touch.PrimaryIndexTrigger) != OVRInput.Touch.None)
				{
					rawTouch |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Touch.PrimaryThumbstick) != OVRInput.Touch.None)
				{
					rawTouch |= this.PrimaryThumbstick;
				}
				if ((virtualMask & OVRInput.Touch.PrimaryThumbRest) != OVRInput.Touch.None)
				{
					rawTouch |= this.PrimaryThumbRest;
				}
				if ((virtualMask & OVRInput.Touch.PrimaryTouchpad) != OVRInput.Touch.None)
				{
					rawTouch |= this.PrimaryTouchpad;
				}
				if ((virtualMask & OVRInput.Touch.SecondaryIndexTrigger) != OVRInput.Touch.None)
				{
					rawTouch |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Touch.SecondaryThumbstick) != OVRInput.Touch.None)
				{
					rawTouch |= this.SecondaryThumbstick;
				}
				if ((virtualMask & OVRInput.Touch.SecondaryThumbRest) != OVRInput.Touch.None)
				{
					rawTouch |= this.SecondaryThumbRest;
				}
				if ((virtualMask & OVRInput.Touch.SecondaryTouchpad) != OVRInput.Touch.None)
				{
					rawTouch |= this.SecondaryTouchpad;
				}
				return rawTouch;
			}

			// Token: 0x04002AE0 RID: 10976
			public OVRInput.RawTouch None;

			// Token: 0x04002AE1 RID: 10977
			public OVRInput.RawTouch One;

			// Token: 0x04002AE2 RID: 10978
			public OVRInput.RawTouch Two;

			// Token: 0x04002AE3 RID: 10979
			public OVRInput.RawTouch Three;

			// Token: 0x04002AE4 RID: 10980
			public OVRInput.RawTouch Four;

			// Token: 0x04002AE5 RID: 10981
			public OVRInput.RawTouch PrimaryIndexTrigger;

			// Token: 0x04002AE6 RID: 10982
			public OVRInput.RawTouch PrimaryThumbstick;

			// Token: 0x04002AE7 RID: 10983
			public OVRInput.RawTouch PrimaryThumbRest;

			// Token: 0x04002AE8 RID: 10984
			public OVRInput.RawTouch PrimaryTouchpad;

			// Token: 0x04002AE9 RID: 10985
			public OVRInput.RawTouch SecondaryIndexTrigger;

			// Token: 0x04002AEA RID: 10986
			public OVRInput.RawTouch SecondaryThumbstick;

			// Token: 0x04002AEB RID: 10987
			public OVRInput.RawTouch SecondaryThumbRest;

			// Token: 0x04002AEC RID: 10988
			public OVRInput.RawTouch SecondaryTouchpad;
		}

		// Token: 0x020008E8 RID: 2280
		public class VirtualNearTouchMap
		{
			// Token: 0x06003954 RID: 14676 RVA: 0x00116FD8 File Offset: 0x001153D8
			public VirtualNearTouchMap()
			{
			}

			// Token: 0x06003955 RID: 14677 RVA: 0x00116FE0 File Offset: 0x001153E0
			public OVRInput.RawNearTouch ToRawMask(OVRInput.NearTouch virtualMask)
			{
				OVRInput.RawNearTouch rawNearTouch = OVRInput.RawNearTouch.None;
				if (virtualMask == OVRInput.NearTouch.None)
				{
					return OVRInput.RawNearTouch.None;
				}
				if ((virtualMask & OVRInput.NearTouch.PrimaryIndexTrigger) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.NearTouch.PrimaryThumbButtons) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.PrimaryThumbButtons;
				}
				if ((virtualMask & OVRInput.NearTouch.SecondaryIndexTrigger) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.NearTouch.SecondaryThumbButtons) != OVRInput.NearTouch.None)
				{
					rawNearTouch |= this.SecondaryThumbButtons;
				}
				return rawNearTouch;
			}

			// Token: 0x04002AED RID: 10989
			public OVRInput.RawNearTouch None;

			// Token: 0x04002AEE RID: 10990
			public OVRInput.RawNearTouch PrimaryIndexTrigger;

			// Token: 0x04002AEF RID: 10991
			public OVRInput.RawNearTouch PrimaryThumbButtons;

			// Token: 0x04002AF0 RID: 10992
			public OVRInput.RawNearTouch SecondaryIndexTrigger;

			// Token: 0x04002AF1 RID: 10993
			public OVRInput.RawNearTouch SecondaryThumbButtons;
		}

		// Token: 0x020008E9 RID: 2281
		public class VirtualAxis1DMap
		{
			// Token: 0x06003956 RID: 14678 RVA: 0x0011703C File Offset: 0x0011543C
			public VirtualAxis1DMap()
			{
			}

			// Token: 0x06003957 RID: 14679 RVA: 0x00117044 File Offset: 0x00115444
			public OVRInput.RawAxis1D ToRawMask(OVRInput.Axis1D virtualMask)
			{
				OVRInput.RawAxis1D rawAxis1D = OVRInput.RawAxis1D.None;
				if (virtualMask == OVRInput.Axis1D.None)
				{
					return OVRInput.RawAxis1D.None;
				}
				if ((virtualMask & OVRInput.Axis1D.PrimaryIndexTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.PrimaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Axis1D.PrimaryHandTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.PrimaryHandTrigger;
				}
				if ((virtualMask & OVRInput.Axis1D.SecondaryIndexTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.SecondaryIndexTrigger;
				}
				if ((virtualMask & OVRInput.Axis1D.SecondaryHandTrigger) != OVRInput.Axis1D.None)
				{
					rawAxis1D |= this.SecondaryHandTrigger;
				}
				return rawAxis1D;
			}

			// Token: 0x04002AF2 RID: 10994
			public OVRInput.RawAxis1D None;

			// Token: 0x04002AF3 RID: 10995
			public OVRInput.RawAxis1D PrimaryIndexTrigger;

			// Token: 0x04002AF4 RID: 10996
			public OVRInput.RawAxis1D PrimaryHandTrigger;

			// Token: 0x04002AF5 RID: 10997
			public OVRInput.RawAxis1D SecondaryIndexTrigger;

			// Token: 0x04002AF6 RID: 10998
			public OVRInput.RawAxis1D SecondaryHandTrigger;
		}

		// Token: 0x020008EA RID: 2282
		public class VirtualAxis2DMap
		{
			// Token: 0x06003958 RID: 14680 RVA: 0x001170A0 File Offset: 0x001154A0
			public VirtualAxis2DMap()
			{
			}

			// Token: 0x06003959 RID: 14681 RVA: 0x001170A8 File Offset: 0x001154A8
			public OVRInput.RawAxis2D ToRawMask(OVRInput.Axis2D virtualMask)
			{
				OVRInput.RawAxis2D rawAxis2D = OVRInput.RawAxis2D.None;
				if (virtualMask == OVRInput.Axis2D.None)
				{
					return OVRInput.RawAxis2D.None;
				}
				if ((virtualMask & OVRInput.Axis2D.PrimaryThumbstick) != OVRInput.Axis2D.None)
				{
					rawAxis2D |= this.PrimaryThumbstick;
				}
				if ((virtualMask & OVRInput.Axis2D.PrimaryTouchpad) != OVRInput.Axis2D.None)
				{
					rawAxis2D |= this.PrimaryTouchpad;
				}
				if ((virtualMask & OVRInput.Axis2D.SecondaryThumbstick) != OVRInput.Axis2D.None)
				{
					rawAxis2D |= this.SecondaryThumbstick;
				}
				if ((virtualMask & OVRInput.Axis2D.SecondaryTouchpad) != OVRInput.Axis2D.None)
				{
					rawAxis2D |= this.SecondaryTouchpad;
				}
				return rawAxis2D;
			}

			// Token: 0x04002AF7 RID: 10999
			public OVRInput.RawAxis2D None;

			// Token: 0x04002AF8 RID: 11000
			public OVRInput.RawAxis2D PrimaryThumbstick;

			// Token: 0x04002AF9 RID: 11001
			public OVRInput.RawAxis2D PrimaryTouchpad;

			// Token: 0x04002AFA RID: 11002
			public OVRInput.RawAxis2D SecondaryThumbstick;

			// Token: 0x04002AFB RID: 11003
			public OVRInput.RawAxis2D SecondaryTouchpad;
		}
	}

	// Token: 0x020008EB RID: 2283
	private class OVRControllerTouch : OVRInput.OVRControllerBase
	{
		// Token: 0x0600395A RID: 14682 RVA: 0x00117104 File Offset: 0x00115504
		public OVRControllerTouch()
		{
			this.controllerType = OVRInput.Controller.Touch;
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x00117114 File Offset: 0x00115514
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.None;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.RHandTrigger;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.None;
			this.buttonMap.DpadDown = OVRInput.RawButton.None;
			this.buttonMap.DpadLeft = OVRInput.RawButton.None;
			this.buttonMap.DpadRight = OVRInput.RawButton.None;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x001172EC File Offset: 0x001156EC
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.A;
			this.touchMap.Two = OVRInput.RawTouch.B;
			this.touchMap.Three = OVRInput.RawTouch.X;
			this.touchMap.Four = OVRInput.RawTouch.Y;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.LThumbstick;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.LThumbRest;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.RThumbstick;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.RThumbRest;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x001173AA File Offset: 0x001157AA
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.LIndexTrigger;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.LThumbButtons;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.RIndexTrigger;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.RThumbButtons;
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x001173E8 File Offset: 0x001157E8
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x00117426 File Offset: 0x00115826
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x020008EC RID: 2284
	private class OVRControllerLTouch : OVRInput.OVRControllerBase
	{
		// Token: 0x06003960 RID: 14688 RVA: 0x00117464 File Offset: 0x00115864
		public OVRControllerLTouch()
		{
			this.controllerType = OVRInput.Controller.LTouch;
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x00117474 File Offset: 0x00115874
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.X;
			this.buttonMap.Two = OVRInput.RawButton.Y;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.None;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.None;
			this.buttonMap.DpadDown = OVRInput.RawButton.None;
			this.buttonMap.DpadLeft = OVRInput.RawButton.None;
			this.buttonMap.DpadRight = OVRInput.RawButton.None;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x00117634 File Offset: 0x00115A34
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.X;
			this.touchMap.Two = OVRInput.RawTouch.Y;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.LThumbstick;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.LThumbRest;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x001176F1 File Offset: 0x00115AF1
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.LIndexTrigger;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.LThumbButtons;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0011772F File Offset: 0x00115B2F
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0011776D File Offset: 0x00115B6D
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x020008ED RID: 2285
	private class OVRControllerRTouch : OVRInput.OVRControllerBase
	{
		// Token: 0x06003966 RID: 14694 RVA: 0x001177AB File Offset: 0x00115BAB
		public OVRControllerRTouch()
		{
			this.controllerType = OVRInput.Controller.RTouch;
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x001177BC File Offset: 0x00115BBC
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.None;
			this.buttonMap.Back = OVRInput.RawButton.None;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.RHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.None;
			this.buttonMap.DpadDown = OVRInput.RawButton.None;
			this.buttonMap.DpadLeft = OVRInput.RawButton.None;
			this.buttonMap.DpadRight = OVRInput.RawButton.None;
			this.buttonMap.Up = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.RThumbstickRight;
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x00117980 File Offset: 0x00115D80
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.A;
			this.touchMap.Two = OVRInput.RawTouch.B;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.RThumbstick;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.RThumbRest;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x00117A2A File Offset: 0x00115E2A
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.RIndexTrigger;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.RThumbButtons;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x00117A68 File Offset: 0x00115E68
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x00117AA6 File Offset: 0x00115EA6
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x020008EE RID: 2286
	private class OVRControllerRemote : OVRInput.OVRControllerBase
	{
		// Token: 0x0600396C RID: 14700 RVA: 0x00117AE4 File Offset: 0x00115EE4
		public OVRControllerRemote()
		{
			this.controllerType = OVRInput.Controller.Remote;
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x00117AF4 File Offset: 0x00115EF4
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.Start;
			this.buttonMap.Two = OVRInput.RawButton.Back;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.DpadUp;
			this.buttonMap.Down = OVRInput.RawButton.DpadDown;
			this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
			this.buttonMap.Right = OVRInput.RawButton.DpadRight;
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x00117CC0 File Offset: 0x001160C0
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x00117D69 File Offset: 0x00116169
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x00117DA7 File Offset: 0x001161A7
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x00117DE5 File Offset: 0x001161E5
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x020008EF RID: 2287
	private class OVRControllerGamepadPC : OVRInput.OVRControllerBase
	{
		// Token: 0x06003972 RID: 14706 RVA: 0x00117E23 File Offset: 0x00116223
		public OVRControllerGamepadPC()
		{
			this.controllerType = OVRInput.Controller.Gamepad;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00117E34 File Offset: 0x00116234
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x0011801C File Offset: 0x0011641C
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x001180C5 File Offset: 0x001164C5
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x00118103 File Offset: 0x00116503
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x00118141 File Offset: 0x00116541
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x020008F0 RID: 2288
	private class OVRControllerGamepadMac : OVRInput.OVRControllerBase
	{
		// Token: 0x06003978 RID: 14712 RVA: 0x0011817F File Offset: 0x0011657F
		public OVRControllerGamepadMac()
		{
			this.controllerType = OVRInput.Controller.Gamepad;
			this.initialized = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Initialize();
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x0011819C File Offset: 0x0011659C
		~OVRControllerGamepadMac()
		{
			if (this.initialized)
			{
				OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Destroy();
			}
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x001181DC File Offset: 0x001165DC
		public override OVRInput.Controller Update()
		{
			if (!this.initialized)
			{
				return OVRInput.Controller.None;
			}
			OVRPlugin.ControllerState4 currentState = default(OVRPlugin.ControllerState4);
			bool flag = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_Update();
			if (flag)
			{
				currentState.ConnectedControllers = 16U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(0))
			{
				currentState.Buttons |= 1U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(1))
			{
				currentState.Buttons |= 2U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(2))
			{
				currentState.Buttons |= 256U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(3))
			{
				currentState.Buttons |= 512U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(4))
			{
				currentState.Buttons |= 65536U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(5))
			{
				currentState.Buttons |= 131072U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(6))
			{
				currentState.Buttons |= 262144U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(7))
			{
				currentState.Buttons |= 524288U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(8))
			{
				currentState.Buttons |= 1048576U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(9))
			{
				currentState.Buttons |= 2097152U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(10))
			{
				currentState.Buttons |= 1024U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(11))
			{
				currentState.Buttons |= 4U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(12))
			{
				currentState.Buttons |= 2048U;
			}
			if (OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetButton(13))
			{
				currentState.Buttons |= 8U;
			}
			currentState.LThumbstick.x = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(0);
			currentState.LThumbstick.y = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(1);
			currentState.RThumbstick.x = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(2);
			currentState.RThumbstick.y = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(3);
			currentState.LIndexTrigger = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(4);
			currentState.RIndexTrigger = OVRInput.OVRControllerGamepadMac.OVR_GamepadController_GetAxis(5);
			if (currentState.LIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 268435456U;
			}
			if (currentState.LHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 536870912U;
			}
			if (currentState.LThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 16U;
			}
			if (currentState.LThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 32U;
			}
			if (currentState.LThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 64U;
			}
			if (currentState.LThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 128U;
			}
			if (currentState.RIndexTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 67108864U;
			}
			if (currentState.RHandTrigger >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 134217728U;
			}
			if (currentState.RThumbstick.y >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 4096U;
			}
			if (currentState.RThumbstick.y <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 8192U;
			}
			if (currentState.RThumbstick.x <= -OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 16384U;
			}
			if (currentState.RThumbstick.x >= OVRInput.AXIS_AS_BUTTON_THRESHOLD)
			{
				currentState.Buttons |= 32768U;
			}
			this.previousState = this.currentState;
			this.currentState = currentState;
			return (OVRInput.Controller)(this.currentState.ConnectedControllers & (uint)this.controllerType);
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x00118608 File Offset: 0x00116A08
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x001187F0 File Offset: 0x00116BF0
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x00118899 File Offset: 0x00116C99
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x001188D7 File Offset: 0x00116CD7
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x00118915 File Offset: 0x00116D15
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x00118954 File Offset: 0x00116D54
		public override void SetControllerVibration(float frequency, float amplitude)
		{
			int node = 0;
			float frequency2 = frequency * 200f;
			OVRInput.OVRControllerGamepadMac.OVR_GamepadController_SetVibration(node, amplitude, frequency2);
		}

		// Token: 0x06003981 RID: 14721
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_Initialize();

		// Token: 0x06003982 RID: 14722
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_Destroy();

		// Token: 0x06003983 RID: 14723
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_Update();

		// Token: 0x06003984 RID: 14724
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern float OVR_GamepadController_GetAxis(int axis);

		// Token: 0x06003985 RID: 14725
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_GetButton(int button);

		// Token: 0x06003986 RID: 14726
		[DllImport("OVRGamepad", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool OVR_GamepadController_SetVibration(int node, float strength, float frequency);

		// Token: 0x04002AFC RID: 11004
		private bool initialized;

		// Token: 0x04002AFD RID: 11005
		private const string DllName = "OVRGamepad";

		// Token: 0x020008F1 RID: 2289
		private enum AxisGPC
		{
			// Token: 0x04002AFF RID: 11007
			None = -1,
			// Token: 0x04002B00 RID: 11008
			LeftXAxis,
			// Token: 0x04002B01 RID: 11009
			LeftYAxis,
			// Token: 0x04002B02 RID: 11010
			RightXAxis,
			// Token: 0x04002B03 RID: 11011
			RightYAxis,
			// Token: 0x04002B04 RID: 11012
			LeftTrigger,
			// Token: 0x04002B05 RID: 11013
			RightTrigger,
			// Token: 0x04002B06 RID: 11014
			DPad_X_Axis,
			// Token: 0x04002B07 RID: 11015
			DPad_Y_Axis,
			// Token: 0x04002B08 RID: 11016
			Max
		}

		// Token: 0x020008F2 RID: 2290
		public enum ButtonGPC
		{
			// Token: 0x04002B0A RID: 11018
			None = -1,
			// Token: 0x04002B0B RID: 11019
			A,
			// Token: 0x04002B0C RID: 11020
			B,
			// Token: 0x04002B0D RID: 11021
			X,
			// Token: 0x04002B0E RID: 11022
			Y,
			// Token: 0x04002B0F RID: 11023
			Up,
			// Token: 0x04002B10 RID: 11024
			Down,
			// Token: 0x04002B11 RID: 11025
			Left,
			// Token: 0x04002B12 RID: 11026
			Right,
			// Token: 0x04002B13 RID: 11027
			Start,
			// Token: 0x04002B14 RID: 11028
			Back,
			// Token: 0x04002B15 RID: 11029
			LStick,
			// Token: 0x04002B16 RID: 11030
			RStick,
			// Token: 0x04002B17 RID: 11031
			LeftShoulder,
			// Token: 0x04002B18 RID: 11032
			RightShoulder,
			// Token: 0x04002B19 RID: 11033
			Max
		}
	}

	// Token: 0x020008F3 RID: 2291
	private class OVRControllerGamepadAndroid : OVRInput.OVRControllerBase
	{
		// Token: 0x06003987 RID: 14727 RVA: 0x00118976 File Offset: 0x00116D76
		public OVRControllerGamepadAndroid()
		{
			this.controllerType = OVRInput.Controller.Gamepad;
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x00118988 File Offset: 0x00116D88
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.A;
			this.buttonMap.Two = OVRInput.RawButton.B;
			this.buttonMap.Three = OVRInput.RawButton.X;
			this.buttonMap.Four = OVRInput.RawButton.Y;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.LShoulder;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.LThumbstick;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.LThumbstickRight;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.RShoulder;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.RThumbstick;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.RThumbstickUp;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.RThumbstickDown;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.RThumbstickLeft;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.RThumbstickRight;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.LThumbstickUp;
			this.buttonMap.Down = OVRInput.RawButton.LThumbstickDown;
			this.buttonMap.Left = OVRInput.RawButton.LThumbstickLeft;
			this.buttonMap.Right = OVRInput.RawButton.LThumbstickRight;
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x00118B70 File Offset: 0x00116F70
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.None;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.None;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x00118C19 File Offset: 0x00117019
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x00118C57 File Offset: 0x00117057
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x00118C95 File Offset: 0x00117095
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.LThumbstick;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.RThumbstick;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}
	}

	// Token: 0x020008F4 RID: 2292
	private class OVRControllerTouchpad : OVRInput.OVRControllerBase
	{
		// Token: 0x0600398D RID: 14733 RVA: 0x00118CD3 File Offset: 0x001170D3
		public OVRControllerTouchpad()
		{
			this.controllerType = OVRInput.Controller.Touchpad;
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x00118CFC File Offset: 0x001170FC
		public override OVRInput.Controller Update()
		{
			OVRInput.Controller result = base.Update();
			if (OVRInput.GetDown(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.Touchpad))
			{
				this.moveAmount = this.currentState.LTouchpad;
			}
			if (OVRInput.GetUp(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.Touchpad))
			{
				this.moveAmount.x = this.previousState.LTouchpad.x - this.moveAmount.x;
				this.moveAmount.y = this.previousState.LTouchpad.y - this.moveAmount.y;
				Vector2 vector = new Vector2(this.moveAmount.x, this.moveAmount.y);
				float magnitude = vector.magnitude;
				if (magnitude < this.maxTapMagnitude)
				{
					this.currentState.Buttons = (this.currentState.Buttons | 1048576U);
					this.currentState.Buttons = (this.currentState.Buttons | 1073741824U);
				}
				else if (magnitude >= this.minMoveMagnitude)
				{
					vector.Normalize();
					if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
					{
						if (vector.x < 0f)
						{
							this.currentState.Buttons = (this.currentState.Buttons | 262144U);
						}
						else
						{
							this.currentState.Buttons = (this.currentState.Buttons | 524288U);
						}
					}
					else if (vector.y < 0f)
					{
						this.currentState.Buttons = (this.currentState.Buttons | 131072U);
					}
					else
					{
						this.currentState.Buttons = (this.currentState.Buttons | 65536U);
					}
				}
			}
			return result;
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x00118EBC File Offset: 0x001172BC
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.LTouchpad;
			this.buttonMap.Two = OVRInput.RawButton.Back;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.LTouchpad;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.DpadUp;
			this.buttonMap.Down = OVRInput.RawButton.DpadDown;
			this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
			this.buttonMap.Right = OVRInput.RawButton.DpadRight;
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x0011908C File Offset: 0x0011748C
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.LTouchpad;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.LTouchpad;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x0011913D File Offset: 0x0011753D
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x0011917B File Offset: 0x0011757B
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x001191B9 File Offset: 0x001175B9
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.LTouchpad;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}

		// Token: 0x04002B1A RID: 11034
		private OVRPlugin.Vector2f moveAmount;

		// Token: 0x04002B1B RID: 11035
		private float maxTapMagnitude = 0.1f;

		// Token: 0x04002B1C RID: 11036
		private float minMoveMagnitude = 0.15f;
	}

	// Token: 0x020008F5 RID: 2293
	private class OVRControllerLTrackedRemote : OVRInput.OVRControllerBase
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x001191F7 File Offset: 0x001175F7
		public OVRControllerLTrackedRemote()
		{
			this.controllerType = OVRInput.Controller.LTrackedRemote;
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x00119218 File Offset: 0x00117618
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.LTouchpad;
			this.buttonMap.Two = OVRInput.RawButton.Back;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.LIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.LHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.LTouchpad;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.DpadUp;
			this.buttonMap.Down = OVRInput.RawButton.DpadDown;
			this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
			this.buttonMap.Right = OVRInput.RawButton.DpadRight;
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x001193F0 File Offset: 0x001177F0
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.LTouchpad;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.LIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.LTouchpad;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x001194A5 File Offset: 0x001178A5
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x001194E3 File Offset: 0x001178E3
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.LIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.LHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x00119521 File Offset: 0x00117921
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.LTouchpad;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x00119560 File Offset: 0x00117960
		public override OVRInput.Controller Update()
		{
			OVRInput.Controller result = base.Update();
			if (OVRInput.GetDown(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.LTrackedRemote))
			{
				this.emitSwipe = true;
				this.moveAmount = this.currentState.LTouchpad;
			}
			if (OVRInput.GetDown(OVRInput.RawButton.LTouchpad, OVRInput.Controller.LTrackedRemote))
			{
				this.emitSwipe = false;
			}
			if (OVRInput.GetUp(OVRInput.RawTouch.LTouchpad, OVRInput.Controller.LTrackedRemote) && this.emitSwipe)
			{
				this.emitSwipe = false;
				this.moveAmount.x = this.previousState.LTouchpad.x - this.moveAmount.x;
				this.moveAmount.y = this.previousState.LTouchpad.y - this.moveAmount.y;
				Vector2 vector = new Vector2(this.moveAmount.x, this.moveAmount.y);
				if (vector.magnitude >= this.minMoveMagnitude)
				{
					vector.Normalize();
					if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
					{
						if (vector.x < 0f)
						{
							this.currentState.Buttons = (this.currentState.Buttons | 262144U);
						}
						else
						{
							this.currentState.Buttons = (this.currentState.Buttons | 524288U);
						}
					}
					else if (vector.y < 0f)
					{
						this.currentState.Buttons = (this.currentState.Buttons | 131072U);
					}
					else
					{
						this.currentState.Buttons = (this.currentState.Buttons | 65536U);
					}
				}
			}
			return result;
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x00119710 File Offset: 0x00117B10
		public override bool WasRecentered()
		{
			return this.currentState.LRecenterCount != this.previousState.LRecenterCount;
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0011972D File Offset: 0x00117B2D
		public override byte GetRecenterCount()
		{
			return this.currentState.LRecenterCount;
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0011973A File Offset: 0x00117B3A
		public override byte GetBatteryPercentRemaining()
		{
			return this.currentState.LBatteryPercentRemaining;
		}

		// Token: 0x04002B1D RID: 11037
		private bool emitSwipe;

		// Token: 0x04002B1E RID: 11038
		private OVRPlugin.Vector2f moveAmount;

		// Token: 0x04002B1F RID: 11039
		private float minMoveMagnitude = 0.3f;
	}

	// Token: 0x020008F6 RID: 2294
	private class OVRControllerRTrackedRemote : OVRInput.OVRControllerBase
	{
		// Token: 0x0600399E RID: 14750 RVA: 0x00119747 File Offset: 0x00117B47
		public OVRControllerRTrackedRemote()
		{
			this.controllerType = OVRInput.Controller.RTrackedRemote;
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x00119768 File Offset: 0x00117B68
		public override void ConfigureButtonMap()
		{
			this.buttonMap.None = OVRInput.RawButton.None;
			this.buttonMap.One = OVRInput.RawButton.RTouchpad;
			this.buttonMap.Two = OVRInput.RawButton.Back;
			this.buttonMap.Three = OVRInput.RawButton.None;
			this.buttonMap.Four = OVRInput.RawButton.None;
			this.buttonMap.Start = OVRInput.RawButton.Start;
			this.buttonMap.Back = OVRInput.RawButton.Back;
			this.buttonMap.PrimaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.PrimaryIndexTrigger = OVRInput.RawButton.RIndexTrigger;
			this.buttonMap.PrimaryHandTrigger = OVRInput.RawButton.RHandTrigger;
			this.buttonMap.PrimaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.PrimaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.PrimaryTouchpad = OVRInput.RawButton.RTouchpad;
			this.buttonMap.SecondaryShoulder = OVRInput.RawButton.None;
			this.buttonMap.SecondaryIndexTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryHandTrigger = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstick = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickUp = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickDown = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickLeft = OVRInput.RawButton.None;
			this.buttonMap.SecondaryThumbstickRight = OVRInput.RawButton.None;
			this.buttonMap.SecondaryTouchpad = OVRInput.RawButton.None;
			this.buttonMap.DpadUp = OVRInput.RawButton.DpadUp;
			this.buttonMap.DpadDown = OVRInput.RawButton.DpadDown;
			this.buttonMap.DpadLeft = OVRInput.RawButton.DpadLeft;
			this.buttonMap.DpadRight = OVRInput.RawButton.DpadRight;
			this.buttonMap.Up = OVRInput.RawButton.DpadUp;
			this.buttonMap.Down = OVRInput.RawButton.DpadDown;
			this.buttonMap.Left = OVRInput.RawButton.DpadLeft;
			this.buttonMap.Right = OVRInput.RawButton.DpadRight;
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x00119940 File Offset: 0x00117D40
		public override void ConfigureTouchMap()
		{
			this.touchMap.None = OVRInput.RawTouch.None;
			this.touchMap.One = OVRInput.RawTouch.RTouchpad;
			this.touchMap.Two = OVRInput.RawTouch.None;
			this.touchMap.Three = OVRInput.RawTouch.None;
			this.touchMap.Four = OVRInput.RawTouch.None;
			this.touchMap.PrimaryIndexTrigger = OVRInput.RawTouch.RIndexTrigger;
			this.touchMap.PrimaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.PrimaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.PrimaryTouchpad = OVRInput.RawTouch.RTouchpad;
			this.touchMap.SecondaryIndexTrigger = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbstick = OVRInput.RawTouch.None;
			this.touchMap.SecondaryThumbRest = OVRInput.RawTouch.None;
			this.touchMap.SecondaryTouchpad = OVRInput.RawTouch.None;
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x001199F2 File Offset: 0x00117DF2
		public override void ConfigureNearTouchMap()
		{
			this.nearTouchMap.None = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.PrimaryThumbButtons = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryIndexTrigger = OVRInput.RawNearTouch.None;
			this.nearTouchMap.SecondaryThumbButtons = OVRInput.RawNearTouch.None;
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x00119A30 File Offset: 0x00117E30
		public override void ConfigureAxis1DMap()
		{
			this.axis1DMap.None = OVRInput.RawAxis1D.None;
			this.axis1DMap.PrimaryIndexTrigger = OVRInput.RawAxis1D.RIndexTrigger;
			this.axis1DMap.PrimaryHandTrigger = OVRInput.RawAxis1D.RHandTrigger;
			this.axis1DMap.SecondaryIndexTrigger = OVRInput.RawAxis1D.None;
			this.axis1DMap.SecondaryHandTrigger = OVRInput.RawAxis1D.None;
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x00119A6E File Offset: 0x00117E6E
		public override void ConfigureAxis2DMap()
		{
			this.axis2DMap.None = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.PrimaryTouchpad = OVRInput.RawAxis2D.RTouchpad;
			this.axis2DMap.SecondaryThumbstick = OVRInput.RawAxis2D.None;
			this.axis2DMap.SecondaryTouchpad = OVRInput.RawAxis2D.None;
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x00119AAC File Offset: 0x00117EAC
		public override OVRInput.Controller Update()
		{
			OVRInput.Controller result = base.Update();
			if (OVRInput.GetDown(OVRInput.RawTouch.RTouchpad, OVRInput.Controller.RTrackedRemote))
			{
				this.emitSwipe = true;
				this.moveAmount = this.currentState.RTouchpad;
			}
			if (OVRInput.GetDown(OVRInput.RawButton.RTouchpad, OVRInput.Controller.RTrackedRemote))
			{
				this.emitSwipe = false;
			}
			if (OVRInput.GetUp(OVRInput.RawTouch.RTouchpad, OVRInput.Controller.RTrackedRemote) && this.emitSwipe)
			{
				this.emitSwipe = false;
				this.moveAmount.x = this.previousState.RTouchpad.x - this.moveAmount.x;
				this.moveAmount.y = this.previousState.RTouchpad.y - this.moveAmount.y;
				Vector2 vector = new Vector2(this.moveAmount.x, this.moveAmount.y);
				if (vector.magnitude >= this.minMoveMagnitude)
				{
					vector.Normalize();
					if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
					{
						if (vector.x < 0f)
						{
							this.currentState.Buttons = (this.currentState.Buttons | 262144U);
						}
						else
						{
							this.currentState.Buttons = (this.currentState.Buttons | 524288U);
						}
					}
					else if (vector.y < 0f)
					{
						this.currentState.Buttons = (this.currentState.Buttons | 131072U);
					}
					else
					{
						this.currentState.Buttons = (this.currentState.Buttons | 65536U);
					}
				}
			}
			return result;
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x00119C5C File Offset: 0x0011805C
		public override bool WasRecentered()
		{
			return this.currentState.RRecenterCount != this.previousState.RRecenterCount;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x00119C79 File Offset: 0x00118079
		public override byte GetRecenterCount()
		{
			return this.currentState.RRecenterCount;
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x00119C86 File Offset: 0x00118086
		public override byte GetBatteryPercentRemaining()
		{
			return this.currentState.RBatteryPercentRemaining;
		}

		// Token: 0x04002B20 RID: 11040
		private bool emitSwipe;

		// Token: 0x04002B21 RID: 11041
		private OVRPlugin.Vector2f moveAmount;

		// Token: 0x04002B22 RID: 11042
		private float minMoveMagnitude = 0.3f;
	}
}
