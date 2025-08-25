using System;
using TypeReferences;
using UnityEngine;

namespace Example
{
	// Token: 0x020002B3 RID: 691
	public class ExampleBehaviour : MonoBehaviour
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x0005B926 File Offset: 0x00059D26
		public ExampleBehaviour()
		{
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0005B944 File Offset: 0x00059D44
		private void Start()
		{
			if (this.greetingLoggerType.Type == null)
			{
				Debug.LogWarning("No greeting logger was specified.");
			}
			else
			{
				IGreetingLogger greetingLogger = Activator.CreateInstance(this.greetingLoggerType) as IGreetingLogger;
				greetingLogger.LogGreeting();
			}
		}

		// Token: 0x04000E6A RID: 3690
		[ClassImplements(typeof(IGreetingLogger))]
		public ClassTypeReference greetingLoggerType = typeof(DefaultGreetingLogger);
	}
}
