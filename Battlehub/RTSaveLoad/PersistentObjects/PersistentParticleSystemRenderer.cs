using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000197 RID: 407
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentParticleSystemRenderer : PersistentRenderer
	{
		// Token: 0x060008B0 RID: 2224 RVA: 0x000374E9 File Offset: 0x000358E9
		public PersistentParticleSystemRenderer()
		{
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x000374F4 File Offset: 0x000358F4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystemRenderer particleSystemRenderer = (ParticleSystemRenderer)obj;
			particleSystemRenderer.renderMode = (ParticleSystemRenderMode)this.renderMode;
			particleSystemRenderer.lengthScale = this.lengthScale;
			particleSystemRenderer.velocityScale = this.velocityScale;
			particleSystemRenderer.cameraVelocityScale = this.cameraVelocityScale;
			particleSystemRenderer.normalDirection = this.normalDirection;
			particleSystemRenderer.alignment = (ParticleSystemRenderSpace)this.alignment;
			particleSystemRenderer.pivot = this.pivot;
			particleSystemRenderer.sortMode = (ParticleSystemSortMode)this.sortMode;
			particleSystemRenderer.sortingFudge = this.sortingFudge;
			particleSystemRenderer.minParticleSize = this.minParticleSize;
			particleSystemRenderer.maxParticleSize = this.maxParticleSize;
			particleSystemRenderer.mesh = (Mesh)objects.Get(this.mesh);
			particleSystemRenderer.trailMaterial = (Material)objects.Get(this.trailMaterial);
			return particleSystemRenderer;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000375D0 File Offset: 0x000359D0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystemRenderer particleSystemRenderer = (ParticleSystemRenderer)obj;
			this.renderMode = (uint)particleSystemRenderer.renderMode;
			this.lengthScale = particleSystemRenderer.lengthScale;
			this.velocityScale = particleSystemRenderer.velocityScale;
			this.cameraVelocityScale = particleSystemRenderer.cameraVelocityScale;
			this.normalDirection = particleSystemRenderer.normalDirection;
			this.alignment = (uint)particleSystemRenderer.alignment;
			this.pivot = particleSystemRenderer.pivot;
			this.sortMode = (uint)particleSystemRenderer.sortMode;
			this.sortingFudge = particleSystemRenderer.sortingFudge;
			this.minParticleSize = particleSystemRenderer.minParticleSize;
			this.maxParticleSize = particleSystemRenderer.maxParticleSize;
			this.mesh = particleSystemRenderer.mesh.GetMappedInstanceID();
			this.trailMaterial = particleSystemRenderer.trailMaterial.GetMappedInstanceID();
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00037698 File Offset: 0x00035A98
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.mesh, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.trailMaterial, dependencies, objects, allowNulls);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x000376C4 File Offset: 0x00035AC4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystemRenderer particleSystemRenderer = (ParticleSystemRenderer)obj;
			base.AddDependency(particleSystemRenderer.mesh, dependencies);
			base.AddDependency(particleSystemRenderer.trailMaterial, dependencies);
		}

		// Token: 0x04000979 RID: 2425
		public uint renderMode;

		// Token: 0x0400097A RID: 2426
		public float lengthScale;

		// Token: 0x0400097B RID: 2427
		public float velocityScale;

		// Token: 0x0400097C RID: 2428
		public float cameraVelocityScale;

		// Token: 0x0400097D RID: 2429
		public float normalDirection;

		// Token: 0x0400097E RID: 2430
		public uint alignment;

		// Token: 0x0400097F RID: 2431
		public Vector3 pivot;

		// Token: 0x04000980 RID: 2432
		public uint sortMode;

		// Token: 0x04000981 RID: 2433
		public float sortingFudge;

		// Token: 0x04000982 RID: 2434
		public float minParticleSize;

		// Token: 0x04000983 RID: 2435
		public float maxParticleSize;

		// Token: 0x04000984 RID: 2436
		public long mesh;

		// Token: 0x04000985 RID: 2437
		public long trailMaterial;
	}
}
