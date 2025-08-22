using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000166 RID: 358
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentFixedJoint : PersistentJoint
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x0003470C File Offset: 0x00032B0C
		public PersistentFixedJoint()
		{
		}
	}
}
