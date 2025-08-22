using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SimpleJSON;

namespace MVR.FileManagement
{
	// Token: 0x02000BFA RID: 3066
	public class VarPackageGroup
	{
		// Token: 0x06005886 RID: 22662 RVA: 0x00207176 File Offset: 0x00205576
		public VarPackageGroup(string name)
		{
			this.Name = name;
			this._versions = new List<int>();
			this._enabledVersions = new List<int>();
			this.Packages = new List<VarPackage>();
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06005887 RID: 22663 RVA: 0x002071B1 File Offset: 0x002055B1
		// (set) Token: 0x06005888 RID: 22664 RVA: 0x002071B9 File Offset: 0x002055B9
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06005889 RID: 22665 RVA: 0x002071C4 File Offset: 0x002055C4
		public List<int> Versions
		{
			get
			{
				List<int> list = this._versions.ToList<int>();
				list.Sort();
				return list;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x0600588A RID: 22666 RVA: 0x002071E4 File Offset: 0x002055E4
		public int NewestVersion
		{
			get
			{
				if (this.NewestPackage != null)
				{
					return this.NewestPackage.Version;
				}
				return 0;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x0600588B RID: 22667 RVA: 0x002071FE File Offset: 0x002055FE
		public int NewestEnabledVersion
		{
			get
			{
				if (this.NewestPackage != null)
				{
					return this.NewestEnabledPackage.Version;
				}
				return 0;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x0600588C RID: 22668 RVA: 0x00207218 File Offset: 0x00205618
		// (set) Token: 0x0600588D RID: 22669 RVA: 0x00207220 File Offset: 0x00205620
		public List<VarPackage> Packages
		{
			[CompilerGenerated]
			get
			{
				return this.<Packages>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Packages>k__BackingField = value;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x0600588E RID: 22670 RVA: 0x00207229 File Offset: 0x00205629
		// (set) Token: 0x0600588F RID: 22671 RVA: 0x00207231 File Offset: 0x00205631
		public VarPackage NewestPackage
		{
			[CompilerGenerated]
			get
			{
				return this.<NewestPackage>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<NewestPackage>k__BackingField = value;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06005890 RID: 22672 RVA: 0x0020723A File Offset: 0x0020563A
		// (set) Token: 0x06005891 RID: 22673 RVA: 0x00207242 File Offset: 0x00205642
		public VarPackage NewestEnabledPackage
		{
			[CompilerGenerated]
			get
			{
				return this.<NewestEnabledPackage>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<NewestEnabledPackage>k__BackingField = value;
			}
		}

		// Token: 0x06005892 RID: 22674 RVA: 0x0020724C File Offset: 0x0020564C
		public VarPackage GetClosestMatchingPackageVersion(int requestVersion, bool onlyUseEnabledPackages = true, bool returnLatestOnMissing = true)
		{
			int num = -1;
			List<int> list;
			if (onlyUseEnabledPackages)
			{
				list = this._enabledVersions;
			}
			else
			{
				list = this._versions;
			}
			foreach (int num2 in list)
			{
				if (requestVersion <= num2)
				{
					num = num2;
					break;
				}
			}
			if (num == -1)
			{
				if (returnLatestOnMissing)
				{
					if (onlyUseEnabledPackages)
					{
						return this.NewestEnabledPackage;
					}
					return this.NewestPackage;
				}
			}
			else
			{
				foreach (VarPackage varPackage in this.Packages)
				{
					if (varPackage.Version == num)
					{
						return varPackage;
					}
				}
			}
			return null;
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x0020734C File Offset: 0x0020574C
		protected void SyncNewestVersion()
		{
			this.NewestPackage = null;
			this.NewestEnabledPackage = null;
			if (this._versions.Count > 0)
			{
				int num = this._versions[this._versions.Count - 1];
				foreach (VarPackage varPackage in this.Packages)
				{
					if (varPackage.Version == num)
					{
						varPackage.isNewestVersion = true;
						this.NewestPackage = varPackage;
					}
					else
					{
						varPackage.isNewestVersion = false;
					}
					if (varPackage.Enabled)
					{
						int num2 = this._enabledVersions[this._enabledVersions.Count - 1];
						if (varPackage.Version == num2)
						{
							varPackage.isNewestEnabledVersion = true;
							this.NewestEnabledPackage = varPackage;
						}
						else
						{
							varPackage.isNewestEnabledVersion = false;
						}
					}
					else
					{
						varPackage.isNewestEnabledVersion = false;
					}
				}
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06005894 RID: 22676 RVA: 0x00207458 File Offset: 0x00205858
		// (set) Token: 0x06005895 RID: 22677 RVA: 0x00207460 File Offset: 0x00205860
		public string UserNotes
		{
			get
			{
				return this._userNotes;
			}
			set
			{
				if (this._userNotes != value)
				{
					this._userNotes = value;
					this.SaveUserPrefs();
				}
			}
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x00207480 File Offset: 0x00205880
		public List<string> GetCustomOptionNames()
		{
			List<string> result;
			if (this.customOptions != null)
			{
				result = this.customOptions.Keys.ToList<string>();
			}
			else
			{
				result = new List<string>();
			}
			return result;
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x002074B8 File Offset: 0x002058B8
		public bool GetCustomOption(string optionName)
		{
			bool result = false;
			if (this.customOptions != null)
			{
				this.customOptions.TryGetValue(optionName, out result);
			}
			return result;
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x002074E4 File Offset: 0x002058E4
		public void SetCustomOption(string optionName, bool optionValue)
		{
			if (this.customOptions != null)
			{
				if (this.customOptions.ContainsKey(optionName))
				{
					this.customOptions.Remove(optionName);
				}
				this.customOptions.Add(optionName, optionValue);
				this.SaveUserPrefs();
				foreach (VarPackage varPackage in this.Packages)
				{
					varPackage.forceRefresh = true;
				}
				FileManager.Refresh();
			}
		}

		// Token: 0x06005899 RID: 22681 RVA: 0x00207584 File Offset: 0x00205984
		protected void LoadUserPrefs()
		{
			string path = FileManager.UserPrefsFolder + "/" + this.Name + ".prefs";
			if (FileManager.FileExists(path, false, false))
			{
				using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(path, false))
				{
					string aJSON = fileEntryStreamReader.ReadToEnd();
					JSONClass asObject = JSON.Parse(aJSON).AsObject;
					if (asObject != null)
					{
						this._userNotes = asObject["userNotes"];
						JSONClass asObject2 = asObject["customOptions"].AsObject;
						this.customOptions = new Dictionary<string, bool>();
						if (asObject2 != null)
						{
							foreach (string text in asObject2.Keys)
							{
								if (!this.customOptions.ContainsKey(text))
								{
									this.customOptions.Add(text, asObject2[text].AsBool);
								}
							}
						}
					}
				}
			}
			else
			{
				this._userNotes = string.Empty;
				this.customOptions = new Dictionary<string, bool>();
				VarPackage newestPackage = this.NewestPackage;
				if (newestPackage != null)
				{
					List<string> customOptionNames = newestPackage.GetCustomOptionNames();
					foreach (string text2 in customOptionNames)
					{
						this.customOptions.Add(text2, newestPackage.GetCustomOption(text2));
					}
				}
			}
		}

		// Token: 0x0600589A RID: 22682 RVA: 0x00207744 File Offset: 0x00205B44
		protected void SaveUserPrefs()
		{
			string text = FileManager.UserPrefsFolder + "/" + this.Name + ".prefs";
			JSONClass jsonclass = new JSONClass();
			jsonclass["userNotes"] = this.UserNotes;
			JSONClass jsonclass2 = new JSONClass();
			jsonclass["customOptions"] = jsonclass2;
			if (this.customOptions != null)
			{
				foreach (KeyValuePair<string, bool> keyValuePair in this.customOptions)
				{
					jsonclass2[keyValuePair.Key].AsBool = keyValuePair.Value;
				}
			}
			string text2 = jsonclass.ToString(string.Empty);
			try
			{
				FileManager.WriteAllText(text, text2);
			}
			catch (Exception ex)
			{
				SuperController.LogError("Error during save of prefs file " + text + ": " + ex.Message);
			}
		}

		// Token: 0x0600589B RID: 22683 RVA: 0x00207850 File Offset: 0x00205C50
		public void AddPackage(VarPackage vp)
		{
			this.Packages.Add(vp);
			if (this._versions.Contains(vp.Version))
			{
				throw new Exception(string.Concat(new object[]
				{
					"Tried to add package to group ",
					this.Name,
					" with version ",
					vp.Version,
					" that was already added"
				}));
			}
			this._versions.Add(vp.Version);
			this._versions.Sort();
			if (vp.Enabled)
			{
				this._enabledVersions.Add(vp.Version);
				this._enabledVersions.Sort();
			}
			this.SyncNewestVersion();
		}

		// Token: 0x0600589C RID: 22684 RVA: 0x00207908 File Offset: 0x00205D08
		public void RemovePackage(VarPackage vp)
		{
			this.Packages.Remove(vp);
			this._versions.Remove(vp.Version);
			this._versions.Sort();
			this._enabledVersions.Remove(vp.Version);
			this._enabledVersions.Sort();
			this.SyncNewestVersion();
		}

		// Token: 0x0600589D RID: 22685 RVA: 0x00207962 File Offset: 0x00205D62
		public void Init()
		{
			this.LoadUserPrefs();
		}

		// Token: 0x040048E9 RID: 18665
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Name>k__BackingField;

		// Token: 0x040048EA RID: 18666
		protected List<int> _versions;

		// Token: 0x040048EB RID: 18667
		protected List<int> _enabledVersions;

		// Token: 0x040048EC RID: 18668
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<VarPackage> <Packages>k__BackingField;

		// Token: 0x040048ED RID: 18669
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarPackage <NewestPackage>k__BackingField;

		// Token: 0x040048EE RID: 18670
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarPackage <NewestEnabledPackage>k__BackingField;

		// Token: 0x040048EF RID: 18671
		protected string _userNotes = string.Empty;

		// Token: 0x040048F0 RID: 18672
		protected Dictionary<string, bool> customOptions;
	}
}
