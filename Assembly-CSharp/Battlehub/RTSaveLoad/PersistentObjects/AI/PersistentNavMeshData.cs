using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.AI
{
	// Token: 0x0200018F RID: 399
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentNavMeshData : PersistentObject
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x00036F9D File Offset: 0x0003539D
		public PersistentNavMeshData()
		{
		}
	}
}
