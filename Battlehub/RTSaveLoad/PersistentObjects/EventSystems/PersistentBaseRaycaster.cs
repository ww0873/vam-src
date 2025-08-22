using System;
using Battlehub.RTSaveLoad.PersistentObjects.UI;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x02000143 RID: 323
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1126, typeof(PersistentPhysicsRaycaster))]
	[ProtoInclude(1127, typeof(PersistentGraphicRaycaster))]
	[Serializable]
	public class PersistentBaseRaycaster : PersistentUIBehaviour
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x0003246D File Offset: 0x0003086D
		public PersistentBaseRaycaster()
		{
		}
	}
}
