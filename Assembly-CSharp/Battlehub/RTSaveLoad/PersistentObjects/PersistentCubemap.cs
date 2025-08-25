using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200015D RID: 349
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCubemap : PersistentTexture
	{
		// Token: 0x060007C7 RID: 1991 RVA: 0x000342C1 File Offset: 0x000326C1
		public PersistentCubemap()
		{
		}
	}
}
