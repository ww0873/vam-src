using System;

// Token: 0x02000CD4 RID: 3284
public class JSONStorableActionSceneFilePath
{
	// Token: 0x06006343 RID: 25411 RVA: 0x0025D0EF File Offset: 0x0025B4EF
	public JSONStorableActionSceneFilePath(string n, JSONStorableActionSceneFilePath.SceneFilePathActionCallback callback)
	{
		this.name = n;
		this.actionCallback = callback;
	}

	// Token: 0x040053CD RID: 21453
	public string name;

	// Token: 0x040053CE RID: 21454
	public JSONStorableActionSceneFilePath.SceneFilePathActionCallback actionCallback;

	// Token: 0x040053CF RID: 21455
	public JSONStorable storable;

	// Token: 0x02000CD5 RID: 3285
	// (Invoke) Token: 0x06006345 RID: 25413
	public delegate void SceneFilePathActionCallback(string path);
}
