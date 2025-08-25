using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using DynamicCSharp;
using MeshVR;
using MeshVR.Hands;
using MK.Glow;
using MVR.FileManagement;
using MVR.Hub;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000C7C RID: 3196
[ExecuteInEditMode]
public class UserPreferences : MonoBehaviour
{
	// Token: 0x06005F81 RID: 24449 RVA: 0x00240D94 File Offset: 0x0023F194
	public UserPreferences()
	{
	}

	// Token: 0x17000DFF RID: 3583
	// (get) Token: 0x06005F82 RID: 24450 RVA: 0x00240F36 File Offset: 0x0023F336
	public bool shouldLoadPrefsFileOnStart
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.loadPrefsFileOnStart;
			}
			return this.loadPrefsFileOnStart;
		}
	}

	// Token: 0x06005F83 RID: 24451 RVA: 0x00240F5C File Offset: 0x0023F35C
	public void SetQuality(string qualityName)
	{
		UserPreferences.QualityLevel qualityLevel;
		if (UserPreferences.QualityLevels.levels.TryGetValue(qualityName, out qualityLevel))
		{
			this._disableSave = true;
			this.renderScale = qualityLevel.renderScale;
			this.msaaLevel = qualityLevel.msaaLevel;
			this.pixelLightCount = qualityLevel.pixelLightCount;
			this.shaderLOD = qualityLevel.shaderLOD;
			this.smoothPasses = qualityLevel.smoothPasses;
			this.mirrorReflections = qualityLevel.mirrorReflections;
			this.realtimeReflectionProbes = qualityLevel.realtimeReflectionProbes;
			this.closeObjectBlur = qualityLevel.closeObjectBlur;
			this.softPhysics = qualityLevel.softPhysics;
			this.glowEffects = qualityLevel.glowEffects;
			this._disableSave = false;
			this.SavePreferences();
		}
		else
		{
			UnityEngine.Debug.LogError("Could not find quality level " + qualityName);
		}
	}

	// Token: 0x06005F84 RID: 24452 RVA: 0x0024101C File Offset: 0x0023F41C
	private void CheckQualityLevels()
	{
		bool flag = this.CheckQualityLevel("UltraLow");
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool isOn = false;
		if (!flag)
		{
			flag2 = this.CheckQualityLevel("Low");
			if (!flag2)
			{
				flag3 = this.CheckQualityLevel("Mid");
				if (!flag3)
				{
					flag4 = this.CheckQualityLevel("High");
					if (!flag4)
					{
						flag5 = this.CheckQualityLevel("Ultra");
						if (!flag5)
						{
							flag6 = this.CheckQualityLevel("Max");
							if (!flag6)
							{
								isOn = true;
							}
						}
					}
				}
			}
		}
		this._disableToggles = true;
		if (this.ultraLowQualityToggle != null)
		{
			this.ultraLowQualityToggle.isOn = flag;
		}
		else if (flag)
		{
			isOn = true;
		}
		if (this.lowQualityToggle != null)
		{
			this.lowQualityToggle.isOn = flag2;
		}
		else if (flag2)
		{
			isOn = true;
		}
		if (this.midQualityToggle != null)
		{
			this.midQualityToggle.isOn = flag3;
		}
		else if (flag3)
		{
			isOn = true;
		}
		if (this.highQualityToggle != null)
		{
			this.highQualityToggle.isOn = flag4;
		}
		else if (flag4)
		{
			isOn = true;
		}
		if (this.ultraQualityToggle != null)
		{
			this.ultraQualityToggle.isOn = flag5;
		}
		else if (flag5)
		{
			isOn = true;
		}
		if (this.maxQualityToggle != null)
		{
			this.maxQualityToggle.isOn = flag6;
		}
		else if (flag6)
		{
			isOn = true;
		}
		if (this.customQualityToggle != null)
		{
			this.customQualityToggle.isOn = isOn;
		}
		this._disableToggles = false;
	}

	// Token: 0x06005F85 RID: 24453 RVA: 0x002411E0 File Offset: 0x0023F5E0
	private bool CheckQualityLevel(string qualityName)
	{
		bool result = false;
		UserPreferences.QualityLevel qualityLevel;
		if (UserPreferences.QualityLevels.levels.TryGetValue(qualityName, out qualityLevel))
		{
			result = true;
			if (this._renderScale != qualityLevel.renderScale || this._msaaLevel != qualityLevel.msaaLevel || this._pixelLightCount != qualityLevel.pixelLightCount || this._shaderLOD != qualityLevel.shaderLOD || this._smoothPasses != qualityLevel.smoothPasses || this._mirrorReflections != qualityLevel.mirrorReflections || this._realtimeReflectionProbes != qualityLevel.realtimeReflectionProbes || this._closeObjectBlur != qualityLevel.closeObjectBlur || this._softPhysics != qualityLevel.softPhysics || this._glowEffects != qualityLevel.glowEffects)
			{
				result = false;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Could not find quality level " + qualityName);
		}
		return result;
	}

	// Token: 0x06005F86 RID: 24454 RVA: 0x002412C8 File Offset: 0x0023F6C8
	public void SavePreferences()
	{
		if (!this._disableSave && Application.isPlaying)
		{
			JSONClass jsonclass = new JSONClass();
			jsonclass["firstTimeUser"].AsBool = this._firstTimeUser;
			jsonclass["renderScale"].AsFloat = this._renderScale;
			jsonclass["msaaLevel"].AsInt = this._msaaLevel;
			jsonclass["desktopVsync"].AsBool = this._desktopVsync;
			jsonclass["pixelLightCount"].AsInt = this._pixelLightCount;
			jsonclass["shaderLOD"] = this._shaderLOD.ToString();
			jsonclass["smoothPasses"].AsInt = this._smoothPasses;
			jsonclass["mirrorReflections"].AsBool = this._mirrorReflections;
			jsonclass["realtimeReflectionProbes"].AsBool = this._realtimeReflectionProbes;
			jsonclass["mirrorToDisplay"].AsBool = this._mirrorToDisplay;
			jsonclass["hideExitButton"].AsBool = this._hideExitButton;
			jsonclass["showTargetsMenuOnlyV2"].AsBool = this._showTargetsMenuOnly;
			jsonclass["alwaysShowPointersOnTouch"].AsBool = this._alwaysShowPointersOnTouch;
			jsonclass["hideInactiveTargets"].AsBool = this._hideInactiveTargets;
			jsonclass["showControllersMenuOnly"].AsBool = this._showControllersMenuOnly;
			jsonclass["targetAlpha"].AsFloat = this._targetAlpha;
			jsonclass["crosshairAlpha"].AsFloat = this._crosshairAlpha;
			jsonclass["useMonitorViewOffsetWhenUIOpen"].AsBool = this._useMonitorViewOffsetWhenUIOpen;
			jsonclass["overlayUI"].AsBool = this._overlayUI;
			jsonclass["oculusSwapGrabAndTrigger"].AsBool = this._oculusSwapGrabAndTrigger;
			jsonclass["oculusDisableFreeMove"].AsBool = this._oculusDisableFreeMove;
			jsonclass["steamVRShowControllers"].AsBool = this._steamVRShowControllers;
			jsonclass["steamVRUseControllerHandPose"].AsBool = this._steamVRUseControllerHandPose;
			jsonclass["steamVRPointerAngle"].AsFloat = this._steamVRPointerAngle;
			jsonclass["fingerInputFactor"].AsFloat = this._fingerInputFactor;
			jsonclass["thumbInputFactor"].AsFloat = this._thumbInputFactor;
			jsonclass["fingerSpreadOffset"].AsFloat = this._fingerSpreadOffset;
			jsonclass["fingerBendOffset"].AsFloat = this._fingerBendOffset;
			jsonclass["thumbSpreadOffset"].AsFloat = this._thumbSpreadOffset;
			jsonclass["thumbBendOffset"].AsFloat = this._thumbBendOffset;
			jsonclass["physicsRate"] = this.physicsRate.ToString();
			jsonclass["physicsUpdateCap"] = this.physicsUpdateCap.ToString();
			jsonclass["physicsHighQuality"].AsBool = this.physicsHighQuality;
			jsonclass["softBodyPhysics"].AsBool = this._softPhysics;
			jsonclass["glowEffects"] = this._glowEffects.ToString();
			jsonclass["useHeadCollider"].AsBool = this._useHeadCollider;
			jsonclass["optimizeMemoryOnSceneLoad"].AsBool = this._optimizeMemoryOnSceneLoad;
			jsonclass["optimizeMemoryOnPresetLoad"].AsBool = this._optimizeMemoryOnPresetLoad;
			jsonclass["enableCaching"].AsBool = this._enableCaching;
			jsonclass["cacheFolder"] = this._cacheFolder;
			jsonclass["confirmLoad"].AsBool = this._confirmLoad;
			jsonclass["flipToolbar"].AsBool = this._flipToolbar;
			jsonclass["enableWebBrowser"].AsBool = this._enableWebBrowser;
			jsonclass["allowNonWhitelistDomains"].AsBool = this._allowNonWhitelistDomains;
			jsonclass["enableWebBrowserProfile"].AsBool = this._enableWebBrowserProfile;
			jsonclass["enableWebMisc"].AsBool = this._enableWebMisc;
			jsonclass["enableHub"].AsBool = this._enableHub;
			jsonclass["enableHubDownloader"].AsBool = this._enableHubDownloader;
			jsonclass["enablePlugins"].AsBool = this._enablePlugins;
			jsonclass["allowPluginsNetworkAccess"].AsBool = this._allowPluginsNetworkAccess;
			jsonclass["alwaysAllowPluginsDownloadedFromHub"].AsBool = this._alwaysAllowPluginsDownloadedFromHub;
			jsonclass["hideDisabledWebMessages"].AsBool = this._hideDisabledWebMessages;
			if (SuperController.singleton != null)
			{
				if (!SuperController.singleton.disableTermsOfUse)
				{
					jsonclass["termsOfUseAccepted"].AsBool = this._termsOfUseAccepted;
				}
				jsonclass["showHelpOverlay"].AsBool = SuperController.singleton.helpOverlayOn;
				jsonclass["lockHeightDuringNavigate"].AsBool = SuperController.singleton.lockHeightDuringNavigate;
				jsonclass["freeMoveFollowFloor"].AsBool = SuperController.singleton.freeMoveFollowFloor;
				jsonclass["disableAllNavigation"].AsBool = SuperController.singleton.disableAllNavigation;
				jsonclass["disableGrabNavigation"].AsBool = SuperController.singleton.disableGrabNavigation;
				jsonclass["disableTeleport"].AsBool = SuperController.singleton.disableTeleport;
				jsonclass["teleportAllowRotation"].AsBool = SuperController.singleton.teleportAllowRotation;
				jsonclass["disableTeleportDuringPossess"].AsBool = SuperController.singleton.disableTeleportDuringPossess;
				jsonclass["freeMoveMultiplier"].AsFloat = SuperController.singleton.freeMoveMultiplier;
				jsonclass["grabNavigationPositionMultiplier"].AsFloat = SuperController.singleton.grabNavigationPositionMultiplier;
				jsonclass["grabNavigationRotationMultiplier"].AsFloat = SuperController.singleton.grabNavigationRotationMultiplier;
				jsonclass["showNavigationHologrid"].AsBool = SuperController.singleton.showNavigationHologrid;
				jsonclass["hologridTransparency"].AsFloat = SuperController.singleton.hologridTransparency;
				jsonclass["oculusThumbstickFunction"] = SuperController.singleton.oculusThumbstickFunction.ToString();
				jsonclass["allowPossessSpringAdjustment"].AsBool = SuperController.singleton.allowPossessSpringAdjustment;
				jsonclass["possessPositionSpring"].AsFloat = SuperController.singleton.possessPositionSpring;
				jsonclass["possessRotationSpring"].AsFloat = SuperController.singleton.possessRotationSpring;
				jsonclass["loResScreenshotCameraFOV"].AsFloat = SuperController.singleton.loResScreenShotCameraFOV;
				jsonclass["hiResScreenshotCameraFOV"].AsFloat = SuperController.singleton.hiResScreenShotCameraFOV;
				jsonclass["allowGrabPlusTriggerHandToggle"].AsBool = SuperController.singleton.allowGrabPlusTriggerHandToggle;
				jsonclass["monitorUIScale"].AsFloat = SuperController.singleton.monitorUIScale;
				jsonclass["monitorUIYOffset"].AsFloat = SuperController.singleton.monitorUIYOffset;
				jsonclass["VRUISide"] = SuperController.singleton.UISide.ToString();
				jsonclass["motionControllerAlwaysUseAlternateHands"].AsBool = SuperController.singleton.alwaysUseAlternateHands;
				jsonclass["useLegacyWorldScaleChange"].AsBool = SuperController.singleton.useLegacyWorldScaleChange;
				jsonclass["onStartupSkipStartScreen"].AsBool = SuperController.singleton.onStartupSkipStartScreen;
				jsonclass["autoFreezeAnimationOnSwitchToEditMode"].AsBool = SuperController.singleton.autoFreezeAnimationOnSwitchToEditMode;
				jsonclass["worldUIVRAnchorDistance"].AsFloat = SuperController.singleton.worldUIVRAnchorDistance;
				jsonclass["worldUIVRAnchorHeight"].AsFloat = SuperController.singleton.worldUIVRAnchorHeight;
				jsonclass["useMonitorRigAudioListenerWhenActive"].AsBool = SuperController.singleton.useMonitorRigAudioListenerWhenActive;
				jsonclass["generateDepthTexture"].AsBool = SuperController.singleton.generateDepthTexture;
				jsonclass["leapMotionEnabled"].AsBool = SuperController.singleton.leapMotionEnabled;
				jsonclass["openMainHUDOnError"].AsBool = SuperController.singleton.openMainHUDOnError;
			}
			if (this.leapHandModelControl != null)
			{
				jsonclass["leapMotionAllowPinchGrab"].AsBool = this.leapHandModelControl.allowPinchGrab;
			}
			if (this.motionHandModelControl != null)
			{
				jsonclass["motionControllerLeftHandChoice"] = this.motionHandModelControl.leftHandChoice;
				jsonclass["motionControllerRightHandChoice"] = this.motionHandModelControl.rightHandChoice;
				jsonclass["motionControllerLinkHands"].AsBool = this.motionHandModelControl.linkHands;
				jsonclass["motionControllerUseCollision"].AsBool = this.motionHandModelControl.useCollision;
				jsonclass["motionControllerHandsPositionOffset"]["x"].AsFloat = this.motionHandModelControl.xPosition;
				jsonclass["motionControllerHandsPositionOffset"]["y"].AsFloat = this.motionHandModelControl.yPosition;
				jsonclass["motionControllerHandsPositionOffset"]["z"].AsFloat = this.motionHandModelControl.zPosition;
				jsonclass["motionControllerHandsRotationOffset"]["x"].AsFloat = this.motionHandModelControl.xRotation;
				jsonclass["motionControllerHandsRotationOffset"]["y"].AsFloat = this.motionHandModelControl.yRotation;
				jsonclass["motionControllerHandsRotationOffset"]["z"].AsFloat = this.motionHandModelControl.zRotation;
			}
			if (this.alternateMotionHandModelControl != null)
			{
				jsonclass["alternateMotionControllerLeftHandChoice"] = this.alternateMotionHandModelControl.leftHandChoice;
				jsonclass["alternateMotionControllerRightHandChoice"] = this.alternateMotionHandModelControl.rightHandChoice;
				jsonclass["alternateMotionControllerLinkHands"].AsBool = this.alternateMotionHandModelControl.linkHands;
			}
			jsonclass["creatorName"] = this._creatorName;
			jsonclass["DAZExtraLibraryFolder"] = this._DAZExtraLibraryFolder;
			jsonclass["DAZDefaultContentFolder"] = this._DAZDefaultContentFolder;
			jsonclass["fileBrowserSortBy"] = this._fileBrowserSortBy.ToString();
			jsonclass["fileBrowserDirectoryOption"] = this._fileBrowserDirectoryOption.ToString();
			string value = jsonclass.ToString(string.Empty);
			StreamWriter streamWriter = new StreamWriter("prefs.json");
			streamWriter.Write(value);
			streamWriter.Close();
			this.CheckQualityLevels();
		}
	}

	// Token: 0x06005F87 RID: 24455 RVA: 0x00241DE4 File Offset: 0x002401E4
	public void RestorePreferences()
	{
		if (Application.isPlaying)
		{
			string path = "prefs.json";
			this._disableSave = true;
			if (File.Exists(path))
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(path))
					{
						string aJSON = streamReader.ReadToEnd();
						SimpleJSON.JSONNode jsonnode = JSON.Parse(aJSON);
						if (jsonnode["firstTimeUser"] != null)
						{
							this.firstTimeUser = jsonnode["firstTimeUser"].AsBool;
						}
						if (jsonnode["renderScale"] != null)
						{
							this.renderScale = jsonnode["renderScale"].AsFloat;
						}
						if (jsonnode["msaaLevel"] != null)
						{
							this.msaaLevel = jsonnode["msaaLevel"].AsInt;
						}
						if (jsonnode["desktopVsync"] != null)
						{
							this.desktopVsync = jsonnode["desktopVsync"].AsBool;
						}
						if (jsonnode["pixelLightCount"] != null)
						{
							this.pixelLightCount = jsonnode["pixelLightCount"].AsInt;
						}
						if (jsonnode["shaderLOD"] != null)
						{
							this.SetShaderLODFromString(jsonnode["shaderLOD"]);
						}
						if (jsonnode["smoothPasses"] != null)
						{
							this.smoothPasses = jsonnode["smoothPasses"].AsInt;
						}
						if (jsonnode["mirrorToDisplay"] != null)
						{
							this.mirrorToDisplay = jsonnode["mirrorToDisplay"].AsBool;
						}
						if (jsonnode["hideExitButton"] != null)
						{
							this.hideExitButton = jsonnode["hideExitButton"].AsBool;
						}
						if (jsonnode["mirrorReflections"] != null)
						{
							this.mirrorReflections = jsonnode["mirrorReflections"].AsBool;
						}
						if (jsonnode["realtimeReflectionProbes"] != null)
						{
							this.realtimeReflectionProbes = jsonnode["realtimeReflectionProbes"].AsBool;
						}
						if (jsonnode["showTargetsMenuOnlyV2"] != null)
						{
							this.showTargetsMenuOnly = jsonnode["showTargetsMenuOnlyV2"].AsBool;
						}
						if (jsonnode["alwaysShowPointersOnTouch"] != null)
						{
							this.alwaysShowPointersOnTouch = jsonnode["alwaysShowPointersOnTouch"].AsBool;
						}
						if (jsonnode["hideInactiveTargets"] != null)
						{
							this.hideInactiveTargets = jsonnode["hideInactiveTargets"].AsBool;
						}
						if (jsonnode["showControllersMenuOnly"] != null)
						{
							this.showControllersMenuOnly = jsonnode["showControllersMenuOnly"].AsBool;
						}
						if (jsonnode["overlayUI"] != null)
						{
							this.overlayUI = jsonnode["overlayUI"].AsBool;
						}
						if (jsonnode["targetAlpha"] != null)
						{
							this.targetAlpha = jsonnode["targetAlpha"].AsFloat;
						}
						if (jsonnode["crosshairAlpha"] != null)
						{
							this.crosshairAlpha = jsonnode["crosshairAlpha"].AsFloat;
						}
						if (jsonnode["useMonitorViewOffsetWhenUIOpen"] != null)
						{
							this.useMonitorViewOffsetWhenUIOpen = jsonnode["useMonitorViewOffsetWhenUIOpen"].AsBool;
						}
						if (jsonnode["oculusSwapGrabAndTrigger"] != null)
						{
							this.oculusSwapGrabAndTrigger = jsonnode["oculusSwapGrabAndTrigger"].AsBool;
						}
						if (jsonnode["oculusDisableFreeMove"] != null)
						{
							this.oculusDisableFreeMove = jsonnode["oculusDisableFreeMove"].AsBool;
						}
						if (jsonnode["steamVRShowControllers"] != null)
						{
							this.steamVRShowControllers = jsonnode["steamVRShowControllers"].AsBool;
						}
						if (jsonnode["steamVRUseControllerHandPose"] != null)
						{
							this.steamVRUseControllerHandPose = jsonnode["steamVRUseControllerHandPose"].AsBool;
						}
						if (jsonnode["steamVRPointerAngle"] != null)
						{
							this.steamVRPointerAngle = jsonnode["steamVRPointerAngle"].AsFloat;
						}
						if (jsonnode["fingerInputFactor"] != null)
						{
							this.fingerInputFactor = jsonnode["fingerInputFactor"].AsFloat;
						}
						if (jsonnode["thumbInputFactor"] != null)
						{
							this.thumbInputFactor = jsonnode["thumbInputFactor"].AsFloat;
						}
						if (jsonnode["fingerSpreadOffset"] != null)
						{
							this.fingerSpreadOffset = jsonnode["fingerSpreadOffset"].AsFloat;
						}
						if (jsonnode["fingerBendOffset"] != null)
						{
							this.fingerBendOffset = jsonnode["fingerBendOffset"].AsFloat;
						}
						if (jsonnode["thumbSpreadOffset"] != null)
						{
							this.thumbSpreadOffset = jsonnode["thumbSpreadOffset"].AsFloat;
						}
						if (jsonnode["thumbBendOffset"] != null)
						{
							this.thumbBendOffset = jsonnode["thumbBendOffset"].AsFloat;
						}
						if (jsonnode["shadowFilterLevel"] != null)
						{
							this.shadowFilterLevel = jsonnode["shadowFilterLevel"].AsFloat;
						}
						if (jsonnode["pointLightShadowBlur"] != null)
						{
							this.pointLightShadowBlur = jsonnode["pointLightShadowBlur"].AsFloat;
						}
						if (jsonnode["pointLightShadowBiasBase"] != null)
						{
							this.pointLightShadowBiasBase = jsonnode["pointLightShadowBiasBase"].AsFloat;
						}
						if (jsonnode["physicsRate"] != null)
						{
							this.SetPhysicsRateFromString(jsonnode["physicsRate"]);
						}
						if (jsonnode["physicsUpdateCap"] != null)
						{
							this.SetPhysicsUpdateCapFromString(jsonnode["physicsUpdateCap"]);
						}
						if (jsonnode["physicsHighQuality"] != null)
						{
							this.physicsHighQuality = jsonnode["physicsHighQuality"].AsBool;
						}
						if (jsonnode["softBodyPhysics"] != null)
						{
							this.softPhysics = jsonnode["softBodyPhysics"].AsBool;
						}
						if (jsonnode["glowEffects"] != null)
						{
							this.SetGlowEffectsFromString(jsonnode["glowEffects"]);
						}
						if (jsonnode["useHeadCollider"] != null)
						{
							this.useHeadCollider = jsonnode["useHeadCollider"].AsBool;
						}
						if (jsonnode["optimizeMemoryOnSceneLoad"] != null)
						{
							this.optimizeMemoryOnSceneLoad = jsonnode["optimizeMemoryOnSceneLoad"].AsBool;
						}
						if (jsonnode["optimizeMemoryOnPresetLoad"] != null)
						{
							this.optimizeMemoryOnPresetLoad = jsonnode["optimizeMemoryOnPresetLoad"].AsBool;
						}
						if (jsonnode["enableCaching"] != null)
						{
							this.enableCaching = jsonnode["enableCaching"].AsBool;
						}
						if (jsonnode["cacheFolder"] != null)
						{
							this.cacheFolder = jsonnode["cacheFolder"];
						}
						if (jsonnode["confirmLoad"] != null)
						{
							this.confirmLoad = jsonnode["confirmLoad"].AsBool;
						}
						if (jsonnode["flipToolbar"] != null)
						{
							this.flipToolbar = jsonnode["flipToolbar"].AsBool;
						}
						if (jsonnode["enableWebBrowser"] != null)
						{
							this.enableWebBrowser = jsonnode["enableWebBrowser"].AsBool;
						}
						if (jsonnode["allowNonWhitelistDomains"] != null)
						{
							this.allowNonWhitelistDomains = jsonnode["allowNonWhitelistDomains"].AsBool;
						}
						if (jsonnode["enableWebBrowserProfile"] != null)
						{
							this.enableWebBrowserProfile = jsonnode["enableWebBrowserProfile"].AsBool;
						}
						if (jsonnode["enableWebMisc"] != null)
						{
							this.enableWebMisc = jsonnode["enableWebMisc"].AsBool;
						}
						if (jsonnode["enableHub"] != null)
						{
							this.enableHub = jsonnode["enableHub"].AsBool;
						}
						if (jsonnode["enableHubDownloader"] != null)
						{
							this.enableHubDownloader = jsonnode["enableHubDownloader"].AsBool;
						}
						if (jsonnode["enablePlugins"] != null)
						{
							this.enablePlugins = jsonnode["enablePlugins"].AsBool;
						}
						if (jsonnode["allowPluginsNetworkAccess"] != null)
						{
							this.allowPluginsNetworkAccess = jsonnode["allowPluginsNetworkAccess"].AsBool;
						}
						if (jsonnode["alwaysAllowPluginsDownloadedFromHub"] != null)
						{
							this.alwaysAllowPluginsDownloadedFromHub = jsonnode["alwaysAllowPluginsDownloadedFromHub"].AsBool;
						}
						if (jsonnode["hideDisabledWebMessages"] != null)
						{
							this.hideDisabledWebMessages = jsonnode["hideDisabledWebMessages"].AsBool;
						}
						if (SuperController.singleton != null)
						{
							if (SuperController.singleton.termsOfUseDisabled)
							{
								this.termsOfUseAccepted = true;
							}
							else if (jsonnode["termsOfUseAccepted"] != null)
							{
								this.termsOfUseAccepted = jsonnode["termsOfUseAccepted"].AsBool;
							}
							if (jsonnode["showHelpOverlay"] != null)
							{
								SuperController.singleton.helpOverlayOn = jsonnode["showHelpOverlay"].AsBool;
							}
							if (jsonnode["lockHeightDuringNavigate"] != null)
							{
								SuperController.singleton.lockHeightDuringNavigate = jsonnode["lockHeightDuringNavigate"].AsBool;
							}
							if (jsonnode["freeMoveFollowFloor"] != null)
							{
								SuperController.singleton.freeMoveFollowFloor = jsonnode["freeMoveFollowFloor"].AsBool;
							}
							if (jsonnode["teleportAllowRotation"] != null)
							{
								SuperController.singleton.teleportAllowRotation = jsonnode["teleportAllowRotation"].AsBool;
							}
							if (jsonnode["disableAllNavigation"] != null)
							{
								SuperController.singleton.disableAllNavigation = jsonnode["disableAllNavigation"].AsBool;
							}
							if (jsonnode["disableGrabNavigation"] != null)
							{
								SuperController.singleton.disableGrabNavigation = jsonnode["disableGrabNavigation"].AsBool;
							}
							if (jsonnode["disableTeleport"] != null)
							{
								SuperController.singleton.disableTeleport = jsonnode["disableTeleport"].AsBool;
							}
							if (jsonnode["disableTeleportDuringPossess"] != null)
							{
								SuperController.singleton.disableTeleportDuringPossess = jsonnode["disableTeleportDuringPossess"].AsBool;
							}
							if (jsonnode["freeMoveMultiplier"] != null)
							{
								SuperController.singleton.freeMoveMultiplier = jsonnode["freeMoveMultiplier"].AsFloat;
							}
							if (jsonnode["grabNavigationPositionMultiplier"] != null)
							{
								SuperController.singleton.grabNavigationPositionMultiplier = jsonnode["grabNavigationPositionMultiplier"].AsFloat;
							}
							if (jsonnode["grabNavigationRotationMultiplier"] != null)
							{
								SuperController.singleton.grabNavigationRotationMultiplier = jsonnode["grabNavigationRotationMultiplier"].AsFloat;
							}
							if (jsonnode["showNavigationHologrid"] != null)
							{
								SuperController.singleton.showNavigationHologrid = jsonnode["showNavigationHologrid"].AsBool;
							}
							if (jsonnode["hologridTransparency"] != null)
							{
								SuperController.singleton.hologridTransparency = jsonnode["hologridTransparency"].AsFloat;
							}
							if (jsonnode["oculusThumbstickFunction"] != null)
							{
								SuperController.singleton.SetOculusThumbstickFunctionFromString(jsonnode["oculusThumbstickFunction"]);
							}
							if (jsonnode["allowPossessSpringAdjustment"] != null)
							{
								SuperController.singleton.allowPossessSpringAdjustment = jsonnode["allowPossessSpringAdjustment"].AsBool;
							}
							if (jsonnode["possessPositionSpring"] != null)
							{
								SuperController.singleton.possessPositionSpring = jsonnode["possessPositionSpring"].AsFloat;
							}
							if (jsonnode["possessRotationSpring"] != null)
							{
								SuperController.singleton.possessRotationSpring = jsonnode["possessRotationSpring"].AsFloat;
							}
							if (jsonnode["loResScreenshotCameraFOV"] != null)
							{
								SuperController.singleton.loResScreenShotCameraFOV = jsonnode["loResScreenshotCameraFOV"].AsFloat;
							}
							if (jsonnode["hiResScreenshotCameraFOV"] != null)
							{
								SuperController.singleton.hiResScreenShotCameraFOV = jsonnode["hiResScreenshotCameraFOV"].AsFloat;
							}
							if (jsonnode["allowGrabPlusTriggerHandToggle"] != null)
							{
								SuperController.singleton.allowGrabPlusTriggerHandToggle = jsonnode["allowGrabPlusTriggerHandToggle"].AsBool;
							}
							if (jsonnode["monitorUIScale"] != null)
							{
								SuperController.singleton.monitorUIScale = jsonnode["monitorUIScale"].AsFloat;
							}
							if (jsonnode["monitorUIYOffset"] != null)
							{
								SuperController.singleton.monitorUIYOffset = jsonnode["monitorUIYOffset"].AsFloat;
							}
							if (jsonnode["VRUISide"] != null)
							{
								SuperController.singleton.SetUISide(jsonnode["VRUISide"]);
							}
							if (jsonnode["motionControllerAlwaysUseAlternateHands"] != null)
							{
								SuperController.singleton.alwaysUseAlternateHands = jsonnode["motionControllerAlwaysUseAlternateHands"].AsBool;
							}
							if (jsonnode["useLegacyWorldScaleChange"] != null)
							{
								SuperController.singleton.useLegacyWorldScaleChange = jsonnode["useLegacyWorldScaleChange"].AsBool;
							}
							if (jsonnode["onStartupSkipStartScreen"] != null)
							{
								SuperController.singleton.onStartupSkipStartScreen = jsonnode["onStartupSkipStartScreen"].AsBool;
							}
							if (jsonnode["autoFreezeAnimationOnSwitchToEditMode"] != null)
							{
								SuperController.singleton.autoFreezeAnimationOnSwitchToEditMode = jsonnode["autoFreezeAnimationOnSwitchToEditMode"].AsBool;
							}
							if (jsonnode["worldUIVRAnchorDistance"] != null)
							{
								SuperController.singleton.worldUIVRAnchorDistance = jsonnode["worldUIVRAnchorDistance"].AsFloat;
							}
							if (jsonnode["worldUIVRAnchorHeight"] != null)
							{
								SuperController.singleton.worldUIVRAnchorHeight = jsonnode["worldUIVRAnchorHeight"].AsFloat;
							}
							if (jsonnode["useMonitorRigAudioListenerWhenActive"] != null)
							{
								SuperController.singleton.useMonitorRigAudioListenerWhenActive = jsonnode["useMonitorRigAudioListenerWhenActive"].AsBool;
							}
							if (jsonnode["generateDepthTexture"] != null)
							{
								SuperController.singleton.generateDepthTexture = jsonnode["generateDepthTexture"].AsBool;
							}
							if (jsonnode["leapMotionEnabled"] != null)
							{
								SuperController.singleton.leapMotionEnabled = jsonnode["leapMotionEnabled"].AsBool;
							}
							if (jsonnode["openMainHUDOnError"] != null)
							{
								SuperController.singleton.openMainHUDOnError = jsonnode["openMainHUDOnError"].AsBool;
							}
						}
						if (this.leapHandModelControl != null && jsonnode["leapMotionAllowPinchGrab"] != null)
						{
							this.leapHandModelControl.allowPinchGrab = jsonnode["leapMotionAllowPinchGrab"].AsBool;
						}
						if (this.motionHandModelControl != null)
						{
							if (jsonnode["motionControllerLeftHandChoice"] != null)
							{
								this.motionHandModelControl.leftHandChoice = jsonnode["motionControllerLeftHandChoice"];
							}
							if (jsonnode["motionControllerRightHandChoice"] != null)
							{
								this.motionHandModelControl.rightHandChoice = jsonnode["motionControllerRightHandChoice"];
							}
							if (jsonnode["motionControllerLinkHands"] != null)
							{
								this.motionHandModelControl.linkHands = jsonnode["motionControllerLinkHands"].AsBool;
							}
							if (jsonnode["motionControllerUseCollision"] != null)
							{
								this.motionHandModelControl.useCollision = jsonnode["motionControllerUseCollision"].AsBool;
							}
							if (jsonnode["motionControllerHandsPositionOffset"] != null)
							{
								this.motionHandModelControl.xPosition = jsonnode["motionControllerHandsPositionOffset"]["x"].AsFloat;
								this.motionHandModelControl.yPosition = jsonnode["motionControllerHandsPositionOffset"]["y"].AsFloat;
								this.motionHandModelControl.zPosition = jsonnode["motionControllerHandsPositionOffset"]["z"].AsFloat;
							}
							if (jsonnode["motionControllerHandsRotationOffset"] != null)
							{
								this.motionHandModelControl.xRotation = jsonnode["motionControllerHandsRotationOffset"]["x"].AsFloat;
								this.motionHandModelControl.yRotation = jsonnode["motionControllerHandsRotationOffset"]["y"].AsFloat;
								this.motionHandModelControl.zRotation = jsonnode["motionControllerHandsRotationOffset"]["z"].AsFloat;
							}
						}
						if (this.alternateMotionHandModelControl != null)
						{
							if (jsonnode["alternateMotionControllerLeftHandChoice"] != null)
							{
								this.alternateMotionHandModelControl.leftHandChoice = jsonnode["alternateMotionControllerLeftHandChoice"];
							}
							if (jsonnode["alternateMotionControllerRightHandChoice"] != null)
							{
								this.alternateMotionHandModelControl.rightHandChoice = jsonnode["alternateMotionControllerRightHandChoice"];
							}
							if (jsonnode["alternateMotionControllerLinkHands"] != null)
							{
								this.alternateMotionHandModelControl.linkHands = jsonnode["alternateMotionControllerLinkHands"].AsBool;
							}
						}
						if (jsonnode["creatorName"] != null)
						{
							this.creatorName = jsonnode["creatorName"];
						}
						if (jsonnode["DAZExtraLibraryFolder"] != null)
						{
							this.DAZExtraLibraryFolder = jsonnode["DAZExtraLibraryFolder"];
						}
						if (jsonnode["DAZDefaultContentFolder"] != null)
						{
							this.DAZDefaultContentFolder = jsonnode["DAZDefaultContentFolder"];
						}
						if (jsonnode["fileBrowserSortBy"] != null)
						{
							this.SetFileBrowserSortBy(jsonnode["fileBrowserSortBy"]);
						}
						if (jsonnode["fileBrowserDirectoryOption"] != null)
						{
							this.SetFileBrowserDirectoryOption(jsonnode["fileBrowserDirectoryOption"]);
						}
					}
				}
				catch (Exception arg)
				{
					SuperController.LogError("Exception during read of prefs file " + arg);
				}
			}
			this._disableSave = false;
			this.CheckQualityLevels();
		}
	}

	// Token: 0x06005F88 RID: 24456 RVA: 0x00243234 File Offset: 0x00241634
	public void ResetPreferences()
	{
		this.SetQuality("High");
		this._disableSave = true;
		this.firstTimeUser = true;
		this.desktopVsync = false;
		this.mirrorToDisplay = false;
		this.hideExitButton = false;
		this.showTargetsMenuOnly = false;
		this.alwaysShowPointersOnTouch = true;
		this.hideInactiveTargets = true;
		this.showControllersMenuOnly = false;
		this.targetAlpha = 1f;
		this.crosshairAlpha = 0.1f;
		this.useMonitorViewOffsetWhenUIOpen = true;
		this.oculusSwapGrabAndTrigger = false;
		this.oculusDisableFreeMove = false;
		this.steamVRShowControllers = false;
		this.steamVRUseControllerHandPose = false;
		this.steamVRPointerAngle = this.defaultSteamVRPointerAngle;
		this.fingerInputFactor = this.defaultFingerInputFactor;
		this.thumbInputFactor = this.defaultThumbInputFactor;
		this.fingerSpreadOffset = this.defaultFingerSpreadOffset;
		this.fingerBendOffset = this.defaultFingerBendOffset;
		this.thumbSpreadOffset = this.defaultThumbSpreadOffset;
		this.thumbBendOffset = this.defaultThumbBendOffset;
		this.shadowFilterLevel = 3f;
		this.pointLightShadowBlur = 0.5f;
		this.pointLightShadowBiasBase = 0.015f;
		this.physicsRate = UserPreferences.PhysicsRate.Auto;
		this.physicsUpdateCap = 2;
		this.physicsHighQuality = false;
		this.overlayUI = true;
		this.useHeadCollider = false;
		this.optimizeMemoryOnSceneLoad = true;
		this.optimizeMemoryOnPresetLoad = false;
		this.enableCaching = true;
		this.confirmLoad = false;
		this.flipToolbar = false;
		this.enableWebBrowser = true;
		this.allowNonWhitelistDomains = false;
		this.enableWebBrowserProfile = true;
		this.enableWebMisc = true;
		this.enableHub = true;
		this.enableHubDownloader = true;
		this.enablePlugins = true;
		this.allowPluginsNetworkAccess = false;
		this.alwaysAllowPluginsDownloadedFromHub = false;
		this.hideDisabledWebMessages = true;
		if (SuperController.singleton != null)
		{
			if (SuperController.singleton.termsOfUseDisabled)
			{
				this.termsOfUseAccepted = true;
			}
			else
			{
				this.termsOfUseAccepted = false;
			}
			SuperController.singleton.helpOverlayOn = true;
			SuperController.singleton.lockHeightDuringNavigate = true;
			SuperController.singleton.freeMoveFollowFloor = true;
			SuperController.singleton.teleportAllowRotation = false;
			SuperController.singleton.disableAllNavigation = false;
			SuperController.singleton.disableGrabNavigation = false;
			SuperController.singleton.disableTeleport = false;
			SuperController.singleton.disableTeleportDuringPossess = true;
			SuperController.singleton.freeMoveMultiplier = 1f;
			SuperController.singleton.grabNavigationPositionMultiplier = 1f;
			SuperController.singleton.grabNavigationRotationMultiplier = 0.5f;
			SuperController.singleton.showNavigationHologrid = true;
			SuperController.singleton.hologridTransparency = 0.01f;
			SuperController.singleton.oculusThumbstickFunction = SuperController.ThumbstickFunction.GrabWorld;
			SuperController.singleton.allowPossessSpringAdjustment = true;
			SuperController.singleton.possessPositionSpring = 10000f;
			SuperController.singleton.possessRotationSpring = 1000f;
			SuperController.singleton.loResScreenShotCameraFOV = 40f;
			SuperController.singleton.hiResScreenShotCameraFOV = 40f;
			SuperController.singleton.allowGrabPlusTriggerHandToggle = true;
			SuperController.singleton.monitorUIScale = 1f;
			SuperController.singleton.monitorUIYOffset = 0f;
			SuperController.singleton.UISide = UISideAlign.Side.Right;
			SuperController.singleton.alwaysUseAlternateHands = false;
			SuperController.singleton.useLegacyWorldScaleChange = false;
			SuperController.singleton.onStartupSkipStartScreen = false;
			SuperController.singleton.autoFreezeAnimationOnSwitchToEditMode = false;
			SuperController.singleton.worldUIVRAnchorDistance = 2f;
			SuperController.singleton.worldUIVRAnchorHeight = 0.8f;
			SuperController.singleton.useMonitorRigAudioListenerWhenActive = true;
			SuperController.singleton.generateDepthTexture = false;
			SuperController.singleton.leapMotionEnabled = false;
			SuperController.singleton.openMainHUDOnError = true;
		}
		if (this.leapHandModelControl != null)
		{
			this.leapHandModelControl.allowPinchGrab = true;
		}
		if (this.motionHandModelControl != null)
		{
			this.motionHandModelControl.xPosition = 0f;
			this.motionHandModelControl.yPosition = 0f;
			this.motionHandModelControl.zPosition = 0f;
			this.motionHandModelControl.xRotation = 0f;
			this.motionHandModelControl.yRotation = 0f;
			this.motionHandModelControl.zRotation = 0f;
			this.motionHandModelControl.useCollision = false;
			this.motionHandModelControl.rightHandChoice = "SphereKinematic";
			this.motionHandModelControl.leftHandChoice = "SphereKinematic";
			this.motionHandModelControl.linkHands = true;
		}
		if (this.alternateMotionHandModelControl != null)
		{
			this.alternateMotionHandModelControl.useCollision = false;
			this.alternateMotionHandModelControl.rightHandChoice = "SphereKinematic";
			this.alternateMotionHandModelControl.leftHandChoice = "SphereKinematic";
			this.alternateMotionHandModelControl.linkHands = true;
		}
		this._disableSave = false;
		this.SavePreferences();
	}

	// Token: 0x06005F89 RID: 24457 RVA: 0x002436AE File Offset: 0x00241AAE
	private void SyncRenderScale()
	{
		if (XRSettings.eyeTextureResolutionScale != this._renderScale)
		{
			XRSettings.eyeTextureResolutionScale = this._renderScale;
		}
	}

	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x06005F8A RID: 24458 RVA: 0x002436CB File Offset: 0x00241ACB
	// (set) Token: 0x06005F8B RID: 24459 RVA: 0x002436D3 File Offset: 0x00241AD3
	public float renderScale
	{
		get
		{
			return this._renderScale;
		}
		set
		{
			if (this._renderScale != value)
			{
				this._renderScale = value;
				if (this.renderScaleSlider != null)
				{
					this.renderScaleSlider.value = value;
				}
				this.SyncRenderScale();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005F8C RID: 24460 RVA: 0x00243711 File Offset: 0x00241B11
	private void SyncMsaa()
	{
		if (QualitySettings.antiAliasing != this.msaaLevel)
		{
			QualitySettings.antiAliasing = this.msaaLevel;
		}
	}

	// Token: 0x06005F8D RID: 24461 RVA: 0x00243730 File Offset: 0x00241B30
	private void SyncMsaaPopup()
	{
		if (this.msaaPopup != null)
		{
			int msaaLevel = this._msaaLevel;
			switch (msaaLevel)
			{
			case 0:
				this.msaaPopup.currentValue = "Off";
				break;
			default:
				if (msaaLevel == 8)
				{
					this.msaaPopup.currentValue = "8X";
				}
				break;
			case 2:
				this.msaaPopup.currentValue = "2X";
				break;
			case 4:
				this.msaaPopup.currentValue = "4X";
				break;
			}
		}
	}

	// Token: 0x17000E01 RID: 3585
	// (get) Token: 0x06005F8E RID: 24462 RVA: 0x002437CF File Offset: 0x00241BCF
	// (set) Token: 0x06005F8F RID: 24463 RVA: 0x00243804 File Offset: 0x00241C04
	public int msaaLevel
	{
		get
		{
			if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.overrideMsaaLevel)
			{
				return GlobalSceneOptions.singleton.msaaLevel;
			}
			return this._msaaLevel;
		}
		set
		{
			if (this._msaaLevel != value && (value == 0 || value == 2 || value == 4 || value == 8))
			{
				this._msaaLevel = value;
				this.SyncMsaa();
				this.SyncMsaaPopup();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005F90 RID: 24464 RVA: 0x00243854 File Offset: 0x00241C54
	public void SetMsaaFromString(string levelString)
	{
		if (levelString != null)
		{
			if (!(levelString == "Off"))
			{
				if (!(levelString == "2X"))
				{
					if (!(levelString == "4X"))
					{
						if (levelString == "8X")
						{
							this.msaaLevel = 8;
						}
					}
					else
					{
						this.msaaLevel = 4;
					}
				}
				else
				{
					this.msaaLevel = 2;
				}
			}
			else
			{
				this.msaaLevel = 0;
			}
		}
	}

	// Token: 0x06005F91 RID: 24465 RVA: 0x002438DC File Offset: 0x00241CDC
	public void DisableFirstTimeUser()
	{
		this.firstTimeUser = false;
	}

	// Token: 0x06005F92 RID: 24466 RVA: 0x002438E8 File Offset: 0x00241CE8
	protected void SyncFirstTimeUser()
	{
		bool firstTimeUser = this.firstTimeUser;
		if (firstTimeUser && SuperController.singleton != null)
		{
			SuperController.singleton.CloseAllWorldUIPanels();
			SuperController.singleton.ActivateWorldUI();
		}
		if (this.firstTimeUserEnableGameObjects != null)
		{
			foreach (GameObject gameObject in this.firstTimeUserEnableGameObjects)
			{
				gameObject.SetActive(firstTimeUser);
			}
		}
		if (this.firstTimeUserDisableGameObjects != null)
		{
			foreach (GameObject gameObject2 in this.firstTimeUserDisableGameObjects)
			{
				gameObject2.SetActive(!firstTimeUser);
			}
		}
	}

	// Token: 0x17000E02 RID: 3586
	// (get) Token: 0x06005F93 RID: 24467 RVA: 0x00243995 File Offset: 0x00241D95
	// (set) Token: 0x06005F94 RID: 24468 RVA: 0x002439BE File Offset: 0x00241DBE
	public bool firstTimeUser
	{
		get
		{
			return (!(GlobalSceneOptions.singleton != null) || !GlobalSceneOptions.singleton.bypassFirstTimeUser) && this._firstTimeUser;
		}
		set
		{
			if (this._firstTimeUser != value)
			{
				this._firstTimeUser = value;
				this.SyncFirstTimeUser();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x002439E0 File Offset: 0x00241DE0
	public void ReviewTerms()
	{
		if (!string.IsNullOrEmpty(this.termsOfUsePath) && File.Exists(this.termsOfUsePath))
		{
			string fullPath = Path.GetFullPath(this.termsOfUsePath);
			Process.Start("file://" + fullPath);
		}
	}

	// Token: 0x06005F96 RID: 24470 RVA: 0x00243A2C File Offset: 0x00241E2C
	protected void TermsAndSettingsAcceptedPressed()
	{
		if (SuperController.singleton && this.termsNotAcceptedGameObject != null)
		{
			this.termsNotAcceptedGameObject.SetActive(!this._termsOfUseAccepted);
		}
		if (this._termsOfUseAccepted)
		{
			this.DisableFirstTimeUser();
		}
	}

	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x06005F97 RID: 24471 RVA: 0x00243A7E File Offset: 0x00241E7E
	// (set) Token: 0x06005F98 RID: 24472 RVA: 0x00243A86 File Offset: 0x00241E86
	public bool termsOfUseAccepted
	{
		get
		{
			return this._termsOfUseAccepted;
		}
		set
		{
			if (this._termsOfUseAccepted != value)
			{
				this._termsOfUseAccepted = value;
				if (this.termsOfUseAcceptedToggle != null)
				{
					this.termsOfUseAcceptedToggle.isOn = value;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005F99 RID: 24473 RVA: 0x00243ABE File Offset: 0x00241EBE
	private void SyncDesktopVsync()
	{
		if ((SuperController.singleton == null || SuperController.singleton.IsMonitorOnly) && this._desktopVsync)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
	}

	// Token: 0x17000E04 RID: 3588
	// (get) Token: 0x06005F9A RID: 24474 RVA: 0x00243AFB File Offset: 0x00241EFB
	// (set) Token: 0x06005F9B RID: 24475 RVA: 0x00243B04 File Offset: 0x00241F04
	public bool desktopVsync
	{
		get
		{
			return this._desktopVsync;
		}
		set
		{
			if (this._desktopVsync != value)
			{
				this._desktopVsync = value;
				if (this.desktopVsyncToggle != null)
				{
					this.desktopVsyncToggle.isOn = this._desktopVsync;
				}
				this.SyncDesktopVsync();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x00243B52 File Offset: 0x00241F52
	private void SyncSmoothPassesPopup()
	{
		if (this.smoothPassesPopup != null)
		{
			this.smoothPassesPopup.currentValue = this._smoothPasses.ToString();
		}
	}

	// Token: 0x17000E05 RID: 3589
	// (get) Token: 0x06005F9D RID: 24477 RVA: 0x00243B81 File Offset: 0x00241F81
	// (set) Token: 0x06005F9E RID: 24478 RVA: 0x00243B89 File Offset: 0x00241F89
	public int smoothPasses
	{
		get
		{
			return this._smoothPasses;
		}
		set
		{
			if (this._smoothPasses != value && value >= 0 && value <= 4)
			{
				this._smoothPasses = value;
				this.SyncSmoothPassesPopup();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x00243BB8 File Offset: 0x00241FB8
	public void SetSmoothPassesFromString(string levelString)
	{
		try
		{
			this.smoothPasses = int.Parse(levelString);
		}
		catch (FormatException)
		{
			UnityEngine.Debug.LogError("Attempted to set smooth passes to " + levelString + " which is not an int");
		}
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x00243C04 File Offset: 0x00242004
	private void SyncPixelLightCount()
	{
		if (QualitySettings.pixelLightCount != this.pixelLightCount)
		{
			QualitySettings.pixelLightCount = this.pixelLightCount;
		}
	}

	// Token: 0x17000E06 RID: 3590
	// (get) Token: 0x06005FA1 RID: 24481 RVA: 0x00243C21 File Offset: 0x00242021
	// (set) Token: 0x06005FA2 RID: 24482 RVA: 0x00243C54 File Offset: 0x00242054
	public int pixelLightCount
	{
		get
		{
			if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.overridePixelLightCount)
			{
				return GlobalSceneOptions.singleton.pixelLightCount;
			}
			return this._pixelLightCount;
		}
		set
		{
			if (this._pixelLightCount != value)
			{
				this._pixelLightCount = value;
				this.SyncPixelLightCount();
				if (this.pixelLightCountPopup != null)
				{
					this.pixelLightCountPopup.currentValue = this.pixelLightCount.ToString();
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FA3 RID: 24483 RVA: 0x00243CB0 File Offset: 0x002420B0
	public void SetPixelLightCountFromString(string countString)
	{
		try
		{
			this.pixelLightCount = int.Parse(countString);
		}
		catch (FormatException)
		{
			UnityEngine.Debug.LogError("Attempted to set pixel light count to " + countString + " which is not an int");
		}
	}

	// Token: 0x06005FA4 RID: 24484 RVA: 0x00243CFC File Offset: 0x002420FC
	private void SetInternalShaderLOD()
	{
		Shader.globalMaximumLOD = (int)this._shaderLOD;
	}

	// Token: 0x17000E07 RID: 3591
	// (get) Token: 0x06005FA5 RID: 24485 RVA: 0x00243D09 File Offset: 0x00242109
	// (set) Token: 0x06005FA6 RID: 24486 RVA: 0x00243D14 File Offset: 0x00242114
	public UserPreferences.ShaderLOD shaderLOD
	{
		get
		{
			return this._shaderLOD;
		}
		set
		{
			if (this._shaderLOD != value)
			{
				this._shaderLOD = value;
				this.SetInternalShaderLOD();
				if (this.shaderLODPopup != null)
				{
					this.shaderLODPopup.currentValue = this._shaderLOD.ToString();
				}
			}
		}
	}

	// Token: 0x06005FA7 RID: 24487 RVA: 0x00243D68 File Offset: 0x00242168
	public void SetShaderLODFromString(string lod)
	{
		try
		{
			this.shaderLOD = (UserPreferences.ShaderLOD)Enum.Parse(typeof(UserPreferences.ShaderLOD), lod);
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set shader lod " + lod + " which is not a valid lod string");
		}
	}

	// Token: 0x06005FA8 RID: 24488 RVA: 0x00243DC0 File Offset: 0x002421C0
	private void SyncMirrorReflections()
	{
		MirrorReflection.globalEnabled = this._mirrorReflections;
		if (this.normalCamera != null)
		{
			this.normalCamera.enabled = !this._mirrorReflections;
		}
		if (this.mirrorReflectionCamera1 != null)
		{
			this.mirrorReflectionCamera1.enabled = this._mirrorReflections;
		}
		if (this.mirrorReflectionCamera2 != null)
		{
			this.mirrorReflectionCamera2.enabled = this._mirrorReflections;
		}
	}

	// Token: 0x17000E08 RID: 3592
	// (get) Token: 0x06005FA9 RID: 24489 RVA: 0x00243E41 File Offset: 0x00242241
	// (set) Token: 0x06005FAA RID: 24490 RVA: 0x00243E4C File Offset: 0x0024224C
	public bool mirrorReflections
	{
		get
		{
			return this._mirrorReflections;
		}
		set
		{
			if (this._mirrorReflections != value)
			{
				this._mirrorReflections = value;
				if (this.mirrorReflectionsToggle != null)
				{
					this.mirrorReflectionsToggle.isOn = this._mirrorReflections;
				}
				this.SyncMirrorReflections();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FAB RID: 24491 RVA: 0x00243E9A File Offset: 0x0024229A
	private void SyncRealtimeReflectionProbes()
	{
		QualitySettings.realtimeReflectionProbes = this._realtimeReflectionProbes;
	}

	// Token: 0x17000E09 RID: 3593
	// (get) Token: 0x06005FAC RID: 24492 RVA: 0x00243EA7 File Offset: 0x002422A7
	// (set) Token: 0x06005FAD RID: 24493 RVA: 0x00243EB0 File Offset: 0x002422B0
	public bool realtimeReflectionProbes
	{
		get
		{
			return this._realtimeReflectionProbes;
		}
		set
		{
			if (this._realtimeReflectionProbes != value)
			{
				this._realtimeReflectionProbes = value;
				if (this.realtimeReflectionProbesToggle != null)
				{
					this.realtimeReflectionProbesToggle.isOn = this._realtimeReflectionProbes;
				}
				this.SyncRealtimeReflectionProbes();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FAE RID: 24494 RVA: 0x00243EFE File Offset: 0x002422FE
	private void SyncMirrorToDisplay()
	{
		if (XRSettings.showDeviceView != this._mirrorToDisplay)
		{
			XRSettings.showDeviceView = this._mirrorToDisplay;
		}
	}

	// Token: 0x17000E0A RID: 3594
	// (get) Token: 0x06005FAF RID: 24495 RVA: 0x00243F1B File Offset: 0x0024231B
	// (set) Token: 0x06005FB0 RID: 24496 RVA: 0x00243F24 File Offset: 0x00242324
	public bool mirrorToDisplay
	{
		get
		{
			return this._mirrorToDisplay;
		}
		set
		{
			if (this._mirrorToDisplay != value)
			{
				this._mirrorToDisplay = value;
				if (this.mirrorToggle != null)
				{
					this.mirrorToggle.isOn = this._mirrorToDisplay;
				}
				this.SyncMirrorToDisplay();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FB1 RID: 24497 RVA: 0x00243F72 File Offset: 0x00242372
	private void SyncHideExitButton()
	{
		if (this.exitButton != null)
		{
			this.exitButton.SetActive(!this._hideExitButton);
		}
	}

	// Token: 0x17000E0B RID: 3595
	// (get) Token: 0x06005FB2 RID: 24498 RVA: 0x00243F99 File Offset: 0x00242399
	// (set) Token: 0x06005FB3 RID: 24499 RVA: 0x00243FA4 File Offset: 0x002423A4
	public bool hideExitButton
	{
		get
		{
			return this._hideExitButton;
		}
		set
		{
			if (this._hideExitButton != value)
			{
				this._hideExitButton = value;
				if (this.hideExitButtonToggle != null)
				{
					this.hideExitButtonToggle.isOn = this._hideExitButton;
				}
				this.SyncHideExitButton();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E0C RID: 3596
	// (get) Token: 0x06005FB4 RID: 24500 RVA: 0x00243FF2 File Offset: 0x002423F2
	// (set) Token: 0x06005FB5 RID: 24501 RVA: 0x00243FFA File Offset: 0x002423FA
	public bool showTargetsMenuOnly
	{
		get
		{
			return this._showTargetsMenuOnly;
		}
		set
		{
			if (this._showTargetsMenuOnly != value)
			{
				this._showTargetsMenuOnly = value;
				if (this.showTargetsMenuOnlyToggle != null)
				{
					this.showTargetsMenuOnlyToggle.isOn = this._showTargetsMenuOnly;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x06005FB6 RID: 24502 RVA: 0x00244037 File Offset: 0x00242437
	// (set) Token: 0x06005FB7 RID: 24503 RVA: 0x0024403F File Offset: 0x0024243F
	public bool alwaysShowPointersOnTouch
	{
		get
		{
			return this._alwaysShowPointersOnTouch;
		}
		set
		{
			if (this._alwaysShowPointersOnTouch != value)
			{
				this._alwaysShowPointersOnTouch = value;
				if (this.alwaysShowPointersOnTouchToggle != null)
				{
					this.alwaysShowPointersOnTouchToggle.isOn = this._alwaysShowPointersOnTouch;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FB8 RID: 24504 RVA: 0x0024407C File Offset: 0x0024247C
	public void ShowInactiveTargets()
	{
		this.hideInactiveTargets = false;
	}

	// Token: 0x06005FB9 RID: 24505 RVA: 0x00244085 File Offset: 0x00242485
	public void HideInactiveTargets()
	{
		this.hideInactiveTargets = true;
	}

	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x06005FBA RID: 24506 RVA: 0x0024408E File Offset: 0x0024248E
	// (set) Token: 0x06005FBB RID: 24507 RVA: 0x00244098 File Offset: 0x00242498
	public bool hideInactiveTargets
	{
		get
		{
			return this._hideInactiveTargets;
		}
		set
		{
			if (this._hideInactiveTargets != value)
			{
				this._hideInactiveTargets = value;
				if (this.hideInactiveTargetsToggle != null)
				{
					this.hideInactiveTargetsToggle.isOn = this._hideInactiveTargets;
				}
				if (this.enableWhenHideInactiveTargets != null)
				{
					this.enableWhenHideInactiveTargets.gameObject.SetActive(this._hideInactiveTargets);
				}
				if (this.enableWhenShowInactiveTargets != null)
				{
					this.enableWhenShowInactiveTargets.gameObject.SetActive(!this._hideInactiveTargets);
				}
				if (SuperController.singleton != null)
				{
					SuperController.singleton.SyncVisibility();
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E0F RID: 3599
	// (get) Token: 0x06005FBC RID: 24508 RVA: 0x0024414B File Offset: 0x0024254B
	// (set) Token: 0x06005FBD RID: 24509 RVA: 0x00244153 File Offset: 0x00242553
	public bool showControllersMenuOnly
	{
		get
		{
			return this._showControllersMenuOnly;
		}
		set
		{
			if (this._showControllersMenuOnly != value)
			{
				this._showControllersMenuOnly = value;
				if (this.showControllersMenuOnlyToggle != null)
				{
					this.showControllersMenuOnlyToggle.isOn = this._showControllersMenuOnly;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E10 RID: 3600
	// (get) Token: 0x06005FBE RID: 24510 RVA: 0x00244190 File Offset: 0x00242590
	// (set) Token: 0x06005FBF RID: 24511 RVA: 0x00244198 File Offset: 0x00242598
	public float targetAlpha
	{
		get
		{
			return this._targetAlpha;
		}
		set
		{
			if (this._targetAlpha != value)
			{
				this._targetAlpha = value;
				FreeControllerV3.targetAlpha = this._targetAlpha;
				SelectTarget.useGlobalAlpha = true;
				SelectTarget.globalAlpha = this._targetAlpha;
				if (this.targetAlphaSlider != null)
				{
					this.targetAlphaSlider.value = this._targetAlpha;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FC0 RID: 24512 RVA: 0x002441FC File Offset: 0x002425FC
	private void SyncCrosshairAlpha()
	{
		if (this.crosshair != null)
		{
			Color color = this.crosshair.color;
			color.a = this._crosshairAlpha;
			this.crosshair.color = color;
		}
	}

	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x06005FC1 RID: 24513 RVA: 0x0024423F File Offset: 0x0024263F
	// (set) Token: 0x06005FC2 RID: 24514 RVA: 0x00244248 File Offset: 0x00242648
	public float crosshairAlpha
	{
		get
		{
			return this._crosshairAlpha;
		}
		set
		{
			if (this._crosshairAlpha != value)
			{
				this._crosshairAlpha = value;
				this.SyncCrosshairAlpha();
				if (this.crosshairAlphaSlider != null)
				{
					this.crosshairAlphaSlider.value = this._crosshairAlpha;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E12 RID: 3602
	// (get) Token: 0x06005FC3 RID: 24515 RVA: 0x00244296 File Offset: 0x00242696
	// (set) Token: 0x06005FC4 RID: 24516 RVA: 0x002442A0 File Offset: 0x002426A0
	public bool useMonitorViewOffsetWhenUIOpen
	{
		get
		{
			return this._useMonitorViewOffsetWhenUIOpen;
		}
		set
		{
			if (this._useMonitorViewOffsetWhenUIOpen != value)
			{
				this._useMonitorViewOffsetWhenUIOpen = value;
				if (this.useMonitorViewOffsetWhenUIOpenToggle != null)
				{
					this.useMonitorViewOffsetWhenUIOpenToggle.isOn = this._useMonitorViewOffsetWhenUIOpen;
				}
				if (SuperController.singleton != null)
				{
					SuperController.singleton.SyncMonitorRigPosition();
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FC5 RID: 24517 RVA: 0x00244304 File Offset: 0x00242704
	private void SyncSteamVRControllerModels()
	{
		if (this.steamVRLeftControllerModel != null)
		{
			this.steamVRLeftControllerModel.enabled = this._steamVRShowControllers;
			IEnumerator enumerator = this.steamVRLeftControllerModel.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(this._steamVRShowControllers);
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
		}
		if (this.steamVRRightControllerModel != null)
		{
			this.steamVRRightControllerModel.enabled = this._steamVRShowControllers;
			IEnumerator enumerator2 = this.steamVRRightControllerModel.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					Transform transform2 = (Transform)obj2;
					transform2.gameObject.SetActive(this._steamVRShowControllers);
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}
	}

	// Token: 0x17000E13 RID: 3603
	// (get) Token: 0x06005FC6 RID: 24518 RVA: 0x00244428 File Offset: 0x00242828
	// (set) Token: 0x06005FC7 RID: 24519 RVA: 0x00244430 File Offset: 0x00242830
	public bool steamVRShowControllers
	{
		get
		{
			return this._steamVRShowControllers;
		}
		set
		{
			if (this._steamVRShowControllers != value)
			{
				this._steamVRShowControllers = value;
				if (this.steamVRShowControllersToggle != null)
				{
					this.steamVRShowControllersToggle.isOn = this._steamVRShowControllers;
				}
				this.SyncSteamVRControllerModels();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FC8 RID: 24520 RVA: 0x00244480 File Offset: 0x00242880
	private void SyncSteamVRUseControllerHandPose()
	{
		if (this.steamVRLeftHandSkeleton != null)
		{
			if (this._steamVRUseControllerHandPose)
			{
				this.steamVRLeftHandSkeleton.rangeOfMotion = EVRSkeletalMotionRange.WithController;
			}
			else
			{
				this.steamVRLeftHandSkeleton.rangeOfMotion = EVRSkeletalMotionRange.WithoutController;
			}
		}
		if (this.steamVRRightHandSkeleton != null)
		{
			if (this._steamVRUseControllerHandPose)
			{
				this.steamVRRightHandSkeleton.rangeOfMotion = EVRSkeletalMotionRange.WithController;
			}
			else
			{
				this.steamVRRightHandSkeleton.rangeOfMotion = EVRSkeletalMotionRange.WithoutController;
			}
		}
	}

	// Token: 0x17000E14 RID: 3604
	// (get) Token: 0x06005FC9 RID: 24521 RVA: 0x002444FF File Offset: 0x002428FF
	// (set) Token: 0x06005FCA RID: 24522 RVA: 0x00244508 File Offset: 0x00242908
	public bool steamVRUseControllerHandPose
	{
		get
		{
			return this._steamVRUseControllerHandPose;
		}
		set
		{
			if (this._steamVRUseControllerHandPose != value)
			{
				this._steamVRUseControllerHandPose = value;
				if (this.steamVRUseControllerHandPoseToggle != null)
				{
					this.steamVRUseControllerHandPoseToggle.isOn = this._steamVRUseControllerHandPose;
				}
				this.SyncSteamVRUseControllerHandPose();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FCB RID: 24523 RVA: 0x00244558 File Offset: 0x00242958
	private void SyncSteamVRPointerAngle()
	{
		Vector3 localEulerAngles;
		localEulerAngles.x = this._steamVRPointerAngle;
		localEulerAngles.y = 0f;
		localEulerAngles.z = 0f;
		if (this.steamVRLeftHandPointer != null)
		{
			this.steamVRLeftHandPointer.localEulerAngles = localEulerAngles;
		}
		if (this.steamVRRightHandPointer != null)
		{
			this.steamVRRightHandPointer.localEulerAngles = localEulerAngles;
		}
	}

	// Token: 0x17000E15 RID: 3605
	// (get) Token: 0x06005FCC RID: 24524 RVA: 0x002445C4 File Offset: 0x002429C4
	// (set) Token: 0x06005FCD RID: 24525 RVA: 0x002445CC File Offset: 0x002429CC
	public float steamVRPointerAngle
	{
		get
		{
			return this._steamVRPointerAngle;
		}
		set
		{
			if (this._steamVRPointerAngle != value)
			{
				this._steamVRPointerAngle = value;
				if (this.steamVRPointerAngleSlider != null)
				{
					this.steamVRPointerAngleSlider.value = this._steamVRPointerAngle;
				}
				this.SyncSteamVRPointerAngle();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FCE RID: 24526 RVA: 0x0024461C File Offset: 0x00242A1C
	private void SyncFingerInputFactor()
	{
		if (this.steamVRLeftHandInput != null)
		{
			this.steamVRLeftHandInput.fingerInputFactor = this._fingerInputFactor;
		}
		if (this.steamVRRightHandInput != null)
		{
			this.steamVRRightHandInput.fingerInputFactor = this._fingerInputFactor;
		}
		if (this.ovrLeftHandInput != null)
		{
			this.ovrLeftHandInput.fingerInputFactor = this._fingerInputFactor;
		}
		if (this.ovrRightHandInput != null)
		{
			this.ovrRightHandInput.fingerInputFactor = this._fingerInputFactor;
		}
	}

	// Token: 0x17000E16 RID: 3606
	// (get) Token: 0x06005FCF RID: 24527 RVA: 0x002446B1 File Offset: 0x00242AB1
	// (set) Token: 0x06005FD0 RID: 24528 RVA: 0x002446BC File Offset: 0x00242ABC
	public float fingerInputFactor
	{
		get
		{
			return this._fingerInputFactor;
		}
		set
		{
			if (this._fingerInputFactor != value)
			{
				this._fingerInputFactor = value;
				if (this.fingerInputFactorSlider != null)
				{
					this.fingerInputFactorSlider.value = this._fingerInputFactor;
				}
				this.SyncFingerInputFactor();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FD1 RID: 24529 RVA: 0x0024470C File Offset: 0x00242B0C
	private void SyncThumbInputFactor()
	{
		if (this.steamVRLeftHandInput != null)
		{
			this.steamVRLeftHandInput.thumbInputFactor = this._thumbInputFactor;
		}
		if (this.steamVRRightHandInput != null)
		{
			this.steamVRRightHandInput.thumbInputFactor = this._thumbInputFactor;
		}
		if (this.ovrLeftHandInput != null)
		{
			this.ovrLeftHandInput.thumbInputFactor = this._thumbInputFactor;
		}
		if (this.ovrRightHandInput != null)
		{
			this.ovrRightHandInput.thumbInputFactor = this._thumbInputFactor;
		}
	}

	// Token: 0x17000E17 RID: 3607
	// (get) Token: 0x06005FD2 RID: 24530 RVA: 0x002447A1 File Offset: 0x00242BA1
	// (set) Token: 0x06005FD3 RID: 24531 RVA: 0x002447AC File Offset: 0x00242BAC
	public float thumbInputFactor
	{
		get
		{
			return this._thumbInputFactor;
		}
		set
		{
			if (this._thumbInputFactor != value)
			{
				this._thumbInputFactor = value;
				if (this.thumbInputFactorSlider != null)
				{
					this.thumbInputFactorSlider.value = this._thumbInputFactor;
				}
				this.SyncThumbInputFactor();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FD4 RID: 24532 RVA: 0x002447FC File Offset: 0x00242BFC
	private void SyncFingerSpreadOffset()
	{
		if (this.steamVRLeftHandInput != null)
		{
			this.steamVRLeftHandInput.fingerSpreadOffset = this._fingerSpreadOffset;
		}
		if (this.steamVRRightHandInput != null)
		{
			this.steamVRRightHandInput.fingerSpreadOffset = this._fingerSpreadOffset;
		}
		if (this.ovrLeftHandInput != null)
		{
			this.ovrLeftHandInput.fingerSpreadOffset = this._fingerSpreadOffset;
		}
		if (this.ovrRightHandInput != null)
		{
			this.ovrRightHandInput.fingerSpreadOffset = this._fingerSpreadOffset;
		}
	}

	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x06005FD5 RID: 24533 RVA: 0x00244891 File Offset: 0x00242C91
	// (set) Token: 0x06005FD6 RID: 24534 RVA: 0x0024489C File Offset: 0x00242C9C
	public float fingerSpreadOffset
	{
		get
		{
			return this._fingerSpreadOffset;
		}
		set
		{
			if (this._fingerSpreadOffset != value)
			{
				this._fingerSpreadOffset = value;
				if (this.fingerSpreadOffsetSlider != null)
				{
					this.fingerSpreadOffsetSlider.value = this._fingerSpreadOffset;
				}
				this.SyncFingerSpreadOffset();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FD7 RID: 24535 RVA: 0x002448EC File Offset: 0x00242CEC
	private void SyncFingerBendOffset()
	{
		if (this.steamVRLeftHandInput != null)
		{
			this.steamVRLeftHandInput.fingerBendOffset = this._fingerBendOffset;
		}
		if (this.steamVRRightHandInput != null)
		{
			this.steamVRRightHandInput.fingerBendOffset = this._fingerBendOffset;
		}
		if (this.ovrLeftHandInput != null)
		{
			this.ovrLeftHandInput.fingerBendOffset = this._fingerBendOffset;
		}
		if (this.ovrRightHandInput != null)
		{
			this.ovrRightHandInput.fingerBendOffset = this._fingerBendOffset;
		}
	}

	// Token: 0x17000E19 RID: 3609
	// (get) Token: 0x06005FD8 RID: 24536 RVA: 0x00244981 File Offset: 0x00242D81
	// (set) Token: 0x06005FD9 RID: 24537 RVA: 0x0024498C File Offset: 0x00242D8C
	public float fingerBendOffset
	{
		get
		{
			return this._fingerBendOffset;
		}
		set
		{
			if (this._fingerBendOffset != value)
			{
				this._fingerBendOffset = value;
				if (this.fingerBendOffsetSlider != null)
				{
					this.fingerBendOffsetSlider.value = this._fingerBendOffset;
				}
				this.SyncFingerBendOffset();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FDA RID: 24538 RVA: 0x002449DC File Offset: 0x00242DDC
	private void SyncThumbSpreadOffset()
	{
		if (this.steamVRLeftHandInput != null)
		{
			this.steamVRLeftHandInput.thumbSpreadOffset = this._thumbSpreadOffset;
		}
		if (this.steamVRRightHandInput != null)
		{
			this.steamVRRightHandInput.thumbSpreadOffset = this._thumbSpreadOffset;
		}
		if (this.ovrLeftHandInput != null)
		{
			this.ovrLeftHandInput.thumbSpreadOffset = this._thumbSpreadOffset;
		}
		if (this.ovrRightHandInput != null)
		{
			this.ovrRightHandInput.thumbSpreadOffset = this._thumbSpreadOffset;
		}
	}

	// Token: 0x17000E1A RID: 3610
	// (get) Token: 0x06005FDB RID: 24539 RVA: 0x00244A71 File Offset: 0x00242E71
	// (set) Token: 0x06005FDC RID: 24540 RVA: 0x00244A7C File Offset: 0x00242E7C
	public float thumbSpreadOffset
	{
		get
		{
			return this._thumbSpreadOffset;
		}
		set
		{
			if (this._thumbSpreadOffset != value)
			{
				this._thumbSpreadOffset = value;
				if (this.thumbSpreadOffsetSlider != null)
				{
					this.thumbSpreadOffsetSlider.value = this._thumbSpreadOffset;
				}
				this.SyncThumbSpreadOffset();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FDD RID: 24541 RVA: 0x00244ACC File Offset: 0x00242ECC
	private void SyncThumbBendOffset()
	{
		if (this.steamVRLeftHandInput != null)
		{
			this.steamVRLeftHandInput.thumbBendOffset = this._thumbBendOffset;
		}
		if (this.steamVRRightHandInput != null)
		{
			this.steamVRRightHandInput.thumbBendOffset = this._thumbBendOffset;
		}
		if (this.ovrLeftHandInput != null)
		{
			this.ovrLeftHandInput.thumbBendOffset = this._thumbBendOffset;
		}
		if (this.ovrRightHandInput != null)
		{
			this.ovrRightHandInput.thumbBendOffset = this._thumbBendOffset;
		}
	}

	// Token: 0x17000E1B RID: 3611
	// (get) Token: 0x06005FDE RID: 24542 RVA: 0x00244B61 File Offset: 0x00242F61
	// (set) Token: 0x06005FDF RID: 24543 RVA: 0x00244B6C File Offset: 0x00242F6C
	public float thumbBendOffset
	{
		get
		{
			return this._thumbBendOffset;
		}
		set
		{
			if (this._thumbBendOffset != value)
			{
				this._thumbBendOffset = value;
				if (this.thumbBendOffsetSlider != null)
				{
					this.thumbBendOffsetSlider.value = this._thumbBendOffset;
				}
				this.SyncThumbBendOffset();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x06005FE0 RID: 24544 RVA: 0x00244BBA File Offset: 0x00242FBA
	// (set) Token: 0x06005FE1 RID: 24545 RVA: 0x00244BC2 File Offset: 0x00242FC2
	public bool oculusSwapGrabAndTrigger
	{
		get
		{
			return this._oculusSwapGrabAndTrigger;
		}
		set
		{
			if (this._oculusSwapGrabAndTrigger != value)
			{
				this._oculusSwapGrabAndTrigger = value;
				if (this.oculusSwapGrabAndTriggerToggle != null)
				{
					this.oculusSwapGrabAndTriggerToggle.isOn = this._oculusSwapGrabAndTrigger;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x06005FE2 RID: 24546 RVA: 0x00244BFF File Offset: 0x00242FFF
	// (set) Token: 0x06005FE3 RID: 24547 RVA: 0x00244C07 File Offset: 0x00243007
	public bool oculusDisableFreeMove
	{
		get
		{
			return this._oculusDisableFreeMove;
		}
		set
		{
			if (this._oculusDisableFreeMove != value)
			{
				this._oculusDisableFreeMove = value;
				if (this.oculusDisableFreeMoveToggle != null)
				{
					this.oculusDisableFreeMoveToggle.isOn = this._oculusDisableFreeMove;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FE4 RID: 24548 RVA: 0x00244C44 File Offset: 0x00243044
	private void SyncShadowBlur()
	{
		Shader.SetGlobalFloat("_ShadowPointKernel", this._pointLightShadowBlur);
	}

	// Token: 0x17000E1E RID: 3614
	// (get) Token: 0x06005FE5 RID: 24549 RVA: 0x00244C56 File Offset: 0x00243056
	// (set) Token: 0x06005FE6 RID: 24550 RVA: 0x00244C60 File Offset: 0x00243060
	public float pointLightShadowBlur
	{
		get
		{
			return this._pointLightShadowBlur;
		}
		set
		{
			if (this._pointLightShadowBlur != value)
			{
				this._pointLightShadowBlur = value;
				this.SyncShadowBlur();
				if (this.pointLightShadowBlurSlider != null)
				{
					this.pointLightShadowBlurSlider.value = this._pointLightShadowBlur;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FE7 RID: 24551 RVA: 0x00244CAE File Offset: 0x002430AE
	private void SyncShadowBiasBase()
	{
		Shader.SetGlobalFloat("_ShadowPointBiasBase", this._pointLightShadowBiasBase);
	}

	// Token: 0x17000E1F RID: 3615
	// (get) Token: 0x06005FE8 RID: 24552 RVA: 0x00244CC0 File Offset: 0x002430C0
	// (set) Token: 0x06005FE9 RID: 24553 RVA: 0x00244CC8 File Offset: 0x002430C8
	public float pointLightShadowBiasBase
	{
		get
		{
			return this._pointLightShadowBiasBase;
		}
		set
		{
			if (this._pointLightShadowBiasBase != value)
			{
				this._pointLightShadowBiasBase = value;
				this.SyncShadowBiasBase();
				if (this.pointLightShadowBiasBaseSlider != null)
				{
					this.pointLightShadowBiasBaseSlider.value = this._pointLightShadowBiasBase;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FEA RID: 24554 RVA: 0x00244D18 File Offset: 0x00243118
	private void SyncShadowFilterLevel()
	{
		int value = (int)this._shadowFilterLevel;
		Shader.SetGlobalInt("_ShadowFilterLevel", value);
	}

	// Token: 0x17000E20 RID: 3616
	// (get) Token: 0x06005FEB RID: 24555 RVA: 0x00244D38 File Offset: 0x00243138
	// (set) Token: 0x06005FEC RID: 24556 RVA: 0x00244D40 File Offset: 0x00243140
	public float shadowFilterLevel
	{
		get
		{
			return this._shadowFilterLevel;
		}
		set
		{
			if (this._shadowFilterLevel != value)
			{
				this._shadowFilterLevel = value;
				this.SyncShadowFilterLevel();
				if (this.shadowFilterLevelSlider != null)
				{
					this.shadowFilterLevelSlider.value = this._shadowFilterLevel;
				}
			}
		}
	}

	// Token: 0x06005FED RID: 24557 RVA: 0x00244D7E File Offset: 0x0024317E
	private void SyncCloseObjectBlur()
	{
	}

	// Token: 0x17000E21 RID: 3617
	// (get) Token: 0x06005FEE RID: 24558 RVA: 0x00244D80 File Offset: 0x00243180
	// (set) Token: 0x06005FEF RID: 24559 RVA: 0x00244D88 File Offset: 0x00243188
	public bool closeObjectBlur
	{
		get
		{
			return this._closeObjectBlur;
		}
		set
		{
			if (this._closeObjectBlur != value)
			{
				this._closeObjectBlur = value;
				if (this.closeObjectBlurToggle != null)
				{
					this.closeObjectBlurToggle.isOn = this._closeObjectBlur;
				}
				this.SyncCloseObjectBlur();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FF0 RID: 24560 RVA: 0x00244DD6 File Offset: 0x002431D6
	private void SyncSoftPhysics()
	{
		DAZPhysicsMesh.globalEnable = this.softPhysics;
	}

	// Token: 0x17000E22 RID: 3618
	// (get) Token: 0x06005FF1 RID: 24561 RVA: 0x00244DE3 File Offset: 0x002431E3
	// (set) Token: 0x06005FF2 RID: 24562 RVA: 0x00244E18 File Offset: 0x00243218
	public bool softPhysics
	{
		get
		{
			if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.overrideSoftPhysics)
			{
				return GlobalSceneOptions.singleton.softPhysics;
			}
			return this._softPhysics;
		}
		set
		{
			if (this._softPhysics != value)
			{
				this._softPhysics = value;
				if (this.softPhysicsToggle != null)
				{
					this.softPhysicsToggle.isOn = this._softPhysics;
				}
				this.SyncSoftPhysics();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FF3 RID: 24563 RVA: 0x00244E66 File Offset: 0x00243266
	public void RegisterGlowObject()
	{
		this.glowObjectCount++;
		this.SyncGlow();
	}

	// Token: 0x06005FF4 RID: 24564 RVA: 0x00244E7C File Offset: 0x0024327C
	public void DeregisterGlowObject()
	{
		this.glowObjectCount--;
		this.SyncGlow();
	}

	// Token: 0x06005FF5 RID: 24565 RVA: 0x00244E92 File Offset: 0x00243292
	public void RegisterGlowCamera(MKGlow mkg)
	{
		this.dynamicGlow.Add(mkg);
		this.SyncGlow();
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x00244EA6 File Offset: 0x002432A6
	public void DeregisterGlowCamera(MKGlow mkg)
	{
		this.dynamicGlow.Remove(mkg);
	}

	// Token: 0x06005FF7 RID: 24567 RVA: 0x00244EB8 File Offset: 0x002432B8
	private void SyncGlow()
	{
		List<MKGlow> list = new List<MKGlow>();
		foreach (MKGlow item in this.glowObjects)
		{
			list.Add(item);
		}
		foreach (MKGlow item2 in this.dynamicGlow)
		{
			list.Add(item2);
		}
		foreach (MKGlow mkglow in list)
		{
			if (this._pauseGlow)
			{
				mkglow.enabled = false;
			}
			else
			{
				UserPreferences.GlowEffectsLevel glowEffects = this._glowEffects;
				if (glowEffects != UserPreferences.GlowEffectsLevel.Off)
				{
					if (glowEffects != UserPreferences.GlowEffectsLevel.Low)
					{
						if (glowEffects == UserPreferences.GlowEffectsLevel.High)
						{
							mkglow.enabled = (this.glowObjectCount > 0);
							mkglow.Samples = 2f;
						}
					}
					else
					{
						mkglow.enabled = (this.glowObjectCount > 0);
						mkglow.Samples = 3f;
					}
				}
				else
				{
					mkglow.enabled = false;
				}
			}
		}
	}

	// Token: 0x17000E23 RID: 3619
	// (get) Token: 0x06005FF8 RID: 24568 RVA: 0x00245010 File Offset: 0x00243410
	// (set) Token: 0x06005FF9 RID: 24569 RVA: 0x00245018 File Offset: 0x00243418
	public bool pauseGlow
	{
		get
		{
			return this._pauseGlow;
		}
		set
		{
			this._pauseGlow = value;
			this.SyncGlow();
		}
	}

	// Token: 0x17000E24 RID: 3620
	// (get) Token: 0x06005FFA RID: 24570 RVA: 0x00245027 File Offset: 0x00243427
	// (set) Token: 0x06005FFB RID: 24571 RVA: 0x00245030 File Offset: 0x00243430
	public UserPreferences.GlowEffectsLevel glowEffects
	{
		get
		{
			return this._glowEffects;
		}
		set
		{
			if (this._glowEffects != value)
			{
				this._glowEffects = value;
				if (this.glowEffectsPopup != null)
				{
					this.glowEffectsPopup.currentValue = this._glowEffects.ToString();
				}
				this.SyncGlow();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06005FFC RID: 24572 RVA: 0x0024508C File Offset: 0x0024348C
	public void SetGlowEffectsFromString(string ge)
	{
		try
		{
			this.glowEffects = (UserPreferences.GlowEffectsLevel)Enum.Parse(typeof(UserPreferences.GlowEffectsLevel), ge);
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set glow effects level " + ge + " which is not a valid glow effects string");
		}
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x002450E4 File Offset: 0x002434E4
	private void SetPhysics45()
	{
		Time.fixedDeltaTime = 0.02222222f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 50 * num;
		}
	}

	// Token: 0x06005FFE RID: 24574 RVA: 0x00245128 File Offset: 0x00243528
	private void SetPhysics60()
	{
		Time.fixedDeltaTime = 0.01666667f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 30 * num;
		}
	}

	// Token: 0x06005FFF RID: 24575 RVA: 0x0024516C File Offset: 0x0024356C
	private void SetPhysics72()
	{
		Time.fixedDeltaTime = 0.01388889f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 20 * num;
		}
	}

	// Token: 0x06006000 RID: 24576 RVA: 0x002451B0 File Offset: 0x002435B0
	private void SetPhysics80()
	{
		Time.fixedDeltaTime = 0.0125f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 17 * num;
		}
	}

	// Token: 0x06006001 RID: 24577 RVA: 0x002451F4 File Offset: 0x002435F4
	private void SetPhysics90()
	{
		Time.fixedDeltaTime = 0.011111111f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 14 * num;
		}
	}

	// Token: 0x06006002 RID: 24578 RVA: 0x00245238 File Offset: 0x00243638
	private void SetPhysics120()
	{
		Time.fixedDeltaTime = 0.008333333f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 10 * num;
		}
	}

	// Token: 0x06006003 RID: 24579 RVA: 0x0024527C File Offset: 0x0024367C
	private void SetPhysics144()
	{
		Time.fixedDeltaTime = 0.0069444445f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 8 * num;
		}
	}

	// Token: 0x06006004 RID: 24580 RVA: 0x002452C0 File Offset: 0x002436C0
	private void SetPhysics240()
	{
		Time.fixedDeltaTime = 0.004166667f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 7 * num;
		}
	}

	// Token: 0x06006005 RID: 24581 RVA: 0x00245304 File Offset: 0x00243704
	private void SetPhysics288()
	{
		Time.fixedDeltaTime = 0.0034722222f;
		int num = 1;
		if (this._physicsHighQuality)
		{
			num = 5;
		}
		if (SuperController.singleton != null)
		{
			SuperController.singleton.solverIterations = 7 * num;
		}
	}

	// Token: 0x06006006 RID: 24582 RVA: 0x00245348 File Offset: 0x00243748
	private void SyncPhysics()
	{
		if (this.autoPhysicsRateText != null)
		{
			this.autoPhysicsRateText.text = string.Empty;
		}
		switch (this.physicsRate)
		{
		case UserPreferences.PhysicsRate.Auto:
			if (!XRDevice.isPresent)
			{
				this.SetPhysics72();
				if (this.autoPhysicsRateText != null)
				{
					this.autoPhysicsRateText.text = "72 HZ";
				}
			}
			else
			{
				float num = XRDevice.refreshRate;
				if (num > 91f)
				{
					num *= 0.5f;
				}
				if (num != 0f)
				{
					if (num < 59f)
					{
						this.SetPhysics45();
						if (this.autoPhysicsRateText != null)
						{
							this.autoPhysicsRateText.text = "45 HZ";
						}
					}
					else if (num < 71f)
					{
						this.SetPhysics60();
						if (this.autoPhysicsRateText != null)
						{
							this.autoPhysicsRateText.text = "60 HZ";
						}
					}
					else if (num < 79f)
					{
						this.SetPhysics72();
						if (this.autoPhysicsRateText != null)
						{
							this.autoPhysicsRateText.text = "72 HZ";
						}
					}
					else if (num < 89f)
					{
						this.SetPhysics80();
						if (this.autoPhysicsRateText != null)
						{
							this.autoPhysicsRateText.text = "80 HZ";
						}
					}
					else
					{
						this.SetPhysics90();
						if (this.autoPhysicsRateText != null)
						{
							this.autoPhysicsRateText.text = "90 HZ";
						}
					}
				}
				else
				{
					this.SetPhysics90();
					if (this.autoPhysicsRateText != null)
					{
						this.autoPhysicsRateText.text = "90 HZ";
					}
				}
			}
			break;
		case UserPreferences.PhysicsRate._45:
			this.SetPhysics45();
			break;
		case UserPreferences.PhysicsRate._60:
			this.SetPhysics60();
			break;
		case UserPreferences.PhysicsRate._72:
			this.SetPhysics72();
			break;
		case UserPreferences.PhysicsRate._80:
			this.SetPhysics80();
			break;
		case UserPreferences.PhysicsRate._90:
			this.SetPhysics90();
			break;
		case UserPreferences.PhysicsRate._120:
			this.SetPhysics120();
			break;
		case UserPreferences.PhysicsRate._144:
			this.SetPhysics144();
			break;
		case UserPreferences.PhysicsRate._240:
			this.SetPhysics240();
			break;
		case UserPreferences.PhysicsRate._288:
			this.SetPhysics288();
			break;
		}
		int physicsUpdateCap = this.physicsUpdateCap;
		if (physicsUpdateCap != 1)
		{
			if (physicsUpdateCap != 2)
			{
				if (physicsUpdateCap == 3)
				{
					Time.maximumDeltaTime = Time.fixedDeltaTime * 3f;
				}
			}
			else
			{
				Time.maximumDeltaTime = Time.fixedDeltaTime * 2f;
			}
		}
		else
		{
			Time.maximumDeltaTime = Time.fixedDeltaTime;
		}
		if (Time.maximumDeltaTime > 0.05f)
		{
			Time.maximumDeltaTime = 0.05f;
		}
	}

	// Token: 0x17000E25 RID: 3621
	// (get) Token: 0x06006007 RID: 24583 RVA: 0x00245616 File Offset: 0x00243A16
	// (set) Token: 0x06006008 RID: 24584 RVA: 0x00245648 File Offset: 0x00243A48
	public UserPreferences.PhysicsRate physicsRate
	{
		get
		{
			if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.overridePhysicsRate)
			{
				return GlobalSceneOptions.singleton.physicsRate;
			}
			return this._physicsRate;
		}
		set
		{
			if (this._physicsRate != value)
			{
				this._physicsRate = value;
				if (this.physicsRatePopup != null)
				{
					this.physicsRatePopup.currentValue = this._physicsRate.ToString();
				}
				this.SyncPhysics();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006009 RID: 24585 RVA: 0x002456A4 File Offset: 0x00243AA4
	public void SetPhysicsRateFromString(string pr)
	{
		try
		{
			this.physicsRate = (UserPreferences.PhysicsRate)Enum.Parse(typeof(UserPreferences.PhysicsRate), pr);
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set physics rate " + pr + " which is not a valid physics rate string. Resetting to Auto");
			this.physicsRate = UserPreferences.PhysicsRate.Auto;
		}
	}

	// Token: 0x17000E26 RID: 3622
	// (get) Token: 0x0600600A RID: 24586 RVA: 0x00245704 File Offset: 0x00243B04
	// (set) Token: 0x0600600B RID: 24587 RVA: 0x00245738 File Offset: 0x00243B38
	public int physicsUpdateCap
	{
		get
		{
			if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.overridePhysicsUpdateCap)
			{
				return GlobalSceneOptions.singleton.physicsUpdateCap;
			}
			return this._physicsUpdateCap;
		}
		set
		{
			if (this._physicsUpdateCap != value)
			{
				this._physicsUpdateCap = value;
				if (this.physicsUpdateCapPopup != null)
				{
					this.physicsUpdateCapPopup.currentValue = this._physicsUpdateCap.ToString();
				}
				this.SyncPhysics();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x0600600C RID: 24588 RVA: 0x00245794 File Offset: 0x00243B94
	public void SetPhysicsUpdateCapFromString(string pr)
	{
		try
		{
			this.physicsUpdateCap = int.Parse(pr);
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set physics update cap " + pr + " which is not a valid physics update cap string");
		}
	}

	// Token: 0x17000E27 RID: 3623
	// (get) Token: 0x0600600D RID: 24589 RVA: 0x002457E0 File Offset: 0x00243BE0
	// (set) Token: 0x0600600E RID: 24590 RVA: 0x00245814 File Offset: 0x00243C14
	public bool physicsHighQuality
	{
		get
		{
			if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.overridePhysicsHighQuality)
			{
				return GlobalSceneOptions.singleton.physicsHighQuality;
			}
			return this._physicsHighQuality;
		}
		set
		{
			if (this._physicsHighQuality != value)
			{
				this._physicsHighQuality = value;
				if (this.physicsHighQualityToggle != null)
				{
					this.physicsHighQualityToggle.isOn = this._physicsHighQuality;
				}
				this.SyncPhysics();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x0600600F RID: 24591 RVA: 0x00245862 File Offset: 0x00243C62
	private void SyncHeadCollider()
	{
		if (this.headCollider != null)
		{
			this.headCollider.gameObject.SetActive(this._useHeadCollider);
		}
	}

	// Token: 0x17000E28 RID: 3624
	// (get) Token: 0x06006010 RID: 24592 RVA: 0x0024588B File Offset: 0x00243C8B
	// (set) Token: 0x06006011 RID: 24593 RVA: 0x00245893 File Offset: 0x00243C93
	public bool useHeadCollider
	{
		get
		{
			return this._useHeadCollider;
		}
		set
		{
			if (this._useHeadCollider != value)
			{
				this._useHeadCollider = value;
				if (this.useHeadColliderToggle != null)
				{
					this.useHeadColliderToggle.isOn = value;
				}
				this.SyncHeadCollider();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E29 RID: 3625
	// (get) Token: 0x06006012 RID: 24594 RVA: 0x002458D1 File Offset: 0x00243CD1
	// (set) Token: 0x06006013 RID: 24595 RVA: 0x002458D9 File Offset: 0x00243CD9
	public bool optimizeMemoryOnSceneLoad
	{
		get
		{
			return this._optimizeMemoryOnSceneLoad;
		}
		set
		{
			if (this._optimizeMemoryOnSceneLoad != value)
			{
				this._optimizeMemoryOnSceneLoad = value;
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x06006014 RID: 24596 RVA: 0x002458F4 File Offset: 0x00243CF4
	// (set) Token: 0x06006015 RID: 24597 RVA: 0x002458FC File Offset: 0x00243CFC
	public bool optimizeMemoryOnPresetLoad
	{
		get
		{
			return this._optimizeMemoryOnPresetLoad;
		}
		set
		{
			if (this._optimizeMemoryOnPresetLoad != value)
			{
				this._optimizeMemoryOnPresetLoad = value;
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E2B RID: 3627
	// (get) Token: 0x06006016 RID: 24598 RVA: 0x00245917 File Offset: 0x00243D17
	// (set) Token: 0x06006017 RID: 24599 RVA: 0x00245920 File Offset: 0x00243D20
	public bool enableCaching
	{
		get
		{
			return this._enableCaching;
		}
		set
		{
			if (this._enableCaching != value)
			{
				this._enableCaching = value;
				CacheManager.CachingEnabled = this._enableCaching;
				FileManager.SyncJSONCache();
				if (this.enableCachingToggle != null)
				{
					this.enableCachingToggle.isOn = value;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E2C RID: 3628
	// (get) Token: 0x06006018 RID: 24600 RVA: 0x00245973 File Offset: 0x00243D73
	// (set) Token: 0x06006019 RID: 24601 RVA: 0x0024597C File Offset: 0x00243D7C
	public string cacheFolder
	{
		get
		{
			return this._cacheFolder;
		}
		private set
		{
			if (this._cacheFolder != value)
			{
				this._cacheFolder = value;
				CacheManager.SetCacheDir(this._cacheFolder);
				if (this.cacheFolderText != null)
				{
					this.cacheFolderText.text = this._cacheFolder;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x0600601A RID: 24602 RVA: 0x002459D4 File Offset: 0x00243DD4
	private void SetCacheFolder(string folder)
	{
		if (folder != null && folder != string.Empty)
		{
			folder = folder.Replace("\\\\", "\\");
			this.cacheFolder = folder;
		}
	}

	// Token: 0x0600601B RID: 24603 RVA: 0x00245A05 File Offset: 0x00243E05
	public void BrowseCacheFolder()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.GetDirectoryPathDialog(new FileBrowserCallback(this.SetCacheFolder), this._cacheFolder, null, true);
		}
	}

	// Token: 0x0600601C RID: 24604 RVA: 0x00245A35 File Offset: 0x00243E35
	public void ResetCacheFolder()
	{
		CacheManager.ResetCacheDir();
		this.cacheFolder = CacheManager.GetCacheDir();
	}

	// Token: 0x17000E2D RID: 3629
	// (get) Token: 0x0600601D RID: 24605 RVA: 0x00245A47 File Offset: 0x00243E47
	// (set) Token: 0x0600601E RID: 24606 RVA: 0x00245A4F File Offset: 0x00243E4F
	public bool confirmLoad
	{
		get
		{
			return this._confirmLoad;
		}
		set
		{
			if (this._confirmLoad != value)
			{
				this._confirmLoad = value;
				if (this.confirmLoadToggle != null)
				{
					this.confirmLoadToggle.isOn = this._confirmLoad;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E2E RID: 3630
	// (get) Token: 0x0600601F RID: 24607 RVA: 0x00245A8C File Offset: 0x00243E8C
	// (set) Token: 0x06006020 RID: 24608 RVA: 0x00245A94 File Offset: 0x00243E94
	public bool flipToolbar
	{
		get
		{
			return this._flipToolbar;
		}
		set
		{
			if (this._flipToolbar != value)
			{
				this._flipToolbar = value;
				if (this.flipToolbarToggle != null)
				{
					this.flipToolbarToggle.isOn = this._flipToolbar;
				}
				this.toolbarFlipper.flipped = this._flipToolbar;
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x00245AF0 File Offset: 0x00243EF0
	private void SyncOverlayUI()
	{
		if (this.panelForUIMaterial != null && this.overlayUIShader != null)
		{
			Material defaultMaterial = this.panelForUIMaterial.defaultMaterial;
			if (this.defaultUIShader == null)
			{
				this.defaultUIShader = defaultMaterial.shader;
			}
			if (this._overlayUI)
			{
				defaultMaterial.shader = this.overlayUIShader;
			}
			else
			{
				defaultMaterial.shader = this.defaultUIShader;
			}
		}
	}

	// Token: 0x17000E2F RID: 3631
	// (get) Token: 0x06006022 RID: 24610 RVA: 0x00245B70 File Offset: 0x00243F70
	// (set) Token: 0x06006023 RID: 24611 RVA: 0x00245B78 File Offset: 0x00243F78
	public bool overlayUI
	{
		get
		{
			return this._overlayUI;
		}
		set
		{
			if (this._overlayUI != value)
			{
				this._overlayUI = value;
				if (this.overlayUIToggle != null)
				{
					this.overlayUIToggle.isOn = this._overlayUI;
				}
				this.SyncOverlayUI();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006024 RID: 24612 RVA: 0x00245BC6 File Offset: 0x00243FC6
	protected void SyncEnableWebBrowser()
	{
		if (HubBrowse.singleton != null)
		{
			HubBrowse.singleton.WebBrowserEnabled = this._enableWebBrowser;
		}
	}

	// Token: 0x17000E30 RID: 3632
	// (get) Token: 0x06006025 RID: 24613 RVA: 0x00245BE8 File Offset: 0x00243FE8
	// (set) Token: 0x06006026 RID: 24614 RVA: 0x00245BF0 File Offset: 0x00243FF0
	public bool enableWebBrowser
	{
		get
		{
			return this._enableWebBrowser;
		}
		set
		{
			if (this._enableWebBrowser != value)
			{
				this._enableWebBrowser = value;
				if (this.enableWebBrowserToggle != null)
				{
					this.enableWebBrowserToggle.isOn = value;
				}
				if (this.enableWebBrowserToggleAlt != null)
				{
					this.enableWebBrowserToggleAlt.isOn = value;
				}
				this.SyncEnableWebBrowser();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006027 RID: 24615 RVA: 0x00245C58 File Offset: 0x00244058
	protected string ExtractTopDomain(string url)
	{
		string text = Regex.Replace(url, "^https://", string.Empty);
		text = Regex.Replace(text, "^http://", string.Empty);
		text = Regex.Replace(text, "/.*", string.Empty);
		Match match;
		if ((match = Regex.Match(text, "\\.([^\\.]+\\.[^\\.]+)")).Success)
		{
			string value = match.Groups[1].Value;
			text = value;
		}
		return text;
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x00245CC8 File Offset: 0x002440C8
	public bool CheckWhitelistDomain(string url)
	{
		if (url == null || this._allowNonWhitelistDomains)
		{
			return true;
		}
		if (url == "about:blank")
		{
			return true;
		}
		if (this.whitelistDomains != null)
		{
			string item = this.ExtractTopDomain(url);
			return this.whitelistDomains.Contains(item);
		}
		return false;
	}

	// Token: 0x06006029 RID: 24617 RVA: 0x00245D1C File Offset: 0x0024411C
	public void SyncWhitelistDomains()
	{
		this.whitelistDomains = new HashSet<string>();
		foreach (string path in this.whitelistDomainPaths)
		{
			if (File.Exists(path))
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(path))
					{
						string aJSON = streamReader.ReadToEnd();
						SimpleJSON.JSONNode jsonnode = JSON.Parse(aJSON);
						JSONArray asArray = jsonnode["sites"].AsArray;
						if (asArray != null)
						{
							for (int j = 0; j < asArray.Count; j++)
							{
								string item = this.ExtractTopDomain(asArray[j]);
								if (!this.whitelistDomains.Contains(item))
								{
									this.whitelistDomains.Add(item);
								}
							}
						}
					}
				}
				catch (Exception arg)
				{
					SuperController.LogError("Exception while reading whitelist sites file " + arg);
				}
			}
		}
	}

	// Token: 0x17000E31 RID: 3633
	// (get) Token: 0x0600602A RID: 24618 RVA: 0x00245E34 File Offset: 0x00244234
	// (set) Token: 0x0600602B RID: 24619 RVA: 0x00245E3C File Offset: 0x0024423C
	public bool allowNonWhitelistDomains
	{
		get
		{
			return this._allowNonWhitelistDomains;
		}
		set
		{
			if (this._allowNonWhitelistDomains != value)
			{
				this._allowNonWhitelistDomains = value;
				if (this.allowNonWhitelistDomainsToggle != null)
				{
					this.allowNonWhitelistDomainsToggle.isOn = value;
				}
				if (this.allowNonWhitelistDomainsToggleAlt != null)
				{
					this.allowNonWhitelistDomainsToggleAlt.isOn = value;
				}
				if (this.allowNonWhitelistDomainsToggleAlt2 != null)
				{
					this.allowNonWhitelistDomainsToggleAlt2.isOn = value;
				}
				if (this.allowNonWhitelistDomainsToggleAlt3 != null)
				{
					this.allowNonWhitelistDomainsToggleAlt3.isOn = value;
				}
				this.SyncWhitelistDomains();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x0600602C RID: 24620 RVA: 0x00245EDC File Offset: 0x002442DC
	protected void SyncEnableWebBrowserProfile()
	{
		if (this._enableWebBrowserProfile)
		{
			try
			{
				Directory.CreateDirectory("BrowserProfile");
				BrowserNative.ProfilePath = Path.GetFullPath("BrowserProfile");
			}
			catch (Exception)
			{
				SuperController.LogError("VaM must be restarted for the web browser profile to be enabled because the web browser has already been activated.");
			}
		}
		else
		{
			try
			{
				BrowserNative.ProfilePath = null;
				if (Directory.Exists("BrowserProfile"))
				{
					Directory.Delete("BrowserProfile", true);
				}
			}
			catch (Exception)
			{
				SuperController.LogError("VaM must be restarted for the web browser profile to be disabled and the removal of BrowserProfile directory to take effect because the web browser has already been activated.");
			}
		}
	}

	// Token: 0x17000E32 RID: 3634
	// (get) Token: 0x0600602D RID: 24621 RVA: 0x00245F7C File Offset: 0x0024437C
	// (set) Token: 0x0600602E RID: 24622 RVA: 0x00245F84 File Offset: 0x00244384
	public bool enableWebBrowserProfile
	{
		get
		{
			return this._enableWebBrowserProfile;
		}
		set
		{
			if (this._enableWebBrowserProfile != value)
			{
				this._enableWebBrowserProfile = value;
				if (this.enableWebBrowserProfileToggle != null)
				{
					this.enableWebBrowserProfileToggle.isOn = value;
				}
				if (this.enableWebBrowserProfileToggleAlt != null)
				{
					this.enableWebBrowserProfileToggleAlt.isOn = value;
				}
				this.SavePreferences();
				this.SyncEnableWebBrowserProfile();
			}
		}
	}

	// Token: 0x17000E33 RID: 3635
	// (get) Token: 0x0600602F RID: 24623 RVA: 0x00245FEA File Offset: 0x002443EA
	// (set) Token: 0x06006030 RID: 24624 RVA: 0x00245FF4 File Offset: 0x002443F4
	public bool enableWebMisc
	{
		get
		{
			return this._enableWebMisc;
		}
		set
		{
			if (this._enableWebMisc != value)
			{
				this._enableWebMisc = value;
				if (this.enableWebMiscToggle != null)
				{
					this.enableWebMiscToggle.isOn = value;
				}
				if (this.enableWebMiscToggleAlt != null)
				{
					this.enableWebMiscToggleAlt.isOn = value;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006031 RID: 24625 RVA: 0x00246054 File Offset: 0x00244454
	protected void SyncEnableHub()
	{
		if (HubBrowse.singleton != null)
		{
			HubBrowse.singleton.HubEnabled = this._enableHub;
		}
	}

	// Token: 0x17000E34 RID: 3636
	// (get) Token: 0x06006032 RID: 24626 RVA: 0x00246076 File Offset: 0x00244476
	// (set) Token: 0x06006033 RID: 24627 RVA: 0x00246080 File Offset: 0x00244480
	public bool enableHub
	{
		get
		{
			return this._enableHub;
		}
		set
		{
			if (this._enableHub != value)
			{
				this._enableHub = value;
				if (this.enableHubToggle != null)
				{
					this.enableHubToggle.isOn = value;
				}
				if (this.enableHubToggleAlt != null)
				{
					this.enableHubToggleAlt.isOn = value;
				}
				this.SyncEnableHub();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006034 RID: 24628 RVA: 0x002460E6 File Offset: 0x002444E6
	protected void SyncEnableHubDownloader()
	{
		if (HubDownloader.singleton != null)
		{
			HubDownloader.singleton.HubDownloaderEnabled = this._enableHubDownloader;
		}
	}

	// Token: 0x17000E35 RID: 3637
	// (get) Token: 0x06006035 RID: 24629 RVA: 0x00246108 File Offset: 0x00244508
	// (set) Token: 0x06006036 RID: 24630 RVA: 0x00246110 File Offset: 0x00244510
	public bool enableHubDownloader
	{
		get
		{
			return this._enableHubDownloader;
		}
		set
		{
			if (this._enableHubDownloader != value)
			{
				this._enableHubDownloader = value;
				if (this.enableHubDownloaderToggle != null)
				{
					this.enableHubDownloaderToggle.isOn = value;
				}
				if (this.enableHubDownloaderToggleAlt != null)
				{
					this.enableHubDownloaderToggleAlt.isOn = value;
				}
				this.SyncEnableHubDownloader();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E36 RID: 3638
	// (get) Token: 0x06006037 RID: 24631 RVA: 0x00246176 File Offset: 0x00244576
	// (set) Token: 0x06006038 RID: 24632 RVA: 0x00246180 File Offset: 0x00244580
	public bool enablePlugins
	{
		get
		{
			return this._enablePlugins;
		}
		set
		{
			if (this._enablePlugins != value)
			{
				this._enablePlugins = value;
				if (this.enablePluginsToggle != null)
				{
					this.enablePluginsToggle.isOn = value;
				}
				if (this.enablePluginsToggleAlt != null)
				{
					this.enablePluginsToggleAlt.isOn = value;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006039 RID: 24633 RVA: 0x002461E0 File Offset: 0x002445E0
	private void SyncAllowPluginsNetworkAccess()
	{
		if (this._allowPluginsNetworkAccess)
		{
			RuntimeRestrictions.RemoveRuntimeNamespaceRestriction("System.Net");
			RuntimeRestrictions.RemoveRuntimeNamespaceRestriction("UnityEngine.Network");
			RuntimeRestrictions.RemoveRuntimeNamespaceRestriction("UnityEngine.Networking");
			RuntimeRestrictions.RemoveRuntimeNamespaceRestriction("ZenFulcrum.EmbeddedBrowser");
		}
		else
		{
			RuntimeRestrictions.AddRuntimeNamespaceRestriction("System.Net");
			RuntimeRestrictions.AddRuntimeNamespaceRestriction("UnityEngine.Network");
			RuntimeRestrictions.AddRuntimeNamespaceRestriction("UnityEngine.Networking");
			RuntimeRestrictions.AddRuntimeNamespaceRestriction("ZenFulcrum.EmbeddedBrowser");
		}
	}

	// Token: 0x17000E37 RID: 3639
	// (get) Token: 0x0600603A RID: 24634 RVA: 0x0024624D File Offset: 0x0024464D
	// (set) Token: 0x0600603B RID: 24635 RVA: 0x00246258 File Offset: 0x00244658
	public bool allowPluginsNetworkAccess
	{
		get
		{
			return this._allowPluginsNetworkAccess;
		}
		private set
		{
			if (this._allowPluginsNetworkAccess != value)
			{
				this._allowPluginsNetworkAccess = value;
				if (this.allowPluginsNetworkAccessToggle != null)
				{
					this.allowPluginsNetworkAccessToggle.isOn = value;
				}
				if (this.allowPluginsNetworkAccessToggleAlt != null)
				{
					this.allowPluginsNetworkAccessToggleAlt.isOn = value;
				}
				this.SyncAllowPluginsNetworkAccess();
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E38 RID: 3640
	// (get) Token: 0x0600603C RID: 24636 RVA: 0x002462BE File Offset: 0x002446BE
	// (set) Token: 0x0600603D RID: 24637 RVA: 0x002462C8 File Offset: 0x002446C8
	public bool alwaysAllowPluginsDownloadedFromHub
	{
		get
		{
			return this._alwaysAllowPluginsDownloadedFromHub;
		}
		set
		{
			if (this._alwaysAllowPluginsDownloadedFromHub != value)
			{
				this._alwaysAllowPluginsDownloadedFromHub = value;
				if (this.alwaysAllowPluginsDownloadedFromHubToggle != null)
				{
					this.alwaysAllowPluginsDownloadedFromHubToggle.isOn = value;
				}
				if (this.alwaysAllowPluginsDownloadedFromHubToggleAlt != null)
				{
					this.alwaysAllowPluginsDownloadedFromHubToggleAlt.isOn = value;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E39 RID: 3641
	// (get) Token: 0x0600603E RID: 24638 RVA: 0x00246328 File Offset: 0x00244728
	// (set) Token: 0x0600603F RID: 24639 RVA: 0x00246330 File Offset: 0x00244730
	public bool hideDisabledWebMessages
	{
		get
		{
			return this._hideDisabledWebMessages;
		}
		set
		{
			if (this._hideDisabledWebMessages != value)
			{
				this._hideDisabledWebMessages = value;
				if (this.hideDisabledWebMessagesToggle != null)
				{
					this.hideDisabledWebMessagesToggle.isOn = value;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x06006040 RID: 24640 RVA: 0x00246368 File Offset: 0x00244768
	// (set) Token: 0x06006041 RID: 24641 RVA: 0x00246370 File Offset: 0x00244770
	public string creatorName
	{
		get
		{
			return this._creatorName;
		}
		set
		{
			if (this._creatorName != value)
			{
				this._creatorName = value;
				if (this.creatorNameInputField != null)
				{
					this.creatorNameInputField.text = this._creatorName;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x06006042 RID: 24642 RVA: 0x002463BD File Offset: 0x002447BD
	// (set) Token: 0x06006043 RID: 24643 RVA: 0x002463C8 File Offset: 0x002447C8
	public string DAZExtraLibraryFolder
	{
		get
		{
			return this._DAZExtraLibraryFolder;
		}
		set
		{
			if (this._DAZExtraLibraryFolder != value)
			{
				this._DAZExtraLibraryFolder = value;
				if (this.DAZExtraLibraryFolderText != null)
				{
					this.DAZExtraLibraryFolderText.text = this._DAZExtraLibraryFolder;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006044 RID: 24644 RVA: 0x00246415 File Offset: 0x00244815
	protected void SetDAZExtraLibraryFolder(string folder)
	{
		folder = folder.Replace("\\\\", "\\");
		folder = folder.Replace("\\", "/");
		this.DAZExtraLibraryFolder = folder;
	}

	// Token: 0x06006045 RID: 24645 RVA: 0x00246442 File Offset: 0x00244842
	public void BrowseDAZExtraLibraryFolder()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.GetDirectoryPathDialog(new FileBrowserCallback(this.SetDAZExtraLibraryFolder), this._DAZExtraLibraryFolder, null, true);
		}
	}

	// Token: 0x06006046 RID: 24646 RVA: 0x00246472 File Offset: 0x00244872
	public void ClearDAZExtraLibraryFolder()
	{
		this.DAZExtraLibraryFolder = string.Empty;
	}

	// Token: 0x17000E3C RID: 3644
	// (get) Token: 0x06006047 RID: 24647 RVA: 0x0024647F File Offset: 0x0024487F
	// (set) Token: 0x06006048 RID: 24648 RVA: 0x00246488 File Offset: 0x00244888
	public string DAZDefaultContentFolder
	{
		get
		{
			return this._DAZDefaultContentFolder;
		}
		set
		{
			if (this._DAZDefaultContentFolder != value)
			{
				this._DAZDefaultContentFolder = value;
				if (this.DAZDefaultContentFolderText != null)
				{
					this.DAZDefaultContentFolderText.text = this._DAZDefaultContentFolder;
				}
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006049 RID: 24649 RVA: 0x002464D5 File Offset: 0x002448D5
	protected void SetDAZDefaultContentFolder(string folder)
	{
		folder = folder.Replace("\\\\", "\\");
		folder = folder.Replace("\\", "/");
		this.DAZDefaultContentFolder = folder;
	}

	// Token: 0x0600604A RID: 24650 RVA: 0x00246502 File Offset: 0x00244902
	public void BrowseDAZDefaultContentFolder()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.GetDirectoryPathDialog(new FileBrowserCallback(this.SetDAZDefaultContentFolder), this._DAZDefaultContentFolder, null, true);
		}
	}

	// Token: 0x0600604B RID: 24651 RVA: 0x00246532 File Offset: 0x00244932
	public void ClearDAZDefaultContentFolder()
	{
		this.DAZDefaultContentFolder = string.Empty;
	}

	// Token: 0x0600604C RID: 24652 RVA: 0x0024653F File Offset: 0x0024493F
	private void SyncShadows()
	{
		this.SyncShadowFilterLevel();
		this.SyncShadowBiasBase();
		this.SyncShadowBlur();
	}

	// Token: 0x0600604D RID: 24653 RVA: 0x00246554 File Offset: 0x00244954
	public void SetFileBrowserSortBy(string sortByString)
	{
		try
		{
			UserPreferences.SortBy fileBrowserSortBy = (UserPreferences.SortBy)Enum.Parse(typeof(UserPreferences.SortBy), sortByString);
			this.fileBrowserSortBy = fileBrowserSortBy;
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set sort by to " + sortByString + " which is not a valid type");
		}
	}

	// Token: 0x17000E3D RID: 3645
	// (get) Token: 0x0600604E RID: 24654 RVA: 0x002465B0 File Offset: 0x002449B0
	// (set) Token: 0x0600604F RID: 24655 RVA: 0x002465B8 File Offset: 0x002449B8
	public UserPreferences.SortBy fileBrowserSortBy
	{
		get
		{
			return this._fileBrowserSortBy;
		}
		set
		{
			if (this._fileBrowserSortBy != value)
			{
				this._fileBrowserSortBy = value;
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006050 RID: 24656 RVA: 0x002465D4 File Offset: 0x002449D4
	public void SetFileBrowserDirectoryOption(string dirOptionString)
	{
		try
		{
			UserPreferences.DirectoryOption fileBrowserDirectoryOption = (UserPreferences.DirectoryOption)Enum.Parse(typeof(UserPreferences.DirectoryOption), dirOptionString);
			this.fileBrowserDirectoryOption = fileBrowserDirectoryOption;
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set directory option to " + dirOptionString + " which is not a valid type");
		}
	}

	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x06006051 RID: 24657 RVA: 0x00246630 File Offset: 0x00244A30
	// (set) Token: 0x06006052 RID: 24658 RVA: 0x00246638 File Offset: 0x00244A38
	public UserPreferences.DirectoryOption fileBrowserDirectoryOption
	{
		get
		{
			return this._fileBrowserDirectoryOption;
		}
		set
		{
			if (this._fileBrowserDirectoryOption != value)
			{
				this._fileBrowserDirectoryOption = value;
				this.SavePreferences();
			}
		}
	}

	// Token: 0x06006053 RID: 24659 RVA: 0x00246654 File Offset: 0x00244A54
	private void InitUI()
	{
		if (this.reviewTermsButton != null)
		{
			this.reviewTermsButton.onClick.AddListener(new UnityAction(this.ReviewTerms));
		}
		if (this.termsAndSettingsAcceptedButton != null)
		{
			this.termsAndSettingsAcceptedButton.onClick.AddListener(new UnityAction(this.TermsAndSettingsAcceptedPressed));
		}
		this.SyncFirstTimeUser();
		if (this.termsOfUseAcceptedToggle != null)
		{
			this.termsOfUseAcceptedToggle.isOn = this._termsOfUseAccepted;
			this.termsOfUseAcceptedToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__0));
		}
		if (this.renderScaleSlider != null)
		{
			this.renderScaleSlider.value = this.renderScale;
			this.renderScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1));
			SliderControl component = this.renderScaleSlider.GetComponent<SliderControl>();
			if (component != null)
			{
				component.defaultValue = 1f;
			}
			this.SyncRenderScale();
		}
		if (this.msaaPopup != null)
		{
			UIPopup uipopup = this.msaaPopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetMsaaFromString));
			this.SyncMsaaPopup();
			this.SyncMsaa();
		}
		if (this.smoothPassesPopup != null)
		{
			UIPopup uipopup2 = this.smoothPassesPopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetSmoothPassesFromString));
			this.SyncSmoothPassesPopup();
		}
		if (this.desktopVsyncToggle != null)
		{
			this.desktopVsyncToggle.isOn = this._desktopVsync;
			this.desktopVsyncToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2));
			this.SyncDesktopVsync();
		}
		if (this.pixelLightCountPopup != null)
		{
			this.pixelLightCountPopup.currentValue = this.pixelLightCount.ToString();
			UIPopup uipopup3 = this.pixelLightCountPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetPixelLightCountFromString));
			this.SyncPixelLightCount();
		}
		if (this.shaderLODPopup != null)
		{
			this.shaderLODPopup.currentValue = this._shaderLOD.ToString();
			UIPopup uipopup4 = this.shaderLODPopup;
			uipopup4.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup4.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetShaderLODFromString));
			this.SetInternalShaderLOD();
		}
		if (this.mirrorReflectionsToggle != null)
		{
			this.mirrorReflectionsToggle.isOn = this.mirrorReflections;
			this.mirrorReflectionsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__3));
			this.SyncMirrorReflections();
		}
		if (this.realtimeReflectionProbesToggle != null)
		{
			this.realtimeReflectionProbesToggle.isOn = this.realtimeReflectionProbes;
			this.realtimeReflectionProbesToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__4));
			this.SyncRealtimeReflectionProbes();
		}
		if (this.mirrorToggle != null)
		{
			this.mirrorToggle.isOn = this.mirrorToDisplay;
			this.mirrorToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__5));
			this.SyncMirrorToDisplay();
		}
		if (this.hideExitButtonToggle != null)
		{
			this.hideExitButtonToggle.isOn = this._hideExitButton;
			this.hideExitButtonToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__6));
		}
		if (this.showTargetsMenuOnlyToggle != null)
		{
			this.showTargetsMenuOnlyToggle.isOn = this._showTargetsMenuOnly;
			this.showTargetsMenuOnlyToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__7));
		}
		if (this.alwaysShowPointersOnTouchToggle != null)
		{
			this.alwaysShowPointersOnTouchToggle.isOn = this._alwaysShowPointersOnTouch;
			this.alwaysShowPointersOnTouchToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__8));
		}
		if (this.hideInactiveTargetsToggle != null)
		{
			this.hideInactiveTargetsToggle.isOn = this._hideInactiveTargets;
			this.hideInactiveTargetsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__9));
		}
		if (this.showControllersMenuOnlyToggle != null)
		{
			this.showControllersMenuOnlyToggle.isOn = this._showControllersMenuOnly;
			this.showControllersMenuOnlyToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__A));
		}
		if (this.targetAlphaSlider != null)
		{
			this.targetAlphaSlider.value = this.targetAlpha;
			FreeControllerV3.targetAlpha = this._targetAlpha;
			this.targetAlphaSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__B));
			SliderControl component2 = this.targetAlphaSlider.GetComponent<SliderControl>();
			if (component2 != null)
			{
				component2.defaultValue = 1f;
			}
		}
		if (this.crosshairAlphaSlider != null)
		{
			this.crosshairAlphaSlider.value = this.crosshairAlpha;
			this.SyncCrosshairAlpha();
			this.crosshairAlphaSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__C));
			SliderControl component3 = this.crosshairAlphaSlider.GetComponent<SliderControl>();
			if (component3 != null)
			{
				component3.defaultValue = 0.1f;
			}
		}
		if (this.useMonitorViewOffsetWhenUIOpenToggle != null)
		{
			this.useMonitorViewOffsetWhenUIOpenToggle.isOn = this._useMonitorViewOffsetWhenUIOpen;
			this.useMonitorViewOffsetWhenUIOpenToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__D));
		}
		if (this.steamVRShowControllersToggle != null)
		{
			this.steamVRShowControllersToggle.isOn = this._steamVRShowControllers;
			this.steamVRShowControllersToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__E));
		}
		if (this.steamVRUseControllerHandPoseToggle != null)
		{
			this.steamVRUseControllerHandPoseToggle.isOn = this._steamVRUseControllerHandPose;
			this.steamVRUseControllerHandPoseToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__F));
		}
		if (this.steamVRPointerAngleSlider != null)
		{
			SliderControl component4 = this.steamVRPointerAngleSlider.GetComponent<SliderControl>();
			if (component4 != null)
			{
				component4.defaultValue = this.defaultSteamVRPointerAngle;
			}
			this.steamVRPointerAngleSlider.value = this._steamVRPointerAngle;
			this.steamVRPointerAngleSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__10));
			this.SyncSteamVRPointerAngle();
		}
		if (this.fingerInputFactorSlider != null)
		{
			SliderControl component5 = this.fingerInputFactorSlider.GetComponent<SliderControl>();
			if (component5 != null)
			{
				component5.defaultValue = this.defaultFingerInputFactor;
			}
			this.fingerInputFactorSlider.value = this._fingerInputFactor;
			this.fingerInputFactorSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__11));
			this.SyncFingerInputFactor();
		}
		if (this.thumbInputFactorSlider != null)
		{
			SliderControl component6 = this.thumbInputFactorSlider.GetComponent<SliderControl>();
			if (component6 != null)
			{
				component6.defaultValue = this.defaultThumbInputFactor;
			}
			this.thumbInputFactorSlider.value = this._thumbInputFactor;
			this.thumbInputFactorSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__12));
			this.SyncThumbInputFactor();
		}
		if (this.fingerSpreadOffsetSlider != null)
		{
			SliderControl component7 = this.fingerSpreadOffsetSlider.GetComponent<SliderControl>();
			if (component7 != null)
			{
				component7.defaultValue = this.defaultFingerSpreadOffset;
			}
			this.fingerSpreadOffsetSlider.value = this._fingerSpreadOffset;
			this.fingerSpreadOffsetSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__13));
			this.SyncFingerSpreadOffset();
		}
		if (this.fingerBendOffsetSlider != null)
		{
			SliderControl component8 = this.fingerBendOffsetSlider.GetComponent<SliderControl>();
			if (component8 != null)
			{
				component8.defaultValue = this.defaultFingerBendOffset;
			}
			this.fingerBendOffsetSlider.value = this._fingerBendOffset;
			this.fingerBendOffsetSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__14));
			this.SyncFingerBendOffset();
		}
		if (this.thumbSpreadOffsetSlider != null)
		{
			SliderControl component9 = this.thumbSpreadOffsetSlider.GetComponent<SliderControl>();
			if (component9 != null)
			{
				component9.defaultValue = this.defaultThumbSpreadOffset;
			}
			this.thumbSpreadOffsetSlider.value = this._thumbSpreadOffset;
			this.thumbSpreadOffsetSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__15));
			this.SyncThumbSpreadOffset();
		}
		if (this.thumbBendOffsetSlider != null)
		{
			SliderControl component10 = this.thumbBendOffsetSlider.GetComponent<SliderControl>();
			if (component10 != null)
			{
				component10.defaultValue = this.defaultThumbBendOffset;
			}
			this.thumbBendOffsetSlider.value = this._thumbBendOffset;
			this.thumbBendOffsetSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__16));
			this.SyncThumbBendOffset();
		}
		if (this.oculusSwapGrabAndTriggerToggle != null)
		{
			this.oculusSwapGrabAndTriggerToggle.isOn = this._oculusSwapGrabAndTrigger;
			this.oculusSwapGrabAndTriggerToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__17));
		}
		if (this.oculusDisableFreeMoveToggle != null)
		{
			this.oculusDisableFreeMoveToggle.isOn = this._oculusDisableFreeMove;
			this.oculusDisableFreeMoveToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__18));
		}
		if (this.pointLightShadowBlurSlider != null)
		{
			this.pointLightShadowBlurSlider.value = this.pointLightShadowBlur;
			this.SyncShadowBlur();
			this.pointLightShadowBlurSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__19));
			SliderControl component11 = this.pointLightShadowBlurSlider.GetComponent<SliderControl>();
			if (component11 != null)
			{
				component11.defaultValue = 0.5f;
			}
		}
		if (this.pointLightShadowBiasBaseSlider != null)
		{
			this.pointLightShadowBiasBaseSlider.value = this.pointLightShadowBiasBase;
			this.SyncShadowBiasBase();
			this.pointLightShadowBiasBaseSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1A));
			SliderControl component12 = this.pointLightShadowBiasBaseSlider.GetComponent<SliderControl>();
			if (component12 != null)
			{
				component12.defaultValue = 0.01f;
			}
		}
		if (this.shadowFilterLevelSlider != null)
		{
			this.shadowFilterLevelSlider.value = this.shadowFilterLevel;
			this.SyncShadowFilterLevel();
			this.shadowFilterLevelSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1B));
			SliderControl component13 = this.shadowFilterLevelSlider.GetComponent<SliderControl>();
			if (component13 != null)
			{
				component13.defaultValue = 3f;
			}
		}
		if (this.closeObjectBlurToggle != null)
		{
			this.closeObjectBlurToggle.isOn = this.closeObjectBlur;
			this.closeObjectBlurToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1C));
			this.SyncCloseObjectBlur();
		}
		if (this.physicsRatePopup != null)
		{
			this.physicsRatePopup.currentValue = this.physicsRate.ToString();
			UIPopup uipopup5 = this.physicsRatePopup;
			uipopup5.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup5.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetPhysicsRateFromString));
		}
		if (this.physicsUpdateCapPopup != null)
		{
			this.physicsUpdateCapPopup.currentValue = this.physicsUpdateCap.ToString();
			UIPopup uipopup6 = this.physicsUpdateCapPopup;
			uipopup6.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup6.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetPhysicsUpdateCapFromString));
		}
		if (this.physicsHighQualityToggle != null)
		{
			this.physicsHighQualityToggle.isOn = this._physicsHighQuality;
			this.physicsHighQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1D));
		}
		this.SyncPhysics();
		if (this.softPhysicsToggle != null)
		{
			this.softPhysicsToggle.isOn = this.softPhysics;
			this.softPhysicsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1E));
			this.SyncSoftPhysics();
		}
		if (this.glowEffectsPopup != null)
		{
			this.glowEffectsPopup.currentValue = this._glowEffects.ToString();
			UIPopup uipopup7 = this.glowEffectsPopup;
			uipopup7.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup7.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetGlowEffectsFromString));
			this.SyncGlow();
		}
		if (this.ultraLowQualityToggle != null)
		{
			this.ultraLowQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1F));
		}
		if (this.lowQualityToggle != null)
		{
			this.lowQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__20));
		}
		if (this.midQualityToggle != null)
		{
			this.midQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__21));
		}
		if (this.highQualityToggle != null)
		{
			this.highQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__22));
		}
		if (this.ultraQualityToggle != null)
		{
			this.ultraQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__23));
		}
		if (this.maxQualityToggle != null)
		{
			this.maxQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__24));
		}
		if (this.customQualityToggle != null)
		{
			this.customQualityToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__25));
		}
		if (this.useHeadColliderToggle != null)
		{
			this.useHeadColliderToggle.isOn = this._useHeadCollider;
			this.useHeadColliderToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__26));
			this.SyncHeadCollider();
		}
		if (this.optimizeMemoryOnSceneLoadToggle != null)
		{
			this.optimizeMemoryOnSceneLoadToggle.isOn = this._optimizeMemoryOnSceneLoad;
			this.optimizeMemoryOnSceneLoadToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__27));
		}
		if (this.optimizeMemoryOnPresetLoadToggle != null)
		{
			this.optimizeMemoryOnPresetLoadToggle.isOn = this._optimizeMemoryOnPresetLoad;
			this.optimizeMemoryOnPresetLoadToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__28));
		}
		if (this.enableCachingToggle != null)
		{
			this.enableCachingToggle.isOn = this._enableCaching;
			this.enableCachingToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__29));
		}
		if (this.browseCacheFolderButton != null)
		{
			this.browseCacheFolderButton.onClick.AddListener(new UnityAction(this.BrowseCacheFolder));
		}
		if (this.resetCacheFolderButton != null)
		{
			this.resetCacheFolderButton.onClick.AddListener(new UnityAction(this.ResetCacheFolder));
		}
		if (this.cacheFolderText != null)
		{
			this.cacheFolderText.text = this._cacheFolder;
		}
		if (this.confirmLoadToggle != null)
		{
			this.confirmLoadToggle.isOn = this._confirmLoad;
			this.confirmLoadToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2A));
		}
		if (this.flipToolbarToggle != null)
		{
			this.flipToolbarToggle.isOn = this._flipToolbar;
			this.flipToolbarToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2B));
		}
		if (this.enableWebBrowserToggle != null)
		{
			this.enableWebBrowserToggle.isOn = this._enableWebBrowser;
			this.enableWebBrowserToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2C));
			this.SyncEnableWebBrowser();
		}
		if (this.enableWebBrowserToggleAlt != null)
		{
			this.enableWebBrowserToggleAlt.isOn = this._enableWebBrowser;
			this.enableWebBrowserToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2D));
		}
		if (this.allowNonWhitelistDomainsToggle != null)
		{
			this.allowNonWhitelistDomainsToggle.isOn = this._allowNonWhitelistDomains;
			this.allowNonWhitelistDomainsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2E));
		}
		if (this.allowNonWhitelistDomainsToggleAlt != null)
		{
			this.allowNonWhitelistDomainsToggleAlt.isOn = this._allowNonWhitelistDomains;
			this.allowNonWhitelistDomainsToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2F));
		}
		if (this.allowNonWhitelistDomainsToggleAlt2 != null)
		{
			this.allowNonWhitelistDomainsToggleAlt2.isOn = this._allowNonWhitelistDomains;
			this.allowNonWhitelistDomainsToggleAlt2.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__30));
		}
		if (this.allowNonWhitelistDomainsToggleAlt3 != null)
		{
			this.allowNonWhitelistDomainsToggleAlt3.isOn = this._allowNonWhitelistDomains;
			this.allowNonWhitelistDomainsToggleAlt3.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__31));
		}
		if (this.enableWebBrowserProfileToggle != null)
		{
			this.enableWebBrowserProfileToggle.isOn = this._enableWebBrowserProfile;
			this.enableWebBrowserProfileToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__32));
		}
		if (this.enableWebBrowserProfileToggleAlt != null)
		{
			this.enableWebBrowserProfileToggleAlt.isOn = this._enableWebBrowserProfile;
			this.enableWebBrowserProfileToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__33));
		}
		if (this.enableWebMiscToggle != null)
		{
			this.enableWebMiscToggle.isOn = this._enableWebMisc;
			this.enableWebMiscToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__34));
		}
		if (this.enableWebMiscToggleAlt != null)
		{
			this.enableWebMiscToggleAlt.isOn = this._enableWebMisc;
			this.enableWebMiscToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__35));
		}
		if (this.enableHubToggle != null)
		{
			this.enableHubToggle.isOn = this._enableHub;
			this.enableHubToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__36));
			this.SyncEnableHub();
		}
		if (this.enableHubToggleAlt != null)
		{
			this.enableHubToggleAlt.isOn = this._enableHub;
			this.enableHubToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__37));
		}
		if (this.enableHubDownloaderToggle != null)
		{
			this.enableHubDownloaderToggle.isOn = this._enableHubDownloader;
			this.enableHubDownloaderToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__38));
			this.SyncEnableHubDownloader();
		}
		if (this.enableHubDownloaderToggleAlt != null)
		{
			this.enableHubDownloaderToggleAlt.isOn = this._enableHubDownloader;
			this.enableHubDownloaderToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__39));
		}
		if (this.enablePluginsToggle != null)
		{
			this.enablePluginsToggle.isOn = this._enablePlugins;
			this.enablePluginsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__3A));
		}
		if (this.enablePluginsToggleAlt != null)
		{
			this.enablePluginsToggleAlt.isOn = this._enablePlugins;
			this.enablePluginsToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__3B));
		}
		if (this.allowPluginsNetworkAccessToggle != null)
		{
			this.allowPluginsNetworkAccessToggle.isOn = this._allowPluginsNetworkAccess;
			this.allowPluginsNetworkAccessToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__3C));
		}
		if (this.allowPluginsNetworkAccessToggleAlt != null)
		{
			this.allowPluginsNetworkAccessToggleAlt.isOn = this._allowPluginsNetworkAccess;
			this.allowPluginsNetworkAccessToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__3D));
		}
		this.SyncAllowPluginsNetworkAccess();
		if (this.alwaysAllowPluginsDownloadedFromHubToggle != null)
		{
			this.alwaysAllowPluginsDownloadedFromHubToggle.isOn = this._alwaysAllowPluginsDownloadedFromHub;
			this.alwaysAllowPluginsDownloadedFromHubToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__3E));
		}
		if (this.alwaysAllowPluginsDownloadedFromHubToggleAlt != null)
		{
			this.alwaysAllowPluginsDownloadedFromHubToggleAlt.isOn = this._alwaysAllowPluginsDownloadedFromHub;
			this.alwaysAllowPluginsDownloadedFromHubToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__3F));
		}
		if (this.hideDisabledWebMessagesToggle != null)
		{
			this.hideDisabledWebMessagesToggle.isOn = this._hideDisabledWebMessages;
			this.hideDisabledWebMessagesToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__40));
		}
		if (this.overlayUIToggle != null)
		{
			this.overlayUIToggle.isOn = this._overlayUI;
			this.overlayUIToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__41));
			this.SyncOverlayUI();
		}
		if (this.creatorNameInputField != null)
		{
			this.creatorNameInputField.text = this._creatorName;
			this.creatorNameInputField.onEndEdit.AddListener(new UnityAction<string>(this.<InitUI>m__42));
		}
		if (this.browseDAZExtraLibraryFolderButton != null)
		{
			this.browseDAZExtraLibraryFolderButton.onClick.AddListener(new UnityAction(this.BrowseDAZExtraLibraryFolder));
		}
		if (this.clearDAZExtraLibraryFolderButton != null)
		{
			this.clearDAZExtraLibraryFolderButton.onClick.AddListener(new UnityAction(this.ClearDAZExtraLibraryFolder));
		}
		if (this.DAZExtraLibraryFolderText != null)
		{
			this.DAZExtraLibraryFolderText.text = this._DAZExtraLibraryFolder;
		}
		if (this.browseDAZDefaultContentFolderButton != null)
		{
			this.browseDAZDefaultContentFolderButton.onClick.AddListener(new UnityAction(this.BrowseDAZDefaultContentFolder));
		}
		if (this.clearDAZDefaultContentFolderButton != null)
		{
			this.clearDAZDefaultContentFolderButton.onClick.AddListener(new UnityAction(this.ClearDAZDefaultContentFolder));
		}
		if (this.DAZDefaultContentFolderText != null)
		{
			this.DAZDefaultContentFolderText.text = this._DAZDefaultContentFolder;
		}
	}

	// Token: 0x06006054 RID: 24660 RVA: 0x00247CC8 File Offset: 0x002460C8
	private void Awake()
	{
		UserPreferences.singleton = this;
		this.dynamicGlow = new List<MKGlow>();
	}

	// Token: 0x06006055 RID: 24661 RVA: 0x00247CDC File Offset: 0x002460DC
	private void Start()
	{
		this._cacheFolder = CacheManager.GetCacheDir();
		CacheManager.CachingEnabled = this._enableCaching;
		if (Application.isPlaying)
		{
			if (this.shouldLoadPrefsFileOnStart)
			{
				this.SyncEnableWebBrowserProfile();
				this.RestorePreferences();
			}
			FileManager.SyncJSONCache();
			this.InitUI();
			this.CheckQualityLevels();
			this.SyncWhitelistDomains();
		}
		else
		{
			this.SyncShadows();
		}
	}

	// Token: 0x06006056 RID: 24662 RVA: 0x00247D42 File Offset: 0x00246142
	private void OnEnable()
	{
		if (!Application.isPlaying)
		{
			this.SyncShadows();
		}
	}

	// Token: 0x06006057 RID: 24663 RVA: 0x00247D54 File Offset: 0x00246154
	[CompilerGenerated]
	private void <InitUI>m__0(bool A_1)
	{
		this.termsOfUseAccepted = this.termsOfUseAcceptedToggle.isOn;
	}

	// Token: 0x06006058 RID: 24664 RVA: 0x00247D67 File Offset: 0x00246167
	[CompilerGenerated]
	private void <InitUI>m__1(float A_1)
	{
		this.renderScale = this.renderScaleSlider.value;
	}

	// Token: 0x06006059 RID: 24665 RVA: 0x00247D7A File Offset: 0x0024617A
	[CompilerGenerated]
	private void <InitUI>m__2(bool A_1)
	{
		this.desktopVsync = this.desktopVsyncToggle.isOn;
	}

	// Token: 0x0600605A RID: 24666 RVA: 0x00247D8D File Offset: 0x0024618D
	[CompilerGenerated]
	private void <InitUI>m__3(bool A_1)
	{
		this.mirrorReflections = this.mirrorReflectionsToggle.isOn;
	}

	// Token: 0x0600605B RID: 24667 RVA: 0x00247DA0 File Offset: 0x002461A0
	[CompilerGenerated]
	private void <InitUI>m__4(bool A_1)
	{
		this.realtimeReflectionProbes = this.realtimeReflectionProbesToggle.isOn;
	}

	// Token: 0x0600605C RID: 24668 RVA: 0x00247DB3 File Offset: 0x002461B3
	[CompilerGenerated]
	private void <InitUI>m__5(bool A_1)
	{
		this.mirrorToDisplay = this.mirrorToggle.isOn;
	}

	// Token: 0x0600605D RID: 24669 RVA: 0x00247DC6 File Offset: 0x002461C6
	[CompilerGenerated]
	private void <InitUI>m__6(bool A_1)
	{
		this.hideExitButton = this.hideExitButtonToggle.isOn;
	}

	// Token: 0x0600605E RID: 24670 RVA: 0x00247DD9 File Offset: 0x002461D9
	[CompilerGenerated]
	private void <InitUI>m__7(bool A_1)
	{
		this.showTargetsMenuOnly = this.showTargetsMenuOnlyToggle.isOn;
	}

	// Token: 0x0600605F RID: 24671 RVA: 0x00247DEC File Offset: 0x002461EC
	[CompilerGenerated]
	private void <InitUI>m__8(bool A_1)
	{
		this.alwaysShowPointersOnTouch = this.alwaysShowPointersOnTouchToggle.isOn;
	}

	// Token: 0x06006060 RID: 24672 RVA: 0x00247DFF File Offset: 0x002461FF
	[CompilerGenerated]
	private void <InitUI>m__9(bool A_1)
	{
		this.hideInactiveTargets = this.hideInactiveTargetsToggle.isOn;
	}

	// Token: 0x06006061 RID: 24673 RVA: 0x00247E12 File Offset: 0x00246212
	[CompilerGenerated]
	private void <InitUI>m__A(bool A_1)
	{
		this.showControllersMenuOnly = this.showControllersMenuOnlyToggle.isOn;
	}

	// Token: 0x06006062 RID: 24674 RVA: 0x00247E25 File Offset: 0x00246225
	[CompilerGenerated]
	private void <InitUI>m__B(float A_1)
	{
		this.targetAlpha = this.targetAlphaSlider.value;
	}

	// Token: 0x06006063 RID: 24675 RVA: 0x00247E38 File Offset: 0x00246238
	[CompilerGenerated]
	private void <InitUI>m__C(float A_1)
	{
		this.crosshairAlpha = this.crosshairAlphaSlider.value;
	}

	// Token: 0x06006064 RID: 24676 RVA: 0x00247E4B File Offset: 0x0024624B
	[CompilerGenerated]
	private void <InitUI>m__D(bool A_1)
	{
		this.useMonitorViewOffsetWhenUIOpen = this.useMonitorViewOffsetWhenUIOpenToggle.isOn;
	}

	// Token: 0x06006065 RID: 24677 RVA: 0x00247E5E File Offset: 0x0024625E
	[CompilerGenerated]
	private void <InitUI>m__E(bool A_1)
	{
		this.steamVRShowControllers = this.steamVRShowControllersToggle.isOn;
	}

	// Token: 0x06006066 RID: 24678 RVA: 0x00247E71 File Offset: 0x00246271
	[CompilerGenerated]
	private void <InitUI>m__F(bool A_1)
	{
		this.steamVRUseControllerHandPose = this.steamVRUseControllerHandPoseToggle.isOn;
	}

	// Token: 0x06006067 RID: 24679 RVA: 0x00247E84 File Offset: 0x00246284
	[CompilerGenerated]
	private void <InitUI>m__10(float A_1)
	{
		this.steamVRPointerAngle = this.steamVRPointerAngleSlider.value;
	}

	// Token: 0x06006068 RID: 24680 RVA: 0x00247E97 File Offset: 0x00246297
	[CompilerGenerated]
	private void <InitUI>m__11(float A_1)
	{
		this.fingerInputFactor = this.fingerInputFactorSlider.value;
	}

	// Token: 0x06006069 RID: 24681 RVA: 0x00247EAA File Offset: 0x002462AA
	[CompilerGenerated]
	private void <InitUI>m__12(float A_1)
	{
		this.thumbInputFactor = this.thumbInputFactorSlider.value;
	}

	// Token: 0x0600606A RID: 24682 RVA: 0x00247EBD File Offset: 0x002462BD
	[CompilerGenerated]
	private void <InitUI>m__13(float A_1)
	{
		this.fingerSpreadOffset = this.fingerSpreadOffsetSlider.value;
	}

	// Token: 0x0600606B RID: 24683 RVA: 0x00247ED0 File Offset: 0x002462D0
	[CompilerGenerated]
	private void <InitUI>m__14(float A_1)
	{
		this.fingerBendOffset = this.fingerBendOffsetSlider.value;
	}

	// Token: 0x0600606C RID: 24684 RVA: 0x00247EE3 File Offset: 0x002462E3
	[CompilerGenerated]
	private void <InitUI>m__15(float A_1)
	{
		this.thumbSpreadOffset = this.thumbSpreadOffsetSlider.value;
	}

	// Token: 0x0600606D RID: 24685 RVA: 0x00247EF6 File Offset: 0x002462F6
	[CompilerGenerated]
	private void <InitUI>m__16(float A_1)
	{
		this.thumbBendOffset = this.thumbBendOffsetSlider.value;
	}

	// Token: 0x0600606E RID: 24686 RVA: 0x00247F09 File Offset: 0x00246309
	[CompilerGenerated]
	private void <InitUI>m__17(bool A_1)
	{
		this.oculusSwapGrabAndTrigger = this.oculusSwapGrabAndTriggerToggle.isOn;
	}

	// Token: 0x0600606F RID: 24687 RVA: 0x00247F1C File Offset: 0x0024631C
	[CompilerGenerated]
	private void <InitUI>m__18(bool A_1)
	{
		this.oculusDisableFreeMove = this.oculusDisableFreeMoveToggle.isOn;
	}

	// Token: 0x06006070 RID: 24688 RVA: 0x00247F2F File Offset: 0x0024632F
	[CompilerGenerated]
	private void <InitUI>m__19(float A_1)
	{
		this.pointLightShadowBlur = this.pointLightShadowBlurSlider.value;
	}

	// Token: 0x06006071 RID: 24689 RVA: 0x00247F42 File Offset: 0x00246342
	[CompilerGenerated]
	private void <InitUI>m__1A(float A_1)
	{
		this.pointLightShadowBiasBase = this.pointLightShadowBiasBaseSlider.value;
	}

	// Token: 0x06006072 RID: 24690 RVA: 0x00247F55 File Offset: 0x00246355
	[CompilerGenerated]
	private void <InitUI>m__1B(float A_1)
	{
		this.shadowFilterLevel = this.shadowFilterLevelSlider.value;
	}

	// Token: 0x06006073 RID: 24691 RVA: 0x00247F68 File Offset: 0x00246368
	[CompilerGenerated]
	private void <InitUI>m__1C(bool A_1)
	{
		this.closeObjectBlur = this.closeObjectBlurToggle.isOn;
	}

	// Token: 0x06006074 RID: 24692 RVA: 0x00247F7B File Offset: 0x0024637B
	[CompilerGenerated]
	private void <InitUI>m__1D(bool A_1)
	{
		this.physicsHighQuality = this.physicsHighQualityToggle.isOn;
	}

	// Token: 0x06006075 RID: 24693 RVA: 0x00247F8E File Offset: 0x0024638E
	[CompilerGenerated]
	private void <InitUI>m__1E(bool A_1)
	{
		this.softPhysics = this.softPhysicsToggle.isOn;
	}

	// Token: 0x06006076 RID: 24694 RVA: 0x00247FA1 File Offset: 0x002463A1
	[CompilerGenerated]
	private void <InitUI>m__1F(bool A_1)
	{
		if (!this._disableToggles && this.ultraLowQualityToggle.isOn)
		{
			this.SetQuality("UltraLow");
		}
	}

	// Token: 0x06006077 RID: 24695 RVA: 0x00247FC9 File Offset: 0x002463C9
	[CompilerGenerated]
	private void <InitUI>m__20(bool A_1)
	{
		if (!this._disableToggles && this.lowQualityToggle.isOn)
		{
			this.SetQuality("Low");
		}
	}

	// Token: 0x06006078 RID: 24696 RVA: 0x00247FF1 File Offset: 0x002463F1
	[CompilerGenerated]
	private void <InitUI>m__21(bool A_1)
	{
		if (!this._disableToggles && this.midQualityToggle.isOn)
		{
			this.SetQuality("Mid");
		}
	}

	// Token: 0x06006079 RID: 24697 RVA: 0x00248019 File Offset: 0x00246419
	[CompilerGenerated]
	private void <InitUI>m__22(bool A_1)
	{
		if (!this._disableToggles && this.highQualityToggle.isOn)
		{
			this.SetQuality("High");
		}
	}

	// Token: 0x0600607A RID: 24698 RVA: 0x00248041 File Offset: 0x00246441
	[CompilerGenerated]
	private void <InitUI>m__23(bool A_1)
	{
		if (!this._disableToggles && this.ultraQualityToggle.isOn)
		{
			this.SetQuality("Ultra");
		}
	}

	// Token: 0x0600607B RID: 24699 RVA: 0x00248069 File Offset: 0x00246469
	[CompilerGenerated]
	private void <InitUI>m__24(bool A_1)
	{
		if (!this._disableToggles && this.maxQualityToggle.isOn)
		{
			this.SetQuality("Max");
		}
	}

	// Token: 0x0600607C RID: 24700 RVA: 0x00248091 File Offset: 0x00246491
	[CompilerGenerated]
	private void <InitUI>m__25(bool A_1)
	{
		if (!this._disableToggles && this.customQualityToggle.isOn)
		{
			this.CheckQualityLevels();
		}
	}

	// Token: 0x0600607D RID: 24701 RVA: 0x002480B4 File Offset: 0x002464B4
	[CompilerGenerated]
	private void <InitUI>m__26(bool A_1)
	{
		this.useHeadCollider = this.useHeadColliderToggle.isOn;
	}

	// Token: 0x0600607E RID: 24702 RVA: 0x002480C7 File Offset: 0x002464C7
	[CompilerGenerated]
	private void <InitUI>m__27(bool A_1)
	{
		this.optimizeMemoryOnSceneLoad = this.optimizeMemoryOnSceneLoadToggle.isOn;
	}

	// Token: 0x0600607F RID: 24703 RVA: 0x002480DA File Offset: 0x002464DA
	[CompilerGenerated]
	private void <InitUI>m__28(bool A_1)
	{
		this.optimizeMemoryOnPresetLoad = this.optimizeMemoryOnPresetLoadToggle.isOn;
	}

	// Token: 0x06006080 RID: 24704 RVA: 0x002480ED File Offset: 0x002464ED
	[CompilerGenerated]
	private void <InitUI>m__29(bool A_1)
	{
		this.enableCaching = this.enableCachingToggle.isOn;
	}

	// Token: 0x06006081 RID: 24705 RVA: 0x00248100 File Offset: 0x00246500
	[CompilerGenerated]
	private void <InitUI>m__2A(bool A_1)
	{
		this.confirmLoad = this.confirmLoadToggle.isOn;
	}

	// Token: 0x06006082 RID: 24706 RVA: 0x00248113 File Offset: 0x00246513
	[CompilerGenerated]
	private void <InitUI>m__2B(bool A_1)
	{
		this.flipToolbar = this.flipToolbarToggle.isOn;
	}

	// Token: 0x06006083 RID: 24707 RVA: 0x00248126 File Offset: 0x00246526
	[CompilerGenerated]
	private void <InitUI>m__2C(bool A_1)
	{
		this.enableWebBrowser = this.enableWebBrowserToggle.isOn;
	}

	// Token: 0x06006084 RID: 24708 RVA: 0x00248139 File Offset: 0x00246539
	[CompilerGenerated]
	private void <InitUI>m__2D(bool A_1)
	{
		this.enableWebBrowser = this.enableWebBrowserToggleAlt.isOn;
	}

	// Token: 0x06006085 RID: 24709 RVA: 0x0024814C File Offset: 0x0024654C
	[CompilerGenerated]
	private void <InitUI>m__2E(bool A_1)
	{
		this.allowNonWhitelistDomains = this.allowNonWhitelistDomainsToggle.isOn;
	}

	// Token: 0x06006086 RID: 24710 RVA: 0x0024815F File Offset: 0x0024655F
	[CompilerGenerated]
	private void <InitUI>m__2F(bool A_1)
	{
		this.allowNonWhitelistDomains = this.allowNonWhitelistDomainsToggleAlt.isOn;
	}

	// Token: 0x06006087 RID: 24711 RVA: 0x00248172 File Offset: 0x00246572
	[CompilerGenerated]
	private void <InitUI>m__30(bool A_1)
	{
		this.allowNonWhitelistDomains = this.allowNonWhitelistDomainsToggleAlt2.isOn;
	}

	// Token: 0x06006088 RID: 24712 RVA: 0x00248185 File Offset: 0x00246585
	[CompilerGenerated]
	private void <InitUI>m__31(bool A_1)
	{
		this.allowNonWhitelistDomains = this.allowNonWhitelistDomainsToggleAlt3.isOn;
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x00248198 File Offset: 0x00246598
	[CompilerGenerated]
	private void <InitUI>m__32(bool A_1)
	{
		this.enableWebBrowserProfile = this.enableWebBrowserProfileToggle.isOn;
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x002481AB File Offset: 0x002465AB
	[CompilerGenerated]
	private void <InitUI>m__33(bool A_1)
	{
		this.enableWebBrowserProfile = this.enableWebBrowserProfileToggleAlt.isOn;
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x002481BE File Offset: 0x002465BE
	[CompilerGenerated]
	private void <InitUI>m__34(bool A_1)
	{
		this.enableWebMisc = this.enableWebMiscToggle.isOn;
	}

	// Token: 0x0600608C RID: 24716 RVA: 0x002481D1 File Offset: 0x002465D1
	[CompilerGenerated]
	private void <InitUI>m__35(bool A_1)
	{
		this.enableWebMisc = this.enableWebMiscToggleAlt.isOn;
	}

	// Token: 0x0600608D RID: 24717 RVA: 0x002481E4 File Offset: 0x002465E4
	[CompilerGenerated]
	private void <InitUI>m__36(bool A_1)
	{
		this.enableHub = this.enableHubToggle.isOn;
	}

	// Token: 0x0600608E RID: 24718 RVA: 0x002481F7 File Offset: 0x002465F7
	[CompilerGenerated]
	private void <InitUI>m__37(bool A_1)
	{
		this.enableHub = this.enableHubToggleAlt.isOn;
	}

	// Token: 0x0600608F RID: 24719 RVA: 0x0024820A File Offset: 0x0024660A
	[CompilerGenerated]
	private void <InitUI>m__38(bool A_1)
	{
		this.enableHubDownloader = this.enableHubDownloaderToggle.isOn;
	}

	// Token: 0x06006090 RID: 24720 RVA: 0x0024821D File Offset: 0x0024661D
	[CompilerGenerated]
	private void <InitUI>m__39(bool A_1)
	{
		this.enableHubDownloader = this.enableHubDownloaderToggleAlt.isOn;
	}

	// Token: 0x06006091 RID: 24721 RVA: 0x00248230 File Offset: 0x00246630
	[CompilerGenerated]
	private void <InitUI>m__3A(bool A_1)
	{
		this.enablePlugins = this.enablePluginsToggle.isOn;
	}

	// Token: 0x06006092 RID: 24722 RVA: 0x00248243 File Offset: 0x00246643
	[CompilerGenerated]
	private void <InitUI>m__3B(bool A_1)
	{
		this.enablePlugins = this.enablePluginsToggleAlt.isOn;
	}

	// Token: 0x06006093 RID: 24723 RVA: 0x00248256 File Offset: 0x00246656
	[CompilerGenerated]
	private void <InitUI>m__3C(bool A_1)
	{
		this.allowPluginsNetworkAccess = this.allowPluginsNetworkAccessToggle.isOn;
	}

	// Token: 0x06006094 RID: 24724 RVA: 0x00248269 File Offset: 0x00246669
	[CompilerGenerated]
	private void <InitUI>m__3D(bool A_1)
	{
		this.allowPluginsNetworkAccess = this.allowPluginsNetworkAccessToggleAlt.isOn;
	}

	// Token: 0x06006095 RID: 24725 RVA: 0x0024827C File Offset: 0x0024667C
	[CompilerGenerated]
	private void <InitUI>m__3E(bool A_1)
	{
		this.alwaysAllowPluginsDownloadedFromHub = this.alwaysAllowPluginsDownloadedFromHubToggle.isOn;
	}

	// Token: 0x06006096 RID: 24726 RVA: 0x0024828F File Offset: 0x0024668F
	[CompilerGenerated]
	private void <InitUI>m__3F(bool A_1)
	{
		this.alwaysAllowPluginsDownloadedFromHub = this.alwaysAllowPluginsDownloadedFromHubToggleAlt.isOn;
	}

	// Token: 0x06006097 RID: 24727 RVA: 0x002482A2 File Offset: 0x002466A2
	[CompilerGenerated]
	private void <InitUI>m__40(bool A_1)
	{
		this.hideDisabledWebMessages = this.hideDisabledWebMessagesToggle.isOn;
	}

	// Token: 0x06006098 RID: 24728 RVA: 0x002482B5 File Offset: 0x002466B5
	[CompilerGenerated]
	private void <InitUI>m__41(bool A_1)
	{
		this.overlayUI = this.overlayUIToggle.isOn;
	}

	// Token: 0x06006099 RID: 24729 RVA: 0x002482C8 File Offset: 0x002466C8
	[CompilerGenerated]
	private void <InitUI>m__42(string s)
	{
		this.creatorName = s;
	}

	// Token: 0x04004F42 RID: 20290
	public static UserPreferences singleton;

	// Token: 0x04004F43 RID: 20291
	private bool _disableSave;

	// Token: 0x04004F44 RID: 20292
	private bool _disableToggles;

	// Token: 0x04004F45 RID: 20293
	public Toggle ultraLowQualityToggle;

	// Token: 0x04004F46 RID: 20294
	public Toggle lowQualityToggle;

	// Token: 0x04004F47 RID: 20295
	public Toggle midQualityToggle;

	// Token: 0x04004F48 RID: 20296
	public Toggle highQualityToggle;

	// Token: 0x04004F49 RID: 20297
	public Toggle ultraQualityToggle;

	// Token: 0x04004F4A RID: 20298
	public Toggle maxQualityToggle;

	// Token: 0x04004F4B RID: 20299
	public Toggle customQualityToggle;

	// Token: 0x04004F4C RID: 20300
	public bool loadPrefsFileOnStart = true;

	// Token: 0x04004F4D RID: 20301
	public LeapHandModelControl leapHandModelControl;

	// Token: 0x04004F4E RID: 20302
	public HandModelControl motionHandModelControl;

	// Token: 0x04004F4F RID: 20303
	public HandModelControl alternateMotionHandModelControl;

	// Token: 0x04004F50 RID: 20304
	public Slider renderScaleSlider;

	// Token: 0x04004F51 RID: 20305
	[SerializeField]
	private float _renderScale = 1f;

	// Token: 0x04004F52 RID: 20306
	public UIPopup msaaPopup;

	// Token: 0x04004F53 RID: 20307
	[SerializeField]
	private int _msaaLevel = 8;

	// Token: 0x04004F54 RID: 20308
	public GameObject[] firstTimeUserEnableGameObjects;

	// Token: 0x04004F55 RID: 20309
	public GameObject[] firstTimeUserDisableGameObjects;

	// Token: 0x04004F56 RID: 20310
	[SerializeField]
	private bool _firstTimeUser = true;

	// Token: 0x04004F57 RID: 20311
	[SerializeField]
	private string termsOfUsePath;

	// Token: 0x04004F58 RID: 20312
	public Button reviewTermsButton;

	// Token: 0x04004F59 RID: 20313
	public GameObject termsNotAcceptedGameObject;

	// Token: 0x04004F5A RID: 20314
	public Button termsAndSettingsAcceptedButton;

	// Token: 0x04004F5B RID: 20315
	public Toggle termsOfUseAcceptedToggle;

	// Token: 0x04004F5C RID: 20316
	[SerializeField]
	private bool _termsOfUseAccepted;

	// Token: 0x04004F5D RID: 20317
	public Toggle desktopVsyncToggle;

	// Token: 0x04004F5E RID: 20318
	[SerializeField]
	private bool _desktopVsync;

	// Token: 0x04004F5F RID: 20319
	public UIPopup smoothPassesPopup;

	// Token: 0x04004F60 RID: 20320
	[SerializeField]
	private int _smoothPasses = 3;

	// Token: 0x04004F61 RID: 20321
	public UIPopup pixelLightCountPopup;

	// Token: 0x04004F62 RID: 20322
	[SerializeField]
	private int _pixelLightCount = 4;

	// Token: 0x04004F63 RID: 20323
	public UIPopup shaderLODPopup;

	// Token: 0x04004F64 RID: 20324
	[SerializeField]
	private UserPreferences.ShaderLOD _shaderLOD = UserPreferences.ShaderLOD.High;

	// Token: 0x04004F65 RID: 20325
	public Camera normalCamera;

	// Token: 0x04004F66 RID: 20326
	public Camera mirrorReflectionCamera1;

	// Token: 0x04004F67 RID: 20327
	public Camera mirrorReflectionCamera2;

	// Token: 0x04004F68 RID: 20328
	public Toggle mirrorReflectionsToggle;

	// Token: 0x04004F69 RID: 20329
	[SerializeField]
	private bool _mirrorReflections = true;

	// Token: 0x04004F6A RID: 20330
	public Toggle realtimeReflectionProbesToggle;

	// Token: 0x04004F6B RID: 20331
	[SerializeField]
	private bool _realtimeReflectionProbes = true;

	// Token: 0x04004F6C RID: 20332
	public Toggle mirrorToggle;

	// Token: 0x04004F6D RID: 20333
	[SerializeField]
	private bool _mirrorToDisplay;

	// Token: 0x04004F6E RID: 20334
	public GameObject exitButton;

	// Token: 0x04004F6F RID: 20335
	public Toggle hideExitButtonToggle;

	// Token: 0x04004F70 RID: 20336
	[SerializeField]
	private bool _hideExitButton;

	// Token: 0x04004F71 RID: 20337
	public Toggle showTargetsMenuOnlyToggle;

	// Token: 0x04004F72 RID: 20338
	[SerializeField]
	private bool _showTargetsMenuOnly = true;

	// Token: 0x04004F73 RID: 20339
	public Toggle alwaysShowPointersOnTouchToggle;

	// Token: 0x04004F74 RID: 20340
	[SerializeField]
	private bool _alwaysShowPointersOnTouch = true;

	// Token: 0x04004F75 RID: 20341
	public RectTransform enableWhenHideInactiveTargets;

	// Token: 0x04004F76 RID: 20342
	public RectTransform enableWhenShowInactiveTargets;

	// Token: 0x04004F77 RID: 20343
	public Toggle hideInactiveTargetsToggle;

	// Token: 0x04004F78 RID: 20344
	[SerializeField]
	private bool _hideInactiveTargets = true;

	// Token: 0x04004F79 RID: 20345
	public Toggle showControllersMenuOnlyToggle;

	// Token: 0x04004F7A RID: 20346
	[SerializeField]
	private bool _showControllersMenuOnly;

	// Token: 0x04004F7B RID: 20347
	public Slider targetAlphaSlider;

	// Token: 0x04004F7C RID: 20348
	[SerializeField]
	private float _targetAlpha = 1f;

	// Token: 0x04004F7D RID: 20349
	public Image crosshair;

	// Token: 0x04004F7E RID: 20350
	public Slider crosshairAlphaSlider;

	// Token: 0x04004F7F RID: 20351
	[SerializeField]
	private float _crosshairAlpha = 0.1f;

	// Token: 0x04004F80 RID: 20352
	public Toggle useMonitorViewOffsetWhenUIOpenToggle;

	// Token: 0x04004F81 RID: 20353
	[SerializeField]
	private bool _useMonitorViewOffsetWhenUIOpen = true;

	// Token: 0x04004F82 RID: 20354
	public SteamVR_RenderModel steamVRLeftControllerModel;

	// Token: 0x04004F83 RID: 20355
	public SteamVR_RenderModel steamVRRightControllerModel;

	// Token: 0x04004F84 RID: 20356
	public Toggle steamVRShowControllersToggle;

	// Token: 0x04004F85 RID: 20357
	[SerializeField]
	private bool _steamVRShowControllers;

	// Token: 0x04004F86 RID: 20358
	public SteamVR_Behaviour_Skeleton steamVRRightHandSkeleton;

	// Token: 0x04004F87 RID: 20359
	public SteamVR_Behaviour_Skeleton steamVRLeftHandSkeleton;

	// Token: 0x04004F88 RID: 20360
	public Toggle steamVRUseControllerHandPoseToggle;

	// Token: 0x04004F89 RID: 20361
	[SerializeField]
	private bool _steamVRUseControllerHandPose;

	// Token: 0x04004F8A RID: 20362
	public Transform steamVRLeftHandPointer;

	// Token: 0x04004F8B RID: 20363
	public Transform steamVRRightHandPointer;

	// Token: 0x04004F8C RID: 20364
	public Slider steamVRPointerAngleSlider;

	// Token: 0x04004F8D RID: 20365
	public float defaultSteamVRPointerAngle;

	// Token: 0x04004F8E RID: 20366
	[SerializeField]
	private float _steamVRPointerAngle;

	// Token: 0x04004F8F RID: 20367
	public HandInput steamVRLeftHandInput;

	// Token: 0x04004F90 RID: 20368
	public HandInput steamVRRightHandInput;

	// Token: 0x04004F91 RID: 20369
	public OVRHandInput ovrLeftHandInput;

	// Token: 0x04004F92 RID: 20370
	public OVRHandInput ovrRightHandInput;

	// Token: 0x04004F93 RID: 20371
	public Slider fingerInputFactorSlider;

	// Token: 0x04004F94 RID: 20372
	public float defaultFingerInputFactor = 0.25f;

	// Token: 0x04004F95 RID: 20373
	[SerializeField]
	private float _fingerInputFactor = 1f;

	// Token: 0x04004F96 RID: 20374
	public Slider thumbInputFactorSlider;

	// Token: 0x04004F97 RID: 20375
	public float defaultThumbInputFactor = 0.25f;

	// Token: 0x04004F98 RID: 20376
	[SerializeField]
	private float _thumbInputFactor = 1f;

	// Token: 0x04004F99 RID: 20377
	public Slider fingerSpreadOffsetSlider;

	// Token: 0x04004F9A RID: 20378
	public float defaultFingerSpreadOffset = -5f;

	// Token: 0x04004F9B RID: 20379
	[SerializeField]
	private float _fingerSpreadOffset = 1f;

	// Token: 0x04004F9C RID: 20380
	public Slider fingerBendOffsetSlider;

	// Token: 0x04004F9D RID: 20381
	public float defaultFingerBendOffset = 10f;

	// Token: 0x04004F9E RID: 20382
	[SerializeField]
	private float _fingerBendOffset = 1f;

	// Token: 0x04004F9F RID: 20383
	public Slider thumbSpreadOffsetSlider;

	// Token: 0x04004FA0 RID: 20384
	public float defaultThumbSpreadOffset;

	// Token: 0x04004FA1 RID: 20385
	[SerializeField]
	private float _thumbSpreadOffset = 1f;

	// Token: 0x04004FA2 RID: 20386
	public Slider thumbBendOffsetSlider;

	// Token: 0x04004FA3 RID: 20387
	public float defaultThumbBendOffset = 10f;

	// Token: 0x04004FA4 RID: 20388
	[SerializeField]
	private float _thumbBendOffset = 1f;

	// Token: 0x04004FA5 RID: 20389
	public Toggle oculusSwapGrabAndTriggerToggle;

	// Token: 0x04004FA6 RID: 20390
	[SerializeField]
	private bool _oculusSwapGrabAndTrigger;

	// Token: 0x04004FA7 RID: 20391
	public Toggle oculusDisableFreeMoveToggle;

	// Token: 0x04004FA8 RID: 20392
	[SerializeField]
	private bool _oculusDisableFreeMove;

	// Token: 0x04004FA9 RID: 20393
	public Slider pointLightShadowBlurSlider;

	// Token: 0x04004FAA RID: 20394
	[SerializeField]
	private float _pointLightShadowBlur = 0.5f;

	// Token: 0x04004FAB RID: 20395
	public Slider pointLightShadowBiasBaseSlider;

	// Token: 0x04004FAC RID: 20396
	[SerializeField]
	private float _pointLightShadowBiasBase = 0.01f;

	// Token: 0x04004FAD RID: 20397
	public Slider shadowFilterLevelSlider;

	// Token: 0x04004FAE RID: 20398
	[SerializeField]
	private float _shadowFilterLevel = 3f;

	// Token: 0x04004FAF RID: 20399
	public Toggle closeObjectBlurToggle;

	// Token: 0x04004FB0 RID: 20400
	[SerializeField]
	private bool _closeObjectBlur = true;

	// Token: 0x04004FB1 RID: 20401
	public Toggle softPhysicsToggle;

	// Token: 0x04004FB2 RID: 20402
	[SerializeField]
	private bool _softPhysics = true;

	// Token: 0x04004FB3 RID: 20403
	protected int glowObjectCount;

	// Token: 0x04004FB4 RID: 20404
	protected List<MKGlow> dynamicGlow;

	// Token: 0x04004FB5 RID: 20405
	public MKGlow[] glowObjects;

	// Token: 0x04004FB6 RID: 20406
	private bool _pauseGlow;

	// Token: 0x04004FB7 RID: 20407
	public UIPopup glowEffectsPopup;

	// Token: 0x04004FB8 RID: 20408
	[SerializeField]
	private UserPreferences.GlowEffectsLevel _glowEffects = UserPreferences.GlowEffectsLevel.Low;

	// Token: 0x04004FB9 RID: 20409
	public Text autoPhysicsRateText;

	// Token: 0x04004FBA RID: 20410
	public UIPopup physicsRatePopup;

	// Token: 0x04004FBB RID: 20411
	[SerializeField]
	private UserPreferences.PhysicsRate _physicsRate;

	// Token: 0x04004FBC RID: 20412
	public UIPopup physicsUpdateCapPopup;

	// Token: 0x04004FBD RID: 20413
	[SerializeField]
	private int _physicsUpdateCap = 2;

	// Token: 0x04004FBE RID: 20414
	public Toggle physicsHighQualityToggle;

	// Token: 0x04004FBF RID: 20415
	[SerializeField]
	private bool _physicsHighQuality;

	// Token: 0x04004FC0 RID: 20416
	public Transform headCollider;

	// Token: 0x04004FC1 RID: 20417
	public Toggle useHeadColliderToggle;

	// Token: 0x04004FC2 RID: 20418
	[SerializeField]
	private bool _useHeadCollider;

	// Token: 0x04004FC3 RID: 20419
	public Toggle optimizeMemoryOnSceneLoadToggle;

	// Token: 0x04004FC4 RID: 20420
	[SerializeField]
	private bool _optimizeMemoryOnSceneLoad = true;

	// Token: 0x04004FC5 RID: 20421
	public Toggle optimizeMemoryOnPresetLoadToggle;

	// Token: 0x04004FC6 RID: 20422
	[SerializeField]
	private bool _optimizeMemoryOnPresetLoad;

	// Token: 0x04004FC7 RID: 20423
	public Toggle enableCachingToggle;

	// Token: 0x04004FC8 RID: 20424
	[SerializeField]
	private bool _enableCaching = true;

	// Token: 0x04004FC9 RID: 20425
	private string _cacheFolder;

	// Token: 0x04004FCA RID: 20426
	public Button browseCacheFolderButton;

	// Token: 0x04004FCB RID: 20427
	public Button resetCacheFolderButton;

	// Token: 0x04004FCC RID: 20428
	public Text cacheFolderText;

	// Token: 0x04004FCD RID: 20429
	public Toggle confirmLoadToggle;

	// Token: 0x04004FCE RID: 20430
	[SerializeField]
	private bool _confirmLoad;

	// Token: 0x04004FCF RID: 20431
	public ChildOrderFlip toolbarFlipper;

	// Token: 0x04004FD0 RID: 20432
	public Toggle flipToolbarToggle;

	// Token: 0x04004FD1 RID: 20433
	[SerializeField]
	private bool _flipToolbar;

	// Token: 0x04004FD2 RID: 20434
	public Image panelForUIMaterial;

	// Token: 0x04004FD3 RID: 20435
	public Shader overlayUIShader;

	// Token: 0x04004FD4 RID: 20436
	public Shader defaultUIShader;

	// Token: 0x04004FD5 RID: 20437
	public Toggle overlayUIToggle;

	// Token: 0x04004FD6 RID: 20438
	[SerializeField]
	private bool _overlayUI = true;

	// Token: 0x04004FD7 RID: 20439
	public Toggle enableWebBrowserToggle;

	// Token: 0x04004FD8 RID: 20440
	public Toggle enableWebBrowserToggleAlt;

	// Token: 0x04004FD9 RID: 20441
	[SerializeField]
	private bool _enableWebBrowser;

	// Token: 0x04004FDA RID: 20442
	protected HashSet<string> whitelistDomains;

	// Token: 0x04004FDB RID: 20443
	protected string[] whitelistDomainPaths = new string[]
	{
		"whitelist_domains.json",
		"whitelist_domains_user.json"
	};

	// Token: 0x04004FDC RID: 20444
	public Toggle allowNonWhitelistDomainsToggle;

	// Token: 0x04004FDD RID: 20445
	public Toggle allowNonWhitelistDomainsToggleAlt;

	// Token: 0x04004FDE RID: 20446
	public Toggle allowNonWhitelistDomainsToggleAlt2;

	// Token: 0x04004FDF RID: 20447
	public Toggle allowNonWhitelistDomainsToggleAlt3;

	// Token: 0x04004FE0 RID: 20448
	[SerializeField]
	private bool _allowNonWhitelistDomains;

	// Token: 0x04004FE1 RID: 20449
	public Toggle enableWebBrowserProfileToggle;

	// Token: 0x04004FE2 RID: 20450
	public Toggle enableWebBrowserProfileToggleAlt;

	// Token: 0x04004FE3 RID: 20451
	[SerializeField]
	private bool _enableWebBrowserProfile = true;

	// Token: 0x04004FE4 RID: 20452
	public Toggle enableWebMiscToggle;

	// Token: 0x04004FE5 RID: 20453
	public Toggle enableWebMiscToggleAlt;

	// Token: 0x04004FE6 RID: 20454
	[SerializeField]
	private bool _enableWebMisc;

	// Token: 0x04004FE7 RID: 20455
	public Toggle enableHubToggle;

	// Token: 0x04004FE8 RID: 20456
	public Toggle enableHubToggleAlt;

	// Token: 0x04004FE9 RID: 20457
	[SerializeField]
	private bool _enableHub;

	// Token: 0x04004FEA RID: 20458
	public Toggle enableHubDownloaderToggle;

	// Token: 0x04004FEB RID: 20459
	public Toggle enableHubDownloaderToggleAlt;

	// Token: 0x04004FEC RID: 20460
	[SerializeField]
	private bool _enableHubDownloader;

	// Token: 0x04004FED RID: 20461
	public Toggle enablePluginsToggle;

	// Token: 0x04004FEE RID: 20462
	public Toggle enablePluginsToggleAlt;

	// Token: 0x04004FEF RID: 20463
	[SerializeField]
	private bool _enablePlugins;

	// Token: 0x04004FF0 RID: 20464
	[SerializeField]
	private Toggle allowPluginsNetworkAccessToggle;

	// Token: 0x04004FF1 RID: 20465
	[SerializeField]
	private Toggle allowPluginsNetworkAccessToggleAlt;

	// Token: 0x04004FF2 RID: 20466
	[SerializeField]
	private bool _allowPluginsNetworkAccess;

	// Token: 0x04004FF3 RID: 20467
	public Toggle alwaysAllowPluginsDownloadedFromHubToggle;

	// Token: 0x04004FF4 RID: 20468
	public Toggle alwaysAllowPluginsDownloadedFromHubToggleAlt;

	// Token: 0x04004FF5 RID: 20469
	[SerializeField]
	private bool _alwaysAllowPluginsDownloadedFromHub;

	// Token: 0x04004FF6 RID: 20470
	public Toggle hideDisabledWebMessagesToggle;

	// Token: 0x04004FF7 RID: 20471
	[SerializeField]
	private bool _hideDisabledWebMessages;

	// Token: 0x04004FF8 RID: 20472
	[SerializeField]
	private string _creatorName = "Anonymous";

	// Token: 0x04004FF9 RID: 20473
	public InputField creatorNameInputField;

	// Token: 0x04004FFA RID: 20474
	private string _DAZExtraLibraryFolder = string.Empty;

	// Token: 0x04004FFB RID: 20475
	public Button browseDAZExtraLibraryFolderButton;

	// Token: 0x04004FFC RID: 20476
	public Button clearDAZExtraLibraryFolderButton;

	// Token: 0x04004FFD RID: 20477
	public Text DAZExtraLibraryFolderText;

	// Token: 0x04004FFE RID: 20478
	private string _DAZDefaultContentFolder = string.Empty;

	// Token: 0x04004FFF RID: 20479
	public Button browseDAZDefaultContentFolderButton;

	// Token: 0x04005000 RID: 20480
	public Button clearDAZDefaultContentFolderButton;

	// Token: 0x04005001 RID: 20481
	public Text DAZDefaultContentFolderText;

	// Token: 0x04005002 RID: 20482
	protected UserPreferences.SortBy _fileBrowserSortBy = UserPreferences.SortBy.NewToOld;

	// Token: 0x04005003 RID: 20483
	protected UserPreferences.DirectoryOption _fileBrowserDirectoryOption;

	// Token: 0x02000C7D RID: 3197
	private class QualityLevel
	{
		// Token: 0x0600609A RID: 24730 RVA: 0x002482D1 File Offset: 0x002466D1
		public QualityLevel()
		{
		}

		// Token: 0x04005004 RID: 20484
		public float renderScale;

		// Token: 0x04005005 RID: 20485
		public int msaaLevel;

		// Token: 0x04005006 RID: 20486
		public int pixelLightCount;

		// Token: 0x04005007 RID: 20487
		public UserPreferences.ShaderLOD shaderLOD;

		// Token: 0x04005008 RID: 20488
		public int smoothPasses;

		// Token: 0x04005009 RID: 20489
		public bool mirrorReflections;

		// Token: 0x0400500A RID: 20490
		public bool realtimeReflectionProbes;

		// Token: 0x0400500B RID: 20491
		public bool closeObjectBlur;

		// Token: 0x0400500C RID: 20492
		public bool softPhysics;

		// Token: 0x0400500D RID: 20493
		public UserPreferences.GlowEffectsLevel glowEffects;
	}

	// Token: 0x02000C7E RID: 3198
	private class QualityLevels
	{
		// Token: 0x0600609B RID: 24731 RVA: 0x002482D9 File Offset: 0x002466D9
		public QualityLevels()
		{
		}

		// Token: 0x0600609C RID: 24732 RVA: 0x002482E4 File Offset: 0x002466E4
		// Note: this type is marked as 'beforefieldinit'.
		static QualityLevels()
		{
		}

		// Token: 0x0400500E RID: 20494
		public static Dictionary<string, UserPreferences.QualityLevel> levels = new Dictionary<string, UserPreferences.QualityLevel>
		{
			{
				"UltraLow",
				new UserPreferences.QualityLevel
				{
					renderScale = 1f,
					msaaLevel = 0,
					pixelLightCount = 0,
					shaderLOD = UserPreferences.ShaderLOD.Low,
					smoothPasses = 1,
					mirrorReflections = false,
					realtimeReflectionProbes = false,
					closeObjectBlur = false,
					softPhysics = false,
					glowEffects = UserPreferences.GlowEffectsLevel.Off
				}
			},
			{
				"Low",
				new UserPreferences.QualityLevel
				{
					renderScale = 1f,
					msaaLevel = 2,
					pixelLightCount = 1,
					shaderLOD = UserPreferences.ShaderLOD.Low,
					smoothPasses = 1,
					mirrorReflections = false,
					realtimeReflectionProbes = false,
					closeObjectBlur = false,
					softPhysics = false,
					glowEffects = UserPreferences.GlowEffectsLevel.Off
				}
			},
			{
				"Mid",
				new UserPreferences.QualityLevel
				{
					renderScale = 1f,
					msaaLevel = 4,
					pixelLightCount = 2,
					shaderLOD = UserPreferences.ShaderLOD.High,
					smoothPasses = 1,
					mirrorReflections = false,
					realtimeReflectionProbes = false,
					closeObjectBlur = false,
					softPhysics = true,
					glowEffects = UserPreferences.GlowEffectsLevel.Low
				}
			},
			{
				"High",
				new UserPreferences.QualityLevel
				{
					renderScale = 1f,
					msaaLevel = 4,
					pixelLightCount = 2,
					shaderLOD = UserPreferences.ShaderLOD.High,
					smoothPasses = 2,
					mirrorReflections = true,
					realtimeReflectionProbes = true,
					closeObjectBlur = false,
					softPhysics = true,
					glowEffects = UserPreferences.GlowEffectsLevel.Low
				}
			},
			{
				"Ultra",
				new UserPreferences.QualityLevel
				{
					renderScale = 1.5f,
					msaaLevel = 2,
					pixelLightCount = 3,
					shaderLOD = UserPreferences.ShaderLOD.High,
					smoothPasses = 3,
					mirrorReflections = true,
					realtimeReflectionProbes = true,
					closeObjectBlur = true,
					softPhysics = true,
					glowEffects = UserPreferences.GlowEffectsLevel.High
				}
			},
			{
				"Max",
				new UserPreferences.QualityLevel
				{
					renderScale = 2f,
					msaaLevel = 2,
					pixelLightCount = 4,
					shaderLOD = UserPreferences.ShaderLOD.High,
					smoothPasses = 4,
					mirrorReflections = true,
					realtimeReflectionProbes = true,
					closeObjectBlur = true,
					softPhysics = true,
					glowEffects = UserPreferences.GlowEffectsLevel.High
				}
			}
		};
	}

	// Token: 0x02000C7F RID: 3199
	public enum ShaderLOD
	{
		// Token: 0x04005010 RID: 20496
		Low = 250,
		// Token: 0x04005011 RID: 20497
		Medium = 400,
		// Token: 0x04005012 RID: 20498
		High = 600
	}

	// Token: 0x02000C80 RID: 3200
	public enum GlowEffectsLevel
	{
		// Token: 0x04005014 RID: 20500
		Off,
		// Token: 0x04005015 RID: 20501
		Low,
		// Token: 0x04005016 RID: 20502
		High
	}

	// Token: 0x02000C81 RID: 3201
	public enum PhysicsRate
	{
		// Token: 0x04005018 RID: 20504
		Auto,
		// Token: 0x04005019 RID: 20505
		_45,
		// Token: 0x0400501A RID: 20506
		_60,
		// Token: 0x0400501B RID: 20507
		_72,
		// Token: 0x0400501C RID: 20508
		_80,
		// Token: 0x0400501D RID: 20509
		_90,
		// Token: 0x0400501E RID: 20510
		_120,
		// Token: 0x0400501F RID: 20511
		_144,
		// Token: 0x04005020 RID: 20512
		_240,
		// Token: 0x04005021 RID: 20513
		_288
	}

	// Token: 0x02000C82 RID: 3202
	public enum SortBy
	{
		// Token: 0x04005023 RID: 20515
		AtoZ,
		// Token: 0x04005024 RID: 20516
		ZtoA,
		// Token: 0x04005025 RID: 20517
		NewToOld,
		// Token: 0x04005026 RID: 20518
		OldToNew,
		// Token: 0x04005027 RID: 20519
		NewToOldPackage,
		// Token: 0x04005028 RID: 20520
		OldToNewPackage
	}

	// Token: 0x02000C83 RID: 3203
	public enum DirectoryOption
	{
		// Token: 0x0400502A RID: 20522
		ShowFirst,
		// Token: 0x0400502B RID: 20523
		ShowLast,
		// Token: 0x0400502C RID: 20524
		Intermix,
		// Token: 0x0400502D RID: 20525
		Hide
	}
}
