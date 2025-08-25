using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C3 RID: 451
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSurfaceEffector2D : PersistentEffector2D
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x0003947D File Offset: 0x0003787D
		public PersistentSurfaceEffector2D()
		{
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00039488 File Offset: 0x00037888
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SurfaceEffector2D surfaceEffector2D = (SurfaceEffector2D)obj;
			surfaceEffector2D.speed = this.speed;
			surfaceEffector2D.speedVariation = this.speedVariation;
			surfaceEffector2D.forceScale = this.forceScale;
			surfaceEffector2D.useContactForce = this.useContactForce;
			surfaceEffector2D.useFriction = this.useFriction;
			surfaceEffector2D.useBounce = this.useBounce;
			return surfaceEffector2D;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x000394F8 File Offset: 0x000378F8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SurfaceEffector2D surfaceEffector2D = (SurfaceEffector2D)obj;
			this.speed = surfaceEffector2D.speed;
			this.speedVariation = surfaceEffector2D.speedVariation;
			this.forceScale = surfaceEffector2D.forceScale;
			this.useContactForce = surfaceEffector2D.useContactForce;
			this.useFriction = surfaceEffector2D.useFriction;
			this.useBounce = surfaceEffector2D.useBounce;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00039562 File Offset: 0x00037962
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A43 RID: 2627
		public float speed;

		// Token: 0x04000A44 RID: 2628
		public float speedVariation;

		// Token: 0x04000A45 RID: 2629
		public float forceScale;

		// Token: 0x04000A46 RID: 2630
		public bool useContactForce;

		// Token: 0x04000A47 RID: 2631
		public bool useFriction;

		// Token: 0x04000A48 RID: 2632
		public bool useBounce;
	}
}
