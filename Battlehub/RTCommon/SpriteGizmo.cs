using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000D9 RID: 217
	public class SpriteGizmo : MonoBehaviour, IGL
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x0001798C File Offset: 0x00015D8C
		public SpriteGizmo()
		{
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00017994 File Offset: 0x00015D94
		private void OnEnable()
		{
			GLRenderer instance = GLRenderer.Instance;
			if (instance)
			{
				instance.Add(this);
			}
			this.m_collider = base.GetComponent<SphereCollider>();
			if (this.m_collider == null)
			{
				this.m_collider = base.gameObject.AddComponent<SphereCollider>();
				this.m_collider.radius = 0.25f;
			}
			if (this.m_collider != null && this.m_collider.hideFlags == HideFlags.None)
			{
				this.m_collider.hideFlags = HideFlags.HideInInspector;
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00017A24 File Offset: 0x00015E24
		private void OnDisable()
		{
			GLRenderer instance = GLRenderer.Instance;
			if (instance)
			{
				instance.Remove(this);
			}
			if (this.m_collider != null)
			{
				UnityEngine.Object.Destroy(this.m_collider);
				this.m_collider = null;
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00017A6C File Offset: 0x00015E6C
		void IGL.Draw(int cullingMask)
		{
			RTLayer rtlayer = RTLayer.SceneView;
			if ((cullingMask & (int)rtlayer) == 0)
			{
				return;
			}
			this.Material.SetPass(0);
			RuntimeGraphics.DrawQuad(base.transform.localToWorldMatrix);
		}

		// Token: 0x04000450 RID: 1104
		public Material Material;

		// Token: 0x04000451 RID: 1105
		[SerializeField]
		[HideInInspector]
		private SphereCollider m_collider;
	}
}
