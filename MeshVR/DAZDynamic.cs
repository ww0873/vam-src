using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using GPUTools.Cloth.Scripts;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Runtime.Physics;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Geometry.Create;
using GPUTools.Skinner.Scripts.Providers;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000AC4 RID: 2756
	public class DAZDynamic : PresetManager
	{
		// Token: 0x0600492D RID: 18733 RVA: 0x001782B2 File Offset: 0x001766B2
		public DAZDynamic()
		{
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x0600492E RID: 18734 RVA: 0x001782CC File Offset: 0x001766CC
		// (set) Token: 0x0600492F RID: 18735 RVA: 0x001782D4 File Offset: 0x001766D4
		public string uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				if (this._uid != value)
				{
					this._uid = value;
					if (this._uid == null || this._uid == string.Empty)
					{
						base.name = "NoUID";
					}
					else
					{
						base.name = value;
					}
				}
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06004930 RID: 18736 RVA: 0x00178330 File Offset: 0x00176730
		// (set) Token: 0x06004931 RID: 18737 RVA: 0x00178338 File Offset: 0x00176738
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

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06004932 RID: 18738 RVA: 0x00178341 File Offset: 0x00176741
		// (set) Token: 0x06004933 RID: 18739 RVA: 0x0017834C File Offset: 0x0017674C
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
					if (this._tags == null || this._tags == string.Empty)
					{
						this.tagsArray = new string[0];
					}
					else
					{
						this.tagsArray = this._tags.Split(new char[]
						{
							','
						});
					}
				}
			}
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x001783F8 File Offset: 0x001767F8
		public bool CheckMatchTag(string tag)
		{
			string b = tag.ToLower();
			if (this._tagsArray != null)
			{
				foreach (string text in this._tagsArray)
				{
					if (text.ToLower() == b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x0017844C File Offset: 0x0017684C
		public string GetMetaStorePath()
		{
			string result = null;
			if (this.CheckReadyForLoad())
			{
				string storeFolderPath = base.GetStoreFolderPath(true);
				result = storeFolderPath + this.storeName + ".vam";
			}
			return result;
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00178484 File Offset: 0x00176884
		public void Delete()
		{
			if (this.CheckReadyForLoad())
			{
				if (this.package == string.Empty)
				{
					string storeFolderPath = base.GetStoreFolderPath(true);
					string path = storeFolderPath + this.storeName + ".vab";
					string path2 = storeFolderPath + this.storeName + ".vam";
					string path3 = storeFolderPath + this.storeName + ".vaj";
					try
					{
						FileManager.AssertNotCalledFromPlugin();
						this.Clear();
						if (Application.isPlaying)
						{
							FileManager.CreateDirectory(storeFolderPath);
						}
						FileManager.DeleteFile(path);
						FileManager.DeleteFile(path2);
						FileManager.DeleteFile(path3);
						string[] files = FileManager.GetFiles(storeFolderPath, this.storeName + "_*.vap", false);
						foreach (string path4 in files)
						{
							FileManager.DeleteFile(path4);
						}
						string[] files2 = FileManager.GetFiles(storeFolderPath, this.storeName + "_*.vap.fav", false);
						foreach (string path5 in files2)
						{
							FileManager.DeleteFile(path5);
						}
					}
					catch (Exception ex)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception while deleting ",
							this.storeName,
							": ",
							ex
						}));
					}
				}
				else
				{
					SuperController.LogError("Tried to delete package item " + this.storeName);
				}
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x00178608 File Offset: 0x00176A08
		protected override void StorePresetBinary()
		{
			string storeFolderPath = base.GetStoreFolderPath(false);
			if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
			{
				if (!base.IsPresetInPackage())
				{
					string text = string.Concat(new string[]
					{
						storeFolderPath,
						this.presetSubPath,
						this.storeName,
						"_",
						this.presetSubName,
						".vapb"
					});
					if (this.itemType == PresetManager.ItemType.HairFemale || this.itemType == PresetManager.ItemType.HairMale || this.itemType == PresetManager.ItemType.HairNeutral)
					{
						RuntimeHairGeometryCreator component = base.GetComponent<RuntimeHairGeometryCreator>();
						if (component != null)
						{
							try
							{
								using (FileStream fileStream = FileManager.OpenStreamForCreate(text))
								{
									using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
									{
										component.StoreAuxToBinaryWriter(binaryWriter);
									}
								}
							}
							catch (Exception ex)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception while storing to ",
									text,
									" ",
									ex
								}));
							}
						}
					}
				}
				else
				{
					SuperController.LogError("Attempted to store a preset binary into a package. Cannot store");
				}
			}
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x0017879C File Offset: 0x00176B9C
		public override void RestorePresetBinary()
		{
			string storeFolderPath = base.GetStoreFolderPath(false);
			if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty)
			{
				RuntimeHairGeometryCreator component = base.GetComponent<RuntimeHairGeometryCreator>();
				if (component != null)
				{
					if (this._presetName != null && this._presetName != string.Empty)
					{
						string text = string.Concat(new string[]
						{
							this.presetPackagePath,
							storeFolderPath,
							this.presetSubPath,
							this.storeName,
							"_",
							this.presetSubName,
							".vapb"
						});
						if (this.itemType == PresetManager.ItemType.HairFemale || this.itemType == PresetManager.ItemType.HairMale || this.itemType == PresetManager.ItemType.HairNeutral)
						{
							if (FileManager.FileExists(text, false, false))
							{
								try
								{
									using (FileEntryStream fileEntryStream = FileManager.OpenStream(text, true))
									{
										using (BinaryReader binaryReader = new BinaryReader(fileEntryStream.Stream))
										{
											component.LoadAuxFromBinaryReader(binaryReader);
											HairSettings component2 = base.GetComponent<HairSettings>();
											if (component2 != null && component2.HairBuidCommand != null)
											{
												component2.HairBuidCommand.RebuildHair();
											}
										}
									}
								}
								catch (Exception ex)
								{
									SuperController.LogError(string.Concat(new object[]
									{
										"Exception while loading ",
										text,
										" ",
										ex
									}));
								}
							}
							else if (component.usingAuxData)
							{
								component.RevertToLoadedData();
								HairSettings component3 = base.GetComponent<HairSettings>();
								if (component3 != null && component3.HairBuidCommand != null)
								{
									component3.HairBuidCommand.RebuildHair();
								}
							}
						}
					}
					else if (component.usingAuxData)
					{
						component.RevertToLoadedData();
						HairSettings component4 = base.GetComponent<HairSettings>();
						if (component4 != null && component4.HairBuidCommand != null)
						{
							component4.HairBuidCommand.RebuildHair();
						}
					}
				}
			}
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x001789E4 File Offset: 0x00176DE4
		public void Clear()
		{
			ClothSettings component = base.GetComponent<ClothSettings>();
			if (component)
			{
				component.enabled = false;
				UnityEngine.Object.Destroy(component);
			}
			ClothPhysics component2 = base.GetComponent<ClothPhysics>();
			if (component2)
			{
				UnityEngine.Object.Destroy(component2);
			}
			IBinaryStorable[] components = this.binaryStorableBucket.GetComponents<IBinaryStorable>();
			foreach (IBinaryStorable binaryStorable in components)
			{
				if (binaryStorable is Component)
				{
					UnityEngine.Object.Destroy(binaryStorable as Component);
				}
			}
			MeshRenderer component3 = base.GetComponent<MeshRenderer>();
			if (component3 != null)
			{
				UnityEngine.Object.Destroy(component3);
			}
			MeshFilter component4 = base.GetComponent<MeshFilter>();
			if (component4 != null)
			{
				UnityEngine.Object.Destroy(component4);
			}
			base.RefreshStorables();
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00178AAC File Offset: 0x00176EAC
		public bool CheckReadyForStore()
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = base.GetStoreFolderPath(true);
				if (base.IsInPackage())
				{
					VarPackage package = FileManager.GetPackage(this.package);
					if (!package.IsSimulated)
					{
						return false;
					}
				}
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty)
				{
					IBinaryStorable[] components = this.binaryStorableBucket.GetComponents<IBinaryStorable>();
					if (this.allowZeroBinaryStorables || components.Length > 0)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x00178B4C File Offset: 0x00176F4C
		public bool CheckReadyForLoad()
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = base.GetStoreFolderPath(true);
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty)
				{
					string path = storeFolderPath + this.storeName + ".vam";
					string path2 = storeFolderPath + this.storeName + ".vaj";
					if (FileManager.FileExists(path, false, false) && FileManager.FileExists(path2, false, false))
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x00178BE8 File Offset: 0x00176FE8
		public bool CheckStoreExistance()
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = base.GetStoreFolderPath(true);
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty)
				{
					string path = storeFolderPath + this.storeName + ".vaj";
					result = FileManager.FileExists(path, false, false);
				}
			}
			return result;
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x00178C5C File Offset: 0x0017705C
		public bool Store()
		{
			if (this.itemType == PresetManager.ItemType.None)
			{
				SuperController.LogError("Item type set to None. Cannot store");
				return false;
			}
			if (this.CheckReadyForStore())
			{
				string storeFolderPath = base.GetStoreFolderPath(true);
				string text = storeFolderPath + this.storeName + ".vab";
				string text2 = storeFolderPath + this.storeName + ".vam";
				string text3 = storeFolderPath + this.storeName + ".vaj";
				JSONClass jsonclass = new JSONClass();
				jsonclass["itemType"] = this.itemType.ToString();
				jsonclass["uid"] = this._uid;
				jsonclass["displayName"] = this.displayName;
				jsonclass["creatorName"] = this.creatorName;
				jsonclass["tags"] = this.tags;
				jsonclass["isRealItem"].AsBool = this.isRealItem;
				this.storedCreatorName = this.creatorName;
				JSONClass jsonclass2 = new JSONClass();
				jsonclass2["components"] = new JSONArray();
				IBinaryStorable[] components = this.binaryStorableBucket.GetComponents<IBinaryStorable>();
				foreach (IBinaryStorable binaryStorable in components)
				{
					JSONClass jsonclass3 = new JSONClass();
					jsonclass3["type"] = binaryStorable.GetType().ToString();
					jsonclass2["components"].Add(jsonclass3);
				}
				try
				{
					FileManager.AssertNotCalledFromPlugin();
					if (Application.isPlaying)
					{
						FileManager.CreateDirectory(storeFolderPath);
					}
					using (FileStream fileStream = FileManager.OpenStreamForCreate(text))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							binaryWriter.Write("DynamicStore");
							binaryWriter.Write("1.0");
							foreach (IBinaryStorable binaryStorable2 in components)
							{
								binaryStorable2.StoreToBinaryWriter(binaryWriter);
							}
							if (this.itemType == PresetManager.ItemType.ClothingFemale || this.itemType == PresetManager.ItemType.ClothingMale || this.itemType == PresetManager.ItemType.ClothingNeutral)
							{
								ClothSettings component = base.GetComponent<ClothSettings>();
								if (component != null)
								{
									ClothGeometryData geometryData = component.GeometryData;
									if (geometryData != null && geometryData.IsProcessed)
									{
										binaryWriter.Write(true);
										geometryData.StoreToBinaryWriter(binaryWriter);
									}
									else
									{
										binaryWriter.Write(false);
									}
								}
								else
								{
									binaryWriter.Write(false);
								}
							}
							else if (this.itemType == PresetManager.ItemType.HairFemale || this.itemType == PresetManager.ItemType.HairMale || this.itemType == PresetManager.ItemType.HairNeutral)
							{
								RuntimeHairGeometryCreator component2 = base.GetComponent<RuntimeHairGeometryCreator>();
								if (component2 != null)
								{
									binaryWriter.Write(true);
									component2.StoreToBinaryWriter(binaryWriter);
								}
								else
								{
									binaryWriter.Write(false);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception while storing to ",
						text,
						" ",
						ex
					}));
					return false;
				}
				base.RefreshStorables();
				FileManager.SetSaveDirFromFilePath(text3, true);
				base.StoreStorables(jsonclass2, true);
				try
				{
					using (StreamWriter streamWriter = FileManager.OpenStreamWriter(text2))
					{
						StringBuilder stringBuilder = new StringBuilder(1000);
						jsonclass.ToString(string.Empty, stringBuilder);
						streamWriter.Write(stringBuilder.ToString());
					}
				}
				catch (Exception ex2)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception while storing to ",
						text2,
						" ",
						ex2
					}));
					return false;
				}
				try
				{
					using (StreamWriter streamWriter2 = FileManager.OpenStreamWriter(text3))
					{
						StringBuilder stringBuilder2 = new StringBuilder(10000);
						jsonclass2.ToString(string.Empty, stringBuilder2);
						streamWriter2.Write(stringBuilder2.ToString());
					}
				}
				catch (Exception ex3)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception while storing to ",
						text3,
						" ",
						ex3
					}));
					return false;
				}
				return true;
			}
			SuperController.LogError("Not ready for store. Store root or name not set");
			return false;
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x00179170 File Offset: 0x00177570
		public void GetScreenshot(SuperController.ScreenShotCallback callback = null)
		{
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = base.GetStoreFolderPath(true);
				if (storeFolderPath != null)
				{
					string path = storeFolderPath + this.storeName + ".vam";
					if (FileManager.FileExists(path, false, false))
					{
						string saveName = storeFolderPath + this.storeName + ".jpg";
						SuperController.singleton.DoSaveScreenshot(saveName, callback);
					}
					else
					{
						SuperController.LogError("Screenshot only works after store");
					}
				}
			}
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x001791E4 File Offset: 0x001775E4
		public ClothSettings CreateNewClothSettings()
		{
			ClothSettings clothSettings = base.GetComponent<ClothSettings>();
			if (clothSettings != null)
			{
				clothSettings.enabled = false;
				UnityEngine.Object.Destroy(clothSettings);
			}
			clothSettings = base.gameObject.AddComponent<ClothSettings>();
			clothSettings.enabled = false;
			DAZSkinWrap component = base.GetComponent<DAZSkinWrap>();
			if (component != null)
			{
				clothSettings.MeshProvider.Type = ScalpMeshType.PreCalc;
				clothSettings.MeshProvider.PreCalcProvider = component;
			}
			ClothSimControl component2 = base.GetComponent<ClothSimControl>();
			if (component2 != null)
			{
				component2.clothSettings = clothSettings;
				component2.SetSimEnabled(false);
			}
			return clothSettings;
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x00179274 File Offset: 0x00177674
		public void SetDefaultsFromCurrent()
		{
			if (this.storables != null)
			{
				foreach (PresetManager.Storable storable in this.storables)
				{
					JSONStorable storable2 = storable.storable;
					if (this.ignoreExclude || !storable2.exclude)
					{
						storable2.SetDefaultsFromCurrent();
					}
				}
			}
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x001792F8 File Offset: 0x001776F8
		public bool Load(bool createWithExclude = false)
		{
			if (this.itemType == PresetManager.ItemType.None)
			{
				SuperController.LogError("Item type set to None. Cannot load");
				return false;
			}
			if (this.CheckReadyForLoad())
			{
				string storeFolderPath = base.GetStoreFolderPath(true);
				string text = storeFolderPath + this.storeName + ".vab";
				string text2 = storeFolderPath + this.storeName + ".vam";
				string text3 = storeFolderPath + this.storeName + ".vaj";
				if (base.IsInPackage())
				{
					VarPackage package = FileManager.GetPackage(this.package);
					PackageInfo component = base.GetComponent<PackageInfo>();
					if (package != null && component != null)
					{
						component.SetPackage(package);
					}
				}
				string aJSON = string.Empty;
				try
				{
					aJSON = FileManager.ReadAllText(text2, true);
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception while loading ",
						text2,
						" ",
						ex
					}));
					return false;
				}
				JSONClass asObject = JSON.Parse(aJSON).AsObject;
				string aJSON2 = string.Empty;
				try
				{
					aJSON2 = FileManager.ReadAllText(text3, true);
				}
				catch (Exception ex2)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception while loading ",
						text3,
						" ",
						ex2
					}));
					return false;
				}
				JSONClass asObject2 = JSON.Parse(aJSON2).AsObject;
				if (asObject != null && asObject2 != null)
				{
					Assembly assembly = base.GetType().Assembly;
					string text4 = asObject["itemType"];
					if (text4 != null)
					{
						try
						{
							this.itemType = (PresetManager.ItemType)Enum.Parse(typeof(PresetManager.ItemType), text4);
						}
						catch (ArgumentException)
						{
							SuperController.LogError("Attempted to set itemType to " + text4 + " which is not a valid item type");
						}
					}
					if (asObject["uid"] != null)
					{
						this.uid = asObject["uid"];
					}
					if (asObject["displayName"] != null)
					{
						this.displayName = asObject["displayName"];
					}
					else
					{
						this.displayName = Regex.Replace(this.storeName, ".*:", string.Empty);
					}
					if (asObject["creatorName"] != null)
					{
						this.storedCreatorName = asObject["creatorName"];
					}
					if (asObject["tags"] != null)
					{
						this.tags = asObject["tags"];
					}
					else
					{
						this.tags = string.Empty;
					}
					if (asObject["isRealItem"] != null)
					{
						this.isRealItem = asObject["isRealItem"].AsBool;
					}
					else
					{
						this.isRealItem = true;
					}
					if (asObject2["components"] != null)
					{
						IEnumerator enumerator = asObject2["components"].AsArray.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								JSONClass jsonclass = (JSONClass)obj;
								string text5 = jsonclass["type"];
								if (text5 != null)
								{
									Type type = assembly.GetType(text5);
									if (type != null)
									{
										this.binaryStorableBucket.gameObject.AddComponent(type);
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
				}
				DAZImport dazimport = null;
				DAZMesh dazmesh = null;
				if (FileManager.FileExists(text, false, false))
				{
					IBinaryStorable[] components = this.binaryStorableBucket.GetComponents<IBinaryStorable>();
					try
					{
						using (FileEntryStream fileEntryStream = FileManager.OpenStream(text, true))
						{
							using (BinaryReader binaryReader = new BinaryReader(fileEntryStream.Stream))
							{
								string a = binaryReader.ReadString();
								if (a != "DynamicStore")
								{
									SuperController.LogError("Binary file " + text + " corrupted. Cannot read");
									this.Clear();
									return false;
								}
								string text6 = binaryReader.ReadString();
								if (text6 != "1.0")
								{
									SuperController.LogError("Binary schema " + text6 + " is not compatible with this version of software");
									this.Clear();
									return false;
								}
								foreach (IBinaryStorable binaryStorable in components)
								{
									if (!binaryStorable.LoadFromBinaryReader(binaryReader))
									{
										this.Clear();
										return false;
									}
								}
								dazimport = this.binaryStorableBucket.GetComponent<DAZImport>();
								dazmesh = this.binaryStorableBucket.GetComponent<DAZMesh>();
								DAZSkinWrap[] components2 = this.binaryStorableBucket.GetComponents<DAZSkinWrap>();
								foreach (DAZSkinWrap dazskinWrap in components2)
								{
									dazskinWrap.dazMesh = dazmesh;
									dazskinWrap.CopyMaterials();
									if (dazimport != null)
									{
										dazskinWrap.skinTransform = dazimport.skinToWrapToTransform;
										dazskinWrap.skin = dazimport.skinToWrapTo;
										if (dazimport.GPUSkinCompute != null)
										{
											dazskinWrap.GPUSkinWrapper = dazimport.GPUSkinCompute;
										}
										if (dazimport.GPUMeshCompute != null)
										{
											dazskinWrap.GPUMeshCompute = dazimport.GPUMeshCompute;
										}
									}
								}
								if (this.itemType == PresetManager.ItemType.ClothingFemale || this.itemType == PresetManager.ItemType.ClothingMale || this.itemType == PresetManager.ItemType.ClothingNeutral)
								{
									bool flag = binaryReader.ReadBoolean();
									if (flag)
									{
										ClothSettings clothSettings = this.CreateNewClothSettings();
										if (clothSettings != null)
										{
											ClothGeometryData clothGeometryData = new ClothGeometryData();
											if (!clothGeometryData.LoadFromBinaryReader(binaryReader))
											{
												this.Clear();
												return false;
											}
											clothGeometryData.IsProcessed = true;
											clothSettings.GeometryData = clothGeometryData;
										}
									}
								}
								else if (this.itemType == PresetManager.ItemType.HairFemale || this.itemType == PresetManager.ItemType.HairMale || this.itemType == PresetManager.ItemType.HairNeutral)
								{
									bool flag2 = binaryReader.ReadBoolean();
									if (flag2)
									{
										RuntimeHairGeometryCreator component2 = base.GetComponent<RuntimeHairGeometryCreator>();
										if (component2 != null)
										{
											component2.LoadFromBinaryReader(binaryReader);
										}
										HairSettings component3 = base.GetComponent<HairSettings>();
										if (component3 != null && component3.HairBuidCommand != null)
										{
											component3.HairBuidCommand.RebuildHair();
										}
									}
								}
							}
						}
					}
					catch (Exception ex3)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception while loading ",
							text,
							" ",
							ex3
						}));
						return false;
					}
					MaterialOptions[] components3 = this.binaryStorableBucket.GetComponents<MaterialOptions>();
					if (dazimport != null)
					{
						dazimport.ClearMaterialConnectors();
					}
					DAZSkinWrap component4 = this.binaryStorableBucket.GetComponent<DAZSkinWrap>();
					if (component4 != null)
					{
						component4.draw = true;
						if (dazimport != null)
						{
						}
						DAZSkinWrapControl component5 = this.binaryStorableBucket.GetComponent<DAZSkinWrapControl>();
						if (component5 != null)
						{
							component5.wrap = component4;
						}
					}
					else if (dazmesh != null)
					{
						dazmesh.createMeshFilterAndRenderer = true;
						if (dazimport != null)
						{
						}
					}
					if (dazimport != null)
					{
						string text7 = null;
						DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(storeFolderPath, false);
						if (directoryEntry != null)
						{
							text7 = directoryEntry.Path;
						}
						for (int k = 0; k < components3.Length; k++)
						{
							components3[k].exclude = createWithExclude;
							string tabName;
							if (components3[k].overrideId != null && components3[k].overrideId != string.Empty)
							{
								tabName = Regex.Replace(components3[k].overrideId, "^\\+parent", string.Empty);
								tabName = Regex.Replace(components3[k].overrideId, "^\\+Material", string.Empty);
							}
							else if (components3.Length == 1)
							{
								tabName = "Combined";
							}
							else
							{
								tabName = "Combined" + (k + 1).ToString();
							}
							dazimport.CreateMaterialOptionsUI(components3[k], tabName);
							components3[k].SetStartingValues();
							if (text7 != null)
							{
								components3[k].SetCustomTextureFolder(text7);
							}
						}
					}
					if (this.includeChildrenMaterialOptions)
					{
						base.SyncMaterialOptions();
					}
					DAZClothSettingsSimTextureReloader component6 = base.GetComponent<DAZClothSettingsSimTextureReloader>();
					if (component6 != null)
					{
						component6.SyncSkinWrapMaterialOptions();
					}
					FileManager.PushLoadDirFromFilePath(text3, false);
					base.RefreshStorables();
					base.RestoreStorables(asObject2);
					FileManager.PopLoadDir();
					this.SetDefaultsFromCurrent();
					return true;
				}
				SuperController.LogError("Could not load binary store " + text);
				this.Clear();
				return false;
			}
			SuperController.LogError("Not ready for load. Invalid load path or params");
			return false;
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x00179C94 File Offset: 0x00178094
		protected override void Awake()
		{
			base.Awake();
			if (this.binaryStorableBucket == null)
			{
				this.binaryStorableBucket = base.transform;
			}
		}

		// Token: 0x040037AC RID: 14252
		public string displayName;

		// Token: 0x040037AD RID: 14253
		protected string _uid;

		// Token: 0x040037AE RID: 14254
		protected string[] _tagsArray;

		// Token: 0x040037AF RID: 14255
		protected string _tags = string.Empty;

		// Token: 0x040037B0 RID: 14256
		public Transform binaryStorableBucket;

		// Token: 0x040037B1 RID: 14257
		public bool allowZeroBinaryStorables;

		// Token: 0x040037B2 RID: 14258
		public bool isRealItem = true;
	}
}
