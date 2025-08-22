using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200019C RID: 412
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1100, typeof(PersistentConstantForce2D))]
	[Serializable]
	public class PersistentPhysicsUpdateBehaviour2D : PersistentBehaviour
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x000340C1 File Offset: 0x000324C1
		public PersistentPhysicsUpdateBehaviour2D()
		{
		}
	}
}
