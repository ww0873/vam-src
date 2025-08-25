using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C0F RID: 3087
public class GenerateEmbeddedSceneSelectorUI : GenerateTabbedUI
{
	// Token: 0x060059C5 RID: 22981 RVA: 0x0021063B File Offset: 0x0020EA3B
	public GenerateEmbeddedSceneSelectorUI()
	{
	}

	// Token: 0x060059C6 RID: 22982 RVA: 0x00210644 File Offset: 0x0020EA44
	private void OnClick(string sceneName)
	{
		JSONEmbed je;
		if (this.sceneNameToJSONEmbed != null && this.sceneNameToJSONEmbed.TryGetValue(sceneName, out je))
		{
			SuperController.singleton.LoadFromJSONEmbed(je, false, false);
		}
	}

	// Token: 0x060059C7 RID: 22983 RVA: 0x0021067C File Offset: 0x0020EA7C
	protected override Transform InstantiateControl(Transform parent, int index)
	{
		Transform transform = base.InstantiateControl(parent, index);
		JSONEmbed jsonembed = this.existingEmbeddedScenes[index];
		string name = jsonembed.name;
		Button component = transform.GetComponent<Button>();
		if (component != null)
		{
			GenerateEmbeddedSceneSelectorUI.<InstantiateControl>c__AnonStorey0 <InstantiateControl>c__AnonStorey = new GenerateEmbeddedSceneSelectorUI.<InstantiateControl>c__AnonStorey0();
			<InstantiateControl>c__AnonStorey.$this = this;
			<InstantiateControl>c__AnonStorey.cname = name;
			component.onClick.AddListener(new UnityAction(<InstantiateControl>c__AnonStorey.<>m__0));
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
						component2.text = name;
					}
				}
				if (transform2.name == "Image")
				{
					Image component3 = transform2.GetComponent<Image>();
					if (component3 != null)
					{
						component3.sprite = jsonembed.sprite;
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

	// Token: 0x060059C8 RID: 22984 RVA: 0x002107AC File Offset: 0x0020EBAC
	protected override void Generate()
	{
		base.Generate();
		this.sceneNameToJSONEmbed = new Dictionary<string, JSONEmbed>();
		this.existingEmbeddedScenes = new List<JSONEmbed>();
		if (this.controlUIPrefab != null && this.tabUIPrefab != null && this.tabButtonUIPrefab != null && this.embeddedScenes != null)
		{
			for (int i = this.embeddedScenes.Length - 1; i >= 0; i--)
			{
				if (this.embeddedScenes[i] != null)
				{
					this.existingEmbeddedScenes.Add(this.embeddedScenes[i]);
					this.sceneNameToJSONEmbed.Add(this.embeddedScenes[i].name, this.embeddedScenes[i]);
					base.AllocateControl();
				}
			}
		}
	}

	// Token: 0x040049E0 RID: 18912
	public JSONEmbed[] embeddedScenes;

	// Token: 0x040049E1 RID: 18913
	protected List<JSONEmbed> existingEmbeddedScenes;

	// Token: 0x040049E2 RID: 18914
	protected Dictionary<string, JSONEmbed> sceneNameToJSONEmbed;

	// Token: 0x02000FFC RID: 4092
	[CompilerGenerated]
	private sealed class <InstantiateControl>c__AnonStorey0
	{
		// Token: 0x06007655 RID: 30293 RVA: 0x0021087A File Offset: 0x0020EC7A
		public <InstantiateControl>c__AnonStorey0()
		{
		}

		// Token: 0x06007656 RID: 30294 RVA: 0x00210882 File Offset: 0x0020EC82
		internal void <>m__0()
		{
			this.$this.OnClick(this.cname);
		}

		// Token: 0x04006A2B RID: 27179
		internal string cname;

		// Token: 0x04006A2C RID: 27180
		internal GenerateEmbeddedSceneSelectorUI $this;
	}
}
