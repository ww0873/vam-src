using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D04 RID: 3332
public class TextStorable : JSONStorable
{
	// Token: 0x060065AE RID: 26030 RVA: 0x002655E7 File Offset: 0x002639E7
	public TextStorable()
	{
	}

	// Token: 0x060065AF RID: 26031 RVA: 0x002655EF File Offset: 0x002639EF
	public void SyncDisplayField(string s)
	{
		this._text = s;
		if (this.displayField != null)
		{
			this.displayField.text = s;
		}
	}

	// Token: 0x060065B0 RID: 26032 RVA: 0x00265615 File Offset: 0x00263A15
	public void SetFontSize(float s)
	{
		if (this.displayField != null)
		{
			this.displayField.fontSize = Mathf.RoundToInt(s);
		}
	}

	// Token: 0x060065B1 RID: 26033 RVA: 0x0026563C File Offset: 0x00263A3C
	protected void Init()
	{
		if (this.displayField != null)
		{
			this.textJSON = new JSONStorableString("text", string.Empty, new JSONStorableString.SetStringCallback(this.SyncDisplayField));
			base.RegisterString(this.textJSON);
			this.fontSizeJSON = new JSONStorableFloat("fontSize", (float)this.displayField.fontSize, new JSONStorableFloat.SetFloatCallback(this.SetFontSize), 10f, 400f, true, true);
			base.RegisterFloat(this.fontSizeJSON);
		}
	}

	// Token: 0x060065B2 RID: 26034 RVA: 0x002656C8 File Offset: 0x00263AC8
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			TextStorableUI componentInChildren = this.UITransform.GetComponentInChildren<TextStorableUI>(true);
			if (componentInChildren != null)
			{
				if (this.textJSON != null)
				{
					this.textJSON.inputField = componentInChildren.inputField;
					this.textJSON.inputFieldAction = componentInChildren.inputFieldAction;
				}
				if (this.fontSizeJSON != null)
				{
					this.fontSizeJSON.slider = componentInChildren.fontSizeSlider;
				}
			}
		}
	}

	// Token: 0x060065B3 RID: 26035 RVA: 0x00265748 File Offset: 0x00263B48
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			TextStorableUI componentInChildren = this.UITransformAlt.GetComponentInChildren<TextStorableUI>(true);
			if (componentInChildren != null)
			{
				if (this.textJSON != null)
				{
					this.textJSON.inputFieldAlt = componentInChildren.inputField;
					this.textJSON.inputFieldActionAlt = componentInChildren.inputFieldAction;
				}
				if (this.fontSizeJSON != null)
				{
					this.fontSizeJSON.sliderAlt = componentInChildren.fontSizeSlider;
				}
			}
		}
	}

	// Token: 0x060065B4 RID: 26036 RVA: 0x002657C8 File Offset: 0x00263BC8
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x04005509 RID: 21769
	public Text displayField;

	// Token: 0x0400550A RID: 21770
	protected string _text;

	// Token: 0x0400550B RID: 21771
	protected JSONStorableString textJSON;

	// Token: 0x0400550C RID: 21772
	protected JSONStorableFloat fontSizeJSON;
}
