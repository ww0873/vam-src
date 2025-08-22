using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace UnityStandardAssets.WaterVR
{
	// Token: 0x02000583 RID: 1411
	[ExecuteInEditMode]
	public class Water : MonoBehaviour
	{
		// Token: 0x06002386 RID: 9094 RVA: 0x000CDB1C File Offset: 0x000CBF1C
		public Water()
		{
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x000CDB88 File Offset: 0x000CBF88
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
			if (Water.s_InsideWater)
			{
				return;
			}
			Water.s_InsideWater = true;
			this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
			Water.WaterMode waterMode = this.GetWaterMode();
			Camera camera;
			Camera camera2;
			this.CreateWaterObjects(current, out camera, out camera2);
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			int pixelLightCount = QualitySettings.pixelLightCount;
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = 0;
			}
			this.UpdateCameraModes(current, camera);
			this.UpdateCameraModes(current, camera2);
			if (waterMode >= Water.WaterMode.Reflective)
			{
				if (current.stereoEnabled)
				{
					if (current.stereoTargetEye == StereoTargetEyeMask.Both || current.stereoTargetEye == StereoTargetEyeMask.Left)
					{
						Vector3 camPos = current.transform.TransformPoint(new Vector3(-0.5f * current.stereoSeparation, 0f, 0f));
						Matrix4x4 stereoProjectionMatrix = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
						this.RenderReflection(camera, this.m_ReflectionTexture0, camPos, current.transform.rotation, stereoProjectionMatrix);
						base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex0", this.m_ReflectionTexture0);
					}
					if (current.stereoTargetEye == StereoTargetEyeMask.Both || current.stereoTargetEye == StereoTargetEyeMask.Right)
					{
						Vector3 camPos2 = current.transform.TransformPoint(new Vector3(0.5f * current.stereoSeparation, 0f, 0f));
						Matrix4x4 stereoProjectionMatrix2 = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
						this.RenderReflection(camera, this.m_ReflectionTexture1, camPos2, current.transform.rotation, stereoProjectionMatrix2);
						base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex1", this.m_ReflectionTexture1);
					}
				}
				else
				{
					this.RenderReflection(camera, this.m_ReflectionTexture0, current.transform.position, current.transform.rotation, current.projectionMatrix);
					base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex0", this.m_ReflectionTexture0);
				}
			}
			if (waterMode >= Water.WaterMode.Refractive)
			{
				camera2.worldToCameraMatrix = current.worldToCameraMatrix;
				Vector4 clipPlane = this.CameraSpacePlane(camera2, position, up, -1f);
				camera2.projectionMatrix = current.CalculateObliqueMatrix(clipPlane);
				camera2.cullingMatrix = current.projectionMatrix * current.worldToCameraMatrix;
				camera2.cullingMask = (-17 & this.refractLayers.value);
				camera2.targetTexture = this.m_RefractionTexture;
				camera2.transform.position = current.transform.position;
				camera2.transform.rotation = current.transform.rotation;
				camera2.Render();
				base.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", this.m_RefractionTexture);
			}
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = pixelLightCount;
			}
			if (waterMode != Water.WaterMode.Simple)
			{
				if (waterMode != Water.WaterMode.Reflective)
				{
					if (waterMode == Water.WaterMode.Refractive)
					{
						Shader.DisableKeyword("WATER_SIMPLE");
						Shader.DisableKeyword("WATER_REFLECTIVE");
						Shader.EnableKeyword("WATER_REFRACTIVE");
					}
				}
				else
				{
					Shader.DisableKeyword("WATER_SIMPLE");
					Shader.EnableKeyword("WATER_REFLECTIVE");
					Shader.DisableKeyword("WATER_REFRACTIVE");
				}
			}
			else
			{
				Shader.EnableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
			}
			Water.s_InsideWater = false;
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000CDF08 File Offset: 0x000CC308
		private void RenderReflection(Camera reflectionCamera, RenderTexture targetTexture, Vector3 camPos, Quaternion camRot, Matrix4x4 camProjMatrix)
		{
			reflectionCamera.ResetWorldToCameraMatrix();
			reflectionCamera.transform.position = camPos;
			reflectionCamera.transform.rotation = camRot;
			reflectionCamera.projectionMatrix = camProjMatrix;
			reflectionCamera.targetTexture = targetTexture;
			reflectionCamera.rect = new Rect(0f, 0f, 1f, 1f);
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
			Vector4 plane = new Vector4(up.x, up.y, up.z, w);
			Matrix4x4 zero = Matrix4x4.zero;
			Water.CalculateReflectionMatrix(ref zero, plane);
			reflectionCamera.worldToCameraMatrix *= zero;
			Vector4 clipPlane = this.CameraSpacePlane(reflectionCamera, position, up, 1f);
			reflectionCamera.projectionMatrix = reflectionCamera.CalculateObliqueMatrix(clipPlane);
			reflectionCamera.transform.position = reflectionCamera.cameraToWorldMatrix.GetColumn(3);
			reflectionCamera.transform.rotation = Quaternion.LookRotation(reflectionCamera.cameraToWorldMatrix.GetColumn(2), reflectionCamera.cameraToWorldMatrix.GetColumn(1));
			reflectionCamera.cullingMask = (-17 & this.reflectLayers.value);
			bool invertCulling = GL.invertCulling;
			GL.invertCulling = !invertCulling;
			reflectionCamera.Render();
			GL.invertCulling = invertCulling;
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000CE074 File Offset: 0x000CC474
		private void OnDisable()
		{
			if (this.m_ReflectionTexture0)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture0);
				this.m_ReflectionTexture0 = null;
			}
			if (this.m_ReflectionTexture1)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture1);
				this.m_ReflectionTexture1 = null;
			}
			if (this.m_RefractionTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
				this.m_RefractionTexture = null;
			}
			foreach (KeyValuePair<Camera, Camera> keyValuePair in this.m_ReflectionCameras)
			{
				UnityEngine.Object.DestroyImmediate(keyValuePair.Value.gameObject);
			}
			this.m_ReflectionCameras.Clear();
			foreach (KeyValuePair<Camera, Camera> keyValuePair2 in this.m_RefractionCameras)
			{
				UnityEngine.Object.DestroyImmediate(keyValuePair2.Value.gameObject);
			}
			this.m_RefractionCameras.Clear();
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000CE1AC File Offset: 0x000CC5AC
		private void Update()
		{
			if (!base.GetComponent<Renderer>())
			{
				return;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return;
			}
			Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
			float @float = sharedMaterial.GetFloat("_WaveScale");
			Vector4 value = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
			double num = (double)Time.timeSinceLevelLoad / 20.0;
			Vector4 value2 = new Vector4((float)Math.IEEERemainder((double)(vector.x * value.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * value.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * value.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * value.w) * num, 1.0));
			sharedMaterial.SetVector("_WaveOffset", value2);
			sharedMaterial.SetVector("_WaveScale4", value);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000CE2CC File Offset: 0x000CC6CC
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
				Skybox component = src.GetComponent<Skybox>();
				Skybox component2 = dest.GetComponent<Skybox>();
				if (!component || !component.material)
				{
					component2.enabled = false;
				}
				else
				{
					component2.enabled = true;
					component2.material = component.material;
				}
			}
			dest.farClipPlane = src.farClipPlane;
			dest.nearClipPlane = src.nearClipPlane;
			dest.orthographic = src.orthographic;
			if (!XRDevice.isPresent)
			{
				dest.fieldOfView = src.fieldOfView;
			}
			dest.aspect = src.aspect;
			dest.orthographicSize = src.orthographicSize;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000CE3A4 File Offset: 0x000CC7A4
		private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
		{
			Water.WaterMode waterMode = this.GetWaterMode();
			reflectionCamera = null;
			refractionCamera = null;
			if (waterMode >= Water.WaterMode.Reflective)
			{
				if (!this.m_ReflectionTexture0 || this.m_OldReflectionTextureSize != this.textureSize)
				{
					if (this.m_ReflectionTexture0)
					{
						UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture0);
					}
					this.m_ReflectionTexture0 = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_ReflectionTexture0.name = "__WaterReflection" + base.GetInstanceID();
					this.m_ReflectionTexture0.isPowerOfTwo = true;
					this.m_ReflectionTexture0.hideFlags = HideFlags.DontSave;
				}
				if (currentCamera.stereoEnabled && (!this.m_ReflectionTexture1 || this.m_OldReflectionTextureSize != this.textureSize))
				{
					if (this.m_ReflectionTexture1)
					{
						UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture1);
					}
					this.m_ReflectionTexture1 = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_ReflectionTexture1.isPowerOfTwo = true;
					this.m_ReflectionTexture1.hideFlags = HideFlags.DontSave;
				}
				this.m_OldReflectionTextureSize = this.textureSize;
				this.m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
				if (!reflectionCamera)
				{
					GameObject gameObject = new GameObject(string.Concat(new object[]
					{
						"Water Refl Camera id",
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
			if (waterMode >= Water.WaterMode.Refractive)
			{
				if (!this.m_RefractionTexture || this.m_OldRefractionTextureSize != this.textureSize)
				{
					if (this.m_RefractionTexture)
					{
						UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
					}
					this.m_RefractionTexture = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_RefractionTexture.name = "__WaterRefraction" + base.GetInstanceID();
					this.m_RefractionTexture.isPowerOfTwo = true;
					this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
					this.m_OldRefractionTextureSize = this.textureSize;
				}
				this.m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
				if (!refractionCamera)
				{
					GameObject gameObject2 = new GameObject(string.Concat(new object[]
					{
						"Water Refr Camera id",
						base.GetInstanceID(),
						" for ",
						currentCamera.GetInstanceID()
					}), new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					});
					refractionCamera = gameObject2.GetComponent<Camera>();
					refractionCamera.enabled = false;
					refractionCamera.transform.position = base.transform.position;
					refractionCamera.transform.rotation = base.transform.rotation;
					refractionCamera.gameObject.AddComponent<FlareLayer>();
					gameObject2.hideFlags = HideFlags.HideAndDontSave;
					this.m_RefractionCameras[currentCamera] = refractionCamera;
				}
			}
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000CE729 File Offset: 0x000CCB29
		private Water.WaterMode GetWaterMode()
		{
			if (this.m_HardwareWaterSupport < this.waterMode)
			{
				return this.m_HardwareWaterSupport;
			}
			return this.waterMode;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000CE74C File Offset: 0x000CCB4C
		private Water.WaterMode FindHardwareWaterSupport()
		{
			if (!base.GetComponent<Renderer>())
			{
				return Water.WaterMode.Simple;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return Water.WaterMode.Simple;
			}
			string tag = sharedMaterial.GetTag("WATERMODE", false);
			if (tag == "Refractive")
			{
				return Water.WaterMode.Refractive;
			}
			if (tag == "Reflective")
			{
				return Water.WaterMode.Reflective;
			}
			return Water.WaterMode.Simple;
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x000CE7B8 File Offset: 0x000CCBB8
		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 point = pos + normal * this.clipPlaneOffset;
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
			Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000CE824 File Offset: 0x000CCC24
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

		// Token: 0x04001DFB RID: 7675
		public Water.WaterMode waterMode = Water.WaterMode.Refractive;

		// Token: 0x04001DFC RID: 7676
		public bool disablePixelLights = true;

		// Token: 0x04001DFD RID: 7677
		public int textureSize = 256;

		// Token: 0x04001DFE RID: 7678
		public float clipPlaneOffset = 0.07f;

		// Token: 0x04001DFF RID: 7679
		public LayerMask reflectLayers = -1;

		// Token: 0x04001E00 RID: 7680
		public LayerMask refractLayers = -1;

		// Token: 0x04001E01 RID: 7681
		private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x04001E02 RID: 7682
		private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x04001E03 RID: 7683
		private RenderTexture m_ReflectionTexture0;

		// Token: 0x04001E04 RID: 7684
		private RenderTexture m_ReflectionTexture1;

		// Token: 0x04001E05 RID: 7685
		private RenderTexture m_RefractionTexture;

		// Token: 0x04001E06 RID: 7686
		private Water.WaterMode m_HardwareWaterSupport = Water.WaterMode.Refractive;

		// Token: 0x04001E07 RID: 7687
		private int m_OldReflectionTextureSize;

		// Token: 0x04001E08 RID: 7688
		private int m_OldRefractionTextureSize;

		// Token: 0x04001E09 RID: 7689
		private static bool s_InsideWater;

		// Token: 0x02000584 RID: 1412
		public enum WaterMode
		{
			// Token: 0x04001E0B RID: 7691
			Simple,
			// Token: 0x04001E0C RID: 7692
			Reflective,
			// Token: 0x04001E0D RID: 7693
			Refractive
		}
	}
}
