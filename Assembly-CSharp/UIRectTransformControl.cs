using System;
using UnityEngine;

// Token: 0x02000DFE RID: 3582
public class UIRectTransformControl : JSONStorable
{
	// Token: 0x06006EBB RID: 28347 RVA: 0x00298B81 File Offset: 0x00296F81
	public UIRectTransformControl()
	{
	}

	// Token: 0x06006EBC RID: 28348 RVA: 0x00298BA0 File Offset: 0x00296FA0
	public void SyncXSize(float s)
	{
		if (this._rectTransform != null)
		{
			Vector2 sizeDelta = this._rectTransform.sizeDelta;
			sizeDelta.x = s;
			this._rectTransform.sizeDelta = sizeDelta;
		}
	}

	// Token: 0x06006EBD RID: 28349 RVA: 0x00298BE0 File Offset: 0x00296FE0
	public void SyncYSize(float s)
	{
		if (this._rectTransform != null)
		{
			Vector2 sizeDelta = this._rectTransform.sizeDelta;
			sizeDelta.y = s;
			this._rectTransform.sizeDelta = sizeDelta;
		}
	}

	// Token: 0x06006EBE RID: 28350 RVA: 0x00298C20 File Offset: 0x00297020
	protected void Init()
	{
		this._rectTransform = base.GetComponent<RectTransform>();
		if (this._rectTransform != null)
		{
			Vector2 sizeDelta = this._rectTransform.sizeDelta;
			this.canvasXSizeJSON = new JSONStorableFloat("xSize", sizeDelta.x, new JSONStorableFloat.SetFloatCallback(this.SyncXSize), this.minSize, this.maxSize, true, true);
			base.RegisterFloat(this.canvasXSizeJSON);
			this.canvasYSizeJSON = new JSONStorableFloat("ySize", sizeDelta.y, new JSONStorableFloat.SetFloatCallback(this.SyncYSize), this.minSize, this.maxSize, true, true);
			base.RegisterFloat(this.canvasYSizeJSON);
		}
	}

	// Token: 0x06006EBF RID: 28351 RVA: 0x00298CD0 File Offset: 0x002970D0
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			UIRectTransformControlUI componentInChildren = this.UITransform.GetComponentInChildren<UIRectTransformControlUI>(true);
			if (componentInChildren != null)
			{
				if (this.canvasXSizeJSON != null)
				{
					this.canvasXSizeJSON.slider = componentInChildren.canvasXSizeSlider;
				}
				if (this.canvasYSizeJSON != null)
				{
					this.canvasYSizeJSON.slider = componentInChildren.canvasYSizeSlider;
				}
			}
		}
	}

	// Token: 0x06006EC0 RID: 28352 RVA: 0x00298D40 File Offset: 0x00297140
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			UIRectTransformControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<UIRectTransformControlUI>(true);
			if (componentInChildren != null)
			{
				if (this.canvasXSizeJSON != null)
				{
					this.canvasXSizeJSON.sliderAlt = componentInChildren.canvasXSizeSlider;
				}
				if (this.canvasYSizeJSON != null)
				{
					this.canvasYSizeJSON.sliderAlt = componentInChildren.canvasYSizeSlider;
				}
			}
		}
	}

	// Token: 0x06006EC1 RID: 28353 RVA: 0x00298DAF File Offset: 0x002971AF
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x04005FC9 RID: 24521
	protected RectTransform _rectTransform;

	// Token: 0x04005FCA RID: 24522
	protected JSONStorableFloat canvasXSizeJSON;

	// Token: 0x04005FCB RID: 24523
	protected JSONStorableFloat canvasYSizeJSON;

	// Token: 0x04005FCC RID: 24524
	public float minSize = 100f;

	// Token: 0x04005FCD RID: 24525
	public float maxSize = 2000f;
}
