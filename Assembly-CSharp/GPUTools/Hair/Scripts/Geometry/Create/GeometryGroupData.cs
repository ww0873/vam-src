using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
	// Token: 0x020009F1 RID: 2545
	[Serializable]
	public class GeometryGroupData
	{
		// Token: 0x0600400E RID: 16398 RVA: 0x00130EF6 File Offset: 0x0012F2F6
		public GeometryGroupData()
		{
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x00130F14 File Offset: 0x0012F314
		public void Generate(Mesh mesh, int segments)
		{
			this.Vertices = new List<Vector3>();
			this.GuideVertices = new List<Vector3>();
			this.Distances = new List<float>();
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < mesh.vertices.Length; i++)
			{
				Vector3 vector = mesh.vertices[i];
				Vector3 normal = mesh.normals[i];
				if (!list.Contains(vector))
				{
					List<Vector3> collection = this.CreateStand(vector, normal, segments);
					this.Vertices.AddRange(collection);
					this.Distances.Add(this.Length / (float)segments);
					this.GuideVertices.AddRange(collection);
					list.Add(vector);
				}
			}
			this.Colors = new List<Color>();
			for (int j = 0; j < this.Vertices.Count; j++)
			{
				this.Colors.Add(new Color(1f, 1f, 1f));
			}
			Debug.Log("Total nodes:" + this.Vertices.Count);
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0013103F File Offset: 0x0012F43F
		public void Fix()
		{
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x00131041 File Offset: 0x0012F441
		public void Reset()
		{
			this.Vertices.Clear();
			this.Vertices = null;
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x00131058 File Offset: 0x0012F458
		private List<Vector3> CreateStand(Vector3 start, Vector3 normal, int segments)
		{
			List<Vector3> list = new List<Vector3>();
			float num = this.Length / (float)segments;
			for (int i = 0; i < segments; i++)
			{
				list.Add(start + normal * (num * (float)i));
			}
			return list;
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0013109E File Offset: 0x0012F49E
		public void Record()
		{
			this.history.Record(this.Vertices);
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x001310B1 File Offset: 0x0012F4B1
		public void Undo()
		{
			if (this.history.IsUndo)
			{
				this.Vertices = this.history.Undo();
			}
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x001310D4 File Offset: 0x0012F4D4
		public void Redo()
		{
			if (this.history.IsRedo)
			{
				this.Vertices = this.history.Redo();
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06004016 RID: 16406 RVA: 0x001310F7 File Offset: 0x0012F4F7
		public bool IsUndo
		{
			get
			{
				return this.history.IsUndo;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06004017 RID: 16407 RVA: 0x00131104 File Offset: 0x0012F504
		public bool IsRedo
		{
			get
			{
				return this.history.IsRedo;
			}
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x00131111 File Offset: 0x0012F511
		public void Clear()
		{
			this.history.Clear();
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00131120 File Offset: 0x0012F520
		public void OnDrawGizmos(int segments, bool isSelected, Matrix4x4 toWorld)
		{
			this.Segments = segments;
			if (this.Vertices == null)
			{
				return;
			}
			if (this.Colors == null || this.Colors.Count != this.Vertices.Count)
			{
				this.FillColors();
			}
			for (int i = 1; i < this.Vertices.Count; i++)
			{
				if (i % segments != 0)
				{
					Color color = this.Colors[i];
					Gizmos.color = ((!isSelected) ? Color.grey : color);
					Vector3 from = toWorld.MultiplyPoint3x4(this.Vertices[i - 1]);
					Vector3 to = toWorld.MultiplyPoint3x4(this.Vertices[i]);
					Gizmos.DrawLine(from, to);
				}
			}
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x001311EC File Offset: 0x0012F5EC
		private void FillColors()
		{
			this.Colors = new List<Color>();
			for (int i = 0; i < this.Vertices.Count; i++)
			{
				this.Colors.Add(Color.white);
			}
		}

		// Token: 0x04003063 RID: 12387
		public float Length = 2f;

		// Token: 0x04003064 RID: 12388
		public int Segments;

		// Token: 0x04003065 RID: 12389
		public List<Vector3> GuideVertices;

		// Token: 0x04003066 RID: 12390
		public List<float> Distances;

		// Token: 0x04003067 RID: 12391
		public List<Vector3> Vertices;

		// Token: 0x04003068 RID: 12392
		public List<Color> Colors;

		// Token: 0x04003069 RID: 12393
		[SerializeField]
		private GeometryGroupHistory history = new GeometryGroupHistory();
	}
}
