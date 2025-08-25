using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004ED RID: 1261
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Image")]
	public class CUIImage : CUIGraphic
	{
		// Token: 0x06001FF1 RID: 8177 RVA: 0x000B6322 File Offset: 0x000B4722
		public CUIImage()
		{
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000B6354 File Offset: 0x000B4754
		public static int ImageTypeCornerRefVertexIdx(Image.Type _type)
		{
			if (_type == Image.Type.Sliced)
			{
				return CUIImage.SlicedImageCornerRefVertexIdx;
			}
			return CUIImage.FilledImageCornerRefVertexIdx;
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x000B6368 File Offset: 0x000B4768
		public Vector2 OriCornerPosRatio
		{
			get
			{
				return this.oriCornerPosRatio;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x000B6370 File Offset: 0x000B4770
		public Image UIImage
		{
			get
			{
				return (Image)this.uiGraphic;
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000B637D File Offset: 0x000B477D
		public override void ReportSet()
		{
			if (this.uiGraphic == null)
			{
				this.uiGraphic = base.GetComponent<Image>();
			}
			base.ReportSet();
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000B63A4 File Offset: 0x000B47A4
		protected override void modifyVertices(List<UIVertex> _verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.UIImage.type == Image.Type.Filled)
			{
				Debug.LogWarning("Might not work well Radial Filled at the moment!");
			}
			else if (this.UIImage.type == Image.Type.Sliced || this.UIImage.type == Image.Type.Tiled)
			{
				if (this.cornerPosRatio == Vector2.one * -1f)
				{
					this.cornerPosRatio = _verts[CUIImage.ImageTypeCornerRefVertexIdx(this.UIImage.type)].position;
					this.cornerPosRatio.x = (this.cornerPosRatio.x + this.rectTrans.pivot.x * this.rectTrans.rect.width) / this.rectTrans.rect.width;
					this.cornerPosRatio.y = (this.cornerPosRatio.y + this.rectTrans.pivot.y * this.rectTrans.rect.height) / this.rectTrans.rect.height;
					this.oriCornerPosRatio = this.cornerPosRatio;
				}
				if (this.cornerPosRatio.x < 0f)
				{
					this.cornerPosRatio.x = 0f;
				}
				if (this.cornerPosRatio.x >= 0.5f)
				{
					this.cornerPosRatio.x = 0.5f;
				}
				if (this.cornerPosRatio.y < 0f)
				{
					this.cornerPosRatio.y = 0f;
				}
				if (this.cornerPosRatio.y >= 0.5f)
				{
					this.cornerPosRatio.y = 0.5f;
				}
				for (int i = 0; i < _verts.Count; i++)
				{
					UIVertex value = _verts[i];
					float num = (value.position.x + this.rectTrans.rect.width * this.rectTrans.pivot.x) / this.rectTrans.rect.width;
					float num2 = (value.position.y + this.rectTrans.rect.height * this.rectTrans.pivot.y) / this.rectTrans.rect.height;
					if (num < this.oriCornerPosRatio.x)
					{
						num = Mathf.Lerp(0f, this.cornerPosRatio.x, num / this.oriCornerPosRatio.x);
					}
					else if (num > 1f - this.oriCornerPosRatio.x)
					{
						num = Mathf.Lerp(1f - this.cornerPosRatio.x, 1f, (num - (1f - this.oriCornerPosRatio.x)) / this.oriCornerPosRatio.x);
					}
					else
					{
						num = Mathf.Lerp(this.cornerPosRatio.x, 1f - this.cornerPosRatio.x, (num - this.oriCornerPosRatio.x) / (1f - this.oriCornerPosRatio.x * 2f));
					}
					if (num2 < this.oriCornerPosRatio.y)
					{
						num2 = Mathf.Lerp(0f, this.cornerPosRatio.y, num2 / this.oriCornerPosRatio.y);
					}
					else if (num2 > 1f - this.oriCornerPosRatio.y)
					{
						num2 = Mathf.Lerp(1f - this.cornerPosRatio.y, 1f, (num2 - (1f - this.oriCornerPosRatio.y)) / this.oriCornerPosRatio.y);
					}
					else
					{
						num2 = Mathf.Lerp(this.cornerPosRatio.y, 1f - this.cornerPosRatio.y, (num2 - this.oriCornerPosRatio.y) / (1f - this.oriCornerPosRatio.y * 2f));
					}
					value.position.x = num * this.rectTrans.rect.width - this.rectTrans.rect.width * this.rectTrans.pivot.x;
					value.position.y = num2 * this.rectTrans.rect.height - this.rectTrans.rect.height * this.rectTrans.pivot.y;
					_verts[i] = value;
				}
			}
			base.modifyVertices(_verts);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000B68B6 File Offset: 0x000B4CB6
		// Note: this type is marked as 'beforefieldinit'.
		static CUIImage()
		{
		}

		// Token: 0x04001ADA RID: 6874
		public static int SlicedImageCornerRefVertexIdx = 2;

		// Token: 0x04001ADB RID: 6875
		public static int FilledImageCornerRefVertexIdx;

		// Token: 0x04001ADC RID: 6876
		[Tooltip("For changing the size of the corner for tiled or sliced Image")]
		[HideInInspector]
		[SerializeField]
		public Vector2 cornerPosRatio = Vector2.one * -1f;

		// Token: 0x04001ADD RID: 6877
		[HideInInspector]
		[SerializeField]
		protected Vector2 oriCornerPosRatio = Vector2.one * -1f;
	}
}
