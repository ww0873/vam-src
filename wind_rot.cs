using System;
using UnityEngine;

// Token: 0x02000421 RID: 1057
public class wind_rot : MonoBehaviour
{
	// Token: 0x06001A88 RID: 6792 RVA: 0x00094832 File Offset: 0x00092C32
	public wind_rot()
	{
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x0009483A File Offset: 0x00092C3A
	private void Start()
	{
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x0009483C File Offset: 0x00092C3C
	private void Update()
	{
		base.transform.Rotate(Vector3.up, 0.05f);
	}
}
