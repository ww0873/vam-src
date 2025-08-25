using System;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000E18 RID: 3608
	public class PerfMonPre : MonoBehaviour
	{
		// Token: 0x06006F26 RID: 28454 RVA: 0x0029BC13 File Offset: 0x0029A013
		public PerfMonPre()
		{
		}

		// Token: 0x06006F27 RID: 28455 RVA: 0x0029BC1B File Offset: 0x0029A01B
		private void FixedUpdate()
		{
			if (!this.updated)
			{
				PerfMonPre.frameStartTime = GlobalStopwatch.GetElapsedMilliseconds();
				PerfMonPre.physicsStartTime = PerfMonPre.frameStartTime;
				this.updated = true;
			}
		}

		// Token: 0x06006F28 RID: 28456 RVA: 0x0029BC44 File Offset: 0x0029A044
		private void Update()
		{
			PerfMonPre.updateStartTime = GlobalStopwatch.GetElapsedMilliseconds();
			if (!this.updated)
			{
				PerfMonPre.physicsTime = 0f;
				PerfMonPre.frameStartTime = PerfMonPre.updateStartTime;
			}
			else
			{
				PerfMonPre.physicsTime = PerfMonPre.updateStartTime - PerfMonPre.physicsStartTime;
				PerfMonPre.lastPhysicsTime = PerfMonPre.physicsTime;
			}
			this.updated = false;
		}

		// Token: 0x04006058 RID: 24664
		public static float frameStartTime;

		// Token: 0x04006059 RID: 24665
		public static float physicsStartTime;

		// Token: 0x0400605A RID: 24666
		public static float updateStartTime;

		// Token: 0x0400605B RID: 24667
		public static float lastPhysicsTime;

		// Token: 0x0400605C RID: 24668
		public static float physicsTime;

		// Token: 0x0400605D RID: 24669
		protected bool updated;
	}
}
