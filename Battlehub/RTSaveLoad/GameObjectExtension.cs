using System;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200024E RID: 590
	public static class GameObjectExtension
	{
		// Token: 0x06000C44 RID: 3140 RVA: 0x0004AF3C File Offset: 0x0004933C
		public static GameObject InstantiatePrefab(this GameObject prefab, Vector3 position, Quaternion rotation)
		{
			if (prefab == null)
			{
				return null;
			}
			PersistentIgnore component = prefab.GetComponent<PersistentIgnore>();
			if (component != null)
			{
				return component.InstantiatePrefab(prefab, position, rotation);
			}
			return UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		}
	}
}
