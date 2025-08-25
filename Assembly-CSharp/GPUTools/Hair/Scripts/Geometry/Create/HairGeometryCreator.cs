using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
	// Token: 0x020009F4 RID: 2548
	[Serializable]
	public class HairGeometryCreator : GeometryProviderBase
	{
		// Token: 0x06004022 RID: 16418 RVA: 0x00131360 File Offset: 0x0012F760
		public HairGeometryCreator()
		{
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x001313BF File Offset: 0x0012F7BF
		private void Awake()
		{
			if (!this.isProcessed && Application.isPlaying)
			{
				this.Process();
			}
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x001313DC File Offset: 0x0012F7DC
		public void Optimize()
		{
			this.Process();
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x001313E4 File Offset: 0x0012F7E4
		public void SetDirty()
		{
			this.isProcessed = false;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x001313ED File Offset: 0x0012F7ED
		public void ClearNearbyVertices()
		{
			this.nearbyVertexGroups = null;
			this.isProcessed = false;
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x001313FD File Offset: 0x0012F7FD
		public bool IsDirty()
		{
			return !this.isProcessed;
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00131408 File Offset: 0x0012F808
		public void Process()
		{
			Debug.Log("Hair Geometry Creator Process() called");
			if (!this.ScalpProvider.Validate(true))
			{
				return;
			}
			List<List<Vector3>> list = new List<List<Vector3>>();
			List<Vector3> list2 = new List<Vector3>();
			List<Color> list3 = new List<Color>();
			foreach (GeometryGroupData geometryGroupData in this.Geomery.List)
			{
				list.Add(geometryGroupData.Vertices);
				list2.AddRange(geometryGroupData.Vertices);
				list3.AddRange(geometryGroupData.Colors);
			}
			this.vertices = list2;
			this.colors = list3;
			Mesh mesh = this.ScalpProvider.Mesh;
			float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(mesh) * 0.1f;
			this.indices = ScalpProcessingTools.ProcessIndices(mesh.GetIndices(0).ToList<int>(), mesh.vertices.ToList<Vector3>(), list, this.Segments, accuracy).ToArray();
			if (this.ScalpProvider.Type == ScalpMeshType.Skinned)
			{
				this.hairRootToScalpIndices = ScalpProcessingTools.HairRootToScalpIndices(mesh.vertices.ToList<Vector3>(), this.vertices, this.Segments, accuracy).ToArray();
			}
			else if (this.ScalpProvider.Type == ScalpMeshType.PreCalc)
			{
				this.hairRootToScalpIndices = ScalpProcessingTools.HairRootToScalpIndices(mesh.vertices.ToList<Vector3>(), this.vertices, this.Segments, accuracy).ToArray();
			}
			else
			{
				this.hairRootToScalpIndices = new int[this.vertices.Count / this.GetSegmentsNum()];
			}
			this.CalculateNearbyVertexGroups();
			this.isProcessed = true;
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x001315BC File Offset: 0x0012F9BC
		public override void Dispatch()
		{
			if (this.ScalpProvider.Type == ScalpMeshType.PreCalc && this.ScalpProvider.PreCalcProvider != null)
			{
				this.ScalpProvider.PreCalcProvider.provideToWorldMatrices = true;
			}
			this.ScalpProvider.Dispatch();
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x0013160C File Offset: 0x0012FA0C
		public override bool Validate(bool log)
		{
			return this.ScalpProvider.Validate(log) && this.Geomery.Validate(log);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x0013162E File Offset: 0x0012FA2E
		private void OnDestroy()
		{
			this.ScalpProvider.Dispose();
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x0013163B File Offset: 0x0012FA3B
		public override Bounds GetBounds()
		{
			return base.transform.TransformBounds(this.Bounds);
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x0013164E File Offset: 0x0012FA4E
		public override int GetSegmentsNum()
		{
			return this.Segments;
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00131656 File Offset: 0x0012FA56
		public override int GetStandsNum()
		{
			return this.vertices.Count / this.Segments;
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x0013166A File Offset: 0x0012FA6A
		public override int[] GetIndices()
		{
			return this.indices;
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00131672 File Offset: 0x0012FA72
		public override List<Vector3> GetVertices()
		{
			return this.vertices;
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x0013167A File Offset: 0x0012FA7A
		public override void SetVertices(List<Vector3> verts)
		{
			this.vertices = verts;
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x00131683 File Offset: 0x0012FA83
		public override List<float> GetRigidities()
		{
			return null;
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00131686 File Offset: 0x0012FA86
		public override void SetRigidities(List<float> rigidities)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x0013168D File Offset: 0x0012FA8D
		private bool AddToHashSet(HashSet<Vector4> set, int i1, int i2, float distance, float distanceRatio)
		{
			return i1 != -1 && i2 != -1 && set.Add((i1 <= i2) ? new Vector4((float)i2, (float)i1, distance, distanceRatio) : new Vector4((float)i1, (float)i2, distance, distanceRatio));
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x001316CC File Offset: 0x0012FACC
		public override void CalculateNearbyVertexGroups()
		{
			this.nearbyVertexGroups = new List<Vector4ListContainer>();
			Matrix4x4 toWorldMatrix = this.GetToWorldMatrix();
			List<Vector3> list = new List<Vector3>();
			foreach (Vector3 point in this.vertices)
			{
				Vector3 item = toWorldMatrix.MultiplyPoint3x4(point);
				list.Add(item);
			}
			HashSet<Vector4> hashSet = new HashSet<Vector4>();
			for (int i = 0; i < list.Count; i++)
			{
				if (i % this.Segments != 0)
				{
					int num = i / this.Segments;
					List<HairGeometryCreator.VertDistance> list2 = new List<HairGeometryCreator.VertDistance>();
					for (int j = 0; j < list.Count; j++)
					{
						if (j % this.Segments != 0)
						{
							int num2 = j / this.Segments;
							if (num != num2)
							{
								float num3 = Vector3.Distance(list[i], list[j]);
								if (num3 < this.NearbyVertexSearchDistance && num3 > this.NearbyVertexSearchMinDistance)
								{
									HairGeometryCreator.VertDistance item2;
									item2.vert = j;
									item2.distance = num3;
									list2.Add(item2);
								}
							}
						}
					}
					List<HairGeometryCreator.VertDistance> list3 = list2;
					if (HairGeometryCreator.<>f__am$cache0 == null)
					{
						HairGeometryCreator.<>f__am$cache0 = new Comparison<HairGeometryCreator.VertDistance>(HairGeometryCreator.<CalculateNearbyVertexGroups>m__0);
					}
					list3.Sort(HairGeometryCreator.<>f__am$cache0);
					int num4 = 0;
					foreach (HairGeometryCreator.VertDistance vertDistance in list2)
					{
						if (num4 > this.MaxNearbyVertsPerVert)
						{
							break;
						}
						this.AddToHashSet(hashSet, i, vertDistance.vert, vertDistance.distance, (this.NearbyVertexSearchDistance - vertDistance.distance) / this.NearbyVertexSearchDistance);
						num4++;
					}
				}
			}
			Debug.Log("Found " + hashSet.Count + " nearby vertex pairs");
			List<Vector4> list4 = hashSet.ToList<Vector4>();
			List<HashSet<int>> list5 = new List<HashSet<int>>();
			foreach (Vector4 item3 in list4)
			{
				bool flag = false;
				int item4 = (int)item3.x;
				int item5 = (int)item3.y;
				for (int k = 0; k < list5.Count; k++)
				{
					HashSet<int> hashSet2 = list5[k];
					if (!hashSet2.Contains(item4) && !hashSet2.Contains(item5))
					{
						flag = true;
						hashSet2.Add(item4);
						hashSet2.Add(item5);
						Vector4ListContainer vector4ListContainer = this.nearbyVertexGroups[k];
						vector4ListContainer.List.Add(item3);
						break;
					}
				}
				if (!flag)
				{
					HashSet<int> hashSet3 = new HashSet<int>();
					list5.Add(hashSet3);
					hashSet3.Add(item4);
					hashSet3.Add(item5);
					Vector4ListContainer vector4ListContainer2 = new Vector4ListContainer();
					vector4ListContainer2.List.Add(item3);
					this.nearbyVertexGroups.Add(vector4ListContainer2);
				}
			}
			Debug.Log("Created " + this.nearbyVertexGroups.Count + " nearby vertex pair groups");
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x00131A4C File Offset: 0x0012FE4C
		public override List<Vector4ListContainer> GetNearbyVertexGroups()
		{
			if (this.nearbyVertexGroups == null || this.nearbyVertexGroups.Count == 0)
			{
				Debug.Log("Vertex groups not precalculated. Must build at runtime which is slow");
				this.CalculateNearbyVertexGroups();
			}
			return this.nearbyVertexGroups;
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x00131A7F File Offset: 0x0012FE7F
		public override List<Color> GetColors()
		{
			return this.colors;
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x00131A87 File Offset: 0x0012FE87
		public override Matrix4x4 GetToWorldMatrix()
		{
			return this.ScalpProvider.ToWorldMatrix;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x00131A94 File Offset: 0x0012FE94
		public override GpuBuffer<Matrix4x4> GetTransformsBuffer()
		{
			return this.ScalpProvider.ToWorldMatricesBuffer;
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x00131AA1 File Offset: 0x0012FEA1
		public override GpuBuffer<Vector3> GetNormalsBuffer()
		{
			return this.ScalpProvider.NormalsBuffer;
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x00131AAE File Offset: 0x0012FEAE
		public override int[] GetHairRootToScalpMap()
		{
			return this.hairRootToScalpIndices;
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x00131AB8 File Offset: 0x0012FEB8
		private void OnDrawGizmos()
		{
			if (!this.DebugDraw || !this.ScalpProvider.Validate(false))
			{
				return;
			}
			foreach (GeometryGroupData geometryGroupData in this.Geomery.List)
			{
				bool flag = this.Geomery.Selected == geometryGroupData;
				if (flag || this.DebugDrawUnselectedGroups)
				{
					geometryGroupData.OnDrawGizmos(this.Segments, flag, this.ScalpProvider.ToWorldMatrix);
				}
			}
			Bounds bounds = this.GetBounds();
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x00131B84 File Offset: 0x0012FF84
		[CompilerGenerated]
		private static int <CalculateNearbyVertexGroups>m__0(HairGeometryCreator.VertDistance vd1, HairGeometryCreator.VertDistance vd2)
		{
			return vd1.distance.CompareTo(vd2.distance);
		}

		// Token: 0x0400306F RID: 12399
		[SerializeField]
		public bool DebugDraw;

		// Token: 0x04003070 RID: 12400
		[SerializeField]
		public bool DebugDrawUnselectedGroups = true;

		// Token: 0x04003071 RID: 12401
		[SerializeField]
		public int Segments = 5;

		// Token: 0x04003072 RID: 12402
		[SerializeField]
		public GeometryBrush Brush = new GeometryBrush();

		// Token: 0x04003073 RID: 12403
		[SerializeField]
		public MeshProvider ScalpProvider = new MeshProvider();

		// Token: 0x04003074 RID: 12404
		[SerializeField]
		public List<GameObject> ColliderProviders = new List<GameObject>();

		// Token: 0x04003075 RID: 12405
		[SerializeField]
		public CreatorGeometry Geomery = new CreatorGeometry();

		// Token: 0x04003076 RID: 12406
		[SerializeField]
		public Bounds Bounds;

		// Token: 0x04003077 RID: 12407
		[SerializeField]
		public float NearbyVertexSearchMinDistance;

		// Token: 0x04003078 RID: 12408
		[SerializeField]
		public float NearbyVertexSearchDistance = 0.01f;

		// Token: 0x04003079 RID: 12409
		[SerializeField]
		public int MaxNearbyVertsPerVert = 2;

		// Token: 0x0400307A RID: 12410
		[SerializeField]
		private int[] indices;

		// Token: 0x0400307B RID: 12411
		[SerializeField]
		private List<Vector3> vertices;

		// Token: 0x0400307C RID: 12412
		[SerializeField]
		private List<Color> colors;

		// Token: 0x0400307D RID: 12413
		[SerializeField]
		private int[] hairRootToScalpIndices;

		// Token: 0x0400307E RID: 12414
		[SerializeField]
		public List<Vector4ListContainer> nearbyVertexGroups;

		// Token: 0x0400307F RID: 12415
		[SerializeField]
		private bool isProcessed;

		// Token: 0x04003080 RID: 12416
		[CompilerGenerated]
		private static Comparison<HairGeometryCreator.VertDistance> <>f__am$cache0;

		// Token: 0x020009F5 RID: 2549
		protected struct VertDistance
		{
			// Token: 0x04003081 RID: 12417
			public int vert;

			// Token: 0x04003082 RID: 12418
			public float distance;
		}
	}
}
