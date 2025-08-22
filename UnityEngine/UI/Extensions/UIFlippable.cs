using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000505 RID: 1285
	[RequireComponent(typeof(RectTransform), typeof(Graphic))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Effects/Extensions/Flippable")]
	public class UIFlippable : BaseMeshEffect
	{
		// Token: 0x06002065 RID: 8293 RVA: 0x000B96D5 File Offset: 0x000B7AD5
		public UIFlippable()
		{
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x000B96DD File Offset: 0x000B7ADD
		// (set) Token: 0x06002067 RID: 8295 RVA: 0x000B96E5 File Offset: 0x000B7AE5
		public bool horizontal
		{
			get
			{
				return this.m_Horizontal;
			}
			set
			{
				this.m_Horizontal = value;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000B96EE File Offset: 0x000B7AEE
		// (set) Token: 0x06002069 RID: 8297 RVA: 0x000B96F6 File Offset: 0x000B7AF6
		public bool vertical
		{
			get
			{
				return this.m_Veritical;
			}
			set
			{
				this.m_Veritical = value;
			}
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x000B9700 File Offset: 0x000B7B00
		public override void ModifyMesh(VertexHelper verts)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			for (int i = 0; i < verts.currentVertCount; i++)
			{
				UIVertex vertex = default(UIVertex);
				verts.PopulateUIVertex(ref vertex, i);
				vertex.position = new Vector3((!this.m_Horizontal) ? vertex.position.x : (vertex.position.x + (rectTransform.rect.center.x - vertex.position.x) * 2f), (!this.m_Veritical) ? vertex.position.y : (vertex.position.y + (rectTransform.rect.center.y - vertex.position.y) * 2f), vertex.position.z);
				verts.SetUIVertex(vertex, i);
			}
		}

		// Token: 0x04001B29 RID: 6953
		[SerializeField]
		private bool m_Horizontal;

		// Token: 0x04001B2A RID: 6954
		[SerializeField]
		private bool m_Veritical;
	}
}
