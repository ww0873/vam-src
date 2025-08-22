using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.Rendering
{
	// Token: 0x0200016F RID: 367
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGraphicsSettings : PersistentObject
	{
		// Token: 0x0600081D RID: 2077 RVA: 0x00034B91 File Offset: 0x00032F91
		public PersistentGraphicsSettings()
		{
		}
	}
}
