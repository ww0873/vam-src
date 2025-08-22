using System;
using UnityEngine;

namespace Assets.OVR.Scripts
{
	// Token: 0x02000974 RID: 2420
	public class FixRecord : Record
	{
		// Token: 0x06003C6A RID: 15466 RVA: 0x00124B1C File Offset: 0x00122F1C
		public FixRecord(string cat, string msg, FixMethodDelegate fix, UnityEngine.Object target, string[] buttons) : base(cat, msg)
		{
			this.buttonNames = buttons;
			this.fixMethod = fix;
			this.targetObject = target;
			this.complete = false;
		}

		// Token: 0x04002E4C RID: 11852
		public FixMethodDelegate fixMethod;

		// Token: 0x04002E4D RID: 11853
		public UnityEngine.Object targetObject;

		// Token: 0x04002E4E RID: 11854
		public string[] buttonNames;

		// Token: 0x04002E4F RID: 11855
		public bool complete;
	}
}
