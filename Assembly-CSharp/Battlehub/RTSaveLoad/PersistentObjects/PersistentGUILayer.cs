using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000172 RID: 370
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGUILayer : PersistentBehaviour
	{
		// Token: 0x06000823 RID: 2083 RVA: 0x00034D21 File Offset: 0x00033121
		public PersistentGUILayer()
		{
		}
	}
}
