using System;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004BB RID: 1211
	public struct HsvColor
	{
		// Token: 0x06001E8F RID: 7823 RVA: 0x000AE080 File Offset: 0x000AC480
		public HsvColor(double h, double s, double v)
		{
			this.H = h;
			this.S = s;
			this.V = v;
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x000AE097 File Offset: 0x000AC497
		// (set) Token: 0x06001E91 RID: 7825 RVA: 0x000AE0A6 File Offset: 0x000AC4A6
		public float NormalizedH
		{
			get
			{
				return (float)this.H / 360f;
			}
			set
			{
				this.H = (double)value * 360.0;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x000AE0BA File Offset: 0x000AC4BA
		// (set) Token: 0x06001E93 RID: 7827 RVA: 0x000AE0C3 File Offset: 0x000AC4C3
		public float NormalizedS
		{
			get
			{
				return (float)this.S;
			}
			set
			{
				this.S = (double)value;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x000AE0CD File Offset: 0x000AC4CD
		// (set) Token: 0x06001E95 RID: 7829 RVA: 0x000AE0D6 File Offset: 0x000AC4D6
		public float NormalizedV
		{
			get
			{
				return (float)this.V;
			}
			set
			{
				this.V = (double)value;
			}
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x000AE0E0 File Offset: 0x000AC4E0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{",
				this.H.ToString("f2"),
				",",
				this.S.ToString("f2"),
				",",
				this.V.ToString("f2"),
				"}"
			});
		}

		// Token: 0x040019B6 RID: 6582
		public double H;

		// Token: 0x040019B7 RID: 6583
		public double S;

		// Token: 0x040019B8 RID: 6584
		public double V;
	}
}
