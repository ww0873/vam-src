using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C4 RID: 708
	public class ScriptProxyBehaviourExample : MonoBehaviour
	{
		// Token: 0x06001066 RID: 4198 RVA: 0x0005C40A File Offset: 0x0005A80A
		public ScriptProxyBehaviourExample()
		{
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0005C420 File Offset: 0x0005A820
		private void Start()
		{
			bool initCompiler = true;
			this.domain = ScriptDomain.CreateDomain("ModDomain", initCompiler);
			ScriptType scriptType = this.domain.CompileAndLoadScriptSource(this.sourceCode);
			ScriptProxy scriptProxy = scriptType.CreateInstance(base.gameObject);
			scriptProxy.Call("TestMethod");
		}

		// Token: 0x04000E93 RID: 3731
		private ScriptDomain domain;

		// Token: 0x04000E94 RID: 3732
		private string sourceCode = "using UnityEngine;class Test : MonoBehaviour{   public void Awake()   {      Debug.Log(\"Hello world - From loaded behaviour 'Awake'\");   }   public void TestMethod()   {       Debug.Log(\"Hello World - From loaded behaviour code\");   }}";
	}
}
