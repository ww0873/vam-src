using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004B2 RID: 1202
	public class ColorPickerPresets : MonoBehaviour
	{
		// Token: 0x06001E6B RID: 7787 RVA: 0x000AD003 File Offset: 0x000AB403
		public ColorPickerPresets()
		{
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000AD00B File Offset: 0x000AB40B
		private void Awake()
		{
			this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000AD02C File Offset: 0x000AB42C
		public void CreatePresetButton()
		{
			for (int i = 0; i < this.presets.Length; i++)
			{
				if (!this.presets[i].activeSelf)
				{
					this.presets[i].SetActive(true);
					this.presets[i].GetComponent<Image>().color = this.picker.CurrentColor;
					break;
				}
			}
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000AD094 File Offset: 0x000AB494
		public void PresetSelect(Image sender)
		{
			this.picker.CurrentColor = sender.color;
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000AD0A7 File Offset: 0x000AB4A7
		private void ColorChanged(Color color)
		{
			this.createPresetImage.color = color;
		}

		// Token: 0x0400199D RID: 6557
		public ColorPickerControl picker;

		// Token: 0x0400199E RID: 6558
		public GameObject[] presets;

		// Token: 0x0400199F RID: 6559
		public Image createPresetImage;
	}
}
