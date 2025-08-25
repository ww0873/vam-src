using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
	// Token: 0x02000A27 RID: 2599
	[Serializable]
	public class RootTipColorProvider : IColorProvider
	{
		// Token: 0x06004329 RID: 17193 RVA: 0x0013B598 File Offset: 0x00139998
		public RootTipColorProvider()
		{
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x0013B609 File Offset: 0x00139A09
		public Color GetColor(HairSettings settings, int x, int y, int sizeY)
		{
			return this.GetStandColor((float)y / (float)sizeY);
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0013B617 File Offset: 0x00139A17
		public Color GetColor(HairSettings settings, float y)
		{
			return this.GetStandColor(y);
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x0013B620 File Offset: 0x00139A20
		private Color GetStandColor(float t)
		{
			float p = Mathf.Pow(2f, this.ColorRolloff) - 1f;
			float f = 1f - t;
			float t2 = Mathf.Pow(f, p);
			return Color.Lerp(this.TipColor, this.RootColor, t2);
		}

		// Token: 0x040031DB RID: 12763
		public Color RootColor = new Color(0.35f, 0.15f, 0.15f);

		// Token: 0x040031DC RID: 12764
		public Color TipColor = new Color(0.15f, 0.05f, 0.05f);

		// Token: 0x040031DD RID: 12765
		public AnimationCurve Blend = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040031DE RID: 12766
		public float ColorRolloff = 1f;
	}
}
