using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000258 RID: 600
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class AssetBundleDescriptor
	{
		// Token: 0x06000C8E RID: 3214 RVA: 0x0004CDF6 File Offset: 0x0004B1F6
		public AssetBundleDescriptor()
		{
		}

		// Token: 0x04000D08 RID: 3336
		public string BundleName;

		// Token: 0x04000D09 RID: 3337
		public string AssetName;

		// Token: 0x04000D0A RID: 3338
		public string TypeName;
	}
}
