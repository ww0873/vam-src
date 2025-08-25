using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020004A1 RID: 1185
	public class TestAddingPoints : MonoBehaviour
	{
		// Token: 0x06001DE9 RID: 7657 RVA: 0x000AB6EE File Offset: 0x000A9AEE
		public TestAddingPoints()
		{
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000AB6F8 File Offset: 0x000A9AF8
		public void AddNewPoint()
		{
			Vector2 item = new Vector2
			{
				x = float.Parse(this.XValue.text),
				y = float.Parse(this.YValue.text)
			};
			List<Vector2> list = new List<Vector2>(this.LineRenderer.Points);
			list.Add(item);
			this.LineRenderer.Points = list.ToArray();
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x000AB766 File Offset: 0x000A9B66
		public void ClearPoints()
		{
			this.LineRenderer.Points = new Vector2[0];
		}

		// Token: 0x0400194D RID: 6477
		public UILineRenderer LineRenderer;

		// Token: 0x0400194E RID: 6478
		public Text XValue;

		// Token: 0x0400194F RID: 6479
		public Text YValue;
	}
}
