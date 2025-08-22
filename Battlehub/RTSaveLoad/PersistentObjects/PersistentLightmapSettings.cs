using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000181 RID: 385
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLightmapSettings : PersistentObject
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x000364C5 File Offset: 0x000348C5
		public PersistentLightmapSettings()
		{
		}
	}
}
