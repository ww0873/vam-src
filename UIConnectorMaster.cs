using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D07 RID: 3335
public class UIConnectorMaster : MonoBehaviour
{
	// Token: 0x060065B9 RID: 26041 RVA: 0x002657E9 File Offset: 0x00263BE9
	public UIConnectorMaster()
	{
	}

	// Token: 0x060065BA RID: 26042 RVA: 0x002657FC File Offset: 0x00263BFC
	protected virtual void InitInstance()
	{
		TabbedUIBuilder componentInChildren = this.instance.GetComponentInChildren<TabbedUIBuilder>();
		if (componentInChildren != null)
		{
			if (this.configureTabbedUIBuilder)
			{
				if (this.tabs.Length > this.tabLimit)
				{
					List<TabbedUIBuilder.Tab> list = new List<TabbedUIBuilder.Tab>();
					List<TabbedUIBuilder.Tab> list2 = new List<TabbedUIBuilder.Tab>();
					for (int i = 0; i < this.tabs.Length; i++)
					{
						if (i >= this.tabLimit)
						{
							list2.Add(this.tabs[i]);
						}
						else
						{
							list.Add(this.tabs[i]);
						}
					}
					componentInChildren.tabs = list.ToArray();
					componentInChildren.column2Tabs = list2.ToArray();
				}
				else
				{
					componentInChildren.tabs = this.tabs;
				}
				componentInChildren.startingTab = this.startingTab;
				componentInChildren.alternateStartingTab = this.alternateStartingTab;
			}
			componentInChildren.Build();
		}
		UIConnector[] components = base.GetComponents<UIConnector>();
		foreach (UIConnector uiconnector in components)
		{
			if (!uiconnector.disable)
			{
				uiconnector.Connect();
			}
		}
		UIMultiConnector[] components2 = base.GetComponents<UIMultiConnector>();
		foreach (UIMultiConnector uimultiConnector in components2)
		{
			if (!uimultiConnector.disable)
			{
				uimultiConnector.Connect();
			}
		}
	}

	// Token: 0x060065BB RID: 26043 RVA: 0x00265958 File Offset: 0x00263D58
	public virtual void AddTab(TabbedUIBuilder.Tab newTab, string addBeforeTabName = null)
	{
		bool flag = false;
		int num = -1;
		if (this.tabs != null)
		{
			for (int i = 0; i < this.tabs.Length; i++)
			{
				if (this.tabs[i].name == newTab.name)
				{
					flag = true;
				}
				if (addBeforeTabName != null && this.tabs[i].name == addBeforeTabName)
				{
					num = i;
				}
			}
		}
		if (!flag)
		{
			int num2;
			if (this.tabs != null)
			{
				num2 = this.tabs.Length + 1;
			}
			else
			{
				num2 = 1;
			}
			List<TabbedUIBuilder.Tab> list = new List<TabbedUIBuilder.Tab>();
			for (int j = 0; j < num2 - 1; j++)
			{
				if (num == j)
				{
					list.Add(newTab);
				}
				list.Add(this.tabs[j]);
			}
			if (num == -1)
			{
				list.Add(newTab);
			}
			if (Application.isPlaying)
			{
				if (this.runtimeAddedTabs == null)
				{
					this.runtimeAddedTabs = new Dictionary<TabbedUIBuilder.Tab, bool>();
				}
				this.runtimeAddedTabs.Add(newTab, true);
			}
			TabbedUIBuilder.Tab[] array = list.ToArray();
			this.tabs = array;
		}
	}

	// Token: 0x060065BC RID: 26044 RVA: 0x00265A80 File Offset: 0x00263E80
	public virtual void RemoveTab(string tabName)
	{
		List<TabbedUIBuilder.Tab> list = new List<TabbedUIBuilder.Tab>();
		if (this.tabs != null)
		{
			bool flag = false;
			for (int i = 0; i < this.tabs.Length; i++)
			{
				if (this.tabs[i].name == tabName)
				{
					flag = true;
					if (this.runtimeAddedTabs != null && this.runtimeAddedTabs.ContainsKey(this.tabs[i]))
					{
						this.runtimeAddedTabs.Remove(this.tabs[i]);
					}
				}
				else
				{
					list.Add(this.tabs[i]);
				}
			}
			if (flag)
			{
				this.tabs = list.ToArray();
			}
		}
	}

	// Token: 0x060065BD RID: 26045 RVA: 0x00265B30 File Offset: 0x00263F30
	public virtual void ClearRuntimeTabs(bool skipRebuild = false)
	{
		if (Application.isPlaying && this.tabs != null && this.runtimeAddedTabs != null)
		{
			List<TabbedUIBuilder.Tab> list = new List<TabbedUIBuilder.Tab>();
			bool flag = false;
			for (int i = 0; i < this.tabs.Length; i++)
			{
				if (this.runtimeAddedTabs.ContainsKey(this.tabs[i]))
				{
					flag = true;
				}
				else
				{
					list.Add(this.tabs[i]);
				}
			}
			if (flag)
			{
				this.tabs = list.ToArray();
				if (!skipRebuild)
				{
					this.Rebuild();
				}
			}
			this.runtimeAddedTabs = new Dictionary<TabbedUIBuilder.Tab, bool>();
		}
	}

	// Token: 0x060065BE RID: 26046 RVA: 0x00265BD4 File Offset: 0x00263FD4
	public virtual void Rebuild()
	{
		if (Application.isPlaying)
		{
			if (this.instance != null)
			{
				TabbedUIBuilder componentInChildren = this.instance.GetComponentInChildren<TabbedUIBuilder>();
				if (componentInChildren != null)
				{
					this.alternateStartingTab = componentInChildren.activeTabName;
				}
				this.DeregisterCanvases();
				UIConnector[] components = base.GetComponents<UIConnector>();
				foreach (UIConnector uiconnector in components)
				{
					if (!uiconnector.disable)
					{
						uiconnector.Disconnect();
					}
				}
				UIMultiConnector[] components2 = base.GetComponents<UIMultiConnector>();
				foreach (UIMultiConnector uimultiConnector in components2)
				{
					if (!uimultiConnector.disable)
					{
						uimultiConnector.Disconnect();
					}
				}
				this.instance.SetParent(null);
				UnityEngine.Object.Destroy(this.instance.gameObject);
				this.instance = null;
			}
			this.CreateInstance();
			this.RegisterCanvases();
		}
	}

	// Token: 0x060065BF RID: 26047 RVA: 0x00265CCC File Offset: 0x002640CC
	protected void RegisterCanvases()
	{
		if (this._enabled && this.instance != null && this.containingAtom != null && this.canvases != null)
		{
			foreach (Canvas c in this.canvases)
			{
				this.containingAtom.AddCanvas(c);
			}
		}
	}

	// Token: 0x060065C0 RID: 26048 RVA: 0x00265D3C File Offset: 0x0026413C
	protected void DeregisterCanvases()
	{
		if (this.instance != null && this.containingAtom != null && this.canvases != null)
		{
			foreach (Canvas c in this.canvases)
			{
				this.containingAtom.RemoveCanvas(c);
			}
		}
	}

	// Token: 0x060065C1 RID: 26049 RVA: 0x00265DA4 File Offset: 0x002641A4
	protected virtual void CreateInstance()
	{
		if (this.instance == null)
		{
			if (this.preInstantiatedUI != null)
			{
				this.instance = this.preInstantiatedUI;
				this.InitInstance();
			}
			else if (this.UIPrefab != null)
			{
				this.instance = UnityEngine.Object.Instantiate<Transform>(this.UIPrefab);
				if (this.instance != null)
				{
					this.instance.SetParent(base.transform);
					this.instance.localPosition = Vector3.zero;
					this.instance.localRotation = Quaternion.identity;
					this.instance.localScale = Vector3.one;
				}
				this.canvases = this.instance.GetComponentsInChildren<Canvas>();
				this.InitInstance();
			}
		}
	}

	// Token: 0x060065C2 RID: 26050 RVA: 0x00265E75 File Offset: 0x00264275
	protected virtual void OnEnable()
	{
		this._enabled = true;
		this.CreateInstance();
		this.RegisterCanvases();
	}

	// Token: 0x060065C3 RID: 26051 RVA: 0x00265E8A File Offset: 0x0026428A
	protected virtual void OnDisable()
	{
		this.DeregisterCanvases();
		this._enabled = false;
	}

	// Token: 0x04005515 RID: 21781
	public Atom containingAtom;

	// Token: 0x04005516 RID: 21782
	public Transform preInstantiatedUI;

	// Token: 0x04005517 RID: 21783
	protected Transform instance;

	// Token: 0x04005518 RID: 21784
	public Transform UIPrefab;

	// Token: 0x04005519 RID: 21785
	public bool configureTabbedUIBuilder;

	// Token: 0x0400551A RID: 21786
	public int tabLimit = 20;

	// Token: 0x0400551B RID: 21787
	public string startingTab;

	// Token: 0x0400551C RID: 21788
	protected string alternateStartingTab;

	// Token: 0x0400551D RID: 21789
	public TabbedUIBuilder.Tab[] tabs;

	// Token: 0x0400551E RID: 21790
	protected Dictionary<TabbedUIBuilder.Tab, bool> runtimeAddedTabs;

	// Token: 0x0400551F RID: 21791
	protected Canvas[] canvases;

	// Token: 0x04005520 RID: 21792
	protected bool _enabled;
}
