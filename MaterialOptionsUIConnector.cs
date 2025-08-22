using System;
using UnityEngine;

// Token: 0x02000D23 RID: 3363
public class MaterialOptionsUIConnector : UIConnector
{
	// Token: 0x06006730 RID: 26416 RVA: 0x0026D335 File Offset: 0x0026B735
	public MaterialOptionsUIConnector()
	{
	}

	// Token: 0x06006731 RID: 26417 RVA: 0x0026D33D File Offset: 0x0026B73D
	public override void Connect()
	{
		Debug.LogError("MaterialOptionsUIConnector obsolete but still in use");
	}

	// Token: 0x04005829 RID: 22569
	public MaterialOptions[] materialOptionsList;
}
