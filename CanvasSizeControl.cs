using System;
using UnityEngine;

// Token: 0x02000C06 RID: 3078
public class CanvasSizeControl : JSONStorable
{
	// Token: 0x0600598B RID: 22923 RVA: 0x0020F24C File Offset: 0x0020D64C
	public CanvasSizeControl()
	{
	}

	// Token: 0x0600598C RID: 22924 RVA: 0x0020F26C File Offset: 0x0020D66C
	protected void SetCanvasSize()
	{
		if (this.controlledCanvas != null)
		{
			RectTransform component = this.controlledCanvas.GetComponent<RectTransform>();
			if (component != null)
			{
				Vector2 sizeDelta;
				sizeDelta.x = this.horSize;
				sizeDelta.y = this.vertSize;
				component.sizeDelta = sizeDelta;
			}
		}
	}

	// Token: 0x0600598D RID: 22925 RVA: 0x0020F2C3 File Offset: 0x0020D6C3
	protected void SyncHorSize(float f)
	{
		this._horSize = f;
		this.SetCanvasSize();
	}

	// Token: 0x17000D39 RID: 3385
	// (get) Token: 0x0600598E RID: 22926 RVA: 0x0020F2D2 File Offset: 0x0020D6D2
	// (set) Token: 0x0600598F RID: 22927 RVA: 0x0020F2DA File Offset: 0x0020D6DA
	public float horSize
	{
		get
		{
			return this._horSize;
		}
		set
		{
			if (this.horSizeJSON != null)
			{
				this.horSizeJSON.val = value;
			}
			else if (this._horSize != value)
			{
				this.SyncHorSize(value);
				this.SetCanvasSize();
			}
		}
	}

	// Token: 0x06005990 RID: 22928 RVA: 0x0020F311 File Offset: 0x0020D711
	protected void SyncVertSize(float f)
	{
		this._vertSize = f;
		this.SetCanvasSize();
	}

	// Token: 0x17000D3A RID: 3386
	// (get) Token: 0x06005991 RID: 22929 RVA: 0x0020F320 File Offset: 0x0020D720
	// (set) Token: 0x06005992 RID: 22930 RVA: 0x0020F328 File Offset: 0x0020D728
	public float vertSize
	{
		get
		{
			return this._vertSize;
		}
		set
		{
			if (this.vertSizeJSON != null)
			{
				this.vertSizeJSON.val = value;
			}
			else if (this._vertSize != value)
			{
				this.SyncVertSize(value);
				this.SetCanvasSize();
			}
		}
	}

	// Token: 0x06005993 RID: 22931 RVA: 0x0020F360 File Offset: 0x0020D760
	protected void Init()
	{
		this.horSizeJSON = new JSONStorableFloat("horizontalSize", this._horSize, new JSONStorableFloat.SetFloatCallback(this.SyncHorSize), 400f, 2000f, true, true);
		base.RegisterFloat(this.horSizeJSON);
		this.vertSizeJSON = new JSONStorableFloat("verticalSize", this._vertSize, new JSONStorableFloat.SetFloatCallback(this.SyncVertSize), 400f, 2000f, true, true);
		base.RegisterFloat(this.vertSizeJSON);
		this.SetCanvasSize();
	}

	// Token: 0x06005994 RID: 22932 RVA: 0x0020F3E8 File Offset: 0x0020D7E8
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			CanvasSizeControlUI componentInChildren = this.UITransform.GetComponentInChildren<CanvasSizeControlUI>(true);
			if (componentInChildren != null)
			{
				this.horSizeJSON.slider = componentInChildren.horSizeSlider;
				this.vertSizeJSON.slider = componentInChildren.vertSizeSlider;
			}
		}
	}

	// Token: 0x06005995 RID: 22933 RVA: 0x0020F444 File Offset: 0x0020D844
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			CanvasSizeControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<CanvasSizeControlUI>(true);
			if (componentInChildren != null)
			{
				this.horSizeJSON.sliderAlt = componentInChildren.horSizeSlider;
				this.vertSizeJSON.sliderAlt = componentInChildren.vertSizeSlider;
			}
		}
	}

	// Token: 0x06005996 RID: 22934 RVA: 0x0020F49D File Offset: 0x0020D89D
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

	// Token: 0x040049B1 RID: 18865
	public Canvas controlledCanvas;

	// Token: 0x040049B2 RID: 18866
	protected JSONStorableFloat horSizeJSON;

	// Token: 0x040049B3 RID: 18867
	[SerializeField]
	protected float _horSize = 1200f;

	// Token: 0x040049B4 RID: 18868
	protected JSONStorableFloat vertSizeJSON;

	// Token: 0x040049B5 RID: 18869
	[SerializeField]
	protected float _vertSize = 1000f;
}
