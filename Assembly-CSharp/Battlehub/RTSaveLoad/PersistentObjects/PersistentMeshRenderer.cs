using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200018A RID: 394
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMeshRenderer : PersistentRenderer
	{
		// Token: 0x06000882 RID: 2178 RVA: 0x00036C08 File Offset: 0x00035008
		public PersistentMeshRenderer()
		{
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00036C10 File Offset: 0x00035010
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			MeshRenderer meshRenderer = (MeshRenderer)obj;
			meshRenderer.additionalVertexStreams = (Mesh)objects.Get(this.additionalVertexStreams);
			return meshRenderer;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00036C50 File Offset: 0x00035050
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			MeshRenderer meshRenderer = (MeshRenderer)obj;
			this.additionalVertexStreams = meshRenderer.additionalVertexStreams.GetMappedInstanceID();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00036C83 File Offset: 0x00035083
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.additionalVertexStreams, dependencies, objects, allowNulls);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00036CA0 File Offset: 0x000350A0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			MeshRenderer meshRenderer = (MeshRenderer)obj;
			base.AddDependency(meshRenderer.additionalVertexStreams, dependencies);
		}

		// Token: 0x04000949 RID: 2377
		public long additionalVertexStreams;
	}
}
