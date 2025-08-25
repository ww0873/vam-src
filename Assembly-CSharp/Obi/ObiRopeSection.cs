using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003C0 RID: 960
	public class ObiRopeSection : ScriptableObject
	{
		// Token: 0x0600187E RID: 6270 RVA: 0x0008ABDB File Offset: 0x00088FDB
		public ObiRopeSection()
		{
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x0008ABE3 File Offset: 0x00088FE3
		public int Segments
		{
			get
			{
				return this.vertices.Count - 1;
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0008ABF2 File Offset: 0x00088FF2
		public void OnEnable()
		{
			if (this.vertices == null)
			{
				this.vertices = new List<Vector2>();
				this.CirclePreset(8);
			}
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0008AC14 File Offset: 0x00089014
		public void CirclePreset(int segments)
		{
			this.vertices.Clear();
			for (int i = 0; i <= segments; i++)
			{
				float f = 6.2831855f / (float)segments * (float)i;
				this.vertices.Add(Mathf.Cos(f) * Vector2.right + Mathf.Sin(f) * Vector2.up);
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0008AC7C File Offset: 0x0008907C
		public static int SnapTo(float val, int snapInterval, int threshold)
		{
			int num = (int)val;
			if (snapInterval <= 0)
			{
				return num;
			}
			int num2 = Mathf.FloorToInt(val / (float)snapInterval) * snapInterval;
			int num3 = num2 + snapInterval;
			if (num - num2 < threshold)
			{
				return num2;
			}
			if (num3 - num < threshold)
			{
				return num3;
			}
			return num;
		}

		// Token: 0x040013DE RID: 5086
		[HideInInspector]
		public List<Vector2> vertices;

		// Token: 0x040013DF RID: 5087
		public int snapX;

		// Token: 0x040013E0 RID: 5088
		public int snapY;
	}
}
