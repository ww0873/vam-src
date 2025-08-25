using System;

namespace Obi
{
	// Token: 0x020003E4 RID: 996
	public interface IObiSolverClient
	{
		// Token: 0x06001931 RID: 6449
		bool AddToSolver(object info);

		// Token: 0x06001932 RID: 6450
		bool RemoveFromSolver(object info);

		// Token: 0x06001933 RID: 6451
		void PushDataToSolver(ParticleData data = ParticleData.NONE);

		// Token: 0x06001934 RID: 6452
		void PullDataFromSolver(ParticleData data = ParticleData.NONE);
	}
}
