using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x0200045E RID: 1118
	public class SimpleExample
	{
		// Token: 0x06001BF1 RID: 7153 RVA: 0x0009E5CC File Offset: 0x0009C9CC
		public SimpleExample()
		{
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0009E5D4 File Offset: 0x0009C9D4
		public static void main(string[] args)
		{
			Point3d[] points = new Point3d[]
			{
				new Point3d(0.0, 0.0, 0.0),
				new Point3d(1.0, 0.5, 0.0),
				new Point3d(2.0, 0.0, 0.0),
				new Point3d(0.5, 0.5, 0.5),
				new Point3d(0.0, 0.0, 2.0),
				new Point3d(0.1, 0.2, 0.3),
				new Point3d(0.0, 2.0, 0.0)
			};
			QuickHull3D quickHull3D = new QuickHull3D();
			quickHull3D.build(points);
		}
	}
}
