using System;
using System.IO;
using UnityEngine;

// Token: 0x02000967 RID: 2407
public class OVRMixedRealityCaptureSettings : ScriptableObject
{
	// Token: 0x06003C1A RID: 15386 RVA: 0x00122A60 File Offset: 0x00120E60
	public OVRMixedRealityCaptureSettings()
	{
	}

	// Token: 0x06003C1B RID: 15387 RVA: 0x00122AD0 File Offset: 0x00120ED0
	public void ReadFrom(OVRManager manager)
	{
		this.enableMixedReality = manager.enableMixedReality;
		this.compositionMethod = manager.compositionMethod;
		this.extraHiddenLayers = manager.extraHiddenLayers;
		this.capturingCameraDevice = manager.capturingCameraDevice;
		this.flipCameraFrameHorizontally = manager.flipCameraFrameHorizontally;
		this.flipCameraFrameVertically = manager.flipCameraFrameVertically;
		this.handPoseStateLatency = manager.handPoseStateLatency;
		this.sandwichCompositionRenderLatency = manager.sandwichCompositionRenderLatency;
		this.sandwichCompositionBufferedFrames = manager.sandwichCompositionBufferedFrames;
		this.chromaKeyColor = manager.chromaKeyColor;
		this.chromaKeySimilarity = manager.chromaKeySimilarity;
		this.chromaKeySmoothRange = manager.chromaKeySmoothRange;
		this.chromaKeySpillRange = manager.chromaKeySpillRange;
		this.useDynamicLighting = manager.useDynamicLighting;
		this.depthQuality = manager.depthQuality;
		this.dynamicLightingSmoothFactor = manager.dynamicLightingSmoothFactor;
		this.dynamicLightingDepthVariationClampingValue = manager.dynamicLightingDepthVariationClampingValue;
		this.virtualGreenScreenType = manager.virtualGreenScreenType;
		this.virtualGreenScreenTopY = manager.virtualGreenScreenTopY;
		this.virtualGreenScreenBottomY = manager.virtualGreenScreenBottomY;
		this.virtualGreenScreenApplyDepthCulling = manager.virtualGreenScreenApplyDepthCulling;
		this.virtualGreenScreenDepthTolerance = manager.virtualGreenScreenDepthTolerance;
	}

	// Token: 0x06003C1C RID: 15388 RVA: 0x00122BE8 File Offset: 0x00120FE8
	public void ApplyTo(OVRManager manager)
	{
		manager.enableMixedReality = this.enableMixedReality;
		manager.compositionMethod = this.compositionMethod;
		manager.extraHiddenLayers = this.extraHiddenLayers;
		manager.capturingCameraDevice = this.capturingCameraDevice;
		manager.flipCameraFrameHorizontally = this.flipCameraFrameHorizontally;
		manager.flipCameraFrameVertically = this.flipCameraFrameVertically;
		manager.handPoseStateLatency = this.handPoseStateLatency;
		manager.sandwichCompositionRenderLatency = this.sandwichCompositionRenderLatency;
		manager.sandwichCompositionBufferedFrames = this.sandwichCompositionBufferedFrames;
		manager.chromaKeyColor = this.chromaKeyColor;
		manager.chromaKeySimilarity = this.chromaKeySimilarity;
		manager.chromaKeySmoothRange = this.chromaKeySmoothRange;
		manager.chromaKeySpillRange = this.chromaKeySpillRange;
		manager.useDynamicLighting = this.useDynamicLighting;
		manager.depthQuality = this.depthQuality;
		manager.dynamicLightingSmoothFactor = this.dynamicLightingSmoothFactor;
		manager.dynamicLightingDepthVariationClampingValue = this.dynamicLightingDepthVariationClampingValue;
		manager.virtualGreenScreenType = this.virtualGreenScreenType;
		manager.virtualGreenScreenTopY = this.virtualGreenScreenTopY;
		manager.virtualGreenScreenBottomY = this.virtualGreenScreenBottomY;
		manager.virtualGreenScreenApplyDepthCulling = this.virtualGreenScreenApplyDepthCulling;
		manager.virtualGreenScreenDepthTolerance = this.virtualGreenScreenDepthTolerance;
	}

	// Token: 0x06003C1D RID: 15389 RVA: 0x00122D00 File Offset: 0x00121100
	public void WriteToConfigurationFile()
	{
		string contents = JsonUtility.ToJson(this, true);
		try
		{
			string text = Path.Combine(Application.dataPath, "mrc.config");
			Debug.Log("Write OVRMixedRealityCaptureSettings to " + text);
			File.WriteAllText(text, contents);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Exception caught " + ex.Message);
		}
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x00122D6C File Offset: 0x0012116C
	public void CombineWithConfigurationFile()
	{
		try
		{
			string text = Path.Combine(Application.dataPath, "mrc.config");
			if (File.Exists(text))
			{
				Debug.Log("MixedRealityCapture configuration file found at " + text);
				string json = File.ReadAllText(text);
				Debug.Log("Apply MixedRealityCapture configuration");
				JsonUtility.FromJsonOverwrite(json, this);
			}
			else
			{
				Debug.Log("MixedRealityCapture configuration file doesn't exist at " + text);
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Exception caught " + ex.Message);
		}
	}

	// Token: 0x04002DF8 RID: 11768
	public bool enableMixedReality;

	// Token: 0x04002DF9 RID: 11769
	public LayerMask extraHiddenLayers;

	// Token: 0x04002DFA RID: 11770
	public OVRManager.CompositionMethod compositionMethod;

	// Token: 0x04002DFB RID: 11771
	public OVRManager.CameraDevice capturingCameraDevice;

	// Token: 0x04002DFC RID: 11772
	public bool flipCameraFrameHorizontally;

	// Token: 0x04002DFD RID: 11773
	public bool flipCameraFrameVertically;

	// Token: 0x04002DFE RID: 11774
	public float handPoseStateLatency;

	// Token: 0x04002DFF RID: 11775
	public float sandwichCompositionRenderLatency;

	// Token: 0x04002E00 RID: 11776
	public int sandwichCompositionBufferedFrames = 8;

	// Token: 0x04002E01 RID: 11777
	public Color chromaKeyColor = Color.green;

	// Token: 0x04002E02 RID: 11778
	public float chromaKeySimilarity = 0.6f;

	// Token: 0x04002E03 RID: 11779
	public float chromaKeySmoothRange = 0.03f;

	// Token: 0x04002E04 RID: 11780
	public float chromaKeySpillRange = 0.04f;

	// Token: 0x04002E05 RID: 11781
	public bool useDynamicLighting;

	// Token: 0x04002E06 RID: 11782
	public OVRManager.DepthQuality depthQuality = OVRManager.DepthQuality.Medium;

	// Token: 0x04002E07 RID: 11783
	public float dynamicLightingSmoothFactor = 8f;

	// Token: 0x04002E08 RID: 11784
	public float dynamicLightingDepthVariationClampingValue = 0.001f;

	// Token: 0x04002E09 RID: 11785
	public OVRManager.VirtualGreenScreenType virtualGreenScreenType;

	// Token: 0x04002E0A RID: 11786
	public float virtualGreenScreenTopY;

	// Token: 0x04002E0B RID: 11787
	public float virtualGreenScreenBottomY;

	// Token: 0x04002E0C RID: 11788
	public bool virtualGreenScreenApplyDepthCulling;

	// Token: 0x04002E0D RID: 11789
	public float virtualGreenScreenDepthTolerance = 0.2f;

	// Token: 0x04002E0E RID: 11790
	private const string configFileName = "mrc.config";
}
