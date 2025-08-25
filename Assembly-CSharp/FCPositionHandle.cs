using System;
using Battlehub.RTHandles;
using UnityEngine;

// Token: 0x02000B96 RID: 2966
public class FCPositionHandle : PositionHandle
{
	// Token: 0x0600538A RID: 21386 RVA: 0x001E4DF1 File Offset: 0x001E31F1
	public FCPositionHandle()
	{
	}

	// Token: 0x17000C25 RID: 3109
	// (get) Token: 0x0600538B RID: 21387 RVA: 0x001E4DF9 File Offset: 0x001E31F9
	// (set) Token: 0x0600538C RID: 21388 RVA: 0x001E4E04 File Offset: 0x001E3204
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
					base.transform.position = this._controller.transform.position;
				}
			}
		}
	}

	// Token: 0x0600538D RID: 21389 RVA: 0x001E4E60 File Offset: 0x001E3260
	protected override bool OnBeginDrag()
	{
		if (this._controller != null && base.OnBeginDrag() && this._controller.canGrabPosition && !this._controller.isGrabbing)
		{
			this.isDragging = true;
			this._controller.isGrabbing = true;
			this._controller.SelectLinkToRigidbody(this.rb, FreeControllerV3.SelectLinkState.PositionAndRotation, false, true);
			return true;
		}
		return false;
	}

	// Token: 0x0600538E RID: 21390 RVA: 0x001E4ED4 File Offset: 0x001E32D4
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

	// Token: 0x17000C26 RID: 3110
	// (get) Token: 0x0600538F RID: 21391 RVA: 0x001E4F21 File Offset: 0x001E3321
	public bool HasSelectedAxis
	{
		get
		{
			return this.SelectedAxis != RuntimeHandleAxis.None;
		}
	}

	// Token: 0x06005390 RID: 21392 RVA: 0x001E4F2F File Offset: 0x001E332F
	public void ForceDrop()
	{
		this.OnRuntimeToolChanged();
	}

	// Token: 0x06005391 RID: 21393 RVA: 0x001E4F37 File Offset: 0x001E3337
	public void ForceDeselectAxis()
	{
		this.SelectedAxis = RuntimeHandleAxis.None;
	}

	// Token: 0x06005392 RID: 21394 RVA: 0x001E4F40 File Offset: 0x001E3340
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06005393 RID: 21395 RVA: 0x001E4F50 File Offset: 0x001E3350
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
			base.transform.position = this._controller.transform.position;
		}
	}

	// Token: 0x06005394 RID: 21396 RVA: 0x001E4FDD File Offset: 0x001E33DD
	protected override void DrawOverride()
	{
		if (this._controller != null && this._controller.canGrabPosition)
		{
			base.DrawOverride();
		}
	}

	// Token: 0x040043A6 RID: 17318
	protected FreeControllerV3 _controller;

	// Token: 0x040043A7 RID: 17319
	public FCRotationHandle priorityHandle;

	// Token: 0x040043A8 RID: 17320
	private Rigidbody rb;

	// Token: 0x040043A9 RID: 17321
	protected bool isDragging;
}
