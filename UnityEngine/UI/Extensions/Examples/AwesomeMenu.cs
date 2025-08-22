using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000498 RID: 1176
	public class AwesomeMenu : Menu<AwesomeMenu>
	{
		// Token: 0x06001DC5 RID: 7621 RVA: 0x000AB17F File Offset: 0x000A957F
		public AwesomeMenu()
		{
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000AB188 File Offset: 0x000A9588
		public static void Show(float awesomeness)
		{
			Menu<AwesomeMenu>.Open();
			Menu<AwesomeMenu>.Instance.Background.color = new Color32((byte)(129f * awesomeness), (byte)(197f * awesomeness), (byte)(34f * awesomeness), byte.MaxValue);
			Menu<AwesomeMenu>.Instance.Title.text = string.Format("This menu is {0:P} awesome", awesomeness);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x000AB1EF File Offset: 0x000A95EF
		public static void Hide()
		{
			Menu<AwesomeMenu>.Close();
		}

		// Token: 0x0400192C RID: 6444
		public Image Background;

		// Token: 0x0400192D RID: 6445
		public Text Title;
	}
}
