using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000908 RID: 2312
internal static class OVRPlugin
{
	// Token: 0x1700065D RID: 1629
	// (get) Token: 0x06003A22 RID: 14882 RVA: 0x0011C4D8 File Offset: 0x0011A8D8
	public static Version version
	{
		get
		{
			if (OVRPlugin._version == null)
			{
				try
				{
					string text = OVRPlugin.OVRP_1_1_0.ovrp_GetVersion();
					if (text != null)
					{
						text = text.Split(new char[]
						{
							'-'
						})[0];
						OVRPlugin._version = new Version(text);
					}
					else
					{
						OVRPlugin._version = OVRPlugin._versionZero;
					}
				}
				catch
				{
					OVRPlugin._version = OVRPlugin._versionZero;
				}
				if (OVRPlugin._version == OVRPlugin.OVRP_0_5_0.version)
				{
					OVRPlugin._version = OVRPlugin.OVRP_0_1_0.version;
				}
				if (OVRPlugin._version > OVRPlugin._versionZero && OVRPlugin._version < OVRPlugin.OVRP_1_3_0.version)
				{
					throw new PlatformNotSupportedException(string.Concat(new object[]
					{
						"Oculus Utilities version ",
						OVRPlugin.wrapperVersion,
						" is too new for OVRPlugin version ",
						OVRPlugin._version.ToString(),
						". Update to the latest version of Unity."
					}));
				}
			}
			return OVRPlugin._version;
		}
	}

	// Token: 0x1700065E RID: 1630
	// (get) Token: 0x06003A23 RID: 14883 RVA: 0x0011C5E0 File Offset: 0x0011A9E0
	public static Version nativeSDKVersion
	{
		get
		{
			if (OVRPlugin._nativeSDKVersion == null)
			{
				try
				{
					string text = string.Empty;
					if (OVRPlugin.version >= OVRPlugin.OVRP_1_1_0.version)
					{
						text = OVRPlugin.OVRP_1_1_0.ovrp_GetNativeSDKVersion();
					}
					else
					{
						text = OVRPlugin._versionZero.ToString();
					}
					if (text != null)
					{
						text = text.Split(new char[]
						{
							'-'
						})[0];
						OVRPlugin._nativeSDKVersion = new Version(text);
					}
					else
					{
						OVRPlugin._nativeSDKVersion = OVRPlugin._versionZero;
					}
				}
				catch
				{
					OVRPlugin._nativeSDKVersion = OVRPlugin._versionZero;
				}
			}
			return OVRPlugin._nativeSDKVersion;
		}
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x06003A24 RID: 14884 RVA: 0x0011C68C File Offset: 0x0011AA8C
	public static bool initialized
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetInitialized() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06003A25 RID: 14885 RVA: 0x0011C696 File Offset: 0x0011AA96
	// (set) Token: 0x06003A26 RID: 14886 RVA: 0x0011C6B6 File Offset: 0x0011AAB6
	public static bool chromatic
	{
		get
		{
			return !(OVRPlugin.version >= OVRPlugin.OVRP_1_7_0.version) || OVRPlugin.OVRP_1_7_0.ovrp_GetAppChromaticCorrection() == OVRPlugin.Bool.True;
		}
		set
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_7_0.version)
			{
				OVRPlugin.OVRP_1_7_0.ovrp_SetAppChromaticCorrection(OVRPlugin.ToBool(value));
			}
		}
	}

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06003A27 RID: 14887 RVA: 0x0011C6D8 File Offset: 0x0011AAD8
	// (set) Token: 0x06003A28 RID: 14888 RVA: 0x0011C6E2 File Offset: 0x0011AAE2
	public static bool monoscopic
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppMonoscopic() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetAppMonoscopic(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06003A29 RID: 14889 RVA: 0x0011C6F0 File Offset: 0x0011AAF0
	// (set) Token: 0x06003A2A RID: 14890 RVA: 0x0011C6FA File Offset: 0x0011AAFA
	public static bool rotation
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingOrientationEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetTrackingOrientationEnabled(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06003A2B RID: 14891 RVA: 0x0011C708 File Offset: 0x0011AB08
	// (set) Token: 0x06003A2C RID: 14892 RVA: 0x0011C712 File Offset: 0x0011AB12
	public static bool position
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingPositionEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetTrackingPositionEnabled(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06003A2D RID: 14893 RVA: 0x0011C720 File Offset: 0x0011AB20
	// (set) Token: 0x06003A2E RID: 14894 RVA: 0x0011C740 File Offset: 0x0011AB40
	public static bool useIPDInPositionTracking
	{
		get
		{
			return !(OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version) || OVRPlugin.OVRP_1_6_0.ovrp_GetTrackingIPDEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
			{
				OVRPlugin.OVRP_1_6_0.ovrp_SetTrackingIPDEnabled(OVRPlugin.ToBool(value));
			}
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06003A2F RID: 14895 RVA: 0x0011C762 File Offset: 0x0011AB62
	public static bool positionSupported
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetTrackingPositionSupported() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x06003A30 RID: 14896 RVA: 0x0011C76C File Offset: 0x0011AB6C
	public static bool positionTracked
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePositionTracked(OVRPlugin.Node.EyeCenter) == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06003A31 RID: 14897 RVA: 0x0011C777 File Offset: 0x0011AB77
	public static bool powerSaving
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemPowerSavingMode() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000668 RID: 1640
	// (get) Token: 0x06003A32 RID: 14898 RVA: 0x0011C781 File Offset: 0x0011AB81
	public static bool hmdPresent
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePresent(OVRPlugin.Node.EyeCenter) == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x06003A33 RID: 14899 RVA: 0x0011C78C File Offset: 0x0011AB8C
	public static bool userPresent
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserPresent() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06003A34 RID: 14900 RVA: 0x0011C796 File Offset: 0x0011AB96
	public static bool headphonesPresent
	{
		get
		{
			return OVRPlugin.OVRP_1_3_0.ovrp_GetSystemHeadphonesPresent() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06003A35 RID: 14901 RVA: 0x0011C7A0 File Offset: 0x0011ABA0
	public static int recommendedMSAALevel
	{
		get
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
			{
				return OVRPlugin.OVRP_1_6_0.ovrp_GetSystemRecommendedMSAALevel();
			}
			return 2;
		}
	}

	// Token: 0x1700066C RID: 1644
	// (get) Token: 0x06003A36 RID: 14902 RVA: 0x0011C7BD File Offset: 0x0011ABBD
	public static OVRPlugin.SystemRegion systemRegion
	{
		get
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_5_0.version)
			{
				return OVRPlugin.OVRP_1_5_0.ovrp_GetSystemRegion();
			}
			return OVRPlugin.SystemRegion.Unspecified;
		}
	}

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06003A37 RID: 14903 RVA: 0x0011C7DC File Offset: 0x0011ABDC
	public static string audioOutId
	{
		get
		{
			try
			{
				if (OVRPlugin._nativeAudioOutGuid == null)
				{
					OVRPlugin._nativeAudioOutGuid = new OVRPlugin.GUID();
				}
				IntPtr intPtr = OVRPlugin.OVRP_1_1_0.ovrp_GetAudioOutId();
				if (intPtr != IntPtr.Zero)
				{
					Marshal.PtrToStructure(intPtr, OVRPlugin._nativeAudioOutGuid);
					Guid guid = new Guid(OVRPlugin._nativeAudioOutGuid.a, OVRPlugin._nativeAudioOutGuid.b, OVRPlugin._nativeAudioOutGuid.c, OVRPlugin._nativeAudioOutGuid.d0, OVRPlugin._nativeAudioOutGuid.d1, OVRPlugin._nativeAudioOutGuid.d2, OVRPlugin._nativeAudioOutGuid.d3, OVRPlugin._nativeAudioOutGuid.d4, OVRPlugin._nativeAudioOutGuid.d5, OVRPlugin._nativeAudioOutGuid.d6, OVRPlugin._nativeAudioOutGuid.d7);
					if (guid != OVRPlugin._cachedAudioOutGuid)
					{
						OVRPlugin._cachedAudioOutGuid = guid;
						OVRPlugin._cachedAudioOutString = OVRPlugin._cachedAudioOutGuid.ToString();
					}
					return OVRPlugin._cachedAudioOutString;
				}
			}
			catch
			{
			}
			return string.Empty;
		}
	}

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x06003A38 RID: 14904 RVA: 0x0011C8EC File Offset: 0x0011ACEC
	public static string audioInId
	{
		get
		{
			try
			{
				if (OVRPlugin._nativeAudioInGuid == null)
				{
					OVRPlugin._nativeAudioInGuid = new OVRPlugin.GUID();
				}
				IntPtr intPtr = OVRPlugin.OVRP_1_1_0.ovrp_GetAudioInId();
				if (intPtr != IntPtr.Zero)
				{
					Marshal.PtrToStructure(intPtr, OVRPlugin._nativeAudioInGuid);
					Guid guid = new Guid(OVRPlugin._nativeAudioInGuid.a, OVRPlugin._nativeAudioInGuid.b, OVRPlugin._nativeAudioInGuid.c, OVRPlugin._nativeAudioInGuid.d0, OVRPlugin._nativeAudioInGuid.d1, OVRPlugin._nativeAudioInGuid.d2, OVRPlugin._nativeAudioInGuid.d3, OVRPlugin._nativeAudioInGuid.d4, OVRPlugin._nativeAudioInGuid.d5, OVRPlugin._nativeAudioInGuid.d6, OVRPlugin._nativeAudioInGuid.d7);
					if (guid != OVRPlugin._cachedAudioInGuid)
					{
						OVRPlugin._cachedAudioInGuid = guid;
						OVRPlugin._cachedAudioInString = OVRPlugin._cachedAudioInGuid.ToString();
					}
					return OVRPlugin._cachedAudioInString;
				}
			}
			catch
			{
			}
			return string.Empty;
		}
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x06003A39 RID: 14905 RVA: 0x0011C9FC File Offset: 0x0011ADFC
	public static bool hasVrFocus
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppHasVrFocus() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06003A3A RID: 14906 RVA: 0x0011CA08 File Offset: 0x0011AE08
	public static bool hasInputFocus
	{
		get
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_18_0.version)
			{
				OVRPlugin.Bool @bool = OVRPlugin.Bool.False;
				return OVRPlugin.OVRP_1_18_0.ovrp_GetAppHasInputFocus(out @bool) == OVRPlugin.Result.Success && @bool == OVRPlugin.Bool.True;
			}
			return true;
		}
	}

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06003A3B RID: 14907 RVA: 0x0011CA41 File Offset: 0x0011AE41
	public static bool shouldQuit
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppShouldQuit() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x06003A3C RID: 14908 RVA: 0x0011CA4B File Offset: 0x0011AE4B
	public static bool shouldRecenter
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppShouldRecenter() == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06003A3D RID: 14909 RVA: 0x0011CA55 File Offset: 0x0011AE55
	public static string productName
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemProductName();
		}
	}

	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06003A3E RID: 14910 RVA: 0x0011CA5C File Offset: 0x0011AE5C
	public static string latency
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetAppLatencyTimings();
		}
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06003A3F RID: 14911 RVA: 0x0011CA63 File Offset: 0x0011AE63
	// (set) Token: 0x06003A40 RID: 14912 RVA: 0x0011CA6A File Offset: 0x0011AE6A
	public static float eyeDepth
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserEyeDepth();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetUserEyeDepth(value);
		}
	}

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06003A41 RID: 14913 RVA: 0x0011CA73 File Offset: 0x0011AE73
	// (set) Token: 0x06003A42 RID: 14914 RVA: 0x0011CA7A File Offset: 0x0011AE7A
	public static float eyeHeight
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserEyeHeight();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetUserEyeHeight(value);
		}
	}

	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06003A43 RID: 14915 RVA: 0x0011CA83 File Offset: 0x0011AE83
	public static float batteryLevel
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryLevel();
		}
	}

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06003A44 RID: 14916 RVA: 0x0011CA8A File Offset: 0x0011AE8A
	public static float batteryTemperature
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryTemperature();
		}
	}

	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06003A45 RID: 14917 RVA: 0x0011CA91 File Offset: 0x0011AE91
	// (set) Token: 0x06003A46 RID: 14918 RVA: 0x0011CA98 File Offset: 0x0011AE98
	public static int cpuLevel
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemCpuLevel();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetSystemCpuLevel(value);
		}
	}

	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06003A47 RID: 14919 RVA: 0x0011CAA1 File Offset: 0x0011AEA1
	// (set) Token: 0x06003A48 RID: 14920 RVA: 0x0011CAA8 File Offset: 0x0011AEA8
	public static int gpuLevel
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemGpuLevel();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetSystemGpuLevel(value);
		}
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06003A49 RID: 14921 RVA: 0x0011CAB1 File Offset: 0x0011AEB1
	// (set) Token: 0x06003A4A RID: 14922 RVA: 0x0011CAB8 File Offset: 0x0011AEB8
	public static int vsyncCount
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemVSyncCount();
		}
		set
		{
			OVRPlugin.OVRP_1_2_0.ovrp_SetSystemVSyncCount(value);
		}
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06003A4B RID: 14923 RVA: 0x0011CAC1 File Offset: 0x0011AEC1
	public static float systemVolume
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemVolume();
		}
	}

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06003A4C RID: 14924 RVA: 0x0011CAC8 File Offset: 0x0011AEC8
	// (set) Token: 0x06003A4D RID: 14925 RVA: 0x0011CACF File Offset: 0x0011AECF
	public static float ipd
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetUserIPD();
		}
		set
		{
			OVRPlugin.OVRP_1_1_0.ovrp_SetUserIPD(value);
		}
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06003A4E RID: 14926 RVA: 0x0011CAD8 File Offset: 0x0011AED8
	// (set) Token: 0x06003A4F RID: 14927 RVA: 0x0011CAE2 File Offset: 0x0011AEE2
	public static bool occlusionMesh
	{
		get
		{
			return OVRPlugin.OVRP_1_3_0.ovrp_GetEyeOcclusionMeshEnabled() == OVRPlugin.Bool.True;
		}
		set
		{
			OVRPlugin.OVRP_1_3_0.ovrp_SetEyeOcclusionMeshEnabled(OVRPlugin.ToBool(value));
		}
	}

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06003A50 RID: 14928 RVA: 0x0011CAF0 File Offset: 0x0011AEF0
	public static OVRPlugin.BatteryStatus batteryStatus
	{
		get
		{
			return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemBatteryStatus();
		}
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x0011CAF7 File Offset: 0x0011AEF7
	public static OVRPlugin.Frustumf GetEyeFrustum(OVRPlugin.Eye eyeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodeFrustum((OVRPlugin.Node)eyeId);
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x0011CAFF File Offset: 0x0011AEFF
	public static OVRPlugin.Sizei GetEyeTextureSize(OVRPlugin.Eye eyeId)
	{
		return OVRPlugin.OVRP_0_1_0.ovrp_GetEyeTextureSize(eyeId);
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x0011CB07 File Offset: 0x0011AF07
	public static OVRPlugin.Posef GetTrackerPose(OVRPlugin.Tracker trackerId)
	{
		return OVRPlugin.GetNodePose((OVRPlugin.Node)(trackerId + 5), OVRPlugin.Step.Render);
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x0011CB12 File Offset: 0x0011AF12
	public static OVRPlugin.Frustumf GetTrackerFrustum(OVRPlugin.Tracker trackerId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodeFrustum((OVRPlugin.Node)(trackerId + 5));
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x0011CB1C File Offset: 0x0011AF1C
	public static bool ShowUI(OVRPlugin.PlatformUI ui)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_ShowSystemUI(ui) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x0011CB28 File Offset: 0x0011AF28
	public static bool EnqueueSubmitLayer(bool onTop, bool headLocked, IntPtr leftTexture, IntPtr rightTexture, int layerId, int frameIndex, OVRPlugin.Posef pose, OVRPlugin.Vector3f scale, int layerIndex = 0, OVRPlugin.OverlayShape shape = OVRPlugin.OverlayShape.Quad)
	{
		if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version))
		{
			return layerIndex == 0 && OVRPlugin.OVRP_0_1_1.ovrp_SetOverlayQuad2(OVRPlugin.ToBool(onTop), OVRPlugin.ToBool(headLocked), leftTexture, IntPtr.Zero, pose, scale) == OVRPlugin.Bool.True;
		}
		uint num = 0U;
		if (onTop)
		{
			num |= 1U;
		}
		if (headLocked)
		{
			num |= 2U;
		}
		if (shape == OVRPlugin.OverlayShape.Cylinder || shape == OVRPlugin.OverlayShape.Cubemap)
		{
			if (shape == OVRPlugin.OverlayShape.Cubemap && OVRPlugin.version >= OVRPlugin.OVRP_1_10_0.version)
			{
				num |= (uint)((uint)shape << 4);
			}
			else
			{
				if (shape != OVRPlugin.OverlayShape.Cylinder || !(OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version))
				{
					return false;
				}
				num |= (uint)((uint)shape << 4);
			}
		}
		if (shape == OVRPlugin.OverlayShape.OffcenterCubemap)
		{
			return false;
		}
		if (shape == OVRPlugin.OverlayShape.Equirect)
		{
			return false;
		}
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && layerId != -1)
		{
			return OVRPlugin.OVRP_1_15_0.ovrp_EnqueueSubmitLayer(num, leftTexture, rightTexture, layerId, frameIndex, ref pose, ref scale, layerIndex) == OVRPlugin.Result.Success;
		}
		return OVRPlugin.OVRP_1_6_0.ovrp_SetOverlayQuad3(num, leftTexture, rightTexture, IntPtr.Zero, pose, scale, layerIndex) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x0011CC48 File Offset: 0x0011B048
	public static OVRPlugin.LayerDesc CalculateLayerDesc(OVRPlugin.OverlayShape shape, OVRPlugin.LayerLayout layout, OVRPlugin.Sizei textureSize, int mipLevels, int sampleCount, OVRPlugin.EyeTextureFormat format, int layerFlags)
	{
		OVRPlugin.LayerDesc result = default(OVRPlugin.LayerDesc);
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
		{
			OVRPlugin.OVRP_1_15_0.ovrp_CalculateLayerDesc(shape, layout, ref textureSize, mipLevels, sampleCount, format, layerFlags, ref result);
		}
		return result;
	}

	// Token: 0x06003A58 RID: 14936 RVA: 0x0011CC85 File Offset: 0x0011B085
	public static bool EnqueueSetupLayer(OVRPlugin.LayerDesc desc, IntPtr layerID)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_EnqueueSetupLayer(ref desc, layerID) == OVRPlugin.Result.Success;
	}

	// Token: 0x06003A59 RID: 14937 RVA: 0x0011CCA8 File Offset: 0x0011B0A8
	public static bool EnqueueDestroyLayer(IntPtr layerID)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_EnqueueDestroyLayer(layerID) == OVRPlugin.Result.Success;
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x0011CCCC File Offset: 0x0011B0CC
	public static IntPtr GetLayerTexture(int layerId, int stage, OVRPlugin.Eye eyeId)
	{
		IntPtr zero = IntPtr.Zero;
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
		{
			OVRPlugin.OVRP_1_15_0.ovrp_GetLayerTexturePtr(layerId, stage, eyeId, ref zero);
		}
		return zero;
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x0011CD00 File Offset: 0x0011B100
	public static int GetLayerTextureStageCount(int layerId)
	{
		int result = 1;
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
		{
			OVRPlugin.OVRP_1_15_0.ovrp_GetLayerTextureStageCount(layerId, ref result);
		}
		return result;
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x0011CD2D File Offset: 0x0011B12D
	public static bool UpdateNodePhysicsPoses(int frameIndex, double predictionSeconds)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_Update2(0, frameIndex, predictionSeconds) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x0011CD50 File Offset: 0x0011B150
	public static OVRPlugin.Posef GetNodePose(OVRPlugin.Node nodeId, OVRPlugin.Step stepId)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
		{
			return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).Pose;
		}
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && stepId == OVRPlugin.Step.Physics)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetNodePose2(0, nodeId);
		}
		return OVRPlugin.OVRP_0_1_2.ovrp_GetNodePose(nodeId);
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x0011CDAC File Offset: 0x0011B1AC
	public static OVRPlugin.Vector3f GetNodeVelocity(OVRPlugin.Node nodeId, OVRPlugin.Step stepId)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
		{
			return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).Velocity;
		}
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && stepId == OVRPlugin.Step.Physics)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetNodeVelocity2(0, nodeId).Position;
		}
		return OVRPlugin.OVRP_0_1_3.ovrp_GetNodeVelocity(nodeId).Position;
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x0011CE18 File Offset: 0x0011B218
	public static OVRPlugin.Vector3f GetNodeAngularVelocity(OVRPlugin.Node nodeId, OVRPlugin.Step stepId)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
		{
			return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).AngularVelocity;
		}
		return default(OVRPlugin.Vector3f);
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x0011CE54 File Offset: 0x0011B254
	public static OVRPlugin.Vector3f GetNodeAcceleration(OVRPlugin.Node nodeId, OVRPlugin.Step stepId)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
		{
			return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).Acceleration;
		}
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && stepId == OVRPlugin.Step.Physics)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetNodeAcceleration2(0, nodeId).Position;
		}
		return OVRPlugin.OVRP_0_1_3.ovrp_GetNodeAcceleration(nodeId).Position;
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x0011CEC0 File Offset: 0x0011B2C0
	public static OVRPlugin.Vector3f GetNodeAngularAcceleration(OVRPlugin.Node nodeId, OVRPlugin.Step stepId)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
		{
			return OVRPlugin.OVRP_1_12_0.ovrp_GetNodePoseState(stepId, nodeId).AngularAcceleration;
		}
		return default(OVRPlugin.Vector3f);
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x0011CEFA File Offset: 0x0011B2FA
	public static bool GetNodePresent(OVRPlugin.Node nodeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePresent(nodeId) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x0011CF05 File Offset: 0x0011B305
	public static bool GetNodeOrientationTracked(OVRPlugin.Node nodeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodeOrientationTracked(nodeId) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A64 RID: 14948 RVA: 0x0011CF10 File Offset: 0x0011B310
	public static bool GetNodePositionTracked(OVRPlugin.Node nodeId)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetNodePositionTracked(nodeId) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x0011CF1B File Offset: 0x0011B31B
	public static OVRPlugin.ControllerState GetControllerState(uint controllerMask)
	{
		return OVRPlugin.OVRP_1_1_0.ovrp_GetControllerState(controllerMask);
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x0011CF23 File Offset: 0x0011B323
	public static OVRPlugin.ControllerState2 GetControllerState2(uint controllerMask)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
		{
			return OVRPlugin.OVRP_1_12_0.ovrp_GetControllerState2(controllerMask);
		}
		return new OVRPlugin.ControllerState2(OVRPlugin.OVRP_1_1_0.ovrp_GetControllerState(controllerMask));
	}

	// Token: 0x06003A67 RID: 14951 RVA: 0x0011CF4C File Offset: 0x0011B34C
	public static OVRPlugin.ControllerState4 GetControllerState4(uint controllerMask)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.ControllerState4 result = default(OVRPlugin.ControllerState4);
			OVRPlugin.OVRP_1_16_0.ovrp_GetControllerState4(controllerMask, ref result);
			return result;
		}
		return new OVRPlugin.ControllerState4(OVRPlugin.GetControllerState2(controllerMask));
	}

	// Token: 0x06003A68 RID: 14952 RVA: 0x0011CF8B File Offset: 0x0011B38B
	public static bool SetControllerVibration(uint controllerMask, float frequency, float amplitude)
	{
		return OVRPlugin.OVRP_0_1_2.ovrp_SetControllerVibration(controllerMask, frequency, amplitude) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x0011CF98 File Offset: 0x0011B398
	public static OVRPlugin.HapticsDesc GetControllerHapticsDesc(uint controllerMask)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetControllerHapticsDesc(controllerMask);
		}
		return default(OVRPlugin.HapticsDesc);
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x0011CFCC File Offset: 0x0011B3CC
	public static OVRPlugin.HapticsState GetControllerHapticsState(uint controllerMask)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetControllerHapticsState(controllerMask);
		}
		return default(OVRPlugin.HapticsState);
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x0011CFFD File Offset: 0x0011B3FD
	public static bool SetControllerHaptics(uint controllerMask, OVRPlugin.HapticsBuffer hapticsBuffer)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version && OVRPlugin.OVRP_1_6_0.ovrp_SetControllerHaptics(controllerMask, hapticsBuffer) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x0011D01F File Offset: 0x0011B41F
	public static float GetEyeRecommendedResolutionScale()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetEyeRecommendedResolutionScale();
		}
		return 1f;
	}

	// Token: 0x06003A6D RID: 14957 RVA: 0x0011D040 File Offset: 0x0011B440
	public static float GetAppCpuStartToGpuEndTime()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_6_0.version)
		{
			return OVRPlugin.OVRP_1_6_0.ovrp_GetAppCpuStartToGpuEndTime();
		}
		return 0f;
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x0011D061 File Offset: 0x0011B461
	public static bool GetBoundaryConfigured()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryConfigured() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A6F RID: 14959 RVA: 0x0011D084 File Offset: 0x0011B484
	public static OVRPlugin.BoundaryTestResult TestBoundaryNode(OVRPlugin.Node nodeId, OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_TestBoundaryNode(nodeId, boundaryType);
		}
		return default(OVRPlugin.BoundaryTestResult);
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x0011D0B8 File Offset: 0x0011B4B8
	public static OVRPlugin.BoundaryTestResult TestBoundaryPoint(OVRPlugin.Vector3f point, OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_TestBoundaryPoint(point, boundaryType);
		}
		return default(OVRPlugin.BoundaryTestResult);
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x0011D0EA File Offset: 0x0011B4EA
	public static bool SetBoundaryLookAndFeel(OVRPlugin.BoundaryLookAndFeel lookAndFeel)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_SetBoundaryLookAndFeel(lookAndFeel) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x0011D10B File Offset: 0x0011B50B
	public static bool ResetBoundaryLookAndFeel()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_ResetBoundaryLookAndFeel() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x0011D12C File Offset: 0x0011B52C
	public static OVRPlugin.BoundaryGeometry GetBoundaryGeometry(OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryGeometry(boundaryType);
		}
		return default(OVRPlugin.BoundaryGeometry);
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x0011D15D File Offset: 0x0011B55D
	public static bool GetBoundaryGeometry2(OVRPlugin.BoundaryType boundaryType, IntPtr points, ref int pointsCount)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetBoundaryGeometry2(boundaryType, points, ref pointsCount) == OVRPlugin.Bool.True;
		}
		pointsCount = 0;
		return false;
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x0011D184 File Offset: 0x0011B584
	public static OVRPlugin.AppPerfStats GetAppPerfStats()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetAppPerfStats();
		}
		return default(OVRPlugin.AppPerfStats);
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x0011D1B4 File Offset: 0x0011B5B4
	public static bool ResetAppPerfStats()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version && OVRPlugin.OVRP_1_9_0.ovrp_ResetAppPerfStats() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x0011D1D4 File Offset: 0x0011B5D4
	public static float GetAppFramerate()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_12_0.version)
		{
			return OVRPlugin.OVRP_1_12_0.ovrp_GetAppFramerate();
		}
		return 0f;
	}

	// Token: 0x06003A78 RID: 14968 RVA: 0x0011D1F8 File Offset: 0x0011B5F8
	public static bool SetHandNodePoseStateLatency(double latencyInSeconds)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_18_0.version && OVRPlugin.OVRP_1_18_0.ovrp_SetHandNodePoseStateLatency(latencyInSeconds) == OVRPlugin.Result.Success;
	}

	// Token: 0x06003A79 RID: 14969 RVA: 0x0011D22C File Offset: 0x0011B62C
	public static double GetHandNodePoseStateLatency()
	{
		if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_18_0.version))
		{
			return 0.0;
		}
		double result = 0.0;
		if (OVRPlugin.OVRP_1_18_0.ovrp_GetHandNodePoseStateLatency(out result) == OVRPlugin.Result.Success)
		{
			return result;
		}
		return 0.0;
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x0011D278 File Offset: 0x0011B678
	public static OVRPlugin.EyeTextureFormat GetDesiredEyeTextureFormat()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_11_0.version)
		{
			uint num = (uint)OVRPlugin.OVRP_1_11_0.ovrp_GetDesiredEyeTextureFormat();
			if (num == 1U)
			{
				num = 0U;
			}
			return (OVRPlugin.EyeTextureFormat)num;
		}
		return OVRPlugin.EyeTextureFormat.Default;
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x0011D2AB File Offset: 0x0011B6AB
	public static bool SetDesiredEyeTextureFormat(OVRPlugin.EyeTextureFormat value)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_11_0.version && OVRPlugin.OVRP_1_11_0.ovrp_SetDesiredEyeTextureFormat(value) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x0011D2CC File Offset: 0x0011B6CC
	public static bool InitializeMixedReality()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_15_0.ovrp_InitializeMixedReality();
			if (result != OVRPlugin.Result.Success)
			{
			}
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x0011D300 File Offset: 0x0011B700
	public static bool ShutdownMixedReality()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_15_0.ovrp_ShutdownMixedReality();
			if (result != OVRPlugin.Result.Success)
			{
			}
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x0011D333 File Offset: 0x0011B733
	public static bool IsMixedRealityInitialized()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version && OVRPlugin.OVRP_1_15_0.ovrp_GetMixedRealityInitialized() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x0011D354 File Offset: 0x0011B754
	public static int GetExternalCameraCount()
	{
		if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version))
		{
			return 0;
		}
		int result = 0;
		OVRPlugin.Result result2 = OVRPlugin.OVRP_1_15_0.ovrp_GetExternalCameraCount(out result);
		if (result2 != OVRPlugin.Result.Success)
		{
			return 0;
		}
		return result;
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x0011D38C File Offset: 0x0011B78C
	public static bool UpdateExternalCamera()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_15_0.ovrp_UpdateExternalCamera();
			if (result != OVRPlugin.Result.Success)
			{
			}
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x0011D3C0 File Offset: 0x0011B7C0
	public static bool GetMixedRealityCameraInfo(int cameraId, out OVRPlugin.CameraExtrinsics cameraExtrinsics, out OVRPlugin.CameraIntrinsics cameraIntrinsics)
	{
		cameraExtrinsics = default(OVRPlugin.CameraExtrinsics);
		cameraIntrinsics = default(OVRPlugin.CameraIntrinsics);
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_15_0.version)
		{
			bool result = true;
			OVRPlugin.Result result2 = OVRPlugin.OVRP_1_15_0.ovrp_GetExternalCameraExtrinsics(cameraId, out cameraExtrinsics);
			if (result2 != OVRPlugin.Result.Success)
			{
				result = false;
			}
			result2 = OVRPlugin.OVRP_1_15_0.ovrp_GetExternalCameraIntrinsics(cameraId, out cameraIntrinsics);
			if (result2 != OVRPlugin.Result.Success)
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x0011D424 File Offset: 0x0011B824
	public static OVRPlugin.Vector3f GetBoundaryDimensions(OVRPlugin.BoundaryType boundaryType)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version)
		{
			return OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryDimensions(boundaryType);
		}
		return default(OVRPlugin.Vector3f);
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x0011D455 File Offset: 0x0011B855
	public static bool GetBoundaryVisible()
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_GetBoundaryVisible() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A84 RID: 14980 RVA: 0x0011D475 File Offset: 0x0011B875
	public static bool SetBoundaryVisible(bool value)
	{
		return OVRPlugin.version >= OVRPlugin.OVRP_1_8_0.version && OVRPlugin.OVRP_1_8_0.ovrp_SetBoundaryVisible(OVRPlugin.ToBool(value)) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A85 RID: 14981 RVA: 0x0011D49B File Offset: 0x0011B89B
	public static OVRPlugin.SystemHeadset GetSystemHeadsetType()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetSystemHeadsetType();
		}
		return OVRPlugin.SystemHeadset.None;
	}

	// Token: 0x06003A86 RID: 14982 RVA: 0x0011D4B8 File Offset: 0x0011B8B8
	public static OVRPlugin.Controller GetActiveController()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetActiveController();
		}
		return OVRPlugin.Controller.None;
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x0011D4D5 File Offset: 0x0011B8D5
	public static OVRPlugin.Controller GetConnectedControllers()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_9_0.version)
		{
			return OVRPlugin.OVRP_1_9_0.ovrp_GetConnectedControllers();
		}
		return OVRPlugin.Controller.None;
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0011D4F2 File Offset: 0x0011B8F2
	private static OVRPlugin.Bool ToBool(bool b)
	{
		return (!b) ? OVRPlugin.Bool.False : OVRPlugin.Bool.True;
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x0011D501 File Offset: 0x0011B901
	public static OVRPlugin.TrackingOrigin GetTrackingOriginType()
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_GetTrackingOriginType();
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x0011D508 File Offset: 0x0011B908
	public static bool SetTrackingOriginType(OVRPlugin.TrackingOrigin originType)
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_SetTrackingOriginType(originType) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x0011D513 File Offset: 0x0011B913
	public static OVRPlugin.Posef GetTrackingCalibratedOrigin()
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_GetTrackingCalibratedOrigin();
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x0011D51A File Offset: 0x0011B91A
	public static bool SetTrackingCalibratedOrigin()
	{
		return OVRPlugin.OVRP_1_2_0.ovrpi_SetTrackingCalibratedOrigin() == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x0011D524 File Offset: 0x0011B924
	public static bool RecenterTrackingOrigin(OVRPlugin.RecenterFlags flags)
	{
		return OVRPlugin.OVRP_1_0_0.ovrp_RecenterTrackingOrigin((uint)flags) == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x0011D530 File Offset: 0x0011B930
	public static bool UpdateCameraDevices()
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_16_0.ovrp_UpdateCameraDevices();
			if (result != OVRPlugin.Result.Success)
			{
			}
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x0011D564 File Offset: 0x0011B964
	public static bool IsCameraDeviceAvailable(OVRPlugin.CameraDevice cameraDevice)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.Bool @bool = OVRPlugin.OVRP_1_16_0.ovrp_IsCameraDeviceAvailable(cameraDevice);
			return @bool == OVRPlugin.Bool.True;
		}
		return false;
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x0011D594 File Offset: 0x0011B994
	public static bool SetCameraDevicePreferredColorFrameSize(OVRPlugin.CameraDevice cameraDevice, int width, int height)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_16_0.ovrp_SetCameraDevicePreferredColorFrameSize(cameraDevice, new OVRPlugin.Sizei
			{
				w = width,
				h = height
			});
			if (result != OVRPlugin.Result.Success)
			{
			}
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x0011D5E4 File Offset: 0x0011B9E4
	public static bool OpenCameraDevice(OVRPlugin.CameraDevice cameraDevice)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_16_0.ovrp_OpenCameraDevice(cameraDevice);
			if (result != OVRPlugin.Result.Success)
			{
			}
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x0011D618 File Offset: 0x0011BA18
	public static bool CloseCameraDevice(OVRPlugin.CameraDevice cameraDevice)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_16_0.ovrp_CloseCameraDevice(cameraDevice);
			if (result != OVRPlugin.Result.Success)
			{
			}
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A93 RID: 14995 RVA: 0x0011D64C File Offset: 0x0011BA4C
	public static bool HasCameraDeviceOpened(OVRPlugin.CameraDevice cameraDevice)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.Bool @bool = OVRPlugin.OVRP_1_16_0.ovrp_HasCameraDeviceOpened(cameraDevice);
			return @bool == OVRPlugin.Bool.True;
		}
		return false;
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x0011D67C File Offset: 0x0011BA7C
	public static bool IsCameraDeviceColorFrameAvailable(OVRPlugin.CameraDevice cameraDevice)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version)
		{
			OVRPlugin.Bool @bool = OVRPlugin.OVRP_1_16_0.ovrp_IsCameraDeviceColorFrameAvailable(cameraDevice);
			return @bool == OVRPlugin.Bool.True;
		}
		return false;
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x0011D6AC File Offset: 0x0011BAAC
	public static Texture2D GetCameraDeviceColorFrameTexture(OVRPlugin.CameraDevice cameraDevice)
	{
		if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_16_0.version))
		{
			return null;
		}
		OVRPlugin.Sizei sizei = default(OVRPlugin.Sizei);
		OVRPlugin.Result result = OVRPlugin.OVRP_1_16_0.ovrp_GetCameraDeviceColorFrameSize(cameraDevice, out sizei);
		if (result != OVRPlugin.Result.Success)
		{
			return null;
		}
		IntPtr data;
		int num;
		result = OVRPlugin.OVRP_1_16_0.ovrp_GetCameraDeviceColorFrameBgraPixels(cameraDevice, out data, out num);
		if (result != OVRPlugin.Result.Success)
		{
			return null;
		}
		if (num != sizei.w * 4)
		{
			return null;
		}
		if (!OVRPlugin.cachedCameraFrameTexture || OVRPlugin.cachedCameraFrameTexture.width != sizei.w || OVRPlugin.cachedCameraFrameTexture.height != sizei.h)
		{
			OVRPlugin.cachedCameraFrameTexture = new Texture2D(sizei.w, sizei.h, TextureFormat.BGRA32, false);
		}
		OVRPlugin.cachedCameraFrameTexture.LoadRawTextureData(data, num * sizei.h);
		OVRPlugin.cachedCameraFrameTexture.Apply();
		return OVRPlugin.cachedCameraFrameTexture;
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x0011D788 File Offset: 0x0011BB88
	public static bool DoesCameraDeviceSupportDepth(OVRPlugin.CameraDevice cameraDevice)
	{
		OVRPlugin.Bool @bool;
		return OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version && OVRPlugin.OVRP_1_17_0.ovrp_DoesCameraDeviceSupportDepth(cameraDevice, out @bool) == OVRPlugin.Result.Success && @bool == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x0011D7C4 File Offset: 0x0011BBC4
	public static bool SetCameraDeviceDepthSensingMode(OVRPlugin.CameraDevice camera, OVRPlugin.CameraDeviceDepthSensingMode depthSensoringMode)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_17_0.ovrp_SetCameraDeviceDepthSensingMode(camera, depthSensoringMode);
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x0011D7F4 File Offset: 0x0011BBF4
	public static bool SetCameraDevicePreferredDepthQuality(OVRPlugin.CameraDevice camera, OVRPlugin.CameraDeviceDepthQuality depthQuality)
	{
		if (OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version)
		{
			OVRPlugin.Result result = OVRPlugin.OVRP_1_17_0.ovrp_SetCameraDevicePreferredDepthQuality(camera, depthQuality);
			return result == OVRPlugin.Result.Success;
		}
		return false;
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x0011D824 File Offset: 0x0011BC24
	public static bool IsCameraDeviceDepthFrameAvailable(OVRPlugin.CameraDevice cameraDevice)
	{
		OVRPlugin.Bool @bool;
		return OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version && OVRPlugin.OVRP_1_17_0.ovrp_IsCameraDeviceDepthFrameAvailable(cameraDevice, out @bool) == OVRPlugin.Result.Success && @bool == OVRPlugin.Bool.True;
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x0011D860 File Offset: 0x0011BC60
	public static Texture2D GetCameraDeviceDepthFrameTexture(OVRPlugin.CameraDevice cameraDevice)
	{
		if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version))
		{
			return null;
		}
		OVRPlugin.Sizei sizei = default(OVRPlugin.Sizei);
		OVRPlugin.Result result = OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthFrameSize(cameraDevice, out sizei);
		if (result != OVRPlugin.Result.Success)
		{
			return null;
		}
		IntPtr data;
		int num;
		result = OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthFramePixels(cameraDevice, out data, out num);
		if (result != OVRPlugin.Result.Success)
		{
			return null;
		}
		if (num != sizei.w * 4)
		{
			return null;
		}
		if (!OVRPlugin.cachedCameraDepthTexture || OVRPlugin.cachedCameraDepthTexture.width != sizei.w || OVRPlugin.cachedCameraDepthTexture.height != sizei.h)
		{
			OVRPlugin.cachedCameraDepthTexture = new Texture2D(sizei.w, sizei.h, TextureFormat.RFloat, false);
			OVRPlugin.cachedCameraDepthTexture.filterMode = FilterMode.Point;
		}
		OVRPlugin.cachedCameraDepthTexture.LoadRawTextureData(data, num * sizei.h);
		OVRPlugin.cachedCameraDepthTexture.Apply();
		return OVRPlugin.cachedCameraDepthTexture;
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x0011D944 File Offset: 0x0011BD44
	public static Texture2D GetCameraDeviceDepthConfidenceTexture(OVRPlugin.CameraDevice cameraDevice)
	{
		if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_17_0.version))
		{
			return null;
		}
		OVRPlugin.Sizei sizei = default(OVRPlugin.Sizei);
		OVRPlugin.Result result = OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthFrameSize(cameraDevice, out sizei);
		if (result != OVRPlugin.Result.Success)
		{
			return null;
		}
		IntPtr data;
		int num;
		result = OVRPlugin.OVRP_1_17_0.ovrp_GetCameraDeviceDepthConfidencePixels(cameraDevice, out data, out num);
		if (result != OVRPlugin.Result.Success)
		{
			return null;
		}
		if (num != sizei.w * 4)
		{
			return null;
		}
		if (!OVRPlugin.cachedCameraDepthConfidenceTexture || OVRPlugin.cachedCameraDepthConfidenceTexture.width != sizei.w || OVRPlugin.cachedCameraDepthConfidenceTexture.height != sizei.h)
		{
			OVRPlugin.cachedCameraDepthConfidenceTexture = new Texture2D(sizei.w, sizei.h, TextureFormat.RFloat, false);
		}
		OVRPlugin.cachedCameraDepthConfidenceTexture.LoadRawTextureData(data, num * sizei.h);
		OVRPlugin.cachedCameraDepthConfidenceTexture.Apply();
		return OVRPlugin.cachedCameraDepthConfidenceTexture;
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06003A9C RID: 15004 RVA: 0x0011DA20 File Offset: 0x0011BE20
	public static bool tiledMultiResSupported
	{
		get
		{
			OVRPlugin.Bool @bool;
			return OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version && OVRPlugin.OVRP_1_21_0.ovrp_GetTiledMultiResSupported(out @bool) == OVRPlugin.Result.Success && @bool == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06003A9D RID: 15005 RVA: 0x0011DA58 File Offset: 0x0011BE58
	// (set) Token: 0x06003A9E RID: 15006 RVA: 0x0011DA94 File Offset: 0x0011BE94
	public static OVRPlugin.TiledMultiResLevel tiledMultiResLevel
	{
		get
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version && OVRPlugin.tiledMultiResSupported)
			{
				OVRPlugin.TiledMultiResLevel result2;
				OVRPlugin.Result result = OVRPlugin.OVRP_1_21_0.ovrp_GetTiledMultiResLevel(out result2);
				if (result != OVRPlugin.Result.Success)
				{
				}
				return result2;
			}
			return OVRPlugin.TiledMultiResLevel.Off;
		}
		set
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version && OVRPlugin.tiledMultiResSupported)
			{
				OVRPlugin.Result result = OVRPlugin.OVRP_1_21_0.ovrp_SetTiledMultiResLevel(value);
				if (result != OVRPlugin.Result.Success)
				{
				}
			}
		}
	}

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06003A9F RID: 15007 RVA: 0x0011DACC File Offset: 0x0011BECC
	public static bool gpuUtilSupported
	{
		get
		{
			OVRPlugin.Bool @bool;
			return OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version && OVRPlugin.OVRP_1_21_0.ovrp_GetGPUUtilSupported(out @bool) == OVRPlugin.Result.Success && @bool == OVRPlugin.Bool.True;
		}
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06003AA0 RID: 15008 RVA: 0x0011DB04 File Offset: 0x0011BF04
	public static float gpuUtilLevel
	{
		get
		{
			if (!(OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version) || !OVRPlugin.gpuUtilSupported)
			{
				return 0f;
			}
			float result;
			if (OVRPlugin.OVRP_1_21_0.ovrp_GetGPUUtilLevel(out result) == OVRPlugin.Result.Success)
			{
				return result;
			}
			return 0f;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06003AA1 RID: 15009 RVA: 0x0011DB4C File Offset: 0x0011BF4C
	public static float[] systemDisplayFrequenciesAvailable
	{
		get
		{
			if (OVRPlugin._cachedSystemDisplayFrequenciesAvailable == null)
			{
				OVRPlugin._cachedSystemDisplayFrequenciesAvailable = new float[0];
				if (OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version)
				{
					int num = 0;
					if (OVRPlugin.OVRP_1_21_0.ovrp_GetSystemDisplayAvailableFrequencies(IntPtr.Zero, out num) == OVRPlugin.Result.Success && num > 0)
					{
						int num2 = num;
						OVRPlugin._nativeSystemDisplayFrequenciesAvailable = new OVRNativeBuffer(4 * num2);
						if (OVRPlugin.OVRP_1_21_0.ovrp_GetSystemDisplayAvailableFrequencies(OVRPlugin._nativeSystemDisplayFrequenciesAvailable.GetPointer(0), out num) == OVRPlugin.Result.Success)
						{
							int num3 = (num > num2) ? num2 : num;
							if (num3 > 0)
							{
								OVRPlugin._cachedSystemDisplayFrequenciesAvailable = new float[num3];
								Marshal.Copy(OVRPlugin._nativeSystemDisplayFrequenciesAvailable.GetPointer(0), OVRPlugin._cachedSystemDisplayFrequenciesAvailable, 0, num3);
							}
						}
					}
				}
			}
			return OVRPlugin._cachedSystemDisplayFrequenciesAvailable;
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06003AA2 RID: 15010 RVA: 0x0011DC04 File Offset: 0x0011C004
	// (set) Token: 0x06003AA3 RID: 15011 RVA: 0x0011DC5A File Offset: 0x0011C05A
	public static float systemDisplayFrequency
	{
		get
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version)
			{
				float result;
				if (OVRPlugin.OVRP_1_21_0.ovrp_GetSystemDisplayFrequency2(out result) == OVRPlugin.Result.Success)
				{
					return result;
				}
				return 0f;
			}
			else
			{
				if (OVRPlugin.version >= OVRPlugin.OVRP_1_1_0.version)
				{
					return OVRPlugin.OVRP_1_1_0.ovrp_GetSystemDisplayFrequency();
				}
				return 0f;
			}
		}
		set
		{
			if (OVRPlugin.version >= OVRPlugin.OVRP_1_21_0.version)
			{
				OVRPlugin.OVRP_1_21_0.ovrp_SetSystemDisplayFrequency(value);
			}
		}
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x0011DC78 File Offset: 0x0011C078
	// Note: this type is marked as 'beforefieldinit'.
	static OVRPlugin()
	{
	}

	// Token: 0x04002BBF RID: 11199
	public static readonly Version wrapperVersion = OVRPlugin.OVRP_1_25_1.version;

	// Token: 0x04002BC0 RID: 11200
	private static Version _version;

	// Token: 0x04002BC1 RID: 11201
	private static Version _nativeSDKVersion;

	// Token: 0x04002BC2 RID: 11202
	private const int OverlayShapeFlagShift = 4;

	// Token: 0x04002BC3 RID: 11203
	public const int AppPerfFrameStatsMaxCount = 5;

	// Token: 0x04002BC4 RID: 11204
	private static OVRPlugin.GUID _nativeAudioOutGuid = new OVRPlugin.GUID();

	// Token: 0x04002BC5 RID: 11205
	private static Guid _cachedAudioOutGuid;

	// Token: 0x04002BC6 RID: 11206
	private static string _cachedAudioOutString;

	// Token: 0x04002BC7 RID: 11207
	private static OVRPlugin.GUID _nativeAudioInGuid = new OVRPlugin.GUID();

	// Token: 0x04002BC8 RID: 11208
	private static Guid _cachedAudioInGuid;

	// Token: 0x04002BC9 RID: 11209
	private static string _cachedAudioInString;

	// Token: 0x04002BCA RID: 11210
	private static Texture2D cachedCameraFrameTexture = null;

	// Token: 0x04002BCB RID: 11211
	private static Texture2D cachedCameraDepthTexture = null;

	// Token: 0x04002BCC RID: 11212
	private static Texture2D cachedCameraDepthConfidenceTexture = null;

	// Token: 0x04002BCD RID: 11213
	private static OVRNativeBuffer _nativeSystemDisplayFrequenciesAvailable = null;

	// Token: 0x04002BCE RID: 11214
	private static float[] _cachedSystemDisplayFrequenciesAvailable = null;

	// Token: 0x04002BCF RID: 11215
	private const string pluginName = "OVRPlugin";

	// Token: 0x04002BD0 RID: 11216
	private static Version _versionZero = new Version(0, 0, 0);

	// Token: 0x02000909 RID: 2313
	[StructLayout(LayoutKind.Sequential)]
	private class GUID
	{
		// Token: 0x06003AA5 RID: 15013 RVA: 0x0011DCCE File Offset: 0x0011C0CE
		public GUID()
		{
		}

		// Token: 0x04002BD1 RID: 11217
		public int a;

		// Token: 0x04002BD2 RID: 11218
		public short b;

		// Token: 0x04002BD3 RID: 11219
		public short c;

		// Token: 0x04002BD4 RID: 11220
		public byte d0;

		// Token: 0x04002BD5 RID: 11221
		public byte d1;

		// Token: 0x04002BD6 RID: 11222
		public byte d2;

		// Token: 0x04002BD7 RID: 11223
		public byte d3;

		// Token: 0x04002BD8 RID: 11224
		public byte d4;

		// Token: 0x04002BD9 RID: 11225
		public byte d5;

		// Token: 0x04002BDA RID: 11226
		public byte d6;

		// Token: 0x04002BDB RID: 11227
		public byte d7;
	}

	// Token: 0x0200090A RID: 2314
	public enum Bool
	{
		// Token: 0x04002BDD RID: 11229
		False,
		// Token: 0x04002BDE RID: 11230
		True
	}

	// Token: 0x0200090B RID: 2315
	public enum Result
	{
		// Token: 0x04002BE0 RID: 11232
		Success,
		// Token: 0x04002BE1 RID: 11233
		Failure = -1000,
		// Token: 0x04002BE2 RID: 11234
		Failure_InvalidParameter = -1001,
		// Token: 0x04002BE3 RID: 11235
		Failure_NotInitialized = -1002,
		// Token: 0x04002BE4 RID: 11236
		Failure_InvalidOperation = -1003,
		// Token: 0x04002BE5 RID: 11237
		Failure_Unsupported = -1004,
		// Token: 0x04002BE6 RID: 11238
		Failure_NotYetImplemented = -1005,
		// Token: 0x04002BE7 RID: 11239
		Failure_OperationFailed = -1006,
		// Token: 0x04002BE8 RID: 11240
		Failure_InsufficientSize = -1007
	}

	// Token: 0x0200090C RID: 2316
	public enum CameraStatus
	{
		// Token: 0x04002BEA RID: 11242
		CameraStatus_None,
		// Token: 0x04002BEB RID: 11243
		CameraStatus_Connected,
		// Token: 0x04002BEC RID: 11244
		CameraStatus_Calibrating,
		// Token: 0x04002BED RID: 11245
		CameraStatus_CalibrationFailed,
		// Token: 0x04002BEE RID: 11246
		CameraStatus_Calibrated,
		// Token: 0x04002BEF RID: 11247
		CameraStatus_EnumSize = 2147483647
	}

	// Token: 0x0200090D RID: 2317
	public enum Eye
	{
		// Token: 0x04002BF1 RID: 11249
		None = -1,
		// Token: 0x04002BF2 RID: 11250
		Left,
		// Token: 0x04002BF3 RID: 11251
		Right,
		// Token: 0x04002BF4 RID: 11252
		Count
	}

	// Token: 0x0200090E RID: 2318
	public enum Tracker
	{
		// Token: 0x04002BF6 RID: 11254
		None = -1,
		// Token: 0x04002BF7 RID: 11255
		Zero,
		// Token: 0x04002BF8 RID: 11256
		One,
		// Token: 0x04002BF9 RID: 11257
		Two,
		// Token: 0x04002BFA RID: 11258
		Three,
		// Token: 0x04002BFB RID: 11259
		Count
	}

	// Token: 0x0200090F RID: 2319
	public enum Node
	{
		// Token: 0x04002BFD RID: 11261
		None = -1,
		// Token: 0x04002BFE RID: 11262
		EyeLeft,
		// Token: 0x04002BFF RID: 11263
		EyeRight,
		// Token: 0x04002C00 RID: 11264
		EyeCenter,
		// Token: 0x04002C01 RID: 11265
		HandLeft,
		// Token: 0x04002C02 RID: 11266
		HandRight,
		// Token: 0x04002C03 RID: 11267
		TrackerZero,
		// Token: 0x04002C04 RID: 11268
		TrackerOne,
		// Token: 0x04002C05 RID: 11269
		TrackerTwo,
		// Token: 0x04002C06 RID: 11270
		TrackerThree,
		// Token: 0x04002C07 RID: 11271
		Head,
		// Token: 0x04002C08 RID: 11272
		DeviceObjectZero,
		// Token: 0x04002C09 RID: 11273
		Count
	}

	// Token: 0x02000910 RID: 2320
	public enum Controller
	{
		// Token: 0x04002C0B RID: 11275
		None,
		// Token: 0x04002C0C RID: 11276
		LTouch,
		// Token: 0x04002C0D RID: 11277
		RTouch,
		// Token: 0x04002C0E RID: 11278
		Touch,
		// Token: 0x04002C0F RID: 11279
		Remote,
		// Token: 0x04002C10 RID: 11280
		Gamepad = 16,
		// Token: 0x04002C11 RID: 11281
		Touchpad = 134217728,
		// Token: 0x04002C12 RID: 11282
		LTrackedRemote = 16777216,
		// Token: 0x04002C13 RID: 11283
		RTrackedRemote = 33554432,
		// Token: 0x04002C14 RID: 11284
		Active = -2147483648,
		// Token: 0x04002C15 RID: 11285
		All = -1
	}

	// Token: 0x02000911 RID: 2321
	public enum TrackingOrigin
	{
		// Token: 0x04002C17 RID: 11287
		EyeLevel,
		// Token: 0x04002C18 RID: 11288
		FloorLevel,
		// Token: 0x04002C19 RID: 11289
		Count
	}

	// Token: 0x02000912 RID: 2322
	public enum RecenterFlags
	{
		// Token: 0x04002C1B RID: 11291
		Default,
		// Token: 0x04002C1C RID: 11292
		Controllers = 1073741824,
		// Token: 0x04002C1D RID: 11293
		IgnoreAll = -2147483648,
		// Token: 0x04002C1E RID: 11294
		Count
	}

	// Token: 0x02000913 RID: 2323
	public enum BatteryStatus
	{
		// Token: 0x04002C20 RID: 11296
		Charging,
		// Token: 0x04002C21 RID: 11297
		Discharging,
		// Token: 0x04002C22 RID: 11298
		Full,
		// Token: 0x04002C23 RID: 11299
		NotCharging,
		// Token: 0x04002C24 RID: 11300
		Unknown
	}

	// Token: 0x02000914 RID: 2324
	public enum EyeTextureFormat
	{
		// Token: 0x04002C26 RID: 11302
		Default,
		// Token: 0x04002C27 RID: 11303
		R8G8B8A8_sRGB = 0,
		// Token: 0x04002C28 RID: 11304
		R8G8B8A8,
		// Token: 0x04002C29 RID: 11305
		R16G16B16A16_FP,
		// Token: 0x04002C2A RID: 11306
		R11G11B10_FP,
		// Token: 0x04002C2B RID: 11307
		B8G8R8A8_sRGB,
		// Token: 0x04002C2C RID: 11308
		B8G8R8A8,
		// Token: 0x04002C2D RID: 11309
		R5G6B5 = 11,
		// Token: 0x04002C2E RID: 11310
		EnumSize = 2147483647
	}

	// Token: 0x02000915 RID: 2325
	public enum PlatformUI
	{
		// Token: 0x04002C30 RID: 11312
		None = -1,
		// Token: 0x04002C31 RID: 11313
		ConfirmQuit = 1,
		// Token: 0x04002C32 RID: 11314
		GlobalMenuTutorial
	}

	// Token: 0x02000916 RID: 2326
	public enum SystemRegion
	{
		// Token: 0x04002C34 RID: 11316
		Unspecified,
		// Token: 0x04002C35 RID: 11317
		Japan,
		// Token: 0x04002C36 RID: 11318
		China
	}

	// Token: 0x02000917 RID: 2327
	public enum SystemHeadset
	{
		// Token: 0x04002C38 RID: 11320
		None,
		// Token: 0x04002C39 RID: 11321
		GearVR_R320,
		// Token: 0x04002C3A RID: 11322
		GearVR_R321,
		// Token: 0x04002C3B RID: 11323
		GearVR_R322,
		// Token: 0x04002C3C RID: 11324
		GearVR_R323,
		// Token: 0x04002C3D RID: 11325
		GearVR_R324,
		// Token: 0x04002C3E RID: 11326
		GearVR_R325,
		// Token: 0x04002C3F RID: 11327
		Oculus_Go,
		// Token: 0x04002C40 RID: 11328
		Rift_DK1 = 4096,
		// Token: 0x04002C41 RID: 11329
		Rift_DK2,
		// Token: 0x04002C42 RID: 11330
		Rift_CV1
	}

	// Token: 0x02000918 RID: 2328
	public enum OverlayShape
	{
		// Token: 0x04002C44 RID: 11332
		Quad,
		// Token: 0x04002C45 RID: 11333
		Cylinder,
		// Token: 0x04002C46 RID: 11334
		Cubemap,
		// Token: 0x04002C47 RID: 11335
		OffcenterCubemap = 4,
		// Token: 0x04002C48 RID: 11336
		Equirect
	}

	// Token: 0x02000919 RID: 2329
	public enum Step
	{
		// Token: 0x04002C4A RID: 11338
		Render = -1,
		// Token: 0x04002C4B RID: 11339
		Physics
	}

	// Token: 0x0200091A RID: 2330
	public enum CameraDevice
	{
		// Token: 0x04002C4D RID: 11341
		None,
		// Token: 0x04002C4E RID: 11342
		WebCamera0 = 100,
		// Token: 0x04002C4F RID: 11343
		WebCamera1,
		// Token: 0x04002C50 RID: 11344
		ZEDCamera = 300
	}

	// Token: 0x0200091B RID: 2331
	public enum CameraDeviceDepthSensingMode
	{
		// Token: 0x04002C52 RID: 11346
		Standard,
		// Token: 0x04002C53 RID: 11347
		Fill
	}

	// Token: 0x0200091C RID: 2332
	public enum CameraDeviceDepthQuality
	{
		// Token: 0x04002C55 RID: 11349
		Low,
		// Token: 0x04002C56 RID: 11350
		Medium,
		// Token: 0x04002C57 RID: 11351
		High
	}

	// Token: 0x0200091D RID: 2333
	public enum TiledMultiResLevel
	{
		// Token: 0x04002C59 RID: 11353
		Off,
		// Token: 0x04002C5A RID: 11354
		LMSLow,
		// Token: 0x04002C5B RID: 11355
		LMSMedium,
		// Token: 0x04002C5C RID: 11356
		LMSHigh,
		// Token: 0x04002C5D RID: 11357
		EnumSize = 2147483647
	}

	// Token: 0x0200091E RID: 2334
	public struct CameraDeviceIntrinsicsParameters
	{
		// Token: 0x04002C5E RID: 11358
		private float fx;

		// Token: 0x04002C5F RID: 11359
		private float fy;

		// Token: 0x04002C60 RID: 11360
		private float cx;

		// Token: 0x04002C61 RID: 11361
		private float cy;

		// Token: 0x04002C62 RID: 11362
		private double disto0;

		// Token: 0x04002C63 RID: 11363
		private double disto1;

		// Token: 0x04002C64 RID: 11364
		private double disto2;

		// Token: 0x04002C65 RID: 11365
		private double disto3;

		// Token: 0x04002C66 RID: 11366
		private double disto4;

		// Token: 0x04002C67 RID: 11367
		private float v_fov;

		// Token: 0x04002C68 RID: 11368
		private float h_fov;

		// Token: 0x04002C69 RID: 11369
		private float d_fov;

		// Token: 0x04002C6A RID: 11370
		private int w;

		// Token: 0x04002C6B RID: 11371
		private int h;
	}

	// Token: 0x0200091F RID: 2335
	private enum OverlayFlag
	{
		// Token: 0x04002C6D RID: 11373
		None,
		// Token: 0x04002C6E RID: 11374
		OnTop,
		// Token: 0x04002C6F RID: 11375
		HeadLocked,
		// Token: 0x04002C70 RID: 11376
		ShapeFlag_Quad = 0,
		// Token: 0x04002C71 RID: 11377
		ShapeFlag_Cylinder = 16,
		// Token: 0x04002C72 RID: 11378
		ShapeFlag_Cubemap = 32,
		// Token: 0x04002C73 RID: 11379
		ShapeFlag_OffcenterCubemap = 64,
		// Token: 0x04002C74 RID: 11380
		ShapeFlagRangeMask = 240
	}

	// Token: 0x02000920 RID: 2336
	public struct Vector2f
	{
		// Token: 0x04002C75 RID: 11381
		public float x;

		// Token: 0x04002C76 RID: 11382
		public float y;
	}

	// Token: 0x02000921 RID: 2337
	public struct Vector3f
	{
		// Token: 0x06003AA6 RID: 15014 RVA: 0x0011DCD6 File Offset: 0x0011C0D6
		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}", this.x, this.y, this.z);
		}

		// Token: 0x04002C77 RID: 11383
		public float x;

		// Token: 0x04002C78 RID: 11384
		public float y;

		// Token: 0x04002C79 RID: 11385
		public float z;
	}

	// Token: 0x02000922 RID: 2338
	public struct Quatf
	{
		// Token: 0x06003AA7 RID: 15015 RVA: 0x0011DD04 File Offset: 0x0011C104
		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}", new object[]
			{
				this.x,
				this.y,
				this.z,
				this.w
			});
		}

		// Token: 0x04002C7A RID: 11386
		public float x;

		// Token: 0x04002C7B RID: 11387
		public float y;

		// Token: 0x04002C7C RID: 11388
		public float z;

		// Token: 0x04002C7D RID: 11389
		public float w;
	}

	// Token: 0x02000923 RID: 2339
	public struct Posef
	{
		// Token: 0x06003AA8 RID: 15016 RVA: 0x0011DD59 File Offset: 0x0011C159
		public override string ToString()
		{
			return string.Format("Position ({0}), Orientation({1})", this.Position, this.Orientation);
		}

		// Token: 0x04002C7E RID: 11390
		public OVRPlugin.Quatf Orientation;

		// Token: 0x04002C7F RID: 11391
		public OVRPlugin.Vector3f Position;
	}

	// Token: 0x02000924 RID: 2340
	public struct PoseStatef
	{
		// Token: 0x04002C80 RID: 11392
		public OVRPlugin.Posef Pose;

		// Token: 0x04002C81 RID: 11393
		public OVRPlugin.Vector3f Velocity;

		// Token: 0x04002C82 RID: 11394
		public OVRPlugin.Vector3f Acceleration;

		// Token: 0x04002C83 RID: 11395
		public OVRPlugin.Vector3f AngularVelocity;

		// Token: 0x04002C84 RID: 11396
		public OVRPlugin.Vector3f AngularAcceleration;

		// Token: 0x04002C85 RID: 11397
		private double Time;
	}

	// Token: 0x02000925 RID: 2341
	public struct ControllerState4
	{
		// Token: 0x06003AA9 RID: 15017 RVA: 0x0011DD7C File Offset: 0x0011C17C
		public ControllerState4(OVRPlugin.ControllerState2 cs)
		{
			this.ConnectedControllers = cs.ConnectedControllers;
			this.Buttons = cs.Buttons;
			this.Touches = cs.Touches;
			this.NearTouches = cs.NearTouches;
			this.LIndexTrigger = cs.LIndexTrigger;
			this.RIndexTrigger = cs.RIndexTrigger;
			this.LHandTrigger = cs.LHandTrigger;
			this.RHandTrigger = cs.RHandTrigger;
			this.LThumbstick = cs.LThumbstick;
			this.RThumbstick = cs.RThumbstick;
			this.LTouchpad = cs.LTouchpad;
			this.RTouchpad = cs.RTouchpad;
			this.LBatteryPercentRemaining = 0;
			this.RBatteryPercentRemaining = 0;
			this.LRecenterCount = 0;
			this.RRecenterCount = 0;
			this.Reserved_27 = 0;
			this.Reserved_26 = 0;
			this.Reserved_25 = 0;
			this.Reserved_24 = 0;
			this.Reserved_23 = 0;
			this.Reserved_22 = 0;
			this.Reserved_21 = 0;
			this.Reserved_20 = 0;
			this.Reserved_19 = 0;
			this.Reserved_18 = 0;
			this.Reserved_17 = 0;
			this.Reserved_16 = 0;
			this.Reserved_15 = 0;
			this.Reserved_14 = 0;
			this.Reserved_13 = 0;
			this.Reserved_12 = 0;
			this.Reserved_11 = 0;
			this.Reserved_10 = 0;
			this.Reserved_09 = 0;
			this.Reserved_08 = 0;
			this.Reserved_07 = 0;
			this.Reserved_06 = 0;
			this.Reserved_05 = 0;
			this.Reserved_04 = 0;
			this.Reserved_03 = 0;
			this.Reserved_02 = 0;
			this.Reserved_01 = 0;
			this.Reserved_00 = 0;
		}

		// Token: 0x04002C86 RID: 11398
		public uint ConnectedControllers;

		// Token: 0x04002C87 RID: 11399
		public uint Buttons;

		// Token: 0x04002C88 RID: 11400
		public uint Touches;

		// Token: 0x04002C89 RID: 11401
		public uint NearTouches;

		// Token: 0x04002C8A RID: 11402
		public float LIndexTrigger;

		// Token: 0x04002C8B RID: 11403
		public float RIndexTrigger;

		// Token: 0x04002C8C RID: 11404
		public float LHandTrigger;

		// Token: 0x04002C8D RID: 11405
		public float RHandTrigger;

		// Token: 0x04002C8E RID: 11406
		public OVRPlugin.Vector2f LThumbstick;

		// Token: 0x04002C8F RID: 11407
		public OVRPlugin.Vector2f RThumbstick;

		// Token: 0x04002C90 RID: 11408
		public OVRPlugin.Vector2f LTouchpad;

		// Token: 0x04002C91 RID: 11409
		public OVRPlugin.Vector2f RTouchpad;

		// Token: 0x04002C92 RID: 11410
		public byte LBatteryPercentRemaining;

		// Token: 0x04002C93 RID: 11411
		public byte RBatteryPercentRemaining;

		// Token: 0x04002C94 RID: 11412
		public byte LRecenterCount;

		// Token: 0x04002C95 RID: 11413
		public byte RRecenterCount;

		// Token: 0x04002C96 RID: 11414
		public byte Reserved_27;

		// Token: 0x04002C97 RID: 11415
		public byte Reserved_26;

		// Token: 0x04002C98 RID: 11416
		public byte Reserved_25;

		// Token: 0x04002C99 RID: 11417
		public byte Reserved_24;

		// Token: 0x04002C9A RID: 11418
		public byte Reserved_23;

		// Token: 0x04002C9B RID: 11419
		public byte Reserved_22;

		// Token: 0x04002C9C RID: 11420
		public byte Reserved_21;

		// Token: 0x04002C9D RID: 11421
		public byte Reserved_20;

		// Token: 0x04002C9E RID: 11422
		public byte Reserved_19;

		// Token: 0x04002C9F RID: 11423
		public byte Reserved_18;

		// Token: 0x04002CA0 RID: 11424
		public byte Reserved_17;

		// Token: 0x04002CA1 RID: 11425
		public byte Reserved_16;

		// Token: 0x04002CA2 RID: 11426
		public byte Reserved_15;

		// Token: 0x04002CA3 RID: 11427
		public byte Reserved_14;

		// Token: 0x04002CA4 RID: 11428
		public byte Reserved_13;

		// Token: 0x04002CA5 RID: 11429
		public byte Reserved_12;

		// Token: 0x04002CA6 RID: 11430
		public byte Reserved_11;

		// Token: 0x04002CA7 RID: 11431
		public byte Reserved_10;

		// Token: 0x04002CA8 RID: 11432
		public byte Reserved_09;

		// Token: 0x04002CA9 RID: 11433
		public byte Reserved_08;

		// Token: 0x04002CAA RID: 11434
		public byte Reserved_07;

		// Token: 0x04002CAB RID: 11435
		public byte Reserved_06;

		// Token: 0x04002CAC RID: 11436
		public byte Reserved_05;

		// Token: 0x04002CAD RID: 11437
		public byte Reserved_04;

		// Token: 0x04002CAE RID: 11438
		public byte Reserved_03;

		// Token: 0x04002CAF RID: 11439
		public byte Reserved_02;

		// Token: 0x04002CB0 RID: 11440
		public byte Reserved_01;

		// Token: 0x04002CB1 RID: 11441
		public byte Reserved_00;
	}

	// Token: 0x02000926 RID: 2342
	public struct ControllerState2
	{
		// Token: 0x06003AAA RID: 15018 RVA: 0x0011DF08 File Offset: 0x0011C308
		public ControllerState2(OVRPlugin.ControllerState cs)
		{
			this.ConnectedControllers = cs.ConnectedControllers;
			this.Buttons = cs.Buttons;
			this.Touches = cs.Touches;
			this.NearTouches = cs.NearTouches;
			this.LIndexTrigger = cs.LIndexTrigger;
			this.RIndexTrigger = cs.RIndexTrigger;
			this.LHandTrigger = cs.LHandTrigger;
			this.RHandTrigger = cs.RHandTrigger;
			this.LThumbstick = cs.LThumbstick;
			this.RThumbstick = cs.RThumbstick;
			this.LTouchpad = new OVRPlugin.Vector2f
			{
				x = 0f,
				y = 0f
			};
			this.RTouchpad = new OVRPlugin.Vector2f
			{
				x = 0f,
				y = 0f
			};
		}

		// Token: 0x04002CB2 RID: 11442
		public uint ConnectedControllers;

		// Token: 0x04002CB3 RID: 11443
		public uint Buttons;

		// Token: 0x04002CB4 RID: 11444
		public uint Touches;

		// Token: 0x04002CB5 RID: 11445
		public uint NearTouches;

		// Token: 0x04002CB6 RID: 11446
		public float LIndexTrigger;

		// Token: 0x04002CB7 RID: 11447
		public float RIndexTrigger;

		// Token: 0x04002CB8 RID: 11448
		public float LHandTrigger;

		// Token: 0x04002CB9 RID: 11449
		public float RHandTrigger;

		// Token: 0x04002CBA RID: 11450
		public OVRPlugin.Vector2f LThumbstick;

		// Token: 0x04002CBB RID: 11451
		public OVRPlugin.Vector2f RThumbstick;

		// Token: 0x04002CBC RID: 11452
		public OVRPlugin.Vector2f LTouchpad;

		// Token: 0x04002CBD RID: 11453
		public OVRPlugin.Vector2f RTouchpad;
	}

	// Token: 0x02000927 RID: 2343
	public struct ControllerState
	{
		// Token: 0x04002CBE RID: 11454
		public uint ConnectedControllers;

		// Token: 0x04002CBF RID: 11455
		public uint Buttons;

		// Token: 0x04002CC0 RID: 11456
		public uint Touches;

		// Token: 0x04002CC1 RID: 11457
		public uint NearTouches;

		// Token: 0x04002CC2 RID: 11458
		public float LIndexTrigger;

		// Token: 0x04002CC3 RID: 11459
		public float RIndexTrigger;

		// Token: 0x04002CC4 RID: 11460
		public float LHandTrigger;

		// Token: 0x04002CC5 RID: 11461
		public float RHandTrigger;

		// Token: 0x04002CC6 RID: 11462
		public OVRPlugin.Vector2f LThumbstick;

		// Token: 0x04002CC7 RID: 11463
		public OVRPlugin.Vector2f RThumbstick;
	}

	// Token: 0x02000928 RID: 2344
	public struct HapticsBuffer
	{
		// Token: 0x04002CC8 RID: 11464
		public IntPtr Samples;

		// Token: 0x04002CC9 RID: 11465
		public int SamplesCount;
	}

	// Token: 0x02000929 RID: 2345
	public struct HapticsState
	{
		// Token: 0x04002CCA RID: 11466
		public int SamplesAvailable;

		// Token: 0x04002CCB RID: 11467
		public int SamplesQueued;
	}

	// Token: 0x0200092A RID: 2346
	public struct HapticsDesc
	{
		// Token: 0x04002CCC RID: 11468
		public int SampleRateHz;

		// Token: 0x04002CCD RID: 11469
		public int SampleSizeInBytes;

		// Token: 0x04002CCE RID: 11470
		public int MinimumSafeSamplesQueued;

		// Token: 0x04002CCF RID: 11471
		public int MinimumBufferSamplesCount;

		// Token: 0x04002CD0 RID: 11472
		public int OptimalBufferSamplesCount;

		// Token: 0x04002CD1 RID: 11473
		public int MaximumBufferSamplesCount;
	}

	// Token: 0x0200092B RID: 2347
	public struct AppPerfFrameStats
	{
		// Token: 0x04002CD2 RID: 11474
		public int HmdVsyncIndex;

		// Token: 0x04002CD3 RID: 11475
		public int AppFrameIndex;

		// Token: 0x04002CD4 RID: 11476
		public int AppDroppedFrameCount;

		// Token: 0x04002CD5 RID: 11477
		public float AppMotionToPhotonLatency;

		// Token: 0x04002CD6 RID: 11478
		public float AppQueueAheadTime;

		// Token: 0x04002CD7 RID: 11479
		public float AppCpuElapsedTime;

		// Token: 0x04002CD8 RID: 11480
		public float AppGpuElapsedTime;

		// Token: 0x04002CD9 RID: 11481
		public int CompositorFrameIndex;

		// Token: 0x04002CDA RID: 11482
		public int CompositorDroppedFrameCount;

		// Token: 0x04002CDB RID: 11483
		public float CompositorLatency;

		// Token: 0x04002CDC RID: 11484
		public float CompositorCpuElapsedTime;

		// Token: 0x04002CDD RID: 11485
		public float CompositorGpuElapsedTime;

		// Token: 0x04002CDE RID: 11486
		public float CompositorCpuStartToGpuEndElapsedTime;

		// Token: 0x04002CDF RID: 11487
		public float CompositorGpuEndToVsyncElapsedTime;
	}

	// Token: 0x0200092C RID: 2348
	public struct AppPerfStats
	{
		// Token: 0x04002CE0 RID: 11488
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public OVRPlugin.AppPerfFrameStats[] FrameStats;

		// Token: 0x04002CE1 RID: 11489
		public int FrameStatsCount;

		// Token: 0x04002CE2 RID: 11490
		public OVRPlugin.Bool AnyFrameStatsDropped;

		// Token: 0x04002CE3 RID: 11491
		public float AdaptiveGpuPerformanceScale;
	}

	// Token: 0x0200092D RID: 2349
	public struct Sizei
	{
		// Token: 0x04002CE4 RID: 11492
		public int w;

		// Token: 0x04002CE5 RID: 11493
		public int h;
	}

	// Token: 0x0200092E RID: 2350
	public struct Sizef
	{
		// Token: 0x04002CE6 RID: 11494
		public float w;

		// Token: 0x04002CE7 RID: 11495
		public float h;
	}

	// Token: 0x0200092F RID: 2351
	public struct Vector2i
	{
		// Token: 0x04002CE8 RID: 11496
		public int x;

		// Token: 0x04002CE9 RID: 11497
		public int y;
	}

	// Token: 0x02000930 RID: 2352
	public struct Recti
	{
		// Token: 0x04002CEA RID: 11498
		private OVRPlugin.Vector2i Pos;

		// Token: 0x04002CEB RID: 11499
		private OVRPlugin.Sizei Size;
	}

	// Token: 0x02000931 RID: 2353
	public struct Rectf
	{
		// Token: 0x04002CEC RID: 11500
		private OVRPlugin.Vector2f Pos;

		// Token: 0x04002CED RID: 11501
		private OVRPlugin.Sizef Size;
	}

	// Token: 0x02000932 RID: 2354
	public struct Frustumf
	{
		// Token: 0x04002CEE RID: 11502
		public float zNear;

		// Token: 0x04002CEF RID: 11503
		public float zFar;

		// Token: 0x04002CF0 RID: 11504
		public float fovX;

		// Token: 0x04002CF1 RID: 11505
		public float fovY;
	}

	// Token: 0x02000933 RID: 2355
	public enum BoundaryType
	{
		// Token: 0x04002CF3 RID: 11507
		OuterBoundary = 1,
		// Token: 0x04002CF4 RID: 11508
		PlayArea = 256
	}

	// Token: 0x02000934 RID: 2356
	public struct BoundaryTestResult
	{
		// Token: 0x04002CF5 RID: 11509
		public OVRPlugin.Bool IsTriggering;

		// Token: 0x04002CF6 RID: 11510
		public float ClosestDistance;

		// Token: 0x04002CF7 RID: 11511
		public OVRPlugin.Vector3f ClosestPoint;

		// Token: 0x04002CF8 RID: 11512
		public OVRPlugin.Vector3f ClosestPointNormal;
	}

	// Token: 0x02000935 RID: 2357
	public struct BoundaryLookAndFeel
	{
		// Token: 0x04002CF9 RID: 11513
		public OVRPlugin.Colorf Color;
	}

	// Token: 0x02000936 RID: 2358
	public struct BoundaryGeometry
	{
		// Token: 0x04002CFA RID: 11514
		public OVRPlugin.BoundaryType BoundaryType;

		// Token: 0x04002CFB RID: 11515
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public OVRPlugin.Vector3f[] Points;

		// Token: 0x04002CFC RID: 11516
		public int PointsCount;
	}

	// Token: 0x02000937 RID: 2359
	public struct Colorf
	{
		// Token: 0x04002CFD RID: 11517
		public float r;

		// Token: 0x04002CFE RID: 11518
		public float g;

		// Token: 0x04002CFF RID: 11519
		public float b;

		// Token: 0x04002D00 RID: 11520
		public float a;
	}

	// Token: 0x02000938 RID: 2360
	public struct Fovf
	{
		// Token: 0x04002D01 RID: 11521
		public float UpTan;

		// Token: 0x04002D02 RID: 11522
		public float DownTan;

		// Token: 0x04002D03 RID: 11523
		public float LeftTan;

		// Token: 0x04002D04 RID: 11524
		public float RightTan;
	}

	// Token: 0x02000939 RID: 2361
	public struct CameraIntrinsics
	{
		// Token: 0x04002D05 RID: 11525
		public bool IsValid;

		// Token: 0x04002D06 RID: 11526
		public double LastChangedTimeSeconds;

		// Token: 0x04002D07 RID: 11527
		public OVRPlugin.Fovf FOVPort;

		// Token: 0x04002D08 RID: 11528
		public float VirtualNearPlaneDistanceMeters;

		// Token: 0x04002D09 RID: 11529
		public float VirtualFarPlaneDistanceMeters;

		// Token: 0x04002D0A RID: 11530
		public OVRPlugin.Sizei ImageSensorPixelResolution;
	}

	// Token: 0x0200093A RID: 2362
	public struct CameraExtrinsics
	{
		// Token: 0x04002D0B RID: 11531
		public bool IsValid;

		// Token: 0x04002D0C RID: 11532
		public double LastChangedTimeSeconds;

		// Token: 0x04002D0D RID: 11533
		public OVRPlugin.CameraStatus CameraStatusData;

		// Token: 0x04002D0E RID: 11534
		public OVRPlugin.Node AttachedToNode;

		// Token: 0x04002D0F RID: 11535
		public OVRPlugin.Posef RelativePose;
	}

	// Token: 0x0200093B RID: 2363
	public enum LayerLayout
	{
		// Token: 0x04002D11 RID: 11537
		Stereo,
		// Token: 0x04002D12 RID: 11538
		Mono,
		// Token: 0x04002D13 RID: 11539
		DoubleWide,
		// Token: 0x04002D14 RID: 11540
		Array,
		// Token: 0x04002D15 RID: 11541
		EnumSize = 15
	}

	// Token: 0x0200093C RID: 2364
	public enum LayerFlags
	{
		// Token: 0x04002D17 RID: 11543
		Static = 1,
		// Token: 0x04002D18 RID: 11544
		LoadingScreen,
		// Token: 0x04002D19 RID: 11545
		SymmetricFov = 4,
		// Token: 0x04002D1A RID: 11546
		TextureOriginAtBottomLeft = 8,
		// Token: 0x04002D1B RID: 11547
		ChromaticAberrationCorrection = 16,
		// Token: 0x04002D1C RID: 11548
		NoAllocation = 32,
		// Token: 0x04002D1D RID: 11549
		ProtectedContent = 64
	}

	// Token: 0x0200093D RID: 2365
	public struct LayerDesc
	{
		// Token: 0x06003AAB RID: 15019 RVA: 0x0011DFE8 File Offset: 0x0011C3E8
		public override string ToString()
		{
			string text = ", ";
			return string.Concat(new string[]
			{
				this.Shape.ToString(),
				text,
				this.Layout.ToString(),
				text,
				this.TextureSize.w.ToString(),
				"x",
				this.TextureSize.h.ToString(),
				text,
				this.MipLevels.ToString(),
				text,
				this.SampleCount.ToString(),
				text,
				this.Format.ToString(),
				text,
				this.LayerFlags.ToString()
			});
		}

		// Token: 0x04002D1E RID: 11550
		public OVRPlugin.OverlayShape Shape;

		// Token: 0x04002D1F RID: 11551
		public OVRPlugin.LayerLayout Layout;

		// Token: 0x04002D20 RID: 11552
		public OVRPlugin.Sizei TextureSize;

		// Token: 0x04002D21 RID: 11553
		public int MipLevels;

		// Token: 0x04002D22 RID: 11554
		public int SampleCount;

		// Token: 0x04002D23 RID: 11555
		public OVRPlugin.EyeTextureFormat Format;

		// Token: 0x04002D24 RID: 11556
		public int LayerFlags;

		// Token: 0x04002D25 RID: 11557
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public OVRPlugin.Fovf[] Fov;

		// Token: 0x04002D26 RID: 11558
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public OVRPlugin.Rectf[] VisibleRect;

		// Token: 0x04002D27 RID: 11559
		public OVRPlugin.Sizei MaxViewportSize;

		// Token: 0x04002D28 RID: 11560
		private OVRPlugin.EyeTextureFormat DepthFormat;
	}

	// Token: 0x0200093E RID: 2366
	public struct LayerSubmit
	{
		// Token: 0x04002D29 RID: 11561
		private int LayerId;

		// Token: 0x04002D2A RID: 11562
		private int TextureStage;

		// Token: 0x04002D2B RID: 11563
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		private OVRPlugin.Recti[] ViewportRect;

		// Token: 0x04002D2C RID: 11564
		private OVRPlugin.Posef Pose;

		// Token: 0x04002D2D RID: 11565
		private int LayerSubmitFlags;
	}

	// Token: 0x0200093F RID: 2367
	private static class OVRP_0_1_0
	{
		// Token: 0x06003AAC RID: 15020
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Sizei ovrp_GetEyeTextureSize(OVRPlugin.Eye eyeId);

		// Token: 0x06003AAD RID: 15021 RVA: 0x0011E0D7 File Offset: 0x0011C4D7
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_0_1_0()
		{
		}

		// Token: 0x04002D2E RID: 11566
		public static readonly Version version = new Version(0, 1, 0);
	}

	// Token: 0x02000940 RID: 2368
	private static class OVRP_0_1_1
	{
		// Token: 0x06003AAE RID: 15022
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetOverlayQuad2(OVRPlugin.Bool onTop, OVRPlugin.Bool headLocked, IntPtr texture, IntPtr device, OVRPlugin.Posef pose, OVRPlugin.Vector3f scale);

		// Token: 0x06003AAF RID: 15023 RVA: 0x0011E0E6 File Offset: 0x0011C4E6
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_0_1_1()
		{
		}

		// Token: 0x04002D2F RID: 11567
		public static readonly Version version = new Version(0, 1, 1);
	}

	// Token: 0x02000941 RID: 2369
	private static class OVRP_0_1_2
	{
		// Token: 0x06003AB0 RID: 15024
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodePose(OVRPlugin.Node nodeId);

		// Token: 0x06003AB1 RID: 15025
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetControllerVibration(uint controllerMask, float frequency, float amplitude);

		// Token: 0x06003AB2 RID: 15026 RVA: 0x0011E0F5 File Offset: 0x0011C4F5
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_0_1_2()
		{
		}

		// Token: 0x04002D30 RID: 11568
		public static readonly Version version = new Version(0, 1, 2);
	}

	// Token: 0x02000942 RID: 2370
	private static class OVRP_0_1_3
	{
		// Token: 0x06003AB3 RID: 15027
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeVelocity(OVRPlugin.Node nodeId);

		// Token: 0x06003AB4 RID: 15028
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeAcceleration(OVRPlugin.Node nodeId);

		// Token: 0x06003AB5 RID: 15029 RVA: 0x0011E104 File Offset: 0x0011C504
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_0_1_3()
		{
		}

		// Token: 0x04002D31 RID: 11569
		public static readonly Version version = new Version(0, 1, 3);
	}

	// Token: 0x02000943 RID: 2371
	private static class OVRP_0_5_0
	{
		// Token: 0x06003AB6 RID: 15030 RVA: 0x0011E113 File Offset: 0x0011C513
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_0_5_0()
		{
		}

		// Token: 0x04002D32 RID: 11570
		public static readonly Version version = new Version(0, 5, 0);
	}

	// Token: 0x02000944 RID: 2372
	private static class OVRP_1_0_0
	{
		// Token: 0x06003AB7 RID: 15031
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.TrackingOrigin ovrp_GetTrackingOriginType();

		// Token: 0x06003AB8 RID: 15032
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingOriginType(OVRPlugin.TrackingOrigin originType);

		// Token: 0x06003AB9 RID: 15033
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetTrackingCalibratedOrigin();

		// Token: 0x06003ABA RID: 15034
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_RecenterTrackingOrigin(uint flags);

		// Token: 0x06003ABB RID: 15035 RVA: 0x0011E122 File Offset: 0x0011C522
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_0_0()
		{
		}

		// Token: 0x04002D33 RID: 11571
		public static readonly Version version = new Version(1, 0, 0);
	}

	// Token: 0x02000945 RID: 2373
	private static class OVRP_1_1_0
	{
		// Token: 0x06003ABC RID: 15036
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetInitialized();

		// Token: 0x06003ABD RID: 15037
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetVersion")]
		private static extern IntPtr _ovrp_GetVersion();

		// Token: 0x06003ABE RID: 15038 RVA: 0x0011E131 File Offset: 0x0011C531
		public static string ovrp_GetVersion()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetVersion());
		}

		// Token: 0x06003ABF RID: 15039
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetNativeSDKVersion")]
		private static extern IntPtr _ovrp_GetNativeSDKVersion();

		// Token: 0x06003AC0 RID: 15040 RVA: 0x0011E13D File Offset: 0x0011C53D
		public static string ovrp_GetNativeSDKVersion()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetNativeSDKVersion());
		}

		// Token: 0x06003AC1 RID: 15041
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrp_GetAudioOutId();

		// Token: 0x06003AC2 RID: 15042
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrp_GetAudioInId();

		// Token: 0x06003AC3 RID: 15043
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetEyeTextureScale();

		// Token: 0x06003AC4 RID: 15044
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetEyeTextureScale(float value);

		// Token: 0x06003AC5 RID: 15045
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingOrientationSupported();

		// Token: 0x06003AC6 RID: 15046
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingOrientationEnabled();

		// Token: 0x06003AC7 RID: 15047
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingOrientationEnabled(OVRPlugin.Bool value);

		// Token: 0x06003AC8 RID: 15048
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingPositionSupported();

		// Token: 0x06003AC9 RID: 15049
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingPositionEnabled();

		// Token: 0x06003ACA RID: 15050
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingPositionEnabled(OVRPlugin.Bool value);

		// Token: 0x06003ACB RID: 15051
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetNodePresent(OVRPlugin.Node nodeId);

		// Token: 0x06003ACC RID: 15052
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetNodeOrientationTracked(OVRPlugin.Node nodeId);

		// Token: 0x06003ACD RID: 15053
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetNodePositionTracked(OVRPlugin.Node nodeId);

		// Token: 0x06003ACE RID: 15054
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Frustumf ovrp_GetNodeFrustum(OVRPlugin.Node nodeId);

		// Token: 0x06003ACF RID: 15055
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.ControllerState ovrp_GetControllerState(uint controllerMask);

		// Token: 0x06003AD0 RID: 15056
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemCpuLevel();

		// Token: 0x06003AD1 RID: 15057
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetSystemCpuLevel(int value);

		// Token: 0x06003AD2 RID: 15058
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemGpuLevel();

		// Token: 0x06003AD3 RID: 15059
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetSystemGpuLevel(int value);

		// Token: 0x06003AD4 RID: 15060
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetSystemPowerSavingMode();

		// Token: 0x06003AD5 RID: 15061
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemDisplayFrequency();

		// Token: 0x06003AD6 RID: 15062
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemVSyncCount();

		// Token: 0x06003AD7 RID: 15063
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemVolume();

		// Token: 0x06003AD8 RID: 15064
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BatteryStatus ovrp_GetSystemBatteryStatus();

		// Token: 0x06003AD9 RID: 15065
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemBatteryLevel();

		// Token: 0x06003ADA RID: 15066
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetSystemBatteryTemperature();

		// Token: 0x06003ADB RID: 15067
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetSystemProductName")]
		private static extern IntPtr _ovrp_GetSystemProductName();

		// Token: 0x06003ADC RID: 15068 RVA: 0x0011E149 File Offset: 0x0011C549
		public static string ovrp_GetSystemProductName()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetSystemProductName());
		}

		// Token: 0x06003ADD RID: 15069
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_ShowSystemUI(OVRPlugin.PlatformUI ui);

		// Token: 0x06003ADE RID: 15070
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppMonoscopic();

		// Token: 0x06003ADF RID: 15071
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetAppMonoscopic(OVRPlugin.Bool value);

		// Token: 0x06003AE0 RID: 15072
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppHasVrFocus();

		// Token: 0x06003AE1 RID: 15073
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppShouldQuit();

		// Token: 0x06003AE2 RID: 15074
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppShouldRecenter();

		// Token: 0x06003AE3 RID: 15075
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrp_GetAppLatencyTimings")]
		private static extern IntPtr _ovrp_GetAppLatencyTimings();

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0011E155 File Offset: 0x0011C555
		public static string ovrp_GetAppLatencyTimings()
		{
			return Marshal.PtrToStringAnsi(OVRPlugin.OVRP_1_1_0._ovrp_GetAppLatencyTimings());
		}

		// Token: 0x06003AE5 RID: 15077
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetUserPresent();

		// Token: 0x06003AE6 RID: 15078
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserIPD();

		// Token: 0x06003AE7 RID: 15079
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetUserIPD(float value);

		// Token: 0x06003AE8 RID: 15080
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserEyeDepth();

		// Token: 0x06003AE9 RID: 15081
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetUserEyeDepth(float value);

		// Token: 0x06003AEA RID: 15082
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetUserEyeHeight();

		// Token: 0x06003AEB RID: 15083
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetUserEyeHeight(float value);

		// Token: 0x06003AEC RID: 15084 RVA: 0x0011E161 File Offset: 0x0011C561
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_1_0()
		{
		}

		// Token: 0x04002D34 RID: 11572
		public static readonly Version version = new Version(1, 1, 0);
	}

	// Token: 0x02000946 RID: 2374
	private static class OVRP_1_2_0
	{
		// Token: 0x06003AED RID: 15085
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetSystemVSyncCount(int vsyncCount);

		// Token: 0x06003AEE RID: 15086
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrpi_SetTrackingCalibratedOrigin();

		// Token: 0x06003AEF RID: 15087 RVA: 0x0011E170 File Offset: 0x0011C570
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_2_0()
		{
		}

		// Token: 0x04002D35 RID: 11573
		public static readonly Version version = new Version(1, 2, 0);
	}

	// Token: 0x02000947 RID: 2375
	private static class OVRP_1_3_0
	{
		// Token: 0x06003AF0 RID: 15088
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetEyeOcclusionMeshEnabled();

		// Token: 0x06003AF1 RID: 15089
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetEyeOcclusionMeshEnabled(OVRPlugin.Bool value);

		// Token: 0x06003AF2 RID: 15090
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetSystemHeadphonesPresent();

		// Token: 0x06003AF3 RID: 15091 RVA: 0x0011E17F File Offset: 0x0011C57F
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_3_0()
		{
		}

		// Token: 0x04002D36 RID: 11574
		public static readonly Version version = new Version(1, 3, 0);
	}

	// Token: 0x02000948 RID: 2376
	private static class OVRP_1_5_0
	{
		// Token: 0x06003AF4 RID: 15092
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.SystemRegion ovrp_GetSystemRegion();

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0011E18E File Offset: 0x0011C58E
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_5_0()
		{
		}

		// Token: 0x04002D37 RID: 11575
		public static readonly Version version = new Version(1, 5, 0);
	}

	// Token: 0x02000949 RID: 2377
	private static class OVRP_1_6_0
	{
		// Token: 0x06003AF6 RID: 15094
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetTrackingIPDEnabled();

		// Token: 0x06003AF7 RID: 15095
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetTrackingIPDEnabled(OVRPlugin.Bool value);

		// Token: 0x06003AF8 RID: 15096
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.HapticsDesc ovrp_GetControllerHapticsDesc(uint controllerMask);

		// Token: 0x06003AF9 RID: 15097
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.HapticsState ovrp_GetControllerHapticsState(uint controllerMask);

		// Token: 0x06003AFA RID: 15098
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetControllerHaptics(uint controllerMask, OVRPlugin.HapticsBuffer hapticsBuffer);

		// Token: 0x06003AFB RID: 15099
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetOverlayQuad3(uint flags, IntPtr textureLeft, IntPtr textureRight, IntPtr device, OVRPlugin.Posef pose, OVRPlugin.Vector3f scale, int layerIndex);

		// Token: 0x06003AFC RID: 15100
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetEyeRecommendedResolutionScale();

		// Token: 0x06003AFD RID: 15101
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetAppCpuStartToGpuEndTime();

		// Token: 0x06003AFE RID: 15102
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovrp_GetSystemRecommendedMSAALevel();

		// Token: 0x06003AFF RID: 15103 RVA: 0x0011E19D File Offset: 0x0011C59D
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_6_0()
		{
		}

		// Token: 0x04002D38 RID: 11576
		public static readonly Version version = new Version(1, 6, 0);
	}

	// Token: 0x0200094A RID: 2378
	private static class OVRP_1_7_0
	{
		// Token: 0x06003B00 RID: 15104
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetAppChromaticCorrection();

		// Token: 0x06003B01 RID: 15105
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetAppChromaticCorrection(OVRPlugin.Bool value);

		// Token: 0x06003B02 RID: 15106 RVA: 0x0011E1AC File Offset: 0x0011C5AC
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_7_0()
		{
		}

		// Token: 0x04002D39 RID: 11577
		public static readonly Version version = new Version(1, 7, 0);
	}

	// Token: 0x0200094B RID: 2379
	private static class OVRP_1_8_0
	{
		// Token: 0x06003B03 RID: 15107
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetBoundaryConfigured();

		// Token: 0x06003B04 RID: 15108
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BoundaryTestResult ovrp_TestBoundaryNode(OVRPlugin.Node nodeId, OVRPlugin.BoundaryType boundaryType);

		// Token: 0x06003B05 RID: 15109
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BoundaryTestResult ovrp_TestBoundaryPoint(OVRPlugin.Vector3f point, OVRPlugin.BoundaryType boundaryType);

		// Token: 0x06003B06 RID: 15110
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetBoundaryLookAndFeel(OVRPlugin.BoundaryLookAndFeel lookAndFeel);

		// Token: 0x06003B07 RID: 15111
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_ResetBoundaryLookAndFeel();

		// Token: 0x06003B08 RID: 15112
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.BoundaryGeometry ovrp_GetBoundaryGeometry(OVRPlugin.BoundaryType boundaryType);

		// Token: 0x06003B09 RID: 15113
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Vector3f ovrp_GetBoundaryDimensions(OVRPlugin.BoundaryType boundaryType);

		// Token: 0x06003B0A RID: 15114
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetBoundaryVisible();

		// Token: 0x06003B0B RID: 15115
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetBoundaryVisible(OVRPlugin.Bool value);

		// Token: 0x06003B0C RID: 15116
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_Update2(int stateId, int frameIndex, double predictionSeconds);

		// Token: 0x06003B0D RID: 15117
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodePose2(int stateId, OVRPlugin.Node nodeId);

		// Token: 0x06003B0E RID: 15118
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeVelocity2(int stateId, OVRPlugin.Node nodeId);

		// Token: 0x06003B0F RID: 15119
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Posef ovrp_GetNodeAcceleration2(int stateId, OVRPlugin.Node nodeId);

		// Token: 0x06003B10 RID: 15120 RVA: 0x0011E1BB File Offset: 0x0011C5BB
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_8_0()
		{
		}

		// Token: 0x04002D3A RID: 11578
		public static readonly Version version = new Version(1, 8, 0);
	}

	// Token: 0x0200094C RID: 2380
	private static class OVRP_1_9_0
	{
		// Token: 0x06003B11 RID: 15121
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.SystemHeadset ovrp_GetSystemHeadsetType();

		// Token: 0x06003B12 RID: 15122
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Controller ovrp_GetActiveController();

		// Token: 0x06003B13 RID: 15123
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Controller ovrp_GetConnectedControllers();

		// Token: 0x06003B14 RID: 15124
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetBoundaryGeometry2(OVRPlugin.BoundaryType boundaryType, IntPtr points, ref int pointsCount);

		// Token: 0x06003B15 RID: 15125
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.AppPerfStats ovrp_GetAppPerfStats();

		// Token: 0x06003B16 RID: 15126
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_ResetAppPerfStats();

		// Token: 0x06003B17 RID: 15127 RVA: 0x0011E1CA File Offset: 0x0011C5CA
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_9_0()
		{
		}

		// Token: 0x04002D3B RID: 11579
		public static readonly Version version = new Version(1, 9, 0);
	}

	// Token: 0x0200094D RID: 2381
	private static class OVRP_1_10_0
	{
		// Token: 0x06003B18 RID: 15128 RVA: 0x0011E1DA File Offset: 0x0011C5DA
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_10_0()
		{
		}

		// Token: 0x04002D3C RID: 11580
		public static readonly Version version = new Version(1, 10, 0);
	}

	// Token: 0x0200094E RID: 2382
	private static class OVRP_1_11_0
	{
		// Token: 0x06003B19 RID: 15129
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_SetDesiredEyeTextureFormat(OVRPlugin.EyeTextureFormat value);

		// Token: 0x06003B1A RID: 15130
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.EyeTextureFormat ovrp_GetDesiredEyeTextureFormat();

		// Token: 0x06003B1B RID: 15131 RVA: 0x0011E1EA File Offset: 0x0011C5EA
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_11_0()
		{
		}

		// Token: 0x04002D3D RID: 11581
		public static readonly Version version = new Version(1, 11, 0);
	}

	// Token: 0x0200094F RID: 2383
	private static class OVRP_1_12_0
	{
		// Token: 0x06003B1C RID: 15132
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrp_GetAppFramerate();

		// Token: 0x06003B1D RID: 15133
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.PoseStatef ovrp_GetNodePoseState(OVRPlugin.Step stepId, OVRPlugin.Node nodeId);

		// Token: 0x06003B1E RID: 15134
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.ControllerState2 ovrp_GetControllerState2(uint controllerMask);

		// Token: 0x06003B1F RID: 15135 RVA: 0x0011E1FA File Offset: 0x0011C5FA
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_12_0()
		{
		}

		// Token: 0x04002D3E RID: 11582
		public static readonly Version version = new Version(1, 12, 0);
	}

	// Token: 0x02000950 RID: 2384
	private static class OVRP_1_15_0
	{
		// Token: 0x06003B20 RID: 15136
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_InitializeMixedReality();

		// Token: 0x06003B21 RID: 15137
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_ShutdownMixedReality();

		// Token: 0x06003B22 RID: 15138
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_GetMixedRealityInitialized();

		// Token: 0x06003B23 RID: 15139
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_UpdateExternalCamera();

		// Token: 0x06003B24 RID: 15140
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetExternalCameraCount(out int cameraCount);

		// Token: 0x06003B25 RID: 15141
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetExternalCameraName(int cameraId, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] char[] cameraName);

		// Token: 0x06003B26 RID: 15142
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetExternalCameraIntrinsics(int cameraId, out OVRPlugin.CameraIntrinsics cameraIntrinsics);

		// Token: 0x06003B27 RID: 15143
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetExternalCameraExtrinsics(int cameraId, out OVRPlugin.CameraExtrinsics cameraExtrinsics);

		// Token: 0x06003B28 RID: 15144
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_CalculateLayerDesc(OVRPlugin.OverlayShape shape, OVRPlugin.LayerLayout layout, ref OVRPlugin.Sizei textureSize, int mipLevels, int sampleCount, OVRPlugin.EyeTextureFormat format, int layerFlags, ref OVRPlugin.LayerDesc layerDesc);

		// Token: 0x06003B29 RID: 15145
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_EnqueueSetupLayer(ref OVRPlugin.LayerDesc desc, IntPtr layerId);

		// Token: 0x06003B2A RID: 15146
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_EnqueueDestroyLayer(IntPtr layerId);

		// Token: 0x06003B2B RID: 15147
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetLayerTextureStageCount(int layerId, ref int layerTextureStageCount);

		// Token: 0x06003B2C RID: 15148
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetLayerTexturePtr(int layerId, int stage, OVRPlugin.Eye eyeId, ref IntPtr textureHandle);

		// Token: 0x06003B2D RID: 15149
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_EnqueueSubmitLayer(uint flags, IntPtr textureLeft, IntPtr textureRight, int layerId, int frameIndex, ref OVRPlugin.Posef pose, ref OVRPlugin.Vector3f scale, int layerIndex);

		// Token: 0x06003B2E RID: 15150 RVA: 0x0011E20A File Offset: 0x0011C60A
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_15_0()
		{
		}

		// Token: 0x04002D3F RID: 11583
		public const int OVRP_EXTERNAL_CAMERA_NAME_SIZE = 32;

		// Token: 0x04002D40 RID: 11584
		public static readonly Version version = new Version(1, 15, 0);
	}

	// Token: 0x02000951 RID: 2385
	private static class OVRP_1_16_0
	{
		// Token: 0x06003B2F RID: 15151
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_UpdateCameraDevices();

		// Token: 0x06003B30 RID: 15152
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_IsCameraDeviceAvailable(OVRPlugin.CameraDevice cameraDevice);

		// Token: 0x06003B31 RID: 15153
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_SetCameraDevicePreferredColorFrameSize(OVRPlugin.CameraDevice cameraDevice, OVRPlugin.Sizei preferredColorFrameSize);

		// Token: 0x06003B32 RID: 15154
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_OpenCameraDevice(OVRPlugin.CameraDevice cameraDevice);

		// Token: 0x06003B33 RID: 15155
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_CloseCameraDevice(OVRPlugin.CameraDevice cameraDevice);

		// Token: 0x06003B34 RID: 15156
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_HasCameraDeviceOpened(OVRPlugin.CameraDevice cameraDevice);

		// Token: 0x06003B35 RID: 15157
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Bool ovrp_IsCameraDeviceColorFrameAvailable(OVRPlugin.CameraDevice cameraDevice);

		// Token: 0x06003B36 RID: 15158
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDeviceColorFrameSize(OVRPlugin.CameraDevice cameraDevice, out OVRPlugin.Sizei colorFrameSize);

		// Token: 0x06003B37 RID: 15159
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDeviceColorFrameBgraPixels(OVRPlugin.CameraDevice cameraDevice, out IntPtr colorFrameBgraPixels, out int colorFrameRowPitch);

		// Token: 0x06003B38 RID: 15160
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetControllerState4(uint controllerMask, ref OVRPlugin.ControllerState4 controllerState);

		// Token: 0x06003B39 RID: 15161 RVA: 0x0011E21A File Offset: 0x0011C61A
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_16_0()
		{
		}

		// Token: 0x04002D41 RID: 11585
		public static readonly Version version = new Version(1, 16, 0);
	}

	// Token: 0x02000952 RID: 2386
	private static class OVRP_1_17_0
	{
		// Token: 0x06003B3A RID: 15162
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetExternalCameraPose(OVRPlugin.CameraDevice camera, out OVRPlugin.Posef cameraPose);

		// Token: 0x06003B3B RID: 15163
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_ConvertPoseToCameraSpace(OVRPlugin.CameraDevice camera, ref OVRPlugin.Posef trackingSpacePose, out OVRPlugin.Posef cameraSpacePose);

		// Token: 0x06003B3C RID: 15164
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDeviceIntrinsicsParameters(OVRPlugin.CameraDevice camera, out OVRPlugin.Bool supportIntrinsics, out OVRPlugin.CameraDeviceIntrinsicsParameters intrinsicsParameters);

		// Token: 0x06003B3D RID: 15165
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_DoesCameraDeviceSupportDepth(OVRPlugin.CameraDevice camera, out OVRPlugin.Bool supportDepth);

		// Token: 0x06003B3E RID: 15166
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthSensingMode(OVRPlugin.CameraDevice camera, out OVRPlugin.CameraDeviceDepthSensingMode depthSensoringMode);

		// Token: 0x06003B3F RID: 15167
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_SetCameraDeviceDepthSensingMode(OVRPlugin.CameraDevice camera, OVRPlugin.CameraDeviceDepthSensingMode depthSensoringMode);

		// Token: 0x06003B40 RID: 15168
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDevicePreferredDepthQuality(OVRPlugin.CameraDevice camera, out OVRPlugin.CameraDeviceDepthQuality depthQuality);

		// Token: 0x06003B41 RID: 15169
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_SetCameraDevicePreferredDepthQuality(OVRPlugin.CameraDevice camera, OVRPlugin.CameraDeviceDepthQuality depthQuality);

		// Token: 0x06003B42 RID: 15170
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_IsCameraDeviceDepthFrameAvailable(OVRPlugin.CameraDevice camera, out OVRPlugin.Bool available);

		// Token: 0x06003B43 RID: 15171
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthFrameSize(OVRPlugin.CameraDevice camera, out OVRPlugin.Sizei depthFrameSize);

		// Token: 0x06003B44 RID: 15172
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthFramePixels(OVRPlugin.CameraDevice cameraDevice, out IntPtr depthFramePixels, out int depthFrameRowPitch);

		// Token: 0x06003B45 RID: 15173
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetCameraDeviceDepthConfidencePixels(OVRPlugin.CameraDevice cameraDevice, out IntPtr depthConfidencePixels, out int depthConfidenceRowPitch);

		// Token: 0x06003B46 RID: 15174 RVA: 0x0011E22A File Offset: 0x0011C62A
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_17_0()
		{
		}

		// Token: 0x04002D42 RID: 11586
		public static readonly Version version = new Version(1, 17, 0);
	}

	// Token: 0x02000953 RID: 2387
	private static class OVRP_1_18_0
	{
		// Token: 0x06003B47 RID: 15175
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_SetHandNodePoseStateLatency(double latencyInSeconds);

		// Token: 0x06003B48 RID: 15176
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetHandNodePoseStateLatency(out double latencyInSeconds);

		// Token: 0x06003B49 RID: 15177
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetAppHasInputFocus(out OVRPlugin.Bool appHasInputFocus);

		// Token: 0x06003B4A RID: 15178 RVA: 0x0011E23A File Offset: 0x0011C63A
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_18_0()
		{
		}

		// Token: 0x04002D43 RID: 11587
		public static readonly Version version = new Version(1, 18, 0);
	}

	// Token: 0x02000954 RID: 2388
	private static class OVRP_1_19_0
	{
		// Token: 0x06003B4B RID: 15179 RVA: 0x0011E24A File Offset: 0x0011C64A
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_19_0()
		{
		}

		// Token: 0x04002D44 RID: 11588
		public static readonly Version version = new Version(1, 19, 0);
	}

	// Token: 0x02000955 RID: 2389
	private static class OVRP_1_21_0
	{
		// Token: 0x06003B4C RID: 15180
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetTiledMultiResSupported(out OVRPlugin.Bool foveationSupported);

		// Token: 0x06003B4D RID: 15181
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetTiledMultiResLevel(out OVRPlugin.TiledMultiResLevel level);

		// Token: 0x06003B4E RID: 15182
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_SetTiledMultiResLevel(OVRPlugin.TiledMultiResLevel level);

		// Token: 0x06003B4F RID: 15183
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetGPUUtilSupported(out OVRPlugin.Bool gpuUtilSupported);

		// Token: 0x06003B50 RID: 15184
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetGPUUtilLevel(out float gpuUtil);

		// Token: 0x06003B51 RID: 15185
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetSystemDisplayFrequency2(out float systemDisplayFrequency);

		// Token: 0x06003B52 RID: 15186
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_GetSystemDisplayAvailableFrequencies(IntPtr systemDisplayAvailableFrequencies, out int numFrequencies);

		// Token: 0x06003B53 RID: 15187
		[DllImport("OVRPlugin", CallingConvention = CallingConvention.Cdecl)]
		public static extern OVRPlugin.Result ovrp_SetSystemDisplayFrequency(float requestedFrequency);

		// Token: 0x06003B54 RID: 15188 RVA: 0x0011E25A File Offset: 0x0011C65A
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_21_0()
		{
		}

		// Token: 0x04002D45 RID: 11589
		public static readonly Version version = new Version(1, 21, 0);
	}

	// Token: 0x02000956 RID: 2390
	private static class OVRP_1_25_1
	{
		// Token: 0x06003B55 RID: 15189 RVA: 0x0011E26A File Offset: 0x0011C66A
		// Note: this type is marked as 'beforefieldinit'.
		static OVRP_1_25_1()
		{
		}

		// Token: 0x04002D46 RID: 11590
		public static readonly Version version = new Version(1, 25, 1);
	}
}
