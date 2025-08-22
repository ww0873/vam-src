using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000734 RID: 1844
	public static class InternalUtility
	{
		// Token: 0x06002CF9 RID: 11513 RVA: 0x000F0AB8 File Offset: 0x000EEEB8
		public static T AddComponent<T>(GameObject obj) where T : Component
		{
			return obj.AddComponent<T>();
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000F0AC0 File Offset: 0x000EEEC0
		public static Component AddComponent(GameObject obj, Type type)
		{
			return obj.AddComponent(type);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000F0AC9 File Offset: 0x000EEEC9
		public static void Destroy(UnityEngine.Object obj)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}
}
