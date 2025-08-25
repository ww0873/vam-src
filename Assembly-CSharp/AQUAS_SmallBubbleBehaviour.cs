using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class AQUAS_SmallBubbleBehaviour : MonoBehaviour
{
	// Token: 0x060000CE RID: 206 RVA: 0x00008155 File Offset: 0x00006555
	public AQUAS_SmallBubbleBehaviour()
	{
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0000815D File Offset: 0x0000655D
	private void Start()
	{
		this.updriftFactor = UnityEngine.Random.Range(-this.averageUpdrift * 0.75f, this.averageUpdrift * 0.75f);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00008184 File Offset: 0x00006584
	private void Update()
	{
		base.transform.Translate(Vector3.up * Time.deltaTime * (this.averageUpdrift + this.updriftFactor), Space.World);
		if (this.mainCamera.transform.position.y > this.waterLevel || base.transform.position.y > this.waterLevel)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400010C RID: 268
	public float averageUpdrift;

	// Token: 0x0400010D RID: 269
	public float waterLevel;

	// Token: 0x0400010E RID: 270
	public GameObject mainCamera;

	// Token: 0x0400010F RID: 271
	private float updriftFactor;
}
