using System;
using UnityEngine;

// Token: 0x02000C3A RID: 3130
public class MVRScriptController
{
	// Token: 0x06005B34 RID: 23348 RVA: 0x00217EFF File Offset: 0x002162FF
	public MVRScriptController()
	{
	}

	// Token: 0x06005B35 RID: 23349 RVA: 0x00217F07 File Offset: 0x00216307
	public void OpenUI()
	{
		if (this.customUI != null)
		{
			this.customUI.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005B36 RID: 23350 RVA: 0x00217F2B File Offset: 0x0021632B
	public void CloseUI()
	{
		if (this.customUI != null)
		{
			this.customUI.gameObject.SetActive(false);
		}
	}

	// Token: 0x04004B2B RID: 19243
	public GameObject gameObject;

	// Token: 0x04004B2C RID: 19244
	public MVRScript script;

	// Token: 0x04004B2D RID: 19245
	public Transform configUI;

	// Token: 0x04004B2E RID: 19246
	public Transform customUI;
}
