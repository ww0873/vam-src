using System;

namespace Leap.Unity.Animation
{
	// Token: 0x02000640 RID: 1600
	public interface IInterpolator : IPoolable, IDisposable
	{
		// Token: 0x06002720 RID: 10016
		void Interpolate(float percent);

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06002721 RID: 10017
		float length { get; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002722 RID: 10018
		bool isValid { get; }
	}
}
