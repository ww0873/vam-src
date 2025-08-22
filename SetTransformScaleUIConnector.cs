using System;
using UnityEngine;

// Token: 0x02000BC6 RID: 3014
public class SetTransformScaleUIConnector : UIConnector
{
	// Token: 0x060055BC RID: 21948 RVA: 0x001F5884 File Offset: 0x001F3C84
	public SetTransformScaleUIConnector()
	{
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x001F588C File Offset: 0x001F3C8C
	public override void Connect()
	{
		Debug.LogError("SetTransformScaleUIConnector obsolete but still in use");
	}

	// Token: 0x040046E0 RID: 18144
	public SetTransformScale transformScale;
}
