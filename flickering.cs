using System;
using UnityEngine;

// Token: 0x0200041C RID: 1052
public class flickering : MonoBehaviour
{
	// Token: 0x06001A7B RID: 6779 RVA: 0x00094484 File Offset: 0x00092884
	public flickering()
	{
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x00094498 File Offset: 0x00092898
	private void Start()
	{
		this.light = base.GetComponent<Light>();
		if (this.light != null)
		{
			this.intensity = this.light.intensity;
			this.intensityTarget = this.intensity;
			this.randomizer = UnityEngine.Random.Range(0.75f, 1.25f);
		}
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000944F4 File Offset: 0x000928F4
	private void Update()
	{
		if (this.light != null)
		{
			this.t += Time.deltaTime * this.randomizer * this.rate;
			this.intensityTarget = this.intensity + Mathf.Sin(this.t * (1f - Mathf.Sin(this.t * 25f)) * 5f) * this.intensity / 5f;
			this.light.intensity = this.intensityTarget;
		}
	}

	// Token: 0x04001592 RID: 5522
	public float intensity;

	// Token: 0x04001593 RID: 5523
	public float rate = 0.02f;

	// Token: 0x04001594 RID: 5524
	private float t;

	// Token: 0x04001595 RID: 5525
	private float randomizer;

	// Token: 0x04001596 RID: 5526
	public float intensityTarget;

	// Token: 0x04001597 RID: 5527
	private Light light;
}
