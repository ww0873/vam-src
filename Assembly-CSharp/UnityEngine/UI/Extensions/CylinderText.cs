using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004F1 RID: 1265
	[RequireComponent(typeof(Text), typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/Extensions/Cylinder Text")]
	public class CylinderText : BaseMeshEffect
	{
		// Token: 0x06002005 RID: 8197 RVA: 0x000B6ACA File Offset: 0x000B4ECA
		public CylinderText()
		{
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000B6AD2 File Offset: 0x000B4ED2
		protected override void Awake()
		{
			base.Awake();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000B6AEC File Offset: 0x000B4EEC
		protected override void OnEnable()
		{
			base.OnEnable();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000B6B08 File Offset: 0x000B4F08
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				UIVertex vertex = default(UIVertex);
				vh.PopulateUIVertex(ref vertex, i);
				float x = vertex.position.x;
				vertex.position.z = -this.radius * Mathf.Cos(x / this.radius);
				vertex.position.x = this.radius * Mathf.Sin(x / this.radius);
				vh.SetUIVertex(vertex, i);
			}
		}

		// Token: 0x04001AE2 RID: 6882
		public float radius;

		// Token: 0x04001AE3 RID: 6883
		private RectTransform rectTrans;
	}
}
