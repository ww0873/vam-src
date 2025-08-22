using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Battlehub.RTSaveLoad.UnityEngineNS;
using Battlehub.RTSaveLoad.UnityEngineNS.AINS;
using Battlehub.RTSaveLoad.UnityEngineNS.ExperimentalNS.DirectorNS;
using Battlehub.RTSaveLoad.UnityEngineNS.SceneManagementNS;
using Battlehub.RTSaveLoad.UnityEngineNS.UINS;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x020001DB RID: 475
	public static class SerializationSurrogates
	{
		// Token: 0x0600098F RID: 2447 RVA: 0x0003AF7C File Offset: 0x0003937C
		static SerializationSurrogates()
		{
			SerializationSurrogates.m_surrogates.Add(typeof(GradientAlphaKey), new GradientAlphaKeySurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(GradientColorKey), new GradientColorKeySurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(LayerMask), new LayerMaskSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(RectOffset), new RectOffsetSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(AnimationTriggers), new AnimationTriggersSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(ColorBlock), new ColorBlockSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(NavMeshPath), new NavMeshPathSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(ClothSkinningCoefficient), new ClothSkinningCoefficientSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(BoneWeight), new BoneWeightSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(TreeInstance), new TreeInstanceSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(CharacterInfo), new CharacterInfoSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Vector3), new Vector3Surrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Color), new ColorSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Rect), new RectSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Matrix4x4), new Matrix4x4Surrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Scene), new SceneSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Bounds), new BoundsSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Vector4), new Vector4Surrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Vector2), new Vector2Surrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(RenderBuffer), new RenderBufferSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Quaternion), new QuaternionSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointMotor), new JointMotorSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointLimits), new JointLimitsSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointSpring), new JointSpringSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointDrive), new JointDriveSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(SoftJointLimitSpring), new SoftJointLimitSpringSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(SoftJointLimit), new SoftJointLimitSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointMotor2D), new JointMotor2DSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointAngleLimits2D), new JointAngleLimits2DSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointTranslationLimits2D), new JointTranslationLimits2DSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(JointSuspension2D), new JointSuspension2DSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(WheelFrictionCurve), new WheelFrictionCurveSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(OffMeshLinkData), new OffMeshLinkDataSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(PlayableGraph), new PlayableGraphSurrogate());
			SerializationSurrogates.m_surrogates.Add(typeof(Color32), new Color32Surrogate());
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0003B300 File Offset: 0x00039700
		public static SurrogateSelector CreateSelector()
		{
			SurrogateSelector surrogateSelector = new SurrogateSelector();
			foreach (KeyValuePair<Type, ISerializationSurrogate> keyValuePair in SerializationSurrogates.m_surrogates)
			{
				surrogateSelector.AddSurrogate(keyValuePair.Key, new StreamingContext(StreamingContextStates.All), keyValuePair.Value);
			}
			return surrogateSelector;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0003B37C File Offset: 0x0003977C
		public static Dictionary<Type, ISerializationSurrogate> GetSurrogates()
		{
			return SerializationSurrogates.m_surrogates;
		}

		// Token: 0x04000AD1 RID: 2769
		private static Dictionary<Type, ISerializationSurrogate> m_surrogates = new Dictionary<Type, ISerializationSurrogate>();
	}
}
