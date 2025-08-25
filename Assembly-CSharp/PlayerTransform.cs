using System;
using UnityEngine;

// Token: 0x02000BBF RID: 3007
public class PlayerTransform : MonoBehaviour
{
	// Token: 0x0600559E RID: 21918 RVA: 0x001F50A9 File Offset: 0x001F34A9
	public PlayerTransform()
	{
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x001F50B1 File Offset: 0x001F34B1
	private void Start()
	{
		PlayerTransform.player = base.transform;
	}

	// Token: 0x040046C8 RID: 18120
	public static Transform player;
}
