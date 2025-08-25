using System;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class SlowmoToggler : MonoBehaviour
{
	// Token: 0x06001692 RID: 5778 RVA: 0x0007EDE9 File Offset: 0x0007D1E9
	public SlowmoToggler()
	{
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0007EDF1 File Offset: 0x0007D1F1
	public void Slowmo(bool slowmo)
	{
		Time.timeScale = ((!slowmo) ? 1f : 0.25f);
	}
}
