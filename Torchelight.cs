using System;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class Torchelight : MonoBehaviour
{
	// Token: 0x06001A75 RID: 6773 RVA: 0x00093EE1 File Offset: 0x000922E1
	public Torchelight()
	{
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x00093EEC File Offset: 0x000922EC
	private void Start()
	{
		this.TorchLight.GetComponent<Light>().intensity = this.IntensityLight;
		this.MainFlame.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 20f;
		this.BaseFlame.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 15f;
		this.Etincelles.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 7f;
		this.Fumee.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 12f;
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x00093FB4 File Offset: 0x000923B4
	private void Update()
	{
		if (this.IntensityLight < 0f)
		{
			this.IntensityLight = 0f;
		}
		if (this.IntensityLight > this.MaxLightIntensity)
		{
			this.IntensityLight = this.MaxLightIntensity;
		}
		this.TorchLight.GetComponent<Light>().intensity = this.IntensityLight / 2f + Mathf.Lerp(this.IntensityLight - 0.1f, this.IntensityLight + 0.1f, Mathf.Cos(Time.time * 30f));
		this.TorchLight.GetComponent<Light>().color = new Color(Mathf.Min(this.IntensityLight / 1.5f, 1f), Mathf.Min(this.IntensityLight / 2f, 1f), 0f);
		this.MainFlame.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 20f;
		this.BaseFlame.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 15f;
		this.Etincelles.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 7f;
		this.Fumee.GetComponent<ParticleSystem>().emission.rateOverTime = this.IntensityLight * 12f;
	}

	// Token: 0x04001583 RID: 5507
	public GameObject TorchLight;

	// Token: 0x04001584 RID: 5508
	public GameObject MainFlame;

	// Token: 0x04001585 RID: 5509
	public GameObject BaseFlame;

	// Token: 0x04001586 RID: 5510
	public GameObject Etincelles;

	// Token: 0x04001587 RID: 5511
	public GameObject Fumee;

	// Token: 0x04001588 RID: 5512
	public float MaxLightIntensity;

	// Token: 0x04001589 RID: 5513
	public float IntensityLight;
}
