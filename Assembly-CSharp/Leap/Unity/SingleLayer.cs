using System;

namespace Leap.Unity
{
	// Token: 0x02000749 RID: 1865
	[Serializable]
	public struct SingleLayer : IEquatable<SingleLayer>
	{
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002D97 RID: 11671 RVA: 0x000F33DE File Offset: 0x000F17DE
		// (set) Token: 0x06002D98 RID: 11672 RVA: 0x000F33EC File Offset: 0x000F17EC
		public int layerMask
		{
			get
			{
				return 1 << this.layerIndex;
			}
			set
			{
				if (value == 0)
				{
					throw new ArgumentException("Single layer can only represent exactly one layer.  The provided mask represents no layers (mask was zero).");
				}
				int num = 0;
				while ((value & 1) == 0)
				{
					value >>= 1;
					num++;
				}
				if (value != 1)
				{
					throw new ArgumentException("Single layer can only represent exactly one layer.  The provided mask represents more than one layer.");
				}
				this.layerIndex = num;
			}
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000F343B File Offset: 0x000F183B
		public static implicit operator int(SingleLayer singleLayer)
		{
			return singleLayer.layerIndex;
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000F3444 File Offset: 0x000F1844
		public static implicit operator SingleLayer(int layerIndex)
		{
			return new SingleLayer
			{
				layerIndex = layerIndex
			};
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000F3462 File Offset: 0x000F1862
		public bool Equals(SingleLayer other)
		{
			return this.layerIndex == other.layerIndex;
		}

		// Token: 0x04002407 RID: 9223
		public int layerIndex;
	}
}
