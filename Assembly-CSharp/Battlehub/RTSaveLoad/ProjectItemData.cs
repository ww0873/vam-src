using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.RTSaveLoad.PersistentObjects;
using ProtoBuf;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200023E RID: 574
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class ProjectItemData
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x0004A99C File Offset: 0x00048D9C
		public ProjectItemData()
		{
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0004A9A4 File Offset: 0x00048DA4
		public void Rename(ProjectItemMeta meta, string name)
		{
			if (this.PersistentData != null && this.PersistentData.Length != 0)
			{
				if (this.PersistentData.Length > 1)
				{
					IEnumerable<PersistentData> persistentData = this.PersistentData;
					if (ProjectItemData.<>f__am$cache0 == null)
					{
						ProjectItemData.<>f__am$cache0 = new Func<PersistentData, long>(ProjectItemData.<Rename>m__0);
					}
					Dictionary<long, PersistentData> dictionary = persistentData.ToDictionary(ProjectItemData.<>f__am$cache0);
					PersistentData persistentData2;
					if (dictionary.TryGetValue(meta.Descriptor.InstanceId, out persistentData2))
					{
						PersistentObject asPersistentObject = persistentData2.AsPersistentObject;
						asPersistentObject.name = name;
						if (meta.Descriptor.Components != null)
						{
							for (int i = 0; i < meta.Descriptor.Components.Length; i++)
							{
								if (dictionary.TryGetValue(meta.Descriptor.Components[i].InstanceId, out persistentData2))
								{
									asPersistentObject = persistentData2.AsPersistentObject;
									asPersistentObject.name = name;
								}
							}
						}
					}
				}
				else
				{
					PersistentObject asPersistentObject2 = this.PersistentData[0].AsPersistentObject;
					asPersistentObject2.name = name;
				}
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0004AA9D File Offset: 0x00048E9D
		[CompilerGenerated]
		private static long <Rename>m__0(PersistentData d)
		{
			return d.InstanceId;
		}

		// Token: 0x04000CBB RID: 3259
		public PersistentData[] PersistentData;

		// Token: 0x04000CBC RID: 3260
		public byte[] RawData;

		// Token: 0x04000CBD RID: 3261
		[CompilerGenerated]
		private static Func<PersistentData, long> <>f__am$cache0;
	}
}
