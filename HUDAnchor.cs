using System;
using UnityEngine;

// Token: 0x02000DC5 RID: 3525
public class HUDAnchor : MonoBehaviour
{
	// Token: 0x06006D5C RID: 27996 RVA: 0x002929D5 File Offset: 0x00290DD5
	public HUDAnchor()
	{
	}

	// Token: 0x06006D5D RID: 27997 RVA: 0x002929E0 File Offset: 0x00290DE0
	public static void SetAnchorsToReference()
	{
		if (HUDAnchor.anchor1 != null)
		{
			HUDAnchor.anchor1.SetAnchorToReference();
		}
		if (HUDAnchor.anchor2 != null)
		{
			HUDAnchor.anchor2.SetAnchorToReference();
		}
		if (HUDAnchor.anchor3 != null)
		{
			HUDAnchor.anchor3.SetAnchorToReference();
		}
		if (HUDAnchor.anchor4 != null)
		{
			HUDAnchor.anchor4.SetAnchorToReference();
		}
		if (HUDAnchor.anchor5 != null)
		{
			HUDAnchor.anchor5.SetAnchorToReference();
		}
		if (HUDAnchor.anchor6 != null)
		{
			HUDAnchor.anchor6.SetAnchorToReference();
		}
		if (HUDAnchor.anchor7 != null)
		{
			HUDAnchor.anchor7.SetAnchorToReference();
		}
		if (HUDAnchor.anchor8 != null)
		{
			HUDAnchor.anchor8.SetAnchorToReference();
		}
	}

	// Token: 0x06006D5E RID: 27998 RVA: 0x00292AC0 File Offset: 0x00290EC0
	public static void AdjustAnchorHeights(float adj)
	{
		if (HUDAnchor.anchor1 != null)
		{
			HUDAnchor.anchor1.AdjustHeight(adj);
		}
		if (HUDAnchor.anchor2 != null)
		{
			HUDAnchor.anchor2.AdjustHeight(adj);
		}
		if (HUDAnchor.anchor3 != null)
		{
			HUDAnchor.anchor3.AdjustHeight(adj);
		}
		if (HUDAnchor.anchor4 != null)
		{
			HUDAnchor.anchor4.AdjustHeight(adj);
		}
		if (HUDAnchor.anchor5 != null)
		{
			HUDAnchor.anchor5.AdjustHeight(adj);
		}
		if (HUDAnchor.anchor6 != null)
		{
			HUDAnchor.anchor6.AdjustHeight(adj);
		}
		if (HUDAnchor.anchor7 != null)
		{
			HUDAnchor.anchor7.AdjustHeight(adj);
		}
		if (HUDAnchor.anchor8 != null)
		{
			HUDAnchor.anchor8.AdjustHeight(adj);
		}
	}

	// Token: 0x06006D5F RID: 27999 RVA: 0x00292BA8 File Offset: 0x00290FA8
	public static Transform GetAnchorTransform(HUDAnchor.AnchorNum anchorNum)
	{
		Transform result = null;
		switch (anchorNum)
		{
		case HUDAnchor.AnchorNum.One:
			if (HUDAnchor.anchor1 != null)
			{
				result = HUDAnchor.anchor1.transform;
			}
			break;
		case HUDAnchor.AnchorNum.Two:
			if (HUDAnchor.anchor2 != null)
			{
				result = HUDAnchor.anchor2.transform;
			}
			break;
		case HUDAnchor.AnchorNum.Three:
			if (HUDAnchor.anchor3 != null)
			{
				result = HUDAnchor.anchor3.transform;
			}
			break;
		case HUDAnchor.AnchorNum.Four:
			if (HUDAnchor.anchor4 != null)
			{
				result = HUDAnchor.anchor4.transform;
			}
			break;
		case HUDAnchor.AnchorNum.Five:
			if (HUDAnchor.anchor5 != null)
			{
				result = HUDAnchor.anchor5.transform;
			}
			break;
		case HUDAnchor.AnchorNum.Six:
			if (HUDAnchor.anchor6 != null)
			{
				result = HUDAnchor.anchor6.transform;
			}
			break;
		case HUDAnchor.AnchorNum.Seven:
			if (HUDAnchor.anchor7 != null)
			{
				result = HUDAnchor.anchor7.transform;
			}
			break;
		case HUDAnchor.AnchorNum.Eight:
			if (HUDAnchor.anchor8 != null)
			{
				result = HUDAnchor.anchor8.transform;
			}
			break;
		default:
			result = null;
			break;
		}
		return result;
	}

	// Token: 0x06006D60 RID: 28000 RVA: 0x00292CEC File Offset: 0x002910EC
	public void SetAnchorToReference()
	{
		if (this.reference != null && this.reference.gameObject.activeInHierarchy)
		{
			base.transform.position = this.reference.position;
			base.transform.rotation = this.reference.rotation;
		}
		if (this.referenceAlt != null && this.referenceAlt.gameObject.activeInHierarchy)
		{
			base.transform.position = this.referenceAlt.position;
			base.transform.rotation = this.referenceAlt.rotation;
		}
	}

	// Token: 0x06006D61 RID: 28001 RVA: 0x00292DA0 File Offset: 0x002911A0
	public void AdjustHeight(float adj)
	{
		if (this.reference != null)
		{
			Vector3 position = base.transform.position;
			position.y += adj;
			base.transform.position = position;
		}
	}

	// Token: 0x06006D62 RID: 28002 RVA: 0x00292DE8 File Offset: 0x002911E8
	private void Update()
	{
		switch (this.anchorNum)
		{
		case HUDAnchor.AnchorNum.One:
			HUDAnchor.anchor1 = this;
			break;
		case HUDAnchor.AnchorNum.Two:
			HUDAnchor.anchor2 = this;
			break;
		case HUDAnchor.AnchorNum.Three:
			HUDAnchor.anchor3 = this;
			break;
		case HUDAnchor.AnchorNum.Four:
			HUDAnchor.anchor4 = this;
			break;
		case HUDAnchor.AnchorNum.Five:
			HUDAnchor.anchor5 = this;
			break;
		case HUDAnchor.AnchorNum.Six:
			HUDAnchor.anchor6 = this;
			break;
		case HUDAnchor.AnchorNum.Seven:
			HUDAnchor.anchor7 = this;
			break;
		case HUDAnchor.AnchorNum.Eight:
			HUDAnchor.anchor8 = this;
			break;
		}
	}

	// Token: 0x04005EBA RID: 24250
	public HUDAnchor.AnchorNum anchorNum;

	// Token: 0x04005EBB RID: 24251
	public static HUDAnchor anchor1;

	// Token: 0x04005EBC RID: 24252
	public static HUDAnchor anchor2;

	// Token: 0x04005EBD RID: 24253
	public static HUDAnchor anchor3;

	// Token: 0x04005EBE RID: 24254
	public static HUDAnchor anchor4;

	// Token: 0x04005EBF RID: 24255
	public static HUDAnchor anchor5;

	// Token: 0x04005EC0 RID: 24256
	public static HUDAnchor anchor6;

	// Token: 0x04005EC1 RID: 24257
	public static HUDAnchor anchor7;

	// Token: 0x04005EC2 RID: 24258
	public static HUDAnchor anchor8;

	// Token: 0x04005EC3 RID: 24259
	public Transform reference;

	// Token: 0x04005EC4 RID: 24260
	public Transform referenceAlt;

	// Token: 0x02000DC6 RID: 3526
	public enum AnchorNum
	{
		// Token: 0x04005EC6 RID: 24262
		One,
		// Token: 0x04005EC7 RID: 24263
		Two,
		// Token: 0x04005EC8 RID: 24264
		Three,
		// Token: 0x04005EC9 RID: 24265
		Four,
		// Token: 0x04005ECA RID: 24266
		Five,
		// Token: 0x04005ECB RID: 24267
		Six,
		// Token: 0x04005ECC RID: 24268
		Seven,
		// Token: 0x04005ECD RID: 24269
		Eight
	}
}
