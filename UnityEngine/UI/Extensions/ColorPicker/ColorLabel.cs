using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004B0 RID: 1200
	[RequireComponent(typeof(Text))]
	public class ColorLabel : MonoBehaviour
	{
		// Token: 0x06001E4C RID: 7756 RVA: 0x000ACAA2 File Offset: 0x000AAEA2
		public ColorLabel()
		{
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000ACAC0 File Offset: 0x000AAEC0
		private void Awake()
		{
			this.label = base.GetComponent<Text>();
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000ACAD0 File Offset: 0x000AAED0
		private void OnEnable()
		{
			if (Application.isPlaying && this.picker != null)
			{
				this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000ACB30 File Offset: 0x000AAF30
		private void OnDestroy()
		{
			if (this.picker != null)
			{
				this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000ACB86 File Offset: 0x000AAF86
		private void ColorChanged(Color color)
		{
			this.UpdateValue();
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000ACB8E File Offset: 0x000AAF8E
		private void HSVChanged(float hue, float sateration, float value)
		{
			this.UpdateValue();
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000ACB98 File Offset: 0x000AAF98
		private void UpdateValue()
		{
			if (this.picker == null)
			{
				this.label.text = this.prefix + "-";
			}
			else
			{
				float value = this.minValue + this.picker.GetValue(this.type) * (this.maxValue - this.minValue);
				this.label.text = this.prefix + this.ConvertToDisplayString(value);
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000ACC1C File Offset: 0x000AB01C
		private string ConvertToDisplayString(float value)
		{
			if (this.precision > 0)
			{
				return value.ToString("f " + this.precision);
			}
			return Mathf.FloorToInt(value).ToString();
		}

		// Token: 0x0400198D RID: 6541
		public ColorPickerControl picker;

		// Token: 0x0400198E RID: 6542
		public ColorValues type;

		// Token: 0x0400198F RID: 6543
		public string prefix = "R: ";

		// Token: 0x04001990 RID: 6544
		public float minValue;

		// Token: 0x04001991 RID: 6545
		public float maxValue = 255f;

		// Token: 0x04001992 RID: 6546
		public int precision;

		// Token: 0x04001993 RID: 6547
		private Text label;
	}
}
