using System;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200022C RID: 556
	public static class ObjectExt
	{
		// Token: 0x06000B78 RID: 2936 RVA: 0x00049251 File Offset: 0x00047651
		public static bool HasMappedInstanceID(this UnityEngine.Object obj)
		{
			if (IdentifiersMap.Instance == null)
			{
				Debug.LogError("Create Resource Map");
			}
			return IdentifiersMap.Instance.IsResource(obj);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00049272 File Offset: 0x00047672
		public static long GetMappedInstanceID(this UnityEngine.Object obj)
		{
			if (IdentifiersMap.Instance == null)
			{
				Debug.LogError("Create Resource Map");
			}
			return IdentifiersMap.Instance.GetMappedInstanceID(obj);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00049293 File Offset: 0x00047693
		public static long[] GetMappedInstanceID(this UnityEngine.Object[] objs)
		{
			if (IdentifiersMap.Instance == null)
			{
				Debug.LogError("Create Resource Map");
			}
			return IdentifiersMap.Instance.GetMappedInstanceID(objs);
		}
	}
}
