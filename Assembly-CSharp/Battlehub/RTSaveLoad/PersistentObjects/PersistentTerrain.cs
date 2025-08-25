using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C5 RID: 453
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTerrain : PersistentBehaviour
	{
		// Token: 0x06000942 RID: 2370 RVA: 0x0003965D File Offset: 0x00037A5D
		public PersistentTerrain()
		{
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00039668 File Offset: 0x00037A68
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Terrain terrain = (Terrain)obj;
			terrain.terrainData = (TerrainData)objects.Get(this.terrainData);
			terrain.treeDistance = this.treeDistance;
			terrain.treeBillboardDistance = this.treeBillboardDistance;
			terrain.treeCrossFadeLength = this.treeCrossFadeLength;
			terrain.treeMaximumFullLODCount = this.treeMaximumFullLODCount;
			terrain.detailObjectDistance = this.detailObjectDistance;
			terrain.detailObjectDensity = this.detailObjectDensity;
			terrain.heightmapPixelError = this.heightmapPixelError;
			terrain.heightmapMaximumLOD = this.heightmapMaximumLOD;
			terrain.basemapDistance = this.basemapDistance;
			terrain.lightmapIndex = this.lightmapIndex;
			terrain.realtimeLightmapIndex = this.realtimeLightmapIndex;
			terrain.lightmapScaleOffset = this.lightmapScaleOffset;
			terrain.realtimeLightmapScaleOffset = this.realtimeLightmapScaleOffset;
			terrain.castShadows = this.castShadows;
			terrain.reflectionProbeUsage = (ReflectionProbeUsage)this.reflectionProbeUsage;
			terrain.materialType = (Terrain.MaterialType)this.materialType;
			terrain.materialTemplate = (Material)objects.Get(this.materialTemplate);
			terrain.legacySpecular = this.legacySpecular;
			terrain.legacyShininess = this.legacyShininess;
			terrain.drawHeightmap = this.drawHeightmap;
			terrain.drawTreesAndFoliage = this.drawTreesAndFoliage;
			terrain.treeLODBiasMultiplier = this.treeLODBiasMultiplier;
			terrain.collectDetailPatches = this.collectDetailPatches;
			terrain.editorRenderFlags = (TerrainRenderFlags)this.editorRenderFlags;
			return terrain;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000397D4 File Offset: 0x00037BD4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Terrain terrain = (Terrain)obj;
			this.terrainData = terrain.terrainData.GetMappedInstanceID();
			this.treeDistance = terrain.treeDistance;
			this.treeBillboardDistance = terrain.treeBillboardDistance;
			this.treeCrossFadeLength = terrain.treeCrossFadeLength;
			this.treeMaximumFullLODCount = terrain.treeMaximumFullLODCount;
			this.detailObjectDistance = terrain.detailObjectDistance;
			this.detailObjectDensity = terrain.detailObjectDensity;
			this.heightmapPixelError = terrain.heightmapPixelError;
			this.heightmapMaximumLOD = terrain.heightmapMaximumLOD;
			this.basemapDistance = terrain.basemapDistance;
			this.lightmapIndex = terrain.lightmapIndex;
			this.realtimeLightmapIndex = terrain.realtimeLightmapIndex;
			this.lightmapScaleOffset = terrain.lightmapScaleOffset;
			this.realtimeLightmapScaleOffset = terrain.realtimeLightmapScaleOffset;
			this.castShadows = terrain.castShadows;
			this.reflectionProbeUsage = (uint)terrain.reflectionProbeUsage;
			this.materialType = (uint)terrain.materialType;
			this.materialTemplate = terrain.materialTemplate.GetMappedInstanceID();
			this.legacySpecular = terrain.legacySpecular;
			this.legacyShininess = terrain.legacyShininess;
			this.drawHeightmap = terrain.drawHeightmap;
			this.drawTreesAndFoliage = terrain.drawTreesAndFoliage;
			this.treeLODBiasMultiplier = terrain.treeLODBiasMultiplier;
			this.collectDetailPatches = terrain.collectDetailPatches;
			this.editorRenderFlags = (uint)terrain.editorRenderFlags;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0003992C File Offset: 0x00037D2C
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.terrainData, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.materialTemplate, dependencies, objects, allowNulls);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00039958 File Offset: 0x00037D58
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Terrain terrain = (Terrain)obj;
			base.AddDependency(terrain.terrainData, dependencies);
			base.AddDependency(terrain.materialTemplate, dependencies);
		}

		// Token: 0x04000A4F RID: 2639
		public long terrainData;

		// Token: 0x04000A50 RID: 2640
		public float treeDistance;

		// Token: 0x04000A51 RID: 2641
		public float treeBillboardDistance;

		// Token: 0x04000A52 RID: 2642
		public float treeCrossFadeLength;

		// Token: 0x04000A53 RID: 2643
		public int treeMaximumFullLODCount;

		// Token: 0x04000A54 RID: 2644
		public float detailObjectDistance;

		// Token: 0x04000A55 RID: 2645
		public float detailObjectDensity;

		// Token: 0x04000A56 RID: 2646
		public float heightmapPixelError;

		// Token: 0x04000A57 RID: 2647
		public int heightmapMaximumLOD;

		// Token: 0x04000A58 RID: 2648
		public float basemapDistance;

		// Token: 0x04000A59 RID: 2649
		public int lightmapIndex;

		// Token: 0x04000A5A RID: 2650
		public int realtimeLightmapIndex;

		// Token: 0x04000A5B RID: 2651
		public Vector4 lightmapScaleOffset;

		// Token: 0x04000A5C RID: 2652
		public Vector4 realtimeLightmapScaleOffset;

		// Token: 0x04000A5D RID: 2653
		public bool castShadows;

		// Token: 0x04000A5E RID: 2654
		public uint reflectionProbeUsage;

		// Token: 0x04000A5F RID: 2655
		public uint materialType;

		// Token: 0x04000A60 RID: 2656
		public long materialTemplate;

		// Token: 0x04000A61 RID: 2657
		public Color legacySpecular;

		// Token: 0x04000A62 RID: 2658
		public float legacyShininess;

		// Token: 0x04000A63 RID: 2659
		public bool drawHeightmap;

		// Token: 0x04000A64 RID: 2660
		public bool drawTreesAndFoliage;

		// Token: 0x04000A65 RID: 2661
		public float treeLODBiasMultiplier;

		// Token: 0x04000A66 RID: 2662
		public bool collectDetailPatches;

		// Token: 0x04000A67 RID: 2663
		public uint editorRenderFlags;
	}
}
