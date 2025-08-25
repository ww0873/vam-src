using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200010D RID: 269
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentBurst : PersistentData
	{
		// Token: 0x0600068A RID: 1674 RVA: 0x0002D126 File Offset: 0x0002B526
		public PersistentBurst()
		{
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0002D130 File Offset: 0x0002B530
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.Burst burst = (ParticleSystem.Burst)obj;
			burst.time = this.time;
			burst.minCount = this.minCount;
			burst.maxCount = this.maxCount;
			burst.cycleCount = this.cycleCount;
			burst.repeatInterval = this.repeatInterval;
			return burst;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0002D1A0 File Offset: 0x0002B5A0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.Burst burst = (ParticleSystem.Burst)obj;
			this.time = burst.time;
			this.minCount = burst.minCount;
			this.maxCount = burst.maxCount;
			this.cycleCount = burst.cycleCount;
			this.repeatInterval = burst.repeatInterval;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002D203 File Offset: 0x0002B603
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000656 RID: 1622
		public float time;

		// Token: 0x04000657 RID: 1623
		public short minCount;

		// Token: 0x04000658 RID: 1624
		public short maxCount;

		// Token: 0x04000659 RID: 1625
		public int cycleCount;

		// Token: 0x0400065A RID: 1626
		public float repeatInterval;
	}
}
