using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x020001A0 RID: 416
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1125, typeof(PersistentStandaloneInputModule))]
	[Serializable]
	public class PersistentPointerInputModule : PersistentBaseInputModule
	{
		// Token: 0x060008CC RID: 2252 RVA: 0x00037B19 File Offset: 0x00035F19
		public PersistentPointerInputModule()
		{
		}
	}
}
