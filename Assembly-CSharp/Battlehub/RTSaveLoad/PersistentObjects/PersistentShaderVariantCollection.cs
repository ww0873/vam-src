using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B4 RID: 436
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentShaderVariantCollection : PersistentObject
	{
		// Token: 0x06000906 RID: 2310 RVA: 0x00038A35 File Offset: 0x00036E35
		public PersistentShaderVariantCollection()
		{
		}
	}
}
