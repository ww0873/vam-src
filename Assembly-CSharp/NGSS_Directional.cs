using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000416 RID: 1046
[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
public class NGSS_Directional : MonoBehaviour
{
	// Token: 0x06001A6A RID: 6762 RVA: 0x000939E8 File Offset: 0x00091DE8
	public NGSS_Directional()
	{
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x00093A55 File Offset: 0x00091E55
	private void OnDisable()
	{
		this.isInitialized = false;
		if (this.KEEP_NGSS_ONDISABLE)
		{
			return;
		}
		if (this.isGraphicSet)
		{
			this.isGraphicSet = false;
			GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
			GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseBuiltin);
		}
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x00093A93 File Offset: 0x00091E93
	private void OnEnable()
	{
		if (this.IsNotSupported())
		{
			Debug.LogWarning("Unsupported graphics API, NGSS requires at least SM3.0 or higher and DX9 is not supported.", this);
			base.enabled = false;
			return;
		}
		this.Init();
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x00093AB9 File Offset: 0x00091EB9
	private void Init()
	{
		if (this.isInitialized)
		{
			return;
		}
		if (!this.isGraphicSet)
		{
			GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseCustom);
			GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/NGSS_Directional"));
			this.isGraphicSet = true;
		}
		this.isInitialized = true;
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x00093AF7 File Offset: 0x00091EF7
	private bool IsNotSupported()
	{
		return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.PlayStationVita || SystemInfo.graphicsDeviceType == GraphicsDeviceType.N3DS;
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x00093B1C File Offset: 0x00091F1C
	private void Update()
	{
		if (this.NOISE_STATIC)
		{
			Shader.EnableKeyword("NGSS_NOISE_STATIC_DIR");
		}
		else
		{
			Shader.DisableKeyword("NGSS_NOISE_STATIC_DIR");
		}
		if (this.CASCADES_BLENDING && QualitySettings.shadowCascades > 1)
		{
			Shader.EnableKeyword("NGSS_USE_CASCADE_BLENDING");
			Shader.SetGlobalFloat("NGSS_CASCADE_BLEND_DISTANCE", this.CASCADES_BLENDING_VALUE * 0.125f);
		}
		else
		{
			Shader.DisableKeyword("NGSS_USE_CASCADE_BLENDING");
		}
		if (this.EARLY_BAILOUT_OPTIMIZATION)
		{
			Shader.EnableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION_DIR");
		}
		else
		{
			Shader.DisableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION_DIR");
		}
		Shader.SetGlobalFloat("NGSS_POISSON_SAMPLING_NOISE_DIR", this.NOISE_SCALE_VALUE / 0.01f);
		Shader.SetGlobalFloat("NGSS_STATIC_NOISE_MOBILE_VALUE", this.NOISE_SCALE_VALUE * 0.5f);
		Shader.SetGlobalFloat("NGSS_PCSS_GLOBAL_SOFTNESS", this.GLOBAL_SOFTNESS / (QualitySettings.shadowDistance * 0.66f));
		Shader.SetGlobalFloat("NGSS_PCSS_GLOBAL_SOFTNESS_MOBILE", 1f - this.GLOBAL_SOFTNESS * 75f / QualitySettings.shadowDistance);
		if (this.PCSS_ENABLED)
		{
			Shader.EnableKeyword("NGSS_PCSS_FILTER_DIR");
		}
		else
		{
			Shader.DisableKeyword("NGSS_PCSS_FILTER_DIR");
		}
		float num = this.PCSS_SOFTNESS_MIN * 0.05f;
		float num2 = this.PCSS_SOFTNESS_MAX * 0.25f;
		Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MIN", (num <= num2) ? num : num2);
		Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MAX", (num2 >= num) ? num2 : num);
		Shader.DisableKeyword("DIR_POISSON_64");
		Shader.DisableKeyword("DIR_POISSON_32");
		Shader.DisableKeyword("DIR_POISSON_25");
		Shader.DisableKeyword("DIR_POISSON_16");
		Shader.EnableKeyword((this.SAMPLERS_COUNT != NGSS_Directional.SAMPLER_COUNT.SAMPLERS_64) ? ((this.SAMPLERS_COUNT != NGSS_Directional.SAMPLER_COUNT.SAMPLERS_32) ? ((this.SAMPLERS_COUNT != NGSS_Directional.SAMPLER_COUNT.SAMPLERS_25) ? "DIR_POISSON_16" : "DIR_POISSON_25") : "DIR_POISSON_32") : "DIR_POISSON_64");
	}

	// Token: 0x04001569 RID: 5481
	[Header("MAIN SETTINGS")]
	[Tooltip("If false, NGSS Directional shadows replacement will be removed from Graphics settings when OnDisable is called in this component.")]
	public bool KEEP_NGSS_ONDISABLE = true;

	// Token: 0x0400156A RID: 5482
	[Header("OPTIMIZATION")]
	[Tooltip("Optimize shadows performance by skipping fragments that are either 100% lit or 100% shadowed. Some macro noisy artefacts can be seen if shadows are too soft or sampling amount is below 64.")]
	public bool EARLY_BAILOUT_OPTIMIZATION = true;

	// Token: 0x0400156B RID: 5483
	[Tooltip("Recommended values: Mobile = 16, Consoles = 25, Desktop VR = 32, Desktop High = 64")]
	public NGSS_Directional.SAMPLER_COUNT SAMPLERS_COUNT = NGSS_Directional.SAMPLER_COUNT.SAMPLERS_64;

	// Token: 0x0400156C RID: 5484
	[Header("SOFTNESS")]
	[Tooltip("Overall softness for both PCF and PCSS shadows.")]
	[Range(0f, 2f)]
	public float GLOBAL_SOFTNESS = 1f;

	// Token: 0x0400156D RID: 5485
	[Header("CASCADES")]
	[Tooltip("Blends cascades at seams intersection.\nAdditional overhead required for this option.")]
	public bool CASCADES_BLENDING = true;

	// Token: 0x0400156E RID: 5486
	[Tooltip("Blends cascades at seams intersection.\nAdditional overhead required for this option.")]
	[Range(0f, 2f)]
	public float CASCADES_BLENDING_VALUE = 1f;

	// Token: 0x0400156F RID: 5487
	[Header("NOISE")]
	[Tooltip("If disabled, noise will be computed normally.\nIf enabled, noise will be computed statically from an internal screen-space texture.")]
	public bool NOISE_STATIC;

	// Token: 0x04001570 RID: 5488
	[Tooltip("Amount of noise. The higher the value the more Noise.")]
	[Range(0f, 2f)]
	public float NOISE_SCALE_VALUE = 1f;

	// Token: 0x04001571 RID: 5489
	[Header("PCSS")]
	[Tooltip("PCSS Requires inline sampling and SM3.5, only available in Unity 2017.\nIt provides Area Light like soft-shadows.\nDisable it if you are looking for PCF filtering (uniform soft-shadows) which runs with SM3.0.")]
	public bool PCSS_ENABLED = true;

	// Token: 0x04001572 RID: 5490
	[Tooltip("PCSS softness when shadows is close to caster.")]
	[Range(0f, 2f)]
	public float PCSS_SOFTNESS_MIN = 1f;

	// Token: 0x04001573 RID: 5491
	[Tooltip("PCSS softness when shadows is far from caster.")]
	[Range(0f, 2f)]
	public float PCSS_SOFTNESS_MAX = 1f;

	// Token: 0x04001574 RID: 5492
	private bool isInitialized;

	// Token: 0x04001575 RID: 5493
	private bool isGraphicSet;

	// Token: 0x02000417 RID: 1047
	public enum SAMPLER_COUNT
	{
		// Token: 0x04001577 RID: 5495
		SAMPLERS_16,
		// Token: 0x04001578 RID: 5496
		SAMPLERS_25,
		// Token: 0x04001579 RID: 5497
		SAMPLERS_32,
		// Token: 0x0400157A RID: 5498
		SAMPLERS_64
	}
}
