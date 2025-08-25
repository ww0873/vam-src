using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.AI;
using Battlehub.RTSaveLoad.PersistentObjects.Rendering;
using Battlehub.RTSaveLoad.PersistentObjects.Video;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000144 RID: 324
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1004, typeof(PersistentCamera))]
	[ProtoInclude(1005, typeof(PersistentFlareLayer))]
	[ProtoInclude(1006, typeof(PersistentLensFlare))]
	[ProtoInclude(1007, typeof(PersistentProjector))]
	[ProtoInclude(1008, typeof(PersistentSkybox))]
	[ProtoInclude(1009, typeof(PersistentGUIElement))]
	[ProtoInclude(1010, typeof(PersistentGUILayer))]
	[ProtoInclude(1011, typeof(PersistentLight))]
	[ProtoInclude(1012, typeof(PersistentLightProbeGroup))]
	[ProtoInclude(1013, typeof(PersistentLightProbeProxyVolume))]
	[ProtoInclude(1014, typeof(PersistentMonoBehaviour))]
	[ProtoInclude(1015, typeof(PersistentNetworkView))]
	[ProtoInclude(1016, typeof(PersistentReflectionProbe))]
	[ProtoInclude(1017, typeof(PersistentSortingGroup))]
	[ProtoInclude(1018, typeof(PersistentConstantForce))]
	[ProtoInclude(1019, typeof(PersistentJoint2D))]
	[ProtoInclude(1020, typeof(PersistentCollider2D))]
	[ProtoInclude(1021, typeof(PersistentPhysicsUpdateBehaviour2D))]
	[ProtoInclude(1022, typeof(PersistentEffector2D))]
	[ProtoInclude(1023, typeof(PersistentNavMeshAgent))]
	[ProtoInclude(1024, typeof(PersistentNavMeshObstacle))]
	[ProtoInclude(1025, typeof(PersistentOffMeshLink))]
	[ProtoInclude(1026, typeof(PersistentAudioSource))]
	[ProtoInclude(1027, typeof(PersistentAudioLowPassFilter))]
	[ProtoInclude(1028, typeof(PersistentAudioHighPassFilter))]
	[ProtoInclude(1029, typeof(PersistentAudioReverbFilter))]
	[ProtoInclude(1030, typeof(PersistentAudioBehaviour))]
	[ProtoInclude(1031, typeof(PersistentAudioListener))]
	[ProtoInclude(1032, typeof(PersistentAudioReverbZone))]
	[ProtoInclude(1033, typeof(PersistentAudioDistortionFilter))]
	[ProtoInclude(1034, typeof(PersistentAudioEchoFilter))]
	[ProtoInclude(1035, typeof(PersistentAudioChorusFilter))]
	[ProtoInclude(1036, typeof(PersistentAnimator))]
	[ProtoInclude(1037, typeof(PersistentAnimation))]
	[ProtoInclude(1038, typeof(PersistentTerrain))]
	[ProtoInclude(1039, typeof(PersistentCanvas))]
	[ProtoInclude(1040, typeof(PersistentVideoPlayer))]
	[Serializable]
	public class PersistentBehaviour : PersistentComponent
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x00030CE9 File Offset: 0x0002F0E9
		public PersistentBehaviour()
		{
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00030CF4 File Offset: 0x0002F0F4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Behaviour behaviour = (Behaviour)obj;
			behaviour.enabled = this.enabled;
			return behaviour;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00030D28 File Offset: 0x0002F128
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Behaviour behaviour = (Behaviour)obj;
			this.enabled = behaviour.enabled;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00030D56 File Offset: 0x0002F156
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040007D8 RID: 2008
		public bool enabled;
	}
}
