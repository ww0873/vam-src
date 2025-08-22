using System;
using System.Linq.Expressions;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000EA RID: 234
	public class SpotlightGizmo : ConeGizmo
	{
		// Token: 0x060004E1 RID: 1249 RVA: 0x0001B135 File Offset: 0x00019535
		public SpotlightGizmo()
		{
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0001B13D File Offset: 0x0001953D
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x0001B14A File Offset: 0x0001954A
		protected override float Height
		{
			get
			{
				return this.m_light.range;
			}
			set
			{
				if (this.m_light != null)
				{
					this.m_light.range = value;
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0001B16C File Offset: 0x0001956C
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x0001B1B8 File Offset: 0x000195B8
		protected override float Radius
		{
			get
			{
				if (this.m_light == null)
				{
					return 0f;
				}
				return this.m_light.range * Mathf.Tan(0.017453292f * this.m_light.spotAngle / 2f);
			}
			set
			{
				if (this.m_light != null)
				{
					this.m_light.spotAngle = Mathf.Atan2(value, this.m_light.range) * 57.29578f * 2f;
				}
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001B1F4 File Offset: 0x000195F4
		protected override void AwakeOverride()
		{
			if (this.m_light == null)
			{
				this.m_light = base.GetComponent<Light>();
			}
			if (this.m_light == null)
			{
				Debug.LogError("Set Light");
			}
			if (this.m_light.type != LightType.Spot)
			{
				Debug.LogWarning("m_light.Type != LightType.Spot");
			}
			base.AwakeOverride();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001B25C File Offset: 0x0001965C
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Light), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(Light), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_light, Strong.PropertyInfo<Light, float>(Expression.Lambda<Func<Light, float>>(Expression.Property(parameterExpression, methodof(Light.get_range())), new ParameterExpression[]
			{
				parameterExpression
			})));
			RuntimeUndo.RecordValue(this.m_light, Strong.PropertyInfo<Light, float>(Expression.Lambda<Func<Light, float>>(Expression.Property(parameterExpression2, methodof(Light.get_spotAngle())), new ParameterExpression[]
			{
				parameterExpression2
			})));
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001B304 File Offset: 0x00019704
		private void Reset()
		{
			this.LineColor = new Color(1f, 1f, 0.5f, 0.5f);
			this.HandlesColor = new Color(1f, 1f, 0.35f, 0.95f);
			this.SelectionColor = new Color(1f, 1f, 0f, 1f);
		}

		// Token: 0x04000485 RID: 1157
		[SerializeField]
		private Light m_light;
	}
}
