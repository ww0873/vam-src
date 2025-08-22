using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x0200045B RID: 1115
	public class Point3d : Vector3d
	{
		// Token: 0x06001BBE RID: 7102 RVA: 0x0009CC5E File Offset: 0x0009B05E
		public Point3d()
		{
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x0009CC66 File Offset: 0x0009B066
		public Point3d(Vector3d v)
		{
			base.set(v);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0009CC75 File Offset: 0x0009B075
		public Point3d(double x, double y, double z)
		{
			base.set(x, y, z);
		}
	}
}
