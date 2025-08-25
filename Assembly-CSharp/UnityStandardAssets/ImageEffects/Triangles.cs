using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E96 RID: 3734
	internal class Triangles
	{
		// Token: 0x06007147 RID: 28999 RVA: 0x002B0E0D File Offset: 0x002AF20D
		public Triangles()
		{
		}

		// Token: 0x06007148 RID: 29000 RVA: 0x002B0E18 File Offset: 0x002AF218
		private static bool HasMeshes()
		{
			if (Triangles.meshes == null)
			{
				return false;
			}
			for (int i = 0; i < Triangles.meshes.Length; i++)
			{
				if (null == Triangles.meshes[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007149 RID: 29001 RVA: 0x002B0E60 File Offset: 0x002AF260
		private static void Cleanup()
		{
			if (Triangles.meshes == null)
			{
				return;
			}
			for (int i = 0; i < Triangles.meshes.Length; i++)
			{
				if (null != Triangles.meshes[i])
				{
					UnityEngine.Object.DestroyImmediate(Triangles.meshes[i]);
					Triangles.meshes[i] = null;
				}
			}
			Triangles.meshes = null;
		}

		// Token: 0x0600714A RID: 29002 RVA: 0x002B0EBC File Offset: 0x002AF2BC
		private static Mesh[] GetMeshes(int totalWidth, int totalHeight)
		{
			if (Triangles.HasMeshes() && Triangles.currentTris == totalWidth * totalHeight)
			{
				return Triangles.meshes;
			}
			int num = 21666;
			int num2 = totalWidth * totalHeight;
			Triangles.currentTris = num2;
			int num3 = Mathf.CeilToInt(1f * (float)num2 / (1f * (float)num));
			Triangles.meshes = new Mesh[num3];
			int num4 = 0;
			for (int i = 0; i < num2; i += num)
			{
				int triCount = Mathf.FloorToInt((float)Mathf.Clamp(num2 - i, 0, num));
				Triangles.meshes[num4] = Triangles.GetMesh(triCount, i, totalWidth, totalHeight);
				num4++;
			}
			return Triangles.meshes;
		}

		// Token: 0x0600714B RID: 29003 RVA: 0x002B0F60 File Offset: 0x002AF360
		private static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
			Vector3[] array = new Vector3[triCount * 3];
			Vector2[] array2 = new Vector2[triCount * 3];
			Vector2[] array3 = new Vector2[triCount * 3];
			int[] array4 = new int[triCount * 3];
			for (int i = 0; i < triCount; i++)
			{
				int num = i * 3;
				int num2 = triOffset + i;
				float num3 = Mathf.Floor((float)(num2 % totalWidth)) / (float)totalWidth;
				float num4 = Mathf.Floor((float)(num2 / totalWidth)) / (float)totalHeight;
				Vector3 vector = new Vector3(num3 * 2f - 1f, num4 * 2f - 1f, 1f);
				array[num] = vector;
				array[num + 1] = vector;
				array[num + 2] = vector;
				array2[num] = new Vector2(0f, 0f);
				array2[num + 1] = new Vector2(1f, 0f);
				array2[num + 2] = new Vector2(0f, 1f);
				array3[num] = new Vector2(num3, num4);
				array3[num + 1] = new Vector2(num3, num4);
				array3[num + 2] = new Vector2(num3, num4);
				array4[num] = num;
				array4[num + 1] = num + 1;
				array4[num + 2] = num + 2;
			}
			mesh.vertices = array;
			mesh.triangles = array4;
			mesh.uv = array2;
			mesh.uv2 = array3;
			return mesh;
		}

		// Token: 0x0600714C RID: 29004 RVA: 0x002B1112 File Offset: 0x002AF512
		// Note: this type is marked as 'beforefieldinit'.
		static Triangles()
		{
		}

		// Token: 0x040064E3 RID: 25827
		private static Mesh[] meshes;

		// Token: 0x040064E4 RID: 25828
		private static int currentTris;
	}
}
