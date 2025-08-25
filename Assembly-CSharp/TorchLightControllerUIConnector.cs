using System;
using UnityEngine;

// Token: 0x02000D14 RID: 3348
public class TorchLightControllerUIConnector : UIConnector
{
	// Token: 0x06006640 RID: 26176 RVA: 0x0026A3C1 File Offset: 0x002687C1
	public TorchLightControllerUIConnector()
	{
	}

	// Token: 0x06006641 RID: 26177 RVA: 0x0026A3C9 File Offset: 0x002687C9
	public override void Connect()
	{
		Debug.LogError("TorchLightControllerUIConnector obsolete but still in use");
	}

	// Token: 0x040055D4 RID: 21972
	public TorchLightController lightController;
}
