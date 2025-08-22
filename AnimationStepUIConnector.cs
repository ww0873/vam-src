using System;
using UnityEngine;

// Token: 0x02000B63 RID: 2915
public class AnimationStepUIConnector : UIConnector
{
	// Token: 0x06005160 RID: 20832 RVA: 0x001D5C68 File Offset: 0x001D4068
	public AnimationStepUIConnector()
	{
	}

	// Token: 0x06005161 RID: 20833 RVA: 0x001D5C70 File Offset: 0x001D4070
	public override void Connect()
	{
		Debug.LogError("AnimationStepUIConnector obsolete but still in use");
	}

	// Token: 0x0400411F RID: 16671
	public AnimationStep animationStep;
}
