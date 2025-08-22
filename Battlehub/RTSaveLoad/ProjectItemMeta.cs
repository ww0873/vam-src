using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200023D RID: 573
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class ProjectItemMeta
	{
		// Token: 0x06000BE6 RID: 3046 RVA: 0x0004A960 File Offset: 0x00048D60
		public ProjectItemMeta()
		{
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x0004A968 File Offset: 0x00048D68
		public string TypeName
		{
			get
			{
				if (this.Descriptor == null)
				{
					return null;
				}
				return this.Descriptor.TypeName;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0004A982 File Offset: 0x00048D82
		public string BundleName
		{
			get
			{
				if (this.BundleDescriptor == null)
				{
					return null;
				}
				return this.BundleDescriptor.BundleName;
			}
		}

		// Token: 0x04000CB6 RID: 3254
		public int TypeCode;

		// Token: 0x04000CB7 RID: 3255
		public string Name;

		// Token: 0x04000CB8 RID: 3256
		public bool IsExposedFromEditor;

		// Token: 0x04000CB9 RID: 3257
		public PersistentDescriptor Descriptor;

		// Token: 0x04000CBA RID: 3258
		public AssetBundleDescriptor BundleDescriptor;
	}
}
