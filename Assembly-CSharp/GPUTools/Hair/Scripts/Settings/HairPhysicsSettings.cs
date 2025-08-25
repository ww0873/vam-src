using System;
using System.Collections.Generic;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
	// Token: 0x02000A29 RID: 2601
	[Serializable]
	public class HairPhysicsSettings : HairSettingsBase
	{
		// Token: 0x06004334 RID: 17204 RVA: 0x0013B834 File Offset: 0x00139C34
		public HairPhysicsSettings()
		{
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x0013B990 File Offset: 0x00139D90
		public List<SphereCollider> GetColliders()
		{
			List<SphereCollider> list = new List<SphereCollider>();
			foreach (GameObject gameObject in this.ColliderProviders)
			{
				list.AddRange(gameObject.GetComponents<SphereCollider>().ToList<SphereCollider>());
			}
			return list;
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x0013BA00 File Offset: 0x00139E00
		public override bool Validate()
		{
			return true;
		}

		// Token: 0x040031E8 RID: 12776
		public bool DebugDraw;

		// Token: 0x040031E9 RID: 12777
		public bool DebugDrawNearbyJoints;

		// Token: 0x040031EA RID: 12778
		public bool IsEnabled = true;

		// Token: 0x040031EB RID: 12779
		public int Iterations = 1;

		// Token: 0x040031EC RID: 12780
		public bool FastMovement = true;

		// Token: 0x040031ED RID: 12781
		public bool RunPhysicsOnUpdate;

		// Token: 0x040031EE RID: 12782
		public Vector3 Gravity = new Vector3(0f, -1f, 0f);

		// Token: 0x040031EF RID: 12783
		public float GravityMultiplier = 1f;

		// Token: 0x040031F0 RID: 12784
		public float Drag;

		// Token: 0x040031F1 RID: 12785
		public bool UseSeparateRootRadius;

		// Token: 0x040031F2 RID: 12786
		public float StandRootRadius = 0.01f;

		// Token: 0x040031F3 RID: 12787
		public float StandRadius = 0.01f;

		// Token: 0x040031F4 RID: 12788
		public float WorldScale = 1f;

		// Token: 0x040031F5 RID: 12789
		public bool StyleMode;

		// Token: 0x040031F6 RID: 12790
		public float StyleModeGravityMultiplier;

		// Token: 0x040031F7 RID: 12791
		public float StyleModeStrandRootRadius = 0.004f;

		// Token: 0x040031F8 RID: 12792
		public float StyleModeStrandRadius = 0.002f;

		// Token: 0x040031F9 RID: 12793
		public bool UsePaintedRigidity;

		// Token: 0x040031FA RID: 12794
		public float PaintedRigidityMultiplier = 1f;

		// Token: 0x040031FB RID: 12795
		public float RootRigidity = 0.1f;

		// Token: 0x040031FC RID: 12796
		public float MainRigidity = 0.02f;

		// Token: 0x040031FD RID: 12797
		public float TipRigidity;

		// Token: 0x040031FE RID: 12798
		public float RigidityRolloffPower = 8f;

		// Token: 0x040031FF RID: 12799
		public float JointRigidity = 0.1f;

		// Token: 0x04003200 RID: 12800
		public AnimationCurve ElasticityCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04003201 RID: 12801
		public float SplineJointPower = 0.2f;

		// Token: 0x04003202 RID: 12802
		public float ReverseSplineJointPower = 0.2f;

		// Token: 0x04003203 RID: 12803
		public float CompressionJointPower = 0.2f;

		// Token: 0x04003204 RID: 12804
		public float Weight = 1.5f;

		// Token: 0x04003205 RID: 12805
		public float Friction = 0.8f;

		// Token: 0x04003206 RID: 12806
		public float CollisionPower = 1f;

		// Token: 0x04003207 RID: 12807
		public float NearbyJointPower = 1f;

		// Token: 0x04003208 RID: 12808
		public float NearbyJointPowerRolloff;

		// Token: 0x04003209 RID: 12809
		public bool UpdateRigidityJointsBeforeRender;

		// Token: 0x0400320A RID: 12810
		public float WindMultiplier = 0.0001f;

		// Token: 0x0400320B RID: 12811
		public bool IsCollisionEnabled = true;

		// Token: 0x0400320C RID: 12812
		public List<GameObject> ColliderProviders = new List<GameObject>();

		// Token: 0x0400320D RID: 12813
		public List<GameObject> AccessoriesProviders = new List<GameObject>();

		// Token: 0x0400320E RID: 12814
		public List<HairJointArea> JointAreas = new List<HairJointArea>();

		// Token: 0x0400320F RID: 12815
		private List<SphereCollider> colliders;
	}
}
