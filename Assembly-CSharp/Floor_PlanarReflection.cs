using System;
using UnityEngine;

// Token: 0x020002F3 RID: 755
[Serializable]
public class Floor_PlanarReflection : MonoBehaviour
{
	// Token: 0x060011C6 RID: 4550 RVA: 0x00061F52 File Offset: 0x00060352
	public Floor_PlanarReflection()
	{
		this.renderTextureSize = 256;
		this.clipPlaneOffset = 0.01f;
		this.disablePixelLights = true;
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x00061F78 File Offset: 0x00060378
	public virtual void Start()
	{
		this.renderTexture = new RenderTexture(this.renderTextureSize, this.renderTextureSize, 16);
		this.renderTexture.isPowerOfTwo = true;
		base.gameObject.AddComponent<Camera>();
		Camera component = base.GetComponent<Camera>();
		Camera main = Camera.main;
		component.targetTexture = this.renderTexture;
		component.clearFlags = main.clearFlags;
		component.backgroundColor = main.backgroundColor;
		component.nearClipPlane = main.nearClipPlane;
		component.farClipPlane = main.farClipPlane;
		component.fieldOfView = main.fieldOfView;
		base.GetComponent<Renderer>().material.SetTexture("_ReflectionTex", this.renderTexture);
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00062028 File Offset: 0x00060428
	public virtual void Update()
	{
		Matrix4x4 lhs = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f));
		base.GetComponent<Renderer>().material.SetMatrix("_ProjMatrix", lhs * Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix * base.transform.localToWorldMatrix);
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x000620AC File Offset: 0x000604AC
	public virtual void OnDisable()
	{
		UnityEngine.Object.Destroy(this.renderTexture);
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x000620BC File Offset: 0x000604BC
	public virtual void LateUpdate()
	{
		this.sourceCamera = Camera.main;
		if (!this.sourceCamera)
		{
			Debug.Log("Reflection rendering requires that a Camera that is tagged \"MainCamera\"! Disabling reflection.");
			base.GetComponent<Camera>().enabled = false;
		}
		else
		{
			base.GetComponent<Camera>().enabled = true;
		}
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0006210C File Offset: 0x0006050C
	public virtual void OnPreCull()
	{
		this.sourceCamera = Camera.main;
		if (this.sourceCamera)
		{
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
			Vector4 plane = new Vector4(up.x, up.y, up.z, w);
			Matrix4x4 rhs = Floor_PlanarReflection.CalculateReflectionMatrix(plane);
			base.GetComponent<Camera>().worldToCameraMatrix = this.sourceCamera.worldToCameraMatrix * rhs;
			Vector4 clipPlane = this.CameraSpacePlane(position, up);
			base.GetComponent<Camera>().projectionMatrix = Floor_PlanarReflection.CalculateObliqueMatrix(this.sourceCamera.projectionMatrix, clipPlane);
		}
		else
		{
			base.GetComponent<Camera>().ResetWorldToCameraMatrix();
		}
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x000621D5 File Offset: 0x000605D5
	public virtual void OnPreRender()
	{
		GL.invertCulling = true;
		if (this.disablePixelLights)
		{
			this.restorePixelLightCount = QualitySettings.pixelLightCount;
		}
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x000621F3 File Offset: 0x000605F3
	public virtual void OnPostRender()
	{
		GL.invertCulling = false;
		if (this.disablePixelLights)
		{
			QualitySettings.pixelLightCount = this.restorePixelLightCount;
		}
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x00062214 File Offset: 0x00060614
	public virtual Vector4 CameraSpacePlane(Vector3 pos, Vector3 normal)
	{
		Vector3 point = pos + normal * this.clipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = base.GetComponent<Camera>().worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
		Vector3 normalized = worldToCameraMatrix.MultiplyVector(normal).normalized;
		return new Vector4(normalized.x, normalized.y, normalized.z, -Vector3.Dot(lhs, normalized));
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x0006227D File Offset: 0x0006067D
	public static float sgn(float a)
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

	// Token: 0x060011D0 RID: 4560 RVA: 0x000622A8 File Offset: 0x000606A8
	public static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = default(Vector4);
		b.x = (Floor_PlanarReflection.sgn(clipPlane.x) + projection[8]) / projection[0];
		b.y = (Floor_PlanarReflection.sgn(clipPlane.y) + projection[9]) / projection[5];
		b.z = -1f;
		b.w = (1f + projection[10]) / projection[14];
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x;
		projection[6] = vector.y;
		projection[10] = vector.z + 1f;
		projection[14] = vector.w;
		return projection;
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x00062390 File Offset: 0x00060790
	public static Matrix4x4 CalculateReflectionMatrix(Vector4 plane)
	{
		return new Matrix4x4
		{
			m00 = 1f - 2f * plane[0] * plane[0],
			m01 = -2f * plane[0] * plane[1],
			m02 = -2f * plane[0] * plane[2],
			m03 = -2f * plane[3] * plane[0],
			m10 = -2f * plane[1] * plane[0],
			m11 = 1f - 2f * plane[1] * plane[1],
			m12 = -2f * plane[1] * plane[2],
			m13 = -2f * plane[3] * plane[1],
			m20 = -2f * plane[2] * plane[0],
			m21 = -2f * plane[2] * plane[1],
			m22 = 1f - 2f * plane[2] * plane[2],
			m23 = -2f * plane[3] * plane[2],
			m30 = 0f,
			m31 = 0f,
			m32 = 0f,
			m33 = 1f
		};
	}

	// Token: 0x04000F48 RID: 3912
	public int renderTextureSize;

	// Token: 0x04000F49 RID: 3913
	public float clipPlaneOffset;

	// Token: 0x04000F4A RID: 3914
	public bool disablePixelLights;

	// Token: 0x04000F4B RID: 3915
	private RenderTexture renderTexture;

	// Token: 0x04000F4C RID: 3916
	private int restorePixelLightCount;

	// Token: 0x04000F4D RID: 3917
	private Camera sourceCamera;
}
