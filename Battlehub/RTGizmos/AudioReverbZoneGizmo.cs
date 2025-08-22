using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000DB RID: 219
	public class AudioReverbZoneGizmo : SphereGizmo
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x000188D0 File Offset: 0x00016CD0
		public AudioReverbZoneGizmo()
		{
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000188DF File Offset: 0x00016CDF
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x000188E6 File Offset: 0x00016CE6
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x000188E8 File Offset: 0x00016CE8
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x00018923 File Offset: 0x00016D23
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

		// Token: 0x06000447 RID: 1095 RVA: 0x00018960 File Offset: 0x00016D60
		protected override void AwakeOverride()
		{
			if (this.m_source == null)
			{
				this.m_source = base.GetComponent<AudioReverbZone>();
			}
			if (this.m_source == null)
			{
				Debug.LogError("Set AudioSource");
			}
			if (base.gameObject.GetComponents<AudioReverbZoneGizmo>().Count(new Func<AudioReverbZoneGizmo, bool>(this.<AwakeOverride>m__0)) == 1)
			{
				AudioReverbZoneGizmo audioReverbZoneGizmo = base.gameObject.AddComponent<AudioReverbZoneGizmo>();
				audioReverbZoneGizmo.LineColor = this.LineColor;
				audioReverbZoneGizmo.HandlesColor = this.HandlesColor;
				audioReverbZoneGizmo.SelectionColor = this.SelectionColor;
				audioReverbZoneGizmo.SelectionMargin = this.SelectionMargin;
				audioReverbZoneGizmo.EnableUndo = this.EnableUndo;
				audioReverbZoneGizmo.m_max = !this.m_max;
			}
			base.AwakeOverride();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00018A24 File Offset: 0x00016E24
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(AudioReverbZone), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(AudioReverbZone), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_source, Strong.PropertyInfo<AudioReverbZone, float>(Expression.Lambda<Func<AudioReverbZone, float>>(Expression.Property(parameterExpression, methodof(AudioReverbZone.get_minDistance())), new ParameterExpression[]
			{
				parameterExpression
			})));
			RuntimeUndo.RecordValue(this.m_source, Strong.PropertyInfo<AudioReverbZone, float>(Expression.Lambda<Func<AudioReverbZone, float>>(Expression.Property(parameterExpression2, methodof(AudioReverbZone.get_maxDistance())), new ParameterExpression[]
			{
				parameterExpression2
			})));
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00018ACC File Offset: 0x00016ECC
		private void Reset()
		{
			this.LineColor = new Color(0.375f, 0.75f, 1f, 0.5f);
			this.HandlesColor = new Color(0.375f, 0.75f, 1f, 0.5f);
			this.SelectionColor = new Color(1f, 1f, 0f, 1f);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00018B36 File Offset: 0x00016F36
		[CompilerGenerated]
		private bool <AwakeOverride>m__0(AudioReverbZoneGizmo a)
		{
			return a.m_source == this.m_source;
		}

		// Token: 0x0400045C RID: 1116
		[SerializeField]
		private AudioReverbZone m_source;

		// Token: 0x0400045D RID: 1117
		[SerializeField]
		[HideInInspector]
		private bool m_max = true;
	}
}
