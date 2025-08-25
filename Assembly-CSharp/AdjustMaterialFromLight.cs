using System;
using UnityEngine;

// Token: 0x02000D0C RID: 3340
public class AdjustMaterialFromLight : MonoBehaviour
{
	// Token: 0x060065E2 RID: 26082 RVA: 0x00267239 File Offset: 0x00265639
	public AdjustMaterialFromLight()
	{
	}

	// Token: 0x060065E3 RID: 26083 RVA: 0x0026724C File Offset: 0x0026564C
	private void Start()
	{
	}

	// Token: 0x060065E4 RID: 26084 RVA: 0x00267250 File Offset: 0x00265650
	private void Update()
	{
		if (this.material && this.lightComponent)
		{
			Vector4 v;
			v.x = Mathf.Clamp01(this.lightComponent.color.r * this.lightComponent.intensity * this.intensityFactor);
			v.y = Mathf.Clamp01(this.lightComponent.color.g * this.lightComponent.intensity * this.intensityFactor);
			v.z = Mathf.Clamp01(this.lightComponent.color.b * this.lightComponent.intensity * this.intensityFactor);
			v.w = 1f;
			if (this.material.HasProperty("_GlowColor"))
			{
				this.material.SetColor("_GlowColor", v);
			}
			else
			{
				this.material.color = v;
			}
		}
	}

	// Token: 0x0400555B RID: 21851
	public Material material;

	// Token: 0x0400555C RID: 21852
	public Light lightComponent;

	// Token: 0x0400555D RID: 21853
	public float intensityFactor = 1f;
}
