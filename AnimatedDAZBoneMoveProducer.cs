using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
public class AnimatedDAZBoneMoveProducer : MoveProducer
{
	// Token: 0x06004676 RID: 18038 RVA: 0x001457A4 File Offset: 0x00143BA4
	public AnimatedDAZBoneMoveProducer()
	{
	}

	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x06004677 RID: 18039 RVA: 0x001457B3 File Offset: 0x00143BB3
	// (set) Token: 0x06004678 RID: 18040 RVA: 0x001457BB File Offset: 0x00143BBB
	public Transform orientationTransform
	{
		[CompilerGenerated]
		get
		{
			return this.<orientationTransform>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<orientationTransform>k__BackingField = value;
		}
	}

	// Token: 0x06004679 RID: 18041 RVA: 0x001457C4 File Offset: 0x00143BC4
	protected override void Init()
	{
		if (!this._wasInit)
		{
			this._wasInit = true;
			base.Init();
			if (this._receiver != null && this._receiver.followWhenOff != null)
			{
				this.dazBone = this._receiver.followWhenOff.GetComponent<DAZBone>();
				if (this.dazBone != null)
				{
					GameObject gameObject = new GameObject(base.name + "_orientation");
					this.orientationTransform = gameObject.transform;
					this.orientationTransform.SetParent(base.transform);
					this.orientationTransform.localPosition = Vector3.zero;
					this.dazBone.Init();
					Quaternion rotation = base.transform.rotation;
					base.transform.rotation = Quaternion.identity;
					this.orientationTransform.rotation = this.dazBone.startingRotationRelativeToRoot;
					base.transform.rotation = rotation;
				}
			}
		}
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x001458C4 File Offset: 0x00143CC4
	protected override void SetCurrentPositionAndRotation()
	{
		if (this.useOrientationOffset && this.orientationTransform != null)
		{
			this._currentPosition = this.orientationTransform.position;
			this._currentRotation = this.orientationTransform.rotation;
		}
		else
		{
			this._currentPosition = base.transform.position;
			this._currentRotation = base.transform.rotation;
		}
	}

	// Token: 0x040033D7 RID: 13271
	public bool useOrientationOffset = true;

	// Token: 0x040033D8 RID: 13272
	protected DAZBone dazBone;

	// Token: 0x040033D9 RID: 13273
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Transform <orientationTransform>k__BackingField;

	// Token: 0x040033DA RID: 13274
	protected bool _wasInit;
}
