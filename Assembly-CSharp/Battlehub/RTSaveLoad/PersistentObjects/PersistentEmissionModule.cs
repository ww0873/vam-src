using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000208 RID: 520
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentEmissionModule : PersistentData
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x0004010A File Offset: 0x0003E50A
		public PersistentEmissionModule()
		{
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00040114 File Offset: 0x0003E514
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.EmissionModule emissionModule = (ParticleSystem.EmissionModule)obj;
			emissionModule.enabled = this.enabled;
			emissionModule.rateOverTime = base.Write<ParticleSystem.MinMaxCurve>(emissionModule.rateOverTime, this.rateOverTime, objects);
			emissionModule.rateOverDistance = base.Write<ParticleSystem.MinMaxCurve>(emissionModule.rateOverDistance, this.rateOverDistance, objects);
			if (this.bursts == null)
			{
				emissionModule.SetBursts(new ParticleSystem.Burst[0]);
			}
			else
			{
				ParticleSystem.Burst[] array = new ParticleSystem.Burst[this.bursts.Length];
				for (int i = 0; i < this.bursts.Length; i++)
				{
					array[i] = base.Write<ParticleSystem.Burst>(default(ParticleSystem.Burst), this.bursts[i], objects);
				}
				emissionModule.SetBursts(array);
			}
			emissionModule.rateOverTimeMultiplier = this.rateOverTimeMultiplier;
			emissionModule.rateOverDistanceMultiplier = this.rateOverDistanceMultiplier;
			return emissionModule;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00040210 File Offset: 0x0003E610
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.EmissionModule emissionModule = (ParticleSystem.EmissionModule)obj;
			this.enabled = emissionModule.enabled;
			this.rateOverTime = base.Read<PersistentMinMaxCurve>(this.rateOverTime, emissionModule.rateOverTime);
			this.rateOverDistance = base.Read<PersistentMinMaxCurve>(this.rateOverDistance, emissionModule.rateOverDistance);
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emissionModule.burstCount];
			this.bursts = new PersistentBurst[emissionModule.burstCount];
			emissionModule.GetBursts(array);
			for (int i = 0; i < this.bursts.Length; i++)
			{
				PersistentBurst persistentBurst = new PersistentBurst();
				persistentBurst.ReadFrom(this.bursts[i]);
				this.bursts[i] = persistentBurst;
			}
			this.rateOverTimeMultiplier = emissionModule.rateOverTimeMultiplier;
			this.rateOverDistanceMultiplier = emissionModule.rateOverDistanceMultiplier;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000402F3 File Offset: 0x0003E6F3
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.rateOverTime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.rateOverDistance, dependencies, objects, allowNulls);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0004031C File Offset: 0x0003E71C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.EmissionModule emissionModule = (ParticleSystem.EmissionModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.rateOverTime, emissionModule.rateOverTime, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.rateOverDistance, emissionModule.rateOverDistance, dependencies);
		}

		// Token: 0x04000B86 RID: 2950
		public PersistentBurst[] bursts;

		// Token: 0x04000B87 RID: 2951
		public bool enabled;

		// Token: 0x04000B88 RID: 2952
		public PersistentMinMaxCurve rateOverTime;

		// Token: 0x04000B89 RID: 2953
		public PersistentMinMaxCurve rateOverDistance;

		// Token: 0x04000B8A RID: 2954
		public float rateOverTimeMultiplier;

		// Token: 0x04000B8B RID: 2955
		public float rateOverDistanceMultiplier;
	}
}
