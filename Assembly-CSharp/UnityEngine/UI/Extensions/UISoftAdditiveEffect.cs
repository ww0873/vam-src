using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000501 RID: 1281
	[AddComponentMenu("UI/Effects/Extensions/UISoftAdditiveEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UISoftAdditiveEffect : MonoBehaviour
	{
		// Token: 0x0600204E RID: 8270 RVA: 0x000B8BDC File Offset: 0x000B6FDC
		public UISoftAdditiveEffect()
		{
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000B8BE4 File Offset: 0x000B6FE4
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000B8BEC File Offset: 0x000B6FEC
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UISoftAdditive"));
				}
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000B8C74 File Offset: 0x000B7074
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04001B14 RID: 6932
		private MaskableGraphic mGraphic;
	}
}
