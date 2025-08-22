using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000527 RID: 1319
	public abstract class Menu : MonoBehaviour
	{
		// Token: 0x0600216E RID: 8558 RVA: 0x000AB06D File Offset: 0x000A946D
		protected Menu()
		{
		}

		// Token: 0x0600216F RID: 8559
		public abstract void OnBackPressed();

		// Token: 0x04001BE4 RID: 7140
		[Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
		public bool DestroyWhenClosed = true;

		// Token: 0x04001BE5 RID: 7141
		[Tooltip("Disable menus that are under this one in the stack")]
		public bool DisableMenusUnderneath = true;
	}
}
