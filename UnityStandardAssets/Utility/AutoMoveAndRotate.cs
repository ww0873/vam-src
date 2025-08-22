using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000369 RID: 873
	public class AutoMoveAndRotate : MonoBehaviour
	{
		// Token: 0x060015D3 RID: 5587 RVA: 0x0007CD09 File Offset: 0x0007B109
		public AutoMoveAndRotate()
		{
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0007CD11 File Offset: 0x0007B111
		private void Start()
		{
			this.m_LastRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0007CD20 File Offset: 0x0007B120
		private void Update()
		{
			float d = Time.deltaTime;
			if (this.ignoreTimescale)
			{
				d = Time.realtimeSinceStartup - this.m_LastRealTime;
				this.m_LastRealTime = Time.realtimeSinceStartup;
			}
			base.transform.Translate(this.moveUnitsPerSecond.value * d, this.moveUnitsPerSecond.space);
			base.transform.Rotate(this.rotateDegreesPerSecond.value * d, this.moveUnitsPerSecond.space);
		}

		// Token: 0x0400123E RID: 4670
		public AutoMoveAndRotate.Vector3andSpace moveUnitsPerSecond;

		// Token: 0x0400123F RID: 4671
		public AutoMoveAndRotate.Vector3andSpace rotateDegreesPerSecond;

		// Token: 0x04001240 RID: 4672
		public bool ignoreTimescale;

		// Token: 0x04001241 RID: 4673
		private float m_LastRealTime;

		// Token: 0x0200036A RID: 874
		[Serializable]
		public class Vector3andSpace
		{
			// Token: 0x060015D6 RID: 5590 RVA: 0x0007CDA4 File Offset: 0x0007B1A4
			public Vector3andSpace()
			{
			}

			// Token: 0x04001242 RID: 4674
			public Vector3 value;

			// Token: 0x04001243 RID: 4675
			public Space space = Space.Self;
		}
	}
}
