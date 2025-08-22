using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200012C RID: 300
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAreaEffector2D : PersistentEffector2D
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x000314E1 File Offset: 0x0002F8E1
		public PersistentAreaEffector2D()
		{
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000314EC File Offset: 0x0002F8EC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AreaEffector2D areaEffector2D = (AreaEffector2D)obj;
			areaEffector2D.forceAngle = this.forceAngle;
			areaEffector2D.useGlobalAngle = this.useGlobalAngle;
			areaEffector2D.forceMagnitude = this.forceMagnitude;
			areaEffector2D.forceVariation = this.forceVariation;
			areaEffector2D.drag = this.drag;
			areaEffector2D.angularDrag = this.angularDrag;
			areaEffector2D.forceTarget = (EffectorSelection2D)this.forceTarget;
			return areaEffector2D;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00031568 File Offset: 0x0002F968
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AreaEffector2D areaEffector2D = (AreaEffector2D)obj;
			this.forceAngle = areaEffector2D.forceAngle;
			this.useGlobalAngle = areaEffector2D.useGlobalAngle;
			this.forceMagnitude = areaEffector2D.forceMagnitude;
			this.forceVariation = areaEffector2D.forceVariation;
			this.drag = areaEffector2D.drag;
			this.angularDrag = areaEffector2D.angularDrag;
			this.forceTarget = (uint)areaEffector2D.forceTarget;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x000315DE File Offset: 0x0002F9DE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400077F RID: 1919
		public float forceAngle;

		// Token: 0x04000780 RID: 1920
		public bool useGlobalAngle;

		// Token: 0x04000781 RID: 1921
		public float forceMagnitude;

		// Token: 0x04000782 RID: 1922
		public float forceVariation;

		// Token: 0x04000783 RID: 1923
		public float drag;

		// Token: 0x04000784 RID: 1924
		public float angularDrag;

		// Token: 0x04000785 RID: 1925
		public uint forceTarget;
	}
}
