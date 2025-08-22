using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C1 RID: 705
	public class ScriptEvaluatorExample
	{
		// Token: 0x0600105E RID: 4190 RVA: 0x0005C22E File Offset: 0x0005A62E
		public ScriptEvaluatorExample()
		{
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0005C236 File Offset: 0x0005A636
		private void Start()
		{
			this.domain = ScriptDomain.CreateDomain("EvalDomain", true);
			this.evaluator = new ScriptEvaluator(this.domain);
			this.evaluator.AddUsing("UnityEngine");
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0005C26C File Offset: 0x0005A66C
		private void onGUI()
		{
			if (GUILayout.Button("EvalMath", new GUILayoutOption[0]))
			{
				Debug.Log(this.evaluator.Eval("return 6 * 3 + 20;"));
			}
			if (GUILayout.Button("EvalLoop", new GUILayoutOption[0]))
			{
				this.evaluator.Eval("for(int i = 0; i < 5; i++) Debug.Log(\"Hello World \" + i);");
			}
			if (GUILayout.Button("EvalVar", new GUILayoutOption[0]))
			{
				this.evaluator.BindVar<float>("floatValue", 23.5f);
				this.evaluator.Eval("Debug.Log(floatValue + 4f);");
			}
			if (GUILayout.Button("EvalRefVar", new GUILayoutOption[0]))
			{
				Variable<float> message = this.evaluator.BindVar<float>("floatValue", 12.3f);
				this.evaluator.Eval("floatValue *= 2;");
				Debug.Log(message);
			}
			if (GUILayout.Button("EvalDelegate", new GUILayoutOption[0]))
			{
				ScriptEvaluator scriptEvaluator = this.evaluator;
				string name = "callback";
				if (ScriptEvaluatorExample.<>f__am$cache0 == null)
				{
					ScriptEvaluatorExample.<>f__am$cache0 = new Action(ScriptEvaluatorExample.<onGUI>m__0);
				}
				scriptEvaluator.BindDelegate(name, ScriptEvaluatorExample.<>f__am$cache0);
				this.evaluator.Eval("callback();");
			}
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0005C39A File Offset: 0x0005A79A
		[CompilerGenerated]
		private static void <onGUI>m__0()
		{
			Debug.Log("Hello from callback");
		}

		// Token: 0x04000E8E RID: 3726
		private ScriptDomain domain;

		// Token: 0x04000E8F RID: 3727
		private ScriptEvaluator evaluator;

		// Token: 0x04000E90 RID: 3728
		[CompilerGenerated]
		private static Action <>f__am$cache0;
	}
}
