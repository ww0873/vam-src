using System;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class ReflectionCamera : MonoBehaviour
{
	// Token: 0x06001258 RID: 4696 RVA: 0x0006613B File Offset: 0x0006453B
	public ReflectionCamera()
	{
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x0006615C File Offset: 0x0006455C
	private void UpdateCamera(Camera cam)
	{
		this.CheckCamera(cam);
		if (cam == null)
		{
			return;
		}
		GL.invertCulling = true;
		Transform transform = base.transform;
		Vector3 eulerAngles = cam.transform.eulerAngles;
		this.reflectionCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
		this.reflectionCamera.transform.position = cam.transform.position;
		Vector3 position = transform.transform.position;
		position.y = transform.position.y;
		Vector3 up = transform.transform.up;
		float w = -Vector3.Dot(up, position) - ReflectionCamera.ClipPlaneOffset;
		Vector4 plane = new Vector4(up.x, up.y, up.z, w);
		Matrix4x4 matrix4x = Matrix4x4.zero;
		matrix4x = ReflectionCamera.CalculateReflectionMatrix(matrix4x, plane);
		this.oldPos = cam.transform.position;
		Vector3 position2 = matrix4x.MultiplyPoint(this.oldPos);
		this.reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
		Vector4 clipPlane = this.CameraSpacePlane(this.reflectionCamera, position, up, 1f);
		Matrix4x4 matrix4x2 = cam.projectionMatrix;
		matrix4x2 = ReflectionCamera.CalculateObliqueMatrix(matrix4x2, clipPlane);
		this.reflectionCamera.projectionMatrix = matrix4x2;
		this.reflectionCamera.transform.position = position2;
		Vector3 eulerAngles2 = cam.transform.eulerAngles;
		this.reflectionCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
		this.reflectionCamera.Render();
		GL.invertCulling = false;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x00066314 File Offset: 0x00064714
	private void CheckCamera(Camera cam)
	{
		if (this.goCam == null)
		{
			this.reflectionTexture = new RenderTexture((int)((float)Screen.width * this.TextureScale), (int)((float)Screen.height * this.TextureScale), 16, RenderTextureFormat.Default);
			this.reflectionTexture.DiscardContents();
			this.goCam = new GameObject("Water Refl Camera");
			this.goCam.hideFlags = HideFlags.DontSave;
			this.goCam.transform.position = base.transform.position;
			this.goCam.transform.rotation = base.transform.rotation;
			this.reflectionCamera = this.goCam.AddComponent<Camera>();
			this.reflectionCamera.depth = cam.depth - 10f;
			this.reflectionCamera.renderingPath = cam.renderingPath;
			this.reflectionCamera.depthTextureMode = DepthTextureMode.None;
			this.reflectionCamera.cullingMask = this.CullingMask;
			this.reflectionCamera.allowHDR = this.HDR;
			this.reflectionCamera.useOcclusionCulling = false;
			this.reflectionCamera.enabled = false;
			this.reflectionCamera.targetTexture = this.reflectionTexture;
			Shader.SetGlobalTexture("_ReflectionTex", this.reflectionTexture);
		}
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0006645F File Offset: 0x0006485F
	private static float Sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x00066488 File Offset: 0x00064888
	private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(ReflectionCamera.Sgn(clipPlane.x), ReflectionCamera.Sgn(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
		return projection;
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x00066544 File Offset: 0x00064944
	private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
		return reflectionMat;
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x000666FC File Offset: 0x00064AFC
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 point = pos + normal * ReflectionCamera.ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x00066766 File Offset: 0x00064B66
	private void SafeDestroy<T>(T component) where T : UnityEngine.Object
	{
		if (component == null)
		{
			return;
		}
		if (!Application.isPlaying)
		{
			UnityEngine.Object.DestroyImmediate(component);
		}
		else
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x000667A0 File Offset: 0x00064BA0
	private void ClearCamera()
	{
		if (this.goCam)
		{
			this.SafeDestroy<GameObject>(this.goCam);
			this.goCam = null;
		}
		if (this.reflectionTexture)
		{
			this.SafeDestroy<RenderTexture>(this.reflectionTexture);
			this.reflectionTexture = null;
		}
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x000667F3 File Offset: 0x00064BF3
	public void OnWillRenderObject()
	{
		this.UpdateCamera(Camera.main);
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x00066800 File Offset: 0x00064C00
	private void OnEnable()
	{
		Shader.EnableKeyword("cubeMap_off");
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x0006680C File Offset: 0x00064C0C
	private void OnDisable()
	{
		this.ClearCamera();
		Shader.DisableKeyword("cubeMap_off");
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x0006681E File Offset: 0x00064C1E
	// Note: this type is marked as 'beforefieldinit'.
	static ReflectionCamera()
	{
	}

	// Token: 0x04000FBA RID: 4026
	public LayerMask CullingMask = -17;

	// Token: 0x04000FBB RID: 4027
	public bool HDR;

	// Token: 0x04000FBC RID: 4028
	[Range(0.1f, 1f)]
	public float TextureScale = 1f;

	// Token: 0x04000FBD RID: 4029
	private RenderTexture reflectionTexture;

	// Token: 0x04000FBE RID: 4030
	private GameObject goCam;

	// Token: 0x04000FBF RID: 4031
	private Camera reflectionCamera;

	// Token: 0x04000FC0 RID: 4032
	private Vector3 oldPos;

	// Token: 0x04000FC1 RID: 4033
	private static float ClipPlaneOffset = 0.07f;
}
