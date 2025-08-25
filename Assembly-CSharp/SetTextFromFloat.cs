using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE0 RID: 3552
public class SetTextFromFloat : MonoBehaviour
{
	// Token: 0x06006DE1 RID: 28129 RVA: 0x002945FA File Offset: 0x002929FA
	public SetTextFromFloat()
	{
	}

	// Token: 0x1700100E RID: 4110
	// (get) Token: 0x06006DE2 RID: 28130 RVA: 0x0029460D File Offset: 0x00292A0D
	// (set) Token: 0x06006DE3 RID: 28131 RVA: 0x00294615 File Offset: 0x00292A15
	public float floatVal
	{
		get
		{
			return this._floatVal;
		}
		set
		{
			this._floatVal = value;
			this.SyncText();
		}
	}

	// Token: 0x06006DE4 RID: 28132 RVA: 0x00294624 File Offset: 0x00292A24
	public void SyncText()
	{
		if (this._isActive)
		{
			if (this.UIInputField != null)
			{
				this.UIInputField.text = this._floatVal.ToString(this.floatFormat);
			}
			if (this.UIText != null)
			{
				this.UIText.text = this._floatVal.ToString(this.floatFormat);
			}
		}
	}

	// Token: 0x06006DE5 RID: 28133 RVA: 0x00294696 File Offset: 0x00292A96
	private void OnDisable()
	{
		this._isActive = false;
	}

	// Token: 0x06006DE6 RID: 28134 RVA: 0x0029469F File Offset: 0x00292A9F
	private void OnEnable()
	{
		this._isActive = true;
		this.SyncText();
	}

	// Token: 0x04005F26 RID: 24358
	protected bool _isActive;

	// Token: 0x04005F27 RID: 24359
	public Text UIText;

	// Token: 0x04005F28 RID: 24360
	public InputField UIInputField;

	// Token: 0x04005F29 RID: 24361
	public string floatFormat = "F2";

	// Token: 0x04005F2A RID: 24362
	[SerializeField]
	private float _floatVal;
}
