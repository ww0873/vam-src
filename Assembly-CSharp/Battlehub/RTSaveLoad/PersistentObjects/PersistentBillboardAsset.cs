using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000145 RID: 325
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentBillboardAsset : PersistentObject
	{
		// Token: 0x06000764 RID: 1892 RVA: 0x00032475 File Offset: 0x00030875
		public PersistentBillboardAsset()
		{
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00032480 File Offset: 0x00030880
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			BillboardAsset billboardAsset = (BillboardAsset)obj;
			billboardAsset.width = this.width;
			billboardAsset.height = this.height;
			billboardAsset.bottom = this.bottom;
			billboardAsset.material = (Material)objects.Get(this.material);
			return billboardAsset;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000324E4 File Offset: 0x000308E4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			BillboardAsset billboardAsset = (BillboardAsset)obj;
			this.width = billboardAsset.width;
			this.height = billboardAsset.height;
			this.bottom = billboardAsset.bottom;
			this.material = billboardAsset.material.GetMappedInstanceID();
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0003253B File Offset: 0x0003093B
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.material, dependencies, objects, allowNulls);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00032558 File Offset: 0x00030958
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			BillboardAsset billboardAsset = (BillboardAsset)obj;
			base.AddDependency(billboardAsset.material, dependencies);
		}

		// Token: 0x040007D9 RID: 2009
		public float width;

		// Token: 0x040007DA RID: 2010
		public float height;

		// Token: 0x040007DB RID: 2011
		public float bottom;

		// Token: 0x040007DC RID: 2012
		public long material;
	}
}
