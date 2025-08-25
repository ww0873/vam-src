using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200011D RID: 285
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentParticle : PersistentData
	{
		// Token: 0x060006D4 RID: 1748 RVA: 0x0002F334 File Offset: 0x0002D734
		public PersistentParticle()
		{
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002F33C File Offset: 0x0002D73C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.Particle particle = (ParticleSystem.Particle)obj;
			particle.position = this.position;
			particle.velocity = this.velocity;
			particle.remainingLifetime = this.remainingLifetime;
			particle.startLifetime = this.startLifetime;
			particle.startSize = this.startSize;
			particle.startSize3D = this.startSize3D;
			particle.axisOfRotation = this.axisOfRotation;
			particle.rotation = this.rotation;
			particle.rotation3D = this.rotation3D;
			particle.angularVelocity = this.angularVelocity;
			particle.angularVelocity3D = this.angularVelocity3D;
			particle.startColor = this.startColor;
			particle.randomSeed = this.randomSeed;
			return particle;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002F414 File Offset: 0x0002D814
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.Particle particle = (ParticleSystem.Particle)obj;
			this.position = particle.position;
			this.velocity = particle.velocity;
			this.remainingLifetime = particle.remainingLifetime;
			this.startLifetime = particle.startLifetime;
			this.startSize = particle.startSize;
			this.startSize3D = particle.startSize3D;
			this.axisOfRotation = particle.axisOfRotation;
			this.rotation = particle.rotation;
			this.rotation3D = particle.rotation3D;
			this.angularVelocity = particle.angularVelocity;
			this.angularVelocity3D = particle.angularVelocity3D;
			this.startColor = particle.startColor;
			this.randomSeed = particle.randomSeed;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002F4DF File Offset: 0x0002D8DF
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040006EC RID: 1772
		public Vector3 position;

		// Token: 0x040006ED RID: 1773
		public Vector3 velocity;

		// Token: 0x040006EE RID: 1774
		public float remainingLifetime;

		// Token: 0x040006EF RID: 1775
		public float startLifetime;

		// Token: 0x040006F0 RID: 1776
		public float startSize;

		// Token: 0x040006F1 RID: 1777
		public Vector3 startSize3D;

		// Token: 0x040006F2 RID: 1778
		public Vector3 axisOfRotation;

		// Token: 0x040006F3 RID: 1779
		public float rotation;

		// Token: 0x040006F4 RID: 1780
		public Vector3 rotation3D;

		// Token: 0x040006F5 RID: 1781
		public float angularVelocity;

		// Token: 0x040006F6 RID: 1782
		public Vector3 angularVelocity3D;

		// Token: 0x040006F7 RID: 1783
		public Color32 startColor;

		// Token: 0x040006F8 RID: 1784
		public uint randomSeed;
	}
}
