using System;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public class lookAt : MonoBehaviour
{
	// Token: 0x06001A82 RID: 6786 RVA: 0x00094663 File Offset: 0x00092A63
	public lookAt()
	{
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x0009466B File Offset: 0x00092A6B
	private void Start()
	{
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x0009466D File Offset: 0x00092A6D
	private void Update()
	{
		base.transform.LookAt(this.player);
	}

	// Token: 0x0400159B RID: 5531
	public Transform player;
}
