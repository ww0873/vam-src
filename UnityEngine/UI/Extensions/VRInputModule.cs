using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000565 RID: 1381
	[AddComponentMenu("Event/VR Input Module")]
	public class VRInputModule : BaseInputModule
	{
		// Token: 0x06002314 RID: 8980 RVA: 0x000C7FB6 File Offset: 0x000C63B6
		public VRInputModule()
		{
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000C7FBE File Offset: 0x000C63BE
		protected override void Awake()
		{
			VRInputModule._singleton = this;
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x000C7FC6 File Offset: 0x000C63C6
		public override void Process()
		{
			if (VRInputModule.targetObject == null)
			{
				VRInputModule.mouseClicked = false;
				return;
			}
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000C7FE0 File Offset: 0x000C63E0
		public static void PointerSubmit(GameObject obj)
		{
			VRInputModule.targetObject = obj;
			VRInputModule.mouseClicked = true;
			if (VRInputModule.mouseClicked)
			{
				BaseEventData baseEventData = new BaseEventData(VRInputModule._singleton.eventSystem);
				baseEventData.selectedObject = VRInputModule.targetObject;
				ExecuteEvents.Execute<ISubmitHandler>(VRInputModule.targetObject, baseEventData, ExecuteEvents.submitHandler);
				MonoBehaviour.print("clicked " + VRInputModule.targetObject.name);
				VRInputModule.mouseClicked = false;
			}
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000C8050 File Offset: 0x000C6450
		public static void PointerExit(GameObject obj)
		{
			MonoBehaviour.print("PointerExit " + obj.name);
			PointerEventData eventData = new PointerEventData(VRInputModule._singleton.eventSystem);
			ExecuteEvents.Execute<IPointerExitHandler>(obj, eventData, ExecuteEvents.pointerExitHandler);
			ExecuteEvents.Execute<IDeselectHandler>(obj, eventData, ExecuteEvents.deselectHandler);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000C809C File Offset: 0x000C649C
		public static void PointerEnter(GameObject obj)
		{
			MonoBehaviour.print("PointerEnter " + obj.name);
			PointerEventData pointerEventData = new PointerEventData(VRInputModule._singleton.eventSystem);
			pointerEventData.pointerEnter = obj;
			RaycastResult pointerCurrentRaycast = new RaycastResult
			{
				worldPosition = VRInputModule.cursorPosition
			};
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			ExecuteEvents.Execute<IPointerEnterHandler>(obj, pointerEventData, ExecuteEvents.pointerEnterHandler);
		}

		// Token: 0x04001D04 RID: 7428
		public static GameObject targetObject;

		// Token: 0x04001D05 RID: 7429
		private static VRInputModule _singleton;

		// Token: 0x04001D06 RID: 7430
		private int counter;

		// Token: 0x04001D07 RID: 7431
		private static bool mouseClicked;

		// Token: 0x04001D08 RID: 7432
		public static Vector3 cursorPosition;
	}
}
