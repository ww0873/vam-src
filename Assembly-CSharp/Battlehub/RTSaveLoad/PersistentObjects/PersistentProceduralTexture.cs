using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001A3 RID: 419
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentProceduralTexture : PersistentTexture
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x00037BD1 File Offset: 0x00035FD1
		public PersistentProceduralTexture()
		{
		}
	}
}
