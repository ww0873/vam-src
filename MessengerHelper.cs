using System;
using UnityEngine;

// Token: 0x020007CC RID: 1996
public sealed class MessengerHelper : MonoBehaviour
{
	// Token: 0x060032A7 RID: 12967 RVA: 0x00107A3B File Offset: 0x00105E3B
	public MessengerHelper()
	{
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x00107A43 File Offset: 0x00105E43
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x00107A50 File Offset: 0x00105E50
	public void OnDisable()
	{
		OVRMessenger.Cleanup();
	}
}
