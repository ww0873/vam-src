using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.Networking.PlayerConnection
{
	// Token: 0x0200019E RID: 414
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPlayerConnection : PersistentScriptableObject
	{
		// Token: 0x060008C7 RID: 2247 RVA: 0x000379F1 File Offset: 0x00035DF1
		public PersistentPlayerConnection()
		{
		}
	}
}
