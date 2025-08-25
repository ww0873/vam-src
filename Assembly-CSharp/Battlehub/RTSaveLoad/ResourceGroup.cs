using System;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200022A RID: 554
	public class ResourceGroup : MonoBehaviour
	{
		// Token: 0x06000B74 RID: 2932 RVA: 0x0004921C File Offset: 0x0004761C
		public ResourceGroup()
		{
		}

		// Token: 0x04000C8D RID: 3213
		[ReadOnly]
		public string Guid;

		// Token: 0x04000C8E RID: 3214
		public ObjectToID[] Mapping;
	}
}
