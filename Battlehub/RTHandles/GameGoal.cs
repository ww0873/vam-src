using System;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F3 RID: 243
	public class GameGoal : MonoBehaviour
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x0001DBA7 File Offset: 0x0001BFA7
		public GameGoal()
		{
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001DBAF File Offset: 0x0001BFAF
		private void Start()
		{
			base.tag = "Finish";
		}
	}
}
