using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CEC RID: 3308
public class JSONStorableUrl : JSONStorableString
{
	// Token: 0x06006445 RID: 25669 RVA: 0x00260AEC File Offset: 0x0025EEEC
	public JSONStorableUrl(string paramName, string startingValue) : base(paramName, startingValue)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = null;
		this.suggestedPath = null;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x06006446 RID: 25670 RVA: 0x00260B27 File Offset: 0x0025EF27
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetStringCallback callback) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = null;
		this.suggestedPath = null;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x06006447 RID: 25671 RVA: 0x00260B63 File Offset: 0x0025EF63
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetJSONStringCallback callback) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = null;
		this.suggestedPath = null;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x06006448 RID: 25672 RVA: 0x00260B9F File Offset: 0x0025EF9F
	public JSONStorableUrl(string paramName, string startingValue, string filter) : base(paramName, startingValue)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = null;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x06006449 RID: 25673 RVA: 0x00260BDA File Offset: 0x0025EFDA
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetStringCallback callback, string filter) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = null;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x0600644A RID: 25674 RVA: 0x00260C17 File Offset: 0x0025F017
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetJSONStringCallback callback, string filter) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = null;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x0600644B RID: 25675 RVA: 0x00260C54 File Offset: 0x0025F054
	public JSONStorableUrl(string paramName, string startingValue, string filter, string suggestPath) : base(paramName, startingValue)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = suggestPath;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x0600644C RID: 25676 RVA: 0x00260C90 File Offset: 0x0025F090
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetStringCallback callback, string filter, string suggestPath) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = suggestPath;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x0600644D RID: 25677 RVA: 0x00260CCE File Offset: 0x0025F0CE
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetJSONStringCallback callback, string filter, string suggestPath) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = suggestPath;
		this._forceCallbackOnSet = false;
	}

	// Token: 0x0600644E RID: 25678 RVA: 0x00260D0C File Offset: 0x0025F10C
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetStringCallback callback, string filter, bool forceCallbackOnSet) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = null;
		this._forceCallbackOnSet = forceCallbackOnSet;
	}

	// Token: 0x0600644F RID: 25679 RVA: 0x00260D4A File Offset: 0x0025F14A
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetJSONStringCallback callback, string filter, bool forceCallbackOnSet) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = null;
		this._forceCallbackOnSet = forceCallbackOnSet;
	}

	// Token: 0x06006450 RID: 25680 RVA: 0x00260D88 File Offset: 0x0025F188
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetStringCallback callback, string filter, string suggestPath, bool forceCallbackOnSet) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = suggestPath;
		this._forceCallbackOnSet = forceCallbackOnSet;
	}

	// Token: 0x06006451 RID: 25681 RVA: 0x00260DC7 File Offset: 0x0025F1C7
	public JSONStorableUrl(string paramName, string startingValue, JSONStorableString.SetJSONStringCallback callback, string filter, string suggestPath, bool forceCallbackOnSet) : base(paramName, startingValue, callback)
	{
		this.type = JSONStorable.Type.Url;
		this._filter = filter;
		this.suggestedPath = suggestPath;
		this._forceCallbackOnSet = forceCallbackOnSet;
	}

	// Token: 0x06006452 RID: 25682 RVA: 0x00260E06 File Offset: 0x0025F206
	protected void ResetActiveSuggestedPath()
	{
		this._activeSuggestedPath = this._normalizedSuggestedPath;
		if (this._activeSuggestedPath != null && this.allowBrowseAboveSuggestedPath)
		{
			this._activeSuggestedPath = this._activeSuggestedPath.Replace('/', '\\');
		}
	}

	// Token: 0x06006453 RID: 25683 RVA: 0x00260E3F File Offset: 0x0025F23F
	public static void SetSuggestedPathGroupPath(string group, string path)
	{
		if (JSONStorableUrl.suggestedPathGroups == null)
		{
			JSONStorableUrl.suggestedPathGroups = new Dictionary<string, string>();
		}
		if (group != null && path != null)
		{
			JSONStorableUrl.suggestedPathGroups.Remove(group);
			JSONStorableUrl.suggestedPathGroups.Add(group, path);
		}
	}

	// Token: 0x17000EC5 RID: 3781
	// (get) Token: 0x06006454 RID: 25684 RVA: 0x00260E79 File Offset: 0x0025F279
	// (set) Token: 0x06006455 RID: 25685 RVA: 0x00260E84 File Offset: 0x0025F284
	public string suggestedPathGroup
	{
		get
		{
			return this._suggestedPathGroup;
		}
		set
		{
			if (this._suggestedPathGroup != value)
			{
				this._suggestedPathGroup = value;
				if (JSONStorableUrl.suggestedPathGroups == null)
				{
					JSONStorableUrl.suggestedPathGroups = new Dictionary<string, string>();
				}
				if (!JSONStorableUrl.suggestedPathGroups.ContainsKey(this._suggestedPathGroup))
				{
					JSONStorableUrl.suggestedPathGroups.Add(this._suggestedPathGroup, this._suggestedPath);
				}
			}
		}
	}

	// Token: 0x17000EC6 RID: 3782
	// (get) Token: 0x06006456 RID: 25686 RVA: 0x00260EE8 File Offset: 0x0025F2E8
	// (set) Token: 0x06006457 RID: 25687 RVA: 0x00260EF0 File Offset: 0x0025F2F0
	public string suggestedPath
	{
		get
		{
			return this._suggestedPath;
		}
		set
		{
			if (this._suggestedPath != value)
			{
				this._suggestedPath = value;
				if (this._suggestedPath == null || this._suggestedPath == string.Empty)
				{
					this._normalizedSuggestedPath = null;
					this._activeSuggestedPath = null;
				}
				else
				{
					this._normalizedSuggestedPath = this._suggestedPath.Replace('\\', '/');
					this._normalizedSuggestedPath = Regex.Replace(this._normalizedSuggestedPath, "/$", string.Empty);
					this.ResetActiveSuggestedPath();
				}
			}
		}
	}

	// Token: 0x17000EC7 RID: 3783
	// (get) Token: 0x06006458 RID: 25688 RVA: 0x00260F7E File Offset: 0x0025F37E
	// (set) Token: 0x06006459 RID: 25689 RVA: 0x00260F86 File Offset: 0x0025F386
	public bool allowBrowseAboveSuggestedPath
	{
		get
		{
			return this._allowBrowseAboveSuggestPath;
		}
		set
		{
			if (this._allowBrowseAboveSuggestPath != value)
			{
				this._allowBrowseAboveSuggestPath = value;
				this.ResetActiveSuggestedPath();
			}
		}
	}

	// Token: 0x17000EC8 RID: 3784
	// (get) Token: 0x0600645A RID: 25690 RVA: 0x00260FA1 File Offset: 0x0025F3A1
	public bool valueSetFromBrowse
	{
		get
		{
			return this._valueSetFromBrowse;
		}
	}

	// Token: 0x17000EC9 RID: 3785
	// (get) Token: 0x0600645B RID: 25691 RVA: 0x00260FA9 File Offset: 0x0025F3A9
	// (set) Token: 0x0600645C RID: 25692 RVA: 0x00260FB4 File Offset: 0x0025F3B4
	public override string val
	{
		get
		{
			return this._val;
		}
		set
		{
			string text;
			if (value != null)
			{
				text = value.TrimEnd(new char[0]).TrimStart(new char[0]);
			}
			else
			{
				text = value;
			}
			if (this._val != text)
			{
				base.val = text;
			}
		}
	}

	// Token: 0x0600645D RID: 25693 RVA: 0x00261000 File Offset: 0x0025F400
	public override bool StoreJSON(JSONClass jc, bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		bool flag = this.NeedsStore(jc, includePhysical, includeAppearance) && (forceStore || this._val != this.defaultVal);
		if (flag)
		{
			if (Regex.IsMatch(this.val, "^http"))
			{
				jc[this.name] = this.val;
			}
			else if (SuperController.singleton != null)
			{
				jc[this.name] = SuperController.singleton.NormalizeSavePath(this.val);
			}
			else
			{
				jc[this.name] = this.val;
			}
		}
		return flag;
	}

	// Token: 0x0600645E RID: 25694 RVA: 0x002610C0 File Offset: 0x0025F4C0
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				string text = jc[this.name];
				if (SuperController.singleton != null)
				{
					text = SuperController.singleton.NormalizeLoadPath(text);
				}
				this.val = text;
			}
			else if (setMissingToDefault)
			{
				this.val = this.defaultVal;
			}
		}
	}

	// Token: 0x0600645F RID: 25695 RVA: 0x00261144 File Offset: 0x0025F544
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsLateRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				string text = jc[this.name];
				if (SuperController.singleton != null)
				{
					text = SuperController.singleton.NormalizeLoadPath(text);
				}
				this.val = text;
			}
			else if (setMissingToDefault)
			{
				this.val = this.defaultVal;
			}
		}
	}

	// Token: 0x06006460 RID: 25696 RVA: 0x002611C5 File Offset: 0x0025F5C5
	protected void ReplaceSuggestedPathGroupPathWithActivePath()
	{
		if (this.suggestedPathGroup != null)
		{
			JSONStorableUrl.suggestedPathGroups.Remove(this.suggestedPathGroup);
			JSONStorableUrl.suggestedPathGroups.Add(this.suggestedPathGroup, this._activeSuggestedPath);
		}
	}

	// Token: 0x06006461 RID: 25697 RVA: 0x002611FC File Offset: 0x0025F5FC
	public void SetFilePath(string path)
	{
		if (path != null && path != string.Empty)
		{
			path = SuperController.singleton.NormalizeMediaPath(path);
			this._activeSuggestedPath = FileManager.GetDirectoryName(path, true);
			if (this.allowBrowseAboveSuggestedPath || this._normalizedSuggestedPath == null)
			{
				this._activeSuggestedPath = this._activeSuggestedPath.Replace('/', '\\');
				this.ReplaceSuggestedPathGroupPathWithActivePath();
			}
			else
			{
				string suggestedBrowserDirectoryFromDirectoryPath = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(this._normalizedSuggestedPath, this._activeSuggestedPath, true);
				if (suggestedBrowserDirectoryFromDirectoryPath != null)
				{
					this._activeSuggestedPath = suggestedBrowserDirectoryFromDirectoryPath;
					this.ReplaceSuggestedPathGroupPathWithActivePath();
				}
			}
			if (this._val != path)
			{
				this.val = path;
			}
			else if (this._forceCallbackOnSet)
			{
				if (this.setCallbackFunction != null)
				{
					this.setCallbackFunction(this._val);
				}
				if (this.setJSONCallbackFunction != null)
				{
					this.setJSONCallbackFunction(this);
				}
			}
		}
	}

	// Token: 0x06006462 RID: 25698 RVA: 0x002612F0 File Offset: 0x0025F6F0
	protected void SetFilePathBrowseCallback(string path, bool didClose)
	{
		this._valueSetFromBrowse = true;
		this.SetFilePath(path);
		this._valueSetFromBrowse = false;
		if (didClose)
		{
			if (this.endBrowseCallback != null)
			{
				this.endBrowseCallback();
			}
			if (this.endBrowseWithObjectCallback != null)
			{
				this.endBrowseWithObjectCallback(this);
			}
		}
	}

	// Token: 0x06006463 RID: 25699 RVA: 0x00261348 File Offset: 0x0025F748
	public void FileBrowse()
	{
		if (this.beginBrowseCallback != null)
		{
			this.beginBrowseCallback();
		}
		if (this.beginBrowseWithObjectCallback != null)
		{
			this.beginBrowseWithObjectCallback(this);
		}
		if (SuperController.singleton != null)
		{
			if (this.suggestedPathGroup != null)
			{
				JSONStorableUrl.suggestedPathGroups.TryGetValue(this.suggestedPathGroup, out this._activeSuggestedPath);
			}
			if (this._activeSuggestedPath == null || this._activeSuggestedPath == string.Empty)
			{
				this._activeSuggestedPath = this._suggestedPath;
				this.ReplaceSuggestedPathGroupPathWithActivePath();
			}
			else if (!FileManager.DirectoryExists(this._activeSuggestedPath, false, false))
			{
				this._activeSuggestedPath = this._suggestedPath;
				this.ReplaceSuggestedPathGroupPathWithActivePath();
			}
			SuperController.singleton.GetMediaPathDialog(new FileBrowserFullCallback(this.SetFilePathBrowseCallback), this._filter, this._activeSuggestedPath, this.allowFullComputerBrowse, this.showDirs, false, this.fileRemovePrefix, this.hideExtension, this.shortCuts, true, this.allowBrowseAboveSuggestedPath);
		}
	}

	// Token: 0x06006464 RID: 25700 RVA: 0x00261457 File Offset: 0x0025F857
	public void RegisterFileBrowseButton(Button b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.fileBrowseButtonAlt = b;
		}
		else
		{
			this.fileBrowseButton = b;
		}
	}

	// Token: 0x17000ECA RID: 3786
	// (get) Token: 0x06006465 RID: 25701 RVA: 0x00261472 File Offset: 0x0025F872
	// (set) Token: 0x06006466 RID: 25702 RVA: 0x0026147C File Offset: 0x0025F87C
	public Button fileBrowseButton
	{
		get
		{
			return this._fileBrowseButton;
		}
		set
		{
			if (this._fileBrowseButton != value)
			{
				if (this._fileBrowseButton != null)
				{
					this._fileBrowseButton.onClick.RemoveListener(new UnityAction(this.FileBrowse));
				}
				this._fileBrowseButton = value;
				if (this._fileBrowseButton != null)
				{
					this._fileBrowseButton.onClick.AddListener(new UnityAction(this.FileBrowse));
				}
			}
		}
	}

	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x06006467 RID: 25703 RVA: 0x002614FB File Offset: 0x0025F8FB
	// (set) Token: 0x06006468 RID: 25704 RVA: 0x00261504 File Offset: 0x0025F904
	public Button fileBrowseButtonAlt
	{
		get
		{
			return this._fileBrowseButtonAlt;
		}
		set
		{
			if (this._fileBrowseButtonAlt != value)
			{
				if (this._fileBrowseButtonAlt != null)
				{
					this._fileBrowseButtonAlt.onClick.RemoveListener(new UnityAction(this.FileBrowse));
				}
				this._fileBrowseButtonAlt = value;
				if (this._fileBrowseButtonAlt != null)
				{
					this._fileBrowseButtonAlt.onClick.AddListener(new UnityAction(this.FileBrowse));
				}
			}
		}
	}

	// Token: 0x06006469 RID: 25705 RVA: 0x00261583 File Offset: 0x0025F983
	public void CopyToClipboard()
	{
		GUIUtility.systemCopyBuffer = this._val;
	}

	// Token: 0x0600646A RID: 25706 RVA: 0x00261590 File Offset: 0x0025F990
	public void RegisterCopyToClipboardButton(Button b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.copyToClipboardButtonAlt = b;
		}
		else
		{
			this.copyToClipboardButton = b;
		}
	}

	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x0600646B RID: 25707 RVA: 0x002615AB File Offset: 0x0025F9AB
	// (set) Token: 0x0600646C RID: 25708 RVA: 0x002615B4 File Offset: 0x0025F9B4
	public Button copyToClipboardButton
	{
		get
		{
			return this._copyToClipboardButton;
		}
		set
		{
			if (this._copyToClipboardButton != value)
			{
				if (this._copyToClipboardButton != null)
				{
					this._copyToClipboardButton.onClick.RemoveListener(new UnityAction(this.CopyToClipboard));
				}
				this._copyToClipboardButton = value;
				if (this._copyToClipboardButton != null)
				{
					this._copyToClipboardButton.onClick.AddListener(new UnityAction(this.CopyToClipboard));
				}
			}
		}
	}

	// Token: 0x17000ECD RID: 3789
	// (get) Token: 0x0600646D RID: 25709 RVA: 0x00261633 File Offset: 0x0025FA33
	// (set) Token: 0x0600646E RID: 25710 RVA: 0x0026163C File Offset: 0x0025FA3C
	public Button copyToClipboardButtonAlt
	{
		get
		{
			return this._copyToClipboardButtonAlt;
		}
		set
		{
			if (this._copyToClipboardButtonAlt != value)
			{
				if (this._copyToClipboardButtonAlt != null)
				{
					this._copyToClipboardButtonAlt.onClick.RemoveListener(new UnityAction(this.CopyToClipboard));
				}
				this._copyToClipboardButtonAlt = value;
				if (this._copyToClipboardButtonAlt != null)
				{
					this._copyToClipboardButtonAlt.onClick.AddListener(new UnityAction(this.CopyToClipboard));
				}
			}
		}
	}

	// Token: 0x0600646F RID: 25711 RVA: 0x002616BB File Offset: 0x0025FABB
	public void CopyFromClipboard()
	{
		this.val = GUIUtility.systemCopyBuffer;
	}

	// Token: 0x06006470 RID: 25712 RVA: 0x002616C8 File Offset: 0x0025FAC8
	public void RegisterCopyFromClipboardButton(Button b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.copyFromClipboardButtonAlt = b;
		}
		else
		{
			this.copyFromClipboardButton = b;
		}
	}

	// Token: 0x17000ECE RID: 3790
	// (get) Token: 0x06006471 RID: 25713 RVA: 0x002616E3 File Offset: 0x0025FAE3
	// (set) Token: 0x06006472 RID: 25714 RVA: 0x002616EC File Offset: 0x0025FAEC
	public Button copyFromClipboardButton
	{
		get
		{
			return this._copyFromClipboardButton;
		}
		set
		{
			if (this._copyFromClipboardButton != value)
			{
				if (this._copyFromClipboardButton != null)
				{
					this._copyFromClipboardButton.onClick.RemoveListener(new UnityAction(this.CopyFromClipboard));
				}
				this._copyFromClipboardButton = value;
				if (this._copyFromClipboardButton != null)
				{
					this._copyFromClipboardButton.onClick.AddListener(new UnityAction(this.CopyFromClipboard));
				}
			}
		}
	}

	// Token: 0x17000ECF RID: 3791
	// (get) Token: 0x06006473 RID: 25715 RVA: 0x0026176B File Offset: 0x0025FB6B
	// (set) Token: 0x06006474 RID: 25716 RVA: 0x00261774 File Offset: 0x0025FB74
	public Button copyFromClipboardButtonAlt
	{
		get
		{
			return this._copyFromClipboardButtonAlt;
		}
		set
		{
			if (this._copyFromClipboardButtonAlt != value)
			{
				if (this._copyFromClipboardButtonAlt != null)
				{
					this._copyFromClipboardButtonAlt.onClick.RemoveListener(new UnityAction(this.CopyFromClipboard));
				}
				this._copyFromClipboardButtonAlt = value;
				if (this._copyFromClipboardButtonAlt != null)
				{
					this._copyFromClipboardButtonAlt.onClick.AddListener(new UnityAction(this.CopyFromClipboard));
				}
			}
		}
	}

	// Token: 0x06006475 RID: 25717 RVA: 0x002617F4 File Offset: 0x0025FBF4
	public void Clear()
	{
		if (this._val != string.Empty)
		{
			this.val = string.Empty;
		}
		else if (this._forceCallbackOnSet)
		{
			if (this.setCallbackFunction != null)
			{
				this.setCallbackFunction(string.Empty);
			}
			if (this.setJSONCallbackFunction != null)
			{
				this.setJSONCallbackFunction(this);
			}
		}
	}

	// Token: 0x06006476 RID: 25718 RVA: 0x00261863 File Offset: 0x0025FC63
	public void RegisterClearButton(Button b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.clearButtonAlt = b;
		}
		else
		{
			this.clearButton = b;
		}
	}

	// Token: 0x17000ED0 RID: 3792
	// (get) Token: 0x06006477 RID: 25719 RVA: 0x0026187E File Offset: 0x0025FC7E
	// (set) Token: 0x06006478 RID: 25720 RVA: 0x00261888 File Offset: 0x0025FC88
	public Button clearButton
	{
		get
		{
			return this._clearButton;
		}
		set
		{
			if (this._clearButton != value)
			{
				if (this._clearButton != null)
				{
					this._clearButton.onClick.RemoveListener(new UnityAction(this.Clear));
				}
				this._clearButton = value;
				if (this._clearButton != null)
				{
					this._clearButton.onClick.AddListener(new UnityAction(this.Clear));
				}
			}
		}
	}

	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x06006479 RID: 25721 RVA: 0x00261907 File Offset: 0x0025FD07
	// (set) Token: 0x0600647A RID: 25722 RVA: 0x00261910 File Offset: 0x0025FD10
	public Button clearButtonAlt
	{
		get
		{
			return this._clearButtonAlt;
		}
		set
		{
			if (this._clearButtonAlt != value)
			{
				if (this._clearButtonAlt != null)
				{
					this._clearButtonAlt.onClick.RemoveListener(new UnityAction(this.Clear));
				}
				this._clearButtonAlt = value;
				if (this._clearButtonAlt != null)
				{
					this._clearButtonAlt.onClick.AddListener(new UnityAction(this.Clear));
				}
			}
		}
	}

	// Token: 0x0600647B RID: 25723 RVA: 0x00261990 File Offset: 0x0025FD90
	public void Reload()
	{
		this._valueSetFromBrowse = true;
		if (this.setCallbackFunction != null)
		{
			this.setCallbackFunction(this._val);
		}
		if (this.setJSONCallbackFunction != null)
		{
			this.setJSONCallbackFunction(this);
		}
		this._valueSetFromBrowse = false;
	}

	// Token: 0x0600647C RID: 25724 RVA: 0x002619DE File Offset: 0x0025FDDE
	public void RegisterReloadButton(Button b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.reloadButtonAlt = b;
		}
		else
		{
			this.reloadButton = b;
		}
	}

	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x0600647D RID: 25725 RVA: 0x002619F9 File Offset: 0x0025FDF9
	// (set) Token: 0x0600647E RID: 25726 RVA: 0x00261A04 File Offset: 0x0025FE04
	public Button reloadButton
	{
		get
		{
			return this._reloadButton;
		}
		set
		{
			if (this._reloadButton != value)
			{
				if (this._reloadButton != null)
				{
					this._reloadButton.onClick.RemoveListener(new UnityAction(this.Reload));
				}
				this._reloadButton = value;
				if (this._reloadButton != null)
				{
					this._reloadButton.onClick.AddListener(new UnityAction(this.Reload));
				}
			}
		}
	}

	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x0600647F RID: 25727 RVA: 0x00261A83 File Offset: 0x0025FE83
	// (set) Token: 0x06006480 RID: 25728 RVA: 0x00261A8C File Offset: 0x0025FE8C
	public Button reloadButtonAlt
	{
		get
		{
			return this._reloadButtonAlt;
		}
		set
		{
			if (this._reloadButtonAlt != value)
			{
				if (this._reloadButtonAlt != null)
				{
					this._reloadButtonAlt.onClick.RemoveListener(new UnityAction(this.Reload));
				}
				this._reloadButtonAlt = value;
				if (this._reloadButtonAlt != null)
				{
					this._reloadButtonAlt.onClick.AddListener(new UnityAction(this.Reload));
				}
			}
		}
	}

	// Token: 0x0400542D RID: 21549
	protected string _filter;

	// Token: 0x0400542E RID: 21550
	protected string _activeSuggestedPath;

	// Token: 0x0400542F RID: 21551
	protected static Dictionary<string, string> suggestedPathGroups;

	// Token: 0x04005430 RID: 21552
	protected string _suggestedPathGroup;

	// Token: 0x04005431 RID: 21553
	protected string _suggestedPath;

	// Token: 0x04005432 RID: 21554
	protected string _normalizedSuggestedPath;

	// Token: 0x04005433 RID: 21555
	protected bool _allowBrowseAboveSuggestPath = true;

	// Token: 0x04005434 RID: 21556
	public bool allowFullComputerBrowse = true;

	// Token: 0x04005435 RID: 21557
	public bool showDirs = true;

	// Token: 0x04005436 RID: 21558
	public List<ShortCut> shortCuts;

	// Token: 0x04005437 RID: 21559
	protected bool _forceCallbackOnSet;

	// Token: 0x04005438 RID: 21560
	public bool hideExtension;

	// Token: 0x04005439 RID: 21561
	public string fileRemovePrefix;

	// Token: 0x0400543A RID: 21562
	protected bool _valueSetFromBrowse;

	// Token: 0x0400543B RID: 21563
	public JSONStorableUrl.BeginBrowseCallback beginBrowseCallback;

	// Token: 0x0400543C RID: 21564
	public JSONStorableUrl.EndBrowseCallback endBrowseCallback;

	// Token: 0x0400543D RID: 21565
	public JSONStorableUrl.BeginBrowseWithObjectCallback beginBrowseWithObjectCallback;

	// Token: 0x0400543E RID: 21566
	public JSONStorableUrl.EndBrowseWithObjectCallback endBrowseWithObjectCallback;

	// Token: 0x0400543F RID: 21567
	protected Button _fileBrowseButton;

	// Token: 0x04005440 RID: 21568
	protected Button _fileBrowseButtonAlt;

	// Token: 0x04005441 RID: 21569
	protected Button _copyToClipboardButton;

	// Token: 0x04005442 RID: 21570
	protected Button _copyToClipboardButtonAlt;

	// Token: 0x04005443 RID: 21571
	protected Button _copyFromClipboardButton;

	// Token: 0x04005444 RID: 21572
	protected Button _copyFromClipboardButtonAlt;

	// Token: 0x04005445 RID: 21573
	protected Button _clearButton;

	// Token: 0x04005446 RID: 21574
	protected Button _clearButtonAlt;

	// Token: 0x04005447 RID: 21575
	protected Button _reloadButton;

	// Token: 0x04005448 RID: 21576
	protected Button _reloadButtonAlt;

	// Token: 0x02000CED RID: 3309
	// (Invoke) Token: 0x06006482 RID: 25730
	public delegate void BeginBrowseCallback();

	// Token: 0x02000CEE RID: 3310
	// (Invoke) Token: 0x06006486 RID: 25734
	public delegate void EndBrowseCallback();

	// Token: 0x02000CEF RID: 3311
	// (Invoke) Token: 0x0600648A RID: 25738
	public delegate void BeginBrowseWithObjectCallback(JSONStorableUrl jsurl);

	// Token: 0x02000CF0 RID: 3312
	// (Invoke) Token: 0x0600648E RID: 25742
	public delegate void EndBrowseWithObjectCallback(JSONStorableUrl jsurl);
}
