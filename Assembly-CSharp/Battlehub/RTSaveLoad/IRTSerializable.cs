using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200020F RID: 527
	public interface IRTSerializable
	{
		// Token: 0x06000A94 RID: 2708
		void Serialize();

		// Token: 0x06000A95 RID: 2709
		void Deserialize(Dictionary<long, UnityEngine.Object> dependencies);

		// Token: 0x06000A96 RID: 2710
		void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies);

		// Token: 0x06000A97 RID: 2711
		void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls);
	}
}
