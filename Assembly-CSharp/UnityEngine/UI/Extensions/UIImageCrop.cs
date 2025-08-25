using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004FD RID: 1277
	[AddComponentMenu("UI/Effects/Extensions/UIImageCrop")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIImageCrop : MonoBehaviour
	{
		// Token: 0x0600203C RID: 8252 RVA: 0x000B88C8 File Offset: 0x000B6CC8
		public UIImageCrop()
		{
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000B88D0 File Offset: 0x000B6CD0
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000B88D8 File Offset: 0x000B6CD8
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			this.XCropProperty = Shader.PropertyToID("_XCrop");
			this.YCropProperty = Shader.PropertyToID("_YCrop");
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UI Image Crop"));
				}
				this.mat = this.mGraphic.material;
			}
			else
			{
				Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x000B8991 File Offset: 0x000B6D91
		public void OnValidate()
		{
			this.SetMaterial();
			this.SetXCrop(this.XCrop);
			this.SetYCrop(this.YCrop);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x000B89B1 File Offset: 0x000B6DB1
		public void SetXCrop(float xcrop)
		{
			this.XCrop = Mathf.Clamp01(xcrop);
			this.mat.SetFloat(this.XCropProperty, this.XCrop);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000B89D6 File Offset: 0x000B6DD6
		public void SetYCrop(float ycrop)
		{
			this.YCrop = Mathf.Clamp01(ycrop);
			this.mat.SetFloat(this.YCropProperty, this.YCrop);
		}

		// Token: 0x04001B0B RID: 6923
		private MaskableGraphic mGraphic;

		// Token: 0x04001B0C RID: 6924
		private Material mat;

		// Token: 0x04001B0D RID: 6925
		private int XCropProperty;

		// Token: 0x04001B0E RID: 6926
		private int YCropProperty;

		// Token: 0x04001B0F RID: 6927
		public float XCrop;

		// Token: 0x04001B10 RID: 6928
		public float YCrop;
	}
}
