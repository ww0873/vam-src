using System;
using UnityEngine;

// Token: 0x02000900 RID: 2304
internal static class OVRMixedReality
{
	// Token: 0x06003A04 RID: 14852 RVA: 0x0011B24C File Offset: 0x0011964C
	public static void Update(GameObject parentObject, Camera mainCamera, OVRManager.CompositionMethod compositionMethod, bool useDynamicLighting, OVRManager.CameraDevice cameraDevice, OVRManager.DepthQuality depthQuality)
	{
		if (!OVRPlugin.initialized)
		{
			Debug.LogError("OVRPlugin not initialized");
			return;
		}
		if (!OVRPlugin.IsMixedRealityInitialized())
		{
			OVRPlugin.InitializeMixedReality();
		}
		if (!OVRPlugin.IsMixedRealityInitialized())
		{
			Debug.LogError("Unable to initialize MixedReality");
			return;
		}
		OVRPlugin.UpdateExternalCamera();
		OVRPlugin.UpdateCameraDevices();
		if (OVRMixedReality.currentComposition != null && OVRMixedReality.currentComposition.CompositionMethod() != compositionMethod)
		{
			OVRMixedReality.currentComposition.Cleanup();
			OVRMixedReality.currentComposition = null;
		}
		if (compositionMethod == OVRManager.CompositionMethod.External)
		{
			if (OVRMixedReality.currentComposition == null)
			{
				OVRMixedReality.currentComposition = new OVRExternalComposition(parentObject, mainCamera);
			}
		}
		else if (compositionMethod == OVRManager.CompositionMethod.Direct)
		{
			if (OVRMixedReality.currentComposition == null)
			{
				OVRMixedReality.currentComposition = new OVRDirectComposition(parentObject, mainCamera, cameraDevice, useDynamicLighting, depthQuality);
			}
		}
		else
		{
			if (compositionMethod != OVRManager.CompositionMethod.Sandwich)
			{
				Debug.LogError("Unknown CompositionMethod : " + compositionMethod);
				return;
			}
			if (OVRMixedReality.currentComposition == null)
			{
				OVRMixedReality.currentComposition = new OVRSandwichComposition(parentObject, mainCamera, cameraDevice, useDynamicLighting, depthQuality);
			}
		}
		OVRMixedReality.currentComposition.Update(mainCamera);
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x0011B359 File Offset: 0x00119759
	public static void Cleanup()
	{
		if (OVRMixedReality.currentComposition != null)
		{
			OVRMixedReality.currentComposition.Cleanup();
			OVRMixedReality.currentComposition = null;
		}
		if (OVRPlugin.IsMixedRealityInitialized())
		{
			OVRPlugin.ShutdownMixedReality();
		}
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x0011B385 File Offset: 0x00119785
	public static void RecenterPose()
	{
		if (OVRMixedReality.currentComposition != null)
		{
			OVRMixedReality.currentComposition.RecenterPose();
		}
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x0011B39C File Offset: 0x0011979C
	// Note: this type is marked as 'beforefieldinit'.
	static OVRMixedReality()
	{
	}

	// Token: 0x04002B89 RID: 11145
	public static Color chromaKeyColor = Color.green;

	// Token: 0x04002B8A RID: 11146
	public static bool useFakeExternalCamera = false;

	// Token: 0x04002B8B RID: 11147
	public static Vector3 fakeCameraPositon = new Vector3(3f, 0f, 3f);

	// Token: 0x04002B8C RID: 11148
	public static Quaternion fakeCameraRotation = Quaternion.LookRotation((new Vector3(0f, 1f, 0f) - OVRMixedReality.fakeCameraPositon).normalized, Vector3.up);

	// Token: 0x04002B8D RID: 11149
	public static float fakeCameraFov = 60f;

	// Token: 0x04002B8E RID: 11150
	public static float fakeCameraAspect = 1.7777778f;

	// Token: 0x04002B8F RID: 11151
	public static OVRComposition currentComposition = null;
}
