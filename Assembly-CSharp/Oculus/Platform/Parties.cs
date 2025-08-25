using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000892 RID: 2194
	public static class Parties
	{
		// Token: 0x060037A4 RID: 14244 RVA: 0x0010E58A File Offset: 0x0010C98A
		public static Request<Party> GetCurrent()
		{
			if (Core.IsInitialized())
			{
				return new Request<Party>(CAPI.ovr_Party_GetCurrent());
			}
			return null;
		}
	}
}
