using System;
using UnityEngine;

// Token: 0x02000D18 RID: 3352
public class ImageControlUIConnector : UIConnector
{
	// Token: 0x0600667C RID: 26236 RVA: 0x0026C246 File Offset: 0x0026A646
	public ImageControlUIConnector()
	{
	}

	// Token: 0x0600667D RID: 26237 RVA: 0x0026C24E File Offset: 0x0026A64E
	public override void Connect()
	{
		Debug.LogError("ImageControlUIConnector obsolete but still in use");
	}

	// Token: 0x04005612 RID: 22034
	public ImageControl imageControl;
}
