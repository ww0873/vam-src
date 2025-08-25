using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000528 RID: 1320
	[AddComponentMenu("UI/Extensions/Menu Manager")]
	[DisallowMultipleComponent]
	public class MenuManager : MonoBehaviour
	{
		// Token: 0x06002170 RID: 8560 RVA: 0x000BF065 File Offset: 0x000BD465
		public MenuManager()
		{
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06002171 RID: 8561 RVA: 0x000BF078 File Offset: 0x000BD478
		// (set) Token: 0x06002172 RID: 8562 RVA: 0x000BF07F File Offset: 0x000BD47F
		public static MenuManager Instance
		{
			[CompilerGenerated]
			get
			{
				return MenuManager.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				MenuManager.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000BF088 File Offset: 0x000BD488
		private void Awake()
		{
			MenuManager.Instance = this;
			if (this.MenuScreens.Length > this.StartScreen)
			{
				this.CreateInstance(this.MenuScreens[this.StartScreen].name);
				this.OpenMenu(this.MenuScreens[this.StartScreen]);
			}
			else
			{
				Debug.LogError("Not enough Menu Screens configured");
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000BF0E8 File Offset: 0x000BD4E8
		private void OnDestroy()
		{
			MenuManager.Instance = null;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000BF0F0 File Offset: 0x000BD4F0
		public void CreateInstance<T>() where T : Menu
		{
			T prefab = this.GetPrefab<T>();
			Object.Instantiate<T>(prefab, base.transform);
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000BF114 File Offset: 0x000BD514
		public void CreateInstance(string MenuName)
		{
			GameObject prefab = this.GetPrefab(MenuName);
			Object.Instantiate<GameObject>(prefab, base.transform);
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000BF138 File Offset: 0x000BD538
		public void OpenMenu(Menu instance)
		{
			if (this.menuStack.Count > 0)
			{
				if (instance.DisableMenusUnderneath)
				{
					foreach (Menu menu in this.menuStack)
					{
						menu.gameObject.SetActive(false);
						if (menu.DisableMenusUnderneath)
						{
							break;
						}
					}
				}
				Canvas component = instance.GetComponent<Canvas>();
				Canvas component2 = this.menuStack.Peek().GetComponent<Canvas>();
				component.sortingOrder = component2.sortingOrder + 1;
			}
			this.menuStack.Push(instance);
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000BF1F8 File Offset: 0x000BD5F8
		private GameObject GetPrefab(string PrefabName)
		{
			for (int i = 0; i < this.MenuScreens.Length; i++)
			{
				if (this.MenuScreens[i].name == PrefabName)
				{
					return this.MenuScreens[i].gameObject;
				}
			}
			throw new MissingReferenceException("Prefab not found for " + PrefabName);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000BF254 File Offset: 0x000BD654
		private T GetPrefab<T>() where T : Menu
		{
			FieldInfo[] fields = base.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				T t = fieldInfo.GetValue(this) as T;
				if (t != null)
				{
					return t;
				}
			}
			throw new MissingReferenceException("Prefab not found for type " + typeof(T));
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000BF2CC File Offset: 0x000BD6CC
		public void CloseMenu(Menu menu)
		{
			if (this.menuStack.Count == 0)
			{
				Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", new object[]
				{
					menu.GetType()
				});
				return;
			}
			if (this.menuStack.Peek() != menu)
			{
				Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", new object[]
				{
					menu.GetType()
				});
				return;
			}
			this.CloseTopMenu();
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000BF33C File Offset: 0x000BD73C
		public void CloseTopMenu()
		{
			Menu menu = this.menuStack.Pop();
			if (menu.DestroyWhenClosed)
			{
				Object.Destroy(menu.gameObject);
			}
			else
			{
				menu.gameObject.SetActive(false);
			}
			foreach (Menu menu2 in this.menuStack)
			{
				menu2.gameObject.SetActive(true);
				if (menu2.DisableMenusUnderneath)
				{
					break;
				}
			}
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000BF3E0 File Offset: 0x000BD7E0
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape) && this.menuStack.Count > 0)
			{
				this.menuStack.Peek().OnBackPressed();
			}
		}

		// Token: 0x04001BE6 RID: 7142
		public Menu[] MenuScreens;

		// Token: 0x04001BE7 RID: 7143
		public int StartScreen;

		// Token: 0x04001BE8 RID: 7144
		private Stack<Menu> menuStack = new Stack<Menu>();

		// Token: 0x04001BE9 RID: 7145
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static MenuManager <Instance>k__BackingField;
	}
}
