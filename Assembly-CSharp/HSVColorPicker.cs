using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DC2 RID: 3522
[ExecuteInEditMode]
public class HSVColorPicker : TwoDPicker
{
	// Token: 0x06006D19 RID: 27929 RVA: 0x00291ACE File Offset: 0x0028FECE
	public HSVColorPicker()
	{
	}

	// Token: 0x06006D1A RID: 27930 RVA: 0x00291AD8 File Offset: 0x0028FED8
	public void SetHSV(HSVColor hc, bool noCallback = false)
	{
		if (this._hue != hc.H || this._xVal != hc.S || this._yVal != hc.V)
		{
			this.SetHueVal(hc.H);
			base.xVal = hc.S;
			base.yVal = hc.V;
			this.RegenerateSVImage();
			this.ignoreRGBCallbacks = true;
			this.RecalcRGB(noCallback);
			this.ignoreRGBCallbacks = false;
		}
	}

	// Token: 0x06006D1B RID: 27931 RVA: 0x00291B60 File Offset: 0x0028FF60
	public void SetHSV(float h, float s, float v, bool noCallback = false)
	{
		if (this._hue != h || this._xVal != s || this._yVal != v)
		{
			this.SetHueVal(h);
			base.xVal = s;
			base.yVal = v;
			this.RegenerateSVImage();
			this.ignoreRGBCallbacks = true;
			this.RecalcRGB(noCallback);
			this.ignoreRGBCallbacks = false;
		}
	}

	// Token: 0x06006D1C RID: 27932 RVA: 0x00291BC4 File Offset: 0x0028FFC4
	private void SetHueVal(float h)
	{
		this._hue = h;
		this._hue = Mathf.Clamp01(this._hue);
		this._hueInt = Mathf.FloorToInt(this._hue * 255f);
		if (this.hueSlider != null)
		{
			this.hueSlider.value = this._hue;
		}
	}

	// Token: 0x06006D1D RID: 27933 RVA: 0x00291C22 File Offset: 0x00290022
	private void SetHue(float h, bool noCallback = false)
	{
		this.ignoreRGBCallbacks = true;
		this.SetHueVal(h);
		this.RegenerateSVImage();
		this.RecalcRGB(noCallback);
		this.ignoreRGBCallbacks = false;
	}

	// Token: 0x17000FF6 RID: 4086
	// (get) Token: 0x06006D1E RID: 27934 RVA: 0x00291C46 File Offset: 0x00290046
	// (set) Token: 0x06006D1F RID: 27935 RVA: 0x00291C4E File Offset: 0x0029004E
	public float hue
	{
		get
		{
			return this._hue;
		}
		set
		{
			if (this._hue != value)
			{
				this.SetHue(value, false);
			}
		}
	}

	// Token: 0x17000FF7 RID: 4087
	// (get) Token: 0x06006D20 RID: 27936 RVA: 0x00291C64 File Offset: 0x00290064
	// (set) Token: 0x06006D21 RID: 27937 RVA: 0x00291C6C File Offset: 0x0029006C
	public float hueNoCallback
	{
		get
		{
			return this._hue;
		}
		set
		{
			if (this._hue != value)
			{
				this.SetHue(value, true);
			}
		}
	}

	// Token: 0x06006D22 RID: 27938 RVA: 0x00291C82 File Offset: 0x00290082
	private void SetSaturation(float s, bool noCallback = false)
	{
		this.ignoreRGBCallbacks = true;
		base.xVal = s;
		this.RecalcRGB(noCallback);
		this.ignoreRGBCallbacks = false;
	}

	// Token: 0x17000FF8 RID: 4088
	// (get) Token: 0x06006D24 RID: 27940 RVA: 0x00291CB6 File Offset: 0x002900B6
	// (set) Token: 0x06006D23 RID: 27939 RVA: 0x00291CA0 File Offset: 0x002900A0
	public float saturation
	{
		get
		{
			return this._xVal;
		}
		set
		{
			if (base.xVal != value)
			{
				this.SetSaturation(value, false);
			}
		}
	}

	// Token: 0x17000FF9 RID: 4089
	// (get) Token: 0x06006D26 RID: 27942 RVA: 0x00291CD4 File Offset: 0x002900D4
	// (set) Token: 0x06006D25 RID: 27941 RVA: 0x00291CBE File Offset: 0x002900BE
	public float saturationNoCallback
	{
		get
		{
			return this._xVal;
		}
		set
		{
			if (base.xVal != value)
			{
				this.SetSaturation(value, true);
			}
		}
	}

	// Token: 0x06006D27 RID: 27943 RVA: 0x00291CDC File Offset: 0x002900DC
	private void SetCValue(float cv, bool noCallback = false)
	{
		this.ignoreRGBCallbacks = true;
		base.yVal = cv;
		this.RecalcRGB(noCallback);
		this.ignoreRGBCallbacks = false;
	}

	// Token: 0x17000FFA RID: 4090
	// (get) Token: 0x06006D29 RID: 27945 RVA: 0x00291D10 File Offset: 0x00290110
	// (set) Token: 0x06006D28 RID: 27944 RVA: 0x00291CFA File Offset: 0x002900FA
	public float cvalue
	{
		get
		{
			return this._yVal;
		}
		set
		{
			if (base.yVal != value)
			{
				this.SetCValue(value, false);
			}
		}
	}

	// Token: 0x17000FFB RID: 4091
	// (get) Token: 0x06006D2B RID: 27947 RVA: 0x00291D2E File Offset: 0x0029012E
	// (set) Token: 0x06006D2A RID: 27946 RVA: 0x00291D18 File Offset: 0x00290118
	public float cvalueNoCallback
	{
		get
		{
			return this._yVal;
		}
		set
		{
			if (base.yVal != value)
			{
				this.SetCValue(value, true);
			}
		}
	}

	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x06006D2D RID: 27949 RVA: 0x00291D62 File Offset: 0x00290162
	// (set) Token: 0x06006D2C RID: 27948 RVA: 0x00291D36 File Offset: 0x00290136
	public float red
	{
		get
		{
			return this._red;
		}
		set
		{
			if (this._red != value)
			{
				this._red = value;
				this._red = Mathf.Clamp01(this._red);
				this.RecalcHSV();
			}
		}
	}

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x06006D2F RID: 27951 RVA: 0x00291D91 File Offset: 0x00290191
	// (set) Token: 0x06006D2E RID: 27950 RVA: 0x00291D6A File Offset: 0x0029016A
	public int red255
	{
		get
		{
			return Mathf.FloorToInt(this._red * 255f);
		}
		set
		{
			if (Mathf.FloorToInt(this._red * 255f) != value)
			{
				this.red = (float)value / 255f;
			}
		}
	}

	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x06006D31 RID: 27953 RVA: 0x00291DC8 File Offset: 0x002901C8
	// (set) Token: 0x06006D30 RID: 27952 RVA: 0x00291DA4 File Offset: 0x002901A4
	public string red255string
	{
		get
		{
			return this.red255.ToString();
		}
		set
		{
			int red;
			if (int.TryParse(value, out red))
			{
				this.red255 = red;
			}
		}
	}

	// Token: 0x06006D32 RID: 27954 RVA: 0x00291DE9 File Offset: 0x002901E9
	public void SetRedFromSlider255(float r)
	{
		this.skipRGBSliderSet = true;
		this.red = r / 255f;
		this.skipRGBSliderSet = false;
	}

	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x06006D34 RID: 27956 RVA: 0x00291E32 File Offset: 0x00290232
	// (set) Token: 0x06006D33 RID: 27955 RVA: 0x00291E06 File Offset: 0x00290206
	public float green
	{
		get
		{
			return this._green;
		}
		set
		{
			if (this._green != value)
			{
				this._green = value;
				this._green = Mathf.Clamp01(this._green);
				this.RecalcHSV();
			}
		}
	}

	// Token: 0x17001000 RID: 4096
	// (get) Token: 0x06006D36 RID: 27958 RVA: 0x00291E61 File Offset: 0x00290261
	// (set) Token: 0x06006D35 RID: 27957 RVA: 0x00291E3A File Offset: 0x0029023A
	public int green255
	{
		get
		{
			return Mathf.FloorToInt(this._green * 255f);
		}
		set
		{
			if (Mathf.FloorToInt(this._green * 255f) != value)
			{
				this.green = (float)value / 255f;
			}
		}
	}

	// Token: 0x17001001 RID: 4097
	// (get) Token: 0x06006D38 RID: 27960 RVA: 0x00291E98 File Offset: 0x00290298
	// (set) Token: 0x06006D37 RID: 27959 RVA: 0x00291E74 File Offset: 0x00290274
	public string green255string
	{
		get
		{
			return this.green255.ToString();
		}
		set
		{
			int green;
			if (int.TryParse(value, out green))
			{
				this.green255 = green;
			}
		}
	}

	// Token: 0x06006D39 RID: 27961 RVA: 0x00291EB9 File Offset: 0x002902B9
	public void SetGreenFromSlider255(float g)
	{
		this.skipRGBSliderSet = true;
		this.green = g / 255f;
		this.skipRGBSliderSet = false;
	}

	// Token: 0x17001002 RID: 4098
	// (get) Token: 0x06006D3B RID: 27963 RVA: 0x00291F02 File Offset: 0x00290302
	// (set) Token: 0x06006D3A RID: 27962 RVA: 0x00291ED6 File Offset: 0x002902D6
	public float blue
	{
		get
		{
			return this._blue;
		}
		set
		{
			if (this._blue != value)
			{
				this._blue = value;
				this._blue = Mathf.Clamp01(this._blue);
				this.RecalcHSV();
			}
		}
	}

	// Token: 0x17001003 RID: 4099
	// (get) Token: 0x06006D3D RID: 27965 RVA: 0x00291F31 File Offset: 0x00290331
	// (set) Token: 0x06006D3C RID: 27964 RVA: 0x00291F0A File Offset: 0x0029030A
	public int blue255
	{
		get
		{
			return Mathf.FloorToInt(this._blue * 255f);
		}
		set
		{
			if (Mathf.FloorToInt(this._blue * 255f) != value)
			{
				this.blue = (float)value / 255f;
			}
		}
	}

	// Token: 0x17001004 RID: 4100
	// (get) Token: 0x06006D3F RID: 27967 RVA: 0x00291F68 File Offset: 0x00290368
	// (set) Token: 0x06006D3E RID: 27966 RVA: 0x00291F44 File Offset: 0x00290344
	public string blue255string
	{
		get
		{
			return this.blue255.ToString();
		}
		set
		{
			int blue;
			if (int.TryParse(value, out blue))
			{
				this.blue255 = blue;
			}
		}
	}

	// Token: 0x06006D40 RID: 27968 RVA: 0x00291F89 File Offset: 0x00290389
	public void SetBlueFromSlider255(float b)
	{
		this.skipRGBSliderSet = true;
		this.blue = b / 255f;
		this.skipRGBSliderSet = false;
	}

	// Token: 0x06006D41 RID: 27969 RVA: 0x00291FA6 File Offset: 0x002903A6
	public void CopyToClipboard()
	{
		HSVColorPicker.clipboardColor = this.currentHSVColor;
	}

	// Token: 0x06006D42 RID: 27970 RVA: 0x00291FB3 File Offset: 0x002903B3
	public void PasteFromClipboard()
	{
		this.SetHSV(HSVColorPicker.clipboardColor, false);
	}

	// Token: 0x06006D43 RID: 27971 RVA: 0x00291FC4 File Offset: 0x002903C4
	private void SetCurrentColorFromRGB()
	{
		this.currentColor = new Color(this._red, this._green, this._blue);
		if (this.colorSample != null)
		{
			this.colorSample.color = this.currentColor;
		}
		if (this.colorObject != null)
		{
			foreach (Component component in this.colorObject.GetComponents<Component>())
			{
				Type type = component.GetType();
				PropertyInfo property = type.GetProperty("color");
				if (property != null)
				{
					property.SetValue(component, this.currentColor, null);
				}
			}
		}
	}

	// Token: 0x06006D44 RID: 27972 RVA: 0x00292074 File Offset: 0x00290474
	private void SetRGBSliders()
	{
		if (!this.skipRGBSliderSet)
		{
			if (this.redSlider != null)
			{
				this.redSlider.value = this._red * 255f;
			}
			if (this.greenSlider != null)
			{
				this.greenSlider.value = this._green * 255f;
			}
			if (this.blueSlider != null)
			{
				this.blueSlider.value = this._blue * 255f;
			}
		}
	}

	// Token: 0x06006D45 RID: 27973 RVA: 0x00292104 File Offset: 0x00290504
	private void RecalcRGB(bool noCallback = false)
	{
		this.currentHSVColor.H = this.hue;
		this.currentHSVColor.S = this.saturation;
		this.currentHSVColor.V = this.cvalue;
		Color color = HSVColorPicker.HSVToRGB(this.hue, this.saturation, this.cvalue);
		this._red = color.r;
		this._green = color.g;
		this._blue = color.b;
		this.SetRGBSliders();
		this.SetCurrentColorFromRGB();
		if (!noCallback)
		{
			if (this.onColorChangedHandlers != null)
			{
				this.onColorChangedHandlers(color);
			}
			if (this.onHSVColorChangedHandlers != null)
			{
				this.onHSVColorChangedHandlers(this.hue, this.saturation, this.cvalue);
			}
		}
	}

	// Token: 0x06006D46 RID: 27974 RVA: 0x002921D4 File Offset: 0x002905D4
	private void RecalcHSV()
	{
		if (!this.ignoreRGBCallbacks)
		{
			HSVColor hsvcolor = HSVColorPicker.RGBToHSV(this._red, this._green, this._blue);
			this.hue = hsvcolor.H;
			this.saturation = hsvcolor.S;
			this.cvalue = hsvcolor.V;
		}
	}

	// Token: 0x06006D47 RID: 27975 RVA: 0x0029222C File Offset: 0x0029062C
	public static HSVColor RGBToHSV(float r, float g, float b)
	{
		HSVColor result = default(HSVColor);
		float num = Mathf.Min(r, b);
		num = Mathf.Min(num, g);
		float num2 = Mathf.Max(r, b);
		num2 = Mathf.Max(num2, g);
		result.V = num2;
		float num3 = num2 - num;
		if (num2 != 0f)
		{
			result.S = num3 / num2;
			if (num3 == 0f)
			{
				result.H = 0f;
			}
			else if (r == num2)
			{
				result.H = (g - b) / num3;
			}
			else if (g == num2)
			{
				result.H = 2f + (b - r) / num3;
			}
			else
			{
				result.H = 4f + (r - g) / num3;
			}
			result.H /= 6f;
			if (result.H < 0f)
			{
				result.H += 1f;
			}
			return result;
		}
		result.S = 0f;
		result.H = 0f;
		return result;
	}

	// Token: 0x06006D48 RID: 27976 RVA: 0x0029233D File Offset: 0x0029073D
	public static Color HSVToRGB(HSVColor hsvColor)
	{
		return HSVColorPicker.HSVToRGB(hsvColor.H, hsvColor.S, hsvColor.V);
	}

	// Token: 0x06006D49 RID: 27977 RVA: 0x0029235C File Offset: 0x0029075C
	public static Color HSVToRGB(float H, float S, float V)
	{
		Color white = Color.white;
		if (S == 0f)
		{
			white.r = V;
			white.g = V;
			white.b = V;
		}
		else if (V == 0f)
		{
			white.r = 0f;
			white.g = 0f;
			white.b = 0f;
		}
		else
		{
			white.r = 0f;
			white.g = 0f;
			white.b = 0f;
			float num = H * 6f;
			int num2 = (int)Mathf.Floor(num);
			float num3 = num - (float)num2;
			float num4 = V * (1f - S);
			float num5 = V * (1f - S * num3);
			float num6 = V * (1f - S * (1f - num3));
			int num7 = num2;
			switch (num7 + 1)
			{
			case 0:
				white.r = V;
				white.g = num4;
				white.b = num5;
				break;
			case 1:
				white.r = V;
				white.g = num6;
				white.b = num4;
				break;
			case 2:
				white.r = num5;
				white.g = V;
				white.b = num4;
				break;
			case 3:
				white.r = num4;
				white.g = V;
				white.b = num6;
				break;
			case 4:
				white.r = num4;
				white.g = num5;
				white.b = V;
				break;
			case 5:
				white.r = num6;
				white.g = num4;
				white.b = V;
				break;
			case 6:
				white.r = V;
				white.g = num4;
				white.b = num5;
				break;
			case 7:
				white.r = V;
				white.g = num6;
				white.b = num4;
				break;
			}
			white.r = Mathf.Clamp(white.r, 0f, 1f);
			white.g = Mathf.Clamp(white.g, 0f, 1f);
			white.b = Mathf.Clamp(white.b, 0f, 1f);
		}
		return white;
	}

	// Token: 0x06006D4A RID: 27978 RVA: 0x002925B4 File Offset: 0x002909B4
	public void RegenerateSVImage()
	{
		if (HSVColorPicker.cachedSprites == null)
		{
			HSVColorPicker.cachedSprites = new Dictionary<int, Sprite>();
		}
		Sprite sprite;
		if (!HSVColorPicker.cachedSprites.TryGetValue(this._hueInt, out sprite))
		{
			Texture2D texture2D = new Texture2D(256, 256);
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < 256; j++)
				{
					texture2D.SetPixel(i, j, HSVColorPicker.HSVToRGB(this._hue, (float)i / 255f, (float)j / 255f));
				}
			}
			texture2D.Apply();
			Rect rect = new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height);
			sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));
			HSVColorPicker.cachedSprites.Add(this._hueInt, sprite);
		}
		if (this.saturationImage == null)
		{
			this.saturationImage = base.GetComponent<Image>();
		}
		if (this.saturationImage != null)
		{
			this.saturationImage.sprite = sprite;
			this.saturationImage.color = Color.white;
			this.saturationImage.type = Image.Type.Simple;
		}
	}

	// Token: 0x06006D4B RID: 27979 RVA: 0x002926F0 File Offset: 0x00290AF0
	private void RegenerateHueImage()
	{
		if (this.hueImage != null)
		{
			Texture2D texture2D = new Texture2D(1, 256);
			for (int i = 0; i < 256; i++)
			{
				texture2D.SetPixel(0, i, HSVColorPicker.HSVToRGB((float)i / 255f, 1f, 1f));
			}
			texture2D.Apply();
			Rect rect = new Rect(0f, 0f, 1f, (float)texture2D.height);
			Sprite sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));
			this.hueImage.sprite = sprite;
			this.hueImage.color = Color.white;
			this.hueImage.type = Image.Type.Simple;
			if (this.lastHueTexture != null && Application.isPlaying)
			{
				UnityEngine.Object.Destroy(this.lastHueTexture);
			}
			this.lastHueTexture = texture2D;
		}
	}

	// Token: 0x06006D4C RID: 27980 RVA: 0x002927E0 File Offset: 0x00290BE0
	private IEnumerator GenerateAllCachedImages()
	{
		int saveHueInt = this._hueInt;
		float saveHue = this._hue;
		for (int i = 0; i < 256; i++)
		{
			this._hueInt = i;
			this._hue = (float)this._hueInt / 255f;
			this.RegenerateSVImage();
			yield return null;
		}
		this._hueInt = saveHueInt;
		this._hue = saveHue;
		yield break;
	}

	// Token: 0x06006D4D RID: 27981 RVA: 0x002927FB File Offset: 0x00290BFB
	private void RegenerateImages()
	{
		this.RegenerateSVImage();
		this.RegenerateHueImage();
	}

	// Token: 0x06006D4E RID: 27982 RVA: 0x00292809 File Offset: 0x00290C09
	protected override void SetXYValFromSelectorPosition()
	{
		base.SetXYValFromSelectorPosition();
		this.ignoreRGBCallbacks = true;
		this.RecalcRGB(false);
		this.ignoreRGBCallbacks = false;
	}

	// Token: 0x06006D4F RID: 27983 RVA: 0x00292826 File Offset: 0x00290C26
	public void Reset()
	{
		this.hue = this.defaultHue;
		this.saturation = this.defaultSaturation;
		this.cvalue = this.defaultCvalue;
		this.RecalcRGB(false);
	}

	// Token: 0x06006D50 RID: 27984 RVA: 0x00292853 File Offset: 0x00290C53
	private void OnEnable()
	{
		this.RegenerateImages();
	}

	// Token: 0x06006D51 RID: 27985 RVA: 0x0029285B File Offset: 0x00290C5B
	public override void Awake()
	{
		base.Awake();
		if (!HSVColorPicker.cacheBuildTriggered)
		{
			HSVColorPicker.cacheBuildTriggered = true;
			this.GenerateAllCachedImages();
		}
		this.RegenerateImages();
	}

	// Token: 0x06006D52 RID: 27986 RVA: 0x00292880 File Offset: 0x00290C80
	private void OnDestroy()
	{
		if (Application.isPlaying && this.lastHueTexture != null)
		{
			UnityEngine.Object.Destroy(this.lastHueTexture);
		}
	}

	// Token: 0x06006D53 RID: 27987 RVA: 0x002928A8 File Offset: 0x00290CA8
	// Note: this type is marked as 'beforefieldinit'.
	static HSVColorPicker()
	{
	}

	// Token: 0x04005EA0 RID: 24224
	public Slider hueSlider;

	// Token: 0x04005EA1 RID: 24225
	public Image hueImage;

	// Token: 0x04005EA2 RID: 24226
	public Image saturationImage;

	// Token: 0x04005EA3 RID: 24227
	public float defaultHue;

	// Token: 0x04005EA4 RID: 24228
	[SerializeField]
	private float _hue;

	// Token: 0x04005EA5 RID: 24229
	[SerializeField]
	private int _hueInt;

	// Token: 0x04005EA6 RID: 24230
	private bool ignoreRGBCallbacks;

	// Token: 0x04005EA7 RID: 24231
	private bool skipRGBSliderSet;

	// Token: 0x04005EA8 RID: 24232
	public float defaultSaturation;

	// Token: 0x04005EA9 RID: 24233
	public float defaultCvalue;

	// Token: 0x04005EAA RID: 24234
	public Slider redSlider;

	// Token: 0x04005EAB RID: 24235
	public Slider greenSlider;

	// Token: 0x04005EAC RID: 24236
	public Slider blueSlider;

	// Token: 0x04005EAD RID: 24237
	public Image colorSample;

	// Token: 0x04005EAE RID: 24238
	public Transform colorObject;

	// Token: 0x04005EAF RID: 24239
	[SerializeField]
	private float _red;

	// Token: 0x04005EB0 RID: 24240
	[SerializeField]
	private float _green;

	// Token: 0x04005EB1 RID: 24241
	[SerializeField]
	private float _blue;

	// Token: 0x04005EB2 RID: 24242
	public Color currentColor;

	// Token: 0x04005EB3 RID: 24243
	public HSVColor currentHSVColor;

	// Token: 0x04005EB4 RID: 24244
	public static HSVColor clipboardColor;

	// Token: 0x04005EB5 RID: 24245
	private static bool cacheBuildTriggered;

	// Token: 0x04005EB6 RID: 24246
	private static Dictionary<int, Sprite> cachedSprites;

	// Token: 0x04005EB7 RID: 24247
	public HSVColorPicker.OnColorChanged onColorChangedHandlers;

	// Token: 0x04005EB8 RID: 24248
	public HSVColorPicker.OnHSVColorChanged onHSVColorChangedHandlers;

	// Token: 0x04005EB9 RID: 24249
	private Texture2D lastHueTexture;

	// Token: 0x02000DC3 RID: 3523
	// (Invoke) Token: 0x06006D55 RID: 27989
	public delegate void OnColorChanged(Color color);

	// Token: 0x02000DC4 RID: 3524
	// (Invoke) Token: 0x06006D59 RID: 27993
	public delegate void OnHSVColorChanged(float hue, float saturation, float value);

	// Token: 0x0200103B RID: 4155
	[CompilerGenerated]
	private sealed class <GenerateAllCachedImages>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600778B RID: 30603 RVA: 0x002928AA File Offset: 0x00290CAA
		[DebuggerHidden]
		public <GenerateAllCachedImages>c__Iterator0()
		{
		}

		// Token: 0x0600778C RID: 30604 RVA: 0x002928B4 File Offset: 0x00290CB4
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				saveHueInt = this._hueInt;
				saveHue = this._hue;
				i = 0;
				break;
			case 1U:
				i++;
				break;
			default:
				return false;
			}
			if (i < 256)
			{
				this._hueInt = i;
				this._hue = (float)this._hueInt / 255f;
				base.RegenerateSVImage();
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			this._hueInt = saveHueInt;
			this._hue = saveHue;
			this.$PC = -1;
			return false;
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x0600778D RID: 30605 RVA: 0x002929AE File Offset: 0x00290DAE
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x0600778E RID: 30606 RVA: 0x002929B6 File Offset: 0x00290DB6
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600778F RID: 30607 RVA: 0x002929BE File Offset: 0x00290DBE
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x002929CE File Offset: 0x00290DCE
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006B8C RID: 27532
		internal int <saveHueInt>__0;

		// Token: 0x04006B8D RID: 27533
		internal float <saveHue>__0;

		// Token: 0x04006B8E RID: 27534
		internal int <i>__1;

		// Token: 0x04006B8F RID: 27535
		internal HSVColorPicker $this;

		// Token: 0x04006B90 RID: 27536
		internal object $current;

		// Token: 0x04006B91 RID: 27537
		internal bool $disposing;

		// Token: 0x04006B92 RID: 27538
		internal int $PC;
	}
}
