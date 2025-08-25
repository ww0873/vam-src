using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using GPUTools.Cloth.Scripts;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Hair.Scripts;
using GPUTools.Hair.Scripts.Geometry.Create;
using GPUTools.Skinner.Scripts.Providers;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000AFB RID: 2811
	public class DAZRuntimeCreator : PresetManagerControl
	{
		// Token: 0x06004C13 RID: 19475 RVA: 0x001AA4EF File Offset: 0x001A88EF
		public DAZRuntimeCreator()
		{
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x001AA520 File Offset: 0x001A8920
		protected void ClearParams()
		{
			this.dufFileJSON.val = string.Empty;
			this.storeBrowsePathJSON.val = string.Empty;
			this.storeFolderNameJSON.val = string.Empty;
			this.storeNameJSON.val = string.Empty;
			this.packageNameJSON.val = string.Empty;
			this.storedCreatorNameJSON.val = string.Empty;
			this.displayNameJSON.val = string.Empty;
			this.tagsJSON.val = string.Empty;
			if (this.clothSimTextureFileJSON != null)
			{
				this.clothSimTextureFileJSON.val = string.Empty;
			}
			if (this.importMessageText != null)
			{
				this.importMessageText.text = "Select DUF scene file to import";
			}
			this.ResetCreatorName();
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x001AA5F0 File Offset: 0x001A89F0
		protected void ClearObjects()
		{
			if (this.dd != null)
			{
				this.dd.Clear();
			}
			if (this.hairCreator != null)
			{
				this.hairCreator.Clear();
			}
			DAZMorphBank component = base.GetComponent<DAZMorphBank>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DAZMorphSubBank component2 = transform.GetComponent<DAZMorphSubBank>();
					if (component2 != null)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
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
			if (this.importMessageText != null)
			{
				this.importMessageText.text = string.Empty;
			}
			if (this.importVertexCountText != null)
			{
				this.importVertexCountText.text = string.Empty;
			}
			if (this.simVertexCountText != null)
			{
				this.simVertexCountText.text = string.Empty;
			}
			if (this.simJointCountText != null)
			{
				this.simJointCountText.text = string.Empty;
			}
			if (this.simNearbyJointCountText != null)
			{
				this.simNearbyJointCountText.text = string.Empty;
			}
			this.SyncHairCountTexts();
			this.SyncOtherTags();
			this.dataCreatedFromImport = false;
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x001AA778 File Offset: 0x001A8B78
		protected void ClearAll()
		{
			this.isWrapping = false;
			base.StopAllCoroutines();
			this.AbortProcessGeometryThreaded(true);
			this.isGeneratingClothSim = false;
			this.ClearObjects();
			this.ClearParams();
			this.SyncOtherTags();
			this.SyncUI();
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x001AA7AD File Offset: 0x001A8BAD
		protected void Cancel()
		{
			this.ClearAll();
			this.SetCreatorStatus("Cancelled");
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x001AA7C0 File Offset: 0x001A8BC0
		protected void ImportCallback()
		{
			this.SetCreatorStatus(this.di.importStatus);
			if (this.importVertexCountText != null)
			{
				DAZMesh component = this.di.GetComponent<DAZMesh>();
				if (component != null)
				{
					int numUVVertices = component.numUVVertices;
					this.importVertexCountText.text = numUVVertices.ToString();
					if (numUVVertices > 50000)
					{
						this.importMessageText.text = "Vertex count very high. Recommend decimation (<50000 for wrap and <25000 for sim) & reimport";
						this.importVertexCountText.color = Color.red;
					}
					else if (numUVVertices > 25000)
					{
						this.importMessageText.text = "Vertex count high. Recommend decimation (<50000 for wrap and <25000 for sim) & reimport";
						this.importVertexCountText.color = Color.yellow;
					}
					else
					{
						this.importMessageText.text = "Import complete. Vertex count in range.";
						this.importVertexCountText.color = Color.green;
					}
				}
			}
			this.SyncSimTextureLoadedHandlers();
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x001AA8AC File Offset: 0x001A8CAC
		protected void ImportDuf()
		{
			if (this.di != null)
			{
				this.ClearObjects();
				DAZCharacterRun componentInParent = base.GetComponentInParent<DAZCharacterRun>();
				if (componentInParent != null)
				{
					componentInParent.doSetMergedVerts = true;
				}
				if (this.IsHair() && this.customScalpChoiceName != null)
				{
					this.scalpChooserJSON.val = this.customScalpChoiceName;
				}
				base.StartCoroutine(this.di.ImportDufCo(new DAZImport.ImportCallback(this.ImportCallback)));
				this.dataCreatedFromImport = true;
			}
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x001AA938 File Offset: 0x001A8D38
		protected void DufFileBeginBrowse(JSONStorableUrl jsurl)
		{
			if (this.di != null)
			{
				string defaultDAZContentPath = this.di.GetDefaultDAZContentPath();
				List<ShortCut> list = new List<ShortCut>();
				if (defaultDAZContentPath != null && defaultDAZContentPath != string.Empty)
				{
					ShortCut shortCut = new ShortCut();
					shortCut.path = defaultDAZContentPath.Replace('/', '\\');
					shortCut.displayName = shortCut.path;
					list.Add(shortCut);
				}
				if (this.di.registryDAZLibraryDirectories != null)
				{
					foreach (string text in this.di.registryDAZLibraryDirectories)
					{
						if (text != defaultDAZContentPath)
						{
							ShortCut shortCut = new ShortCut();
							shortCut.path = text.Replace('/', '\\');
							shortCut.displayName = shortCut.path;
							list.Add(shortCut);
						}
					}
				}
				jsurl.shortCuts = list;
			}
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x001AAA44 File Offset: 0x001A8E44
		protected void SyncDufFile(string url)
		{
			this._dufStoreName = null;
			if (url != null && url != string.Empty)
			{
				this._dufStoreName = Regex.Replace(url, ".*/", string.Empty);
				this._dufStoreName = Regex.Replace(this._dufStoreName, "\\.duf", string.Empty);
			}
			this.SyncStoreFolderNameToDuf();
			this.SyncStoreNameToDuf();
			if (this.di != null)
			{
				this.di.DAZSceneDufFile = url;
			}
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x001AAAC8 File Offset: 0x001A8EC8
		protected void SyncCombineMaterials(bool b)
		{
			if (this.di != null)
			{
				this.di.combineMaterials = b;
			}
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x001AAAE7 File Offset: 0x001A8EE7
		protected void SyncWrapToMorphedVertices(bool b)
		{
			if (this.di != null)
			{
				this.di.wrapToMorphedVertices = b;
			}
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x001AAB06 File Offset: 0x001A8F06
		public bool IsClothing()
		{
			return this.dd.itemType == PresetManager.ItemType.ClothingFemale || this.dd.itemType == PresetManager.ItemType.ClothingMale || this.dd.itemType == PresetManager.ItemType.ClothingNeutral;
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x001AAB40 File Offset: 0x001A8F40
		protected void SyncClothSimTextures(ClothSettings cs)
		{
			if (cs != null)
			{
				cs.EditorTexture = this.clothSimTexture;
				if (this.clothSimUseIndividualSimTextures)
				{
					cs.EditorType = ClothEditorType.Provider;
				}
				else if (this.clothSimTexture == null)
				{
					cs.EditorType = ClothEditorType.None;
				}
				else
				{
					cs.EditorType = ClothEditorType.Texture;
				}
				if (cs.GeometryData != null)
				{
					cs.GeometryData.ResetParticlesBlend();
				}
				if (cs.builder != null)
				{
					if (cs.builder.physicsBlend != null)
					{
						cs.builder.physicsBlend.UpdateSettings();
					}
					cs.Reset();
				}
			}
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x001AABE8 File Offset: 0x001A8FE8
		protected void SetClothSimTexture(Texture2D tex)
		{
			this.clothSimTexture = tex;
			ClothSettings component = base.GetComponent<ClothSettings>();
			this.SyncClothSimTextures(component);
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x001AAC0A File Offset: 0x001A900A
		protected void LoadSimTextureCallback(ImageLoaderThreaded.QueuedImage qi)
		{
			this.SetClothSimTexture(qi.tex);
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x001AAC18 File Offset: 0x001A9018
		protected void BeginClothSimTextureBrowse(JSONStorableUrl jsurl)
		{
			string storeFolderPath = this.dd.GetStoreFolderPath(false);
			if (!FileManager.IsDirectoryInPackage(storeFolderPath) && !storeFolderPath.Contains(":") && FileManager.IsSecureWritePath(storeFolderPath) && !FileManager.DirectoryExists(storeFolderPath, false, false))
			{
				FileManager.CreateDirectory(storeFolderPath);
			}
			jsurl.suggestedPath = storeFolderPath;
			jsurl.shortCuts = FileManager.GetShortCutsForDirectory(storeFolderPath, false, false, false, false);
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x001AAC84 File Offset: 0x001A9084
		protected void SyncClothSimTextureFile(string url)
		{
			if (FileManager.FileExists(url, false, false))
			{
				if (this.clothSimTextureRawImage != null)
				{
					this.LoadThumbnailImage(url, this.clothSimTextureRawImage, true);
				}
				ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
				queuedImage.imgPath = url;
				queuedImage.callback = new ImageLoaderThreaded.ImageLoaderCallback(this.LoadSimTextureCallback);
				queuedImage.forceReload = true;
				ImageLoaderThreaded.singleton.QueueImage(queuedImage);
			}
			else
			{
				this.SetClothSimTexture(null);
				this.clothSimTextureRawImage.texture = null;
			}
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x001AAD08 File Offset: 0x001A9108
		protected void SyncClothSimUseIndividualSimTextures(bool b)
		{
			this.clothSimUseIndividualSimTextures = b;
			ClothSettings component = base.GetComponent<ClothSettings>();
			this.SyncClothSimTextures(component);
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x001AAD2C File Offset: 0x001A912C
		protected void SyncSimTextureLoadedHandlers()
		{
			DAZSkinWrapMaterialOptions[] components = base.GetComponents<DAZSkinWrapMaterialOptions>();
			foreach (DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions in components)
			{
				DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions2 = dazskinWrapMaterialOptions;
				dazskinWrapMaterialOptions2.simTextureLoadedHandlers = (DAZSkinWrapMaterialOptions.SimTextureLoaded)Delegate.Combine(dazskinWrapMaterialOptions2.simTextureLoadedHandlers, new DAZSkinWrapMaterialOptions.SimTextureLoaded(this.IndividualSimTextureUpdated));
			}
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x001AAD7C File Offset: 0x001A917C
		protected void IndividualSimTextureUpdated()
		{
			if (this.clothSimUseIndividualSimTextures)
			{
				ClothSettings component = base.GetComponent<ClothSettings>();
				this.SyncClothSimTextures(component);
			}
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x001AADA4 File Offset: 0x001A91A4
		protected void SetUniformClothSimTexture(float uniformVal)
		{
			int num = 4;
			int num2 = 4;
			Color color = new Color(uniformVal, 0f, 0f);
			Texture2D texture2D = new Texture2D(num, num2);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					texture2D.SetPixel(i, j, color);
				}
			}
			texture2D.Apply();
			this.SetClothSimTexture(texture2D);
			this.clothSimTextureRawImage.texture = texture2D;
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x001AAE1F File Offset: 0x001A921F
		protected void SetUniformClothSimTexture()
		{
			if (this.uniformClothSimTextureValueJSON != null)
			{
				this.SetUniformClothSimTexture(this.uniformClothSimTextureValueJSON.val);
			}
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001AAE40 File Offset: 0x001A9240
		protected void SyncClothSimCreateNearbyJoints(bool b)
		{
			ClothSettings component = base.GetComponent<ClothSettings>();
			if (component != null)
			{
				component.CreateNearbyJoints = b;
			}
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x001AAE68 File Offset: 0x001A9268
		protected void SyncClothSimNearbyJointsDistance(float f)
		{
			ClothSettings component = base.GetComponent<ClothSettings>();
			if (component != null)
			{
				component.NearbyJointsMaxDistance = f;
			}
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x001AAE90 File Offset: 0x001A9290
		protected void AbortProcessGeometryThreaded(bool wait = true)
		{
			if (this.processGeometryThread != null && this.processGeometryThread.IsAlive)
			{
				this.clothSettingsForThread.CancelProcessGeometryThreaded();
				if (wait)
				{
					while (this.processGeometryThread.IsAlive)
					{
						Thread.Sleep(0);
					}
				}
			}
			this.abortGenerateClothSim = true;
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x001AAEEC File Offset: 0x001A92EC
		protected void ProcessGeometryThreaded()
		{
			try
			{
				this.clothSettingsForThread.ProcessGeometryThreaded();
			}
			catch (ThreadAbortException arg)
			{
				UnityEngine.Debug.LogError("Thread aborted " + arg);
			}
			catch (Exception arg2)
			{
				this.threadError = "Exception on thread while generating sim data " + arg2;
			}
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x001AAF54 File Offset: 0x001A9354
		protected IEnumerator GenerateClothSimCo(ClothSettings cs)
		{
			yield return null;
			if (cs != null)
			{
				if (this.simVertexCountText != null)
				{
					this.simVertexCountText.text = string.Empty;
				}
				if (this.simJointCountText != null)
				{
					this.simJointCountText.text = string.Empty;
				}
				if (this.simNearbyJointCountText != null)
				{
					this.simNearbyJointCountText.text = string.Empty;
				}
				this.SetCreatorStatus("Process Main Thread Sim Data Creation");
				cs.ProcessGeometryMainThread();
				this.SetCreatorStatus("Starting Sim Data Creation Thread");
				this.clothSettingsForThread = cs;
				this.threadError = null;
				this.processGeometryThread = new Thread(new ThreadStart(this.ProcessGeometryThreaded));
				this.processGeometryThread.Start();
				if (this.abortGenerateClothSim)
				{
					this.SetCreatorStatus("Sim Data Creation Aborted");
					this.isGeneratingClothSim = false;
					yield break;
				}
				while (this.processGeometryThread.IsAlive)
				{
					if (cs.GeometryData != null && cs.GeometryData.status != null && cs.GeometryData.status != string.Empty)
					{
						this.SetCreatorStatus(cs.GeometryData.status);
					}
					yield return null;
				}
				if (this.threadError != null)
				{
					this.SetCreatorStatus(this.threadError);
					this.isGeneratingClothSim = false;
					yield break;
				}
				if (this.abortGenerateClothSim)
				{
					this.SetCreatorStatus("Sim Data Creation Aborted");
					this.isGeneratingClothSim = false;
					yield break;
				}
				if (cs.GeometryData != null && cs.GeometryData.Particles != null)
				{
					if (this.simVertexCountText != null)
					{
						int num = cs.GeometryData.Particles.Length;
						this.simVertexCountText.text = num.ToString();
						if (num > 25000)
						{
							this.simVertexCountText.color = Color.red;
						}
						else
						{
							this.simVertexCountText.color = Color.green;
						}
					}
					if (this.simJointCountText != null)
					{
						IEnumerable<Int2ListContainer> stiffnessJointGroups = cs.GeometryData.StiffnessJointGroups;
						if (DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache0 == null)
						{
							DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache0 = new Func<Int2ListContainer, int>(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>m__0);
						}
						int num2 = stiffnessJointGroups.Sum(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache0);
						this.simJointCountText.text = num2.ToString();
					}
					if (this.simNearbyJointCountText != null)
					{
						IEnumerable<Int2ListContainer> nearbyJointGroups = cs.GeometryData.NearbyJointGroups;
						if (DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache1 == null)
						{
							DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache1 = new Func<Int2ListContainer, int>(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>m__1);
						}
						int num3 = nearbyJointGroups.Sum(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache1);
						this.simNearbyJointCountText.text = num3.ToString();
					}
					this.SetCreatorStatus("Sim Data Creation Finished");
				}
				else
				{
					this.SetCreatorStatus("Sim Data Creation Failed");
				}
				this.SyncClothSimTextures(cs);
			}
			this.isGeneratingClothSim = false;
			yield break;
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x001AAF78 File Offset: 0x001A9378
		protected void GenerateClothSim()
		{
			if (!this.isGeneratingClothSim && this.dd != null)
			{
				this.isGeneratingClothSim = true;
				this.abortGenerateClothSim = false;
				ClothSettings clothSettings = this.dd.CreateNewClothSettings();
				clothSettings.CreateNearbyJoints = this.clothSimCreateNearbyJointsJSON.val;
				clothSettings.NearbyJointsMaxDistance = this.clothSimNearbyJointsDistanceJSON.val;
				base.StartCoroutine(this.GenerateClothSimCo(clothSettings));
			}
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x001AAFEB File Offset: 0x001A93EB
		protected void CancelGenerateClothSim()
		{
			this.AbortProcessGeometryThreaded(true);
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x001AAFF4 File Offset: 0x001A93F4
		public bool IsHair()
		{
			return this.hairCreator != null;
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x001AB00C File Offset: 0x001A940C
		protected void SyncHairCountTexts()
		{
			if (this.IsHair())
			{
				if (this.simVertexCountText != null)
				{
					int[] hairRootToScalpMap = this.hairCreator.GetHairRootToScalpMap();
					if (hairRootToScalpMap != null)
					{
						this.simVertexCountText.text = hairRootToScalpMap.Length.ToString();
					}
					else
					{
						this.simVertexCountText.text = "0";
					}
				}
				if (this.simJointCountText != null)
				{
					List<Vector3> vertices = this.hairCreator.GetVertices();
					if (vertices != null)
					{
						this.simJointCountText.text = vertices.Count.ToString();
					}
					else
					{
						this.simJointCountText.text = "0";
					}
				}
				HairSimControl component = base.GetComponent<HairSimControl>();
				if (component != null)
				{
					component.SyncStyleText();
				}
			}
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x001AB0EC File Offset: 0x001A94EC
		public void Rebuild()
		{
			HairSettings component = base.GetComponent<HairSettings>();
			if (component != null && component.HairBuidCommand != null)
			{
				component.HairBuidCommand.RebuildHair();
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06004C33 RID: 19507 RVA: 0x001AB122 File Offset: 0x001A9522
		// (set) Token: 0x06004C34 RID: 19508 RVA: 0x001AB12A File Offset: 0x001A952A
		public ObjectChoice CurrentScalpChoice
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentScalpChoice>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CurrentScalpChoice>k__BackingField = value;
			}
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x001AB134 File Offset: 0x001A9534
		protected void InitScalpChoices()
		{
			this.scalpChoiceNames = new List<string>();
			this.scalpChoices = base.GetComponentsInChildren<ObjectChoice>(true);
			this.scalpChoiceNameToObjectChoice = new Dictionary<string, ObjectChoice>();
			foreach (ObjectChoice objectChoice in this.scalpChoices)
			{
				this.scalpChoiceNames.Add(objectChoice.displayName);
				this.scalpChoiceNameToObjectChoice.Add(objectChoice.displayName, objectChoice);
				if (objectChoice.gameObject.activeSelf)
				{
					this.CurrentScalpChoice = objectChoice;
					this.startingScalpChoiceName = this.CurrentScalpChoice.displayName;
				}
			}
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x001AB1D0 File Offset: 0x001A95D0
		protected ObjectChoice SyncScalpChoiceGameObject(string s)
		{
			ObjectChoice objectChoice;
			if (this.scalpChoiceNameToObjectChoice.TryGetValue(s, out objectChoice))
			{
				if (this.CurrentScalpChoice != null)
				{
					this.CurrentScalpChoice.gameObject.SetActive(false);
				}
				this.CurrentScalpChoice = objectChoice;
				objectChoice.gameObject.SetActive(false);
				objectChoice.gameObject.SetActive(true);
				return objectChoice;
			}
			return null;
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x001AB234 File Offset: 0x001A9634
		protected void SyncScalpChoice(string s)
		{
			ObjectChoice objectChoice = this.SyncScalpChoiceGameObject(s);
			if (objectChoice != null)
			{
				PreCalcMeshProvider component = objectChoice.GetComponent<PreCalcMeshProvider>();
				this.hairCreator.ScalpProvider = component;
				this.Rebuild();
			}
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x001AB26E File Offset: 0x001A966E
		protected void SyncSegments(float f)
		{
			if (this.IsHair())
			{
				this.hairCreator.Segments = (int)f;
				this.Rebuild();
			}
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x001AB28E File Offset: 0x001A968E
		protected void SyncSegmentsLength(float f)
		{
			if (this.IsHair())
			{
				this.hairCreator.SegmentLength = f;
			}
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x001AB2A8 File Offset: 0x001A96A8
		protected void SyncScalpMaskSelectableSize(float f)
		{
			if (this.createdSelectables != null)
			{
				foreach (Selectable selectable in this.createdSelectables)
				{
					GameObject gameObject = selectable.gameObject;
					gameObject.transform.localScale = Vector3.one * f;
				}
			}
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x001AB328 File Offset: 0x001A9728
		protected void ClearSelectables()
		{
			if (this.createdSelectables != null)
			{
				Selector.RemoveAll();
				foreach (Selectable selectable in this.createdSelectables)
				{
					GameObject gameObject = selectable.gameObject;
					UnityEngine.Object.Destroy(gameObject);
				}
				Selector.hideBackfaces = false;
				this.createdSelectables = null;
			}
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x001AB3A8 File Offset: 0x001A97A8
		protected void SelectionChanged(int uid, bool b)
		{
			if (this.hairCreator != null)
			{
				this.hairCreator.strandsMaskWorking.vertices[uid] = !b;
			}
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x001AB3D4 File Offset: 0x001A97D4
		protected void SyncSelectables()
		{
			if (this.IsHair() && this.createdSelectables != null)
			{
				bool[] vertices = this.hairCreator.strandsMaskWorking.vertices;
				foreach (Selectable selectable in this.createdSelectables)
				{
					selectable.isSelected = !vertices[selectable.id];
				}
			}
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x001AB464 File Offset: 0x001A9864
		protected void CreateSelectables()
		{
			if (this.IsHair() && this.selectablePrefab != null)
			{
				this.createdSelectables = new List<Selectable>();
				bool[] vertices = this.hairCreator.strandsMaskWorking.vertices;
				bool[] enabledIndices = this.hairCreator.enabledIndices;
				Vector3[] vertices2 = this.hairCreator.ScalpProvider.Mesh.vertices;
				Vector3[] normals = this.hairCreator.ScalpProvider.Mesh.normals;
				for (int i = 0; i < vertices.Length; i++)
				{
					if (enabledIndices[i])
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectablePrefab);
						transform.localScale = Vector3.one * this.scalpMaskSelectableSizeJSON.val;
						Selectable component = transform.GetComponent<Selectable>();
						if (component != null)
						{
							this.createdSelectables.Add(component);
							component.id = i;
							component.isSelected = !vertices[i];
							Selectable selectable = component;
							selectable.selectionChanged = (Selectable.SelectionChanged)Delegate.Combine(selectable.selectionChanged, new Selectable.SelectionChanged(this.SelectionChanged));
						}
						transform.position = vertices2[i];
						transform.LookAt(transform.position + normals[i]);
					}
				}
				Selector.hideBackfaces = this._scalpMaskEditModeHideBackfaces;
			}
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x001AB5CC File Offset: 0x001A99CC
		protected void SyncScalpMaskEditModeHideBackfaces(bool b)
		{
			this._scalpMaskEditModeHideBackfaces = b;
			if (this.scalpMaskEditMode)
			{
				Selector.hideBackfaces = this._scalpMaskEditModeHideBackfaces;
			}
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x001AB5EC File Offset: 0x001A99EC
		protected void SetHairRenderEnabled(bool b)
		{
			HairSettings component = base.GetComponent<HairSettings>();
			if (component != null && component.HairBuidCommand != null && component.HairBuidCommand.render != null)
			{
				MeshRenderer component2 = component.HairBuidCommand.render.GetComponent<MeshRenderer>();
				if (component2 != null)
				{
					component2.enabled = b;
				}
			}
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x001AB654 File Offset: 0x001A9A54
		protected void SyncScalpMaskButtons()
		{
			if (this.startScalpMaskEditModeAction.button != null)
			{
				this.startScalpMaskEditModeAction.button.interactable = !this.scalpMaskEditMode;
			}
			if (this.cancelScalpMaskEditModeAction.button != null)
			{
				this.cancelScalpMaskEditModeAction.button.interactable = this.scalpMaskEditMode;
			}
			if (this.finishScalpMaskEditModeAction.button != null)
			{
				this.finishScalpMaskEditModeAction.button.interactable = this.scalpMaskEditMode;
			}
			if (this.scalpMaskClearAllAction.button != null)
			{
				this.scalpMaskClearAllAction.button.interactable = this.scalpMaskEditMode;
			}
			if (this.scalpMaskSetAllAction.button != null)
			{
				this.scalpMaskSetAllAction.button.interactable = this.scalpMaskEditMode;
			}
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x001AB740 File Offset: 0x001A9B40
		protected void SyncScalpMaskTool()
		{
			HairSimControlTools componentInParent = base.GetComponentInParent<HairSimControlTools>();
			if (componentInParent != null)
			{
				componentInParent.SetScalpMaskToolVisible(this.scalpMaskEditMode);
				componentInParent.SetOnlyToolsControllable(this.scalpMaskEditMode);
			}
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x001AB778 File Offset: 0x001A9B78
		public void ScalpMaskClearAll()
		{
			if (this.IsHair())
			{
				this.hairCreator.MaskClearAll();
				this.SyncSelectables();
			}
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x001AB796 File Offset: 0x001A9B96
		public void ScalpMaskSetAll()
		{
			if (this.IsHair())
			{
				this.hairCreator.MaskSetAll();
				this.SyncSelectables();
			}
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x001AB7B4 File Offset: 0x001A9BB4
		public void StartScalpMaskEditMode()
		{
			if (this.IsHair())
			{
				this.scalpMaskEditMode = true;
				this.SyncScalpMaskButtons();
				this.SyncScalpMaskTool();
				this.SetHairRenderEnabled(false);
				DAZSkinV2.staticDraw = true;
				DAZSkinWrap.staticDraw = true;
				this.ClearSelectables();
				this.hairCreator.SetWorkingMaskToCurrentMask();
				this.CreateSelectables();
				Selector.Activate();
				HairSimControl component = base.GetComponent<HairSimControl>();
				if (component != null)
				{
					component.CancelStyleMode();
				}
				if (SuperController.singleton != null)
				{
					SuperController.singleton.SelectModeCustomWithVRTargetControl("Select scalp vertices that will have hair strands. White = Has Strand. Red = No Strand. Mouse: Click - Toggles vertex. Click+Drag - Add. LeftCtrl+Click+Drag - Remove. VR: Grab Mask Tool, Select Mode, and Move Over Scalp");
				}
			}
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x001AB848 File Offset: 0x001A9C48
		public void CancelScalpMaskEditMode()
		{
			if (this.IsHair())
			{
				this.ClearSelectables();
				Selector.Deactivate();
				this.scalpMaskEditMode = false;
				this.SyncScalpMaskButtons();
				this.SyncScalpMaskTool();
				this.SetHairRenderEnabled(true);
				DAZSkinV2.staticDraw = false;
				DAZSkinWrap.staticDraw = false;
				if (SuperController.singleton != null)
				{
					SuperController.singleton.SelectModeOff();
				}
			}
		}

		// Token: 0x06004C47 RID: 19527 RVA: 0x001AB8AC File Offset: 0x001A9CAC
		public void FinishScalpMaskEditMode()
		{
			if (this.IsHair())
			{
				this.ClearSelectables();
				Selector.Deactivate();
				this.scalpMaskEditMode = false;
				this.SyncScalpMaskButtons();
				this.SyncScalpMaskTool();
				this.SetHairRenderEnabled(true);
				DAZSkinV2.staticDraw = false;
				DAZSkinWrap.staticDraw = false;
				this.hairCreator.ApplyMaskChanges();
				HairSimControl component = base.GetComponent<HairSimControl>();
				if (component != null)
				{
					component.ClearStyleJoints();
				}
				this.Rebuild();
				if (SuperController.singleton != null)
				{
					SuperController.singleton.SelectModeOff();
				}
			}
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x001AB93C File Offset: 0x001A9D3C
		protected void GenerateHairSim()
		{
			if (!this.isGeneratingHairSim && this.dd != null)
			{
				this.abortGenerateHairSim = false;
				this.hairCreator.ClearAllStrands();
				this.hairCreator.GenerateAll();
				this.SyncHairCountTexts();
				this.Rebuild();
				HairSimControl component = base.GetComponent<HairSimControl>();
				if (component != null)
				{
					component.StartStyleMode(false);
				}
			}
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x001AB9A8 File Offset: 0x001A9DA8
		protected void CancelGenerateHairSim()
		{
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x001AB9AC File Offset: 0x001A9DAC
		protected void SyncDazImportMaterialFolder()
		{
			if (this.di != null)
			{
				string text = string.Empty;
				if (this.creatorNameJSON != null && this.creatorNameJSON.val != null && this.creatorNameJSON.val != string.Empty)
				{
					text = text + this.creatorNameJSON.val + "/";
				}
				if (this.storeFolderNameJSON != null && this.storeFolderNameJSON.val != null && this.storeFolderNameJSON.val != string.Empty)
				{
					text += this.storeFolderNameJSON.val;
				}
				if (text != string.Empty)
				{
					this.di.MaterialOverrideFolderName = text;
					this.di.overrideMaterialFolderName = true;
				}
				else
				{
					this.di.overrideMaterialFolderName = false;
				}
			}
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x001ABA9C File Offset: 0x001A9E9C
		protected void SyncStoreFolderNameToDuf()
		{
			if (this.autoSetStoreFolderNameFromDufJSON.val && this._dufStoreName != null && this._dufStoreName != string.Empty)
			{
				this.storeFolderNameJSON.val = this._dufStoreName;
			}
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x001ABAEC File Offset: 0x001A9EEC
		protected void SyncStoreFolderName(string sname)
		{
			this.SyncDazImportMaterialFolder();
			this.ClearPackage();
			if (this.dd != null)
			{
				this.dd.storeFolderName = sname;
				this.dd.SyncMaterialOptions();
			}
			base.SyncPresetUI();
			this.SyncStoreFolderNameToDuf();
			this.SyncUI();
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x001ABB3F File Offset: 0x001A9F3F
		protected void SyncAutoSetStoreFolderNameFromDuf(bool b)
		{
			this.SyncStoreFolderNameToDuf();
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x001ABB47 File Offset: 0x001A9F47
		protected void SyncUID()
		{
			if (this.dd != null)
			{
				this.dd.uid = this.dd.creatorName + ":" + this.dd.storeName;
			}
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x001ABB85 File Offset: 0x001A9F85
		protected void SyncPackageName(string pname)
		{
			if (this.dd != null)
			{
				this.dd.package = pname;
				this.dd.SyncMaterialOptions();
			}
			base.SyncPresetUI();
			this.SyncUI();
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x001ABBBB File Offset: 0x001A9FBB
		protected void ClearPackage()
		{
			this.packageNameJSON.val = string.Empty;
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x001ABBD0 File Offset: 0x001A9FD0
		protected void SyncStoreNameToDuf()
		{
			if (this.autoSetStoreNameFromDufJSON.val && this._dufStoreName != null && this._dufStoreName != string.Empty)
			{
				this.storeNameJSON.val = this._dufStoreName;
			}
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x001ABC1E File Offset: 0x001AA01E
		protected void SyncStoreName(string sname)
		{
			if (this.dd != null)
			{
				this.dd.storeName = sname;
			}
			this.SyncUID();
			base.SyncPresetUI();
			this.SyncStoreNameToDuf();
			this.SyncUI();
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x001ABC55 File Offset: 0x001AA055
		protected void SyncDisplayName(string dname)
		{
			if (this.dd != null)
			{
				this.dd.displayName = dname;
			}
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x001ABC74 File Offset: 0x001AA074
		protected void SyncAutoSetStoreNameFromDuf(bool b)
		{
			this.SyncStoreNameToDuf();
		}

		// Token: 0x17000AD4 RID: 2772
		// (set) Token: 0x06004C55 RID: 19541 RVA: 0x001ABC7C File Offset: 0x001AA07C
		protected bool dataCreatedFromImport
		{
			set
			{
				if (this.creatorNameJSON != null)
				{
					this.creatorNameJSON.interactable = !value;
				}
			}
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x001ABC98 File Offset: 0x001AA098
		protected void ResetCreatorName()
		{
			string text = "Anonymous";
			if (UserPreferences.singleton != null)
			{
				text = UserPreferences.singleton.creatorName;
			}
			if (this.creatorNameJSON != null)
			{
				this.creatorNameJSON.val = text;
			}
			else
			{
				this.SyncCreatorName(text);
			}
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x001ABCE9 File Offset: 0x001AA0E9
		protected void SyncCreatorName(string s)
		{
			this._creatorName = s;
			this.SyncDazImportMaterialFolder();
			if (this.dd != null)
			{
				this.dd.creatorName = s;
			}
			this.SyncUID();
			this.SyncUI();
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x001ABD21 File Offset: 0x001AA121
		protected void OpenTagsPanel()
		{
			if (this.tagsPanel != null)
			{
				DAZRuntimeCreator.tagsPanelOpen = true;
				this.tagsPanel.gameObject.SetActive(true);
			}
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x001ABD4B File Offset: 0x001AA14B
		protected void CloseTagsPanel()
		{
			if (this.tagsPanel != null)
			{
				DAZRuntimeCreator.tagsPanelOpen = false;
				this.tagsPanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004C5A RID: 19546 RVA: 0x001ABD78 File Offset: 0x001AA178
		protected void SyncOtherTags()
		{
			DAZClothingItemControl component = base.GetComponent<DAZClothingItemControl>();
			if (component != null)
			{
				this.otherTags = component.GetAllClothingOtherTags();
			}
			else
			{
				DAZHairGroupControl component2 = base.GetComponent<DAZHairGroupControl>();
				if (component2 != null)
				{
					this.otherTags = component2.GetAllHairOtherTags();
				}
			}
			this.SyncOtherTagsUI();
		}

		// Token: 0x06004C5B RID: 19547 RVA: 0x001ABDD0 File Offset: 0x001AA1D0
		protected void SyncTagsSetToTags()
		{
			if (this.tagsJSON != null && this.tagsJSON.val != string.Empty)
			{
				string[] collection = this.tagsJSON.val.Split(new char[]
				{
					','
				});
				this.tagsSet = new HashSet<string>(collection);
			}
			else
			{
				this.tagsSet = new HashSet<string>();
			}
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x001ABE3C File Offset: 0x001AA23C
		protected void SyncTagsToTagsSet()
		{
			string[] array = new string[this.tagsSet.Count];
			this.tagsSet.CopyTo(array);
			this.tagsJSON.valNoCallback = string.Join(",", array);
			this.SyncDDTagsToJSON();
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x001ABE82 File Offset: 0x001AA282
		protected void SyncTagFromToggle(string tag, bool isEnabled)
		{
			if (!this.ignoreTagFromToggleCallback)
			{
				if (isEnabled)
				{
					this.tagsSet.Add(tag);
				}
				else
				{
					this.tagsSet.Remove(tag);
				}
				this.SyncTagsToTagsSet();
			}
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x001ABEBC File Offset: 0x001AA2BC
		protected void SyncTagTogglesToTags()
		{
			this.ignoreTagFromToggleCallback = true;
			foreach (KeyValuePair<string, Toggle> keyValuePair in this.tagToToggle)
			{
				if (this.tagsSet.Contains(keyValuePair.Key))
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
			}
			this.ignoreTagFromToggleCallback = false;
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x001ABF7C File Offset: 0x001AA37C
		protected void CreateTagToggle(string tag, Transform parent)
		{
			DAZRuntimeCreator.<CreateTagToggle>c__AnonStorey2 <CreateTagToggle>c__AnonStorey = new DAZRuntimeCreator.<CreateTagToggle>c__AnonStorey2();
			<CreateTagToggle>c__AnonStorey.tag = tag;
			<CreateTagToggle>c__AnonStorey.$this = this;
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.tagTogglePrefab);
			Text componentInChildren = transform.GetComponentInChildren<Text>();
			componentInChildren.text = <CreateTagToggle>c__AnonStorey.tag;
			Toggle componentInChildren2 = transform.GetComponentInChildren<Toggle>();
			componentInChildren2.onValueChanged.AddListener(new UnityAction<bool>(<CreateTagToggle>c__AnonStorey.<>m__0));
			this.tagToToggle.Remove(<CreateTagToggle>c__AnonStorey.tag);
			this.tagToToggle.Add(<CreateTagToggle>c__AnonStorey.tag, componentInChildren2);
			transform.SetParent(parent, false);
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x001AC008 File Offset: 0x001AA408
		protected void SyncOtherTagsUI()
		{
			if (this.tagTogglePrefab != null && this.otherTagsContent != null)
			{
				IEnumerator enumerator = this.otherTagsContent.GetEnumerator();
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
					this.CreateTagToggle(tag, this.otherTagsContent);
				}
				this.SyncTagTogglesToTags();
			}
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x001AC0FC File Offset: 0x001AA4FC
		protected void InitTagsUI()
		{
			this.tagToToggle = new Dictionary<string, Toggle>();
			if (this.tagTogglePrefab != null)
			{
				if (this.regionTags != null && this.regionTagsContent != null)
				{
					List<string> list = new List<string>(this.regionTags);
					list.Sort();
					foreach (string tag in list)
					{
						this.CreateTagToggle(tag, this.regionTagsContent);
					}
				}
				if (this.pm != null)
				{
					if (this.pm.itemType == PresetManager.ItemType.ClothingFemale)
					{
						if (this.femaleTypeTags != null && this.typeTagsContent != null)
						{
							List<string> list2 = new List<string>(this.femaleTypeTags);
							list2.Sort();
							foreach (string tag2 in list2)
							{
								this.CreateTagToggle(tag2, this.typeTagsContent);
							}
						}
					}
					else if (this.pm.itemType == PresetManager.ItemType.ClothingMale && this.maleTypeTags != null && this.typeTagsContent != null)
					{
						List<string> list3 = new List<string>(this.maleTypeTags);
						list3.Sort();
						foreach (string tag3 in list3)
						{
							this.CreateTagToggle(tag3, this.typeTagsContent);
						}
					}
				}
				this.SyncOtherTagsUI();
			}
			this.SyncTagTogglesToTags();
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x001AC2E4 File Offset: 0x001AA6E4
		protected void SyncDDTagsToJSON()
		{
			if (this.dd != null)
			{
				this.dd.tags = this.tagsJSON.val;
			}
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x001AC310 File Offset: 0x001AA710
		protected void SyncTagsFromJSON(string tags)
		{
			string text = tags.Trim();
			text = Regex.Replace(text, ",\\s+", ",");
			text = Regex.Replace(text, "\\s+,", ",");
			if (text != tags)
			{
				this.tagsJSON.valNoCallback = text;
			}
			this.SyncTagsSetToTags();
			this.SyncTagTogglesToTags();
			this.SyncDDTagsToJSON();
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x001AC370 File Offset: 0x001AA770
		protected void SyncBrowsePath(string url)
		{
			if (url != null && url != string.Empty)
			{
				string[] array = this.dd.PathToNames(url);
				this.storeFolderNameJSON.val = array[0];
				this.creatorNameJSON.val = array[1];
				this.storeNameJSON.val = array[2];
				this.packageNameJSON.val = array[3];
				this.Load();
			}
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x001AC3DE File Offset: 0x001AA7DE
		public void LoadFromPath(string path)
		{
			this.storeBrowsePathJSON.val = path;
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x001AC3EC File Offset: 0x001AA7EC
		protected void BeginBrowseCreator(JSONStorableUrl jsurl)
		{
			if (this.pm != null)
			{
				string storeRootPath = this.dd.GetStoreRootPath(false);
				string text = storeRootPath;
				text = Regex.Replace(text, "/$", string.Empty);
				jsurl.suggestedPath = text;
				List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(storeRootPath, false, false, true, true);
				jsurl.shortCuts = shortCutsForDirectory;
			}
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x001AC444 File Offset: 0x001AA844
		protected void Store()
		{
			if (this.dd != null)
			{
				bool flag = this.dd.CheckStoreExistance();
				DAZClothingItemControl component = base.GetComponent<DAZClothingItemControl>();
				DAZHairGroupControl component2 = base.GetComponent<DAZHairGroupControl>();
				bool flag2 = true;
				if (!flag)
				{
					if (component != null)
					{
						flag2 = component.IsClothingUIDAvailable(this.dd.uid);
					}
					else if (component2 != null)
					{
						flag2 = component2.IsHairItemUIDAvailable(this.dd.uid);
					}
				}
				if (flag2)
				{
					this.dd.isRealItem = true;
					if (component != null && component.clothingItem != null)
					{
						this.dd.isRealItem = component.clothingItem.isRealItem;
					}
					if (component2 != null && component2.hairItem != null)
					{
						this.dd.isRealItem = component2.hairItem.isRealItem;
					}
					if (this.dd.Store())
					{
						if (this.storedCreatorNameJSON != null)
						{
							this.storedCreatorNameJSON.val = this.dd.storedCreatorName;
						}
						this.SetCreatorStatus("Store to " + this.dd.storeName + " complete");
						base.SyncPresetUI();
						if (component != null)
						{
							component.RefreshClothingItems();
						}
						else if (component2 != null)
						{
							component2.RefreshHairItems();
						}
						this.SyncOtherTags();
					}
					else
					{
						string text = "Store to " + this.dd.storeName + " failed";
						SuperController.LogError(text);
						this.SetCreatorStatus(text);
					}
				}
				else
				{
					string text2 = string.Concat(new string[]
					{
						"Store to ",
						this.dd.storeName,
						" failed. UID ",
						this.dd.uid,
						" is already being used"
					});
					SuperController.LogError(text2);
					this.SetCreatorStatus(text2);
				}
				this.SyncUI();
			}
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x001AC650 File Offset: 0x001AAA50
		private IEnumerator LoadDelay()
		{
			yield return null;
			DAZClothingItemControl itemControl = base.GetComponent<DAZClothingItemControl>();
			if (itemControl != null)
			{
				itemControl.ResetIsRealClothingItem();
			}
			if (this.dd.Load(true))
			{
				if (this.storedCreatorNameJSON != null)
				{
					this.storedCreatorNameJSON.val = this.dd.storedCreatorName;
				}
				if (this.displayNameJSON != null)
				{
					this.displayNameJSON.valNoCallback = this.dd.displayName;
				}
				if (this.tagsJSON != null)
				{
					this.tagsJSON.valNoCallback = this.dd.tags;
					this.SyncTagsSetToTags();
					this.SyncTagTogglesToTags();
				}
				if (this.di != null && this.di.materialUIConnectorMaster != null)
				{
					this.di.materialUIConnectorMaster.Rebuild();
				}
				if (this.IsHair())
				{
					this.segmentsJSON.valNoCallback = (float)this.hairCreator.Segments;
					this.segmentsLengthJSON.valNoCallback = this.hairCreator.SegmentLength;
					this.scalpChooserJSON.valNoCallback = this.hairCreator.ScalpProviderName;
					this.SyncScalpChoiceGameObject(this.hairCreator.ScalpProviderName);
				}
				this.LoadThumbnailImage(this.dd.GetStorePathBase() + ".jpg");
				this.SetCreatorStatus("Load from " + this.dd.storeName + " complete");
				this.SyncSimTextureLoadedHandlers();
			}
			else
			{
				this.ClearObjects();
				this.SetCreatorStatus("Load from " + this.dd.storeName + " failed");
			}
			yield return null;
			yield break;
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x001AC66B File Offset: 0x001AAA6B
		protected void Load()
		{
			if (this.dd != null)
			{
				this.ClearObjects();
				base.StartCoroutine(this.LoadDelay());
			}
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x001AC691 File Offset: 0x001AAA91
		protected virtual void SetCreatorStatus(string status)
		{
			if (this.creatorStatusText != null)
			{
				this.creatorStatusText.text = status;
			}
			if (this.creatorStatusTextAlt != null)
			{
				this.creatorStatusTextAlt.text = status;
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x001AC6D0 File Offset: 0x001AAAD0
		protected void LoadThumbnailImage(string imgPath, RawImage rawImage, bool forceReload = false)
		{
			if (rawImage != null && FileManager.FileExists(imgPath, false, false))
			{
				ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
				queuedImage.imgPath = imgPath;
				queuedImage.width = 512;
				queuedImage.height = 512;
				queuedImage.setSize = true;
				queuedImage.fillBackground = true;
				queuedImage.forceReload = forceReload;
				queuedImage.rawImageToLoad = rawImage;
				ImageLoaderThreaded.singleton.QueueThumbnail(queuedImage);
			}
			DAZClothingItemControl component = base.GetComponent<DAZClothingItemControl>();
			if (component != null)
			{
				component.RefreshClothingItemThumbnail(imgPath);
			}
			else
			{
				DAZHairGroupControl component2 = base.GetComponent<DAZHairGroupControl>();
				if (component2 != null)
				{
					component2.RefreshHairItemThumbnail(imgPath);
				}
			}
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001AC779 File Offset: 0x001AAB79
		protected void LoadThumbnailImage(string imgPath)
		{
			this.LoadThumbnailImage(imgPath, this.thumbnailRawImage, false);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x001AC789 File Offset: 0x001AAB89
		protected void GetScreenshot()
		{
			if (this.dd != null)
			{
				this.dd.GetScreenshot(new SuperController.ScreenShotCallback(this.LoadThumbnailImage));
			}
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x001AC7B4 File Offset: 0x001AABB4
		protected override void Init()
		{
			base.Init();
			if (this.dazImport != null)
			{
				this.di = this.dazImport;
			}
			else
			{
				this.di = base.GetComponent<DAZImport>();
			}
			this.dd = base.GetComponent<DAZDynamic>();
			this.dd.ignoreExclude = true;
			string suggestPath = null;
			if (this.di != null)
			{
				this.di.SetRegistryLibPaths();
				suggestPath = this.di.GetDefaultDAZContentPath();
				if (this.IsHair())
				{
					this.di.combineToSingleMaterial = true;
				}
				else
				{
					this.di.combineMaterials = true;
				}
			}
			this.dufFileJSON = new JSONStorableUrl("dufFile", string.Empty, new JSONStorableString.SetStringCallback(this.SyncDufFile), "duf", suggestPath);
			this.dufFileJSON.suggestedPathGroup = "DAZRuntimeCreator";
			this.dufFileJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.DufFileBeginBrowse);
			this.dufFileJSON.isStorable = false;
			this.dufFileJSON.isRestorable = false;
			base.RegisterUrl(this.dufFileJSON);
			this.clearAction = new JSONStorableAction("Clear", new JSONStorableAction.ActionCallback(this.ClearAll));
			base.RegisterAction(this.clearAction);
			this.cancelAction = new JSONStorableAction("Cancel", new JSONStorableAction.ActionCallback(this.Cancel));
			base.RegisterAction(this.cancelAction);
			this.combineMaterialsJSON = new JSONStorableBool("combineMaterials", true, new JSONStorableBool.SetBoolCallback(this.SyncCombineMaterials));
			this.combineMaterialsJSON.isStorable = false;
			this.combineMaterialsJSON.isRestorable = false;
			base.RegisterBool(this.combineMaterialsJSON);
			this.wrapToMorphedVerticesJSON = new JSONStorableBool("wrapToMorphedVertices", false, new JSONStorableBool.SetBoolCallback(this.SyncWrapToMorphedVertices));
			this.wrapToMorphedVerticesJSON.isStorable = false;
			this.wrapToMorphedVerticesJSON.isRestorable = false;
			base.RegisterBool(this.wrapToMorphedVerticesJSON);
			this.importDufAction = new JSONStorableAction("Import", new JSONStorableAction.ActionCallback(this.ImportDuf));
			base.RegisterAction(this.importDufAction);
			if (this.IsClothing())
			{
				this.generateClothSimAction = new JSONStorableAction("CreateClothSim", new JSONStorableAction.ActionCallback(this.GenerateClothSim));
				base.RegisterAction(this.generateClothSimAction);
				this.cancelGenerateClothSimAction = new JSONStorableAction("CancelCreateClothSim", new JSONStorableAction.ActionCallback(this.CancelGenerateClothSim));
				base.RegisterAction(this.cancelGenerateClothSimAction);
				this.uniformClothSimTextureValueJSON = new JSONStorableFloat("uniformClothSimTextureValue", 0.95f, 0f, 1f, true, true);
				this.uniformClothSimTextureValueJSON.isStorable = false;
				this.uniformClothSimTextureValueJSON.isRestorable = false;
				base.RegisterFloat(this.uniformClothSimTextureValueJSON);
				this.setUniformClothSimTextureAction = new JSONStorableAction("SetUniformTexture", new JSONStorableAction.ActionCallback(this.SetUniformClothSimTexture));
				base.RegisterAction(this.setUniformClothSimTextureAction);
				this.clothSimUseIndividualSimTexturesJSON = new JSONStorableBool("clothSimUseIndividualSimTextures", this.clothSimUseIndividualSimTextures, new JSONStorableBool.SetBoolCallback(this.SyncClothSimUseIndividualSimTextures));
				this.clothSimUseIndividualSimTexturesJSON.isStorable = false;
				this.clothSimUseIndividualSimTexturesJSON.isRestorable = false;
				base.RegisterBool(this.clothSimUseIndividualSimTexturesJSON);
				this.clothSimCreateNearbyJointsJSON = new JSONStorableBool("clothSimCreateNearbyJoints", false, new JSONStorableBool.SetBoolCallback(this.SyncClothSimCreateNearbyJoints));
				this.clothSimCreateNearbyJointsJSON.isStorable = false;
				this.clothSimCreateNearbyJointsJSON.isRestorable = false;
				base.RegisterBool(this.clothSimCreateNearbyJointsJSON);
				this.clothSimNearbyJointsDistanceJSON = new JSONStorableFloat("clothSimNearbyJointsDistance", 0.002f, new JSONStorableFloat.SetFloatCallback(this.SyncClothSimNearbyJointsDistance), 0.0001f, 0.01f, true, true);
				this.clothSimNearbyJointsDistanceJSON.isStorable = false;
				this.clothSimNearbyJointsDistanceJSON.isRestorable = false;
				base.RegisterFloat(this.clothSimNearbyJointsDistanceJSON);
			}
			if (this.IsHair())
			{
				this.InitScalpChoices();
				this.scalpChooserJSON = new JSONStorableStringChooser("scalpChoice", this.scalpChoiceNames, this.startingScalpChoiceName, "Scalp Choice", new JSONStorableStringChooser.SetStringCallback(this.SyncScalpChoice));
				this.scalpChooserJSON.isStorable = false;
				this.scalpChooserJSON.isRestorable = false;
				base.RegisterStringChooser(this.scalpChooserJSON);
				this.generateHairSimAction = new JSONStorableAction("CreateHairSim", new JSONStorableAction.ActionCallback(this.GenerateHairSim));
				base.RegisterAction(this.generateHairSimAction);
				this.cancelGenerateHairSimAction = new JSONStorableAction("CancelCreateHairSim", new JSONStorableAction.ActionCallback(this.CancelGenerateHairSim));
				base.RegisterAction(this.cancelGenerateHairSimAction);
				this.segmentsJSON = new JSONStorableFloat("segments", (float)this.hairCreator.Segments, new JSONStorableFloat.SetFloatCallback(this.SyncSegments), 2f, 50f, true, true);
				this.segmentsJSON.isStorable = false;
				this.segmentsJSON.isRestorable = false;
				base.RegisterFloat(this.segmentsJSON);
				this.segmentsLengthJSON = new JSONStorableFloat("segmentsLength", this.hairCreator.SegmentLength, new JSONStorableFloat.SetFloatCallback(this.SyncSegmentsLength), 0.0001f, 0.03f, true, true);
				this.segmentsLengthJSON.isStorable = false;
				this.segmentsLengthJSON.isRestorable = false;
				base.RegisterFloat(this.segmentsLengthJSON);
				this.scalpMaskSelectableSizeJSON = new JSONStorableFloat("scalpMaskSelectableSize", 0.002f, new JSONStorableFloat.SetFloatCallback(this.SyncScalpMaskSelectableSize), 0.0002f, 0.004f, true, true);
				this.scalpMaskSelectableSizeJSON.isStorable = false;
				this.scalpMaskSelectableSizeJSON.isRestorable = false;
				base.RegisterFloat(this.scalpMaskSelectableSizeJSON);
				this.scalpMaskClearAllAction = new JSONStorableAction("ScalpMaskClearAll", new JSONStorableAction.ActionCallback(this.ScalpMaskClearAll));
				base.RegisterAction(this.scalpMaskClearAllAction);
				this.scalpMaskSetAllAction = new JSONStorableAction("ScalpMaskSetAll", new JSONStorableAction.ActionCallback(this.ScalpMaskSetAll));
				base.RegisterAction(this.scalpMaskSetAllAction);
				this.startScalpMaskEditModeAction = new JSONStorableAction("StartScalpMaskEditMode", new JSONStorableAction.ActionCallback(this.StartScalpMaskEditMode));
				base.RegisterAction(this.startScalpMaskEditModeAction);
				this.cancelScalpMaskEditModeAction = new JSONStorableAction("CancelScalpMaskEditMode", new JSONStorableAction.ActionCallback(this.CancelScalpMaskEditMode));
				base.RegisterAction(this.cancelScalpMaskEditModeAction);
				this.finishScalpMaskEditModeAction = new JSONStorableAction("FinishScalpMaskEditMode", new JSONStorableAction.ActionCallback(this.FinishScalpMaskEditMode));
				base.RegisterAction(this.finishScalpMaskEditModeAction);
				this.scalpMaskEditModeHideBackfacesJSON = new JSONStorableBool("ScalpMaskEditModeHideBackfaces", this._scalpMaskEditModeHideBackfaces, new JSONStorableBool.SetBoolCallback(this.SyncScalpMaskEditModeHideBackfaces));
				this.scalpMaskEditModeHideBackfacesJSON.isStorable = false;
				this.scalpMaskEditModeHideBackfacesJSON.isRestorable = false;
				base.RegisterBool(this.scalpMaskEditModeHideBackfacesJSON);
			}
			this.autoSetStoreFolderNameFromDufJSON = new JSONStorableBool("autoSetStoreFolderNameFromDuf", true, new JSONStorableBool.SetBoolCallback(this.SyncAutoSetStoreFolderNameFromDuf));
			this.autoSetStoreFolderNameFromDufJSON.isStorable = false;
			this.autoSetStoreFolderNameFromDufJSON.isRestorable = false;
			base.RegisterBool(this.autoSetStoreFolderNameFromDufJSON);
			this.storeFolderNameJSON = new JSONStorableString("storeFolderName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncStoreFolderName));
			this.storeFolderNameJSON.isStorable = false;
			this.storeFolderNameJSON.isRestorable = false;
			this.storeFolderNameJSON.enableOnChange = true;
			base.RegisterString(this.storeFolderNameJSON);
			this.autoSetStoreNameFromDufJSON = new JSONStorableBool("autoSetStoreNameFromDuf", true, new JSONStorableBool.SetBoolCallback(this.SyncAutoSetStoreNameFromDuf));
			this.autoSetStoreNameFromDufJSON.isStorable = false;
			this.autoSetStoreNameFromDufJSON.isRestorable = false;
			base.RegisterBool(this.autoSetStoreNameFromDufJSON);
			this.packageNameJSON = new JSONStorableString("packageName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPackageName));
			this.packageNameJSON.isStorable = false;
			this.packageNameJSON.isRestorable = false;
			base.RegisterString(this.packageNameJSON);
			this.clearPackageAction = new JSONStorableAction("ClearPackage", new JSONStorableAction.ActionCallback(this.ClearPackage));
			base.RegisterAction(this.clearPackageAction);
			this.storeNameJSON = new JSONStorableString("storeName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncStoreName));
			this.storeNameJSON.isStorable = false;
			this.storeNameJSON.isRestorable = false;
			this.storeNameJSON.enableOnChange = true;
			base.RegisterString(this.storeNameJSON);
			this.displayNameJSON = new JSONStorableString("displayName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncDisplayName));
			this.displayNameJSON.isStorable = false;
			this.displayNameJSON.isRestorable = false;
			base.RegisterString(this.displayNameJSON);
			this.ResetCreatorName();
			this.creatorNameJSON = new JSONStorableString("creatorName", this._creatorName, new JSONStorableString.SetStringCallback(this.SyncCreatorName));
			this.creatorNameJSON.isStorable = false;
			this.creatorNameJSON.isRestorable = false;
			this.creatorNameJSON.enableOnChange = true;
			base.RegisterString(this.creatorNameJSON);
			this.storedCreatorNameJSON = new JSONStorableString("storedCreatorName", this.dd.storedCreatorName);
			this.storedCreatorNameJSON.isStorable = false;
			this.storedCreatorNameJSON.isRestorable = false;
			this.tagsJSON = new JSONStorableString("tags", string.Empty, new JSONStorableString.SetStringCallback(this.SyncTagsFromJSON));
			this.tagsJSON.isStorable = false;
			this.tagsJSON.isRestorable = false;
			base.RegisterString(this.tagsJSON);
			this.storeAction = new JSONStorableAction("Store", new JSONStorableAction.ActionCallback(this.Store));
			base.RegisterAction(this.storeAction);
			this.loadAction = new JSONStorableAction("Load", new JSONStorableAction.ActionCallback(this.Load));
			base.RegisterAction(this.loadAction);
			this.getScreenshotAction = new JSONStorableAction("GetScreenshot", new JSONStorableAction.ActionCallback(this.GetScreenshot));
			base.RegisterAction(this.getScreenshotAction);
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x001AD14C File Offset: 0x001AB54C
		protected void StartInit()
		{
			string suggestPath = null;
			if (this.dd != null)
			{
				suggestPath = this.dd.GetStoreRootPath(false);
			}
			this.storeBrowsePathJSON = new JSONStorableUrl("storeBrowsePath", string.Empty, new JSONStorableString.SetStringCallback(this.SyncBrowsePath), "vam", suggestPath, true);
			this.storeBrowsePathJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowseCreator);
			this.storeBrowsePathJSON.allowFullComputerBrowse = false;
			this.storeBrowsePathJSON.allowBrowseAboveSuggestedPath = false;
			this.storeBrowsePathJSON.isStorable = false;
			this.storeBrowsePathJSON.isRestorable = false;
			base.RegisterUrl(this.storeBrowsePathJSON);
			if (this.IsClothing())
			{
				this.clothSimTextureFileJSON = new JSONStorableUrl("clothSimTextureFile", string.Empty, new JSONStorableString.SetStringCallback(this.SyncClothSimTextureFile), "png|jpg", suggestPath, true);
				this.clothSimTextureFileJSON.allowFullComputerBrowse = false;
				this.clothSimTextureFileJSON.allowBrowseAboveSuggestedPath = true;
				this.clothSimTextureFileJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginClothSimTextureBrowse);
				this.clothSimTextureFileJSON.isStorable = false;
				this.clothSimTextureFileJSON.isRestorable = false;
				base.RegisterUrl(this.clothSimTextureFileJSON);
			}
			this.SyncOtherTags();
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x001AD280 File Offset: 0x001AB680
		protected void SyncLoadButton()
		{
			if (this.dd != null)
			{
				if (this.dd.CheckStoreExistance())
				{
					if (this.loadAction != null && this.loadAction.dynamicButton != null && this.loadAction.dynamicButton.button != null)
					{
						this.loadAction.dynamicButton.button.interactable = true;
					}
				}
				else if (this.loadAction != null && this.loadAction.dynamicButton != null && this.loadAction.dynamicButton.button != null)
				{
					this.loadAction.dynamicButton.button.interactable = false;
				}
			}
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x001AD358 File Offset: 0x001AB758
		protected void SyncStoreButton()
		{
			if (this.dd != null)
			{
				if (!this.isWrapping && this.dd.CheckReadyForStore())
				{
					if (this.dd.CheckStoreExistance())
					{
						if (this.storeAction != null && this.storeAction.dynamicButton != null)
						{
							this.storeAction.dynamicButton.buttonColor = Color.red;
							if (this.storeAction.dynamicButton.button != null)
							{
								this.storeAction.dynamicButton.button.interactable = true;
							}
							if (this.storeAction.dynamicButton.buttonText != null)
							{
								this.storeAction.dynamicButton.buttonText.text = "Overwrite Existing Item";
							}
						}
					}
					else if (this.storeAction != null && this.storeAction.dynamicButton != null)
					{
						this.storeAction.dynamicButton.buttonColor = Color.green;
						if (this.storeAction.dynamicButton.button != null)
						{
							this.storeAction.dynamicButton.button.interactable = true;
						}
						if (this.storeAction.dynamicButton.buttonText != null)
						{
							this.storeAction.dynamicButton.buttonText.text = "Create New Item";
						}
					}
				}
				else if (this.dd.IsInPackage())
				{
					if (this.storeAction != null && this.storeAction.dynamicButton != null)
					{
						this.storeAction.dynamicButton.buttonColor = Color.gray;
						if (this.storeAction.dynamicButton.button != null)
						{
							this.storeAction.dynamicButton.button.interactable = false;
						}
						if (this.storeAction.dynamicButton.buttonText != null)
						{
							this.storeAction.dynamicButton.buttonText.text = "Create New Item Disabled Due To Being In Package. Clear Package To Make Local Copy.";
						}
					}
				}
				else if (this.storeAction != null && this.storeAction.dynamicButton != null)
				{
					this.storeAction.dynamicButton.buttonColor = Color.gray;
					if (this.storeAction.dynamicButton.button != null)
					{
						this.storeAction.dynamicButton.button.interactable = false;
					}
					if (this.storeAction.dynamicButton.buttonText != null)
					{
						this.storeAction.dynamicButton.buttonText.text = "Create New Item Disabled Until All Fields Are Set";
					}
				}
			}
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x001AD630 File Offset: 0x001ABA30
		protected void SyncUI()
		{
			this.SyncLoadButton();
			this.SyncStoreButton();
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x001AD640 File Offset: 0x001ABA40
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				DAZRuntimeCreatorUI componentInChildren = t.GetComponentInChildren<DAZRuntimeCreatorUI>();
				if (componentInChildren != null)
				{
					this.dufFileJSON.RegisterText(componentInChildren.dufFileUrlText, isAlt);
					this.dufFileJSON.RegisterFileBrowseButton(componentInChildren.dufFileBrowseButton, isAlt);
					this.clearAction.RegisterButton(componentInChildren.clearButton, isAlt);
					this.cancelAction.RegisterButton(componentInChildren.cancelButton, isAlt);
					this.combineMaterialsJSON.RegisterToggle(componentInChildren.combineMaterialsToggle, isAlt);
					this.wrapToMorphedVerticesJSON.RegisterToggle(componentInChildren.wrapToMorphedVerticesToggle, isAlt);
					this.importDufAction.RegisterButton(componentInChildren.importButton, isAlt);
					if (this.IsClothing())
					{
						this.generateClothSimAction.RegisterButton(componentInChildren.generateSimButton, isAlt);
						this.cancelGenerateClothSimAction.RegisterButton(componentInChildren.cancelGenerateSimButton, isAlt);
						this.setUniformClothSimTextureAction.RegisterButton(componentInChildren.setUniformClothSimTextureButton, isAlt);
						this.uniformClothSimTextureValueJSON.RegisterSlider(componentInChildren.uniformClothSimTextureValueSlider, isAlt);
						this.clothSimTextureFileJSON.RegisterClearButton(componentInChildren.clearClothSimTextureButton, isAlt);
						this.clothSimTextureFileJSON.RegisterFileBrowseButton(componentInChildren.selectClothSimTextureButton, isAlt);
						this.clothSimUseIndividualSimTexturesJSON.RegisterToggle(componentInChildren.clothSimUseIndividualSimTexturesToggle, isAlt);
						this.clothSimCreateNearbyJointsJSON.RegisterToggle(componentInChildren.clothSimCreateNearbyJointsToggle, isAlt);
						this.clothSimNearbyJointsDistanceJSON.RegisterSlider(componentInChildren.clothSimNearbyJointsDistanceSlider, isAlt);
					}
					if (this.IsHair())
					{
						foreach (ObjectChoice objectChoice in this.scalpChoices)
						{
							JSONStorable[] componentsInChildren = objectChoice.GetComponentsInChildren<JSONStorable>(true);
							foreach (JSONStorable jsonstorable in componentsInChildren)
							{
								if (isAlt)
								{
									jsonstorable.SetUIAlt(t);
								}
								else
								{
									jsonstorable.SetUI(t);
								}
							}
						}
						this.scalpChooserJSON.RegisterPopup(componentInChildren.scalpChooserPopup, isAlt);
						this.generateHairSimAction.RegisterButton(componentInChildren.generateSimButton, isAlt);
						this.cancelGenerateHairSimAction.RegisterButton(componentInChildren.cancelGenerateSimButton, isAlt);
						this.segmentsJSON.RegisterSlider(componentInChildren.hairSimSegmentsSlider, isAlt);
						this.segmentsLengthJSON.RegisterSlider(componentInChildren.hairSimSegmentsLengthSlider, isAlt);
						this.scalpMaskSelectableSizeJSON.RegisterSlider(componentInChildren.scalpMaskSelectableSizeSlider, isAlt);
						this.scalpMaskSetAllAction.RegisterButton(componentInChildren.scalpMaskSetAllButton, isAlt);
						this.scalpMaskClearAllAction.RegisterButton(componentInChildren.scalpMaskClearAllButton, isAlt);
						this.startScalpMaskEditModeAction.RegisterButton(componentInChildren.startScalpMaskEditModeButton, isAlt);
						this.cancelScalpMaskEditModeAction.RegisterButton(componentInChildren.cancelScalpMaskEditModeButton, isAlt);
						this.finishScalpMaskEditModeAction.RegisterButton(componentInChildren.finishScalpMaskEditModeButton, isAlt);
						this.scalpMaskEditModeHideBackfacesJSON.RegisterToggle(componentInChildren.scalpMaskEditModeHideBackfacesToggle, isAlt);
						this.SyncScalpMaskButtons();
					}
					this.autoSetStoreFolderNameFromDufJSON.RegisterToggle(componentInChildren.autoSetStoreFolderNameToDufToggle, isAlt);
					this.storeFolderNameJSON.RegisterInputField(componentInChildren.storeFolderNameField, isAlt);
					this.autoSetStoreNameFromDufJSON.RegisterToggle(componentInChildren.autoSetStoreNameToDufToggle, isAlt);
					this.packageNameJSON.RegisterText(componentInChildren.packageNameText, isAlt);
					this.clearPackageAction.RegisterButton(componentInChildren.clearPackageButton, isAlt);
					this.storeNameJSON.RegisterInputField(componentInChildren.storeNameField, isAlt);
					this.displayNameJSON.RegisterInputField(componentInChildren.displayNameField, isAlt);
					this.creatorNameJSON.RegisterInputField(componentInChildren.creatorNameField, isAlt);
					this.storedCreatorNameJSON.RegisterText(componentInChildren.storedCreatorNameText, isAlt);
					this.tagsJSON.RegisterInputField(componentInChildren.tagsField, isAlt);
					this.storeBrowsePathJSON.RegisterFileBrowseButton(componentInChildren.browseStoreButton, isAlt);
					this.storeAction.RegisterButton(componentInChildren.storeButton, isAlt);
					this.loadAction.RegisterButton(componentInChildren.loadButton, isAlt);
					this.getScreenshotAction.RegisterButton(componentInChildren.getScreenshotButton, isAlt);
					if (isAlt)
					{
						this.creatorStatusTextAlt = componentInChildren.creatorStatusText;
					}
					else
					{
						this.creatorStatusText = componentInChildren.creatorStatusText;
						this.thumbnailRawImage = componentInChildren.thumbnailRawImage;
						this.importMessageText = componentInChildren.importMessageText;
						this.importVertexCountText = componentInChildren.importVertexCountText;
						this.simVertexCountText = componentInChildren.simVertexCountText;
						this.simJointCountText = componentInChildren.simJointCountText;
						this.simNearbyJointCountText = componentInChildren.simNearbyJointCountText;
						if (this.IsClothing())
						{
							this.clothSimTextureRawImage = componentInChildren.clothSimTextureRawImage;
							ClothSimControl component = base.GetComponent<ClothSimControl>();
							if (component != null && component.simEnabledJSON != null)
							{
								component.simEnabledJSON.toggleAlt = componentInChildren.clothSimEnabledToggle;
							}
							DAZClothingItemControl component2 = base.GetComponent<DAZClothingItemControl>();
							if (component2 != null && component2.disableAnatomyJSON != null)
							{
								component2.disableAnatomyJSON.toggleAlt = componentInChildren.disableAnatomyToggle;
							}
						}
						else if (this.IsHair())
						{
							DAZHairGroupControl component3 = base.GetComponent<DAZHairGroupControl>();
							if (component3 != null && component3.disableAnatomyJSON != null)
							{
								component3.disableAnatomyJSON.toggleAlt = componentInChildren.disableAnatomyToggle;
							}
						}
						this.tagsPanel = componentInChildren.tagsPanel;
						this.regionTagsContent = componentInChildren.regionTagsContent;
						this.typeTagsContent = componentInChildren.typeTagsContent;
						this.otherTagsContent = componentInChildren.otherTagsContent;
						if (componentInChildren.openTagsPanelButton != null)
						{
							componentInChildren.openTagsPanelButton.onClick.AddListener(new UnityAction(this.OpenTagsPanel));
						}
						if (componentInChildren.closeTagsPanelButton != null)
						{
							componentInChildren.closeTagsPanelButton.onClick.AddListener(new UnityAction(this.CloseTagsPanel));
						}
						this.InitTagsUI();
					}
					if (DAZRuntimeCreator.tagsPanelOpen)
					{
						this.OpenTagsPanel();
					}
					this.SyncUI();
				}
			}
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x001ADBBC File Offset: 0x001ABFBC
		private void Update()
		{
			if (this.di != null && this.di.isImporting)
			{
				this.SetCreatorStatus(this.di.importStatus);
			}
			bool flag = false;
			DAZSkinWrap[] components = this.di.GetComponents<DAZSkinWrap>();
			foreach (DAZSkinWrap dazskinWrap in components)
			{
				if (dazskinWrap.IsWrapping)
				{
					flag = true;
					this.isWrapping = true;
					this.SetCreatorStatus(dazskinWrap.WrapStatus);
					this.SyncStoreButton();
				}
			}
			if (this.isWrapping && !flag)
			{
				this.isWrapping = false;
				this.SetCreatorStatus("Wrapping Complete");
				this.SyncStoreButton();
				if (this.IsHair() && this.customScalpChoiceName != null)
				{
					this.SyncScalpChoice(this.customScalpChoiceName);
				}
			}
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x001ADC99 File Offset: 0x001AC099
		private void Start()
		{
			this.StartInit();
		}

		// Token: 0x06004C76 RID: 19574 RVA: 0x001ADCA1 File Offset: 0x001AC0A1
		private void OnEnable()
		{
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x001ADCA4 File Offset: 0x001AC0A4
		private void OnDisable()
		{
			DAZCharacterRun componentInParent = base.GetComponentInParent<DAZCharacterRun>();
			if (componentInParent != null)
			{
				componentInParent.doSetMergedVerts = false;
			}
			if (this.scalpMaskEditMode)
			{
				this.CancelScalpMaskEditMode();
			}
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x001ADCDC File Offset: 0x001AC0DC
		private void OnDestroy()
		{
			this.AbortProcessGeometryThreaded(false);
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x001ADCE5 File Offset: 0x001AC0E5
		// Note: this type is marked as 'beforefieldinit'.
		static DAZRuntimeCreator()
		{
		}

		// Token: 0x04003A98 RID: 15000
		protected DAZImport di;

		// Token: 0x04003A99 RID: 15001
		protected DAZDynamic dd;

		// Token: 0x04003A9A RID: 15002
		public DAZImport dazImport;

		// Token: 0x04003A9B RID: 15003
		protected JSONStorableAction clearAction;

		// Token: 0x04003A9C RID: 15004
		protected JSONStorableAction cancelAction;

		// Token: 0x04003A9D RID: 15005
		protected JSONStorableAction importDufAction;

		// Token: 0x04003A9E RID: 15006
		protected string _dufStoreName;

		// Token: 0x04003A9F RID: 15007
		protected JSONStorableUrl dufFileJSON;

		// Token: 0x04003AA0 RID: 15008
		protected Text importMessageText;

		// Token: 0x04003AA1 RID: 15009
		protected Text importVertexCountText;

		// Token: 0x04003AA2 RID: 15010
		protected JSONStorableBool combineMaterialsJSON;

		// Token: 0x04003AA3 RID: 15011
		protected JSONStorableBool wrapToMorphedVerticesJSON;

		// Token: 0x04003AA4 RID: 15012
		protected Text simVertexCountText;

		// Token: 0x04003AA5 RID: 15013
		protected Text simJointCountText;

		// Token: 0x04003AA6 RID: 15014
		protected Text simNearbyJointCountText;

		// Token: 0x04003AA7 RID: 15015
		protected Texture2D clothSimTexture;

		// Token: 0x04003AA8 RID: 15016
		protected RawImage clothSimTextureRawImage;

		// Token: 0x04003AA9 RID: 15017
		protected JSONStorableUrl clothSimTextureFileJSON;

		// Token: 0x04003AAA RID: 15018
		protected bool clothSimUseIndividualSimTextures;

		// Token: 0x04003AAB RID: 15019
		protected JSONStorableBool clothSimUseIndividualSimTexturesJSON;

		// Token: 0x04003AAC RID: 15020
		protected JSONStorableAction setUniformClothSimTextureAction;

		// Token: 0x04003AAD RID: 15021
		protected JSONStorableFloat uniformClothSimTextureValueJSON;

		// Token: 0x04003AAE RID: 15022
		protected JSONStorableBool clothSimCreateNearbyJointsJSON;

		// Token: 0x04003AAF RID: 15023
		protected JSONStorableFloat clothSimNearbyJointsDistanceJSON;

		// Token: 0x04003AB0 RID: 15024
		protected Thread processGeometryThread;

		// Token: 0x04003AB1 RID: 15025
		protected ClothSettings clothSettingsForThread;

		// Token: 0x04003AB2 RID: 15026
		protected string threadError;

		// Token: 0x04003AB3 RID: 15027
		protected bool isGeneratingClothSim;

		// Token: 0x04003AB4 RID: 15028
		protected bool abortGenerateClothSim;

		// Token: 0x04003AB5 RID: 15029
		protected JSONStorableAction generateClothSimAction;

		// Token: 0x04003AB6 RID: 15030
		protected JSONStorableAction cancelGenerateClothSimAction;

		// Token: 0x04003AB7 RID: 15031
		public RuntimeHairGeometryCreator hairCreator;

		// Token: 0x04003AB8 RID: 15032
		public string customScalpChoiceName;

		// Token: 0x04003AB9 RID: 15033
		protected string startingScalpChoiceName;

		// Token: 0x04003ABA RID: 15034
		protected ObjectChoice[] scalpChoices;

		// Token: 0x04003ABB RID: 15035
		protected List<string> scalpChoiceNames;

		// Token: 0x04003ABC RID: 15036
		protected Dictionary<string, ObjectChoice> scalpChoiceNameToObjectChoice;

		// Token: 0x04003ABD RID: 15037
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ObjectChoice <CurrentScalpChoice>k__BackingField;

		// Token: 0x04003ABE RID: 15038
		protected JSONStorableStringChooser scalpChooserJSON;

		// Token: 0x04003ABF RID: 15039
		protected JSONStorableFloat segmentsJSON;

		// Token: 0x04003AC0 RID: 15040
		protected JSONStorableFloat segmentsLengthJSON;

		// Token: 0x04003AC1 RID: 15041
		protected bool scalpMaskEditMode;

		// Token: 0x04003AC2 RID: 15042
		protected JSONStorableFloat scalpMaskSelectableSizeJSON;

		// Token: 0x04003AC3 RID: 15043
		public Transform selectablePrefab;

		// Token: 0x04003AC4 RID: 15044
		protected List<Selectable> createdSelectables;

		// Token: 0x04003AC5 RID: 15045
		protected bool _scalpMaskEditModeHideBackfaces = true;

		// Token: 0x04003AC6 RID: 15046
		protected JSONStorableBool scalpMaskEditModeHideBackfacesJSON;

		// Token: 0x04003AC7 RID: 15047
		protected JSONStorableAction scalpMaskClearAllAction;

		// Token: 0x04003AC8 RID: 15048
		protected JSONStorableAction scalpMaskSetAllAction;

		// Token: 0x04003AC9 RID: 15049
		protected JSONStorableAction startScalpMaskEditModeAction;

		// Token: 0x04003ACA RID: 15050
		protected JSONStorableAction cancelScalpMaskEditModeAction;

		// Token: 0x04003ACB RID: 15051
		protected JSONStorableAction finishScalpMaskEditModeAction;

		// Token: 0x04003ACC RID: 15052
		protected bool isGeneratingHairSim;

		// Token: 0x04003ACD RID: 15053
		protected bool abortGenerateHairSim;

		// Token: 0x04003ACE RID: 15054
		protected JSONStorableAction generateHairSimAction;

		// Token: 0x04003ACF RID: 15055
		protected JSONStorableAction cancelGenerateHairSimAction;

		// Token: 0x04003AD0 RID: 15056
		protected JSONStorableString storeFolderNameJSON;

		// Token: 0x04003AD1 RID: 15057
		protected JSONStorableBool autoSetStoreFolderNameFromDufJSON;

		// Token: 0x04003AD2 RID: 15058
		protected JSONStorableString packageNameJSON;

		// Token: 0x04003AD3 RID: 15059
		protected JSONStorableAction clearPackageAction;

		// Token: 0x04003AD4 RID: 15060
		protected JSONStorableString storeNameJSON;

		// Token: 0x04003AD5 RID: 15061
		protected JSONStorableString displayNameJSON;

		// Token: 0x04003AD6 RID: 15062
		protected JSONStorableBool autoSetStoreNameFromDufJSON;

		// Token: 0x04003AD7 RID: 15063
		protected string _creatorName;

		// Token: 0x04003AD8 RID: 15064
		protected JSONStorableString creatorNameJSON;

		// Token: 0x04003AD9 RID: 15065
		protected JSONStorableString storedCreatorNameJSON;

		// Token: 0x04003ADA RID: 15066
		protected RectTransform tagsPanel;

		// Token: 0x04003ADB RID: 15067
		public Transform tagTogglePrefab;

		// Token: 0x04003ADC RID: 15068
		protected RectTransform regionTagsContent;

		// Token: 0x04003ADD RID: 15069
		protected RectTransform typeTagsContent;

		// Token: 0x04003ADE RID: 15070
		protected RectTransform otherTagsContent;

		// Token: 0x04003ADF RID: 15071
		protected static bool tagsPanelOpen;

		// Token: 0x04003AE0 RID: 15072
		public string[] regionTags;

		// Token: 0x04003AE1 RID: 15073
		public string[] maleTypeTags;

		// Token: 0x04003AE2 RID: 15074
		public string[] femaleTypeTags;

		// Token: 0x04003AE3 RID: 15075
		protected HashSet<string> otherTags = new HashSet<string>();

		// Token: 0x04003AE4 RID: 15076
		protected HashSet<string> tagsSet = new HashSet<string>();

		// Token: 0x04003AE5 RID: 15077
		private bool ignoreTagFromToggleCallback;

		// Token: 0x04003AE6 RID: 15078
		protected Dictionary<string, Toggle> tagToToggle = new Dictionary<string, Toggle>();

		// Token: 0x04003AE7 RID: 15079
		protected JSONStorableString tagsJSON;

		// Token: 0x04003AE8 RID: 15080
		protected JSONStorableUrl storeBrowsePathJSON;

		// Token: 0x04003AE9 RID: 15081
		protected JSONStorableAction storeAction;

		// Token: 0x04003AEA RID: 15082
		protected JSONStorableAction loadAction;

		// Token: 0x04003AEB RID: 15083
		protected Text creatorStatusText;

		// Token: 0x04003AEC RID: 15084
		protected Text creatorStatusTextAlt;

		// Token: 0x04003AED RID: 15085
		protected RawImage thumbnailRawImage;

		// Token: 0x04003AEE RID: 15086
		protected JSONStorableAction getScreenshotAction;

		// Token: 0x04003AEF RID: 15087
		protected bool isWrapping;

		// Token: 0x02000FCB RID: 4043
		[CompilerGenerated]
		private sealed class <GenerateClothSimCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007537 RID: 30007 RVA: 0x001ADCE7 File Offset: 0x001AC0E7
			[DebuggerHidden]
			public <GenerateClothSimCo>c__Iterator0()
			{
			}

			// Token: 0x06007538 RID: 30008 RVA: 0x001ADCF0 File Offset: 0x001AC0F0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (!(cs != null))
					{
						goto IL_41F;
					}
					if (this.simVertexCountText != null)
					{
						this.simVertexCountText.text = string.Empty;
					}
					if (this.simJointCountText != null)
					{
						this.simJointCountText.text = string.Empty;
					}
					if (this.simNearbyJointCountText != null)
					{
						this.simNearbyJointCountText.text = string.Empty;
					}
					this.SetCreatorStatus("Process Main Thread Sim Data Creation");
					cs.ProcessGeometryMainThread();
					this.SetCreatorStatus("Starting Sim Data Creation Thread");
					this.clothSettingsForThread = cs;
					this.threadError = null;
					this.processGeometryThread = new Thread(new ThreadStart(base.ProcessGeometryThreaded));
					this.processGeometryThread.Start();
					if (this.abortGenerateClothSim)
					{
						this.SetCreatorStatus("Sim Data Creation Aborted");
						this.isGeneratingClothSim = false;
						return false;
					}
					break;
				case 2U:
					break;
				default:
					return false;
				}
				if (this.processGeometryThread.IsAlive)
				{
					if (cs.GeometryData != null && cs.GeometryData.status != null && cs.GeometryData.status != string.Empty)
					{
						this.SetCreatorStatus(cs.GeometryData.status);
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				if (this.threadError != null)
				{
					this.SetCreatorStatus(this.threadError);
					this.isGeneratingClothSim = false;
					return false;
				}
				if (this.abortGenerateClothSim)
				{
					this.SetCreatorStatus("Sim Data Creation Aborted");
					this.isGeneratingClothSim = false;
					return false;
				}
				if (cs.GeometryData != null && cs.GeometryData.Particles != null)
				{
					if (this.simVertexCountText != null)
					{
						int num2 = cs.GeometryData.Particles.Length;
						this.simVertexCountText.text = num2.ToString();
						if (num2 > 25000)
						{
							this.simVertexCountText.color = Color.red;
						}
						else
						{
							this.simVertexCountText.color = Color.green;
						}
					}
					if (this.simJointCountText != null)
					{
						IEnumerable<Int2ListContainer> stiffnessJointGroups = cs.GeometryData.StiffnessJointGroups;
						if (DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache0 == null)
						{
							DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache0 = new Func<Int2ListContainer, int>(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>m__0);
						}
						int num3 = stiffnessJointGroups.Sum(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache0);
						this.simJointCountText.text = num3.ToString();
					}
					if (this.simNearbyJointCountText != null)
					{
						IEnumerable<Int2ListContainer> nearbyJointGroups = cs.GeometryData.NearbyJointGroups;
						if (DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache1 == null)
						{
							DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache1 = new Func<Int2ListContainer, int>(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>m__1);
						}
						int num4 = nearbyJointGroups.Sum(DAZRuntimeCreator.<GenerateClothSimCo>c__Iterator0.<>f__am$cache1);
						this.simNearbyJointCountText.text = num4.ToString();
					}
					this.SetCreatorStatus("Sim Data Creation Finished");
				}
				else
				{
					this.SetCreatorStatus("Sim Data Creation Failed");
				}
				base.SyncClothSimTextures(cs);
				IL_41F:
				this.isGeneratingClothSim = false;
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001153 RID: 4435
			// (get) Token: 0x06007539 RID: 30009 RVA: 0x001AE132 File Offset: 0x001AC532
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001154 RID: 4436
			// (get) Token: 0x0600753A RID: 30010 RVA: 0x001AE13A File Offset: 0x001AC53A
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600753B RID: 30011 RVA: 0x001AE142 File Offset: 0x001AC542
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600753C RID: 30012 RVA: 0x001AE152 File Offset: 0x001AC552
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600753D RID: 30013 RVA: 0x001AE159 File Offset: 0x001AC559
			private static int <>m__0(Int2ListContainer container)
			{
				return container.List.Count;
			}

			// Token: 0x0600753E RID: 30014 RVA: 0x001AE166 File Offset: 0x001AC566
			private static int <>m__1(Int2ListContainer container)
			{
				return container.List.Count;
			}

			// Token: 0x04006950 RID: 26960
			internal ClothSettings cs;

			// Token: 0x04006951 RID: 26961
			internal DAZRuntimeCreator $this;

			// Token: 0x04006952 RID: 26962
			internal object $current;

			// Token: 0x04006953 RID: 26963
			internal bool $disposing;

			// Token: 0x04006954 RID: 26964
			internal int $PC;

			// Token: 0x04006955 RID: 26965
			private static Func<Int2ListContainer, int> <>f__am$cache0;

			// Token: 0x04006956 RID: 26966
			private static Func<Int2ListContainer, int> <>f__am$cache1;
		}

		// Token: 0x02000FCC RID: 4044
		[CompilerGenerated]
		private sealed class <CreateTagToggle>c__AnonStorey2
		{
			// Token: 0x0600753F RID: 30015 RVA: 0x001AE173 File Offset: 0x001AC573
			public <CreateTagToggle>c__AnonStorey2()
			{
			}

			// Token: 0x06007540 RID: 30016 RVA: 0x001AE17B File Offset: 0x001AC57B
			internal void <>m__0(bool b)
			{
				this.$this.SyncTagFromToggle(this.tag, b);
			}

			// Token: 0x04006957 RID: 26967
			internal string tag;

			// Token: 0x04006958 RID: 26968
			internal DAZRuntimeCreator $this;
		}

		// Token: 0x02000FCD RID: 4045
		[CompilerGenerated]
		private sealed class <LoadDelay>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007541 RID: 30017 RVA: 0x001AE18F File Offset: 0x001AC58F
			[DebuggerHidden]
			public <LoadDelay>c__Iterator1()
			{
			}

			// Token: 0x06007542 RID: 30018 RVA: 0x001AE198 File Offset: 0x001AC598
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					itemControl = base.GetComponent<DAZClothingItemControl>();
					if (itemControl != null)
					{
						itemControl.ResetIsRealClothingItem();
					}
					if (this.dd.Load(true))
					{
						if (this.storedCreatorNameJSON != null)
						{
							this.storedCreatorNameJSON.val = this.dd.storedCreatorName;
						}
						if (this.displayNameJSON != null)
						{
							this.displayNameJSON.valNoCallback = this.dd.displayName;
						}
						if (this.tagsJSON != null)
						{
							this.tagsJSON.valNoCallback = this.dd.tags;
							base.SyncTagsSetToTags();
							base.SyncTagTogglesToTags();
						}
						if (this.di != null && this.di.materialUIConnectorMaster != null)
						{
							this.di.materialUIConnectorMaster.Rebuild();
						}
						if (base.IsHair())
						{
							this.segmentsJSON.valNoCallback = (float)this.hairCreator.Segments;
							this.segmentsLengthJSON.valNoCallback = this.hairCreator.SegmentLength;
							this.scalpChooserJSON.valNoCallback = this.hairCreator.ScalpProviderName;
							base.SyncScalpChoiceGameObject(this.hairCreator.ScalpProviderName);
						}
						base.LoadThumbnailImage(this.dd.GetStorePathBase() + ".jpg");
						this.SetCreatorStatus("Load from " + this.dd.storeName + " complete");
						base.SyncSimTextureLoadedHandlers();
					}
					else
					{
						base.ClearObjects();
						this.SetCreatorStatus("Load from " + this.dd.storeName + " failed");
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2U:
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x17001155 RID: 4437
			// (get) Token: 0x06007543 RID: 30019 RVA: 0x001AE45A File Offset: 0x001AC85A
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001156 RID: 4438
			// (get) Token: 0x06007544 RID: 30020 RVA: 0x001AE462 File Offset: 0x001AC862
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007545 RID: 30021 RVA: 0x001AE46A File Offset: 0x001AC86A
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007546 RID: 30022 RVA: 0x001AE47A File Offset: 0x001AC87A
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006959 RID: 26969
			internal DAZClothingItemControl <itemControl>__0;

			// Token: 0x0400695A RID: 26970
			internal DAZRuntimeCreator $this;

			// Token: 0x0400695B RID: 26971
			internal object $current;

			// Token: 0x0400695C RID: 26972
			internal bool $disposing;

			// Token: 0x0400695D RID: 26973
			internal int $PC;
		}
	}
}
