using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020008C3 RID: 2243
public class OVRSandwichComposition : OVRCameraComposition
{
	// Token: 0x06003869 RID: 14441 RVA: 0x00112818 File Offset: 0x00110C18
	public OVRSandwichComposition(GameObject parentObject, Camera mainCamera, OVRManager.CameraDevice cameraDevice, bool useDynamicLighting, OVRManager.DepthQuality depthQuality) : base(cameraDevice, useDynamicLighting, depthQuality)
	{
		this.frameRealtime = Time.realtimeSinceStartup;
		this.historyRecordCount = OVRManager.instance.sandwichCompositionBufferedFrames;
		if (this.historyRecordCount < 1)
		{
			Debug.LogWarning("Invalid sandwichCompositionBufferedFrames in OVRManager. It should be at least 1");
			this.historyRecordCount = 1;
		}
		if (this.historyRecordCount > 16)
		{
			Debug.LogWarning("The value of sandwichCompositionBufferedFrames in OVRManager is too big. It would consume a lot of memory. It has been override to 16");
			this.historyRecordCount = 16;
		}
		this.historyRecordArray = new OVRSandwichComposition.HistoryRecord[this.historyRecordCount];
		for (int i = 0; i < this.historyRecordCount; i++)
		{
			this.historyRecordArray[i] = new OVRSandwichComposition.HistoryRecord();
		}
		this.historyRecordCursorIndex = 0;
		this.fgCamera = new GameObject("MRSandwichForegroundCamera")
		{
			transform = 
			{
				parent = parentObject.transform
			}
		}.AddComponent<Camera>();
		this.fgCamera.depth = 200f;
		this.fgCamera.clearFlags = CameraClearFlags.Color;
		this.fgCamera.backgroundColor = Color.clear;
		this.fgCamera.cullingMask = (mainCamera.cullingMask & ~OVRManager.instance.extraHiddenLayers);
		this.fgCamera.nearClipPlane = mainCamera.nearClipPlane;
		this.fgCamera.farClipPlane = mainCamera.farClipPlane;
		this.bgCamera = new GameObject("MRSandwichBackgroundCamera")
		{
			transform = 
			{
				parent = parentObject.transform
			}
		}.AddComponent<Camera>();
		this.bgCamera.depth = 100f;
		this.bgCamera.clearFlags = mainCamera.clearFlags;
		this.bgCamera.backgroundColor = mainCamera.backgroundColor;
		this.bgCamera.cullingMask = (mainCamera.cullingMask & ~OVRManager.instance.extraHiddenLayers);
		this.bgCamera.nearClipPlane = mainCamera.nearClipPlane;
		this.bgCamera.farClipPlane = mainCamera.farClipPlane;
		this.cameraProxyPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		this.cameraProxyPlane.name = "MRProxyClipPlane";
		this.cameraProxyPlane.transform.parent = parentObject.transform;
		this.cameraProxyPlane.GetComponent<Collider>().enabled = false;
		this.cameraProxyPlane.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
		Material material = new Material(Shader.Find("Oculus/OVRMRClipPlane"));
		this.cameraProxyPlane.GetComponent<MeshRenderer>().material = material;
		material.SetColor("_Color", Color.clear);
		material.SetFloat("_Visible", 0f);
		this.cameraProxyPlane.transform.localScale = new Vector3(1000f, 1000f, 1000f);
		this.cameraProxyPlane.SetActive(true);
		OVRMRForegroundCameraManager ovrmrforegroundCameraManager = this.fgCamera.gameObject.AddComponent<OVRMRForegroundCameraManager>();
		ovrmrforegroundCameraManager.clipPlaneGameObj = this.cameraProxyPlane;
		this.compositionCamera = new GameObject("MRSandwichCaptureCamera")
		{
			transform = 
			{
				parent = parentObject.transform
			}
		}.AddComponent<Camera>();
		this.compositionCamera.stereoTargetEye = StereoTargetEyeMask.None;
		this.compositionCamera.depth = float.MaxValue;
		this.compositionCamera.rect = new Rect(0f, 0f, 1f, 1f);
		this.compositionCamera.clearFlags = CameraClearFlags.Depth;
		this.compositionCamera.backgroundColor = mainCamera.backgroundColor;
		this.compositionCamera.cullingMask = 1 << this.cameraFramePlaneLayer;
		this.compositionCamera.nearClipPlane = mainCamera.nearClipPlane;
		this.compositionCamera.farClipPlane = mainCamera.farClipPlane;
		if (!this.hasCameraDeviceOpened)
		{
			Debug.LogError("Unable to open camera device " + cameraDevice);
		}
		else
		{
			Debug.Log("SandwichComposition activated : useDynamicLighting " + ((!useDynamicLighting) ? "OFF" : "ON"));
			base.CreateCameraFramePlaneObject(parentObject, this.compositionCamera, useDynamicLighting);
			this.cameraFramePlaneObject.layer = this.cameraFramePlaneLayer;
			this.RefreshRenderTextures(mainCamera);
			this.compositionManager = this.compositionCamera.gameObject.AddComponent<OVRSandwichComposition.OVRSandwichCompositionManager>();
			this.compositionManager.fgTexture = this.historyRecordArray[this.historyRecordCursorIndex].fgRenderTexture;
			this.compositionManager.bgTexture = this.historyRecordArray[this.historyRecordCursorIndex].bgRenderTexture;
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x0600386A RID: 14442 RVA: 0x00112C6C File Offset: 0x0011106C
	public int cameraFramePlaneLayer
	{
		get
		{
			if (this._cameraFramePlaneLayer < 0)
			{
				for (int i = 24; i <= 29; i++)
				{
					string text = LayerMask.LayerToName(i);
					if (text == null || text.Length == 0)
					{
						this._cameraFramePlaneLayer = i;
						break;
					}
				}
				if (this._cameraFramePlaneLayer == -1)
				{
					Debug.LogWarning("Unable to find an unnamed layer between 24 and 29.");
					this._cameraFramePlaneLayer = 25;
				}
				Debug.LogFormat("Set the CameraFramePlaneLayer in SandwichComposition to {0}. Please do NOT put any other gameobject in this layer.", new object[]
				{
					this._cameraFramePlaneLayer
				});
			}
			return this._cameraFramePlaneLayer;
		}
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x00112CFF File Offset: 0x001110FF
	public override OVRManager.CompositionMethod CompositionMethod()
	{
		return OVRManager.CompositionMethod.Sandwich;
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x00112D04 File Offset: 0x00111104
	public override void Update(Camera mainCamera)
	{
		if (!this.hasCameraDeviceOpened)
		{
			return;
		}
		this.frameRealtime = Time.realtimeSinceStartup;
		this.historyRecordCursorIndex++;
		if (this.historyRecordCursorIndex >= this.historyRecordCount)
		{
			this.historyRecordCursorIndex = 0;
		}
		if (!OVRPlugin.SetHandNodePoseStateLatency((double)OVRManager.instance.handPoseStateLatency))
		{
			Debug.LogWarning("HandPoseStateLatency is invalid. Expect a value between 0.0 to 0.5, get " + OVRManager.instance.handPoseStateLatency);
		}
		this.RefreshRenderTextures(mainCamera);
		this.bgCamera.clearFlags = mainCamera.clearFlags;
		this.bgCamera.backgroundColor = mainCamera.backgroundColor;
		this.bgCamera.cullingMask = (mainCamera.cullingMask & ~OVRManager.instance.extraHiddenLayers);
		this.fgCamera.cullingMask = (mainCamera.cullingMask & ~OVRManager.instance.extraHiddenLayers);
		OVRPlugin.CameraExtrinsics extrinsics;
		OVRPlugin.CameraIntrinsics cameraIntrinsics;
		if (OVRMixedReality.useFakeExternalCamera || OVRPlugin.GetExternalCameraCount() == 0)
		{
			OVRPose pose = default(OVRPose);
			pose = OVRExtensions.ToWorldSpacePose(new OVRPose
			{
				position = OVRMixedReality.fakeCameraPositon,
				orientation = OVRMixedReality.fakeCameraRotation
			});
			this.RefreshCameraPoses(OVRMixedReality.fakeCameraFov, OVRMixedReality.fakeCameraAspect, pose);
		}
		else if (OVRPlugin.GetMixedRealityCameraInfo(0, out extrinsics, out cameraIntrinsics))
		{
			OVRPose pose2 = base.ComputeCameraWorldSpacePose(extrinsics);
			float fovY = Mathf.Atan(cameraIntrinsics.FOVPort.UpTan) * 57.29578f * 2f;
			float aspect = cameraIntrinsics.FOVPort.LeftTan / cameraIntrinsics.FOVPort.UpTan;
			this.RefreshCameraPoses(fovY, aspect, pose2);
		}
		else
		{
			Debug.LogWarning("Failed to get external camera information");
		}
		this.compositionCamera.GetComponent<OVRCameraComposition.OVRCameraFrameCompositionManager>().boundaryMeshMaskTexture = this.historyRecordArray[this.historyRecordCursorIndex].boundaryMeshMaskTexture;
		OVRSandwichComposition.HistoryRecord historyRecordForComposition = this.GetHistoryRecordForComposition();
		base.UpdateCameraFramePlaneObject(mainCamera, this.compositionCamera, historyRecordForComposition.boundaryMeshMaskTexture);
		OVRSandwichComposition.OVRSandwichCompositionManager component = this.compositionCamera.gameObject.GetComponent<OVRSandwichComposition.OVRSandwichCompositionManager>();
		component.fgTexture = historyRecordForComposition.fgRenderTexture;
		component.bgTexture = historyRecordForComposition.bgRenderTexture;
		this.cameraProxyPlane.transform.position = this.fgCamera.transform.position + this.fgCamera.transform.forward * this.cameraFramePlaneDistance;
		this.cameraProxyPlane.transform.LookAt(this.cameraProxyPlane.transform.position + this.fgCamera.transform.forward);
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x00112F98 File Offset: 0x00111398
	public override void Cleanup()
	{
		base.Cleanup();
		Camera[] array = new Camera[]
		{
			this.fgCamera,
			this.bgCamera,
			this.compositionCamera
		};
		foreach (Camera camera in array)
		{
			OVRCompositionUtil.SafeDestroy(camera.gameObject);
		}
		this.fgCamera = null;
		this.bgCamera = null;
		this.compositionCamera = null;
		Debug.Log("SandwichComposition deactivated");
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x00113011 File Offset: 0x00111411
	private RenderTextureFormat DesiredRenderTextureFormat(RenderTextureFormat originalFormat)
	{
		if (originalFormat == RenderTextureFormat.RGB565)
		{
			return RenderTextureFormat.ARGB1555;
		}
		if (originalFormat == RenderTextureFormat.RGB111110Float)
		{
			return RenderTextureFormat.ARGBHalf;
		}
		return originalFormat;
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x00113028 File Offset: 0x00111428
	protected void RefreshRenderTextures(Camera mainCamera)
	{
		int width = Screen.width;
		int height = Screen.height;
		RenderTextureFormat renderTextureFormat = (!mainCamera.targetTexture) ? RenderTextureFormat.ARGB32 : this.DesiredRenderTextureFormat(mainCamera.targetTexture.format);
		int num = (!mainCamera.targetTexture) ? 24 : mainCamera.targetTexture.depth;
		OVRSandwichComposition.HistoryRecord historyRecord = this.historyRecordArray[this.historyRecordCursorIndex];
		historyRecord.timestamp = this.frameRealtime;
		if (historyRecord.fgRenderTexture == null || historyRecord.fgRenderTexture.width != width || historyRecord.fgRenderTexture.height != height || historyRecord.fgRenderTexture.format != renderTextureFormat || historyRecord.fgRenderTexture.depth != num)
		{
			historyRecord.fgRenderTexture = new RenderTexture(width, height, num, renderTextureFormat);
			historyRecord.fgRenderTexture.name = "Sandwich FG " + this.historyRecordCursorIndex.ToString();
		}
		this.fgCamera.targetTexture = historyRecord.fgRenderTexture;
		if (historyRecord.bgRenderTexture == null || historyRecord.bgRenderTexture.width != width || historyRecord.bgRenderTexture.height != height || historyRecord.bgRenderTexture.format != renderTextureFormat || historyRecord.bgRenderTexture.depth != num)
		{
			historyRecord.bgRenderTexture = new RenderTexture(width, height, num, renderTextureFormat);
			historyRecord.bgRenderTexture.name = "Sandwich BG " + this.historyRecordCursorIndex.ToString();
		}
		this.bgCamera.targetTexture = historyRecord.bgRenderTexture;
		if (OVRManager.instance.virtualGreenScreenType != OVRManager.VirtualGreenScreenType.Off)
		{
			if (historyRecord.boundaryMeshMaskTexture == null || historyRecord.boundaryMeshMaskTexture.width != width || historyRecord.boundaryMeshMaskTexture.height != height)
			{
				historyRecord.boundaryMeshMaskTexture = new RenderTexture(width, height, 0, RenderTextureFormat.R8);
				historyRecord.boundaryMeshMaskTexture.name = "Boundary Mask " + this.historyRecordCursorIndex.ToString();
				historyRecord.boundaryMeshMaskTexture.Create();
			}
		}
		else
		{
			historyRecord.boundaryMeshMaskTexture = null;
		}
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x00113288 File Offset: 0x00111688
	protected OVRSandwichComposition.HistoryRecord GetHistoryRecordForComposition()
	{
		float num = this.frameRealtime - OVRManager.instance.sandwichCompositionRenderLatency;
		int num2 = this.historyRecordCursorIndex;
		int num3 = num2 - 1;
		if (num3 < 0)
		{
			num3 = this.historyRecordCount - 1;
		}
		while (num3 != this.historyRecordCursorIndex)
		{
			if (this.historyRecordArray[num3].timestamp <= num)
			{
				float num4 = this.historyRecordArray[num2].timestamp - num;
				float num5 = num - this.historyRecordArray[num3].timestamp;
				return (num4 > num5) ? this.historyRecordArray[num3] : this.historyRecordArray[num2];
			}
			num2 = num3;
			num3 = num2 - 1;
			if (num3 < 0)
			{
				num3 = this.historyRecordCount - 1;
			}
		}
		return this.historyRecordArray[num2];
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x00113344 File Offset: 0x00111744
	protected void RefreshCameraPoses(float fovY, float aspect, OVRPose pose)
	{
		Camera[] array = new Camera[]
		{
			this.fgCamera,
			this.bgCamera,
			this.compositionCamera
		};
		foreach (Camera camera in array)
		{
			camera.fieldOfView = fovY;
			camera.aspect = aspect;
			camera.transform.FromOVRPose(pose, false);
		}
	}

	// Token: 0x040029AD RID: 10669
	public float frameRealtime;

	// Token: 0x040029AE RID: 10670
	public Camera fgCamera;

	// Token: 0x040029AF RID: 10671
	public Camera bgCamera;

	// Token: 0x040029B0 RID: 10672
	public readonly int historyRecordCount = 8;

	// Token: 0x040029B1 RID: 10673
	public readonly OVRSandwichComposition.HistoryRecord[] historyRecordArray;

	// Token: 0x040029B2 RID: 10674
	public int historyRecordCursorIndex;

	// Token: 0x040029B3 RID: 10675
	public GameObject cameraProxyPlane;

	// Token: 0x040029B4 RID: 10676
	public Camera compositionCamera;

	// Token: 0x040029B5 RID: 10677
	public OVRSandwichComposition.OVRSandwichCompositionManager compositionManager;

	// Token: 0x040029B6 RID: 10678
	private int _cameraFramePlaneLayer = -1;

	// Token: 0x020008C4 RID: 2244
	public class HistoryRecord
	{
		// Token: 0x06003872 RID: 14450 RVA: 0x001133A8 File Offset: 0x001117A8
		public HistoryRecord()
		{
		}

		// Token: 0x040029B7 RID: 10679
		public float timestamp = float.MinValue;

		// Token: 0x040029B8 RID: 10680
		public RenderTexture fgRenderTexture;

		// Token: 0x040029B9 RID: 10681
		public RenderTexture bgRenderTexture;

		// Token: 0x040029BA RID: 10682
		public RenderTexture boundaryMeshMaskTexture;
	}

	// Token: 0x020008C5 RID: 2245
	public class OVRSandwichCompositionManager : MonoBehaviour
	{
		// Token: 0x06003873 RID: 14451 RVA: 0x001133BB File Offset: 0x001117BB
		public OVRSandwichCompositionManager()
		{
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x001133C4 File Offset: 0x001117C4
		private void Start()
		{
			Shader shader = Shader.Find("Oculus/UnlitTransparent");
			if (shader == null)
			{
				Debug.LogError("Unable to create transparent shader");
				return;
			}
			this.alphaBlendMaterial = new Material(shader);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x00113400 File Offset: 0x00111800
		private void OnPreRender()
		{
			if (this.fgTexture == null || this.bgTexture == null || this.alphaBlendMaterial == null)
			{
				Debug.LogError("OVRSandwichCompositionManager has not setup properly");
				return;
			}
			Graphics.Blit(this.bgTexture, RenderTexture.active);
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x0011345C File Offset: 0x0011185C
		private void OnPostRender()
		{
			if (this.fgTexture == null || this.bgTexture == null || this.alphaBlendMaterial == null)
			{
				Debug.LogError("OVRSandwichCompositionManager has not setup properly");
				return;
			}
			Graphics.Blit(this.fgTexture, RenderTexture.active, this.alphaBlendMaterial);
		}

		// Token: 0x040029BB RID: 10683
		public RenderTexture fgTexture;

		// Token: 0x040029BC RID: 10684
		public RenderTexture bgTexture;

		// Token: 0x040029BD RID: 10685
		public Material alphaBlendMaterial;
	}
}
