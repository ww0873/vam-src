using System;

namespace Oculus.Platform
{
	// Token: 0x02000899 RID: 2201
	public sealed class Request<T> : Request
	{
		// Token: 0x060037BD RID: 14269 RVA: 0x0010EA49 File Offset: 0x0010CE49
		public Request(ulong requestID) : base(requestID)
		{
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x0010EA52 File Offset: 0x0010CE52
		public Request<T> OnComplete(Message<T>.Callback callback)
		{
			Callback.OnComplete<T>(this, callback);
			return this;
		}
	}
}
