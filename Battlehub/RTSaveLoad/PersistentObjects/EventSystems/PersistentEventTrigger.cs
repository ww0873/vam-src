using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x02000165 RID: 357
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentEventTrigger : PersistentMonoBehaviour
	{
		// Token: 0x060007FF RID: 2047 RVA: 0x00034704 File Offset: 0x00032B04
		public PersistentEventTrigger()
		{
		}
	}
}
