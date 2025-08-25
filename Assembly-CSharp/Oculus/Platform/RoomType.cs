using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x0200089F RID: 2207
	public enum RoomType
	{
		// Token: 0x040028EC RID: 10476
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x040028ED RID: 10477
		[Description("MATCHMAKING")]
		Matchmaking,
		// Token: 0x040028EE RID: 10478
		[Description("MODERATED")]
		Moderated,
		// Token: 0x040028EF RID: 10479
		[Description("PRIVATE")]
		Private,
		// Token: 0x040028F0 RID: 10480
		[Description("SOLO")]
		Solo
	}
}
