using System;
using System.Collections.Generic;
using System.IO;
using MVR.FileManagement;
using UnityEngine;

// Token: 0x02000C23 RID: 3107
public class JSONSceneLoader : JSONStorable
{
	// Token: 0x06005A50 RID: 23120 RVA: 0x00213013 File Offset: 0x00211413
	public JSONSceneLoader()
	{
	}

	// Token: 0x06005A51 RID: 23121 RVA: 0x0021301C File Offset: 0x0021141C
	protected void LoadScene(string path)
	{
		if (SuperController.singleton != null)
		{
			if (SuperController.singleton.gameMode == SuperController.GameMode.Play)
			{
				if (FileManager.FileExists(path, false, false))
				{
					SuperController.singleton.Load(path);
				}
				else
				{
					string fileName = Path.GetFileName(path);
					JSONEmbed je;
					if (this.embedSceneNameToScene.TryGetValue(fileName, out je))
					{
						SuperController.singleton.LoadFromJSONEmbed(je, false, false);
					}
				}
			}
			else
			{
				SuperController.LogError("Scene load triggered while in edit mode. Scene load triggers only apply in Play mode");
			}
		}
	}

	// Token: 0x06005A52 RID: 23122 RVA: 0x0021309C File Offset: 0x0021149C
	protected void MergeLoadScene(string path)
	{
		if (SuperController.singleton != null)
		{
			if (SuperController.singleton.gameMode == SuperController.GameMode.Play)
			{
				if (FileManager.FileExists(path, false, false))
				{
					SuperController.singleton.LoadMerge(path);
				}
				else
				{
					string fileName = Path.GetFileName(path);
					JSONEmbed je;
					if (this.embedSceneNameToScene.TryGetValue(fileName, out je))
					{
						SuperController.singleton.LoadFromJSONEmbed(je, true, false);
					}
				}
			}
			else
			{
				SuperController.LogError("Merge scene load triggered while in edit mode. Scene load triggers only apply in Play mode");
			}
		}
	}

	// Token: 0x06005A53 RID: 23123 RVA: 0x0021311C File Offset: 0x0021151C
	protected void LoadEmbedScene()
	{
		JSONEmbed je;
		if (this.embedSceneNameJSON != null && this.embedSceneNameJSON.val != string.Empty && this.embedSceneNameToScene.TryGetValue(this.embedSceneNameJSON.val, out je))
		{
			SuperController.singleton.LoadFromJSONEmbed(je, false, false);
		}
	}

	// Token: 0x06005A54 RID: 23124 RVA: 0x00213178 File Offset: 0x00211578
	protected void LoadEmbedSceneMerge()
	{
		JSONEmbed je;
		if (this.embedSceneNameJSON != null && this.embedSceneNameJSON.val != string.Empty && this.embedSceneNameToScene.TryGetValue(this.embedSceneNameJSON.val, out je))
		{
			SuperController.singleton.LoadFromJSONEmbed(je, true, false);
		}
	}

	// Token: 0x06005A55 RID: 23125 RVA: 0x002131D4 File Offset: 0x002115D4
	protected void Init()
	{
		this.loadSceneJSONAction = new JSONStorableActionSceneFilePath("LoadScene", new JSONStorableActionSceneFilePath.SceneFilePathActionCallback(this.LoadScene));
		base.RegisterSceneFilePathAction(this.loadSceneJSONAction);
		this.mergeLoadSceneJSONAction = new JSONStorableActionSceneFilePath("MergeLoadScene", new JSONStorableActionSceneFilePath.SceneFilePathActionCallback(this.MergeLoadScene));
		base.RegisterSceneFilePathAction(this.mergeLoadSceneJSONAction);
		this.embedSceneNameJSON = new JSONStorableString("embedSceneName", string.Empty);
		base.RegisterString(this.embedSceneNameJSON);
		this.loadEmbedScene = new JSONStorableAction("LoadEmbedScene", new JSONStorableAction.ActionCallback(this.LoadEmbedScene));
		base.RegisterAction(this.loadEmbedScene);
		this.loadEmbedSceneMerge = new JSONStorableAction("LoadEmbedSceneMerge", new JSONStorableAction.ActionCallback(this.LoadEmbedSceneMerge));
		base.RegisterAction(this.loadEmbedSceneMerge);
		this.embedSceneNameToScene = new Dictionary<string, JSONEmbed>();
		if (this.embedSceneContainer != null)
		{
			JSONEmbed[] componentsInChildren = this.embedSceneContainer.GetComponentsInChildren<JSONEmbed>();
			foreach (JSONEmbed jsonembed in componentsInChildren)
			{
				this.embedSceneNameToScene.Add(jsonembed.name, jsonembed);
			}
		}
	}

	// Token: 0x06005A56 RID: 23126 RVA: 0x002132F6 File Offset: 0x002116F6
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

	// Token: 0x04004A95 RID: 19093
	protected JSONStorableActionSceneFilePath loadSceneJSONAction;

	// Token: 0x04004A96 RID: 19094
	protected JSONStorableActionSceneFilePath mergeLoadSceneJSONAction;

	// Token: 0x04004A97 RID: 19095
	public Transform embedSceneContainer;

	// Token: 0x04004A98 RID: 19096
	protected Dictionary<string, JSONEmbed> embedSceneNameToScene;

	// Token: 0x04004A99 RID: 19097
	protected JSONStorableString embedSceneNameJSON;

	// Token: 0x04004A9A RID: 19098
	protected JSONStorableAction loadEmbedScene;

	// Token: 0x04004A9B RID: 19099
	protected JSONStorableAction loadEmbedSceneMerge;
}
