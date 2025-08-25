using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200015E RID: 350
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCubemapArray : PersistentTexture
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x000342C9 File Offset: 0x000326C9
		public PersistentCubemapArray()
		{
		}
	}
}
