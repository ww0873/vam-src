using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE1 RID: 3553
public class SliderControl : MonoBehaviour
{
	// Token: 0x06006DE7 RID: 28135 RVA: 0x002946AE File Offset: 0x00292AAE
	public SliderControl()
	{
	}

	// Token: 0x06006DE8 RID: 28136 RVA: 0x002946BD File Offset: 0x00292ABD
	public void setSliderToMinimum()
	{
		if (this.slider != null)
		{
			this.slider.value = this.slider.minValue;
		}
	}

	// Token: 0x06006DE9 RID: 28137 RVA: 0x002946E6 File Offset: 0x00292AE6
	public void setSliderToMaximum()
	{
		if (this.slider != null)
		{
			this.slider.value = this.slider.maxValue;
		}
	}

	// Token: 0x06006DEA RID: 28138 RVA: 0x00294710 File Offset: 0x00292B10
	public void setSliderToValue(string value)
	{
		float sliderToValue;
		if (float.TryParse(value, out sliderToValue))
		{
			this.setSliderToValue(sliderToValue);
		}
	}

	// Token: 0x06006DEB RID: 28139 RVA: 0x00294734 File Offset: 0x00292B34
	public void setSliderToValue(float value)
	{
		if (this.slider != null)
		{
			if (!this.clamp)
			{
				if (value > this.slider.maxValue)
				{
					this.slider.maxValue = value;
				}
				if (value < this.slider.minValue)
				{
					this.slider.minValue = value;
				}
			}
			this.slider.value = value;
		}
	}

	// Token: 0x06006DEC RID: 28140 RVA: 0x002947A4 File Offset: 0x00292BA4
	public void incrementSlider(float value)
	{
		float sliderToValue = this.slider.value + value;
		this.setSliderToValue(sliderToValue);
	}

	// Token: 0x06006DED RID: 28141 RVA: 0x002947C8 File Offset: 0x00292BC8
	public void decrementSlider(float value)
	{
		float sliderToValue = this.slider.value - value;
		this.setSliderToValue(sliderToValue);
	}

	// Token: 0x06006DEE RID: 28142 RVA: 0x002947EA File Offset: 0x00292BEA
	public void setSliderToDefaultValue()
	{
		if (this.slider != null)
		{
			this.valueAdjustRange = this.defaultValue;
		}
	}

	// Token: 0x1700100F RID: 4111
	// (get) Token: 0x06006DEF RID: 28143 RVA: 0x00294809 File Offset: 0x00292C09
	// (set) Token: 0x06006DF0 RID: 28144 RVA: 0x00294830 File Offset: 0x00292C30
	public float valueAdjustRange
	{
		get
		{
			if (this.slider != null)
			{
				return this.slider.value;
			}
			return 0f;
		}
		set
		{
			if (this.slider != null)
			{
				if (value > this.slider.maxValue)
				{
					if (this.slider.maxValue > 0f)
					{
						while (value > this.slider.maxValue)
						{
							this.slider.maxValue *= 10f;
						}
					}
					else
					{
						this.slider.maxValue = value;
					}
				}
				if (value < this.slider.minValue)
				{
					this.slider.minValue = value;
				}
				this.slider.value = value;
			}
		}
	}

	// Token: 0x06006DF1 RID: 28145 RVA: 0x002948DB File Offset: 0x00292CDB
	public void multiplyMaxRange(float multiplier)
	{
		if (this.slider != null)
		{
			this.slider.maxValue *= multiplier;
		}
	}

	// Token: 0x06006DF2 RID: 28146 RVA: 0x00294904 File Offset: 0x00292D04
	public void multiplyRange(float multiplier)
	{
		if (this.slider != null)
		{
			if (this.slider.minValue == 0f)
			{
				this.multiplyMaxRange(multiplier);
			}
			else
			{
				float num = (this.slider.minValue + this.slider.maxValue) * 0.5f;
				float num2 = this.slider.maxValue - this.slider.minValue;
				float num3 = num2 * multiplier * 0.5f;
				float minValue = num - num3;
				float maxValue = num + num3;
				this.slider.minValue = minValue;
				this.slider.maxValue = maxValue;
			}
		}
	}

	// Token: 0x04005F2B RID: 24363
	public Slider slider;

	// Token: 0x04005F2C RID: 24364
	public bool clamp = true;

	// Token: 0x04005F2D RID: 24365
	public float defaultValue;

	// Token: 0x04005F2E RID: 24366
	public bool disableLookDrag;
}
