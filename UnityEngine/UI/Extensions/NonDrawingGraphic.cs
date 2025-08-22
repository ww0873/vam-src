using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200054A RID: 1354
	[AddComponentMenu("Layout/Extensions/NonDrawingGraphic")]
	public class NonDrawingGraphic : MaskableGraphic
	{
		// Token: 0x06002282 RID: 8834 RVA: 0x000C5230 File Offset: 0x000C3630
		public NonDrawingGraphic()
		{
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000C5238 File Offset: 0x000C3638
		public override void SetMaterialDirty()
		{
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000C523A File Offset: 0x000C363A
		public override void SetVerticesDirty()
		{
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000C523C File Offset: 0x000C363C
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}
