using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MHLab.PATCH.Settings;
using UnityEngine;

// Token: 0x0200033C RID: 828
public class Localizatron : Singleton<Localizatron>
{
	// Token: 0x06001417 RID: 5143 RVA: 0x00074516 File Offset: 0x00072916
	public Localizatron()
	{
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x0007451E File Offset: 0x0007291E
	public Dictionary<string, string> GetLanguageTable()
	{
		return this.languageTable;
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x00074528 File Offset: 0x00072928
	public bool SetLanguage(string language)
	{
		if (Regex.IsMatch(language, "^[a-z]{2}_[A-Z]{2}$"))
		{
			this._currentLanguage = language;
			this._languagePath = this._currentLanguage;
			this.languageTable = this.loadLanguageTable(this._languagePath);
			Debug.Log("[Localizatron] Locale loaded at: " + this._languagePath);
			return true;
		}
		return false;
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x00074582 File Offset: 0x00072982
	public string GetCurrentLanguage()
	{
		return this._currentLanguage;
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x0007458A File Offset: 0x0007298A
	public string Translate(string key)
	{
		if (this.languageTable == null)
		{
			return key;
		}
		if (this.languageTable.ContainsKey(key))
		{
			return this.languageTable[key];
		}
		return key;
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x000745B8 File Offset: 0x000729B8
	public Dictionary<string, string> loadLanguageTable(string fileName)
	{
		Dictionary<string, string> result;
		try
		{
			TextAsset textAsset = Resources.Load<TextAsset>("Localizatron/Locale/" + fileName);
			string text = textAsset.text;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Regex regex = new Regex("<key>(.*?)</key>");
			Regex regex2 = new Regex("<value>(.*?)</value>");
			MatchCollection matchCollection = regex.Matches(text);
			MatchCollection matchCollection2 = regex2.Matches(text);
			IEnumerator enumerator = matchCollection.GetEnumerator();
			IEnumerator enumerator2 = matchCollection2.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator2.MoveNext();
				dictionary.Add(enumerator.Current.ToString().Replace("<key>", string.Empty).Replace("</key>", string.Empty), enumerator2.Current.ToString().Replace("<value>", string.Empty).Replace("</value>", string.Empty));
			}
			result = dictionary;
		}
		catch (FileNotFoundException ex)
		{
			Debug.Log(ex.Message);
			result = null;
		}
		return result;
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000746C4 File Offset: 0x00072AC4
	private void Init()
	{
		this.SetLanguage(Settings.LANGUAGE_DEFAULT);
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x000746D2 File Offset: 0x00072AD2
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x0400116A RID: 4458
	private string _languagePath;

	// Token: 0x0400116B RID: 4459
	private string _currentLanguage;

	// Token: 0x0400116C RID: 4460
	private Dictionary<string, string> languageTable;
}
