using System;
using UnityEngine;

namespace mset
{
	// Token: 0x0200032A RID: 810
	public class Logo : MonoBehaviour
	{
		// Token: 0x0600131D RID: 4893 RVA: 0x0006E4C8 File Offset: 0x0006C8C8
		public Logo()
		{
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0006E536 File Offset: 0x0006C936
		private void Reset()
		{
			this.logoTexture = (Resources.Load("renderedLogo") as Texture2D);
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x0006E54D File Offset: 0x0006C94D
		private void Start()
		{
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0006E550 File Offset: 0x0006C950
		private void updateTexRect()
		{
			if (this.logoTexture)
			{
				float num = (float)this.logoTexture.width;
				float num2 = (float)this.logoTexture.height;
				float num3 = 0f;
				float num4 = 0f;
				if (base.GetComponent<Camera>())
				{
					num3 = (float)base.GetComponent<Camera>().pixelWidth;
					num4 = (float)base.GetComponent<Camera>().pixelHeight;
				}
				else if (Camera.main)
				{
					num3 = (float)Camera.main.pixelWidth;
					num4 = (float)Camera.main.pixelHeight;
				}
				else if (Camera.current)
				{
				}
				float num5 = this.logoPixelOffset.x + this.logoPercentOffset.x * num3 * 0.01f;
				float num6 = this.logoPixelOffset.y + this.logoPercentOffset.y * num4 * 0.01f;
				switch (this.placement)
				{
				case Corner.TopLeft:
					this.texRect.x = num5;
					this.texRect.y = num6;
					break;
				case Corner.TopRight:
					this.texRect.x = num3 - num5 - num;
					this.texRect.y = num6;
					break;
				case Corner.BottomLeft:
					this.texRect.x = num5;
					this.texRect.y = num4 - num6 - num2;
					break;
				case Corner.BottomRight:
					this.texRect.x = num3 - num5 - num;
					this.texRect.y = num4 - num6 - num2;
					break;
				}
				this.texRect.width = num;
				this.texRect.height = num2;
			}
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0006E707 File Offset: 0x0006CB07
		private void OnGUI()
		{
			this.updateTexRect();
			if (this.logoTexture)
			{
				GUI.color = this.color;
				GUI.DrawTexture(this.texRect, this.logoTexture);
			}
		}

		// Token: 0x040010C1 RID: 4289
		public Texture2D logoTexture;

		// Token: 0x040010C2 RID: 4290
		public Color color = Color.white;

		// Token: 0x040010C3 RID: 4291
		public Vector2 logoPixelOffset = new Vector2(0f, 0f);

		// Token: 0x040010C4 RID: 4292
		public Vector2 logoPercentOffset = new Vector2(0f, 0f);

		// Token: 0x040010C5 RID: 4293
		public Corner placement = Corner.BottomLeft;

		// Token: 0x040010C6 RID: 4294
		private Rect texRect = new Rect(0f, 0f, 0f, 0f);
	}
}
