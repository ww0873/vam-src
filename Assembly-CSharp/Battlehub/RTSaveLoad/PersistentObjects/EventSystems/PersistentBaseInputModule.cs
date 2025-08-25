using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x02000141 RID: 321
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1124, typeof(PersistentPointerInputModule))]
	[Serializable]
	public class PersistentBaseInputModule : PersistentUIBehaviour
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x0003245D File Offset: 0x0003085D
		public PersistentBaseInputModule()
		{
		}
	}
}
