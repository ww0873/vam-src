using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leap.Unity.Query;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000732 RID: 1842
	public static class HandUtils
	{
		// Token: 0x06002CE7 RID: 11495 RVA: 0x000F07C0 File Offset: 0x000EEBC0
		public static void Fill(this Hand toFill, long frameID, int id, float confidence, float grabStrength, float grabAngle, float pinchStrength, float pinchDistance, float palmWidth, bool isLeft, float timeVisible, List<Finger> fingers, Vector palmPosition, Vector stabilizedPalmPosition, Vector palmVelocity, Vector palmNormal, LeapQuaternion rotation, Vector direction, Vector wristPosition)
		{
			toFill.FrameId = frameID;
			toFill.Id = id;
			toFill.Confidence = confidence;
			toFill.GrabStrength = grabStrength;
			toFill.GrabAngle = grabAngle;
			toFill.PinchStrength = pinchStrength;
			toFill.PinchDistance = pinchDistance;
			toFill.PalmWidth = palmWidth;
			toFill.IsLeft = isLeft;
			toFill.TimeVisible = timeVisible;
			if (fingers != null)
			{
				toFill.Fingers = fingers;
			}
			toFill.PalmPosition = palmPosition;
			toFill.StabilizedPalmPosition = stabilizedPalmPosition;
			toFill.PalmVelocity = palmVelocity;
			toFill.PalmNormal = palmNormal;
			toFill.Rotation = rotation;
			toFill.Direction = direction;
			toFill.WristPosition = wristPosition;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000F0861 File Offset: 0x000EEC61
		public static void Fill(this Bone toFill, Vector prevJoint, Vector nextJoint, Vector center, Vector direction, float length, float width, Bone.BoneType type, LeapQuaternion rotation)
		{
			toFill.PrevJoint = prevJoint;
			toFill.NextJoint = nextJoint;
			toFill.Center = center;
			toFill.Direction = direction;
			toFill.Length = length;
			toFill.Width = width;
			toFill.Type = type;
			toFill.Rotation = rotation;
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000F08A0 File Offset: 0x000EECA0
		public static void Fill(this Finger toFill, long frameId, int handId, int fingerId, float timeVisible, Vector tipPosition, Vector direction, float width, float length, bool isExtended, Finger.FingerType type, Bone metacarpal = null, Bone proximal = null, Bone intermediate = null, Bone distal = null)
		{
			toFill.Id = handId;
			toFill.HandId = handId;
			toFill.TimeVisible = timeVisible;
			toFill.TipPosition = tipPosition;
			toFill.Direction = direction;
			toFill.Width = width;
			toFill.Length = length;
			toFill.IsExtended = isExtended;
			toFill.Type = type;
			if (metacarpal != null)
			{
				toFill.bones[0] = metacarpal;
			}
			if (proximal != null)
			{
				toFill.bones[1] = proximal;
			}
			if (intermediate != null)
			{
				toFill.bones[2] = intermediate;
			}
			if (distal != null)
			{
				toFill.bones[3] = distal;
			}
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000F0937 File Offset: 0x000EED37
		public static void Fill(this Arm toFill, Vector elbow, Vector wrist, Vector center, Vector direction, float length, float width, LeapQuaternion rotation)
		{
			toFill.PrevJoint = elbow;
			toFill.NextJoint = wrist;
			toFill.Center = center;
			toFill.Direction = direction;
			toFill.Length = length;
			toFill.Width = width;
			toFill.Rotation = rotation;
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000F096E File Offset: 0x000EED6E
		public static void FillTemporalData(this Hand toFill, Hand previousHand, float deltaTime)
		{
			toFill.PalmVelocity = (toFill.PalmPosition - previousHand.PalmPosition) / deltaTime;
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000F0990 File Offset: 0x000EED90
		public static Hand Get(this Frame frame, Chirality whichHand)
		{
			HandUtils.<Get>c__AnonStorey0 <Get>c__AnonStorey = new HandUtils.<Get>c__AnonStorey0();
			<Get>c__AnonStorey.whichHand = whichHand;
			return frame.Hands.Query<Hand>().FirstOrDefault(new Func<Hand, bool>(<Get>c__AnonStorey.<>m__0));
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000F09C8 File Offset: 0x000EEDC8
		public static Hand Get(this LeapProvider provider, Chirality whichHand)
		{
			Frame frame;
			if (Time.inFixedTimeStep)
			{
				frame = provider.CurrentFixedFrame;
			}
			else
			{
				frame = provider.CurrentFrame;
			}
			return frame.Get(whichHand);
		}

		// Token: 0x02000FAE RID: 4014
		[CompilerGenerated]
		private sealed class <Get>c__AnonStorey0
		{
			// Token: 0x060074CA RID: 29898 RVA: 0x000F09F9 File Offset: 0x000EEDF9
			public <Get>c__AnonStorey0()
			{
			}

			// Token: 0x060074CB RID: 29899 RVA: 0x000F0A01 File Offset: 0x000EEE01
			internal bool <>m__0(Hand h)
			{
				return h.IsLeft == (this.whichHand == Chirality.Left);
			}

			// Token: 0x040068DC RID: 26844
			internal Chirality whichHand;
		}
	}
}
