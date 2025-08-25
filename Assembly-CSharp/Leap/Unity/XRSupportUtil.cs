using System;
using UnityEngine;
using UnityEngine.XR;

namespace Leap.Unity
{
	// Token: 0x02000751 RID: 1873
	public static class XRSupportUtil
	{
		// Token: 0x06003029 RID: 12329 RVA: 0x000F99DA File Offset: 0x000F7DDA
		public static bool IsXREnabled()
		{
			return XRSettings.enabled;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x000F99E1 File Offset: 0x000F7DE1
		public static bool IsXRDevicePresent()
		{
			return XRDevice.isPresent;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000F99E8 File Offset: 0x000F7DE8
		public static bool IsUserPresent(bool defaultPresence = true)
		{
			UserPresenceState userPresence = XRDevice.userPresence;
			if (userPresence == UserPresenceState.Present)
			{
				return true;
			}
			if (!XRSupportUtil.outputPresenceWarning && userPresence == UserPresenceState.Unsupported)
			{
				Debug.LogWarning("XR UserPresenceState unsupported (XR support is probably disabled).");
				XRSupportUtil.outputPresenceWarning = true;
			}
			return defaultPresence;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000F9A26 File Offset: 0x000F7E26
		public static Vector3 GetXRNodeCenterEyeLocalPosition()
		{
			return InputTracking.GetLocalPosition(XRNode.CenterEye);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000F9A2E File Offset: 0x000F7E2E
		public static Quaternion GetXRNodeCenterEyeLocalRotation()
		{
			return InputTracking.GetLocalRotation(XRNode.CenterEye);
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000F9A36 File Offset: 0x000F7E36
		public static Vector3 GetXRNodeHeadLocalPosition()
		{
			return InputTracking.GetLocalPosition(XRNode.Head);
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x000F9A3E File Offset: 0x000F7E3E
		public static Quaternion GetXRNodeHeadLocalRotation()
		{
			return InputTracking.GetLocalRotation(XRNode.Head);
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x000F9A46 File Offset: 0x000F7E46
		public static void Recenter()
		{
			InputTracking.Recenter();
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x000F9A4D File Offset: 0x000F7E4D
		public static string GetLoadedDeviceName()
		{
			return XRSettings.loadedDeviceName;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x000F9A54 File Offset: 0x000F7E54
		public static bool IsRoomScale()
		{
			return XRDevice.GetTrackingSpaceType() == TrackingSpaceType.RoomScale;
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x000F9A60 File Offset: 0x000F7E60
		public static float GetGPUTime()
		{
			float result = 0f;
			XRStats.TryGetGPUTimeLastFrame(out result);
			return result;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x000F9A7C File Offset: 0x000F7E7C
		// Note: this type is marked as 'beforefieldinit'.
		static XRSupportUtil()
		{
		}

		// Token: 0x04002414 RID: 9236
		private static bool outputPresenceWarning;
	}
}
