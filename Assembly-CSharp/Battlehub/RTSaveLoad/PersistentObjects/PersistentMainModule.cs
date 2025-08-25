using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000119 RID: 281
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMainModule : PersistentData
	{
		// Token: 0x060006C0 RID: 1728 RVA: 0x0002E153 File Offset: 0x0002C553
		public PersistentMainModule()
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002E15C File Offset: 0x0002C55C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.MainModule mainModule = (ParticleSystem.MainModule)obj;
			mainModule.duration = this.duration;
			mainModule.loop = this.loop;
			mainModule.prewarm = this.prewarm;
			mainModule.startDelay = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startDelay, this.startDelay, objects);
			mainModule.startDelayMultiplier = this.startDelayMultiplier;
			mainModule.startLifetime = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startLifetime, this.startLifetime, objects);
			mainModule.startLifetimeMultiplier = this.startLifetimeMultiplier;
			mainModule.startSpeed = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startSpeed, this.startSpeed, objects);
			mainModule.startSpeedMultiplier = this.startSpeedMultiplier;
			mainModule.startSize3D = this.startSize3D;
			mainModule.startSize = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startSize, this.startSize, objects);
			mainModule.startSizeMultiplier = this.startSizeMultiplier;
			mainModule.startSizeX = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startSizeX, this.startSizeX, objects);
			mainModule.startSizeXMultiplier = this.startSizeXMultiplier;
			mainModule.startSizeY = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startSizeY, this.startSizeY, objects);
			mainModule.startSizeYMultiplier = this.startSizeYMultiplier;
			mainModule.startSizeZ = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startSizeZ, this.startSizeZ, objects);
			mainModule.startSizeZMultiplier = this.startSizeZMultiplier;
			mainModule.startRotation3D = this.startRotation3D;
			mainModule.startRotation = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startRotation, this.startRotation, objects);
			mainModule.startRotationMultiplier = this.startRotationMultiplier;
			mainModule.startRotationX = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startRotationX, this.startRotationX, objects);
			mainModule.startRotationXMultiplier = this.startRotationXMultiplier;
			mainModule.startRotationY = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startRotationY, this.startRotationY, objects);
			mainModule.startRotationYMultiplier = this.startRotationYMultiplier;
			mainModule.startRotationZ = base.Write<ParticleSystem.MinMaxCurve>(mainModule.startRotationZ, this.startRotationZ, objects);
			mainModule.startRotationZMultiplier = this.startRotationZMultiplier;
			mainModule.startColor = base.Write<ParticleSystem.MinMaxGradient>(mainModule.startColor, this.startColor, objects);
			mainModule.gravityModifier = base.Write<ParticleSystem.MinMaxCurve>(mainModule.gravityModifier, this.gravityModifier, objects);
			mainModule.gravityModifierMultiplier = this.gravityModifierMultiplier;
			mainModule.simulationSpace = (ParticleSystemSimulationSpace)this.simulationSpace;
			mainModule.customSimulationSpace = (Transform)objects.Get(this.customSimulationSpace);
			mainModule.simulationSpeed = this.simulationSpeed;
			mainModule.scalingMode = (ParticleSystemScalingMode)this.scalingMode;
			mainModule.playOnAwake = this.playOnAwake;
			mainModule.maxParticles = this.maxParticles;
			return mainModule;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002E420 File Offset: 0x0002C820
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.MainModule mainModule = (ParticleSystem.MainModule)obj;
			this.duration = mainModule.duration;
			this.loop = mainModule.loop;
			this.prewarm = mainModule.prewarm;
			this.startDelay = base.Read<PersistentMinMaxCurve>(this.startDelay, mainModule.startDelay);
			this.startDelayMultiplier = mainModule.startDelayMultiplier;
			this.startLifetime = base.Read<PersistentMinMaxCurve>(this.startLifetime, mainModule.startLifetime);
			this.startLifetimeMultiplier = mainModule.startLifetimeMultiplier;
			this.startSpeed = base.Read<PersistentMinMaxCurve>(this.startSpeed, mainModule.startSpeed);
			this.startSpeedMultiplier = mainModule.startSpeedMultiplier;
			this.startSize3D = mainModule.startSize3D;
			this.startSize = base.Read<PersistentMinMaxCurve>(this.startSize, mainModule.startSize);
			this.startSizeMultiplier = mainModule.startSizeMultiplier;
			this.startSizeX = base.Read<PersistentMinMaxCurve>(this.startSizeX, mainModule.startSizeX);
			this.startSizeXMultiplier = mainModule.startSizeXMultiplier;
			this.startSizeY = base.Read<PersistentMinMaxCurve>(this.startSizeY, mainModule.startSizeY);
			this.startSizeYMultiplier = mainModule.startSizeYMultiplier;
			this.startSizeZ = base.Read<PersistentMinMaxCurve>(this.startSizeZ, mainModule.startSizeZ);
			this.startSizeZMultiplier = mainModule.startSizeZMultiplier;
			this.startRotation3D = mainModule.startRotation3D;
			this.startRotation = base.Read<PersistentMinMaxCurve>(this.startRotation, mainModule.startRotation);
			this.startRotationMultiplier = mainModule.startRotationMultiplier;
			this.startRotationX = base.Read<PersistentMinMaxCurve>(this.startRotationX, mainModule.startRotationX);
			this.startRotationXMultiplier = mainModule.startRotationXMultiplier;
			this.startRotationY = base.Read<PersistentMinMaxCurve>(this.startRotationY, mainModule.startRotationY);
			this.startRotationYMultiplier = mainModule.startRotationYMultiplier;
			this.startRotationZ = base.Read<PersistentMinMaxCurve>(this.startRotationZ, mainModule.startRotationZ);
			this.startRotationZMultiplier = mainModule.startRotationZMultiplier;
			this.startColor = base.Read<PersistentMinMaxGradient>(this.startColor, mainModule.startColor);
			this.gravityModifier = base.Read<PersistentMinMaxCurve>(this.gravityModifier, mainModule.gravityModifier);
			this.gravityModifierMultiplier = mainModule.gravityModifierMultiplier;
			this.simulationSpace = (uint)mainModule.simulationSpace;
			this.customSimulationSpace = mainModule.customSimulationSpace.GetMappedInstanceID();
			this.simulationSpeed = mainModule.simulationSpeed;
			this.scalingMode = (uint)mainModule.scalingMode;
			this.playOnAwake = mainModule.playOnAwake;
			this.maxParticles = mainModule.maxParticles;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002E6F8 File Offset: 0x0002CAF8
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startDelay, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startSpeed, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startSize, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startSizeX, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startSizeY, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startSizeZ, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startRotation, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startRotationX, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startRotationY, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startRotationZ, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxGradient>(this.startColor, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.gravityModifier, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.customSimulationSpace, dependencies, objects, allowNulls);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002E7E0 File Offset: 0x0002CBE0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.MainModule mainModule = (ParticleSystem.MainModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.startDelay, mainModule.startDelay, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startLifetime, mainModule.startLifetime, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startSpeed, mainModule.startSpeed, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startSize, mainModule.startSize, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startSizeX, mainModule.startSizeX, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startSizeY, mainModule.startSizeY, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startSizeZ, mainModule.startSizeZ, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startRotation, mainModule.startRotation, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startRotationX, mainModule.startRotationX, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startRotationY, mainModule.startRotationY, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startRotationZ, mainModule.startRotationZ, dependencies);
			base.GetDependencies<PersistentMinMaxGradient>(this.startColor, mainModule.startColor, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.gravityModifier, mainModule.gravityModifier, dependencies);
			base.AddDependency(mainModule.customSimulationSpace, dependencies);
		}

		// Token: 0x0400069D RID: 1693
		public float duration;

		// Token: 0x0400069E RID: 1694
		public bool loop;

		// Token: 0x0400069F RID: 1695
		public bool prewarm;

		// Token: 0x040006A0 RID: 1696
		public PersistentMinMaxCurve startDelay;

		// Token: 0x040006A1 RID: 1697
		public float startDelayMultiplier;

		// Token: 0x040006A2 RID: 1698
		public PersistentMinMaxCurve startLifetime;

		// Token: 0x040006A3 RID: 1699
		public float startLifetimeMultiplier;

		// Token: 0x040006A4 RID: 1700
		public PersistentMinMaxCurve startSpeed;

		// Token: 0x040006A5 RID: 1701
		public float startSpeedMultiplier;

		// Token: 0x040006A6 RID: 1702
		public bool startSize3D;

		// Token: 0x040006A7 RID: 1703
		public PersistentMinMaxCurve startSize;

		// Token: 0x040006A8 RID: 1704
		public float startSizeMultiplier;

		// Token: 0x040006A9 RID: 1705
		public PersistentMinMaxCurve startSizeX;

		// Token: 0x040006AA RID: 1706
		public float startSizeXMultiplier;

		// Token: 0x040006AB RID: 1707
		public PersistentMinMaxCurve startSizeY;

		// Token: 0x040006AC RID: 1708
		public float startSizeYMultiplier;

		// Token: 0x040006AD RID: 1709
		public PersistentMinMaxCurve startSizeZ;

		// Token: 0x040006AE RID: 1710
		public float startSizeZMultiplier;

		// Token: 0x040006AF RID: 1711
		public bool startRotation3D;

		// Token: 0x040006B0 RID: 1712
		public PersistentMinMaxCurve startRotation;

		// Token: 0x040006B1 RID: 1713
		public float startRotationMultiplier;

		// Token: 0x040006B2 RID: 1714
		public PersistentMinMaxCurve startRotationX;

		// Token: 0x040006B3 RID: 1715
		public float startRotationXMultiplier;

		// Token: 0x040006B4 RID: 1716
		public PersistentMinMaxCurve startRotationY;

		// Token: 0x040006B5 RID: 1717
		public float startRotationYMultiplier;

		// Token: 0x040006B6 RID: 1718
		public PersistentMinMaxCurve startRotationZ;

		// Token: 0x040006B7 RID: 1719
		public float startRotationZMultiplier;

		// Token: 0x040006B8 RID: 1720
		public float randomizeRotationDirection;

		// Token: 0x040006B9 RID: 1721
		public PersistentMinMaxGradient startColor;

		// Token: 0x040006BA RID: 1722
		public PersistentMinMaxCurve gravityModifier;

		// Token: 0x040006BB RID: 1723
		public float gravityModifierMultiplier;

		// Token: 0x040006BC RID: 1724
		public uint simulationSpace;

		// Token: 0x040006BD RID: 1725
		public long customSimulationSpace;

		// Token: 0x040006BE RID: 1726
		public float simulationSpeed;

		// Token: 0x040006BF RID: 1727
		public uint scalingMode;

		// Token: 0x040006C0 RID: 1728
		public bool playOnAwake;

		// Token: 0x040006C1 RID: 1729
		public int maxParticles;
	}
}
