using System;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000142 RID: 322
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1134, typeof(PersistentPositionAsUV1))]
	[ProtoInclude(1135, typeof(PersistentShadow))]
	[Serializable]
	public class PersistentBaseMeshEffect : PersistentUIBehaviour
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x00032465 File Offset: 0x00030865
		public PersistentBaseMeshEffect()
		{
		}
	}
}
