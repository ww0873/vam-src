using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004FA RID: 1274
	[AddComponentMenu("UI/Effects/Extensions/Nicer Outline")]
	public class NicerOutline : BaseMeshEffect
	{
		// Token: 0x0600202B RID: 8235 RVA: 0x000B80A0 File Offset: 0x000B64A0
		public NicerOutline()
		{
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x000B80EE File Offset: 0x000B64EE
		// (set) Token: 0x0600202D RID: 8237 RVA: 0x000B80F6 File Offset: 0x000B64F6
		public Color effectColor
		{
			get
			{
				return this.m_EffectColor;
			}
			set
			{
				this.m_EffectColor = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x000B811B File Offset: 0x000B651B
		// (set) Token: 0x0600202F RID: 8239 RVA: 0x000B8124 File Offset: 0x000B6524
		public Vector2 effectDistance
		{
			get
			{
				return this.m_EffectDistance;
			}
			set
			{
				if (value.x > 600f)
				{
					value.x = 600f;
				}
				if (value.x < -600f)
				{
					value.x = -600f;
				}
				if (value.y > 600f)
				{
					value.y = 600f;
				}
				if (value.y < -600f)
				{
					value.y = -600f;
				}
				if (this.m_EffectDistance == value)
				{
					return;
				}
				this.m_EffectDistance = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x000B81DA File Offset: 0x000B65DA
		// (set) Token: 0x06002031 RID: 8241 RVA: 0x000B81E2 File Offset: 0x000B65E2
		public bool useGraphicAlpha
		{
			get
			{
				return this.m_UseGraphicAlpha;
			}
			set
			{
				this.m_UseGraphicAlpha = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x000B8208 File Offset: 0x000B6608
		protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			for (int i = start; i < end; i++)
			{
				UIVertex uivertex = verts[i];
				verts.Add(uivertex);
				Vector3 position = uivertex.position;
				position.x += x;
				position.y += y;
				uivertex.position = position;
				Color32 color2 = color;
				if (this.m_UseGraphicAlpha)
				{
					color2.a = color2.a * verts[i].color.a / byte.MaxValue;
				}
				uivertex.color = color2;
				verts[i] = uivertex;
			}
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000B82D0 File Offset: 0x000B66D0
		protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			this.ApplyShadowZeroAlloc(verts, color, start, end, x, y);
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000B8308 File Offset: 0x000B6708
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			Text component = base.GetComponent<Text>();
			float num = 1f;
			if (component && component.resizeTextForBestFit)
			{
				num = (float)component.cachedTextGenerator.fontSizeUsedForBestFit / (float)(component.resizeTextMaxSize - 1);
			}
			float num2 = this.effectDistance.x * num;
			float num3 = this.effectDistance.y * num;
			int start = 0;
			int count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, -num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, -num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, 0f);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, 0f);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, 0f, num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, 0f, -num3);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x04001B05 RID: 6917
		[SerializeField]
		private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04001B06 RID: 6918
		[SerializeField]
		private Vector2 m_EffectDistance = new Vector2(1f, -1f);

		// Token: 0x04001B07 RID: 6919
		[SerializeField]
		private bool m_UseGraphicAlpha = true;
	}
}
