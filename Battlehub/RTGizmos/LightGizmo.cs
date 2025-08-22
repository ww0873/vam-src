using System;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E4 RID: 228
	public class LightGizmo : MonoBehaviour
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x00019B2A File Offset: 0x00017F2A
		public LightGizmo()
		{
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00019B34 File Offset: 0x00017F34
		private void Awake()
		{
			this.m_light = base.GetComponent<Light>();
			if (this.m_light == null)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				this.m_lightType = this.m_light.type;
				this.CreateGizmo();
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00019B80 File Offset: 0x00017F80
		private void OnDestroy()
		{
			if (this.m_gizmo != null)
			{
				UnityEngine.Object.Destroy(this.m_gizmo);
				this.m_gizmo = null;
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00019BA8 File Offset: 0x00017FA8
		private void Update()
		{
			if (this.m_light == null)
			{
				UnityEngine.Object.Destroy(this);
			}
			else if (this.m_lightType != this.m_light.type)
			{
				this.m_lightType = this.m_light.type;
				this.CreateGizmo();
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00019C00 File Offset: 0x00018000
		private void CreateGizmo()
		{
			if (this.m_gizmo != null)
			{
				UnityEngine.Object.Destroy(this.m_gizmo);
				this.m_gizmo = null;
			}
			if (this.m_lightType == LightType.Point)
			{
				if (this.m_gizmo == null)
				{
					this.m_gizmo = base.gameObject.AddComponent<PointLightGizmo>();
				}
			}
			else if (this.m_lightType == LightType.Spot)
			{
				if (this.m_gizmo == null)
				{
					this.m_gizmo = base.gameObject.AddComponent<SpotlightGizmo>();
				}
			}
			else if (this.m_lightType == LightType.Directional && this.m_gizmo == null)
			{
				this.m_gizmo = base.gameObject.AddComponent<DirectionalLightGizmo>();
			}
		}

		// Token: 0x04000477 RID: 1143
		private Light m_light;

		// Token: 0x04000478 RID: 1144
		private LightType m_lightType;

		// Token: 0x04000479 RID: 1145
		[SerializeField]
		[HideInInspector]
		private Component m_gizmo;
	}
}
