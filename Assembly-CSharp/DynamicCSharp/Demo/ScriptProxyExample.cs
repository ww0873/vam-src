using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C5 RID: 709
	public class ScriptProxyExample : MonoBehaviour
	{
		// Token: 0x06001068 RID: 4200 RVA: 0x0005C46B File Offset: 0x0005A86B
		public ScriptProxyExample()
		{
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0005C480 File Offset: 0x0005A880
		private void Start()
		{
			bool initCompiler = true;
			this.domain = ScriptDomain.CreateDomain("ModDomain", initCompiler);
			ScriptType scriptType = this.domain.CompileAndLoadScriptSource(this.sourceCode);
			ScriptProxy scriptProxy = scriptType.CreateInstance(null);
			scriptProxy.Call("TestMethod");
		}

		// Token: 0x04000E95 RID: 3733
		private ScriptDomain domain;

		// Token: 0x04000E96 RID: 3734
		private string sourceCode = "using UnityEngine;class Test{   public void TestMethod()   {       Debug.Log(\"Hello World - From loaded code\");   }}";
	}
}
