using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.RTSaveLoad.PersistentObjects;
using Battlehub.RTSaveLoad.PersistentObjects.AI;
using Battlehub.RTSaveLoad.PersistentObjects.Audio;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using Battlehub.RTSaveLoad.PersistentObjects.Experimental.Rendering;
using Battlehub.RTSaveLoad.PersistentObjects.Networking.Match;
using Battlehub.RTSaveLoad.PersistentObjects.Networking.PlayerConnection;
using Battlehub.RTSaveLoad.PersistentObjects.Rendering;
using Battlehub.RTSaveLoad.PersistentObjects.UI;
using Battlehub.RTSaveLoad.PersistentObjects.Video;
using ProtoBuf;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200015F RID: 351
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(500, typeof(PersistentAnimationCurve))]
	[ProtoInclude(501, typeof(PersistentBurst))]
	[ProtoInclude(502, typeof(PersistentColorBySpeedModule))]
	[ProtoInclude(503, typeof(PersistentColorOverLifetimeModule))]
	[ProtoInclude(504, typeof(PersistentCustomDataModule))]
	[ProtoInclude(505, typeof(PersistentEmitParams))]
	[ProtoInclude(506, typeof(PersistentExternalForcesModule))]
	[ProtoInclude(507, typeof(PersistentForceOverLifetimeModule))]
	[ProtoInclude(508, typeof(PersistentGradient))]
	[ProtoInclude(509, typeof(PersistentInheritVelocityModule))]
	[ProtoInclude(510, typeof(PersistentKeyframe))]
	[ProtoInclude(511, typeof(PersistentLightsModule))]
	[ProtoInclude(512, typeof(PersistentLimitVelocityOverLifetimeModule))]
	[ProtoInclude(513, typeof(PersistentMainModule))]
	[ProtoInclude(514, typeof(PersistentMinMaxCurve))]
	[ProtoInclude(515, typeof(PersistentMinMaxGradient))]
	[ProtoInclude(516, typeof(PersistentNoiseModule))]
	[ProtoInclude(517, typeof(PersistentParticle))]
	[ProtoInclude(518, typeof(PersistentRotationBySpeedModule))]
	[ProtoInclude(519, typeof(PersistentRotationOverLifetimeModule))]
	[ProtoInclude(520, typeof(PersistentShapeModule))]
	[ProtoInclude(521, typeof(PersistentSizeBySpeedModule))]
	[ProtoInclude(522, typeof(PersistentSizeOverLifetimeModule))]
	[ProtoInclude(523, typeof(PersistentSubEmittersModule))]
	[ProtoInclude(524, typeof(PersistentTextureSheetAnimationModule))]
	[ProtoInclude(525, typeof(PersistentTrailModule))]
	[ProtoInclude(526, typeof(PersistentVelocityOverLifetimeModule))]
	[ProtoInclude(527, typeof(PersistentGUIStyle))]
	[ProtoInclude(528, typeof(PersistentGUIStyleState))]
	[ProtoInclude(529, typeof(PersistentCollisionModule))]
	[ProtoInclude(530, typeof(PersistentEmissionModule))]
	[ProtoInclude(531, typeof(PersistentTriggerModule))]
	[ProtoInclude(532, typeof(PersistentObject))]
	[ProtoInclude(533, typeof(PersistentNavigation))]
	[ProtoInclude(534, typeof(PersistentOptionData))]
	[ProtoInclude(535, typeof(PersistentSpriteState))]
	[Serializable]
	public abstract class PersistentData
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x0002AF24 File Offset: 0x00029324
		static PersistentData()
		{
			PersistentData.m_objToData.Add(typeof(AssetBundle), typeof(PersistentAssetBundle));
			PersistentData.m_objToData.Add(typeof(AssetBundleManifest), typeof(PersistentAssetBundleManifest));
			PersistentData.m_objToData.Add(typeof(ScriptableObject), typeof(PersistentScriptableObject));
			PersistentData.m_objToData.Add(typeof(Behaviour), typeof(PersistentBehaviour));
			PersistentData.m_objToData.Add(typeof(BillboardAsset), typeof(PersistentBillboardAsset));
			PersistentData.m_objToData.Add(typeof(BillboardRenderer), typeof(PersistentBillboardRenderer));
			PersistentData.m_objToData.Add(typeof(Camera), typeof(PersistentCamera));
			PersistentData.m_objToData.Add(typeof(Component), typeof(PersistentComponent));
			PersistentData.m_objToData.Add(typeof(ComputeShader), typeof(PersistentComputeShader));
			PersistentData.m_objToData.Add(typeof(FlareLayer), typeof(PersistentFlareLayer));
			PersistentData.m_objToData.Add(typeof(GameObject), typeof(PersistentGameObject));
			PersistentData.m_objToData.Add(typeof(OcclusionArea), typeof(PersistentOcclusionArea));
			PersistentData.m_objToData.Add(typeof(OcclusionPortal), typeof(PersistentOcclusionPortal));
			PersistentData.m_objToData.Add(typeof(RenderSettings), typeof(PersistentRenderSettings));
			PersistentData.m_objToData.Add(typeof(QualitySettings), typeof(PersistentQualitySettings));
			PersistentData.m_objToData.Add(typeof(MeshFilter), typeof(PersistentMeshFilter));
			PersistentData.m_objToData.Add(typeof(SkinnedMeshRenderer), typeof(PersistentSkinnedMeshRenderer));
			PersistentData.m_objToData.Add(typeof(Flare), typeof(PersistentFlare));
			PersistentData.m_objToData.Add(typeof(LensFlare), typeof(PersistentLensFlare));
			PersistentData.m_objToData.Add(typeof(Renderer), typeof(PersistentRenderer));
			PersistentData.m_objToData.Add(typeof(Projector), typeof(PersistentProjector));
			PersistentData.m_objToData.Add(typeof(Skybox), typeof(PersistentSkybox));
			PersistentData.m_objToData.Add(typeof(TrailRenderer), typeof(PersistentTrailRenderer));
			PersistentData.m_objToData.Add(typeof(LineRenderer), typeof(PersistentLineRenderer));
			PersistentData.m_objToData.Add(typeof(LightProbes), typeof(PersistentLightProbes));
			PersistentData.m_objToData.Add(typeof(LightmapSettings), typeof(PersistentLightmapSettings));
			PersistentData.m_objToData.Add(typeof(MeshRenderer), typeof(PersistentMeshRenderer));
			PersistentData.m_objToData.Add(typeof(GUIElement), typeof(PersistentGUIElement));
			PersistentData.m_objToData.Add(typeof(Light), typeof(PersistentLight));
			PersistentData.m_objToData.Add(typeof(LightProbeGroup), typeof(PersistentLightProbeGroup));
			PersistentData.m_objToData.Add(typeof(LightProbeProxyVolume), typeof(PersistentLightProbeProxyVolume));
			PersistentData.m_objToData.Add(typeof(LODGroup), typeof(PersistentLODGroup));
			PersistentData.m_objToData.Add(typeof(Mesh), typeof(PersistentMesh));
			PersistentData.m_objToData.Add(typeof(MonoBehaviour), typeof(PersistentMonoBehaviour));
			PersistentData.m_objToData.Add(typeof(ReflectionProbe), typeof(PersistentReflectionProbe));
			PersistentData.m_objToData.Add(typeof(GraphicsSettings), typeof(PersistentGraphicsSettings));
			PersistentData.m_objToData.Add(typeof(Shader), typeof(PersistentShader));
			PersistentData.m_objToData.Add(typeof(Material), typeof(PersistentMaterial));
			PersistentData.m_objToData.Add(typeof(ShaderVariantCollection), typeof(PersistentShaderVariantCollection));
			PersistentData.m_objToData.Add(typeof(Sprite), typeof(PersistentSprite));
			PersistentData.m_objToData.Add(typeof(SpriteRenderer), typeof(PersistentSpriteRenderer));
			PersistentData.m_objToData.Add(typeof(TextAsset), typeof(PersistentTextAsset));
			PersistentData.m_objToData.Add(typeof(Texture), typeof(PersistentTexture));
			PersistentData.m_objToData.Add(typeof(Texture2D), typeof(PersistentTexture2D));
			PersistentData.m_objToData.Add(typeof(Cubemap), typeof(PersistentCubemap));
			PersistentData.m_objToData.Add(typeof(Texture3D), typeof(PersistentTexture3D));
			PersistentData.m_objToData.Add(typeof(Texture2DArray), typeof(PersistentTexture2DArray));
			PersistentData.m_objToData.Add(typeof(CubemapArray), typeof(PersistentCubemapArray));
			PersistentData.m_objToData.Add(typeof(SparseTexture), typeof(PersistentSparseTexture));
			PersistentData.m_objToData.Add(typeof(RenderTexture), typeof(PersistentRenderTexture));
			PersistentData.m_objToData.Add(typeof(WindZone), typeof(PersistentWindZone));
			PersistentData.m_objToData.Add(typeof(Transform), typeof(PersistentTransform));
			PersistentData.m_objToData.Add(typeof(RectTransform), typeof(PersistentRectTransform));
			PersistentData.m_objToData.Add(typeof(SortingGroup), typeof(PersistentSortingGroup));
			PersistentData.m_objToData.Add(typeof(ParticleSystem), typeof(PersistentParticleSystem));
			PersistentData.m_objToData.Add(typeof(ParticleSystemRenderer), typeof(PersistentParticleSystemRenderer));
			PersistentData.m_objToData.Add(typeof(Rigidbody), typeof(PersistentRigidbody));
			PersistentData.m_objToData.Add(typeof(Joint), typeof(PersistentJoint));
			PersistentData.m_objToData.Add(typeof(HingeJoint), typeof(PersistentHingeJoint));
			PersistentData.m_objToData.Add(typeof(SpringJoint), typeof(PersistentSpringJoint));
			PersistentData.m_objToData.Add(typeof(FixedJoint), typeof(PersistentFixedJoint));
			PersistentData.m_objToData.Add(typeof(CharacterJoint), typeof(PersistentCharacterJoint));
			PersistentData.m_objToData.Add(typeof(ConfigurableJoint), typeof(PersistentConfigurableJoint));
			PersistentData.m_objToData.Add(typeof(ConstantForce), typeof(PersistentConstantForce));
			PersistentData.m_objToData.Add(typeof(Collider), typeof(PersistentCollider));
			PersistentData.m_objToData.Add(typeof(BoxCollider), typeof(PersistentBoxCollider));
			PersistentData.m_objToData.Add(typeof(SphereCollider), typeof(PersistentSphereCollider));
			PersistentData.m_objToData.Add(typeof(MeshCollider), typeof(PersistentMeshCollider));
			PersistentData.m_objToData.Add(typeof(CapsuleCollider), typeof(PersistentCapsuleCollider));
			PersistentData.m_objToData.Add(typeof(PhysicMaterial), typeof(PersistentPhysicMaterial));
			PersistentData.m_objToData.Add(typeof(CharacterController), typeof(PersistentCharacterController));
			PersistentData.m_objToData.Add(typeof(CircleCollider2D), typeof(PersistentCircleCollider2D));
			PersistentData.m_objToData.Add(typeof(BoxCollider2D), typeof(PersistentBoxCollider2D));
			PersistentData.m_objToData.Add(typeof(Joint2D), typeof(PersistentJoint2D));
			PersistentData.m_objToData.Add(typeof(AreaEffector2D), typeof(PersistentAreaEffector2D));
			PersistentData.m_objToData.Add(typeof(PlatformEffector2D), typeof(PersistentPlatformEffector2D));
			PersistentData.m_objToData.Add(typeof(Rigidbody2D), typeof(PersistentRigidbody2D));
			PersistentData.m_objToData.Add(typeof(Collider2D), typeof(PersistentCollider2D));
			PersistentData.m_objToData.Add(typeof(EdgeCollider2D), typeof(PersistentEdgeCollider2D));
			PersistentData.m_objToData.Add(typeof(CapsuleCollider2D), typeof(PersistentCapsuleCollider2D));
			PersistentData.m_objToData.Add(typeof(CompositeCollider2D), typeof(PersistentCompositeCollider2D));
			PersistentData.m_objToData.Add(typeof(PolygonCollider2D), typeof(PersistentPolygonCollider2D));
			PersistentData.m_objToData.Add(typeof(AnchoredJoint2D), typeof(PersistentAnchoredJoint2D));
			PersistentData.m_objToData.Add(typeof(SpringJoint2D), typeof(PersistentSpringJoint2D));
			PersistentData.m_objToData.Add(typeof(DistanceJoint2D), typeof(PersistentDistanceJoint2D));
			PersistentData.m_objToData.Add(typeof(FrictionJoint2D), typeof(PersistentFrictionJoint2D));
			PersistentData.m_objToData.Add(typeof(HingeJoint2D), typeof(PersistentHingeJoint2D));
			PersistentData.m_objToData.Add(typeof(RelativeJoint2D), typeof(PersistentRelativeJoint2D));
			PersistentData.m_objToData.Add(typeof(SliderJoint2D), typeof(PersistentSliderJoint2D));
			PersistentData.m_objToData.Add(typeof(TargetJoint2D), typeof(PersistentTargetJoint2D));
			PersistentData.m_objToData.Add(typeof(FixedJoint2D), typeof(PersistentFixedJoint2D));
			PersistentData.m_objToData.Add(typeof(WheelJoint2D), typeof(PersistentWheelJoint2D));
			PersistentData.m_objToData.Add(typeof(PhysicsMaterial2D), typeof(PersistentPhysicsMaterial2D));
			PersistentData.m_objToData.Add(typeof(PhysicsUpdateBehaviour2D), typeof(PersistentPhysicsUpdateBehaviour2D));
			PersistentData.m_objToData.Add(typeof(ConstantForce2D), typeof(PersistentConstantForce2D));
			PersistentData.m_objToData.Add(typeof(Effector2D), typeof(PersistentEffector2D));
			PersistentData.m_objToData.Add(typeof(BuoyancyEffector2D), typeof(PersistentBuoyancyEffector2D));
			PersistentData.m_objToData.Add(typeof(PointEffector2D), typeof(PersistentPointEffector2D));
			PersistentData.m_objToData.Add(typeof(SurfaceEffector2D), typeof(PersistentSurfaceEffector2D));
			PersistentData.m_objToData.Add(typeof(WheelCollider), typeof(PersistentWheelCollider));
			PersistentData.m_objToData.Add(typeof(Cloth), typeof(PersistentCloth));
			PersistentData.m_objToData.Add(typeof(NavMeshData), typeof(PersistentNavMeshData));
			PersistentData.m_objToData.Add(typeof(NavMeshAgent), typeof(PersistentNavMeshAgent));
			PersistentData.m_objToData.Add(typeof(NavMeshObstacle), typeof(PersistentNavMeshObstacle));
			PersistentData.m_objToData.Add(typeof(OffMeshLink), typeof(PersistentOffMeshLink));
			PersistentData.m_objToData.Add(typeof(AudioSource), typeof(PersistentAudioSource));
			PersistentData.m_objToData.Add(typeof(AudioLowPassFilter), typeof(PersistentAudioLowPassFilter));
			PersistentData.m_objToData.Add(typeof(AudioHighPassFilter), typeof(PersistentAudioHighPassFilter));
			PersistentData.m_objToData.Add(typeof(AudioReverbFilter), typeof(PersistentAudioReverbFilter));
			PersistentData.m_objToData.Add(typeof(AudioClip), typeof(PersistentAudioClip));
			PersistentData.m_objToData.Add(typeof(AudioBehaviour), typeof(PersistentAudioBehaviour));
			PersistentData.m_objToData.Add(typeof(AudioListener), typeof(PersistentAudioListener));
			PersistentData.m_objToData.Add(typeof(AudioReverbZone), typeof(PersistentAudioReverbZone));
			PersistentData.m_objToData.Add(typeof(AudioDistortionFilter), typeof(PersistentAudioDistortionFilter));
			PersistentData.m_objToData.Add(typeof(AudioEchoFilter), typeof(PersistentAudioEchoFilter));
			PersistentData.m_objToData.Add(typeof(AudioChorusFilter), typeof(PersistentAudioChorusFilter));
			PersistentData.m_objToData.Add(typeof(AudioMixer), typeof(PersistentAudioMixer));
			PersistentData.m_objToData.Add(typeof(AudioMixerSnapshot), typeof(PersistentAudioMixerSnapshot));
			PersistentData.m_objToData.Add(typeof(AudioMixerGroup), typeof(PersistentAudioMixerGroup));
			PersistentData.m_objToData.Add(typeof(MovieTexture), typeof(PersistentMovieTexture));
			PersistentData.m_objToData.Add(typeof(WebCamTexture), typeof(PersistentWebCamTexture));
			PersistentData.m_objToData.Add(typeof(Animator), typeof(PersistentAnimator));
			PersistentData.m_objToData.Add(typeof(StateMachineBehaviour), typeof(PersistentStateMachineBehaviour));
			PersistentData.m_objToData.Add(typeof(AnimatorOverrideController), typeof(PersistentAnimatorOverrideController));
			PersistentData.m_objToData.Add(typeof(AnimationClip), typeof(PersistentAnimationClip));
			PersistentData.m_objToData.Add(typeof(Animation), typeof(PersistentAnimation));
			PersistentData.m_objToData.Add(typeof(RuntimeAnimatorController), typeof(PersistentRuntimeAnimatorController));
			PersistentData.m_objToData.Add(typeof(Avatar), typeof(PersistentAvatar));
			PersistentData.m_objToData.Add(typeof(AvatarMask), typeof(PersistentAvatarMask));
			PersistentData.m_objToData.Add(typeof(Motion), typeof(PersistentMotion));
			PersistentData.m_objToData.Add(typeof(TerrainData), typeof(PersistentTerrainData));
			PersistentData.m_objToData.Add(typeof(Terrain), typeof(PersistentTerrain));
			PersistentData.m_objToData.Add(typeof(Tree), typeof(PersistentTree));
			PersistentData.m_objToData.Add(typeof(TextMesh), typeof(PersistentTextMesh));
			PersistentData.m_objToData.Add(typeof(Font), typeof(PersistentFont));
			PersistentData.m_objToData.Add(typeof(Canvas), typeof(PersistentCanvas));
			PersistentData.m_objToData.Add(typeof(CanvasGroup), typeof(PersistentCanvasGroup));
			PersistentData.m_objToData.Add(typeof(CanvasRenderer), typeof(PersistentCanvasRenderer));
			PersistentData.m_objToData.Add(typeof(TerrainCollider), typeof(PersistentTerrainCollider));
			PersistentData.m_objToData.Add(typeof(GUISkin), typeof(PersistentGUISkin));
			PersistentData.m_objToData.Add(typeof(NetworkMatch), typeof(PersistentNetworkMatch));
			PersistentData.m_objToData.Add(typeof(VideoPlayer), typeof(PersistentVideoPlayer));
			PersistentData.m_objToData.Add(typeof(VideoClip), typeof(PersistentVideoClip));
			PersistentData.m_objToData.Add(typeof(PlayerConnection), typeof(PersistentPlayerConnection));
			PersistentData.m_objToData.Add(typeof(RenderPipelineAsset), typeof(PersistentRenderPipelineAsset));
			PersistentData.m_objToData.Add(typeof(EventSystem), typeof(PersistentEventSystem));
			PersistentData.m_objToData.Add(typeof(EventTrigger), typeof(PersistentEventTrigger));
			PersistentData.m_objToData.Add(typeof(UIBehaviour), typeof(PersistentUIBehaviour));
			PersistentData.m_objToData.Add(typeof(BaseInput), typeof(PersistentBaseInput));
			PersistentData.m_objToData.Add(typeof(BaseInputModule), typeof(PersistentBaseInputModule));
			PersistentData.m_objToData.Add(typeof(PointerInputModule), typeof(PersistentPointerInputModule));
			PersistentData.m_objToData.Add(typeof(StandaloneInputModule), typeof(PersistentStandaloneInputModule));
			PersistentData.m_objToData.Add(typeof(BaseRaycaster), typeof(PersistentBaseRaycaster));
			PersistentData.m_objToData.Add(typeof(Physics2DRaycaster), typeof(PersistentPhysics2DRaycaster));
			PersistentData.m_objToData.Add(typeof(PhysicsRaycaster), typeof(PersistentPhysicsRaycaster));
			PersistentData.m_objToData.Add(typeof(Button), typeof(PersistentButton));
			PersistentData.m_objToData.Add(typeof(Dropdown), typeof(PersistentDropdown));
			PersistentData.m_objToData.Add(typeof(Graphic), typeof(PersistentGraphic));
			PersistentData.m_objToData.Add(typeof(GraphicRaycaster), typeof(PersistentGraphicRaycaster));
			PersistentData.m_objToData.Add(typeof(Image), typeof(PersistentImage));
			PersistentData.m_objToData.Add(typeof(InputField), typeof(PersistentInputField));
			PersistentData.m_objToData.Add(typeof(Mask), typeof(PersistentMask));
			PersistentData.m_objToData.Add(typeof(MaskableGraphic), typeof(PersistentMaskableGraphic));
			PersistentData.m_objToData.Add(typeof(RawImage), typeof(PersistentRawImage));
			PersistentData.m_objToData.Add(typeof(RectMask2D), typeof(PersistentRectMask2D));
			PersistentData.m_objToData.Add(typeof(Scrollbar), typeof(PersistentScrollbar));
			PersistentData.m_objToData.Add(typeof(ScrollRect), typeof(PersistentScrollRect));
			PersistentData.m_objToData.Add(typeof(Selectable), typeof(PersistentSelectable));
			PersistentData.m_objToData.Add(typeof(Slider), typeof(PersistentSlider));
			PersistentData.m_objToData.Add(typeof(Text), typeof(PersistentText));
			PersistentData.m_objToData.Add(typeof(Toggle), typeof(PersistentToggle));
			PersistentData.m_objToData.Add(typeof(ToggleGroup), typeof(PersistentToggleGroup));
			PersistentData.m_objToData.Add(typeof(AspectRatioFitter), typeof(PersistentAspectRatioFitter));
			PersistentData.m_objToData.Add(typeof(CanvasScaler), typeof(PersistentCanvasScaler));
			PersistentData.m_objToData.Add(typeof(ContentSizeFitter), typeof(PersistentContentSizeFitter));
			PersistentData.m_objToData.Add(typeof(GridLayoutGroup), typeof(PersistentGridLayoutGroup));
			PersistentData.m_objToData.Add(typeof(HorizontalLayoutGroup), typeof(PersistentHorizontalLayoutGroup));
			PersistentData.m_objToData.Add(typeof(HorizontalOrVerticalLayoutGroup), typeof(PersistentHorizontalOrVerticalLayoutGroup));
			PersistentData.m_objToData.Add(typeof(LayoutElement), typeof(PersistentLayoutElement));
			PersistentData.m_objToData.Add(typeof(LayoutGroup), typeof(PersistentLayoutGroup));
			PersistentData.m_objToData.Add(typeof(VerticalLayoutGroup), typeof(PersistentVerticalLayoutGroup));
			PersistentData.m_objToData.Add(typeof(BaseMeshEffect), typeof(PersistentBaseMeshEffect));
			PersistentData.m_objToData.Add(typeof(Outline), typeof(PersistentOutline));
			PersistentData.m_objToData.Add(typeof(PositionAsUV1), typeof(PersistentPositionAsUV1));
			PersistentData.m_objToData.Add(typeof(Shadow), typeof(PersistentShadow));
			PersistentData.m_objToData.Add(typeof(UnityEngine.Object), typeof(PersistentObject));
			PersistentData.m_objToData.Add(typeof(GUIStyle), typeof(PersistentGUIStyle));
			PersistentData.m_objToData.Add(typeof(GUIStyleState), typeof(PersistentGUIStyleState));
			PersistentData.m_objToData.Add(typeof(DetailPrototype), typeof(PersistentDetailPrototype));
			PersistentData.m_objToData.Add(typeof(TreePrototype), typeof(PersistentTreePrototype));
			PersistentData.m_objToData.Add(typeof(SplatPrototype), typeof(PersistentSplatPrototype));
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002C59D File Offset: 0x0002A99D
		protected PersistentData()
		{
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0002C5A5 File Offset: 0x0002A9A5
		public PersistentObject AsPersistentObject
		{
			get
			{
				return this as PersistentObject;
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002C5B0 File Offset: 0x0002A9B0
		public virtual void ReadFrom(object obj)
		{
			UnityEngine.Object @object = obj as UnityEngine.Object;
			if (@object != null)
			{
				this.InstanceId = @object.GetMappedInstanceID();
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002C5DC File Offset: 0x0002A9DC
		public void GetDependencies(object obj, Dictionary<long, UnityEngine.Object> dependencies)
		{
			this.GetDependencies(dependencies, obj);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002C5E6 File Offset: 0x0002A9E6
		protected virtual void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002C5E8 File Offset: 0x0002A9E8
		protected void AddDependencies(UnityEngine.Object[] objs, Dictionary<long, UnityEngine.Object> dependencies)
		{
			foreach (UnityEngine.Object obj in objs)
			{
				this.AddDependency(obj, dependencies);
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002C618 File Offset: 0x0002AA18
		protected void AddDependency(UnityEngine.Object obj, Dictionary<long, UnityEngine.Object> dependencies)
		{
			if (obj == null)
			{
				return;
			}
			long mappedInstanceID = obj.GetMappedInstanceID();
			if (!dependencies.ContainsKey(mappedInstanceID))
			{
				dependencies.Add(mappedInstanceID, obj);
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002C650 File Offset: 0x0002AA50
		protected void GetDependencies<T, V>(T[] dst, V[] src, Dictionary<long, UnityEngine.Object> dependencies) where T : PersistentData, new()
		{
			if (src == null)
			{
				return;
			}
			if (dst == null)
			{
				dst = new T[src.Length];
			}
			if (dst.Length != src.Length)
			{
				Array.Resize<T>(ref dst, src.Length);
			}
			for (int i = 0; i < src.Length; i++)
			{
				this.GetDependencies<T>(dst[i], src[i], dependencies);
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002C6B7 File Offset: 0x0002AAB7
		protected void GetDependencies<T>(T dst, object obj, Dictionary<long, UnityEngine.Object> dependencies) where T : PersistentData, new()
		{
			if (obj == null)
			{
				return;
			}
			if (dst == null)
			{
				dst = Activator.CreateInstance<T>();
			}
			dst.GetDependencies(dependencies, obj);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002C6E1 File Offset: 0x0002AAE1
		public virtual void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002C6E4 File Offset: 0x0002AAE4
		protected void AddDependencies<T>(long[] ids, Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			T[] array = this.Resolve<T, T>(ids, objects);
			for (int i = 0; i < ids.Length; i++)
			{
				T t = array[i];
				if (t != null || allowNulls)
				{
					long key = ids[i];
					if (!dependencies.ContainsKey(key))
					{
						dependencies.Add(key, t);
					}
				}
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002C740 File Offset: 0x0002AB40
		protected void AddDependency<T>(long id, Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			T t = objects.Get(id);
			if ((t != null || allowNulls) && !dependencies.ContainsKey(id))
			{
				dependencies.Add(id, t);
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002C77B File Offset: 0x0002AB7B
		protected void FindDependencies<T, V>(V data, Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls) where V : PersistentData
		{
			if (data == null)
			{
				return;
			}
			data.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002C79C File Offset: 0x0002AB9C
		protected void FindDependencies<T, V>(V[] data, Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls) where V : PersistentData
		{
			if (data == null)
			{
				return;
			}
			for (int i = 0; i < data.Length; i++)
			{
				this.FindDependencies<T, V>(data[i], dependencies, objects, allowNulls);
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002C7D8 File Offset: 0x0002ABD8
		public virtual object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			if (obj is UnityEngine.Object)
			{
				UnityEngine.Object x = (UnityEngine.Object)obj;
				if (x == null)
				{
					return null;
				}
			}
			return obj;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002C808 File Offset: 0x0002AC08
		protected T[] Read<T, V>(T[] dst, V[] src) where T : PersistentData, new()
		{
			if (src == null)
			{
				return null;
			}
			if (dst == null)
			{
				dst = new T[src.Length];
			}
			if (dst.Length != src.Length)
			{
				Array.Resize<T>(ref dst, src.Length);
			}
			for (int i = 0; i < dst.Length; i++)
			{
				dst[i] = this.Read<T>(dst[i], src[i]);
			}
			return dst;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0002C877 File Offset: 0x0002AC77
		protected T Read<T>(T dst, object src) where T : PersistentData, new()
		{
			if (src == null)
			{
				return (T)((object)null);
			}
			if (dst == null)
			{
				dst = Activator.CreateInstance<T>();
			}
			dst.ReadFrom(src);
			return dst;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0002C8A7 File Offset: 0x0002ACA7
		protected PersistentUnityEventBase Read(PersistentUnityEventBase dst, object src)
		{
			if (src == null)
			{
				return null;
			}
			if (dst == null)
			{
				dst = new PersistentUnityEventBase();
			}
			dst.ReadFrom((UnityEventBase)src);
			return dst;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0002C8CC File Offset: 0x0002ACCC
		protected T[] Write<T>(T[] dst, PersistentData[] src, Dictionary<long, UnityEngine.Object> objects)
		{
			if (src == null)
			{
				return null;
			}
			if (dst == null)
			{
				dst = new T[src.Length];
			}
			if (dst.Length != src.Length)
			{
				Array.Resize<T>(ref dst, src.Length);
			}
			for (int i = 0; i < dst.Length; i++)
			{
				dst[i] = this.Write<T>(dst[i], src[i], objects);
			}
			return dst;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002C934 File Offset: 0x0002AD34
		protected T Write<T>(T dst, PersistentUnityEventBase src, Dictionary<long, UnityEngine.Object> objects) where T : UnityEventBase
		{
			if (src == null)
			{
				return (T)((object)null);
			}
			if (dst == null)
			{
				try
				{
					dst = Activator.CreateInstance<T>();
				}
				catch (MissingMethodException)
				{
					Debug.LogWarningFormat("Unable to instantiate object. {0} default constructor missing", new object[]
					{
						typeof(T).FullName
					});
				}
			}
			src.WriteTo(dst, objects);
			return dst;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002C9AC File Offset: 0x0002ADAC
		protected T Write<T>(T dst, PersistentData src, Dictionary<long, UnityEngine.Object> objects)
		{
			if (src == null)
			{
				return default(T);
			}
			if (dst == null)
			{
				try
				{
					dst = Activator.CreateInstance<T>();
				}
				catch (MissingMethodException)
				{
					Debug.LogWarningFormat("Unable to instantiate object. {0} default constructor missing", new object[]
					{
						typeof(T).FullName
					});
				}
			}
			return (T)((object)src.WriteTo(dst, objects));
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002CA2C File Offset: 0x0002AE2C
		protected T2[] Resolve<T2, T1>(long[] ids, Dictionary<long, T1> objects) where T2 : T1
		{
			T2[] array = new T2[ids.Length];
			for (int i = 0; i < ids.Length; i++)
			{
				array[i] = (T2)((object)objects.Get(ids[i]));
			}
			return array;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002CA74 File Offset: 0x0002AE74
		public static bool CanCreate(object obj)
		{
			Type type = obj.GetType();
			if (PersistentData.m_objToData.ContainsKey(type))
			{
				return PersistentData.m_objToData.ContainsKey(type);
			}
			if (type.IsScript())
			{
				do
				{
					type = type.BaseType();
				}
				while (type != null && !PersistentData.m_objToData.ContainsKey(type));
				return true;
			}
			return false;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002CAD0 File Offset: 0x0002AED0
		public static PersistentData Create(object obj)
		{
			Type type = obj.GetType();
			if (PersistentData.m_objToData.ContainsKey(type))
			{
				return (PersistentData)Activator.CreateInstance(PersistentData.m_objToData[type]);
			}
			if (type.IsScript())
			{
				do
				{
					type = type.BaseType();
				}
				while (type != null && !PersistentData.m_objToData.ContainsKey(type));
				PersistentData baseObjData = null;
				if (type != null)
				{
					baseObjData = (PersistentData)Activator.CreateInstance(PersistentData.m_objToData[type]);
				}
				return new PersistentScript(baseObjData);
			}
			Debug.Log(string.Format("there is no persistent data object for {0}", type));
			return null;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002CB68 File Offset: 0x0002AF68
		public static void RestoreDataAndResolveDependencies(PersistentData[] dataObjects, Dictionary<long, UnityEngine.Object> objects)
		{
			Dictionary<UnityEngine.Object, PersistentData> dictionary = new Dictionary<UnityEngine.Object, PersistentData>();
			foreach (PersistentData persistentData in dataObjects)
			{
				UnityEngine.Object key;
				if (objects.TryGetValue(persistentData.InstanceId, out key))
				{
					dictionary.Add(key, persistentData);
				}
			}
			foreach (KeyValuePair<UnityEngine.Object, PersistentData> keyValuePair in dictionary)
			{
				PersistentIgnore persistentIgnore = keyValuePair.Key as PersistentIgnore;
				if (!(persistentIgnore == null))
				{
					GameObject gameObject = persistentIgnore.gameObject;
					PersistentData persistentData2 = dictionary[gameObject];
					PersistentData value = keyValuePair.Value;
					PersistentData persistentData3 = dictionary[gameObject.transform];
					persistentData2.WriteTo(gameObject, objects);
					value.WriteTo(persistentIgnore, objects);
					persistentData3.WriteTo(gameObject.transform, objects);
				}
			}
			List<GameObject> list = new List<GameObject>();
			List<bool> list2 = new List<bool>();
			foreach (PersistentData persistentData4 in dataObjects)
			{
				if (!objects.ContainsKey(persistentData4.InstanceId))
				{
					Debug.LogWarningFormat("objects does not have object with instance id {0} however PersistentData of type {1} is present", new object[]
					{
						persistentData4.InstanceId,
						persistentData4.GetType()
					});
				}
				else
				{
					UnityEngine.Object @object = objects[persistentData4.InstanceId];
					persistentData4.WriteTo(@object, objects);
					if (@object is GameObject)
					{
						list.Add((GameObject)@object);
						list2.Add(persistentData4.ActiveSelf);
					}
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				bool active = list2[k];
				GameObject gameObject2 = list[k];
				gameObject2.SetActive(active);
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002CD50 File Offset: 0x0002B150
		public static void CreatePersistentDescriptorsAndData(GameObject[] gameObjects, out PersistentDescriptor[] descriptors, out PersistentData[] data)
		{
			List<PersistentData> list = new List<PersistentData>();
			List<PersistentDescriptor> list2 = new List<PersistentDescriptor>();
			foreach (GameObject go in gameObjects)
			{
				PersistentDescriptor persistentDescriptor = PersistentDescriptor.CreateDescriptor(go, null);
				if (persistentDescriptor != null)
				{
					list2.Add(persistentDescriptor);
				}
				PersistentData.CreatePersistentData(go, list);
			}
			descriptors = list2.ToArray();
			data = list.ToArray();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002CDB0 File Offset: 0x0002B1B0
		public static void CreatePersistentDescriptorsAndData(GameObject[] gameObjects, out PersistentDescriptor[] descriptors, out PersistentData[][] data)
		{
			List<PersistentData[]> list = new List<PersistentData[]>();
			List<PersistentDescriptor> list2 = new List<PersistentDescriptor>();
			foreach (GameObject go in gameObjects)
			{
				PersistentDescriptor persistentDescriptor = PersistentDescriptor.CreateDescriptor(go, null);
				if (persistentDescriptor != null)
				{
					list2.Add(persistentDescriptor);
				}
				List<PersistentData> list3 = new List<PersistentData>();
				PersistentData.CreatePersistentData(go, list3);
				list.Add(list3.ToArray());
			}
			descriptors = list2.ToArray();
			data = list.ToArray();
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002CE28 File Offset: 0x0002B228
		public static PersistentData[] CreatePersistentData(UnityEngine.Object[] objects)
		{
			List<PersistentData> list = new List<PersistentData>();
			foreach (UnityEngine.Object @object in objects)
			{
				if (!(@object == null))
				{
					PersistentData persistentData = PersistentData.Create(@object);
					if (persistentData != null)
					{
						persistentData.ReadFrom(@object);
						list.Add(persistentData);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002CE8C File Offset: 0x0002B28C
		private static void CreatePersistentData(GameObject go, List<PersistentData> data)
		{
			PersistentIgnore component = go.GetComponent<PersistentIgnore>();
			if (component != null)
			{
				return;
			}
			PersistentData persistentData = PersistentData.Create(go);
			if (persistentData != null)
			{
				persistentData.ActiveSelf = go.activeSelf;
				persistentData.ReadFrom(go);
				data.Add(persistentData);
			}
			Component[] array;
			if (component == null)
			{
				IEnumerable<Component> components = go.GetComponents<Component>();
				if (PersistentData.<>f__am$cache0 == null)
				{
					PersistentData.<>f__am$cache0 = new Func<Component, bool>(PersistentData.<CreatePersistentData>m__0);
				}
				array = components.Where(PersistentData.<>f__am$cache0).ToArray<Component>();
			}
			else
			{
				array = go.GetComponents<Transform>();
				Array.Resize<Component>(ref array, array.Length + 1);
				array[array.Length - 1] = component;
			}
			foreach (Component obj in array)
			{
				PersistentData persistentData2 = PersistentData.Create(obj);
				if (persistentData2 != null)
				{
					persistentData2.ReadFrom(obj);
					data.Add(persistentData2);
				}
			}
			Transform transform = go.transform;
			IEnumerator enumerator = transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj2 = enumerator.Current;
					Transform transform2 = (Transform)obj2;
					if (component == null)
					{
						PersistentData.CreatePersistentData(transform2.gameObject, data);
					}
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

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002CFE0 File Offset: 0x0002B3E0
		public static void RegisterPersistentType<T, TPersistent>() where TPersistent : PersistentData
		{
			PersistentData.m_objToData.Add(typeof(T), typeof(TPersistent));
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0002D000 File Offset: 0x0002B400
		[CompilerGenerated]
		private static bool <CreatePersistentData>m__0(Component c)
		{
			return c != null && !PersistentDescriptor.IgnoreTypes.Contains(c.GetType());
		}

		// Token: 0x0400087B RID: 2171
		public const int USER_DEFINED_FIELD_TAG = 100000;

		// Token: 0x0400087C RID: 2172
		protected static readonly Dictionary<Type, Type> m_objToData = new Dictionary<Type, Type>();

		// Token: 0x0400087D RID: 2173
		public bool ActiveSelf;

		// Token: 0x0400087E RID: 2174
		public long InstanceId;

		// Token: 0x0400087F RID: 2175
		[CompilerGenerated]
		private static Func<Component, bool> <>f__am$cache0;
	}
}
