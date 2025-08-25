using System;
using UnityEngine;

namespace Example
{
	// Token: 0x020002B5 RID: 693
	public class DefaultGreetingLogger : IGreetingLogger
	{
		// Token: 0x0600103C RID: 4156 RVA: 0x0005B98C File Offset: 0x00059D8C
		public DefaultGreetingLogger()
		{
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0005B994 File Offset: 0x00059D94
		public void LogGreeting()
		{
			Debug.Log("Hello, World!");
		}
	}
}
