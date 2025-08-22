using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000249 RID: 585
	public interface ISerializer
	{
		// Token: 0x06000C2E RID: 3118
		byte[] Serialize<TData>(TData data);

		// Token: 0x06000C2F RID: 3119
		TData Deserialize<TData>(byte[] data);

		// Token: 0x06000C30 RID: 3120
		TData DeepClone<TData>(TData data);
	}
}
