using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000130 RID: 304
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioBehaviour : PersistentBehaviour
	{
		// Token: 0x06000725 RID: 1829 RVA: 0x00031709 File Offset: 0x0002FB09
		public PersistentAudioBehaviour()
		{
		}
	}
}
