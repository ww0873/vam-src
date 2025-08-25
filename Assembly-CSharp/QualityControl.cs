using System;
using UnityEngine;

// Token: 0x02000C47 RID: 3143
public class QualityControl : MonoBehaviour
{
	// Token: 0x06005B66 RID: 23398 RVA: 0x00218844 File Offset: 0x00216C44
	public QualityControl()
	{
	}

	// Token: 0x06005B67 RID: 23399 RVA: 0x0021885C File Offset: 0x00216C5C
	private void SetMsaaSelectorCurrentValue()
	{
		if (this._msaaSelector != null)
		{
			int msaaLevel = this._msaaLevel;
			switch (msaaLevel)
			{
			case 0:
				this._msaaSelector.currentValue = "Off";
				break;
			default:
				if (msaaLevel == 8)
				{
					this._msaaSelector.currentValue = "8X";
				}
				break;
			case 2:
				this._msaaSelector.currentValue = "2X";
				break;
			case 4:
				this._msaaSelector.currentValue = "4X";
				break;
			}
		}
	}

	// Token: 0x06005B68 RID: 23400 RVA: 0x002188FB File Offset: 0x00216CFB
	private void InitMsaaSelector()
	{
		if (this._msaaSelector != null)
		{
			UIPopup msaaSelector = this._msaaSelector;
			msaaSelector.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(msaaSelector.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetMsaaFromString));
			this.SetMsaaSelectorCurrentValue();
		}
	}

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06005B69 RID: 23401 RVA: 0x0021893B File Offset: 0x00216D3B
	// (set) Token: 0x06005B6A RID: 23402 RVA: 0x00218944 File Offset: 0x00216D44
	public UIPopup msaaSelector
	{
		get
		{
			return this._msaaSelector;
		}
		set
		{
			if (this._msaaSelector != value)
			{
				if (this._msaaSelector != null)
				{
					UIPopup msaaSelector = this._msaaSelector;
					msaaSelector.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(msaaSelector.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetMsaaFromString));
				}
				this._msaaSelector = value;
				this.InitMsaaSelector();
			}
		}
	}

	// Token: 0x06005B6B RID: 23403 RVA: 0x002189A7 File Offset: 0x00216DA7
	private void SetMsaaLevel()
	{
		if (this._msaaLevel == 0 || this._msaaLevel == 2 || this._msaaLevel == 4 || this._msaaLevel == 8)
		{
			QualitySettings.antiAliasing = this._msaaLevel;
		}
	}

	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x06005B6C RID: 23404 RVA: 0x002189E3 File Offset: 0x00216DE3
	// (set) Token: 0x06005B6D RID: 23405 RVA: 0x002189EB File Offset: 0x00216DEB
	public int msaaLevel
	{
		get
		{
			return this._msaaLevel;
		}
		set
		{
			if (this._msaaLevel != value && (value == 0 || value == 2 || value == 4 || value == 8))
			{
				this._msaaLevel = value;
				this.SetMsaaLevel();
				this.SetMsaaSelectorCurrentValue();
			}
		}
	}

	// Token: 0x06005B6E RID: 23406 RVA: 0x00218A28 File Offset: 0x00216E28
	public void SetMsaaFromString(string levelString)
	{
		if (levelString != null)
		{
			if (!(levelString == "Off"))
			{
				if (!(levelString == "2X"))
				{
					if (!(levelString == "4X"))
					{
						if (levelString == "8X")
						{
							this.msaaLevel = 8;
						}
					}
					else
					{
						this.msaaLevel = 4;
					}
				}
				else
				{
					this.msaaLevel = 2;
				}
			}
			else
			{
				this.msaaLevel = 0;
			}
		}
	}

	// Token: 0x06005B6F RID: 23407 RVA: 0x00218AB0 File Offset: 0x00216EB0
	private void SetPixelLightCountSelectorCurrentValue()
	{
		if (this._pixelLightCountSelector != null)
		{
			this._pixelLightCountSelector.currentValue = this._pixelLightCount.ToString();
		}
	}

	// Token: 0x06005B70 RID: 23408 RVA: 0x00218ADF File Offset: 0x00216EDF
	private void InitPixelLightCountSelector()
	{
		if (this._pixelLightCountSelector != null)
		{
			UIPopup pixelLightCountSelector = this._pixelLightCountSelector;
			pixelLightCountSelector.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(pixelLightCountSelector.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetPixelLightCountFromString));
			this.SetPixelLightCountSelectorCurrentValue();
		}
	}

	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x06005B71 RID: 23409 RVA: 0x00218B1F File Offset: 0x00216F1F
	// (set) Token: 0x06005B72 RID: 23410 RVA: 0x00218B28 File Offset: 0x00216F28
	public UIPopup pixelLightCountSelector
	{
		get
		{
			return this._pixelLightCountSelector;
		}
		set
		{
			if (this._pixelLightCountSelector != value)
			{
				if (this._pixelLightCountSelector != null)
				{
					UIPopup pixelLightCountSelector = this._pixelLightCountSelector;
					pixelLightCountSelector.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(pixelLightCountSelector.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetPixelLightCountFromString));
				}
				this._pixelLightCountSelector = value;
				this.InitPixelLightCountSelector();
			}
		}
	}

	// Token: 0x06005B73 RID: 23411 RVA: 0x00218B8B File Offset: 0x00216F8B
	private void SetPixelLightCount()
	{
		QualitySettings.pixelLightCount = this._pixelLightCount;
	}

	// Token: 0x17000D6E RID: 3438
	// (get) Token: 0x06005B74 RID: 23412 RVA: 0x00218B98 File Offset: 0x00216F98
	// (set) Token: 0x06005B75 RID: 23413 RVA: 0x00218BA0 File Offset: 0x00216FA0
	public int pixelLightCount
	{
		get
		{
			return this._pixelLightCount;
		}
		set
		{
			if (this._pixelLightCount != value)
			{
				this._pixelLightCount = value;
				this.SetPixelLightCount();
				this.SetPixelLightCountSelectorCurrentValue();
			}
		}
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x00218BC1 File Offset: 0x00216FC1
	public void SetPixelLightCountFromString(string lightCount)
	{
		this.pixelLightCount = int.Parse(lightCount);
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x00218BCF File Offset: 0x00216FCF
	private void Start()
	{
		this.SetMsaaLevel();
		if (this._msaaSelector != null)
		{
			this.InitMsaaSelector();
		}
		this.SetPixelLightCount();
		if (this._pixelLightCountSelector != null)
		{
			this.InitPixelLightCountSelector();
		}
	}

	// Token: 0x04004B52 RID: 19282
	[SerializeField]
	private UIPopup _msaaSelector;

	// Token: 0x04004B53 RID: 19283
	[SerializeField]
	private int _msaaLevel = 8;

	// Token: 0x04004B54 RID: 19284
	[SerializeField]
	private UIPopup _pixelLightCountSelector;

	// Token: 0x04004B55 RID: 19285
	[SerializeField]
	private int _pixelLightCount = 4;
}
