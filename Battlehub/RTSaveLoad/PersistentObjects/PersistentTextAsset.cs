using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C9 RID: 457
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTextAsset : PersistentObject
	{
		// Token: 0x06000956 RID: 2390 RVA: 0x00039EB4 File Offset: 0x000382B4
		public PersistentTextAsset()
		{
		}
	}
}
