using System;
using UnityEngine;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000679 RID: 1657
	public class CurveBoundsAttribute : CombinablePropertyAttribute, IFullPropertyDrawer
	{
		// Token: 0x0600284E RID: 10318 RVA: 0x000DE8A3 File Offset: 0x000DCCA3
		public CurveBoundsAttribute(Rect bounds)
		{
			this.bounds = bounds;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000DE8B2 File Offset: 0x000DCCB2
		public CurveBoundsAttribute(float width, float height)
		{
			this.bounds = new Rect(0f, 0f, width, height);
		}

		// Token: 0x040021A6 RID: 8614
		public readonly Rect bounds;
	}
}
