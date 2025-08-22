using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x0200068A RID: 1674
	public class QuickButtonAttribute : CombinablePropertyAttribute, IAfterFieldAdditiveDrawer, IAdditiveDrawer
	{
		// Token: 0x06002865 RID: 10341 RVA: 0x000DEB3D File Offset: 0x000DCF3D
		public QuickButtonAttribute(string buttonLabel, string methodOnPress, string tooltip = "")
		{
			this.label = buttonLabel;
			this.methodOnPress = methodOnPress;
			this.tooltip = tooltip;
		}

		// Token: 0x040021B9 RID: 8633
		public const float PADDING_RIGHT = 12f;

		// Token: 0x040021BA RID: 8634
		public readonly string label = "Quick Button";

		// Token: 0x040021BB RID: 8635
		public readonly string methodOnPress;

		// Token: 0x040021BC RID: 8636
		public readonly string tooltip = string.Empty;
	}
}
