using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using DynamicCSharp;
using Mono.CSharp;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C35 RID: 3125
public class MVRPluginManager : JSONStorable
{
	// Token: 0x06005AD8 RID: 23256 RVA: 0x00214E58 File Offset: 0x00213258
	public MVRPluginManager()
	{
	}

	// Token: 0x06005AD9 RID: 23257 RVA: 0x00214EAC File Offset: 0x002132AC
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (((includeAppearance && includePhysical) || forceStore) && (this.plugins.Count > 0 || forceStore))
		{
			this.needsStore = true;
			JSONClass jsonclass = new JSONClass();
			json["plugins"] = jsonclass;
			foreach (MVRPlugin mvrplugin in this.plugins)
			{
				mvrplugin.pluginURLJSON.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore);
			}
		}
		return json;
	}

	// Token: 0x06005ADA RID: 23258 RVA: 0x00214F60 File Offset: 0x00213360
	public override void PreRestore(bool restorePhysical, bool restoreAppearance)
	{
		if (restorePhysical && restoreAppearance && !base.mergeRestore && !base.physicalLocked && !base.appearanceLocked && !base.IsCustomPhysicalParamLocked("plugins") && !base.IsCustomAppearanceParamLocked("plugins"))
		{
			this.RemoveAllPlugins();
		}
	}

	// Token: 0x06005ADB RID: 23259 RVA: 0x00214FC0 File Offset: 0x002133C0
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		this.insideRestore = true;
		if (!base.physicalLocked && !base.appearanceLocked && restoreAppearance && restorePhysical && !base.IsCustomPhysicalParamLocked("plugins") && !base.IsCustomAppearanceParamLocked("plugins"))
		{
			if (jc["plugins"] != null)
			{
				if (base.mergeRestore)
				{
					this.GivePluginsNewUIDs(this.plugins, true);
				}
				else
				{
					this.RemoveAllPlugins();
				}
				JSONClass asObject = jc["plugins"].AsObject;
				List<MVRPlugin> list = new List<MVRPlugin>();
				if (asObject != null)
				{
					foreach (string text in asObject.Keys)
					{
						MVRPlugin mvrplugin = this.CreatePluginWithId(text);
						this.pluginUIDs.Add(text, true);
						list.Add(mvrplugin);
						mvrplugin.pluginURLJSON.RestoreFromJSON(asObject, true, true, true);
					}
				}
				if (base.mergeRestore)
				{
					this.GivePluginsNewUIDs(list, true);
					this.GivePluginsNewUIDs(this.plugins, false);
				}
			}
			else if (setMissingToDefault && !base.mergeRestore)
			{
				this.RemoveAllPlugins();
			}
		}
		this.insideRestore = false;
	}

	// Token: 0x06005ADC RID: 23260 RVA: 0x00215134 File Offset: 0x00213534
	protected string CreatePluginUID(bool isTempName = false)
	{
		string text;
		if (isTempName)
		{
			text = "pluginTemp#";
		}
		else
		{
			text = "plugin#";
		}
		for (int i = 0; i < 1000; i++)
		{
			string text2 = text + i.ToString();
			if (!this.pluginUIDs.ContainsKey(text2))
			{
				text = text2;
				this.pluginUIDs.Add(text, true);
				break;
			}
		}
		return text;
	}

	// Token: 0x06005ADD RID: 23261 RVA: 0x002151A8 File Offset: 0x002135A8
	protected void BeginBrowse(JSONStorableUrl jsurl)
	{
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory("Custom/Scripts", true, true, true, true);
		List<ShortCut> list = new List<ShortCut>();
		list.Add(new ShortCut
		{
			displayName = "Root",
			path = Path.GetFullPath(".")
		});
		foreach (ShortCut shortCut in shortCutsForDirectory)
		{
			VarPackage package = FileManager.GetPackage(shortCut.package);
			if (package != null)
			{
				if (!package.PluginsAlwaysDisabled)
				{
					list.Add(shortCut);
				}
			}
			else
			{
				list.Add(shortCut);
			}
		}
		jsurl.shortCuts = list;
	}

	// Token: 0x06005ADE RID: 23262 RVA: 0x00215270 File Offset: 0x00213670
	protected MVRPlugin CreatePluginWithId(string pluginUID)
	{
		MVRPluginManager.<CreatePluginWithId>c__AnonStorey0 <CreatePluginWithId>c__AnonStorey = new MVRPluginManager.<CreatePluginWithId>c__AnonStorey0();
		<CreatePluginWithId>c__AnonStorey.$this = this;
		<CreatePluginWithId>c__AnonStorey.mvrp = new MVRPlugin();
		<CreatePluginWithId>c__AnonStorey.mvrp.uid = pluginUID;
		this.plugins.Add(<CreatePluginWithId>c__AnonStorey.mvrp);
		JSONStorableUrl jsonstorableUrl = new JSONStorableUrl(pluginUID, string.Empty, new JSONStorableString.SetStringCallback(<CreatePluginWithId>c__AnonStorey.<>m__0), "cs|cslist|dll", "Custom/Scripts");
		<CreatePluginWithId>c__AnonStorey.mvrp.pluginURLJSON = jsonstorableUrl;
		jsonstorableUrl.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		if (this.pluginPanelPrefab != null)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.pluginPanelPrefab);
			if (this.pluginListPanel != null)
			{
				transform.SetParent(this.pluginListPanel, false);
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
			<CreatePluginWithId>c__AnonStorey.mvrp.configUI = transform;
			MVRPluginUI component = transform.GetComponent<MVRPluginUI>();
			if (component != null)
			{
				<CreatePluginWithId>c__AnonStorey.mvrp.scriptControllerContent = component.scriptControllerContent;
				jsonstorableUrl.fileBrowseButton = component.fileBrowseButton;
				jsonstorableUrl.clearButton = component.clearButton;
				jsonstorableUrl.reloadButton = component.reloadButton;
				jsonstorableUrl.text = component.urlText;
				if (component.uidText != null)
				{
					component.uidText.text = pluginUID;
				}
				if (component.removeButton != null)
				{
					component.removeButton.onClick.AddListener(new UnityAction(<CreatePluginWithId>c__AnonStorey.<>m__1));
				}
			}
		}
		return <CreatePluginWithId>c__AnonStorey.mvrp;
	}

	// Token: 0x06005ADF RID: 23263 RVA: 0x002153F0 File Offset: 0x002137F0
	protected void GivePluginNewUID(MVRPlugin mvrp, bool isTempName = true)
	{
		string uid = mvrp.uid;
		string text = this.CreatePluginUID(isTempName);
		mvrp.uid = text;
		if (mvrp.pluginURLJSON != null)
		{
			mvrp.pluginURLJSON.name = text;
		}
		if (mvrp.configUI != null)
		{
			MVRPluginUI component = mvrp.configUI.GetComponent<MVRPluginUI>();
			if (component != null && component.uidText != null)
			{
				component.uidText.text = text;
			}
		}
		foreach (MVRScriptController mvrscriptController in mvrp.scriptControllers)
		{
			if (mvrscriptController.script != null)
			{
				this.containingAtom.UnregisterAdditionalStorable(mvrscriptController.script);
			}
			if (mvrscriptController.gameObject != null)
			{
				mvrscriptController.gameObject.name = mvrscriptController.gameObject.name.Replace(uid, text);
				if (mvrscriptController.configUI != null)
				{
					MVRScriptControllerUI component2 = mvrscriptController.configUI.GetComponent<MVRScriptControllerUI>();
					if (component2 != null && component2.label != null)
					{
						component2.label.text = mvrscriptController.gameObject.name;
					}
				}
			}
			if (mvrscriptController.script != null)
			{
				this.containingAtom.RegisterAdditionalStorable(mvrscriptController.script);
			}
		}
		this.pluginUIDs.Remove(uid);
	}

	// Token: 0x06005AE0 RID: 23264 RVA: 0x00215590 File Offset: 0x00213990
	protected void GivePluginsNewUIDs(List<MVRPlugin> pluginsList, bool isTemp = true)
	{
		foreach (MVRPlugin mvrp in pluginsList)
		{
			this.GivePluginNewUID(mvrp, isTemp);
		}
	}

	// Token: 0x06005AE1 RID: 23265 RVA: 0x002155E8 File Offset: 0x002139E8
	public MVRPlugin CreatePlugin()
	{
		string pluginUID = this.CreatePluginUID(false);
		return this.CreatePluginWithId(pluginUID);
	}

	// Token: 0x06005AE2 RID: 23266 RVA: 0x00215604 File Offset: 0x00213A04
	public void ReloadPluginWithUID(string uid)
	{
		if (this.plugins != null)
		{
			foreach (MVRPlugin mvrplugin in this.plugins)
			{
				if (mvrplugin.uid == uid)
				{
					mvrplugin.Reload();
				}
			}
		}
	}

	// Token: 0x06005AE3 RID: 23267 RVA: 0x0021567C File Offset: 0x00213A7C
	protected void CreatePluginCallback()
	{
		this.CreatePlugin();
	}

	// Token: 0x06005AE4 RID: 23268 RVA: 0x00215688 File Offset: 0x00213A88
	protected void DestroyScriptController(MVRScriptController mvrsc)
	{
		Exception ex = null;
		if (mvrsc.script != null)
		{
			if (mvrsc.script.enabledJSON != null)
			{
				mvrsc.script.enabledJSON.toggle = null;
			}
			if (mvrsc.script.pluginLabelJSON != null)
			{
				mvrsc.script.pluginLabelJSON.inputField = null;
				mvrsc.script.pluginLabelJSON.inputFieldAction = null;
			}
			try
			{
				UnityEngine.Object.DestroyImmediate(mvrsc.script);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			mvrsc.script = null;
		}
		if (mvrsc.configUI != null)
		{
			UnityEngine.Object.Destroy(mvrsc.configUI.gameObject);
			mvrsc.configUI = null;
		}
		if (mvrsc.customUI != null)
		{
			Canvas componentInChildren = mvrsc.customUI.GetComponentInChildren<Canvas>();
			if (componentInChildren != null)
			{
				SuperController.singleton.RemoveCanvas(componentInChildren);
			}
			UnityEngine.Object.Destroy(mvrsc.customUI.gameObject);
			mvrsc.customUI = null;
		}
		if (mvrsc.gameObject != null)
		{
			UnityEngine.Object.Destroy(mvrsc.gameObject);
			mvrsc.gameObject = null;
		}
		if (ex != null)
		{
			throw ex;
		}
	}

	// Token: 0x06005AE5 RID: 23269 RVA: 0x002157C8 File Offset: 0x00213BC8
	protected void RemovePluginScriptControllers(MVRPlugin mvrp)
	{
		Exception ex = null;
		if (mvrp.scriptControllers != null)
		{
			foreach (MVRScriptController mvrscriptController in mvrp.scriptControllers)
			{
				if (mvrscriptController.script != null)
				{
					this.containingAtom.UnregisterAdditionalStorable(mvrscriptController.script);
				}
				try
				{
					this.DestroyScriptController(mvrscriptController);
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
			}
		}
		mvrp.scriptControllers = new List<MVRScriptController>();
		if (ex != null)
		{
			throw ex;
		}
	}

	// Token: 0x06005AE6 RID: 23270 RVA: 0x00215880 File Offset: 0x00213C80
	protected void RemovePlugin(MVRPlugin mvrp)
	{
		try
		{
			this.RemovePluginScriptControllers(mvrp);
			if (mvrp.configUI != null)
			{
				UnityEngine.Object.Destroy(mvrp.configUI.gameObject);
				mvrp.configUI = null;
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Exception while trying to remove plugin ",
				mvrp.pluginURLJSON.val,
				": ",
				ex
			}));
		}
		this.pluginUIDs.Remove(mvrp.uid);
		this.plugins.Remove(mvrp);
	}

	// Token: 0x06005AE7 RID: 23271 RVA: 0x00215928 File Offset: 0x00213D28
	public void RemovePluginWithUID(string uid)
	{
		if (this.plugins != null)
		{
			MVRPlugin mvrplugin = null;
			foreach (MVRPlugin mvrplugin2 in this.plugins)
			{
				if (mvrplugin2.uid == uid)
				{
					mvrplugin = mvrplugin2;
					break;
				}
			}
			if (mvrplugin != null)
			{
				this.RemovePlugin(mvrplugin);
			}
		}
	}

	// Token: 0x06005AE8 RID: 23272 RVA: 0x002159B0 File Offset: 0x00213DB0
	public void RemoveAllPlugins()
	{
		List<MVRPlugin> list = new List<MVRPlugin>(this.plugins);
		foreach (MVRPlugin mvrp in list)
		{
			this.RemovePlugin(mvrp);
		}
	}

	// Token: 0x06005AE9 RID: 23273 RVA: 0x00215A14 File Offset: 0x00213E14
	protected MVRScriptController CreateScriptController(MVRPlugin mvrp, ScriptType type)
	{
		MVRScriptController mvrscriptController = new MVRScriptController();
		GameObject gameObject = new GameObject(mvrp.uid + "temp");
		gameObject.transform.SetParent(this.pluginContainer);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		mvrscriptController.gameObject = gameObject;
		ScriptProxy scriptProxy = type.CreateInstance(gameObject);
		if (scriptProxy == null)
		{
			SuperController.LogError("Failed to create instance of " + mvrp.pluginURLJSON.val);
			UnityEngine.Object.Destroy(gameObject);
			return null;
		}
		string text = mvrp.uid + "_" + scriptProxy.GetInstanceType().ToString();
		gameObject.name = text;
		if (this.scriptUIPrefab != null)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.scriptUIPrefab);
			if (this.scriptUIParent != null)
			{
				transform.SetParent(this.scriptUIParent, false);
			}
			transform.gameObject.SetActive(false);
			mvrscriptController.customUI = transform;
		}
		Toggle toggle = null;
		InputField inputField = null;
		InputFieldAction inputFieldAction = null;
		if (this.scriptControllerPanelPrefab != null)
		{
			Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.scriptControllerPanelPrefab);
			if (mvrp.scriptControllerContent != null)
			{
				transform2.SetParent(mvrp.scriptControllerContent, false);
			}
			MVRScriptControllerUI component = transform2.GetComponent<MVRScriptControllerUI>();
			if (component != null)
			{
				if (component.label != null)
				{
					component.label.text = text;
				}
				if (component.openUIButton != null)
				{
					component.openUIButton.onClick.AddListener(new UnityAction(mvrscriptController.OpenUI));
				}
				toggle = component.enabledToggle;
				inputField = component.userLabelInputField;
				inputFieldAction = component.userLabelInputFieldAction;
			}
			mvrscriptController.configUI = transform2;
		}
		MVRScript component2 = gameObject.GetComponent<MVRScript>();
		component2.exclude = this.exclude;
		mvrscriptController.script = component2;
		if (component2.ShouldIgnore())
		{
			try
			{
				this.DestroyScriptController(mvrscriptController);
			}
			catch
			{
			}
			mvrscriptController = null;
		}
		else
		{
			try
			{
				component2.ForceAwake();
				component2.containingAtom = this.containingAtom;
				component2.manager = this;
				try
				{
					component2.Init();
				}
				catch (Exception arg)
				{
					SuperController.LogError("Exception during plugin script Init: " + arg);
				}
				if (component2.enabledJSON != null)
				{
					component2.enabledJSON.toggle = toggle;
				}
				if (component2.pluginLabelJSON != null)
				{
					component2.pluginLabelJSON.inputField = inputField;
					component2.pluginLabelJSON.inputFieldAction = inputFieldAction;
				}
				if (mvrscriptController.customUI != null)
				{
					component2.UITransform = mvrscriptController.customUI;
					component2.InitUI();
					MVRScriptUI componentInChildren = mvrscriptController.customUI.GetComponentInChildren<MVRScriptUI>();
					if (componentInChildren != null && componentInChildren.closeButton != null)
					{
						componentInChildren.closeButton.onClick.AddListener(new UnityAction(mvrscriptController.CloseUI));
					}
					Canvas componentInChildren2 = mvrscriptController.customUI.GetComponentInChildren<Canvas>();
					if (componentInChildren2 != null)
					{
						SuperController.singleton.AddCanvas(componentInChildren2);
					}
				}
				this.containingAtom.RegisterAdditionalStorable(component2);
				if (this.insideRestore)
				{
					this.containingAtom.RestoreFromLast(component2);
				}
			}
			catch (Exception innerException)
			{
				this.DestroyScriptController(mvrscriptController);
				mvrscriptController = null;
				throw new Exception("Exception during script init ", innerException);
			}
		}
		return mvrscriptController;
	}

	// Token: 0x06005AEA RID: 23274 RVA: 0x00215DE0 File Offset: 0x002141E0
	protected void UserConfirmDenyComplete(MVRPlugin mvrp, bool didConfirm)
	{
		this.insideRestore = true;
		this.SyncPluginUrlInternal(mvrp, true);
		this.insideRestore = false;
	}

	// Token: 0x06005AEB RID: 23275 RVA: 0x00215DF8 File Offset: 0x002141F8
	protected void SyncPluginUrl(MVRPlugin mvrp)
	{
		mvrp.SetupUserConfirmDeny(new MVRPlugin.UserConfirmDenyComplete(this.UserConfirmDenyComplete));
		this.SyncPluginUrlInternal(mvrp, false);
	}

	// Token: 0x06005AEC RID: 23276 RVA: 0x00215E14 File Offset: 0x00214214
	protected void SetPluginPanelColor(MVRPlugin mvrp, Color c)
	{
		if (mvrp.configUI != null)
		{
			Image component = mvrp.configUI.GetComponent<Image>();
			if (component != null)
			{
				component.color = c;
			}
		}
	}

	// Token: 0x06005AED RID: 23277 RVA: 0x00215E51 File Offset: 0x00214251
	protected void SetPluginPanelErrorColor(MVRPlugin mvrp)
	{
		this.SetPluginPanelColor(mvrp, this.pluginPanelErrorColor);
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x00215E60 File Offset: 0x00214260
	protected void SetPluginPanelWarningColor(MVRPlugin mvrp)
	{
		this.SetPluginPanelColor(mvrp, this.pluginPanelWarningColor);
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x00215E6F File Offset: 0x0021426F
	protected void SetPluginPanelValidColor(MVRPlugin mvrp)
	{
		this.SetPluginPanelColor(mvrp, this.pluginPanelValidColor);
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x00215E7E File Offset: 0x0021427E
	protected void AlertUserToNetworkRestriction(string url)
	{
		string alert = "Load of plugin\n\n" + url + "\n\nfailed due to this plugin requiring networking and 'Allow Plugins Network Access' User Preference being set to off.\n\nClick OK to open the User Preference panel if you want to change this setting to be on. You will need to reload the plugin and/or the scene after changing this setting.\n\nClick Cancel to continue without making changes to your User Preferences.";
		if (MVRPluginManager.<>f__am$cache0 == null)
		{
			MVRPluginManager.<>f__am$cache0 = new UnityAction(MVRPluginManager.<AlertUserToNetworkRestriction>m__0);
		}
		SuperController.AlertUser(alert, MVRPluginManager.<>f__am$cache0, null, SuperController.DisplayUIChoice.Auto);
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x00215EB4 File Offset: 0x002142B4
	protected void SyncPluginUrlInternal(MVRPlugin mvrp, bool isFromConfirmDenyResponse)
	{
        // 检查，是否，开启脚本
		if (UserPreferences.singleton == null || UserPreferences.singleton.enablePlugins)
		{
			if (this.domain == null)
			{
				this.domain = ScriptDomain.CreateDomain("MVRPlugins", true);
				IEnumerable<string> resolvedVersionDefines = SuperController.singleton.GetResolvedVersionDefines();
				if (resolvedVersionDefines != null)
				{
					foreach (string symbol in resolvedVersionDefines)
					{
						this.domain.CompilerService.AddConditionalSymbol(symbol);
					}
				}
			}
			this.RemovePluginScriptControllers(mvrp);
			string val = mvrp.pluginURLJSON.val;
			if (this.domain != null && this.pluginContainer != null && val != null && val != string.Empty)
			{
				if (FileManager.FileExists(val, false, false))
				{
					try
					{
						Location.Reset();
						string text = val.Replace("/", "_");
						text = text.Replace("\\", "_");
						text = text.Replace(".", "_");
						text = text.Replace(":", "_");
						if (val.EndsWith(".cslist") || val.EndsWith(".dll"))
						{
							this.SetPluginPanelValidColor(mvrp);
							ScriptAssembly scriptAssembly = null;
							if (val.EndsWith(".cslist"))
							{
								string directoryName = FileManager.GetDirectoryName(val, false);
								List<string> list = new List<string>();
								bool flag = false;
								HashSet<string> hashSet = new HashSet<string>();
								using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(val, true))
								{
									StreamReader streamReader = fileEntryStreamReader.StreamReader;
									string text2;
									while ((text2 = streamReader.ReadLine()) != null)
									{
										string text3 = text2.Trim();
										if (directoryName != string.Empty && !text3.Contains(":/"))
										{
											text3 = directoryName + "/" + text3;
										}
										if (text3 != string.Empty)
										{
											text3 = text3.Replace('/', '\\');
											if (FileManager.IsFileInPackage(text3))
											{
												VarFileEntry varFileEntry = FileManager.GetVarFileEntry(text3);
												if (varFileEntry != null)
												{
													flag = true;
													VarPackage package = varFileEntry.Package;
													if (mvrp.IsVarPackageConfirmed(package))
													{
														list.Add(text3);
													}
													else if (package.PluginsAlwaysDisabled)
													{
														this.SetPluginPanelWarningColor(mvrp);
														hashSet.Add(package.Uid);
													}
													else if (!isFromConfirmDenyResponse)
													{
														mvrp.AddRequestedPackage(package);
													}
													else
													{
														this.SetPluginPanelWarningColor(mvrp);
													}
												}
											}
											else
											{
												list.Add(text3);
											}
										}
									}
								}
								if (!isFromConfirmDenyResponse && mvrp.HasRequestedPackages)
								{
									mvrp.UserConfirm();
									return;
								}
								if (hashSet.Count > 0)
								{
									string[] array = new string[hashSet.Count];
									hashSet.CopyTo(array);
									string str = string.Join(" ", array);
									SuperController.LogMessage("Plugin scripts from packages " + str + " were not loaded because they are in packages that have always been denied plugin loading. Go to Package Manager, open the package, and click User Prefs tab to change", true, true);
								}
								if (list.Count <= 0)
								{
									return;
								}
								try
								{
									if (flag)
									{
										List<string> list2 = new List<string>();
										StringBuilder stringBuilder = new StringBuilder();
										foreach (string path in list)
										{
											string text4 = FileManager.ReadAllText(path, true);
											stringBuilder.Append(text4);
											list2.Add(text4);
										}
										string text5 = "MVRPlugin_" + text + "_" + this.GetMD5Hash(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
										FileManager.RegisterPluginHashToPluginPath(text5, val);
										this.domain.CompilerService.SetSuggestedAssemblyNamePrefix(text5);
										scriptAssembly = this.domain.CompileAndLoadScriptSources(list2.ToArray());
									}
									else
									{
										StringBuilder stringBuilder2 = new StringBuilder();
										foreach (string path2 in list)
										{
											string value = FileManager.ReadAllText(path2, true);
											stringBuilder2.Append(value);
										}
										string text6 = "MVRPlugin_" + text + "_" + this.GetMD5Hash(Encoding.ASCII.GetBytes(stringBuilder2.ToString()));
										FileManager.RegisterPluginHashToPluginPath(text6, val);
										this.domain.CompilerService.SetSuggestedAssemblyNamePrefix(text6);
										scriptAssembly = this.domain.CompileAndLoadScriptFiles(list.ToArray());
									}
									if (scriptAssembly == null)
									{
										SuperController.LogError("Compile of " + val + " failed. Errors:");
										foreach (string text7 in this.domain.CompilerService.Errors)
										{
											if (!text7.StartsWith("[CS]"))
											{
												SuperController.LogError(text7 + "\n");
											}
										}
										this.SetPluginPanelErrorColor(mvrp);
										return;
									}
								}
								catch (Exception ex)
								{
									if (ex.Message.Contains("RuntimeNamespaceRestriction"))
									{
										this.AlertUserToNetworkRestriction(val);
									}
									SuperController.LogError(string.Concat(new object[]
									{
										"Compile of ",
										val,
										" failed. Exception: ",
										ex
									}));
									if (this.domain.CompilerService.Errors.Length > 0)
									{
										SuperController.LogError("Compile of " + val + " failed. Errors:");
										foreach (string err in this.domain.CompilerService.Errors)
										{
											SuperController.LogError(err);
										}
									}
									this.SetPluginPanelErrorColor(mvrp);
									return;
								}
							}
							else if (FileManager.IsFileInPackage(val))
							{
								VarFileEntry varFileEntry2 = FileManager.GetVarFileEntry(val);
								if (varFileEntry2 != null)
								{
									VarPackage package2 = varFileEntry2.Package;
									if (mvrp.IsVarPackageConfirmed(package2))
									{
										byte[] array2 = FileManager.ReadAllBytes(val, true);
										string text8 = "MVRPlugin_" + text + "_" + this.GetMD5Hash(array2);
										FileManager.RegisterPluginHashToPluginPath(text8, val);
										this.domain.CompilerService.SetSuggestedAssemblyNamePrefix(text8);
										scriptAssembly = this.domain.LoadAssembly(array2, null);
									}
									else
									{
										if (package2.PluginsAlwaysDisabled)
										{
											this.SetPluginPanelWarningColor(mvrp);
											SuperController.LogMessage("Plugin " + val + " was not loaded because it is in a package that has always been denied plugin loading. Go to Package Manager, open the package, and click User Prefs tab to change", true, true);
											return;
										}
										if (!isFromConfirmDenyResponse)
										{
											mvrp.AddRequestedPackage(package2);
											mvrp.UserConfirm();
											return;
										}
										this.SetPluginPanelWarningColor(mvrp);
										return;
									}
								}
							}
							else
							{
								byte[] bytes = FileManager.ReadAllBytes(val, true);
								string text9 = "MVRPlugin_" + text + "_" + this.GetMD5Hash(bytes);
								FileManager.RegisterPluginHashToPluginPath(text9, val);
								this.domain.CompilerService.SetSuggestedAssemblyNamePrefix(text9);
								scriptAssembly = this.domain.LoadAssembly(val);
							}
							if (scriptAssembly != null)
							{
								ScriptType[] array3 = scriptAssembly.FindAllSubtypesOf<MVRScript>();
								if (array3.Length > 0)
								{
									foreach (ScriptType type in array3)
									{
										MVRScriptController mvrscriptController = this.CreateScriptController(mvrp, type);
										if (mvrscriptController != null)
										{
											mvrp.scriptControllers.Add(mvrscriptController);
										}
									}
								}
								else
								{
									Debug.LogError("No MVRScript types found");
									this.SetPluginPanelErrorColor(mvrp);
								}
							}
							else
							{
								SuperController.LogError("Unable to load assembly from " + val);
								this.SetPluginPanelErrorColor(mvrp);
							}
						}
						else
						{
							ScriptType scriptType = null;
							try
							{
								if (FileManager.IsFileInPackage(val))
								{
									VarFileEntry varFileEntry3 = FileManager.GetVarFileEntry(val);
									if (varFileEntry3 != null)
									{
										VarPackage package3 = varFileEntry3.Package;
										if (mvrp.IsVarPackageConfirmed(package3))
										{
											byte[] bytes2 = FileManager.ReadAllBytes(val, true);
											string text10 = "MVRPlugin_" + text + "_" + this.GetMD5Hash(bytes2);
											FileManager.RegisterPluginHashToPluginPath(text10, val);
											this.domain.CompilerService.SetSuggestedAssemblyNamePrefix(text10);
											scriptType = this.domain.CompileAndLoadScriptSource(FileManager.ReadAllText(val, false));
										}
										else
										{
											if (package3.PluginsAlwaysDisabled)
											{
												SuperController.LogMessage("Plugin " + val + " was not loaded because it is in a package that has always been denied plugin loading. Go to Package Manager, open the package, and click User Prefs tab to change", true, true);
												this.SetPluginPanelWarningColor(mvrp);
												return;
											}
											if (!isFromConfirmDenyResponse)
											{
												mvrp.AddRequestedPackage(package3);
												mvrp.UserConfirm();
												return;
											}
											this.SetPluginPanelWarningColor(mvrp);
											return;
										}
									}
								}
								else
								{
									byte[] bytes3 = FileManager.ReadAllBytes(val, true);
									string text11 = "MVRPlugin_" + text + "_" + this.GetMD5Hash(bytes3);
									FileManager.RegisterPluginHashToPluginPath(text11, val);
									this.domain.CompilerService.SetSuggestedAssemblyNamePrefix(text11);
									scriptType = this.domain.CompileAndLoadScriptFile(val);
								}
								if (scriptType == null)
								{
									SuperController.LogError("Compile of " + val + " failed. Errors:");
									foreach (string err2 in this.domain.CompilerService.Errors)
									{
										SuperController.LogError(err2);
									}
									this.SetPluginPanelErrorColor(mvrp);
									return;
								}
							}
							catch (Exception ex2)
							{
								if (ex2.Message.Contains("RuntimeNamespaceRestriction"))
								{
									this.AlertUserToNetworkRestriction(val);
								}
								SuperController.LogError(string.Concat(new object[]
								{
									"Compile of ",
									val,
									" failed. Exception: ",
									ex2
								}));
								if (this.domain.CompilerService.Errors.Length > 0)
								{
									SuperController.LogError("Compile of " + val + " failed. Errors:");
									foreach (string err3 in this.domain.CompilerService.Errors)
									{
										SuperController.LogError(err3);
									}
								}
								this.SetPluginPanelErrorColor(mvrp);
								return;
							}
							if (scriptType.IsSubtypeOf<MVRScript>())
							{
								MVRScriptController mvrscriptController2 = this.CreateScriptController(mvrp, scriptType);
								if (mvrscriptController2 != null)
								{
									mvrp.scriptControllers.Add(mvrscriptController2);
								}
								this.SetPluginPanelValidColor(mvrp);
							}
							else
							{
								this.SetPluginPanelErrorColor(mvrp);
								SuperController.LogError("Script loaded at " + val + " must inherit from MVRScript");
							}
						}
					}
					catch (Exception ex3)
					{
						this.SetPluginPanelErrorColor(mvrp);
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception during compile of ",
							val,
							": ",
							ex3
						}));
					}
				}
				else
				{
					this.SetPluginPanelErrorColor(mvrp);
					SuperController.LogError("Plugin file " + val + " does not exist");
				}
			}
		}
		else
		{
			SuperController.LogError("Attempted to load plugin when plugins option is disabled. To enable, see User Preferences -> Security tab");
			SuperController.singleton.ShowMainHUDAuto();
			SuperController.singleton.SetActiveUI("MainMenu");
			SuperController.singleton.SetMainMenuTab("TabUserPrefs");
			SuperController.singleton.SetUserPrefsTab("TabSecurity");
		}
	}

	// Token: 0x06005AF2 RID: 23282 RVA: 0x00216A44 File Offset: 0x00214E44
	public string GetMD5Hash(byte[] bytes)
	{
		if (this.md5 == null)
		{
			this.md5 = new MD5CryptoServiceProvider();
		}
		byte[] array = this.md5.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x00216AAC File Offset: 0x00214EAC
	protected void Init()
	{
		if (this.pluginContainer == null)
		{
			this.pluginContainer = base.transform;
		}
		this.plugins = new List<MVRPlugin>();
		this.pluginUIDs = new Dictionary<string, bool>();
		this.CreatePluginAction = new JSONStorableAction("CreatePlugin", new JSONStorableAction.ActionCallback(this.CreatePluginCallback));
		base.RegisterAction(this.CreatePluginAction);
	}

	// Token: 0x06005AF4 RID: 23284 RVA: 0x00216B14 File Offset: 0x00214F14
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			MVRPluginManagerUI componentInChildren = this.UITransform.GetComponentInChildren<MVRPluginManagerUI>();
			if (componentInChildren != null)
			{
				this.scriptUIParent = componentInChildren.scriptUIParent;
				this.pluginListPanel = componentInChildren.pluginListPanel;
				if (this.pluginListPanel != null)
				{
					foreach (MVRPlugin mvrplugin in this.plugins)
					{
						if (mvrplugin.configUI != null)
						{
							mvrplugin.configUI.SetParent(this.pluginListPanel, false);
							mvrplugin.configUI.gameObject.SetActive(true);
						}
						foreach (MVRScriptController mvrscriptController in mvrplugin.scriptControllers)
						{
							if (mvrscriptController.customUI != null && this.scriptUIParent != null)
							{
								mvrscriptController.customUI.SetParent(this.scriptUIParent, false);
							}
						}
					}
				}
				this.CreatePluginAction.button = componentInChildren.addPluginButton;
			}
		}
	}

	// Token: 0x06005AF5 RID: 23285 RVA: 0x00216C7C File Offset: 0x0021507C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
		}
	}

	// Token: 0x06005AF6 RID: 23286 RVA: 0x00216C8F File Offset: 0x0021508F
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x06005AF7 RID: 23287 RVA: 0x00216CB4 File Offset: 0x002150B4
	protected void OnDestroy()
	{
		foreach (MVRPlugin mvrplugin in this.plugins)
		{
			foreach (MVRScriptController mvrscriptController in mvrplugin.scriptControllers)
			{
				if (mvrscriptController.customUI != null)
				{
					UnityEngine.Object.Destroy(mvrscriptController.customUI.gameObject);
				}
			}
			if (mvrplugin.configUI != null)
			{
				UnityEngine.Object.Destroy(mvrplugin.configUI.gameObject);
			}
		}
	}

	// Token: 0x06005AF8 RID: 23288 RVA: 0x00216D90 File Offset: 0x00215190
	[CompilerGenerated]
	private static void <AlertUserToNetworkRestriction>m__0()
	{
		SuperController.singleton.DeactivateWorldUI();
		SuperController.singleton.activeUI = SuperController.ActiveUI.MainMenu;
		SuperController.singleton.SetMainMenuTab("TabUserPrefs");
		SuperController.singleton.SetUserPrefsTab("TabSecurity");
	}

	// Token: 0x04004AFD RID: 19197
	public Transform pluginPanelPrefab;

	// Token: 0x04004AFE RID: 19198
	public Transform scriptControllerPanelPrefab;

	// Token: 0x04004AFF RID: 19199
	public Transform scriptUIPrefab;

	// Token: 0x04004B00 RID: 19200
	public Transform configurableSliderPrefab;

	// Token: 0x04004B01 RID: 19201
	public Transform configurableTogglePrefab;

	// Token: 0x04004B02 RID: 19202
	public Transform configurableColorPickerPrefab;

	// Token: 0x04004B03 RID: 19203
	public Transform configurableButtonPrefab;

	// Token: 0x04004B04 RID: 19204
	public Transform configurablePopupPrefab;

	// Token: 0x04004B05 RID: 19205
	public Transform configurableScrollablePopupPrefab;

	// Token: 0x04004B06 RID: 19206
	public Transform configurableFilterablePopupPrefab;

	// Token: 0x04004B07 RID: 19207
	public Transform configurableTextFieldPrefab;

	// Token: 0x04004B08 RID: 19208
	public Transform configurableSpacerPrefab;

	// Token: 0x04004B09 RID: 19209
	public Transform pluginListPanel;

	// Token: 0x04004B0A RID: 19210
	public RectTransform scriptUIParent;

	// Token: 0x04004B0B RID: 19211
	public Transform pluginContainer;

	// Token: 0x04004B0C RID: 19212
	protected List<MVRPlugin> plugins;

	// Token: 0x04004B0D RID: 19213
	protected Dictionary<string, bool> pluginUIDs;

	// Token: 0x04004B0E RID: 19214
	protected JSONStorableAction CreatePluginAction;

	// Token: 0x04004B0F RID: 19215
	protected Color pluginPanelErrorColor = new Color(1f, 0.5f, 0.5f);

	// Token: 0x04004B10 RID: 19216
	protected Color pluginPanelWarningColor = new Color(1f, 0.95f, 0.5f);

	// Token: 0x04004B11 RID: 19217
	protected Color pluginPanelValidColor = Color.gray;

	// Token: 0x04004B12 RID: 19218
	protected ScriptDomain domain;

	// Token: 0x04004B13 RID: 19219
	protected MD5CryptoServiceProvider md5;

	// Token: 0x04004B14 RID: 19220
	[CompilerGenerated]
	private static UnityAction <>f__am$cache0;

	// Token: 0x02001005 RID: 4101
	[CompilerGenerated]
	private sealed class <CreatePluginWithId>c__AnonStorey0
	{
		// Token: 0x06007685 RID: 30341 RVA: 0x00216DC5 File Offset: 0x002151C5
		public <CreatePluginWithId>c__AnonStorey0()
		{
		}

		// Token: 0x06007686 RID: 30342 RVA: 0x00216DCD File Offset: 0x002151CD
		internal void <>m__0(string s)
		{
			this.$this.SyncPluginUrl(this.mvrp);
		}

		// Token: 0x06007687 RID: 30343 RVA: 0x00216DE0 File Offset: 0x002151E0
		internal void <>m__1()
		{
			this.$this.RemovePlugin(this.mvrp);
		}

		// Token: 0x04006A52 RID: 27218
		internal MVRPlugin mvrp;

		// Token: 0x04006A53 RID: 27219
		internal MVRPluginManager $this;
	}
}
