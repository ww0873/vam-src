using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C0 RID: 704
	public class ScriptCoroutineExample : MonoBehaviour
	{
		// Token: 0x0600105C RID: 4188 RVA: 0x0005C1C9 File Offset: 0x0005A5C9
		public ScriptCoroutineExample()
		{
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0005C1DC File Offset: 0x0005A5DC
		private void Start()
		{
			bool initCompiler = true;
			this.domain = ScriptDomain.CreateDomain("ModDomain", initCompiler);
			ScriptType scriptType = this.domain.CompileAndLoadScriptSource(this.sourceCode);
			ScriptProxy scriptProxy = scriptType.CreateInstance(base.gameObject);
			scriptProxy.supportCoroutines = true;
			scriptProxy.Call("TestMethod");
		}

		// Token: 0x04000E8C RID: 3724
		private ScriptDomain domain;

		// Token: 0x04000E8D RID: 3725
		private string sourceCode = "using UnityEngine;using System.Collections;class Test : MonoBehaviour{   public IEnumerator TestMethod()   {       for(int i = 0; i < 10; i++)       {           Debug.Log(\"Hello World - From loaded behaviour code\");           yield return new WaitForSeconds(1);       }   }}";
	}
}
