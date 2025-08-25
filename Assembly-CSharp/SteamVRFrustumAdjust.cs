using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000E2A RID: 3626
[AddComponentMenu("SteamVR/SteamVRFrustumAdjust")]
public class SteamVRFrustumAdjust : MonoBehaviour
{
	// Token: 0x06006F98 RID: 28568 RVA: 0x002A0D43 File Offset: 0x0029F143
	public SteamVRFrustumAdjust()
	{
	}

	// Token: 0x06006F99 RID: 28569 RVA: 0x002A0D4C File Offset: 0x0029F14C
	private void OnEnable()
	{
		this.m_Camera = base.GetComponent<Camera>();
		HmdMatrix34_t eyeToHeadTransform = SteamVR.instance.hmd.GetEyeToHeadTransform(EVREye.Eye_Left);
		if (eyeToHeadTransform.m0 < 1f)
		{
			this.isCantedFov = true;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			SteamVR.instance.hmd.GetProjectionRaw(EVREye.Eye_Left, ref num, ref num2, ref num3, ref num4);
			float num5 = Mathf.Acos(eyeToHeadTransform.m0);
			float num6 = Mathf.Atan(SteamVR.instance.tanHalfFov.x);
			float num7 = Mathf.Tan(num5 + num6);
			this.projectionMatrix.m00 = 1f / num7;
			float num8 = Mathf.Atan(-num);
			float num9 = SteamVR.instance.tanHalfFov.y * Mathf.Cos(num8) / Mathf.Cos(num8 + num5);
			this.projectionMatrix.m11 = 1f / num9;
			this.projectionMatrix.m22 = -(this.m_Camera.farClipPlane + this.m_Camera.nearClipPlane) / (this.m_Camera.farClipPlane - this.m_Camera.nearClipPlane);
			this.projectionMatrix.m23 = -2f * this.m_Camera.farClipPlane * this.m_Camera.nearClipPlane / (this.m_Camera.farClipPlane - this.m_Camera.nearClipPlane);
			this.projectionMatrix.m32 = -1f;
		}
		else
		{
			this.isCantedFov = false;
		}
	}

	// Token: 0x06006F9A RID: 28570 RVA: 0x002A0EE1 File Offset: 0x0029F2E1
	private void OnDisable()
	{
		if (this.isCantedFov)
		{
			this.isCantedFov = false;
			this.m_Camera.ResetCullingMatrix();
		}
	}

	// Token: 0x06006F9B RID: 28571 RVA: 0x002A0F00 File Offset: 0x0029F300
	private void OnPreCull()
	{
		if (this.isCantedFov)
		{
			this.m_Camera.cullingMatrix = this.projectionMatrix * this.m_Camera.worldToCameraMatrix;
		}
	}

	// Token: 0x04006155 RID: 24917
	private bool isCantedFov;

	// Token: 0x04006156 RID: 24918
	private Camera m_Camera;

	// Token: 0x04006157 RID: 24919
	private Matrix4x4 projectionMatrix;
}
