using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BC3 RID: 3011
public class SetTransformScaleIndependent : JSONStorable
{
	// Token: 0x060055B0 RID: 21936 RVA: 0x001F541D File Offset: 0x001F381D
	public SetTransformScaleIndependent()
	{
	}

	// Token: 0x060055B1 RID: 21937 RVA: 0x001F5458 File Offset: 0x001F3858
	protected void SyncScale(float f)
	{
		this._scale = f;
		this.SyncScale();
	}

	// Token: 0x060055B2 RID: 21938 RVA: 0x001F5467 File Offset: 0x001F3867
	protected void SyncScaleX(float f)
	{
		this._scaleX = f;
		this.SyncScale();
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x001F5476 File Offset: 0x001F3876
	protected void SyncScaleY(float f)
	{
		this._scaleY = f;
		this.SyncScale();
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x001F5485 File Offset: 0x001F3885
	protected void SyncScaleZ(float f)
	{
		this._scaleZ = f;
		this.SyncScale();
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x001F5494 File Offset: 0x001F3894
	protected void SyncScale()
	{
		Vector3 localScale;
		localScale.x = this.scaleXJSON.val * this.scaleJSON.val;
		localScale.y = this.scaleYJSON.val * this.scaleJSON.val;
		localScale.z = this.scaleZJSON.val * this.scaleJSON.val;
		if (this.additionalScaleTransform != null)
		{
			if (this.alignAdditionalScaleTransform)
			{
				List<Transform> list = new List<Transform>();
				IEnumerator enumerator = this.additionalScaleTransform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform item = (Transform)obj;
						list.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				foreach (Transform transform in list)
				{
					transform.SetParent(null, true);
				}
				this.additionalScaleTransform.position = base.transform.position;
				this.additionalScaleTransform.rotation = base.transform.rotation;
				foreach (Transform transform2 in list)
				{
					transform2.SetParent(this.additionalScaleTransform, true);
					transform2.localScale = Vector3.one;
				}
			}
			this.additionalScaleTransform.localScale = localScale;
		}
		base.transform.localScale = localScale;
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x001F5660 File Offset: 0x001F3A60
	protected void Init()
	{
		this.scaleXJSON = new JSONStorableFloat("scaleX", this._scaleX, new JSONStorableFloat.SetFloatCallback(this.SyncScaleX), 0.01f, 10f, true, true);
		base.RegisterFloat(this.scaleXJSON);
		this.scaleYJSON = new JSONStorableFloat("scaleY", this._scaleY, new JSONStorableFloat.SetFloatCallback(this.SyncScaleY), 0.01f, 10f, true, true);
		base.RegisterFloat(this.scaleYJSON);
		this.scaleZJSON = new JSONStorableFloat("scaleZ", this._scaleZ, new JSONStorableFloat.SetFloatCallback(this.SyncScaleZ), 0.01f, 10f, true, true);
		base.RegisterFloat(this.scaleZJSON);
		this.scaleJSON = new JSONStorableFloat("scale", this._scale, new JSONStorableFloat.SetFloatCallback(this.SyncScale), 0.01f, 10f, true, true);
		base.RegisterFloat(this.scaleJSON);
	}

	// Token: 0x060055B7 RID: 21943 RVA: 0x001F5758 File Offset: 0x001F3B58
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			SetTransformScaleIndependentUI componentInChildren = this.UITransform.GetComponentInChildren<SetTransformScaleIndependentUI>(true);
			if (componentInChildren != null)
			{
				this.scaleJSON.slider = componentInChildren.scaleSlider;
				this.scaleXJSON.slider = componentInChildren.scaleXSlider;
				this.scaleYJSON.slider = componentInChildren.scaleYSlider;
				this.scaleZJSON.slider = componentInChildren.scaleZSlider;
			}
		}
	}

	// Token: 0x060055B8 RID: 21944 RVA: 0x001F57D4 File Offset: 0x001F3BD4
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			SetTransformScaleIndependentUI componentInChildren = this.UITransformAlt.GetComponentInChildren<SetTransformScaleIndependentUI>(true);
			if (componentInChildren != null)
			{
				this.scaleJSON.sliderAlt = componentInChildren.scaleSlider;
				this.scaleXJSON.sliderAlt = componentInChildren.scaleXSlider;
				this.scaleYJSON.sliderAlt = componentInChildren.scaleYSlider;
				this.scaleZJSON.sliderAlt = componentInChildren.scaleZSlider;
			}
		}
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x001F584F File Offset: 0x001F3C4F
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

	// Token: 0x040046D1 RID: 18129
	public Transform additionalScaleTransform;

	// Token: 0x040046D2 RID: 18130
	public bool alignAdditionalScaleTransform = true;

	// Token: 0x040046D3 RID: 18131
	protected JSONStorableFloat scaleJSON;

	// Token: 0x040046D4 RID: 18132
	protected JSONStorableFloat scaleXJSON;

	// Token: 0x040046D5 RID: 18133
	protected JSONStorableFloat scaleYJSON;

	// Token: 0x040046D6 RID: 18134
	protected JSONStorableFloat scaleZJSON;

	// Token: 0x040046D7 RID: 18135
	[SerializeField]
	protected float _scaleX = 1f;

	// Token: 0x040046D8 RID: 18136
	[SerializeField]
	protected float _scaleY = 1f;

	// Token: 0x040046D9 RID: 18137
	[SerializeField]
	protected float _scaleZ = 1f;

	// Token: 0x040046DA RID: 18138
	[SerializeField]
	protected float _scale = 1f;
}
