using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000188 RID: 392
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMeshCollider : PersistentCollider
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x00036A2D File Offset: 0x00034E2D
		public PersistentMeshCollider()
		{
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00036A38 File Offset: 0x00034E38
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			MeshCollider meshCollider = (MeshCollider)obj;
			meshCollider.sharedMesh = (Mesh)objects.Get(this.sharedMesh);
			meshCollider.convex = this.convex;
			meshCollider.inflateMesh = this.inflateMesh;
			meshCollider.skinWidth = this.skinWidth;
			return meshCollider;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00036A9C File Offset: 0x00034E9C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			MeshCollider meshCollider = (MeshCollider)obj;
			this.sharedMesh = meshCollider.sharedMesh.GetMappedInstanceID();
			this.convex = meshCollider.convex;
			this.inflateMesh = meshCollider.inflateMesh;
			this.skinWidth = meshCollider.skinWidth;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00036AF3 File Offset: 0x00034EF3
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sharedMesh, dependencies, objects, allowNulls);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00036B10 File Offset: 0x00034F10
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			MeshCollider meshCollider = (MeshCollider)obj;
			base.AddDependency(meshCollider.sharedMesh, dependencies);
		}

		// Token: 0x04000944 RID: 2372
		public long sharedMesh;

		// Token: 0x04000945 RID: 2373
		public bool convex;

		// Token: 0x04000946 RID: 2374
		public bool inflateMesh;

		// Token: 0x04000947 RID: 2375
		public float skinWidth;
	}
}
