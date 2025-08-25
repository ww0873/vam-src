using System;
using UnityEngine;

// Token: 0x02000E0F RID: 3599
public class UIVisibility : MonoBehaviour
{
	// Token: 0x06006EFF RID: 28415 RVA: 0x0029A497 File Offset: 0x00298897
	public UIVisibility()
	{
	}

	// Token: 0x1700103C RID: 4156
	// (get) Token: 0x06006F00 RID: 28416 RVA: 0x0029A49F File Offset: 0x0029889F
	// (set) Token: 0x06006F01 RID: 28417 RVA: 0x0029A4A7 File Offset: 0x002988A7
	public bool keepVisible
	{
		get
		{
			return this._keepVisible;
		}
		set
		{
			this._keepVisible = value;
		}
	}

	// Token: 0x04006003 RID: 24579
	[SerializeField]
	private bool _keepVisible;
}
