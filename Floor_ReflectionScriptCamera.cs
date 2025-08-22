using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class Floor_ReflectionScriptCamera : MonoBehaviour
{
	// Token: 0x060011D2 RID: 4562 RVA: 0x00062552 File Offset: 0x00060952
	public Floor_ReflectionScriptCamera()
	{
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x00062588 File Offset: 0x00060988
	public void Start()
	{
		this.initialReflectionTextures = new Texture2D[this.reflectiveMaterials.Length];
		for (int i = 0; i < this.reflectiveMaterials.Length; i++)
		{
			this.initialReflectionTextures[i] = this.reflectiveMaterials[i].GetTexture(this.reflectionSampler);
		}
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x000625DC File Offset: 0x000609DC
	public void OnDisable()
	{
		if (this.initialReflectionTextures == null)
		{
			return;
		}
		for (int i = 0; i < this.reflectiveMaterials.Length; i++)
		{
			this.reflectiveMaterials[i].SetTexture(this.reflectionSampler, this.initialReflectionTextures[i]);
		}
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x0006262C File Offset: 0x00060A2C
	private Camera CreateReflectionCameraFor(Camera cam)
	{
		string text = base.gameObject.name + "Reflection" + cam.name;
		Debug.Log("AngryBots: created internal reflection camera " + text);
		GameObject gameObject = GameObject.Find(text);
		if (!gameObject)
		{
			gameObject = new GameObject(text, new Type[]
			{
				typeof(Camera)
			});
		}
		if (!gameObject.GetComponent(typeof(Camera)))
		{
			gameObject.AddComponent(typeof(Camera));
		}
		Camera component = gameObject.GetComponent<Camera>();
		component.backgroundColor = this.clearColor;
		component.clearFlags = CameraClearFlags.Color;
		this.SetStandardCameraParameter(component, this.reflectionMask);
		if (!component.targetTexture)
		{
			component.targetTexture = this.CreateTextureFor(cam);
		}
		return component;
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x00062701 File Offset: 0x00060B01
	public void HighQuality()
	{
		this.highQuality = true;
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x0006270A File Offset: 0x00060B0A
	private void SetStandardCameraParameter(Camera cam, LayerMask mask)
	{
		cam.backgroundColor = Color.black;
		cam.enabled = false;
		cam.cullingMask = this.reflectionMask;
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x00062730 File Offset: 0x00060B30
	private RenderTexture CreateTextureFor(Camera cam)
	{
		RenderTextureFormat format = RenderTextureFormat.RGB565;
		if (!SystemInfo.SupportsRenderTextureFormat(format))
		{
			format = RenderTextureFormat.Default;
		}
		float num = (!this.highQuality) ? 0.5f : 0.75f;
		return new RenderTexture(Mathf.FloorToInt((float)cam.pixelWidth * num), Mathf.FloorToInt((float)cam.pixelHeight * num), 24, format)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x00062798 File Offset: 0x00060B98
	public void RenderHelpCameras(Camera currentCam)
	{
		if (this.helperCameras == null)
		{
			this.helperCameras = new Dictionary<Camera, bool>();
		}
		if (!this.helperCameras.ContainsKey(currentCam))
		{
			this.helperCameras.Add(currentCam, false);
		}
		if (this.helperCameras[currentCam])
		{
			return;
		}
		if (!this.reflectionCamera)
		{
			this.reflectionCamera = this.CreateReflectionCameraFor(currentCam);
			foreach (Material material in this.reflectiveMaterials)
			{
				material.SetTexture(this.reflectionSampler, this.reflectionCamera.targetTexture);
			}
		}
		this.RenderReflectionFor(currentCam, this.reflectionCamera);
		this.helperCameras[currentCam] = true;
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x00062858 File Offset: 0x00060C58
	public void LateUpdate()
	{
		Transform transform = null;
		float num = float.PositiveInfinity;
		Vector3 position = Camera.main.transform.position;
		foreach (Transform transform2 in this.reflectiveObjects)
		{
			if (transform2.GetComponent<Renderer>().isVisible)
			{
				float sqrMagnitude = (position - transform2.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					transform = transform2;
				}
			}
		}
		if (!transform)
		{
			return;
		}
		this.ObjectBeingRendered(transform, Camera.main);
		if (this.helperCameras != null)
		{
			this.helperCameras.Clear();
		}
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x00062906 File Offset: 0x00060D06
	private void ObjectBeingRendered(Transform tr, Camera currentCam)
	{
		if (null == tr)
		{
			return;
		}
		this.reflectiveSurfaceHeight = tr;
		this.RenderHelpCameras(currentCam);
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x00062924 File Offset: 0x00060D24
	private void RenderReflectionFor(Camera cam, Camera reflectCamera)
	{
		if (!reflectCamera)
		{
			return;
		}
		this.SaneCameraSettings(reflectCamera);
		reflectCamera.backgroundColor = this.clearColor;
		GL.invertCulling = false;
		Transform transform = this.reflectiveSurfaceHeight;
		Vector3 eulerAngles = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
		reflectCamera.transform.position = cam.transform.position;
		Vector3 position = transform.transform.position;
		position.y = transform.position.y;
		Vector3 up = transform.transform.up;
		float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
		Vector4 plane = new Vector4(up.x, up.y, up.z, w);
		Matrix4x4 matrix4x = Matrix4x4.zero;
		matrix4x = Floor_ReflectionScriptCamera.CalculateReflectionMatrix(matrix4x, plane);
		this.oldpos = cam.transform.position;
		Vector3 position2 = matrix4x.MultiplyPoint(this.oldpos);
		reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
		Vector4 clipPlane = this.CameraSpacePlane(reflectCamera, position, up, 1f);
		Matrix4x4 matrix4x2 = cam.projectionMatrix;
		matrix4x2 = Floor_ReflectionScriptCamera.CalculateObliqueMatrix(matrix4x2, clipPlane);
		reflectCamera.projectionMatrix = matrix4x2;
		reflectCamera.transform.position = position2;
		Vector3 eulerAngles2 = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
		reflectCamera.RenderWithShader(this.replacementShader, "Reflection");
		GL.invertCulling = false;
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x00062ACB File Offset: 0x00060ECB
	private void SaneCameraSettings(Camera helperCam)
	{
		helperCam.depthTextureMode = DepthTextureMode.None;
		helperCam.backgroundColor = Color.black;
		helperCam.clearFlags = CameraClearFlags.Color;
		helperCam.renderingPath = RenderingPath.Forward;
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x00062AF0 File Offset: 0x00060EF0
	private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(Floor_ReflectionScriptCamera.sgn(clipPlane.x), Floor_ReflectionScriptCamera.sgn(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
		return projection;
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x00062BAC File Offset: 0x00060FAC
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

	// Token: 0x060011E0 RID: 4576 RVA: 0x00062D64 File Offset: 0x00061164
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

	// Token: 0x060011E1 RID: 4577 RVA: 0x00062D90 File Offset: 0x00061190
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 point = pos + normal * this.clipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
	}

	// Token: 0x04000F4E RID: 3918
	public Transform[] reflectiveObjects;

	// Token: 0x04000F4F RID: 3919
	public LayerMask reflectionMask;

	// Token: 0x04000F50 RID: 3920
	public Material[] reflectiveMaterials;

	// Token: 0x04000F51 RID: 3921
	private Transform reflectiveSurfaceHeight;

	// Token: 0x04000F52 RID: 3922
	public Shader replacementShader;

	// Token: 0x04000F53 RID: 3923
	private bool highQuality;

	// Token: 0x04000F54 RID: 3924
	public Color clearColor = Color.black;

	// Token: 0x04000F55 RID: 3925
	public string reflectionSampler = "_ReflectionTex";

	// Token: 0x04000F56 RID: 3926
	public float clipPlaneOffset = 0.07f;

	// Token: 0x04000F57 RID: 3927
	private Vector3 oldpos = Vector3.zero;

	// Token: 0x04000F58 RID: 3928
	private Camera reflectionCamera;

	// Token: 0x04000F59 RID: 3929
	private Dictionary<Camera, bool> helperCameras;

	// Token: 0x04000F5A RID: 3930
	private Texture[] initialReflectionTextures;
}
