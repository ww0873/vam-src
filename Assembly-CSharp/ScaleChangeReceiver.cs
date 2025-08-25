using System;
using UnityEngine;

// Token: 0x02000BC0 RID: 3008
public class ScaleChangeReceiver : MonoBehaviour
{
	// Token: 0x060055A0 RID: 21920 RVA: 0x00145936 File Offset: 0x00143D36
	public ScaleChangeReceiver()
	{
	}

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x060055A1 RID: 21921 RVA: 0x00145949 File Offset: 0x00143D49
	public float scale
	{
		get
		{
			return this._scale;
		}
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x00145951 File Offset: 0x00143D51
	public virtual void ScaleChanged(float scale)
	{
		this._scale = scale;
	}

	// Token: 0x040046C9 RID: 18121
	protected float _scale = 1f;
}
