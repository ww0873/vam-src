using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Oculus.Avatar;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200077D RID: 1917
public class OvrAvatar : MonoBehaviour
{
	// Token: 0x06003170 RID: 12656 RVA: 0x00101534 File Offset: 0x000FF934
	public OvrAvatar()
	{
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x0010159E File Offset: 0x000FF99E
	private void OnDestroy()
	{
		if (this.sdkAvatar != IntPtr.Zero)
		{
			CAPI.ovrAvatar_Destroy(this.sdkAvatar);
		}
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x001015C0 File Offset: 0x000FF9C0
	public void AssetLoadedCallback(OvrAvatarAsset asset)
	{
		this.assetLoadingIds.Remove(asset.assetID);
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x001015D4 File Offset: 0x000FF9D4
	private void AddAvatarComponent(GameObject componentObject, ovrAvatarComponent component)
	{
		OvrAvatarComponent ovrAvatarComponent = componentObject.AddComponent<OvrAvatarComponent>();
		this.trackedComponents.Add(component.name, ovrAvatarComponent);
		bool flag = this.AddRenderParts(ovrAvatarComponent, component, componentObject.transform) && this.CombineMeshes && componentObject.name == "body";
		if (flag)
		{
			ovrAvatarComponent.StartMeshCombining(component);
		}
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x0010163C File Offset: 0x000FFA3C
	private OvrAvatarSkinnedMeshRenderComponent AddSkinnedMeshRenderComponent(GameObject gameObject, ovrAvatarRenderPart_SkinnedMeshRender skinnedMeshRender)
	{
		OvrAvatarSkinnedMeshRenderComponent ovrAvatarSkinnedMeshRenderComponent = gameObject.AddComponent<OvrAvatarSkinnedMeshRenderComponent>();
		ovrAvatarSkinnedMeshRenderComponent.Initialize(skinnedMeshRender, this.SurfaceShader, this.SurfaceShaderSelfOccluding, this.ThirdPersonLayer.layerIndex, this.FirstPersonLayer.layerIndex, this.renderPartCount++);
		return ovrAvatarSkinnedMeshRenderComponent;
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x0010168C File Offset: 0x000FFA8C
	private OvrAvatarSkinnedMeshRenderPBSComponent AddSkinnedMeshRenderPBSComponent(GameObject gameObject, ovrAvatarRenderPart_SkinnedMeshRenderPBS skinnedMeshRenderPBS)
	{
		OvrAvatarSkinnedMeshRenderPBSComponent ovrAvatarSkinnedMeshRenderPBSComponent = gameObject.AddComponent<OvrAvatarSkinnedMeshRenderPBSComponent>();
		ovrAvatarSkinnedMeshRenderPBSComponent.Initialize(skinnedMeshRenderPBS, this.SurfaceShaderPBS, this.ThirdPersonLayer.layerIndex, this.FirstPersonLayer.layerIndex, this.renderPartCount++);
		return ovrAvatarSkinnedMeshRenderPBSComponent;
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x001016D8 File Offset: 0x000FFAD8
	private OvrAvatarSkinnedMeshPBSV2RenderComponent AddSkinnedMeshRenderPBSV2Component(GameObject gameObject, ovrAvatarRenderPart_SkinnedMeshRenderPBS_V2 skinnedMeshRenderPBSV2)
	{
		OvrAvatarSkinnedMeshPBSV2RenderComponent ovrAvatarSkinnedMeshPBSV2RenderComponent = gameObject.AddComponent<OvrAvatarSkinnedMeshPBSV2RenderComponent>();
		ovrAvatarSkinnedMeshPBSV2RenderComponent.Initialize(skinnedMeshRenderPBSV2, this.SurfaceShaderPBSV2, this.ThirdPersonLayer.layerIndex, this.FirstPersonLayer.layerIndex, this.renderPartCount++);
		return ovrAvatarSkinnedMeshPBSV2RenderComponent;
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x00101724 File Offset: 0x000FFB24
	private OvrAvatarProjectorRenderComponent AddProjectorRenderComponent(GameObject gameObject, ovrAvatarRenderPart_ProjectorRender projectorRender)
	{
		ovrAvatarComponent ovrAvatarComponent = CAPI.ovrAvatarComponent_Get(this.sdkAvatar, projectorRender.componentIndex);
		OvrAvatarComponent ovrAvatarComponent2;
		if (this.trackedComponents.TryGetValue(ovrAvatarComponent.name, out ovrAvatarComponent2) && (ulong)projectorRender.renderPartIndex < (ulong)((long)ovrAvatarComponent2.RenderParts.Count))
		{
			OvrAvatarRenderComponent target = ovrAvatarComponent2.RenderParts[(int)projectorRender.renderPartIndex];
			OvrAvatarProjectorRenderComponent ovrAvatarProjectorRenderComponent = gameObject.AddComponent<OvrAvatarProjectorRenderComponent>();
			ovrAvatarProjectorRenderComponent.InitializeProjectorRender(projectorRender, this.SurfaceShader, target);
			return ovrAvatarProjectorRenderComponent;
		}
		return null;
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x001017A4 File Offset: 0x000FFBA4
	public static IntPtr GetRenderPart(ovrAvatarComponent component, uint renderPartIndex)
	{
		long num = (long)Marshal.SizeOf(typeof(IntPtr)) * (long)((ulong)renderPartIndex);
		IntPtr ptr = new IntPtr(component.renderParts.ToInt64() + num);
		return (IntPtr)Marshal.PtrToStructure(ptr, typeof(IntPtr));
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x001017F0 File Offset: 0x000FFBF0
	private void UpdateAvatarComponent(ovrAvatarComponent component)
	{
		OvrAvatarComponent ovrAvatarComponent;
		if (!this.trackedComponents.TryGetValue(component.name, out ovrAvatarComponent))
		{
			throw new Exception(string.Format("trackedComponents didn't have {0}", component.name));
		}
		ovrAvatarComponent.UpdateAvatar(component, this);
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x00101835 File Offset: 0x000FFC35
	private static string GetRenderPartName(ovrAvatarComponent component, uint renderPartIndex)
	{
		return component.name + "_renderPart_" + (int)renderPartIndex;
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x00101850 File Offset: 0x000FFC50
	internal static void ConvertTransform(ovrAvatarTransform transform, Transform target)
	{
		Vector3 position = transform.position;
		position.z = -position.z;
		Quaternion orientation = transform.orientation;
		orientation.x = -orientation.x;
		orientation.y = -orientation.y;
		target.localPosition = position;
		target.localRotation = orientation;
		target.localScale = transform.scale;
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x001018B8 File Offset: 0x000FFCB8
	public static ovrAvatarTransform CreateOvrAvatarTransform(Vector3 position, Quaternion orientation)
	{
		return new ovrAvatarTransform
		{
			position = new Vector3(position.x, position.y, -position.z),
			orientation = new Quaternion(-orientation.x, -orientation.y, orientation.z, orientation.w),
			scale = Vector3.one
		};
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x00101928 File Offset: 0x000FFD28
	private void RemoveAvatarComponent(string name)
	{
		OvrAvatarComponent ovrAvatarComponent;
		this.trackedComponents.TryGetValue(name, out ovrAvatarComponent);
		UnityEngine.Object.Destroy(ovrAvatarComponent.gameObject);
		this.trackedComponents.Remove(name);
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x0010195C File Offset: 0x000FFD5C
	private void UpdateSDKAvatarUnityState()
	{
		uint num = CAPI.ovrAvatarComponent_Count(this.sdkAvatar);
		HashSet<string> hashSet = new HashSet<string>();
		for (uint num2 = 0U; num2 < num; num2 += 1U)
		{
			IntPtr intPtr = CAPI.ovrAvatarComponent_Get_Native(this.sdkAvatar, num2);
			ovrAvatarComponent component = (ovrAvatarComponent)Marshal.PtrToStructure(intPtr, typeof(ovrAvatarComponent));
			hashSet.Add(component.name);
			if (!this.trackedComponents.ContainsKey(component.name))
			{
				GameObject gameObject = null;
				Type type = null;
				if ((this.Capabilities & ovrAvatarCapabilities.Base) != (ovrAvatarCapabilities)0)
				{
					ovrAvatarBaseComponent? ovrAvatarBaseComponent = CAPI.ovrAvatarPose_GetBaseComponent(this.sdkAvatar);
					if (ovrAvatarBaseComponent != null && intPtr == ovrAvatarBaseComponent.Value.renderComponent)
					{
						type = typeof(OvrAvatarBase);
						if (this.Base != null)
						{
							gameObject = this.Base.gameObject;
						}
					}
				}
				if (type == null && (this.Capabilities & ovrAvatarCapabilities.Body) != (ovrAvatarCapabilities)0)
				{
					ovrAvatarBodyComponent? ovrAvatarBodyComponent = CAPI.ovrAvatarPose_GetBodyComponent(this.sdkAvatar);
					if (ovrAvatarBodyComponent != null && intPtr == ovrAvatarBodyComponent.Value.renderComponent)
					{
						type = typeof(OvrAvatarBody);
						if (this.Body != null)
						{
							gameObject = this.Body.gameObject;
						}
					}
				}
				if (type == null && (this.Capabilities & ovrAvatarCapabilities.Hands) != (ovrAvatarCapabilities)0)
				{
					ovrAvatarControllerComponent? ovrAvatarControllerComponent = CAPI.ovrAvatarPose_GetLeftControllerComponent(this.sdkAvatar);
					if (type == null && ovrAvatarControllerComponent != null && intPtr == ovrAvatarControllerComponent.Value.renderComponent)
					{
						type = typeof(OvrAvatarTouchController);
						if (this.ControllerLeft != null)
						{
							gameObject = this.ControllerLeft.gameObject;
						}
					}
					ovrAvatarControllerComponent = CAPI.ovrAvatarPose_GetRightControllerComponent(this.sdkAvatar);
					if (type == null && ovrAvatarControllerComponent != null && intPtr == ovrAvatarControllerComponent.Value.renderComponent)
					{
						type = typeof(OvrAvatarTouchController);
						if (this.ControllerRight != null)
						{
							gameObject = this.ControllerRight.gameObject;
						}
					}
					ovrAvatarHandComponent? ovrAvatarHandComponent = CAPI.ovrAvatarPose_GetLeftHandComponent(this.sdkAvatar);
					if (type == null && ovrAvatarHandComponent != null && intPtr == ovrAvatarHandComponent.Value.renderComponent)
					{
						type = typeof(OvrAvatarHand);
						if (this.HandLeft != null)
						{
							gameObject = this.HandLeft.gameObject;
						}
					}
					ovrAvatarHandComponent = CAPI.ovrAvatarPose_GetRightHandComponent(this.sdkAvatar);
					if (type == null && ovrAvatarHandComponent != null && intPtr == ovrAvatarHandComponent.Value.renderComponent)
					{
						type = typeof(OvrAvatarHand);
						if (this.HandRight != null)
						{
							gameObject = this.HandRight.gameObject;
						}
					}
				}
				if (gameObject == null && type == null)
				{
					gameObject = new GameObject();
					gameObject.name = component.name;
					gameObject.transform.SetParent(base.transform);
				}
				if (gameObject != null)
				{
					this.AddAvatarComponent(gameObject, component);
				}
			}
			this.UpdateAvatarComponent(component);
		}
		HashSet<string> hashSet2 = new HashSet<string>(this.trackedComponents.Keys);
		hashSet2.ExceptWith(hashSet);
		foreach (string name in hashSet2)
		{
			this.RemoveAvatarComponent(name);
		}
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x00101D24 File Offset: 0x00100124
	private void UpdateCustomPoses()
	{
		if (OvrAvatar.UpdatePoseRoot(this.LeftHandCustomPose, ref this.cachedLeftHandCustomPose, ref this.cachedCustomLeftHandJoints, ref this.cachedLeftHandTransforms) && this.cachedLeftHandCustomPose == null && this.sdkAvatar != IntPtr.Zero)
		{
			CAPI.ovrAvatar_SetLeftHandGesture(this.sdkAvatar, ovrAvatarHandGesture.Default);
		}
		if (OvrAvatar.UpdatePoseRoot(this.RightHandCustomPose, ref this.cachedRightHandCustomPose, ref this.cachedCustomRightHandJoints, ref this.cachedRightHandTransforms) && this.cachedRightHandCustomPose == null && this.sdkAvatar != IntPtr.Zero)
		{
			CAPI.ovrAvatar_SetRightHandGesture(this.sdkAvatar, ovrAvatarHandGesture.Default);
		}
		if (this.sdkAvatar != IntPtr.Zero)
		{
			if (this.cachedLeftHandCustomPose != null && OvrAvatar.UpdateTransforms(this.cachedCustomLeftHandJoints, this.cachedLeftHandTransforms))
			{
				CAPI.ovrAvatar_SetLeftHandCustomGesture(this.sdkAvatar, (uint)this.cachedLeftHandTransforms.Length, this.cachedLeftHandTransforms);
			}
			if (this.cachedRightHandCustomPose != null && OvrAvatar.UpdateTransforms(this.cachedCustomRightHandJoints, this.cachedRightHandTransforms))
			{
				CAPI.ovrAvatar_SetRightHandCustomGesture(this.sdkAvatar, (uint)this.cachedRightHandTransforms.Length, this.cachedRightHandTransforms);
			}
		}
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x00101E70 File Offset: 0x00100270
	private static bool UpdatePoseRoot(Transform poseRoot, ref Transform cachedPoseRoot, ref Transform[] cachedPoseJoints, ref ovrAvatarTransform[] transforms)
	{
		if (poseRoot == cachedPoseRoot)
		{
			return false;
		}
		if (!poseRoot)
		{
			cachedPoseRoot = null;
			cachedPoseJoints = null;
			transforms = null;
		}
		else
		{
			List<Transform> list = new List<Transform>();
			OvrAvatar.OrderJoints(poseRoot, list);
			cachedPoseRoot = poseRoot;
			cachedPoseJoints = list.ToArray();
			transforms = new ovrAvatarTransform[list.Count];
		}
		return true;
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x00101ECC File Offset: 0x001002CC
	private static bool UpdateTransforms(Transform[] joints, ovrAvatarTransform[] transforms)
	{
		bool result = false;
		for (int i = 0; i < joints.Length; i++)
		{
			Transform transform = joints[i];
			ovrAvatarTransform ovrAvatarTransform = OvrAvatar.CreateOvrAvatarTransform(transform.localPosition, transform.localRotation);
			if (ovrAvatarTransform.position != transforms[i].position || ovrAvatarTransform.orientation != transforms[i].orientation)
			{
				transforms[i] = ovrAvatarTransform;
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x00101F50 File Offset: 0x00100350
	private static void OrderJoints(Transform transform, List<Transform> joints)
	{
		joints.Add(transform);
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			OvrAvatar.OrderJoints(child, joints);
		}
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x00101F8C File Offset: 0x0010038C
	private void AvatarSpecificationCallback(IntPtr avatarSpecification)
	{
		this.sdkAvatar = CAPI.ovrAvatar_Create(avatarSpecification, this.Capabilities);
		this.ShowLeftController(this.showLeftController);
		this.ShowRightController(this.showRightController);
		uint num = CAPI.ovrAvatar_GetReferencedAssetCount(this.sdkAvatar);
		for (uint num2 = 0U; num2 < num; num2 += 1U)
		{
			ulong num3 = CAPI.ovrAvatar_GetReferencedAsset(this.sdkAvatar, num2);
			if (OvrAvatarSDKManager.Instance.GetAsset(num3) == null)
			{
				OvrAvatarSDKManager.Instance.BeginLoadingAsset(num3, new assetLoadedCallback(this.AssetLoadedCallback));
				this.assetLoadingIds.Add(num3);
			}
		}
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x00102024 File Offset: 0x00100424
	private void Start()
	{
		this.ShowLeftController(this.StartWithControllers);
		this.ShowRightController(this.StartWithControllers);
		OvrAvatarSDKManager.Instance.RequestAvatarSpecification(this.oculusUserID, new specificationCallback(this.AvatarSpecificationCallback));
		this.Driver.Mode = ((!this.UseSDKPackets) ? OvrAvatarDriver.PacketMode.Unity : OvrAvatarDriver.PacketMode.SDK);
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x00102084 File Offset: 0x00100484
	private void Update()
	{
		if (this.sdkAvatar == IntPtr.Zero)
		{
			return;
		}
		if (this.Driver != null)
		{
			this.Driver.UpdateTransforms(this.sdkAvatar);
			foreach (float[] pcmData in this.voiceUpdates)
			{
				CAPI.ovrAvatarPose_UpdateVoiceVisualization(this.sdkAvatar, pcmData);
			}
			this.voiceUpdates.Clear();
			CAPI.ovrAvatarPose_Finalize(this.sdkAvatar, Time.deltaTime);
		}
		if (this.RecordPackets)
		{
			this.RecordFrame();
		}
		if (this.assetLoadingIds.Count == 0)
		{
			this.UpdateSDKAvatarUnityState();
			this.UpdateCustomPoses();
			if (!this.assetsFinishedLoading)
			{
				this.AssetsDoneLoading.Invoke();
				this.assetsFinishedLoading = true;
			}
		}
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x00102184 File Offset: 0x00100584
	public static ovrAvatarHandInputState CreateInputState(ovrAvatarTransform transform, OvrAvatarDriver.ControllerPose pose)
	{
		return new ovrAvatarHandInputState
		{
			transform = transform,
			buttonMask = pose.buttons,
			touchMask = pose.touches,
			joystickX = pose.joystickPosition.x,
			joystickY = pose.joystickPosition.y,
			indexTrigger = pose.indexTrigger,
			handTrigger = pose.handTrigger,
			isActive = pose.isActive
		};
	}

	// Token: 0x06003187 RID: 12679 RVA: 0x0010220E File Offset: 0x0010060E
	public void ShowControllers(bool show)
	{
		this.ShowLeftController(show);
		this.ShowRightController(show);
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x0010221E File Offset: 0x0010061E
	public void ShowLeftController(bool show)
	{
		if (this.sdkAvatar != IntPtr.Zero)
		{
			CAPI.ovrAvatar_SetLeftControllerVisibility(this.sdkAvatar, show);
		}
		this.showLeftController = show;
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x00102248 File Offset: 0x00100648
	public void ShowRightController(bool show)
	{
		if (this.sdkAvatar != IntPtr.Zero)
		{
			CAPI.ovrAvatar_SetRightControllerVisibility(this.sdkAvatar, show);
		}
		this.showRightController = show;
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x00102272 File Offset: 0x00100672
	public void UpdateVoiceVisualization(float[] voiceSamples)
	{
		this.voiceUpdates.Add(voiceSamples);
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x00102280 File Offset: 0x00100680
	private void RecordFrame()
	{
		if (this.UseSDKPackets)
		{
			this.RecordSDKFrame();
		}
		else
		{
			this.RecordUnityFrame();
		}
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x001022A0 File Offset: 0x001006A0
	private void RecordUnityFrame()
	{
		float num = Time.deltaTime;
		OvrAvatarDriver.PoseFrame currentPose = this.Driver.GetCurrentPose();
		if (this.CurrentUnityPacket == null)
		{
			this.CurrentUnityPacket = new OvrAvatarPacket(currentPose);
			num = 0f;
		}
		float num2 = 0f;
		while (num2 < num)
		{
			float num3 = num - num2;
			float num4 = this.PacketSettings.UpdateRate - this.CurrentUnityPacket.Duration;
			if (num3 < num4)
			{
				this.CurrentUnityPacket.AddFrame(currentPose, num3);
				num2 += num3;
			}
			else
			{
				OvrAvatarDriver.PoseFrame finalFrame = this.CurrentUnityPacket.FinalFrame;
				OvrAvatarDriver.PoseFrame b = currentPose;
				float t = num4 / num3;
				OvrAvatarDriver.PoseFrame poseFrame = OvrAvatarDriver.PoseFrame.Interpolate(finalFrame, b, t);
				this.CurrentUnityPacket.AddFrame(poseFrame, num4);
				num2 += num4;
				if (this.PacketRecorded != null)
				{
					this.PacketRecorded(this, new OvrAvatar.PacketEventArgs(this.CurrentUnityPacket));
				}
				this.CurrentUnityPacket = new OvrAvatarPacket(poseFrame);
			}
		}
	}

	// Token: 0x0600318D RID: 12685 RVA: 0x00102390 File Offset: 0x00100790
	private void RecordSDKFrame()
	{
		if (this.sdkAvatar == IntPtr.Zero)
		{
			return;
		}
		if (!this.PacketSettings.RecordingFrames)
		{
			CAPI.ovrAvatarPacket_BeginRecording(this.sdkAvatar);
			this.PacketSettings.AccumulatedTime = 0f;
			this.PacketSettings.RecordingFrames = true;
		}
		this.PacketSettings.AccumulatedTime += Time.deltaTime;
		if (this.PacketSettings.AccumulatedTime >= this.PacketSettings.UpdateRate)
		{
			this.PacketSettings.AccumulatedTime = 0f;
			IntPtr intPtr = CAPI.ovrAvatarPacket_EndRecording(this.sdkAvatar);
			CAPI.ovrAvatarPacket_BeginRecording(this.sdkAvatar);
			if (this.PacketRecorded != null)
			{
				this.PacketRecorded(this, new OvrAvatar.PacketEventArgs(new OvrAvatarPacket
				{
					ovrNativePacket = intPtr
				}));
			}
			CAPI.ovrAvatarPacket_Free(intPtr);
		}
	}

	// Token: 0x0600318E RID: 12686 RVA: 0x00102474 File Offset: 0x00100874
	private bool AddRenderParts(OvrAvatarComponent ovrComponent, ovrAvatarComponent component, Transform parent)
	{
		bool result = true;
		for (uint num = 0U; num < component.renderPartCount; num += 1U)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = OvrAvatar.GetRenderPartName(component, num);
			gameObject.transform.SetParent(parent);
			IntPtr renderPart = OvrAvatar.GetRenderPart(component, num);
			ovrAvatarRenderPartType ovrAvatarRenderPartType = CAPI.ovrAvatarRenderPart_GetType(renderPart);
			OvrAvatarRenderComponent item;
			switch (ovrAvatarRenderPartType)
			{
			case ovrAvatarRenderPartType.SkinnedMeshRender:
				item = this.AddSkinnedMeshRenderComponent(gameObject, CAPI.ovrAvatarRenderPart_GetSkinnedMeshRender(renderPart));
				break;
			case ovrAvatarRenderPartType.SkinnedMeshRenderPBS:
				item = this.AddSkinnedMeshRenderPBSComponent(gameObject, CAPI.ovrAvatarRenderPart_GetSkinnedMeshRenderPBS(renderPart));
				break;
			case ovrAvatarRenderPartType.ProjectorRender:
				result = false;
				item = this.AddProjectorRenderComponent(gameObject, CAPI.ovrAvatarRenderPart_GetProjectorRender(renderPart));
				break;
			case ovrAvatarRenderPartType.SkinnedMeshRenderPBS_V2:
				result = false;
				item = this.AddSkinnedMeshRenderPBSV2Component(gameObject, CAPI.ovrAvatarRenderPart_GetSkinnedMeshRenderPBSV2(renderPart));
				break;
			default:
				throw new NotImplementedException(string.Format("Unsupported render part type: {0}", ovrAvatarRenderPartType.ToString()));
			}
			ovrComponent.RenderParts.Add(item);
		}
		return result;
	}

	// Token: 0x0600318F RID: 12687 RVA: 0x00102568 File Offset: 0x00100968
	public void RefreshBodyParts()
	{
		OvrAvatarComponent ovrAvatarComponent;
		if (this.trackedComponents.TryGetValue("body", out ovrAvatarComponent) && this.Body != null)
		{
			foreach (OvrAvatarRenderComponent ovrAvatarRenderComponent in ovrAvatarComponent.RenderParts)
			{
				UnityEngine.Object.Destroy(ovrAvatarRenderComponent.gameObject);
			}
			ovrAvatarComponent.RenderParts.Clear();
			ovrAvatarBodyComponent? ovrAvatarBodyComponent = CAPI.ovrAvatarPose_GetBodyComponent(this.sdkAvatar);
			if (ovrAvatarBodyComponent == null)
			{
				throw new Exception("Destroyed the body component, but didn't find a new one in the SDK");
			}
			ovrAvatarComponent component = (ovrAvatarComponent)Marshal.PtrToStructure(ovrAvatarBodyComponent.Value.renderComponent, typeof(ovrAvatarComponent));
			this.AddRenderParts(ovrAvatarComponent, component, this.Body.gameObject.transform);
		}
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x00102664 File Offset: 0x00100A64
	public ovrAvatarBodyComponent? GetBodyComponent()
	{
		return CAPI.ovrAvatarPose_GetBodyComponent(this.sdkAvatar);
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x00102674 File Offset: 0x00100A74
	public Transform GetHandTransform(OvrAvatar.HandType hand, OvrAvatar.HandJoint joint)
	{
		if (hand >= OvrAvatar.HandType.Max || joint >= OvrAvatar.HandJoint.Max)
		{
			return null;
		}
		OvrAvatarHand ovrAvatarHand = (hand != OvrAvatar.HandType.Left) ? this.HandRight : this.HandLeft;
		if (ovrAvatarHand != null)
		{
			OvrAvatarComponent component = ovrAvatarHand.GetComponent<OvrAvatarComponent>();
			if (component != null && component.RenderParts.Count > 0)
			{
				OvrAvatarRenderComponent ovrAvatarRenderComponent = component.RenderParts[0];
				return ovrAvatarRenderComponent.transform.Find(OvrAvatar.HandJoints[(int)hand, (int)joint]);
			}
		}
		return null;
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x00102700 File Offset: 0x00100B00
	public void GetPointingDirection(OvrAvatar.HandType hand, ref Vector3 forward, ref Vector3 up)
	{
		Transform handTransform = this.GetHandTransform(hand, OvrAvatar.HandJoint.HandBase);
		if (handTransform != null)
		{
			forward = handTransform.forward;
			up = handTransform.up;
		}
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x0010273C File Offset: 0x00100B3C
	public Transform GetMouthTransform()
	{
		OvrAvatarComponent ovrAvatarComponent;
		if (this.trackedComponents.TryGetValue("voice", out ovrAvatarComponent) && ovrAvatarComponent.RenderParts.Count > 0)
		{
			return ovrAvatarComponent.RenderParts[0].transform;
		}
		return null;
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x00102784 File Offset: 0x00100B84
	// Note: this type is marked as 'beforefieldinit'.
	static OvrAvatar()
	{
		string[,] array = new string[2, 5];
		array[0, 0] = "hands:r_hand_world";
		array[0, 1] = "hands:r_hand_world/hands:b_r_hand/hands:b_r_index1";
		array[0, 2] = "hands:r_hand_world/hands:b_r_hand/hands:b_r_index1/hands:b_r_index2/hands:b_r_index3/hands:b_r_index_ignore";
		array[0, 3] = "hands:r_hand_world/hands:b_r_hand/hands:b_r_thumb1/hands:b_r_thumb2";
		array[0, 4] = "hands:r_hand_world/hands:b_r_hand/hands:b_r_thumb1/hands:b_r_thumb2/hands:b_r_thumb3/hands:b_r_thumb_ignore";
		array[1, 0] = "hands:l_hand_world";
		array[1, 1] = "hands:l_hand_world/hands:b_l_hand/hands:b_l_index1";
		array[1, 2] = "hands:l_hand_world/hands:b_l_hand/hands:b_l_index1/hands:b_l_index2/hands:b_l_index3/hands:b_l_index_ignore";
		array[1, 3] = "hands:l_hand_world/hands:b_l_hand/hands:b_l_thumb1/hands:b_l_thumb2";
		array[1, 4] = "hands:l_hand_world/hands:b_l_hand/hands:b_l_thumb1/hands:b_l_thumb2/hands:b_l_thumb3/hands:b_l_thumb_ignore";
		OvrAvatar.HandJoints = array;
	}

	// Token: 0x04002538 RID: 9528
	public OvrAvatarDriver Driver;

	// Token: 0x04002539 RID: 9529
	public OvrAvatarBase Base;

	// Token: 0x0400253A RID: 9530
	public OvrAvatarBody Body;

	// Token: 0x0400253B RID: 9531
	public OvrAvatarTouchController ControllerLeft;

	// Token: 0x0400253C RID: 9532
	public OvrAvatarTouchController ControllerRight;

	// Token: 0x0400253D RID: 9533
	public OvrAvatarHand HandLeft;

	// Token: 0x0400253E RID: 9534
	public OvrAvatarHand HandRight;

	// Token: 0x0400253F RID: 9535
	public bool RecordPackets;

	// Token: 0x04002540 RID: 9536
	public bool UseSDKPackets = true;

	// Token: 0x04002541 RID: 9537
	public bool StartWithControllers;

	// Token: 0x04002542 RID: 9538
	public AvatarLayer FirstPersonLayer;

	// Token: 0x04002543 RID: 9539
	public AvatarLayer ThirdPersonLayer;

	// Token: 0x04002544 RID: 9540
	public bool ShowFirstPerson = true;

	// Token: 0x04002545 RID: 9541
	public bool ShowThirdPerson;

	// Token: 0x04002546 RID: 9542
	public ovrAvatarCapabilities Capabilities = ovrAvatarCapabilities.All;

	// Token: 0x04002547 RID: 9543
	public Shader SurfaceShader;

	// Token: 0x04002548 RID: 9544
	public Shader SurfaceShaderSelfOccluding;

	// Token: 0x04002549 RID: 9545
	public Shader SurfaceShaderPBS;

	// Token: 0x0400254A RID: 9546
	public Shader SurfaceShaderPBSV2;

	// Token: 0x0400254B RID: 9547
	private int renderPartCount;

	// Token: 0x0400254C RID: 9548
	private bool showLeftController;

	// Token: 0x0400254D RID: 9549
	private bool showRightController;

	// Token: 0x0400254E RID: 9550
	private List<float[]> voiceUpdates = new List<float[]>();

	// Token: 0x0400254F RID: 9551
	public ulong oculusUserID;

	// Token: 0x04002550 RID: 9552
	public bool CombineMeshes;

	// Token: 0x04002551 RID: 9553
	public IntPtr sdkAvatar = IntPtr.Zero;

	// Token: 0x04002552 RID: 9554
	private HashSet<ulong> assetLoadingIds = new HashSet<ulong>();

	// Token: 0x04002553 RID: 9555
	private Dictionary<string, OvrAvatarComponent> trackedComponents = new Dictionary<string, OvrAvatarComponent>();

	// Token: 0x04002554 RID: 9556
	public UnityEvent AssetsDoneLoading = new UnityEvent();

	// Token: 0x04002555 RID: 9557
	private bool assetsFinishedLoading;

	// Token: 0x04002556 RID: 9558
	public Transform LeftHandCustomPose;

	// Token: 0x04002557 RID: 9559
	public Transform RightHandCustomPose;

	// Token: 0x04002558 RID: 9560
	private Transform cachedLeftHandCustomPose;

	// Token: 0x04002559 RID: 9561
	private Transform[] cachedCustomLeftHandJoints;

	// Token: 0x0400255A RID: 9562
	private ovrAvatarTransform[] cachedLeftHandTransforms;

	// Token: 0x0400255B RID: 9563
	private Transform cachedRightHandCustomPose;

	// Token: 0x0400255C RID: 9564
	private Transform[] cachedCustomRightHandJoints;

	// Token: 0x0400255D RID: 9565
	private ovrAvatarTransform[] cachedRightHandTransforms;

	// Token: 0x0400255E RID: 9566
	public PacketRecordSettings PacketSettings = new PacketRecordSettings();

	// Token: 0x0400255F RID: 9567
	private OvrAvatarPacket CurrentUnityPacket;

	// Token: 0x04002560 RID: 9568
	private static string[,] HandJoints;

	// Token: 0x04002561 RID: 9569
	public EventHandler<OvrAvatar.PacketEventArgs> PacketRecorded;

	// Token: 0x0200077E RID: 1918
	public class PacketEventArgs : EventArgs
	{
		// Token: 0x06003195 RID: 12693 RVA: 0x0010281F File Offset: 0x00100C1F
		public PacketEventArgs(OvrAvatarPacket packet)
		{
			this.Packet = packet;
		}

		// Token: 0x04002562 RID: 9570
		public readonly OvrAvatarPacket Packet;
	}

	// Token: 0x0200077F RID: 1919
	public enum HandType
	{
		// Token: 0x04002564 RID: 9572
		Right,
		// Token: 0x04002565 RID: 9573
		Left,
		// Token: 0x04002566 RID: 9574
		Max
	}

	// Token: 0x02000780 RID: 1920
	public enum HandJoint
	{
		// Token: 0x04002568 RID: 9576
		HandBase,
		// Token: 0x04002569 RID: 9577
		IndexBase,
		// Token: 0x0400256A RID: 9578
		IndexTip,
		// Token: 0x0400256B RID: 9579
		ThumbBase,
		// Token: 0x0400256C RID: 9580
		ThumbTip,
		// Token: 0x0400256D RID: 9581
		Max
	}
}
