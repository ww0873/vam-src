using System;
using UnityEngine;

// Token: 0x02000B70 RID: 2928
public class MoveProducerUIConnector : UIConnector
{
	// Token: 0x06005235 RID: 21045 RVA: 0x001DA93A File Offset: 0x001D8D3A
	public MoveProducerUIConnector()
	{
	}

	// Token: 0x06005236 RID: 21046 RVA: 0x001DA942 File Offset: 0x001D8D42
	public override void Connect()
	{
		Debug.LogError("MoveProducerUIConnector obsolete but still in use");
	}

	// Token: 0x040041E6 RID: 16870
	public MoveProducer moveProducer;
}
