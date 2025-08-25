using System;
using UnityEngine;

// Token: 0x02000C0E RID: 3086
public class EnableLink : MonoBehaviour
{
	// Token: 0x060059C2 RID: 22978 RVA: 0x002105EB File Offset: 0x0020E9EB
	public EnableLink()
	{
	}

	// Token: 0x060059C3 RID: 22979 RVA: 0x002105F3 File Offset: 0x0020E9F3
	private void OnEnable()
	{
		if (this.linkTransform != null)
		{
			this.linkTransform.gameObject.SetActive(true);
		}
	}

	// Token: 0x060059C4 RID: 22980 RVA: 0x00210617 File Offset: 0x0020EA17
	private void OnDisable()
	{
		if (this.linkTransform != null)
		{
			this.linkTransform.gameObject.SetActive(false);
		}
	}

	// Token: 0x040049DF RID: 18911
	public Transform linkTransform;
}
