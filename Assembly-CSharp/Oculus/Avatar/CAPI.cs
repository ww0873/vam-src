using System;
using System.Runtime.InteropServices;

namespace Oculus.Avatar
{
	// Token: 0x020007B8 RID: 1976
	public class CAPI
	{
		// Token: 0x060031EA RID: 12778 RVA: 0x001060E1 File Offset: 0x001044E1
		public CAPI()
		{
		}

		// Token: 0x060031EB RID: 12779
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_Initialize(string appID);

		// Token: 0x060031EC RID: 12780
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_Shutdown();

		// Token: 0x060031ED RID: 12781
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrAvatarMessage_Pop();

		// Token: 0x060031EE RID: 12782
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarMessageType ovrAvatarMessage_GetType(IntPtr msg);

		// Token: 0x060031EF RID: 12783 RVA: 0x001060EC File Offset: 0x001044EC
		public static ovrAvatarMessage_AvatarSpecification ovrAvatarMessage_GetAvatarSpecification(IntPtr msg)
		{
			IntPtr ptr = CAPI.ovrAvatarMessage_GetAvatarSpecification_Native(msg);
			return (ovrAvatarMessage_AvatarSpecification)Marshal.PtrToStructure(ptr, typeof(ovrAvatarMessage_AvatarSpecification));
		}

		// Token: 0x060031F0 RID: 12784
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarMessage_GetAvatarSpecification")]
		private static extern IntPtr ovrAvatarMessage_GetAvatarSpecification_Native(IntPtr msg);

		// Token: 0x060031F1 RID: 12785 RVA: 0x00106118 File Offset: 0x00104518
		public static ovrAvatarMessage_AssetLoaded ovrAvatarMessage_GetAssetLoaded(IntPtr msg)
		{
			IntPtr ptr = CAPI.ovrAvatarMessage_GetAssetLoaded_Native(msg);
			return (ovrAvatarMessage_AssetLoaded)Marshal.PtrToStructure(ptr, typeof(ovrAvatarMessage_AssetLoaded));
		}

		// Token: 0x060031F2 RID: 12786
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarMessage_GetAssetLoaded")]
		private static extern IntPtr ovrAvatarMessage_GetAssetLoaded_Native(IntPtr msg);

		// Token: 0x060031F3 RID: 12787
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarMessage_Free(IntPtr msg);

		// Token: 0x060031F4 RID: 12788
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrAvatarSpecificationRequest_Create(ulong userID);

		// Token: 0x060031F5 RID: 12789
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarSpecificationRequest_Destroy(IntPtr specificationRequest);

		// Token: 0x060031F6 RID: 12790
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_RequestAvatarSpecification(ulong userID);

		// Token: 0x060031F7 RID: 12791
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_RequestAvatarSpecificationFromSpecRequest(IntPtr specificationRequest);

		// Token: 0x060031F8 RID: 12792
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrAvatar_Create(IntPtr avatarSpecification, ovrAvatarCapabilities capabilities);

		// Token: 0x060031F9 RID: 12793
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_Destroy(IntPtr avatar);

		// Token: 0x060031FA RID: 12794
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarPose_UpdateBody(IntPtr avatar, ovrAvatarTransform headPose);

		// Token: 0x060031FB RID: 12795 RVA: 0x00106141 File Offset: 0x00104541
		public static void ovrAvatarPose_UpdateVoiceVisualization(IntPtr avatar, float[] pcmData)
		{
			CAPI.ovrAvatarPose_UpdateVoiceVisualization_Native(avatar, (uint)pcmData.Length, pcmData);
		}

		// Token: 0x060031FC RID: 12796
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarPose_UpdateVoiceVisualization")]
		private static extern void ovrAvatarPose_UpdateVoiceVisualization_Native(IntPtr avatar, uint pcmDataSize, [In] float[] pcmData);

		// Token: 0x060031FD RID: 12797
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarPose_UpdateHands(IntPtr avatar, ovrAvatarHandInputState inputStateLeft, ovrAvatarHandInputState inputStateRight);

		// Token: 0x060031FE RID: 12798
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarPose_Update3DofHands(IntPtr avatar, IntPtr inputStateLeft, IntPtr inputStateRight, ovrAvatarControllerType type);

		// Token: 0x060031FF RID: 12799 RVA: 0x00106150 File Offset: 0x00104550
		public static void ovrAvatarPose_UpdateSDK3DofHands(IntPtr avatar, ovrAvatarHandInputState inputStateLeft, ovrAvatarHandInputState inputStateRight, ovrAvatarControllerType type)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(inputStateLeft));
			IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(inputStateRight));
			Marshal.StructureToPtr(inputStateLeft, intPtr, false);
			Marshal.StructureToPtr(inputStateRight, intPtr2, false);
			CAPI.ovrAvatar_SetLeftControllerVisibility(avatar, true);
			CAPI.ovrAvatar_SetRightControllerVisibility(avatar, true);
			CAPI.ovrAvatar_SetLeftHandVisibility(avatar, true);
			CAPI.ovrAvatar_SetRightHandVisibility(avatar, true);
			CAPI.ovrAvatarPose_Update3DofHands(avatar, intPtr, intPtr2, type);
		}

		// Token: 0x06003200 RID: 12800
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarPose_Finalize(IntPtr avatar, float elapsedSeconds);

		// Token: 0x06003201 RID: 12801
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetLeftControllerVisibility(IntPtr avatar, bool show);

		// Token: 0x06003202 RID: 12802
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetRightControllerVisibility(IntPtr avatar, bool show);

		// Token: 0x06003203 RID: 12803
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetLeftHandVisibility(IntPtr avatar, bool show);

		// Token: 0x06003204 RID: 12804
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetRightHandVisibility(IntPtr avatar, bool show);

		// Token: 0x06003205 RID: 12805
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovrAvatarComponent_Count(IntPtr avatar);

		// Token: 0x06003206 RID: 12806 RVA: 0x001061C0 File Offset: 0x001045C0
		public static ovrAvatarComponent ovrAvatarComponent_Get(IntPtr avatar, uint index)
		{
			IntPtr ptr = CAPI.ovrAvatarComponent_Get_Native(avatar, index);
			return (ovrAvatarComponent)Marshal.PtrToStructure(ptr, typeof(ovrAvatarComponent));
		}

		// Token: 0x06003207 RID: 12807
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarComponent_Get")]
		public static extern IntPtr ovrAvatarComponent_Get_Native(IntPtr avatar, uint index);

		// Token: 0x06003208 RID: 12808 RVA: 0x001061EC File Offset: 0x001045EC
		public static ovrAvatarBaseComponent? ovrAvatarPose_GetBaseComponent(IntPtr avatar)
		{
			IntPtr intPtr = CAPI.ovrAvatarPose_GetBaseComponent_Native(avatar);
			return (!(intPtr == IntPtr.Zero)) ? new ovrAvatarBaseComponent?((ovrAvatarBaseComponent)Marshal.PtrToStructure(intPtr, typeof(ovrAvatarBaseComponent))) : null;
		}

		// Token: 0x06003209 RID: 12809
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarPose_GetBaseComponent")]
		private static extern IntPtr ovrAvatarPose_GetBaseComponent_Native(IntPtr avatar);

		// Token: 0x0600320A RID: 12810 RVA: 0x00106238 File Offset: 0x00104638
		public static ovrAvatarBodyComponent? ovrAvatarPose_GetBodyComponent(IntPtr avatar)
		{
			IntPtr intPtr = CAPI.ovrAvatarPose_GetBodyComponent_Native(avatar);
			return (!(intPtr == IntPtr.Zero)) ? new ovrAvatarBodyComponent?((ovrAvatarBodyComponent)Marshal.PtrToStructure(intPtr, typeof(ovrAvatarBodyComponent))) : null;
		}

		// Token: 0x0600320B RID: 12811
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarPose_GetBodyComponent")]
		private static extern IntPtr ovrAvatarPose_GetBodyComponent_Native(IntPtr avatar);

		// Token: 0x0600320C RID: 12812 RVA: 0x00106284 File Offset: 0x00104684
		public static ovrAvatarControllerComponent? ovrAvatarPose_GetLeftControllerComponent(IntPtr avatar)
		{
			IntPtr intPtr = CAPI.ovrAvatarPose_GetLeftControllerComponent_Native(avatar);
			return (!(intPtr == IntPtr.Zero)) ? new ovrAvatarControllerComponent?((ovrAvatarControllerComponent)Marshal.PtrToStructure(intPtr, typeof(ovrAvatarControllerComponent))) : null;
		}

		// Token: 0x0600320D RID: 12813
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarPose_GetLeftControllerComponent")]
		private static extern IntPtr ovrAvatarPose_GetLeftControllerComponent_Native(IntPtr avatar);

		// Token: 0x0600320E RID: 12814 RVA: 0x001062D0 File Offset: 0x001046D0
		public static ovrAvatarControllerComponent? ovrAvatarPose_GetRightControllerComponent(IntPtr avatar)
		{
			IntPtr intPtr = CAPI.ovrAvatarPose_GetRightControllerComponent_Native(avatar);
			return (!(intPtr == IntPtr.Zero)) ? new ovrAvatarControllerComponent?((ovrAvatarControllerComponent)Marshal.PtrToStructure(intPtr, typeof(ovrAvatarControllerComponent))) : null;
		}

		// Token: 0x0600320F RID: 12815
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarPose_GetRightControllerComponent")]
		private static extern IntPtr ovrAvatarPose_GetRightControllerComponent_Native(IntPtr avatar);

		// Token: 0x06003210 RID: 12816 RVA: 0x0010631C File Offset: 0x0010471C
		public static ovrAvatarHandComponent? ovrAvatarPose_GetLeftHandComponent(IntPtr avatar)
		{
			IntPtr intPtr = CAPI.ovrAvatarPose_GetLeftHandComponent_Native(avatar);
			return (!(intPtr == IntPtr.Zero)) ? new ovrAvatarHandComponent?((ovrAvatarHandComponent)Marshal.PtrToStructure(intPtr, typeof(ovrAvatarHandComponent))) : null;
		}

		// Token: 0x06003211 RID: 12817
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarPose_GetLeftHandComponent")]
		private static extern IntPtr ovrAvatarPose_GetLeftHandComponent_Native(IntPtr avatar);

		// Token: 0x06003212 RID: 12818 RVA: 0x00106368 File Offset: 0x00104768
		public static ovrAvatarHandComponent? ovrAvatarPose_GetRightHandComponent(IntPtr avatar)
		{
			IntPtr intPtr = CAPI.ovrAvatarPose_GetRightHandComponent_Native(avatar);
			return (!(intPtr == IntPtr.Zero)) ? new ovrAvatarHandComponent?((ovrAvatarHandComponent)Marshal.PtrToStructure(intPtr, typeof(ovrAvatarHandComponent))) : null;
		}

		// Token: 0x06003213 RID: 12819
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarPose_GetRightHandComponent")]
		private static extern IntPtr ovrAvatarPose_GetRightHandComponent_Native(IntPtr avatar);

		// Token: 0x06003214 RID: 12820
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarAsset_BeginLoading(ulong assetID);

		// Token: 0x06003215 RID: 12821
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarAssetType ovrAvatarAsset_GetType(IntPtr assetHandle);

		// Token: 0x06003216 RID: 12822 RVA: 0x001063B4 File Offset: 0x001047B4
		public static ovrAvatarMeshAssetData ovrAvatarAsset_GetMeshData(IntPtr assetPtr)
		{
			IntPtr ptr = CAPI.ovrAvatarAsset_GetMeshData_Native(assetPtr);
			return (ovrAvatarMeshAssetData)Marshal.PtrToStructure(ptr, typeof(ovrAvatarMeshAssetData));
		}

		// Token: 0x06003217 RID: 12823
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarAsset_GetMeshData")]
		private static extern IntPtr ovrAvatarAsset_GetMeshData_Native(IntPtr assetPtr);

		// Token: 0x06003218 RID: 12824 RVA: 0x001063E0 File Offset: 0x001047E0
		public static ovrAvatarTextureAssetData ovrAvatarAsset_GetTextureData(IntPtr assetPtr)
		{
			IntPtr ptr = CAPI.ovrAvatarAsset_GetTextureData_Native(assetPtr);
			return (ovrAvatarTextureAssetData)Marshal.PtrToStructure(ptr, typeof(ovrAvatarTextureAssetData));
		}

		// Token: 0x06003219 RID: 12825
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarAsset_GetTextureData")]
		private static extern IntPtr ovrAvatarAsset_GetTextureData_Native(IntPtr assetPtr);

		// Token: 0x0600321A RID: 12826
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarAsset_GetMaterialData")]
		private static extern IntPtr ovrAvatarAsset_GetMaterialData_Native(IntPtr assetPtr);

		// Token: 0x0600321B RID: 12827 RVA: 0x0010640C File Offset: 0x0010480C
		public static ovrAvatarMaterialState ovrAvatarAsset_GetMaterialState(IntPtr assetPtr)
		{
			IntPtr ptr = CAPI.ovrAvatarAsset_GetMaterialData_Native(assetPtr);
			return (ovrAvatarMaterialState)Marshal.PtrToStructure(ptr, typeof(ovrAvatarMaterialState));
		}

		// Token: 0x0600321C RID: 12828
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarRenderPartType ovrAvatarRenderPart_GetType(IntPtr renderPart);

		// Token: 0x0600321D RID: 12829 RVA: 0x00106438 File Offset: 0x00104838
		public static ovrAvatarRenderPart_SkinnedMeshRender ovrAvatarRenderPart_GetSkinnedMeshRender(IntPtr renderPart)
		{
			IntPtr ptr = CAPI.ovrAvatarRenderPart_GetSkinnedMeshRender_Native(renderPart);
			return (ovrAvatarRenderPart_SkinnedMeshRender)Marshal.PtrToStructure(ptr, typeof(ovrAvatarRenderPart_SkinnedMeshRender));
		}

		// Token: 0x0600321E RID: 12830
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarRenderPart_GetSkinnedMeshRender")]
		private static extern IntPtr ovrAvatarRenderPart_GetSkinnedMeshRender_Native(IntPtr renderPart);

		// Token: 0x0600321F RID: 12831
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarTransform ovrAvatarSkinnedMeshRender_GetTransform(IntPtr renderPart);

		// Token: 0x06003220 RID: 12832
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarTransform ovrAvatarSkinnedMeshRenderPBS_GetTransform(IntPtr renderPart);

		// Token: 0x06003221 RID: 12833
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarTransform ovrAvatarSkinnedMeshRenderPBSV2_GetTransform(IntPtr renderPart);

		// Token: 0x06003222 RID: 12834
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarVisibilityFlags ovrAvatarSkinnedMeshRender_GetVisibilityMask(IntPtr renderPart);

		// Token: 0x06003223 RID: 12835
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovrAvatarSkinnedMeshRender_MaterialStateChanged(IntPtr renderPart);

		// Token: 0x06003224 RID: 12836
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovrAvatarSkinnedMeshRenderPBSV2_MaterialStateChanged(IntPtr renderPart);

		// Token: 0x06003225 RID: 12837
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarVisibilityFlags ovrAvatarSkinnedMeshRenderPBS_GetVisibilityMask(IntPtr renderPart);

		// Token: 0x06003226 RID: 12838
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarVisibilityFlags ovrAvatarSkinnedMeshRenderPBSV2_GetVisibilityMask(IntPtr renderPart);

		// Token: 0x06003227 RID: 12839
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarMaterialState ovrAvatarSkinnedMeshRender_GetMaterialState(IntPtr renderPart);

		// Token: 0x06003228 RID: 12840
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarPBSMaterialState ovrAvatarSkinnedMeshRenderPBSV2_GetPBSMaterialState(IntPtr renderPart);

		// Token: 0x06003229 RID: 12841
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovrAvatarSkinnedMeshRender_GetDirtyJoints(IntPtr renderPart);

		// Token: 0x0600322A RID: 12842
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovrAvatarSkinnedMeshRenderPBS_GetDirtyJoints(IntPtr renderPart);

		// Token: 0x0600322B RID: 12843
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovrAvatarSkinnedMeshRenderPBSV2_GetDirtyJoints(IntPtr renderPart);

		// Token: 0x0600322C RID: 12844
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarTransform ovrAvatarSkinnedMeshRender_GetJointTransform(IntPtr renderPart, uint jointIndex);

		// Token: 0x0600322D RID: 12845
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarTransform ovrAvatarSkinnedMeshRenderPBS_GetJointTransform(IntPtr renderPart, uint jointIndex);

		// Token: 0x0600322E RID: 12846
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ovrAvatarTransform ovrAvatarSkinnedMeshRenderPBSV2_GetJointTransform(IntPtr renderPart, uint jointIndex);

		// Token: 0x0600322F RID: 12847
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovrAvatarSkinnedMeshRenderPBS_GetAlbedoTextureAssetID(IntPtr renderPart);

		// Token: 0x06003230 RID: 12848
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovrAvatarSkinnedMeshRenderPBS_GetSurfaceTextureAssetID(IntPtr renderPart);

		// Token: 0x06003231 RID: 12849 RVA: 0x00106464 File Offset: 0x00104864
		public static ovrAvatarRenderPart_SkinnedMeshRenderPBS ovrAvatarRenderPart_GetSkinnedMeshRenderPBS(IntPtr renderPart)
		{
			IntPtr ptr = CAPI.ovrAvatarRenderPart_GetSkinnedMeshRenderPBS_Native(renderPart);
			return (ovrAvatarRenderPart_SkinnedMeshRenderPBS)Marshal.PtrToStructure(ptr, typeof(ovrAvatarRenderPart_SkinnedMeshRenderPBS));
		}

		// Token: 0x06003232 RID: 12850
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarRenderPart_GetSkinnedMeshRenderPBS")]
		private static extern IntPtr ovrAvatarRenderPart_GetSkinnedMeshRenderPBS_Native(IntPtr renderPart);

		// Token: 0x06003233 RID: 12851 RVA: 0x00106490 File Offset: 0x00104890
		public static ovrAvatarRenderPart_SkinnedMeshRenderPBS_V2 ovrAvatarRenderPart_GetSkinnedMeshRenderPBSV2(IntPtr renderPart)
		{
			IntPtr ptr = CAPI.ovrAvatarRenderPart_GetSkinnedMeshRenderPBSV2_Native(renderPart);
			return (ovrAvatarRenderPart_SkinnedMeshRenderPBS_V2)Marshal.PtrToStructure(ptr, typeof(ovrAvatarRenderPart_SkinnedMeshRenderPBS_V2));
		}

		// Token: 0x06003234 RID: 12852
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarRenderPart_GetSkinnedMeshRenderPBSV2")]
		private static extern IntPtr ovrAvatarRenderPart_GetSkinnedMeshRenderPBSV2_Native(IntPtr renderPart);

		// Token: 0x06003235 RID: 12853 RVA: 0x001064BC File Offset: 0x001048BC
		public static ovrAvatarRenderPart_ProjectorRender ovrAvatarRenderPart_GetProjectorRender(IntPtr renderPart)
		{
			IntPtr ptr = CAPI.ovrAvatarRenderPart_GetProjectorRender_Native(renderPart);
			return (ovrAvatarRenderPart_ProjectorRender)Marshal.PtrToStructure(ptr, typeof(ovrAvatarRenderPart_ProjectorRender));
		}

		// Token: 0x06003236 RID: 12854
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovrAvatarRenderPart_GetProjectorRender")]
		private static extern IntPtr ovrAvatarRenderPart_GetProjectorRender_Native(IntPtr renderPart);

		// Token: 0x06003237 RID: 12855
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovrAvatar_GetReferencedAssetCount(IntPtr avatar);

		// Token: 0x06003238 RID: 12856
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovrAvatar_GetReferencedAsset(IntPtr avatar, uint index);

		// Token: 0x06003239 RID: 12857
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetLeftHandGesture(IntPtr avatar, ovrAvatarHandGesture gesture);

		// Token: 0x0600323A RID: 12858
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetRightHandGesture(IntPtr avatar, ovrAvatarHandGesture gesture);

		// Token: 0x0600323B RID: 12859
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetLeftHandCustomGesture(IntPtr avatar, uint jointCount, [In] ovrAvatarTransform[] customJointTransforms);

		// Token: 0x0600323C RID: 12860
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_SetRightHandCustomGesture(IntPtr avatar, uint jointCount, [In] ovrAvatarTransform[] customJointTransforms);

		// Token: 0x0600323D RID: 12861
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatar_UpdatePoseFromPacket(IntPtr avatar, IntPtr packet, float secondsFromStart);

		// Token: 0x0600323E RID: 12862
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarPacket_BeginRecording(IntPtr avatar);

		// Token: 0x0600323F RID: 12863
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrAvatarPacket_EndRecording(IntPtr avatar);

		// Token: 0x06003240 RID: 12864
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovrAvatarPacket_GetSize(IntPtr packet);

		// Token: 0x06003241 RID: 12865
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ovrAvatarPacket_GetDurationSeconds(IntPtr packet);

		// Token: 0x06003242 RID: 12866
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovrAvatarPacket_Free(IntPtr packet);

		// Token: 0x06003243 RID: 12867
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovrAvatarPacket_Write(IntPtr packet, uint bufferSize, [Out] byte[] buffer);

		// Token: 0x06003244 RID: 12868
		[DllImport("libovravatar", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovrAvatarPacket_Read(uint bufferSize, [In] byte[] buffer);

		// Token: 0x0400267F RID: 9855
		private const string LibFile = "libovravatar";
	}
}
