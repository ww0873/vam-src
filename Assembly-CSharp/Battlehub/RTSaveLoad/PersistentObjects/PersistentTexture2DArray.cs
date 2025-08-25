using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001CB RID: 459
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTexture2DArray : PersistentTexture
	{
		// Token: 0x0600095C RID: 2396 RVA: 0x0003A08C File Offset: 0x0003848C
		public PersistentTexture2DArray()
		{
		}
	}
}
