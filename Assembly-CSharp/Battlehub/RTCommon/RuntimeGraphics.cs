using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000C2 RID: 194
	public static class RuntimeGraphics
	{
		// Token: 0x0600036E RID: 878 RVA: 0x0001514A File Offset: 0x0001354A
		static RuntimeGraphics()
		{
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00015160 File Offset: 0x00013560
		public static float GetScreenScale(Vector3 position, Camera camera)
		{
			float num = (float)camera.pixelHeight;
			if (camera.orthographic)
			{
				return camera.orthographicSize * 2f / num * 90f;
			}
			Transform transform = camera.transform;
			float num2 = Vector3.Dot(position - transform.position, transform.forward);
			float num3 = 2f * num2 * Mathf.Tan(camera.fieldOfView * 0.5f * 0.017453292f);
			return num3 / num * 90f;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000151E0 File Offset: 0x000135E0
		public static Mesh CreateCubeMesh(Color color, Vector3 center, float scale, float cubeLength = 1f, float cubeWidth = 1f, float cubeHeight = 1f)
		{
			cubeHeight *= scale;
			cubeWidth *= scale;
			cubeLength *= scale;
			Vector3 vector = center + new Vector3(-cubeLength * 0.5f, -cubeWidth * 0.5f, cubeHeight * 0.5f);
			Vector3 vector2 = center + new Vector3(cubeLength * 0.5f, -cubeWidth * 0.5f, cubeHeight * 0.5f);
			Vector3 vector3 = center + new Vector3(cubeLength * 0.5f, -cubeWidth * 0.5f, -cubeHeight * 0.5f);
			Vector3 vector4 = center + new Vector3(-cubeLength * 0.5f, -cubeWidth * 0.5f, -cubeHeight * 0.5f);
			Vector3 vector5 = center + new Vector3(-cubeLength * 0.5f, cubeWidth * 0.5f, cubeHeight * 0.5f);
			Vector3 vector6 = center + new Vector3(cubeLength * 0.5f, cubeWidth * 0.5f, cubeHeight * 0.5f);
			Vector3 vector7 = center + new Vector3(cubeLength * 0.5f, cubeWidth * 0.5f, -cubeHeight * 0.5f);
			Vector3 vector8 = center + new Vector3(-cubeLength * 0.5f, cubeWidth * 0.5f, -cubeHeight * 0.5f);
			Vector3[] array = new Vector3[]
			{
				vector,
				vector2,
				vector3,
				vector4,
				vector8,
				vector5,
				vector,
				vector4,
				vector5,
				vector6,
				vector2,
				vector,
				vector7,
				vector8,
				vector4,
				vector3,
				vector6,
				vector7,
				vector3,
				vector2,
				vector8,
				vector7,
				vector6,
				vector5
			};
			int[] triangles = new int[]
			{
				3,
				1,
				0,
				3,
				2,
				1,
				7,
				5,
				4,
				7,
				6,
				5,
				11,
				9,
				8,
				11,
				10,
				9,
				15,
				13,
				12,
				15,
				14,
				13,
				19,
				17,
				16,
				19,
				18,
				17,
				23,
				21,
				20,
				23,
				22,
				21
			};
			Color[] array2 = new Color[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = color;
			}
			Mesh mesh = new Mesh();
			mesh.name = "cube";
			mesh.vertices = array;
			mesh.triangles = triangles;
			mesh.colors = array2;
			mesh.RecalculateNormals();
			return mesh;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00015500 File Offset: 0x00013900
		public static Mesh CreateQuadMesh(float quadWidth = 1f, float quadHeight = 1f)
		{
			Vector3 vector = new Vector3(-quadWidth * 0.5f, -quadHeight * 0.5f, 0f);
			Vector3 vector2 = new Vector3(quadWidth * 0.5f, -quadHeight * 0.5f, 0f);
			Vector3 vector3 = new Vector3(-quadWidth * 0.5f, quadHeight * 0.5f, 0f);
			Vector3 vector4 = new Vector3(quadWidth * 0.5f, quadHeight * 0.5f, 0f);
			Vector3[] vertices = new Vector3[]
			{
				vector3,
				vector4,
				vector2,
				vector
			};
			int[] triangles = new int[]
			{
				3,
				1,
				0,
				3,
				2,
				1
			};
			Vector2[] uv = new Vector2[]
			{
				new Vector2(1f, 0f),
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f)
			};
			Mesh mesh = new Mesh();
			mesh.name = "quad";
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uv;
			mesh.RecalculateNormals();
			return mesh;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00015673 File Offset: 0x00013A73
		public static void DrawQuad(Matrix4x4 transform)
		{
			Graphics.DrawMeshNow(RuntimeGraphics.m_quadMesh, transform);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00015680 File Offset: 0x00013A80
		public static void DrawCircleGL(Matrix4x4 transform, float radius = 1f, int pointsCount = 64)
		{
			RuntimeGraphics.DrawArcGL(transform, Vector3.zero, radius, pointsCount, 0f, 6.2831855f);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001569C File Offset: 0x00013A9C
		public static void DrawArcGL(Matrix4x4 transform, Vector3 offset, float radius = 1f, int pointsCount = 64, float fromAngle = 0f, float toAngle = 6.2831855f)
		{
			float num = fromAngle;
			float num2 = toAngle - fromAngle;
			float z = 0f;
			float x = radius * Mathf.Cos(num);
			float y = radius * Mathf.Sin(num);
			Vector3 v = transform.MultiplyPoint(new Vector3(x, y, z) + offset);
			for (int i = 0; i < pointsCount; i++)
			{
				GL.Vertex(v);
				num += num2 / (float)pointsCount;
				x = radius * Mathf.Cos(num);
				y = radius * Mathf.Sin(num);
				Vector3 vector = transform.MultiplyPoint(new Vector3(x, y, z) + offset);
				GL.Vertex(vector);
				v = vector;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00015740 File Offset: 0x00013B40
		public static void DrawWireConeGL(Matrix4x4 transform, Vector3 offset, float radius = 1f, float length = 2f, int pointsCount = 64, float fromAngle = 0f, float toAngle = 6.2831855f)
		{
			float num = fromAngle;
			float num2 = toAngle - fromAngle;
			float z = 0f;
			for (int i = 0; i < pointsCount; i++)
			{
				float x = radius * Mathf.Cos(num);
				float y = radius * Mathf.Sin(num);
				Vector3 v = transform.MultiplyPoint(new Vector3(x, y, z) + offset);
				Vector3 v2 = transform.MultiplyPoint(new Vector3(x, y, z) + offset + Vector3.forward * length);
				GL.Vertex(v);
				GL.Vertex(v2);
				num += num2 / (float)pointsCount;
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000157DC File Offset: 0x00013BDC
		public static void DrawCapsule2DGL(Matrix4x4 transform, float radius = 1f, float height = 1f, int pointsCount = 64)
		{
			RuntimeGraphics.DrawArcGL(transform, Vector3.up * height / 2f, radius, pointsCount / 2, 0f, 3.1415927f);
			RuntimeGraphics.DrawArcGL(transform, Vector3.down * height / 2f, radius, pointsCount / 2, 3.1415927f, 6.2831855f);
			GL.Vertex(transform.MultiplyPoint(new Vector3(radius, height / 2f, 0f)));
			GL.Vertex(transform.MultiplyPoint(new Vector3(radius, -height / 2f, 0f)));
			GL.Vertex(transform.MultiplyPoint(new Vector3(-radius, height / 2f, 0f)));
			GL.Vertex(transform.MultiplyPoint(new Vector3(-radius, -height / 2f, 0f)));
		}

		// Token: 0x040003F7 RID: 1015
		public const int RuntimeHandlesLayer = 24;

		// Token: 0x040003F8 RID: 1016
		private static Mesh m_quadMesh = RuntimeGraphics.CreateQuadMesh(1f, 1f);
	}
}
