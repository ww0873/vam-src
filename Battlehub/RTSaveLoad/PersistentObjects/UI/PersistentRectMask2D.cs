using System;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x020001A7 RID: 423
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRectMask2D : PersistentUIBehaviour
	{
		// Token: 0x060008DE RID: 2270 RVA: 0x00037E34 File Offset: 0x00036234
		public PersistentRectMask2D()
		{
		}
	}
}
