using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
	// Token: 0x020009C5 RID: 2501
	public class ExecuteTimer
	{
		// Token: 0x06003F2D RID: 16173 RVA: 0x0012EAB4 File Offset: 0x0012CEB4
		public ExecuteTimer()
		{
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x0012EABC File Offset: 0x0012CEBC
		public static void Start()
		{
			ExecuteTimer.StartTime = DateTime.Now;
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x0012EAC8 File Offset: 0x0012CEC8
		public static double TotalMiliseconds()
		{
			return (DateTime.Now - ExecuteTimer.StartTime).TotalMilliseconds;
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0012EAEC File Offset: 0x0012CEEC
		public static void Log()
		{
			Debug.Log("Total Miliseconds: " + ExecuteTimer.TotalMiliseconds());
		}

		// Token: 0x04002FF5 RID: 12277
		public static DateTime StartTime;
	}
}
