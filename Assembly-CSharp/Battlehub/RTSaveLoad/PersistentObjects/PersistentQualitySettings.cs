using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001A5 RID: 421
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentQualitySettings : PersistentObject
	{
		// Token: 0x060008D8 RID: 2264 RVA: 0x00037D4C File Offset: 0x0003614C
		public PersistentQualitySettings()
		{
		}
	}
}
