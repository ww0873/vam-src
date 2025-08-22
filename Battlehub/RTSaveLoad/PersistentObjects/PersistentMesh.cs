using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200020D RID: 525
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMesh : PersistentObject
	{
		// Token: 0x06000A8C RID: 2700 RVA: 0x00041638 File Offset: 0x0003FA38
		public PersistentMesh()
		{
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00041640 File Offset: 0x0003FA40
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Mesh mesh = (Mesh)obj;
			mesh.vertices = this.vertices;
			mesh.subMeshCount = this.subMeshCount;
			if (this.m_tris != null)
			{
				for (int i = 0; i < this.subMeshCount; i++)
				{
					mesh.SetTriangles(this.m_tris[i].Array, i);
				}
			}
			mesh.bounds = this.bounds;
			mesh.boneWeights = this.boneWeights;
			mesh.bindposes = this.bindposes;
			mesh.normals = this.normals;
			mesh.tangents = this.tangents;
			mesh.uv = this.uv;
			mesh.uv2 = this.uv2;
			mesh.uv3 = this.uv3;
			mesh.uv4 = this.uv4;
			mesh.colors = this.colors;
			return mesh;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00041730 File Offset: 0x0003FB30
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Mesh mesh = (Mesh)obj;
			this.bounds = mesh.bounds;
			this.subMeshCount = mesh.subMeshCount;
			this.boneWeights = mesh.boneWeights;
			this.bindposes = mesh.bindposes;
			this.vertices = mesh.vertices;
			this.normals = mesh.normals;
			this.tangents = mesh.tangents;
			this.uv = mesh.uv;
			this.uv2 = mesh.uv2;
			this.uv3 = mesh.uv3;
			this.uv4 = mesh.uv4;
			this.colors = mesh.colors;
			this.m_tris = new IntArray[this.subMeshCount];
			for (int i = 0; i < this.subMeshCount; i++)
			{
				this.m_tris[i] = new IntArray();
				this.m_tris[i].Array = mesh.GetTriangles(i);
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0004182B File Offset: 0x0003FC2B
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000BC9 RID: 3017
		public IntArray[] m_tris;

		// Token: 0x04000BCA RID: 3018
		public Bounds bounds;

		// Token: 0x04000BCB RID: 3019
		public int subMeshCount;

		// Token: 0x04000BCC RID: 3020
		public BoneWeight[] boneWeights;

		// Token: 0x04000BCD RID: 3021
		public Matrix4x4[] bindposes;

		// Token: 0x04000BCE RID: 3022
		public Vector3[] vertices;

		// Token: 0x04000BCF RID: 3023
		public Vector3[] normals;

		// Token: 0x04000BD0 RID: 3024
		public Vector4[] tangents;

		// Token: 0x04000BD1 RID: 3025
		public Vector2[] uv;

		// Token: 0x04000BD2 RID: 3026
		public Vector2[] uv2;

		// Token: 0x04000BD3 RID: 3027
		public Vector2[] uv3;

		// Token: 0x04000BD4 RID: 3028
		public Vector2[] uv4;

		// Token: 0x04000BD5 RID: 3029
		public Color[] colors;

		// Token: 0x04000BD6 RID: 3030
		public int[] triangles;
	}
}
