using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x020001D3 RID: 467
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentVerticalLayoutGroup : PersistentHorizontalOrVerticalLayoutGroup
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x0003A854 File Offset: 0x00038C54
		public PersistentVerticalLayoutGroup()
		{
		}
	}
}
