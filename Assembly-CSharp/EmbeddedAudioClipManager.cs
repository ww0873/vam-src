using System;

// Token: 0x02000B7E RID: 2942
public class EmbeddedAudioClipManager : AudioClipManager
{
	// Token: 0x060052C0 RID: 21184 RVA: 0x001DEC00 File Offset: 0x001DD000
	public EmbeddedAudioClipManager()
	{
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x001DEC08 File Offset: 0x001DD008
	protected override void Init()
	{
		base.Init();
		EmbeddedAudioClipManager.singleton = this;
		foreach (NamedAudioClip nac in this.embeddedClips)
		{
			this.AddClip(nac);
		}
	}

	// Token: 0x04004298 RID: 17048
	public static EmbeddedAudioClipManager singleton;

	// Token: 0x04004299 RID: 17049
	public string clipsFile;

	// Token: 0x0400429A RID: 17050
	public NamedAudioClip[] embeddedClips;
}
