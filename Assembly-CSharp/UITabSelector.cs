using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E0E RID: 3598
public class UITabSelector : MonoBehaviour
{
	// Token: 0x06006EF0 RID: 28400 RVA: 0x00299F30 File Offset: 0x00298330
	public UITabSelector()
	{
	}

	// Token: 0x1700103A RID: 4154
	// (get) Token: 0x06006EF1 RID: 28401 RVA: 0x00299F4E File Offset: 0x0029834E
	// (set) Token: 0x06006EF2 RID: 28402 RVA: 0x00299F56 File Offset: 0x00298356
	public string alternateStartingTabName
	{
		[CompilerGenerated]
		get
		{
			return this.<alternateStartingTabName>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<alternateStartingTabName>k__BackingField = value;
		}
	}

	// Token: 0x06006EF3 RID: 28403 RVA: 0x00299F60 File Offset: 0x00298360
	private string FindActiveToggle()
	{
		IEnumerator enumerator = this.toggleContainer.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeSelf)
				{
					Toggle component = transform.GetComponent<Toggle>();
					if (component != null && component.isOn)
					{
						return transform.name;
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}

	// Token: 0x06006EF4 RID: 28404 RVA: 0x0029A000 File Offset: 0x00298400
	public void SelectNextTab()
	{
		bool flag = false;
		IEnumerator enumerator = this.toggleContainer.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeSelf)
				{
					Toggle component = transform.GetComponent<Toggle>();
					if (component != null)
					{
						if (flag)
						{
							component.isOn = true;
							this.SetActiveTab(transform.name);
							break;
						}
						if (component.isOn)
						{
							flag = true;
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06006EF5 RID: 28405 RVA: 0x0029A0B0 File Offset: 0x002984B0
	public void SelectPreviousTab()
	{
		Toggle toggle = null;
		IEnumerator enumerator = this.toggleContainer.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeSelf)
				{
					Toggle component = transform.GetComponent<Toggle>();
					if (component != null)
					{
						if (component.isOn && toggle != null)
						{
							toggle.isOn = true;
							this.SetActiveTab(toggle.name);
							break;
						}
						toggle = component;
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x1700103B RID: 4155
	// (get) Token: 0x06006EF6 RID: 28406 RVA: 0x0029A168 File Offset: 0x00298568
	// (set) Token: 0x06006EF7 RID: 28407 RVA: 0x0029A170 File Offset: 0x00298570
	public string activeTabName
	{
		[CompilerGenerated]
		get
		{
			return this.<activeTabName>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<activeTabName>k__BackingField = value;
		}
	}

	// Token: 0x06006EF8 RID: 28408 RVA: 0x0029A17C File Offset: 0x0029857C
	public bool HasTab(string name)
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				UITab component = transform.GetComponent<UITab>();
				if (component != null && component.name == name)
				{
					return true;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return false;
	}

	// Token: 0x06006EF9 RID: 28409 RVA: 0x0029A20C File Offset: 0x0029860C
	public void SetActiveTab(string name)
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				UITab component = transform.GetComponent<UITab>();
				if (component != null)
				{
					if (component.name == name)
					{
						this.activeTabName = name;
						component.gameObject.SetActive(true);
					}
					else
					{
						component.gameObject.SetActive(false);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (this.toggleContainer == null)
		{
			this.toggleContainer = base.transform;
		}
		IEnumerator enumerator2 = this.toggleContainer.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				Transform transform2 = (Transform)obj2;
				Toggle component2 = transform2.GetComponent<Toggle>();
				if (component2 != null && component2.name == name)
				{
					component2.isOn = true;
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	// Token: 0x06006EFA RID: 28410 RVA: 0x0029A354 File Offset: 0x00298754
	protected void ActiveTabChangedProcess()
	{
		if (this.activeTabChanged)
		{
			this.activeTabChanged = false;
			if (UITabSelector.activeTabSelector != null && UITabSelector.activeTabSelector.tabBackground != null)
			{
				UITabSelector.activeTabSelector.tabBackground.color = this.normalColor;
			}
			if (base.gameObject.activeInHierarchy)
			{
				UITabSelector.activeTabSelector = this;
				if (this.tabBackground != null)
				{
					this.tabBackground.color = this.activeColor;
				}
			}
			string text = this.FindActiveToggle();
			if (text != null)
			{
				this.SetActiveTab(text);
			}
		}
	}

	// Token: 0x06006EFB RID: 28411 RVA: 0x0029A3F9 File Offset: 0x002987F9
	public void ActiveTabChanged()
	{
		this.activeTabChanged = true;
	}

	// Token: 0x06006EFC RID: 28412 RVA: 0x0029A402 File Offset: 0x00298802
	private void Update()
	{
		this.ActiveTabChangedProcess();
	}

	// Token: 0x06006EFD RID: 28413 RVA: 0x0029A40A File Offset: 0x0029880A
	private void Awake()
	{
		if (this.toggleContainer == null)
		{
			this.toggleContainer = base.transform;
		}
	}

	// Token: 0x06006EFE RID: 28414 RVA: 0x0029A42C File Offset: 0x0029882C
	private void Start()
	{
		if (this.startingTabName != string.Empty)
		{
			this.SetActiveTab(this.startingTabName);
		}
		if (this.alternateStartingTabName != null && this.alternateStartingTabName != string.Empty && this.HasTab(this.alternateStartingTabName))
		{
			this.SetActiveTab(this.alternateStartingTabName);
		}
	}

	// Token: 0x04005FFA RID: 24570
	public static UITabSelector activeTabSelector;

	// Token: 0x04005FFB RID: 24571
	public Color normalColor = Color.white;

	// Token: 0x04005FFC RID: 24572
	public Color activeColor = Color.blue;

	// Token: 0x04005FFD RID: 24573
	public Image tabBackground;

	// Token: 0x04005FFE RID: 24574
	public Transform toggleContainer;

	// Token: 0x04005FFF RID: 24575
	public string startingTabName;

	// Token: 0x04006000 RID: 24576
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string <alternateStartingTabName>k__BackingField;

	// Token: 0x04006001 RID: 24577
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string <activeTabName>k__BackingField;

	// Token: 0x04006002 RID: 24578
	protected bool activeTabChanged;
}
