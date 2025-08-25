using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000485 RID: 1157
	public class ComboBoxChanged : MonoBehaviour
	{
		// Token: 0x06001D82 RID: 7554 RVA: 0x000AA26A File Offset: 0x000A866A
		public ComboBoxChanged()
		{
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000AA272 File Offset: 0x000A8672
		public void ComboBoxChangedEvent(string text)
		{
			Debug.Log("ComboBox changed [" + text + "]");
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000AA289 File Offset: 0x000A8689
		public void AutoCompleteComboBoxChangedEvent(string text)
		{
			Debug.Log("AutoCompleteComboBox changed [" + text + "]");
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000AA2A0 File Offset: 0x000A86A0
		public void AutoCompleteComboBoxSelectionChangedEvent(string text, bool valid)
		{
			Debug.Log(string.Concat(new object[]
			{
				"AutoCompleteComboBox selection changed [",
				text,
				"] and its validity was [",
				valid,
				"]"
			}));
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000AA2D7 File Offset: 0x000A86D7
		public void DropDownChangedEvent(int newValue)
		{
			Debug.Log("DropDown changed [" + newValue + "]");
		}
	}
}
