using System;
using System.Runtime.CompilerServices;
using Leap.Unity.Query;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Leap.Unity
{
	// Token: 0x02000731 RID: 1841
	public static class Hands
	{
		// Token: 0x06002CC2 RID: 11458 RVA: 0x000F000C File Offset: 0x000EE40C
		static Hands()
		{
			Hands.InitStatic();
			if (Hands.<>f__mg$cache0 == null)
			{
				Hands.<>f__mg$cache0 = new UnityAction<Scene, Scene>(Hands.InitStaticOnNewScene);
			}
			SceneManager.activeSceneChanged += Hands.<>f__mg$cache0;
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000F0035 File Offset: 0x000EE435
		private static void InitStaticOnNewScene(Scene unused, Scene unused2)
		{
			Hands.InitStatic();
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000F003C File Offset: 0x000EE43C
		private static void InitStatic()
		{
			Hands.s_provider = UnityEngine.Object.FindObjectOfType<LeapServiceProvider>();
			if (Hands.s_provider == null)
			{
				Hands.s_provider = UnityEngine.Object.FindObjectOfType<LeapProvider>();
				if (Hands.s_provider == null)
				{
					return;
				}
			}
			Camera componentInParent = Hands.s_provider.GetComponentInParent<Camera>();
			if (componentInParent == null)
			{
				return;
			}
			if (componentInParent.transform.parent == null)
			{
				return;
			}
			Hands.s_leapRig = componentInParent.transform.parent.gameObject;
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002CC5 RID: 11461 RVA: 0x000F00C2 File Offset: 0x000EE4C2
		public static GameObject CameraRig
		{
			get
			{
				if (Hands.s_leapRig == null)
				{
					Hands.InitStatic();
				}
				return Hands.s_leapRig;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x000F00DE File Offset: 0x000EE4DE
		public static LeapProvider Provider
		{
			get
			{
				if (Hands.s_provider == null)
				{
					Hands.InitStatic();
				}
				return Hands.s_provider;
			}
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000F00FA File Offset: 0x000EE4FA
		public static Hand Get(Chirality chirality)
		{
			if (chirality == Chirality.Left)
			{
				return Hands.Left;
			}
			return Hands.Right;
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000F010D File Offset: 0x000EE50D
		public static Hand GetFixed(Chirality chirality)
		{
			if (chirality == Chirality.Left)
			{
				return Hands.FixedLeft;
			}
			return Hands.FixedRight;
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002CC9 RID: 11465 RVA: 0x000F0120 File Offset: 0x000EE520
		public static Hand Left
		{
			get
			{
				if (Hands.Provider == null)
				{
					return null;
				}
				if (Hands.Provider.CurrentFrame == null)
				{
					return null;
				}
				Query<Hand> query = Hands.Provider.CurrentFrame.Hands.Query<Hand>();
				if (Hands.<>f__am$cache0 == null)
				{
					Hands.<>f__am$cache0 = new Func<Hand, bool>(Hands.<get_Left>m__0);
				}
				return query.FirstOrDefault(Hands.<>f__am$cache0);
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002CCA RID: 11466 RVA: 0x000F0188 File Offset: 0x000EE588
		public static Hand Right
		{
			get
			{
				if (Hands.Provider == null)
				{
					return null;
				}
				if (Hands.Provider.CurrentFrame == null)
				{
					return null;
				}
				Query<Hand> query = Hands.Provider.CurrentFrame.Hands.Query<Hand>();
				if (Hands.<>f__am$cache1 == null)
				{
					Hands.<>f__am$cache1 = new Func<Hand, bool>(Hands.<get_Right>m__1);
				}
				return query.FirstOrDefault(Hands.<>f__am$cache1);
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002CCB RID: 11467 RVA: 0x000F01F0 File Offset: 0x000EE5F0
		public static Hand FixedLeft
		{
			get
			{
				if (Hands.Provider == null)
				{
					return null;
				}
				if (Hands.Provider.CurrentFixedFrame == null)
				{
					return null;
				}
				Query<Hand> query = Hands.Provider.CurrentFixedFrame.Hands.Query<Hand>();
				if (Hands.<>f__am$cache2 == null)
				{
					Hands.<>f__am$cache2 = new Func<Hand, bool>(Hands.<get_FixedLeft>m__2);
				}
				return query.FirstOrDefault(Hands.<>f__am$cache2);
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002CCC RID: 11468 RVA: 0x000F0258 File Offset: 0x000EE658
		public static Hand FixedRight
		{
			get
			{
				if (Hands.Provider == null)
				{
					return null;
				}
				if (Hands.Provider.CurrentFixedFrame == null)
				{
					return null;
				}
				Query<Hand> query = Hands.Provider.CurrentFixedFrame.Hands.Query<Hand>();
				if (Hands.<>f__am$cache3 == null)
				{
					Hands.<>f__am$cache3 = new Func<Hand, bool>(Hands.<get_FixedRight>m__3);
				}
				return query.FirstOrDefault(Hands.<>f__am$cache3);
			}
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000F02BE File Offset: 0x000EE6BE
		public static Finger GetThumb(this Hand hand)
		{
			return hand.Fingers[0];
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000F02CC File Offset: 0x000EE6CC
		public static Finger GetIndex(this Hand hand)
		{
			return hand.Fingers[1];
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000F02DA File Offset: 0x000EE6DA
		public static Finger GetMiddle(this Hand hand)
		{
			return hand.Fingers[2];
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000F02E8 File Offset: 0x000EE6E8
		public static Finger GetRing(this Hand hand)
		{
			return hand.Fingers[3];
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000F02F6 File Offset: 0x000EE6F6
		public static Finger GetPinky(this Hand hand)
		{
			return hand.Fingers[4];
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000F0304 File Offset: 0x000EE704
		public static Pose GetPalmPose(this Hand hand)
		{
			return new Pose(hand.PalmPosition.ToVector3(), hand.Rotation.ToQuaternion());
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000F0321 File Offset: 0x000EE721
		public static void SetPalmPose(this Hand hand, Pose newPalmPose)
		{
			hand.SetTransform(newPalmPose.position, newPalmPose.rotation);
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000F0338 File Offset: 0x000EE738
		public static Vector3 PalmarAxis(this Hand hand)
		{
			return -hand.Basis.yBasis.ToVector3();
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000F0360 File Offset: 0x000EE760
		public static Vector3 RadialAxis(this Hand hand)
		{
			if (hand.IsRight)
			{
				return -hand.Basis.xBasis.ToVector3();
			}
			return hand.Basis.xBasis.ToVector3();
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000F03A4 File Offset: 0x000EE7A4
		public static Vector3 DistalAxis(this Hand hand)
		{
			return hand.Basis.zBasis.ToVector3();
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000F03C4 File Offset: 0x000EE7C4
		public static bool IsPinching(this Hand hand)
		{
			return hand.PinchStrength > 0.8f;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000F03D4 File Offset: 0x000EE7D4
		public static Vector3 GetPinchPosition(this Hand hand)
		{
			Vector tipPosition = hand.Fingers[1].TipPosition;
			Vector tipPosition2 = hand.Fingers[0].TipPosition;
			return (2f * tipPosition2 + tipPosition).ToVector3() * 0.333333f;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000F0428 File Offset: 0x000EE828
		public static Vector3 GetPredictedPinchPosition(this Hand hand)
		{
			Vector3 b = hand.GetIndex().TipPosition.ToVector3();
			Vector3 vector = hand.GetThumb().TipPosition.ToVector3();
			Vector3 vector2 = hand.Fingers[1].bones[1].PrevJoint.ToVector3();
			float length = hand.Fingers[1].Length;
			Vector3 vector3 = hand.RadialAxis();
			float t = Vector3.Dot((vector - vector2).normalized, vector3).Map(0f, 1f, 0.5f, 0f);
			Vector3 a = vector2 + hand.PalmarAxis() * length * 0.85f + hand.DistalAxis() * length * 0.2f + vector3 * length * 0.2f;
			a = Vector3.Lerp(a, vector, t);
			return Vector3.Lerp(a, b, 0.15f);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000F0530 File Offset: 0x000EE930
		public static bool IsFacing(this Vector3 facingVector, Vector3 fromWorldPosition, Vector3 towardsWorldPosition, float maxOffsetAngleAllowed)
		{
			Vector3 normalized = (towardsWorldPosition - fromWorldPosition).normalized;
			return Vector3.Angle(facingVector, normalized) <= maxOffsetAngleAllowed;
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000F055C File Offset: 0x000EE95C
		public static float GetFistStrength(this Hand hand)
		{
			return (Vector3.Dot(hand.Fingers[1].Direction.ToVector3(), -hand.DistalAxis()) + Vector3.Dot(hand.Fingers[2].Direction.ToVector3(), -hand.DistalAxis()) + Vector3.Dot(hand.Fingers[3].Direction.ToVector3(), -hand.DistalAxis()) + Vector3.Dot(hand.Fingers[4].Direction.ToVector3(), -hand.DistalAxis()) + Vector3.Dot(hand.Fingers[0].Direction.ToVector3(), -hand.RadialAxis())).Map(-5f, 5f, 0f, 1f);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000F0644 File Offset: 0x000EEA44
		public static void Transform(this Bone bone, Vector3 position, Quaternion rotation)
		{
			bone.Transform(new LeapTransform(position.ToVector(), rotation.ToLeapQuaternion()));
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000F065E File Offset: 0x000EEA5E
		public static void Transform(this Finger finger, Vector3 position, Quaternion rotation)
		{
			finger.Transform(new LeapTransform(position.ToVector(), rotation.ToLeapQuaternion()));
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000F0678 File Offset: 0x000EEA78
		public static void Transform(this Hand hand, Vector3 position, Quaternion rotation)
		{
			hand.Transform(new LeapTransform(position.ToVector(), rotation.ToLeapQuaternion()));
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000F0692 File Offset: 0x000EEA92
		public static void Transform(this Frame frame, Vector3 position, Quaternion rotation)
		{
			frame.Transform(new LeapTransform(position.ToVector(), rotation.ToLeapQuaternion()));
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000F06AC File Offset: 0x000EEAAC
		public static void SetTransform(this Bone bone, Vector3 position, Quaternion rotation)
		{
			bone.Transform(Vector3.zero, rotation * Quaternion.Inverse(bone.Rotation.ToQuaternion()));
			bone.Transform(position - bone.PrevJoint.ToVector3(), Quaternion.identity);
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000F06EC File Offset: 0x000EEAEC
		public static void SetTipTransform(this Finger finger, Vector3 position, Quaternion rotation)
		{
			finger.Transform(Vector3.zero, rotation * Quaternion.Inverse(finger.bones[3].Rotation.ToQuaternion()));
			finger.Transform(position - finger.bones[3].NextJoint.ToVector3(), Quaternion.identity);
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000F0744 File Offset: 0x000EEB44
		public static void SetTransform(this Hand hand, Vector3 position, Quaternion rotation)
		{
			hand.Transform(Vector3.zero, Quaternion.Slerp(rotation * Quaternion.Inverse(hand.Rotation.ToQuaternion()), Quaternion.identity, 0f));
			hand.Transform(position - hand.PalmPosition.ToVector3(), Quaternion.identity);
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000F079D File Offset: 0x000EEB9D
		[CompilerGenerated]
		private static bool <get_Left>m__0(Hand hand)
		{
			return hand.IsLeft;
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000F07A5 File Offset: 0x000EEBA5
		[CompilerGenerated]
		private static bool <get_Right>m__1(Hand hand)
		{
			return hand.IsRight;
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000F07AD File Offset: 0x000EEBAD
		[CompilerGenerated]
		private static bool <get_FixedLeft>m__2(Hand hand)
		{
			return hand.IsLeft;
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000F07B5 File Offset: 0x000EEBB5
		[CompilerGenerated]
		private static bool <get_FixedRight>m__3(Hand hand)
		{
			return hand.IsRight;
		}

		// Token: 0x040023B2 RID: 9138
		private static LeapProvider s_provider;

		// Token: 0x040023B3 RID: 9139
		private static GameObject s_leapRig;

		// Token: 0x040023B4 RID: 9140
		[CompilerGenerated]
		private static UnityAction<Scene, Scene> <>f__mg$cache0;

		// Token: 0x040023B5 RID: 9141
		[CompilerGenerated]
		private static Func<Hand, bool> <>f__am$cache0;

		// Token: 0x040023B6 RID: 9142
		[CompilerGenerated]
		private static Func<Hand, bool> <>f__am$cache1;

		// Token: 0x040023B7 RID: 9143
		[CompilerGenerated]
		private static Func<Hand, bool> <>f__am$cache2;

		// Token: 0x040023B8 RID: 9144
		[CompilerGenerated]
		private static Func<Hand, bool> <>f__am$cache3;
	}
}
