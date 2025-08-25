using System;
using UnityEngine;

// Token: 0x02000BB8 RID: 3000
public class MoveAndRotateAsHUDAnchor : MonoBehaviour
{
	// Token: 0x06005579 RID: 21881 RVA: 0x001F41FF File Offset: 0x001F25FF
	public MoveAndRotateAsHUDAnchor()
	{
	}

	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x0600557A RID: 21882 RVA: 0x001F421C File Offset: 0x001F261C
	// (set) Token: 0x0600557B RID: 21883 RVA: 0x001F4224 File Offset: 0x001F2624
	public bool MoveAsEnabled
	{
		get
		{
			return this._MoveAsEnabled;
		}
		set
		{
			this._MoveAsEnabled = value;
		}
	}

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x0600557C RID: 21884 RVA: 0x001F422D File Offset: 0x001F262D
	// (set) Token: 0x0600557D RID: 21885 RVA: 0x001F4238 File Offset: 0x001F2638
	public bool MoveAsDisabled
	{
		get
		{
			return !this._MoveAsEnabled;
		}
		set
		{
			this._MoveAsEnabled = !value;
		}
	}

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x0600557E RID: 21886 RVA: 0x001F4244 File Offset: 0x001F2644
	// (set) Token: 0x0600557F RID: 21887 RVA: 0x001F424C File Offset: 0x001F264C
	public bool RotateAsEnabled
	{
		get
		{
			return this._RotateAsEnabled;
		}
		set
		{
			this._RotateAsEnabled = value;
		}
	}

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x06005580 RID: 21888 RVA: 0x001F4255 File Offset: 0x001F2655
	// (set) Token: 0x06005581 RID: 21889 RVA: 0x001F4260 File Offset: 0x001F2660
	public bool RotateAsDisabled
	{
		get
		{
			return !this._RotateAsEnabled;
		}
		set
		{
			this._RotateAsEnabled = !value;
		}
	}

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x06005582 RID: 21890 RVA: 0x001F426C File Offset: 0x001F266C
	// (set) Token: 0x06005583 RID: 21891 RVA: 0x001F4274 File Offset: 0x001F2674
	public bool lockPlayerPosition
	{
		get
		{
			return this._lockPlayerPosition;
		}
		set
		{
			if (this._lockPlayerPosition != value)
			{
				this._lockPlayerPosition = value;
				if (this._lockPlayerPosition && PlayerTransform.player != null)
				{
					this.tracker = new GameObject("tracker");
					this.tracker.transform.position = base.transform.position;
					this.tracker.transform.rotation = base.transform.rotation;
					this.tracker.transform.parent = PlayerTransform.player;
				}
				else if (this.tracker != null)
				{
					UnityEngine.Object.Destroy(this.tracker);
					this.tracker = null;
				}
			}
		}
	}

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x06005584 RID: 21892 RVA: 0x001F4332 File Offset: 0x001F2732
	// (set) Token: 0x06005585 RID: 21893 RVA: 0x001F433A File Offset: 0x001F273A
	public bool lockWorldPosition
	{
		get
		{
			return this._lockWorldPosition;
		}
		set
		{
			this._lockWorldPosition = value;
		}
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x001F4344 File Offset: 0x001F2744
	public void ResetLocalPositionAndRotation()
	{
		base.transform.localPosition = this._startLocalPosition;
		base.transform.localRotation = this._startLocalRotation;
		this._lastPosition = base.transform.position;
		this._lastRotation = base.transform.rotation;
		if (this.tracker != null)
		{
			this.tracker.transform.position = this._lastPosition;
			this.tracker.transform.rotation = this._lastRotation;
		}
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x001F43D4 File Offset: 0x001F27D4
	private void moveAndRotate()
	{
		Transform anchorTransform = HUDAnchor.GetAnchorTransform(this.anchorNum);
		if (anchorTransform != null)
		{
			if (this.useSuperControllerWorldScale && SuperController.singleton != null)
			{
				Vector3 localScale;
				localScale.x = SuperController.singleton.worldScale;
				localScale.y = SuperController.singleton.worldScale;
				localScale.z = SuperController.singleton.worldScale;
				if (this.scaleAsAnchor)
				{
					localScale.x *= anchorTransform.localScale.x;
					localScale.y *= anchorTransform.localScale.y;
					localScale.z *= anchorTransform.localScale.z;
				}
				base.transform.localScale = localScale;
			}
			else if (this.scaleAsAnchor)
			{
				base.transform.localScale = anchorTransform.localScale;
			}
			if (this._lockPlayerPosition)
			{
				if (this.tracker != null)
				{
					base.transform.position = this.tracker.transform.position;
				}
			}
			else if (this._lockWorldPosition)
			{
				base.transform.position = this._lastPosition;
			}
			else if (this._MoveAsEnabled && anchorTransform != null)
			{
				base.transform.position = anchorTransform.position + this.Offset;
			}
			if (this._lockPlayerPosition)
			{
				if (this.tracker != null)
				{
					base.transform.rotation = this.tracker.transform.rotation;
				}
			}
			else if (this._lockWorldPosition)
			{
				base.transform.rotation = this._lastRotation;
			}
			else if (this._RotateAsEnabled && anchorTransform != null)
			{
				base.transform.rotation = anchorTransform.rotation;
			}
			this._lastPosition = base.transform.position;
			this._lastRotation = base.transform.rotation;
		}
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x001F460C File Offset: 0x001F2A0C
	private void Start()
	{
		this._startLocalPosition = base.transform.localPosition;
		this._startLocalRotation = base.transform.localRotation;
		this._lastPosition = base.transform.position;
		this._lastRotation = base.transform.rotation;
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x001F465D File Offset: 0x001F2A5D
	private void LateUpdate()
	{
		this.moveAndRotate();
	}

	// Token: 0x04004695 RID: 18069
	public Vector3 Offset;

	// Token: 0x04004696 RID: 18070
	public HUDAnchor.AnchorNum anchorNum;

	// Token: 0x04004697 RID: 18071
	public bool useSuperControllerWorldScale = true;

	// Token: 0x04004698 RID: 18072
	[SerializeField]
	private bool _MoveAsEnabled = true;

	// Token: 0x04004699 RID: 18073
	[SerializeField]
	private bool _RotateAsEnabled = true;

	// Token: 0x0400469A RID: 18074
	private GameObject tracker;

	// Token: 0x0400469B RID: 18075
	[SerializeField]
	private bool _lockPlayerPosition;

	// Token: 0x0400469C RID: 18076
	private Vector3 _lastPosition;

	// Token: 0x0400469D RID: 18077
	private Quaternion _lastRotation;

	// Token: 0x0400469E RID: 18078
	[SerializeField]
	private bool _lockWorldPosition;

	// Token: 0x0400469F RID: 18079
	public bool scaleAsAnchor;

	// Token: 0x040046A0 RID: 18080
	private Vector3 _startLocalPosition;

	// Token: 0x040046A1 RID: 18081
	private Quaternion _startLocalRotation;
}
