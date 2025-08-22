using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000158 RID: 344
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentComputeShader : PersistentObject
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x00033CB1 File Offset: 0x000320B1
		public PersistentComputeShader()
		{
		}
	}
}
