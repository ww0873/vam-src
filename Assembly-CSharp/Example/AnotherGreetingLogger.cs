using System;
using UnityEngine;

namespace Example
{
	// Token: 0x020002B6 RID: 694
	public class AnotherGreetingLogger : IGreetingLogger
	{
		// Token: 0x0600103E RID: 4158 RVA: 0x0005B9A0 File Offset: 0x00059DA0
		public AnotherGreetingLogger()
		{
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0005B9A8 File Offset: 0x00059DA8
		public void LogGreeting()
		{
			Debug.Log("Greetings!");
		}
	}
}
