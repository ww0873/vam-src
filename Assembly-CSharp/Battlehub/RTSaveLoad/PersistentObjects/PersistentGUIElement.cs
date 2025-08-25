using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000171 RID: 369
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGUIElement : PersistentBehaviour
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x00034D19 File Offset: 0x00033119
		public PersistentGUIElement()
		{
		}
	}
}
