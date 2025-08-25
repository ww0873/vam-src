using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200019D RID: 413
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPlatformEffector2D : PersistentEffector2D
	{
		// Token: 0x060008C3 RID: 2243 RVA: 0x000378E9 File Offset: 0x00035CE9
		public PersistentPlatformEffector2D()
		{
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000378F4 File Offset: 0x00035CF4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			PlatformEffector2D platformEffector2D = (PlatformEffector2D)obj;
			platformEffector2D.useOneWay = this.useOneWay;
			platformEffector2D.useOneWayGrouping = this.useOneWayGrouping;
			platformEffector2D.useSideFriction = this.useSideFriction;
			platformEffector2D.useSideBounce = this.useSideBounce;
			platformEffector2D.surfaceArc = this.surfaceArc;
			platformEffector2D.sideArc = this.sideArc;
			platformEffector2D.rotationalOffset = this.rotationalOffset;
			return platformEffector2D;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00037970 File Offset: 0x00035D70
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			PlatformEffector2D platformEffector2D = (PlatformEffector2D)obj;
			this.useOneWay = platformEffector2D.useOneWay;
			this.useOneWayGrouping = platformEffector2D.useOneWayGrouping;
			this.useSideFriction = platformEffector2D.useSideFriction;
			this.useSideBounce = platformEffector2D.useSideBounce;
			this.surfaceArc = platformEffector2D.surfaceArc;
			this.sideArc = platformEffector2D.sideArc;
			this.rotationalOffset = platformEffector2D.rotationalOffset;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000379E6 File Offset: 0x00035DE6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400098E RID: 2446
		public bool useOneWay;

		// Token: 0x0400098F RID: 2447
		public bool useOneWayGrouping;

		// Token: 0x04000990 RID: 2448
		public bool useSideFriction;

		// Token: 0x04000991 RID: 2449
		public bool useSideBounce;

		// Token: 0x04000992 RID: 2450
		public float surfaceArc;

		// Token: 0x04000993 RID: 2451
		public float sideArc;

		// Token: 0x04000994 RID: 2452
		public float rotationalOffset;
	}
}
