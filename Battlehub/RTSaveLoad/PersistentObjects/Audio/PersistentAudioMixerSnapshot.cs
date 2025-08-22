using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.Audio
{
	// Token: 0x0200013A RID: 314
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioMixerSnapshot : PersistentObject
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x00031C50 File Offset: 0x00030050
		public PersistentAudioMixerSnapshot()
		{
		}
	}
}
