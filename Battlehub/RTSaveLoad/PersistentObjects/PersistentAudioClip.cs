using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000132 RID: 306
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioClip : PersistentObject
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x00031819 File Offset: 0x0002FC19
		public PersistentAudioClip()
		{
		}
	}
}
