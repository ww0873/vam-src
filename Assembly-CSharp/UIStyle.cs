using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E02 RID: 3586
public abstract class UIStyle : MonoBehaviour
{
	// Token: 0x06006ECE RID: 28366 RVA: 0x00299103 File Offset: 0x00297503
	protected UIStyle()
	{
	}

	// Token: 0x06006ECF RID: 28367
	public abstract List<UnityEngine.Object> GetControlledObjects();

	// Token: 0x06006ED0 RID: 28368
	public abstract List<UnityEngine.Object> UpdateStyle();

	// Token: 0x04005FE6 RID: 24550
	public string styleName;
}
