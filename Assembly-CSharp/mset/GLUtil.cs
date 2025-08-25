using System;
using UnityEngine;

namespace mset
{
	// Token: 0x02000328 RID: 808
	public class GLUtil
	{
		// Token: 0x06001315 RID: 4885 RVA: 0x0006DF32 File Offset: 0x0006C332
		public GLUtil()
		{
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0006DF3A File Offset: 0x0006C33A
		public static void StripFirstVertex(Vector3 v)
		{
			GLUtil.prevStripVertex = v;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0006DF42 File Offset: 0x0006C342
		public static void StripFirstVertex3(float x, float y, float z)
		{
			GLUtil.prevStripVertex.Set(x, y, z);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x0006DF51 File Offset: 0x0006C351
		public static void StripVertex3(float x, float y, float z)
		{
			GL.Vertex(GLUtil.prevStripVertex);
			GL.Vertex3(x, y, z);
			GLUtil.prevStripVertex.Set(x, y, z);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0006DF72 File Offset: 0x0006C372
		public static void StripVertex(Vector3 v)
		{
			GL.Vertex(GLUtil.prevStripVertex);
			GL.Vertex(v);
			GLUtil.prevStripVertex = v;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0006DF8C File Offset: 0x0006C38C
		public static void DrawCube(Vector3 pos, Vector3 radius)
		{
			Vector3 vector = pos - radius;
			Vector3 vector2 = pos + radius;
			GL.Begin(7);
			GL.Vertex3(vector.x, vector.y, vector.z);
			GL.Vertex3(vector2.x, vector.y, vector.z);
			GL.Vertex3(vector2.x, vector.y, vector2.z);
			GL.Vertex3(vector.x, vector.y, vector2.z);
			GL.Vertex3(vector2.x, vector2.y, vector.z);
			GL.Vertex3(vector.x, vector2.y, vector.z);
			GL.Vertex3(vector.x, vector2.y, vector2.z);
			GL.Vertex3(vector2.x, vector2.y, vector2.z);
			GL.Vertex3(vector2.x, vector.y, vector.z);
			GL.Vertex3(vector2.x, vector2.y, vector.z);
			GL.Vertex3(vector2.x, vector2.y, vector2.z);
			GL.Vertex3(vector2.x, vector.y, vector2.z);
			GL.Vertex3(vector.x, vector2.y, vector.z);
			GL.Vertex3(vector.x, vector.y, vector.z);
			GL.Vertex3(vector.x, vector.y, vector2.z);
			GL.Vertex3(vector.x, vector2.y, vector2.z);
			GL.Vertex3(vector2.x, vector2.y, vector2.z);
			GL.Vertex3(vector.x, vector2.y, vector2.z);
			GL.Vertex3(vector.x, vector.y, vector2.z);
			GL.Vertex3(vector2.x, vector.y, vector2.z);
			GL.Vertex3(vector.x, vector2.y, vector.z);
			GL.Vertex3(vector2.x, vector2.y, vector.z);
			GL.Vertex3(vector2.x, vector.y, vector.z);
			GL.Vertex3(vector.x, vector.y, vector.z);
			GL.End();
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0006E224 File Offset: 0x0006C624
		public static void DrawWireCube(Vector3 pos, Vector3 radius)
		{
			Vector3 vector = pos - radius;
			Vector3 vector2 = pos + radius;
			GL.Begin(1);
			GLUtil.StripFirstVertex3(vector.x, vector.y, vector.z);
			GLUtil.StripVertex3(vector2.x, vector.y, vector.z);
			GLUtil.StripVertex3(vector2.x, vector.y, vector2.z);
			GLUtil.StripVertex3(vector.x, vector.y, vector2.z);
			GLUtil.StripFirstVertex3(vector2.x, vector2.y, vector.z);
			GLUtil.StripVertex3(vector.x, vector2.y, vector.z);
			GLUtil.StripVertex3(vector.x, vector2.y, vector2.z);
			GLUtil.StripVertex3(vector2.x, vector2.y, vector2.z);
			GLUtil.StripFirstVertex3(vector2.x, vector.y, vector.z);
			GLUtil.StripVertex3(vector2.x, vector2.y, vector.z);
			GLUtil.StripVertex3(vector2.x, vector2.y, vector2.z);
			GLUtil.StripVertex3(vector2.x, vector.y, vector2.z);
			GLUtil.StripFirstVertex3(vector.x, vector2.y, vector.z);
			GLUtil.StripVertex3(vector.x, vector.y, vector.z);
			GLUtil.StripVertex3(vector.x, vector.y, vector2.z);
			GLUtil.StripVertex3(vector.x, vector2.y, vector2.z);
			GLUtil.StripFirstVertex3(vector2.x, vector2.y, vector2.z);
			GLUtil.StripVertex3(vector.x, vector2.y, vector2.z);
			GLUtil.StripVertex3(vector.x, vector.y, vector2.z);
			GLUtil.StripVertex3(vector2.x, vector.y, vector2.z);
			GLUtil.StripFirstVertex3(vector.x, vector2.y, vector.z);
			GLUtil.StripVertex3(vector2.x, vector2.y, vector.z);
			GLUtil.StripVertex3(vector2.x, vector.y, vector.z);
			GLUtil.StripVertex3(vector.x, vector.y, vector.z);
			GL.End();
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0006E4BC File Offset: 0x0006C8BC
		// Note: this type is marked as 'beforefieldinit'.
		static GLUtil()
		{
		}

		// Token: 0x040010BB RID: 4283
		private static Vector3 prevStripVertex = Vector3.zero;
	}
}
