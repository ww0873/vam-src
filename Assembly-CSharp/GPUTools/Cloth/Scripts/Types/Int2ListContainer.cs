using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Types
{
	// Token: 0x020009AC RID: 2476
	[Serializable]
	public class Int2ListContainer
	{
		// Token: 0x06003E6D RID: 15981 RVA: 0x0012CA7F File Offset: 0x0012AE7F
		public Int2ListContainer()
		{
		}

		// Token: 0x04002FA9 RID: 12201
		[SerializeField]
		public List<Int2> List = new List<Int2>();
	}
}
