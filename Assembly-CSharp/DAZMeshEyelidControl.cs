using System;
using UnityEngine;

// Token: 0x02000AE9 RID: 2793
public class DAZMeshEyelidControl : JSONStorable
{
	// Token: 0x06004AAA RID: 19114 RVA: 0x0019CE6C File Offset: 0x0019B26C
	public DAZMeshEyelidControl()
	{
	}

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x06004AAB RID: 19115 RVA: 0x0019CEED File Offset: 0x0019B2ED
	// (set) Token: 0x06004AAC RID: 19116 RVA: 0x0019CEF5 File Offset: 0x0019B2F5
	public DAZMorphBank morphBank
	{
		get
		{
			return this._morphBank;
		}
		set
		{
			if (this._morphBank != value)
			{
				this._morphBank = value;
				this.ZeroMorphs();
				this.InitMorphs();
			}
		}
	}

	// Token: 0x06004AAD RID: 19117 RVA: 0x0019CF1B File Offset: 0x0019B31B
	protected void SyncBlinkEnabled(bool b)
	{
	}

	// Token: 0x06004AAE RID: 19118 RVA: 0x0019CF20 File Offset: 0x0019B320
	protected void SyncEyelidLookMorphsEnabled(bool b)
	{
		if (!b)
		{
			if (this.LeftTopEyelidUpMorph != null)
			{
				this.LeftTopEyelidUpMorph.morphValue = 0f;
			}
			if (this.LeftBottomEyelidDownMorph != null)
			{
				this.LeftBottomEyelidDownMorph.morphValue = 0f;
			}
			if (this.RightTopEyelidUpMorph != null)
			{
				this.RightTopEyelidUpMorph.morphValue = 0f;
			}
			if (this.RightBottomEyelidDownMorph != null)
			{
				this.RightBottomEyelidDownMorph.morphValue = 0f;
			}
		}
	}

	// Token: 0x06004AAF RID: 19119 RVA: 0x0019CF9F File Offset: 0x0019B39F
	public void Close()
	{
		this.closed = true;
		this.BlinkClose();
	}

	// Token: 0x06004AB0 RID: 19120 RVA: 0x0019CFAE File Offset: 0x0019B3AE
	public void Open()
	{
		this.closed = false;
		this.BlinkOpen();
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x0019CFBD File Offset: 0x0019B3BD
	public void Blink()
	{
		this.blinking = true;
		this.BlinkClose();
		this.blinkTime = UnityEngine.Random.Range(this.blinkTimeMin, this.blinkTimeMax);
	}

	// Token: 0x06004AB2 RID: 19122 RVA: 0x0019CFE3 File Offset: 0x0019B3E3
	private void BlinkClose()
	{
		this.targetWeight = 1f;
	}

	// Token: 0x06004AB3 RID: 19123 RVA: 0x0019CFF0 File Offset: 0x0019B3F0
	private void BlinkOpen()
	{
		if (!this.closed)
		{
			this.targetWeight = 0f;
		}
	}

	// Token: 0x06004AB4 RID: 19124 RVA: 0x0019D008 File Offset: 0x0019B408
	private void ZeroMorphs()
	{
		if (this.LeftTopEyelidDownMorph != null)
		{
			this.LeftTopEyelidDownMorph.morphValue = 0f;
		}
		if (this.RightTopEyelidDownMorph != null)
		{
			this.RightTopEyelidDownMorph.morphValue = 0f;
		}
		if (this.LeftTopEyelidUpMorph != null)
		{
			this.LeftTopEyelidUpMorph.morphValue = 0f;
		}
		if (this.RightTopEyelidUpMorph != null)
		{
			this.RightTopEyelidUpMorph.morphValue = 0f;
		}
		if (this.LeftBottomEyelidDownMorph != null)
		{
			this.LeftBottomEyelidDownMorph.morphValue = 0f;
		}
		if (this.RightBottomEyelidDownMorph != null)
		{
			this.RightBottomEyelidDownMorph.morphValue = 0f;
		}
		if (this.LeftBottomEyelidUpMorph != null)
		{
			this.LeftBottomEyelidUpMorph.morphValue = 0f;
		}
		if (this.RightBottomEyelidUpMorph != null)
		{
			this.RightBottomEyelidUpMorph.morphValue = 0f;
		}
	}

	// Token: 0x06004AB5 RID: 19125 RVA: 0x0019D0F0 File Offset: 0x0019B4F0
	private void InitMorphs()
	{
		if (this._morphBank)
		{
			DAZMorph builtInMorph = this._morphBank.GetBuiltInMorph(this.LeftTopEyelidDownMorphName);
			if (builtInMorph != null)
			{
				this.LeftTopEyelidDownMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.LeftTopEyelidDownMorphName);
			}
			builtInMorph = this._morphBank.GetBuiltInMorph(this.RightTopEyelidDownMorphName);
			if (builtInMorph != null)
			{
				this.RightTopEyelidDownMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.RightTopEyelidDownMorphName);
			}
			builtInMorph = this._morphBank.GetBuiltInMorph(this.LeftTopEyelidUpMorphName);
			if (builtInMorph != null)
			{
				this.LeftTopEyelidUpMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.LeftTopEyelidUpMorphName);
			}
			builtInMorph = this._morphBank.GetBuiltInMorph(this.RightTopEyelidUpMorphName);
			if (builtInMorph != null)
			{
				this.RightTopEyelidUpMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.RightTopEyelidUpMorphName);
			}
			builtInMorph = this._morphBank.GetBuiltInMorph(this.LeftBottomEyelidDownMorphName);
			if (builtInMorph != null)
			{
				this.LeftBottomEyelidDownMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.LeftBottomEyelidDownMorphName);
			}
			builtInMorph = this._morphBank.GetBuiltInMorph(this.RightBottomEyelidDownMorphName);
			if (builtInMorph != null)
			{
				this.RightBottomEyelidDownMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.RightBottomEyelidDownMorphName);
			}
			builtInMorph = this._morphBank.GetBuiltInMorph(this.LeftBottomEyelidUpMorphName);
			if (builtInMorph != null)
			{
				this.LeftBottomEyelidUpMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.LeftBottomEyelidUpMorphName);
			}
			builtInMorph = this._morphBank.GetBuiltInMorph(this.RightBottomEyelidUpMorphName);
			if (builtInMorph != null)
			{
				this.RightBottomEyelidUpMorph = builtInMorph;
			}
			else
			{
				Debug.LogError("Could not get eyelid morph " + this.RightBottomEyelidUpMorphName);
			}
		}
	}

	// Token: 0x06004AB6 RID: 19126 RVA: 0x0019D2D5 File Offset: 0x0019B6D5
	private void Start()
	{
		this.InitMorphs();
	}

	// Token: 0x06004AB7 RID: 19127 RVA: 0x0019D2E0 File Offset: 0x0019B6E0
	private void UpdateBlink()
	{
		if (this.blinking)
		{
			if (this.currentWeight > this.targetWeight)
			{
				this.currentWeight -= Time.deltaTime / (this.blinkTime * (1f - this.blinkDownUpRatio));
			}
			else
			{
				this.currentWeight += Time.deltaTime / (this.blinkTime * this.blinkDownUpRatio);
			}
			if (this.currentWeight < 0f)
			{
				this.currentWeight = 0f;
				this.blinking = false;
			}
			else if (this.currentWeight > 1f)
			{
				this.currentWeight = 1f;
				this.BlinkOpen();
			}
			if (this.eyesControl != null)
			{
				this.eyesControl.blinkWeight = this.currentWeight;
			}
		}
		if (this.blinkEnabledJSON.val)
		{
			this.blinkStartTimer -= Time.deltaTime;
			if (this.blinkStartTimer < 0f)
			{
				this.Blink();
				this.blinkStartTimer = UnityEngine.Random.Range(this.blinkSpaceMin, this.blinkSpaceMax);
			}
		}
	}

	// Token: 0x06004AB8 RID: 19128 RVA: 0x0019D410 File Offset: 0x0019B810
	private void UpdateWeights()
	{
		this.leftEyeWeight = this.currentWeight;
		if (this.leftEye != null && this.eyelidLookMorphsEnabledJSON.val)
		{
			float x = Quaternion2Angles.GetAngles(this.leftEye.localRotation, Quaternion2Angles.RotationOrder.ZYX).x;
			if (x > 0f)
			{
				this.leftEyeWeight += x * this.lookDownTopEyelidFactor;
				if (this.LeftBottomEyelidDownMorph != null)
				{
					this.LeftBottomEyelidDownMorph.morphValue = x * this.lookDownBottomEyelidFactor;
				}
				if (this.LeftTopEyelidUpMorph != null)
				{
					this.LeftTopEyelidUpMorph.morphValue = 0f;
				}
			}
			else
			{
				if (this.LeftBottomEyelidDownMorph != null)
				{
					this.LeftBottomEyelidDownMorph.morphValue = 0f;
				}
				if (this.LeftTopEyelidUpMorph != null)
				{
					float morphValue = -x * this.lookUpTopEyelidFactor * Mathf.Max(0f, 1f - this.leftEyeWeight);
					this.LeftTopEyelidUpMorph.morphValue = morphValue;
				}
				if (this.LeftBottomEyelidUpMorph != null)
				{
					this.LeftBottomEyelidUpMorph.morphValue = this.currentWeight * this.blinkBottomEyelidFactor + -x * this.lookUpBottomEyelidFactor;
				}
			}
		}
		if (this.LeftTopEyelidDownMorph != null)
		{
			this.LeftTopEyelidDownMorph.morphValue = this.leftEyeWeight;
		}
		this.rightEyeWeight = this.currentWeight;
		if (this.rightEye != null && this.eyelidLookMorphsEnabledJSON.val)
		{
			float x2 = Quaternion2Angles.GetAngles(this.rightEye.localRotation, Quaternion2Angles.RotationOrder.ZYX).x;
			if (x2 > 0f)
			{
				this.rightEyeWeight += x2 * this.lookDownTopEyelidFactor;
				if (this.RightBottomEyelidDownMorph != null)
				{
					this.RightBottomEyelidDownMorph.morphValue = x2 * this.lookDownBottomEyelidFactor;
				}
				if (this.RightTopEyelidUpMorph != null)
				{
					this.RightTopEyelidUpMorph.morphValue = 0f;
				}
				if (this.RightBottomEyelidUpMorph != null)
				{
					this.RightBottomEyelidUpMorph.morphValue = this.currentWeight * this.blinkBottomEyelidFactor;
				}
			}
			else
			{
				if (this.RightBottomEyelidDownMorph != null)
				{
					this.RightBottomEyelidDownMorph.morphValue = 0f;
				}
				if (this.RightTopEyelidUpMorph != null)
				{
					float morphValue2 = -x2 * this.lookUpTopEyelidFactor * Mathf.Max(0f, 1f - this.rightEyeWeight);
					this.RightTopEyelidUpMorph.morphValue = morphValue2;
				}
				if (this.RightBottomEyelidUpMorph != null)
				{
					this.RightBottomEyelidUpMorph.morphValue = this.currentWeight * this.blinkBottomEyelidFactor + -x2 * this.lookUpBottomEyelidFactor;
				}
			}
		}
		if (this.RightTopEyelidDownMorph != null)
		{
			this.RightTopEyelidDownMorph.morphValue = this.rightEyeWeight;
		}
	}

	// Token: 0x06004AB9 RID: 19129 RVA: 0x0019D6CA File Offset: 0x0019BACA
	private void Update()
	{
		this.UpdateBlink();
		this.UpdateWeights();
	}

	// Token: 0x06004ABA RID: 19130 RVA: 0x0019D6D8 File Offset: 0x0019BAD8
	protected void Init()
	{
		this.blinkEnabledJSON = new JSONStorableBool("blinkEnabled", true, new JSONStorableBool.SetBoolCallback(this.SyncBlinkEnabled));
		this.blinkEnabledJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.blinkEnabledJSON);
		this.eyelidLookMorphsEnabledJSON = new JSONStorableBool("eyelidLookMorphsEnabled", true, new JSONStorableBool.SetBoolCallback(this.SyncEyelidLookMorphsEnabled));
		base.RegisterBool(this.eyelidLookMorphsEnabledJSON);
	}

	// Token: 0x06004ABB RID: 19131 RVA: 0x0019D744 File Offset: 0x0019BB44
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			DAZMeshEyelidControlUI componentInChildren = this.UITransform.GetComponentInChildren<DAZMeshEyelidControlUI>(true);
			if (componentInChildren != null)
			{
				if (this.blinkEnabledJSON != null)
				{
					this.blinkEnabledJSON.toggle = componentInChildren.blinkEnabledToggle;
				}
				if (this.eyelidLookMorphsEnabledJSON != null)
				{
					this.eyelidLookMorphsEnabledJSON.toggle = componentInChildren.eyelidLookMorphsEnabledToggle;
				}
			}
		}
	}

	// Token: 0x06004ABC RID: 19132 RVA: 0x0019D7B4 File Offset: 0x0019BBB4
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			DAZMeshEyelidControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<DAZMeshEyelidControlUI>(true);
			if (componentInChildren != null)
			{
				if (this.blinkEnabledJSON != null)
				{
					this.blinkEnabledJSON.toggleAlt = componentInChildren.blinkEnabledToggle;
				}
				if (this.eyelidLookMorphsEnabledJSON != null)
				{
					this.eyelidLookMorphsEnabledJSON.toggleAlt = componentInChildren.eyelidLookMorphsEnabledToggle;
				}
			}
		}
	}

	// Token: 0x06004ABD RID: 19133 RVA: 0x0019D823 File Offset: 0x0019BC23
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

	// Token: 0x04003979 RID: 14713
	[SerializeField]
	private DAZMorphBank _morphBank;

	// Token: 0x0400397A RID: 14714
	public Transform leftEye;

	// Token: 0x0400397B RID: 14715
	public Transform rightEye;

	// Token: 0x0400397C RID: 14716
	public EyesControl eyesControl;

	// Token: 0x0400397D RID: 14717
	public string LeftTopEyelidDownMorphName;

	// Token: 0x0400397E RID: 14718
	public string RightTopEyelidDownMorphName;

	// Token: 0x0400397F RID: 14719
	public string LeftBottomEyelidUpMorphName;

	// Token: 0x04003980 RID: 14720
	public string RightBottomEyelidUpMorphName;

	// Token: 0x04003981 RID: 14721
	public string LeftTopEyelidUpMorphName;

	// Token: 0x04003982 RID: 14722
	public string RightTopEyelidUpMorphName;

	// Token: 0x04003983 RID: 14723
	public string LeftBottomEyelidDownMorphName;

	// Token: 0x04003984 RID: 14724
	public string RightBottomEyelidDownMorphName;

	// Token: 0x04003985 RID: 14725
	public float blinkSpaceMin = 1f;

	// Token: 0x04003986 RID: 14726
	public float blinkSpaceMax = 7f;

	// Token: 0x04003987 RID: 14727
	public float blinkTimeMin = 0.1f;

	// Token: 0x04003988 RID: 14728
	public float blinkTimeMax = 0.4f;

	// Token: 0x04003989 RID: 14729
	public float blinkDownUpRatio = 0.4f;

	// Token: 0x0400398A RID: 14730
	public float blinkBottomEyelidFactor = 0.5f;

	// Token: 0x0400398B RID: 14731
	public float lookUpTopEyelidFactor = 3f;

	// Token: 0x0400398C RID: 14732
	public float lookUpBottomEyelidFactor = 1f;

	// Token: 0x0400398D RID: 14733
	public float lookDownTopEyelidFactor = 1.5f;

	// Token: 0x0400398E RID: 14734
	public float lookDownBottomEyelidFactor = 4f;

	// Token: 0x0400398F RID: 14735
	private DAZMorph LeftTopEyelidDownMorph;

	// Token: 0x04003990 RID: 14736
	private DAZMorph RightTopEyelidDownMorph;

	// Token: 0x04003991 RID: 14737
	private DAZMorph LeftBottomEyelidUpMorph;

	// Token: 0x04003992 RID: 14738
	private DAZMorph RightBottomEyelidUpMorph;

	// Token: 0x04003993 RID: 14739
	private DAZMorph LeftTopEyelidUpMorph;

	// Token: 0x04003994 RID: 14740
	private DAZMorph RightTopEyelidUpMorph;

	// Token: 0x04003995 RID: 14741
	private DAZMorph LeftBottomEyelidDownMorph;

	// Token: 0x04003996 RID: 14742
	private DAZMorph RightBottomEyelidDownMorph;

	// Token: 0x04003997 RID: 14743
	protected JSONStorableBool blinkEnabledJSON;

	// Token: 0x04003998 RID: 14744
	protected JSONStorableBool eyelidLookMorphsEnabledJSON;

	// Token: 0x04003999 RID: 14745
	private bool closed;

	// Token: 0x0400399A RID: 14746
	private bool blinking;

	// Token: 0x0400399B RID: 14747
	private float blinkStartTimer;

	// Token: 0x0400399C RID: 14748
	public float blinkTime;

	// Token: 0x0400399D RID: 14749
	public float currentWeight;

	// Token: 0x0400399E RID: 14750
	public float leftEyeWeight;

	// Token: 0x0400399F RID: 14751
	public float rightEyeWeight;

	// Token: 0x040039A0 RID: 14752
	public float targetWeight;
}
