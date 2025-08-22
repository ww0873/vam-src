using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000564 RID: 1380
	[AddComponentMenu("UI/Extensions/VR Cursor")]
	public class VRCursor : MonoBehaviour
	{
		// Token: 0x06002310 RID: 8976 RVA: 0x000C7ED9 File Offset: 0x000C62D9
		public VRCursor()
		{
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x000C7EE4 File Offset: 0x000C62E4
		private void Update()
		{
			Vector3 position;
			position.x = Input.mousePosition.x * this.xSens;
			position.y = Input.mousePosition.y * this.ySens - 1f;
			position.z = base.transform.position.z;
			base.transform.position = position;
			VRInputModule.cursorPosition = base.transform.position;
			if (Input.GetMouseButtonDown(0) && this.currentCollider)
			{
				VRInputModule.PointerSubmit(this.currentCollider.gameObject);
			}
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x000C7F8E File Offset: 0x000C638E
		private void OnTriggerEnter(Collider other)
		{
			VRInputModule.PointerEnter(other.gameObject);
			this.currentCollider = other;
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x000C7FA2 File Offset: 0x000C63A2
		private void OnTriggerExit(Collider other)
		{
			VRInputModule.PointerExit(other.gameObject);
			this.currentCollider = null;
		}

		// Token: 0x04001D01 RID: 7425
		public float xSens;

		// Token: 0x04001D02 RID: 7426
		public float ySens;

		// Token: 0x04001D03 RID: 7427
		private Collider currentCollider;
	}
}
