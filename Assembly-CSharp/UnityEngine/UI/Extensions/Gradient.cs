using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004F2 RID: 1266
	[AddComponentMenu("UI/Effects/Extensions/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		// Token: 0x06002009 RID: 8201 RVA: 0x000B6BB8 File Offset: 0x000B4FB8
		public Gradient()
		{
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x000B6BD6 File Offset: 0x000B4FD6
		// (set) Token: 0x0600200B RID: 8203 RVA: 0x000B6BDE File Offset: 0x000B4FDE
		public GradientMode GradientMode
		{
			get
			{
				return this._gradientMode;
			}
			set
			{
				this._gradientMode = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x000B6BF2 File Offset: 0x000B4FF2
		// (set) Token: 0x0600200D RID: 8205 RVA: 0x000B6BFA File Offset: 0x000B4FFA
		public GradientDir GradientDir
		{
			get
			{
				return this._gradientDir;
			}
			set
			{
				this._gradientDir = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x000B6C0E File Offset: 0x000B500E
		// (set) Token: 0x0600200F RID: 8207 RVA: 0x000B6C16 File Offset: 0x000B5016
		public bool OverwriteAllColor
		{
			get
			{
				return this._overwriteAllColor;
			}
			set
			{
				this._overwriteAllColor = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x000B6C2A File Offset: 0x000B502A
		// (set) Token: 0x06002011 RID: 8209 RVA: 0x000B6C32 File Offset: 0x000B5032
		public Color Vertex1
		{
			get
			{
				return this._vertex1;
			}
			set
			{
				this._vertex1 = value;
				base.graphic.SetAllDirty();
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x000B6C46 File Offset: 0x000B5046
		// (set) Token: 0x06002013 RID: 8211 RVA: 0x000B6C4E File Offset: 0x000B504E
		public Color Vertex2
		{
			get
			{
				return this._vertex2;
			}
			set
			{
				this._vertex2 = value;
				base.graphic.SetAllDirty();
			}
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x000B6C62 File Offset: 0x000B5062
		protected override void Awake()
		{
			this.targetGraphic = base.GetComponent<Graphic>();
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x000B6C70 File Offset: 0x000B5070
		public override void ModifyMesh(VertexHelper vh)
		{
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			UIVertex vertex = default(UIVertex);
			if (this._gradientMode == GradientMode.Global)
			{
				if (this._gradientDir == GradientDir.DiagonalLeftToRight || this._gradientDir == GradientDir.DiagonalRightToLeft)
				{
					this._gradientDir = GradientDir.Vertical;
				}
				float num = (this._gradientDir != GradientDir.Vertical) ? list[list.Count - 1].position.x : list[list.Count - 1].position.y;
				float num2 = (this._gradientDir != GradientDir.Vertical) ? list[0].position.x : list[0].position.y;
				float num3 = num2 - num;
				for (int i = 0; i < currentVertCount; i++)
				{
					vh.PopulateUIVertex(ref vertex, i);
					if (this._overwriteAllColor || !(vertex.color != this.targetGraphic.color))
					{
						vertex.color *= Color.Lerp(this._vertex2, this._vertex1, (((this._gradientDir != GradientDir.Vertical) ? vertex.position.x : vertex.position.y) - num) / num3);
						vh.SetUIVertex(vertex, i);
					}
				}
			}
			else
			{
				for (int j = 0; j < currentVertCount; j++)
				{
					vh.PopulateUIVertex(ref vertex, j);
					if (this._overwriteAllColor || this.CompareCarefully(vertex.color, this.targetGraphic.color))
					{
						switch (this._gradientDir)
						{
						case GradientDir.Vertical:
							vertex.color *= ((j % 4 != 0 && (j - 1) % 4 != 0) ? this._vertex2 : this._vertex1);
							break;
						case GradientDir.Horizontal:
							vertex.color *= ((j % 4 != 0 && (j - 3) % 4 != 0) ? this._vertex2 : this._vertex1);
							break;
						case GradientDir.DiagonalLeftToRight:
							vertex.color *= ((j % 4 != 0) ? (((j - 2) % 4 != 0) ? Color.Lerp(this._vertex2, this._vertex1, 0.5f) : this._vertex2) : this._vertex1);
							break;
						case GradientDir.DiagonalRightToLeft:
							vertex.color *= (((j - 1) % 4 != 0) ? (((j - 3) % 4 != 0) ? Color.Lerp(this._vertex2, this._vertex1, 0.5f) : this._vertex2) : this._vertex1);
							break;
						}
						vh.SetUIVertex(vertex, j);
					}
				}
			}
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000B6FE8 File Offset: 0x000B53E8
		private bool CompareCarefully(Color col1, Color col2)
		{
			return Mathf.Abs(col1.r - col2.r) < 0.003f && Mathf.Abs(col1.g - col2.g) < 0.003f && Mathf.Abs(col1.b - col2.b) < 0.003f && Mathf.Abs(col1.a - col2.a) < 0.003f;
		}

		// Token: 0x04001AE4 RID: 6884
		[SerializeField]
		private GradientMode _gradientMode;

		// Token: 0x04001AE5 RID: 6885
		[SerializeField]
		private GradientDir _gradientDir;

		// Token: 0x04001AE6 RID: 6886
		[SerializeField]
		private bool _overwriteAllColor;

		// Token: 0x04001AE7 RID: 6887
		[SerializeField]
		private Color _vertex1 = Color.white;

		// Token: 0x04001AE8 RID: 6888
		[SerializeField]
		private Color _vertex2 = Color.black;

		// Token: 0x04001AE9 RID: 6889
		private Graphic targetGraphic;
	}
}
