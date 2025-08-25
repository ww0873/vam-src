using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000146 RID: 326
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentBillboardRenderer : PersistentRenderer
	{
		// Token: 0x06000769 RID: 1897 RVA: 0x0003285B File Offset: 0x00030C5B
		public PersistentBillboardRenderer()
		{
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00032864 File Offset: 0x00030C64
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			BillboardRenderer billboardRenderer = (BillboardRenderer)obj;
			billboardRenderer.billboard = (BillboardAsset)objects.Get(this.billboard);
			return billboardRenderer;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000328A4 File Offset: 0x00030CA4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			BillboardRenderer billboardRenderer = (BillboardRenderer)obj;
			this.billboard = billboardRenderer.billboard.GetMappedInstanceID();
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000328D7 File Offset: 0x00030CD7
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.billboard, dependencies, objects, allowNulls);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000328F4 File Offset: 0x00030CF4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			BillboardRenderer billboardRenderer = (BillboardRenderer)obj;
			base.AddDependency(billboardRenderer.billboard, dependencies);
		}

		// Token: 0x040007DD RID: 2013
		public long billboard;
	}
}
