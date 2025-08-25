using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.Video
{
	// Token: 0x020001D4 RID: 468
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentVideoClip : PersistentObject
	{
		// Token: 0x06000978 RID: 2424 RVA: 0x0003A85C File Offset: 0x00038C5C
		public PersistentVideoClip()
		{
		}
	}
}
