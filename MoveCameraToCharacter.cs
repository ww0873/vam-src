using System;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class MoveCameraToCharacter : MonoBehaviour
{
	// Token: 0x06001250 RID: 4688 RVA: 0x00065FB8 File Offset: 0x000643B8
	public MoveCameraToCharacter()
	{
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x00065FC0 File Offset: 0x000643C0
	private void Update()
	{
		base.transform.position = this.Target.transform.position;
	}

	// Token: 0x04000FB0 RID: 4016
	public GameObject Target;
}
