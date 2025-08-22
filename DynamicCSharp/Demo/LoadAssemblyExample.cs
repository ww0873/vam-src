using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002BE RID: 702
	public class LoadAssemblyExample : MonoBehaviour
	{
		// Token: 0x06001058 RID: 4184 RVA: 0x0005C10D File Offset: 0x0005A50D
		public LoadAssemblyExample()
		{
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0005C118 File Offset: 0x0005A518
		private void Start()
		{
			bool initCompiler = true;
			this.domain = ScriptDomain.CreateDomain("ModDomain", initCompiler);
			ScriptAssembly scriptAssembly = this.domain.LoadAssembly("ModAssembly.dll");
			foreach (ScriptType scriptType in scriptAssembly.FindAllTypes())
			{
				Debug.Log(scriptType.ToString());
			}
		}

		// Token: 0x04000E89 RID: 3721
		private ScriptDomain domain;
	}
}
