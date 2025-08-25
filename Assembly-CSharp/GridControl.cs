using System;
using UnityEngine;

// Token: 0x02000C18 RID: 3096
public class GridControl : JSONStorable
{
	// Token: 0x060059F5 RID: 23029 RVA: 0x002115FA File Offset: 0x0020F9FA
	public GridControl()
	{
	}

	// Token: 0x17000D45 RID: 3397
	// (get) Token: 0x060059F6 RID: 23030 RVA: 0x00211602 File Offset: 0x0020FA02
	public float positionGrid
	{
		get
		{
			if (this.positionGridJSON != null)
			{
				return this.positionGridJSON.val;
			}
			return 0.1f;
		}
	}

	// Token: 0x060059F7 RID: 23031 RVA: 0x00211620 File Offset: 0x0020FA20
	protected void SyncPositionGrid(float f)
	{
		float num = f * 1000f;
		num = Mathf.Round(num);
		num /= 1000f;
		this.positionGridJSON.valNoCallback = num;
	}

	// Token: 0x17000D46 RID: 3398
	// (get) Token: 0x060059F8 RID: 23032 RVA: 0x00211652 File Offset: 0x0020FA52
	public float rotationGrid
	{
		get
		{
			if (this.rotationGridJSON != null)
			{
				return this.rotationGridJSON.val;
			}
			return 15f;
		}
	}

	// Token: 0x060059F9 RID: 23033 RVA: 0x00211670 File Offset: 0x0020FA70
	protected void SyncRotationGrid(float f)
	{
		float num = f * 100f;
		num = Mathf.Round(num);
		num /= 100f;
		this.rotationGridJSON.valNoCallback = num;
	}

	// Token: 0x060059FA RID: 23034 RVA: 0x002116A4 File Offset: 0x0020FAA4
	protected void Init()
	{
		this.positionGridJSON = new JSONStorableFloat("positionGrid", 0.1f, new JSONStorableFloat.SetFloatCallback(this.SyncPositionGrid), 0.001f, 10f, false, true);
		base.RegisterFloat(this.positionGridJSON);
		this.rotationGridJSON = new JSONStorableFloat("rotationGrid", 15f, new JSONStorableFloat.SetFloatCallback(this.SyncRotationGrid), 0.01f, 90f, true, true);
		base.RegisterFloat(this.rotationGridJSON);
	}

	// Token: 0x060059FB RID: 23035 RVA: 0x00211724 File Offset: 0x0020FB24
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			GridControlUI componentInChildren = this.UITransform.GetComponentInChildren<GridControlUI>(true);
			if (componentInChildren != null)
			{
				this.positionGridJSON.slider = componentInChildren.positionGridSlider;
				this.rotationGridJSON.slider = componentInChildren.rotationGridSlider;
			}
		}
	}

	// Token: 0x060059FC RID: 23036 RVA: 0x00211780 File Offset: 0x0020FB80
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			GridControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<GridControlUI>(true);
			if (componentInChildren != null)
			{
				this.positionGridJSON.sliderAlt = componentInChildren.positionGridSlider;
				this.rotationGridJSON.sliderAlt = componentInChildren.rotationGridSlider;
			}
		}
	}

	// Token: 0x060059FD RID: 23037 RVA: 0x002117D9 File Offset: 0x0020FBD9
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			GridControl.singleton = this;
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x04004A32 RID: 18994
	public static GridControl singleton;

	// Token: 0x04004A33 RID: 18995
	protected JSONStorableFloat positionGridJSON;

	// Token: 0x04004A34 RID: 18996
	protected JSONStorableFloat rotationGridJSON;
}
