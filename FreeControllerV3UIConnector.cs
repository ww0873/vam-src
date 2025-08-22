using System;
using UnityEngine;

// Token: 0x02000BAB RID: 2987
public class FreeControllerV3UIConnector : UIConnector
{
	// Token: 0x0600554D RID: 21837 RVA: 0x001F358F File Offset: 0x001F198F
	public FreeControllerV3UIConnector()
	{
	}

	// Token: 0x0600554E RID: 21838 RVA: 0x001F3597 File Offset: 0x001F1997
	public override void Connect()
	{
		Debug.LogError("FreeControllerV3UIConnector obsolete but still in use");
	}

	// Token: 0x0400463D RID: 17981
	public FreeControllerV3 controller;
}
