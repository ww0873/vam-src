using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004F5 RID: 1269
	[AddComponentMenu("UI/Effects/Extensions/Gradient2")]
	public class Gradient2 : BaseMeshEffect
	{
		// Token: 0x06002017 RID: 8215 RVA: 0x000B7070 File Offset: 0x000B5470
		public Gradient2()
		{
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x000B70D9 File Offset: 0x000B54D9
		// (set) Token: 0x06002019 RID: 8217 RVA: 0x000B70E1 File Offset: 0x000B54E1
		public Gradient2.Blend BlendMode
		{
			get
			{
				return this._blendMode;
			}
			set
			{
				this._blendMode = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x000B70F5 File Offset: 0x000B54F5
		// (set) Token: 0x0600201B RID: 8219 RVA: 0x000B70FD File Offset: 0x000B54FD
		public Gradient EffectGradient
		{
			get
			{
				return this._effectGradient;
			}
			set
			{
				this._effectGradient = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x000B7111 File Offset: 0x000B5511
		// (set) Token: 0x0600201D RID: 8221 RVA: 0x000B7119 File Offset: 0x000B5519
		public Gradient2.Type GradientType
		{
			get
			{
				return this._gradientType;
			}
			set
			{
				this._gradientType = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x000B712D File Offset: 0x000B552D
		// (set) Token: 0x0600201F RID: 8223 RVA: 0x000B7135 File Offset: 0x000B5535
		public float Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x000B714C File Offset: 0x000B554C
		public override void ModifyMesh(VertexHelper helper)
		{
			if (!this.IsActive() || helper.currentVertCount == 0)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			helper.GetUIVertexStream(list);
			int count = list.Count;
			switch (this.GradientType)
			{
			case Gradient2.Type.Horizontal:
			{
				float num = list[0].position.x;
				float num2 = list[0].position.x;
				for (int i = count - 1; i >= 1; i--)
				{
					float x = list[i].position.x;
					if (x > num2)
					{
						num2 = x;
					}
					else if (x < num)
					{
						num = x;
					}
				}
				float num3 = 1f / (num2 - num);
				UIVertex vertex = default(UIVertex);
				for (int j = 0; j < helper.currentVertCount; j++)
				{
					helper.PopulateUIVertex(ref vertex, j);
					vertex.color = this.BlendColor(vertex.color, this.EffectGradient.Evaluate((vertex.position.x - num) * num3 - this.Offset));
					helper.SetUIVertex(vertex, j);
				}
				break;
			}
			case Gradient2.Type.Vertical:
			{
				float num4 = list[0].position.y;
				float num5 = list[0].position.y;
				for (int k = count - 1; k >= 1; k--)
				{
					float y = list[k].position.y;
					if (y > num5)
					{
						num5 = y;
					}
					else if (y < num4)
					{
						num4 = y;
					}
				}
				float num6 = 1f / (num5 - num4);
				UIVertex vertex2 = default(UIVertex);
				for (int l = 0; l < helper.currentVertCount; l++)
				{
					helper.PopulateUIVertex(ref vertex2, l);
					vertex2.color = this.BlendColor(vertex2.color, this.EffectGradient.Evaluate((vertex2.position.y - num4) * num6 - this.Offset));
					helper.SetUIVertex(vertex2, l);
				}
				break;
			}
			case Gradient2.Type.Radial:
			{
				float num7 = list[0].position.x;
				float num8 = list[0].position.x;
				float num9 = list[0].position.y;
				float num10 = list[0].position.y;
				for (int m = count - 1; m >= 1; m--)
				{
					float x2 = list[m].position.x;
					if (x2 > num8)
					{
						num8 = x2;
					}
					else if (x2 < num7)
					{
						num7 = x2;
					}
					float y2 = list[m].position.y;
					if (y2 > num10)
					{
						num10 = y2;
					}
					else if (y2 < num9)
					{
						num9 = y2;
					}
				}
				float num11 = 1f / (num8 - num7);
				float num12 = 1f / (num10 - num9);
				helper.Clear();
				float num13 = (num8 + num7) / 2f;
				float num14 = (num9 + num10) / 2f;
				float num15 = (num8 - num7) / 2f;
				float num16 = (num10 - num9) / 2f;
				UIVertex v = default(UIVertex);
				v.position = Vector3.right * num13 + Vector3.up * num14 + Vector3.forward * list[0].position.z;
				v.normal = list[0].normal;
				v.color = Color.white;
				int num17 = 64;
				for (int n = 0; n < num17; n++)
				{
					UIVertex v2 = default(UIVertex);
					float num18 = (float)n * 360f / (float)num17;
					float d = Mathf.Cos(0.017453292f * num18) * num15;
					float d2 = Mathf.Sin(0.017453292f * num18) * num16;
					v2.position = Vector3.right * d + Vector3.up * d2 + Vector3.forward * list[0].position.z;
					v2.normal = list[0].normal;
					v2.color = Color.white;
					helper.AddVert(v2);
				}
				helper.AddVert(v);
				for (int num19 = 1; num19 < num17; num19++)
				{
					helper.AddTriangle(num19 - 1, num19, num17);
				}
				helper.AddTriangle(0, num17 - 1, num17);
				UIVertex vertex3 = default(UIVertex);
				for (int num20 = 0; num20 < helper.currentVertCount; num20++)
				{
					helper.PopulateUIVertex(ref vertex3, num20);
					vertex3.color = this.BlendColor(vertex3.color, this.EffectGradient.Evaluate(Mathf.Sqrt(Mathf.Pow(Mathf.Abs(vertex3.position.x - num13) * num11, 2f) + Mathf.Pow(Mathf.Abs(vertex3.position.y - num14) * num12, 2f)) * 2f - this.Offset));
					helper.SetUIVertex(vertex3, num20);
				}
				break;
			}
			case Gradient2.Type.Diamond:
			{
				float num21 = list[0].position.y;
				float num22 = list[0].position.y;
				for (int num23 = count - 1; num23 >= 1; num23--)
				{
					float y3 = list[num23].position.y;
					if (y3 > num22)
					{
						num22 = y3;
					}
					else if (y3 < num21)
					{
						num21 = y3;
					}
				}
				float num24 = 1f / (num22 - num21);
				helper.Clear();
				for (int num25 = 0; num25 < count; num25++)
				{
					helper.AddVert(list[num25]);
				}
				float d3 = (num21 + num22) / 2f;
				UIVertex v3 = new UIVertex
				{
					position = (Vector3.right + Vector3.up) * d3 + Vector3.forward * list[0].position.z,
					normal = list[0].normal,
					color = Color.white
				};
				helper.AddVert(v3);
				for (int num26 = 1; num26 < count; num26++)
				{
					helper.AddTriangle(num26 - 1, num26, count);
				}
				helper.AddTriangle(0, count - 1, count);
				UIVertex vertex4 = default(UIVertex);
				for (int num27 = 0; num27 < helper.currentVertCount; num27++)
				{
					helper.PopulateUIVertex(ref vertex4, num27);
					vertex4.color = this.BlendColor(vertex4.color, this.EffectGradient.Evaluate(Vector3.Distance(vertex4.position, v3.position) * num24 - this.Offset));
					helper.SetUIVertex(vertex4, num27);
				}
				break;
			}
			}
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000B7930 File Offset: 0x000B5D30
		private Color BlendColor(Color colorA, Color colorB)
		{
			Gradient2.Blend blendMode = this.BlendMode;
			if (blendMode == Gradient2.Blend.Add)
			{
				return colorA + colorB;
			}
			if (blendMode != Gradient2.Blend.Multiply)
			{
				return colorB;
			}
			return colorA * colorB;
		}

		// Token: 0x04001AF2 RID: 6898
		[SerializeField]
		private Gradient2.Type _gradientType;

		// Token: 0x04001AF3 RID: 6899
		[SerializeField]
		private Gradient2.Blend _blendMode = Gradient2.Blend.Multiply;

		// Token: 0x04001AF4 RID: 6900
		[SerializeField]
		[Range(-1f, 1f)]
		private float _offset;

		// Token: 0x04001AF5 RID: 6901
		[SerializeField]
		private Gradient _effectGradient = new Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.black, 0f),
				new GradientColorKey(Color.white, 1f)
			}
		};

		// Token: 0x020004F6 RID: 1270
		public enum Type
		{
			// Token: 0x04001AF7 RID: 6903
			Horizontal,
			// Token: 0x04001AF8 RID: 6904
			Vertical,
			// Token: 0x04001AF9 RID: 6905
			Radial,
			// Token: 0x04001AFA RID: 6906
			Diamond
		}

		// Token: 0x020004F7 RID: 1271
		public enum Blend
		{
			// Token: 0x04001AFC RID: 6908
			Override,
			// Token: 0x04001AFD RID: 6909
			Add,
			// Token: 0x04001AFE RID: 6910
			Multiply
		}
	}
}
