using System;
using UnityEngine;

// Token: 0x02000C09 RID: 3081
public class CopyToClipboard : MonoBehaviour
{
	// Token: 0x0600599A RID: 22938 RVA: 0x0020F4DE File Offset: 0x0020D8DE
	public CopyToClipboard()
	{
	}

	// Token: 0x0600599B RID: 22939 RVA: 0x0020F4E6 File Offset: 0x0020D8E6
	public void CopyStringToClipboard(string val)
	{
		GUIUtility.systemCopyBuffer = val;
	}
}
