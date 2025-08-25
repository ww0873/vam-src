using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001BA RID: 442
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSparseTexture : PersistentTexture
	{
		// Token: 0x0600091D RID: 2333 RVA: 0x00038E3D File Offset: 0x0003723D
		public PersistentSparseTexture()
		{
		}
	}
}
