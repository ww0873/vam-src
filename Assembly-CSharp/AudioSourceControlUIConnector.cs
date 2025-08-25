using System;
using UnityEngine;

// Token: 0x02000B7D RID: 2941
public class AudioSourceControlUIConnector : UIConnector
{
	// Token: 0x060052BE RID: 21182 RVA: 0x001DEBEC File Offset: 0x001DCFEC
	public AudioSourceControlUIConnector()
	{
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x001DEBF4 File Offset: 0x001DCFF4
	public override void Connect()
	{
		Debug.LogError("AudioSourceControlUIConnect obsolete but still in use");
	}

	// Token: 0x04004297 RID: 17047
	public AudioSourceControl audioSourceControl;
}
