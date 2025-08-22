using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E69 RID: 3689
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Camera Motion Blur")]
	public class CameraMotionBlur : PostEffectsBase
	{
		// Token: 0x060070A3 RID: 28835 RVA: 0x002A9904 File Offset: 0x002A7D04
		public CameraMotionBlur()
		{
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x002A99AC File Offset: 0x002A7DAC
		private void CalculateViewProjection()
		{
			Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
			Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
			this.currentViewProjMat = gpuprojectionMatrix * worldToCameraMatrix;
			if (this._camera.stereoEnabled)
			{
				for (int i = 0; i < 2; i++)
				{
					Matrix4x4 stereoViewMatrix = this._camera.GetStereoViewMatrix((i != 0) ? Camera.StereoscopicEye.Right : Camera.StereoscopicEye.Left);
					Matrix4x4 matrix4x = this._camera.GetStereoProjectionMatrix((i != 0) ? Camera.StereoscopicEye.Right : Camera.StereoscopicEye.Left);
					matrix4x = GL.GetGPUProjectionMatrix(matrix4x, true);
					this.currentStereoViewProjMat[i] = matrix4x * stereoViewMatrix;
				}
			}
		}

		// Token: 0x060070A5 RID: 28837 RVA: 0x002A9A5C File Offset: 0x002A7E5C
		private new void Start()
		{
			this.CheckResources();
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			this.currentStereoViewProjMat = new Matrix4x4[2];
			this.prevStereoViewProjMat = new Matrix4x4[2];
			this.CalculateViewProjection();
			this.Remember();
			this.wasActive = false;
		}

		// Token: 0x060070A6 RID: 28838 RVA: 0x002A9AC9 File Offset: 0x002A7EC9
		private void OnEnable()
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this._camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x060070A7 RID: 28839 RVA: 0x002A9AFC File Offset: 0x002A7EFC
		private void OnDisable()
		{
			if (null != this.motionBlurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.motionBlurMaterial);
				this.motionBlurMaterial = null;
			}
			if (null != this.dx11MotionBlurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.dx11MotionBlurMaterial);
				this.dx11MotionBlurMaterial = null;
			}
			if (null != this.tmpCam)
			{
				UnityEngine.Object.DestroyImmediate(this.tmpCam);
				this.tmpCam = null;
			}
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x002A9B74 File Offset: 0x002A7F74
		public override bool CheckResources()
		{
			base.CheckSupport(true, true);
			this.motionBlurMaterial = base.CheckShaderAndCreateMaterial(this.shader, this.motionBlurMaterial);
			if (this.supportDX11 && this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11)
			{
				this.dx11MotionBlurMaterial = base.CheckShaderAndCreateMaterial(this.dx11MotionBlurShader, this.dx11MotionBlurMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060070A9 RID: 28841 RVA: 0x002A9BE8 File Offset: 0x002A7FE8
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				this.StartFrame();
			}
			RenderTextureFormat format = (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf)) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.RGHalf;
			RenderTexture temporary = RenderTexture.GetTemporary(CameraMotionBlur.divRoundUp(source.width, this.velocityDownsample), CameraMotionBlur.divRoundUp(source.height, this.velocityDownsample), 0, format);
			this.maxVelocity = Mathf.Max(2f, this.maxVelocity);
			float value = this.maxVelocity;
			bool flag = this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && this.dx11MotionBlurMaterial == null;
			int num;
			int height;
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag || this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.maxVelocity = Mathf.Min(this.maxVelocity, CameraMotionBlur.MAX_RADIUS);
				num = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				height = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				value = (float)(temporary.width / num);
			}
			else
			{
				num = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				height = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				value = (float)(temporary.width / num);
			}
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, height, 0, format);
			RenderTexture temporary3 = RenderTexture.GetTemporary(num, height, 0, format);
			temporary.filterMode = FilterMode.Point;
			temporary2.filterMode = FilterMode.Point;
			temporary3.filterMode = FilterMode.Point;
			if (this.noiseTexture)
			{
				this.noiseTexture.filterMode = FilterMode.Point;
			}
			source.wrapMode = TextureWrapMode.Clamp;
			temporary.wrapMode = TextureWrapMode.Clamp;
			temporary3.wrapMode = TextureWrapMode.Clamp;
			temporary2.wrapMode = TextureWrapMode.Clamp;
			this.CalculateViewProjection();
			if (base.gameObject.activeInHierarchy && !this.wasActive)
			{
				this.Remember();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			Matrix4x4 matrix4x = Matrix4x4.Inverse(this.currentViewProjMat);
			this.motionBlurMaterial.SetMatrix("_InvViewProj", matrix4x);
			this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
			this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			if (this._camera.stereoEnabled)
			{
				Matrix4x4[] array = new Matrix4x4[]
				{
					Matrix4x4.Inverse(this.currentStereoViewProjMat[0]),
					Matrix4x4.Inverse(this.currentStereoViewProjMat[1])
				};
				Matrix4x4 value2 = this.prevStereoViewProjMat[0] * array[0];
				this.motionBlurMaterial.SetMatrix("_StereoToPrevViewProjCombined0", value2);
				this.motionBlurMaterial.SetMatrix("_StereoToPrevViewProjCombined1", this.prevStereoViewProjMat[1] * array[1]);
			}
			this.motionBlurMaterial.SetFloat("_MaxVelocity", value);
			this.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", value);
			this.motionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
			this.motionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
			this.motionBlurMaterial.SetFloat("_Jitter", this.jitter);
			this.motionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
			this.motionBlurMaterial.SetTexture("_VelTex", temporary);
			this.motionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
			this.motionBlurMaterial.SetTexture("_TileTexDebug", temporary2);
			if (this.preview)
			{
				Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
				Matrix4x4 identity = Matrix4x4.identity;
				identity.SetTRS(this.previewScale * 0.3333f, Quaternion.identity, Vector3.one);
				Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
				this.prevViewProjMat = gpuprojectionMatrix * identity * worldToCameraMatrix;
				this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
				this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Vector4 zero = Vector4.zero;
				float num2 = Vector3.Dot(base.transform.up, Vector3.up);
				Vector3 rhs = this.prevFramePos - base.transform.position;
				float magnitude = rhs.magnitude;
				float num3 = Vector3.Angle(base.transform.up, this.prevFrameUp) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.x = this.rotationScale * num3;
				num3 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.y = this.rotationScale * num2 * num3;
				num3 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.z = this.rotationScale * (1f - num2) * num3;
				if (magnitude > Mathf.Epsilon && this.movementScale > Mathf.Epsilon)
				{
					zero.w = this.movementScale * Vector3.Dot(base.transform.forward, rhs) * ((float)source.width * 0.5f);
					zero.x += this.movementScale * Vector3.Dot(base.transform.up, rhs) * ((float)source.width * 0.5f);
					zero.y += this.movementScale * Vector3.Dot(base.transform.right, rhs) * ((float)source.width * 0.5f);
				}
				if (this.preview)
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", new Vector4(this.previewScale.y, this.previewScale.x, 0f, this.previewScale.z) * 0.5f * this._camera.fieldOfView);
				}
				else
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
				}
			}
			else
			{
				Graphics.Blit(source, temporary, this.motionBlurMaterial, 0);
				Camera camera = null;
				if (this.excludeLayers.value != 0)
				{
					camera = this.GetTmpCam();
				}
				if (camera && this.excludeLayers.value != 0 && this.replacementClear && this.replacementClear.isSupported)
				{
					camera.targetTexture = temporary;
					camera.cullingMask = this.excludeLayers;
					camera.RenderWithShader(this.replacementClear, string.Empty);
				}
			}
			if (!this.preview && Time.frameCount != this.prevFrameCount)
			{
				this.prevFrameCount = Time.frameCount;
				this.Remember();
			}
			source.filterMode = FilterMode.Bilinear;
			if (this.showVelocity)
			{
				this.motionBlurMaterial.SetFloat("_DisplayVelocityScale", this.showVelocityScale);
				Graphics.Blit(temporary, destination, this.motionBlurMaterial, 1);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && !flag)
			{
				this.dx11MotionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
				this.dx11MotionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
				this.dx11MotionBlurMaterial.SetFloat("_Jitter", this.jitter);
				this.dx11MotionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.dx11MotionBlurMaterial.SetTexture("_VelTex", temporary);
				this.dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
				this.dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				this.dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", value);
				Graphics.Blit(temporary, temporary2, this.dx11MotionBlurMaterial, 0);
				Graphics.Blit(temporary2, temporary3, this.dx11MotionBlurMaterial, 1);
				Graphics.Blit(source, destination, this.dx11MotionBlurMaterial, 2);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 4);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 6);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 7);
			}
			else
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 5);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
		}

		// Token: 0x060070AA RID: 28842 RVA: 0x002AA57C File Offset: 0x002A897C
		private void Remember()
		{
			this.prevViewProjMat = this.currentViewProjMat;
			this.prevFrameForward = base.transform.forward;
			this.prevFrameUp = base.transform.up;
			this.prevFramePos = base.transform.position;
			this.prevStereoViewProjMat[0] = this.currentStereoViewProjMat[0];
			this.prevStereoViewProjMat[1] = this.currentStereoViewProjMat[1];
		}

		// Token: 0x060070AB RID: 28843 RVA: 0x002AA60C File Offset: 0x002A8A0C
		private Camera GetTmpCam()
		{
			if (this.tmpCam == null)
			{
				string name = "_" + this._camera.name + "_MotionBlurTmpCam";
				GameObject y = GameObject.Find(name);
				if (null == y)
				{
					this.tmpCam = new GameObject(name, new Type[]
					{
						typeof(Camera)
					});
				}
				else
				{
					this.tmpCam = y;
				}
			}
			this.tmpCam.hideFlags = HideFlags.DontSave;
			this.tmpCam.transform.position = this._camera.transform.position;
			this.tmpCam.transform.rotation = this._camera.transform.rotation;
			this.tmpCam.transform.localScale = this._camera.transform.localScale;
			this.tmpCam.GetComponent<Camera>().CopyFrom(this._camera);
			this.tmpCam.GetComponent<Camera>().enabled = false;
			this.tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
			this.tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
			return this.tmpCam.GetComponent<Camera>();
		}

		// Token: 0x060070AC RID: 28844 RVA: 0x002AA744 File Offset: 0x002A8B44
		private void StartFrame()
		{
			this.prevFramePos = Vector3.Slerp(this.prevFramePos, base.transform.position, 0.75f);
		}

		// Token: 0x060070AD RID: 28845 RVA: 0x002AA767 File Offset: 0x002A8B67
		private static int divRoundUp(int x, int d)
		{
			return (x + d - 1) / d;
		}

		// Token: 0x060070AE RID: 28846 RVA: 0x002AA770 File Offset: 0x002A8B70
		// Note: this type is marked as 'beforefieldinit'.
		static CameraMotionBlur()
		{
		}

		// Token: 0x04006374 RID: 25460
		private static float MAX_RADIUS = 10f;

		// Token: 0x04006375 RID: 25461
		public CameraMotionBlur.MotionBlurFilter filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction;

		// Token: 0x04006376 RID: 25462
		public bool preview;

		// Token: 0x04006377 RID: 25463
		public Vector3 previewScale = Vector3.one;

		// Token: 0x04006378 RID: 25464
		public float movementScale;

		// Token: 0x04006379 RID: 25465
		public float rotationScale = 1f;

		// Token: 0x0400637A RID: 25466
		public float maxVelocity = 8f;

		// Token: 0x0400637B RID: 25467
		public float minVelocity = 0.1f;

		// Token: 0x0400637C RID: 25468
		public float velocityScale = 0.375f;

		// Token: 0x0400637D RID: 25469
		public float softZDistance = 0.005f;

		// Token: 0x0400637E RID: 25470
		public int velocityDownsample = 1;

		// Token: 0x0400637F RID: 25471
		public LayerMask excludeLayers = 0;

		// Token: 0x04006380 RID: 25472
		private GameObject tmpCam;

		// Token: 0x04006381 RID: 25473
		public Shader shader;

		// Token: 0x04006382 RID: 25474
		public Shader dx11MotionBlurShader;

		// Token: 0x04006383 RID: 25475
		public Shader replacementClear;

		// Token: 0x04006384 RID: 25476
		private Material motionBlurMaterial;

		// Token: 0x04006385 RID: 25477
		private Material dx11MotionBlurMaterial;

		// Token: 0x04006386 RID: 25478
		public Texture2D noiseTexture;

		// Token: 0x04006387 RID: 25479
		public float jitter = 0.05f;

		// Token: 0x04006388 RID: 25480
		public bool showVelocity;

		// Token: 0x04006389 RID: 25481
		public float showVelocityScale = 1f;

		// Token: 0x0400638A RID: 25482
		private Matrix4x4 currentViewProjMat;

		// Token: 0x0400638B RID: 25483
		private Matrix4x4[] currentStereoViewProjMat;

		// Token: 0x0400638C RID: 25484
		private Matrix4x4 prevViewProjMat;

		// Token: 0x0400638D RID: 25485
		private Matrix4x4[] prevStereoViewProjMat;

		// Token: 0x0400638E RID: 25486
		private int prevFrameCount;

		// Token: 0x0400638F RID: 25487
		private bool wasActive;

		// Token: 0x04006390 RID: 25488
		private Vector3 prevFrameForward = Vector3.forward;

		// Token: 0x04006391 RID: 25489
		private Vector3 prevFrameUp = Vector3.up;

		// Token: 0x04006392 RID: 25490
		private Vector3 prevFramePos = Vector3.zero;

		// Token: 0x04006393 RID: 25491
		private Camera _camera;

		// Token: 0x02000E6A RID: 3690
		public enum MotionBlurFilter
		{
			// Token: 0x04006395 RID: 25493
			CameraMotion,
			// Token: 0x04006396 RID: 25494
			LocalBlur,
			// Token: 0x04006397 RID: 25495
			Reconstruction,
			// Token: 0x04006398 RID: 25496
			ReconstructionDX11,
			// Token: 0x04006399 RID: 25497
			ReconstructionDisc
		}
	}
}
