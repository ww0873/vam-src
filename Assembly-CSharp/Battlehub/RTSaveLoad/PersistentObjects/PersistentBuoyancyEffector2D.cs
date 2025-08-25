using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000149 RID: 329
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentBuoyancyEffector2D : PersistentEffector2D
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x00032CA9 File Offset: 0x000310A9
		public PersistentBuoyancyEffector2D()
		{
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00032CB4 File Offset: 0x000310B4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			BuoyancyEffector2D buoyancyEffector2D = (BuoyancyEffector2D)obj;
			buoyancyEffector2D.surfaceLevel = this.surfaceLevel;
			buoyancyEffector2D.density = this.density;
			buoyancyEffector2D.linearDrag = this.linearDrag;
			buoyancyEffector2D.angularDrag = this.angularDrag;
			buoyancyEffector2D.flowAngle = this.flowAngle;
			buoyancyEffector2D.flowMagnitude = this.flowMagnitude;
			buoyancyEffector2D.flowVariation = this.flowVariation;
			return buoyancyEffector2D;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00032D30 File Offset: 0x00031130
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			BuoyancyEffector2D buoyancyEffector2D = (BuoyancyEffector2D)obj;
			this.surfaceLevel = buoyancyEffector2D.surfaceLevel;
			this.density = buoyancyEffector2D.density;
			this.linearDrag = buoyancyEffector2D.linearDrag;
			this.angularDrag = buoyancyEffector2D.angularDrag;
			this.flowAngle = buoyancyEffector2D.flowAngle;
			this.flowMagnitude = buoyancyEffector2D.flowMagnitude;
			this.flowVariation = buoyancyEffector2D.flowVariation;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00032DA6 File Offset: 0x000311A6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040007E3 RID: 2019
		public float surfaceLevel;

		// Token: 0x040007E4 RID: 2020
		public float density;

		// Token: 0x040007E5 RID: 2021
		public float linearDrag;

		// Token: 0x040007E6 RID: 2022
		public float angularDrag;

		// Token: 0x040007E7 RID: 2023
		public float flowAngle;

		// Token: 0x040007E8 RID: 2024
		public float flowMagnitude;

		// Token: 0x040007E9 RID: 2025
		public float flowVariation;
	}
}
