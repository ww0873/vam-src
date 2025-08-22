using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000693 RID: 1683
	public class DeltaQuaternionBuffer : DeltaBuffer<Quaternion, Vector3>
	{
		// Token: 0x06002888 RID: 10376 RVA: 0x000DEF1F File Offset: 0x000DD31F
		public DeltaQuaternionBuffer(int bufferSize) : base(bufferSize)
		{
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000DEF28 File Offset: 0x000DD328
		public override Vector3 Delta()
		{
			if (base.Count <= 1)
			{
				return Vector3.zero;
			}
			Vector3 a = Vector3.zero;
			for (int i = 0; i < base.Count - 1; i++)
			{
				DeltaBuffer<Quaternion, Vector3>.ValueTimePair valueTimePair = this._buffer.Get(i);
				DeltaBuffer<Quaternion, Vector3>.ValueTimePair valueTimePair2 = this._buffer.Get(i + 1);
				Quaternion value = valueTimePair.value;
				float time = valueTimePair.time;
				Quaternion value2 = valueTimePair2.value;
				float time2 = valueTimePair2.time;
				Vector3 a2 = value2.From(value).ToAngleAxisVector();
				float d = time2.From(time);
				a += a2 / d;
			}
			return a / (float)(base.Count - 1);
		}
	}
}
