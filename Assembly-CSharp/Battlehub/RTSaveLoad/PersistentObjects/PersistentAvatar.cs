using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200013E RID: 318
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAvatar : PersistentObject
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x0003234D File Offset: 0x0003074D
		public PersistentAvatar()
		{
		}
	}
}
