using System;

// Token: 0x02000CD0 RID: 3280
public class JSONStorableActionAudioClip
{
	// Token: 0x06006338 RID: 25400 RVA: 0x0025D098 File Offset: 0x0025B498
	public JSONStorableActionAudioClip(string n, JSONStorableActionAudioClip.AudioClipActionCallback callback)
	{
		this.name = n;
		this.actionCallback = callback;
	}

	// Token: 0x040053C6 RID: 21446
	public string name;

	// Token: 0x040053C7 RID: 21447
	public JSONStorableActionAudioClip.AudioClipActionCallback actionCallback;

	// Token: 0x040053C8 RID: 21448
	public JSONStorable storable;

	// Token: 0x02000CD1 RID: 3281
	// (Invoke) Token: 0x0600633A RID: 25402
	public delegate void AudioClipActionCallback(NamedAudioClip nac);
}
