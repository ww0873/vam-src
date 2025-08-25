using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D26 RID: 3366
public class PixelationControl : JSONStorable
{
	// Token: 0x0600673C RID: 26428 RVA: 0x0026D7C6 File Offset: 0x0026BBC6
	public PixelationControl()
	{
	}

	// Token: 0x0600673D RID: 26429 RVA: 0x0026D7DC File Offset: 0x0026BBDC
	protected void SyncMaterials()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material != null && material.HasProperty("_Pixelation"))
				{
					material.SetFloat("_Pixelation", this._pixelation);
				}
			}
		}
		RawImage[] componentsInChildren2 = base.GetComponentsInChildren<RawImage>();
		foreach (RawImage rawImage in componentsInChildren2)
		{
			Material materialForRendering = rawImage.materialForRendering;
			if (materialForRendering != null && materialForRendering.HasProperty("_Pixelation"))
			{
				materialForRendering.SetFloat("_Pixelation", this._pixelation);
			}
		}
	}

	// Token: 0x0600673E RID: 26430 RVA: 0x0026D8CB File Offset: 0x0026BCCB
	protected void SyncPixelation(float f)
	{
		this._pixelation = f;
		this.SyncMaterials();
	}

	// Token: 0x0600673F RID: 26431 RVA: 0x0026D8DC File Offset: 0x0026BCDC
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			PixelationControlUI componentInChildren = t.GetComponentInChildren<PixelationControlUI>(true);
			if (componentInChildren != null)
			{
				this.pixelationJSON.RegisterSlider(componentInChildren.pixelationSlider, isAlt);
			}
		}
	}

	// Token: 0x06006740 RID: 26432 RVA: 0x0026D91C File Offset: 0x0026BD1C
	protected void Init()
	{
		this.pixelationJSON = new JSONStorableFloat("pixelation", 0.02f, new JSONStorableFloat.SetFloatCallback(this.SyncPixelation), 0.001f, 0.1f, true, true);
		base.RegisterFloat(this.pixelationJSON);
		this.SyncMaterials();
	}

	// Token: 0x06006741 RID: 26433 RVA: 0x0026D968 File Offset: 0x0026BD68
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x04005840 RID: 22592
	[SerializeField]
	protected float _pixelation = 0.02f;

	// Token: 0x04005841 RID: 22593
	public JSONStorableFloat pixelationJSON;
}
