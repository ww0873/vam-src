using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DED RID: 3565
public class UIDynamicColorPicker : UIDynamic
{
	// Token: 0x06006E30 RID: 28208 RVA: 0x00295E3A File Offset: 0x0029423A
	public UIDynamicColorPicker()
	{
	}

	// Token: 0x06006E31 RID: 28209 RVA: 0x00295E4C File Offset: 0x0029424C
	protected void SyncShowLabel()
	{
		if (this.labelText != null)
		{
			this.labelText.gameObject.SetActive(this._showLabel);
			if (this.pickerContainer != null)
			{
				Vector2 offsetMax = this.pickerContainer.offsetMax;
				if (this._showLabel)
				{
					offsetMax.y = -50f;
				}
				else
				{
					offsetMax.y = -15f;
				}
				this.pickerContainer.offsetMax = offsetMax;
			}
		}
	}

	// Token: 0x1700101A RID: 4122
	// (get) Token: 0x06006E32 RID: 28210 RVA: 0x00295ED1 File Offset: 0x002942D1
	// (set) Token: 0x06006E33 RID: 28211 RVA: 0x00295ED9 File Offset: 0x002942D9
	public bool showLabel
	{
		get
		{
			return this._showLabel;
		}
		set
		{
			if (this._showLabel != value)
			{
				this._showLabel = value;
				this.SyncShowLabel();
			}
		}
	}

	// Token: 0x1700101B RID: 4123
	// (get) Token: 0x06006E34 RID: 28212 RVA: 0x00295EF4 File Offset: 0x002942F4
	// (set) Token: 0x06006E35 RID: 28213 RVA: 0x00295F14 File Offset: 0x00294314
	public string label
	{
		get
		{
			if (this.labelText != null)
			{
				return this.labelText.text;
			}
			return null;
		}
		set
		{
			if (this.labelText != null)
			{
				this.labelText.text = value;
			}
		}
	}

	// Token: 0x04005F66 RID: 24422
	public Text labelText;

	// Token: 0x04005F67 RID: 24423
	public RectTransform pickerContainer;

	// Token: 0x04005F68 RID: 24424
	public HSVColorPicker colorPicker;

	// Token: 0x04005F69 RID: 24425
	[HideInInspector]
	[SerializeField]
	protected bool _showLabel = true;
}
