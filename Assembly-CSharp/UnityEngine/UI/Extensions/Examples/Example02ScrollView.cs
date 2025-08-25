using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200048E RID: 1166
	public class Example02ScrollView : FancyScrollView<Example02CellDto, Example02ScrollViewContext>
	{
		// Token: 0x06001D9E RID: 7582 RVA: 0x000AA9CE File Offset: 0x000A8DCE
		public Example02ScrollView()
		{
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000AA9D8 File Offset: 0x000A8DD8
		private new void Awake()
		{
			this.scrollPositionController.OnUpdatePosition.AddListener(new UnityAction<float>(base.UpdatePosition));
			this.scrollPositionController.OnItemSelected.AddListener(new UnityAction<int>(this.CellSelected));
			base.SetContext(new Example02ScrollViewContext
			{
				OnPressedCell = new Action<Example02ScrollViewCell>(this.OnPressedCell)
			});
			base.Awake();
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000AAA42 File Offset: 0x000A8E42
		public void UpdateData(List<Example02CellDto> data)
		{
			this.cellData = data;
			this.scrollPositionController.SetDataCount(this.cellData.Count);
			base.UpdateContents();
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000AAA67 File Offset: 0x000A8E67
		private void OnPressedCell(Example02ScrollViewCell cell)
		{
			this.scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
			this.context.SelectedIndex = cell.DataIndex;
			base.UpdateContents();
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000AAA96 File Offset: 0x000A8E96
		private void CellSelected(int cellIndex)
		{
			this.context.SelectedIndex = cellIndex;
			base.UpdateContents();
		}

		// Token: 0x04001910 RID: 6416
		[SerializeField]
		private ScrollPositionController scrollPositionController;
	}
}
