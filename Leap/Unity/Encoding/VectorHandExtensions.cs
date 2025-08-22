using System;
using UnityEngine;

namespace Leap.Unity.Encoding
{
	// Token: 0x020006DD RID: 1757
	public static class VectorHandExtensions
	{
		// Token: 0x06002A44 RID: 10820 RVA: 0x000E45D5 File Offset: 0x000E29D5
		public static Bone GetBone(this Hand hand, int boneIdx)
		{
			return hand.Fingers[boneIdx / 4].bones[boneIdx % 4];
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000E45F0 File Offset: 0x000E29F0
		public static byte FloatToByte(float inFloat, float movementRange = 0.3f)
		{
			float num = Mathf.Clamp(inFloat, -movementRange / 2f, movementRange / 2f);
			num += movementRange / 2f;
			num /= movementRange;
			num *= 255f;
			num = Mathf.Floor(num);
			return (byte)num;
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000E4634 File Offset: 0x000E2A34
		public static float ByteToFloat(byte inByte, float movementRange = 0.3f)
		{
			float num = (float)inByte;
			num /= 255f;
			num *= movementRange;
			return num - movementRange / 2f;
		}
	}
}
