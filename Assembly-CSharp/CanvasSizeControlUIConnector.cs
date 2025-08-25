using System;
using UnityEngine;

// Token: 0x02000C08 RID: 3080
public class CanvasSizeControlUIConnector : UIConnector
{
	// Token: 0x06005998 RID: 22936 RVA: 0x0020F4CA File Offset: 0x0020D8CA
	public CanvasSizeControlUIConnector()
	{
	}

	// Token: 0x06005999 RID: 22937 RVA: 0x0020F4D2 File Offset: 0x0020D8D2
	public override void Connect()
	{
		Debug.LogError("CanvasSizeControlUIConnector obsolete but still in use");
	}

	// Token: 0x040049B8 RID: 18872
	public CanvasSizeControl canvasSizeControl;
}
