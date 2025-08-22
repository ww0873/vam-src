using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000499 RID: 1177
	public class GameMenu : SimpleMenu<GameMenu>
	{
		// Token: 0x06001DC8 RID: 7624 RVA: 0x000AB20C File Offset: 0x000A960C
		public GameMenu()
		{
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000AB214 File Offset: 0x000A9614
		public override void OnBackPressed()
		{
			SimpleMenu<PauseMenu>.Show();
		}
	}
}
