using System;
using Battlehub.RTHandles;
using UnityEngine;

// Token: 0x02000B97 RID: 2967
public class FCRotationHandle : RotationHandle
{
	// Token: 0x06005395 RID: 21397 RVA: 0x001E5006 File Offset: 0x001E3406
	public FCRotationHandle()
	{
	}

	// Token: 0x17000C27 RID: 3111
	// (get) Token: 0x06005396 RID: 21398 RVA: 0x001E500E File Offset: 0x001E340E
	// (set) Token: 0x06005397 RID: 21399 RVA: 0x001E5018 File Offset: 0x001E3418
	public FreeControllerV3 controller
	{
		get
		{
			return this._controller;
		}
		set
		{
			if (this._controller != value)
			{
				this._controller = value;
				if (!this.isDragging && this._controller != null)
				{
					base.transform.rotation = this._controller.transform.rotation;
				}
			}
		}
	}

	// Token: 0x06005398 RID: 21400 RVA: 0x001E5074 File Offset: 0x001E3474
	protected override bool OnBeginDrag()
	{
		if (this._controller != null && base.OnBeginDrag() && this._controller.canGrabRotation && !this._controller.isGrabbing)
		{
			this.isDragging = true;
			this._controller.isGrabbing = true;
			this._controller.SelectLinkToRigidbody(this.rb, FreeControllerV3.SelectLinkState.PositionAndRotation, false, true);
			return true;
		}
		return false;
	}

	// Token: 0x06005399 RID: 21401 RVA: 0x001E50E8 File Offset: 0x001E34E8
	protected override void OnDrop()
	{
		base.OnDrop();
		if (this.isDragging)
		{
			this.isDragging = false;
			if (this._controller != null)
			{
				this._controller.isGrabbing = false;
				this._controller.RestorePreLinkState();
			}
		}
	}

	// Token: 0x17000C28 RID: 3112
	// (get) Token: 0x0600539A RID: 21402 RVA: 0x001E5135 File Offset: 0x001E3535
	public bool HasSelectedAxis
	{
		get
		{
			return this.SelectedAxis != RuntimeHandleAxis.None;
		}
	}

	// Token: 0x0600539B RID: 21403 RVA: 0x001E5143 File Offset: 0x001E3543
	public void ForceDrop()
	{
		this.OnRuntimeToolChanged();
	}

	// Token: 0x0600539C RID: 21404 RVA: 0x001E514B File Offset: 0x001E354B
	public void ForceDeselectAxis()
	{
		this.SelectedAxis = RuntimeHandleAxis.None;
	}

	// Token: 0x0600539D RID: 21405 RVA: 0x001E5154 File Offset: 0x001E3554
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600539E RID: 21406 RVA: 0x001E5164 File Offset: 0x001E3564
	protected override void UpdateOverride()
	{
		base.UpdateOverride();
		if (this.priorityHandle != null)
		{
			if (this.isDragging)
			{
				this.priorityHandle.ForceDeselectAxis();
			}
			else if (this.priorityHandle.HasSelectedAxis)
			{
				this.SelectedAxis = RuntimeHandleAxis.None;
			}
		}
		if (!this.isDragging && this._controller != null)
		{
			base.transform.rotation = this._controller.transform.rotation;
		}
	}

	// Token: 0x0600539F RID: 21407 RVA: 0x001E51F1 File Offset: 0x001E35F1
	protected override void DrawOverride()
	{
		if (this._controller != null && this._controller.canGrabRotation)
		{
			base.DrawOverride();
		}
	}

	// Token: 0x040043AA RID: 17322
	public FCPositionHandle priorityHandle;

	// Token: 0x040043AB RID: 17323
	protected FreeControllerV3 _controller;

	// Token: 0x040043AC RID: 17324
	private Rigidbody rb;

	// Token: 0x040043AD RID: 17325
	protected bool isDragging;
}
