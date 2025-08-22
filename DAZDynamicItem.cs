using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MeshVR;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AC7 RID: 2759
[ExecuteInEditMode]
public class DAZDynamicItem : JSONStorableDynamic
{
	// Token: 0x0600494B RID: 18763 RVA: 0x001711A4 File Offset: 0x0016F5A4
	public DAZDynamicItem()
	{
	}

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x0600494C RID: 18764 RVA: 0x00171219 File Offset: 0x0016F619
	// (set) Token: 0x0600494D RID: 18765 RVA: 0x00171248 File Offset: 0x0016F648
	public string uid
	{
		get
		{
			if (this._uid == null || this._uid == string.Empty)
			{
				return this.displayName;
			}
			return this._uid;
		}
		set
		{
			if (this._uid != value)
			{
				this._uid = value;
			}
		}
	}

	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x0600494E RID: 18766 RVA: 0x00171262 File Offset: 0x0016F662
	public bool IsInPackage
	{
		get
		{
			return this.version != null && this.version != string.Empty;
		}
	}

	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x0600494F RID: 18767 RVA: 0x00171287 File Offset: 0x0016F687
	// (set) Token: 0x06004950 RID: 18768 RVA: 0x0017128F File Offset: 0x0016F68F
	public string[] tagsArray
	{
		get
		{
			return this._tagsArray;
		}
		protected set
		{
			this._tagsArray = value;
		}
	}

	// Token: 0x06004951 RID: 18769 RVA: 0x00171298 File Offset: 0x0016F698
	protected void InitTagsLookups()
	{
		if (this._tagsArray == null || this._tagsHash == null)
		{
			this.SyncTagsLookups();
		}
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x001712B8 File Offset: 0x0016F6B8
	protected void SyncTagsLookups()
	{
		if (this._tags == null || this._tags == string.Empty)
		{
			this._builtInTagsArray = new string[0];
		}
		else
		{
			this._builtInTagsArray = this._tags.Split(new char[]
			{
				','
			});
			for (int i = 0; i < this._builtInTagsArray.Length; i++)
			{
				this._builtInTagsArray[i] = this._builtInTagsArray[i].ToLower();
			}
		}
		if (this._builtInTagsSet == null)
		{
			this._builtInTagsSet = new HashSet<string>();
		}
		else
		{
			this._builtInTagsSet.Clear();
		}
		this._builtInTagsSet.UnionWith(this._builtInTagsArray);
		if (this._replaceTags)
		{
			if (this._userTags == null || this._userTags == string.Empty)
			{
				this._tagsArray = new string[0];
			}
			else
			{
				this._tagsArray = this._userTags.Split(new char[]
				{
					','
				});
				for (int j = 0; j < this._tagsArray.Length; j++)
				{
					this._tagsArray[j] = this._tagsArray[j].ToLower();
				}
			}
		}
		else
		{
			List<string> list = new List<string>();
			if (this._tags != null && this._tags != string.Empty)
			{
				string[] array = this._tags.Split(new char[]
				{
					','
				});
				for (int k = 0; k < array.Length; k++)
				{
					list.Add(array[k].ToLower());
				}
			}
			if (this._userTags != null && this._userTags != string.Empty)
			{
				string[] array2 = this._userTags.Split(new char[]
				{
					','
				});
				for (int l = 0; l < array2.Length; l++)
				{
					list.Add(array2[l].ToLower());
				}
			}
			this._tagsArray = list.ToArray();
		}
		if (this._tagsHash == null)
		{
			this._tagsHash = new HashSet<string>();
		}
		else
		{
			this._tagsHash.Clear();
		}
		this._tagsHash.UnionWith(this._tagsArray);
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x0017150A File Offset: 0x0016F90A
	public bool CheckMatchTag(string tag)
	{
		return this._tagsHash.Contains(tag);
	}

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x06004954 RID: 18772 RVA: 0x00171518 File Offset: 0x0016F918
	// (set) Token: 0x06004955 RID: 18773 RVA: 0x00171520 File Offset: 0x0016F920
	public string tags
	{
		get
		{
			return this._tags;
		}
		set
		{
			string text = value;
			if (text != null)
			{
				text = text.Trim();
				text = Regex.Replace(text, ",\\s+", ",");
				text = Regex.Replace(text, "\\s+,", ",");
				text = text.ToLower();
			}
			if (this._tags != text)
			{
				this._tags = text;
				this.SyncTagsLookups();
			}
		}
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x00171583 File Offset: 0x0016F983
	protected void SyncReplaceTags(bool b)
	{
		this._replaceTags = b;
		this.SyncTagsLookups();
		this.SaveUserPrefs();
	}

	// Token: 0x06004957 RID: 18775 RVA: 0x00171598 File Offset: 0x0016F998
	protected void SyncUserTags(string tags)
	{
		string text = tags.Trim();
		text = Regex.Replace(text, ",\\s+", ",");
		text = Regex.Replace(text, "\\s+,", ",");
		text = text.ToLower();
		if (text != this._userTags)
		{
			this.userTagsJSON.valNoCallback = text;
			this._userTags = text;
			if (this._userTagsSet == null)
			{
				this._userTagsSet = new HashSet<string>();
			}
			else
			{
				this._userTagsSet.Clear();
			}
			if (this._userTags != string.Empty)
			{
				string[] other = this._userTags.Split(new char[]
				{
					','
				});
				this._userTagsSet.UnionWith(other);
			}
			this.SyncTagsLookups();
			this.SyncTagTogglesToTags();
			this.SaveUserPrefs();
		}
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x00171669 File Offset: 0x0016FA69
	protected virtual void SetHidePath()
	{
	}

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x06004959 RID: 18777 RVA: 0x0017166B File Offset: 0x0016FA6B
	public bool isHidden
	{
		get
		{
			return this.hidePath != null && FileManager.FileExists(this.hidePath, false, false);
		}
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x00171688 File Offset: 0x0016FA88
	protected void SyncHide(bool b)
	{
		if (this.hidePath != null)
		{
			if (FileManager.FileExists(this.hidePath, false, false))
			{
				if (!b)
				{
					FileManager.DeleteFile(this.hidePath);
				}
			}
			else if (b)
			{
				string directoryName = FileManager.GetDirectoryName(this.hidePath, false);
				if (!FileManager.DirectoryExists(directoryName, false, false))
				{
					FileManager.CreateDirectory(directoryName);
				}
				FileManager.WriteAllText(this.hidePath, string.Empty);
			}
		}
	}

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x0600495B RID: 18779 RVA: 0x001716FE File Offset: 0x0016FAFE
	public bool hasUserPrefs
	{
		get
		{
			return this.userPrefsPath != null && this.userPrefsPath != string.Empty;
		}
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x00171723 File Offset: 0x0016FB23
	protected virtual void SetUserPrefsPath()
	{
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x00171728 File Offset: 0x0016FB28
	protected virtual void LoadUserPrefs()
	{
		if (this.userPrefsPath != null && FileManager.FileExists(this.userPrefsPath, false, false))
		{
			try
			{
				string aJSON = FileManager.ReadAllText(this.userPrefsPath, true);
				JSONClass asObject = JSON.Parse(aJSON).AsObject;
				if (asObject != null)
				{
					this.isLoadingUserPrefs = true;
					this.replaceTagsJSON.RestoreFromJSON(asObject, true, true, true);
					this.userTagsJSON.RestoreFromJSON(asObject, true, true, true);
					this.isLoadingUserPrefs = false;
				}
			}
			catch (Exception ex)
			{
				SuperController.LogError("Exception during load of user prefs for item " + this.displayName + ": " + ex.Message);
			}
		}
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x001717E0 File Offset: 0x0016FBE0
	protected virtual void SaveUserPrefs()
	{
		if (!this.isLoadingUserPrefs && this.userPrefsPath != null && this.userPrefsPath != string.Empty)
		{
			try
			{
				string directoryName = FileManager.GetDirectoryName(this.userPrefsPath, false);
				FileManager.CreateDirectory(directoryName);
				JSONClass jsonclass = new JSONClass();
				this.replaceTagsJSON.StoreJSON(jsonclass, true, true, true);
				this.userTagsJSON.StoreJSON(jsonclass, true, true, true);
				FileManager.WriteAllText(this.userPrefsPath, jsonclass.ToString(string.Empty));
				if (this.dynamicSelectorUI != null)
				{
					this.dynamicSelectorUI.ResyncTags();
					this.dynamicSelectorUI.ResyncUI();
				}
			}
			catch (Exception ex)
			{
				SuperController.LogError("Exception during save of user prefs for item " + this.displayName + ": " + ex.Message);
			}
		}
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x001718CC File Offset: 0x0016FCCC
	public void ActivateUserPrefs()
	{
		this.InitTagsUI();
		if (this.dynamicSelectorUI.userPrefsLabel != null)
		{
			this.dynamicSelectorUI.userPrefsLabel.text = this.displayName + " prefs";
		}
		if (this.dynamicSelectorUI.userPrefsBuiltInTagsText != null)
		{
			this.dynamicSelectorUI.userPrefsBuiltInTagsText.text = this._tags;
		}
		this.replaceTagsJSON.toggle = this.dynamicSelectorUI.userPrefsReplaceTagsToggle;
		this.userTagsJSON.inputField = this.dynamicSelectorUI.userPrefsTagsInputField;
		this.hideJSON.valNoCallback = this.isHidden;
		this.hideJSON.toggle = this.dynamicSelectorUI.userPrefsHideToggle;
	}

	// Token: 0x06004960 RID: 18784 RVA: 0x00171994 File Offset: 0x0016FD94
	public void DeactivateUserPrefs()
	{
		this.ClearTagsUI();
		this.replaceTagsJSON.toggle = null;
		this.userTagsJSON.inputField = null;
		this.hideJSON.toggle = null;
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x001719C0 File Offset: 0x0016FDC0
	protected virtual void SyncOtherTags()
	{
		this.SyncOtherTagsUI();
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x001719C8 File Offset: 0x0016FDC8
	protected void SyncTagsToTagsSet()
	{
		string[] array = new string[this._userTagsSet.Count];
		this._userTagsSet.CopyTo(array);
		this.userTagsJSON.val = string.Join(",", array);
	}

	// Token: 0x06004963 RID: 18787 RVA: 0x00171A08 File Offset: 0x0016FE08
	protected void SyncTagFromToggle(string tag, bool isEnabled)
	{
		if (!this.ignoreTagFromToggleCallback)
		{
			if (isEnabled)
			{
				this._userTagsSet.Add(tag);
			}
			else
			{
				this._userTagsSet.Remove(tag);
			}
			this.SyncTagsToTagsSet();
		}
	}

	// Token: 0x06004964 RID: 18788 RVA: 0x00171A40 File Offset: 0x0016FE40
	protected void SyncTagTogglesToTags()
	{
		this.ignoreTagFromToggleCallback = true;
		foreach (KeyValuePair<string, Toggle> keyValuePair in this.tagToToggle)
		{
			if (this._userTagsSet.Contains(keyValuePair.Key))
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.isOn = true;
				}
			}
			else if (keyValuePair.Value != null)
			{
				keyValuePair.Value.isOn = false;
			}
			if (keyValuePair.Value != null && this._builtInTagsSet.Contains(keyValuePair.Key))
			{
				ColorBlock colors = keyValuePair.Value.colors;
				colors.normalColor = Color.yellow;
				colors.highlightedColor = Color.yellow;
				keyValuePair.Value.colors = colors;
			}
		}
		this.ignoreTagFromToggleCallback = false;
	}

	// Token: 0x06004965 RID: 18789 RVA: 0x00171B58 File Offset: 0x0016FF58
	protected void CreateTagToggle(string tag, Transform parent)
	{
		DAZDynamicItem.<CreateTagToggle>c__AnonStorey0 <CreateTagToggle>c__AnonStorey = new DAZDynamicItem.<CreateTagToggle>c__AnonStorey0();
		<CreateTagToggle>c__AnonStorey.tag = tag;
		<CreateTagToggle>c__AnonStorey.$this = this;
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.dynamicSelectorUI.tagTogglePrefab);
		Text componentInChildren = transform.GetComponentInChildren<Text>();
		componentInChildren.text = <CreateTagToggle>c__AnonStorey.tag;
		Toggle componentInChildren2 = transform.GetComponentInChildren<Toggle>();
		componentInChildren2.onValueChanged.AddListener(new UnityAction<bool>(<CreateTagToggle>c__AnonStorey.<>m__0));
		this.tagToToggle.Remove(<CreateTagToggle>c__AnonStorey.tag);
		this.tagToToggle.Add(<CreateTagToggle>c__AnonStorey.tag, componentInChildren2);
		transform.SetParent(parent, false);
	}

	// Token: 0x06004966 RID: 18790 RVA: 0x00171BE8 File Offset: 0x0016FFE8
	protected void SyncOtherTagsUI()
	{
		if (this.dynamicSelectorUI.tagTogglePrefab != null && this.dynamicSelectorUI.userPrefsOtherTagsContent != null)
		{
			IEnumerator enumerator = this.dynamicSelectorUI.userPrefsOtherTagsContent.GetEnumerator();
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
			List<string> list = this.otherTags.ToList<string>();
			list.Sort();
			foreach (string tag in list)
			{
				this.CreateTagToggle(tag, this.dynamicSelectorUI.userPrefsOtherTagsContent);
			}
			this.SyncTagTogglesToTags();
		}
	}

	// Token: 0x06004967 RID: 18791 RVA: 0x00171CF0 File Offset: 0x001700F0
	protected void ClearTagsToToggle()
	{
		if (this.tagToToggle == null)
		{
			this.tagToToggle = new Dictionary<string, Toggle>();
		}
		else
		{
			this.tagToToggle.Clear();
		}
	}

	// Token: 0x06004968 RID: 18792 RVA: 0x00171D18 File Offset: 0x00170118
	protected void ClearTagsUI()
	{
		this.ClearTagsToToggle();
		if (this.dynamicSelectorUI.userPrefsRegionTagsContent != null)
		{
			IEnumerator enumerator = this.dynamicSelectorUI.userPrefsRegionTagsContent.GetEnumerator();
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
		if (this.dynamicSelectorUI.userPrefsTypeTagsContent != null)
		{
			IEnumerator enumerator2 = this.dynamicSelectorUI.userPrefsTypeTagsContent.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					Transform transform2 = (Transform)obj2;
					UnityEngine.Object.Destroy(transform2.gameObject);
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
		if (this.dynamicSelectorUI.userPrefsOtherTagsContent != null)
		{
			IEnumerator enumerator3 = this.dynamicSelectorUI.userPrefsOtherTagsContent.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					object obj3 = enumerator3.Current;
					Transform transform3 = (Transform)obj3;
					UnityEngine.Object.Destroy(transform3.gameObject);
				}
			}
			finally
			{
				IDisposable disposable3;
				if ((disposable3 = (enumerator3 as IDisposable)) != null)
				{
					disposable3.Dispose();
				}
			}
		}
	}

	// Token: 0x06004969 RID: 18793 RVA: 0x00171E98 File Offset: 0x00170298
	protected void InitTagsUI()
	{
		this.ClearTagsUI();
		if (this.dynamicSelectorUI.tagTogglePrefab != null)
		{
			if (this.dynamicSelectorUI.regionTags != null && this.dynamicSelectorUI.userPrefsRegionTagsContent != null)
			{
				List<string> list = new List<string>(this.dynamicSelectorUI.regionTags);
				list.Sort();
				foreach (string tag in list)
				{
					this.CreateTagToggle(tag, this.dynamicSelectorUI.userPrefsRegionTagsContent);
				}
			}
			if (this.dynamicSelectorUI.typeTags != null && this.dynamicSelectorUI.userPrefsTypeTagsContent != null)
			{
				List<string> list2 = new List<string>(this.dynamicSelectorUI.typeTags);
				list2.Sort();
				foreach (string tag2 in list2)
				{
					this.CreateTagToggle(tag2, this.dynamicSelectorUI.userPrefsTypeTagsContent);
				}
			}
			this.SyncOtherTags();
		}
		this.SyncTagTogglesToTags();
	}

	// Token: 0x0600496A RID: 18794 RVA: 0x00171FF0 File Offset: 0x001703F0
	public void GetThumbnail(DAZDynamicItem.ThumbnailLoadedCallback callback)
	{
		DAZDynamicItem.<GetThumbnail>c__AnonStorey2 <GetThumbnail>c__AnonStorey = new DAZDynamicItem.<GetThumbnail>c__AnonStorey2();
		<GetThumbnail>c__AnonStorey.callback = callback;
		<GetThumbnail>c__AnonStorey.$this = this;
		if (this.thumbnail == null)
		{
			if (this.dynamicRuntimeLoadPath != null && this.dynamicRuntimeLoadPath != string.Empty)
			{
				string text = Regex.Replace(this.dynamicRuntimeLoadPath, "\\.vam$", ".jpg");
				if (FileManager.FileExists(text, false, false))
				{
					DAZDynamicItem.<GetThumbnail>c__AnonStorey1 <GetThumbnail>c__AnonStorey2 = new DAZDynamicItem.<GetThumbnail>c__AnonStorey1();
					<GetThumbnail>c__AnonStorey2.<>f__ref$2 = <GetThumbnail>c__AnonStorey;
					<GetThumbnail>c__AnonStorey2.qi = new ImageLoaderThreaded.QueuedImage();
					<GetThumbnail>c__AnonStorey2.qi.imgPath = text;
					<GetThumbnail>c__AnonStorey2.qi.width = 512;
					<GetThumbnail>c__AnonStorey2.qi.height = 512;
					<GetThumbnail>c__AnonStorey2.qi.setSize = true;
					<GetThumbnail>c__AnonStorey2.qi.fillBackground = true;
					<GetThumbnail>c__AnonStorey2.qi.callback = new ImageLoaderThreaded.ImageLoaderCallback(<GetThumbnail>c__AnonStorey2.<>m__0);
					ImageLoaderThreaded.singleton.QueueThumbnail(<GetThumbnail>c__AnonStorey2.qi);
				}
			}
		}
		else
		{
			<GetThumbnail>c__AnonStorey.callback(this.thumbnail);
		}
	}

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x0600496B RID: 18795 RVA: 0x001720FE File Offset: 0x001704FE
	// (set) Token: 0x0600496C RID: 18796 RVA: 0x00172106 File Offset: 0x00170506
	public bool active
	{
		get
		{
			return this._active;
		}
		set
		{
			if (this._active != value)
			{
				this._active = value;
			}
		}
	}

	// Token: 0x0600496D RID: 18797 RVA: 0x0017211B File Offset: 0x0017051B
	public void OpenUI()
	{
		if (this.active && this.customizationUI != null && SuperController.singleton != null)
		{
			SuperController.singleton.SetCustomUI(this.customizationUI);
		}
	}

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x0600496E RID: 18798 RVA: 0x00172159 File Offset: 0x00170559
	// (set) Token: 0x0600496F RID: 18799 RVA: 0x00172161 File Offset: 0x00170561
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			if (this._skin != value)
			{
				this._skin = value;
				this.Connect();
			}
		}
	}

	// Token: 0x06004970 RID: 18800 RVA: 0x00172184 File Offset: 0x00170584
	protected override void InitInstance()
	{
		base.InitInstance();
		DAZImport componentInChildren = base.GetComponentInChildren<DAZImport>(true);
		if (componentInChildren != null)
		{
			if (this.gender == DAZDynamicItem.Gender.Female)
			{
				componentInChildren.importGender = DAZImport.Gender.Female;
			}
			else if (this.gender == DAZDynamicItem.Gender.Male)
			{
				componentInChildren.importGender = DAZImport.Gender.Male;
			}
			else
			{
				componentInChildren.importGender = DAZImport.Gender.Neutral;
			}
		}
	}

	// Token: 0x06004971 RID: 18801 RVA: 0x001721E4 File Offset: 0x001705E4
	protected override void Connect()
	{
		base.Connect();
		if (this.hasCustomizationUI && this.customizationUI == null)
		{
			DynamicLoadedUI componentInChildren = base.GetComponentInChildren<DynamicLoadedUI>(true);
			if (componentInChildren != null)
			{
				this.customizationUI = componentInChildren.transform;
			}
			if (Application.isPlaying && this.customizationUI != null)
			{
				this.customizationUI.gameObject.SetActive(false);
				if (this.UIbucket != null)
				{
					this.customizationUI.SetParent(this.UIbucket);
					this.customizationUI.localScale = Vector3.one;
				}
			}
		}
		DAZSkinWrap[] componentsInChildren = base.GetComponentsInChildren<DAZSkinWrap>(true);
		if (componentsInChildren != null && this.skin != null)
		{
			foreach (DAZSkinWrap dazskinWrap in componentsInChildren)
			{
				dazskinWrap.skinTransform = this.skin.transform;
				dazskinWrap.skin = this.skin;
			}
		}
		if (this.drawRigidOnBone != null || this.drawRigidOnBoneLeft != null || this.drawRigidOnBoneRight != null)
		{
			DAZMesh[] componentsInChildren2 = base.GetComponentsInChildren<DAZMesh>(true);
			if (componentsInChildren2 != null)
			{
				foreach (DAZMesh dazmesh in componentsInChildren2)
				{
					DAZMesh.BoneSide boneSide = dazmesh.boneSide;
					if (boneSide != DAZMesh.BoneSide.Both)
					{
						if (boneSide != DAZMesh.BoneSide.Left)
						{
							if (boneSide == DAZMesh.BoneSide.Right)
							{
								dazmesh.drawFromBone = this.drawRigidOnBoneRight;
							}
						}
						else
						{
							dazmesh.drawFromBone = this.drawRigidOnBoneLeft;
						}
					}
					else
					{
						dazmesh.drawFromBone = this.drawRigidOnBone;
					}
				}
			}
			if (this.drawRigidOnBone != null)
			{
				AlignTransform[] componentsInChildren3 = base.GetComponentsInChildren<AlignTransform>(true);
				foreach (AlignTransform alignTransform in componentsInChildren3)
				{
					alignTransform.alignTo = this.drawRigidOnBone.transform;
				}
			}
		}
		AutoCollider[] componentsInChildren4 = base.GetComponentsInChildren<AutoCollider>(true);
		if (componentsInChildren4 != null && this.skin != null)
		{
			foreach (AutoCollider autoCollider in componentsInChildren4)
			{
				autoCollider.skinTransform = this.skin.transform;
				autoCollider.skin = this.skin;
				if (this.autoColliderReference != null)
				{
					autoCollider.reference = this.autoColliderReference;
				}
			}
		}
		DAZImport[] componentsInChildren5 = base.GetComponentsInChildren<DAZImport>(true);
		if (componentsInChildren5 != null && this.skin != null)
		{
			foreach (DAZImport dazimport in componentsInChildren5)
			{
				dazimport.skinToWrapToTransform = this.skin.transform;
				dazimport.skinToWrapTo = this.skin;
			}
		}
	}

	// Token: 0x06004972 RID: 18802 RVA: 0x001724E4 File Offset: 0x001708E4
	public virtual void PartialResetPhysics()
	{
	}

	// Token: 0x06004973 RID: 18803 RVA: 0x001724E6 File Offset: 0x001708E6
	public virtual void ResetPhysics()
	{
	}

	// Token: 0x06004974 RID: 18804 RVA: 0x001724E8 File Offset: 0x001708E8
	public virtual void Init()
	{
		if (!this._wasInit)
		{
			this._wasInit = true;
			this.replaceTagsJSON = new JSONStorableBool("replaceBuiltInTags", false, new JSONStorableBool.SetBoolCallback(this.SyncReplaceTags));
			this.userTagsJSON = new JSONStorableString("userTags", string.Empty, new JSONStorableString.SetStringCallback(this.SyncUserTags));
			this.SetHidePath();
			this.hideJSON = new JSONStorableBool("hide", this.isHidden, new JSONStorableBool.SetBoolCallback(this.SyncHide));
			this.SetUserPrefsPath();
			this.LoadUserPrefs();
			this.InitTagsLookups();
		}
	}

	// Token: 0x06004975 RID: 18805 RVA: 0x00172580 File Offset: 0x00170980
	protected override void OnEnable()
	{
		base.OnEnable();
		if (Application.isPlaying && this.customizationUI != null && this.UIbucket != null)
		{
			this.customizationUI.SetParent(this.UIbucket);
			this.customizationUI.localScale = Vector3.one;
		}
		this.ResetPhysics();
	}

	// Token: 0x06004976 RID: 18806 RVA: 0x001725E8 File Offset: 0x001709E8
	protected override void OnDisable()
	{
		if (this.customizationUI != null && this.UIbucket != null && this.instance != null)
		{
			this.customizationUI.SetParent(this.instance);
		}
		base.OnDisable();
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x0017263F File Offset: 0x00170A3F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.ClearTagsToToggle();
	}

	// Token: 0x040037BA RID: 14266
	public DAZDynamicItem.Type type;

	// Token: 0x040037BB RID: 14267
	public bool isDynamicRuntimeLoaded;

	// Token: 0x040037BC RID: 14268
	public string dynamicRuntimeLoadPath = string.Empty;

	// Token: 0x040037BD RID: 14269
	[SerializeField]
	protected string _uid;

	// Token: 0x040037BE RID: 14270
	public string internalUid;

	// Token: 0x040037BF RID: 14271
	public string backupId;

	// Token: 0x040037C0 RID: 14272
	public string creatorName = "None";

	// Token: 0x040037C1 RID: 14273
	public string displayName;

	// Token: 0x040037C2 RID: 14274
	public string version;

	// Token: 0x040037C3 RID: 14275
	public string packageUid;

	// Token: 0x040037C4 RID: 14276
	public string packageLicense;

	// Token: 0x040037C5 RID: 14277
	public bool isRealItem = true;

	// Token: 0x040037C6 RID: 14278
	protected HashSet<string> _builtInTagsSet;

	// Token: 0x040037C7 RID: 14279
	protected HashSet<string> _tagsHash;

	// Token: 0x040037C8 RID: 14280
	protected string[] _builtInTagsArray;

	// Token: 0x040037C9 RID: 14281
	protected string[] _tagsArray;

	// Token: 0x040037CA RID: 14282
	[SerializeField]
	protected string _tags = string.Empty;

	// Token: 0x040037CB RID: 14283
	protected bool _replaceTags;

	// Token: 0x040037CC RID: 14284
	protected JSONStorableBool replaceTagsJSON;

	// Token: 0x040037CD RID: 14285
	protected HashSet<string> _userTagsSet = new HashSet<string>();

	// Token: 0x040037CE RID: 14286
	protected string _userTags = string.Empty;

	// Token: 0x040037CF RID: 14287
	protected JSONStorableString userTagsJSON;

	// Token: 0x040037D0 RID: 14288
	protected string hidePath;

	// Token: 0x040037D1 RID: 14289
	protected JSONStorableBool hideJSON;

	// Token: 0x040037D2 RID: 14290
	protected string userPrefsPath;

	// Token: 0x040037D3 RID: 14291
	protected bool isLoadingUserPrefs;

	// Token: 0x040037D4 RID: 14292
	public GenerateDAZDynamicSelectorUI dynamicSelectorUI;

	// Token: 0x040037D5 RID: 14293
	protected bool userPrefsRegisteredClose;

	// Token: 0x040037D6 RID: 14294
	protected HashSet<string> otherTags = new HashSet<string>();

	// Token: 0x040037D7 RID: 14295
	private bool ignoreTagFromToggleCallback;

	// Token: 0x040037D8 RID: 14296
	protected Dictionary<string, Toggle> tagToToggle = new Dictionary<string, Toggle>();

	// Token: 0x040037D9 RID: 14297
	public Texture2D thumbnail;

	// Token: 0x040037DA RID: 14298
	public bool startActive;

	// Token: 0x040037DB RID: 14299
	protected bool _active;

	// Token: 0x040037DC RID: 14300
	public bool locked;

	// Token: 0x040037DD RID: 14301
	public bool isLegacy;

	// Token: 0x040037DE RID: 14302
	public bool isLatestVersion = true;

	// Token: 0x040037DF RID: 14303
	public bool hasCustomizationUI;

	// Token: 0x040037E0 RID: 14304
	public Transform UIbucket;

	// Token: 0x040037E1 RID: 14305
	public Transform customizationUI;

	// Token: 0x040037E2 RID: 14306
	public bool showFirstInUI;

	// Token: 0x040037E3 RID: 14307
	public DAZCharacterSelector characterSelector;

	// Token: 0x040037E4 RID: 14308
	public DAZDynamicItem.Gender gender = DAZDynamicItem.Gender.Female;

	// Token: 0x040037E5 RID: 14309
	public bool disableAnatomy;

	// Token: 0x040037E6 RID: 14310
	public DAZDynamicItem.BoneType drawRigidOnBoneType;

	// Token: 0x040037E7 RID: 14311
	public DAZDynamicItem.BoneType drawRigidOnBoneTypeLeft;

	// Token: 0x040037E8 RID: 14312
	public DAZDynamicItem.BoneType drawRigidOnBoneTypeRight;

	// Token: 0x040037E9 RID: 14313
	public DAZDynamicItem.BoneType autoColliderReferenceBoneType;

	// Token: 0x040037EA RID: 14314
	public DAZBone drawRigidOnBone;

	// Token: 0x040037EB RID: 14315
	public DAZBone drawRigidOnBoneLeft;

	// Token: 0x040037EC RID: 14316
	public DAZBone drawRigidOnBoneRight;

	// Token: 0x040037ED RID: 14317
	public Transform autoColliderReference;

	// Token: 0x040037EE RID: 14318
	[SerializeField]
	protected DAZSkinV2 _skin;

	// Token: 0x040037EF RID: 14319
	protected bool _wasInit;

	// Token: 0x02000AC8 RID: 2760
	public enum Type
	{
		// Token: 0x040037F1 RID: 14321
		Wrap,
		// Token: 0x040037F2 RID: 14322
		Sim,
		// Token: 0x040037F3 RID: 14323
		Custom,
		// Token: 0x040037F4 RID: 14324
		Ignore
	}

	// Token: 0x02000AC9 RID: 2761
	// (Invoke) Token: 0x06004979 RID: 18809
	public delegate void ThumbnailLoadedCallback(Texture2D thumb);

	// Token: 0x02000ACA RID: 2762
	public enum Gender
	{
		// Token: 0x040037F6 RID: 14326
		Neutral,
		// Token: 0x040037F7 RID: 14327
		Female,
		// Token: 0x040037F8 RID: 14328
		Male
	}

	// Token: 0x02000ACB RID: 2763
	public enum BoneType
	{
		// Token: 0x040037FA RID: 14330
		None,
		// Token: 0x040037FB RID: 14331
		Hip,
		// Token: 0x040037FC RID: 14332
		Pelvis,
		// Token: 0x040037FD RID: 14333
		Chest,
		// Token: 0x040037FE RID: 14334
		Head,
		// Token: 0x040037FF RID: 14335
		LeftHand,
		// Token: 0x04003800 RID: 14336
		RightHand,
		// Token: 0x04003801 RID: 14337
		LeftFoot,
		// Token: 0x04003802 RID: 14338
		RightFoot
	}

	// Token: 0x02000FC7 RID: 4039
	[CompilerGenerated]
	private sealed class <CreateTagToggle>c__AnonStorey0
	{
		// Token: 0x0600752C RID: 29996 RVA: 0x0017264D File Offset: 0x00170A4D
		public <CreateTagToggle>c__AnonStorey0()
		{
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x00172655 File Offset: 0x00170A55
		internal void <>m__0(bool b)
		{
			this.$this.SyncTagFromToggle(this.tag, b);
		}

		// Token: 0x04006945 RID: 26949
		internal string tag;

		// Token: 0x04006946 RID: 26950
		internal DAZDynamicItem $this;
	}

	// Token: 0x02000FC8 RID: 4040
	[CompilerGenerated]
	private sealed class <GetThumbnail>c__AnonStorey2
	{
		// Token: 0x0600752E RID: 29998 RVA: 0x00172669 File Offset: 0x00170A69
		public <GetThumbnail>c__AnonStorey2()
		{
		}

		// Token: 0x04006947 RID: 26951
		internal DAZDynamicItem.ThumbnailLoadedCallback callback;

		// Token: 0x04006948 RID: 26952
		internal DAZDynamicItem $this;
	}

	// Token: 0x02000FC9 RID: 4041
	[CompilerGenerated]
	private sealed class <GetThumbnail>c__AnonStorey1
	{
		// Token: 0x0600752F RID: 29999 RVA: 0x00172671 File Offset: 0x00170A71
		public <GetThumbnail>c__AnonStorey1()
		{
		}

		// Token: 0x06007530 RID: 30000 RVA: 0x00172679 File Offset: 0x00170A79
		internal void <>m__0(ImageLoaderThreaded.QueuedImage A_1)
		{
			this.<>f__ref$2.$this.thumbnail = this.qi.tex;
			this.<>f__ref$2.callback(this.<>f__ref$2.$this.thumbnail);
		}

		// Token: 0x04006949 RID: 26953
		internal ImageLoaderThreaded.QueuedImage qi;

		// Token: 0x0400694A RID: 26954
		internal DAZDynamicItem.<GetThumbnail>c__AnonStorey2 <>f__ref$2;
	}
}
