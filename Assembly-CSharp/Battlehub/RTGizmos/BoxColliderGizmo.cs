using System;
using System.Linq.Expressions;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000DE RID: 222
	public class BoxColliderGizmo : BoxGizmo
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x00018F71 File Offset: 0x00017371
		public BoxColliderGizmo()
		{
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x00018F79 File Offset: 0x00017379
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x00018FB0 File Offset: 0x000173B0
		protected override Bounds Bounds
		{
			get
			{
				if (this.m_collider == null)
				{
					return BoxColliderGizmo.m_zeroBounds;
				}
				return new Bounds(this.m_collider.center, this.m_collider.size);
			}
			set
			{
				if (this.m_collider != null)
				{
					this.m_collider.center = value.center;
					this.m_collider.size = value.extents * 2f;
				}
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00018FFC File Offset: 0x000173FC
		protected override void AwakeOverride()
		{
			if (this.m_collider == null)
			{
				this.m_collider = base.GetComponent<BoxCollider>();
			}
			if (this.m_collider == null)
			{
				Debug.LogError("Set Collider");
			}
			base.AwakeOverride();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001903C File Offset: 0x0001743C
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(BoxCollider), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(BoxCollider), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_collider, Strong.PropertyInfo<BoxCollider, Vector3>(Expression.Lambda<Func<BoxCollider, Vector3>>(Expression.Property(parameterExpression, methodof(BoxCollider.get_center())), new ParameterExpression[]
			{
				parameterExpression
			})));
			RuntimeUndo.RecordValue(this.m_collider, Strong.PropertyInfo<BoxCollider, Vector3>(Expression.Lambda<Func<BoxCollider, Vector3>>(Expression.Property(parameterExpression2, methodof(BoxCollider.get_size())), new ParameterExpression[]
			{
				parameterExpression2
			})));
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000190E4 File Offset: 0x000174E4
		// Note: this type is marked as 'beforefieldinit'.
		static BoxColliderGizmo()
		{
		}

		// Token: 0x04000472 RID: 1138
		[SerializeField]
		private BoxCollider m_collider;

		// Token: 0x04000473 RID: 1139
		private static readonly Bounds m_zeroBounds = default(Bounds);
	}
}
