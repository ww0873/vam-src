using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008BC RID: 2236
public abstract class OVRCameraComposition : OVRComposition
{
	// Token: 0x06003846 RID: 14406 RVA: 0x00111250 File Offset: 0x0010F650
	protected OVRCameraComposition(OVRManager.CameraDevice inCameraDevice, bool inUseDynamicLighting, OVRManager.DepthQuality depthQuality)
	{
		this.cameraDevice = OVRCompositionUtil.ConvertCameraDevice(inCameraDevice);
		this.hasCameraDeviceOpened = false;
		this.useDynamicLighting = inUseDynamicLighting;
		bool flag = OVRPlugin.DoesCameraDeviceSupportDepth(this.cameraDevice);
		if (this.useDynamicLighting && !flag)
		{
			Debug.LogWarning("The camera device doesn't support depth. The result of dynamic lighting might not be correct");
		}
		if (OVRPlugin.IsCameraDeviceAvailable(this.cameraDevice))
		{
			OVRPlugin.CameraExtrinsics cameraExtrinsics;
			OVRPlugin.CameraIntrinsics cameraIntrinsics;
			if (OVRPlugin.GetExternalCameraCount() > 0 && OVRPlugin.GetMixedRealityCameraInfo(0, out cameraExtrinsics, out cameraIntrinsics))
			{
				OVRPlugin.SetCameraDevicePreferredColorFrameSize(this.cameraDevice, cameraIntrinsics.ImageSensorPixelResolution.w, cameraIntrinsics.ImageSensorPixelResolution.h);
			}
			if (this.useDynamicLighting)
			{
				OVRPlugin.SetCameraDeviceDepthSensingMode(this.cameraDevice, OVRPlugin.CameraDeviceDepthSensingMode.Fill);
				OVRPlugin.CameraDeviceDepthQuality depthQuality2 = OVRPlugin.CameraDeviceDepthQuality.Medium;
				if (depthQuality == OVRManager.DepthQuality.Low)
				{
					depthQuality2 = OVRPlugin.CameraDeviceDepthQuality.Low;
				}
				else if (depthQuality == OVRManager.DepthQuality.Medium)
				{
					depthQuality2 = OVRPlugin.CameraDeviceDepthQuality.Medium;
				}
				else if (depthQuality == OVRManager.DepthQuality.High)
				{
					depthQuality2 = OVRPlugin.CameraDeviceDepthQuality.High;
				}
				else
				{
					Debug.LogWarning("Unknown depth quality");
				}
				OVRPlugin.SetCameraDevicePreferredDepthQuality(this.cameraDevice, depthQuality2);
			}
			OVRPlugin.OpenCameraDevice(this.cameraDevice);
			if (OVRPlugin.HasCameraDeviceOpened(this.cameraDevice))
			{
				this.hasCameraDeviceOpened = true;
			}
		}
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x00111377 File Offset: 0x0010F777
	public override void Cleanup()
	{
		OVRCompositionUtil.SafeDestroy(ref this.cameraFramePlaneObject);
		if (this.hasCameraDeviceOpened)
		{
			OVRPlugin.CloseCameraDevice(this.cameraDevice);
		}
	}

	// Token: 0x06003848 RID: 14408 RVA: 0x0011139B File Offset: 0x0010F79B
	public override void RecenterPose()
	{
		this.boundaryMesh = null;
	}

	// Token: 0x06003849 RID: 14409 RVA: 0x001113A4 File Offset: 0x0010F7A4
	protected void CreateCameraFramePlaneObject(GameObject parentObject, Camera mixedRealityCamera, bool useDynamicLighting)
	{
		this.cameraFramePlaneObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		this.cameraFramePlaneObject.name = "MRCameraFrame";
		this.cameraFramePlaneObject.transform.parent = parentObject.transform;
		this.cameraFramePlaneObject.GetComponent<Collider>().enabled = false;
		this.cameraFramePlaneObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
		Material material = new Material(Shader.Find((!useDynamicLighting) ? "Oculus/OVRMRCameraFrame" : "Oculus/OVRMRCameraFrameLit"));
		this.cameraFramePlaneObject.GetComponent<MeshRenderer>().material = material;
		material.SetColor("_Color", Color.white);
		material.SetFloat("_Visible", 0f);
		this.cameraFramePlaneObject.transform.localScale = new Vector3(4f, 4f, 4f);
		this.cameraFramePlaneObject.SetActive(true);
		OVRCameraComposition.OVRCameraFrameCompositionManager ovrcameraFrameCompositionManager = mixedRealityCamera.gameObject.AddComponent<OVRCameraComposition.OVRCameraFrameCompositionManager>();
		ovrcameraFrameCompositionManager.cameraFrameGameObj = this.cameraFramePlaneObject;
		ovrcameraFrameCompositionManager.composition = this;
	}

	// Token: 0x0600384A RID: 14410 RVA: 0x001114A8 File Offset: 0x0010F8A8
	protected void UpdateCameraFramePlaneObject(Camera mainCamera, Camera mixedRealityCamera, RenderTexture boundaryMeshMaskTexture)
	{
		bool flag = false;
		Material material = this.cameraFramePlaneObject.GetComponent<MeshRenderer>().material;
		Texture2D texture2D = Texture2D.blackTexture;
		Texture2D value = Texture2D.whiteTexture;
		if (OVRPlugin.IsCameraDeviceColorFrameAvailable(this.cameraDevice))
		{
			texture2D = OVRPlugin.GetCameraDeviceColorFrameTexture(this.cameraDevice);
		}
		else
		{
			Debug.LogWarning("Camera: color frame not ready");
			flag = true;
		}
		bool flag2 = OVRPlugin.DoesCameraDeviceSupportDepth(this.cameraDevice);
		if (this.useDynamicLighting && flag2)
		{
			if (OVRPlugin.IsCameraDeviceDepthFrameAvailable(this.cameraDevice))
			{
				value = OVRPlugin.GetCameraDeviceDepthFrameTexture(this.cameraDevice);
			}
			else
			{
				Debug.LogWarning("Camera: depth frame not ready");
				flag = true;
			}
		}
		if (!flag)
		{
			Vector3 rhs = mainCamera.transform.position - mixedRealityCamera.transform.position;
			float num = Vector3.Dot(mixedRealityCamera.transform.forward, rhs);
			this.cameraFramePlaneDistance = num;
			this.cameraFramePlaneObject.transform.position = mixedRealityCamera.transform.position + mixedRealityCamera.transform.forward * num;
			this.cameraFramePlaneObject.transform.rotation = mixedRealityCamera.transform.rotation;
			float num2 = Mathf.Tan(mixedRealityCamera.fieldOfView * 0.017453292f * 0.5f);
			this.cameraFramePlaneObject.transform.localScale = new Vector3(num * mixedRealityCamera.aspect * num2 * 2f, num * num2 * 2f, 1f);
			float num3 = num * num2 * 2f;
			float x = num3 * mixedRealityCamera.aspect;
			float maxValue = float.MaxValue;
			this.cameraRig = null;
			if (OVRManager.instance.virtualGreenScreenType != OVRManager.VirtualGreenScreenType.Off)
			{
				this.cameraRig = mainCamera.GetComponentInParent<OVRCameraRig>();
				if (this.cameraRig != null && this.cameraRig.centerEyeAnchor == null)
				{
					this.cameraRig = null;
				}
				this.RefreshBoundaryMesh(mixedRealityCamera, out maxValue);
			}
			material.mainTexture = texture2D;
			material.SetTexture("_DepthTex", value);
			material.SetVector("_FlipParams", new Vector4((!OVRManager.instance.flipCameraFrameHorizontally) ? 0f : 1f, (!OVRManager.instance.flipCameraFrameVertically) ? 0f : 1f, 0f, 0f));
			material.SetColor("_ChromaKeyColor", OVRManager.instance.chromaKeyColor);
			material.SetFloat("_ChromaKeySimilarity", OVRManager.instance.chromaKeySimilarity);
			material.SetFloat("_ChromaKeySmoothRange", OVRManager.instance.chromaKeySmoothRange);
			material.SetFloat("_ChromaKeySpillRange", OVRManager.instance.chromaKeySpillRange);
			material.SetVector("_TextureDimension", new Vector4((float)texture2D.width, (float)texture2D.height, 1f / (float)texture2D.width, 1f / (float)texture2D.height));
			material.SetVector("_TextureWorldSize", new Vector4(x, num3, 0f, 0f));
			material.SetFloat("_SmoothFactor", OVRManager.instance.dynamicLightingSmoothFactor);
			material.SetFloat("_DepthVariationClamp", OVRManager.instance.dynamicLightingDepthVariationClampingValue);
			material.SetFloat("_CullingDistance", maxValue);
			if (OVRManager.instance.virtualGreenScreenType == OVRManager.VirtualGreenScreenType.Off || this.boundaryMesh == null || boundaryMeshMaskTexture == null)
			{
				material.SetTexture("_MaskTex", Texture2D.whiteTexture);
			}
			else if (this.cameraRig == null)
			{
				if (!this.nullcameraRigWarningDisplayed)
				{
					Debug.LogWarning("Could not find the OVRCameraRig/CenterEyeAnchor object. Please check if the OVRCameraRig has been setup properly. The virtual green screen has been temporarily disabled");
					this.nullcameraRigWarningDisplayed = true;
				}
				material.SetTexture("_MaskTex", Texture2D.whiteTexture);
			}
			else
			{
				if (this.nullcameraRigWarningDisplayed)
				{
					Debug.Log("OVRCameraRig/CenterEyeAnchor object found. Virtual green screen is activated");
					this.nullcameraRigWarningDisplayed = false;
				}
				material.SetTexture("_MaskTex", boundaryMeshMaskTexture);
			}
		}
	}

	// Token: 0x0600384B RID: 14411 RVA: 0x00111898 File Offset: 0x0010FC98
	protected void RefreshBoundaryMesh(Camera camera, out float cullingDistance)
	{
		float num = (!OVRManager.instance.virtualGreenScreenApplyDepthCulling) ? float.PositiveInfinity : OVRManager.instance.virtualGreenScreenDepthTolerance;
		cullingDistance = OVRCompositionUtil.GetMaximumBoundaryDistance(camera, OVRCompositionUtil.ToBoundaryType(OVRManager.instance.virtualGreenScreenType)) + num;
		if (this.boundaryMesh == null || this.boundaryMeshType != OVRManager.instance.virtualGreenScreenType || this.boundaryMeshTopY != OVRManager.instance.virtualGreenScreenTopY || this.boundaryMeshBottomY != OVRManager.instance.virtualGreenScreenBottomY)
		{
			this.boundaryMeshTopY = OVRManager.instance.virtualGreenScreenTopY;
			this.boundaryMeshBottomY = OVRManager.instance.virtualGreenScreenBottomY;
			this.boundaryMesh = OVRCompositionUtil.BuildBoundaryMesh(OVRCompositionUtil.ToBoundaryType(OVRManager.instance.virtualGreenScreenType), this.boundaryMeshTopY, this.boundaryMeshBottomY);
			this.boundaryMeshType = OVRManager.instance.virtualGreenScreenType;
		}
	}

	// Token: 0x04002991 RID: 10641
	protected GameObject cameraFramePlaneObject;

	// Token: 0x04002992 RID: 10642
	protected float cameraFramePlaneDistance;

	// Token: 0x04002993 RID: 10643
	protected readonly bool hasCameraDeviceOpened;

	// Token: 0x04002994 RID: 10644
	protected readonly bool useDynamicLighting;

	// Token: 0x04002995 RID: 10645
	internal readonly OVRPlugin.CameraDevice cameraDevice = OVRPlugin.CameraDevice.WebCamera0;

	// Token: 0x04002996 RID: 10646
	private OVRCameraRig cameraRig;

	// Token: 0x04002997 RID: 10647
	private Mesh boundaryMesh;

	// Token: 0x04002998 RID: 10648
	private float boundaryMeshTopY;

	// Token: 0x04002999 RID: 10649
	private float boundaryMeshBottomY;

	// Token: 0x0400299A RID: 10650
	private OVRManager.VirtualGreenScreenType boundaryMeshType;

	// Token: 0x0400299B RID: 10651
	private bool nullcameraRigWarningDisplayed;

	// Token: 0x020008BD RID: 2237
	public class OVRCameraFrameCompositionManager : MonoBehaviour
	{
		// Token: 0x0600384C RID: 14412 RVA: 0x00111988 File Offset: 0x0010FD88
		public OVRCameraFrameCompositionManager()
		{
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x00111990 File Offset: 0x0010FD90
		private void Start()
		{
			Shader shader = Shader.Find("Oculus/Unlit");
			if (!shader)
			{
				Debug.LogError("Oculus/Unlit shader does not exist");
				return;
			}
			this.whiteMaterial = new Material(shader);
			this.whiteMaterial.color = Color.white;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x001119DC File Offset: 0x0010FDDC
		private void OnPreRender()
		{
			if (OVRManager.instance.virtualGreenScreenType != OVRManager.VirtualGreenScreenType.Off && this.boundaryMeshMaskTexture != null && this.composition.boundaryMesh != null)
			{
				RenderTexture active = RenderTexture.active;
				RenderTexture.active = this.boundaryMeshMaskTexture;
				GL.PushMatrix();
				GL.LoadProjectionMatrix(base.GetComponent<Camera>().projectionMatrix);
				GL.Clear(false, true, Color.black);
				for (int i = 0; i < this.whiteMaterial.passCount; i++)
				{
					if (this.whiteMaterial.SetPass(i))
					{
						Graphics.DrawMeshNow(this.composition.boundaryMesh, this.composition.cameraRig.ComputeTrackReferenceMatrix());
					}
				}
				GL.PopMatrix();
				RenderTexture.active = active;
			}
			if (this.cameraFrameGameObj)
			{
				if (this.cameraFrameMaterial == null)
				{
					this.cameraFrameMaterial = this.cameraFrameGameObj.GetComponent<MeshRenderer>().material;
				}
				this.cameraFrameMaterial.SetFloat("_Visible", 1f);
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x00111AF5 File Offset: 0x0010FEF5
		private void OnPostRender()
		{
			if (this.cameraFrameGameObj)
			{
				this.cameraFrameMaterial.SetFloat("_Visible", 0f);
			}
		}

		// Token: 0x0400299C RID: 10652
		public GameObject cameraFrameGameObj;

		// Token: 0x0400299D RID: 10653
		public OVRCameraComposition composition;

		// Token: 0x0400299E RID: 10654
		public RenderTexture boundaryMeshMaskTexture;

		// Token: 0x0400299F RID: 10655
		private Material cameraFrameMaterial;

		// Token: 0x040029A0 RID: 10656
		private Material whiteMaterial;
	}
}
