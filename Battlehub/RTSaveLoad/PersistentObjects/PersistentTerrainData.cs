using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C7 RID: 455
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTerrainData : PersistentObject
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x00039A60 File Offset: 0x00037E60
		public PersistentTerrainData()
		{
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00039A68 File Offset: 0x00037E68
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			TerrainData terrainData = (TerrainData)obj;
			terrainData.heightmapResolution = this.heightmapResolution;
			terrainData.size = this.size;
			terrainData.thickness = this.thickness;
			terrainData.wavingGrassStrength = this.wavingGrassStrength;
			terrainData.wavingGrassAmount = this.wavingGrassAmount;
			terrainData.wavingGrassSpeed = this.wavingGrassSpeed;
			terrainData.wavingGrassTint = this.wavingGrassTint;
			terrainData.detailPrototypes = base.Write<DetailPrototype>(terrainData.detailPrototypes, this.detailPrototypes, objects);
			terrainData.treeInstances = this.treeInstances;
			terrainData.treePrototypes = base.Write<TreePrototype>(terrainData.treePrototypes, this.treePrototypes, objects);
			terrainData.alphamapResolution = this.alphamapResolution;
			terrainData.baseMapResolution = this.baseMapResolution;
			terrainData.splatPrototypes = base.Write<SplatPrototype>(terrainData.splatPrototypes, this.splatPrototypes, objects);
			return terrainData;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00039B54 File Offset: 0x00037F54
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			TerrainData terrainData = (TerrainData)obj;
			this.heightmapResolution = terrainData.heightmapResolution;
			this.size = terrainData.size;
			this.thickness = terrainData.thickness;
			this.wavingGrassStrength = terrainData.wavingGrassStrength;
			this.wavingGrassAmount = terrainData.wavingGrassAmount;
			this.wavingGrassSpeed = terrainData.wavingGrassSpeed;
			this.wavingGrassTint = terrainData.wavingGrassTint;
			this.detailPrototypes = base.Read<PersistentDetailPrototype, DetailPrototype>(this.detailPrototypes, terrainData.detailPrototypes);
			this.treeInstances = terrainData.treeInstances;
			this.treePrototypes = base.Read<PersistentTreePrototype, TreePrototype>(this.treePrototypes, terrainData.treePrototypes);
			this.alphamapResolution = terrainData.alphamapResolution;
			this.baseMapResolution = terrainData.baseMapResolution;
			this.splatPrototypes = base.Read<PersistentSplatPrototype, SplatPrototype>(this.splatPrototypes, terrainData.splatPrototypes);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00039C36 File Offset: 0x00038036
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentDetailPrototype>(this.detailPrototypes, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentTreePrototype>(this.treePrototypes, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentSplatPrototype>(this.splatPrototypes, dependencies, objects, allowNulls);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00039C70 File Offset: 0x00038070
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			TerrainData terrainData = (TerrainData)obj;
			base.GetDependencies<PersistentDetailPrototype, DetailPrototype>(this.detailPrototypes, terrainData.detailPrototypes, dependencies);
			base.GetDependencies<PersistentTreePrototype, TreePrototype>(this.treePrototypes, terrainData.treePrototypes, dependencies);
			base.GetDependencies<PersistentSplatPrototype, SplatPrototype>(this.splatPrototypes, terrainData.splatPrototypes, dependencies);
		}

		// Token: 0x04000A69 RID: 2665
		public int heightmapResolution;

		// Token: 0x04000A6A RID: 2666
		public Vector3 size;

		// Token: 0x04000A6B RID: 2667
		public float thickness;

		// Token: 0x04000A6C RID: 2668
		public float wavingGrassStrength;

		// Token: 0x04000A6D RID: 2669
		public float wavingGrassAmount;

		// Token: 0x04000A6E RID: 2670
		public float wavingGrassSpeed;

		// Token: 0x04000A6F RID: 2671
		public Color wavingGrassTint;

		// Token: 0x04000A70 RID: 2672
		public PersistentDetailPrototype[] detailPrototypes;

		// Token: 0x04000A71 RID: 2673
		public TreeInstance[] treeInstances;

		// Token: 0x04000A72 RID: 2674
		public PersistentTreePrototype[] treePrototypes;

		// Token: 0x04000A73 RID: 2675
		public int alphamapResolution;

		// Token: 0x04000A74 RID: 2676
		public int baseMapResolution;

		// Token: 0x04000A75 RID: 2677
		public PersistentSplatPrototype[] splatPrototypes;
	}
}
