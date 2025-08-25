using System;
using UnityEngine;

// Token: 0x02000311 RID: 785
public class UnderwaterPostEffects : MonoBehaviour
{
	// Token: 0x06001279 RID: 4729 RVA: 0x000671EC File Offset: 0x000655EC
	public UnderwaterPostEffects()
	{
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x00067258 File Offset: 0x00065658
	private void OnEnable()
	{
		this.cam = Camera.main;
		this.SunShafts = this.cam.gameObject.AddComponent<WFX_SunShafts>();
		this.SunShafts.sunShaftIntensity = this.ShuftsIntensity;
		GameObject gameObject = new GameObject("SunShaftTarget");
		this.SunShafts.sunTransform = gameObject.transform;
		gameObject.transform.parent = this.cam.transform;
		gameObject.transform.localPosition = this.SunShaftTargetPosition;
		this.SunShafts.screenBlendMode = this.SunShuftsScreenBlend;
		this.SunShafts.sunShaftsShader = Shader.Find("Hidden/SunShaftsComposite");
		this.SunShafts.simpleClearShader = Shader.Find("Hidden/SimpleClear");
		Underwater underwater = this.cam.gameObject.AddComponent<Underwater>();
		underwater.UnderwaterLevel = base.transform.position.y;
		underwater.FogColor = this.FogColor;
		underwater.FogDensity = this.FogDensity;
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00067358 File Offset: 0x00065758
	private void Update()
	{
		if (this.cam == null)
		{
			return;
		}
		if (this.cam.transform.position.y < base.transform.position.y)
		{
			if (!this.SunShafts.enabled)
			{
				this.SunShafts.enabled = true;
			}
		}
		else if (this.SunShafts.enabled)
		{
			this.SunShafts.enabled = false;
		}
	}

	// Token: 0x04000FEC RID: 4076
	public Color FogColor = new Color(0.34117648f, 0.74509805f, 0.85882354f, 1f);

	// Token: 0x04000FED RID: 4077
	public float FogDensity = 0.05f;

	// Token: 0x04000FEE RID: 4078
	public bool UseSunShafts = true;

	// Token: 0x04000FEF RID: 4079
	public float ShuftsIntensity = 5f;

	// Token: 0x04000FF0 RID: 4080
	public WFX_SunShafts.ShaftsScreenBlendMode SunShuftsScreenBlend;

	// Token: 0x04000FF1 RID: 4081
	private Vector3 SunShaftTargetPosition = new Vector3(0f, 7f, 10f);

	// Token: 0x04000FF2 RID: 4082
	private Camera cam;

	// Token: 0x04000FF3 RID: 4083
	private WFX_SunShafts SunShafts;
}
