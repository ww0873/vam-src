using System;
using UnityEngine;

// Token: 0x02000DBB RID: 3515
public class ChildOrderFlip : MonoBehaviour
{
	// Token: 0x06006CF6 RID: 27894 RVA: 0x00291234 File Offset: 0x0028F634
	public ChildOrderFlip()
	{
	}

	// Token: 0x17000FF5 RID: 4085
	// (get) Token: 0x06006CF7 RID: 27895 RVA: 0x0029123C File Offset: 0x0028F63C
	// (set) Token: 0x06006CF8 RID: 27896 RVA: 0x00291244 File Offset: 0x0028F644
	public bool flipped
	{
		get
		{
			return this._flipped;
		}
		set
		{
			if (this._flipped != value)
			{
				this._flipped = value;
				this.Flip();
			}
		}
	}

	// Token: 0x06006CF9 RID: 27897 RVA: 0x00291260 File Offset: 0x0028F660
	protected void Flip()
	{
		foreach (Transform transform in this.transformsToFlip)
		{
			int childCount = transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				Transform child = transform.GetChild(childCount - 1);
				child.SetSiblingIndex(j);
			}
		}
	}

	// Token: 0x04005E74 RID: 24180
	public Transform[] transformsToFlip;

	// Token: 0x04005E75 RID: 24181
	[SerializeField]
	protected bool _flipped;
}
