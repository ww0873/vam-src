using System;

// Token: 0x0200077C RID: 1916
[Serializable]
public class PacketRecordSettings
{
	// Token: 0x0600316F RID: 12655 RVA: 0x00101520 File Offset: 0x000FF920
	public PacketRecordSettings()
	{
	}

	// Token: 0x04002535 RID: 9525
	internal bool RecordingFrames;

	// Token: 0x04002536 RID: 9526
	public float UpdateRate = 0.033333335f;

	// Token: 0x04002537 RID: 9527
	internal float AccumulatedTime;
}
