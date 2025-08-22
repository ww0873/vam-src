using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E6 RID: 230
	public static class RuntimeGizmos
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x00019EA4 File Offset: 0x000182A4
		static RuntimeGizmos()
		{
			RuntimeGizmos.HandlesMaterial.enableInstancing = true;
			RuntimeGizmos.LinesMaterial = new Material(Shader.Find("Battlehub/RTHandles/VertexColor"));
			RuntimeGizmos.LinesMaterial.enableInstancing = true;
			RuntimeGizmos.SelectionMaterial = new Material(Shader.Find("Battlehub/RTGizmos/Handles"));
			RuntimeGizmos.SelectionMaterial.SetFloat("_Offset", 1f);
			RuntimeGizmos.SelectionMaterial.SetFloat("_MinAlpha", 1f);
			RuntimeGizmos.SelectionMaterial.enableInstancing = true;
			RuntimeGizmos.CubeHandles = RuntimeGizmos.CreateCubeHandles(2f);
			RuntimeGizmos.ConeHandles = RuntimeGizmos.CreateConeHandles(2f);
			RuntimeGizmos.Selection = RuntimeGizmos.CreateHandlesMesh(2f, new Vector3[]
			{
				Vector3.zero
			}, new Vector3[]
			{
				Vector3.back
			});
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00019F94 File Offset: 0x00018394
		public static Vector3[] GetHandlesPositions()
		{
			return new Vector3[]
			{
				Vector3.up,
				Vector3.down,
				Vector3.right,
				Vector3.left,
				Vector3.forward,
				Vector3.back
			};
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001A010 File Offset: 0x00018410
		public static Vector3[] GetHandlesNormals()
		{
			return new Vector3[]
			{
				Vector3.up,
				Vector3.down,
				Vector3.right,
				Vector3.left,
				Vector3.forward,
				Vector3.back
			};
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001A08C File Offset: 0x0001848C
		public static Vector3[] GetConeHandlesPositions()
		{
			Vector3[] array = new Vector3[5];
			array[0] = Vector3.zero;
			int num = 1;
			Vector3 vector = new Vector3(1f, 1f, 0f);
			array[num] = vector.normalized;
			int num2 = 2;
			Vector3 vector2 = new Vector3(-1f, 1f, 0f);
			array[num2] = vector2.normalized;
			int num3 = 3;
			Vector3 vector3 = new Vector3(-1f, -1f, 0f);
			array[num3] = vector3.normalized;
			int num4 = 4;
			Vector3 vector4 = new Vector3(1f, -1f, 0f);
			array[num4] = vector4.normalized;
			return array;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001A154 File Offset: 0x00018554
		public static Vector3[] GetConeHandlesNormals()
		{
			Vector3[] array = new Vector3[5];
			array[0] = Vector3.forward;
			int num = 1;
			Vector3 vector = new Vector3(1f, 1f, 0f);
			array[num] = vector.normalized;
			int num2 = 2;
			Vector3 vector2 = new Vector3(-1f, 1f, 0f);
			array[num2] = vector2.normalized;
			int num3 = 3;
			Vector3 vector3 = new Vector3(-1f, -1f, 0f);
			array[num3] = vector3.normalized;
			int num4 = 4;
			Vector3 vector4 = new Vector3(1f, -1f, 0f);
			array[num4] = vector4.normalized;
			return array;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001A21C File Offset: 0x0001861C
		private static Mesh CreateConeHandles(float size)
		{
			Vector3[] coneHandlesPositions = RuntimeGizmos.GetConeHandlesPositions();
			Vector3[] coneHandlesNormals = RuntimeGizmos.GetConeHandlesNormals();
			return RuntimeGizmos.CreateHandlesMesh(size, coneHandlesPositions, coneHandlesNormals);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001A240 File Offset: 0x00018640
		private static Mesh CreateCubeHandles(float size)
		{
			Vector3[] handlesPositions = RuntimeGizmos.GetHandlesPositions();
			Vector3[] handlesNormals = RuntimeGizmos.GetHandlesNormals();
			return RuntimeGizmos.CreateHandlesMesh(size, handlesPositions, handlesNormals);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001A264 File Offset: 0x00018664
		private static Mesh CreateHandlesMesh(float size, Vector3[] vertices, Vector3[] normals)
		{
			Vector2[] array = new Vector2[vertices.Length * 4];
			Vector3[] array2 = new Vector3[vertices.Length * 4];
			Vector3[] array3 = new Vector3[normals.Length * 4];
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vert = vertices[i];
				Vector3 vert2 = normals[i];
				RuntimeGizmos.SetVertex(i, array2, vert);
				RuntimeGizmos.SetVertex(i, array3, vert2);
				RuntimeGizmos.SetOffset(i, array, size);
			}
			int[] array4 = new int[array2.Length + array2.Length / 2];
			int num = 0;
			for (int j = 0; j < array4.Length; j += 6)
			{
				array4[j] = num;
				array4[j + 1] = num + 1;
				array4[j + 2] = num + 2;
				array4[j + 3] = num;
				array4[j + 4] = num + 2;
				array4[j + 5] = num + 3;
				num += 4;
			}
			return new Mesh
			{
				vertices = array2,
				triangles = array4,
				normals = array3,
				uv = array
			};
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001A374 File Offset: 0x00018774
		private static void SetVertex(int index, Vector3[] vertices, Vector3 vert)
		{
			for (int i = 0; i < 4; i++)
			{
				vertices[index * 4 + i] = vert;
			}
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001A3A4 File Offset: 0x000187A4
		private static void SetOffset(int index, Vector2[] offsets, float size)
		{
			float num = size / 2f;
			offsets[index * 4] = new Vector2(-num, -num);
			offsets[index * 4 + 1] = new Vector2(-num, num);
			offsets[index * 4 + 2] = new Vector2(num, num);
			offsets[index * 4 + 3] = new Vector2(num, -num);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001A418 File Offset: 0x00018818
		public static void DrawSelection(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
			RuntimeGizmos.SelectionMaterial.color = color;
			RuntimeGizmos.SelectionMaterial.SetPass(0);
			Graphics.DrawMeshNow(RuntimeGizmos.Selection, matrix);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001A450 File Offset: 0x00018850
		public static void DrawCubeHandles(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
			RuntimeGizmos.HandlesMaterial.color = color;
			RuntimeGizmos.HandlesMaterial.SetPass(0);
			Graphics.DrawMeshNow(RuntimeGizmos.CubeHandles, matrix);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001A488 File Offset: 0x00018888
		public static void DrawConeHandles(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);
			RuntimeGizmos.HandlesMaterial.color = color;
			RuntimeGizmos.HandlesMaterial.SetPass(0);
			Graphics.DrawMeshNow(RuntimeGizmos.ConeHandles, matrix);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001A4C0 File Offset: 0x000188C0
		public static void DrawWireConeGL(float height, float radius, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			Matrix4x4 transform = Matrix4x4.TRS(height * Vector3.forward, Quaternion.identity, Vector3.one);
			Matrix4x4 m = Matrix4x4.TRS(position, rotation, scale);
			RuntimeGizmos.LinesMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(color);
			RuntimeGraphics.DrawCircleGL(transform, radius, 64);
			GL.Vertex(Vector3.zero);
			Vector3 a = Vector3.forward * height;
			Vector3 vector = new Vector3(1f, 1f, 0f);
			GL.Vertex(a + vector.normalized * radius);
			GL.Vertex(Vector3.zero);
			Vector3 a2 = Vector3.forward * height;
			Vector3 vector2 = new Vector3(-1f, 1f, 0f);
			GL.Vertex(a2 + vector2.normalized * radius);
			GL.Vertex(Vector3.zero);
			Vector3 a3 = Vector3.forward * height;
			Vector3 vector3 = new Vector3(-1f, -1f, 0f);
			GL.Vertex(a3 + vector3.normalized * radius);
			GL.Vertex(Vector3.zero);
			Vector3 a4 = Vector3.forward * height;
			Vector3 vector4 = new Vector3(1f, -1f, 0f);
			GL.Vertex(a4 + vector4.normalized * radius);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001A634 File Offset: 0x00018A34
		public static void DrawWireCapsuleGL(int axis, float height, float radius, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			radius = Mathf.Abs(radius);
			if (Mathf.Abs(height) < 2f * radius)
			{
				height = 0f;
			}
			else
			{
				height = Mathf.Abs(height) - 2f * radius;
			}
			Matrix4x4 transform;
			Matrix4x4 transform2;
			Matrix4x4 transform3;
			Matrix4x4 transform4;
			if (axis == 1)
			{
				transform = Matrix4x4.TRS(Vector3.up * height / 2f, Quaternion.AngleAxis(-90f, Vector3.right), Vector3.one);
				transform2 = Matrix4x4.TRS(Vector3.down * height / 2f, Quaternion.AngleAxis(-90f, Vector3.right), Vector3.one);
				transform3 = Matrix4x4.identity;
				transform4 = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
			}
			else if (axis == 0)
			{
				transform = Matrix4x4.TRS(Vector3.right * height / 2f, Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
				transform2 = Matrix4x4.TRS(Vector3.left * height / 2f, Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
				transform3 = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.forward), Vector3.one);
				transform4 = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.forward) * Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
			}
			else
			{
				transform = Matrix4x4.TRS(Vector3.forward * height / 2f, Quaternion.identity, Vector3.one);
				transform2 = Matrix4x4.TRS(Vector3.back * height / 2f, Quaternion.identity, Vector3.one);
				transform3 = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.right), Vector3.one);
				transform4 = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(-90f, Vector3.right) * Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
			}
			Matrix4x4 m = Matrix4x4.TRS(position, rotation, scale);
			RuntimeGizmos.LinesMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(color);
			RuntimeGraphics.DrawCircleGL(transform, radius, 64);
			RuntimeGraphics.DrawCircleGL(transform2, radius, 64);
			RuntimeGraphics.DrawCapsule2DGL(transform3, radius, height, 64);
			RuntimeGraphics.DrawCapsule2DGL(transform4, radius, height, 64);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001A8C0 File Offset: 0x00018CC0
		public static void DrawDirectionalLight(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			float screenScale = RuntimeGraphics.GetScreenScale(position, Camera.current);
			Matrix4x4 transform = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
			Matrix4x4 m = Matrix4x4.TRS(position, Quaternion.identity, scale * screenScale);
			RuntimeGizmos.LinesMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(color);
			float radius = 0.25f;
			float num = 1.25f;
			RuntimeGraphics.DrawCircleGL(transform, radius, 64);
			RuntimeGraphics.DrawWireConeGL(transform, Vector3.zero, radius, num, 8, 0f, 6.2831855f);
			Vector3 v = transform.MultiplyPoint(Vector3.zero);
			Vector3 v2 = transform.MultiplyPoint(Vector3.forward * num);
			GL.Vertex(v);
			GL.Vertex(v2);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001A98C File Offset: 0x00018D8C
		public static void DrawWireDisc(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			Matrix4x4 transform = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
			Matrix4x4 m = Matrix4x4.TRS(position, Quaternion.identity, scale);
			RuntimeGizmos.LinesMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(color);
			RuntimeGraphics.DrawCircleGL(transform, 1f, 64);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001A9F4 File Offset: 0x00018DF4
		public static void DrawWireSphereGL(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			Matrix4x4 transform = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
			Matrix4x4 transform2 = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.AngleAxis(-90f, Vector3.right), Vector3.one);
			Matrix4x4 transform3 = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
			Matrix4x4 m = Matrix4x4.TRS(position, Quaternion.identity, scale);
			RuntimeGizmos.LinesMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(color);
			RuntimeGraphics.DrawCircleGL(transform, 1f, 64);
			RuntimeGraphics.DrawCircleGL(transform2, 1f, 64);
			RuntimeGraphics.DrawCircleGL(transform3, 1f, 64);
			if (Camera.current.orthographic)
			{
				Matrix4x4 transform4 = Matrix4x4.TRS(Vector3.zero, Camera.current.transform.rotation, Vector3.one);
				RuntimeGraphics.DrawCircleGL(transform4, 1f, 64);
			}
			else
			{
				Vector3 vector = Camera.current.transform.position - position;
				Vector3 normalized = vector.normalized;
				if (Vector3.Dot(normalized, Camera.current.transform.forward) < 0f)
				{
					float magnitude = vector.magnitude;
					Matrix4x4 transform5 = Matrix4x4.TRS(normalized * 0.56f * scale.x / magnitude, Quaternion.LookRotation(normalized, Camera.current.transform.up), Vector3.one);
					RuntimeGraphics.DrawCircleGL(transform5, 1f, 64);
				}
			}
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001AB98 File Offset: 0x00018F98
		public static void DrawWireCubeGL(ref Bounds bounds, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			RuntimeGizmos.LinesMaterial.SetPass(0);
			Matrix4x4 m = Matrix4x4.TRS(position, rotation, scale);
			Vector3 v = new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
			Vector3 v2 = new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
			Vector3 v3 = new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
			Vector3 v4 = new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
			Vector3 v5 = new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
			Vector3 v6 = new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
			Vector3 v7 = new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
			Vector3 v8 = new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GL.Begin(1);
			GL.Color(color);
			GL.Vertex(v);
			GL.Vertex(v2);
			GL.Vertex(v3);
			GL.Vertex(v4);
			GL.Vertex(v5);
			GL.Vertex(v6);
			GL.Vertex(v7);
			GL.Vertex(v8);
			GL.Vertex(v);
			GL.Vertex(v3);
			GL.Vertex(v2);
			GL.Vertex(v4);
			GL.Vertex(v5);
			GL.Vertex(v7);
			GL.Vertex(v6);
			GL.Vertex(v8);
			GL.Vertex(v);
			GL.Vertex(v5);
			GL.Vertex(v2);
			GL.Vertex(v6);
			GL.Vertex(v3);
			GL.Vertex(v7);
			GL.Vertex(v4);
			GL.Vertex(v8);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x0400047B RID: 1147
		private static readonly Material LinesMaterial;

		// Token: 0x0400047C RID: 1148
		private static readonly Material HandlesMaterial = new Material(Shader.Find("Battlehub/RTGizmos/Handles"));

		// Token: 0x0400047D RID: 1149
		private static readonly Material SelectionMaterial;

		// Token: 0x0400047E RID: 1150
		private static Mesh CubeHandles;

		// Token: 0x0400047F RID: 1151
		private static Mesh ConeHandles;

		// Token: 0x04000480 RID: 1152
		private static Mesh Selection;

		// Token: 0x04000481 RID: 1153
		public const float HandleScale = 2f;
	}
}
