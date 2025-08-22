using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Import
{
	// Token: 0x020009FB RID: 2555
	[ExecuteInEditMode]
	public class HairGeometryImporter : GeometryProviderBase
	{
		// Token: 0x060040A5 RID: 16549 RVA: 0x00133EE6 File Offset: 0x001322E6
		public HairGeometryImporter()
		{
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x00133F12 File Offset: 0x00132312
		public override bool Validate(bool log)
		{
			if (this.Indices == null || this.Indices.Length == 0)
			{
				if (log)
				{
					Debug.LogError("Provider does not have any generated hair geometry");
				}
				return false;
			}
			return this.ValidateImpl(log);
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x00133F45 File Offset: 0x00132345
		private bool ValidateImpl(bool log)
		{
			if (!this.ScalpProvider.Validate(false))
			{
				if (log)
				{
					Debug.LogError("Scalp field is null");
				}
				return false;
			}
			return this.HairGroupsProvider.Validate(log);
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x00133F78 File Offset: 0x00132378
		public void Process()
		{
			if (!this.ValidateImpl(true))
			{
				return;
			}
			this.HairGroupsProvider.Process(this.ScalpProvider.ToWorldMatrix.inverse);
			this.Indices = this.ProcessIndices();
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x00133FBC File Offset: 0x001323BC
		public override void Dispatch()
		{
			this.ScalpProvider.Dispatch();
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x00133FC9 File Offset: 0x001323C9
		private void OnDestroy()
		{
			this.ScalpProvider.Dispose();
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x00133FD8 File Offset: 0x001323D8
		private int[] ProcessMap()
		{
			float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(this.ScalpProvider.Mesh) * 0.1f;
			if (this.ScalpProvider.Type == ScalpMeshType.Skinned || this.ScalpProvider.Type == ScalpMeshType.PreCalc)
			{
				List<Vector3> scalpVertices = this.ScalpProvider.Mesh.vertices.ToList<Vector3>();
				return ScalpProcessingTools.HairRootToScalpIndices(scalpVertices, this.HairGroupsProvider.Vertices, this.GetSegmentsNum(), accuracy).ToArray();
			}
			return new int[this.HairGroupsProvider.Vertices.Count / this.GetSegmentsNum()];
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x00134070 File Offset: 0x00132470
		private int[] ProcessIndices()
		{
			float accuracy = ScalpProcessingTools.MiddleDistanceBetweenPoints(this.ScalpProvider.Mesh) * 0.1f;
			List<int> scalpIndices = this.ScalpProvider.Mesh.GetIndices(0).ToList<int>();
			List<Vector3> scalpVertices = this.ScalpProvider.Mesh.vertices.ToList<Vector3>();
			return ScalpProcessingTools.ProcessIndices(scalpIndices, scalpVertices, this.HairGroupsProvider.VerticesGroups, this.GetSegmentsNum(), accuracy).ToArray();
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x001340DF File Offset: 0x001324DF
		public override GpuBuffer<Matrix4x4> GetTransformsBuffer()
		{
			return this.ScalpProvider.ToWorldMatricesBuffer;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x001340EC File Offset: 0x001324EC
		public override GpuBuffer<Vector3> GetNormalsBuffer()
		{
			return this.ScalpProvider.NormalsBuffer;
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x001340F9 File Offset: 0x001324F9
		public override Matrix4x4 GetToWorldMatrix()
		{
			return this.ScalpProvider.ToWorldMatrix;
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x00134106 File Offset: 0x00132506
		public override int[] GetHairRootToScalpMap()
		{
			return this.ProcessMap();
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0013410E File Offset: 0x0013250E
		public override Bounds GetBounds()
		{
			return base.transform.TransformBounds(this.Bounds);
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x00134121 File Offset: 0x00132521
		public override int GetSegmentsNum()
		{
			return this.Segments;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x00134129 File Offset: 0x00132529
		public override int GetStandsNum()
		{
			return this.HairGroupsProvider.Vertices.Count / this.Segments;
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x00134142 File Offset: 0x00132542
		public override int[] GetIndices()
		{
			return this.Indices;
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0013414A File Offset: 0x0013254A
		public override List<Vector3> GetVertices()
		{
			return this.HairGroupsProvider.Vertices;
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x00134157 File Offset: 0x00132557
		public override void SetVertices(List<Vector3> verts)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x0013415E File Offset: 0x0013255E
		public override List<float> GetRigidities()
		{
			return null;
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x00134161 File Offset: 0x00132561
		public override void SetRigidities(List<float> rigidities)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x00134168 File Offset: 0x00132568
		public override void CalculateNearbyVertexGroups()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x0013416F File Offset: 0x0013256F
		public override List<Vector4ListContainer> GetNearbyVertexGroups()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x00134176 File Offset: 0x00132576
		public override List<Color> GetColors()
		{
			return this.HairGroupsProvider.Colors;
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x00134184 File Offset: 0x00132584
		private void OnDrawGizmos()
		{
			if (!this.DebugDraw || this.GetVertices() == null || !this.ValidateImpl(false))
			{
				return;
			}
			Matrix4x4 toWorldMatrix = this.ScalpProvider.ToWorldMatrix;
			List<Vector3> vertices = this.GetVertices();
			for (int i = 1; i < vertices.Count; i++)
			{
				if (i % this.Segments != 0)
				{
					Vector3 from = toWorldMatrix.MultiplyPoint3x4(vertices[i - 1]);
					Vector3 to = toWorldMatrix.MultiplyPoint3x4(vertices[i]);
					Gizmos.DrawLine(from, to);
				}
			}
			Bounds bounds = this.GetBounds();
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}

		// Token: 0x040030BA RID: 12474
		[SerializeField]
		public bool DebugDraw = true;

		// Token: 0x040030BB RID: 12475
		[SerializeField]
		public int Segments = 5;

		// Token: 0x040030BC RID: 12476
		[SerializeField]
		public HairGroupsProvider HairGroupsProvider = new HairGroupsProvider();

		// Token: 0x040030BD RID: 12477
		[SerializeField]
		public MeshProvider ScalpProvider = new MeshProvider();

		// Token: 0x040030BE RID: 12478
		[SerializeField]
		public int[] Indices;

		// Token: 0x040030BF RID: 12479
		[SerializeField]
		public Bounds Bounds;
	}
}
