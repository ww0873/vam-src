using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004BC RID: 1212
	[RequireComponent(typeof(BoxSlider), typeof(RawImage))]
	[ExecuteInEditMode]
	public class SVBoxSlider : MonoBehaviour
	{
		// Token: 0x06001E97 RID: 7831 RVA: 0x000AE151 File Offset: 0x000AC551
		public SVBoxSlider()
		{
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x000AE16B File Offset: 0x000AC56B
		public RectTransform RectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x000AE178 File Offset: 0x000AC578
		private void Awake()
		{
			this.slider = base.GetComponent<BoxSlider>();
			this.image = base.GetComponent<RawImage>();
			this.RegenerateSVTexture();
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x000AE198 File Offset: 0x000AC598
		private void OnEnable()
		{
			if (Application.isPlaying && this.picker != null)
			{
				this.slider.OnValueChanged.AddListener(new UnityAction<float, float>(this.SliderChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x000AE1F8 File Offset: 0x000AC5F8
		private void OnDisable()
		{
			if (this.picker != null)
			{
				this.slider.OnValueChanged.RemoveListener(new UnityAction<float, float>(this.SliderChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000AE24E File Offset: 0x000AC64E
		private void OnDestroy()
		{
			if (this.image.texture != null)
			{
				Object.DestroyImmediate(this.image.texture);
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000AE276 File Offset: 0x000AC676
		private void SliderChanged(float saturation, float value)
		{
			if (this.listen)
			{
				this.picker.AssignColor(ColorValues.Saturation, saturation);
				this.picker.AssignColor(ColorValues.Value, value);
			}
			this.listen = true;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000AE2A4 File Offset: 0x000AC6A4
		private void HSVChanged(float h, float s, float v)
		{
			if (this.lastH != h)
			{
				this.lastH = h;
				this.RegenerateSVTexture();
			}
			if (s != this.slider.NormalizedValueX)
			{
				this.listen = false;
				this.slider.NormalizedValueX = s;
			}
			if (v != this.slider.NormalizedValueY)
			{
				this.listen = false;
				this.slider.NormalizedValueY = v;
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000AE314 File Offset: 0x000AC714
		private void RegenerateSVTexture()
		{
			double h = (double)((!(this.picker != null)) ? 0f : (this.picker.H * 360f));
			if (this.image.texture != null)
			{
				Object.DestroyImmediate(this.image.texture);
			}
			Texture2D texture2D = new Texture2D(100, 100)
			{
				hideFlags = HideFlags.DontSave
			};
			for (int i = 0; i < 100; i++)
			{
				Color32[] array = new Color32[100];
				for (int j = 0; j < 100; j++)
				{
					array[j] = HSVUtil.ConvertHsvToRgb(h, (double)((float)i / 100f), (double)((float)j / 100f), 1f);
				}
				texture2D.SetPixels32(i, 0, 1, 100, array);
			}
			texture2D.Apply();
			this.image.texture = texture2D;
		}

		// Token: 0x040019B9 RID: 6585
		public ColorPickerControl picker;

		// Token: 0x040019BA RID: 6586
		private BoxSlider slider;

		// Token: 0x040019BB RID: 6587
		private RawImage image;

		// Token: 0x040019BC RID: 6588
		private float lastH = -1f;

		// Token: 0x040019BD RID: 6589
		private bool listen = true;
	}
}
