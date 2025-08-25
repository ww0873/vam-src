using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DE5 RID: 3557
public class TabbedUIBuilder : MonoBehaviour
{
	// Token: 0x06006E00 RID: 28160 RVA: 0x00294CF7 File Offset: 0x002930F7
	public TabbedUIBuilder()
	{
	}

	// Token: 0x06006E01 RID: 28161 RVA: 0x00294D0C File Offset: 0x0029310C
	protected void CopyFields(Type t, object source, object dest)
	{
		if (source.GetType() == dest.GetType())
		{
			foreach (FieldInfo fieldInfo in t.GetFields())
			{
				try
				{
					object value = fieldInfo.GetValue(source);
					if (!object.ReferenceEquals(value, null))
					{
						if (value.GetType().IsSubclassOf(typeof(Transform)))
						{
							Transform x = value as Transform;
							if (x != null)
							{
								fieldInfo.SetValue(dest, value);
							}
						}
						else
						{
							fieldInfo.SetValue(dest, value);
						}
					}
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.Log("Caught exception " + arg);
				}
			}
		}
	}

	// Token: 0x06006E02 RID: 28162 RVA: 0x00294DD4 File Offset: 0x002931D4
	public void AddTab(TabbedUIBuilder.Tab tab, bool addTab = true, bool column2 = false)
	{
		if (this.selector != null && tab.prefab != null)
		{
			if (this.tabPrefab != null && addTab)
			{
				Transform transform;
				if (column2 && this.column2TabPrefab != null)
				{
					transform = UnityEngine.Object.Instantiate<Transform>(this.column2TabPrefab);
				}
				else
				{
					transform = UnityEngine.Object.Instantiate<Transform>(this.tabPrefab);
				}
				transform.SetParent(this.selector.toggleContainer, false);
				transform.name = tab.name;
				RectTransform component = transform.GetComponent<RectTransform>();
				if (component != null)
				{
					Vector2 zero = Vector2.zero;
					zero.y = this.currentYPos;
					if (column2)
					{
						zero.x += this.column2Offset;
					}
					component.anchoredPosition = zero;
					Vector2 sizeDelta = component.sizeDelta;
					this.currentYPos += sizeDelta.y;
					this.tabWidth = sizeDelta.x;
					if (this.currentYPos > this.maxHeight)
					{
						this.maxHeight = this.currentYPos;
					}
				}
				UISideAlign component2 = transform.GetComponent<UISideAlign>();
				if (component2 != null)
				{
					component2.Sync();
				}
				foreach (Image image in transform.GetComponentsInChildren<Image>())
				{
					image.color = tab.color;
				}
				Toggle component3 = transform.GetComponent<Toggle>();
				if (component3 != null)
				{
					component3.onValueChanged.AddListener(new UnityAction<bool>(this.<AddTab>m__0));
					ToggleGroup component4 = this.selector.GetComponent<ToggleGroup>();
					if (component4 != null)
					{
						component3.group = component4;
					}
				}
				Text componentInChildren = transform.GetComponentInChildren<Text>();
				if (componentInChildren != null)
				{
					componentInChildren.text = tab.name;
				}
			}
			Transform transform2;
			if (tab.prefab.gameObject.scene.rootCount == 0)
			{
				transform2 = UnityEngine.Object.Instantiate<Transform>(tab.prefab);
				transform2.SetParent(this.selector.transform, false);
			}
			else
			{
				transform2 = tab.prefab;
			}
			transform2.name = tab.name;
			Image component5 = transform2.GetComponent<Image>();
			if (component5 != null)
			{
				component5.color = tab.color;
			}
			foreach (UIProvider uiprovider in transform2.GetComponentsInChildren<UIProvider>())
			{
				Type type = uiprovider.GetType();
				if (!uiprovider.completeProvider)
				{
					UIProvider uiprovider2 = (UIProvider)base.GetComponent(type);
					if (uiprovider2 == null)
					{
						uiprovider2 = (UIProvider)base.gameObject.AddComponent(type);
					}
					if (uiprovider2 != null)
					{
						this.CopyFields(type, uiprovider, uiprovider2);
						uiprovider2.completeProvider = true;
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to AddTab when UITabSelector is null");
		}
	}

	// Token: 0x17001010 RID: 4112
	// (get) Token: 0x06006E03 RID: 28163 RVA: 0x002950D7 File Offset: 0x002934D7
	// (set) Token: 0x06006E04 RID: 28164 RVA: 0x002950DF File Offset: 0x002934DF
	public string alternateStartingTab
	{
		[CompilerGenerated]
		get
		{
			return this.<alternateStartingTab>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<alternateStartingTab>k__BackingField = value;
		}
	}

	// Token: 0x17001011 RID: 4113
	// (get) Token: 0x06006E05 RID: 28165 RVA: 0x002950E8 File Offset: 0x002934E8
	public string activeTabName
	{
		get
		{
			if (this.selector != null)
			{
				return this.selector.activeTabName;
			}
			return null;
		}
	}

	// Token: 0x06006E06 RID: 28166 RVA: 0x00295108 File Offset: 0x00293508
	public void Build()
	{
		if (!this.wasBuilt && this.tabs != null && this.tabs.Length > 0)
		{
			this.wasBuilt = true;
			bool addTab = true;
			if (this.tabs.Length == 1)
			{
				addTab = false;
			}
			if (this.selector != null)
			{
				GameObject gameObject = new GameObject("ToggleContainer");
				gameObject.transform.SetParent(this.selector.transform);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				this.selector.toggleContainer = gameObject.transform;
				Vector2 vector;
				vector.x = 1f;
				vector.y = 0f;
				rectTransform.anchorMin = vector;
				rectTransform.anchorMax = vector;
				Vector2 zero = Vector2.zero;
				rectTransform.pivot = zero;
				Vector2 zero2 = Vector2.zero;
				rectTransform.anchoredPosition = zero2;
				Vector2 sizeDelta;
				if (this.column2Tabs != null && this.column2Tabs.Length > 0)
				{
					sizeDelta.x = this.tabWidth * 2f;
				}
				else
				{
					sizeDelta.x = this.tabWidth;
				}
				sizeDelta.y = this.maxHeight;
				rectTransform.sizeDelta = sizeDelta;
				UISideAlign uisideAlign = gameObject.AddComponent<UISideAlign>();
				uisideAlign.Sync();
				foreach (TabbedUIBuilder.Tab tab in this.tabs)
				{
					this.AddTab(tab, addTab, false);
				}
				if (this.column2Tabs != null && this.column2Tabs.Length > 0)
				{
					this.currentYPos = 0f;
					foreach (TabbedUIBuilder.Tab tab2 in this.column2Tabs)
					{
						this.AddTab(tab2, addTab, true);
					}
				}
				this.selector.startingTabName = this.startingTab;
				this.selector.alternateStartingTabName = this.alternateStartingTab;
				if (this.selector.transform.parent != null && this.selector.transform.parent.parent != null)
				{
					gameObject = new GameObject("TabsBackPanel");
					gameObject.transform.SetParent(this.selector.transform.parent.parent);
					gameObject.transform.SetAsFirstSibling();
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.identity;
					rectTransform = gameObject.AddComponent<RectTransform>();
					vector.x = 1f;
					vector.y = 0f;
					rectTransform.anchorMin = vector;
					rectTransform.anchorMax = vector;
					rectTransform.pivot = zero;
					zero2.x = -25f;
					zero2.y = -10f;
					rectTransform.anchoredPosition = zero2;
					if (this.column2Tabs != null && this.column2Tabs.Length > 0)
					{
						sizeDelta.x = this.tabWidth * 2f + 50f;
					}
					else
					{
						sizeDelta.x = this.tabWidth + 50f;
					}
					sizeDelta.y = this.maxHeight + 35f;
					rectTransform.sizeDelta = sizeDelta;
					uisideAlign = gameObject.AddComponent<UISideAlign>();
					uisideAlign.leftSideOffsetX = 25f;
					uisideAlign.rightSideOffsetX = -25f;
					uisideAlign.Sync();
					Image image = gameObject.AddComponent<Image>();
					Color color = image.color;
					color.a = 0.02f;
					image.color = color;
					gameObject = new GameObject("BackPanel");
					gameObject.transform.SetParent(this.selector.transform.parent.parent);
					gameObject.transform.SetAsFirstSibling();
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.identity;
					rectTransform = gameObject.AddComponent<RectTransform>();
					vector.x = 0f;
					vector.y = 0f;
					rectTransform.anchorMin = vector;
					Vector2 anchorMax;
					anchorMax.x = 1f;
					anchorMax.y = 1f;
					rectTransform.anchorMax = anchorMax;
					zero.x = 0.5f;
					zero.y = 0.5f;
					rectTransform.pivot = zero;
					zero2.x = 0f;
					zero2.y = 7.5f;
					rectTransform.anchoredPosition = zero2;
					sizeDelta.x = 50f;
					sizeDelta.y = 35f;
					rectTransform.sizeDelta = sizeDelta;
					image = gameObject.AddComponent<Image>();
					image.color = color;
				}
			}
		}
	}

	// Token: 0x06006E07 RID: 28167 RVA: 0x002955E1 File Offset: 0x002939E1
	private void Awake()
	{
		this.Build();
	}

	// Token: 0x06006E08 RID: 28168 RVA: 0x002955E9 File Offset: 0x002939E9
	[CompilerGenerated]
	private void <AddTab>m__0(bool b)
	{
		this.selector.ActiveTabChanged();
	}

	// Token: 0x04005F39 RID: 24377
	public Transform tabPrefab;

	// Token: 0x04005F3A RID: 24378
	public Transform column2TabPrefab;

	// Token: 0x04005F3B RID: 24379
	public float column2Offset = 200f;

	// Token: 0x04005F3C RID: 24380
	public UITabSelector selector;

	// Token: 0x04005F3D RID: 24381
	protected float currentYPos;

	// Token: 0x04005F3E RID: 24382
	protected float tabWidth;

	// Token: 0x04005F3F RID: 24383
	protected float maxHeight;

	// Token: 0x04005F40 RID: 24384
	public string startingTab;

	// Token: 0x04005F41 RID: 24385
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string <alternateStartingTab>k__BackingField;

	// Token: 0x04005F42 RID: 24386
	public TabbedUIBuilder.Tab[] tabs;

	// Token: 0x04005F43 RID: 24387
	public TabbedUIBuilder.Tab[] column2Tabs;

	// Token: 0x04005F44 RID: 24388
	protected bool wasBuilt;

	// Token: 0x02000DE6 RID: 3558
	[Serializable]
	public class Tab
	{
		// Token: 0x06006E09 RID: 28169 RVA: 0x002955F6 File Offset: 0x002939F6
		public Tab()
		{
		}

		// Token: 0x04005F45 RID: 24389
		public string name;

		// Token: 0x04005F46 RID: 24390
		public Transform prefab;

		// Token: 0x04005F47 RID: 24391
		public Color color;
	}
}
