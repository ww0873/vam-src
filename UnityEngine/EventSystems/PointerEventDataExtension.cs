using System;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200096D RID: 2413
	public static class PointerEventDataExtension
	{
		// Token: 0x06003C4F RID: 15439 RVA: 0x0012439D File Offset: 0x0012279D
		public static bool IsVRPointer(this PointerEventData pointerEventData)
		{
			return pointerEventData is OVRPointerEventData;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x001243A8 File Offset: 0x001227A8
		public static Ray GetRay(this PointerEventData pointerEventData)
		{
			OVRPointerEventData ovrpointerEventData = pointerEventData as OVRPointerEventData;
			return ovrpointerEventData.worldSpaceRay;
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x001243C4 File Offset: 0x001227C4
		public static Vector2 GetSwipeStart(this PointerEventData pointerEventData)
		{
			OVRPointerEventData ovrpointerEventData = pointerEventData as OVRPointerEventData;
			return ovrpointerEventData.swipeStart;
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x001243E0 File Offset: 0x001227E0
		public static void SetSwipeStart(this PointerEventData pointerEventData, Vector2 start)
		{
			OVRPointerEventData ovrpointerEventData = pointerEventData as OVRPointerEventData;
			ovrpointerEventData.swipeStart = start;
		}
	}
}
