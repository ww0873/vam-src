using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004FF RID: 1279
	[AddComponentMenu("UI/Effects/Extensions/UIMultiplyEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIMultiplyEffect : MonoBehaviour
	{
		// Token: 0x06002046 RID: 8262 RVA: 0x000B8A9C File Offset: 0x000B6E9C
		public UIMultiplyEffect()
		{
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000B8AA4 File Offset: 0x000B6EA4
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000B8AAC File Offset: 0x000B6EAC
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIMultiply"));
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000B8B34 File Offset: 0x000B6F34
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04001B12 RID: 6930
		private MaskableGraphic mGraphic;
	}
}
