using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000493 RID: 1171
	public class Example03ScrollView : FancyScrollView<Example03CellDto, Example03ScrollViewContext>
	{
		// Token: 0x06001DAE RID: 7598 RVA: 0x000AAC82 File Offset: 0x000A9082
		public Example03ScrollView()
		{
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000AAC8C File Offset: 0x000A908C
		private new void Awake()
		{
			this.scrollPositionController.OnUpdatePosition.AddListener(new UnityAction<float>(base.UpdatePosition));
			this.scrollPositionController.OnItemSelected.AddListener(new UnityAction<int>(this.CellSelected));
			base.SetContext(new Example03ScrollViewContext
			{
				OnPressedCell = new Action<Example03ScrollViewCell>(this.OnPressedCell)
			});
			base.Awake();
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000AACF6 File Offset: 0x000A90F6
		public void UpdateData(List<Example03CellDto> data)
		{
			this.cellData = data;
			this.scrollPositionController.SetDataCount(this.cellData.Count);
			base.UpdateContents();
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x000AAD1B File Offset: 0x000A911B
		private void OnPressedCell(Example03ScrollViewCell cell)
		{
			this.scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
			this.context.SelectedIndex = cell.DataIndex;
			base.UpdateContents();
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000AAD4A File Offset: 0x000A914A
		private void CellSelected(int cellIndex)
		{
			this.context.SelectedIndex = cellIndex;
			base.UpdateContents();
		}

		// Token: 0x0400191C RID: 6428
		[SerializeField]
		private ScrollPositionController scrollPositionController;
	}
}
