using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001E RID: 30
[AddComponentMenu("AQUAS/Reflection")]
[ExecuteInEditMode]
public class AQUAS_Reflection : MonoBehaviour
{
	// Token: 0x060000C2 RID: 194 RVA: 0x000077C3 File Offset: 0x00005BC3
	public AQUAS_Reflection()
	{
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00007800 File Offset: 0x00005C00
	public void OnWillRenderObject()
	{
		if (!base.enabled || !base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || !base.GetComponent<Renderer>().enabled)
		{
			return;
		}
		Camera current = Camera.current;
		if (!current)
		{
			return;
		}
		if (AQUAS_Reflection.s_InsideRendering)
		{
			return;
		}
		AQUAS_Reflection.s_InsideRendering = true;
		Camera camera;
		this.CreateMirrorObjects(current, out camera);
		Vector3 position = base.transform.position;
		Vector3 up = base.transform.up;
		int pixelLightCount = QualitySettings.pixelLightCount;
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = 0;
		}
		this.UpdateCameraModes(current, camera);
		float w = -Vector3.Dot(up, position) - this.m_ClipPlaneOffset;
		Vector4 plane = new Vector4(up.x, up.y, up.z, w);
		if (this.ignoreOcclusionCulling)
		{
			camera.useOcclusionCulling = false;
		}
		else
		{
			camera.useOcclusionCulling = true;
		}
		Matrix4x4 zero = Matrix4x4.zero;
		AQUAS_Reflection.CalculateReflectionMatrix(ref zero, plane);
		Vector3 position2 = current.transform.position;
		Vector3 position3 = zero.MultiplyPoint(position2);
		camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
		Vector4 clipPlane = this.CameraSpacePlane(camera, position, up, 1f);
		Matrix4x4 projectionMatrix = current.projectionMatrix;
		AQUAS_Reflection.CalculateObliqueMatrix(ref projectionMatrix, clipPlane);
		camera.projectionMatrix = projectionMatrix;
		camera.cullingMask = (-17 & this.m_ReflectLayers.value);
		camera.targetTexture = this.m_ReflectionTexture;
		GL.invertCulling = true;
		camera.transform.position = position3;
		Vector3 eulerAngles = current.transform.eulerAngles;
		camera.transform.eulerAngles = new Vector3(0f, eulerAngles.y, eulerAngles.z);
		camera.Render();
		camera.transform.position = position2;
		GL.invertCulling = false;
		Material[] sharedMaterials = base.GetComponent<Renderer>().sharedMaterials;
		foreach (Material material in sharedMaterials)
		{
			if (material.HasProperty("_ReflectionTex"))
			{
				material.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
			}
		}
		Matrix4x4 lhs = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f));
		Vector3 lossyScale = base.transform.lossyScale;
		Matrix4x4 matrix4x = base.transform.localToWorldMatrix * Matrix4x4.Scale(new Vector3(1f / lossyScale.x, 1f / lossyScale.y, 1f / lossyScale.z));
		matrix4x = lhs * current.projectionMatrix * current.worldToCameraMatrix * matrix4x;
		foreach (Material material2 in sharedMaterials)
		{
			material2.SetMatrix("_ProjMatrix", matrix4x);
		}
		if (this.m_DisablePixelLights)
		{
			QualitySettings.pixelLightCount = pixelLightCount;
		}
		AQUAS_Reflection.s_InsideRendering = false;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00007B24 File Offset: 0x00005F24
	private void OnDisable()
	{
		if (this.m_ReflectionTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
			this.m_ReflectionTexture = null;
		}
		IDictionaryEnumerator enumerator = this.m_ReflectionCameras.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				UnityEngine.Object.DestroyImmediate(((Camera)((DictionaryEntry)obj).Value).gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.m_ReflectionCameras.Clear();
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00007BC8 File Offset: 0x00005FC8
	private void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox skybox = src.GetComponent(typeof(Skybox)) as Skybox;
			Skybox skybox2 = dest.GetComponent(typeof(Skybox)) as Skybox;
			if (!skybox || !skybox.material)
			{
				skybox2.enabled = false;
			}
			else
			{
				skybox2.enabled = true;
				skybox2.material = skybox.material;
			}
		}
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00007CB4 File Offset: 0x000060B4
	private void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
	{
		reflectionCamera = null;
		if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.m_TextureSize)
		{
			if (this.m_ReflectionTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
			}
			this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
			this.m_ReflectionTexture.name = "__MirrorReflection" + base.GetInstanceID();
			this.m_ReflectionTexture.isPowerOfTwo = true;
			this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
			this.m_OldReflectionTextureSize = this.m_TextureSize;
		}
		reflectionCamera = (this.m_ReflectionCameras[currentCamera] as Camera);
		if (!reflectionCamera)
		{
			GameObject gameObject = new GameObject(string.Concat(new object[]
			{
				"Mirror Refl Camera id",
				base.GetInstanceID(),
				" for ",
				currentCamera.GetInstanceID()
			}), new Type[]
			{
				typeof(Camera),
				typeof(Skybox)
			});
			reflectionCamera = gameObject.GetComponent<Camera>();
			reflectionCamera.enabled = false;
			reflectionCamera.transform.position = base.transform.position;
			reflectionCamera.transform.rotation = base.transform.rotation;
			reflectionCamera.gameObject.AddComponent<FlareLayer>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			this.m_ReflectionCameras[currentCamera] = reflectionCamera;
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00007E3B File Offset: 0x0000623B
	private static float sgn(float a)
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

	// Token: 0x060000C8 RID: 200 RVA: 0x00007E64 File Offset: 0x00006264
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 point = pos + normal * this.m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00007ED0 File Offset: 0x000062D0
	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(AQUAS_Reflection.sgn(clipPlane.x), AQUAS_Reflection.sgn(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00007F80 File Offset: 0x00006380
	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
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
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00008127 File Offset: 0x00006527
	// Note: this type is marked as 'beforefieldinit'.
	static AQUAS_Reflection()
	{
	}

	// Token: 0x04000102 RID: 258
	public bool m_DisablePixelLights = true;

	// Token: 0x04000103 RID: 259
	public int m_TextureSize = 256;

	// Token: 0x04000104 RID: 260
	public float m_ClipPlaneOffset = 0.07f;

	// Token: 0x04000105 RID: 261
	public LayerMask m_ReflectLayers = -1;

	// Token: 0x04000106 RID: 262
	private Hashtable m_ReflectionCameras = new Hashtable();

	// Token: 0x04000107 RID: 263
	private RenderTexture m_ReflectionTexture;

	// Token: 0x04000108 RID: 264
	private int m_OldReflectionTextureSize;

	// Token: 0x04000109 RID: 265
	private static bool s_InsideRendering;

	// Token: 0x0400010A RID: 266
	public bool ignoreOcclusionCulling;
}
