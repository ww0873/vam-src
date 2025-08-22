using System;
using UnityEngine;

namespace Wilberforce.VAO
{
	// Token: 0x02000571 RID: 1393
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[HelpURL("https://projectwilberforce.github.io/vaomanual/")]
	[AddComponentMenu("Image Effects/Rendering/Volumetric Ambient Occlusion")]
	public class VAOEffect : VAOEffectCommandBuffer
	{
		// Token: 0x0600234B RID: 9035 RVA: 0x000CCE75 File Offset: 0x000CB275
		public VAOEffect()
		{
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000CCE7D File Offset: 0x000CB27D
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.PerformOnRenderImage(source, destination);
		}
	}
}
