using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000255 RID: 597
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class ProjectMeta
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x0004CCA8 File Offset: 0x0004B0A8
		public ProjectMeta()
		{
		}

		// Token: 0x04000CFC RID: 3324
		public int Counter;
	}
}
