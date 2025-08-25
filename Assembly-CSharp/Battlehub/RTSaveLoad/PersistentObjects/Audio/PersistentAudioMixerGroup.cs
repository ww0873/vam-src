using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.Audio
{
	// Token: 0x02000139 RID: 313
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioMixerGroup : PersistentObject
	{
		// Token: 0x06000745 RID: 1861 RVA: 0x00031C48 File Offset: 0x00030048
		public PersistentAudioMixerGroup()
		{
		}
	}
}
