using System;

namespace Weelco.VRKeyboard
{
	// Token: 0x02000596 RID: 1430
	public class VRKeyboardBase : VRKeyboardData
	{
		// Token: 0x060023FD RID: 9213 RVA: 0x000D0796 File Offset: 0x000CEB96
		public VRKeyboardBase()
		{
		}

		// Token: 0x04001E4D RID: 7757
		public VRKeyboardBase.VRKeyboardBtnClick OnVRKeyboardBtnClick;

		// Token: 0x04001E4E RID: 7758
		protected bool Initialized;

		// Token: 0x02000597 RID: 1431
		// (Invoke) Token: 0x060023FF RID: 9215
		public delegate void VRKeyboardBtnClick(string value);
	}
}
