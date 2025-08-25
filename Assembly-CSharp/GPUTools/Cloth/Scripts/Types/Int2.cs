using System;

namespace GPUTools.Cloth.Scripts.Types
{
	// Token: 0x020009AB RID: 2475
	[Serializable]
	public struct Int2
	{
		// Token: 0x06003E6B RID: 15979 RVA: 0x0012CA6C File Offset: 0x0012AE6C
		public Int2(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x0012CA7C File Offset: 0x0012AE7C
		public static int SizeOf()
		{
			return 8;
		}

		// Token: 0x04002FA7 RID: 12199
		public int X;

		// Token: 0x04002FA8 RID: 12200
		public int Y;
	}
}
