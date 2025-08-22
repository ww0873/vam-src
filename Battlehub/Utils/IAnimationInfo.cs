using System;

namespace Battlehub.Utils
{
	// Token: 0x020002AB RID: 683
	public interface IAnimationInfo
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06001013 RID: 4115
		float Duration { get; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06001014 RID: 4116
		// (set) Token: 0x06001015 RID: 4117
		float T { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06001016 RID: 4118
		bool InProgress { get; }

		// Token: 0x06001017 RID: 4119
		void Abort();
	}
}
