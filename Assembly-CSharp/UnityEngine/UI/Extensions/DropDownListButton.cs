using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004C7 RID: 1223
	[RequireComponent(typeof(RectTransform), typeof(Button))]
	public class DropDownListButton
	{
		// Token: 0x06001EDD RID: 7901 RVA: 0x000B026C File Offset: 0x000AE66C
		public DropDownListButton(GameObject btnObj)
		{
			this.gameobject = btnObj;
			this.rectTransform = btnObj.GetComponent<RectTransform>();
			this.btnImg = btnObj.GetComponent<Image>();
			this.btn = btnObj.GetComponent<Button>();
			this.txt = this.rectTransform.Find("Text").GetComponent<Text>();
			this.img = this.rectTransform.Find("Image").GetComponent<Image>();
		}

		// Token: 0x04001A14 RID: 6676
		public RectTransform rectTransform;

		// Token: 0x04001A15 RID: 6677
		public Button btn;

		// Token: 0x04001A16 RID: 6678
		public Text txt;

		// Token: 0x04001A17 RID: 6679
		public Image btnImg;

		// Token: 0x04001A18 RID: 6680
		public Image img;

		// Token: 0x04001A19 RID: 6681
		public GameObject gameobject;
	}
}
