using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200018C RID: 396
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1107, typeof(PersistentAnimationClip))]
	[Serializable]
	public class PersistentMotion : PersistentObject
	{
		// Token: 0x0600088B RID: 2187 RVA: 0x0003105C File Offset: 0x0002F45C
		public PersistentMotion()
		{
		}
	}
}
