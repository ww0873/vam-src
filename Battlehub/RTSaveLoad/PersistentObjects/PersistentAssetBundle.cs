using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200012E RID: 302
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAssetBundle : PersistentObject
	{
		// Token: 0x06000723 RID: 1827 RVA: 0x000316F9 File Offset: 0x0002FAF9
		public PersistentAssetBundle()
		{
		}
	}
}
