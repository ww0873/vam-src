using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004B4 RID: 1204
	[RequireComponent(typeof(Slider))]
	public class ColorSlider : MonoBehaviour
	{
		// Token: 0x06001E74 RID: 7796 RVA: 0x000AD0FC File Offset: 0x000AB4FC
		public ColorSlider()
		{
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000AD10C File Offset: 0x000AB50C
		private void Awake()
		{
			this.slider = base.GetComponent<Slider>();
			this.ColorPicker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.SliderChanged));
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000AD17C File Offset: 0x000AB57C
		private void OnDestroy()
		{
			this.ColorPicker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SliderChanged));
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000AD1E0 File Offset: 0x000AB5E0
		private void ColorChanged(Color newColor)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.R:
				this.slider.normalizedValue = newColor.r;
				break;
			case ColorValues.G:
				this.slider.normalizedValue = newColor.g;
				break;
			case ColorValues.B:
				this.slider.normalizedValue = newColor.b;
				break;
			case ColorValues.A:
				this.slider.normalizedValue = newColor.a;
				break;
			}
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000AD278 File Offset: 0x000AB678
		private void HSVChanged(float hue, float saturation, float value)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.Hue:
				this.slider.normalizedValue = hue;
				break;
			case ColorValues.Saturation:
				this.slider.normalizedValue = saturation;
				break;
			case ColorValues.Value:
				this.slider.normalizedValue = value;
				break;
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000AD2E4 File Offset: 0x000AB6E4
		private void SliderChanged(float newValue)
		{
			if (this.listen)
			{
				newValue = this.slider.normalizedValue;
				this.ColorPicker.AssignColor(this.type, newValue);
			}
			this.listen = true;
		}

		// Token: 0x040019A2 RID: 6562
		public ColorPickerControl ColorPicker;

		// Token: 0x040019A3 RID: 6563
		public ColorValues type;

		// Token: 0x040019A4 RID: 6564
		private Slider slider;

		// Token: 0x040019A5 RID: 6565
		private bool listen = true;
	}
}
