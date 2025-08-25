using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE3 RID: 3555
public class SpeechBubbleControl : JSONStorable
{
	// Token: 0x06006DF6 RID: 28150 RVA: 0x002949F5 File Offset: 0x00292DF5
	public SpeechBubbleControl()
	{
	}

	// Token: 0x06006DF7 RID: 28151 RVA: 0x002949FD File Offset: 0x00292DFD
	protected void SyncBubbleLifeTime(float f)
	{
	}

	// Token: 0x06006DF8 RID: 28152 RVA: 0x002949FF File Offset: 0x00292DFF
	protected void SyncBubbleText(string s)
	{
		this.UpdateText(s, this.bubbleLifetimeJSON.val);
	}

	// Token: 0x06006DF9 RID: 28153 RVA: 0x00294A14 File Offset: 0x00292E14
	public void UpdateText(string text, float newTimeToLive)
	{
		if (this.bubbleTransform != null)
		{
			this.bubbleTransform.gameObject.SetActive(true);
		}
		this.isDisplaying = true;
		Color color = this.bubbleImage.color;
		color.a = 1f;
		this.bubbleImage.color = color;
		color = this.bubbleText.color;
		color.a = 1f;
		this.bubbleText.color = color;
		this.bubbleText.text = text;
		this._timeToLive = newTimeToLive;
	}

	// Token: 0x06006DFA RID: 28154 RVA: 0x00294AA8 File Offset: 0x00292EA8
	protected void Update()
	{
		this._timeToLive -= Time.unscaledDeltaTime;
		if (0f < this._timeToLive && this._timeToLive < 1f)
		{
			Color color = this.bubbleImage.color;
			color.a = this._timeToLive;
			this.bubbleImage.color = color;
			color = this.bubbleText.color;
			color.a = this._timeToLive;
			this.bubbleText.color = color;
		}
		if (this.isDisplaying && this._timeToLive < 0f && this.bubbleTransform != null)
		{
			this.isDisplaying = false;
			this.bubbleText.text = string.Empty;
			this.bubbleTextJSON.valNoCallback = string.Empty;
			this.bubbleTransform.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006DFB RID: 28155 RVA: 0x00294B98 File Offset: 0x00292F98
	protected void Init()
	{
		this.bubbleLifetimeJSON = new JSONStorableFloat("bubbleLifetime", 5f, new JSONStorableFloat.SetFloatCallback(this.SyncBubbleLifeTime), 0f, 20f, false, true);
		base.RegisterFloat(this.bubbleLifetimeJSON);
		this.bubbleTextJSON = new JSONStorableString("bubbleText", string.Empty, new JSONStorableString.SetStringCallback(this.SyncBubbleText));
		base.RegisterString(this.bubbleTextJSON);
		this.bubbleTextJSON.isStorable = false;
		this.bubbleTextJSON.isRestorable = false;
	}

	// Token: 0x06006DFC RID: 28156 RVA: 0x00294C24 File Offset: 0x00293024
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			SpeechBubbleControlUI componentInChildren = this.UITransform.GetComponentInChildren<SpeechBubbleControlUI>();
			if (componentInChildren != null && this.bubbleLifetimeJSON != null)
			{
				this.bubbleLifetimeJSON.slider = componentInChildren.bubbleLifetimeSlider;
			}
		}
	}

	// Token: 0x06006DFD RID: 28157 RVA: 0x00294C78 File Offset: 0x00293078
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			SpeechBubbleControlUI componentInChildren = this.UITransform.GetComponentInChildren<SpeechBubbleControlUI>();
			if (componentInChildren != null && this.bubbleLifetimeJSON != null)
			{
				this.bubbleLifetimeJSON.sliderAlt = componentInChildren.bubbleLifetimeSlider;
			}
		}
	}

	// Token: 0x06006DFE RID: 28158 RVA: 0x00294CCA File Offset: 0x002930CA
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

	// Token: 0x04005F31 RID: 24369
	public Transform bubbleTransform;

	// Token: 0x04005F32 RID: 24370
	public Image bubbleImage;

	// Token: 0x04005F33 RID: 24371
	public Text bubbleText;

	// Token: 0x04005F34 RID: 24372
	protected float _timeToLive;

	// Token: 0x04005F35 RID: 24373
	protected JSONStorableFloat bubbleLifetimeJSON;

	// Token: 0x04005F36 RID: 24374
	protected JSONStorableString bubbleTextJSON;

	// Token: 0x04005F37 RID: 24375
	protected bool isDisplaying;
}
