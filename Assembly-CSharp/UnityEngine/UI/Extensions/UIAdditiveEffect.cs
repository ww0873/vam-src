using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004FC RID: 1276
	[AddComponentMenu("UI/Effects/Extensions/UIAdditiveEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIAdditiveEffect : MonoBehaviour
	{
		// Token: 0x06002038 RID: 8248 RVA: 0x000B8828 File Offset: 0x000B6C28
		public UIAdditiveEffect()
		{
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000B8830 File Offset: 0x000B6C30
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x000B8838 File Offset: 0x000B6C38
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIAdditive"));
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000B88C0 File Offset: 0x000B6CC0
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04001B0A RID: 6922
		private MaskableGraphic mGraphic;
	}
}
