using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000691 RID: 1681
	public class DeltaBuffer : DeltaBuffer<Vector3, Vector3>
	{
		// Token: 0x06002884 RID: 10372 RVA: 0x000DEE1C File Offset: 0x000DD21C
		public DeltaBuffer(int bufferSize) : base(bufferSize)
		{
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000DEE28 File Offset: 0x000DD228
		public override Vector3 Delta()
		{
			if (base.Count <= 1)
			{
				return Vector3.zero;
			}
			Vector3 a = Vector3.zero;
			for (int i = 0; i < base.Count - 1; i++)
			{
				a += (base.Get(i + 1) - base.Get(i)) / (base.GetTime(i + 1) - base.GetTime(i));
			}
			return a / (float)(base.Count - 1);
		}
	}
}
