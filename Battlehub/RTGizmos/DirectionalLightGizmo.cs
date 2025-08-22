using System;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E3 RID: 227
	public class DirectionalLightGizmo : BaseGizmo
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x00019A68 File Offset: 0x00017E68
		public DirectionalLightGizmo()
		{
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00019A70 File Offset: 0x00017E70
		protected override Matrix4x4 HandlesTransform
		{
			get
			{
				return this.Target.localToWorldMatrix;
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00019A7D File Offset: 0x00017E7D
		protected override void DrawOverride()
		{
			base.DrawOverride();
			if (this.Target == null)
			{
				return;
			}
			RuntimeGizmos.DrawDirectionalLight(this.Target.position, this.Target.rotation, Vector3.one, this.LineColor);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00019AC0 File Offset: 0x00017EC0
		private void Reset()
		{
			this.LineColor = new Color(1f, 1f, 0.5f, 0.5f);
			this.HandlesColor = new Color(1f, 1f, 0.35f, 0.95f);
			this.SelectionColor = new Color(1f, 1f, 0f, 1f);
		}
	}
}
