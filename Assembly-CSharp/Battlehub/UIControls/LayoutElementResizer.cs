using System;
using Battlehub.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x0200027A RID: 634
	public class LayoutElementResizer : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06000E21 RID: 3617 RVA: 0x00052B3C File Offset: 0x00050F3C
		public LayoutElementResizer()
		{
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00052B50 File Offset: 0x00050F50
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (this.Parent != null && this.SecondaryTarget != null)
			{
				if (this.XSign != 0f)
				{
					this.Target.flexibleWidth = Mathf.Clamp01(this.Target.flexibleWidth);
					this.SecondaryTarget.flexibleWidth = Mathf.Clamp01(this.SecondaryTarget.flexibleWidth);
				}
				if (this.YSign != 0f)
				{
					this.Target.flexibleHeight = Mathf.Clamp01(this.Target.flexibleHeight);
					this.SecondaryTarget.flexibleHeight = Mathf.Clamp01(this.SecondaryTarget.flexibleHeight);
				}
				this.m_midY = this.Target.flexibleHeight / (this.Target.flexibleHeight + this.SecondaryTarget.flexibleHeight);
				this.m_midY *= Math.Max(this.Parent.rect.height - this.Target.minHeight - this.SecondaryTarget.minHeight, 0f);
				this.m_midX = this.Target.flexibleWidth / (this.Target.flexibleWidth + this.SecondaryTarget.flexibleWidth);
				this.m_midX *= Math.Max(this.Parent.rect.width - this.Target.minWidth - this.SecondaryTarget.minWidth, 0f);
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00052CE4 File Offset: 0x000510E4
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (this.Parent != null && this.SecondaryTarget != null)
			{
				if (this.XSign != 0f)
				{
					float num = this.m_midX + eventData.delta.x * (float)Math.Sign(this.XSign);
					float num2 = num / (this.Parent.rect.width - this.Target.minWidth - this.SecondaryTarget.minWidth);
					this.Target.flexibleWidth = num2;
					this.SecondaryTarget.flexibleWidth = 1f - num2;
					this.m_midX = num;
				}
				if (this.YSign != 0f)
				{
					float num3 = this.m_midY + eventData.delta.y * (float)Math.Sign(this.YSign);
					float num4 = num3 / (this.Parent.rect.height - this.Target.minHeight - this.SecondaryTarget.minHeight);
					this.Target.flexibleHeight = num4;
					this.SecondaryTarget.flexibleHeight = 1f - num4;
					this.m_midY = num3;
				}
				if (this.XSign != 0f)
				{
					this.Target.flexibleWidth = Mathf.Clamp01(this.Target.flexibleWidth);
					this.SecondaryTarget.flexibleWidth = Mathf.Clamp01(this.SecondaryTarget.flexibleWidth);
				}
				if (this.YSign != 0f)
				{
					this.Target.flexibleHeight = Mathf.Clamp01(this.Target.flexibleHeight);
					this.SecondaryTarget.flexibleHeight = Mathf.Clamp01(this.SecondaryTarget.flexibleHeight);
				}
			}
			else
			{
				if (this.XSign != 0f)
				{
					this.Target.preferredWidth += eventData.delta.x * (float)Math.Sign(this.XSign);
					if (this.HasMaxSize && this.Target.preferredWidth > this.MaxSize)
					{
						this.Target.preferredWidth = this.MaxSize;
					}
				}
				if (this.YSign != 0f)
				{
					this.Target.preferredHeight += eventData.delta.y * (float)Math.Sign(this.YSign);
					if (this.HasMaxSize && this.Target.preferredHeight > this.MaxSize)
					{
						this.Target.preferredHeight = this.MaxSize;
					}
				}
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00052F98 File Offset: 0x00051398
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00052F9A File Offset: 0x0005139A
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00052F9C File Offset: 0x0005139C
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			this.m_pointerInside = true;
			CursorHelper.SetCursor(this, this.CursorTexture, new Vector2((float)(this.CursorTexture.width / 2), (float)(this.CursorTexture.height / 2)), CursorMode.Auto);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00052FD3 File Offset: 0x000513D3
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			this.m_pointerInside = false;
			if (!this.m_pointerDown)
			{
				CursorHelper.ResetCursor(this);
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00052FF0 File Offset: 0x000513F0
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			this.m_pointerDown = true;
			if (this.Target.preferredWidth < -1f)
			{
				this.Target.preferredWidth = this.Target.minWidth;
			}
			if (this.Target.preferredHeight < -1f)
			{
				this.Target.preferredHeight = this.Target.minHeight;
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0005305A File Offset: 0x0005145A
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			this.m_pointerDown = false;
			if (!this.m_pointerInside)
			{
				CursorHelper.ResetCursor(this);
			}
		}

		// Token: 0x04000D9A RID: 3482
		public LayoutElement Target;

		// Token: 0x04000D9B RID: 3483
		public RectTransform Parent;

		// Token: 0x04000D9C RID: 3484
		public LayoutElement SecondaryTarget;

		// Token: 0x04000D9D RID: 3485
		public Texture2D CursorTexture;

		// Token: 0x04000D9E RID: 3486
		public float XSign = 1f;

		// Token: 0x04000D9F RID: 3487
		public float YSign;

		// Token: 0x04000DA0 RID: 3488
		public float MaxSize;

		// Token: 0x04000DA1 RID: 3489
		public bool HasMaxSize;

		// Token: 0x04000DA2 RID: 3490
		private bool m_pointerInside;

		// Token: 0x04000DA3 RID: 3491
		private bool m_pointerDown;

		// Token: 0x04000DA4 RID: 3492
		private float m_midX;

		// Token: 0x04000DA5 RID: 3493
		private float m_midY;
	}
}
