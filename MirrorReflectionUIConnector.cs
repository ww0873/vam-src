using System;
using UnityEngine;

// Token: 0x02000D0F RID: 3343
public class MirrorReflectionUIConnector : UIConnector
{
	// Token: 0x06006612 RID: 26130 RVA: 0x002690F6 File Offset: 0x002674F6
	public MirrorReflectionUIConnector()
	{
	}

	// Token: 0x06006613 RID: 26131 RVA: 0x002690FE File Offset: 0x002674FE
	public override void Connect()
	{
		Debug.LogError("MirrorReflectionUIConnector obsolete but still in use");
	}

	// Token: 0x0400558B RID: 21899
	public MirrorReflection mirrorReflection;
}
