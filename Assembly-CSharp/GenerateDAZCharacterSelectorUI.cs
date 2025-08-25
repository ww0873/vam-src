using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B21 RID: 2849
public class GenerateDAZCharacterSelectorUI : GenerateTabbedUI
{
	// Token: 0x06004DB3 RID: 19891 RVA: 0x001B5108 File Offset: 0x001B3508
	public GenerateDAZCharacterSelectorUI()
	{
	}

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001B5110 File Offset: 0x001B3510
	private void OnClick(string characterName)
	{
		if (this.characterSelector != null && !this.ignoreClick)
		{
			this.characterSelector.SelectCharacterByName(characterName, false);
		}
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001B513C File Offset: 0x001B353C
	public void SetActiveCharacterToggle(string characterName)
	{
		if (this.characters != null && this.characterNameToToggle != null)
		{
			for (int i = 0; i < this.characters.Length; i++)
			{
				string displayName = this.characters[i].displayName;
				Toggle toggle;
				if (this.characterNameToToggle.TryGetValue(displayName, out toggle))
				{
					if (displayName == characterName)
					{
						toggle.isOn = true;
					}
					else
					{
						toggle.isOn = false;
					}
				}
			}
		}
	}

	// Token: 0x06004DB6 RID: 19894 RVA: 0x001B51B8 File Offset: 0x001B35B8
	public void SetActiveCharacterToggleNoCallback(string characterName)
	{
		this.ignoreClick = true;
		this.SetActiveCharacterToggle(characterName);
		this.ignoreClick = false;
	}

	// Token: 0x06004DB7 RID: 19895 RVA: 0x001B51CF File Offset: 0x001B35CF
	public override void TabChange(string name, bool on)
	{
		this.characterNameToToggle = new Dictionary<string, Toggle>();
		base.TabChange(name, on);
	}

	// Token: 0x06004DB8 RID: 19896 RVA: 0x001B51E4 File Offset: 0x001B35E4
	protected override Transform InstantiateControl(Transform parent, int index)
	{
		Transform transform = base.InstantiateControl(parent, index);
		DAZCharacter dazcharacter = this.characters[index];
		string displayName = dazcharacter.displayName;
		Toggle component = transform.GetComponent<Toggle>();
		if (component != null)
		{
			GenerateDAZCharacterSelectorUI.<InstantiateControl>c__AnonStorey0 <InstantiateControl>c__AnonStorey = new GenerateDAZCharacterSelectorUI.<InstantiateControl>c__AnonStorey0();
			<InstantiateControl>c__AnonStorey.$this = this;
			<InstantiateControl>c__AnonStorey.cname = displayName;
			this.characterNameToToggle.Add(<InstantiateControl>c__AnonStorey.cname, component);
			component.onValueChanged.AddListener(new UnityAction<bool>(<InstantiateControl>c__AnonStorey.<>m__0));
			if (this.characterSelector.selectedCharacter != null && this.characterSelector.selectedCharacter.displayName == displayName)
			{
				component.isOn = true;
			}
			else
			{
				component.isOn = false;
			}
		}
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform2 = (Transform)obj;
				if (transform2.name == "Text")
				{
					Text component2 = transform2.GetComponent<Text>();
					if (component2 != null)
					{
						component2.text = displayName;
					}
				}
				else if (transform2.name == "TextAlt")
				{
					Text component3 = transform2.GetComponent<Text>();
					if (component3 != null)
					{
						component3.text = dazcharacter.displayNameAlt;
					}
				}
				else if (transform2.name == "UVText")
				{
					Text component4 = transform2.GetComponent<Text>();
					if (component4 != null)
					{
						component4.text = dazcharacter.UVname;
					}
				}
				else if (transform2.name == "RawImage")
				{
					RawImage component5 = transform2.GetComponent<RawImage>();
					if (component5 != null)
					{
						component5.texture = this.characters[index].thumbnail;
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
		return transform;
	}

	// Token: 0x06004DB9 RID: 19897 RVA: 0x001B53F8 File Offset: 0x001B37F8
	public void AutoGenerateItems()
	{
		if (this.characterSelector != null)
		{
			this.characters = this.characterSelector.characters;
		}
	}

	// Token: 0x06004DBA RID: 19898 RVA: 0x001B541C File Offset: 0x001B381C
	protected override void Generate()
	{
		this.AutoGenerateItems();
		base.Generate();
		this.characterNameToToggle = new Dictionary<string, Toggle>();
		if (this.controlUIPrefab != null && this.tabUIPrefab != null && this.tabButtonUIPrefab != null && this.characters != null)
		{
			for (int i = 0; i < this.characters.Length; i++)
			{
				base.AllocateControl();
			}
		}
	}

	// Token: 0x04003D6E RID: 15726
	public DAZCharacterSelector characterSelector;

	// Token: 0x04003D6F RID: 15727
	private bool ignoreClick;

	// Token: 0x04003D70 RID: 15728
	protected DAZCharacter[] characters;

	// Token: 0x04003D71 RID: 15729
	protected Dictionary<string, Toggle> characterNameToToggle;

	// Token: 0x02000FCF RID: 4047
	[CompilerGenerated]
	private sealed class <InstantiateControl>c__AnonStorey0
	{
		// Token: 0x0600754D RID: 30029 RVA: 0x001B549D File Offset: 0x001B389D
		public <InstantiateControl>c__AnonStorey0()
		{
		}

		// Token: 0x0600754E RID: 30030 RVA: 0x001B54A5 File Offset: 0x001B38A5
		internal void <>m__0(bool arg0)
		{
			if (arg0)
			{
				this.$this.OnClick(this.cname);
			}
		}

		// Token: 0x04006962 RID: 26978
		internal string cname;

		// Token: 0x04006963 RID: 26979
		internal GenerateDAZCharacterSelectorUI $this;
	}
}
