using System;
using UnityEngine;

namespace GPUTools.Painter.Scripts
{
	// Token: 0x02000A31 RID: 2609
	[Serializable]
	public class ColorBrush
	{
		// Token: 0x06004342 RID: 17218 RVA: 0x0013BF31 File Offset: 0x0013A331
		public ColorBrush()
		{
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06004343 RID: 17219 RVA: 0x0013BF69 File Offset: 0x0013A369
		public Color CurrentDrawColor
		{
			get
			{
				if (this.Channel == ColorChannel.R)
				{
					return Color.red;
				}
				if (this.Channel == ColorChannel.G)
				{
					return Color.green;
				}
				if (this.Channel == ColorChannel.B)
				{
					return Color.blue;
				}
				return Color.white;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06004344 RID: 17220 RVA: 0x0013BFA8 File Offset: 0x0013A3A8
		// (set) Token: 0x06004345 RID: 17221 RVA: 0x0013C008 File Offset: 0x0013A408
		public float CurrentChannelValue
		{
			get
			{
				if (this.Channel == ColorChannel.R)
				{
					return this.Color.r;
				}
				if (this.Channel == ColorChannel.G)
				{
					return this.Color.g;
				}
				if (this.Channel == ColorChannel.B)
				{
					return this.Color.b;
				}
				return this.Color.a;
			}
			set
			{
				if (this.Channel == ColorChannel.R)
				{
					this.Color.r = value;
				}
				if (this.Channel == ColorChannel.G)
				{
					this.Color.g = value;
				}
				if (this.Channel == ColorChannel.B)
				{
					this.Color.b = value;
				}
				if (this.Channel == ColorChannel.A)
				{
					this.Color.a = value;
				}
			}
		}

		// Token: 0x0400324D RID: 12877
		[SerializeField]
		public Color Color = new Color(0.95f, 0f, 0f);

		// Token: 0x0400324E RID: 12878
		[SerializeField]
		public float Radius = 0.02f;

		// Token: 0x0400324F RID: 12879
		[SerializeField]
		public float Strength = 1f;

		// Token: 0x04003250 RID: 12880
		[SerializeField]
		public ColorChannel Channel;
	}
}
