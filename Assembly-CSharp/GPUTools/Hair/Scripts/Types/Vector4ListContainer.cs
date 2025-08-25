using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Types
{
	// Token: 0x02000A2F RID: 2607
	[Serializable]
	public class Vector4ListContainer
	{
		// Token: 0x0600433E RID: 17214 RVA: 0x0013BCDE File Offset: 0x0013A0DE
		public Vector4ListContainer()
		{
		}

		// Token: 0x0400324C RID: 12876
		[SerializeField]
		public List<Vector4> List = new List<Vector4>();
	}
}
