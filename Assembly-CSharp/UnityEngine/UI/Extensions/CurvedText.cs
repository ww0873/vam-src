using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004F0 RID: 1264
	[RequireComponent(typeof(Text), typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/Extensions/Curved Text")]
	public class CurvedText : BaseMeshEffect
	{
		// Token: 0x06001FFC RID: 8188 RVA: 0x000B6912 File Offset: 0x000B4D12
		public CurvedText()
		{
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001FFD RID: 8189 RVA: 0x000B6944 File Offset: 0x000B4D44
		// (set) Token: 0x06001FFE RID: 8190 RVA: 0x000B694C File Offset: 0x000B4D4C
		public AnimationCurve CurveForText
		{
			get
			{
				return this._curveForText;
			}
			set
			{
				this._curveForText = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x000B6960 File Offset: 0x000B4D60
		// (set) Token: 0x06002000 RID: 8192 RVA: 0x000B6968 File Offset: 0x000B4D68
		public float CurveMultiplier
		{
			get
			{
				return this._curveMultiplier;
			}
			set
			{
				this._curveMultiplier = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000B697C File Offset: 0x000B4D7C
		protected override void Awake()
		{
			base.Awake();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x000B6996 File Offset: 0x000B4D96
		protected override void OnEnable()
		{
			base.OnEnable();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x000B69B0 File Offset: 0x000B4DB0
		public override void ModifyMesh(VertexHelper vh)
		{
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				UIVertex vertex = default(UIVertex);
				vh.PopulateUIVertex(ref vertex, i);
				vertex.position.y = vertex.position.y + this._curveForText.Evaluate(this.rectTrans.rect.width * this.rectTrans.pivot.x + vertex.position.x) * this._curveMultiplier;
				vh.SetUIVertex(vertex, i);
			}
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x000B6A60 File Offset: 0x000B4E60
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.rectTrans)
			{
				Keyframe key = this._curveForText[this._curveForText.length - 1];
				key.time = this.rectTrans.rect.width;
				this._curveForText.MoveKey(this._curveForText.length - 1, key);
			}
		}

		// Token: 0x04001ADF RID: 6879
		[SerializeField]
		private AnimationCurve _curveForText = AnimationCurve.Linear(0f, 0f, 1f, 10f);

		// Token: 0x04001AE0 RID: 6880
		[SerializeField]
		private float _curveMultiplier = 1f;

		// Token: 0x04001AE1 RID: 6881
		private RectTransform rectTrans;
	}
}
