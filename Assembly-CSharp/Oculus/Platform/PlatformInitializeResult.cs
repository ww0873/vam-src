using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x02000894 RID: 2196
	public enum PlatformInitializeResult
	{
		// Token: 0x040028A6 RID: 10406
		[Description("SUCCESS")]
		Success,
		// Token: 0x040028A7 RID: 10407
		[Description("UNINITIALIZED")]
		Uninitialized = -1,
		// Token: 0x040028A8 RID: 10408
		[Description("PRE_LOADED")]
		PreLoaded = -2,
		// Token: 0x040028A9 RID: 10409
		[Description("FILE_INVALID")]
		FileInvalid = -3,
		// Token: 0x040028AA RID: 10410
		[Description("SIGNATURE_INVALID")]
		SignatureInvalid = -4,
		// Token: 0x040028AB RID: 10411
		[Description("UNABLE_TO_VERIFY")]
		UnableToVerify = -5,
		// Token: 0x040028AC RID: 10412
		[Description("VERSION_MISMATCH")]
		VersionMismatch = -6,
		// Token: 0x040028AD RID: 10413
		[Description("UNKNOWN")]
		Unknown = -7,
		// Token: 0x040028AE RID: 10414
		[Description("INVALID_CREDENTIALS")]
		InvalidCredentials = -8,
		// Token: 0x040028AF RID: 10415
		[Description("NOT_ENTITLED")]
		NotEntitled = -9
	}
}
