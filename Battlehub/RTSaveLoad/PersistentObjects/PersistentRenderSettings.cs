using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001AD RID: 429
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRenderSettings : PersistentObject
	{
		// Token: 0x060008F2 RID: 2290 RVA: 0x000383AD File Offset: 0x000367AD
		public PersistentRenderSettings()
		{
		}
	}
}
