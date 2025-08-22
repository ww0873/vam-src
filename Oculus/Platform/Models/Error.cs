using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200084A RID: 2122
	public class Error
	{
		// Token: 0x060036ED RID: 14061 RVA: 0x0010C4CA File Offset: 0x0010A8CA
		public Error(int code, string message, int httpCode)
		{
			this.Message = message;
			this.Code = code;
			this.HttpCode = httpCode;
		}

		// Token: 0x04002811 RID: 10257
		public readonly int Code;

		// Token: 0x04002812 RID: 10258
		public readonly int HttpCode;

		// Token: 0x04002813 RID: 10259
		public readonly string Message;
	}
}
