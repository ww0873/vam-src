using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200004A RID: 74
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x06000171 RID: 369 RVA: 0x0000D984 File Offset: 0x0000BD84
		public UserLutComponent()
		{
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000D98C File Offset: 0x0000BD8C
		public override bool active
		{
			get
			{
				if (base.model != null)
				{
					UserLutModel.Settings settings = base.model.settings;
					return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && this.context != null && !this.context.interrupted;
				}
				return false;
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000DA28 File Offset: 0x0000BE28
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000DAB0 File Offset: 0x0000BEB0
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			Rect position = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height);
			GUI.DrawTexture(position, settings.lut);
		}

		// Token: 0x0200004B RID: 75
		private static class Uniforms
		{
			// Token: 0x06000175 RID: 373 RVA: 0x0000DB1C File Offset: 0x0000BF1C
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x04000202 RID: 514
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x04000203 RID: 515
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
