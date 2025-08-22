using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B1 RID: 433
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1106, typeof(PersistentAnimatorOverrideController))]
	[Serializable]
	public class PersistentRuntimeAnimatorController : PersistentObject
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x00031381 File Offset: 0x0002F781
		public PersistentRuntimeAnimatorController()
		{
		}
	}
}
