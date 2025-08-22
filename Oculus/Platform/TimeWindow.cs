using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020008A7 RID: 2215
	public enum TimeWindow
	{
		// Token: 0x04002903 RID: 10499
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x04002904 RID: 10500
		[Description("ONE_HOUR")]
		OneHour,
		// Token: 0x04002905 RID: 10501
		[Description("ONE_DAY")]
		OneDay,
		// Token: 0x04002906 RID: 10502
		[Description("ONE_WEEK")]
		OneWeek,
		// Token: 0x04002907 RID: 10503
		[Description("THIRTY_DAYS")]
		ThirtyDays,
		// Token: 0x04002908 RID: 10504
		[Description("NINETY_DAYS")]
		NinetyDays
	}
}
