using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000189 RID: 393
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMeshFilter : PersistentComponent
	{
		// Token: 0x0600087D RID: 2173 RVA: 0x00036B40 File Offset: 0x00034F40
		public PersistentMeshFilter()
		{
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00036B48 File Offset: 0x00034F48
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			MeshFilter meshFilter = (MeshFilter)obj;
			meshFilter.sharedMesh = (Mesh)objects.Get(this.sharedMesh);
			return meshFilter;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00036B88 File Offset: 0x00034F88
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			MeshFilter meshFilter = (MeshFilter)obj;
			this.sharedMesh = meshFilter.sharedMesh.GetMappedInstanceID();
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00036BBB File Offset: 0x00034FBB
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sharedMesh, dependencies, objects, allowNulls);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00036BD8 File Offset: 0x00034FD8
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			MeshFilter meshFilter = (MeshFilter)obj;
			base.AddDependency(meshFilter.sharedMesh, dependencies);
		}

		// Token: 0x04000948 RID: 2376
		public long sharedMesh;
	}
}
