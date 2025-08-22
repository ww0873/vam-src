using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200012F RID: 303
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAssetBundleManifest : PersistentObject
	{
		// Token: 0x06000724 RID: 1828 RVA: 0x00031701 File Offset: 0x0002FB01
		public PersistentAssetBundleManifest()
		{
		}
	}
}
