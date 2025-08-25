using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002CB RID: 715
	public sealed class TankManagerAsync : MonoBehaviour
	{
		// Token: 0x06001086 RID: 4230 RVA: 0x0005CE76 File Offset: 0x0005B276
		public TankManagerAsync()
		{
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0005CE8C File Offset: 0x0005B28C
		public void Awake()
		{
			if (this.statusText != null)
			{
				this.initialText = this.statusText.text;
			}
			this.domain = ScriptDomain.CreateDomain("ScriptDomain", true);
			this.startPosition = this.tankObject.transform.position;
			this.startRotation = this.tankObject.transform.rotation;
			Delegate onNewClicked = CodeUI.onNewClicked;
			if (TankManagerAsync.<>f__am$cache0 == null)
			{
				TankManagerAsync.<>f__am$cache0 = new Action<CodeUI>(TankManagerAsync.<Awake>m__0);
			}
			CodeUI.onNewClicked = (Action<CodeUI>)Delegate.Combine(onNewClicked, TankManagerAsync.<>f__am$cache0);
			Delegate onLoadClicked = CodeUI.onLoadClicked;
			if (TankManagerAsync.<>f__am$cache1 == null)
			{
				TankManagerAsync.<>f__am$cache1 = new Action<CodeUI>(TankManagerAsync.<Awake>m__1);
			}
			CodeUI.onLoadClicked = (Action<CodeUI>)Delegate.Combine(onLoadClicked, TankManagerAsync.<>f__am$cache1);
			CodeUI.onCompileClicked = (Action<CodeUI>)Delegate.Combine(CodeUI.onCompileClicked, new Action<CodeUI>(this.<Awake>m__2));
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0005CF80 File Offset: 0x0005B380
		public void Update()
		{
			if (this.statusText != null)
			{
				if (this.domain.CompilerService.IsCompiling)
				{
					if (Time.time > this.timer + 0.1f)
					{
						this.timer = Time.time;
						this.counter++;
						if (this.counter > 3)
						{
							this.counter = 0;
						}
						this.statusText.text = "Compiling";
						for (int i = 0; i < this.counter; i++)
						{
							Text text = this.statusText;
							text.text += '.';
						}
					}
				}
				else
				{
					this.statusText.text = this.initialText;
				}
			}
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0005D050 File Offset: 0x0005B450
		public IEnumerator RunTankScript(string source)
		{
			TankController old = this.tankObject.GetComponent<TankController>();
			if (old != null)
			{
				UnityEngine.Object.Destroy(old);
			}
			this.RespawnTank();
			AsyncCompileLoadOperation task = this.domain.CompileAndLoadScriptSourcesAsync(new string[]
			{
				source
			});
			yield return task;
			if (!task.IsSuccessful)
			{
				UnityEngine.Debug.LogError("Compile failed");
				yield break;
			}
			ScriptType type = task.MainType;
			if (type.IsSubtypeOf<TankController>())
			{
				ScriptProxy scriptProxy = type.CreateInstance(this.tankObject);
				if (scriptProxy == null)
				{
					UnityEngine.Debug.LogError(string.Format("Failed to create an instance of '{0}'", type.RawType));
					yield break;
				}
				scriptProxy.Fields["bulletObject"] = this.bulletObject;
				scriptProxy.Call("RunTank");
			}
			else
			{
				UnityEngine.Debug.LogError("The script must inherit from 'TankController'");
			}
			yield break;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0005D072 File Offset: 0x0005B472
		public void RespawnTank()
		{
			this.tankObject.transform.position = this.startPosition;
			this.tankObject.transform.rotation = this.startRotation;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0005D0A5 File Offset: 0x0005B4A5
		[CompilerGenerated]
		private static void <Awake>m__0(CodeUI ui)
		{
			ui.codeEditor.text = Resources.Load<TextAsset>("BlankTemplate").text;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0005D0C1 File Offset: 0x0005B4C1
		[CompilerGenerated]
		private static void <Awake>m__1(CodeUI ui)
		{
			ui.codeEditor.text = Resources.Load<TextAsset>("ExampleTemplate").text;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0005D0DD File Offset: 0x0005B4DD
		[CompilerGenerated]
		private void <Awake>m__2(CodeUI ui)
		{
			base.StartCoroutine(this.RunTankScript(ui.codeEditor.text));
		}

		// Token: 0x04000EB1 RID: 3761
		private ScriptDomain domain;

		// Token: 0x04000EB2 RID: 3762
		private Vector2 startPosition;

		// Token: 0x04000EB3 RID: 3763
		private Quaternion startRotation;

		// Token: 0x04000EB4 RID: 3764
		private string initialText = string.Empty;

		// Token: 0x04000EB5 RID: 3765
		private int counter;

		// Token: 0x04000EB6 RID: 3766
		private float timer;

		// Token: 0x04000EB7 RID: 3767
		private const string newTemplate = "BlankTemplate";

		// Token: 0x04000EB8 RID: 3768
		private const string exampleTemplate = "ExampleTemplate";

		// Token: 0x04000EB9 RID: 3769
		public GameObject bulletObject;

		// Token: 0x04000EBA RID: 3770
		public GameObject tankObject;

		// Token: 0x04000EBB RID: 3771
		public Text statusText;

		// Token: 0x04000EBC RID: 3772
		[CompilerGenerated]
		private static Action<CodeUI> <>f__am$cache0;

		// Token: 0x04000EBD RID: 3773
		[CompilerGenerated]
		private static Action<CodeUI> <>f__am$cache1;

		// Token: 0x02000EE8 RID: 3816
		[CompilerGenerated]
		private sealed class <RunTankScript>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600722F RID: 29231 RVA: 0x0005D0F7 File Offset: 0x0005B4F7
			[DebuggerHidden]
			public <RunTankScript>c__Iterator0()
			{
			}

			// Token: 0x06007230 RID: 29232 RVA: 0x0005D100 File Offset: 0x0005B500
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					old = this.tankObject.GetComponent<TankController>();
					if (old != null)
					{
						UnityEngine.Object.Destroy(old);
					}
					base.RespawnTank();
					task = this.domain.CompileAndLoadScriptSourcesAsync(new string[]
					{
						source
					});
					this.$current = task;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (!task.IsSuccessful)
					{
						UnityEngine.Debug.LogError("Compile failed");
					}
					else
					{
						type = task.MainType;
						if (type.IsSubtypeOf<TankController>())
						{
							ScriptProxy scriptProxy = type.CreateInstance(this.tankObject);
							if (scriptProxy == null)
							{
								UnityEngine.Debug.LogError(string.Format("Failed to create an instance of '{0}'", type.RawType));
								break;
							}
							scriptProxy.Fields["bulletObject"] = this.bulletObject;
							scriptProxy.Call("RunTank");
						}
						else
						{
							UnityEngine.Debug.LogError("The script must inherit from 'TankController'");
						}
						this.$PC = -1;
					}
					break;
				}
				return false;
			}

			// Token: 0x170010B5 RID: 4277
			// (get) Token: 0x06007231 RID: 29233 RVA: 0x0005D26C File Offset: 0x0005B66C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010B6 RID: 4278
			// (get) Token: 0x06007232 RID: 29234 RVA: 0x0005D274 File Offset: 0x0005B674
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007233 RID: 29235 RVA: 0x0005D27C File Offset: 0x0005B67C
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007234 RID: 29236 RVA: 0x0005D28C File Offset: 0x0005B68C
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006606 RID: 26118
			internal TankController <old>__0;

			// Token: 0x04006607 RID: 26119
			internal string source;

			// Token: 0x04006608 RID: 26120
			internal AsyncCompileLoadOperation <task>__0;

			// Token: 0x04006609 RID: 26121
			internal ScriptType <type>__0;

			// Token: 0x0400660A RID: 26122
			internal TankManagerAsync $this;

			// Token: 0x0400660B RID: 26123
			internal object $current;

			// Token: 0x0400660C RID: 26124
			internal bool $disposing;

			// Token: 0x0400660D RID: 26125
			internal int $PC;
		}
	}
}
