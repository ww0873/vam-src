using System;

namespace Leap.Unity
{
	// Token: 0x02000692 RID: 1682
	public class DeltaFloatBuffer : DeltaBuffer<float, float>
	{
		// Token: 0x06002886 RID: 10374 RVA: 0x000DEEA7 File Offset: 0x000DD2A7
		public DeltaFloatBuffer(int bufferSize) : base(bufferSize)
		{
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000DEEB0 File Offset: 0x000DD2B0
		public override float Delta()
		{
			if (base.Count <= 1)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < base.Count - 1; i++)
			{
				num += (base.Get(i + 1) - base.Get(i)) / (base.GetTime(i + 1) - base.GetTime(i));
			}
			return num / (float)(base.Count - 1);
		}
	}
}
