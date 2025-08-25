using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000526 RID: 1318
	public abstract class Menu<T> : Menu where T : Menu<T>
	{
		// Token: 0x06002166 RID: 8550 RVA: 0x000AB083 File Offset: 0x000A9483
		protected Menu()
		{
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06002167 RID: 8551 RVA: 0x000AB08B File Offset: 0x000A948B
		// (set) Token: 0x06002168 RID: 8552 RVA: 0x000AB092 File Offset: 0x000A9492
		public static T Instance
		{
			[CompilerGenerated]
			get
			{
				return Menu<T>.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				Menu<T>.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000AB09A File Offset: 0x000A949A
		protected virtual void Awake()
		{
			Menu<T>.Instance = (T)((object)this);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000AB0A7 File Offset: 0x000A94A7
		protected virtual void OnDestroy()
		{
			Menu<T>.Instance = (T)((object)null);
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000AB0B4 File Offset: 0x000A94B4
		protected static void Open()
		{
			if (Menu<T>.Instance == null)
			{
				MenuManager.Instance.CreateInstance(typeof(T).Name);
			}
			else
			{
				T instance = Menu<T>.Instance;
				instance.gameObject.SetActive(true);
			}
			MenuManager.Instance.OpenMenu(Menu<T>.Instance);
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000AB124 File Offset: 0x000A9524
		protected static void Close()
		{
			if (Menu<T>.Instance == null)
			{
				Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", new object[]
				{
					typeof(T)
				});
				return;
			}
			MenuManager.Instance.CloseMenu(Menu<T>.Instance);
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x000AB178 File Offset: 0x000A9578
		public override void OnBackPressed()
		{
			Menu<T>.Close();
		}

		// Token: 0x04001BE3 RID: 7139
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static T <Instance>k__BackingField;
	}
}
