using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200049D RID: 1181
	public class UpdateRadialValue : MonoBehaviour
	{
		// Token: 0x06001DD2 RID: 7634 RVA: 0x000AB271 File Offset: 0x000A9671
		public UpdateRadialValue()
		{
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x000AB279 File Offset: 0x000A9679
		private void Start()
		{
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x000AB27C File Offset: 0x000A967C
		public void UpdateSliderValue()
		{
			float value;
			float.TryParse(this.input.text, out value);
			this.slider.Value = value;
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x000AB2A8 File Offset: 0x000A96A8
		public void UpdateSliderAndle()
		{
			int num;
			int.TryParse(this.input.text, out num);
			this.slider.Angle = (float)num;
		}

		// Token: 0x0400192F RID: 6447
		public InputField input;

		// Token: 0x04001930 RID: 6448
		public RadialSlider slider;
	}
}
