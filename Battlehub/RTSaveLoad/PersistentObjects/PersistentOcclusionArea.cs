using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000193 RID: 403
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentOcclusionArea : PersistentComponent
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x000371AC File Offset: 0x000355AC
		public PersistentOcclusionArea()
		{
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000371B4 File Offset: 0x000355B4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			OcclusionArea occlusionArea = (OcclusionArea)obj;
			occlusionArea.center = this.center;
			occlusionArea.size = this.size;
			return occlusionArea;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000371F4 File Offset: 0x000355F4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			OcclusionArea occlusionArea = (OcclusionArea)obj;
			this.center = occlusionArea.center;
			this.size = occlusionArea.size;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0003722E File Offset: 0x0003562E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400096F RID: 2415
		public Vector3 center;

		// Token: 0x04000970 RID: 2416
		public Vector3 size;
	}
}
