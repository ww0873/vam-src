using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004B9 RID: 1209
	[RequireComponent(typeof(InputField))]
	public class HexColorField : MonoBehaviour
	{
		// Token: 0x06001E85 RID: 7813 RVA: 0x000ADA0A File Offset: 0x000ABE0A
		public HexColorField()
		{
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x000ADA14 File Offset: 0x000ABE14
		private void Awake()
		{
			this.hexInputField = base.GetComponent<InputField>();
			this.hexInputField.onEndEdit.AddListener(new UnityAction<string>(this.UpdateColor));
			this.ColorPicker.onValueChanged.AddListener(new UnityAction<Color>(this.UpdateHex));
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000ADA65 File Offset: 0x000ABE65
		private void OnDestroy()
		{
			this.hexInputField.onValueChanged.RemoveListener(new UnityAction<string>(this.UpdateColor));
			this.ColorPicker.onValueChanged.RemoveListener(new UnityAction<Color>(this.UpdateHex));
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000ADA9F File Offset: 0x000ABE9F
		private void UpdateHex(Color newColor)
		{
			this.hexInputField.text = this.ColorToHex(newColor);
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x000ADAB8 File Offset: 0x000ABEB8
		private void UpdateColor(string newHex)
		{
			Color32 c;
			if (HexColorField.HexToColor(newHex, out c))
			{
				this.ColorPicker.CurrentColor = c;
			}
			else
			{
				Debug.Log("hex value is in the wrong format, valid formats are: #RGB, #RGBA, #RRGGBB and #RRGGBBAA (# is optional)");
			}
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x000ADAF4 File Offset: 0x000ABEF4
		private string ColorToHex(Color32 color)
		{
			if (this.displayAlpha)
			{
				return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
				{
					color.r,
					color.g,
					color.b,
					color.a
				});
			}
			return string.Format("#{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x000ADB88 File Offset: 0x000ABF88
		public static bool HexToColor(string hex, out Color32 color)
		{
			if (Regex.IsMatch(hex, "^#?(?:[0-9a-fA-F]{3,4}){1,2}$"))
			{
				int num = (!hex.StartsWith("#")) ? 0 : 1;
				if (hex.Length == num + 8)
				{
					color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 6, 2), NumberStyles.AllowHexSpecifier));
				}
				else if (hex.Length == num + 6)
				{
					color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.MaxValue);
				}
				else if (hex.Length == num + 4)
				{
					color = new Color32(byte.Parse(string.Empty + hex[num] + hex[num], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 1] + hex[num + 1], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 2] + hex[num + 2], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 3] + hex[num + 3], NumberStyles.AllowHexSpecifier));
				}
				else
				{
					color = new Color32(byte.Parse(string.Empty + hex[num] + hex[num], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 1] + hex[num + 1], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 2] + hex[num + 2], NumberStyles.AllowHexSpecifier), byte.MaxValue);
				}
				return true;
			}
			color = default(Color32);
			return false;
		}

		// Token: 0x040019B2 RID: 6578
		public ColorPickerControl ColorPicker;

		// Token: 0x040019B3 RID: 6579
		public bool displayAlpha;

		// Token: 0x040019B4 RID: 6580
		private InputField hexInputField;

		// Token: 0x040019B5 RID: 6581
		private const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";
	}
}
