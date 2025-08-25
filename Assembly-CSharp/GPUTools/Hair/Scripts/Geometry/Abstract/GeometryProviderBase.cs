using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Types;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Abstract
{
	// Token: 0x020009EC RID: 2540
	public abstract class GeometryProviderBase : MonoBehaviour
	{
		// Token: 0x06003FF0 RID: 16368 RVA: 0x00130D09 File Offset: 0x0012F109
		protected GeometryProviderBase()
		{
		}

		// Token: 0x06003FF1 RID: 16369
		public abstract Bounds GetBounds();

		// Token: 0x06003FF2 RID: 16370
		public abstract int GetSegmentsNum();

		// Token: 0x06003FF3 RID: 16371
		public abstract int GetStandsNum();

		// Token: 0x06003FF4 RID: 16372
		public abstract int[] GetIndices();

		// Token: 0x06003FF5 RID: 16373
		public abstract List<Vector3> GetVertices();

		// Token: 0x06003FF6 RID: 16374
		public abstract void SetVertices(List<Vector3> verts);

		// Token: 0x06003FF7 RID: 16375
		public abstract List<float> GetRigidities();

		// Token: 0x06003FF8 RID: 16376
		public abstract void SetRigidities(List<float> rigidities);

		// Token: 0x06003FF9 RID: 16377
		public abstract List<Color> GetColors();

		// Token: 0x06003FFA RID: 16378
		public abstract void CalculateNearbyVertexGroups();

		// Token: 0x06003FFB RID: 16379
		public abstract List<Vector4ListContainer> GetNearbyVertexGroups();

		// Token: 0x06003FFC RID: 16380
		public abstract GpuBuffer<Matrix4x4> GetTransformsBuffer();

		// Token: 0x06003FFD RID: 16381
		public abstract GpuBuffer<Vector3> GetNormalsBuffer();

		// Token: 0x06003FFE RID: 16382
		public abstract Matrix4x4 GetToWorldMatrix();

		// Token: 0x06003FFF RID: 16383
		public abstract int[] GetHairRootToScalpMap();

		// Token: 0x06004000 RID: 16384
		public abstract void Dispatch();

		// Token: 0x06004001 RID: 16385
		public abstract bool Validate(bool log);
	}
}
