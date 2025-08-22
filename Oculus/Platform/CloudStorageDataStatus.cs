using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020007ED RID: 2029
	public enum CloudStorageDataStatus
	{
		// Token: 0x04002727 RID: 10023
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x04002728 RID: 10024
		[Description("IN_SYNC")]
		InSync,
		// Token: 0x04002729 RID: 10025
		[Description("NEEDS_DOWNLOAD")]
		NeedsDownload,
		// Token: 0x0400272A RID: 10026
		[Description("REMOTE_DOWNLOADING")]
		RemoteDownloading,
		// Token: 0x0400272B RID: 10027
		[Description("NEEDS_UPLOAD")]
		NeedsUpload,
		// Token: 0x0400272C RID: 10028
		[Description("LOCAL_UPLOADING")]
		LocalUploading,
		// Token: 0x0400272D RID: 10029
		[Description("IN_CONFLICT")]
		InConflict
	}
}
