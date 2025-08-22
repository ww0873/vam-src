using System;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class Underwater : MonoBehaviour
{
	// Token: 0x06001276 RID: 4726 RVA: 0x000670F4 File Offset: 0x000654F4
	public Underwater()
	{
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x0006712D File Offset: 0x0006552D
	private void Start()
	{
		this.defaultFog = RenderSettings.fog;
		this.defaultFogColor = RenderSettings.fogColor;
		this.defaultFogDensity = RenderSettings.fogDensity;
		this.defaultFogMod = RenderSettings.fogMode;
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x0006715C File Offset: 0x0006555C
	private void Update()
	{
		if (base.transform.position.y < this.UnderwaterLevel)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = this.FogColor;
			RenderSettings.fogDensity = this.FogDensity;
			RenderSettings.fogMode = this.FogMode;
		}
		else
		{
			RenderSettings.fog = this.defaultFog;
			RenderSettings.fogColor = this.defaultFogColor;
			RenderSettings.fogDensity = this.defaultFogDensity;
			RenderSettings.fogMode = this.defaultFogMod;
			RenderSettings.fogStartDistance = -300f;
		}
	}

	// Token: 0x04000FE3 RID: 4067
	public float UnderwaterLevel;

	// Token: 0x04000FE4 RID: 4068
	public Color FogColor = new Color(0f, 0.4f, 0.7f, 1f);

	// Token: 0x04000FE5 RID: 4069
	public float FogDensity = 0.04f;

	// Token: 0x04000FE6 RID: 4070
	public FogMode FogMode = FogMode.Exponential;

	// Token: 0x04000FE7 RID: 4071
	private bool defaultFog;

	// Token: 0x04000FE8 RID: 4072
	private Color defaultFogColor;

	// Token: 0x04000FE9 RID: 4073
	private float defaultFogDensity;

	// Token: 0x04000FEA RID: 4074
	private FogMode defaultFogMod;

	// Token: 0x04000FEB RID: 4075
	private Material defaultSkybox;
}
