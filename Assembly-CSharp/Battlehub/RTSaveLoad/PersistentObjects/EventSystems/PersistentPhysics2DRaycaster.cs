using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x02000199 RID: 409
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPhysics2DRaycaster : PersistentPhysicsRaycaster
	{
		// Token: 0x060008B9 RID: 2233 RVA: 0x00037851 File Offset: 0x00035C51
		public PersistentPhysics2DRaycaster()
		{
		}
	}
}
