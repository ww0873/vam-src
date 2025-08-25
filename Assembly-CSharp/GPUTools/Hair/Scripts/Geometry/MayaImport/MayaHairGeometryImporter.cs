using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Debug;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Hair.Scripts.Utils;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport
{
	// Token: 0x02000A04 RID: 2564
	public class MayaHairGeometryImporter : GeometryProviderBase
	{
		// Token: 0x060040DE RID: 16606 RVA: 0x00134BC9 File Offset: 0x00132FC9
		public MayaHairGeometryImporter()
		{
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x00134BEE File Offset: 0x00132FEE
		public void Process()
		{
			if (this.ValidateImpl(true))
			{
				new CacheMayaHairData(this).Cache();
				this.Data.Validate(true);
			}
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x00134C14 File Offset: 0x00133014
		public override void Dispatch()
		{
			this.ScalpProvider.Dispatch();
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x00134C21 File Offset: 0x00133021
		public bool ValidateHairContainer(bool log)
		{
			if (this.HairContainer == null)
			{
				if (log)
				{
				}
				return false;
			}
			return true;
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x00134C3D File Offset: 0x0013303D
		public override bool Validate(bool log)
		{
			return this.ValidateImpl(log) && this.Data.Validate(log);
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x00134C5A File Offset: 0x0013305A
		private bool ValidateImpl(bool log)
		{
			return this.ScalpProvider.Validate(log) && this.ValidateHairContainer(log);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x00134C77 File Offset: 0x00133077
		public override Bounds GetBounds()
		{
			return base.transform.TransformBounds(this.Bounds);
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x00134C8A File Offset: 0x0013308A
		public override int GetSegmentsNum()
		{
			return this.Data.Segments;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x00134C97 File Offset: 0x00133097
		public override int GetStandsNum()
		{
			return this.Data.Vertices.Count / this.Data.Segments;
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x00134CB5 File Offset: 0x001330B5
		public override int[] GetIndices()
		{
			return this.Data.Indices;
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x00134CC2 File Offset: 0x001330C2
		public override List<Vector3> GetVertices()
		{
			return this.Data.Vertices;
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x00134CCF File Offset: 0x001330CF
		public override void SetVertices(List<Vector3> verts)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x00134CD6 File Offset: 0x001330D6
		public override List<float> GetRigidities()
		{
			return null;
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00134CD9 File Offset: 0x001330D9
		public override void SetRigidities(List<float> rigidities)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00134CE0 File Offset: 0x001330E0
		public override void CalculateNearbyVertexGroups()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x00134CE7 File Offset: 0x001330E7
		public override List<Vector4ListContainer> GetNearbyVertexGroups()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x00134CEE File Offset: 0x001330EE
		public override List<Color> GetColors()
		{
			return new Color[this.Data.Vertices.Count].ToList<Color>();
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x00134D0A File Offset: 0x0013310A
		public override GpuBuffer<Matrix4x4> GetTransformsBuffer()
		{
			return this.ScalpProvider.ToWorldMatricesBuffer;
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x00134D17 File Offset: 0x00133117
		public override GpuBuffer<Vector3> GetNormalsBuffer()
		{
			return this.ScalpProvider.NormalsBuffer;
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00134D24 File Offset: 0x00133124
		public override Matrix4x4 GetToWorldMatrix()
		{
			return this.ScalpProvider.ToWorldMatrix;
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x00134D31 File Offset: 0x00133131
		public override int[] GetHairRootToScalpMap()
		{
			return this.Data.HairRootToScalpMap;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x00134D3E File Offset: 0x0013313E
		private void OnDrawGizmos()
		{
			if (this.DebugDraw && this.Validate(false))
			{
				MayaImporterDebugDraw.Draw(this);
			}
		}

		// Token: 0x040030D5 RID: 12501
		[SerializeField]
		public bool DebugDraw = true;

		// Token: 0x040030D6 RID: 12502
		[SerializeField]
		public Texture2D RegionsTexture;

		// Token: 0x040030D7 RID: 12503
		[SerializeField]
		public MeshProvider ScalpProvider;

		// Token: 0x040030D8 RID: 12504
		[SerializeField]
		public GameObject HairContainer;

		// Token: 0x040030D9 RID: 12505
		[SerializeField]
		public float RegionThresholdDistance = 0.5f;

		// Token: 0x040030DA RID: 12506
		[SerializeField]
		public MayaHairData Data = new MayaHairData();

		// Token: 0x040030DB RID: 12507
		[SerializeField]
		public Bounds Bounds;
	}
}
