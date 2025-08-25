using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200052A RID: 1322
	[AddComponentMenu("UI/Extensions/Primitives/Diamond Graph")]
	public class DiamondGraph : UIPrimitiveBase
	{
		// Token: 0x06002180 RID: 8576 RVA: 0x000BFBD5 File Offset: 0x000BDFD5
		public DiamondGraph()
		{
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x000BFC09 File Offset: 0x000BE009
		// (set) Token: 0x06002182 RID: 8578 RVA: 0x000BFC11 File Offset: 0x000BE011
		public float A
		{
			get
			{
				return this.m_a;
			}
			set
			{
				this.m_a = value;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x000BFC1A File Offset: 0x000BE01A
		// (set) Token: 0x06002184 RID: 8580 RVA: 0x000BFC22 File Offset: 0x000BE022
		public float B
		{
			get
			{
				return this.m_b;
			}
			set
			{
				this.m_b = value;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000BFC2B File Offset: 0x000BE02B
		// (set) Token: 0x06002186 RID: 8582 RVA: 0x000BFC33 File Offset: 0x000BE033
		public float C
		{
			get
			{
				return this.m_c;
			}
			set
			{
				this.m_c = value;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000BFC3C File Offset: 0x000BE03C
		// (set) Token: 0x06002188 RID: 8584 RVA: 0x000BFC44 File Offset: 0x000BE044
		public float D
		{
			get
			{
				return this.m_d;
			}
			set
			{
				this.m_d = value;
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x000BFC50 File Offset: 0x000BE050
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			float num = base.rectTransform.rect.width / 2f;
			this.m_a = Math.Min(1f, Math.Max(0f, this.m_a));
			this.m_b = Math.Min(1f, Math.Max(0f, this.m_b));
			this.m_c = Math.Min(1f, Math.Max(0f, this.m_c));
			this.m_d = Math.Min(1f, Math.Max(0f, this.m_d));
			Color32 color = this.color;
			vh.AddVert(new Vector3(-num * this.m_a, 0f), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(0f, num * this.m_b), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(num * this.m_c, 0f), color, new Vector2(1f, 1f));
			vh.AddVert(new Vector3(0f, -num * this.m_d), color, new Vector2(1f, 0f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		// Token: 0x04001BEA RID: 7146
		[SerializeField]
		private float m_a = 1f;

		// Token: 0x04001BEB RID: 7147
		[SerializeField]
		private float m_b = 1f;

		// Token: 0x04001BEC RID: 7148
		[SerializeField]
		private float m_c = 1f;

		// Token: 0x04001BED RID: 7149
		[SerializeField]
		private float m_d = 1f;
	}
}
