using System;
using System.Collections.Generic;
using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

// Token: 0x02000D75 RID: 3445
public class HairSimStyleToolControl : CapsuleToolControl
{
	// Token: 0x06006A28 RID: 27176 RVA: 0x0027F685 File Offset: 0x0027DA85
	public HairSimStyleToolControl()
	{
	}

	// Token: 0x06006A29 RID: 27177 RVA: 0x0027F698 File Offset: 0x0027DA98
	protected void SyncOn(bool b)
	{
		this.SyncTool();
	}

	// Token: 0x06006A2A RID: 27178 RVA: 0x0027F6A0 File Offset: 0x0027DAA0
	protected void SyncTool()
	{
		if (this.dynamicSlider != null)
		{
			this.dynamicSlider.gameObject.SetActive(false);
		}
		if (this.dynamicButton != null)
		{
			this.dynamicButton.gameObject.SetActive(false);
		}
		if (this.cutCapsule != null)
		{
			this.cutCapsule.enabled = false;
		}
		if (this.growCapsule != null)
		{
			this.growCapsule.enabled = false;
		}
		if (this.holdCapsule != null)
		{
			this.holdCapsule.enabled = false;
		}
		if (this.grabCapsule != null)
		{
			this.grabCapsule.enabled = false;
		}
		if (this.pushCapsule != null)
		{
			this.pushCapsule.enabled = false;
		}
		if (this.pullCapsule != null)
		{
			this.pullCapsule.enabled = false;
		}
		if (this.brushCapsule != null)
		{
			this.brushCapsule.enabled = false;
		}
		if (this.rigidityIncreaseCapsule != null)
		{
			this.rigidityIncreaseCapsule.enabled = false;
		}
		if (this.rigidityDecreaseCapsule != null)
		{
			this.rigidityDecreaseCapsule.enabled = false;
		}
		if (this.rigiditySetCapsule != null)
		{
			this.rigiditySetCapsule.enabled = false;
		}
		if (this.cutStrengthJSON != null)
		{
			this.cutStrengthJSON.slider = null;
		}
		if (this.growStrengthJSON != null)
		{
			this.growStrengthJSON.slider = null;
		}
		if (this.holdStrengthJSON != null)
		{
			this.holdStrengthJSON.slider = null;
		}
		if (this.pushStrengthJSON != null)
		{
			this.pushStrengthJSON.slider = null;
		}
		if (this.pullStrengthJSON != null)
		{
			this.pullStrengthJSON.slider = null;
		}
		if (this.brushStrengthJSON != null)
		{
			this.brushStrengthJSON.slider = null;
		}
		if (this.rigidityIncreaseStrengthJSON != null)
		{
			this.rigidityIncreaseStrengthJSON.slider = null;
		}
		if (this.rigidityDecreaseStrengthJSON != null)
		{
			this.rigidityDecreaseStrengthJSON.slider = null;
		}
		if (this.rigiditySetStrengthJSON != null)
		{
			this.rigiditySetStrengthJSON.slider = null;
		}
		switch (this.toolChoice)
		{
		case HairSimStyleToolControl.ToolChoice.Cut:
			if (this.cutCapsule != null)
			{
				this.cutCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.cutStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.cutStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Cut Strength";
			}
			if (this.dynamicButton != null)
			{
				this.dynamicButton.gameObject.SetActive(true);
				this.dynamicButton.label = "Burst Cut";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.Grow:
			if (this.growCapsule != null)
			{
				this.growCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.growStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.growStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Grow Strength";
			}
			if (this.dynamicButton != null)
			{
				this.dynamicButton.gameObject.SetActive(true);
				this.dynamicButton.label = "Burst Grow";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.Hold:
			if (this.holdCapsule != null)
			{
				this.holdCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.holdStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.holdStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Hold Strength";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.Grab:
			if (this.grabCapsule != null)
			{
				this.grabCapsule.enabled = this.onJSON.val;
			}
			break;
		case HairSimStyleToolControl.ToolChoice.Push:
			if (this.pushCapsule != null)
			{
				this.pushCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.pushStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.pushStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Push Strength";
			}
			if (this.dynamicButton != null)
			{
				this.dynamicButton.gameObject.SetActive(true);
				this.dynamicButton.label = "Burst Push";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.Pull:
			if (this.pullCapsule != null)
			{
				this.pullCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.pullStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.pullStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Pull Strength";
			}
			if (this.dynamicButton != null)
			{
				this.dynamicButton.gameObject.SetActive(true);
				this.dynamicButton.label = "Burst Pull";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.Brush:
			if (this.brushCapsule != null)
			{
				this.brushCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.brushStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.brushStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Brush Strength";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.RigidityIncrease:
			if (this.rigidityIncreaseCapsule != null)
			{
				this.rigidityIncreaseCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.rigidityIncreaseStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.rigidityIncreaseStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Rigidity + Strength";
			}
			if (this.dynamicButton != null)
			{
				this.dynamicButton.gameObject.SetActive(true);
				this.dynamicButton.label = "Burst Rigidity +";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.RigidityDecrease:
			if (this.rigidityDecreaseCapsule != null)
			{
				this.rigidityDecreaseCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.rigidityDecreaseStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.rigidityDecreaseStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Rigidity - Strength";
			}
			if (this.dynamicButton != null)
			{
				this.dynamicButton.gameObject.SetActive(true);
				this.dynamicButton.label = "Burst Rigidity -";
			}
			break;
		case HairSimStyleToolControl.ToolChoice.RigiditySet:
			if (this.rigiditySetCapsule != null)
			{
				this.rigiditySetCapsule.enabled = this.onJSON.val;
			}
			if (this.dynamicSlider != null && this.rigiditySetStrengthJSON != null)
			{
				this.dynamicSlider.gameObject.SetActive(true);
				this.rigiditySetStrengthJSON.slider = this.dynamicSlider.slider;
				this.dynamicSlider.label = "Rigidity Set Level";
			}
			if (this.dynamicButton != null)
			{
				this.dynamicButton.gameObject.SetActive(true);
				this.dynamicButton.label = "Burst Rigidity Set";
			}
			break;
		}
	}

	// Token: 0x06006A2B RID: 27179 RVA: 0x0027FF20 File Offset: 0x0027E320
	protected void SyncToolChoice(string s)
	{
		try
		{
			this.toolChoice = (HairSimStyleToolControl.ToolChoice)Enum.Parse(typeof(HairSimStyleToolControl.ToolChoice), s);
			this.burstForFrames = 0;
			this.SyncTool();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set tool to " + s + " which is not a valid ToolChoice");
		}
	}

	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x06006A2C RID: 27180 RVA: 0x0027FF88 File Offset: 0x0027E388
	// (set) Token: 0x06006A2D RID: 27181 RVA: 0x0027FF90 File Offset: 0x0027E390
	public bool allowRigidityPaint
	{
		get
		{
			return this._allowRigidityPaint;
		}
		set
		{
			if (this._allowRigidityPaint != value)
			{
				this._allowRigidityPaint = value;
				if (this.toolChoiceJSON != null)
				{
					if (this._allowRigidityPaint)
					{
						this.toolChoiceJSON.choices = this.allToolChoices;
					}
					else
					{
						this.toolChoiceJSON.choices = this.standardToolChoices;
						if (this.toolChoiceJSON.val == "RigidityIncrease" || this.toolChoiceJSON.val == "RigidityDecrease" || this.toolChoiceJSON.val == "RigiditySet")
						{
							this.toolChoiceJSON.val = "Brush";
						}
					}
				}
			}
		}
	}

	// Token: 0x06006A2E RID: 27182 RVA: 0x0028004B File Offset: 0x0027E44B
	protected void SyncCutStrength(float f)
	{
		if (this.cutCapsule != null)
		{
			this.cutCapsule.strength = f * this.toolStrengthMultiplier;
		}
	}

	// Token: 0x06006A2F RID: 27183 RVA: 0x00280071 File Offset: 0x0027E471
	protected void SyncGrowStrength(float f)
	{
		if (this.growCapsule != null)
		{
			this.growCapsule.strength = f * this.toolStrengthMultiplier;
		}
	}

	// Token: 0x06006A30 RID: 27184 RVA: 0x00280097 File Offset: 0x0027E497
	protected void SyncPushStrength(float f)
	{
		if (this.pushCapsule != null)
		{
			this.pushCapsule.strength = f * this.toolStrengthMultiplier;
		}
	}

	// Token: 0x06006A31 RID: 27185 RVA: 0x002800BD File Offset: 0x0027E4BD
	protected void SyncPullStrength(float f)
	{
		if (this.pullCapsule != null)
		{
			this.pullCapsule.strength = f * this.toolStrengthMultiplier;
		}
	}

	// Token: 0x06006A32 RID: 27186 RVA: 0x002800E3 File Offset: 0x0027E4E3
	protected void SyncBrushStrength(float f)
	{
		if (this.brushCapsule != null)
		{
			this.brushCapsule.strength = f * this.toolStrengthMultiplier;
		}
	}

	// Token: 0x06006A33 RID: 27187 RVA: 0x00280109 File Offset: 0x0027E509
	protected void SyncHoldStrength(float f)
	{
		if (this.holdCapsule != null)
		{
			this.holdCapsule.strength = f;
		}
	}

	// Token: 0x06006A34 RID: 27188 RVA: 0x00280128 File Offset: 0x0027E528
	protected void SyncGrabStrength()
	{
		if (this.grabCapsule != null)
		{
			if (this.toolStrengthMultiplier > 0.5f)
			{
				this.grabCapsule.strength = 1f;
			}
			else
			{
				this.grabCapsule.strength = 0f;
			}
		}
	}

	// Token: 0x06006A35 RID: 27189 RVA: 0x0028017B File Offset: 0x0027E57B
	protected void SyncRigidityIncreaseStrength(float f)
	{
		if (this.rigidityIncreaseCapsule != null)
		{
			this.rigidityIncreaseCapsule.strength = f * this.toolStrengthMultiplier;
		}
	}

	// Token: 0x06006A36 RID: 27190 RVA: 0x002801A1 File Offset: 0x0027E5A1
	protected void SyncRigidityDecreaseStrength(float f)
	{
		if (this.rigidityDecreaseCapsule != null)
		{
			this.rigidityDecreaseCapsule.strength = f * this.toolStrengthMultiplier;
		}
	}

	// Token: 0x06006A37 RID: 27191 RVA: 0x002801C8 File Offset: 0x0027E5C8
	protected void SyncRigiditySetStrength(float f)
	{
		if (this.rigiditySetCapsule != null)
		{
			if (this.toolStrengthMultiplier > 0.5f)
			{
				this.rigiditySetCapsule.strength = f;
			}
			else
			{
				this.rigiditySetCapsule.strength = -1f;
			}
		}
	}

	// Token: 0x06006A38 RID: 27192 RVA: 0x00280217 File Offset: 0x0027E617
	protected void BurstAction()
	{
		this.onJSON.val = true;
		this.burstForFrames = 10;
	}

	// Token: 0x06006A39 RID: 27193 RVA: 0x0028022D File Offset: 0x0027E62D
	public void ResetToolStrengthMultiplier()
	{
		this.SetToolStrengthMultiplier(1f);
	}

	// Token: 0x06006A3A RID: 27194 RVA: 0x0028023C File Offset: 0x0027E63C
	public void SetToolStrengthMultiplier(float f)
	{
		this.toolStrengthMultiplier = f;
		if (this.brushStrengthJSON != null)
		{
			this.SyncBrushStrength(this.brushStrengthJSON.val);
		}
		if (this.cutStrengthJSON != null)
		{
			this.SyncCutStrength(this.cutStrengthJSON.val);
		}
		if (this.growStrengthJSON != null)
		{
			this.SyncGrowStrength(this.growStrengthJSON.val);
		}
		this.SyncGrabStrength();
		if (this.pullStrengthJSON != null)
		{
			this.SyncPullStrength(this.pullStrengthJSON.val);
		}
		if (this.pushStrengthJSON != null)
		{
			this.SyncPushStrength(this.pushStrengthJSON.val);
		}
		if (this.rigidityIncreaseStrengthJSON != null)
		{
			this.SyncRigidityIncreaseStrength(this.rigidityIncreaseStrengthJSON.val);
		}
		if (this.rigidityDecreaseStrengthJSON != null)
		{
			this.SyncRigidityDecreaseStrength(this.rigidityDecreaseStrengthJSON.val);
		}
		if (this.rigiditySetStrengthJSON != null)
		{
			this.SyncRigiditySetStrength(this.rigiditySetStrengthJSON.val);
		}
	}

	// Token: 0x06006A3B RID: 27195 RVA: 0x00280338 File Offset: 0x0027E738
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (this.wasInit)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				HairSimStyleToolControlUI componentInChildren = this.UITransform.GetComponentInChildren<HairSimStyleToolControlUI>();
				if (componentInChildren != null)
				{
					this.onJSON.RegisterToggle(componentInChildren.onToggle, isAlt);
					this.toolChoiceJSON.RegisterPopup(componentInChildren.toolChoicePopup, isAlt);
					if (!isAlt)
					{
						this.dynamicButton = componentInChildren.dynamicButton;
						if (this.dynamicButton != null)
						{
							this.burstAction.RegisterButton(componentInChildren.dynamicButton.button, isAlt);
						}
						this.dynamicSlider = componentInChildren.dynamicSlider;
					}
				}
			}
			this.SyncTool();
		}
	}

	// Token: 0x06006A3C RID: 27196 RVA: 0x002803EC File Offset: 0x0027E7EC
	protected override void Init()
	{
		base.Init();
		this.onJSON = new JSONStorableBool("on", true, new JSONStorableBool.SetBoolCallback(this.SyncOn));
		base.RegisterBool(this.onJSON);
		this.allToolChoices = new List<string>();
		this.allToolChoices.Add("Cut");
		this.allToolChoices.Add("Grow");
		this.allToolChoices.Add("Hold");
		this.allToolChoices.Add("Grab");
		this.allToolChoices.Add("Push");
		this.allToolChoices.Add("Pull");
		this.allToolChoices.Add("Brush");
		this.allToolChoices.Add("RigidityIncrease");
		this.allToolChoices.Add("RigidityDecrease");
		this.allToolChoices.Add("RigiditySet");
		this.standardToolChoices = new List<string>();
		this.standardToolChoices.Add("Cut");
		this.standardToolChoices.Add("Grow");
		this.standardToolChoices.Add("Hold");
		this.standardToolChoices.Add("Grab");
		this.standardToolChoices.Add("Push");
		this.standardToolChoices.Add("Pull");
		this.standardToolChoices.Add("Brush");
		List<string> choicesList;
		if (this.allowRigidityPaint)
		{
			choicesList = this.allToolChoices;
		}
		else
		{
			choicesList = this.standardToolChoices;
		}
		this.toolChoiceJSON = new JSONStorableStringChooser("toolChoice", choicesList, this.toolChoice.ToString(), "Tool", new JSONStorableStringChooser.SetStringCallback(this.SyncToolChoice));
		base.RegisterStringChooser(this.toolChoiceJSON);
		this.burstAction = new JSONStorableAction("burstAction", new JSONStorableAction.ActionCallback(this.BurstAction));
		base.RegisterAction(this.burstAction);
		if (this.cutCapsule != null)
		{
			this.cutStrengthJSON = new JSONStorableFloat("cutStrength", this.cutCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncCutStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.cutStrengthJSON);
		}
		if (this.growCapsule != null)
		{
			this.growStrengthJSON = new JSONStorableFloat("growStrength", this.growCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncGrowStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.growStrengthJSON);
		}
		if (this.pushCapsule != null)
		{
			this.pushStrengthJSON = new JSONStorableFloat("pushStrength", this.pushCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncPushStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.pushStrengthJSON);
		}
		if (this.pullCapsule != null)
		{
			this.pullStrengthJSON = new JSONStorableFloat("pullStrength", this.pullCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncPullStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.pullStrengthJSON);
		}
		if (this.brushCapsule != null)
		{
			this.brushStrengthJSON = new JSONStorableFloat("brushStrength", this.brushCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncBrushStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.brushStrengthJSON);
		}
		if (this.holdCapsule != null)
		{
			this.holdStrengthJSON = new JSONStorableFloat("holdStrength", this.holdCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncHoldStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.holdStrengthJSON);
		}
		if (this.rigidityIncreaseCapsule != null)
		{
			this.rigidityIncreaseStrengthJSON = new JSONStorableFloat("rigidityIncreaseStrength", this.rigidityIncreaseCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncRigidityIncreaseStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.rigidityIncreaseStrengthJSON);
		}
		if (this.rigidityDecreaseCapsule != null)
		{
			this.rigidityDecreaseStrengthJSON = new JSONStorableFloat("rigidityDecreaseStrength", this.rigidityDecreaseCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncRigidityDecreaseStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.rigidityDecreaseStrengthJSON);
		}
		if (this.rigiditySetCapsule != null)
		{
			this.rigiditySetStrengthJSON = new JSONStorableFloat("rigiditySetStrength", this.rigiditySetCapsule.strength, new JSONStorableFloat.SetFloatCallback(this.SyncRigiditySetStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.rigiditySetStrengthJSON);
		}
		this.SyncTool();
	}

	// Token: 0x06006A3D RID: 27197 RVA: 0x002808AC File Offset: 0x0027ECAC
	private void FixedUpdate()
	{
		if (this.burstForFrames > 0)
		{
			this.burstForFrames--;
			if (this.burstForFrames == 0)
			{
				this.onJSON.val = false;
			}
			else
			{
				this.onJSON.val = true;
			}
		}
	}

	// Token: 0x04005C0B RID: 23563
	public GpuCutCapsule cutCapsule;

	// Token: 0x04005C0C RID: 23564
	public GpuGrowCapsule growCapsule;

	// Token: 0x04005C0D RID: 23565
	public GpuHoldCapsule holdCapsule;

	// Token: 0x04005C0E RID: 23566
	public GpuGrabCapsule grabCapsule;

	// Token: 0x04005C0F RID: 23567
	public GpuPushCapsule pushCapsule;

	// Token: 0x04005C10 RID: 23568
	public GpuPullCapsule pullCapsule;

	// Token: 0x04005C11 RID: 23569
	public GpuBrushCapsule brushCapsule;

	// Token: 0x04005C12 RID: 23570
	public GpuRigidityIncreaseCapsule rigidityIncreaseCapsule;

	// Token: 0x04005C13 RID: 23571
	public GpuRigidityDecreaseCapsule rigidityDecreaseCapsule;

	// Token: 0x04005C14 RID: 23572
	public GpuRigiditySetCapsule rigiditySetCapsule;

	// Token: 0x04005C15 RID: 23573
	protected JSONStorableBool onJSON;

	// Token: 0x04005C16 RID: 23574
	protected List<string> allToolChoices;

	// Token: 0x04005C17 RID: 23575
	protected List<string> standardToolChoices;

	// Token: 0x04005C18 RID: 23576
	public HairSimStyleToolControl.ToolChoice toolChoice;

	// Token: 0x04005C19 RID: 23577
	protected JSONStorableStringChooser toolChoiceJSON;

	// Token: 0x04005C1A RID: 23578
	protected bool _allowRigidityPaint;

	// Token: 0x04005C1B RID: 23579
	protected JSONStorableFloat cutStrengthJSON;

	// Token: 0x04005C1C RID: 23580
	protected JSONStorableFloat growStrengthJSON;

	// Token: 0x04005C1D RID: 23581
	protected JSONStorableFloat pushStrengthJSON;

	// Token: 0x04005C1E RID: 23582
	protected JSONStorableFloat pullStrengthJSON;

	// Token: 0x04005C1F RID: 23583
	protected JSONStorableFloat brushStrengthJSON;

	// Token: 0x04005C20 RID: 23584
	protected JSONStorableFloat holdStrengthJSON;

	// Token: 0x04005C21 RID: 23585
	protected JSONStorableFloat rigidityIncreaseStrengthJSON;

	// Token: 0x04005C22 RID: 23586
	protected JSONStorableFloat rigidityDecreaseStrengthJSON;

	// Token: 0x04005C23 RID: 23587
	protected JSONStorableFloat rigiditySetStrengthJSON;

	// Token: 0x04005C24 RID: 23588
	protected int burstForFrames;

	// Token: 0x04005C25 RID: 23589
	protected JSONStorableAction burstAction;

	// Token: 0x04005C26 RID: 23590
	protected float toolStrengthMultiplier = 1f;

	// Token: 0x04005C27 RID: 23591
	protected UIDynamicSlider dynamicSlider;

	// Token: 0x04005C28 RID: 23592
	protected UIDynamicButton dynamicButton;

	// Token: 0x02000D76 RID: 3446
	public enum ToolChoice
	{
		// Token: 0x04005C2A RID: 23594
		Cut,
		// Token: 0x04005C2B RID: 23595
		Grow,
		// Token: 0x04005C2C RID: 23596
		Hold,
		// Token: 0x04005C2D RID: 23597
		Grab,
		// Token: 0x04005C2E RID: 23598
		Push,
		// Token: 0x04005C2F RID: 23599
		Pull,
		// Token: 0x04005C30 RID: 23600
		Brush,
		// Token: 0x04005C31 RID: 23601
		RigidityIncrease,
		// Token: 0x04005C32 RID: 23602
		RigidityDecrease,
		// Token: 0x04005C33 RID: 23603
		RigiditySet
	}
}
