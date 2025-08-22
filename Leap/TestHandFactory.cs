using System;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine;

namespace Leap
{
	// Token: 0x020006F1 RID: 1777
	public static class TestHandFactory
	{
		// Token: 0x06002B0F RID: 11023 RVA: 0x000E85DC File Offset: 0x000E69DC
		public static Frame MakeTestFrame(int frameId, bool includeLeftHand = true, bool includeRightHand = true, TestHandFactory.TestHandPose handPose = TestHandFactory.TestHandPose.HeadMountedA, TestHandFactory.UnitType unitType = TestHandFactory.UnitType.LeapUnits)
		{
			Frame frame = new Frame((long)frameId, 0L, 120f, new List<Hand>());
			if (includeLeftHand)
			{
				frame.Hands.Add(TestHandFactory.MakeTestHand(true, handPose, frameId, 10, unitType));
			}
			if (includeRightHand)
			{
				frame.Hands.Add(TestHandFactory.MakeTestHand(false, handPose, frameId, 20, unitType));
			}
			return frame;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000E8638 File Offset: 0x000E6A38
		public static Hand MakeTestHand(bool isLeft, LeapTransform leftHandTransform, int frameId = 0, int handId = 0, TestHandFactory.UnitType unitType = TestHandFactory.UnitType.LeapUnits)
		{
			if (!isLeft)
			{
				leftHandTransform.translation = new Vector(-leftHandTransform.translation.x, leftHandTransform.translation.y, leftHandTransform.translation.z);
				leftHandTransform.rotation = new LeapQuaternion(-leftHandTransform.rotation.x, leftHandTransform.rotation.y, leftHandTransform.rotation.z, -leftHandTransform.rotation.w);
				leftHandTransform.MirrorX();
			}
			Hand hand = TestHandFactory.makeLeapSpaceTestHand(frameId, handId, isLeft).Transform(leftHandTransform);
			Quaternion quaternion = Quaternion.Euler(90f, 0f, 180f);
			LeapQuaternion rotation = new LeapQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
			Hand hand2 = hand.Transform(new LeapTransform(Vector.Zero, rotation));
			if (unitType == TestHandFactory.UnitType.UnityUnits)
			{
				hand2.TransformToUnityUnits();
			}
			return hand2;
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x000E8748 File Offset: 0x000E6B48
		public static Hand MakeTestHand(bool isLeft, int frameId = 0, int handId = 0, TestHandFactory.UnitType unitType = TestHandFactory.UnitType.LeapUnits)
		{
			return TestHandFactory.MakeTestHand(isLeft, LeapTransform.Identity, frameId, handId, unitType);
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000E8758 File Offset: 0x000E6B58
		public static Hand MakeTestHand(bool isLeft, TestHandFactory.TestHandPose pose, int frameId = 0, int handId = 0, TestHandFactory.UnitType unitType = TestHandFactory.UnitType.LeapUnits)
		{
			return TestHandFactory.MakeTestHand(isLeft, TestHandFactory.GetTestPoseLeftHandTransform(pose), frameId, handId, unitType);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000E876C File Offset: 0x000E6B6C
		public static LeapTransform GetTestPoseLeftHandTransform(TestHandFactory.TestHandPose pose)
		{
			LeapTransform identity = LeapTransform.Identity;
			if (pose != TestHandFactory.TestHandPose.HeadMountedA)
			{
				if (pose != TestHandFactory.TestHandPose.HeadMountedB)
				{
					if (pose == TestHandFactory.TestHandPose.DesktopModeA)
					{
						identity.rotation = TestHandFactory.angleAxis(0f, Vector.Forward).Multiply(TestHandFactory.angleAxis(-1.5707964f, Vector.Right)).Multiply(TestHandFactory.angleAxis(3.1415927f, Vector.Up));
						identity.translation = new Vector(120f, 0f, -170f);
					}
				}
				else
				{
					identity.rotation = Quaternion.Euler(30f, -10f, -20f).ToLeapQuaternion();
					identity.translation = new Vector(220f, 270f, 130f);
				}
			}
			else
			{
				identity.rotation = TestHandFactory.angleAxis(3.1415927f, Vector.Forward);
				identity.translation = new Vector(80f, 120f, 0f);
			}
			return identity;
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x000E8874 File Offset: 0x000E6C74
		private static Hand makeLeapSpaceTestHand(int frameId, int handId, bool isLeft)
		{
			List<Finger> list = new List<Finger>(5);
			list.Add(TestHandFactory.makeThumb(frameId, handId, isLeft));
			list.Add(TestHandFactory.makeIndexFinger(frameId, handId, isLeft));
			list.Add(TestHandFactory.makeMiddleFinger(frameId, handId, isLeft));
			list.Add(TestHandFactory.makeRingFinger(frameId, handId, isLeft));
			list.Add(TestHandFactory.makePinky(frameId, handId, isLeft));
			Vector vector = new Vector(-7.0580993f, 4f, 50f);
			Vector vector2 = vector + 250f * Vector.Backward;
			Arm arm = new Arm(vector2, vector, (vector2 + vector) / 2f, Vector.Forward, 250f, 41f, LeapQuaternion.Identity);
			return new Hand((long)frameId, handId, 1f, 0f, 0f, 0f, 0f, 85f, isLeft, 0f, arm, list, new Vector(0f, 0f, 0f), new Vector(0f, 0f, 0f), new Vector(0f, 0f, 0f), Vector.Down, LeapQuaternion.Identity, Vector.Forward, new Vector(-4.3638577f, 6.5f, 31.011135f));
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x000E89B8 File Offset: 0x000E6DB8
		private static LeapQuaternion angleAxis(float angle, Vector axis)
		{
			if (!axis.MagnitudeSquared.NearlyEquals(1f, 1.1920929E-07f))
			{
				throw new ArgumentException("Axis must be a unit vector.");
			}
			float num = Mathf.Sin(angle / 2f);
			LeapQuaternion leapQuaternion = new LeapQuaternion(num * axis.x, num * axis.y, num * axis.z, Mathf.Cos(angle / 2f));
			return leapQuaternion.Normalized;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000E8A30 File Offset: 0x000E6E30
		private static LeapQuaternion rotationBetween(Vector fromDirection, Vector toDirection)
		{
			float num = Mathf.Sqrt(2f + 2f * fromDirection.Dot(toDirection));
			Vector vector = 1f / num * fromDirection.Cross(toDirection);
			return new LeapQuaternion(vector.x, vector.y, vector.z, 0.5f * num);
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000E8A90 File Offset: 0x000E6E90
		private static Finger makeThumb(int frameId, int handId, bool isLeft)
		{
			Vector position = new Vector(19.33826f, -6f, 53.168484f);
			Vector forward = new Vector(0.6363291f, -0.5f, -0.8997871f);
			Vector up = new Vector(0.80479395f, 0.44721392f, 0.39026454f);
			float[] jointLengths = new float[]
			{
				0f,
				46.22f,
				31.57f,
				21.67f
			};
			return TestHandFactory.makeFinger(Finger.FingerType.TYPE_THUMB, position, forward, up, jointLengths, frameId, handId, handId, isLeft);
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000E8B00 File Offset: 0x000E6F00
		private static Finger makeIndexFinger(int frameId, int handId, bool isLeft)
		{
			Vector position = new Vector(23.181286f, 2f, -23.149345f);
			Vector forward = new Vector(0.16604431f, -0.14834045f, -0.97489715f);
			Vector up = new Vector(0.024906646f, 0.98893636f, -0.14623457f);
			float[] jointLengths = new float[]
			{
				68.12f,
				39.78f,
				22.38f,
				15.82f
			};
			return TestHandFactory.makeFinger(Finger.FingerType.TYPE_INDEX, position, forward, up, jointLengths, frameId, handId, handId + 1, isLeft);
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000E8B74 File Offset: 0x000E6F74
		private static Finger makeMiddleFinger(int frameId, int handId, bool isLeft)
		{
			Vector position = new Vector(2.7887783f, 4f, -23.252106f);
			Vector forward = new Vector(0.029520785f, -0.14834045f, -0.98849565f);
			Vector up = new Vector(-0.14576527f, 0.97771597f, -0.15107597f);
			float[] jointLengths = new float[]
			{
				64.6f,
				44.63f,
				26.33f,
				17.4f
			};
			return TestHandFactory.makeFinger(Finger.FingerType.TYPE_MIDDLE, position, forward, up, jointLengths, frameId, handId, handId + 2, isLeft);
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000E8BE8 File Offset: 0x000E6FE8
		private static Finger makeRingFinger(int frameId, int handId, bool isLeft)
		{
			Vector position = new Vector(-17.447168f, 4f, -17.279144f);
			Vector forward = new Vector(-0.12131794f, -0.14834034f, -0.9814668f);
			Vector up = new Vector(-0.21691047f, 0.96883494f, -0.1196191f);
			float[] jointLengths = new float[]
			{
				58f,
				41.37f,
				25.65f,
				17.3f
			};
			return TestHandFactory.makeFinger(Finger.FingerType.TYPE_RING, position, forward, up, jointLengths, frameId, handId, handId + 3, isLeft);
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000E8C5C File Offset: 0x000E705C
		private static Finger makePinky(int frameId, int handId, bool isLeft)
		{
			Vector position = new Vector(-35.33744f, 0f, -9.728714f);
			Vector forward = new Vector(-0.25932893f, -0.105851226f, -0.95997083f);
			Vector up = new Vector(-0.35335022f, 0.9354595f, -0.007693566f);
			float[] jointLengths = new float[]
			{
				53.69f,
				32.74f,
				18.11f,
				15.96f
			};
			return TestHandFactory.makeFinger(Finger.FingerType.TYPE_PINKY, position, forward, up, jointLengths, frameId, handId, handId + 4, isLeft);
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000E8CD0 File Offset: 0x000E70D0
		private static Finger makeFinger(Finger.FingerType name, Vector position, Vector forward, Vector up, float[] jointLengths, int frameId, int handId, int fingerId, bool isLeft)
		{
			forward = forward.Normalized;
			up = up.Normalized;
			Bone[] array = new Bone[5];
			float num = -jointLengths[0];
			Bone bone = TestHandFactory.makeBone(Bone.BoneType.TYPE_METACARPAL, position + forward * num, jointLengths[0], 8f, forward, up, isLeft);
			num += jointLengths[0];
			array[0] = bone;
			Bone bone2 = TestHandFactory.makeBone(Bone.BoneType.TYPE_PROXIMAL, position + forward * num, jointLengths[1], 8f, forward, up, isLeft);
			num += jointLengths[1];
			array[1] = bone2;
			Bone bone3 = TestHandFactory.makeBone(Bone.BoneType.TYPE_INTERMEDIATE, position + forward * num, jointLengths[2], 8f, forward, up, isLeft);
			num += jointLengths[2];
			array[2] = bone3;
			Bone bone4 = TestHandFactory.makeBone(Bone.BoneType.TYPE_DISTAL, position + forward * num, jointLengths[3], 8f, forward, up, isLeft);
			array[3] = bone4;
			return new Finger((long)frameId, handId, fingerId, 0f, bone4.NextJoint, forward, 8f, jointLengths[1] + jointLengths[2] + jointLengths[3], true, name, array[0], array[1], array[2], array[3]);
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000E8DE4 File Offset: 0x000E71E4
		private static Bone makeBone(Bone.BoneType name, Vector proximalPosition, float length, float width, Vector direction, Vector up, bool isLeft)
		{
			LeapQuaternion rotation = Quaternion.LookRotation(-direction.ToVector3(), up.ToVector3()).ToLeapQuaternion();
			return new Bone(proximalPosition, proximalPosition + direction * length, Vector.Lerp(proximalPosition, proximalPosition + direction * length, 0.5f), direction, length, width, name, rotation);
		}

		// Token: 0x020006F2 RID: 1778
		public enum UnitType
		{
			// Token: 0x040022DB RID: 8923
			LeapUnits,
			// Token: 0x040022DC RID: 8924
			UnityUnits
		}

		// Token: 0x020006F3 RID: 1779
		public enum TestHandPose
		{
			// Token: 0x040022DE RID: 8926
			HeadMountedA,
			// Token: 0x040022DF RID: 8927
			HeadMountedB,
			// Token: 0x040022E0 RID: 8928
			DesktopModeA
		}
	}
}
