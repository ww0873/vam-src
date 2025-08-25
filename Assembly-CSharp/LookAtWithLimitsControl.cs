using System;
using UnityEngine;

// Token: 0x02000BB2 RID: 2994
public class LookAtWithLimitsControl : JSONStorable
{
	// Token: 0x0600555C RID: 21852 RVA: 0x001E3CF3 File Offset: 0x001E20F3
	public LookAtWithLimitsControl()
	{
	}

	// Token: 0x0600555D RID: 21853 RVA: 0x001E3CFC File Offset: 0x001E20FC
	protected void SyncMaxLeft(float f)
	{
		this._maxLeft = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.MaxLeft = this._maxLeft;
		}
		if (this.lookAt2 != null)
		{
			if (this.lookAt2InvertRightLeft)
			{
				this.lookAt2.MaxRight = this._maxLeft;
			}
			else
			{
				this.lookAt2.MaxLeft = this._maxLeft;
			}
		}
	}

	// Token: 0x0600555E RID: 21854 RVA: 0x001E3D78 File Offset: 0x001E2178
	protected void SyncMaxRight(float f)
	{
		this._maxRight = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.MaxRight = this._maxRight;
		}
		if (this.lookAt2 != null)
		{
			if (this.lookAt2InvertRightLeft)
			{
				this.lookAt2.MaxLeft = this._maxRight;
			}
			else
			{
				this.lookAt2.MaxRight = this._maxRight;
			}
		}
	}

	// Token: 0x0600555F RID: 21855 RVA: 0x001E3DF4 File Offset: 0x001E21F4
	protected void SyncMaxUp(float f)
	{
		this._maxUp = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.MaxUp = this._maxUp;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.MaxUp = this._maxUp;
		}
	}

	// Token: 0x06005560 RID: 21856 RVA: 0x001E3E4C File Offset: 0x001E224C
	protected void SyncMaxDown(float f)
	{
		this._maxDown = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.MaxDown = this._maxDown;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.MaxDown = this._maxDown;
		}
	}

	// Token: 0x06005561 RID: 21857 RVA: 0x001E3EA4 File Offset: 0x001E22A4
	protected void SyncMinEngageDistance(float f)
	{
		this._minEngageDistance = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.MinEngageDistance = this._minEngageDistance;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.MinEngageDistance = this._minEngageDistance;
		}
	}

	// Token: 0x06005562 RID: 21858 RVA: 0x001E3EFC File Offset: 0x001E22FC
	protected void SyncLookSpeed(float f)
	{
		this._lookSpeed = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.smoothFactor = this._lookSpeed;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.smoothFactor = this._lookSpeed;
		}
	}

	// Token: 0x06005563 RID: 21859 RVA: 0x001E3F54 File Offset: 0x001E2354
	protected virtual void SyncLookAtLeftRightAngleAdjust()
	{
		if (this.lookAt1 != null)
		{
			this.lookAt1.LeftRightAngleAdjust = this._leftRightAngleAdjust;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.LeftRightAngleAdjust = this._leftRightAngleAdjust;
		}
	}

	// Token: 0x06005564 RID: 21860 RVA: 0x001E3FA5 File Offset: 0x001E23A5
	protected void SyncLeftRightAngleAdjust(float f)
	{
		this._leftRightAngleAdjust = f;
		this.SyncLookAtLeftRightAngleAdjust();
	}

	// Token: 0x06005565 RID: 21861 RVA: 0x001E3FB4 File Offset: 0x001E23B4
	protected void SyncUpDownAngleAdjust(float f)
	{
		this._upDownAngleAdjust = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.UpDownAngleAdjust = this._upDownAngleAdjust;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.UpDownAngleAdjust = this._upDownAngleAdjust;
		}
	}

	// Token: 0x06005566 RID: 21862 RVA: 0x001E400C File Offset: 0x001E240C
	protected void SyncDepthAdjust(float f)
	{
		this._depthAdjust = f;
		if (this.lookAt1 != null)
		{
			this.lookAt1.DepthAdjust = this._depthAdjust;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.DepthAdjust = this._depthAdjust;
		}
	}

	// Token: 0x06005567 RID: 21863 RVA: 0x001E4064 File Offset: 0x001E2464
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			LookAtWithLimitsControlUI componentInChildren = t.GetComponentInChildren<LookAtWithLimitsControlUI>(true);
			if (componentInChildren != null)
			{
				this.maxDownJSON.RegisterSlider(componentInChildren.maxDownSlider, false);
				this.maxUpJSON.RegisterSlider(componentInChildren.maxUpSlider, false);
				this.maxLeftJSON.RegisterSlider(componentInChildren.maxLeftSlider, false);
				this.maxRightJSON.RegisterSlider(componentInChildren.maxRightSlider, false);
				this.minEngageDistanceJSON.RegisterSlider(componentInChildren.minEngageDistanceSlider, false);
				this.lookSpeedJSON.RegisterSlider(componentInChildren.lookSpeedSlider, false);
				this.leftRightAngleAdjustJSON.RegisterSlider(componentInChildren.leftRightAngleAdjustSlider, false);
				this.upDownAngleAdjustJSON.RegisterSlider(componentInChildren.upDownAngleAdjustSlider, false);
				this.depthAdjustJSON.RegisterSlider(componentInChildren.depthAdjustSlider, false);
			}
		}
	}

	// Token: 0x06005568 RID: 21864 RVA: 0x001E4134 File Offset: 0x001E2534
	protected virtual void Init()
	{
		LookAtWithLimits lookAtWithLimits;
		if (this.lookAt1 != null)
		{
			lookAtWithLimits = this.lookAt1;
		}
		else
		{
			lookAtWithLimits = this.lookAt2;
		}
		if (lookAtWithLimits != null)
		{
			this._maxDown = lookAtWithLimits.MaxDown;
			this._maxUp = lookAtWithLimits.MaxUp;
			this._maxLeft = lookAtWithLimits.MaxLeft;
			this._maxRight = lookAtWithLimits.MaxRight;
			this._minEngageDistance = lookAtWithLimits.MinEngageDistance;
			this._lookSpeed = lookAtWithLimits.smoothFactor;
			this.SyncMaxUp(this._maxUp);
			this.SyncMaxLeft(this._maxLeft);
			this.SyncMaxRight(this._maxRight);
			this.SyncMinEngageDistance(this._minEngageDistance);
			this.SyncLookSpeed(this._lookSpeed);
		}
		this.maxDownJSON = new JSONStorableFloat("maxDown", this._maxDown, new JSONStorableFloat.SetFloatCallback(this.SyncMaxDown), 0f, 89f, true, true);
		this.maxDownJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.maxDownJSON);
		this.maxUpJSON = new JSONStorableFloat("maxUp", this._maxUp, new JSONStorableFloat.SetFloatCallback(this.SyncMaxUp), 0f, 89f, true, true);
		this.maxUpJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.maxUpJSON);
		this.maxLeftJSON = new JSONStorableFloat("maxLeft", this._maxLeft, new JSONStorableFloat.SetFloatCallback(this.SyncMaxLeft), 0f, 89f, true, true);
		this.maxLeftJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.maxLeftJSON);
		this.maxRightJSON = new JSONStorableFloat("maxRight", this._maxRight, new JSONStorableFloat.SetFloatCallback(this.SyncMaxRight), 0f, 89f, true, true);
		this.maxRightJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.maxRightJSON);
		this.minEngageDistanceJSON = new JSONStorableFloat("minEngageDistance", this._minEngageDistance, new JSONStorableFloat.SetFloatCallback(this.SyncMinEngageDistance), 0f, 1f, false, true);
		this.minEngageDistanceJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.minEngageDistanceJSON);
		this.lookSpeedJSON = new JSONStorableFloat("lookSpeed", this._lookSpeed, new JSONStorableFloat.SetFloatCallback(this.SyncLookSpeed), 0f, 50f, true, true);
		this.lookSpeedJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.lookSpeedJSON);
		this.leftRightAngleAdjustJSON = new JSONStorableFloat("leftRightAngleAdjust", this._leftRightAngleAdjust, new JSONStorableFloat.SetFloatCallback(this.SyncLeftRightAngleAdjust), -60f, 60f, true, true);
		this.leftRightAngleAdjustJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.leftRightAngleAdjustJSON);
		this.upDownAngleAdjustJSON = new JSONStorableFloat("upDownAngleAdjust", this._upDownAngleAdjust, new JSONStorableFloat.SetFloatCallback(this.SyncUpDownAngleAdjust), -60f, 60f, true, true);
		this.upDownAngleAdjustJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.upDownAngleAdjustJSON);
		this.depthAdjustJSON = new JSONStorableFloat("depthAdjust", this._depthAdjust, new JSONStorableFloat.SetFloatCallback(this.SyncDepthAdjust), -0.5f, 0.5f, true, true);
		this.depthAdjustJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.depthAdjustJSON);
	}

	// Token: 0x04004666 RID: 18022
	public LookAtWithLimits lookAt1;

	// Token: 0x04004667 RID: 18023
	public LookAtWithLimits lookAt2;

	// Token: 0x04004668 RID: 18024
	public bool lookAt2InvertRightLeft;

	// Token: 0x04004669 RID: 18025
	protected float _maxLeft;

	// Token: 0x0400466A RID: 18026
	protected JSONStorableFloat maxLeftJSON;

	// Token: 0x0400466B RID: 18027
	protected float _maxRight;

	// Token: 0x0400466C RID: 18028
	protected JSONStorableFloat maxRightJSON;

	// Token: 0x0400466D RID: 18029
	protected float _maxUp;

	// Token: 0x0400466E RID: 18030
	protected JSONStorableFloat maxUpJSON;

	// Token: 0x0400466F RID: 18031
	protected float _maxDown;

	// Token: 0x04004670 RID: 18032
	protected JSONStorableFloat maxDownJSON;

	// Token: 0x04004671 RID: 18033
	protected float _minEngageDistance;

	// Token: 0x04004672 RID: 18034
	protected JSONStorableFloat minEngageDistanceJSON;

	// Token: 0x04004673 RID: 18035
	protected float _lookSpeed;

	// Token: 0x04004674 RID: 18036
	protected JSONStorableFloat lookSpeedJSON;

	// Token: 0x04004675 RID: 18037
	protected float _leftRightAngleAdjust;

	// Token: 0x04004676 RID: 18038
	protected JSONStorableFloat leftRightAngleAdjustJSON;

	// Token: 0x04004677 RID: 18039
	protected float _upDownAngleAdjust;

	// Token: 0x04004678 RID: 18040
	protected JSONStorableFloat upDownAngleAdjustJSON;

	// Token: 0x04004679 RID: 18041
	protected float _depthAdjust;

	// Token: 0x0400467A RID: 18042
	protected JSONStorableFloat depthAdjustJSON;
}
