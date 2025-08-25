using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002BF RID: 703
	public class LoadScriptExample : MonoBehaviour
	{
		// Token: 0x0600105A RID: 4186 RVA: 0x0005C178 File Offset: 0x0005A578
		public LoadScriptExample()
		{
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0005C18C File Offset: 0x0005A58C
		private void Start()
		{
			bool initCompiler = true;
			this.domain = ScriptDomain.CreateDomain("ModDomain", initCompiler);
			ScriptType scriptType = this.domain.CompileAndLoadScriptSource(this.sourceCode);
			Debug.Log(scriptType.ToString());
		}

		// Token: 0x04000E8A RID: 3722
		private ScriptDomain domain;

		// Token: 0x04000E8B RID: 3723
		private string sourceCode = "using UnityEngine;class Test{   public void TestMethod()   {       Debug.Log(\"Hello World - From loaded code\");   }}";
	}
}
