using System;

// Token: 0x02000AE7 RID: 2791
public class DAZMeshDrawOffsetControl : JSONStorable
{
	// Token: 0x06004A86 RID: 19078 RVA: 0x0019C61F File Offset: 0x0019AA1F
	public DAZMeshDrawOffsetControl()
	{
	}

	// Token: 0x06004A87 RID: 19079 RVA: 0x0019C654 File Offset: 0x0019AA54
	protected void SyncMesh()
	{
		if (this.mesh != null)
		{
			this.mesh.useDrawOffset = true;
			this.mesh.drawOffset.x = this._xPositionOffset;
			this.mesh.drawOffset.y = this._yPositionOffset;
			this.mesh.drawOffset.z = this._zPositionOffset;
			this.mesh.drawOffsetRotation.x = this._xRotationOffset;
			this.mesh.drawOffsetRotation.y = this._yRotationOffset;
			this.mesh.drawOffsetRotation.z = this._zRotationOffset;
			this.mesh.drawOffsetOverallScale = this._overallScale;
			this.mesh.drawOffsetScale.x = this._xScale;
			this.mesh.drawOffsetScale.y = this._yScale;
			this.mesh.drawOffsetScale.z = this._zScale;
		}
	}

	// Token: 0x06004A88 RID: 19080 RVA: 0x0019C755 File Offset: 0x0019AB55
	protected void SyncXPositionOffset(float f)
	{
		this._xPositionOffset = f;
		this.SyncMesh();
	}

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x06004A89 RID: 19081 RVA: 0x0019C764 File Offset: 0x0019AB64
	// (set) Token: 0x06004A8A RID: 19082 RVA: 0x0019C76C File Offset: 0x0019AB6C
	public float xPositionOffset
	{
		get
		{
			return this._xPositionOffset;
		}
		set
		{
			if (this.xPositionOffsetJSON != null)
			{
				this.xPositionOffsetJSON.val = value;
			}
			else if (this._xPositionOffset != value)
			{
				this.SyncXPositionOffset(value);
			}
		}
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x0019C79D File Offset: 0x0019AB9D
	protected void SyncYPositionOffset(float f)
	{
		this._yPositionOffset = f;
		this.SyncMesh();
	}

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x06004A8C RID: 19084 RVA: 0x0019C7AC File Offset: 0x0019ABAC
	// (set) Token: 0x06004A8D RID: 19085 RVA: 0x0019C7B4 File Offset: 0x0019ABB4
	public float yPositionOffset
	{
		get
		{
			return this._yPositionOffset;
		}
		set
		{
			if (this.yPositionOffsetJSON != null)
			{
				this.yPositionOffsetJSON.val = value;
			}
			else if (this._yPositionOffset != value)
			{
				this.SyncYPositionOffset(value);
			}
		}
	}

	// Token: 0x06004A8E RID: 19086 RVA: 0x0019C7E5 File Offset: 0x0019ABE5
	protected void SyncZPositionOffset(float f)
	{
		this._zPositionOffset = f;
		this.SyncMesh();
	}

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x06004A8F RID: 19087 RVA: 0x0019C7F4 File Offset: 0x0019ABF4
	// (set) Token: 0x06004A90 RID: 19088 RVA: 0x0019C7FC File Offset: 0x0019ABFC
	public float zPositionOffset
	{
		get
		{
			return this._zPositionOffset;
		}
		set
		{
			if (this.zPositionOffsetJSON != null)
			{
				this.zPositionOffsetJSON.val = value;
			}
			else if (this._zPositionOffset != value)
			{
				this.SyncZPositionOffset(value);
			}
		}
	}

	// Token: 0x06004A91 RID: 19089 RVA: 0x0019C82D File Offset: 0x0019AC2D
	protected void SyncXRotationOffset(float f)
	{
		this._xRotationOffset = f;
		this.SyncMesh();
	}

	// Token: 0x17000A96 RID: 2710
	// (get) Token: 0x06004A92 RID: 19090 RVA: 0x0019C83C File Offset: 0x0019AC3C
	// (set) Token: 0x06004A93 RID: 19091 RVA: 0x0019C844 File Offset: 0x0019AC44
	public float xRotationOffset
	{
		get
		{
			return this._xRotationOffset;
		}
		set
		{
			if (this.xRotationOffsetJSON != null)
			{
				this.xRotationOffsetJSON.val = value;
			}
			else if (this._xRotationOffset != value)
			{
				this.SyncXRotationOffset(value);
			}
		}
	}

	// Token: 0x06004A94 RID: 19092 RVA: 0x0019C875 File Offset: 0x0019AC75
	protected void SyncYRotationOffset(float f)
	{
		this._yRotationOffset = f;
		this.SyncMesh();
	}

	// Token: 0x17000A97 RID: 2711
	// (get) Token: 0x06004A95 RID: 19093 RVA: 0x0019C884 File Offset: 0x0019AC84
	// (set) Token: 0x06004A96 RID: 19094 RVA: 0x0019C88C File Offset: 0x0019AC8C
	public float yRotationOffset
	{
		get
		{
			return this._yRotationOffset;
		}
		set
		{
			if (this.yRotationOffsetJSON != null)
			{
				this.yRotationOffsetJSON.val = value;
			}
			else if (this._yRotationOffset != value)
			{
				this.SyncYRotationOffset(value);
			}
		}
	}

	// Token: 0x06004A97 RID: 19095 RVA: 0x0019C8BD File Offset: 0x0019ACBD
	protected void SyncZRotationOffset(float f)
	{
		this._zRotationOffset = f;
		this.SyncMesh();
	}

	// Token: 0x17000A98 RID: 2712
	// (get) Token: 0x06004A98 RID: 19096 RVA: 0x0019C8CC File Offset: 0x0019ACCC
	// (set) Token: 0x06004A99 RID: 19097 RVA: 0x0019C8D4 File Offset: 0x0019ACD4
	public float zRotationOffset
	{
		get
		{
			return this._zRotationOffset;
		}
		set
		{
			if (this.zRotationOffsetJSON != null)
			{
				this.zRotationOffsetJSON.val = value;
			}
			else if (this._zRotationOffset != value)
			{
				this.SyncZRotationOffset(value);
			}
		}
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x0019C905 File Offset: 0x0019AD05
	protected void SyncOverallScale(float f)
	{
		this._overallScale = f;
		this.SyncMesh();
	}

	// Token: 0x17000A99 RID: 2713
	// (get) Token: 0x06004A9B RID: 19099 RVA: 0x0019C914 File Offset: 0x0019AD14
	// (set) Token: 0x06004A9C RID: 19100 RVA: 0x0019C91C File Offset: 0x0019AD1C
	public float overallScale
	{
		get
		{
			return this._overallScale;
		}
		set
		{
			if (this.overallScaleJSON != null)
			{
				this.overallScaleJSON.val = value;
			}
			else if (this._overallScale != value)
			{
				this.SyncOverallScale(value);
			}
		}
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x0019C94D File Offset: 0x0019AD4D
	protected void SyncXScale(float f)
	{
		this._xScale = f;
		this.SyncMesh();
	}

	// Token: 0x17000A9A RID: 2714
	// (get) Token: 0x06004A9E RID: 19102 RVA: 0x0019C95C File Offset: 0x0019AD5C
	// (set) Token: 0x06004A9F RID: 19103 RVA: 0x0019C964 File Offset: 0x0019AD64
	public float xScale
	{
		get
		{
			return this._xScale;
		}
		set
		{
			if (this.xScaleJSON != null)
			{
				this.xScaleJSON.val = value;
			}
			else if (this._xScale != value)
			{
				this.SyncXScale(value);
			}
		}
	}

	// Token: 0x06004AA0 RID: 19104 RVA: 0x0019C995 File Offset: 0x0019AD95
	protected void SyncYScale(float f)
	{
		this._yScale = f;
		this.SyncMesh();
	}

	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x0019C9A4 File Offset: 0x0019ADA4
	// (set) Token: 0x06004AA2 RID: 19106 RVA: 0x0019C9AC File Offset: 0x0019ADAC
	public float yScale
	{
		get
		{
			return this._yScale;
		}
		set
		{
			if (this.yScaleJSON != null)
			{
				this.yScaleJSON.val = value;
			}
			else if (this._yScale != value)
			{
				this.SyncYScale(value);
			}
		}
	}

	// Token: 0x06004AA3 RID: 19107 RVA: 0x0019C9DD File Offset: 0x0019ADDD
	protected void SyncZScale(float f)
	{
		this._zScale = f;
		this.SyncMesh();
	}

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x06004AA4 RID: 19108 RVA: 0x0019C9EC File Offset: 0x0019ADEC
	// (set) Token: 0x06004AA5 RID: 19109 RVA: 0x0019C9F4 File Offset: 0x0019ADF4
	public float zScale
	{
		get
		{
			return this._zScale;
		}
		set
		{
			if (this.zScaleJSON != null)
			{
				this.zScaleJSON.val = value;
			}
			else if (this._zScale != value)
			{
				this.SyncZScale(value);
			}
		}
	}

	// Token: 0x06004AA6 RID: 19110 RVA: 0x0019CA28 File Offset: 0x0019AE28
	protected void Init()
	{
		if (this.mesh != null)
		{
			this._xPositionOffset = this.mesh.drawOffset.x;
			this._yPositionOffset = this.mesh.drawOffset.y;
			this._zPositionOffset = this.mesh.drawOffset.z;
			this._xRotationOffset = this.mesh.drawOffsetRotation.x;
			this._yRotationOffset = this.mesh.drawOffsetRotation.y;
			this._zRotationOffset = this.mesh.drawOffsetRotation.z;
			this._overallScale = this.mesh.drawOffsetOverallScale;
			this._xScale = this.mesh.drawOffsetScale.x;
			this._yScale = this.mesh.drawOffsetScale.y;
			this._zScale = this.mesh.drawOffsetScale.z;
		}
		this.xPositionOffsetJSON = new JSONStorableFloat("positionOffset:x", this._xPositionOffset, new JSONStorableFloat.SetFloatCallback(this.SyncXPositionOffset), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.xPositionOffsetJSON);
		this.yPositionOffsetJSON = new JSONStorableFloat("positionOffset:y", this._yPositionOffset, new JSONStorableFloat.SetFloatCallback(this.SyncYPositionOffset), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.yPositionOffsetJSON);
		this.zPositionOffsetJSON = new JSONStorableFloat("positionOffset:z", this._zPositionOffset, new JSONStorableFloat.SetFloatCallback(this.SyncZPositionOffset), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.zPositionOffsetJSON);
		this.xRotationOffsetJSON = new JSONStorableFloat("rotationOffset:x", this._xRotationOffset, new JSONStorableFloat.SetFloatCallback(this.SyncXRotationOffset), -180f, 180f, true, true);
		base.RegisterFloat(this.xRotationOffsetJSON);
		this.yRotationOffsetJSON = new JSONStorableFloat("rotationOffset:y", this._yRotationOffset, new JSONStorableFloat.SetFloatCallback(this.SyncYRotationOffset), -180f, 180f, true, true);
		base.RegisterFloat(this.yRotationOffsetJSON);
		this.zRotationOffsetJSON = new JSONStorableFloat("rotationOffset:z", this._zRotationOffset, new JSONStorableFloat.SetFloatCallback(this.SyncZRotationOffset), -180f, 180f, true, true);
		base.RegisterFloat(this.zRotationOffsetJSON);
		this.overallScaleJSON = new JSONStorableFloat("scale:overall", this._overallScale, new JSONStorableFloat.SetFloatCallback(this.SyncOverallScale), 0.1f, 2f, false, true);
		base.RegisterFloat(this.overallScaleJSON);
		this.xScaleJSON = new JSONStorableFloat("scale:x", this._xScale, new JSONStorableFloat.SetFloatCallback(this.SyncXScale), 0.1f, 2f, false, true);
		base.RegisterFloat(this.xScaleJSON);
		this.yScaleJSON = new JSONStorableFloat("scale:y", this._yScale, new JSONStorableFloat.SetFloatCallback(this.SyncYScale), 0.1f, 2f, false, true);
		base.RegisterFloat(this.yScaleJSON);
		this.zScaleJSON = new JSONStorableFloat("scale:z", this._zScale, new JSONStorableFloat.SetFloatCallback(this.SyncZScale), 0.1f, 2f, false, true);
		base.RegisterFloat(this.zScaleJSON);
	}

	// Token: 0x06004AA7 RID: 19111 RVA: 0x0019CD64 File Offset: 0x0019B164
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			DAZMeshDrawOffsetControlUI componentInChildren = this.UITransform.GetComponentInChildren<DAZMeshDrawOffsetControlUI>();
			if (componentInChildren != null)
			{
				this.xPositionOffsetJSON.slider = componentInChildren.xPositionOffsetSlider;
				this.yPositionOffsetJSON.slider = componentInChildren.yPositionOffsetSlider;
				this.zPositionOffsetJSON.slider = componentInChildren.zPositionOffsetSlider;
				this.xRotationOffsetJSON.slider = componentInChildren.xRotationOffsetSlider;
				this.yRotationOffsetJSON.slider = componentInChildren.yRotationOffsetSlider;
				this.zRotationOffsetJSON.slider = componentInChildren.zRotationOffsetSlider;
				this.overallScaleJSON.slider = componentInChildren.overallScaleSlider;
				this.xScaleJSON.slider = componentInChildren.xScaleSlider;
				this.yScaleJSON.slider = componentInChildren.yScaleSlider;
				this.zScaleJSON.slider = componentInChildren.zScaleSlider;
			}
		}
	}

	// Token: 0x06004AA8 RID: 19112 RVA: 0x0019CE44 File Offset: 0x0019B244
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x0400395A RID: 14682
	public DAZMesh mesh;

	// Token: 0x0400395B RID: 14683
	protected JSONStorableFloat xPositionOffsetJSON;

	// Token: 0x0400395C RID: 14684
	protected float _xPositionOffset;

	// Token: 0x0400395D RID: 14685
	protected JSONStorableFloat yPositionOffsetJSON;

	// Token: 0x0400395E RID: 14686
	protected float _yPositionOffset;

	// Token: 0x0400395F RID: 14687
	protected JSONStorableFloat zPositionOffsetJSON;

	// Token: 0x04003960 RID: 14688
	protected float _zPositionOffset;

	// Token: 0x04003961 RID: 14689
	protected JSONStorableFloat xRotationOffsetJSON;

	// Token: 0x04003962 RID: 14690
	protected float _xRotationOffset;

	// Token: 0x04003963 RID: 14691
	protected JSONStorableFloat yRotationOffsetJSON;

	// Token: 0x04003964 RID: 14692
	protected float _yRotationOffset;

	// Token: 0x04003965 RID: 14693
	protected JSONStorableFloat zRotationOffsetJSON;

	// Token: 0x04003966 RID: 14694
	protected float _zRotationOffset;

	// Token: 0x04003967 RID: 14695
	protected JSONStorableFloat overallScaleJSON;

	// Token: 0x04003968 RID: 14696
	protected float _overallScale = 1f;

	// Token: 0x04003969 RID: 14697
	protected JSONStorableFloat xScaleJSON;

	// Token: 0x0400396A RID: 14698
	protected float _xScale = 1f;

	// Token: 0x0400396B RID: 14699
	protected JSONStorableFloat yScaleJSON;

	// Token: 0x0400396C RID: 14700
	protected float _yScale = 1f;

	// Token: 0x0400396D RID: 14701
	protected JSONStorableFloat zScaleJSON;

	// Token: 0x0400396E RID: 14702
	protected float _zScale = 1f;
}
