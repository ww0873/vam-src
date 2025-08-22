using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200049B RID: 1179
	public class OptionsMenu : SimpleMenu<OptionsMenu>
	{
		// Token: 0x06001DCE RID: 7630 RVA: 0x000AB238 File Offset: 0x000A9638
		public OptionsMenu()
		{
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x000AB240 File Offset: 0x000A9640
		public void OnMagicButtonPressed()
		{
			AwesomeMenu.Show(this.Slider.value);
		}

		// Token: 0x0400192E RID: 6446
		public Slider Slider;
	}
}
