using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E81 RID: 3713
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Motion Blur (Color Accumulation)")]
	[RequireComponent(typeof(Camera))]
	public class MotionBlur : ImageEffectBase
	{
		// Token: 0x06007100 RID: 28928 RVA: 0x002AE2F6 File Offset: 0x002AC6F6
		public MotionBlur()
		{
		}

		// Token: 0x06007101 RID: 28929 RVA: 0x002AE309 File Offset: 0x002AC709
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06007102 RID: 28930 RVA: 0x002AE311 File Offset: 0x002AC711
		protected override void OnDisable()
		{
			base.OnDisable();
			UnityEngine.Object.DestroyImmediate(this.accumTexture);
		}

		// Token: 0x06007103 RID: 28931 RVA: 0x002AE324 File Offset: 0x002AC724
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.accumTexture == null || this.accumTexture.width != source.width || this.accumTexture.height != source.height)
			{
				UnityEngine.Object.DestroyImmediate(this.accumTexture);
				this.accumTexture = new RenderTexture(source.width, source.height, 0);
				this.accumTexture.hideFlags = HideFlags.HideAndDontSave;
				Graphics.Blit(source, this.accumTexture);
			}
			if (this.extraBlur)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
				this.accumTexture.MarkRestoreExpected();
				Graphics.Blit(this.accumTexture, temporary);
				Graphics.Blit(temporary, this.accumTexture);
				RenderTexture.ReleaseTemporary(temporary);
			}
			this.blurAmount = Mathf.Clamp(this.blurAmount, 0f, 0.92f);
			base.material.SetTexture("_MainTex", this.accumTexture);
			base.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
			this.accumTexture.MarkRestoreExpected();
			Graphics.Blit(source, this.accumTexture, base.material);
			Graphics.Blit(this.accumTexture, destination);
		}

		// Token: 0x04006453 RID: 25683
		[Range(0f, 0.92f)]
		public float blurAmount = 0.8f;

		// Token: 0x04006454 RID: 25684
		public bool extraBlur;

		// Token: 0x04006455 RID: 25685
		private RenderTexture accumTexture;
	}
}
