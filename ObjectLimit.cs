using System;
using UnityEngine;

// Token: 0x02000389 RID: 905
public class ObjectLimit : MonoBehaviour
{
	// Token: 0x0600168E RID: 5774 RVA: 0x0007EB92 File Offset: 0x0007CF92
	public ObjectLimit()
	{
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x0007EBBC File Offset: 0x0007CFBC
	private void Update()
	{
		base.transform.localPosition = new Vector3(Mathf.Clamp(base.gameObject.transform.localPosition.x, this.minX, this.maxX), Mathf.Clamp(base.gameObject.transform.localPosition.y, this.minY, this.maxY), Mathf.Clamp(base.gameObject.transform.localPosition.z, this.minZ, this.maxZ));
	}

	// Token: 0x040012B1 RID: 4785
	public float minX;

	// Token: 0x040012B2 RID: 4786
	public float maxX = 1f;

	// Token: 0x040012B3 RID: 4787
	public float minY;

	// Token: 0x040012B4 RID: 4788
	public float maxY = 1f;

	// Token: 0x040012B5 RID: 4789
	public float minZ;

	// Token: 0x040012B6 RID: 4790
	public float maxZ = 1f;
}
