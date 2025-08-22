using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000186 RID: 390
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLODGroup : PersistentComponent
	{
		// Token: 0x06000870 RID: 2160 RVA: 0x000368DD File Offset: 0x00034CDD
		public PersistentLODGroup()
		{
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000368E8 File Offset: 0x00034CE8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			LODGroup lodgroup = (LODGroup)obj;
			lodgroup.localReferencePoint = this.localReferencePoint;
			lodgroup.size = this.size;
			lodgroup.fadeMode = (LODFadeMode)this.fadeMode;
			lodgroup.animateCrossFading = this.animateCrossFading;
			lodgroup.enabled = this.enabled;
			return lodgroup;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0003694C File Offset: 0x00034D4C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			LODGroup lodgroup = (LODGroup)obj;
			this.localReferencePoint = lodgroup.localReferencePoint;
			this.size = lodgroup.size;
			this.fadeMode = (uint)lodgroup.fadeMode;
			this.animateCrossFading = lodgroup.animateCrossFading;
			this.enabled = lodgroup.enabled;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000369AA File Offset: 0x00034DAA
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400093E RID: 2366
		public Vector3 localReferencePoint;

		// Token: 0x0400093F RID: 2367
		public float size;

		// Token: 0x04000940 RID: 2368
		public uint fadeMode;

		// Token: 0x04000941 RID: 2369
		public bool animateCrossFading;

		// Token: 0x04000942 RID: 2370
		public bool enabled;
	}
}
