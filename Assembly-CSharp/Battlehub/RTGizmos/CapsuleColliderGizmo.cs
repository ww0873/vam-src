using System;
using System.Linq.Expressions;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E0 RID: 224
	public class CapsuleColliderGizmo : CapsuleGizmo
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x00019616 File Offset: 0x00017A16
		public CapsuleColliderGizmo()
		{
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0001961E File Offset: 0x00017A1E
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x00019642 File Offset: 0x00017A42
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

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00019661 File Offset: 0x00017A61
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x00019685 File Offset: 0x00017A85
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x000196A4 File Offset: 0x00017AA4
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x000196C8 File Offset: 0x00017AC8
		protected override float Height
		{
			get
			{
				if (this.m_collider == null)
				{
					return 0f;
				}
				return this.m_collider.height;
			}
			set
			{
				if (this.m_collider != null)
				{
					this.m_collider.height = value;
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x000196E7 File Offset: 0x00017AE7
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x00019707 File Offset: 0x00017B07
		protected override int Direction
		{
			get
			{
				if (this.m_collider == null)
				{
					return 0;
				}
				return this.m_collider.direction;
			}
			set
			{
				if (this.m_collider != null)
				{
					this.m_collider.direction = value;
				}
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00019726 File Offset: 0x00017B26
		protected override void AwakeOverride()
		{
			if (this.m_collider == null)
			{
				this.m_collider = base.GetComponent<CapsuleCollider>();
			}
			if (this.m_collider == null)
			{
				Debug.LogError("Set Collider");
			}
			base.AwakeOverride();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00019768 File Offset: 0x00017B68
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(CapsuleCollider), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(CapsuleCollider), "x");
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(CapsuleCollider), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_collider, Strong.PropertyInfo<CapsuleCollider, Vector3>(Expression.Lambda<Func<CapsuleCollider, Vector3>>(Expression.Property(parameterExpression, methodof(CapsuleCollider.get_center())), new ParameterExpression[]
			{
				parameterExpression
			})));
			RuntimeUndo.RecordValue(this.m_collider, Strong.PropertyInfo<CapsuleCollider, float>(Expression.Lambda<Func<CapsuleCollider, float>>(Expression.Property(parameterExpression2, methodof(CapsuleCollider.get_height())), new ParameterExpression[]
			{
				parameterExpression2
			})));
			RuntimeUndo.RecordValue(this.m_collider, Strong.PropertyInfo<CapsuleCollider, int>(Expression.Lambda<Func<CapsuleCollider, int>>(Expression.Property(parameterExpression3, methodof(CapsuleCollider.get_direction())), new ParameterExpression[]
			{
				parameterExpression3
			})));
		}

		// Token: 0x04000474 RID: 1140
		[SerializeField]
		private CapsuleCollider m_collider;
	}
}
