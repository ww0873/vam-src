using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000157 RID: 343
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCompositeCollider2D : PersistentCollider2D
	{
		// Token: 0x060007B2 RID: 1970 RVA: 0x00033BF1 File Offset: 0x00031FF1
		public PersistentCompositeCollider2D()
		{
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00033BFC File Offset: 0x00031FFC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CompositeCollider2D compositeCollider2D = (CompositeCollider2D)obj;
			compositeCollider2D.geometryType = (CompositeCollider2D.GeometryType)this.geometryType;
			compositeCollider2D.generationType = (CompositeCollider2D.GenerationType)this.generationType;
			compositeCollider2D.vertexDistance = this.vertexDistance;
			compositeCollider2D.edgeRadius = this.edgeRadius;
			return compositeCollider2D;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00033C54 File Offset: 0x00032054
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CompositeCollider2D compositeCollider2D = (CompositeCollider2D)obj;
			this.geometryType = (uint)compositeCollider2D.geometryType;
			this.generationType = (uint)compositeCollider2D.generationType;
			this.vertexDistance = compositeCollider2D.vertexDistance;
			this.edgeRadius = compositeCollider2D.edgeRadius;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00033CA6 File Offset: 0x000320A6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400084F RID: 2127
		public uint geometryType;

		// Token: 0x04000850 RID: 2128
		public uint generationType;

		// Token: 0x04000851 RID: 2129
		public float vertexDistance;

		// Token: 0x04000852 RID: 2130
		public float edgeRadius;
	}
}
