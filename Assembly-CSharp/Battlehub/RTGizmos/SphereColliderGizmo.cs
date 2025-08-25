using System;
using System.Linq.Expressions;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E8 RID: 232
	public class SphereColliderGizmo : SphereGizmo
	{
		// Token: 0x060004D2 RID: 1234 RVA: 0x0001AFBF File Offset: 0x000193BF
		public SphereColliderGizmo()
		{
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0001AFC7 File Offset: 0x000193C7
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x0001AFEB File Offset: 0x000193EB
		protected override Vector3 Center
		{
			get
			{
				if (this.m_collider == null)
				{
					return Vector3.zero;
				}
				return this.m_collider.center;
			}
			set
			{
				if (this.m_collider != null)
				{
					this.m_collider.center = value;
				}
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001B00A File Offset: 0x0001940A
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x0001B02E File Offset: 0x0001942E
		protected override float Radius
		{
			get
			{
				if (this.m_collider == null)
				{
					return 0f;
				}
				return this.m_collider.radius;
			}
			set
			{
				if (this.m_collider != null)
				{
					this.m_collider.radius = value;
				}
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001B04D File Offset: 0x0001944D
		protected override void AwakeOverride()
		{
			if (this.m_collider == null)
			{
				this.m_collider = base.GetComponent<SphereCollider>();
			}
			if (this.m_collider == null)
			{
				Debug.LogError("Set Collider");
			}
			base.AwakeOverride();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001B090 File Offset: 0x00019490
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(SphereCollider), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(SphereCollider), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_collider, Strong.PropertyInfo<SphereCollider, Vector3>(Expression.Lambda<Func<SphereCollider, Vector3>>(Expression.Property(parameterExpression, methodof(SphereCollider.get_center())), new ParameterExpression[]
			{
				parameterExpression
			})));
			RuntimeUndo.RecordValue(this.m_collider, Strong.PropertyInfo<SphereCollider, float>(Expression.Lambda<Func<SphereCollider, float>>(Expression.Property(parameterExpression2, methodof(SphereCollider.get_radius())), new ParameterExpression[]
			{
				parameterExpression2
			})));
		}

		// Token: 0x04000484 RID: 1156
		[SerializeField]
		private SphereCollider m_collider;
	}
}
