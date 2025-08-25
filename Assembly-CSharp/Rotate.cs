using System;
using UnityEngine;

// Token: 0x020002F9 RID: 761
[Serializable]
public class Rotate : MonoBehaviour
{
	// Token: 0x060011E9 RID: 4585 RVA: 0x00062F7C File Offset: 0x0006137C
	public Rotate()
	{
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x00062F84 File Offset: 0x00061384
	public virtual void Update()
	{
		base.transform.Rotate(0f, Time.deltaTime * 10f, 0f);
	}
}
