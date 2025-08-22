using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000DC RID: 220
	public class AudioSourceGizmo : SphereGizmo
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00018B49 File Offset: 0x00016F49
		public AudioSourceGizmo()
		{
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00018B58 File Offset: 0x00016F58
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00018B5F File Offset: 0x00016F5F
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00018B61 File Offset: 0x00016F61
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x00018B9C File Offset: 0x00016F9C
		protected override float Radius
		{
			get
			{
				if (this.m_source == null)
				{
					return 0f;
				}
				if (this.m_max)
				{
					return this.m_source.maxDistance;
				}
				return this.m_source.minDistance;
			}
			set
			{
				if (this.m_source != null)
				{
					if (this.m_max)
					{
						this.m_source.maxDistance = value;
					}
					else
					{
						this.m_source.minDistance = value;
					}
				}
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00018BD8 File Offset: 0x00016FD8
		protected override void AwakeOverride()
		{
			if (this.m_source == null)
			{
				this.m_source = base.GetComponent<AudioSource>();
			}
			if (this.m_source == null)
			{
				Debug.LogError("Set AudioSource");
			}
			if (base.gameObject.GetComponents<AudioSourceGizmo>().Count(new Func<AudioSourceGizmo, bool>(this.<AwakeOverride>m__0)) == 1)
			{
				AudioSourceGizmo audioSourceGizmo = base.gameObject.AddComponent<AudioSourceGizmo>();
				audioSourceGizmo.LineColor = this.LineColor;
				audioSourceGizmo.HandlesColor = this.HandlesColor;
				audioSourceGizmo.SelectionColor = this.SelectionColor;
				audioSourceGizmo.SelectionMargin = this.SelectionMargin;
				audioSourceGizmo.EnableUndo = this.EnableUndo;
				audioSourceGizmo.m_max = !this.m_max;
			}
			base.AwakeOverride();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00018C9C File Offset: 0x0001709C
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(AudioSource), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(AudioSource), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_source, Strong.PropertyInfo<AudioSource, float>(Expression.Lambda<Func<AudioSource, float>>(Expression.Property(parameterExpression, methodof(AudioSource.get_minDistance())), new ParameterExpression[]
			{
				parameterExpression
			})));
			RuntimeUndo.RecordValue(this.m_source, Strong.PropertyInfo<AudioSource, float>(Expression.Lambda<Func<AudioSource, float>>(Expression.Property(parameterExpression2, methodof(AudioSource.get_maxDistance())), new ParameterExpression[]
			{
				parameterExpression2
			})));
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00018D44 File Offset: 0x00017144
		private void Reset()
		{
			this.LineColor = new Color(0.375f, 0.75f, 1f, 0.5f);
			this.HandlesColor = new Color(0.375f, 0.75f, 1f, 0.5f);
			this.SelectionColor = new Color(1f, 1f, 0f, 1f);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00018DAE File Offset: 0x000171AE
		[CompilerGenerated]
		private bool <AwakeOverride>m__0(AudioSourceGizmo a)
		{
			return a.m_source == this.m_source;
		}

		// Token: 0x0400045E RID: 1118
		[SerializeField]
		private AudioSource m_source;

		// Token: 0x0400045F RID: 1119
		[SerializeField]
		[HideInInspector]
		private bool m_max = true;
	}
}
