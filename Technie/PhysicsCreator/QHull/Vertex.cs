using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x02000460 RID: 1120
	public class Vertex
	{
		// Token: 0x06001C0A RID: 7178 RVA: 0x0009E6EA File Offset: 0x0009CAEA
		public Vertex()
		{
			this.pnt = new Point3d();
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x0009E6FD File Offset: 0x0009CAFD
		public Vertex(double x, double y, double z, int idx)
		{
			this.pnt = new Point3d(x, y, z);
			this.index = idx;
		}

		// Token: 0x040017C6 RID: 6086
		public Point3d pnt;

		// Token: 0x040017C7 RID: 6087
		public int index;

		// Token: 0x040017C8 RID: 6088
		public Vertex prev;

		// Token: 0x040017C9 RID: 6089
		public Vertex next;

		// Token: 0x040017CA RID: 6090
		public Face face;
	}
}
