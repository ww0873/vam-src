using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000178 RID: 376
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentHorizontalLayoutGroup : PersistentHorizontalOrVerticalLayoutGroup
	{
		// Token: 0x0600083B RID: 2107 RVA: 0x00035D3D File Offset: 0x0003413D
		public PersistentHorizontalLayoutGroup()
		{
		}
	}
}
