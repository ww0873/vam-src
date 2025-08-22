using System;
using System.Threading;

// Token: 0x02000B01 RID: 2817
public class DAZSkinTaskInfo
{
	// Token: 0x06004C7E RID: 19582 RVA: 0x001AE4A9 File Offset: 0x001AC8A9
	public DAZSkinTaskInfo()
	{
	}

	// Token: 0x04003B4E RID: 15182
	public DAZSkinTaskType taskType;

	// Token: 0x04003B4F RID: 15183
	public int threadIndex;

	// Token: 0x04003B50 RID: 15184
	public string name;

	// Token: 0x04003B51 RID: 15185
	public AutoResetEvent resetEvent;

	// Token: 0x04003B52 RID: 15186
	public Thread thread;

	// Token: 0x04003B53 RID: 15187
	public volatile bool working;

	// Token: 0x04003B54 RID: 15188
	public volatile bool kill;

	// Token: 0x04003B55 RID: 15189
	public int index1;

	// Token: 0x04003B56 RID: 15190
	public int index2;
}
