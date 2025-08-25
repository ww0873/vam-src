using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200048A RID: 1162
	public class Example01ScrollView : FancyScrollView<Example01CellDto>
	{
		// Token: 0x06001D93 RID: 7571 RVA: 0x000AA82A File Offset: 0x000A8C2A
		public Example01ScrollView()
		{
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000AA832 File Offset: 0x000A8C32
		private new void Awake()
		{
			base.Awake();
			this.scrollPositionController.OnUpdatePosition.AddListener(new UnityAction<float>(base.UpdatePosition));
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000AA856 File Offset: 0x000A8C56
		public void UpdateData(List<Example01CellDto> data)
		{
			this.cellData = data;
			this.scrollPositionController.SetDataCount(this.cellData.Count);
			base.UpdateContents();
		}

		// Token: 0x04001909 RID: 6409
		[SerializeField]
		private ScrollPositionController scrollPositionController;
	}
}
