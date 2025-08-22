using System;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004B1 RID: 1201
	public class ColorPickerControl : MonoBehaviour
	{
		// Token: 0x06001E54 RID: 7764 RVA: 0x000ACC66 File Offset: 0x000AB066
		public ColorPickerControl()
		{
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x000ACC8F File Offset: 0x000AB08F
		// (set) Token: 0x06001E56 RID: 7766 RVA: 0x000ACCB0 File Offset: 0x000AB0B0
		public Color CurrentColor
		{
			get
			{
				return new Color(this._red, this._green, this._blue, this._alpha);
			}
			set
			{
				if (this.CurrentColor == value)
				{
					return;
				}
				this._red = value.r;
				this._green = value.g;
				this._blue = value.b;
				this._alpha = value.a;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000ACD0F File Offset: 0x000AB10F
		private void Start()
		{
			this.SendChangedEvent();
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x000ACD17 File Offset: 0x000AB117
		// (set) Token: 0x06001E59 RID: 7769 RVA: 0x000ACD1F File Offset: 0x000AB11F
		public float H
		{
			get
			{
				return this._hue;
			}
			set
			{
				if (this._hue == value)
				{
					return;
				}
				this._hue = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x000ACD41 File Offset: 0x000AB141
		// (set) Token: 0x06001E5B RID: 7771 RVA: 0x000ACD49 File Offset: 0x000AB149
		public float S
		{
			get
			{
				return this._saturation;
			}
			set
			{
				if (this._saturation == value)
				{
					return;
				}
				this._saturation = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x000ACD6B File Offset: 0x000AB16B
		// (set) Token: 0x06001E5D RID: 7773 RVA: 0x000ACD73 File Offset: 0x000AB173
		public float V
		{
			get
			{
				return this._brightness;
			}
			set
			{
				if (this._brightness == value)
				{
					return;
				}
				this._brightness = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x000ACD95 File Offset: 0x000AB195
		// (set) Token: 0x06001E5F RID: 7775 RVA: 0x000ACD9D File Offset: 0x000AB19D
		public float R
		{
			get
			{
				return this._red;
			}
			set
			{
				if (this._red == value)
				{
					return;
				}
				this._red = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001E60 RID: 7776 RVA: 0x000ACDBF File Offset: 0x000AB1BF
		// (set) Token: 0x06001E61 RID: 7777 RVA: 0x000ACDC7 File Offset: 0x000AB1C7
		public float G
		{
			get
			{
				return this._green;
			}
			set
			{
				if (this._green == value)
				{
					return;
				}
				this._green = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x000ACDE9 File Offset: 0x000AB1E9
		// (set) Token: 0x06001E63 RID: 7779 RVA: 0x000ACDF1 File Offset: 0x000AB1F1
		public float B
		{
			get
			{
				return this._blue;
			}
			set
			{
				if (this._blue == value)
				{
					return;
				}
				this._blue = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x000ACE13 File Offset: 0x000AB213
		// (set) Token: 0x06001E65 RID: 7781 RVA: 0x000ACE1B File Offset: 0x000AB21B
		private float A
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (this._alpha == value)
				{
					return;
				}
				this._alpha = value;
				this.SendChangedEvent();
			}
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000ACE38 File Offset: 0x000AB238
		private void RGBChanged()
		{
			HsvColor hsvColor = HSVUtil.ConvertRgbToHsv(this.CurrentColor);
			this._hue = hsvColor.NormalizedH;
			this._saturation = hsvColor.NormalizedS;
			this._brightness = hsvColor.NormalizedV;
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x000ACE78 File Offset: 0x000AB278
		private void HSVChanged()
		{
			Color color = HSVUtil.ConvertHsvToRgb((double)(this._hue * 360f), (double)this._saturation, (double)this._brightness, this._alpha);
			this._red = color.r;
			this._green = color.g;
			this._blue = color.b;
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000ACED3 File Offset: 0x000AB2D3
		private void SendChangedEvent()
		{
			this.onValueChanged.Invoke(this.CurrentColor);
			this.onHSVChanged.Invoke(this._hue, this._saturation, this._brightness);
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000ACF04 File Offset: 0x000AB304
		public void AssignColor(ColorValues type, float value)
		{
			switch (type)
			{
			case ColorValues.R:
				this.R = value;
				break;
			case ColorValues.G:
				this.G = value;
				break;
			case ColorValues.B:
				this.B = value;
				break;
			case ColorValues.A:
				this.A = value;
				break;
			case ColorValues.Hue:
				this.H = value;
				break;
			case ColorValues.Saturation:
				this.S = value;
				break;
			case ColorValues.Value:
				this.V = value;
				break;
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000ACF94 File Offset: 0x000AB394
		public float GetValue(ColorValues type)
		{
			switch (type)
			{
			case ColorValues.R:
				return this.R;
			case ColorValues.G:
				return this.G;
			case ColorValues.B:
				return this.B;
			case ColorValues.A:
				return this.A;
			case ColorValues.Hue:
				return this.H;
			case ColorValues.Saturation:
				return this.S;
			case ColorValues.Value:
				return this.V;
			default:
				throw new NotImplementedException(string.Empty);
			}
		}

		// Token: 0x04001994 RID: 6548
		private float _hue;

		// Token: 0x04001995 RID: 6549
		private float _saturation;

		// Token: 0x04001996 RID: 6550
		private float _brightness;

		// Token: 0x04001997 RID: 6551
		private float _red;

		// Token: 0x04001998 RID: 6552
		private float _green;

		// Token: 0x04001999 RID: 6553
		private float _blue;

		// Token: 0x0400199A RID: 6554
		private float _alpha = 1f;

		// Token: 0x0400199B RID: 6555
		public ColorChangedEvent onValueChanged = new ColorChangedEvent();

		// Token: 0x0400199C RID: 6556
		public HSVChangedEvent onHSVChanged = new HSVChangedEvent();
	}
}
