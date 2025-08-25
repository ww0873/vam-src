using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000196 RID: 406
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentOutline : PersistentShadow
	{
		// Token: 0x060008AF RID: 2223 RVA: 0x000374E1 File Offset: 0x000358E1
		public PersistentOutline()
		{
		}
	}
}
