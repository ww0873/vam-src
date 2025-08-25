using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001AB RID: 427
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1060, typeof(PersistentBillboardRenderer))]
	[ProtoInclude(1061, typeof(PersistentSkinnedMeshRenderer))]
	[ProtoInclude(1062, typeof(PersistentTrailRenderer))]
	[ProtoInclude(1063, typeof(PersistentLineRenderer))]
	[ProtoInclude(1064, typeof(PersistentMeshRenderer))]
	[ProtoInclude(1065, typeof(PersistentSpriteRenderer))]
	[ProtoInclude(1066, typeof(PersistentParticleSystemRenderer))]
	[Serializable]
	public class PersistentRenderer : PersistentComponent
	{
		// Token: 0x060008EC RID: 2284 RVA: 0x00032588 File Offset: 0x00030988
		public PersistentRenderer()
		{
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00032590 File Offset: 0x00030990
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Renderer renderer = (Renderer)obj;
			renderer.enabled = this.enabled;
			renderer.shadowCastingMode = (ShadowCastingMode)this.shadowCastingMode;
			renderer.receiveShadows = this.receiveShadows;
			renderer.sharedMaterial = (Material)objects.Get(this.sharedMaterial);
			renderer.sharedMaterials = base.Resolve<Material, UnityEngine.Object>(this.sharedMaterials, objects);
			renderer.lightmapIndex = this.lightmapIndex;
			renderer.realtimeLightmapIndex = this.realtimeLightmapIndex;
			renderer.lightmapScaleOffset = this.lightmapScaleOffset;
			renderer.motionVectorGenerationMode = (MotionVectorGenerationMode)this.motionVectorGenerationMode;
			renderer.realtimeLightmapScaleOffset = this.realtimeLightmapScaleOffset;
			renderer.lightProbeUsage = (LightProbeUsage)this.lightProbeUsage;
			renderer.lightProbeProxyVolumeOverride = (GameObject)objects.Get(this.lightProbeProxyVolumeOverride);
			renderer.probeAnchor = (Transform)objects.Get(this.probeAnchor);
			renderer.reflectionProbeUsage = (ReflectionProbeUsage)this.reflectionProbeUsage;
			renderer.sortingLayerName = this.sortingLayerName;
			renderer.sortingLayerID = this.sortingLayerID;
			renderer.sortingOrder = this.sortingOrder;
			return renderer;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x000326AC File Offset: 0x00030AAC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Renderer renderer = (Renderer)obj;
			this.enabled = renderer.enabled;
			this.shadowCastingMode = (uint)renderer.shadowCastingMode;
			this.receiveShadows = renderer.receiveShadows;
			this.sharedMaterial = renderer.sharedMaterial.GetMappedInstanceID();
			this.sharedMaterials = renderer.sharedMaterials.GetMappedInstanceID();
			this.lightmapIndex = renderer.lightmapIndex;
			this.realtimeLightmapIndex = renderer.realtimeLightmapIndex;
			this.lightmapScaleOffset = renderer.lightmapScaleOffset;
			this.motionVectorGenerationMode = (uint)renderer.motionVectorGenerationMode;
			this.realtimeLightmapScaleOffset = renderer.realtimeLightmapScaleOffset;
			this.lightProbeUsage = (uint)renderer.lightProbeUsage;
			this.lightProbeProxyVolumeOverride = renderer.lightProbeProxyVolumeOverride.GetMappedInstanceID();
			this.probeAnchor = renderer.probeAnchor.GetMappedInstanceID();
			this.reflectionProbeUsage = (uint)renderer.reflectionProbeUsage;
			this.sortingLayerName = renderer.sortingLayerName;
			this.sortingLayerID = renderer.sortingLayerID;
			this.sortingOrder = renderer.sortingOrder;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x000327B0 File Offset: 0x00030BB0
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sharedMaterial, dependencies, objects, allowNulls);
			base.AddDependencies<T>(this.sharedMaterials, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.lightProbeProxyVolumeOverride, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.probeAnchor, dependencies, objects, allowNulls);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00032804 File Offset: 0x00030C04
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Renderer renderer = (Renderer)obj;
			base.AddDependency(renderer.sharedMaterial, dependencies);
			base.AddDependencies(renderer.sharedMaterials, dependencies);
			base.AddDependency(renderer.lightProbeProxyVolumeOverride, dependencies);
			base.AddDependency(renderer.probeAnchor, dependencies);
		}

		// Token: 0x040009CB RID: 2507
		public bool enabled;

		// Token: 0x040009CC RID: 2508
		public uint shadowCastingMode;

		// Token: 0x040009CD RID: 2509
		public bool receiveShadows;

		// Token: 0x040009CE RID: 2510
		public long sharedMaterial;

		// Token: 0x040009CF RID: 2511
		public long[] sharedMaterials;

		// Token: 0x040009D0 RID: 2512
		public int lightmapIndex;

		// Token: 0x040009D1 RID: 2513
		public int realtimeLightmapIndex;

		// Token: 0x040009D2 RID: 2514
		public Vector4 lightmapScaleOffset;

		// Token: 0x040009D3 RID: 2515
		public uint motionVectorGenerationMode;

		// Token: 0x040009D4 RID: 2516
		public Vector4 realtimeLightmapScaleOffset;

		// Token: 0x040009D5 RID: 2517
		public uint lightProbeUsage;

		// Token: 0x040009D6 RID: 2518
		public long lightProbeProxyVolumeOverride;

		// Token: 0x040009D7 RID: 2519
		public long probeAnchor;

		// Token: 0x040009D8 RID: 2520
		public uint reflectionProbeUsage;

		// Token: 0x040009D9 RID: 2521
		public string sortingLayerName;

		// Token: 0x040009DA RID: 2522
		public int sortingLayerID;

		// Token: 0x040009DB RID: 2523
		public int sortingOrder;
	}
}
