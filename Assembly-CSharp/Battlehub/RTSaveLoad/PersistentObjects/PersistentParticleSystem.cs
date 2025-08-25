using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000209 RID: 521
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentParticleSystem : PersistentComponent
	{
		// Token: 0x06000A78 RID: 2680 RVA: 0x00040371 File Offset: 0x0003E771
		public PersistentParticleSystem()
		{
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0004037C File Offset: 0x0003E77C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem particleSystem = (ParticleSystem)obj;
			particleSystem.time = this.time;
			particleSystem.randomSeed = this.randomSeed;
			particleSystem.useAutoRandomSeed = this.useAutoRandomSeed;
			base.Write<ParticleSystem.EmissionModule>(particleSystem.emission, this.emission, objects);
			base.Write<ParticleSystem.CollisionModule>(particleSystem.collision, this.collision, objects);
			base.Write<ParticleSystem.TriggerModule>(particleSystem.trigger, this.trigger, objects);
			base.Write<ParticleSystem.ShapeModule>(particleSystem.shape, this.shape, objects);
			base.Write<ParticleSystem.VelocityOverLifetimeModule>(particleSystem.velocityOverLifetime, this.velocityOverLifetime, objects);
			base.Write<ParticleSystem.LimitVelocityOverLifetimeModule>(particleSystem.limitVelocityOverLifetime, this.limitVelocityOverLifetime, objects);
			base.Write<ParticleSystem.InheritVelocityModule>(particleSystem.inheritVelocity, this.inheritVelocity, objects);
			base.Write<ParticleSystem.ForceOverLifetimeModule>(particleSystem.forceOverLifetime, this.forceOverLifetime, objects);
			base.Write<ParticleSystem.ColorOverLifetimeModule>(particleSystem.colorOverLifetime, this.colorOverLifetime, objects);
			base.Write<ParticleSystem.ColorBySpeedModule>(particleSystem.colorBySpeed, this.colorBySpeed, objects);
			base.Write<ParticleSystem.SizeOverLifetimeModule>(particleSystem.sizeOverLifetime, this.sizeOverLifetime, objects);
			base.Write<ParticleSystem.SizeBySpeedModule>(particleSystem.sizeBySpeed, this.sizeBySpeed, objects);
			base.Write<ParticleSystem.RotationOverLifetimeModule>(particleSystem.rotationOverLifetime, this.rotationOverLifetime, objects);
			base.Write<ParticleSystem.RotationBySpeedModule>(particleSystem.rotationBySpeed, this.rotationBySpeed, objects);
			base.Write<ParticleSystem.ExternalForcesModule>(particleSystem.externalForces, this.externalForces, objects);
			base.Write<ParticleSystem.SubEmittersModule>(particleSystem.subEmitters, this.subEmitters, objects);
			base.Write<ParticleSystem.TextureSheetAnimationModule>(particleSystem.textureSheetAnimation, this.textureSheetAnimation, objects);
			base.Write<ParticleSystem.LightsModule>(particleSystem.lights, this.lights, objects);
			base.Write<ParticleSystem.MainModule>(particleSystem.main, this.main, objects);
			base.Write<ParticleSystem.NoiseModule>(particleSystem.noise, this.noise, objects);
			base.Write<ParticleSystem.TrailModule>(particleSystem.trails, this.trails, objects);
			return particleSystem;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0004056C File Offset: 0x0003E96C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem particleSystem = (ParticleSystem)obj;
			this.time = particleSystem.time;
			this.randomSeed = particleSystem.randomSeed;
			this.useAutoRandomSeed = particleSystem.useAutoRandomSeed;
			this.emission = base.Read<PersistentEmissionModule>(this.emission, particleSystem.emission);
			this.collision = base.Read<PersistentCollisionModule>(this.collision, particleSystem.collision);
			this.trigger = base.Read<PersistentTriggerModule>(this.trigger, particleSystem.trigger);
			this.shape = base.Read<PersistentShapeModule>(this.shape, particleSystem.shape);
			this.velocityOverLifetime = base.Read<PersistentVelocityOverLifetimeModule>(this.velocityOverLifetime, particleSystem.velocityOverLifetime);
			this.limitVelocityOverLifetime = base.Read<PersistentLimitVelocityOverLifetimeModule>(this.limitVelocityOverLifetime, particleSystem.limitVelocityOverLifetime);
			this.inheritVelocity = base.Read<PersistentInheritVelocityModule>(this.inheritVelocity, particleSystem.inheritVelocity);
			this.forceOverLifetime = base.Read<PersistentForceOverLifetimeModule>(this.forceOverLifetime, particleSystem.forceOverLifetime);
			this.colorOverLifetime = base.Read<PersistentColorOverLifetimeModule>(this.colorOverLifetime, particleSystem.colorOverLifetime);
			this.colorBySpeed = base.Read<PersistentColorBySpeedModule>(this.colorBySpeed, particleSystem.colorBySpeed);
			this.sizeOverLifetime = base.Read<PersistentSizeOverLifetimeModule>(this.sizeOverLifetime, particleSystem.sizeOverLifetime);
			this.sizeBySpeed = base.Read<PersistentSizeBySpeedModule>(this.sizeBySpeed, particleSystem.sizeBySpeed);
			this.rotationOverLifetime = base.Read<PersistentRotationOverLifetimeModule>(this.rotationOverLifetime, particleSystem.rotationOverLifetime);
			this.rotationBySpeed = base.Read<PersistentRotationBySpeedModule>(this.rotationBySpeed, particleSystem.rotationBySpeed);
			this.externalForces = base.Read<PersistentExternalForcesModule>(this.externalForces, particleSystem.externalForces);
			this.subEmitters = base.Read<PersistentSubEmittersModule>(this.subEmitters, particleSystem.subEmitters);
			this.textureSheetAnimation = base.Read<PersistentTextureSheetAnimationModule>(this.textureSheetAnimation, particleSystem.textureSheetAnimation);
			this.lights = base.Read<PersistentLightsModule>(this.lights, particleSystem.lights);
			this.main = base.Read<PersistentMainModule>(this.main, particleSystem.main);
			this.noise = base.Read<PersistentNoiseModule>(this.noise, particleSystem.noise);
			this.trails = base.Read<PersistentTrailModule>(this.trails, particleSystem.trails);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00040814 File Offset: 0x0003EC14
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentEmissionModule>(this.emission, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentCollisionModule>(this.collision, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentTriggerModule>(this.trigger, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentShapeModule>(this.shape, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentVelocityOverLifetimeModule>(this.velocityOverLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentLimitVelocityOverLifetimeModule>(this.limitVelocityOverLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentInheritVelocityModule>(this.inheritVelocity, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentForceOverLifetimeModule>(this.forceOverLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentColorOverLifetimeModule>(this.colorOverLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentColorBySpeedModule>(this.colorBySpeed, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentSizeOverLifetimeModule>(this.sizeOverLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentSizeBySpeedModule>(this.sizeBySpeed, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentRotationOverLifetimeModule>(this.rotationOverLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentRotationBySpeedModule>(this.rotationBySpeed, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentExternalForcesModule>(this.externalForces, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentSubEmittersModule>(this.subEmitters, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentTextureSheetAnimationModule>(this.textureSheetAnimation, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentLightsModule>(this.lights, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMainModule>(this.main, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentNoiseModule>(this.noise, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentTrailModule>(this.trails, dependencies, objects, allowNulls);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00040968 File Offset: 0x0003ED68
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem particleSystem = (ParticleSystem)obj;
			base.GetDependencies<PersistentEmissionModule>(this.emission, particleSystem.emission, dependencies);
			base.GetDependencies<PersistentCollisionModule>(this.collision, particleSystem.collision, dependencies);
			base.GetDependencies<PersistentTriggerModule>(this.trigger, particleSystem.trigger, dependencies);
			base.GetDependencies<PersistentShapeModule>(this.shape, particleSystem.shape, dependencies);
			base.GetDependencies<PersistentVelocityOverLifetimeModule>(this.velocityOverLifetime, particleSystem.velocityOverLifetime, dependencies);
			base.GetDependencies<PersistentLimitVelocityOverLifetimeModule>(this.limitVelocityOverLifetime, particleSystem.limitVelocityOverLifetime, dependencies);
			base.GetDependencies<PersistentInheritVelocityModule>(this.inheritVelocity, particleSystem.inheritVelocity, dependencies);
			base.GetDependencies<PersistentForceOverLifetimeModule>(this.forceOverLifetime, particleSystem.forceOverLifetime, dependencies);
			base.GetDependencies<PersistentColorOverLifetimeModule>(this.colorOverLifetime, particleSystem.colorOverLifetime, dependencies);
			base.GetDependencies<PersistentColorBySpeedModule>(this.colorBySpeed, particleSystem.colorBySpeed, dependencies);
			base.GetDependencies<PersistentSizeOverLifetimeModule>(this.sizeOverLifetime, particleSystem.sizeOverLifetime, dependencies);
			base.GetDependencies<PersistentSizeBySpeedModule>(this.sizeBySpeed, particleSystem.sizeBySpeed, dependencies);
			base.GetDependencies<PersistentRotationOverLifetimeModule>(this.rotationOverLifetime, particleSystem.rotationOverLifetime, dependencies);
			base.GetDependencies<PersistentRotationBySpeedModule>(this.rotationBySpeed, particleSystem.rotationBySpeed, dependencies);
			base.GetDependencies<PersistentExternalForcesModule>(this.externalForces, particleSystem.externalForces, dependencies);
			base.GetDependencies<PersistentSubEmittersModule>(this.subEmitters, particleSystem.subEmitters, dependencies);
			base.GetDependencies<PersistentTextureSheetAnimationModule>(this.textureSheetAnimation, particleSystem.textureSheetAnimation, dependencies);
			base.GetDependencies<PersistentLightsModule>(this.lights, particleSystem.lights, dependencies);
			base.GetDependencies<PersistentMainModule>(this.main, particleSystem.main, dependencies);
			base.GetDependencies<PersistentNoiseModule>(this.noise, particleSystem.noise, dependencies);
			base.GetDependencies<PersistentTrailModule>(this.trails, particleSystem.trails, dependencies);
		}

		// Token: 0x04000B8C RID: 2956
		public float time;

		// Token: 0x04000B8D RID: 2957
		public uint randomSeed;

		// Token: 0x04000B8E RID: 2958
		public bool useAutoRandomSeed;

		// Token: 0x04000B8F RID: 2959
		public PersistentEmissionModule emission;

		// Token: 0x04000B90 RID: 2960
		public PersistentCollisionModule collision;

		// Token: 0x04000B91 RID: 2961
		public PersistentTriggerModule trigger;

		// Token: 0x04000B92 RID: 2962
		public PersistentShapeModule shape;

		// Token: 0x04000B93 RID: 2963
		public PersistentVelocityOverLifetimeModule velocityOverLifetime;

		// Token: 0x04000B94 RID: 2964
		public PersistentLimitVelocityOverLifetimeModule limitVelocityOverLifetime;

		// Token: 0x04000B95 RID: 2965
		public PersistentInheritVelocityModule inheritVelocity;

		// Token: 0x04000B96 RID: 2966
		public PersistentForceOverLifetimeModule forceOverLifetime;

		// Token: 0x04000B97 RID: 2967
		public PersistentColorOverLifetimeModule colorOverLifetime;

		// Token: 0x04000B98 RID: 2968
		public PersistentColorBySpeedModule colorBySpeed;

		// Token: 0x04000B99 RID: 2969
		public PersistentSizeOverLifetimeModule sizeOverLifetime;

		// Token: 0x04000B9A RID: 2970
		public PersistentSizeBySpeedModule sizeBySpeed;

		// Token: 0x04000B9B RID: 2971
		public PersistentRotationOverLifetimeModule rotationOverLifetime;

		// Token: 0x04000B9C RID: 2972
		public PersistentRotationBySpeedModule rotationBySpeed;

		// Token: 0x04000B9D RID: 2973
		public PersistentExternalForcesModule externalForces;

		// Token: 0x04000B9E RID: 2974
		public PersistentSubEmittersModule subEmitters;

		// Token: 0x04000B9F RID: 2975
		public PersistentTextureSheetAnimationModule textureSheetAnimation;

		// Token: 0x04000BA0 RID: 2976
		public PersistentLightsModule lights;

		// Token: 0x04000BA1 RID: 2977
		public PersistentMainModule main;

		// Token: 0x04000BA2 RID: 2978
		public PersistentNoiseModule noise;

		// Token: 0x04000BA3 RID: 2979
		public PersistentTrailModule trails;
	}
}
