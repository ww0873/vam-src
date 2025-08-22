using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E80 RID: 3712
	[AddComponentMenu("")]
	public class ImageEffects
	{
		// Token: 0x060070FC RID: 28924 RVA: 0x002AE225 File Offset: 0x002AC625
		public ImageEffects()
		{
		}

		// Token: 0x060070FD RID: 28925 RVA: 0x002AE230 File Offset: 0x002AC630
		public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
		{
			bool flag = source.texelSize.y < 0f;
			if (flag)
			{
				center.y = 1f - center.y;
				angle = -angle;
			}
			Matrix4x4 value = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
			material.SetMatrix("_RotationMatrix", value);
			material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
			material.SetFloat("_Angle", angle * 0.017453292f);
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x060070FE RID: 28926 RVA: 0x002AE2E3 File Offset: 0x002AC6E3
		[Obsolete("Use Graphics.Blit(source,dest) instead")]
		public static void Blit(RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest);
		}

		// Token: 0x060070FF RID: 28927 RVA: 0x002AE2EC File Offset: 0x002AC6EC
		[Obsolete("Use Graphics.Blit(source, destination, material) instead")]
		public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest, material);
		}
	}
}
