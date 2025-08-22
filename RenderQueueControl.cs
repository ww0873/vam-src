using System;
using UnityEngine;

// Token: 0x02000D28 RID: 3368
public class RenderQueueControl : JSONStorable
{
	// Token: 0x06006743 RID: 26435 RVA: 0x0026D995 File Offset: 0x0026BD95
	public RenderQueueControl()
	{
	}

	// Token: 0x06006744 RID: 26436 RVA: 0x0026D9A4 File Offset: 0x0026BDA4
	protected void SyncMaterials()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer renderer in componentsInChildren)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material != null)
				{
					material.renderQueue = this._renderQueue;
				}
			}
		}
	}

	// Token: 0x06006745 RID: 26437 RVA: 0x0026DA19 File Offset: 0x0026BE19
	protected void SyncRenderQueue(float f)
	{
		this._renderQueue = Mathf.FloorToInt(f);
		this.SyncMaterials();
	}

	// Token: 0x06006746 RID: 26438 RVA: 0x0026DA30 File Offset: 0x0026BE30
	protected void Init()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer renderer in componentsInChildren)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material != null)
				{
					this._renderQueue = material.renderQueue;
					break;
				}
			}
		}
		this.renderQueueJSON = new JSONStorableFloat("renderQueue", (float)this._renderQueue, new JSONStorableFloat.SetFloatCallback(this.SyncRenderQueue), -1f, 5000f, true, true);
		base.RegisterFloat(this.renderQueueJSON);
		this.SyncMaterials();
	}

	// Token: 0x06006747 RID: 26439 RVA: 0x0026DAEC File Offset: 0x0026BEEC
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			RenderQueueControlUI componentInChildren = t.GetComponentInChildren<RenderQueueControlUI>(true);
			if (componentInChildren != null && this.renderQueueJSON != null)
			{
				this.renderQueueJSON.RegisterSlider(componentInChildren.renderQueueSlider, isAlt);
			}
		}
	}

	// Token: 0x06006748 RID: 26440 RVA: 0x0026DB36 File Offset: 0x0026BF36
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			if (Application.isPlaying)
			{
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}
	}

	// Token: 0x04005843 RID: 22595
	protected int _renderQueue = -1;

	// Token: 0x04005844 RID: 22596
	public JSONStorableFloat renderQueueJSON;
}
