using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000529 RID: 1321
	public abstract class SimpleMenu<T> : Menu<T> where T : SimpleMenu<T>
	{
		// Token: 0x0600217D RID: 8573 RVA: 0x000AB1F6 File Offset: 0x000A95F6
		protected SimpleMenu()
		{
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000AB1FE File Offset: 0x000A95FE
		public static void Show()
		{
			Menu<T>.Open();
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000AB205 File Offset: 0x000A9605
		public static void Hide()
		{
			Menu<T>.Close();
		}
	}
}
