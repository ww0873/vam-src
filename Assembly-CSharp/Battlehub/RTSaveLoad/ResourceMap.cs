using System;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200022B RID: 555
	[ExecuteInEditMode]
	public class ResourceMap : BundleResourceMap
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x00049224 File Offset: 0x00047624
		public ResourceMap()
		{
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00049233 File Offset: 0x00047633
		public int GetCounter()
		{
			return this.m_counter;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0004923B File Offset: 0x0004763B
		public int IncCounter()
		{
			this.m_counter++;
			return this.m_counter;
		}

		// Token: 0x04000C8F RID: 3215
		[SerializeField]
		[ReadOnly]
		private int m_counter = 1;
	}
}
