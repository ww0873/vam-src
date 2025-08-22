using System;
using UnityEngine;

// Token: 0x02000B14 RID: 2836
public class DAZSkinWrapControl : JSONStorable
{
	// Token: 0x06004D68 RID: 19816 RVA: 0x001B20F5 File Offset: 0x001B04F5
	public DAZSkinWrapControl()
	{
	}

	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x06004D69 RID: 19817 RVA: 0x001B20FD File Offset: 0x001B04FD
	// (set) Token: 0x06004D6A RID: 19818 RVA: 0x001B2108 File Offset: 0x001B0508
	public DAZSkinWrap wrap
	{
		get
		{
			return this._wrap;
		}
		set
		{
			if (this._wrap != value)
			{
				this._wrap = value;
				if (this.surfaceOffsetJSON != null)
				{
					this.surfaceOffsetJSON.val = this._wrap.surfaceOffset;
					this.surfaceOffsetJSON.defaultVal = this._wrap.defaultSurfaceOffset;
				}
				if (this.additionalThicknessMultiplierJSON != null)
				{
					this.additionalThicknessMultiplierJSON.val = this._wrap.additionalThicknessMultiplier;
					this.additionalThicknessMultiplierJSON.defaultVal = this._wrap.defaultAdditionalThicknessMultiplier;
				}
				if (this.wrapToSmoothedVertsJSON != null)
				{
					this.wrapToSmoothedVertsJSON.val = !this._wrap.forceRawSkinVerts;
					this.wrapToSmoothedVertsJSON.defaultVal = !this._wrap.forceRawSkinVerts;
				}
				if (this.smoothIterationsJSON != null)
				{
					this.smoothIterationsJSON.val = (float)this._wrap.smoothOuterLoops;
					this.smoothIterationsJSON.defaultVal = (float)this._wrap.smoothOuterLoops;
				}
			}
		}
	}

	// Token: 0x17000B09 RID: 2825
	// (get) Token: 0x06004D6B RID: 19819 RVA: 0x001B2211 File Offset: 0x001B0611
	// (set) Token: 0x06004D6C RID: 19820 RVA: 0x001B221C File Offset: 0x001B061C
	public DAZSkinWrap wrap2
	{
		get
		{
			return this._wrap2;
		}
		set
		{
			if (this._wrap2 != value)
			{
				this._wrap2 = value;
				if (this.surfaceOffsetJSON != null)
				{
					this._wrap2.surfaceOffset = this.surfaceOffsetJSON.val;
				}
				if (this.additionalThicknessMultiplierJSON != null)
				{
					this._wrap2.additionalThicknessMultiplier = this.additionalThicknessMultiplierJSON.val;
				}
				if (this.wrapToSmoothedVertsJSON != null)
				{
					this._wrap2.forceRawSkinVerts = !this.wrapToSmoothedVertsJSON.val;
				}
				if (this.smoothIterationsJSON != null)
				{
					this._wrap2.smoothOuterLoops = Mathf.FloorToInt(this.smoothIterationsJSON.val);
				}
			}
		}
	}

	// Token: 0x06004D6D RID: 19821 RVA: 0x001B22CD File Offset: 0x001B06CD
	protected void SyncSurfaceOffset(float f)
	{
		if (this._wrap != null)
		{
			this._wrap.surfaceOffset = f;
		}
		if (this._wrap2 != null)
		{
			this._wrap2.surfaceOffset = f;
		}
	}

	// Token: 0x06004D6E RID: 19822 RVA: 0x001B2309 File Offset: 0x001B0709
	protected void SyncAdditionalThicknessMultiplier(float f)
	{
		if (this._wrap != null)
		{
			this._wrap.additionalThicknessMultiplier = f;
		}
		if (this._wrap2 != null)
		{
			this._wrap2.additionalThicknessMultiplier = f;
		}
	}

	// Token: 0x06004D6F RID: 19823 RVA: 0x001B2348 File Offset: 0x001B0748
	protected void SyncWrapToSmoothedVerts(bool b)
	{
		if (this._wrap != null)
		{
			this._wrap.forceRawSkinVerts = !b;
		}
		if (this._wrap2 != null)
		{
			this._wrap2.forceRawSkinVerts = !b;
		}
	}

	// Token: 0x06004D70 RID: 19824 RVA: 0x001B2398 File Offset: 0x001B0798
	protected void SyncSmoothIterations(float f)
	{
		int smoothOuterLoops = Mathf.FloorToInt(f);
		if (this._wrap != null)
		{
			this._wrap.smoothOuterLoops = smoothOuterLoops;
		}
		if (this._wrap2 != null)
		{
			this._wrap2.smoothOuterLoops = smoothOuterLoops;
		}
	}

	// Token: 0x06004D71 RID: 19825 RVA: 0x001B23E8 File Offset: 0x001B07E8
	protected void Init()
	{
		if (this._wrap == null)
		{
			DAZSkinWrap[] components = base.GetComponents<DAZSkinWrap>();
			foreach (DAZSkinWrap dazskinWrap in components)
			{
				if (dazskinWrap.enabled && dazskinWrap.draw)
				{
					this._wrap = dazskinWrap;
					break;
				}
			}
		}
		float startingValue = 0f;
		float startingValue2 = 0f;
		bool startingValue3 = false;
		float startingValue4 = 0f;
		if (this._wrap != null)
		{
			startingValue = this._wrap.surfaceOffset;
			startingValue2 = this._wrap.additionalThicknessMultiplier;
			startingValue3 = !this._wrap.forceRawSkinVerts;
			startingValue4 = (float)this._wrap.smoothOuterLoops;
		}
		this.surfaceOffsetJSON = new JSONStorableFloat("surfaceOffset", startingValue, new JSONStorableFloat.SetFloatCallback(this.SyncSurfaceOffset), -0.01f, 0.01f, false, true);
		base.RegisterFloat(this.surfaceOffsetJSON);
		this.additionalThicknessMultiplierJSON = new JSONStorableFloat("additionalThicknessMultiplier", startingValue2, new JSONStorableFloat.SetFloatCallback(this.SyncAdditionalThicknessMultiplier), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.additionalThicknessMultiplierJSON);
		this.wrapToSmoothedVertsJSON = new JSONStorableBool("wrapToSmoothedVerts", startingValue3, new JSONStorableBool.SetBoolCallback(this.SyncWrapToSmoothedVerts));
		base.RegisterBool(this.wrapToSmoothedVertsJSON);
		this.smoothIterationsJSON = new JSONStorableFloat("smoothIterations", startingValue4, new JSONStorableFloat.SetFloatCallback(this.SyncSmoothIterations), 0f, 3f, true, true);
		base.RegisterFloat(this.smoothIterationsJSON);
	}

	// Token: 0x06004D72 RID: 19826 RVA: 0x001B2578 File Offset: 0x001B0978
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			DAZSkinWrapControlUI componentInChildren = t.GetComponentInChildren<DAZSkinWrapControlUI>(true);
			if (componentInChildren != null)
			{
				if (this.surfaceOffsetJSON != null)
				{
					this.surfaceOffsetJSON.RegisterSlider(componentInChildren.surfaceOffsetSlider, isAlt);
				}
				if (this.additionalThicknessMultiplierJSON != null)
				{
					this.additionalThicknessMultiplierJSON.RegisterSlider(componentInChildren.additionalThicknessMultiplierSlider, isAlt);
				}
				if (this.wrapToSmoothedVertsJSON != null)
				{
					this.wrapToSmoothedVertsJSON.RegisterToggle(componentInChildren.wrapToSmoothedVertsToggle, isAlt);
				}
				if (this.smoothIterationsJSON != null)
				{
					this.smoothIterationsJSON.RegisterSlider(componentInChildren.smoothIterationsSlider, isAlt);
				}
			}
		}
	}

	// Token: 0x06004D73 RID: 19827 RVA: 0x001B2619 File Offset: 0x001B0A19
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

	// Token: 0x04003D37 RID: 15671
	[SerializeField]
	protected DAZSkinWrap _wrap;

	// Token: 0x04003D38 RID: 15672
	[SerializeField]
	protected DAZSkinWrap _wrap2;

	// Token: 0x04003D39 RID: 15673
	protected JSONStorableFloat surfaceOffsetJSON;

	// Token: 0x04003D3A RID: 15674
	protected JSONStorableFloat additionalThicknessMultiplierJSON;

	// Token: 0x04003D3B RID: 15675
	protected JSONStorableBool wrapToSmoothedVertsJSON;

	// Token: 0x04003D3C RID: 15676
	protected JSONStorableFloat smoothIterationsJSON;
}
