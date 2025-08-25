using System;
using UnityEngine;

// Token: 0x02000C30 RID: 3120
public class MKGlowObject : MonoBehaviour
{
	// Token: 0x06005AB2 RID: 23218 RVA: 0x0021418C File Offset: 0x0021258C
	public MKGlowObject()
	{
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x00214194 File Offset: 0x00212594
	private void OnEnable()
	{
		if (UserPreferences.singleton != null)
		{
			UserPreferences.singleton.RegisterGlowObject();
		}
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x002141B0 File Offset: 0x002125B0
	private void OnDisable()
	{
		if (UserPreferences.singleton != null)
		{
			UserPreferences.singleton.DeregisterGlowObject();
		}
	}
}
