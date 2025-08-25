using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using MeshVR;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D20 RID: 3360
public class MaterialOptions : JSONStorable, IBinaryStorable
{
	// Token: 0x060066AF RID: 26287 RVA: 0x00150674 File Offset: 0x0014EA74
	public MaterialOptions()
	{
	}

	// Token: 0x060066B0 RID: 26288 RVA: 0x001508AA File Offset: 0x0014ECAA
	protected void RegisterAllocatedObject(UnityEngine.Object o)
	{
		if (Application.isPlaying)
		{
			if (this.allocatedObjects == null)
			{
				this.allocatedObjects = new List<UnityEngine.Object>();
			}
			this.allocatedObjects.Add(o);
		}
	}

	// Token: 0x060066B1 RID: 26289 RVA: 0x001508D8 File Offset: 0x0014ECD8
	protected void DestroyAllocatedObjects()
	{
		if (Application.isPlaying && this.allocatedObjects != null)
		{
			foreach (UnityEngine.Object obj in this.allocatedObjects)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	// Token: 0x060066B2 RID: 26290 RVA: 0x00150948 File Offset: 0x0014ED48
	public bool LoadFromBinaryReader(BinaryReader binReader)
	{
		try
		{
			string a = binReader.ReadString();
			if (a != "MaterialOptions")
			{
				SuperController.LogError("Binary file corrupted. Tried to read MaterialOptions in wrong section");
				return false;
			}
			string text = binReader.ReadString();
			if (text != "1.0")
			{
				SuperController.LogError("MaterialOptions schema " + text + " is not compatible with this version of software");
				return false;
			}
			if (this.textureGroup1 == null)
			{
				this.textureGroup1 = new MaterialOptionTextureGroup();
			}
			this.overrideId = binReader.ReadString();
			int num = binReader.ReadInt32();
			this.paramMaterialSlots = new int[num];
			this.textureGroup1.materialSlots = new int[num];
			for (int i = 0; i < num; i++)
			{
				this.paramMaterialSlots[i] = binReader.ReadInt32();
				this.textureGroup1.materialSlots[i] = this.paramMaterialSlots[i];
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while loading MaterialOptions from binary reader " + arg);
			return false;
		}
		return true;
	}

	// Token: 0x060066B3 RID: 26291 RVA: 0x00150A64 File Offset: 0x0014EE64
	public bool LoadFromBinaryFile(string path)
	{
		bool result = false;
		try
		{
			using (FileEntryStream fileEntryStream = FileManager.OpenStream(path, true))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileEntryStream.Stream))
				{
					result = this.LoadFromBinaryReader(binaryReader);
				}
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Error while loading MaterialOptions from binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x060066B4 RID: 26292 RVA: 0x00150B10 File Offset: 0x0014EF10
	public bool StoreToBinaryWriter(BinaryWriter binWriter)
	{
		try
		{
			binWriter.Write("MaterialOptions");
			binWriter.Write("1.0");
			binWriter.Write(this.overrideId);
			binWriter.Write(this.paramMaterialSlots.Length);
			for (int i = 0; i < this.paramMaterialSlots.Length; i++)
			{
				binWriter.Write(this.paramMaterialSlots[i]);
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while storing MaterialOptions to binary writer " + arg);
			return false;
		}
		return true;
	}

	// Token: 0x060066B5 RID: 26293 RVA: 0x00150BA4 File Offset: 0x0014EFA4
	public bool StoreToBinaryFile(string path)
	{
		bool result = false;
		try
		{
			FileManager.AssertNotCalledFromPlugin();
			using (FileStream fileStream = FileManager.OpenStreamForCreate(path))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					result = this.StoreToBinaryWriter(binaryWriter);
				}
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Error while storing MaterialOptions to binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x060066B6 RID: 26294 RVA: 0x00150C50 File Offset: 0x0014F050
	protected void SyncCustomName(string s)
	{
		if (this.origOverrideId == null)
		{
			this.origOverrideId = this.overrideId;
		}
		if (s != null && s != string.Empty)
		{
			this.overrideId = "+Material" + s;
		}
		else
		{
			this.overrideId = this.origOverrideId;
		}
	}

	// Token: 0x060066B7 RID: 26295 RVA: 0x00150CAC File Offset: 0x0014F0AC
	protected void SyncRenderQueue(float f)
	{
		int materialRenderQueue = Mathf.FloorToInt(f);
		this.SetMaterialRenderQueue(materialRenderQueue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				materialOptions.renderQueueJSON.val = this.renderQueueJSON.val;
			}
		}
	}

	// Token: 0x060066B8 RID: 26296 RVA: 0x00150D3C File Offset: 0x0014F13C
	protected void SyncHideMaterial(bool b)
	{
		this.SetMaterialHide(b);
		if (!b)
		{
			int materialRenderQueue = Mathf.FloorToInt(this.renderQueueJSON.val);
			this.SetMaterialRenderQueue(materialRenderQueue);
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				materialOptions.hideMaterialJSON.val = this.hideMaterialJSON.val;
			}
		}
	}

	// Token: 0x060066B9 RID: 26297 RVA: 0x00150DE4 File Offset: 0x0014F1E4
	protected virtual void SyncLinkToOtherMaterials(bool b)
	{
		this._linkToOtherMaterials = b;
		if (this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				materialOptions.renderQueueJSON.val = this.renderQueueJSON.val;
				if (materialOptions.hideMaterialJSON != null && this.hideMaterialJSON != null)
				{
					materialOptions.hideMaterialJSON.val = this.hideMaterialJSON.val;
				}
				if (materialOptions.color1JSONParam != null && this.color1JSONParam != null)
				{
					materialOptions.color1JSONParam.val = this.color1JSONParam.val;
				}
				if (materialOptions.color2JSONParam != null && this.color2JSONParam != null)
				{
					materialOptions.color2JSONParam.val = this.color2JSONParam.val;
				}
				if (materialOptions.color3JSONParam != null && this.color3JSONParam != null)
				{
					materialOptions.color3JSONParam.val = this.color3JSONParam.val;
				}
				if (materialOptions.param1JSONParam != null && this.param1JSONParam != null)
				{
					materialOptions.param1JSONParam.val = this.param1JSONParam.val;
				}
				if (materialOptions.param2JSONParam != null && this.param2JSONParam != null)
				{
					materialOptions.param2JSONParam.val = this.param2JSONParam.val;
				}
				if (materialOptions.param3JSONParam != null && this.param3JSONParam != null)
				{
					materialOptions.param3JSONParam.val = this.param3JSONParam.val;
				}
				if (materialOptions.param4JSONParam != null && this.param4JSONParam != null)
				{
					materialOptions.param4JSONParam.val = this.param4JSONParam.val;
				}
				if (materialOptions.param5JSONParam != null && this.param5JSONParam != null)
				{
					materialOptions.param5JSONParam.val = this.param5JSONParam.val;
				}
				if (materialOptions.param6JSONParam != null && this.param6JSONParam != null)
				{
					materialOptions.param6JSONParam.val = this.param6JSONParam.val;
				}
				if (materialOptions.param7JSONParam != null && this.param7JSONParam != null)
				{
					materialOptions.param7JSONParam.val = this.param7JSONParam.val;
				}
				if (materialOptions.param8JSONParam != null && this.param8JSONParam != null)
				{
					materialOptions.param8JSONParam.val = this.param8JSONParam.val;
				}
				if (materialOptions.param9JSONParam != null && this.param9JSONParam != null)
				{
					materialOptions.param9JSONParam.val = this.param9JSONParam.val;
				}
				if (materialOptions.param10JSONParam != null && this.param10JSONParam != null)
				{
					materialOptions.param10JSONParam.val = this.param10JSONParam.val;
				}
				if (materialOptions.textureGroup1JSON != null && this.textureGroup1JSON != null)
				{
					materialOptions.textureGroup1JSON.val = this.textureGroup1JSON.val;
				}
				if (materialOptions.textureGroup2JSON != null && this.textureGroup2JSON != null)
				{
					materialOptions.textureGroup2JSON.val = this.textureGroup2JSON.val;
				}
				if (materialOptions.textureGroup3JSON != null && this.textureGroup3JSON != null)
				{
					materialOptions.textureGroup3JSON.val = this.textureGroup3JSON.val;
				}
				if (materialOptions.textureGroup4JSON != null && this.textureGroup4JSON != null)
				{
					materialOptions.textureGroup4JSON.val = this.textureGroup4JSON.val;
				}
				if (materialOptions.textureGroup5JSON != null && this.textureGroup5JSON != null)
				{
					materialOptions.textureGroup5JSON.val = this.textureGroup5JSON.val;
				}
				if (materialOptions.customTexture1UrlJSON != null && this.customTexture1UrlJSON != null)
				{
					materialOptions.customTexture1UrlJSON.val = this.customTexture1UrlJSON.val;
					materialOptions.customTexture1TileXJSON.val = this.customTexture1TileXJSON.val;
					materialOptions.customTexture1TileYJSON.val = this.customTexture1TileYJSON.val;
					materialOptions.customTexture1OffsetXJSON.val = this.customTexture1OffsetXJSON.val;
					materialOptions.customTexture1OffsetYJSON.val = this.customTexture1OffsetYJSON.val;
				}
				if (materialOptions.customTexture2UrlJSON != null && this.customTexture2UrlJSON != null)
				{
					materialOptions.customTexture2UrlJSON.val = this.customTexture2UrlJSON.val;
					materialOptions.customTexture2TileXJSON.val = this.customTexture2TileXJSON.val;
					materialOptions.customTexture2TileYJSON.val = this.customTexture2TileYJSON.val;
					materialOptions.customTexture2OffsetXJSON.val = this.customTexture2OffsetXJSON.val;
					materialOptions.customTexture2OffsetYJSON.val = this.customTexture2OffsetYJSON.val;
				}
				if (materialOptions.customTexture3UrlJSON != null && this.customTexture3UrlJSON != null)
				{
					materialOptions.customTexture3UrlJSON.val = this.customTexture3UrlJSON.val;
					materialOptions.customTexture3TileXJSON.val = this.customTexture3TileXJSON.val;
					materialOptions.customTexture3TileYJSON.val = this.customTexture3TileYJSON.val;
					materialOptions.customTexture3OffsetXJSON.val = this.customTexture3OffsetXJSON.val;
					materialOptions.customTexture3OffsetYJSON.val = this.customTexture3OffsetYJSON.val;
				}
				if (materialOptions.customTexture4UrlJSON != null && this.customTexture4UrlJSON != null)
				{
					materialOptions.customTexture4UrlJSON.val = this.customTexture4UrlJSON.val;
					materialOptions.customTexture4TileXJSON.val = this.customTexture4TileXJSON.val;
					materialOptions.customTexture4TileYJSON.val = this.customTexture4TileYJSON.val;
					materialOptions.customTexture4OffsetXJSON.val = this.customTexture4OffsetXJSON.val;
					materialOptions.customTexture4OffsetYJSON.val = this.customTexture4OffsetYJSON.val;
				}
				if (materialOptions.customTexture5UrlJSON != null && this.customTexture5UrlJSON != null)
				{
					materialOptions.customTexture5UrlJSON.val = this.customTexture5UrlJSON.val;
					materialOptions.customTexture5TileXJSON.val = this.customTexture5TileXJSON.val;
					materialOptions.customTexture5TileYJSON.val = this.customTexture5TileYJSON.val;
					materialOptions.customTexture5OffsetXJSON.val = this.customTexture5OffsetXJSON.val;
					materialOptions.customTexture5OffsetYJSON.val = this.customTexture5OffsetYJSON.val;
				}
				if (materialOptions.customTexture6UrlJSON != null && this.customTexture6UrlJSON != null)
				{
					materialOptions.customTexture6UrlJSON.val = this.customTexture6UrlJSON.val;
					materialOptions.customTexture6TileXJSON.val = this.customTexture6TileXJSON.val;
					materialOptions.customTexture6TileYJSON.val = this.customTexture6TileYJSON.val;
					materialOptions.customTexture6OffsetXJSON.val = this.customTexture6OffsetXJSON.val;
					materialOptions.customTexture6OffsetYJSON.val = this.customTexture6OffsetYJSON.val;
				}
			}
		}
		foreach (MaterialOptions materialOptions2 in this.otherMaterialOptionsList)
		{
			materialOptions2.linkToOtherMaterialsJSON.val = this.linkToOtherMaterialsJSON.val;
		}
	}

	// Token: 0x060066BA RID: 26298 RVA: 0x00151530 File Offset: 0x0014F930
	public void OpenTextureFolderInExplorer()
	{
		string text = this.customTextureFolder;
		text = Regex.Replace(text, ".*:\\\\", string.Empty);
		if (!FileManager.DirectoryExists(text, false, false) && FileManager.IsSecureWritePath(text))
		{
			FileManager.CreateDirectory(text);
		}
		if (FileManager.DirectoryExists(text, false, false))
		{
			SuperController.singleton.OpenFolderInExplorer(text);
		}
	}

	// Token: 0x060066BB RID: 26299 RVA: 0x0015158C File Offset: 0x0014F98C
	public virtual void SetCustomTextureFolder(string path)
	{
		if (this.customTextureFolder != null && FileManager.IsDirectoryInPackage(this.customTextureFolder))
		{
			this.customTexturePackageFolder = this.customTextureFolder;
		}
		this.customTextureFolder = path;
		if (this.customTexture1UrlJSON != null)
		{
			this.customTexture1UrlJSON.suggestedPath = this.customTextureFolder;
		}
		if (this.customTexture2UrlJSON != null)
		{
			this.customTexture2UrlJSON.suggestedPath = this.customTextureFolder;
		}
		if (this.customTexture3UrlJSON != null)
		{
			this.customTexture3UrlJSON.suggestedPath = this.customTextureFolder;
		}
		if (this.customTexture4UrlJSON != null)
		{
			this.customTexture4UrlJSON.suggestedPath = this.customTextureFolder;
		}
		if (this.customTexture5UrlJSON != null)
		{
			this.customTexture5UrlJSON.suggestedPath = this.customTextureFolder;
		}
		if (this.customTexture6UrlJSON != null)
		{
			this.customTexture6UrlJSON.suggestedPath = this.customTextureFolder;
		}
	}

	// Token: 0x060066BC RID: 26300 RVA: 0x00151670 File Offset: 0x0014FA70
	protected void QueueCustomTexture(string url, bool forceReload, bool isLinear, bool isNormalMap, bool isTransparency, ImageLoaderThreaded.ImageLoaderCallback callback)
	{
		if (ImageLoaderThreaded.singleton != null)
		{
			ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
			queuedImage.imgPath = url;
			queuedImage.forceReload = forceReload;
			queuedImage.createMipMaps = true;
			queuedImage.isNormalMap = isNormalMap;
			queuedImage.isThumbnail = false;
			queuedImage.linear = isLinear;
			queuedImage.createAlphaFromGrayscale = isTransparency;
			queuedImage.compress = !queuedImage.isNormalMap;
			queuedImage.callback = callback;
			ImageLoaderThreaded.singleton.QueueImage(queuedImage);
		}
	}

	// Token: 0x060066BD RID: 26301 RVA: 0x001516E8 File Offset: 0x0014FAE8
	protected void OnTexture1Loaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (!qi.hadError)
		{
			if (this != null)
			{
				this.RegisterTexture(qi.tex);
				this.DeregisterTexture(this.customTexture1);
				this.customTexture1 = qi.tex;
				this.customTexture1IsNull = (qi.imgPath == "NULL");
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 0, qi.tex, this.customTexture1IsNull);
			}
		}
		else
		{
			SuperController.LogError("Error during texture load: " + qi.errorText);
		}
	}

	// Token: 0x060066BE RID: 26302 RVA: 0x00151780 File Offset: 0x0014FB80
	protected void SyncCustomTexture1Url(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (val != null && val != string.Empty)
		{
			if (Regex.IsMatch(val, "^http"))
			{
				SuperController.LogError("Texture load does not currently support http image urls");
			}
			else
			{
				this.QueueCustomTexture(val, jsonstorableUrl.valueSetFromBrowse, this.customTexture1IsLinear, this.customTexture1IsNormal, this.customTexture1IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture1Loaded));
			}
		}
		else
		{
			this.QueueCustomTexture(null, jsonstorableUrl.valueSetFromBrowse, this.customTexture1IsLinear, this.customTexture1IsNormal, this.customTexture1IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture1Loaded));
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture1UrlJSON != null)
				{
					materialOptions.customTexture1UrlJSON.val = this.customTexture1UrlJSON.val;
				}
			}
		}
	}

	// Token: 0x060066BF RID: 26303 RVA: 0x001518AC File Offset: 0x0014FCAC
	protected void SetCustomTexture1ToNull()
	{
		if (this.customTexture1UrlJSON != null)
		{
			this.customTexture1UrlJSON.val = "NULL";
		}
	}

	// Token: 0x060066C0 RID: 26304 RVA: 0x001518CC File Offset: 0x0014FCCC
	protected void SyncCustomTexture1Tile(float f)
	{
		Vector2 scale;
		scale.x = this.customTexture1TileXJSON.val;
		scale.y = this.customTexture1TileYJSON.val;
		this.SetTextureGroupTextureScale(this.textureGroup1, 0, scale);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture1TileXJSON != null)
				{
					materialOptions.customTexture1TileXJSON.val = this.customTexture1TileXJSON.val;
					materialOptions.customTexture1TileYJSON.val = this.customTexture1TileYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066C1 RID: 26305 RVA: 0x001519A0 File Offset: 0x0014FDA0
	protected void SyncCustomTexture1Offset(float f)
	{
		Vector2 offset;
		offset.x = this.customTexture1OffsetXJSON.val;
		offset.y = this.customTexture1OffsetYJSON.val;
		this.SetTextureGroupTextureOffset(this.textureGroup1, 0, offset);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture1OffsetXJSON != null)
				{
					materialOptions.customTexture1OffsetXJSON.val = this.customTexture1OffsetXJSON.val;
					materialOptions.customTexture1OffsetYJSON.val = this.customTexture1OffsetYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066C2 RID: 26306 RVA: 0x00151A74 File Offset: 0x0014FE74
	protected void OnTexture2Loaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (!qi.hadError)
		{
			if (this != null)
			{
				this.RegisterTexture(qi.tex);
				this.DeregisterTexture(this.customTexture2);
				this.customTexture2 = qi.tex;
				this.customTexture2IsNull = (qi.imgPath == "NULL");
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 1, qi.tex, this.customTexture2IsNull);
			}
		}
		else
		{
			SuperController.LogError("Error during texture load: " + qi.errorText);
		}
	}

	// Token: 0x060066C3 RID: 26307 RVA: 0x00151B0C File Offset: 0x0014FF0C
	protected void SyncCustomTexture2Url(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (val != null && val != string.Empty)
		{
			if (Regex.IsMatch(val, "^http"))
			{
				SuperController.LogError("Texture load does not currently support http image urls");
			}
			else
			{
				this.QueueCustomTexture(val, jsonstorableUrl.valueSetFromBrowse, this.customTexture2IsLinear, this.customTexture2IsNormal, this.customTexture2IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture2Loaded));
			}
		}
		else
		{
			this.QueueCustomTexture(null, jsonstorableUrl.valueSetFromBrowse, this.customTexture2IsLinear, this.customTexture2IsNormal, this.customTexture2IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture2Loaded));
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture2UrlJSON != null)
				{
					materialOptions.customTexture2UrlJSON.val = this.customTexture2UrlJSON.val;
				}
			}
		}
	}

	// Token: 0x060066C4 RID: 26308 RVA: 0x00151C38 File Offset: 0x00150038
	protected void SetCustomTexture2ToNull()
	{
		if (this.customTexture2UrlJSON != null)
		{
			this.customTexture2UrlJSON.val = "NULL";
		}
	}

	// Token: 0x060066C5 RID: 26309 RVA: 0x00151C58 File Offset: 0x00150058
	protected void SyncCustomTexture2Tile(float f)
	{
		Vector2 scale;
		scale.x = this.customTexture2TileXJSON.val;
		scale.y = this.customTexture2TileYJSON.val;
		this.SetTextureGroupTextureScale(this.textureGroup1, 1, scale);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture2TileXJSON != null)
				{
					materialOptions.customTexture2TileXJSON.val = this.customTexture2TileXJSON.val;
					materialOptions.customTexture2TileYJSON.val = this.customTexture2TileYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066C6 RID: 26310 RVA: 0x00151D2C File Offset: 0x0015012C
	protected void SyncCustomTexture2Offset(float f)
	{
		Vector2 offset;
		offset.x = this.customTexture2OffsetXJSON.val;
		offset.y = this.customTexture2OffsetYJSON.val;
		this.SetTextureGroupTextureOffset(this.textureGroup1, 1, offset);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture2OffsetXJSON != null)
				{
					materialOptions.customTexture2OffsetXJSON.val = this.customTexture2OffsetXJSON.val;
					materialOptions.customTexture2OffsetYJSON.val = this.customTexture2OffsetYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066C7 RID: 26311 RVA: 0x00151E00 File Offset: 0x00150200
	protected void OnTexture3Loaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (!qi.hadError)
		{
			if (this != null)
			{
				this.RegisterTexture(qi.tex);
				this.DeregisterTexture(this.customTexture3);
				this.customTexture3 = qi.tex;
				this.customTexture3IsNull = (qi.imgPath == "NULL");
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 2, qi.tex, this.customTexture3IsNull);
			}
		}
		else
		{
			SuperController.LogError("Error during texture load: " + qi.errorText);
		}
	}

	// Token: 0x060066C8 RID: 26312 RVA: 0x00151E98 File Offset: 0x00150298
	protected void SyncCustomTexture3Url(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (val != null && val != string.Empty)
		{
			if (Regex.IsMatch(val, "^http"))
			{
				SuperController.LogError("Texture load does not currently support http image urls");
			}
			else
			{
				this.QueueCustomTexture(val, jsonstorableUrl.valueSetFromBrowse, this.customTexture3IsLinear, this.customTexture3IsNormal, this.customTexture3IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture3Loaded));
			}
		}
		else
		{
			this.QueueCustomTexture(null, jsonstorableUrl.valueSetFromBrowse, this.customTexture3IsLinear, this.customTexture3IsNormal, this.customTexture3IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture3Loaded));
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture3UrlJSON != null)
				{
					materialOptions.customTexture3UrlJSON.val = this.customTexture3UrlJSON.val;
				}
			}
		}
	}

	// Token: 0x060066C9 RID: 26313 RVA: 0x00151FC4 File Offset: 0x001503C4
	protected void SetCustomTexture3ToNull()
	{
		if (this.customTexture3UrlJSON != null)
		{
			this.customTexture3UrlJSON.val = "NULL";
		}
	}

	// Token: 0x060066CA RID: 26314 RVA: 0x00151FE4 File Offset: 0x001503E4
	protected void SyncCustomTexture3Tile(float f)
	{
		Vector2 scale;
		scale.x = this.customTexture3TileXJSON.val;
		scale.y = this.customTexture3TileYJSON.val;
		this.SetTextureGroupTextureScale(this.textureGroup1, 2, scale);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture3TileXJSON != null)
				{
					materialOptions.customTexture3TileXJSON.val = this.customTexture3TileXJSON.val;
					materialOptions.customTexture3TileYJSON.val = this.customTexture3TileYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066CB RID: 26315 RVA: 0x001520B8 File Offset: 0x001504B8
	protected void SyncCustomTexture3Offset(float f)
	{
		Vector2 offset;
		offset.x = this.customTexture3OffsetXJSON.val;
		offset.y = this.customTexture3OffsetYJSON.val;
		this.SetTextureGroupTextureOffset(this.textureGroup1, 2, offset);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture3OffsetXJSON != null)
				{
					materialOptions.customTexture3OffsetXJSON.val = this.customTexture3OffsetXJSON.val;
					materialOptions.customTexture3OffsetYJSON.val = this.customTexture3OffsetYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066CC RID: 26316 RVA: 0x0015218C File Offset: 0x0015058C
	protected void OnTexture4Loaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (!qi.hadError)
		{
			if (this != null)
			{
				this.RegisterTexture(qi.tex);
				this.DeregisterTexture(this.customTexture4);
				this.customTexture4 = qi.tex;
				this.customTexture4IsNull = (qi.imgPath == "NULL");
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 3, qi.tex, this.customTexture4IsNull);
			}
		}
		else
		{
			SuperController.LogError("Error during texture load: " + qi.errorText);
		}
	}

	// Token: 0x060066CD RID: 26317 RVA: 0x00152224 File Offset: 0x00150624
	protected void SyncCustomTexture4Url(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (val != null && val != string.Empty)
		{
			if (Regex.IsMatch(val, "^http"))
			{
				SuperController.LogError("Texture load does not currently support http image urls");
			}
			else
			{
				this.QueueCustomTexture(val, jsonstorableUrl.valueSetFromBrowse, this.customTexture4IsLinear, this.customTexture4IsNormal, this.customTexture4IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture4Loaded));
			}
		}
		else
		{
			this.QueueCustomTexture(null, jsonstorableUrl.valueSetFromBrowse, this.customTexture4IsLinear, this.customTexture4IsNormal, this.customTexture4IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture4Loaded));
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture4UrlJSON != null)
				{
					materialOptions.customTexture4UrlJSON.val = this.customTexture4UrlJSON.val;
				}
			}
		}
	}

	// Token: 0x060066CE RID: 26318 RVA: 0x00152350 File Offset: 0x00150750
	protected void SetCustomTexture4ToNull()
	{
		if (this.customTexture4UrlJSON != null)
		{
			this.customTexture4UrlJSON.val = "NULL";
		}
	}

	// Token: 0x060066CF RID: 26319 RVA: 0x00152370 File Offset: 0x00150770
	protected void SyncCustomTexture4Tile(float f)
	{
		Vector2 scale;
		scale.x = this.customTexture4TileXJSON.val;
		scale.y = this.customTexture4TileYJSON.val;
		this.SetTextureGroupTextureScale(this.textureGroup1, 3, scale);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture4TileXJSON != null)
				{
					materialOptions.customTexture4TileXJSON.val = this.customTexture4TileXJSON.val;
					materialOptions.customTexture4TileYJSON.val = this.customTexture4TileYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066D0 RID: 26320 RVA: 0x00152444 File Offset: 0x00150844
	protected void SyncCustomTexture4Offset(float f)
	{
		Vector2 offset;
		offset.x = this.customTexture4OffsetXJSON.val;
		offset.y = this.customTexture4OffsetYJSON.val;
		this.SetTextureGroupTextureOffset(this.textureGroup1, 3, offset);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture4OffsetXJSON != null)
				{
					materialOptions.customTexture4OffsetXJSON.val = this.customTexture4OffsetXJSON.val;
					materialOptions.customTexture4OffsetYJSON.val = this.customTexture4OffsetYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066D1 RID: 26321 RVA: 0x00152518 File Offset: 0x00150918
	protected void OnTexture5Loaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (!qi.hadError)
		{
			if (this != null)
			{
				this.RegisterTexture(qi.tex);
				this.DeregisterTexture(this.customTexture5);
				this.customTexture5 = qi.tex;
				this.customTexture5IsNull = (qi.imgPath == "NULL");
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 4, qi.tex, this.customTexture5IsNull);
			}
		}
		else
		{
			SuperController.LogError("Error during texture load: " + qi.errorText);
		}
	}

	// Token: 0x060066D2 RID: 26322 RVA: 0x001525B0 File Offset: 0x001509B0
	protected void SyncCustomTexture5Url(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (val != null && val != string.Empty)
		{
			if (Regex.IsMatch(val, "^http"))
			{
				SuperController.LogError("Texture load does not currently support http image urls");
			}
			else
			{
				this.QueueCustomTexture(val, jsonstorableUrl.valueSetFromBrowse, this.customTexture5IsLinear, this.customTexture5IsNormal, this.customTexture5IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture5Loaded));
			}
		}
		else
		{
			this.QueueCustomTexture(null, jsonstorableUrl.valueSetFromBrowse, this.customTexture5IsLinear, this.customTexture5IsNormal, this.customTexture5IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture5Loaded));
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture5UrlJSON != null)
				{
					materialOptions.customTexture5UrlJSON.val = this.customTexture5UrlJSON.val;
				}
			}
		}
	}

	// Token: 0x060066D3 RID: 26323 RVA: 0x001526DC File Offset: 0x00150ADC
	protected void SetCustomTexture5ToNull()
	{
		if (this.customTexture5UrlJSON != null)
		{
			this.customTexture5UrlJSON.val = "NULL";
		}
	}

	// Token: 0x060066D4 RID: 26324 RVA: 0x001526FC File Offset: 0x00150AFC
	protected void SyncCustomTexture5Tile(float f)
	{
		Vector2 scale;
		scale.x = this.customTexture5TileXJSON.val;
		scale.y = this.customTexture5TileYJSON.val;
		this.SetTextureGroupTextureScale(this.textureGroup1, 4, scale);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture5TileXJSON != null)
				{
					materialOptions.customTexture5TileXJSON.val = this.customTexture5TileXJSON.val;
					materialOptions.customTexture5TileYJSON.val = this.customTexture5TileYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066D5 RID: 26325 RVA: 0x001527D0 File Offset: 0x00150BD0
	protected void SyncCustomTexture5Offset(float f)
	{
		Vector2 offset;
		offset.x = this.customTexture5OffsetXJSON.val;
		offset.y = this.customTexture5OffsetYJSON.val;
		this.SetTextureGroupTextureOffset(this.textureGroup1, 4, offset);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture5OffsetXJSON != null)
				{
					materialOptions.customTexture5OffsetXJSON.val = this.customTexture5OffsetXJSON.val;
					materialOptions.customTexture5OffsetYJSON.val = this.customTexture5OffsetYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066D6 RID: 26326 RVA: 0x001528A4 File Offset: 0x00150CA4
	protected void OnTexture6Loaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (!qi.hadError)
		{
			if (this != null)
			{
				this.RegisterTexture(qi.tex);
				this.DeregisterTexture(this.customTexture6);
				this.customTexture6 = qi.tex;
				this.customTexture6IsNull = (qi.imgPath == "NULL");
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 5, qi.tex, this.customTexture6IsNull);
			}
		}
		else
		{
			SuperController.LogError("Error during texture load: " + qi.errorText);
		}
	}

	// Token: 0x060066D7 RID: 26327 RVA: 0x0015293C File Offset: 0x00150D3C
	protected void SyncCustomTexture6Url(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (val != null && val != string.Empty)
		{
			if (Regex.IsMatch(val, "^http"))
			{
				SuperController.LogError("Texture load does not currently support http image urls");
			}
			else
			{
				this.QueueCustomTexture(val, jsonstorableUrl.valueSetFromBrowse, this.customTexture6IsLinear, this.customTexture6IsNormal, this.customTexture6IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture6Loaded));
			}
		}
		else
		{
			this.QueueCustomTexture(null, jsonstorableUrl.valueSetFromBrowse, this.customTexture6IsLinear, this.customTexture6IsNormal, this.customTexture6IsTransparency, new ImageLoaderThreaded.ImageLoaderCallback(this.OnTexture6Loaded));
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture6UrlJSON != null)
				{
					materialOptions.customTexture6UrlJSON.val = this.customTexture6UrlJSON.val;
				}
			}
		}
	}

	// Token: 0x060066D8 RID: 26328 RVA: 0x00152A68 File Offset: 0x00150E68
	protected void SetCustomTexture6ToNull()
	{
		if (this.customTexture6UrlJSON != null)
		{
			this.customTexture6UrlJSON.val = "NULL";
		}
	}

	// Token: 0x060066D9 RID: 26329 RVA: 0x00152A88 File Offset: 0x00150E88
	protected void SyncCustomTexture6Tile(float f)
	{
		Vector2 scale;
		scale.x = this.customTexture6TileXJSON.val;
		scale.y = this.customTexture6TileYJSON.val;
		this.SetTextureGroupTextureScale(this.textureGroup1, 5, scale);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture6TileXJSON != null)
				{
					materialOptions.customTexture6TileXJSON.val = this.customTexture6TileXJSON.val;
					materialOptions.customTexture6TileYJSON.val = this.customTexture6TileYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066DA RID: 26330 RVA: 0x00152B5C File Offset: 0x00150F5C
	protected void SyncCustomTexture6Offset(float f)
	{
		Vector2 offset;
		offset.x = this.customTexture6OffsetXJSON.val;
		offset.y = this.customTexture6OffsetYJSON.val;
		this.SetTextureGroupTextureOffset(this.textureGroup1, 5, offset);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.customTexture6OffsetXJSON != null)
				{
					materialOptions.customTexture6OffsetXJSON.val = this.customTexture6OffsetXJSON.val;
					materialOptions.customTexture6OffsetYJSON.val = this.customTexture6OffsetYJSON.val;
				}
			}
		}
	}

	// Token: 0x060066DB RID: 26331 RVA: 0x00152C30 File Offset: 0x00151030
	protected void BeginBrowse(JSONStorableUrl jsurl)
	{
		if (!FileManager.IsDirectoryInPackage(this.customTextureFolder) && !this.customTextureFolder.Contains(":") && FileManager.IsSecureWritePath(this.customTextureFolder) && !FileManager.DirectoryExists(this.customTextureFolder, false, false))
		{
			FileManager.CreateDirectory(this.customTextureFolder);
		}
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(this.customTextureFolder, true, true, true, true);
		shortCutsForDirectory.Insert(0, new ShortCut
		{
			displayName = "Root",
			path = Path.GetFullPath(".")
		});
		if (this.customTexturePackageFolder != null)
		{
			VarDirectoryEntry varDirectoryEntry = FileManager.GetVarDirectoryEntry(this.customTexturePackageFolder);
			if (varDirectoryEntry != null)
			{
				shortCutsForDirectory.Add(new ShortCut
				{
					package = varDirectoryEntry.Package.Uid,
					displayName = varDirectoryEntry.InternalSlashPath,
					path = varDirectoryEntry.SlashPath
				});
			}
		}
		jsurl.shortCuts = shortCutsForDirectory;
	}

	// Token: 0x060066DC RID: 26332 RVA: 0x00152D24 File Offset: 0x00151124
	protected void CreateUVTemplateTexture(Mesh mesh, System.Drawing.Color backgroundColor, System.Drawing.Color lineColor, string suffix = "UVTemplate")
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		foreach (int key in this.paramMaterialSlots)
		{
			dictionary.Add(key, true);
		}
		int num = 4096;
		int num2 = 4096;
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.BGRA32, false, false);
		string text = base.storeId + suffix;
		text = Regex.Replace(text, ".*:", string.Empty);
		texture2D.name = text;
		Bitmap bitmap = new Bitmap(4096, 4096, PixelFormat.Format32bppArgb);
		System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
		Rectangle rect = new Rectangle(0, 0, num, num2);
		Brush brush = new SolidBrush(backgroundColor);
		Brush brush2 = new SolidBrush(lineColor);
		Pen pen = new Pen(brush2, 2f);
		graphics.FillRectangle(brush, rect);
		Vector2[] uv = mesh.uv;
		for (int j = 0; j < mesh.subMeshCount; j++)
		{
			if (dictionary.ContainsKey(j))
			{
				int[] triangles = mesh.GetTriangles(j);
				for (int k = 0; k < triangles.Length; k += 3)
				{
					int num3 = triangles[k];
					int num4 = triangles[k + 1];
					int num5 = triangles[k + 2];
					Vector2 vector = uv[num3];
					vector.x *= (float)num;
					vector.y *= (float)num2;
					Vector2 vector2 = uv[num4];
					vector2.x *= (float)num;
					vector2.y *= (float)num2;
					Vector2 vector3 = uv[num5];
					vector3.x *= (float)num;
					vector3.y *= (float)num2;
					graphics.DrawLine(pen, vector.x, vector.y, vector2.x, vector2.y);
					graphics.DrawLine(pen, vector2.x, vector2.y, vector3.x, vector3.y);
					graphics.DrawLine(pen, vector3.x, vector3.y, vector.x, vector.y);
					graphics.DrawLine(pen, vector.x + (float)num, vector.y, vector2.x + (float)num, vector2.y);
					graphics.DrawLine(pen, vector2.x + (float)num, vector2.y, vector3.x + (float)num, vector3.y);
					graphics.DrawLine(pen, vector3.x + (float)num, vector3.y, vector.x + (float)num, vector.y);
					graphics.DrawLine(pen, vector.x + (float)num, vector.y + (float)num2, vector2.x + (float)num, vector2.y + (float)num2);
					graphics.DrawLine(pen, vector2.x + (float)num, vector2.y + (float)num2, vector3.x + (float)num, vector3.y + (float)num2);
					graphics.DrawLine(pen, vector3.x + (float)num, vector3.y + (float)num2, vector.x + (float)num, vector.y + (float)num2);
					graphics.DrawLine(pen, vector.x + (float)num, vector.y - (float)num2, vector2.x + (float)num, vector2.y - (float)num2);
					graphics.DrawLine(pen, vector2.x + (float)num, vector2.y - (float)num2, vector3.x + (float)num, vector3.y - (float)num2);
					graphics.DrawLine(pen, vector3.x + (float)num, vector3.y - (float)num2, vector.x + (float)num, vector.y - (float)num2);
					graphics.DrawLine(pen, vector.x - (float)num, vector.y, vector2.x - (float)num, vector2.y);
					graphics.DrawLine(pen, vector2.x - (float)num, vector2.y, vector3.x - (float)num, vector3.y);
					graphics.DrawLine(pen, vector3.x - (float)num, vector3.y, vector.x - (float)num, vector.y);
					graphics.DrawLine(pen, vector.x - (float)num, vector.y - (float)num2, vector2.x - (float)num, vector2.y - (float)num2);
					graphics.DrawLine(pen, vector2.x - (float)num, vector2.y - (float)num2, vector3.x - (float)num, vector3.y - (float)num2);
					graphics.DrawLine(pen, vector3.x - (float)num, vector3.y - (float)num2, vector.x - (float)num, vector.y - (float)num2);
					graphics.DrawLine(pen, vector.x - (float)num, vector.y + (float)num2, vector2.x - (float)num, vector2.y + (float)num2);
					graphics.DrawLine(pen, vector2.x - (float)num, vector2.y + (float)num2, vector3.x - (float)num, vector3.y + (float)num2);
					graphics.DrawLine(pen, vector3.x - (float)num, vector3.y + (float)num2, vector.x - (float)num, vector.y + (float)num2);
					graphics.DrawLine(pen, vector.x, vector.y + (float)num2, vector2.x, vector2.y + (float)num2);
					graphics.DrawLine(pen, vector2.x, vector2.y + (float)num2, vector3.x, vector3.y + (float)num2);
					graphics.DrawLine(pen, vector3.x, vector3.y + (float)num2, vector.x, vector.y + (float)num2);
					graphics.DrawLine(pen, vector.x, vector.y - (float)num2, vector2.x, vector2.y - (float)num2);
					graphics.DrawLine(pen, vector2.x, vector2.y - (float)num2, vector3.x, vector3.y - (float)num2);
					graphics.DrawLine(pen, vector3.x, vector3.y - (float)num2, vector.x, vector.y - (float)num2);
				}
			}
		}
		BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
		int size = num * num2 * 4;
		texture2D.LoadRawTextureData(bitmapData.Scan0, size);
		bitmap.UnlockBits(bitmapData);
		byte[] bytes = texture2D.EncodeToPNG();
		string text2 = this.customTextureFolder;
		text2 = Regex.Replace(text2, ".*:\\\\", string.Empty);
		if (text2 != string.Empty && !Regex.IsMatch(text2, "/$"))
		{
			text2 += "/";
		}
		if (!FileManager.DirectoryExists(text2, false, false) && FileManager.IsSecureWritePath(text2))
		{
			FileManager.CreateDirectory(text2);
		}
		if (FileManager.DirectoryExists(text2, false, false))
		{
			FileManager.WriteAllBytes(text2 + text + ".png", bytes);
		}
		pen.Dispose();
		brush.Dispose();
		brush2.Dispose();
		graphics.Dispose();
		bitmap.Dispose();
		UnityEngine.Object.Destroy(texture2D);
	}

	// Token: 0x060066DD RID: 26333 RVA: 0x001534FC File Offset: 0x001518FC
	public virtual Mesh GetMesh()
	{
		Mesh result = null;
		if (this.meshFilter != null)
		{
			result = this.meshFilter.mesh;
		}
		return result;
	}

	// Token: 0x060066DE RID: 26334 RVA: 0x0015352C File Offset: 0x0015192C
	public virtual void CreateUVTemplateTexture()
	{
		Mesh mesh = this.GetMesh();
		if (mesh != null)
		{
			this.CreateUVTemplateTexture(mesh, System.Drawing.Color.White, System.Drawing.Color.Black, "UVTemplate");
		}
	}

	// Token: 0x060066DF RID: 26335 RVA: 0x00153564 File Offset: 0x00151964
	public virtual void CreateSimTemplateTexture()
	{
		Mesh mesh = this.GetMesh();
		if (mesh != null)
		{
			this.CreateUVTemplateTexture(mesh, System.Drawing.Color.Black, System.Drawing.Color.Blue, "SimTemplate");
		}
	}

	// Token: 0x060066E0 RID: 26336 RVA: 0x0015359C File Offset: 0x0015199C
	public void CopyUI()
	{
		if (this.copyUIFrom != null)
		{
			this.color1DisplayNameText = this.copyUIFrom.color1DisplayNameText;
			this.color1Picker = this.copyUIFrom.color1Picker;
			this.color1Container = this.copyUIFrom.color1Container;
			this.color2DisplayNameText = this.copyUIFrom.color2DisplayNameText;
			this.color2Picker = this.copyUIFrom.color2Picker;
			this.color2Container = this.copyUIFrom.color2Container;
			this.color3DisplayNameText = this.copyUIFrom.color3DisplayNameText;
			this.color3Picker = this.copyUIFrom.color3Picker;
			this.color3Container = this.copyUIFrom.color3Container;
			this.param1DisplayNameText = this.copyUIFrom.param1DisplayNameText;
			this.param1DisplayNameTextAlt = this.copyUIFrom.param1DisplayNameTextAlt;
			this.param1Slider = this.copyUIFrom.param1Slider;
			this.param1SliderAlt = this.copyUIFrom.param1SliderAlt;
			this.param2DisplayNameText = this.copyUIFrom.param2DisplayNameText;
			this.param2DisplayNameTextAlt = this.copyUIFrom.param2DisplayNameTextAlt;
			this.param2Slider = this.copyUIFrom.param2Slider;
			this.param2SliderAlt = this.copyUIFrom.param2SliderAlt;
			this.param3DisplayNameText = this.copyUIFrom.param3DisplayNameText;
			this.param3DisplayNameTextAlt = this.copyUIFrom.param3DisplayNameTextAlt;
			this.param3Slider = this.copyUIFrom.param3Slider;
			this.param3SliderAlt = this.copyUIFrom.param3SliderAlt;
			this.param4DisplayNameText = this.copyUIFrom.param4DisplayNameText;
			this.param4DisplayNameTextAlt = this.copyUIFrom.param4DisplayNameTextAlt;
			this.param4Slider = this.copyUIFrom.param4Slider;
			this.param4SliderAlt = this.copyUIFrom.param4SliderAlt;
			this.param5DisplayNameText = this.copyUIFrom.param5DisplayNameText;
			this.param5DisplayNameTextAlt = this.copyUIFrom.param5DisplayNameTextAlt;
			this.param5Slider = this.copyUIFrom.param5Slider;
			this.param5SliderAlt = this.copyUIFrom.param5SliderAlt;
			this.param6DisplayNameText = this.copyUIFrom.param6DisplayNameText;
			this.param6DisplayNameTextAlt = this.copyUIFrom.param6DisplayNameTextAlt;
			this.param6Slider = this.copyUIFrom.param6Slider;
			this.param6SliderAlt = this.copyUIFrom.param6SliderAlt;
			this.param7DisplayNameText = this.copyUIFrom.param7DisplayNameText;
			this.param7DisplayNameTextAlt = this.copyUIFrom.param7DisplayNameTextAlt;
			this.param7Slider = this.copyUIFrom.param7Slider;
			this.param7SliderAlt = this.copyUIFrom.param7SliderAlt;
			this.param8DisplayNameText = this.copyUIFrom.param8DisplayNameText;
			this.param8DisplayNameTextAlt = this.copyUIFrom.param8DisplayNameTextAlt;
			this.param8Slider = this.copyUIFrom.param8Slider;
			this.param8SliderAlt = this.copyUIFrom.param8SliderAlt;
			this.param9DisplayNameText = this.copyUIFrom.param9DisplayNameText;
			this.param9DisplayNameTextAlt = this.copyUIFrom.param9DisplayNameTextAlt;
			this.param9Slider = this.copyUIFrom.param9Slider;
			this.param9SliderAlt = this.copyUIFrom.param9SliderAlt;
			this.param10DisplayNameText = this.copyUIFrom.param10DisplayNameText;
			this.param10DisplayNameTextAlt = this.copyUIFrom.param10DisplayNameTextAlt;
			this.param10Slider = this.copyUIFrom.param10Slider;
			this.param10SliderAlt = this.copyUIFrom.param10SliderAlt;
			this.textureGroup1Popup = this.copyUIFrom.textureGroup1Popup;
			this.textureGroup1PopupAlt = this.copyUIFrom.textureGroup1PopupAlt;
			this.textureGroup2Popup = this.copyUIFrom.textureGroup2Popup;
			this.textureGroup2PopupAlt = this.copyUIFrom.textureGroup2PopupAlt;
			this.textureGroup3Popup = this.copyUIFrom.textureGroup3Popup;
			this.textureGroup3PopupAlt = this.copyUIFrom.textureGroup3PopupAlt;
			this.textureGroup4Popup = this.copyUIFrom.textureGroup4Popup;
			this.textureGroup4PopupAlt = this.copyUIFrom.textureGroup4PopupAlt;
			this.textureGroup5Popup = this.copyUIFrom.textureGroup5Popup;
			this.textureGroup5PopupAlt = this.copyUIFrom.textureGroup5PopupAlt;
		}
	}

	// Token: 0x060066E1 RID: 26337 RVA: 0x001539A8 File Offset: 0x00151DA8
	public void CopyParams()
	{
		if (this.copyUIFrom != null)
		{
			this.paramMaterialSlots = (int[])this.copyUIFrom.paramMaterialSlots.Clone();
			this.color1Name = this.copyUIFrom.color1Name;
			this.color1Name2 = this.copyUIFrom.color1Name2;
			this.color1DisplayName = this.copyUIFrom.color1DisplayName;
			this.color2Name = this.copyUIFrom.color2Name;
			this.color2DisplayName = this.copyUIFrom.color2DisplayName;
			this.color3Name = this.copyUIFrom.color3Name;
			this.color3DisplayName = this.copyUIFrom.color3DisplayName;
			this.param1Name = this.copyUIFrom.param1Name;
			this.param1DisplayName = this.copyUIFrom.param1DisplayName;
			this.param1MaxValue = this.copyUIFrom.param1MaxValue;
			this.param1MinValue = this.copyUIFrom.param1MinValue;
			this.param2Name = this.copyUIFrom.param2Name;
			this.param2DisplayName = this.copyUIFrom.param2DisplayName;
			this.param2MaxValue = this.copyUIFrom.param2MaxValue;
			this.param2MinValue = this.copyUIFrom.param2MinValue;
			this.param3Name = this.copyUIFrom.param3Name;
			this.param3DisplayName = this.copyUIFrom.param3DisplayName;
			this.param3MaxValue = this.copyUIFrom.param3MaxValue;
			this.param3MinValue = this.copyUIFrom.param3MinValue;
			this.param4Name = this.copyUIFrom.param4Name;
			this.param4DisplayName = this.copyUIFrom.param4DisplayName;
			this.param4MaxValue = this.copyUIFrom.param4MaxValue;
			this.param4MinValue = this.copyUIFrom.param4MinValue;
			this.param5Name = this.copyUIFrom.param5Name;
			this.param5DisplayName = this.copyUIFrom.param5DisplayName;
			this.param5MaxValue = this.copyUIFrom.param5MaxValue;
			this.param5MinValue = this.copyUIFrom.param5MinValue;
			this.param6Name = this.copyUIFrom.param6Name;
			this.param6DisplayName = this.copyUIFrom.param6DisplayName;
			this.param6MaxValue = this.copyUIFrom.param6MaxValue;
			this.param6MinValue = this.copyUIFrom.param6MinValue;
			this.param7Name = this.copyUIFrom.param7Name;
			this.param7DisplayName = this.copyUIFrom.param7DisplayName;
			this.param7MaxValue = this.copyUIFrom.param7MaxValue;
			this.param7MinValue = this.copyUIFrom.param7MinValue;
			this.param8Name = this.copyUIFrom.param8Name;
			this.param8DisplayName = this.copyUIFrom.param8DisplayName;
			this.param8MaxValue = this.copyUIFrom.param8MaxValue;
			this.param8MinValue = this.copyUIFrom.param8MinValue;
			this.param9Name = this.copyUIFrom.param9Name;
			this.param9DisplayName = this.copyUIFrom.param9DisplayName;
			this.param9MaxValue = this.copyUIFrom.param9MaxValue;
			this.param9MinValue = this.copyUIFrom.param9MinValue;
			this.param10Name = this.copyUIFrom.param10Name;
			this.param10DisplayName = this.copyUIFrom.param10DisplayName;
			this.param10MaxValue = this.copyUIFrom.param10MaxValue;
			this.param10MinValue = this.copyUIFrom.param10MinValue;
		}
	}

	// Token: 0x060066E2 RID: 26338 RVA: 0x00153D00 File Offset: 0x00152100
	public void CopyTextureGroup()
	{
		if (this.copyUIFrom != null)
		{
			MaterialOptionTextureGroup materialOptionTextureGroup;
			string text;
			switch (this.copyFromTextureGroup)
			{
			case 1:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup1;
				text = this.copyUIFrom.startingTextureGroup1Set;
				break;
			case 2:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup2;
				text = this.copyUIFrom.startingTextureGroup2Set;
				break;
			case 3:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup3;
				text = this.copyUIFrom.startingTextureGroup3Set;
				break;
			case 4:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup4;
				text = this.copyUIFrom.startingTextureGroup4Set;
				break;
			case 5:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup5;
				text = this.copyUIFrom.startingTextureGroup5Set;
				break;
			default:
				return;
			}
			MaterialOptionTextureGroup materialOptionTextureGroup2;
			switch (this.copyToTextureGroup)
			{
			case 1:
				materialOptionTextureGroup2 = this.textureGroup1;
				this.startingTextureGroup1Set = text;
				break;
			case 2:
				materialOptionTextureGroup2 = this.textureGroup2;
				this.startingTextureGroup2Set = text;
				break;
			case 3:
				materialOptionTextureGroup2 = this.textureGroup3;
				this.startingTextureGroup3Set = text;
				break;
			case 4:
				materialOptionTextureGroup2 = this.textureGroup4;
				this.startingTextureGroup4Set = text;
				break;
			case 5:
				materialOptionTextureGroup2 = this.textureGroup5;
				this.startingTextureGroup5Set = text;
				break;
			default:
				return;
			}
			if (materialOptionTextureGroup != null && materialOptionTextureGroup2 != null)
			{
				materialOptionTextureGroup2.name = materialOptionTextureGroup.name;
				materialOptionTextureGroup2.textureName = materialOptionTextureGroup.textureName;
				materialOptionTextureGroup2.secondaryTextureName = materialOptionTextureGroup.secondaryTextureName;
				materialOptionTextureGroup2.thirdTextureName = materialOptionTextureGroup.thirdTextureName;
				materialOptionTextureGroup2.fourthTextureName = materialOptionTextureGroup.fourthTextureName;
				materialOptionTextureGroup2.fifthTextureName = materialOptionTextureGroup.fifthTextureName;
				materialOptionTextureGroup2.sixthTextureName = materialOptionTextureGroup.sixthTextureName;
				materialOptionTextureGroup2.mapTexturesToTextureNames = materialOptionTextureGroup.mapTexturesToTextureNames;
				materialOptionTextureGroup2.autoCreateDefaultTexture = materialOptionTextureGroup.autoCreateDefaultTexture;
				materialOptionTextureGroup2.autoCreateDefaultSetName = materialOptionTextureGroup.autoCreateDefaultSetName;
				materialOptionTextureGroup2.autoCreateTextureFilePrefix = materialOptionTextureGroup.autoCreateTextureFilePrefix;
				materialOptionTextureGroup2.autoCreateSetNamePrefix = materialOptionTextureGroup.autoCreateSetNamePrefix;
				materialOptionTextureGroup2.sets = new MaterialOptionTextureSet[materialOptionTextureGroup.sets.Length];
				for (int i = 0; i < materialOptionTextureGroup.sets.Length; i++)
				{
					materialOptionTextureGroup2.sets[i] = new MaterialOptionTextureSet();
					materialOptionTextureGroup2.sets[i].name = materialOptionTextureGroup.sets[i].name;
					materialOptionTextureGroup2.sets[i].textures = (Texture[])materialOptionTextureGroup.sets[i].textures.Clone();
					materialOptionTextureGroup2.sets[i].textures2 = (Texture[])materialOptionTextureGroup.sets[i].textures2.Clone();
				}
			}
		}
	}

	// Token: 0x060066E3 RID: 26339 RVA: 0x00153FA4 File Offset: 0x001523A4
	public void MergeTextureGroup()
	{
		if (this.copyUIFrom != null)
		{
			MaterialOptionTextureGroup materialOptionTextureGroup;
			switch (this.copyFromTextureGroup)
			{
			case 1:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup1;
				break;
			case 2:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup2;
				break;
			case 3:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup3;
				break;
			case 4:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup4;
				break;
			case 5:
				materialOptionTextureGroup = this.copyUIFrom.textureGroup5;
				break;
			default:
				return;
			}
			MaterialOptionTextureGroup materialOptionTextureGroup2;
			switch (this.copyToTextureGroup)
			{
			case 1:
				materialOptionTextureGroup2 = this.textureGroup1;
				break;
			case 2:
				materialOptionTextureGroup2 = this.textureGroup2;
				break;
			case 3:
				materialOptionTextureGroup2 = this.textureGroup3;
				break;
			case 4:
				materialOptionTextureGroup2 = this.textureGroup4;
				break;
			case 5:
				materialOptionTextureGroup2 = this.textureGroup5;
				break;
			default:
				return;
			}
			if (materialOptionTextureGroup != null && materialOptionTextureGroup2 != null)
			{
				int num = materialOptionTextureGroup.sets.Length;
				if (num > 0)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Merging in ",
						num,
						" sets from ",
						this.copyFromTransform.name,
						" ",
						materialOptionTextureGroup.name
					}));
					MaterialOptionTextureSet[] array = new MaterialOptionTextureSet[materialOptionTextureGroup2.sets.Length + num];
					int num2 = 0;
					for (int i = 0; i < materialOptionTextureGroup2.sets.Length; i++)
					{
						array[i] = materialOptionTextureGroup2.sets[i];
						num2++;
					}
					for (int j = 0; j < materialOptionTextureGroup.sets.Length; j++)
					{
						array[num2] = new MaterialOptionTextureSet();
						array[num2].name = materialOptionTextureGroup2.autoCreateSetNamePrefix + " " + (num2 + 1).ToString();
						array[num2].textures = (Texture[])materialOptionTextureGroup.sets[j].textures.Clone();
						num2++;
					}
					materialOptionTextureGroup2.sets = array;
				}
			}
		}
	}

	// Token: 0x060066E4 RID: 26340 RVA: 0x001541D8 File Offset: 0x001525D8
	public virtual void SetMaterialHide(Material m, bool b)
	{
		if (m != null)
		{
			Shader shader;
			if (b)
			{
				if (!this.materialToOriginalShader.ContainsKey(m))
				{
					this.materialToOriginalShader.Add(m, m.shader);
				}
				m.shader = this.hideShader;
			}
			else if (this.materialToOriginalShader.TryGetValue(m, out shader))
			{
				m.shader = shader;
			}
		}
	}

	// Token: 0x060066E5 RID: 26341 RVA: 0x00154248 File Offset: 0x00152648
	protected virtual void SetMaterialHide(bool b)
	{
		if (this.hideShader != null)
		{
			if (this.materialToOriginalShader == null)
			{
				this.materialToOriginalShader = new Dictionary<Material, Shader>();
			}
			if (this.paramMaterialSlots != null)
			{
				foreach (Renderer renderer in this.renderers)
				{
					for (int j = 0; j < this.paramMaterialSlots.Length; j++)
					{
						int num = this.paramMaterialSlots[j];
						Material[] array2;
						if (Application.isPlaying)
						{
							array2 = renderer.materials;
						}
						else
						{
							array2 = renderer.sharedMaterials;
						}
						if (num < array2.Length)
						{
							Material m = array2[num];
							this.SetMaterialHide(m, b);
						}
					}
				}
			}
			if (this.paramMaterialSlots2 != null)
			{
				foreach (Renderer renderer2 in this.renderers2)
				{
					for (int l = 0; l < this.paramMaterialSlots2.Length; l++)
					{
						int num2 = this.paramMaterialSlots2[l];
						Material[] array4;
						if (Application.isPlaying)
						{
							array4 = renderer2.materials;
						}
						else
						{
							array4 = renderer2.sharedMaterials;
						}
						if (num2 < array4.Length)
						{
							Material m2 = array4[num2];
							this.SetMaterialHide(m2, b);
						}
					}
				}
			}
		}
	}

	// Token: 0x060066E6 RID: 26342 RVA: 0x0015439C File Offset: 0x0015279C
	protected virtual void SetMaterialRenderQueue(int q)
	{
		if (this.paramMaterialSlots != null)
		{
			foreach (Renderer renderer in this.renderers)
			{
				for (int j = 0; j < this.paramMaterialSlots.Length; j++)
				{
					int num = this.paramMaterialSlots[j];
					Material[] array2;
					if (Application.isPlaying)
					{
						array2 = renderer.materials;
					}
					else
					{
						array2 = renderer.sharedMaterials;
					}
					if (num < array2.Length)
					{
						Material material = array2[num];
						material.renderQueue = q;
					}
				}
			}
			if (this.rawImageMaterials != null)
			{
				foreach (Material material2 in this.rawImageMaterials)
				{
					material2.renderQueue = q;
				}
			}
		}
		if (this.paramMaterialSlots2 != null)
		{
			foreach (Renderer renderer2 in this.renderers2)
			{
				for (int m = 0; m < this.paramMaterialSlots2.Length; m++)
				{
					int num2 = this.paramMaterialSlots2[m];
					Material[] array5;
					if (Application.isPlaying)
					{
						array5 = renderer2.materials;
					}
					else
					{
						array5 = renderer2.sharedMaterials;
					}
					if (num2 < array5.Length)
					{
						Material material3 = array5[num2];
						material3.renderQueue = q;
					}
				}
			}
		}
	}

	// Token: 0x060066E7 RID: 26343 RVA: 0x00154500 File Offset: 0x00152900
	protected virtual void SetMaterialParam(string name, float value)
	{
		if (this.paramMaterialSlots != null)
		{
			foreach (Renderer renderer in this.renderers)
			{
				for (int j = 0; j < this.paramMaterialSlots.Length; j++)
				{
					int num = this.paramMaterialSlots[j];
					Material[] array2;
					if (Application.isPlaying)
					{
						array2 = renderer.materials;
					}
					else
					{
						array2 = renderer.sharedMaterials;
					}
					if (num < array2.Length)
					{
						Material material = array2[num];
						if (material.HasProperty(name))
						{
							material.SetFloat(name, value);
						}
					}
				}
			}
			if (this.rawImageMaterials != null)
			{
				foreach (Material material2 in this.rawImageMaterials)
				{
					if (material2.HasProperty(name))
					{
						material2.SetFloat(name, value);
					}
				}
			}
		}
		if (this.paramMaterialSlots2 != null)
		{
			foreach (Renderer renderer2 in this.renderers2)
			{
				for (int m = 0; m < this.paramMaterialSlots2.Length; m++)
				{
					int num2 = this.paramMaterialSlots2[m];
					Material[] array5;
					if (Application.isPlaying)
					{
						array5 = renderer2.materials;
					}
					else
					{
						array5 = renderer2.sharedMaterials;
					}
					if (num2 < array5.Length)
					{
						Material material3 = array5[num2];
						if (material3.HasProperty(name))
						{
							material3.SetFloat(name, value);
						}
					}
				}
			}
		}
	}

	// Token: 0x060066E8 RID: 26344 RVA: 0x00154690 File Offset: 0x00152A90
	protected virtual void SetMaterialColor(string name, UnityEngine.Color c)
	{
		if (this.paramMaterialSlots != null)
		{
			foreach (Renderer renderer in this.renderers)
			{
				for (int j = 0; j < this.paramMaterialSlots.Length; j++)
				{
					int num = this.paramMaterialSlots[j];
					Material[] array2;
					if (Application.isPlaying)
					{
						array2 = renderer.materials;
					}
					else
					{
						array2 = renderer.sharedMaterials;
					}
					if (num < array2.Length)
					{
						Material material = array2[num];
						if (material.HasProperty(name))
						{
							material.SetColor(name, c);
						}
					}
				}
			}
			if (this.rawImageMaterials != null)
			{
				foreach (Material material2 in this.rawImageMaterials)
				{
					if (material2.HasProperty(name))
					{
						material2.SetColor(name, c);
					}
				}
			}
		}
		if (this.paramMaterialSlots2 != null)
		{
			foreach (Renderer renderer2 in this.renderers2)
			{
				for (int m = 0; m < this.paramMaterialSlots2.Length; m++)
				{
					int num2 = this.paramMaterialSlots2[m];
					Material[] array5;
					if (Application.isPlaying)
					{
						array5 = renderer2.materials;
					}
					else
					{
						array5 = renderer2.sharedMaterials;
					}
					if (num2 < array5.Length)
					{
						Material material3 = array5[num2];
						if (material3.HasProperty(name))
						{
							material3.SetColor(name, c);
						}
					}
				}
			}
		}
	}

	// Token: 0x060066E9 RID: 26345 RVA: 0x00154820 File Offset: 0x00152C20
	protected void RegisterTexture(Texture2D tex)
	{
		if (tex != null && ImageLoaderThreaded.singleton != null && ImageLoaderThreaded.singleton.RegisterTextureUse(tex))
		{
			if (this.textureUseCount == null)
			{
				this.textureUseCount = new Dictionary<Texture2D, int>();
			}
			int num = 0;
			if (this.textureUseCount.TryGetValue(tex, out num))
			{
				this.textureUseCount.Remove(tex);
			}
			num++;
			this.textureUseCount.Add(tex, num);
		}
	}

	// Token: 0x060066EA RID: 26346 RVA: 0x001548A4 File Offset: 0x00152CA4
	protected void DeregisterTexture(Texture2D tex)
	{
		if (tex != null)
		{
			int num = 0;
			if (ImageLoaderThreaded.singleton != null && this.textureUseCount != null && this.textureUseCount.TryGetValue(tex, out num))
			{
				ImageLoaderThreaded.singleton.DeregisterTextureUse(tex);
				this.textureUseCount.Remove(tex);
				num--;
				if (num > 0)
				{
					this.textureUseCount.Add(tex, num);
				}
			}
		}
	}

	// Token: 0x060066EB RID: 26347 RVA: 0x00154920 File Offset: 0x00152D20
	protected void DeregisterAllTextures()
	{
		if (ImageLoaderThreaded.singleton != null && this.textureUseCount != null)
		{
			foreach (Texture2D texture2D in this.textureUseCount.Keys)
			{
				int num = 0;
				if (this.textureUseCount.TryGetValue(texture2D, out num))
				{
					for (int i = 0; i < num; i++)
					{
						ImageLoaderThreaded.singleton.DeregisterTextureUse(texture2D);
					}
				}
			}
			this.textureUseCount.Clear();
		}
	}

	// Token: 0x060066EC RID: 26348 RVA: 0x001549D4 File Offset: 0x00152DD4
	protected virtual void SetMaterialTexture(Material m, string propName, Texture texture)
	{
		if (m != null && m.HasProperty(propName))
		{
			Texture texture2 = m.GetTexture(propName);
			m.SetTexture(propName, texture);
			if (texture != null && texture is Texture2D)
			{
				this.RegisterTexture(texture as Texture2D);
			}
			if (texture2 != null && texture2 is Texture2D)
			{
				this.DeregisterTexture(texture2 as Texture2D);
			}
		}
	}

	// Token: 0x060066ED RID: 26349 RVA: 0x00154A50 File Offset: 0x00152E50
	protected virtual void SetMaterialTexture(int slot, string propName, Texture texture)
	{
		foreach (Renderer renderer in this.renderers)
		{
			Material[] array2;
			if (Application.isPlaying)
			{
				array2 = renderer.materials;
			}
			else
			{
				array2 = renderer.sharedMaterials;
			}
			if (slot < array2.Length)
			{
				Material m = array2[slot];
				this.SetMaterialTexture(m, propName, texture);
			}
		}
	}

	// Token: 0x060066EE RID: 26350 RVA: 0x00154AB4 File Offset: 0x00152EB4
	protected virtual void SetMaterialTexture2(int slot, string propName, Texture texture)
	{
		foreach (Renderer renderer in this.renderers2)
		{
			Material[] array2;
			if (Application.isPlaying)
			{
				array2 = renderer.materials;
			}
			else
			{
				array2 = renderer.sharedMaterials;
			}
			if (slot < array2.Length)
			{
				Material m = array2[slot];
				this.SetMaterialTexture(m, propName, texture);
			}
		}
	}

	// Token: 0x060066EF RID: 26351 RVA: 0x00154B15 File Offset: 0x00152F15
	protected virtual void SetMaterialTextureScale(Material m, string propName, Vector2 scale)
	{
		if (m != null && m.HasProperty(propName))
		{
			m.SetTextureScale(propName, scale);
		}
	}

	// Token: 0x060066F0 RID: 26352 RVA: 0x00154B38 File Offset: 0x00152F38
	protected virtual void SetMaterialTextureScale(int slot, string propName, Vector2 scale)
	{
		foreach (Renderer renderer in this.renderers)
		{
			Material[] array2;
			if (Application.isPlaying)
			{
				array2 = renderer.materials;
			}
			else
			{
				array2 = renderer.sharedMaterials;
			}
			if (slot < array2.Length)
			{
				Material m = array2[slot];
				this.SetMaterialTextureScale(m, propName, scale);
			}
		}
	}

	// Token: 0x060066F1 RID: 26353 RVA: 0x00154B9C File Offset: 0x00152F9C
	protected virtual void SetMaterialTextureScale2(int slot, string propName, Vector2 scale)
	{
		foreach (Renderer renderer in this.renderers2)
		{
			Material[] array2;
			if (Application.isPlaying)
			{
				array2 = renderer.materials;
			}
			else
			{
				array2 = renderer.sharedMaterials;
			}
			if (slot < array2.Length)
			{
				Material m = array2[slot];
				this.SetMaterialTextureScale(m, propName, scale);
			}
		}
	}

	// Token: 0x060066F2 RID: 26354 RVA: 0x00154BFD File Offset: 0x00152FFD
	protected virtual void SetMaterialTextureOffset(Material m, string propName, Vector2 offset)
	{
		if (m != null && m.HasProperty(propName))
		{
			m.SetTextureOffset(propName, offset);
		}
	}

	// Token: 0x060066F3 RID: 26355 RVA: 0x00154C20 File Offset: 0x00153020
	protected virtual void SetMaterialTextureOffset(int slot, string propName, Vector2 offset)
	{
		foreach (Renderer renderer in this.renderers)
		{
			Material[] array2;
			if (Application.isPlaying)
			{
				array2 = renderer.materials;
			}
			else
			{
				array2 = renderer.sharedMaterials;
			}
			if (slot < array2.Length)
			{
				Material m = array2[slot];
				this.SetMaterialTextureOffset(m, propName, offset);
			}
		}
	}

	// Token: 0x060066F4 RID: 26356 RVA: 0x00154C84 File Offset: 0x00153084
	protected virtual void SetMaterialTextureOffset2(int slot, string propName, Vector2 offset)
	{
		foreach (Renderer renderer in this.renderers2)
		{
			Material[] array2;
			if (Application.isPlaying)
			{
				array2 = renderer.materials;
			}
			else
			{
				array2 = renderer.sharedMaterials;
			}
			if (slot < array2.Length)
			{
				Material m = array2[slot];
				this.SetMaterialTextureOffset(m, propName, offset);
			}
		}
	}

	// Token: 0x060066F5 RID: 26357 RVA: 0x00154CE8 File Offset: 0x001530E8
	protected virtual void SetParam1CurrentValue(float val)
	{
		this.param1CurrentValue = val;
		this.SetMaterialParam(this.param1Name, this.param1CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param1JSONParam != null)
				{
					materialOptions.param1JSONParam.val = this.param1JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066F6 RID: 26358 RVA: 0x00154D90 File Offset: 0x00153190
	protected virtual void SetParam2CurrentValue(float val)
	{
		this.param2CurrentValue = val;
		this.SetMaterialParam(this.param2Name, this.param2CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param2JSONParam != null)
				{
					materialOptions.param2JSONParam.val = this.param2JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066F7 RID: 26359 RVA: 0x00154E38 File Offset: 0x00153238
	protected virtual void SetParam3CurrentValue(float val)
	{
		this.param3CurrentValue = val;
		this.SetMaterialParam(this.param3Name, this.param3CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param3JSONParam != null)
				{
					materialOptions.param3JSONParam.val = this.param3JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066F8 RID: 26360 RVA: 0x00154EE0 File Offset: 0x001532E0
	protected virtual void SetParam4CurrentValue(float val)
	{
		this.param4CurrentValue = val;
		this.SetMaterialParam(this.param4Name, this.param4CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param4JSONParam != null)
				{
					materialOptions.param4JSONParam.val = this.param4JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066F9 RID: 26361 RVA: 0x00154F88 File Offset: 0x00153388
	protected virtual void SetParam5CurrentValue(float val)
	{
		this.param5CurrentValue = val;
		this.SetMaterialParam(this.param5Name, this.param5CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param5JSONParam != null)
				{
					materialOptions.param5JSONParam.val = this.param5JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066FA RID: 26362 RVA: 0x00155030 File Offset: 0x00153430
	protected virtual void SetParam6CurrentValue(float val)
	{
		this.param6CurrentValue = val;
		this.SetMaterialParam(this.param6Name, this.param6CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param6JSONParam != null)
				{
					materialOptions.param6JSONParam.val = this.param6JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066FB RID: 26363 RVA: 0x001550D8 File Offset: 0x001534D8
	protected virtual void SetParam7CurrentValue(float val)
	{
		this.param7CurrentValue = val;
		this.SetMaterialParam(this.param7Name, this.param7CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param7JSONParam != null)
				{
					materialOptions.param7JSONParam.val = this.param7JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066FC RID: 26364 RVA: 0x00155180 File Offset: 0x00153580
	protected virtual void SetParam8CurrentValue(float val)
	{
		this.param8CurrentValue = val;
		this.SetMaterialParam(this.param8Name, this.param8CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param8JSONParam != null)
				{
					materialOptions.param8JSONParam.val = this.param8JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066FD RID: 26365 RVA: 0x00155228 File Offset: 0x00153628
	protected virtual void SetParam9CurrentValue(float val)
	{
		this.param9CurrentValue = val;
		this.SetMaterialParam(this.param9Name, this.param9CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param9JSONParam != null)
				{
					materialOptions.param9JSONParam.val = this.param1JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066FE RID: 26366 RVA: 0x001552D0 File Offset: 0x001536D0
	protected virtual void SetParam10CurrentValue(float val)
	{
		this.param10CurrentValue = val;
		this.SetMaterialParam(this.param10Name, this.param10CurrentValue);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.param10JSONParam != null)
				{
					materialOptions.param10JSONParam.val = this.param10JSONParam.val;
				}
			}
		}
	}

	// Token: 0x060066FF RID: 26367 RVA: 0x00155378 File Offset: 0x00153778
	public virtual void SetColor1(UnityEngine.Color c)
	{
		if (this.color1JSONParam != null)
		{
			this.color1JSONParam.SetColor(c);
		}
	}

	// Token: 0x06006700 RID: 26368 RVA: 0x00155391 File Offset: 0x00153791
	public virtual void SetColor2(UnityEngine.Color c)
	{
		if (this.color2JSONParam != null)
		{
			this.color2JSONParam.SetColor(c);
		}
	}

	// Token: 0x06006701 RID: 26369 RVA: 0x001553AA File Offset: 0x001537AA
	public virtual void SetColor3(UnityEngine.Color c)
	{
		if (this.color3JSONParam != null)
		{
			this.color3JSONParam.SetColor(c);
		}
	}

	// Token: 0x06006702 RID: 26370 RVA: 0x001553C4 File Offset: 0x001537C4
	protected virtual void SetColor1FromHSV(float h, float s, float v)
	{
		this.color1CurrentHSVColor.H = h;
		this.color1CurrentHSVColor.S = s;
		this.color1CurrentHSVColor.V = v;
		this.color1CurrentColor = HSVColorPicker.HSVToRGB(h, s, v);
		this.color1CurrentColor.a = this.color1Alpha;
		this.SetMaterialColor(this.color1Name, this.color1CurrentColor);
		if (this.color1Name2 != null && this.color1Name2 != string.Empty)
		{
			this.SetMaterialColor(this.color1Name2, this.color1CurrentColor);
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.color1JSONParam != null)
				{
					materialOptions.color1JSONParam.val = this.color1JSONParam.val;
				}
			}
		}
	}

	// Token: 0x06006703 RID: 26371 RVA: 0x001554D8 File Offset: 0x001538D8
	protected virtual void SetColor2FromHSV(float h, float s, float v)
	{
		this.color2CurrentHSVColor.H = h;
		this.color2CurrentHSVColor.S = s;
		this.color2CurrentHSVColor.V = v;
		this.color2CurrentColor = HSVColorPicker.HSVToRGB(h, s, v);
		this.color2CurrentColor.a = this.color2Alpha;
		this.SetMaterialColor(this.color2Name, this.color2CurrentColor);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.color2JSONParam != null)
				{
					materialOptions.color2JSONParam.val = this.color2JSONParam.val;
				}
			}
		}
	}

	// Token: 0x06006704 RID: 26372 RVA: 0x001555BC File Offset: 0x001539BC
	protected virtual void SetColor3FromHSV(float h, float s, float v)
	{
		this.color3CurrentHSVColor.H = h;
		this.color3CurrentHSVColor.S = s;
		this.color3CurrentHSVColor.V = v;
		this.color3CurrentColor = HSVColorPicker.HSVToRGB(h, s, v);
		this.color3CurrentColor.a = this.color1Alpha;
		this.SetMaterialColor(this.color3Name, this.color3CurrentColor);
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.color3JSONParam != null)
				{
					materialOptions.color3JSONParam.val = this.color3JSONParam.val;
				}
			}
		}
	}

	// Token: 0x06006705 RID: 26373 RVA: 0x001556A0 File Offset: 0x00153AA0
	protected virtual void SetTextureGroupTextureScale(MaterialOptionTextureGroup tg, int textureNumber, Vector2 scale)
	{
		string text;
		switch (textureNumber)
		{
		case 0:
			text = tg.textureName;
			break;
		case 1:
			text = tg.secondaryTextureName;
			break;
		case 2:
			text = tg.thirdTextureName;
			break;
		case 3:
			text = tg.fourthTextureName;
			break;
		case 4:
			text = tg.fifthTextureName;
			break;
		case 5:
			text = tg.sixthTextureName;
			break;
		default:
			text = null;
			break;
		}
		int[] materialSlots = tg.materialSlots;
		int[] materialSlots2 = tg.materialSlots2;
		if (text != null)
		{
			if (materialSlots != null)
			{
				for (int i = 0; i < materialSlots.Length; i++)
				{
					this.SetMaterialTextureScale(materialSlots[i], text, scale);
				}
			}
			if (materialSlots2 != null)
			{
				for (int j = 0; j < materialSlots2.Length; j++)
				{
					this.SetMaterialTextureScale2(materialSlots2[j], text, scale);
				}
			}
		}
	}

	// Token: 0x06006706 RID: 26374 RVA: 0x00155784 File Offset: 0x00153B84
	protected virtual void SetTextureGroupTextureOffset(MaterialOptionTextureGroup tg, int textureNumber, Vector2 offset)
	{
		string text;
		switch (textureNumber)
		{
		case 0:
			text = tg.textureName;
			break;
		case 1:
			text = tg.secondaryTextureName;
			break;
		case 2:
			text = tg.thirdTextureName;
			break;
		case 3:
			text = tg.fourthTextureName;
			break;
		case 4:
			text = tg.fifthTextureName;
			break;
		case 5:
			text = tg.sixthTextureName;
			break;
		default:
			text = null;
			break;
		}
		int[] materialSlots = tg.materialSlots;
		int[] materialSlots2 = tg.materialSlots2;
		if (text != null)
		{
			if (materialSlots != null)
			{
				for (int i = 0; i < materialSlots.Length; i++)
				{
					this.SetMaterialTextureOffset(materialSlots[i], text, offset);
				}
			}
			if (materialSlots2 != null)
			{
				for (int j = 0; j < materialSlots2.Length; j++)
				{
					this.SetMaterialTextureOffset2(materialSlots2[j], text, offset);
				}
			}
		}
	}

	// Token: 0x06006707 RID: 26375 RVA: 0x00155868 File Offset: 0x00153C68
	protected virtual void SetTextureGroupTexture(MaterialOptionTextureGroup tg, int textureNumber, Texture texture1, Texture texture2)
	{
		string text;
		switch (textureNumber)
		{
		case 0:
			text = tg.textureName;
			break;
		case 1:
			text = tg.secondaryTextureName;
			break;
		case 2:
			text = tg.thirdTextureName;
			break;
		case 3:
			text = tg.fourthTextureName;
			break;
		case 4:
			text = tg.fifthTextureName;
			break;
		case 5:
			text = tg.sixthTextureName;
			break;
		default:
			text = null;
			break;
		}
		int[] materialSlots = tg.materialSlots;
		int[] materialSlots2 = tg.materialSlots2;
		if (text != null)
		{
			if (materialSlots != null)
			{
				for (int i = 0; i < materialSlots.Length; i++)
				{
					this.SetMaterialTexture(materialSlots[i], text, texture1);
				}
			}
			if (materialSlots2 != null)
			{
				for (int j = 0; j < materialSlots2.Length; j++)
				{
					this.SetMaterialTexture2(materialSlots2[j], text, texture2);
				}
			}
		}
	}

	// Token: 0x06006708 RID: 26376 RVA: 0x0015594C File Offset: 0x00153D4C
	protected virtual void SetTextureGroupSet(MaterialOptionTextureGroup tg, string setName, int onlyTextureNumber = -1, Texture customTexture = null, bool allowNull = false)
	{
		MaterialOptionTextureSet materialOptionTextureSet = null;
		if (setName != null)
		{
			materialOptionTextureSet = tg.GetSetByName(setName);
		}
		if (materialOptionTextureSet != null)
		{
			Texture[] textures = materialOptionTextureSet.textures;
			Texture[] textures2 = materialOptionTextureSet.textures2;
			int[] materialSlots = tg.materialSlots;
			if (tg.mapTexturesToTextureNames)
			{
				if (onlyTextureNumber != -1)
				{
					Texture texture = null;
					Texture texture2 = null;
					if (customTexture != null || allowNull)
					{
						texture = customTexture;
						texture2 = customTexture;
					}
					else
					{
						if (textures != null && onlyTextureNumber < textures.Length)
						{
							texture = textures[onlyTextureNumber];
						}
						if (textures2 != null && onlyTextureNumber < textures2.Length)
						{
							texture2 = textures2[onlyTextureNumber];
						}
					}
					this.SetTextureGroupTexture(tg, onlyTextureNumber, texture, texture2);
				}
				else
				{
					for (int i = 0; i < textures.Length; i++)
					{
						Texture texture3 = null;
						Texture texture4 = null;
						if (textures != null && i < textures.Length)
						{
							texture3 = textures[i];
						}
						if (textures2 != null && i < textures2.Length)
						{
							texture4 = textures2[i];
						}
						this.SetTextureGroupTexture(tg, i, texture3, texture4);
					}
				}
			}
			else if (materialSlots.Length == textures.Length)
			{
				for (int j = 0; j < materialSlots.Length; j++)
				{
					this.SetMaterialTexture(materialSlots[j], tg.textureName, textures[j]);
					if (tg.secondaryTextureName != null && tg.secondaryTextureName != string.Empty)
					{
						this.SetMaterialTexture(materialSlots[j], tg.secondaryTextureName, textures[j]);
					}
				}
			}
		}
		else if (tg.mapTexturesToTextureNames && onlyTextureNumber != -1)
		{
			this.SetTextureGroupTexture(tg, onlyTextureNumber, customTexture, customTexture);
		}
	}

	// Token: 0x06006709 RID: 26377 RVA: 0x00155AE0 File Offset: 0x00153EE0
	protected virtual void SetTextureGroup1Set(string setName)
	{
		if (this.textureGroup1 != null && this.textureGroup1.sets != null)
		{
			this.SetTextureGroupSet(this.textureGroup1, setName, -1, null, false);
			this.currentTextureGroup1Set = setName;
			if (this.customTexture1 != null || this.customTexture1IsNull)
			{
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 0, this.customTexture1, this.customTexture1IsNull);
			}
			if (this.customTexture2 != null || this.customTexture2IsNull)
			{
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 1, this.customTexture2, this.customTexture2IsNull);
			}
			if (this.customTexture3 != null || this.customTexture3IsNull)
			{
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 2, this.customTexture3, this.customTexture3IsNull);
			}
			if (this.customTexture4 != null || this.customTexture4IsNull)
			{
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 3, this.customTexture4, this.customTexture4IsNull);
			}
			if (this.customTexture5 != null || this.customTexture5IsNull)
			{
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 4, this.customTexture5, this.customTexture5IsNull);
			}
			if (this.customTexture6 != null || this.customTexture6IsNull)
			{
				this.SetTextureGroupSet(this.textureGroup1, this.currentTextureGroup1Set, 5, this.customTexture6, this.customTexture6IsNull);
			}
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.textureGroup1JSON != null)
				{
					materialOptions.textureGroup1JSON.val = this.textureGroup1JSON.val;
				}
			}
		}
	}

	// Token: 0x0600670A RID: 26378 RVA: 0x00155D00 File Offset: 0x00154100
	protected virtual void SetTextureGroup2Set(string setName)
	{
		if (this.textureGroup2 != null && this.textureGroup2.sets != null)
		{
			this.SetTextureGroupSet(this.textureGroup2, setName, -1, null, false);
			this.currentTextureGroup2Set = setName;
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.textureGroup2JSON != null)
				{
					materialOptions.textureGroup2JSON.val = this.textureGroup2JSON.val;
				}
			}
		}
	}

	// Token: 0x0600670B RID: 26379 RVA: 0x00155DC0 File Offset: 0x001541C0
	protected virtual void SetTextureGroup3Set(string setName)
	{
		if (this.textureGroup3 != null && this.textureGroup3.sets != null)
		{
			this.SetTextureGroupSet(this.textureGroup3, setName, -1, null, false);
			this.currentTextureGroup3Set = setName;
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.textureGroup3JSON != null)
				{
					materialOptions.textureGroup3JSON.val = this.textureGroup3JSON.val;
				}
			}
		}
	}

	// Token: 0x0600670C RID: 26380 RVA: 0x00155E80 File Offset: 0x00154280
	protected virtual void SetTextureGroup4Set(string setName)
	{
		if (this.textureGroup4 != null && this.textureGroup4.sets != null)
		{
			this.SetTextureGroupSet(this.textureGroup4, setName, -1, null, false);
			this.currentTextureGroup4Set = setName;
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.textureGroup4JSON != null)
				{
					materialOptions.textureGroup4JSON.val = this.textureGroup4JSON.val;
				}
			}
		}
	}

	// Token: 0x0600670D RID: 26381 RVA: 0x00155F40 File Offset: 0x00154340
	protected virtual void SetTextureGroup5Set(string setName)
	{
		if (this.textureGroup5 != null && this.textureGroup5.sets != null)
		{
			this.SetTextureGroupSet(this.textureGroup5, setName, -1, null, false);
			this.currentTextureGroup5Set = setName;
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				if (materialOptions.textureGroup5JSON != null)
				{
					materialOptions.textureGroup5JSON.val = this.textureGroup5JSON.val;
				}
			}
		}
	}

	// Token: 0x0600670E RID: 26382 RVA: 0x00156000 File Offset: 0x00154400
	public void CopyUI(MaterialOptionsUI moui)
	{
		this.color1Container = moui.color1Container;
		this.color1DisplayNameText = moui.color1DisplayNameText;
		this.color1Picker = moui.color1Picker;
		this.color2Container = moui.color2Container;
		this.color2DisplayNameText = moui.color2DisplayNameText;
		this.color2Picker = moui.color2Picker;
		this.color3Container = moui.color3Container;
		this.color3DisplayNameText = moui.color3DisplayNameText;
		this.color3Picker = moui.color3Picker;
		this.param1Slider = moui.param1Slider;
		this.param1DisplayNameText = moui.param1DisplayNameText;
		this.param2Slider = moui.param2Slider;
		this.param2DisplayNameText = moui.param2DisplayNameText;
		this.param3Slider = moui.param3Slider;
		this.param3DisplayNameText = moui.param3DisplayNameText;
		this.param4Slider = moui.param4Slider;
		this.param4DisplayNameText = moui.param4DisplayNameText;
		this.param5Slider = moui.param5Slider;
		this.param5DisplayNameText = moui.param5DisplayNameText;
		this.param6Slider = moui.param6Slider;
		this.param6DisplayNameText = moui.param6DisplayNameText;
		this.param7Slider = moui.param7Slider;
		this.param7DisplayNameText = moui.param7DisplayNameText;
		this.param8Slider = moui.param8Slider;
		this.param8DisplayNameText = moui.param8DisplayNameText;
		this.param9Slider = moui.param9Slider;
		this.param9DisplayNameText = moui.param9DisplayNameText;
		this.param10Slider = moui.param10Slider;
		this.param10DisplayNameText = moui.param10DisplayNameText;
		this.textureGroup1Popup = moui.textureGroup1Popup;
		this.textureGroup2Popup = moui.textureGroup2Popup;
		this.textureGroup3Popup = moui.textureGroup3Popup;
		this.textureGroup4Popup = moui.textureGroup4Popup;
		this.textureGroup5Popup = moui.textureGroup5Popup;
		this.customTexture1FileBrowseButton = moui.customTexture1FileBrowseButton;
		this.customTexture1ReloadButton = moui.customTexture1ReloadButton;
		this.customTexture1ClearButton = moui.customTexture1ClearButton;
		this.customTexture1NullButton = moui.customTexture1NullButton;
		this.customTexture1DefaultButton = moui.customTexture1DefaultButton;
		this.customTexture1UrlText = moui.customTexture1UrlText;
		this.customTexture1Label = moui.customTexture1Label;
		this.customTexture2FileBrowseButton = moui.customTexture2FileBrowseButton;
		this.customTexture2ReloadButton = moui.customTexture2ReloadButton;
		this.customTexture2ClearButton = moui.customTexture2ClearButton;
		this.customTexture2NullButton = moui.customTexture2NullButton;
		this.customTexture2DefaultButton = moui.customTexture2DefaultButton;
		this.customTexture2UrlText = moui.customTexture2UrlText;
		this.customTexture2Label = moui.customTexture2Label;
		this.customTexture3FileBrowseButton = moui.customTexture3FileBrowseButton;
		this.customTexture3ReloadButton = moui.customTexture3ReloadButton;
		this.customTexture3ClearButton = moui.customTexture3ClearButton;
		this.customTexture3NullButton = moui.customTexture3NullButton;
		this.customTexture3DefaultButton = moui.customTexture3DefaultButton;
		this.customTexture3UrlText = moui.customTexture3UrlText;
		this.customTexture3Label = moui.customTexture3Label;
		this.customTexture4FileBrowseButton = moui.customTexture4FileBrowseButton;
		this.customTexture4ReloadButton = moui.customTexture4ReloadButton;
		this.customTexture4ClearButton = moui.customTexture4ClearButton;
		this.customTexture4NullButton = moui.customTexture4NullButton;
		this.customTexture4DefaultButton = moui.customTexture4DefaultButton;
		this.customTexture4UrlText = moui.customTexture4UrlText;
		this.customTexture4Label = moui.customTexture4Label;
		this.customTexture5FileBrowseButton = moui.customTexture5FileBrowseButton;
		this.customTexture5ReloadButton = moui.customTexture5ReloadButton;
		this.customTexture5ClearButton = moui.customTexture5ClearButton;
		this.customTexture5NullButton = moui.customTexture5NullButton;
		this.customTexture5DefaultButton = moui.customTexture5DefaultButton;
		this.customTexture5UrlText = moui.customTexture5UrlText;
		this.customTexture5Label = moui.customTexture5Label;
		this.customTexture6FileBrowseButton = moui.customTexture6FileBrowseButton;
		this.customTexture6ReloadButton = moui.customTexture6ReloadButton;
		this.customTexture6ClearButton = moui.customTexture6ClearButton;
		this.customTexture6NullButton = moui.customTexture6NullButton;
		this.customTexture6DefaultButton = moui.customTexture6DefaultButton;
		this.customTexture6UrlText = moui.customTexture6UrlText;
		this.customTexture6Label = moui.customTexture6Label;
		this.customTexture1TileXSlider = moui.customTexture1TileXSlider;
		this.customTexture1TileYSlider = moui.customTexture1TileYSlider;
		this.customTexture1OffsetXSlider = moui.customTexture1OffsetXSlider;
		this.customTexture1OffsetYSlider = moui.customTexture1OffsetYSlider;
		this.customTexture2TileXSlider = moui.customTexture2TileXSlider;
		this.customTexture2TileYSlider = moui.customTexture2TileYSlider;
		this.customTexture2OffsetXSlider = moui.customTexture2OffsetXSlider;
		this.customTexture2OffsetYSlider = moui.customTexture2OffsetYSlider;
		this.customTexture3TileXSlider = moui.customTexture3TileXSlider;
		this.customTexture3TileYSlider = moui.customTexture3TileYSlider;
		this.customTexture3OffsetXSlider = moui.customTexture3OffsetXSlider;
		this.customTexture3OffsetYSlider = moui.customTexture3OffsetYSlider;
		this.customTexture4TileXSlider = moui.customTexture4TileXSlider;
		this.customTexture4TileYSlider = moui.customTexture4TileYSlider;
		this.customTexture4OffsetXSlider = moui.customTexture4OffsetXSlider;
		this.customTexture4OffsetYSlider = moui.customTexture4OffsetYSlider;
		this.customTexture5TileXSlider = moui.customTexture5TileXSlider;
		this.customTexture5TileYSlider = moui.customTexture5TileYSlider;
		this.customTexture5OffsetXSlider = moui.customTexture5OffsetXSlider;
		this.customTexture5OffsetYSlider = moui.customTexture5OffsetYSlider;
		this.customTexture6TileXSlider = moui.customTexture6TileXSlider;
		this.customTexture6TileYSlider = moui.customTexture6TileYSlider;
		this.customTexture6OffsetXSlider = moui.customTexture6OffsetXSlider;
		this.customTexture6OffsetYSlider = moui.customTexture6OffsetYSlider;
	}

	// Token: 0x0600670F RID: 26383 RVA: 0x001564C0 File Offset: 0x001548C0
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			MaterialOptionsUI componentInChildren = this.UITransform.GetComponentInChildren<MaterialOptionsUI>(true);
			if (componentInChildren != null)
			{
				this.CopyUI(componentInChildren);
				this.restoreAllFromDefaultsAction.button = componentInChildren.restoreFromDefaultsButton;
				this.saveToStore1Action.button = componentInChildren.saveToStore1Button;
				this.restoreAllFromStore1Action.button = componentInChildren.restoreFromStore1Button;
				this.saveToStore2Action.button = componentInChildren.saveToStore2Button;
				this.restoreAllFromStore2Action.button = componentInChildren.restoreFromStore2Button;
				this.saveToStore3Action.button = componentInChildren.saveToStore3Button;
				this.restoreAllFromStore3Action.button = componentInChildren.restoreFromStore3Button;
				if (this.createUVTemplateTextureJSON != null)
				{
					this.createUVTemplateTextureJSON.button = componentInChildren.createUVTemplateTextureButton;
					if (componentInChildren.createUVTemplateTextureButton != null)
					{
						componentInChildren.createUVTemplateTextureButton.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.createUVTemplateTextureButton != null)
				{
					componentInChildren.createUVTemplateTextureButton.gameObject.SetActive(false);
				}
				if (this.createSimTemplateTextureJSON != null)
				{
					this.createSimTemplateTextureJSON.button = componentInChildren.createSimTemplateTextureButton;
					if (componentInChildren.createSimTemplateTextureButton != null)
					{
						componentInChildren.createSimTemplateTextureButton.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.createSimTemplateTextureButton != null)
				{
					componentInChildren.createSimTemplateTextureButton.gameObject.SetActive(false);
				}
				if (this.openTextureFolderInExplorerAction != null)
				{
					this.openTextureFolderInExplorerAction.button = componentInChildren.openTextureFolderInExplorerButton;
					if (componentInChildren.openTextureFolderInExplorerButton != null)
					{
						componentInChildren.openTextureFolderInExplorerButton.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.openTextureFolderInExplorerButton != null)
				{
					componentInChildren.openTextureFolderInExplorerButton.gameObject.SetActive(false);
				}
				this.customNameJSON.inputField = componentInChildren.customNameField;
				if (this.renderQueueJSON != null)
				{
					this.renderQueueJSON.slider = componentInChildren.renderQueueSlider;
				}
				if (this.hideMaterialJSON != null)
				{
					this.hideMaterialJSON.toggle = componentInChildren.hideMaterialToggle;
					if (componentInChildren.hideMaterialToggle != null)
					{
						componentInChildren.hideMaterialToggle.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.hideMaterialToggle != null)
				{
					componentInChildren.hideMaterialToggle.gameObject.SetActive(false);
				}
				if (this.linkToOtherMaterialsJSON != null)
				{
					this.linkToOtherMaterialsJSON.toggle = componentInChildren.linkToOtherMaterialsToggle;
					if (componentInChildren.linkToOtherMaterialsToggle != null)
					{
						componentInChildren.linkToOtherMaterialsToggle.gameObject.SetActive(true);
					}
				}
				else if (componentInChildren.linkToOtherMaterialsToggle != null)
				{
					componentInChildren.linkToOtherMaterialsToggle.gameObject.SetActive(false);
				}
			}
		}
		if (this.color1Picker != null)
		{
			if (this.color1Name != null && this.color1Name != string.Empty && this.materialHasColor1)
			{
				if (this.color1Container != null)
				{
					this.color1Container.gameObject.SetActive(true);
				}
				if (this.color1DisplayNameText != null)
				{
					this.color1DisplayNameText.text = this.color1DisplayName;
				}
				if (this.color1JSONParam != null)
				{
					this.color1JSONParam.colorPicker = this.color1Picker;
				}
			}
			else if (this.color1Container != null)
			{
				this.color1Container.gameObject.SetActive(false);
			}
		}
		if (this.color2Picker != null)
		{
			if (this.color2Name != null && this.color2Name != string.Empty && this.materialHasColor2)
			{
				if (this.color2Container != null)
				{
					this.color2Container.gameObject.SetActive(true);
				}
				if (this.color2DisplayNameText != null)
				{
					this.color2DisplayNameText.text = this.color2DisplayName;
				}
				if (this.color2JSONParam != null)
				{
					this.color2JSONParam.colorPicker = this.color2Picker;
				}
			}
			else if (this.color2Container != null)
			{
				this.color2Container.gameObject.SetActive(false);
			}
		}
		if (this.color3Picker != null)
		{
			if (this.color3Name != null && this.color3Name != string.Empty && this.materialHasColor3)
			{
				if (this.color3Container != null)
				{
					this.color3Container.gameObject.SetActive(true);
				}
				if (this.color3DisplayNameText != null)
				{
					this.color3DisplayNameText.text = this.color3DisplayName;
				}
				if (this.color3JSONParam != null)
				{
					this.color3JSONParam.colorPicker = this.color3Picker;
				}
			}
			else if (this.color3Container != null)
			{
				this.color3Container.gameObject.SetActive(false);
			}
		}
		if (this.param1Slider != null)
		{
			if (this.materialHasParam1)
			{
				this.param1Slider.transform.parent.gameObject.SetActive(true);
				if (this.param1DisplayNameText != null)
				{
					this.param1DisplayNameText.text = this.param1DisplayName;
				}
				if (this.param1JSONParam != null)
				{
					this.param1JSONParam.slider = this.param1Slider;
				}
			}
			else
			{
				this.param1Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param2Slider != null)
		{
			if (this.materialHasParam2)
			{
				this.param2Slider.transform.parent.gameObject.SetActive(true);
				if (this.param2DisplayNameText != null)
				{
					this.param2DisplayNameText.text = this.param2DisplayName;
				}
				if (this.param2JSONParam != null)
				{
					this.param2JSONParam.slider = this.param2Slider;
				}
			}
			else
			{
				this.param2Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param3Slider != null)
		{
			if (this.materialHasParam3)
			{
				this.param3Slider.transform.parent.gameObject.SetActive(true);
				if (this.param3DisplayNameText != null)
				{
					this.param3DisplayNameText.text = this.param3DisplayName;
				}
				if (this.param3JSONParam != null)
				{
					this.param3JSONParam.slider = this.param3Slider;
				}
			}
			else
			{
				this.param3Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param4Slider != null)
		{
			if (this.materialHasParam4)
			{
				this.param4Slider.transform.parent.gameObject.SetActive(true);
				if (this.param4DisplayNameText != null)
				{
					this.param4DisplayNameText.text = this.param4DisplayName;
				}
				if (this.param4JSONParam != null)
				{
					this.param4JSONParam.slider = this.param4Slider;
				}
			}
			else
			{
				this.param4Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param5Slider != null)
		{
			if (this.materialHasParam5)
			{
				this.param5Slider.transform.parent.gameObject.SetActive(true);
				if (this.param5DisplayNameText != null)
				{
					this.param5DisplayNameText.text = this.param5DisplayName;
				}
				if (this.param5JSONParam != null)
				{
					this.param5JSONParam.slider = this.param5Slider;
				}
			}
			else
			{
				this.param5Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param6Slider != null)
		{
			if (this.materialHasParam6)
			{
				this.param6Slider.transform.parent.gameObject.SetActive(true);
				if (this.param6DisplayNameText != null)
				{
					this.param6DisplayNameText.text = this.param6DisplayName;
				}
				if (this.param6JSONParam != null)
				{
					this.param6JSONParam.slider = this.param6Slider;
				}
			}
			else
			{
				this.param6Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param7Slider != null)
		{
			if (this.materialHasParam7)
			{
				this.param7Slider.transform.parent.gameObject.SetActive(true);
				if (this.param7DisplayNameText != null)
				{
					this.param7DisplayNameText.text = this.param7DisplayName;
				}
				if (this.param7JSONParam != null)
				{
					this.param7JSONParam.slider = this.param7Slider;
				}
			}
			else
			{
				this.param7Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param8Slider != null)
		{
			if (this.materialHasParam8)
			{
				this.param8Slider.transform.parent.gameObject.SetActive(true);
				if (this.param8DisplayNameText != null)
				{
					this.param8DisplayNameText.text = this.param8DisplayName;
				}
				if (this.param8JSONParam != null)
				{
					this.param8JSONParam.slider = this.param8Slider;
				}
			}
			else
			{
				this.param8Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param9Slider != null)
		{
			if (this.materialHasParam9)
			{
				this.param9Slider.transform.parent.gameObject.SetActive(true);
				if (this.param9DisplayNameText != null)
				{
					this.param9DisplayNameText.text = this.param9DisplayName;
				}
				if (this.param9JSONParam != null)
				{
					this.param9JSONParam.slider = this.param9Slider;
				}
			}
			else
			{
				this.param9Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param10Slider != null)
		{
			if (this.materialHasParam10)
			{
				this.param10Slider.transform.parent.gameObject.SetActive(true);
				if (this.param10DisplayNameText != null)
				{
					this.param10DisplayNameText.text = this.param10DisplayName;
				}
				if (this.param10JSONParam != null)
				{
					this.param10JSONParam.slider = this.param10Slider;
				}
			}
			else
			{
				this.param10Slider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup1Popup != null)
		{
			if (this.hasTextureGroup1)
			{
				this.textureGroup1Popup.gameObject.SetActive(true);
				if (this.textureGroup1JSON != null)
				{
					this.textureGroup1JSON.popup = this.textureGroup1Popup;
				}
			}
			else
			{
				this.textureGroup1Popup.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup2Popup != null)
		{
			if (this.hasTextureGroup2)
			{
				this.textureGroup2Popup.gameObject.SetActive(true);
				if (this.textureGroup2JSON != null)
				{
					this.textureGroup2JSON.popup = this.textureGroup2Popup;
				}
			}
			else
			{
				this.textureGroup2Popup.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup3Popup != null)
		{
			if (this.hasTextureGroup3)
			{
				this.textureGroup3Popup.gameObject.SetActive(true);
				if (this.textureGroup3JSON != null)
				{
					this.textureGroup3JSON.popup = this.textureGroup3Popup;
				}
			}
			else
			{
				this.textureGroup3Popup.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup4Popup != null)
		{
			if (this.hasTextureGroup4)
			{
				this.textureGroup4Popup.gameObject.SetActive(true);
				if (this.textureGroup4JSON != null)
				{
					this.textureGroup4JSON.popup = this.textureGroup4Popup;
				}
			}
			else
			{
				this.textureGroup4Popup.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup5Popup != null)
		{
			if (this.hasTextureGroup5)
			{
				this.textureGroup5Popup.gameObject.SetActive(true);
				if (this.textureGroup5JSON != null)
				{
					this.textureGroup5JSON.popup = this.textureGroup5Popup;
				}
			}
			else
			{
				this.textureGroup5Popup.gameObject.SetActive(false);
			}
		}
		if (this.customTexture1TileXJSON != null)
		{
			this.customTexture1TileXJSON.slider = this.customTexture1TileXSlider;
			if (this.customTexture1TileXSlider != null)
			{
				this.customTexture1TileXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture1TileXSlider != null)
		{
			this.customTexture1TileXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture1TileYJSON != null)
		{
			this.customTexture1TileYJSON.slider = this.customTexture1TileYSlider;
			if (this.customTexture1TileYSlider != null)
			{
				this.customTexture1TileYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture1TileYSlider != null)
		{
			this.customTexture1TileYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture1OffsetXJSON != null)
		{
			this.customTexture1OffsetXJSON.slider = this.customTexture1OffsetXSlider;
			if (this.customTexture1OffsetXSlider != null)
			{
				this.customTexture1OffsetXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture1OffsetXSlider != null)
		{
			this.customTexture1OffsetXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture1OffsetYJSON != null)
		{
			this.customTexture1OffsetYJSON.slider = this.customTexture1OffsetYSlider;
			if (this.customTexture1OffsetYSlider != null)
			{
				this.customTexture1OffsetYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture1OffsetYSlider != null)
		{
			this.customTexture1OffsetYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture1UrlJSON != null)
		{
			this.customTexture1UrlJSON.fileBrowseButton = this.customTexture1FileBrowseButton;
			this.customTexture1UrlJSON.reloadButton = this.customTexture1ReloadButton;
			this.customTexture1UrlJSON.clearButton = this.customTexture1ClearButton;
			this.customTexture1UrlJSON.defaultButton = this.customTexture1DefaultButton;
			this.customTexture1UrlJSON.text = this.customTexture1UrlText;
			if (this.customTexture1FileBrowseButton != null)
			{
				this.customTexture1FileBrowseButton.gameObject.SetActive(true);
			}
			if (this.customTexture1ReloadButton != null)
			{
				this.customTexture1ReloadButton.gameObject.SetActive(true);
			}
			if (this.customTexture1ClearButton != null)
			{
				this.customTexture1ClearButton.gameObject.SetActive(true);
			}
			if (this.customTexture1DefaultButton != null)
			{
				this.customTexture1DefaultButton.gameObject.SetActive(true);
			}
			if (this.customTexture1UrlText != null)
			{
				this.customTexture1UrlText.gameObject.SetActive(true);
			}
			if (this.customTexture1Label != null)
			{
				this.customTexture1Label.gameObject.SetActive(true);
				if (this.textureGroup1 != null)
				{
					this.customTexture1Label.text = this.textureGroup1.textureName;
				}
				else
				{
					this.customTexture1Label.text = string.Empty;
				}
			}
			if (this.customTexture1NullButton != null)
			{
				this.customTexture1NullButton.gameObject.SetActive(true);
				this.customTexture1NullButton.onClick.AddListener(new UnityAction(this.SetCustomTexture1ToNull));
			}
		}
		else
		{
			if (this.customTexture1FileBrowseButton != null)
			{
				this.customTexture1FileBrowseButton.gameObject.SetActive(false);
			}
			if (this.customTexture1ReloadButton != null)
			{
				this.customTexture1ReloadButton.gameObject.SetActive(false);
			}
			if (this.customTexture1ClearButton != null)
			{
				this.customTexture1ClearButton.gameObject.SetActive(false);
			}
			if (this.customTexture1DefaultButton != null)
			{
				this.customTexture1DefaultButton.gameObject.SetActive(false);
			}
			if (this.customTexture1UrlText != null)
			{
				this.customTexture1UrlText.gameObject.SetActive(false);
			}
			if (this.customTexture1Label != null)
			{
				this.customTexture1Label.gameObject.SetActive(false);
			}
			if (this.customTexture1NullButton != null)
			{
				this.customTexture1NullButton.gameObject.SetActive(false);
			}
		}
		if (this.customTexture2TileXJSON != null)
		{
			this.customTexture2TileXJSON.slider = this.customTexture2TileXSlider;
			if (this.customTexture2TileXSlider != null)
			{
				this.customTexture2TileXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture2TileXSlider != null)
		{
			this.customTexture2TileXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture2TileYJSON != null)
		{
			this.customTexture2TileYJSON.slider = this.customTexture2TileYSlider;
			if (this.customTexture2TileYSlider != null)
			{
				this.customTexture2TileYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture2TileYSlider != null)
		{
			this.customTexture2TileYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture2OffsetXJSON != null)
		{
			this.customTexture2OffsetXJSON.slider = this.customTexture2OffsetXSlider;
			if (this.customTexture2OffsetXSlider != null)
			{
				this.customTexture2OffsetXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture2OffsetXSlider != null)
		{
			this.customTexture2OffsetXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture2OffsetYJSON != null)
		{
			this.customTexture2OffsetYJSON.slider = this.customTexture2OffsetYSlider;
			if (this.customTexture2OffsetYSlider != null)
			{
				this.customTexture2OffsetYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture2OffsetYSlider != null)
		{
			this.customTexture2OffsetYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture2UrlJSON != null)
		{
			this.customTexture2UrlJSON.fileBrowseButton = this.customTexture2FileBrowseButton;
			this.customTexture2UrlJSON.reloadButton = this.customTexture2ReloadButton;
			this.customTexture2UrlJSON.clearButton = this.customTexture2ClearButton;
			this.customTexture2UrlJSON.defaultButton = this.customTexture2DefaultButton;
			this.customTexture2UrlJSON.text = this.customTexture2UrlText;
			if (this.customTexture2FileBrowseButton != null)
			{
				this.customTexture2FileBrowseButton.gameObject.SetActive(true);
			}
			if (this.customTexture2ReloadButton != null)
			{
				this.customTexture2ReloadButton.gameObject.SetActive(true);
			}
			if (this.customTexture2ClearButton != null)
			{
				this.customTexture2ClearButton.gameObject.SetActive(true);
			}
			if (this.customTexture2DefaultButton != null)
			{
				this.customTexture2DefaultButton.gameObject.SetActive(true);
			}
			if (this.customTexture2UrlText != null)
			{
				this.customTexture2UrlText.gameObject.SetActive(true);
			}
			if (this.customTexture2Label != null)
			{
				this.customTexture2Label.gameObject.SetActive(true);
				if (this.textureGroup1 != null)
				{
					this.customTexture2Label.text = this.textureGroup1.secondaryTextureName;
				}
				else
				{
					this.customTexture2Label.text = string.Empty;
				}
			}
			if (this.customTexture2NullButton != null)
			{
				this.customTexture2NullButton.gameObject.SetActive(true);
				this.customTexture2NullButton.onClick.AddListener(new UnityAction(this.SetCustomTexture2ToNull));
			}
		}
		else
		{
			if (this.customTexture2FileBrowseButton != null)
			{
				this.customTexture2FileBrowseButton.gameObject.SetActive(false);
			}
			if (this.customTexture2ReloadButton != null)
			{
				this.customTexture2ReloadButton.gameObject.SetActive(false);
			}
			if (this.customTexture2ClearButton != null)
			{
				this.customTexture2ClearButton.gameObject.SetActive(false);
			}
			if (this.customTexture2DefaultButton != null)
			{
				this.customTexture2DefaultButton.gameObject.SetActive(false);
			}
			if (this.customTexture2UrlText != null)
			{
				this.customTexture2UrlText.gameObject.SetActive(false);
			}
			if (this.customTexture2Label != null)
			{
				this.customTexture2Label.gameObject.SetActive(false);
			}
			if (this.customTexture2NullButton != null)
			{
				this.customTexture2NullButton.gameObject.SetActive(false);
			}
		}
		if (this.customTexture3TileXJSON != null)
		{
			this.customTexture3TileXJSON.slider = this.customTexture3TileXSlider;
			if (this.customTexture3TileXSlider != null)
			{
				this.customTexture3TileXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture3TileXSlider != null)
		{
			this.customTexture3TileXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture3TileYJSON != null)
		{
			this.customTexture3TileYJSON.slider = this.customTexture3TileYSlider;
			if (this.customTexture3TileYSlider != null)
			{
				this.customTexture3TileYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture3TileYSlider != null)
		{
			this.customTexture3TileYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture3OffsetXJSON != null)
		{
			this.customTexture3OffsetXJSON.slider = this.customTexture3OffsetXSlider;
			if (this.customTexture3OffsetXSlider != null)
			{
				this.customTexture3OffsetXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture3OffsetXSlider != null)
		{
			this.customTexture3OffsetXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture3OffsetYJSON != null)
		{
			this.customTexture3OffsetYJSON.slider = this.customTexture3OffsetYSlider;
			if (this.customTexture3OffsetYSlider != null)
			{
				this.customTexture3OffsetYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture3OffsetYSlider != null)
		{
			this.customTexture3OffsetYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture3UrlJSON != null)
		{
			this.customTexture3UrlJSON.fileBrowseButton = this.customTexture3FileBrowseButton;
			this.customTexture3UrlJSON.reloadButton = this.customTexture3ReloadButton;
			this.customTexture3UrlJSON.clearButton = this.customTexture3ClearButton;
			this.customTexture3UrlJSON.defaultButton = this.customTexture3DefaultButton;
			this.customTexture3UrlJSON.text = this.customTexture3UrlText;
			if (this.customTexture3FileBrowseButton != null)
			{
				this.customTexture3FileBrowseButton.gameObject.SetActive(true);
			}
			if (this.customTexture3ReloadButton != null)
			{
				this.customTexture3ReloadButton.gameObject.SetActive(true);
			}
			if (this.customTexture3ClearButton != null)
			{
				this.customTexture3ClearButton.gameObject.SetActive(true);
			}
			if (this.customTexture3DefaultButton != null)
			{
				this.customTexture3DefaultButton.gameObject.SetActive(true);
			}
			if (this.customTexture3UrlText != null)
			{
				this.customTexture3UrlText.gameObject.SetActive(true);
			}
			if (this.customTexture3Label != null)
			{
				this.customTexture3Label.gameObject.SetActive(true);
				if (this.textureGroup1 != null)
				{
					this.customTexture3Label.text = this.textureGroup1.thirdTextureName;
				}
				else
				{
					this.customTexture3Label.text = string.Empty;
				}
			}
			if (this.customTexture3NullButton != null)
			{
				this.customTexture3NullButton.gameObject.SetActive(true);
				this.customTexture3NullButton.onClick.AddListener(new UnityAction(this.SetCustomTexture3ToNull));
			}
		}
		else
		{
			if (this.customTexture3FileBrowseButton != null)
			{
				this.customTexture3FileBrowseButton.gameObject.SetActive(false);
			}
			if (this.customTexture3ReloadButton != null)
			{
				this.customTexture3ReloadButton.gameObject.SetActive(false);
			}
			if (this.customTexture3ClearButton != null)
			{
				this.customTexture3ClearButton.gameObject.SetActive(false);
			}
			if (this.customTexture3DefaultButton != null)
			{
				this.customTexture3DefaultButton.gameObject.SetActive(false);
			}
			if (this.customTexture3UrlText != null)
			{
				this.customTexture3UrlText.gameObject.SetActive(false);
			}
			if (this.customTexture3Label != null)
			{
				this.customTexture3Label.gameObject.SetActive(false);
			}
			if (this.customTexture3NullButton != null)
			{
				this.customTexture3NullButton.gameObject.SetActive(false);
			}
		}
		if (this.customTexture4TileXJSON != null)
		{
			this.customTexture4TileXJSON.slider = this.customTexture4TileXSlider;
			if (this.customTexture4TileXSlider != null)
			{
				this.customTexture4TileXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture4TileXSlider != null)
		{
			this.customTexture4TileXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture4TileYJSON != null)
		{
			this.customTexture4TileYJSON.slider = this.customTexture4TileYSlider;
			if (this.customTexture4TileYSlider != null)
			{
				this.customTexture4TileYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture4TileYSlider != null)
		{
			this.customTexture4TileYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture4OffsetXJSON != null)
		{
			this.customTexture4OffsetXJSON.slider = this.customTexture4OffsetXSlider;
			if (this.customTexture4OffsetXSlider != null)
			{
				this.customTexture4OffsetXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture4OffsetXSlider != null)
		{
			this.customTexture4OffsetXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture4OffsetYJSON != null)
		{
			this.customTexture4OffsetYJSON.slider = this.customTexture4OffsetYSlider;
			if (this.customTexture4OffsetYSlider != null)
			{
				this.customTexture4OffsetYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture4OffsetYSlider != null)
		{
			this.customTexture4OffsetYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture4UrlJSON != null)
		{
			this.customTexture4UrlJSON.fileBrowseButton = this.customTexture4FileBrowseButton;
			this.customTexture4UrlJSON.reloadButton = this.customTexture4ReloadButton;
			this.customTexture4UrlJSON.clearButton = this.customTexture4ClearButton;
			this.customTexture4UrlJSON.defaultButton = this.customTexture4DefaultButton;
			this.customTexture4UrlJSON.text = this.customTexture4UrlText;
			if (this.customTexture4FileBrowseButton != null)
			{
				this.customTexture4FileBrowseButton.gameObject.SetActive(true);
			}
			if (this.customTexture4ReloadButton != null)
			{
				this.customTexture4ReloadButton.gameObject.SetActive(true);
			}
			if (this.customTexture4ClearButton != null)
			{
				this.customTexture4ClearButton.gameObject.SetActive(true);
			}
			if (this.customTexture4DefaultButton != null)
			{
				this.customTexture4DefaultButton.gameObject.SetActive(true);
			}
			if (this.customTexture4UrlText != null)
			{
				this.customTexture4UrlText.gameObject.SetActive(true);
			}
			if (this.customTexture4Label != null)
			{
				this.customTexture4Label.gameObject.SetActive(true);
				if (this.textureGroup1 != null)
				{
					this.customTexture4Label.text = this.textureGroup1.fourthTextureName;
				}
				else
				{
					this.customTexture4Label.text = string.Empty;
				}
			}
			if (this.customTexture4NullButton != null)
			{
				this.customTexture4NullButton.gameObject.SetActive(true);
				this.customTexture4NullButton.onClick.AddListener(new UnityAction(this.SetCustomTexture4ToNull));
			}
		}
		else
		{
			if (this.customTexture4FileBrowseButton != null)
			{
				this.customTexture4FileBrowseButton.gameObject.SetActive(false);
			}
			if (this.customTexture4ReloadButton != null)
			{
				this.customTexture4ReloadButton.gameObject.SetActive(false);
			}
			if (this.customTexture4ClearButton != null)
			{
				this.customTexture4ClearButton.gameObject.SetActive(false);
			}
			if (this.customTexture4DefaultButton != null)
			{
				this.customTexture4DefaultButton.gameObject.SetActive(false);
			}
			if (this.customTexture4UrlText != null)
			{
				this.customTexture4UrlText.gameObject.SetActive(false);
			}
			if (this.customTexture4Label != null)
			{
				this.customTexture4Label.gameObject.SetActive(false);
			}
			if (this.customTexture4NullButton != null)
			{
				this.customTexture4NullButton.gameObject.SetActive(false);
			}
		}
		if (this.customTexture5TileXJSON != null)
		{
			this.customTexture5TileXJSON.slider = this.customTexture5TileXSlider;
			if (this.customTexture5TileXSlider != null)
			{
				this.customTexture5TileXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture5TileXSlider != null)
		{
			this.customTexture5TileXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture5TileYJSON != null)
		{
			this.customTexture5TileYJSON.slider = this.customTexture5TileYSlider;
			if (this.customTexture5TileYSlider != null)
			{
				this.customTexture5TileYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture5TileYSlider != null)
		{
			this.customTexture5TileYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture5OffsetXJSON != null)
		{
			this.customTexture5OffsetXJSON.slider = this.customTexture5OffsetXSlider;
			if (this.customTexture5OffsetXSlider != null)
			{
				this.customTexture5OffsetXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture5OffsetXSlider != null)
		{
			this.customTexture5OffsetXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture5OffsetYJSON != null)
		{
			this.customTexture5OffsetYJSON.slider = this.customTexture5OffsetYSlider;
			if (this.customTexture5OffsetYSlider != null)
			{
				this.customTexture5OffsetYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture5OffsetYSlider != null)
		{
			this.customTexture5OffsetYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture5UrlJSON != null)
		{
			this.customTexture5UrlJSON.fileBrowseButton = this.customTexture5FileBrowseButton;
			this.customTexture5UrlJSON.reloadButton = this.customTexture5ReloadButton;
			this.customTexture5UrlJSON.clearButton = this.customTexture5ClearButton;
			this.customTexture5UrlJSON.defaultButton = this.customTexture5DefaultButton;
			this.customTexture5UrlJSON.text = this.customTexture5UrlText;
			if (this.customTexture5FileBrowseButton != null)
			{
				this.customTexture5FileBrowseButton.gameObject.SetActive(true);
			}
			if (this.customTexture5ReloadButton != null)
			{
				this.customTexture5ReloadButton.gameObject.SetActive(true);
			}
			if (this.customTexture5ClearButton != null)
			{
				this.customTexture5ClearButton.gameObject.SetActive(true);
			}
			if (this.customTexture5DefaultButton != null)
			{
				this.customTexture5DefaultButton.gameObject.SetActive(true);
			}
			if (this.customTexture5UrlText != null)
			{
				this.customTexture5UrlText.gameObject.SetActive(true);
			}
			if (this.customTexture5Label != null)
			{
				this.customTexture5Label.gameObject.SetActive(true);
				if (this.textureGroup1 != null)
				{
					this.customTexture5Label.text = this.textureGroup1.fifthTextureName;
				}
				else
				{
					this.customTexture5Label.text = string.Empty;
				}
			}
			if (this.customTexture5NullButton != null)
			{
				this.customTexture5NullButton.gameObject.SetActive(true);
				this.customTexture5NullButton.onClick.AddListener(new UnityAction(this.SetCustomTexture5ToNull));
			}
		}
		else
		{
			if (this.customTexture5FileBrowseButton != null)
			{
				this.customTexture5FileBrowseButton.gameObject.SetActive(false);
			}
			if (this.customTexture5ReloadButton != null)
			{
				this.customTexture5ReloadButton.gameObject.SetActive(false);
			}
			if (this.customTexture5ClearButton != null)
			{
				this.customTexture5ClearButton.gameObject.SetActive(false);
			}
			if (this.customTexture5DefaultButton != null)
			{
				this.customTexture5DefaultButton.gameObject.SetActive(false);
			}
			if (this.customTexture5UrlText != null)
			{
				this.customTexture5UrlText.gameObject.SetActive(false);
			}
			if (this.customTexture5Label != null)
			{
				this.customTexture5Label.gameObject.SetActive(false);
			}
			if (this.customTexture5NullButton != null)
			{
				this.customTexture5NullButton.gameObject.SetActive(false);
			}
		}
		if (this.customTexture6TileXJSON != null)
		{
			this.customTexture6TileXJSON.slider = this.customTexture6TileXSlider;
			if (this.customTexture6TileXSlider != null)
			{
				this.customTexture6TileXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture6TileXSlider != null)
		{
			this.customTexture6TileXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture6TileYJSON != null)
		{
			this.customTexture6TileYJSON.slider = this.customTexture6TileYSlider;
			if (this.customTexture6TileYSlider != null)
			{
				this.customTexture6TileYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture6TileYSlider != null)
		{
			this.customTexture6TileYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture6OffsetXJSON != null)
		{
			this.customTexture6OffsetXJSON.slider = this.customTexture6OffsetXSlider;
			if (this.customTexture6OffsetXSlider != null)
			{
				this.customTexture6OffsetXSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture6OffsetXSlider != null)
		{
			this.customTexture6OffsetXSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture6OffsetYJSON != null)
		{
			this.customTexture6OffsetYJSON.slider = this.customTexture6OffsetYSlider;
			if (this.customTexture6OffsetYSlider != null)
			{
				this.customTexture6OffsetYSlider.transform.parent.gameObject.SetActive(true);
			}
		}
		else if (this.customTexture6OffsetYSlider != null)
		{
			this.customTexture6OffsetYSlider.transform.parent.gameObject.SetActive(false);
		}
		if (this.customTexture6UrlJSON != null)
		{
			this.customTexture6UrlJSON.fileBrowseButton = this.customTexture6FileBrowseButton;
			this.customTexture6UrlJSON.reloadButton = this.customTexture6ReloadButton;
			this.customTexture6UrlJSON.clearButton = this.customTexture6ClearButton;
			this.customTexture6UrlJSON.defaultButton = this.customTexture6DefaultButton;
			this.customTexture6UrlJSON.text = this.customTexture6UrlText;
			if (this.customTexture6FileBrowseButton != null)
			{
				this.customTexture6FileBrowseButton.gameObject.SetActive(true);
			}
			if (this.customTexture6ReloadButton != null)
			{
				this.customTexture6ReloadButton.gameObject.SetActive(true);
			}
			if (this.customTexture6ClearButton != null)
			{
				this.customTexture6ClearButton.gameObject.SetActive(true);
			}
			if (this.customTexture6DefaultButton != null)
			{
				this.customTexture6DefaultButton.gameObject.SetActive(true);
			}
			if (this.customTexture6UrlText != null)
			{
				this.customTexture6UrlText.gameObject.SetActive(true);
			}
			if (this.customTexture6Label != null)
			{
				this.customTexture6Label.gameObject.SetActive(true);
				if (this.textureGroup1 != null)
				{
					this.customTexture6Label.text = this.textureGroup1.sixthTextureName;
				}
				else
				{
					this.customTexture6Label.text = string.Empty;
				}
			}
			if (this.customTexture6NullButton != null)
			{
				this.customTexture6NullButton.gameObject.SetActive(true);
				this.customTexture6NullButton.onClick.AddListener(new UnityAction(this.SetCustomTexture6ToNull));
			}
		}
		else
		{
			if (this.customTexture6FileBrowseButton != null)
			{
				this.customTexture6FileBrowseButton.gameObject.SetActive(false);
			}
			if (this.customTexture6ReloadButton != null)
			{
				this.customTexture6ReloadButton.gameObject.SetActive(false);
			}
			if (this.customTexture6ClearButton != null)
			{
				this.customTexture6ClearButton.gameObject.SetActive(false);
			}
			if (this.customTexture6DefaultButton != null)
			{
				this.customTexture6DefaultButton.gameObject.SetActive(false);
			}
			if (this.customTexture6UrlText != null)
			{
				this.customTexture6UrlText.gameObject.SetActive(false);
			}
			if (this.customTexture6Label != null)
			{
				this.customTexture6Label.gameObject.SetActive(false);
			}
			if (this.customTexture6NullButton != null)
			{
				this.customTexture6NullButton.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06006710 RID: 26384 RVA: 0x00158C5C File Offset: 0x0015705C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			MaterialOptionsUI componentInChildren = this.UITransformAlt.GetComponentInChildren<MaterialOptionsUI>(true);
			this.param1SliderAlt = componentInChildren.param1Slider;
			this.param1DisplayNameTextAlt = componentInChildren.param1DisplayNameText;
			this.param2SliderAlt = componentInChildren.param2Slider;
			this.param2DisplayNameTextAlt = componentInChildren.param2DisplayNameText;
			this.param3SliderAlt = componentInChildren.param3Slider;
			this.param3DisplayNameTextAlt = componentInChildren.param3DisplayNameText;
			this.param4SliderAlt = componentInChildren.param4Slider;
			this.param4DisplayNameTextAlt = componentInChildren.param4DisplayNameText;
			this.param5SliderAlt = componentInChildren.param5Slider;
			this.param5DisplayNameTextAlt = componentInChildren.param5DisplayNameText;
			this.param6SliderAlt = componentInChildren.param6Slider;
			this.param6DisplayNameTextAlt = componentInChildren.param6DisplayNameText;
			this.param7SliderAlt = componentInChildren.param7Slider;
			this.param7DisplayNameTextAlt = componentInChildren.param7DisplayNameText;
			this.param8SliderAlt = componentInChildren.param8Slider;
			this.param8DisplayNameTextAlt = componentInChildren.param8DisplayNameText;
			this.param9SliderAlt = componentInChildren.param9Slider;
			this.param9DisplayNameTextAlt = componentInChildren.param9DisplayNameText;
			this.param10SliderAlt = componentInChildren.param10Slider;
			this.param10DisplayNameTextAlt = componentInChildren.param10DisplayNameText;
			this.textureGroup1PopupAlt = componentInChildren.textureGroup1Popup;
			this.textureGroup2PopupAlt = componentInChildren.textureGroup2Popup;
			this.textureGroup3PopupAlt = componentInChildren.textureGroup3Popup;
			this.textureGroup4PopupAlt = componentInChildren.textureGroup4Popup;
			this.textureGroup5PopupAlt = componentInChildren.textureGroup5Popup;
			this.restoreAllFromDefaultsAction.buttonAlt = componentInChildren.restoreFromDefaultsButton;
			this.saveToStore1Action.buttonAlt = componentInChildren.saveToStore1Button;
			this.restoreAllFromStore1Action.buttonAlt = componentInChildren.restoreFromStore1Button;
			this.saveToStore2Action.buttonAlt = componentInChildren.saveToStore2Button;
			this.restoreAllFromStore2Action.buttonAlt = componentInChildren.restoreFromStore2Button;
			this.saveToStore3Action.buttonAlt = componentInChildren.saveToStore3Button;
			this.restoreAllFromStore3Action.buttonAlt = componentInChildren.restoreFromStore3Button;
		}
		if (this.param1SliderAlt != null)
		{
			if (this.materialHasParam1)
			{
				this.param1SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param1DisplayNameTextAlt != null)
				{
					this.param1DisplayNameTextAlt.text = this.param1DisplayName;
				}
				if (this.param1JSONParam != null)
				{
					this.param1JSONParam.sliderAlt = this.param1SliderAlt;
				}
			}
			else
			{
				this.param1SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param2SliderAlt != null)
		{
			if (this.materialHasParam2)
			{
				this.param2SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param2DisplayNameTextAlt != null)
				{
					this.param2DisplayNameTextAlt.text = this.param2DisplayName;
				}
				if (this.param2JSONParam != null)
				{
					this.param2JSONParam.sliderAlt = this.param2SliderAlt;
				}
			}
			else
			{
				this.param2SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param3SliderAlt != null)
		{
			if (this.materialHasParam3)
			{
				this.param3SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param3DisplayNameTextAlt != null)
				{
					this.param3DisplayNameTextAlt.text = this.param3DisplayName;
				}
				if (this.param3JSONParam != null)
				{
					this.param3JSONParam.sliderAlt = this.param3SliderAlt;
				}
			}
			else
			{
				this.param3SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param4SliderAlt != null)
		{
			if (this.materialHasParam4)
			{
				this.param4SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param4DisplayNameTextAlt != null)
				{
					this.param4DisplayNameTextAlt.text = this.param4DisplayName;
				}
				if (this.param4JSONParam != null)
				{
					this.param4JSONParam.sliderAlt = this.param4SliderAlt;
				}
			}
			else
			{
				this.param4SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param5SliderAlt != null)
		{
			if (this.materialHasParam5)
			{
				this.param5SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param5DisplayNameTextAlt != null)
				{
					this.param5DisplayNameTextAlt.text = this.param5DisplayName;
				}
				if (this.param5JSONParam != null)
				{
					this.param5JSONParam.sliderAlt = this.param5SliderAlt;
				}
			}
			else
			{
				this.param5SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param6SliderAlt != null)
		{
			if (this.materialHasParam6)
			{
				this.param6SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param6DisplayNameTextAlt != null)
				{
					this.param6DisplayNameTextAlt.text = this.param6DisplayName;
				}
				if (this.param6JSONParam != null)
				{
					this.param6JSONParam.sliderAlt = this.param6SliderAlt;
				}
			}
			else
			{
				this.param6SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param7SliderAlt != null)
		{
			if (this.materialHasParam7)
			{
				this.param7SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param7DisplayNameTextAlt != null)
				{
					this.param7DisplayNameTextAlt.text = this.param7DisplayName;
				}
				if (this.param7JSONParam != null)
				{
					this.param7JSONParam.sliderAlt = this.param7SliderAlt;
				}
			}
			else
			{
				this.param7SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param8SliderAlt != null)
		{
			if (this.materialHasParam8)
			{
				this.param8SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param8DisplayNameTextAlt != null)
				{
					this.param8DisplayNameTextAlt.text = this.param8DisplayName;
				}
				if (this.param8JSONParam != null)
				{
					this.param8JSONParam.sliderAlt = this.param8SliderAlt;
				}
			}
			else
			{
				this.param8SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param9SliderAlt != null)
		{
			if (this.materialHasParam9)
			{
				this.param9SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param9DisplayNameTextAlt != null)
				{
					this.param9DisplayNameTextAlt.text = this.param9DisplayName;
				}
				if (this.param9JSONParam != null)
				{
					this.param9JSONParam.sliderAlt = this.param9SliderAlt;
				}
			}
			else
			{
				this.param9SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.param10SliderAlt != null)
		{
			if (this.materialHasParam10)
			{
				this.param10SliderAlt.transform.parent.gameObject.SetActive(true);
				if (this.param10DisplayNameTextAlt != null)
				{
					this.param10DisplayNameTextAlt.text = this.param10DisplayName;
				}
				if (this.param10JSONParam != null)
				{
					this.param10JSONParam.sliderAlt = this.param10SliderAlt;
				}
			}
			else
			{
				this.param10SliderAlt.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup1PopupAlt != null)
		{
			if (this.hasTextureGroup1)
			{
				this.textureGroup1PopupAlt.gameObject.SetActive(true);
				if (this.textureGroup1JSON != null)
				{
					this.textureGroup1JSON.popupAlt = this.textureGroup1PopupAlt;
				}
			}
			else
			{
				this.textureGroup1PopupAlt.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup2PopupAlt != null)
		{
			if (this.hasTextureGroup2)
			{
				this.textureGroup2PopupAlt.gameObject.SetActive(true);
				if (this.textureGroup2JSON != null)
				{
					this.textureGroup2JSON.popupAlt = this.textureGroup2PopupAlt;
				}
			}
			else
			{
				this.textureGroup2PopupAlt.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup3PopupAlt != null)
		{
			if (this.hasTextureGroup3)
			{
				this.textureGroup3PopupAlt.gameObject.SetActive(true);
				if (this.textureGroup3JSON != null)
				{
					this.textureGroup3JSON.popupAlt = this.textureGroup3PopupAlt;
				}
			}
			else
			{
				this.textureGroup3PopupAlt.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup4PopupAlt != null)
		{
			if (this.hasTextureGroup4)
			{
				this.textureGroup4PopupAlt.gameObject.SetActive(true);
				if (this.textureGroup4JSON != null)
				{
					this.textureGroup4JSON.popupAlt = this.textureGroup4PopupAlt;
				}
			}
			else
			{
				this.textureGroup4PopupAlt.gameObject.SetActive(false);
			}
		}
		if (this.textureGroup5PopupAlt != null)
		{
			if (this.hasTextureGroup5)
			{
				this.textureGroup5PopupAlt.gameObject.SetActive(true);
				if (this.textureGroup5JSON != null)
				{
					this.textureGroup5JSON.popupAlt = this.textureGroup5PopupAlt;
				}
			}
			else
			{
				this.textureGroup5PopupAlt.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06006711 RID: 26385 RVA: 0x001595D8 File Offset: 0x001579D8
	public virtual void DeregisterUI()
	{
		if (this.customNameJSON != null)
		{
			this.customNameJSON.inputField = null;
		}
		if (this.renderQueueJSON != null)
		{
			this.renderQueueJSON.slider = null;
		}
		if (this.hideMaterialJSON != null)
		{
			this.hideMaterialJSON.toggle = null;
		}
		if (this.color1JSONParam != null)
		{
			this.color1JSONParam.colorPicker = null;
			this.color1JSONParam.colorPickerAlt = null;
		}
		if (this.color2JSONParam != null)
		{
			this.color2JSONParam.colorPicker = null;
			this.color2JSONParam.colorPickerAlt = null;
		}
		if (this.color3JSONParam != null)
		{
			this.color3JSONParam.colorPicker = null;
			this.color3JSONParam.colorPickerAlt = null;
		}
		if (this.param1JSONParam != null)
		{
			this.param1JSONParam.slider = null;
			this.param1JSONParam.sliderAlt = null;
		}
		if (this.param2JSONParam != null)
		{
			this.param2JSONParam.slider = null;
			this.param2JSONParam.sliderAlt = null;
		}
		if (this.param3JSONParam != null)
		{
			this.param3JSONParam.slider = null;
			this.param3JSONParam.sliderAlt = null;
		}
		if (this.param4JSONParam != null)
		{
			this.param4JSONParam.slider = null;
			this.param4JSONParam.sliderAlt = null;
		}
		if (this.param5JSONParam != null)
		{
			this.param5JSONParam.slider = null;
			this.param5JSONParam.sliderAlt = null;
		}
		if (this.param6JSONParam != null)
		{
			this.param6JSONParam.slider = null;
			this.param6JSONParam.sliderAlt = null;
		}
		if (this.param7JSONParam != null)
		{
			this.param7JSONParam.slider = null;
			this.param7JSONParam.sliderAlt = null;
		}
		if (this.param8JSONParam != null)
		{
			this.param8JSONParam.slider = null;
			this.param8JSONParam.sliderAlt = null;
		}
		if (this.param9JSONParam != null)
		{
			this.param9JSONParam.slider = null;
			this.param9JSONParam.sliderAlt = null;
		}
		if (this.param10JSONParam != null)
		{
			this.param10JSONParam.slider = null;
			this.param10JSONParam.sliderAlt = null;
		}
		if (this.textureGroup1JSON != null)
		{
			this.textureGroup1JSON.popup = null;
			this.textureGroup1JSON.popupAlt = null;
		}
		if (this.textureGroup2JSON != null)
		{
			this.textureGroup2JSON.popup = null;
			this.textureGroup2JSON.popupAlt = null;
		}
		if (this.textureGroup3JSON != null)
		{
			this.textureGroup3JSON.popup = null;
			this.textureGroup3JSON.popupAlt = null;
		}
		if (this.textureGroup4JSON != null)
		{
			this.textureGroup4JSON.popup = null;
			this.textureGroup4JSON.popupAlt = null;
		}
		if (this.textureGroup5JSON != null)
		{
			this.textureGroup5JSON.popup = null;
			this.textureGroup5JSON.popupAlt = null;
		}
		if (this.restoreAllFromDefaultsAction != null)
		{
			this.restoreAllFromDefaultsAction.button = null;
			this.restoreAllFromDefaultsAction.buttonAlt = null;
		}
		if (this.saveToStore1Action != null)
		{
			this.saveToStore1Action.button = null;
			this.saveToStore1Action.buttonAlt = null;
		}
		if (this.restoreAllFromStore1Action != null)
		{
			this.restoreAllFromStore1Action.button = null;
			this.restoreAllFromStore1Action.buttonAlt = null;
		}
		if (this.saveToStore2Action != null)
		{
			this.saveToStore2Action.button = null;
			this.saveToStore2Action.buttonAlt = null;
		}
		if (this.restoreAllFromStore2Action != null)
		{
			this.restoreAllFromStore2Action.button = null;
			this.restoreAllFromStore2Action.buttonAlt = null;
		}
		if (this.saveToStore3Action != null)
		{
			this.saveToStore3Action.button = null;
			this.saveToStore3Action.buttonAlt = null;
		}
		if (this.restoreAllFromStore3Action != null)
		{
			this.restoreAllFromStore3Action.button = null;
			this.restoreAllFromStore3Action.buttonAlt = null;
		}
		if (this.customTexture1UrlJSON != null)
		{
			this.customTexture1UrlJSON.fileBrowseButton = null;
			this.customTexture1UrlJSON.fileBrowseButtonAlt = null;
			this.customTexture1UrlJSON.clearButton = null;
			this.customTexture1UrlJSON.clearButtonAlt = null;
			this.customTexture1UrlJSON.text = null;
			this.customTexture1UrlJSON.textAlt = null;
		}
		if (this.customTexture2UrlJSON != null)
		{
			this.customTexture2UrlJSON.fileBrowseButton = null;
			this.customTexture2UrlJSON.fileBrowseButtonAlt = null;
			this.customTexture2UrlJSON.clearButton = null;
			this.customTexture2UrlJSON.clearButtonAlt = null;
			this.customTexture2UrlJSON.text = null;
			this.customTexture2UrlJSON.textAlt = null;
		}
		if (this.customTexture3UrlJSON != null)
		{
			this.customTexture3UrlJSON.fileBrowseButton = null;
			this.customTexture3UrlJSON.fileBrowseButtonAlt = null;
			this.customTexture3UrlJSON.clearButton = null;
			this.customTexture3UrlJSON.clearButtonAlt = null;
			this.customTexture3UrlJSON.text = null;
			this.customTexture3UrlJSON.textAlt = null;
		}
		if (this.customTexture4UrlJSON != null)
		{
			this.customTexture4UrlJSON.fileBrowseButton = null;
			this.customTexture4UrlJSON.fileBrowseButtonAlt = null;
			this.customTexture4UrlJSON.clearButton = null;
			this.customTexture4UrlJSON.clearButtonAlt = null;
			this.customTexture4UrlJSON.text = null;
			this.customTexture4UrlJSON.textAlt = null;
		}
		if (this.customTexture5UrlJSON != null)
		{
			this.customTexture5UrlJSON.fileBrowseButton = null;
			this.customTexture5UrlJSON.fileBrowseButtonAlt = null;
			this.customTexture5UrlJSON.clearButton = null;
			this.customTexture5UrlJSON.clearButtonAlt = null;
			this.customTexture5UrlJSON.text = null;
			this.customTexture5UrlJSON.textAlt = null;
		}
		if (this.customTexture6UrlJSON != null)
		{
			this.customTexture6UrlJSON.fileBrowseButton = null;
			this.customTexture6UrlJSON.fileBrowseButtonAlt = null;
			this.customTexture6UrlJSON.clearButton = null;
			this.customTexture6UrlJSON.clearButtonAlt = null;
			this.customTexture6UrlJSON.text = null;
			this.customTexture6UrlJSON.textAlt = null;
		}
	}

	// Token: 0x06006712 RID: 26386 RVA: 0x00159B88 File Offset: 0x00157F88
	public override void SetUI(Transform t)
	{
		if (this.deregisterOnDisable)
		{
			if (this.UITransform != t)
			{
				this.UITransform = t;
				if (base.isActiveAndEnabled)
				{
					this.InitUI();
				}
			}
		}
		else
		{
			base.SetUI(t);
		}
	}

	// Token: 0x06006713 RID: 26387 RVA: 0x00159BD8 File Offset: 0x00157FD8
	public override void SetUIAlt(Transform t)
	{
		if (this.deregisterOnDisable)
		{
			if (this.UITransformAlt != t)
			{
				this.UITransformAlt = t;
				if (base.isActiveAndEnabled)
				{
					this.InitUIAlt();
				}
			}
		}
		else
		{
			base.SetUIAlt(t);
		}
	}

	// Token: 0x06006714 RID: 26388 RVA: 0x00159C25 File Offset: 0x00158025
	public virtual void SetStartingValues()
	{
		this.SetStartingValues(null);
	}

	// Token: 0x06006715 RID: 26389 RVA: 0x00159C30 File Offset: 0x00158030
	public virtual void SetStartingValues(Dictionary<Texture2D, string> textureToSourcePath)
	{
		if (Application.isPlaying)
		{
			this.otherMaterialOptionsList = new List<MaterialOptions>();
			if (this.allowLinkToOtherMaterials)
			{
				MaterialOptions[] components = base.GetComponents<MaterialOptions>();
				foreach (MaterialOptions materialOptions in components)
				{
					if (materialOptions != this && materialOptions.allowLinkToOtherMaterials)
					{
						this.otherMaterialOptionsList.Add(materialOptions);
					}
				}
			}
			if (this.color1JSONParam != null)
			{
				base.DeregisterColor(this.color1JSONParam);
				this.color1JSONParam = null;
			}
			if (this.color2JSONParam != null)
			{
				base.DeregisterColor(this.color2JSONParam);
				this.color2JSONParam = null;
			}
			if (this.color3JSONParam != null)
			{
				base.DeregisterColor(this.color3JSONParam);
				this.color3JSONParam = null;
			}
			if (this.param1JSONParam != null)
			{
				base.DeregisterFloat(this.param1JSONParam);
				this.param1JSONParam = null;
			}
			if (this.param2JSONParam != null)
			{
				base.DeregisterFloat(this.param2JSONParam);
				this.param2JSONParam = null;
			}
			if (this.param3JSONParam != null)
			{
				base.DeregisterFloat(this.param3JSONParam);
				this.param3JSONParam = null;
			}
			if (this.param4JSONParam != null)
			{
				base.DeregisterFloat(this.param4JSONParam);
				this.param4JSONParam = null;
			}
			if (this.param5JSONParam != null)
			{
				base.DeregisterFloat(this.param5JSONParam);
				this.param5JSONParam = null;
			}
			if (this.param6JSONParam != null)
			{
				base.DeregisterFloat(this.param6JSONParam);
				this.param6JSONParam = null;
			}
			if (this.param7JSONParam != null)
			{
				base.DeregisterFloat(this.param7JSONParam);
				this.param7JSONParam = null;
			}
			if (this.param8JSONParam != null)
			{
				base.DeregisterFloat(this.param8JSONParam);
				this.param8JSONParam = null;
			}
			if (this.param9JSONParam != null)
			{
				base.DeregisterFloat(this.param9JSONParam);
				this.param9JSONParam = null;
			}
			if (this.param10JSONParam != null)
			{
				base.DeregisterFloat(this.param10JSONParam);
				this.param10JSONParam = null;
			}
			if (this.textureGroup1JSON != null)
			{
				base.DeregisterStringChooser(this.textureGroup1JSON);
				this.textureGroup1JSON = null;
			}
			if (this.textureGroup2JSON != null)
			{
				base.DeregisterStringChooser(this.textureGroup2JSON);
				this.textureGroup2JSON = null;
			}
			if (this.textureGroup3JSON != null)
			{
				base.DeregisterStringChooser(this.textureGroup3JSON);
				this.textureGroup3JSON = null;
			}
			if (this.textureGroup4JSON != null)
			{
				base.DeregisterStringChooser(this.textureGroup4JSON);
				this.textureGroup4JSON = null;
			}
			if (this.textureGroup5JSON != null)
			{
				base.DeregisterStringChooser(this.textureGroup5JSON);
				this.textureGroup5JSON = null;
			}
			if (this.createUVTemplateTextureJSON != null)
			{
				base.DeregisterAction(this.createUVTemplateTextureJSON);
				this.createUVTemplateTextureJSON = null;
			}
			if (this.createSimTemplateTextureJSON != null)
			{
				base.DeregisterAction(this.createSimTemplateTextureJSON);
				this.createSimTemplateTextureJSON = null;
			}
			if (this.openTextureFolderInExplorerAction != null)
			{
				base.DeregisterAction(this.openTextureFolderInExplorerAction);
				this.openTextureFolderInExplorerAction = null;
			}
			if (this.customTexture1UrlJSON != null)
			{
				base.DeregisterUrl(this.customTexture1UrlJSON);
				this.customTexture1UrlJSON = null;
			}
			if (this.customTexture1TileXJSON != null)
			{
				base.DeregisterFloat(this.customTexture1TileXJSON);
				this.customTexture1TileXJSON = null;
			}
			if (this.customTexture1TileYJSON != null)
			{
				base.DeregisterFloat(this.customTexture1TileYJSON);
				this.customTexture1TileYJSON = null;
			}
			if (this.customTexture1OffsetXJSON != null)
			{
				base.DeregisterFloat(this.customTexture1OffsetXJSON);
				this.customTexture1OffsetXJSON = null;
			}
			if (this.customTexture1OffsetYJSON != null)
			{
				base.DeregisterFloat(this.customTexture1OffsetYJSON);
				this.customTexture1OffsetYJSON = null;
			}
			if (this.customTexture2UrlJSON != null)
			{
				base.DeregisterUrl(this.customTexture2UrlJSON);
				this.customTexture2UrlJSON = null;
			}
			if (this.customTexture2TileXJSON != null)
			{
				base.DeregisterFloat(this.customTexture2TileXJSON);
				this.customTexture2TileXJSON = null;
			}
			if (this.customTexture2TileYJSON != null)
			{
				base.DeregisterFloat(this.customTexture2TileYJSON);
				this.customTexture2TileYJSON = null;
			}
			if (this.customTexture2OffsetXJSON != null)
			{
				base.DeregisterFloat(this.customTexture2OffsetXJSON);
				this.customTexture2OffsetXJSON = null;
			}
			if (this.customTexture2OffsetYJSON != null)
			{
				base.DeregisterFloat(this.customTexture2OffsetYJSON);
				this.customTexture2OffsetYJSON = null;
			}
			if (this.customTexture3UrlJSON != null)
			{
				base.DeregisterUrl(this.customTexture3UrlJSON);
				this.customTexture3UrlJSON = null;
			}
			if (this.customTexture3TileXJSON != null)
			{
				base.DeregisterFloat(this.customTexture3TileXJSON);
				this.customTexture3TileXJSON = null;
			}
			if (this.customTexture3TileYJSON != null)
			{
				base.DeregisterFloat(this.customTexture3TileYJSON);
				this.customTexture3TileYJSON = null;
			}
			if (this.customTexture3OffsetXJSON != null)
			{
				base.DeregisterFloat(this.customTexture3OffsetXJSON);
				this.customTexture3OffsetXJSON = null;
			}
			if (this.customTexture3OffsetYJSON != null)
			{
				base.DeregisterFloat(this.customTexture3OffsetYJSON);
				this.customTexture3OffsetYJSON = null;
			}
			if (this.customTexture4UrlJSON != null)
			{
				base.DeregisterUrl(this.customTexture4UrlJSON);
				this.customTexture4UrlJSON = null;
			}
			if (this.customTexture4TileXJSON != null)
			{
				base.DeregisterFloat(this.customTexture4TileXJSON);
				this.customTexture4TileXJSON = null;
			}
			if (this.customTexture4TileYJSON != null)
			{
				base.DeregisterFloat(this.customTexture4TileYJSON);
				this.customTexture4TileYJSON = null;
			}
			if (this.customTexture4OffsetXJSON != null)
			{
				base.DeregisterFloat(this.customTexture4OffsetXJSON);
				this.customTexture4OffsetXJSON = null;
			}
			if (this.customTexture4OffsetYJSON != null)
			{
				base.DeregisterFloat(this.customTexture4OffsetYJSON);
				this.customTexture4OffsetYJSON = null;
			}
			if (this.customTexture5UrlJSON != null)
			{
				base.DeregisterUrl(this.customTexture5UrlJSON);
				this.customTexture5UrlJSON = null;
			}
			if (this.customTexture5TileXJSON != null)
			{
				base.DeregisterFloat(this.customTexture5TileXJSON);
				this.customTexture5TileXJSON = null;
			}
			if (this.customTexture5TileYJSON != null)
			{
				base.DeregisterFloat(this.customTexture5TileYJSON);
				this.customTexture5TileYJSON = null;
			}
			if (this.customTexture5OffsetXJSON != null)
			{
				base.DeregisterFloat(this.customTexture5OffsetXJSON);
				this.customTexture5OffsetXJSON = null;
			}
			if (this.customTexture5OffsetYJSON != null)
			{
				base.DeregisterFloat(this.customTexture5OffsetYJSON);
				this.customTexture5OffsetYJSON = null;
			}
			if (this.customTexture6UrlJSON != null)
			{
				base.DeregisterUrl(this.customTexture6UrlJSON);
				this.customTexture6UrlJSON = null;
			}
			if (this.customTexture6TileXJSON != null)
			{
				base.DeregisterFloat(this.customTexture6TileXJSON);
				this.customTexture6TileXJSON = null;
			}
			if (this.customTexture6TileYJSON != null)
			{
				base.DeregisterFloat(this.customTexture6TileYJSON);
				this.customTexture6TileYJSON = null;
			}
			if (this.customTexture6OffsetXJSON != null)
			{
				base.DeregisterFloat(this.customTexture6OffsetXJSON);
				this.customTexture6OffsetXJSON = null;
			}
			if (this.customTexture6OffsetYJSON != null)
			{
				base.DeregisterFloat(this.customTexture6OffsetYJSON);
				this.customTexture6OffsetYJSON = null;
			}
		}
		this.customNameJSON = new JSONStorableString("customName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncCustomName));
		this.customNameJSON.isStorable = false;
		this.customNameJSON.isRestorable = false;
		Renderer[] array2;
		if (this.materialContainer != null)
		{
			if (this.searchInChildren)
			{
				array2 = this.materialContainer.GetComponentsInChildren<Renderer>();
			}
			else
			{
				array2 = this.materialContainer.GetComponents<Renderer>();
			}
		}
		else if (this.searchInChildren)
		{
			array2 = base.GetComponentsInChildren<Renderer>();
		}
		else
		{
			array2 = base.GetComponents<Renderer>();
		}
		List<Renderer> list = new List<Renderer>();
		foreach (Renderer renderer in array2)
		{
			MaterialOptionsIgnore component = renderer.GetComponent<MaterialOptionsIgnore>();
			if (component == null)
			{
				list.Add(renderer);
			}
		}
		this.renderers = list.ToArray();
		Renderer[] array4 = null;
		if (this.materialContainer2 != null)
		{
			if (this.searchInChildren)
			{
				array4 = this.materialContainer2.GetComponentsInChildren<Renderer>();
			}
			else
			{
				array4 = this.materialContainer2.GetComponents<Renderer>();
			}
		}
		List<Renderer> list2 = new List<Renderer>();
		if (array4 != null)
		{
			foreach (Renderer renderer2 in array4)
			{
				MaterialOptionsIgnore component2 = renderer2.GetComponent<MaterialOptionsIgnore>();
				if (component2 == null)
				{
					list2.Add(renderer2);
				}
			}
		}
		this.renderers2 = list2.ToArray();
		if (this.materialForDefaults == null && this.renderers.Length > 0 && this.paramMaterialSlots != null && this.paramMaterialSlots.Length > 0)
		{
			Renderer renderer3 = this.renderers[0];
			int num = this.paramMaterialSlots[0];
			if (num < renderer3.materials.Length)
			{
				this.materialForDefaults = renderer3.materials[num];
			}
		}
		if (this.renderers.Length > 0)
		{
			Renderer renderer4 = this.renderers[0];
			this.meshFilter = renderer4.GetComponent<MeshFilter>();
		}
		if (Application.isPlaying && this.textureGroup1 != null && this.paramMaterialSlots != null && this.paramMaterialSlots.Length > 0 && (this.textureGroup1.materialSlots == null || this.textureGroup1.materialSlots.Length == 0))
		{
			this.textureGroup1.materialSlots = new int[this.paramMaterialSlots.Length];
			for (int l = 0; l < this.paramMaterialSlots.Length; l++)
			{
				this.textureGroup1.materialSlots[l] = this.paramMaterialSlots[l];
			}
		}
		if (this.controlRawImage)
		{
			RawImage[] array6;
			if (this.materialContainer != null)
			{
				if (this.searchInChildren)
				{
					array6 = this.materialContainer.GetComponentsInChildren<RawImage>();
				}
				else
				{
					array6 = this.materialContainer.GetComponents<RawImage>();
				}
			}
			else if (this.searchInChildren)
			{
				array6 = base.GetComponentsInChildren<RawImage>();
			}
			else
			{
				array6 = base.GetComponents<RawImage>();
			}
			List<Material> list3 = new List<Material>();
			foreach (RawImage rawImage in array6)
			{
				MaterialOptionsIgnore component3 = rawImage.GetComponent<MaterialOptionsIgnore>();
				if (component3 == null)
				{
					if (Application.isPlaying)
					{
						Material material = new Material(rawImage.material);
						this.RegisterAllocatedObject(material);
						rawImage.material = material;
						list3.Add(material);
					}
					else
					{
						list3.Add(rawImage.material);
					}
				}
			}
			if (this.materialForDefaults == null && array6.Length > 0)
			{
				this.materialForDefaults = array6[0].material;
			}
			this.rawImageMaterials = list3.ToArray();
		}
		if (this.materialForDefaults != null)
		{
			if (Application.isPlaying)
			{
				this.renderQueueJSON = new JSONStorableFloat("renderQueue", (float)this.materialForDefaults.renderQueue, new JSONStorableFloat.SetFloatCallback(this.SyncRenderQueue), -1f, 5000f, true, true);
				base.RegisterFloat(this.renderQueueJSON);
				if (this.hideShader != null)
				{
					this.hideMaterialJSON = new JSONStorableBool("hideMaterial", false, new JSONStorableBool.SetBoolCallback(this.SyncHideMaterial));
					base.RegisterBool(this.hideMaterialJSON);
				}
				if (this.allowLinkToOtherMaterials)
				{
					this.linkToOtherMaterialsJSON = new JSONStorableBool("linkToOtherMaterials", this._linkToOtherMaterials, new JSONStorableBool.SetBoolCallback(this.SyncLinkToOtherMaterials));
					base.RegisterBool(this.linkToOtherMaterialsJSON);
				}
			}
			if (this.color1Name != null && this.color1Name != string.Empty && this.materialForDefaults.HasProperty(this.color1Name))
			{
				this.materialHasColor1 = true;
				this.color1CurrentColor = this.materialForDefaults.GetColor(this.color1Name);
				this.color1Alpha = this.color1CurrentColor.a;
				this.color1CurrentHSVColor = HSVColorPicker.RGBToHSV(this.color1CurrentColor.r, this.color1CurrentColor.g, this.color1CurrentColor.b);
				if (Application.isPlaying)
				{
					this.color1JSONParam = new JSONStorableColor(this.color1DisplayName, this.color1CurrentHSVColor, new JSONStorableColor.SetHSVColorCallback(this.SetColor1FromHSV));
					base.RegisterColor(this.color1JSONParam);
				}
			}
			else
			{
				this.materialHasColor1 = false;
				this.color1CurrentColor = UnityEngine.Color.black;
				this.color1Alpha = 1f;
				this.color1CurrentHSVColor = HSVColorPicker.RGBToHSV(0f, 0f, 0f);
			}
			if (this.color2Name != null && this.color2Name != string.Empty && this.materialForDefaults.HasProperty(this.color2Name))
			{
				this.materialHasColor2 = true;
				this.color2CurrentColor = this.materialForDefaults.GetColor(this.color2Name);
				this.color2Alpha = this.color2CurrentColor.a;
				this.color2CurrentHSVColor = HSVColorPicker.RGBToHSV(this.color2CurrentColor.r, this.color2CurrentColor.g, this.color2CurrentColor.b);
				if (Application.isPlaying)
				{
					this.color2JSONParam = new JSONStorableColor(this.color2DisplayName, this.color2CurrentHSVColor, new JSONStorableColor.SetHSVColorCallback(this.SetColor2FromHSV));
					base.RegisterColor(this.color2JSONParam);
				}
			}
			else
			{
				this.materialHasColor2 = false;
				this.color2CurrentColor = UnityEngine.Color.black;
				this.color2Alpha = 1f;
				this.color2CurrentHSVColor = HSVColorPicker.RGBToHSV(0f, 0f, 0f);
			}
			if (this.color3Name != null && this.color3Name != string.Empty && this.materialForDefaults.HasProperty(this.color3Name))
			{
				this.materialHasColor3 = true;
				this.color3CurrentColor = this.materialForDefaults.GetColor(this.color3Name);
				this.color3Alpha = this.color3CurrentColor.a;
				this.color3CurrentHSVColor = HSVColorPicker.RGBToHSV(this.color3CurrentColor.r, this.color3CurrentColor.g, this.color3CurrentColor.b);
				if (Application.isPlaying)
				{
					this.color3JSONParam = new JSONStorableColor(this.color3DisplayName, this.color3CurrentHSVColor, new JSONStorableColor.SetHSVColorCallback(this.SetColor3FromHSV));
					base.RegisterColor(this.color3JSONParam);
				}
			}
			else
			{
				this.materialHasColor3 = false;
				this.color3CurrentColor = UnityEngine.Color.black;
				this.color3Alpha = 1f;
				this.color3CurrentHSVColor = HSVColorPicker.RGBToHSV(0f, 0f, 0f);
			}
			this.materialHasParam1 = false;
			this.materialHasParam2 = false;
			this.materialHasParam3 = false;
			this.materialHasParam4 = false;
			this.materialHasParam5 = false;
			this.materialHasParam6 = false;
			this.materialHasParam7 = false;
			this.materialHasParam8 = false;
			this.materialHasParam9 = false;
			this.materialHasParam10 = false;
			if (this.param1Name != null && this.param1Name != string.Empty && this.materialForDefaults.HasProperty(this.param1Name))
			{
				this.materialHasParam1 = true;
				this.param1CurrentValue = this.materialForDefaults.GetFloat(this.param1Name);
				if (Application.isPlaying)
				{
					this.param1JSONParam = new JSONStorableFloat(this.param1DisplayName, this.param1CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam1CurrentValue), this.param1MinValue, this.param1MaxValue, true, true);
					base.RegisterFloat(this.param1JSONParam);
				}
			}
			if (this.param2Name != null && this.param2Name != string.Empty && this.materialForDefaults.HasProperty(this.param2Name))
			{
				this.materialHasParam2 = true;
				this.param2CurrentValue = this.materialForDefaults.GetFloat(this.param2Name);
				if (Application.isPlaying)
				{
					this.param2JSONParam = new JSONStorableFloat(this.param2DisplayName, this.param2CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam2CurrentValue), this.param2MinValue, this.param2MaxValue, true, true);
					base.RegisterFloat(this.param2JSONParam);
				}
			}
			if (this.param3Name != null && this.param3Name != string.Empty && this.materialForDefaults.HasProperty(this.param3Name))
			{
				this.materialHasParam3 = true;
				this.param3CurrentValue = this.materialForDefaults.GetFloat(this.param3Name);
				if (Application.isPlaying)
				{
					this.param3JSONParam = new JSONStorableFloat(this.param3DisplayName, this.param3CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam3CurrentValue), this.param3MinValue, this.param3MaxValue, true, true);
					base.RegisterFloat(this.param3JSONParam);
				}
			}
			if (this.param4Name != null && this.param4Name != string.Empty && this.materialForDefaults.HasProperty(this.param4Name))
			{
				this.materialHasParam4 = true;
				this.param4CurrentValue = this.materialForDefaults.GetFloat(this.param4Name);
				if (Application.isPlaying)
				{
					this.param4JSONParam = new JSONStorableFloat(this.param4DisplayName, this.param4CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam4CurrentValue), this.param4MinValue, this.param4MaxValue, true, true);
					base.RegisterFloat(this.param4JSONParam);
				}
			}
			if (this.param5Name != null && this.param5Name != string.Empty && this.materialForDefaults.HasProperty(this.param5Name))
			{
				this.materialHasParam5 = true;
				this.param5CurrentValue = this.materialForDefaults.GetFloat(this.param5Name);
				if (Application.isPlaying)
				{
					this.param5JSONParam = new JSONStorableFloat(this.param5DisplayName, this.param5CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam5CurrentValue), this.param5MinValue, this.param5MaxValue, true, true);
					base.RegisterFloat(this.param5JSONParam);
				}
			}
			if (this.param6Name != null && this.param6Name != string.Empty && this.materialForDefaults.HasProperty(this.param6Name))
			{
				this.materialHasParam6 = true;
				this.param6CurrentValue = this.materialForDefaults.GetFloat(this.param6Name);
				if (Application.isPlaying)
				{
					this.param6JSONParam = new JSONStorableFloat(this.param6DisplayName, this.param6CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam6CurrentValue), this.param6MinValue, this.param6MaxValue, true, true);
					base.RegisterFloat(this.param6JSONParam);
				}
			}
			if (this.param7Name != null && this.param7Name != string.Empty && this.materialForDefaults.HasProperty(this.param7Name))
			{
				this.materialHasParam7 = true;
				this.param7CurrentValue = this.materialForDefaults.GetFloat(this.param7Name);
				if (Application.isPlaying)
				{
					this.param7JSONParam = new JSONStorableFloat(this.param7DisplayName, this.param7CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam7CurrentValue), this.param7MinValue, this.param7MaxValue, true, true);
					base.RegisterFloat(this.param7JSONParam);
				}
			}
			if (this.param8Name != null && this.param8Name != string.Empty && this.materialForDefaults.HasProperty(this.param8Name))
			{
				this.materialHasParam8 = true;
				this.param8CurrentValue = this.materialForDefaults.GetFloat(this.param8Name);
				if (Application.isPlaying)
				{
					this.param8JSONParam = new JSONStorableFloat(this.param8DisplayName, this.param8CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam8CurrentValue), this.param8MinValue, this.param8MaxValue, true, true);
					base.RegisterFloat(this.param8JSONParam);
				}
			}
			if (this.param9Name != null && this.param9Name != string.Empty && this.materialForDefaults.HasProperty(this.param9Name))
			{
				this.materialHasParam9 = true;
				this.param9CurrentValue = this.materialForDefaults.GetFloat(this.param9Name);
				if (Application.isPlaying)
				{
					this.param9JSONParam = new JSONStorableFloat(this.param9DisplayName, this.param9CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam9CurrentValue), this.param9MinValue, this.param9MaxValue, true, true);
					base.RegisterFloat(this.param9JSONParam);
				}
			}
			if (this.param10Name != null && this.param10Name != string.Empty && this.materialForDefaults.HasProperty(this.param10Name))
			{
				this.materialHasParam10 = true;
				this.param10CurrentValue = this.materialForDefaults.GetFloat(this.param10Name);
				if (Application.isPlaying)
				{
					this.param10JSONParam = new JSONStorableFloat(this.param10DisplayName, this.param10CurrentValue, new JSONStorableFloat.SetFloatCallback(this.SetParam10CurrentValue), this.param10MinValue, this.param10MaxValue, true, true);
					base.RegisterFloat(this.param10JSONParam);
				}
			}
		}
		this.hasTextureGroup1 = false;
		this.hasTextureGroup2 = false;
		this.hasTextureGroup3 = false;
		this.hasTextureGroup4 = false;
		this.hasTextureGroup5 = false;
		this.currentTextureGroup1Set = this.startingTextureGroup1Set;
		if (this.textureGroup1 != null && this.textureGroup1.name != null && this.textureGroup1.name != string.Empty && this.textureGroup1.sets != null && this.textureGroup1.sets.Length > 1)
		{
			this.hasTextureGroup1 = true;
			if (Application.isPlaying)
			{
				List<string> list4 = new List<string>();
				for (int n = 0; n < this.textureGroup1.sets.Length; n++)
				{
					list4.Add(this.textureGroup1.sets[n].name);
				}
				this.textureGroup1JSON = new JSONStorableStringChooser(this.textureGroup1.name, list4, this.startingTextureGroup1Set, this.textureGroup1.name, new JSONStorableStringChooser.SetStringCallback(this.SetTextureGroup1Set));
				base.RegisterStringChooser(this.textureGroup1JSON);
			}
		}
		this.currentTextureGroup2Set = this.startingTextureGroup2Set;
		if (this.textureGroup2 != null && this.textureGroup2.name != null && this.textureGroup2.name != string.Empty && this.textureGroup2.sets != null && this.textureGroup2.sets.Length > 1)
		{
			this.hasTextureGroup2 = true;
			if (Application.isPlaying)
			{
				List<string> list5 = new List<string>();
				for (int num2 = 0; num2 < this.textureGroup2.sets.Length; num2++)
				{
					list5.Add(this.textureGroup2.sets[num2].name);
				}
				this.textureGroup2JSON = new JSONStorableStringChooser(this.textureGroup2.name, list5, this.startingTextureGroup2Set, this.textureGroup2.name, new JSONStorableStringChooser.SetStringCallback(this.SetTextureGroup2Set));
				base.RegisterStringChooser(this.textureGroup2JSON);
			}
		}
		this.currentTextureGroup3Set = this.startingTextureGroup3Set;
		if (this.textureGroup3 != null && this.textureGroup3.name != null && this.textureGroup3.name != string.Empty && this.textureGroup3.sets != null && this.textureGroup3.sets.Length > 1)
		{
			this.hasTextureGroup3 = true;
			if (Application.isPlaying)
			{
				List<string> list6 = new List<string>();
				for (int num3 = 0; num3 < this.textureGroup3.sets.Length; num3++)
				{
					list6.Add(this.textureGroup3.sets[num3].name);
				}
				this.textureGroup3JSON = new JSONStorableStringChooser(this.textureGroup3.name, list6, this.startingTextureGroup3Set, this.textureGroup3.name, new JSONStorableStringChooser.SetStringCallback(this.SetTextureGroup3Set));
				base.RegisterStringChooser(this.textureGroup3JSON);
			}
		}
		this.currentTextureGroup4Set = this.startingTextureGroup4Set;
		if (this.textureGroup4 != null && this.textureGroup4.name != null && this.textureGroup4.name != string.Empty && this.textureGroup4.sets != null && this.textureGroup4.sets.Length > 1)
		{
			this.hasTextureGroup4 = true;
			if (Application.isPlaying)
			{
				List<string> list7 = new List<string>();
				for (int num4 = 0; num4 < this.textureGroup4.sets.Length; num4++)
				{
					list7.Add(this.textureGroup4.sets[num4].name);
				}
				this.textureGroup4JSON = new JSONStorableStringChooser(this.textureGroup4.name, list7, this.startingTextureGroup4Set, this.textureGroup4.name, new JSONStorableStringChooser.SetStringCallback(this.SetTextureGroup4Set));
				base.RegisterStringChooser(this.textureGroup4JSON);
			}
		}
		this.currentTextureGroup5Set = this.startingTextureGroup5Set;
		if (this.textureGroup5 != null && this.textureGroup5.name != null && this.textureGroup5.name != string.Empty && this.textureGroup5.sets != null && this.textureGroup5.sets.Length > 1)
		{
			this.hasTextureGroup5 = true;
			if (Application.isPlaying)
			{
				List<string> list8 = new List<string>();
				for (int num5 = 0; num5 < this.textureGroup5.sets.Length; num5++)
				{
					list8.Add(this.textureGroup5.sets[num5].name);
				}
				this.textureGroup5JSON = new JSONStorableStringChooser(this.textureGroup5.name, list8, this.startingTextureGroup5Set, this.textureGroup5.name, new JSONStorableStringChooser.SetStringCallback(this.SetTextureGroup5Set));
				base.RegisterStringChooser(this.textureGroup5JSON);
			}
		}
		if (Application.isPlaying && this.textureGroup1 != null && this.textureGroup1.mapTexturesToTextureNames && this.materialForDefaults != null)
		{
			MaterialOptionTextureSet materialOptionTextureSet = null;
			if (this.textureGroup1.sets == null || this.textureGroup1.sets.Length == 0)
			{
				this.textureGroup1.sets = new MaterialOptionTextureSet[1];
				materialOptionTextureSet = new MaterialOptionTextureSet();
				materialOptionTextureSet.name = "Default";
				materialOptionTextureSet.textures = new Texture[6];
				this.textureGroup1.sets[0] = materialOptionTextureSet;
				this.startingTextureGroup1Set = "Default";
				this.currentTextureGroup1Set = this.startingTextureGroup1Set;
			}
			this.createUVTemplateTextureJSON = new JSONStorableAction("CreateUVTemplateTexture", new JSONStorableAction.ActionCallback(this.CreateUVTemplateTexture));
			base.RegisterAction(this.createUVTemplateTextureJSON);
			this.createSimTemplateTextureJSON = new JSONStorableAction("CreateSimTemplateTexture", new JSONStorableAction.ActionCallback(this.CreateSimTemplateTexture));
			base.RegisterAction(this.createSimTemplateTextureJSON);
			this.openTextureFolderInExplorerAction = new JSONStorableAction("OpenTextureFolderInExplorer", new JSONStorableAction.ActionCallback(this.OpenTextureFolderInExplorer));
			base.RegisterAction(this.openTextureFolderInExplorerAction);
			if (this.textureGroup1.textureName != null && this.textureGroup1.textureName != string.Empty && this.materialForDefaults.HasProperty(this.textureGroup1.textureName))
			{
				this.customTexture1UrlJSON = new JSONStorableUrl("customTexture" + this.textureGroup1.textureName, string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncCustomTexture1Url), "jpg|jpeg|png|tif|tiff", this.customTextureFolder, true);
				this.customTexture1UrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				Texture2D texture2D = (Texture2D)this.materialForDefaults.GetTexture(this.textureGroup1.textureName);
				if (texture2D != null)
				{
					if (materialOptionTextureSet != null)
					{
						materialOptionTextureSet.textures[0] = texture2D;
					}
					string valNoCallback;
					if (textureToSourcePath != null && textureToSourcePath.TryGetValue(texture2D, out valNoCallback))
					{
						this.customTexture1UrlJSON.valNoCallback = valNoCallback;
					}
				}
				base.RegisterUrl(this.customTexture1UrlJSON);
				Vector2 textureScale = this.materialForDefaults.GetTextureScale(this.textureGroup1.textureName);
				Vector2 textureOffset = this.materialForDefaults.GetTextureOffset(this.textureGroup1.textureName);
				this.customTexture1TileXJSON = new JSONStorableFloat("customTexture1TileX", textureScale.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture1Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture1TileXJSON);
				this.customTexture1TileYJSON = new JSONStorableFloat("customTexture1TileY", textureScale.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture1Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture1TileYJSON);
				this.customTexture1OffsetXJSON = new JSONStorableFloat("customTexture1OffsetX", textureOffset.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture1Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture1OffsetXJSON);
				this.customTexture1OffsetYJSON = new JSONStorableFloat("customTexture1OffsetY", textureOffset.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture1Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture1OffsetYJSON);
			}
			if (this.textureGroup1.secondaryTextureName != null && this.textureGroup1.secondaryTextureName != string.Empty && this.materialForDefaults.HasProperty(this.textureGroup1.secondaryTextureName))
			{
				this.customTexture2UrlJSON = new JSONStorableUrl("customTexture" + this.textureGroup1.secondaryTextureName, string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncCustomTexture2Url), "jpg|jpeg|png|tif|tiff", this.customTextureFolder, true);
				this.customTexture2UrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				Texture2D texture2D2 = (Texture2D)this.materialForDefaults.GetTexture(this.textureGroup1.secondaryTextureName);
				if (texture2D2 != null)
				{
					if (materialOptionTextureSet != null)
					{
						materialOptionTextureSet.textures[1] = texture2D2;
					}
					string valNoCallback2;
					if (textureToSourcePath != null && textureToSourcePath.TryGetValue(texture2D2, out valNoCallback2))
					{
						this.customTexture2UrlJSON.valNoCallback = valNoCallback2;
					}
				}
				base.RegisterUrl(this.customTexture2UrlJSON);
				Vector2 textureScale2 = this.materialForDefaults.GetTextureScale(this.textureGroup1.secondaryTextureName);
				Vector2 textureOffset2 = this.materialForDefaults.GetTextureOffset(this.textureGroup1.secondaryTextureName);
				this.customTexture2TileXJSON = new JSONStorableFloat("customTexture2TileX", textureScale2.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture2Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture2TileXJSON);
				this.customTexture2TileYJSON = new JSONStorableFloat("customTexture2TileY", textureScale2.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture2Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture2TileYJSON);
				this.customTexture2OffsetXJSON = new JSONStorableFloat("customTexture2OffsetX", textureOffset2.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture2Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture2OffsetXJSON);
				this.customTexture2OffsetYJSON = new JSONStorableFloat("customTexture2OffsetY", textureOffset2.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture2Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture2OffsetYJSON);
			}
			if (this.textureGroup1.thirdTextureName != null && this.textureGroup1.thirdTextureName != string.Empty && this.materialForDefaults.HasProperty(this.textureGroup1.thirdTextureName))
			{
				this.customTexture3UrlJSON = new JSONStorableUrl("customTexture" + this.textureGroup1.thirdTextureName, string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncCustomTexture3Url), "jpg|jpeg|png|tif|tiff", this.customTextureFolder, true);
				this.customTexture3UrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				Texture2D texture2D3 = (Texture2D)this.materialForDefaults.GetTexture(this.textureGroup1.thirdTextureName);
				if (texture2D3 != null)
				{
					if (materialOptionTextureSet != null)
					{
						materialOptionTextureSet.textures[2] = texture2D3;
					}
					string valNoCallback3;
					if (textureToSourcePath != null && textureToSourcePath.TryGetValue(texture2D3, out valNoCallback3))
					{
						this.customTexture3UrlJSON.valNoCallback = valNoCallback3;
					}
				}
				base.RegisterUrl(this.customTexture3UrlJSON);
				Vector2 textureScale3 = this.materialForDefaults.GetTextureScale(this.textureGroup1.thirdTextureName);
				Vector2 textureOffset3 = this.materialForDefaults.GetTextureOffset(this.textureGroup1.thirdTextureName);
				this.customTexture3TileXJSON = new JSONStorableFloat("customTexture3TileX", textureScale3.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture3Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture3TileXJSON);
				this.customTexture3TileYJSON = new JSONStorableFloat("customTexture3TileY", textureScale3.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture3Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture3TileYJSON);
				this.customTexture3OffsetXJSON = new JSONStorableFloat("customTexture3OffsetX", textureOffset3.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture3Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture3OffsetXJSON);
				this.customTexture3OffsetYJSON = new JSONStorableFloat("customTexture3OffsetY", textureOffset3.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture3Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture3OffsetYJSON);
			}
			if (this.textureGroup1.fourthTextureName != null && this.textureGroup1.fourthTextureName != string.Empty && this.materialForDefaults.HasProperty(this.textureGroup1.fourthTextureName))
			{
				this.customTexture4UrlJSON = new JSONStorableUrl("customTexture" + this.textureGroup1.fourthTextureName, string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncCustomTexture4Url), "jpg|jpeg|png|tif|tiff", this.customTextureFolder, true);
				this.customTexture4UrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				Texture2D texture2D4 = (Texture2D)this.materialForDefaults.GetTexture(this.textureGroup1.fourthTextureName);
				if (texture2D4 != null)
				{
					if (materialOptionTextureSet != null)
					{
						materialOptionTextureSet.textures[3] = texture2D4;
					}
					string valNoCallback4;
					if (textureToSourcePath != null && textureToSourcePath.TryGetValue(texture2D4, out valNoCallback4))
					{
						this.customTexture4UrlJSON.valNoCallback = valNoCallback4;
					}
				}
				base.RegisterUrl(this.customTexture4UrlJSON);
				Vector2 textureScale4 = this.materialForDefaults.GetTextureScale(this.textureGroup1.fourthTextureName);
				Vector2 textureOffset4 = this.materialForDefaults.GetTextureOffset(this.textureGroup1.fourthTextureName);
				this.customTexture4TileXJSON = new JSONStorableFloat("customTexture4TileX", textureScale4.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture4Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture4TileXJSON);
				this.customTexture4TileYJSON = new JSONStorableFloat("customTexture4TileY", textureScale4.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture4Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture4TileYJSON);
				this.customTexture4OffsetXJSON = new JSONStorableFloat("customTexture4OffsetX", textureOffset4.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture4Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture4OffsetXJSON);
				this.customTexture4OffsetYJSON = new JSONStorableFloat("customTexture4OffsetY", textureOffset4.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture4Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture4OffsetYJSON);
			}
			if (this.textureGroup1.fifthTextureName != null && this.textureGroup1.fifthTextureName != string.Empty && this.materialForDefaults.HasProperty(this.textureGroup1.fifthTextureName))
			{
				this.customTexture5UrlJSON = new JSONStorableUrl("customTexture" + this.textureGroup1.fifthTextureName, string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncCustomTexture5Url), "jpg|jpeg|png|tif|tiff", this.customTextureFolder, true);
				this.customTexture5UrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				Texture2D texture2D5 = (Texture2D)this.materialForDefaults.GetTexture(this.textureGroup1.fifthTextureName);
				if (texture2D5 != null)
				{
					if (materialOptionTextureSet != null)
					{
						materialOptionTextureSet.textures[4] = texture2D5;
					}
					string valNoCallback5;
					if (textureToSourcePath != null && textureToSourcePath.TryGetValue(texture2D5, out valNoCallback5))
					{
						this.customTexture5UrlJSON.valNoCallback = valNoCallback5;
					}
				}
				base.RegisterUrl(this.customTexture5UrlJSON);
				Vector2 textureScale5 = this.materialForDefaults.GetTextureScale(this.textureGroup1.fifthTextureName);
				Vector2 textureOffset5 = this.materialForDefaults.GetTextureOffset(this.textureGroup1.fifthTextureName);
				this.customTexture5TileXJSON = new JSONStorableFloat("customTexture5TileX", textureScale5.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture5Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture5TileXJSON);
				this.customTexture5TileYJSON = new JSONStorableFloat("customTexture5TileY", textureScale5.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture5Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture5TileYJSON);
				this.customTexture5OffsetXJSON = new JSONStorableFloat("customTexture5OffsetX", textureOffset5.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture5Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture5OffsetXJSON);
				this.customTexture5OffsetYJSON = new JSONStorableFloat("customTexture5OffsetY", textureOffset5.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture5Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture5OffsetYJSON);
			}
			if (this.textureGroup1.sixthTextureName != null && this.textureGroup1.sixthTextureName != string.Empty && this.materialForDefaults.HasProperty(this.textureGroup1.sixthTextureName))
			{
				this.customTexture6UrlJSON = new JSONStorableUrl("customTexture" + this.textureGroup1.sixthTextureName, string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncCustomTexture6Url), "jpg|jpeg|png|tif|tiff", this.customTextureFolder, true);
				this.customTexture6UrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				Texture2D texture2D6 = (Texture2D)this.materialForDefaults.GetTexture(this.textureGroup1.sixthTextureName);
				if (texture2D6 != null)
				{
					if (materialOptionTextureSet != null)
					{
						materialOptionTextureSet.textures[5] = texture2D6;
					}
					string valNoCallback6;
					if (textureToSourcePath != null && textureToSourcePath.TryGetValue(texture2D6, out valNoCallback6))
					{
						this.customTexture6UrlJSON.valNoCallback = valNoCallback6;
					}
				}
				base.RegisterUrl(this.customTexture6UrlJSON);
				Vector2 textureScale6 = this.materialForDefaults.GetTextureScale(this.textureGroup1.sixthTextureName);
				Vector2 textureOffset6 = this.materialForDefaults.GetTextureOffset(this.textureGroup1.sixthTextureName);
				this.customTexture6TileXJSON = new JSONStorableFloat("customTexture6TileX", textureScale6.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture6Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture6TileXJSON);
				this.customTexture6TileYJSON = new JSONStorableFloat("customTexture6TileY", textureScale6.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture6Tile), 0f, 10f, false, true);
				base.RegisterFloat(this.customTexture6TileYJSON);
				this.customTexture6OffsetXJSON = new JSONStorableFloat("customTexture6OffsetX", textureOffset6.x, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture6Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture6OffsetXJSON);
				this.customTexture6OffsetYJSON = new JSONStorableFloat("customTexture6OffsetY", textureOffset6.y, new JSONStorableFloat.SetFloatCallback(this.SyncCustomTexture6Offset), -1f, 1f, false, true);
				base.RegisterFloat(this.customTexture6OffsetYJSON);
			}
		}
	}

	// Token: 0x06006716 RID: 26390 RVA: 0x0015C3A9 File Offset: 0x0015A7A9
	protected virtual void SetAllStartingValuesToCurrentValues()
	{
	}

	// Token: 0x06006717 RID: 26391 RVA: 0x0015C3AC File Offset: 0x0015A7AC
	protected virtual void SetAllParameters()
	{
		if (this.materialForDefaults != null)
		{
			if (this.color1Name != null && this.color1Name != string.Empty && this.materialForDefaults.HasProperty(this.color1Name))
			{
				this.SetMaterialColor(this.color1Name, this.color1CurrentColor);
			}
			if (this.color1Name2 != null && this.color1Name2 != string.Empty && this.materialForDefaults.HasProperty(this.color1Name2))
			{
				this.SetMaterialColor(this.color1Name2, this.color1CurrentColor);
			}
			if (this.color2Name != null && this.color2Name != string.Empty && this.materialForDefaults.HasProperty(this.color2Name))
			{
				this.SetMaterialColor(this.color2Name, this.color2CurrentColor);
			}
			if (this.color3Name != null && this.color3Name != string.Empty && this.materialForDefaults.HasProperty(this.color3Name))
			{
				this.SetMaterialColor(this.color3Name, this.color3CurrentColor);
			}
			if (this.param1Name != null && this.param1Name != string.Empty && this.materialForDefaults.HasProperty(this.param1Name))
			{
				this.SetMaterialParam(this.param1Name, this.param1CurrentValue);
			}
			if (this.param2Name != null && this.param2Name != string.Empty && this.materialForDefaults.HasProperty(this.param2Name))
			{
				this.SetMaterialParam(this.param2Name, this.param2CurrentValue);
			}
			if (this.param3Name != null && this.param3Name != string.Empty && this.materialForDefaults.HasProperty(this.param3Name))
			{
				this.SetMaterialParam(this.param3Name, this.param3CurrentValue);
			}
			if (this.param4Name != null && this.param4Name != string.Empty && this.materialForDefaults.HasProperty(this.param4Name))
			{
				this.SetMaterialParam(this.param4Name, this.param4CurrentValue);
			}
			if (this.param5Name != null && this.param5Name != string.Empty && this.materialForDefaults.HasProperty(this.param5Name))
			{
				this.SetMaterialParam(this.param5Name, this.param5CurrentValue);
			}
			if (this.param6Name != null && this.param6Name != string.Empty && this.materialForDefaults.HasProperty(this.param6Name))
			{
				this.SetMaterialParam(this.param6Name, this.param6CurrentValue);
			}
			if (this.param7Name != null && this.param7Name != string.Empty && this.materialForDefaults.HasProperty(this.param7Name))
			{
				this.SetMaterialParam(this.param7Name, this.param7CurrentValue);
			}
			if (this.param8Name != null && this.param8Name != string.Empty && this.materialForDefaults.HasProperty(this.param8Name))
			{
				this.SetMaterialParam(this.param8Name, this.param8CurrentValue);
			}
			if (this.param9Name != null && this.param9Name != string.Empty && this.materialForDefaults.HasProperty(this.param9Name))
			{
				this.SetMaterialParam(this.param9Name, this.param9CurrentValue);
			}
			if (this.param10Name != null && this.param10Name != string.Empty && this.materialForDefaults.HasProperty(this.param10Name))
			{
				this.SetMaterialParam(this.param10Name, this.param10CurrentValue);
			}
			if (this.renderQueueJSON != null)
			{
				this.SyncRenderQueue(this.renderQueueJSON.val);
			}
			if (this.hideMaterialJSON != null)
			{
				this.SyncHideMaterial(this.hideMaterialJSON.val);
			}
		}
		if (this.textureGroup1 != null && this.textureGroup1.sets != null && this.textureGroup1.sets.Length > 0)
		{
			this.SetTextureGroup1Set(this.currentTextureGroup1Set);
		}
		if (this.textureGroup2 != null && this.textureGroup2.sets != null && this.textureGroup2.sets.Length > 0)
		{
			this.SetTextureGroup2Set(this.currentTextureGroup2Set);
		}
		if (this.textureGroup3 != null && this.textureGroup3.sets != null && this.textureGroup3.sets.Length > 0)
		{
			this.SetTextureGroup3Set(this.currentTextureGroup3Set);
		}
		if (this.textureGroup4 != null && this.textureGroup4.sets != null && this.textureGroup4.sets.Length > 0)
		{
			this.SetTextureGroup4Set(this.currentTextureGroup4Set);
		}
		if (this.textureGroup5 != null && this.textureGroup5.sets != null && this.textureGroup5.sets.Length > 0)
		{
			this.SetTextureGroup5Set(this.currentTextureGroup5Set);
		}
	}

	// Token: 0x06006718 RID: 26392 RVA: 0x0015C914 File Offset: 0x0015AD14
	public virtual void SyncAllParameters()
	{
		this.SetStartingValues();
		this.SetAllParameters();
	}

	// Token: 0x06006719 RID: 26393 RVA: 0x0015C922 File Offset: 0x0015AD22
	private void Start()
	{
		if (!this.deregisterOnDisable)
		{
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x0600671A RID: 26394 RVA: 0x0015C93B File Offset: 0x0015AD3B
	public void CheckAwake()
	{
		this.Awake();
	}

	// Token: 0x0600671B RID: 26395 RVA: 0x0015C944 File Offset: 0x0015AD44
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			if (base.enabled)
			{
				if (Application.isPlaying && this.hideShader == null)
				{
					this.hideShader = Shader.Find("Custom/Discard");
				}
				this.SetStartingValues();
			}
		}
	}

	// Token: 0x0600671C RID: 26396 RVA: 0x0015C99E File Offset: 0x0015AD9E
	protected virtual void OnEnable()
	{
		if (this.deregisterOnDisable)
		{
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x0600671D RID: 26397 RVA: 0x0015C9B7 File Offset: 0x0015ADB7
	protected virtual void OnDisable()
	{
		if (this.deregisterOnDisable)
		{
			this.DeregisterUI();
		}
	}

	// Token: 0x0600671E RID: 26398 RVA: 0x0015C9CA File Offset: 0x0015ADCA
	protected virtual void OnDestroy()
	{
		this.DestroyAllocatedObjects();
		this.DeregisterAllTextures();
	}

	// Token: 0x04005660 RID: 22112
	protected List<UnityEngine.Object> allocatedObjects;

	// Token: 0x04005661 RID: 22113
	[HideInInspector]
	public Transform copyFromTransform;

	// Token: 0x04005662 RID: 22114
	[HideInInspector]
	public int copyFromTextureGroup = 1;

	// Token: 0x04005663 RID: 22115
	[HideInInspector]
	public int copyToTextureGroup = 1;

	// Token: 0x04005664 RID: 22116
	[HideInInspector]
	public MaterialOptions copyUIFrom;

	// Token: 0x04005665 RID: 22117
	[HideInInspector]
	public MaterialOptionsUI copyUIFromUI;

	// Token: 0x04005666 RID: 22118
	public bool controlRawImage;

	// Token: 0x04005667 RID: 22119
	public bool searchInChildren = true;

	// Token: 0x04005668 RID: 22120
	public Transform materialContainer;

	// Token: 0x04005669 RID: 22121
	public Transform materialContainer2;

	// Token: 0x0400566A RID: 22122
	public Material materialForDefaults;

	// Token: 0x0400566B RID: 22123
	public Shader hideShader;

	// Token: 0x0400566C RID: 22124
	public int[] paramMaterialSlots;

	// Token: 0x0400566D RID: 22125
	public int[] paramMaterialSlots2;

	// Token: 0x0400566E RID: 22126
	public bool deregisterOnDisable;

	// Token: 0x0400566F RID: 22127
	protected string origOverrideId;

	// Token: 0x04005670 RID: 22128
	protected JSONStorableString customNameJSON;

	// Token: 0x04005671 RID: 22129
	protected JSONStorableFloat renderQueueJSON;

	// Token: 0x04005672 RID: 22130
	protected JSONStorableBool hideMaterialJSON;

	// Token: 0x04005673 RID: 22131
	protected List<MaterialOptions> otherMaterialOptionsList;

	// Token: 0x04005674 RID: 22132
	public bool allowLinkToOtherMaterials;

	// Token: 0x04005675 RID: 22133
	[SerializeField]
	protected bool _linkToOtherMaterials = true;

	// Token: 0x04005676 RID: 22134
	protected JSONStorableBool linkToOtherMaterialsJSON;

	// Token: 0x04005677 RID: 22135
	protected JSONStorableColor color1JSONParam;

	// Token: 0x04005678 RID: 22136
	public string color1Name = "_Color";

	// Token: 0x04005679 RID: 22137
	public string color1Name2;

	// Token: 0x0400567A RID: 22138
	public string color1DisplayName = "Diffuse Color";

	// Token: 0x0400567B RID: 22139
	public Text color1DisplayNameText;

	// Token: 0x0400567C RID: 22140
	public UnityEngine.Color color1CurrentColor;

	// Token: 0x0400567D RID: 22141
	public HSVColor color1CurrentHSVColor;

	// Token: 0x0400567E RID: 22142
	public HSVColorPicker color1Picker;

	// Token: 0x0400567F RID: 22143
	public RectTransform color1Container;

	// Token: 0x04005680 RID: 22144
	public float color1Alpha = 1f;

	// Token: 0x04005681 RID: 22145
	protected bool materialHasColor1;

	// Token: 0x04005682 RID: 22146
	protected JSONStorableColor color2JSONParam;

	// Token: 0x04005683 RID: 22147
	public string color2Name = "_SpecColor";

	// Token: 0x04005684 RID: 22148
	public string color2DisplayName = "Specular Color";

	// Token: 0x04005685 RID: 22149
	public Text color2DisplayNameText;

	// Token: 0x04005686 RID: 22150
	public UnityEngine.Color color2CurrentColor;

	// Token: 0x04005687 RID: 22151
	public HSVColor color2CurrentHSVColor;

	// Token: 0x04005688 RID: 22152
	public HSVColorPicker color2Picker;

	// Token: 0x04005689 RID: 22153
	public RectTransform color2Container;

	// Token: 0x0400568A RID: 22154
	public float color2Alpha = 1f;

	// Token: 0x0400568B RID: 22155
	protected bool materialHasColor2;

	// Token: 0x0400568C RID: 22156
	protected JSONStorableColor color3JSONParam;

	// Token: 0x0400568D RID: 22157
	public string color3Name = "_SubdermisColor";

	// Token: 0x0400568E RID: 22158
	public string color3DisplayName = "Subsurface Color";

	// Token: 0x0400568F RID: 22159
	public Text color3DisplayNameText;

	// Token: 0x04005690 RID: 22160
	public UnityEngine.Color color3CurrentColor;

	// Token: 0x04005691 RID: 22161
	public HSVColor color3CurrentHSVColor;

	// Token: 0x04005692 RID: 22162
	public HSVColorPicker color3Picker;

	// Token: 0x04005693 RID: 22163
	public RectTransform color3Container;

	// Token: 0x04005694 RID: 22164
	public float color3Alpha = 1f;

	// Token: 0x04005695 RID: 22165
	protected bool materialHasColor3;

	// Token: 0x04005696 RID: 22166
	protected JSONStorableFloat param1JSONParam;

	// Token: 0x04005697 RID: 22167
	public string param1Name = "_SpecOffset";

	// Token: 0x04005698 RID: 22168
	public string param1DisplayName = "Specular Texture Offset";

	// Token: 0x04005699 RID: 22169
	public Text param1DisplayNameText;

	// Token: 0x0400569A RID: 22170
	public Text param1DisplayNameTextAlt;

	// Token: 0x0400569B RID: 22171
	public float param1CurrentValue;

	// Token: 0x0400569C RID: 22172
	public float param1MinValue = -1f;

	// Token: 0x0400569D RID: 22173
	public float param1MaxValue = 1f;

	// Token: 0x0400569E RID: 22174
	public Slider param1Slider;

	// Token: 0x0400569F RID: 22175
	public Slider param1SliderAlt;

	// Token: 0x040056A0 RID: 22176
	protected bool materialHasParam1;

	// Token: 0x040056A1 RID: 22177
	protected JSONStorableFloat param2JSONParam;

	// Token: 0x040056A2 RID: 22178
	public string param2Name = "_SpecInt";

	// Token: 0x040056A3 RID: 22179
	public string param2DisplayName = "Specular Intensity";

	// Token: 0x040056A4 RID: 22180
	public Text param2DisplayNameText;

	// Token: 0x040056A5 RID: 22181
	public Text param2DisplayNameTextAlt;

	// Token: 0x040056A6 RID: 22182
	public float param2CurrentValue;

	// Token: 0x040056A7 RID: 22183
	public float param2MinValue;

	// Token: 0x040056A8 RID: 22184
	public float param2MaxValue = 10f;

	// Token: 0x040056A9 RID: 22185
	public Slider param2Slider;

	// Token: 0x040056AA RID: 22186
	public Slider param2SliderAlt;

	// Token: 0x040056AB RID: 22187
	protected bool materialHasParam2;

	// Token: 0x040056AC RID: 22188
	protected JSONStorableFloat param3JSONParam;

	// Token: 0x040056AD RID: 22189
	public string param3Name = "_Shininess";

	// Token: 0x040056AE RID: 22190
	public string param3DisplayName = "Gloss";

	// Token: 0x040056AF RID: 22191
	public Text param3DisplayNameText;

	// Token: 0x040056B0 RID: 22192
	public Text param3DisplayNameTextAlt;

	// Token: 0x040056B1 RID: 22193
	public float param3CurrentValue;

	// Token: 0x040056B2 RID: 22194
	public float param3MinValue = 2f;

	// Token: 0x040056B3 RID: 22195
	public float param3MaxValue = 8f;

	// Token: 0x040056B4 RID: 22196
	public Slider param3Slider;

	// Token: 0x040056B5 RID: 22197
	public Slider param3SliderAlt;

	// Token: 0x040056B6 RID: 22198
	protected bool materialHasParam3;

	// Token: 0x040056B7 RID: 22199
	protected JSONStorableFloat param4JSONParam;

	// Token: 0x040056B8 RID: 22200
	public string param4Name = "_Fresnel";

	// Token: 0x040056B9 RID: 22201
	public string param4DisplayName = "Specular Fresnel";

	// Token: 0x040056BA RID: 22202
	public Text param4DisplayNameText;

	// Token: 0x040056BB RID: 22203
	public Text param4DisplayNameTextAlt;

	// Token: 0x040056BC RID: 22204
	public float param4CurrentValue;

	// Token: 0x040056BD RID: 22205
	public float param4MinValue;

	// Token: 0x040056BE RID: 22206
	public float param4MaxValue = 1f;

	// Token: 0x040056BF RID: 22207
	public Slider param4Slider;

	// Token: 0x040056C0 RID: 22208
	public Slider param4SliderAlt;

	// Token: 0x040056C1 RID: 22209
	protected bool materialHasParam4;

	// Token: 0x040056C2 RID: 22210
	protected JSONStorableFloat param5JSONParam;

	// Token: 0x040056C3 RID: 22211
	public string param5Name = "_GlossOffset";

	// Token: 0x040056C4 RID: 22212
	public string param5DisplayName = "Gloss Texture Offset";

	// Token: 0x040056C5 RID: 22213
	public Text param5DisplayNameText;

	// Token: 0x040056C6 RID: 22214
	public Text param5DisplayNameTextAlt;

	// Token: 0x040056C7 RID: 22215
	public float param5CurrentValue;

	// Token: 0x040056C8 RID: 22216
	public float param5MinValue = -1f;

	// Token: 0x040056C9 RID: 22217
	public float param5MaxValue = 1f;

	// Token: 0x040056CA RID: 22218
	public Slider param5Slider;

	// Token: 0x040056CB RID: 22219
	public Slider param5SliderAlt;

	// Token: 0x040056CC RID: 22220
	protected bool materialHasParam5;

	// Token: 0x040056CD RID: 22221
	protected JSONStorableFloat param6JSONParam;

	// Token: 0x040056CE RID: 22222
	public string param6Name = "_IBLFilter";

	// Token: 0x040056CF RID: 22223
	public string param6DisplayName = "Global Illumination Filter";

	// Token: 0x040056D0 RID: 22224
	public Text param6DisplayNameText;

	// Token: 0x040056D1 RID: 22225
	public Text param6DisplayNameTextAlt;

	// Token: 0x040056D2 RID: 22226
	public float param6CurrentValue;

	// Token: 0x040056D3 RID: 22227
	public float param6MinValue;

	// Token: 0x040056D4 RID: 22228
	public float param6MaxValue = 1f;

	// Token: 0x040056D5 RID: 22229
	public Slider param6Slider;

	// Token: 0x040056D6 RID: 22230
	public Slider param6SliderAlt;

	// Token: 0x040056D7 RID: 22231
	protected bool materialHasParam6;

	// Token: 0x040056D8 RID: 22232
	protected JSONStorableFloat param7JSONParam;

	// Token: 0x040056D9 RID: 22233
	public string param7Name = "_AlphaAdjust";

	// Token: 0x040056DA RID: 22234
	public string param7DisplayName = "Alpha Adjust";

	// Token: 0x040056DB RID: 22235
	public Text param7DisplayNameText;

	// Token: 0x040056DC RID: 22236
	public Text param7DisplayNameTextAlt;

	// Token: 0x040056DD RID: 22237
	public float param7CurrentValue;

	// Token: 0x040056DE RID: 22238
	public float param7MinValue = -1f;

	// Token: 0x040056DF RID: 22239
	public float param7MaxValue = 1f;

	// Token: 0x040056E0 RID: 22240
	public Slider param7Slider;

	// Token: 0x040056E1 RID: 22241
	public Slider param7SliderAlt;

	// Token: 0x040056E2 RID: 22242
	protected bool materialHasParam7;

	// Token: 0x040056E3 RID: 22243
	protected JSONStorableFloat param8JSONParam;

	// Token: 0x040056E4 RID: 22244
	public string param8Name = "_DiffOffset";

	// Token: 0x040056E5 RID: 22245
	public string param8DisplayName = "Diffuse Texture Offset";

	// Token: 0x040056E6 RID: 22246
	public Text param8DisplayNameText;

	// Token: 0x040056E7 RID: 22247
	public Text param8DisplayNameTextAlt;

	// Token: 0x040056E8 RID: 22248
	public float param8CurrentValue;

	// Token: 0x040056E9 RID: 22249
	public float param8MinValue = -1f;

	// Token: 0x040056EA RID: 22250
	public float param8MaxValue = 1f;

	// Token: 0x040056EB RID: 22251
	public Slider param8Slider;

	// Token: 0x040056EC RID: 22252
	public Slider param8SliderAlt;

	// Token: 0x040056ED RID: 22253
	protected bool materialHasParam8;

	// Token: 0x040056EE RID: 22254
	protected JSONStorableFloat param9JSONParam;

	// Token: 0x040056EF RID: 22255
	public string param9Name = "_DiffuseBumpiness";

	// Token: 0x040056F0 RID: 22256
	public string param9DisplayName = "Diffuse Bumpiness";

	// Token: 0x040056F1 RID: 22257
	public Text param9DisplayNameText;

	// Token: 0x040056F2 RID: 22258
	public Text param9DisplayNameTextAlt;

	// Token: 0x040056F3 RID: 22259
	public float param9CurrentValue;

	// Token: 0x040056F4 RID: 22260
	public float param9MinValue;

	// Token: 0x040056F5 RID: 22261
	public float param9MaxValue = 2f;

	// Token: 0x040056F6 RID: 22262
	public Slider param9Slider;

	// Token: 0x040056F7 RID: 22263
	public Slider param9SliderAlt;

	// Token: 0x040056F8 RID: 22264
	protected bool materialHasParam9;

	// Token: 0x040056F9 RID: 22265
	protected JSONStorableFloat param10JSONParam;

	// Token: 0x040056FA RID: 22266
	public string param10Name = "_SpecularBumpiness";

	// Token: 0x040056FB RID: 22267
	public string param10DisplayName = "Specular Bumpiness";

	// Token: 0x040056FC RID: 22268
	public Text param10DisplayNameText;

	// Token: 0x040056FD RID: 22269
	public Text param10DisplayNameTextAlt;

	// Token: 0x040056FE RID: 22270
	public float param10CurrentValue;

	// Token: 0x040056FF RID: 22271
	public float param10MinValue;

	// Token: 0x04005700 RID: 22272
	public float param10MaxValue = 2f;

	// Token: 0x04005701 RID: 22273
	public Slider param10Slider;

	// Token: 0x04005702 RID: 22274
	public Slider param10SliderAlt;

	// Token: 0x04005703 RID: 22275
	protected bool materialHasParam10;

	// Token: 0x04005704 RID: 22276
	public MaterialOptionTextureGroup textureGroup1;

	// Token: 0x04005705 RID: 22277
	protected JSONStorableStringChooser textureGroup1JSON;

	// Token: 0x04005706 RID: 22278
	public UIPopup textureGroup1Popup;

	// Token: 0x04005707 RID: 22279
	public UIPopup textureGroup1PopupAlt;

	// Token: 0x04005708 RID: 22280
	public string startingTextureGroup1Set;

	// Token: 0x04005709 RID: 22281
	public string currentTextureGroup1Set;

	// Token: 0x0400570A RID: 22282
	protected bool hasTextureGroup1;

	// Token: 0x0400570B RID: 22283
	public MaterialOptionTextureGroup textureGroup2;

	// Token: 0x0400570C RID: 22284
	protected JSONStorableStringChooser textureGroup2JSON;

	// Token: 0x0400570D RID: 22285
	public UIPopup textureGroup2Popup;

	// Token: 0x0400570E RID: 22286
	public UIPopup textureGroup2PopupAlt;

	// Token: 0x0400570F RID: 22287
	public string startingTextureGroup2Set;

	// Token: 0x04005710 RID: 22288
	public string currentTextureGroup2Set;

	// Token: 0x04005711 RID: 22289
	protected bool hasTextureGroup2;

	// Token: 0x04005712 RID: 22290
	public MaterialOptionTextureGroup textureGroup3;

	// Token: 0x04005713 RID: 22291
	protected JSONStorableStringChooser textureGroup3JSON;

	// Token: 0x04005714 RID: 22292
	public UIPopup textureGroup3Popup;

	// Token: 0x04005715 RID: 22293
	public UIPopup textureGroup3PopupAlt;

	// Token: 0x04005716 RID: 22294
	public string startingTextureGroup3Set;

	// Token: 0x04005717 RID: 22295
	public string currentTextureGroup3Set;

	// Token: 0x04005718 RID: 22296
	protected bool hasTextureGroup3;

	// Token: 0x04005719 RID: 22297
	public MaterialOptionTextureGroup textureGroup4;

	// Token: 0x0400571A RID: 22298
	protected JSONStorableStringChooser textureGroup4JSON;

	// Token: 0x0400571B RID: 22299
	public UIPopup textureGroup4Popup;

	// Token: 0x0400571C RID: 22300
	public UIPopup textureGroup4PopupAlt;

	// Token: 0x0400571D RID: 22301
	public string startingTextureGroup4Set;

	// Token: 0x0400571E RID: 22302
	public string currentTextureGroup4Set;

	// Token: 0x0400571F RID: 22303
	protected bool hasTextureGroup4;

	// Token: 0x04005720 RID: 22304
	public MaterialOptionTextureGroup textureGroup5;

	// Token: 0x04005721 RID: 22305
	protected JSONStorableStringChooser textureGroup5JSON;

	// Token: 0x04005722 RID: 22306
	public UIPopup textureGroup5Popup;

	// Token: 0x04005723 RID: 22307
	public UIPopup textureGroup5PopupAlt;

	// Token: 0x04005724 RID: 22308
	public string startingTextureGroup5Set;

	// Token: 0x04005725 RID: 22309
	public string currentTextureGroup5Set;

	// Token: 0x04005726 RID: 22310
	protected bool hasTextureGroup5;

	// Token: 0x04005727 RID: 22311
	public string customTextureFolder;

	// Token: 0x04005728 RID: 22312
	protected string customTexturePackageFolder;

	// Token: 0x04005729 RID: 22313
	protected JSONStorableAction openTextureFolderInExplorerAction;

	// Token: 0x0400572A RID: 22314
	public bool customTexture1IsLinear;

	// Token: 0x0400572B RID: 22315
	public bool customTexture1IsNormal;

	// Token: 0x0400572C RID: 22316
	public bool customTexture1IsTransparency;

	// Token: 0x0400572D RID: 22317
	public Button customTexture1FileBrowseButton;

	// Token: 0x0400572E RID: 22318
	public Button customTexture1ReloadButton;

	// Token: 0x0400572F RID: 22319
	public Button customTexture1ClearButton;

	// Token: 0x04005730 RID: 22320
	public Button customTexture1NullButton;

	// Token: 0x04005731 RID: 22321
	public Button customTexture1DefaultButton;

	// Token: 0x04005732 RID: 22322
	public Text customTexture1UrlText;

	// Token: 0x04005733 RID: 22323
	public Text customTexture1Label;

	// Token: 0x04005734 RID: 22324
	protected Texture2D customTexture1;

	// Token: 0x04005735 RID: 22325
	protected bool customTexture1IsNull;

	// Token: 0x04005736 RID: 22326
	protected JSONStorableUrl customTexture1UrlJSON;

	// Token: 0x04005737 RID: 22327
	protected JSONStorableFloat customTexture1TileXJSON;

	// Token: 0x04005738 RID: 22328
	public Slider customTexture1TileXSlider;

	// Token: 0x04005739 RID: 22329
	protected JSONStorableFloat customTexture1TileYJSON;

	// Token: 0x0400573A RID: 22330
	public Slider customTexture1TileYSlider;

	// Token: 0x0400573B RID: 22331
	protected JSONStorableFloat customTexture1OffsetXJSON;

	// Token: 0x0400573C RID: 22332
	public Slider customTexture1OffsetXSlider;

	// Token: 0x0400573D RID: 22333
	protected JSONStorableFloat customTexture1OffsetYJSON;

	// Token: 0x0400573E RID: 22334
	public Slider customTexture1OffsetYSlider;

	// Token: 0x0400573F RID: 22335
	public bool customTexture2IsLinear = true;

	// Token: 0x04005740 RID: 22336
	public bool customTexture2IsNormal;

	// Token: 0x04005741 RID: 22337
	public bool customTexture2IsTransparency;

	// Token: 0x04005742 RID: 22338
	public Button customTexture2FileBrowseButton;

	// Token: 0x04005743 RID: 22339
	public Button customTexture2ReloadButton;

	// Token: 0x04005744 RID: 22340
	public Button customTexture2ClearButton;

	// Token: 0x04005745 RID: 22341
	public Button customTexture2NullButton;

	// Token: 0x04005746 RID: 22342
	public Button customTexture2DefaultButton;

	// Token: 0x04005747 RID: 22343
	public Text customTexture2UrlText;

	// Token: 0x04005748 RID: 22344
	public Text customTexture2Label;

	// Token: 0x04005749 RID: 22345
	protected Texture2D customTexture2;

	// Token: 0x0400574A RID: 22346
	protected bool customTexture2IsNull;

	// Token: 0x0400574B RID: 22347
	protected JSONStorableUrl customTexture2UrlJSON;

	// Token: 0x0400574C RID: 22348
	protected JSONStorableFloat customTexture2TileXJSON;

	// Token: 0x0400574D RID: 22349
	public Slider customTexture2TileXSlider;

	// Token: 0x0400574E RID: 22350
	protected JSONStorableFloat customTexture2TileYJSON;

	// Token: 0x0400574F RID: 22351
	public Slider customTexture2TileYSlider;

	// Token: 0x04005750 RID: 22352
	protected JSONStorableFloat customTexture2OffsetXJSON;

	// Token: 0x04005751 RID: 22353
	public Slider customTexture2OffsetXSlider;

	// Token: 0x04005752 RID: 22354
	protected JSONStorableFloat customTexture2OffsetYJSON;

	// Token: 0x04005753 RID: 22355
	public Slider customTexture2OffsetYSlider;

	// Token: 0x04005754 RID: 22356
	public bool customTexture3IsLinear = true;

	// Token: 0x04005755 RID: 22357
	public bool customTexture3IsNormal;

	// Token: 0x04005756 RID: 22358
	public bool customTexture3IsTransparency;

	// Token: 0x04005757 RID: 22359
	public Button customTexture3FileBrowseButton;

	// Token: 0x04005758 RID: 22360
	public Button customTexture3ReloadButton;

	// Token: 0x04005759 RID: 22361
	public Button customTexture3ClearButton;

	// Token: 0x0400575A RID: 22362
	public Button customTexture3NullButton;

	// Token: 0x0400575B RID: 22363
	public Button customTexture3DefaultButton;

	// Token: 0x0400575C RID: 22364
	public Text customTexture3UrlText;

	// Token: 0x0400575D RID: 22365
	public Text customTexture3Label;

	// Token: 0x0400575E RID: 22366
	protected Texture2D customTexture3;

	// Token: 0x0400575F RID: 22367
	protected bool customTexture3IsNull;

	// Token: 0x04005760 RID: 22368
	protected JSONStorableUrl customTexture3UrlJSON;

	// Token: 0x04005761 RID: 22369
	protected JSONStorableFloat customTexture3TileXJSON;

	// Token: 0x04005762 RID: 22370
	public Slider customTexture3TileXSlider;

	// Token: 0x04005763 RID: 22371
	protected JSONStorableFloat customTexture3TileYJSON;

	// Token: 0x04005764 RID: 22372
	public Slider customTexture3TileYSlider;

	// Token: 0x04005765 RID: 22373
	protected JSONStorableFloat customTexture3OffsetXJSON;

	// Token: 0x04005766 RID: 22374
	public Slider customTexture3OffsetXSlider;

	// Token: 0x04005767 RID: 22375
	protected JSONStorableFloat customTexture3OffsetYJSON;

	// Token: 0x04005768 RID: 22376
	public Slider customTexture3OffsetYSlider;

	// Token: 0x04005769 RID: 22377
	public bool customTexture4IsLinear;

	// Token: 0x0400576A RID: 22378
	public bool customTexture4IsNormal;

	// Token: 0x0400576B RID: 22379
	public bool customTexture4IsTransparency = true;

	// Token: 0x0400576C RID: 22380
	public Button customTexture4FileBrowseButton;

	// Token: 0x0400576D RID: 22381
	public Button customTexture4ReloadButton;

	// Token: 0x0400576E RID: 22382
	public Button customTexture4ClearButton;

	// Token: 0x0400576F RID: 22383
	public Button customTexture4NullButton;

	// Token: 0x04005770 RID: 22384
	public Button customTexture4DefaultButton;

	// Token: 0x04005771 RID: 22385
	public Text customTexture4UrlText;

	// Token: 0x04005772 RID: 22386
	public Text customTexture4Label;

	// Token: 0x04005773 RID: 22387
	protected Texture2D customTexture4;

	// Token: 0x04005774 RID: 22388
	protected bool customTexture4IsNull;

	// Token: 0x04005775 RID: 22389
	protected JSONStorableUrl customTexture4UrlJSON;

	// Token: 0x04005776 RID: 22390
	protected JSONStorableFloat customTexture4TileXJSON;

	// Token: 0x04005777 RID: 22391
	public Slider customTexture4TileXSlider;

	// Token: 0x04005778 RID: 22392
	protected JSONStorableFloat customTexture4TileYJSON;

	// Token: 0x04005779 RID: 22393
	public Slider customTexture4TileYSlider;

	// Token: 0x0400577A RID: 22394
	protected JSONStorableFloat customTexture4OffsetXJSON;

	// Token: 0x0400577B RID: 22395
	public Slider customTexture4OffsetXSlider;

	// Token: 0x0400577C RID: 22396
	protected JSONStorableFloat customTexture4OffsetYJSON;

	// Token: 0x0400577D RID: 22397
	public Slider customTexture4OffsetYSlider;

	// Token: 0x0400577E RID: 22398
	public bool customTexture5IsLinear = true;

	// Token: 0x0400577F RID: 22399
	public bool customTexture5IsNormal = true;

	// Token: 0x04005780 RID: 22400
	public bool customTexture5IsTransparency;

	// Token: 0x04005781 RID: 22401
	public Button customTexture5FileBrowseButton;

	// Token: 0x04005782 RID: 22402
	public Button customTexture5ReloadButton;

	// Token: 0x04005783 RID: 22403
	public Button customTexture5ClearButton;

	// Token: 0x04005784 RID: 22404
	public Button customTexture5NullButton;

	// Token: 0x04005785 RID: 22405
	public Button customTexture5DefaultButton;

	// Token: 0x04005786 RID: 22406
	public Text customTexture5UrlText;

	// Token: 0x04005787 RID: 22407
	public Text customTexture5Label;

	// Token: 0x04005788 RID: 22408
	protected Texture2D customTexture5;

	// Token: 0x04005789 RID: 22409
	protected bool customTexture5IsNull;

	// Token: 0x0400578A RID: 22410
	protected JSONStorableUrl customTexture5UrlJSON;

	// Token: 0x0400578B RID: 22411
	protected JSONStorableFloat customTexture5TileXJSON;

	// Token: 0x0400578C RID: 22412
	public Slider customTexture5TileXSlider;

	// Token: 0x0400578D RID: 22413
	protected JSONStorableFloat customTexture5TileYJSON;

	// Token: 0x0400578E RID: 22414
	public Slider customTexture5TileYSlider;

	// Token: 0x0400578F RID: 22415
	protected JSONStorableFloat customTexture5OffsetXJSON;

	// Token: 0x04005790 RID: 22416
	public Slider customTexture5OffsetXSlider;

	// Token: 0x04005791 RID: 22417
	protected JSONStorableFloat customTexture5OffsetYJSON;

	// Token: 0x04005792 RID: 22418
	public Slider customTexture5OffsetYSlider;

	// Token: 0x04005793 RID: 22419
	public bool customTexture6IsLinear;

	// Token: 0x04005794 RID: 22420
	public bool customTexture6IsNormal;

	// Token: 0x04005795 RID: 22421
	public bool customTexture6IsTransparency;

	// Token: 0x04005796 RID: 22422
	public Button customTexture6FileBrowseButton;

	// Token: 0x04005797 RID: 22423
	public Button customTexture6ReloadButton;

	// Token: 0x04005798 RID: 22424
	public Button customTexture6ClearButton;

	// Token: 0x04005799 RID: 22425
	public Button customTexture6NullButton;

	// Token: 0x0400579A RID: 22426
	public Button customTexture6DefaultButton;

	// Token: 0x0400579B RID: 22427
	public Text customTexture6UrlText;

	// Token: 0x0400579C RID: 22428
	public Text customTexture6Label;

	// Token: 0x0400579D RID: 22429
	protected Texture2D customTexture6;

	// Token: 0x0400579E RID: 22430
	protected bool customTexture6IsNull;

	// Token: 0x0400579F RID: 22431
	protected JSONStorableUrl customTexture6UrlJSON;

	// Token: 0x040057A0 RID: 22432
	protected JSONStorableFloat customTexture6TileXJSON;

	// Token: 0x040057A1 RID: 22433
	public Slider customTexture6TileXSlider;

	// Token: 0x040057A2 RID: 22434
	protected JSONStorableFloat customTexture6TileYJSON;

	// Token: 0x040057A3 RID: 22435
	public Slider customTexture6TileYSlider;

	// Token: 0x040057A4 RID: 22436
	protected JSONStorableFloat customTexture6OffsetXJSON;

	// Token: 0x040057A5 RID: 22437
	public Slider customTexture6OffsetXSlider;

	// Token: 0x040057A6 RID: 22438
	protected JSONStorableFloat customTexture6OffsetYJSON;

	// Token: 0x040057A7 RID: 22439
	public Slider customTexture6OffsetYSlider;

	// Token: 0x040057A8 RID: 22440
	protected JSONStorableAction createUVTemplateTextureJSON;

	// Token: 0x040057A9 RID: 22441
	protected JSONStorableAction createSimTemplateTextureJSON;

	// Token: 0x040057AA RID: 22442
	protected Material[] rawImageMaterials;

	// Token: 0x040057AB RID: 22443
	protected MeshFilter meshFilter;

	// Token: 0x040057AC RID: 22444
	protected Renderer[] renderers;

	// Token: 0x040057AD RID: 22445
	protected Renderer[] renderers2;

	// Token: 0x040057AE RID: 22446
	protected Dictionary<Material, Shader> materialToOriginalShader;

	// Token: 0x040057AF RID: 22447
	protected Dictionary<Texture2D, int> textureUseCount;
}
