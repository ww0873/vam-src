using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200053A RID: 1338
	[AddComponentMenu("UI/Extensions/HoverTooltip")]
	public class HoverTooltip : MonoBehaviour
	{
		// Token: 0x0600220A RID: 8714 RVA: 0x000C2977 File Offset: 0x000C0D77
		public HoverTooltip()
		{
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000C2980 File Offset: 0x000C0D80
		private void Start()
		{
			this.GUICamera = GameObject.Find("GUICamera").GetComponent<Camera>();
			this.GUIMode = base.transform.parent.parent.GetComponent<Canvas>().renderMode;
			this.bgImageSource = this.bgImage.GetComponent<Image>();
			this.inside = false;
			this.HideTooltipVisibility();
			base.transform.parent.gameObject.SetActive(false);
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000C29F6 File Offset: 0x000C0DF6
		public void SetTooltip(string text)
		{
			this.NewTooltip();
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000C2A10 File Offset: 0x000C0E10
		public void SetTooltip(string[] texts)
		{
			this.NewTooltip();
			string text = string.Empty;
			int num = 0;
			foreach (string text2 in texts)
			{
				if (num == 0)
				{
					text += text2;
				}
				else
				{
					text = text + "\n" + text2;
				}
				num++;
			}
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000C2A80 File Offset: 0x000C0E80
		public void SetTooltip(string text, bool test)
		{
			this.NewTooltip();
			this.thisText.text = text;
			this.OnScreenSpaceCamera();
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000C2A9C File Offset: 0x000C0E9C
		public void OnScreenSpaceCamera()
		{
			Vector3 position = this.GUICamera.ScreenToViewportPoint(Input.mousePosition);
			float num = this.GUICamera.ViewportToScreenPoint(position).x + this.tooltipRealWidth * this.bgImage.pivot.x;
			if (num > this.upperRight.x)
			{
				float num2 = this.upperRight.x - num;
				float num3;
				if ((double)num2 > (double)this.defaultXOffset * 0.75)
				{
					num3 = num2;
				}
				else
				{
					num3 = this.defaultXOffset - this.tooltipRealWidth * 2f;
				}
				Vector3 position2 = new Vector3(this.GUICamera.ViewportToScreenPoint(position).x + num3, 0f, 0f);
				position.x = this.GUICamera.ScreenToViewportPoint(position2).x;
			}
			num = this.GUICamera.ViewportToScreenPoint(position).x - this.tooltipRealWidth * this.bgImage.pivot.x;
			if (num < this.lowerLeft.x)
			{
				float num4 = this.lowerLeft.x - num;
				float num3;
				if ((double)num4 < (double)this.defaultXOffset * 0.75 - (double)this.tooltipRealWidth)
				{
					num3 = -num4;
				}
				else
				{
					num3 = this.tooltipRealWidth * 2f;
				}
				Vector3 position3 = new Vector3(this.GUICamera.ViewportToScreenPoint(position).x - num3, 0f, 0f);
				position.x = this.GUICamera.ScreenToViewportPoint(position3).x;
			}
			num = this.GUICamera.ViewportToScreenPoint(position).y - (this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y - this.tooltipRealHeight);
			if (num > this.upperRight.y)
			{
				float num5 = this.upperRight.y - num;
				float num6 = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				if ((double)num5 > (double)this.defaultYOffset * 0.75)
				{
					num6 = num5;
				}
				else
				{
					num6 = this.defaultYOffset - this.tooltipRealHeight * 2f;
				}
				Vector3 position4 = new Vector3(position.x, this.GUICamera.ViewportToScreenPoint(position).y + num6, 0f);
				position.y = this.GUICamera.ScreenToViewportPoint(position4).y;
			}
			num = this.GUICamera.ViewportToScreenPoint(position).y - this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
			if (num < this.lowerLeft.y)
			{
				float num7 = this.lowerLeft.y - num;
				float num6 = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				if ((double)num7 < (double)this.defaultYOffset * 0.75 - (double)this.tooltipRealHeight)
				{
					num6 = num7;
				}
				else
				{
					num6 = this.tooltipRealHeight * 2f;
				}
				Vector3 position5 = new Vector3(position.x, this.GUICamera.ViewportToScreenPoint(position).y + num6, 0f);
				position.y = this.GUICamera.ScreenToViewportPoint(position5).y;
			}
			base.transform.parent.transform.position = new Vector3(this.GUICamera.ViewportToWorldPoint(position).x, this.GUICamera.ViewportToWorldPoint(position).y, 0f);
			base.transform.parent.gameObject.SetActive(true);
			this.inside = true;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000C2EF6 File Offset: 0x000C12F6
		public void HideTooltip()
		{
			if (this.GUIMode == RenderMode.ScreenSpaceCamera && this != null)
			{
				base.transform.parent.gameObject.SetActive(false);
				this.inside = false;
				this.HideTooltipVisibility();
			}
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000C2F33 File Offset: 0x000C1333
		private void Update()
		{
			this.LayoutInit();
			if (this.inside && this.GUIMode == RenderMode.ScreenSpaceCamera)
			{
				this.OnScreenSpaceCamera();
			}
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000C2F58 File Offset: 0x000C1358
		private void LayoutInit()
		{
			if (this.firstUpdate)
			{
				this.firstUpdate = false;
				this.bgImage.sizeDelta = new Vector2(this.hlG.preferredWidth + (float)this.horizontalPadding, this.hlG.preferredHeight + (float)this.verticalPadding);
				this.defaultYOffset = this.bgImage.sizeDelta.y * this.currentYScaleFactor * this.bgImage.pivot.y;
				this.defaultXOffset = this.bgImage.sizeDelta.x * this.currentXScaleFactor * this.bgImage.pivot.x;
				this.tooltipRealHeight = this.bgImage.sizeDelta.y * this.currentYScaleFactor;
				this.tooltipRealWidth = this.bgImage.sizeDelta.x * this.currentXScaleFactor;
				this.ActivateTooltipVisibility();
			}
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000C3060 File Offset: 0x000C1460
		private void NewTooltip()
		{
			this.firstUpdate = true;
			this.lowerLeft = this.GUICamera.ViewportToScreenPoint(new Vector3(0f, 0f, 0f));
			this.upperRight = this.GUICamera.ViewportToScreenPoint(new Vector3(1f, 1f, 0f));
			this.currentYScaleFactor = (float)Screen.height / base.transform.root.GetComponent<CanvasScaler>().referenceResolution.y;
			this.currentXScaleFactor = (float)Screen.width / base.transform.root.GetComponent<CanvasScaler>().referenceResolution.x;
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000C3114 File Offset: 0x000C1514
		public void ActivateTooltipVisibility()
		{
			Color color = this.thisText.color;
			this.thisText.color = new Color(color.r, color.g, color.b, 1f);
			this.bgImageSource.color = new Color(this.bgImageSource.color.r, this.bgImageSource.color.g, this.bgImageSource.color.b, 0.8f);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000C31A8 File Offset: 0x000C15A8
		public void HideTooltipVisibility()
		{
			Color color = this.thisText.color;
			this.thisText.color = new Color(color.r, color.g, color.b, 0f);
			this.bgImageSource.color = new Color(this.bgImageSource.color.r, this.bgImageSource.color.g, this.bgImageSource.color.b, 0f);
		}

		// Token: 0x04001C4A RID: 7242
		public int horizontalPadding;

		// Token: 0x04001C4B RID: 7243
		public int verticalPadding;

		// Token: 0x04001C4C RID: 7244
		public Text thisText;

		// Token: 0x04001C4D RID: 7245
		public HorizontalLayoutGroup hlG;

		// Token: 0x04001C4E RID: 7246
		public RectTransform bgImage;

		// Token: 0x04001C4F RID: 7247
		private Image bgImageSource;

		// Token: 0x04001C50 RID: 7248
		private bool firstUpdate;

		// Token: 0x04001C51 RID: 7249
		private bool inside;

		// Token: 0x04001C52 RID: 7250
		private RenderMode GUIMode;

		// Token: 0x04001C53 RID: 7251
		private Camera GUICamera;

		// Token: 0x04001C54 RID: 7252
		private Vector3 lowerLeft;

		// Token: 0x04001C55 RID: 7253
		private Vector3 upperRight;

		// Token: 0x04001C56 RID: 7254
		private float currentYScaleFactor;

		// Token: 0x04001C57 RID: 7255
		private float currentXScaleFactor;

		// Token: 0x04001C58 RID: 7256
		private float defaultYOffset;

		// Token: 0x04001C59 RID: 7257
		private float defaultXOffset;

		// Token: 0x04001C5A RID: 7258
		private float tooltipRealHeight;

		// Token: 0x04001C5B RID: 7259
		private float tooltipRealWidth;
	}
}
