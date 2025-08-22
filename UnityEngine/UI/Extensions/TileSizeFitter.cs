using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000523 RID: 1315
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("Layout/Extensions/Tile Size Fitter")]
	public class TileSizeFitter : UIBehaviour, ILayoutSelfController, ILayoutController
	{
		// Token: 0x0600213A RID: 8506 RVA: 0x000BE0EC File Offset: 0x000BC4EC
		public TileSizeFitter()
		{
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x000BE10A File Offset: 0x000BC50A
		// (set) Token: 0x0600213C RID: 8508 RVA: 0x000BE112 File Offset: 0x000BC512
		public Vector2 Border
		{
			get
			{
				return this.m_Border;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Vector2>(ref this.m_Border, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x000BE12B File Offset: 0x000BC52B
		// (set) Token: 0x0600213E RID: 8510 RVA: 0x000BE133 File Offset: 0x000BC533
		public Vector2 TileSize
		{
			get
			{
				return this.m_TileSize;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Vector2>(ref this.m_TileSize, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x000BE14C File Offset: 0x000BC54C
		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000BE171 File Offset: 0x000BC571
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000BE17F File Offset: 0x000BC57F
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000BE19D File Offset: 0x000BC59D
		protected override void OnRectTransformDimensionsChange()
		{
			this.UpdateRect();
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000BE1A8 File Offset: 0x000BC5A8
		private void UpdateRect()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_Tracker.Clear();
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY);
			this.rectTransform.anchorMin = Vector2.zero;
			this.rectTransform.anchorMax = Vector2.one;
			this.rectTransform.anchoredPosition = Vector2.zero;
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDelta);
			Vector2 a = this.GetParentSize() - this.Border;
			if (this.TileSize.x > 0.001f)
			{
				a.x -= Mathf.Floor(a.x / this.TileSize.x) * this.TileSize.x;
			}
			else
			{
				a.x = 0f;
			}
			if (this.TileSize.y > 0.001f)
			{
				a.y -= Mathf.Floor(a.y / this.TileSize.y) * this.TileSize.y;
			}
			else
			{
				a.y = 0f;
			}
			this.rectTransform.sizeDelta = -a;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000BE314 File Offset: 0x000BC714
		private Vector2 GetParentSize()
		{
			RectTransform rectTransform = this.rectTransform.parent as RectTransform;
			if (!rectTransform)
			{
				return Vector2.zero;
			}
			return rectTransform.rect.size;
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000BE351 File Offset: 0x000BC751
		public virtual void SetLayoutHorizontal()
		{
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000BE353 File Offset: 0x000BC753
		public virtual void SetLayoutVertical()
		{
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000BE355 File Offset: 0x000BC755
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateRect();
		}

		// Token: 0x04001BD2 RID: 7122
		[SerializeField]
		private Vector2 m_Border = Vector2.zero;

		// Token: 0x04001BD3 RID: 7123
		[SerializeField]
		private Vector2 m_TileSize = Vector2.zero;

		// Token: 0x04001BD4 RID: 7124
		[NonSerialized]
		private RectTransform m_Rect;

		// Token: 0x04001BD5 RID: 7125
		private DrivenRectTransformTracker m_Tracker;
	}
}
