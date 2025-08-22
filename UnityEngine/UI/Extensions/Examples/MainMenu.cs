using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200049A RID: 1178
	public class MainMenu : SimpleMenu<MainMenu>
	{
		// Token: 0x06001DCA RID: 7626 RVA: 0x000AB21B File Offset: 0x000A961B
		public MainMenu()
		{
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000AB223 File Offset: 0x000A9623
		public void OnPlayPressed()
		{
			SimpleMenu<GameMenu>.Show();
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000AB22A File Offset: 0x000A962A
		public void OnOptionsPressed()
		{
			SimpleMenu<OptionsMenu>.Show();
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000AB231 File Offset: 0x000A9631
		public override void OnBackPressed()
		{
			Application.Quit();
		}
	}
}
