using System;
using UnityEngine;

// Token: 0x0200041B RID: 1051
public class fire_wiggle : MonoBehaviour
{
	// Token: 0x06001A78 RID: 6776 RVA: 0x0009412D File Offset: 0x0009252D
	public fire_wiggle()
	{
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x00094140 File Offset: 0x00092540
	private void Start()
	{
		this.randomizer = UnityEngine.Random.Range(0.75f, 1.25f);
		ParticleSystem component = base.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = component.main;
		ParticleSystem.EmissionModule emission = component.emission;
		this.initial_start_speed = main.startSpeed.constant;
		this.initial_emission_rate = emission.rateOverTime.constant;
		this.initial_lifetime = main.startLifetime.constant;
		this.initial_size = main.startSize.constant;
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x000941D0 File Offset: 0x000925D0
	private void Update()
	{
		this.t += Time.deltaTime * this.randomizer;
		this.wiggle_t += Time.deltaTime * this.randomizer;
		ParticleSystem component = base.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = component.main;
		ParticleSystem.EmissionModule emission = component.emission;
		if (this.t > 2f + (2f - Mathf.Sin(this.wiggle_t)))
		{
			emission.rateOverTime = emission.rateOverTime.constant + (this.initial_emission_rate * 0.4f * this.fire_k - emission.rateOverTime.constant) / 30f;
			main.startLifetime = main.startLifetime.constant + (this.initial_lifetime * 0.9f * this.fire_k - main.startLifetime.constant) / 30f;
			if (emission.rateOverTime.constant < this.initial_emission_rate * 0.42f * this.fire_k)
			{
				emission.rateOverTime = this.initial_emission_rate * 1.1f * this.fire_k;
				main.startLifetime = this.initial_lifetime * 1.1f * this.fire_k;
				main.startSpeed = this.initial_start_speed * 0.7f * this.fire_k;
				main.startSize = this.initial_size * 1.1f * this.fire_k;
				this.randomizer = UnityEngine.Random.Range(0.75f, 1.25f);
				this.t = 0f;
			}
		}
		else
		{
			emission.rateOverTime = emission.rateOverTime.constant + (this.initial_emission_rate - emission.rateOverTime.constant) / 30f;
			main.startLifetime = main.startLifetime.constant + (this.initial_lifetime - main.startLifetime.constant) / 100f;
			main.startSpeed = main.startSpeed.constant + (this.initial_start_speed - main.startSpeed.constant) / 30f;
			main.startSize = main.startSize.constant + (this.initial_size - main.startSize.constant) / 30f;
		}
	}

	// Token: 0x0400158A RID: 5514
	private float t;

	// Token: 0x0400158B RID: 5515
	private float wiggle_t;

	// Token: 0x0400158C RID: 5516
	public float fire_k = 1f;

	// Token: 0x0400158D RID: 5517
	private float initial_start_speed;

	// Token: 0x0400158E RID: 5518
	private float initial_emission_rate;

	// Token: 0x0400158F RID: 5519
	private float initial_lifetime;

	// Token: 0x04001590 RID: 5520
	private float initial_size;

	// Token: 0x04001591 RID: 5521
	private float randomizer;
}
