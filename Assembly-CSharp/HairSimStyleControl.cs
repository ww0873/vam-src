using System;
using GPUTools.Hair.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D73 RID: 3443
public class HairSimStyleControl : ObjectChooser
{
	// Token: 0x06006A22 RID: 27170 RVA: 0x0027F39D File Offset: 0x0027D79D
	public HairSimStyleControl()
	{
	}

	// Token: 0x06006A23 RID: 27171 RVA: 0x0027F3B0 File Offset: 0x0027D7B0
	protected void SetMatchScalpRatio(float f)
	{
		this.matchScalpRatio = f;
	}

	// Token: 0x06006A24 RID: 27172 RVA: 0x0027F3BC File Offset: 0x0027D7BC
	public void MatchScalp()
	{
		if (base.CurrentChoice != null && this.scalpChooser != null && this.scalpChooser.CurrentChoice != null)
		{
			MaterialOptions componentInChildren = this.scalpChooser.CurrentChoice.GetComponentInChildren<MaterialOptions>();
			HairSettings componentInChildren2 = base.CurrentChoice.GetComponentInChildren<HairSettings>();
			if (componentInChildren2 != null && componentInChildren != null)
			{
				Color rootColor = componentInChildren2.RenderSettings.RootTipColorProvider.RootColor;
				Color tipColor = componentInChildren2.RenderSettings.RootTipColorProvider.TipColor;
				Color color = Color.Lerp(rootColor, tipColor, this.matchScalpRatio);
				componentInChildren.SetColor1(color);
				componentInChildren.SetColor2(color);
			}
		}
	}

	// Token: 0x06006A25 RID: 27173 RVA: 0x0027F478 File Offset: 0x0027D878
	public override void InitUI()
	{
		base.InitUI();
		if (this.UITransform != null)
		{
			HairSimStyleControlUI componentInChildren = this.UITransform.GetComponentInChildren<HairSimStyleControlUI>();
			if (componentInChildren != null)
			{
				if (this.matchScalpButton != null)
				{
					this.matchScalpButton.onClick.RemoveAllListeners();
				}
				this.matchScalpButton = componentInChildren.matchScalpButton;
				if (this.matchScalpButton != null)
				{
					this.matchScalpButton.onClick.AddListener(new UnityAction(this.MatchScalp));
				}
				if (this.matchScalpRatioSlider != null)
				{
					this.matchScalpRatioSlider.onValueChanged.RemoveAllListeners();
				}
				this.matchScalpRatioSlider = componentInChildren.matchScalpRatioSlider;
				if (this.matchScalpRatioSlider != null)
				{
					this.matchScalpRatioSlider.value = this.matchScalpRatio;
					this.matchScalpRatioSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetMatchScalpRatio));
				}
			}
		}
	}

	// Token: 0x06006A26 RID: 27174 RVA: 0x0027F57C File Offset: 0x0027D97C
	public override void InitUIAlt()
	{
		base.InitUIAlt();
		if (this.UITransformAlt != null)
		{
			HairSimStyleControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<HairSimStyleControlUI>();
			if (componentInChildren != null)
			{
				if (this.matchScalpButtonAlt != null)
				{
					this.matchScalpButtonAlt.onClick.RemoveAllListeners();
				}
				this.matchScalpButtonAlt = componentInChildren.matchScalpButton;
				if (this.matchScalpButtonAlt != null)
				{
					this.matchScalpButtonAlt.onClick.AddListener(new UnityAction(this.MatchScalp));
				}
				if (this.matchScalpRatioSliderAlt != null)
				{
					this.matchScalpRatioSliderAlt.onValueChanged.RemoveAllListeners();
				}
				this.matchScalpRatioSliderAlt = componentInChildren.matchScalpRatioSlider;
				if (this.matchScalpRatioSliderAlt != null)
				{
					this.matchScalpRatioSliderAlt.value = this.matchScalpRatio;
					this.matchScalpRatioSliderAlt.onValueChanged.AddListener(new UnityAction<float>(this.SetMatchScalpRatio));
				}
			}
		}
	}

	// Token: 0x04005C03 RID: 23555
	public Button matchScalpButton;

	// Token: 0x04005C04 RID: 23556
	public Button matchScalpButtonAlt;

	// Token: 0x04005C05 RID: 23557
	public float matchScalpRatio = 0.7f;

	// Token: 0x04005C06 RID: 23558
	public Slider matchScalpRatioSlider;

	// Token: 0x04005C07 RID: 23559
	public Slider matchScalpRatioSliderAlt;

	// Token: 0x04005C08 RID: 23560
	public ObjectChooser scalpChooser;
}
