using System;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C3 RID: 707
	public class ScriptInterfaceExample : MonoBehaviour
	{
		// Token: 0x06001064 RID: 4196 RVA: 0x0005C3A6 File Offset: 0x0005A7A6
		public ScriptInterfaceExample()
		{
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0005C3BC File Offset: 0x0005A7BC
		private void Start()
		{
			bool initCompiler = true;
			this.domain = ScriptDomain.CreateDomain("ModDomain", initCompiler);
			ScriptType scriptType = this.domain.CompileAndLoadScriptSource(this.sourceCode);
			ScriptProxy scriptProxy = scriptType.CreateInstance(null);
			IExampleInterface instanceAs = scriptProxy.GetInstanceAs<IExampleInterface>(true);
			instanceAs.SayHello();
			instanceAs.SayGoodbye();
		}

		// Token: 0x04000E91 RID: 3729
		private ScriptDomain domain;

		// Token: 0x04000E92 RID: 3730
		private string sourceCode = "using UnityEngine;using DynamicCSharp.Demo;class Test : IExampleInterface{   public void SayHello()   {       Debug.Log(\"Hello - From loaded code\");   }   public void SayGoodbye()   {       Debug.Log(\"Goodbye - From loaded code\");   }}";
	}
}
