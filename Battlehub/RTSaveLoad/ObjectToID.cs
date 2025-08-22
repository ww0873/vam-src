using System;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000229 RID: 553
	[Serializable]
	public struct ObjectToID
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x00049200 File Offset: 0x00047600
		public ObjectToID(UnityEngine.Object obj, int id)
		{
			this.Name = obj.name;
			this.Object = obj;
			this.Id = id;
		}

		// Token: 0x04000C8A RID: 3210
		[HideInInspector]
		public string Name;

		// Token: 0x04000C8B RID: 3211
		public UnityEngine.Object Object;

		// Token: 0x04000C8C RID: 3212
		[ReadOnly]
		public int Id;
	}
}
