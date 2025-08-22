using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200019F RID: 415
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPointEffector2D : PersistentEffector2D
	{
		// Token: 0x060008C8 RID: 2248 RVA: 0x000379F9 File Offset: 0x00035DF9
		public PersistentPointEffector2D()
		{
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00037A04 File Offset: 0x00035E04
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			PointEffector2D pointEffector2D = (PointEffector2D)obj;
			pointEffector2D.forceMagnitude = this.forceMagnitude;
			pointEffector2D.forceVariation = this.forceVariation;
			pointEffector2D.distanceScale = this.distanceScale;
			pointEffector2D.drag = this.drag;
			pointEffector2D.angularDrag = this.angularDrag;
			pointEffector2D.forceSource = (EffectorSelection2D)this.forceSource;
			pointEffector2D.forceTarget = (EffectorSelection2D)this.forceTarget;
			pointEffector2D.forceMode = (EffectorForceMode2D)this.forceMode;
			return pointEffector2D;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00037A8C File Offset: 0x00035E8C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			PointEffector2D pointEffector2D = (PointEffector2D)obj;
			this.forceMagnitude = pointEffector2D.forceMagnitude;
			this.forceVariation = pointEffector2D.forceVariation;
			this.distanceScale = pointEffector2D.distanceScale;
			this.drag = pointEffector2D.drag;
			this.angularDrag = pointEffector2D.angularDrag;
			this.forceSource = (uint)pointEffector2D.forceSource;
			this.forceTarget = (uint)pointEffector2D.forceTarget;
			this.forceMode = (uint)pointEffector2D.forceMode;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00037B0E File Offset: 0x00035F0E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000995 RID: 2453
		public float forceMagnitude;

		// Token: 0x04000996 RID: 2454
		public float forceVariation;

		// Token: 0x04000997 RID: 2455
		public float distanceScale;

		// Token: 0x04000998 RID: 2456
		public float drag;

		// Token: 0x04000999 RID: 2457
		public float angularDrag;

		// Token: 0x0400099A RID: 2458
		public uint forceSource;

		// Token: 0x0400099B RID: 2459
		public uint forceTarget;

		// Token: 0x0400099C RID: 2460
		public uint forceMode;
	}
}
