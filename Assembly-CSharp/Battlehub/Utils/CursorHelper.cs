using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x0200029D RID: 669
	public static class CursorHelper
	{
		// Token: 0x06000FDD RID: 4061 RVA: 0x0005AD7E File Offset: 0x0005917E
		public static void SetCursor(object locker, Texture2D texture, Vector2 hotspot, CursorMode mode)
		{
			if (CursorHelper.m_locker != null && CursorHelper.m_locker != locker)
			{
				return;
			}
			CursorHelper.m_locker = locker;
			Cursor.SetCursor(texture, hotspot, mode);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0005ADA4 File Offset: 0x000591A4
		public static void ResetCursor(object locker)
		{
			if (CursorHelper.m_locker != locker)
			{
				return;
			}
			CursorHelper.m_locker = null;
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x04000E53 RID: 3667
		private static object m_locker;
	}
}
