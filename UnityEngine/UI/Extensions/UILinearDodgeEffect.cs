using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004FE RID: 1278
	[AddComponentMenu("UI/Effects/Extensions/UILinearDodgeEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UILinearDodgeEffect : MonoBehaviour
	{
		// Token: 0x06002042 RID: 8258 RVA: 0x000B89FB File Offset: 0x000B6DFB
		public UILinearDodgeEffect()
		{
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000B8A03 File Offset: 0x000B6E03
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000B8A0C File Offset: 0x000B6E0C
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UILinearDodge"));
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000B8A94 File Offset: 0x000B6E94
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04001B11 RID: 6929
		private MaskableGraphic mGraphic;
	}
}
