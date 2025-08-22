using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001BF RID: 447
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSprite : PersistentObject
	{
		// Token: 0x0600092F RID: 2351 RVA: 0x000391F5 File Offset: 0x000375F5
		public PersistentSprite()
		{
		}
	}
}
