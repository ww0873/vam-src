using System;
using System.Linq.Expressions;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E5 RID: 229
	public class PointLightGizmo : SphereGizmo
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x00019CC3 File Offset: 0x000180C3
		public PointLightGizmo()
		{
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00019CCB File Offset: 0x000180CB
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x00019CD2 File Offset: 0x000180D2
		protected override Vector3 Center
		{
			get
			{
				return Vector3.zero;
			}
			set
			{
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00019CD4 File Offset: 0x000180D4
		// (set) Token: 0x060004B3 RID: 1203 RVA: 0x00019CF8 File Offset: 0x000180F8
		protected override float Radius
		{
			get
			{
				if (this.m_light == null)
				{
					return 0f;
				}
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

		// Token: 0x060004B4 RID: 1204 RVA: 0x00019D18 File Offset: 0x00018118
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
			if (this.m_light.type != LightType.Point)
			{
				Debug.LogWarning("m_light.Type != LightType.Point");
			}
			base.AwakeOverride();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00019D80 File Offset: 0x00018180
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Light), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_light, Strong.PropertyInfo<Light, float>(Expression.Lambda<Func<Light, float>>(Expression.Property(parameterExpression, methodof(Light.get_range())), new ParameterExpression[]
			{
				parameterExpression
			})));
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00019DDC File Offset: 0x000181DC
		protected override bool OnDrag(int index, Vector3 offset)
		{
			this.Radius += offset.magnitude * Mathf.Sign(Vector3.Dot(offset, this.HandlesNormals[index]));
			if (this.Radius < 0f)
			{
				this.Radius = 0f;
				return false;
			}
			return true;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00019E38 File Offset: 0x00018238
		private void Reset()
		{
			this.LineColor = new Color(1f, 1f, 0.5f, 0.5f);
			this.HandlesColor = new Color(1f, 1f, 0.35f, 0.95f);
			this.SelectionColor = new Color(1f, 1f, 0f, 1f);
		}

		// Token: 0x0400047A RID: 1146
		[SerializeField]
		private Light m_light;
	}
}
