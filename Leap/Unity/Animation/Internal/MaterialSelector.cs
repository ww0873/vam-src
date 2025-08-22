using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x0200064A RID: 1610
	public struct MaterialSelector
	{
		// Token: 0x0600276B RID: 10091 RVA: 0x000DBB2B File Offset: 0x000D9F2B
		public MaterialSelector(Material target, Tween tween)
		{
			this._target = target;
			this._tween = tween;
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000DBB3B File Offset: 0x000D9F3B
		public Tween Color(Color a, Color b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialColorInterpolator>.Spawn().Init(a, b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000DBB60 File Offset: 0x000D9F60
		public Tween Color(Color a, Color b, string propertyName = "_Color")
		{
			return this.Color(a, b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000DBB70 File Offset: 0x000D9F70
		public Tween Color(Gradient gradient, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialGradientInterpolator>.Spawn().Init(gradient, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000DBB94 File Offset: 0x000D9F94
		public Tween Color(Gradient gradient, string propertyName = "_Color")
		{
			return this.Color(gradient, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000DBBA3 File Offset: 0x000D9FA3
		public Tween ToColor(Color b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialColorInterpolator>.Spawn().Init(this._target.GetColor(propertyId), b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000DBBD3 File Offset: 0x000D9FD3
		public Tween ToColor(Color b, string propertyName = "_Color")
		{
			return this.ToColor(b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000DBBE2 File Offset: 0x000D9FE2
		public Tween RGB(Color a, Color b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialRGBInterpolator>.Spawn().Init(a, b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000DBC1B File Offset: 0x000DA01B
		public Tween RGB(Color a, Color b, string propertyName = "_Color")
		{
			return this.RGB(a, b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000DBC2C File Offset: 0x000DA02C
		public Tween ToRGB(Color b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialRGBInterpolator>.Spawn().Init(this._target.GetColor(propertyId), b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000DBC7B File Offset: 0x000DA07B
		public Tween ToRGB(Color b, string propertyName = "_Color")
		{
			return this.ToRGB(b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000DBC8A File Offset: 0x000DA08A
		public Tween Alpha(float a, float b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialAlphaInterpolator>.Spawn().Init(a, b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000DBCAF File Offset: 0x000DA0AF
		public Tween Alpha(float a, float b, string propertyName = "_Color")
		{
			return this.Alpha(a, b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000DBCC0 File Offset: 0x000DA0C0
		public Tween ToAlpha(float b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialAlphaInterpolator>.Spawn().Init(this._target.GetColor(propertyId).a, b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000DBD03 File Offset: 0x000DA103
		public Tween ToAlpha(float b, string propertyName = "_Color")
		{
			return this.ToAlpha(b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000DBD12 File Offset: 0x000DA112
		public Tween Float(float a, float b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialFloatInterpolator>.Spawn().Init(a, b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000DBD37 File Offset: 0x000DA137
		public Tween Float(float a, float b, string propertyName)
		{
			return this.Float(a, b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000DBD47 File Offset: 0x000DA147
		public Tween ToFloat(float b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialFloatInterpolator>.Spawn().Init(this._target.GetFloat(propertyId), b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000DBD77 File Offset: 0x000DA177
		public Tween ToFloat(float b, string propertyName)
		{
			return this.ToFloat(b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000DBD86 File Offset: 0x000DA186
		public Tween Vector(Vector4 a, Vector4 b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialVectorInterpolator>.Spawn().Init(a, b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000DBDAB File Offset: 0x000DA1AB
		public Tween Vector(Vector4 a, Vector4 b, string propertyName)
		{
			return this.Vector(a, b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000DBDBB File Offset: 0x000DA1BB
		public Tween Vector(Vector3 a, Vector3 b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialVectorInterpolator>.Spawn().Init(a, b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000DBDEA File Offset: 0x000DA1EA
		public Tween Vector(Vector3 a, Vector3 b, string propertyName)
		{
			return this.Vector(a, b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000DBDFA File Offset: 0x000DA1FA
		public Tween ToVector(Vector4 b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialVectorInterpolator>.Spawn().Init(this._target.GetVector(propertyId), b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000DBE2A File Offset: 0x000DA22A
		public Tween ToVector(Vector4 b, string propertyName)
		{
			return this.ToVector(b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000DBE39 File Offset: 0x000DA239
		public Tween ToVector(Vector3 b, int propertyId)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialVectorInterpolator>.Spawn().Init(this._target.GetVector(propertyId), b, new MaterialSelector.MaterialPropertyKey(this._target, propertyId)));
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000DBE6E File Offset: 0x000DA26E
		public Tween ToVector(Vector3 b, string propertyName)
		{
			return this.ToVector(b, Shader.PropertyToID(propertyName));
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000DBE7D File Offset: 0x000DA27D
		public Tween Material(Material a, Material b)
		{
			return this._tween.AddInterpolator(Pool<MaterialSelector.MaterialInterpolator>.Spawn().Init(a, b, this._target));
		}

		// Token: 0x04002136 RID: 8502
		private Material _target;

		// Token: 0x04002137 RID: 8503
		private Tween _tween;

		// Token: 0x0200064B RID: 1611
		private class MaterialColorInterpolator : ColorInterpolatorBase<MaterialSelector.MaterialPropertyKey>
		{
			// Token: 0x06002787 RID: 10119 RVA: 0x000DBE9C File Offset: 0x000DA29C
			public MaterialColorInterpolator()
			{
			}

			// Token: 0x06002788 RID: 10120 RVA: 0x000DBEA4 File Offset: 0x000DA2A4
			public override void Interpolate(float percent)
			{
				this._target.material.SetColor(this._target.propertyId, this._a + this._b * percent);
			}

			// Token: 0x06002789 RID: 10121 RVA: 0x000DBED8 File Offset: 0x000DA2D8
			public override void Dispose()
			{
				this._target.material = null;
				Pool<MaterialSelector.MaterialColorInterpolator>.Recycle(this);
			}

			// Token: 0x170004D9 RID: 1241
			// (get) Token: 0x0600278A RID: 10122 RVA: 0x000DBEEC File Offset: 0x000DA2EC
			public override bool isValid
			{
				get
				{
					return this._target.material != null;
				}
			}
		}

		// Token: 0x0200064C RID: 1612
		private class MaterialGradientInterpolator : GradientInterpolatorBase
		{
			// Token: 0x0600278B RID: 10123 RVA: 0x000DBEFF File Offset: 0x000DA2FF
			public MaterialGradientInterpolator()
			{
			}

			// Token: 0x0600278C RID: 10124 RVA: 0x000DBF07 File Offset: 0x000DA307
			public MaterialSelector.MaterialGradientInterpolator Init(Gradient gradient, MaterialSelector.MaterialPropertyKey matPropKey)
			{
				this._matPropKey = matPropKey;
				base.Init(gradient);
				return this;
			}

			// Token: 0x0600278D RID: 10125 RVA: 0x000DBF19 File Offset: 0x000DA319
			public override void Interpolate(float percent)
			{
				this._matPropKey.material.SetColor(this._matPropKey.propertyId, this._gradient.Evaluate(percent));
			}

			// Token: 0x170004DA RID: 1242
			// (get) Token: 0x0600278E RID: 10126 RVA: 0x000DBF42 File Offset: 0x000DA342
			public override bool isValid
			{
				get
				{
					return this._matPropKey.material != null;
				}
			}

			// Token: 0x04002138 RID: 8504
			private MaterialSelector.MaterialPropertyKey _matPropKey;
		}

		// Token: 0x0200064D RID: 1613
		private class MaterialRGBInterpolator : Vector3InterpolatorBase<MaterialSelector.MaterialPropertyKey>
		{
			// Token: 0x0600278F RID: 10127 RVA: 0x000DBF55 File Offset: 0x000DA355
			public MaterialRGBInterpolator()
			{
			}

			// Token: 0x06002790 RID: 10128 RVA: 0x000DBF60 File Offset: 0x000DA360
			public override void Interpolate(float percent)
			{
				float a = this._target.material.GetColor(this._target.propertyId).a;
				Color value = this._a + this._b * percent;
				value.a = a;
				this._target.material.SetColor(this._target.propertyId, value);
			}

			// Token: 0x06002791 RID: 10129 RVA: 0x000DBFD7 File Offset: 0x000DA3D7
			public override void Dispose()
			{
				this._target.material = null;
				Pool<MaterialSelector.MaterialRGBInterpolator>.Recycle(this);
			}

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x06002792 RID: 10130 RVA: 0x000DBFEB File Offset: 0x000DA3EB
			public override bool isValid
			{
				get
				{
					return this._target.material != null;
				}
			}
		}

		// Token: 0x0200064E RID: 1614
		private class MaterialAlphaInterpolator : FloatInterpolatorBase<MaterialSelector.MaterialPropertyKey>
		{
			// Token: 0x06002793 RID: 10131 RVA: 0x000DBFFE File Offset: 0x000DA3FE
			public MaterialAlphaInterpolator()
			{
			}

			// Token: 0x06002794 RID: 10132 RVA: 0x000DC008 File Offset: 0x000DA408
			public override void Interpolate(float percent)
			{
				Color color = this._target.material.GetColor(this._target.propertyId);
				color.a = Mathf.Lerp(this._a, this._b, percent);
				this._target.material.SetColor(this._target.propertyId, color);
			}

			// Token: 0x06002795 RID: 10133 RVA: 0x000DC066 File Offset: 0x000DA466
			public override void Dispose()
			{
				this._target.material = null;
				Pool<MaterialSelector.MaterialAlphaInterpolator>.Recycle(this);
			}

			// Token: 0x170004DC RID: 1244
			// (get) Token: 0x06002796 RID: 10134 RVA: 0x000DC07A File Offset: 0x000DA47A
			public override bool isValid
			{
				get
				{
					return this._target.material != null;
				}
			}
		}

		// Token: 0x0200064F RID: 1615
		private class MaterialFloatInterpolator : FloatInterpolatorBase<MaterialSelector.MaterialPropertyKey>
		{
			// Token: 0x06002797 RID: 10135 RVA: 0x000DC08D File Offset: 0x000DA48D
			public MaterialFloatInterpolator()
			{
			}

			// Token: 0x06002798 RID: 10136 RVA: 0x000DC095 File Offset: 0x000DA495
			public override void Interpolate(float percent)
			{
				this._target.material.SetFloat(this._target.propertyId, this._a + this._b * percent);
			}

			// Token: 0x06002799 RID: 10137 RVA: 0x000DC0C1 File Offset: 0x000DA4C1
			public override void Dispose()
			{
				this._target.material = null;
				Pool<MaterialSelector.MaterialFloatInterpolator>.Recycle(this);
			}

			// Token: 0x170004DD RID: 1245
			// (get) Token: 0x0600279A RID: 10138 RVA: 0x000DC0D5 File Offset: 0x000DA4D5
			public override bool isValid
			{
				get
				{
					return this._target.material != null;
				}
			}
		}

		// Token: 0x02000650 RID: 1616
		private class MaterialVectorInterpolator : Vector4InterpolatorBase<MaterialSelector.MaterialPropertyKey>
		{
			// Token: 0x0600279B RID: 10139 RVA: 0x000DC0E8 File Offset: 0x000DA4E8
			public MaterialVectorInterpolator()
			{
			}

			// Token: 0x0600279C RID: 10140 RVA: 0x000DC0F0 File Offset: 0x000DA4F0
			public override void Interpolate(float percent)
			{
				this._target.material.SetVector(this._target.propertyId, this._a + this._b * percent);
			}

			// Token: 0x0600279D RID: 10141 RVA: 0x000DC124 File Offset: 0x000DA524
			public override void Dispose()
			{
				this._target.material = null;
				Pool<MaterialSelector.MaterialVectorInterpolator>.Recycle(this);
			}

			// Token: 0x170004DE RID: 1246
			// (get) Token: 0x0600279E RID: 10142 RVA: 0x000DC138 File Offset: 0x000DA538
			public override bool isValid
			{
				get
				{
					return this._target.material != null;
				}
			}
		}

		// Token: 0x02000651 RID: 1617
		private class MaterialInterpolator : InterpolatorBase<Material, Material>
		{
			// Token: 0x0600279F RID: 10143 RVA: 0x000DC14B File Offset: 0x000DA54B
			public MaterialInterpolator()
			{
			}

			// Token: 0x170004DF RID: 1247
			// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000DC153 File Offset: 0x000DA553
			public override float length
			{
				get
				{
					return 1f;
				}
			}

			// Token: 0x060027A1 RID: 10145 RVA: 0x000DC15A File Offset: 0x000DA55A
			public override void Interpolate(float percent)
			{
				this._target.Lerp(this._a, this._b, percent);
			}

			// Token: 0x060027A2 RID: 10146 RVA: 0x000DC174 File Offset: 0x000DA574
			public override void Dispose()
			{
				this._target = null;
				this._a = null;
				this._b = null;
				Pool<MaterialSelector.MaterialInterpolator>.Recycle(this);
			}

			// Token: 0x170004E0 RID: 1248
			// (get) Token: 0x060027A3 RID: 10147 RVA: 0x000DC191 File Offset: 0x000DA591
			public override bool isValid
			{
				get
				{
					return this._target != null;
				}
			}
		}

		// Token: 0x02000652 RID: 1618
		private struct MaterialPropertyKey
		{
			// Token: 0x060027A4 RID: 10148 RVA: 0x000DC19F File Offset: 0x000DA59F
			public MaterialPropertyKey(Material material, int propertyId)
			{
				this.material = material;
				this.propertyId = propertyId;
			}

			// Token: 0x04002139 RID: 8505
			public Material material;

			// Token: 0x0400213A RID: 8506
			public int propertyId;
		}
	}
}
