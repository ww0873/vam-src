using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000500 RID: 1280
	[AddComponentMenu("UI/Effects/Extensions/UIScreenEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIScreenEffect : MonoBehaviour
	{
		// Token: 0x0600204A RID: 8266 RVA: 0x000B8B3C File Offset: 0x000B6F3C
		public UIScreenEffect()
		{
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000B8B44 File Offset: 0x000B6F44
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000B8B4C File Offset: 0x000B6F4C
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIScreen"));
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000B8BD4 File Offset: 0x000B6FD4
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04001B13 RID: 6931
		private MaskableGraphic mGraphic;
	}
}
