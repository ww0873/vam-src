using System;
using UnityEngine;

// Token: 0x02000BB9 RID: 3001
public class MoveAndRotateAsJSONStorable : JSONStorable
{
	// Token: 0x0600558A RID: 21898 RVA: 0x001F4665 File Offset: 0x001F2A65
	public MoveAndRotateAsJSONStorable()
	{
	}

	// Token: 0x0600558B RID: 21899 RVA: 0x001F467B File Offset: 0x001F2A7B
	protected void SyncXOffset(float f)
	{
		this.offset.x = f;
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x001F4689 File Offset: 0x001F2A89
	protected void SyncYOffset(float f)
	{
		this.offset.y = f;
	}

	// Token: 0x0600558D RID: 21901 RVA: 0x001F4697 File Offset: 0x001F2A97
	protected void SyncZOffset(float f)
	{
		this.offset.z = f;
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x001F46A8 File Offset: 0x001F2AA8
	public void DoUpdate()
	{
		if (this.moveAndRotateAsTransform)
		{
			if (this.moveAsEnabled)
			{
				if (this.useRigidbody)
				{
					Rigidbody component = base.transform.GetComponent<Rigidbody>();
					if (component != null)
					{
						Vector3 b = this.moveAndRotateAsTransform.right * this.offset.x + this.moveAndRotateAsTransform.up * this.offset.y + this.moveAndRotateAsTransform.forward * this.offset.z;
						component.MovePosition(this.moveAndRotateAsTransform.position + b);
					}
				}
				else
				{
					base.transform.position = this.moveAndRotateAsTransform.position;
					base.transform.localPosition += this.offset;
				}
			}
			if (this.rotateAsEnabled)
			{
				if (this.useRigidbody)
				{
					Rigidbody component2 = base.transform.GetComponent<Rigidbody>();
					if (component2 != null)
					{
						component2.MoveRotation(this.moveAndRotateAsTransform.rotation);
					}
				}
				else
				{
					base.transform.rotation = this.moveAndRotateAsTransform.rotation;
				}
			}
		}
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x001F47F8 File Offset: 0x001F2BF8
	protected void Init()
	{
		this.xOffsetJSON = new JSONStorableFloat("xOffset", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncXOffset), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.xOffsetJSON);
		this.yOffsetJSON = new JSONStorableFloat("yOffset", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncYOffset), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.yOffsetJSON);
		this.zOffsetJSON = new JSONStorableFloat("zOffset", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncZOffset), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.zOffsetJSON);
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x001F48B0 File Offset: 0x001F2CB0
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			MoveAndRotateAsJSONStorableUI componentInChildren = this.UITransform.GetComponentInChildren<MoveAndRotateAsJSONStorableUI>();
			if (componentInChildren != null)
			{
				this.xOffsetJSON.slider = componentInChildren.xOffsetSlider;
				this.yOffsetJSON.slider = componentInChildren.yOffsetSlider;
				this.zOffsetJSON.slider = componentInChildren.zOffsetSlider;
			}
		}
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x001F4919 File Offset: 0x001F2D19
	private void OnEnable()
	{
		this.DoUpdate();
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x001F4921 File Offset: 0x001F2D21
	private void Start()
	{
		this.DoUpdate();
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x001F4929 File Offset: 0x001F2D29
	private void FixedUpdate()
	{
		if (this.updateTime == MoveAndRotateAsJSONStorable.UpdateTime.Fixed)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x001F493D File Offset: 0x001F2D3D
	private void Update()
	{
		if (this.updateTime == MoveAndRotateAsJSONStorable.UpdateTime.Normal)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06005595 RID: 21909 RVA: 0x001F4950 File Offset: 0x001F2D50
	private void LateUpdate()
	{
		if (this.updateTime == MoveAndRotateAsJSONStorable.UpdateTime.Late)
		{
			this.DoUpdate();
		}
	}

	// Token: 0x06005596 RID: 21910 RVA: 0x001F4964 File Offset: 0x001F2D64
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x040046A2 RID: 18082
	public Transform moveAndRotateAsTransform;

	// Token: 0x040046A3 RID: 18083
	protected JSONStorableFloat xOffsetJSON;

	// Token: 0x040046A4 RID: 18084
	protected JSONStorableFloat yOffsetJSON;

	// Token: 0x040046A5 RID: 18085
	protected JSONStorableFloat zOffsetJSON;

	// Token: 0x040046A6 RID: 18086
	protected Vector3 offset;

	// Token: 0x040046A7 RID: 18087
	public bool moveAsEnabled = true;

	// Token: 0x040046A8 RID: 18088
	public bool rotateAsEnabled = true;

	// Token: 0x040046A9 RID: 18089
	public bool useRigidbody;

	// Token: 0x040046AA RID: 18090
	public MoveAndRotateAsJSONStorable.UpdateTime updateTime;

	// Token: 0x02000BBA RID: 3002
	public enum UpdateTime
	{
		// Token: 0x040046AC RID: 18092
		Normal,
		// Token: 0x040046AD RID: 18093
		Late,
		// Token: 0x040046AE RID: 18094
		Fixed,
		// Token: 0x040046AF RID: 18095
		Batch
	}
}
