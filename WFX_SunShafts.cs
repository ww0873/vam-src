using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class WFX_SunShafts : MonoBehaviour
{
	// Token: 0x06001293 RID: 4755 RVA: 0x00068C88 File Offset: 0x00067088
	public WFX_SunShafts()
	{
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x00068D08 File Offset: 0x00067108
	protected void NotSupported()
	{
		base.enabled = false;
		this.isSupported = false;
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x00068D18 File Offset: 0x00067118
	private bool CheckSupport(bool needDepth)
	{
		this.isSupported = true;
		if (!SystemInfo.supportsImageEffects)
		{
			this.NotSupported();
			return false;
		}
		if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.NotSupported();
			return false;
		}
		if (needDepth)
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}
		return true;
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x00068D74 File Offset: 0x00067174
	protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log("Missing shader in " + this.ToString());
			base.enabled = false;
			return null;
		}
		if (s.isSupported && m2Create && m2Create.shader == s)
		{
			return m2Create;
		}
		if (!s.isSupported)
		{
			this.NotSupported();
			Debug.Log(string.Concat(new string[]
			{
				"The shader ",
				s.ToString(),
				" on effect ",
				this.ToString(),
				" is not supported on this platform!"
			}));
			return null;
		}
		m2Create = new Material(s);
		this.createdMaterials.Add(m2Create);
		m2Create.hideFlags = HideFlags.DontSave;
		return m2Create;
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x00068E3D File Offset: 0x0006723D
	protected void ReportAutoDisable()
	{
		Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x00068E5C File Offset: 0x0006725C
	public bool CheckResources()
	{
		this.CheckSupport(this.useDepthTexture);
		this.sunShaftsShader = Shader.Find("Hidden/SunShaftsComposite");
		this.simpleClearShader = Shader.Find("Hidden/SimpleClear");
		this.sunShaftsMaterial = this.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
		this.simpleClearMaterial = this.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
		if (!this.isSupported)
		{
			this.ReportAutoDisable();
		}
		return this.isSupported;
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x00068EE0 File Offset: 0x000672E0
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (this.useDepthTexture)
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}
		int num = 4;
		if (this.resolution == WFX_SunShafts.SunShaftsResolution.Normal)
		{
			num = 2;
		}
		else if (this.resolution == WFX_SunShafts.SunShaftsResolution.High)
		{
			num = 1;
		}
		Vector3 vector = Vector3.one * 0.5f;
		if (this.sunTransform)
		{
			vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
		}
		else
		{
			vector = new Vector3(0.5f, 0.5f, 0f);
		}
		int width = source.width / num;
		int height = source.height / num;
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
		this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
		this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
		this.sunShaftsMaterial.SetVector("_SunThreshold", this.sunThreshold);
		if (!this.useDepthTexture)
		{
			RenderTextureFormat format = (!base.GetComponent<Camera>().allowHDR) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, format);
			RenderTexture.active = temporary2;
			GL.ClearWithSkybox(false, base.GetComponent<Camera>());
			this.sunShaftsMaterial.SetTexture("_Skybox", temporary2);
			Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
			RenderTexture.ReleaseTemporary(temporary2);
		}
		else
		{
			Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
		}
		this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
		float num2 = this.sunShaftBlurRadius * 0.0013020834f;
		this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
		this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
		for (int i = 0; i < this.radialBlurIterations; i++)
		{
			RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0);
			Graphics.Blit(temporary, temporary3, this.sunShaftsMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary);
			num2 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			temporary = RenderTexture.GetTemporary(width, height, 0);
			Graphics.Blit(temporary3, temporary, this.sunShaftsMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary3);
			num2 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
		}
		if (vector.z >= 0f)
		{
			this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
		}
		else
		{
			this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
		}
		this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
		Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode != WFX_SunShafts.ShaftsScreenBlendMode.Screen) ? 4 : 0);
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x0400101D RID: 4125
	public WFX_SunShafts.SunShaftsResolution resolution = WFX_SunShafts.SunShaftsResolution.Normal;

	// Token: 0x0400101E RID: 4126
	public WFX_SunShafts.ShaftsScreenBlendMode screenBlendMode;

	// Token: 0x0400101F RID: 4127
	public Transform sunTransform;

	// Token: 0x04001020 RID: 4128
	public int radialBlurIterations = 2;

	// Token: 0x04001021 RID: 4129
	public Color sunColor = Color.white;

	// Token: 0x04001022 RID: 4130
	public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

	// Token: 0x04001023 RID: 4131
	public float sunShaftBlurRadius = 2.5f;

	// Token: 0x04001024 RID: 4132
	public float sunShaftIntensity = 1.15f;

	// Token: 0x04001025 RID: 4133
	public float maxRadius = 0.75f;

	// Token: 0x04001026 RID: 4134
	public bool useDepthTexture = true;

	// Token: 0x04001027 RID: 4135
	public Shader sunShaftsShader;

	// Token: 0x04001028 RID: 4136
	private Material sunShaftsMaterial;

	// Token: 0x04001029 RID: 4137
	public Shader simpleClearShader;

	// Token: 0x0400102A RID: 4138
	private Material simpleClearMaterial;

	// Token: 0x0400102B RID: 4139
	private bool isSupported = true;

	// Token: 0x0400102C RID: 4140
	private List<Material> createdMaterials = new List<Material>();

	// Token: 0x02000315 RID: 789
	public enum SunShaftsResolution
	{
		// Token: 0x0400102E RID: 4142
		Low,
		// Token: 0x0400102F RID: 4143
		Normal,
		// Token: 0x04001030 RID: 4144
		High
	}

	// Token: 0x02000316 RID: 790
	public enum ShaftsScreenBlendMode
	{
		// Token: 0x04001032 RID: 4146
		Screen,
		// Token: 0x04001033 RID: 4147
		Add
	}
}
