using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200049C RID: 1180
	public class PauseMenu : SimpleMenu<PauseMenu>
	{
		// Token: 0x06001DD0 RID: 7632 RVA: 0x000AB252 File Offset: 0x000A9652
		public PauseMenu()
		{
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000AB25A File Offset: 0x000A965A
		public void OnQuitPressed()
		{
			SimpleMenu<PauseMenu>.Hide();
			Object.Destroy(base.gameObject);
			SimpleMenu<GameMenu>.Hide();
		}
	}
}
