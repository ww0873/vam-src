using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DBF RID: 3519
public class GenerateTabbedUI : MonoBehaviour
{
	// Token: 0x06006D05 RID: 27909 RVA: 0x001B4AEC File Offset: 0x001B2EEC
	public GenerateTabbedUI()
	{
	}

	// Token: 0x06006D06 RID: 27910 RVA: 0x001B4B1C File Offset: 0x001B2F1C
	protected virtual void ClearChildren(Transform t)
	{
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				UnityEngine.Object.Destroy(transform.gameObject);
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

	// Token: 0x06006D07 RID: 27911 RVA: 0x001B4B80 File Offset: 0x001B2F80
	protected virtual Transform InstantiateControl(Transform parent, int index)
	{
		Vector2 anchoredPosition = this.controlPositions[index];
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.controlUIPrefab);
		transform.SetParent(parent, false);
		RectTransform component = transform.GetComponent<RectTransform>();
		component.anchoredPosition = anchoredPosition;
		Vector2 sizeDelta = component.sizeDelta;
		sizeDelta.y = this.elementHeight;
		component.sizeDelta = sizeDelta;
		float num = 1f / (float)this.numColumns;
		Vector2 anchorMin;
		anchorMin.x = anchoredPosition.x * num + this.columnPercentBuffer;
		anchorMin.y = 1f;
		Vector2 anchorMax;
		anchorMax.x = (anchoredPosition.x + 1f) * num - this.columnPercentBuffer;
		anchorMax.y = 1f;
		component.anchorMin = anchorMin;
		component.anchorMax = anchorMax;
		return transform;
	}

	// Token: 0x06006D08 RID: 27912 RVA: 0x001B4C48 File Offset: 0x001B3048
	protected virtual Transform CreateTabButton(bool on)
	{
		this.tabCount++;
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.tabButtonUIPrefab);
		transform.name = this.tabCount.ToString();
		if (this.tabButtonParent != null)
		{
			transform.SetParent(this.tabButtonParent, false);
		}
		else
		{
			transform.SetParent(base.transform, false);
			RectTransform component = transform.GetComponent<RectTransform>();
			Vector2 zero = Vector2.zero;
			zero.x = (float)this.tabCount * component.rect.width;
			zero.y = -component.rect.height;
			RectTransform component2 = transform.GetComponent<RectTransform>();
			component2.anchoredPosition = zero;
		}
		Text[] componentsInChildren = transform.GetComponentsInChildren<Text>(true);
		if (componentsInChildren != null)
		{
			componentsInChildren[0].text = this.tabCount.ToString();
		}
		Toggle[] componentsInChildren2 = transform.GetComponentsInChildren<Toggle>(true);
		if (this.tg != null && componentsInChildren2 != null)
		{
			GenerateTabbedUI.<CreateTabButton>c__AnonStorey0 <CreateTabButton>c__AnonStorey = new GenerateTabbedUI.<CreateTabButton>c__AnonStorey0();
			<CreateTabButton>c__AnonStorey.$this = this;
			componentsInChildren2[0].isOn = on;
			componentsInChildren2[0].group = this.tg;
			<CreateTabButton>c__AnonStorey.toggleName = transform.name;
			this.tabNumToToggle.Add(this.tabCount, componentsInChildren2[0]);
			componentsInChildren2[0].onValueChanged.AddListener(new UnityAction<bool>(<CreateTabButton>c__AnonStorey.<>m__0));
		}
		return transform;
	}

	// Token: 0x06006D09 RID: 27913 RVA: 0x001B4DB8 File Offset: 0x001B31B8
	public virtual void GotoTab(int tabNum)
	{
		Toggle toggle;
		if (this.tabNumToToggle.TryGetValue(tabNum, out toggle))
		{
			toggle.isOn = true;
		}
	}

	// Token: 0x06006D0A RID: 27914 RVA: 0x001B4DE0 File Offset: 0x001B31E0
	public virtual void TabChange(string name, bool on)
	{
		if (on && this.controlContainer != null)
		{
			this.ClearChildren(this.controlContainer);
			int num;
			if (int.TryParse(name, out num))
			{
				this.currentTab = num;
				int num2 = (num - 1) * this.numElementsPerTab;
				int num3 = num2 + this.numElementsPerTab;
				if (num3 > this.controlsCount)
				{
					num3 = this.controlsCount;
				}
				for (int i = num2; i < num3; i++)
				{
					this.InstantiateControl(this.controlContainer, i);
				}
			}
		}
	}

	// Token: 0x06006D0B RID: 27915 RVA: 0x001B4E6C File Offset: 0x001B326C
	protected void AllocateControl()
	{
		this.controlsCount++;
		if (this.controlsOnColumnCount == this.numElementsPerColumn)
		{
			this.currentColumn++;
			this.controlsOnColumnCount = 0;
		}
		if (this.controlsOnTabCount == this.numElementsPerTab)
		{
			this.CreateTabButton(false);
			this.controlsOnTabCount = 0;
			this.currentColumn = 0;
			this.controlsOnColumnCount = 0;
		}
		this.controlsOnTabCount++;
		this.controlsOnColumnCount++;
		Vector2 zero = Vector2.zero;
		zero.x = (float)this.currentColumn;
		zero.y = (float)this.controlsOnColumnCount * -this.elementHeight - (float)(this.controlsOnColumnCount - 1) * this.rowBuffer + this.elementHeight * 0.5f;
		this.controlPositions.Add(zero);
	}

	// Token: 0x06006D0C RID: 27916 RVA: 0x001B4F4C File Offset: 0x001B334C
	protected virtual void GenerateStart()
	{
		if (this.tabButtonUIPrefab != null && this.controlUIPrefab != null)
		{
			this.ClearChildren(base.transform);
			if (this.tabButtonParent != null)
			{
				this.ClearChildren(this.tabButtonParent);
			}
			this.controlPositions = new List<Vector2>();
			this.tg = base.GetComponent<ToggleGroup>();
			this.controlsCount = 0;
			this.controlsOnTabCount = 0;
			this.controlsOnColumnCount = 0;
			this.currentColumn = 0;
			this.tabCount = 0;
			if (this.tabNumToToggle == null)
			{
				this.tabNumToToggle = new Dictionary<int, Toggle>();
			}
			else
			{
				this.tabNumToToggle.Clear();
			}
			this.tabSelector = base.GetComponent<UITabSelector>();
			this.controlContainer = UnityEngine.Object.Instantiate<Transform>(this.tabUIPrefab);
			this.controlContainer.SetParent(base.transform, false);
			this.controlContainer.gameObject.SetActive(true);
			this.controlContainer.name = "ControlContainer";
			this.CreateTabButton(true);
			RectTransform component = this.controlContainer.GetComponent<RectTransform>();
			this.numElementsPerColumn = Mathf.FloorToInt(component.rect.height / (this.elementHeight + this.rowBuffer));
			this.numElementsPerTab = this.numElementsPerColumn * this.numColumns;
		}
	}

	// Token: 0x06006D0D RID: 27917 RVA: 0x001B50A2 File Offset: 0x001B34A2
	protected virtual void Generate()
	{
	}

	// Token: 0x06006D0E RID: 27918 RVA: 0x001B50A4 File Offset: 0x001B34A4
	protected virtual void GenerateFinish()
	{
		this.TabChange("1", true);
	}

	// Token: 0x06006D0F RID: 27919 RVA: 0x001B50B2 File Offset: 0x001B34B2
	protected virtual void Start()
	{
		this.GenerateStart();
		this.Generate();
		this.GenerateFinish();
	}

	// Token: 0x04005E87 RID: 24199
	public float elementHeight = 100f;

	// Token: 0x04005E88 RID: 24200
	public int numColumns = 1;

	// Token: 0x04005E89 RID: 24201
	public float columnPercentBuffer = 0.01f;

	// Token: 0x04005E8A RID: 24202
	public float rowBuffer = 10f;

	// Token: 0x04005E8B RID: 24203
	public Transform controlUIPrefab;

	// Token: 0x04005E8C RID: 24204
	public Transform tabUIPrefab;

	// Token: 0x04005E8D RID: 24205
	public Transform tabButtonUIPrefab;

	// Token: 0x04005E8E RID: 24206
	public Transform tabButtonParent;

	// Token: 0x04005E8F RID: 24207
	protected UITabSelector tabSelector;

	// Token: 0x04005E90 RID: 24208
	protected int numElementsPerColumn;

	// Token: 0x04005E91 RID: 24209
	protected int numElementsPerTab;

	// Token: 0x04005E92 RID: 24210
	protected int controlsCount;

	// Token: 0x04005E93 RID: 24211
	protected int controlsOnColumnCount;

	// Token: 0x04005E94 RID: 24212
	protected int controlsOnTabCount;

	// Token: 0x04005E95 RID: 24213
	protected int tabCount;

	// Token: 0x04005E96 RID: 24214
	protected int currentColumn;

	// Token: 0x04005E97 RID: 24215
	protected int currentTab;

	// Token: 0x04005E98 RID: 24216
	protected List<Vector2> controlPositions;

	// Token: 0x04005E99 RID: 24217
	protected ToggleGroup tg;

	// Token: 0x04005E9A RID: 24218
	protected Transform controlContainer;

	// Token: 0x04005E9B RID: 24219
	protected Dictionary<int, Toggle> tabNumToToggle;

	// Token: 0x0200103A RID: 4154
	[CompilerGenerated]
	private sealed class <CreateTabButton>c__AnonStorey0
	{
		// Token: 0x06007789 RID: 30601 RVA: 0x001B50C6 File Offset: 0x001B34C6
		public <CreateTabButton>c__AnonStorey0()
		{
		}

		// Token: 0x0600778A RID: 30602 RVA: 0x001B50CE File Offset: 0x001B34CE
		internal void <>m__0(bool arg0)
		{
			this.$this.TabChange(this.toggleName, arg0);
			if (this.$this.tabSelector != null)
			{
				this.$this.tabSelector.ActiveTabChanged();
			}
		}

		// Token: 0x04006B8A RID: 27530
		internal string toggleName;

		// Token: 0x04006B8B RID: 27531
		internal GenerateTabbedUI $this;
	}
}
