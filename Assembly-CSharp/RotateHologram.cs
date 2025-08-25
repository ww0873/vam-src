using System;
using UnityEngine;

// Token: 0x02000E9B RID: 3739
public class RotateHologram : MonoBehaviour
{
	// Token: 0x06007154 RID: 29012 RVA: 0x002B1568 File Offset: 0x002AF968
	public RotateHologram()
	{
	}

	// Token: 0x06007155 RID: 29013 RVA: 0x002B1570 File Offset: 0x002AF970
	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 25f, 0f) * Time.deltaTime);
	}
}
