using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D2D RID: 3373
public class UIRenderQueueControl : JSONStorable
{
	// Token: 0x06006756 RID: 26454 RVA: 0x0026DFA3 File Offset: 0x0026C3A3
	public UIRenderQueueControl()
	{
	}

	// Token: 0x06006757 RID: 26455 RVA: 0x0026DFAC File Offset: 0x0026C3AC
	protected void SyncRenderQueue(float f)
	{
		int renderQueue = Mathf.FloorToInt(f);
		if (this.runtimeMaterial != null)
		{
			this.runtimeMaterial.renderQueue = renderQueue;
		}
		if (this.graphicsToUse != null)
		{
			foreach (Graphic graphic in this.graphicsToUse)
			{
				graphic.materialForRendering.renderQueue = renderQueue;
			}
		}
	}

	// Token: 0x17000F14 RID: 3860
	// (get) Token: 0x06006758 RID: 26456 RVA: 0x0026E014 File Offset: 0x0026C414
	// (set) Token: 0x06006759 RID: 26457 RVA: 0x0026E040 File Offset: 0x0026C440
	public int renderQueue
	{
		get
		{
			if (this.renderQueueJSON != null)
			{
				return Mathf.FloorToInt(this.renderQueueJSON.val);
			}
			return 0;
		}
		set
		{
			if (this.renderQueueJSON != null)
			{
				this.renderQueueJSON.val = (float)value;
			}
		}
	}

	// Token: 0x0600675A RID: 26458 RVA: 0x0026E05C File Offset: 0x0026C45C
	protected void Init()
	{
		if (this.sharedMaterial != null)
		{
			this.runtimeMaterial = UnityEngine.Object.Instantiate<Material>(this.sharedMaterial);
			this.renderQueueJSON = new JSONStorableFloat("renderQueue", (float)this.runtimeMaterial.renderQueue, new JSONStorableFloat.SetFloatCallback(this.SyncRenderQueue), -1f, 5000f, true, true);
			base.RegisterFloat(this.renderQueueJSON);
			if (this.rendererContainer != null)
			{
				Graphic[] componentsInChildren = this.rendererContainer.GetComponentsInChildren<Graphic>(true);
				List<Graphic> list = new List<Graphic>();
				foreach (Graphic graphic in componentsInChildren)
				{
					if (!graphic.GetComponent<UIRenderQueueControlIgnore>())
					{
						list.Add(graphic);
						graphic.material = this.runtimeMaterial;
					}
				}
				this.graphicsToUse = list.ToArray();
			}
		}
	}

	// Token: 0x0600675B RID: 26459 RVA: 0x0026E140 File Offset: 0x0026C540
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			UIRenderQueueControlUI componentInChildren = t.GetComponentInChildren<UIRenderQueueControlUI>(true);
			if (componentInChildren != null && this.renderQueueJSON != null)
			{
				this.renderQueueJSON.RegisterSlider(componentInChildren.renderQueueSlider, isAlt);
			}
		}
	}

	// Token: 0x0600675C RID: 26460 RVA: 0x0026E18A File Offset: 0x0026C58A
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

	// Token: 0x04005854 RID: 22612
	protected JSONStorableFloat renderQueueJSON;

	// Token: 0x04005855 RID: 22613
	public Transform rendererContainer;

	// Token: 0x04005856 RID: 22614
	public Material sharedMaterial;

	// Token: 0x04005857 RID: 22615
	protected Material runtimeMaterial;

	// Token: 0x04005858 RID: 22616
	protected Graphic[] graphicsToUse;
}
