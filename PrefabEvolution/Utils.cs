using System;

namespace PrefabEvolution
{
	// Token: 0x02000414 RID: 1044
	public static class Utils
	{
		// Token: 0x06001A5D RID: 6749 RVA: 0x000933F4 File Offset: 0x000917F4
		public static T Create<T>() where T : class, new()
		{
			return (T)((object)null);
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001A5E RID: 6750 RVA: 0x000933FC File Offset: 0x000917FC
		public static bool IsBuildingPlayer
		{
			get
			{
				return false;
			}
		}
	}
}
