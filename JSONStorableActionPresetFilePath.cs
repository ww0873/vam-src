using System;

// Token: 0x02000CD2 RID: 3282
public class JSONStorableActionPresetFilePath
{
	// Token: 0x0600633D RID: 25405 RVA: 0x0025D0AE File Offset: 0x0025B4AE
	public JSONStorableActionPresetFilePath(string n, JSONStorableActionPresetFilePath.PresetFilePathActionCallback callback, JSONStorableUrl urlJSON)
	{
		this.name = n;
		this.actionCallback = callback;
		this.url = urlJSON;
	}

	// Token: 0x0600633E RID: 25406 RVA: 0x0025D0CB File Offset: 0x0025B4CB
	public void Browse(JSONStorableString.SetStringCallback callback)
	{
		if (this.url != null)
		{
			this.url.setCallbackFunction = callback;
			this.url.FileBrowse();
		}
	}

	// Token: 0x040053C9 RID: 21449
	protected JSONStorableUrl url;

	// Token: 0x040053CA RID: 21450
	public string name;

	// Token: 0x040053CB RID: 21451
	public JSONStorableActionPresetFilePath.PresetFilePathActionCallback actionCallback;

	// Token: 0x040053CC RID: 21452
	public JSONStorable storable;

	// Token: 0x02000CD3 RID: 3283
	// (Invoke) Token: 0x06006340 RID: 25408
	public delegate void PresetFilePathActionCallback(string path);
}
