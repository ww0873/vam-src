using System;
using System.Threading;

// Token: 0x02000AF3 RID: 2803
public class DAZMorphTaskInfo
{
	// Token: 0x06004B4C RID: 19276 RVA: 0x001A1096 File Offset: 0x0019F496
	public DAZMorphTaskInfo()
	{
	}

	// Token: 0x04003A23 RID: 14883
	public DAZMorphTaskType taskType;

	// Token: 0x04003A24 RID: 14884
	public int threadIndex;

	// Token: 0x04003A25 RID: 14885
	public string name;

	// Token: 0x04003A26 RID: 14886
	public AutoResetEvent resetEvent;

	// Token: 0x04003A27 RID: 14887
	public Thread thread;

	// Token: 0x04003A28 RID: 14888
	public volatile bool working;

	// Token: 0x04003A29 RID: 14889
	public volatile bool kill;

	// Token: 0x04003A2A RID: 14890
	public int index1;

	// Token: 0x04003A2B RID: 14891
	public int index2;
}
