using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002CA RID: 714
	public sealed class TankManager : MonoBehaviour
	{
		// Token: 0x0600107F RID: 4223 RVA: 0x0005CC65 File Offset: 0x0005B065
		public TankManager()
		{
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0005CC70 File Offset: 0x0005B070
		public void Awake()
		{
			this.domain = ScriptDomain.CreateDomain("ScriptDomain", true);
			this.startPosition = this.tankObject.transform.position;
			this.startRotation = this.tankObject.transform.rotation;
			Delegate onNewClicked = CodeUI.onNewClicked;
			if (TankManager.<>f__am$cache0 == null)
			{
				TankManager.<>f__am$cache0 = new Action<CodeUI>(TankManager.<Awake>m__0);
			}
			CodeUI.onNewClicked = (Action<CodeUI>)Delegate.Combine(onNewClicked, TankManager.<>f__am$cache0);
			Delegate onLoadClicked = CodeUI.onLoadClicked;
			if (TankManager.<>f__am$cache1 == null)
			{
				TankManager.<>f__am$cache1 = new Action<CodeUI>(TankManager.<Awake>m__1);
			}
			CodeUI.onLoadClicked = (Action<CodeUI>)Delegate.Combine(onLoadClicked, TankManager.<>f__am$cache1);
			CodeUI.onCompileClicked = (Action<CodeUI>)Delegate.Combine(CodeUI.onCompileClicked, new Action<CodeUI>(this.<Awake>m__2));
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0005CD44 File Offset: 0x0005B144
		public void RunTankScript(string source)
		{
			TankController component = this.tankObject.GetComponent<TankController>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			this.RespawnTank();
			ScriptType scriptType = this.domain.CompileAndLoadScriptSource(source);
			if (scriptType == null)
			{
				Debug.LogError("Compile failed");
				return;
			}
			if (scriptType.IsSubtypeOf<TankController>())
			{
				ScriptProxy scriptProxy = scriptType.CreateInstance(this.tankObject);
				if (scriptProxy == null)
				{
					Debug.LogError(string.Format("Failed to create an instance of '{0}'", scriptType.RawType));
					return;
				}
				scriptProxy.Fields["bulletObject"] = this.bulletObject;
				scriptProxy.Call("RunTank");
			}
			else
			{
				Debug.LogError("The script must inherit from 'TankController'");
			}
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0005CDF8 File Offset: 0x0005B1F8
		public void RespawnTank()
		{
			this.tankObject.transform.position = this.startPosition;
			this.tankObject.transform.rotation = this.startRotation;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0005CE2B File Offset: 0x0005B22B
		[CompilerGenerated]
		private static void <Awake>m__0(CodeUI ui)
		{
			ui.codeEditor.text = Resources.Load<TextAsset>("BlankTemplate").text;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0005CE47 File Offset: 0x0005B247
		[CompilerGenerated]
		private static void <Awake>m__1(CodeUI ui)
		{
			ui.codeEditor.text = Resources.Load<TextAsset>("ExampleTemplate").text;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0005CE63 File Offset: 0x0005B263
		[CompilerGenerated]
		private void <Awake>m__2(CodeUI ui)
		{
			this.RunTankScript(ui.codeEditor.text);
		}

		// Token: 0x04000EA8 RID: 3752
		private ScriptDomain domain;

		// Token: 0x04000EA9 RID: 3753
		private Vector2 startPosition;

		// Token: 0x04000EAA RID: 3754
		private Quaternion startRotation;

		// Token: 0x04000EAB RID: 3755
		private const string newTemplate = "BlankTemplate";

		// Token: 0x04000EAC RID: 3756
		private const string exampleTemplate = "ExampleTemplate";

		// Token: 0x04000EAD RID: 3757
		public GameObject bulletObject;

		// Token: 0x04000EAE RID: 3758
		public GameObject tankObject;

		// Token: 0x04000EAF RID: 3759
		[CompilerGenerated]
		private static Action<CodeUI> <>f__am$cache0;

		// Token: 0x04000EB0 RID: 3760
		[CompilerGenerated]
		private static Action<CodeUI> <>f__am$cache1;
	}
}
