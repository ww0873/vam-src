using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001A9 RID: 425
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentReflectionProbe : PersistentBehaviour
	{
		// Token: 0x060008E3 RID: 2275 RVA: 0x0003800D File Offset: 0x0003640D
		public PersistentReflectionProbe()
		{
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00038018 File Offset: 0x00036418
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ReflectionProbe reflectionProbe = (ReflectionProbe)obj;
			reflectionProbe.hdr = this.hdr;
			reflectionProbe.size = this.size;
			reflectionProbe.center = this.center;
			reflectionProbe.nearClipPlane = this.nearClipPlane;
			reflectionProbe.farClipPlane = this.farClipPlane;
			reflectionProbe.shadowDistance = this.shadowDistance;
			reflectionProbe.resolution = this.resolution;
			reflectionProbe.cullingMask = this.cullingMask;
			reflectionProbe.clearFlags = (ReflectionProbeClearFlags)this.clearFlags;
			reflectionProbe.backgroundColor = this.backgroundColor;
			reflectionProbe.intensity = this.intensity;
			reflectionProbe.blendDistance = this.blendDistance;
			reflectionProbe.boxProjection = this.boxProjection;
			reflectionProbe.mode = (ReflectionProbeMode)this.mode;
			reflectionProbe.importance = this.importance;
			reflectionProbe.refreshMode = (ReflectionProbeRefreshMode)this.refreshMode;
			reflectionProbe.timeSlicingMode = (ReflectionProbeTimeSlicingMode)this.timeSlicingMode;
			reflectionProbe.bakedTexture = (Texture)objects.Get(this.bakedTexture);
			reflectionProbe.customBakedTexture = (Texture)objects.Get(this.customBakedTexture);
			return reflectionProbe;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0003813C File Offset: 0x0003653C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ReflectionProbe reflectionProbe = (ReflectionProbe)obj;
			this.hdr = reflectionProbe.hdr;
			this.size = reflectionProbe.size;
			this.center = reflectionProbe.center;
			this.nearClipPlane = reflectionProbe.nearClipPlane;
			this.farClipPlane = reflectionProbe.farClipPlane;
			this.shadowDistance = reflectionProbe.shadowDistance;
			this.resolution = reflectionProbe.resolution;
			this.cullingMask = reflectionProbe.cullingMask;
			this.clearFlags = (uint)reflectionProbe.clearFlags;
			this.backgroundColor = reflectionProbe.backgroundColor;
			this.intensity = reflectionProbe.intensity;
			this.blendDistance = reflectionProbe.blendDistance;
			this.boxProjection = reflectionProbe.boxProjection;
			this.mode = (uint)reflectionProbe.mode;
			this.importance = reflectionProbe.importance;
			this.refreshMode = (uint)reflectionProbe.refreshMode;
			this.timeSlicingMode = (uint)reflectionProbe.timeSlicingMode;
			this.bakedTexture = reflectionProbe.bakedTexture.GetMappedInstanceID();
			this.customBakedTexture = reflectionProbe.customBakedTexture.GetMappedInstanceID();
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0003824C File Offset: 0x0003664C
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.bakedTexture, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.customBakedTexture, dependencies, objects, allowNulls);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00038278 File Offset: 0x00036678
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ReflectionProbe reflectionProbe = (ReflectionProbe)obj;
			base.AddDependency(reflectionProbe.bakedTexture, dependencies);
			base.AddDependency(reflectionProbe.customBakedTexture, dependencies);
		}

		// Token: 0x040009B2 RID: 2482
		public bool hdr;

		// Token: 0x040009B3 RID: 2483
		public Vector3 size;

		// Token: 0x040009B4 RID: 2484
		public Vector3 center;

		// Token: 0x040009B5 RID: 2485
		public float nearClipPlane;

		// Token: 0x040009B6 RID: 2486
		public float farClipPlane;

		// Token: 0x040009B7 RID: 2487
		public float shadowDistance;

		// Token: 0x040009B8 RID: 2488
		public int resolution;

		// Token: 0x040009B9 RID: 2489
		public int cullingMask;

		// Token: 0x040009BA RID: 2490
		public uint clearFlags;

		// Token: 0x040009BB RID: 2491
		public Color backgroundColor;

		// Token: 0x040009BC RID: 2492
		public float intensity;

		// Token: 0x040009BD RID: 2493
		public float blendDistance;

		// Token: 0x040009BE RID: 2494
		public bool boxProjection;

		// Token: 0x040009BF RID: 2495
		public uint mode;

		// Token: 0x040009C0 RID: 2496
		public int importance;

		// Token: 0x040009C1 RID: 2497
		public uint refreshMode;

		// Token: 0x040009C2 RID: 2498
		public uint timeSlicingMode;

		// Token: 0x040009C3 RID: 2499
		public long bakedTexture;

		// Token: 0x040009C4 RID: 2500
		public long customBakedTexture;
	}
}
