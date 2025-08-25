using System;
using UnityEngine;

// Token: 0x02000E00 RID: 3584
public class UISideAlign : MonoBehaviour
{
	// Token: 0x06006EC3 RID: 28355 RVA: 0x00298DDC File Offset: 0x002971DC
	public UISideAlign()
	{
	}

	// Token: 0x06006EC4 RID: 28356 RVA: 0x00298E38 File Offset: 0x00297238
	public void Sync()
	{
		if (this.rt == null)
		{
			this.rt = base.GetComponent<RectTransform>();
		}
		if (this.rt != null)
		{
			Vector2 anchorMin = this.rt.anchorMin;
			Vector2 anchorMax = this.rt.anchorMax;
			Vector2 pivot = this.rt.pivot;
			Vector2 anchoredPosition = this.rt.anchoredPosition;
			Vector3 localEulerAngles = this.rt.localEulerAngles;
			if (this._currentSide == UISideAlign.Side.Left)
			{
				pivot.x = this.leftSidePivotX;
				this.rt.pivot = pivot;
				anchorMin.x = this.leftSideAnchorMinMax;
				this.rt.anchorMin = anchorMin;
				anchorMax.x = this.leftSideAnchorMinMax;
				this.rt.anchorMax = anchorMax;
				anchoredPosition.x = this.leftSideOffsetX;
				this.rt.anchoredPosition = anchoredPosition;
				if (UISideAlign.useNeutralRotation)
				{
					localEulerAngles.y = this.neutralAngleY;
					localEulerAngles.z = this.neutralAngleZ;
				}
				else
				{
					localEulerAngles.y = this.leftSideAngleY;
					localEulerAngles.z = this.leftSideAngleZ;
				}
				this.rt.localEulerAngles = localEulerAngles;
			}
			else
			{
				pivot.x = this.rightSidePivotX;
				this.rt.pivot = pivot;
				anchorMin.x = this.rightSideAnchorMinMax;
				this.rt.anchorMin = anchorMin;
				anchorMax.x = this.rightSideAnchorMinMax;
				this.rt.anchorMax = anchorMax;
				anchoredPosition.x = this.rightSideOffsetX;
				this.rt.anchoredPosition = anchoredPosition;
				if (UISideAlign.useNeutralRotation)
				{
					localEulerAngles.y = this.neutralAngleY;
					localEulerAngles.z = this.neutralAngleZ;
				}
				else
				{
					localEulerAngles.y = this.rightSideAngleY;
					localEulerAngles.z = this.rightSideAngleZ;
				}
				this.rt.localEulerAngles = localEulerAngles;
			}
		}
	}

	// Token: 0x17001038 RID: 4152
	// (get) Token: 0x06006EC5 RID: 28357 RVA: 0x00299028 File Offset: 0x00297428
	// (set) Token: 0x06006EC6 RID: 28358 RVA: 0x00299030 File Offset: 0x00297430
	public UISideAlign.Side currentSide
	{
		get
		{
			return this._currentSide;
		}
		protected set
		{
			if (this._currentSide != value)
			{
				this._currentSide = value;
				this.Sync();
			}
		}
	}

	// Token: 0x17001039 RID: 4153
	// (get) Token: 0x06006EC7 RID: 28359 RVA: 0x0029904B File Offset: 0x0029744B
	// (set) Token: 0x06006EC8 RID: 28360 RVA: 0x00299053 File Offset: 0x00297453
	public bool usingNeutralRotation
	{
		get
		{
			return this._usingNeutralRotation;
		}
		set
		{
			if (this._usingNeutralRotation != value)
			{
				this._usingNeutralRotation = value;
				this.Sync();
			}
		}
	}

	// Token: 0x06006EC9 RID: 28361 RVA: 0x00299070 File Offset: 0x00297470
	protected void SyncToGlobal()
	{
		if (this.obeyGlobalSide)
		{
			if (this.invertGlobalSide)
			{
				if (UISideAlign.globalSide == UISideAlign.Side.Left)
				{
					this.currentSide = UISideAlign.Side.Right;
				}
				else
				{
					this.currentSide = UISideAlign.Side.Left;
				}
			}
			else if (UISideAlign.globalSide == UISideAlign.Side.Left)
			{
				this.currentSide = UISideAlign.Side.Left;
			}
			else
			{
				this.currentSide = UISideAlign.Side.Right;
			}
			this.usingNeutralRotation = UISideAlign.useNeutralRotation;
		}
	}

	// Token: 0x06006ECA RID: 28362 RVA: 0x002990DD File Offset: 0x002974DD
	protected void Awake()
	{
		this.SyncToGlobal();
		this.Sync();
	}

	// Token: 0x06006ECB RID: 28363 RVA: 0x002990EB File Offset: 0x002974EB
	protected void OnEnable()
	{
		this.SyncToGlobal();
	}

	// Token: 0x06006ECC RID: 28364 RVA: 0x002990F3 File Offset: 0x002974F3
	protected void Update()
	{
		this.SyncToGlobal();
	}

	// Token: 0x06006ECD RID: 28365 RVA: 0x002990FB File Offset: 0x002974FB
	// Note: this type is marked as 'beforefieldinit'.
	static UISideAlign()
	{
	}

	// Token: 0x04005FD0 RID: 24528
	public static UISideAlign.Side globalSide = UISideAlign.Side.Right;

	// Token: 0x04005FD1 RID: 24529
	public static bool useNeutralRotation;

	// Token: 0x04005FD2 RID: 24530
	public bool obeyGlobalSide = true;

	// Token: 0x04005FD3 RID: 24531
	public bool invertGlobalSide;

	// Token: 0x04005FD4 RID: 24532
	protected RectTransform rt;

	// Token: 0x04005FD5 RID: 24533
	[SerializeField]
	protected UISideAlign.Side _currentSide;

	// Token: 0x04005FD6 RID: 24534
	protected bool _usingNeutralRotation;

	// Token: 0x04005FD7 RID: 24535
	public float neutralAngleY;

	// Token: 0x04005FD8 RID: 24536
	public float neutralAngleZ;

	// Token: 0x04005FD9 RID: 24537
	public float leftSideAnchorMinMax;

	// Token: 0x04005FDA RID: 24538
	public float leftSidePivotX = 1f;

	// Token: 0x04005FDB RID: 24539
	public float leftSideOffsetX = -2f;

	// Token: 0x04005FDC RID: 24540
	public float leftSideAngleY = -30f;

	// Token: 0x04005FDD RID: 24541
	public float leftSideAngleZ;

	// Token: 0x04005FDE RID: 24542
	public float rightSideAnchorMinMax = 1f;

	// Token: 0x04005FDF RID: 24543
	public float rightSidePivotX;

	// Token: 0x04005FE0 RID: 24544
	public float rightSideOffsetX = 2f;

	// Token: 0x04005FE1 RID: 24545
	public float rightSideAngleY = 30f;

	// Token: 0x04005FE2 RID: 24546
	public float rightSideAngleZ;

	// Token: 0x02000E01 RID: 3585
	public enum Side
	{
		// Token: 0x04005FE4 RID: 24548
		Left,
		// Token: 0x04005FE5 RID: 24549
		Right
	}
}
