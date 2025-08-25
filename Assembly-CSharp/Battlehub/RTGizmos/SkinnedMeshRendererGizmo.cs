using System;
using System.Linq.Expressions;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E7 RID: 231
	public class SkinnedMeshRendererGizmo : BoxGizmo
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x0001AE2B File Offset: 0x0001922B
		public SkinnedMeshRendererGizmo()
		{
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0001AE33 File Offset: 0x00019233
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x0001AE57 File Offset: 0x00019257
		protected override Bounds Bounds
		{
			get
			{
				if (this.m_skinnedMeshRenderer == null)
				{
					return SkinnedMeshRendererGizmo.m_zeroBounds;
				}
				return this.m_skinnedMeshRenderer.localBounds;
			}
			set
			{
				if (this.m_skinnedMeshRenderer != null)
				{
					this.m_skinnedMeshRenderer.localBounds = value;
				}
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001AE78 File Offset: 0x00019278
		protected override void AwakeOverride()
		{
			if (this.m_skinnedMeshRenderer == null)
			{
				this.m_skinnedMeshRenderer = base.GetComponent<SkinnedMeshRenderer>();
			}
			if (this.m_skinnedMeshRenderer == null)
			{
				Debug.LogError("Set SkinnedMeshRenderer");
			}
			else
			{
				this.Target = this.m_skinnedMeshRenderer.rootBone;
			}
			base.AwakeOverride();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001AEDC File Offset: 0x000192DC
		protected override void RecordOverride()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(SkinnedMeshRenderer), "x");
			base.RecordOverride();
			RuntimeUndo.RecordValue(this.m_skinnedMeshRenderer, Strong.PropertyInfo<SkinnedMeshRenderer, Bounds>(Expression.Lambda<Func<SkinnedMeshRenderer, Bounds>>(Expression.Property(parameterExpression, methodof(SkinnedMeshRenderer.get_localBounds())), new ParameterExpression[]
			{
				parameterExpression
			})));
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001AF38 File Offset: 0x00019338
		private void Reset()
		{
			this.LineColor = new Color(0.75f, 0.75f, 0.75f, 0.75f);
			this.HandlesColor = new Color(0.9f, 0.9f, 0.9f, 0.95f);
			this.SelectionColor = new Color(1f, 1f, 0f, 1f);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001AFA4 File Offset: 0x000193A4
		// Note: this type is marked as 'beforefieldinit'.
		static SkinnedMeshRendererGizmo()
		{
		}

		// Token: 0x04000482 RID: 1154
		[SerializeField]
		private SkinnedMeshRenderer m_skinnedMeshRenderer;

		// Token: 0x04000483 RID: 1155
		private static readonly Bounds m_zeroBounds = default(Bounds);
	}
}
