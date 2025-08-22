using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E7A RID: 3706
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
	public class EdgeDetection : PostEffectsBase
	{
		// Token: 0x060070EA RID: 28906 RVA: 0x002ADB40 File Offset: 0x002ABF40
		public EdgeDetection()
		{
		}

		// Token: 0x060070EB RID: 28907 RVA: 0x002ADBA4 File Offset: 0x002ABFA4
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.edgeDetectMaterial = base.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
			if (this.mode != this.oldMode)
			{
				this.SetCameraFlag();
			}
			this.oldMode = this.mode;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060070EC RID: 28908 RVA: 0x002ADC0B File Offset: 0x002AC00B
		private new void Start()
		{
			this.oldMode = this.mode;
		}

		// Token: 0x060070ED RID: 28909 RVA: 0x002ADC1C File Offset: 0x002AC01C
		private void SetCameraFlag()
		{
			if (this.mode == EdgeDetection.EdgeDetectMode.SobelDepth || this.mode == EdgeDetection.EdgeDetectMode.SobelDepthThin)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			else if (this.mode == EdgeDetection.EdgeDetectMode.TriangleDepthNormals || this.mode == EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
			}
		}

		// Token: 0x060070EE RID: 28910 RVA: 0x002ADC83 File Offset: 0x002AC083
		private void OnEnable()
		{
			this.SetCameraFlag();
		}

		// Token: 0x060070EF RID: 28911 RVA: 0x002ADC8C File Offset: 0x002AC08C
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector2 vector = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
			this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
			this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
			this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
			this.edgeDetectMaterial.SetVector("_BgColor", this.edgesOnlyBgColor);
			this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
			this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
			Graphics.Blit(source, destination, this.edgeDetectMaterial, (int)this.mode);
		}

		// Token: 0x04006431 RID: 25649
		public EdgeDetection.EdgeDetectMode mode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x04006432 RID: 25650
		public float sensitivityDepth = 1f;

		// Token: 0x04006433 RID: 25651
		public float sensitivityNormals = 1f;

		// Token: 0x04006434 RID: 25652
		public float lumThreshold = 0.2f;

		// Token: 0x04006435 RID: 25653
		public float edgeExp = 1f;

		// Token: 0x04006436 RID: 25654
		public float sampleDist = 1f;

		// Token: 0x04006437 RID: 25655
		public float edgesOnly;

		// Token: 0x04006438 RID: 25656
		public Color edgesOnlyBgColor = Color.white;

		// Token: 0x04006439 RID: 25657
		public Shader edgeDetectShader;

		// Token: 0x0400643A RID: 25658
		private Material edgeDetectMaterial;

		// Token: 0x0400643B RID: 25659
		private EdgeDetection.EdgeDetectMode oldMode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x02000E7B RID: 3707
		public enum EdgeDetectMode
		{
			// Token: 0x0400643D RID: 25661
			TriangleDepthNormals,
			// Token: 0x0400643E RID: 25662
			RobertsCrossDepthNormals,
			// Token: 0x0400643F RID: 25663
			SobelDepth,
			// Token: 0x04006440 RID: 25664
			SobelDepthThin,
			// Token: 0x04006441 RID: 25665
			TriangleLuminance
		}
	}
}
