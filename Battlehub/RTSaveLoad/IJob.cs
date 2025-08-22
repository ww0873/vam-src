using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200024A RID: 586
	public interface IJob
	{
		// Token: 0x06000C31 RID: 3121
		void Submit(Action<Action> job, Action completed);
	}
}
