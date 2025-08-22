using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004AF RID: 1199
	[RequireComponent(typeof(Image))]
	public class ColorImage : MonoBehaviour
	{
		// Token: 0x06001E48 RID: 7752 RVA: 0x000ACA44 File Offset: 0x000AAE44
		public ColorImage()
		{
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000ACA4C File Offset: 0x000AAE4C
		private void Awake()
		{
			this.image = base.GetComponent<Image>();
			this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000ACA76 File Offset: 0x000AAE76
		private void OnDestroy()
		{
			this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000ACA94 File Offset: 0x000AAE94
		private void ColorChanged(Color newColor)
		{
			this.image.color = newColor;
		}

		// Token: 0x0400198B RID: 6539
		public ColorPickerControl picker;

		// Token: 0x0400198C RID: 6540
		private Image image;
	}
}
