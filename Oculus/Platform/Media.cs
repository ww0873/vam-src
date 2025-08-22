using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000890 RID: 2192
	public static class Media
	{
		// Token: 0x060037A0 RID: 14240 RVA: 0x0010E504 File Offset: 0x0010C904
		public static Request<ShareMediaResult> ShareToFacebook(string postTextSuggestion, string filePath, MediaContentType contentType)
		{
			if (Core.IsInitialized())
			{
				return new Request<ShareMediaResult>(CAPI.ovr_Media_ShareToFacebook(postTextSuggestion, filePath, contentType));
			}
			return null;
		}
	}
}
