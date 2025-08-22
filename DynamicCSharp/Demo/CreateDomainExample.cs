using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002BD RID: 701
	public class CreateDomainExample : MonoBehaviour
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x0005C0D0 File Offset: 0x0005A4D0
		public CreateDomainExample()
		{
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0005C0D8 File Offset: 0x0005A4D8
		private void Start()
		{
			bool initCompiler = true;
			this.domain = ScriptDomain.CreateDomain("ModDomain", initCompiler);
			if (this.domain == null)
			{
				Debug.LogError("Failed to create ScriptDomain");
			}
		}

		// Token: 0x04000E88 RID: 3720
		private ScriptDomain domain;
	}
}
