using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000D0D RID: 3341
[ExecuteInEditMode]
public class MirrorReflection : JSONStorable
{
	// Token: 0x060065E5 RID: 26085 RVA: 0x00267360 File Offset: 0x00265760
	public MirrorReflection()
	{
	}

	// Token: 0x060065E6 RID: 26086 RVA: 0x002673CF File Offset: 0x002657CF
	protected void SyncDisablePixelLights(bool b)
	{
		this._disablePixelLights = b;
		if (this.slaveReflection != null)
		{
			this.slaveReflection.disablePixelLights = b;
		}
	}

	// Token: 0x17000F02 RID: 3842
	// (get) Token: 0x060065E7 RID: 26087 RVA: 0x002673F5 File Offset: 0x002657F5
	// (set) Token: 0x060065E8 RID: 26088 RVA: 0x002673FD File Offset: 0x002657FD
	public bool disablePixelLights
	{
		get
		{
			return this._disablePixelLights;
		}
		set
		{
			if (this.disablePixelLightsJSON != null)
			{
				this.disablePixelLightsJSON.val = value;
			}
			else if (this._disablePixelLights != value)
			{
				this.SyncDisablePixelLights(value);
			}
		}
	}

	// Token: 0x060065E9 RID: 26089 RVA: 0x00267430 File Offset: 0x00265830
	protected void SetTextureSizeFromString(string size)
	{
		try
		{
			int num = int.Parse(size);
			if (num == 512 || num == 1024 || num == 2048 || num == 4096)
			{
				this._textureSize = num;
				if (this.slaveReflection != null)
				{
					this.slaveReflection.textureSize = this._textureSize;
				}
			}
			else
			{
				if (this.textureSizeJSON != null)
				{
					this.textureSizeJSON.valNoCallback = this._textureSize.ToString();
				}
				Debug.LogError("Attempted to set texture size to " + size + " which is not a valid value of 512, 1024, 2048, 4096");
			}
		}
		catch (FormatException)
		{
			Debug.LogError("Attempted to set texture size to " + size + " which is not a valid integer");
		}
	}

	// Token: 0x17000F03 RID: 3843
	// (get) Token: 0x060065EA RID: 26090 RVA: 0x0026750C File Offset: 0x0026590C
	// (set) Token: 0x060065EB RID: 26091 RVA: 0x00267514 File Offset: 0x00265914
	public int textureSize
	{
		get
		{
			return this._textureSize;
		}
		set
		{
			if (this.textureSizeJSON != null)
			{
				this.textureSizeJSON.val = value.ToString();
			}
			else if (this._textureSize != value && (value == 512 || value == 1024 || value == 2048 || value == 4096))
			{
				this.SetTextureSizeFromString(value.ToString());
			}
		}
	}

	// Token: 0x060065EC RID: 26092 RVA: 0x00267594 File Offset: 0x00265994
	protected void SetAntialiasingFromString(string aa)
	{
		try
		{
			int num = int.Parse(aa);
			if (num == 1 || num == 2 || num == 4 || num == 8)
			{
				this._antiAliasing = num;
				if (this.slaveReflection != null)
				{
					this.slaveReflection.antiAliasing = this._antiAliasing;
				}
			}
			else
			{
				if (this.antiAliasingJSON != null)
				{
					this.antiAliasingJSON.valNoCallback = this._antiAliasing.ToString();
				}
				Debug.LogError("Attempted to set antialiasing to " + aa + " which is not a valid value of 1, 2, 4, or 8");
			}
		}
		catch (FormatException)
		{
			Debug.LogError("Attempted to set antialiasing to " + aa + " which is not a valid integer");
		}
	}

	// Token: 0x17000F04 RID: 3844
	// (get) Token: 0x060065ED RID: 26093 RVA: 0x00267660 File Offset: 0x00265A60
	// (set) Token: 0x060065EE RID: 26094 RVA: 0x00267668 File Offset: 0x00265A68
	public int antiAliasing
	{
		get
		{
			return this._antiAliasing;
		}
		set
		{
			if (this.antiAliasingJSON != null)
			{
				this.antiAliasingJSON.val = value.ToString();
			}
			else if (this._antiAliasing != value && (value == 1 || value == 2 || value == 4 || value == 8))
			{
				this.SetAntialiasingFromString(value.ToString());
			}
		}
	}

	// Token: 0x060065EF RID: 26095 RVA: 0x002676D8 File Offset: 0x00265AD8
	protected bool MaterialHasProp(string propName)
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			Material material;
			if (Application.isPlaying)
			{
				material = component.material;
			}
			else
			{
				material = component.sharedMaterial;
			}
			if (material.HasProperty(propName))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060065F0 RID: 26096 RVA: 0x00267724 File Offset: 0x00265B24
	protected void SetMaterialProp(string propName, float propValue)
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			Material[] array;
			if (Application.isPlaying)
			{
				array = component.materials;
			}
			else
			{
				array = component.sharedMaterials;
			}
			foreach (Material material in array)
			{
				if (material.HasProperty(propName))
				{
					material.SetFloat(propName, propValue);
				}
			}
		}
	}

	// Token: 0x060065F1 RID: 26097 RVA: 0x00267794 File Offset: 0x00265B94
	protected void SyncReflectionOpacity(float f)
	{
		this._reflectionOpacity = f;
		this.SetMaterialProp("_ReflectionOpacity", this._reflectionOpacity);
		if (this.slaveReflection != null)
		{
			this.slaveReflection.reflectionOpacity = f;
		}
	}

	// Token: 0x17000F05 RID: 3845
	// (get) Token: 0x060065F2 RID: 26098 RVA: 0x002677CB File Offset: 0x00265BCB
	// (set) Token: 0x060065F3 RID: 26099 RVA: 0x002677D3 File Offset: 0x00265BD3
	public float reflectionOpacity
	{
		get
		{
			return this._reflectionOpacity;
		}
		set
		{
			if (this.reflectionOpacityJSON != null)
			{
				this.reflectionOpacityJSON.val = value;
			}
			else if (this._reflectionOpacity != value)
			{
				this.SyncReflectionOpacity(value);
			}
		}
	}

	// Token: 0x060065F4 RID: 26100 RVA: 0x00267804 File Offset: 0x00265C04
	protected void SyncReflectionBlend(float f)
	{
		this._reflectionBlend = f;
		this.SetMaterialProp("_ReflectionBlendTexPower", this._reflectionBlend);
		if (this.slaveReflection != null)
		{
			this.slaveReflection.reflectionBlend = f;
		}
	}

	// Token: 0x17000F06 RID: 3846
	// (get) Token: 0x060065F5 RID: 26101 RVA: 0x0026783B File Offset: 0x00265C3B
	// (set) Token: 0x060065F6 RID: 26102 RVA: 0x00267843 File Offset: 0x00265C43
	public float reflectionBlend
	{
		get
		{
			return this._reflectionBlend;
		}
		set
		{
			if (this.reflectionBlendJSON != null)
			{
				this.reflectionBlendJSON.val = value;
			}
			else if (this._reflectionBlend != value)
			{
				this.SyncReflectionBlend(value);
			}
		}
	}

	// Token: 0x060065F7 RID: 26103 RVA: 0x00267874 File Offset: 0x00265C74
	protected void SyncSurfaceTexturePower(float f)
	{
		this._surfaceTexturePower = f;
		this.SetMaterialProp("_MainTexPower", this._surfaceTexturePower);
		if (this.slaveReflection != null)
		{
			this.slaveReflection.surfaceTexturePower = f;
		}
	}

	// Token: 0x17000F07 RID: 3847
	// (get) Token: 0x060065F8 RID: 26104 RVA: 0x002678AB File Offset: 0x00265CAB
	// (set) Token: 0x060065F9 RID: 26105 RVA: 0x002678B3 File Offset: 0x00265CB3
	public float surfaceTexturePower
	{
		get
		{
			return this._surfaceTexturePower;
		}
		set
		{
			if (this.surfaceTexturePowerJSON != null)
			{
				this.surfaceTexturePowerJSON.val = value;
			}
			else if (this._surfaceTexturePower != value)
			{
				this.SyncSurfaceTexturePower(value);
			}
		}
	}

	// Token: 0x060065FA RID: 26106 RVA: 0x002678E4 File Offset: 0x00265CE4
	protected void SyncSpecularIntensity(float f)
	{
		this._specularIntensity = f;
		this.SetMaterialProp("_SpecularIntensity", this._specularIntensity);
		if (this.slaveReflection != null)
		{
			this.slaveReflection.specularIntensity = f;
		}
	}

	// Token: 0x17000F08 RID: 3848
	// (get) Token: 0x060065FB RID: 26107 RVA: 0x0026791B File Offset: 0x00265D1B
	// (set) Token: 0x060065FC RID: 26108 RVA: 0x00267923 File Offset: 0x00265D23
	public float specularIntensity
	{
		get
		{
			return this._specularIntensity;
		}
		set
		{
			if (this.specularIntensityJSON != null)
			{
				this.specularIntensityJSON.val = value;
			}
			else if (this._specularIntensity != value)
			{
				this.SyncSpecularIntensity(value);
			}
		}
	}

	// Token: 0x060065FD RID: 26109 RVA: 0x00267954 File Offset: 0x00265D54
	public void SetReflectionMaterialColor(Color c)
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			Material[] array;
			if (Application.isPlaying)
			{
				array = component.materials;
			}
			else
			{
				array = component.sharedMaterials;
			}
			foreach (Material material in array)
			{
				if (material.HasProperty("_ReflectionColor"))
				{
					material.SetColor("_ReflectionColor", c);
				}
			}
		}
	}

	// Token: 0x060065FE RID: 26110 RVA: 0x002679CC File Offset: 0x00265DCC
	protected void SyncReflectionColor(float h, float s, float v)
	{
		this._currentReflectionHSVColor.H = h;
		this._currentReflectionHSVColor.S = s;
		this._currentReflectionHSVColor.V = v;
		this._currentReflectionColor = HSVColorPicker.HSVToRGB(h, s, v);
		this.SetReflectionMaterialColor(this._currentReflectionColor);
		if (this.slaveReflection != null)
		{
			this.slaveReflection.SetReflectionColor(this._currentReflectionHSVColor);
		}
	}

	// Token: 0x060065FF RID: 26111 RVA: 0x00267A39 File Offset: 0x00265E39
	public void SetReflectionColor(HSVColor hsvColor)
	{
		if (this.reflectionColorJSON != null)
		{
			this.reflectionColorJSON.val = hsvColor;
		}
		else
		{
			this.SyncReflectionColor(hsvColor.H, hsvColor.S, hsvColor.V);
		}
	}

	// Token: 0x06006600 RID: 26112 RVA: 0x00267A74 File Offset: 0x00265E74
	protected void SyncRenderQueue(float f)
	{
		int renderQueue = Mathf.FloorToInt(f);
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			Material[] array;
			if (Application.isPlaying)
			{
				array = component.materials;
			}
			else
			{
				array = component.sharedMaterials;
			}
			foreach (Material material in array)
			{
				material.renderQueue = renderQueue;
			}
		}
		if (this.slaveReflection != null)
		{
			this.slaveReflection.renderQueue = renderQueue;
		}
	}

	// Token: 0x17000F09 RID: 3849
	// (get) Token: 0x06006601 RID: 26113 RVA: 0x00267B00 File Offset: 0x00265F00
	// (set) Token: 0x06006602 RID: 26114 RVA: 0x00267B2C File Offset: 0x00265F2C
	public int renderQueue
	{
		get
		{
			if (this.renderQueueJSON != null)
			{
				return Mathf.FloorToInt(this.renderQueueJSON.val);
			}
			return 0;
		}
		set
		{
			if (this.renderQueueJSON != null)
			{
				this.renderQueueJSON.val = (float)value;
			}
		}
	}

	// Token: 0x06006603 RID: 26115 RVA: 0x00267B48 File Offset: 0x00265F48
	private void RenderMirror(Camera reflectionCamera)
	{
		Vector3 position = base.transform.position;
		Vector3 up = base.transform.up;
		float w = -Vector3.Dot(up, position) - this.m_ClipPlaneOffset;
		Vector4 plane = new Vector4(up.x, up.y, up.z, w);
		Matrix4x4 zero = Matrix4x4.zero;
		MirrorReflection.CalculateReflectionMatrix(ref zero, plane);
		reflectionCamera.worldToCameraMatrix *= zero;
		if (this.m_UseObliqueClip)
		{
			Vector4 clipPlane = this.CameraSpacePlane(reflectionCamera, position, up, 1f);
			reflectionCamera.projectionMatrix = reflectionCamera.CalculateObliqueMatrix(clipPlane);
		}
		reflectionCamera.cullingMask = (-17 & this.m_ReflectLayers.value);
		GL.invertCulling = true;
		reflectionCamera.transform.position = reflectionCamera.cameraToWorldMatrix.GetPosition();
		reflectionCamera.transform.rotation = reflectionCamera.cameraToWorldMatrix.GetRotation();
		reflectionCamera.Render();
		GL.invertCulling = false;
	}

	// Token: 0x06006604 RID: 26116 RVA: 0x00267C38 File Offset: 0x00266038
	public void OnWillRenderObject()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (!base.enabled || !component || !component.sharedMaterial || !component.enabled || !MirrorReflection.globalEnabled)
		{
			return;
		}
		Camera current = Camera.current;
		if (!current)
		{
			return;
		}
		Vector3 rhs = current.transform.position - base.transform.position;
		if (!this.renderBackside)
		{
			float num = Vector3.Dot(base.transform.up, rhs);
			if (num <= 0.001f)
			{
				return;
			}
		}
		if (MirrorReflection.s_InsideRendering)
		{
			return;
		}
		MirrorReflection.s_InsideRendering = true;
		Camera camera;
		this.CreateMirrorObjects(current, out camera);
		int pixelLightCount = QualitySettings.pixelLightCount;
		if (this._disablePixelLights)
		{
			QualitySettings.pixelLightCount = 0;
		}
		this.UpdateCameraModes(current, camera);
		Vector3 position = current.transform.position;
		if (current.stereoEnabled)
		{
			if (current.stereoTargetEye == StereoTargetEyeMask.Both)
			{
				camera.ResetWorldToCameraMatrix();
				if (CameraTarget.rightTarget != null && CameraTarget.rightTarget.targetCamera != null && current.transform.parent != null)
				{
					camera.transform.position = current.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.RightEye));
					camera.transform.rotation = current.transform.parent.rotation * InputTracking.GetLocalRotation(XRNode.RightEye);
					camera.worldToCameraMatrix = CameraTarget.rightTarget.worldToCameraMatrix;
					camera.projectionMatrix = CameraTarget.rightTarget.projectionMatrix;
				}
				else
				{
					camera.transform.position = current.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.RightEye));
					camera.transform.rotation = current.transform.parent.rotation * InputTracking.GetLocalRotation(XRNode.RightEye);
					camera.worldToCameraMatrix = current.GetStereoViewMatrix(Camera.StereoscopicEye.Right);
					camera.projectionMatrix = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				}
				camera.targetTexture = this.m_ReflectionTextureRight;
				this.RenderMirror(camera);
				camera.ResetWorldToCameraMatrix();
				if (CameraTarget.leftTarget != null && CameraTarget.leftTarget.targetCamera != null && current.transform.parent != null)
				{
					camera.transform.position = current.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.LeftEye));
					camera.transform.rotation = current.transform.parent.rotation * InputTracking.GetLocalRotation(XRNode.LeftEye);
					camera.worldToCameraMatrix = CameraTarget.leftTarget.worldToCameraMatrix;
					camera.projectionMatrix = CameraTarget.leftTarget.projectionMatrix;
				}
				else
				{
					camera.transform.position = current.transform.parent.TransformPoint(InputTracking.GetLocalPosition(XRNode.LeftEye));
					camera.transform.rotation = current.transform.parent.rotation * InputTracking.GetLocalRotation(XRNode.LeftEye);
					camera.worldToCameraMatrix = current.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
					camera.projectionMatrix = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
				}
				position = camera.transform.position;
				camera.targetTexture = this.m_ReflectionTextureLeft;
				this.RenderMirror(camera);
			}
			else if (current.stereoTargetEye == StereoTargetEyeMask.Left)
			{
				camera.ResetWorldToCameraMatrix();
				camera.transform.position = current.transform.position;
				camera.transform.rotation = current.transform.rotation;
				camera.worldToCameraMatrix = current.worldToCameraMatrix;
				camera.projectionMatrix = current.projectionMatrix;
				camera.targetTexture = this.m_ReflectionTextureLeft;
				this.RenderMirror(camera);
			}
			else if (current.stereoTargetEye == StereoTargetEyeMask.Right)
			{
				camera.ResetWorldToCameraMatrix();
				camera.transform.position = current.transform.position;
				camera.transform.rotation = current.transform.rotation;
				camera.worldToCameraMatrix = current.worldToCameraMatrix;
				camera.projectionMatrix = current.projectionMatrix;
				camera.targetTexture = this.m_ReflectionTextureLeft;
				this.RenderMirror(camera);
			}
		}
		else
		{
			camera.ResetWorldToCameraMatrix();
			camera.transform.position = current.transform.position;
			camera.transform.rotation = current.transform.rotation;
			camera.worldToCameraMatrix = current.worldToCameraMatrix;
			camera.projectionMatrix = current.projectionMatrix;
			camera.targetTexture = this.m_ReflectionTextureLeft;
			this.RenderMirror(camera);
		}
		Material[] array;
		if (Application.isPlaying)
		{
			array = component.materials;
		}
		else
		{
			array = component.sharedMaterials;
		}
		Vector4 value;
		value.x = position.x;
		value.y = position.y;
		value.z = position.z;
		value.w = 0f;
		foreach (Material material in array)
		{
			if (material.HasProperty("_ReflectionTex"))
			{
				material.SetTexture("_ReflectionTex", this.m_ReflectionTextureLeft);
			}
			if (material.HasProperty("_LeftReflectionTex"))
			{
				material.SetTexture("_LeftReflectionTex", this.m_ReflectionTextureLeft);
			}
			if (material.HasProperty("_RightReflectionTex"))
			{
				material.SetTexture("_RightReflectionTex", this.m_ReflectionTextureRight);
			}
			if (material.HasProperty("_LeftCameraPosition"))
			{
				material.SetVector("_LeftCameraPosition", value);
			}
		}
		if (this._disablePixelLights)
		{
			QualitySettings.pixelLightCount = pixelLightCount;
		}
		MirrorReflection.s_InsideRendering = false;
	}

	// Token: 0x06006605 RID: 26117 RVA: 0x00268210 File Offset: 0x00266610
	protected void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
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

	// Token: 0x06006606 RID: 26118 RVA: 0x002682F0 File Offset: 0x002666F0
	protected void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
	{
		reflectionCamera = null;
		if (!this.m_ReflectionTextureRight || !this.m_ReflectionTextureLeft || this._oldReflectionTextureSize != this._textureSize || this._oldAntiAliasing != this._antiAliasing)
		{
			if (this.m_ReflectionTextureLeft)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTextureLeft);
			}
			this.m_ReflectionTextureLeft = new RenderTexture(this._textureSize, this._textureSize, 24);
			this.m_ReflectionTextureLeft.name = "__MirrorReflectionLeft" + base.GetInstanceID();
			this.m_ReflectionTextureLeft.antiAliasing = this._antiAliasing;
			this.m_ReflectionTextureLeft.isPowerOfTwo = true;
			this.m_ReflectionTextureLeft.hideFlags = HideFlags.DontSave;
			if (this.m_ReflectionTextureRight)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTextureRight);
			}
			this.m_ReflectionTextureRight = new RenderTexture(this._textureSize, this._textureSize, 24);
			this.m_ReflectionTextureRight.name = "__MirrorReflectionRight" + base.GetInstanceID();
			this.m_ReflectionTextureRight.antiAliasing = this._antiAliasing;
			this.m_ReflectionTextureRight.isPowerOfTwo = true;
			this.m_ReflectionTextureRight.hideFlags = HideFlags.DontSave;
			this._oldReflectionTextureSize = this._textureSize;
			this._oldAntiAliasing = this._antiAliasing;
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
			}), new System.Type[]
			{
				typeof(Camera),
				typeof(Skybox)
			});
			reflectionCamera = gameObject.GetComponent<Camera>();
			reflectionCamera.enabled = false;
			reflectionCamera.transform.position = base.transform.position;
			reflectionCamera.transform.rotation = base.transform.rotation;
			reflectionCamera.gameObject.AddComponent<FlareLayer>();
			gameObject.hideFlags = HideFlags.DontSave;
			this.m_ReflectionCameras[currentCamera] = reflectionCamera;
		}
	}

	// Token: 0x06006607 RID: 26119 RVA: 0x00268533 File Offset: 0x00266933
	protected static float sgn(float a)
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

	// Token: 0x06006608 RID: 26120 RVA: 0x0026855C File Offset: 0x0026695C
	protected Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 point = pos + normal * this.m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
	}

	// Token: 0x06006609 RID: 26121 RVA: 0x002685C8 File Offset: 0x002669C8
	protected static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
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

	// Token: 0x0600660A RID: 26122 RVA: 0x00268770 File Offset: 0x00266B70
	protected void Init()
	{
		Material material = null;
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			Material[] materials = component.materials;
			if (materials != null)
			{
				material = materials[0];
			}
			if (material != null && material.HasProperty("_ReflectionColor"))
			{
				Color color = material.GetColor("_ReflectionColor");
				this._currentReflectionHSVColor = HSVColorPicker.RGBToHSV(color.r, color.g, color.b);
			}
			else
			{
				this._currentReflectionHSVColor = default(HSVColor);
				this._currentReflectionHSVColor.H = 1f;
				this._currentReflectionHSVColor.S = 1f;
				this._currentReflectionHSVColor.V = 1f;
			}
			this.SyncReflectionColor(this._currentReflectionHSVColor.H, this._currentReflectionHSVColor.S, this._currentReflectionHSVColor.V);
			this.reflectionColorJSON = new JSONStorableColor("reflectionColor", this._currentReflectionHSVColor, new JSONStorableColor.SetHSVColorCallback(this.SyncReflectionColor));
			base.RegisterColor(this.reflectionColorJSON);
			if (material != null)
			{
				if (material.HasProperty("_ReflectionOpacity"))
				{
					this.SyncReflectionOpacity(material.GetFloat("_ReflectionOpacity"));
					this.reflectionOpacityJSON = new JSONStorableFloat("reflectionOpacity", this._reflectionOpacity, new JSONStorableFloat.SetFloatCallback(this.SyncReflectionOpacity), 0f, 1f, true, true);
					base.RegisterFloat(this.reflectionOpacityJSON);
				}
				if (material.HasProperty("_ReflectionBlendTexPower"))
				{
					this.SyncReflectionBlend(material.GetFloat("_ReflectionBlendTexPower"));
					this.reflectionBlendJSON = new JSONStorableFloat("reflectionBlend", this._reflectionBlend, new JSONStorableFloat.SetFloatCallback(this.SyncReflectionBlend), 0f, 2f, true, true);
					base.RegisterFloat(this.reflectionBlendJSON);
				}
				if (material.HasProperty("_MainTexPower"))
				{
					this.SyncSurfaceTexturePower(material.GetFloat("_MainTexPower"));
					this.surfaceTexturePowerJSON = new JSONStorableFloat("surfaceTexturePower", this._surfaceTexturePower, new JSONStorableFloat.SetFloatCallback(this.SyncSurfaceTexturePower), 0f, 1f, true, true);
					base.RegisterFloat(this.surfaceTexturePowerJSON);
				}
				if (material.HasProperty("_SpecularIntensity"))
				{
					this.SyncSpecularIntensity(material.GetFloat("_SpecularIntensity"));
					this.specularIntensityJSON = new JSONStorableFloat("specularIntensity", this._specularIntensity, new JSONStorableFloat.SetFloatCallback(this.SyncSpecularIntensity), 0f, 2f, true, true);
					base.RegisterFloat(this.specularIntensityJSON);
				}
				this.SyncRenderQueue((float)material.renderQueue);
				this.renderQueueJSON = new JSONStorableFloat("renderQueue", (float)material.renderQueue, new JSONStorableFloat.SetFloatCallback(this.SyncRenderQueue), -1f, 5000f, true, true);
				base.RegisterFloat(this.renderQueueJSON);
			}
			this.antiAliasingJSON = new JSONStorableStringChooser("antiAliasing", new List<string>
			{
				"1",
				"2",
				"4",
				"8"
			}, this._antiAliasing.ToString(), "Anti-aliasing", new JSONStorableStringChooser.SetStringCallback(this.SetAntialiasingFromString));
			base.RegisterStringChooser(this.antiAliasingJSON);
			this.disablePixelLightsJSON = new JSONStorableBool("disablePixelLights", this._disablePixelLights, new JSONStorableBool.SetBoolCallback(this.SyncDisablePixelLights));
			base.RegisterBool(this.disablePixelLightsJSON);
			this.textureSizeJSON = new JSONStorableStringChooser("textureSize", new List<string>
			{
				"512",
				"1024",
				"2048",
				"4096"
			}, this._textureSize.ToString(), "Texture Size", new JSONStorableStringChooser.SetStringCallback(this.SetTextureSizeFromString));
			base.RegisterStringChooser(this.textureSizeJSON);
		}
	}

	// Token: 0x0600660B RID: 26123 RVA: 0x00268B64 File Offset: 0x00266F64
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			MirrorReflectionUI componentInChildren = this.UITransform.GetComponentInChildren<MirrorReflectionUI>(true);
			if (componentInChildren != null)
			{
				this.disablePixelLightsJSON.toggle = componentInChildren.disablePixelLightsToggle;
				this.reflectionColorJSON.colorPicker = componentInChildren.reflectionColorPicker;
				if (this.reflectionOpacityJSON != null)
				{
					this.reflectionOpacityJSON.slider = componentInChildren.reflectionOpacitySlider;
				}
				else if (componentInChildren.reflectionOpacityContainer != null)
				{
					componentInChildren.reflectionOpacityContainer.gameObject.SetActive(false);
				}
				if (this.reflectionBlendJSON != null)
				{
					this.reflectionBlendJSON.slider = componentInChildren.reflectionBlendSlider;
				}
				else if (componentInChildren.reflectionBlendContainer != null)
				{
					componentInChildren.reflectionBlendContainer.gameObject.SetActive(false);
				}
				if (this.surfaceTexturePowerJSON != null)
				{
					this.surfaceTexturePowerJSON.slider = componentInChildren.surfaceTexturePowerSlider;
				}
				else if (componentInChildren.surfaceTexturePowerContainer != null)
				{
					componentInChildren.surfaceTexturePowerContainer.gameObject.SetActive(false);
				}
				if (this.specularIntensityJSON != null)
				{
					this.specularIntensityJSON.slider = componentInChildren.specularIntensitySlider;
				}
				else if (componentInChildren.specularIntensityContainer != null)
				{
					componentInChildren.specularIntensityContainer.gameObject.SetActive(false);
				}
				if (this.renderQueueJSON != null)
				{
					this.renderQueueJSON.slider = componentInChildren.renderQueueSlider;
				}
				this.antiAliasingJSON.popup = componentInChildren.antiAliasingPopup;
				this.textureSizeJSON.popup = componentInChildren.textureSizePopup;
			}
		}
	}

	// Token: 0x0600660C RID: 26124 RVA: 0x00268D08 File Offset: 0x00267108
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			MirrorReflectionUI componentInChildren = this.UITransformAlt.GetComponentInChildren<MirrorReflectionUI>(true);
			if (componentInChildren != null)
			{
				this.disablePixelLightsJSON.toggleAlt = componentInChildren.disablePixelLightsToggle;
				this.reflectionColorJSON.colorPickerAlt = componentInChildren.reflectionColorPicker;
				if (this.reflectionOpacityJSON != null)
				{
					this.reflectionOpacityJSON.sliderAlt = componentInChildren.reflectionOpacitySlider;
				}
				else if (componentInChildren.reflectionOpacityContainer != null)
				{
					componentInChildren.reflectionOpacityContainer.gameObject.SetActive(false);
				}
				if (this.reflectionBlendJSON != null)
				{
					this.reflectionBlendJSON.sliderAlt = componentInChildren.reflectionBlendSlider;
				}
				else if (componentInChildren.reflectionBlendContainer != null)
				{
					componentInChildren.reflectionBlendContainer.gameObject.SetActive(false);
				}
				if (this.surfaceTexturePowerJSON != null)
				{
					this.surfaceTexturePowerJSON.sliderAlt = componentInChildren.surfaceTexturePowerSlider;
				}
				else if (componentInChildren.surfaceTexturePowerContainer != null)
				{
					componentInChildren.surfaceTexturePowerContainer.gameObject.SetActive(false);
				}
				if (this.specularIntensityJSON != null)
				{
					this.specularIntensityJSON.sliderAlt = componentInChildren.specularIntensitySlider;
				}
				else if (componentInChildren.specularIntensityContainer != null)
				{
					componentInChildren.specularIntensityContainer.gameObject.SetActive(false);
				}
				if (this.renderQueueJSON != null)
				{
					this.renderQueueJSON.sliderAlt = componentInChildren.renderQueueSlider;
				}
				this.antiAliasingJSON.popupAlt = componentInChildren.antiAliasingPopup;
				this.textureSizeJSON.popupAlt = componentInChildren.textureSizePopup;
			}
		}
	}

	// Token: 0x0600660D RID: 26125 RVA: 0x00268EAC File Offset: 0x002672AC
	private void OnDisable()
	{
		if (this.m_ReflectionTextureRight)
		{
			UnityEngine.Object.DestroyImmediate(this.m_ReflectionTextureRight);
			this.m_ReflectionTextureRight = null;
		}
		if (this.m_ReflectionTextureLeft)
		{
			UnityEngine.Object.DestroyImmediate(this.m_ReflectionTextureLeft);
			this.m_ReflectionTextureLeft = null;
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

	// Token: 0x0600660E RID: 26126 RVA: 0x00268F70 File Offset: 0x00267370
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			if (Application.isPlaying)
			{
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}
	}

	// Token: 0x0600660F RID: 26127 RVA: 0x00268FA0 File Offset: 0x002673A0
	private void Update()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			component.enabled = ((MirrorReflection.globalEnabled || this.useSameMaterialWhenMirrorDisabled) && (this.containingAtom == null || (!this.containingAtom.globalDisableRender && !this.containingAtom.tempDisableRender)));
		}
		if (this.useSameMaterialWhenMirrorDisabled)
		{
			if (!MirrorReflection.globalEnabled)
			{
				Material[] array;
				if (Application.isPlaying)
				{
					array = component.materials;
				}
				else
				{
					array = component.sharedMaterials;
				}
				foreach (Material material in array)
				{
					if (material.HasProperty("_ReflectionTex"))
					{
						material.SetTexture("_ReflectionTex", null);
					}
					if (material.HasProperty("_LeftReflectionTex"))
					{
						material.SetTexture("_LeftReflectionTex", null);
					}
					if (material.HasProperty("_RightReflectionTex"))
					{
						material.SetTexture("_RightReflectionTex", null);
					}
				}
			}
		}
		else if (this.altObjectWhenMirrorDisabled != null)
		{
			this.altObjectWhenMirrorDisabled.gameObject.SetActive(!MirrorReflection.globalEnabled);
		}
	}

	// Token: 0x06006610 RID: 26128 RVA: 0x002690E6 File Offset: 0x002674E6
	// Note: this type is marked as 'beforefieldinit'.
	static MirrorReflection()
	{
	}

	// Token: 0x0400555E RID: 21854
	public MirrorReflection slaveReflection;

	// Token: 0x0400555F RID: 21855
	protected JSONStorableBool disablePixelLightsJSON;

	// Token: 0x04005560 RID: 21856
	[SerializeField]
	protected bool _disablePixelLights;

	// Token: 0x04005561 RID: 21857
	protected JSONStorableStringChooser textureSizeJSON;

	// Token: 0x04005562 RID: 21858
	protected int _oldReflectionTextureSize;

	// Token: 0x04005563 RID: 21859
	[SerializeField]
	protected int _textureSize = 1024;

	// Token: 0x04005564 RID: 21860
	protected JSONStorableStringChooser antiAliasingJSON;

	// Token: 0x04005565 RID: 21861
	protected int _oldAntiAliasing;

	// Token: 0x04005566 RID: 21862
	[SerializeField]
	protected int _antiAliasing = 8;

	// Token: 0x04005567 RID: 21863
	protected JSONStorableFloat reflectionOpacityJSON;

	// Token: 0x04005568 RID: 21864
	[SerializeField]
	protected float _reflectionOpacity = 0.5f;

	// Token: 0x04005569 RID: 21865
	protected JSONStorableFloat reflectionBlendJSON;

	// Token: 0x0400556A RID: 21866
	[SerializeField]
	protected float _reflectionBlend = 1f;

	// Token: 0x0400556B RID: 21867
	protected JSONStorableFloat surfaceTexturePowerJSON;

	// Token: 0x0400556C RID: 21868
	[SerializeField]
	protected float _surfaceTexturePower = 1f;

	// Token: 0x0400556D RID: 21869
	protected JSONStorableFloat specularIntensityJSON;

	// Token: 0x0400556E RID: 21870
	[SerializeField]
	protected float _specularIntensity = 1f;

	// Token: 0x0400556F RID: 21871
	protected JSONStorableColor reflectionColorJSON;

	// Token: 0x04005570 RID: 21872
	protected HSVColor _currentReflectionHSVColor;

	// Token: 0x04005571 RID: 21873
	protected Color _currentReflectionColor;

	// Token: 0x04005572 RID: 21874
	protected JSONStorableFloat renderQueueJSON;

	// Token: 0x04005573 RID: 21875
	public static bool globalEnabled = true;

	// Token: 0x04005574 RID: 21876
	public Transform altObjectWhenMirrorDisabled;

	// Token: 0x04005575 RID: 21877
	public bool useSameMaterialWhenMirrorDisabled;

	// Token: 0x04005576 RID: 21878
	public float m_ClipPlaneOffset;

	// Token: 0x04005577 RID: 21879
	public LayerMask m_ReflectLayers = -1;

	// Token: 0x04005578 RID: 21880
	public bool m_UseObliqueClip = true;

	// Token: 0x04005579 RID: 21881
	public bool renderBackside;

	// Token: 0x0400557A RID: 21882
	protected Hashtable m_ReflectionCameras = new Hashtable();

	// Token: 0x0400557B RID: 21883
	protected RenderTexture m_ReflectionTextureLeft;

	// Token: 0x0400557C RID: 21884
	protected RenderTexture m_ReflectionTextureRight;

	// Token: 0x0400557D RID: 21885
	protected static bool s_InsideRendering;
}
