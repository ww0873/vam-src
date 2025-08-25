﻿using System;
using Leap;

namespace LeapInternal
{
	// Token: 0x020005BB RID: 1467
	public static class CopyFromLeapCExtensions
	{
		// Token: 0x06002537 RID: 9527 RVA: 0x000D5ED8 File Offset: 0x000D42D8
		public static Leap.Frame CopyFrom(this Leap.Frame frame, ref LEAP_TRACKING_EVENT trackingMsg)
		{
			frame.Id = trackingMsg.info.frame_id;
			frame.Timestamp = trackingMsg.info.timestamp;
			frame.CurrentFramesPerSecond = trackingMsg.framerate;
			frame.ResizeHandList((int)trackingMsg.nHands);
			int count = frame.Hands.Count;
			while (count-- != 0)
			{
				LEAP_HAND leap_HAND;
				StructMarshal<LEAP_HAND>.ArrayElementToStruct(trackingMsg.pHands, count, out leap_HAND);
				frame.Hands[count].CopyFrom(ref leap_HAND, frame.Id);
			}
			return frame;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000D5F64 File Offset: 0x000D4364
		public static Hand CopyFrom(this Hand hand, ref LEAP_HAND leapHand, long frameId)
		{
			hand.FrameId = frameId;
			hand.Id = (int)leapHand.id;
			hand.Arm.CopyFrom(leapHand.arm, Bone.BoneType.TYPE_INVALID);
			hand.Confidence = leapHand.confidence;
			hand.GrabStrength = leapHand.grab_strength;
			hand.GrabAngle = leapHand.grab_angle;
			hand.PinchStrength = leapHand.pinch_strength;
			hand.PinchDistance = leapHand.pinch_distance;
			hand.PalmWidth = leapHand.palm.width;
			hand.IsLeft = (leapHand.type == eLeapHandType.eLeapHandType_Left);
			hand.TimeVisible = (float)(leapHand.visible_time * 1E-06);
			hand.PalmPosition = leapHand.palm.position.ToLeapVector();
			hand.StabilizedPalmPosition = leapHand.palm.stabilized_position.ToLeapVector();
			hand.PalmVelocity = leapHand.palm.velocity.ToLeapVector();
			hand.PalmNormal = leapHand.palm.normal.ToLeapVector();
			hand.Rotation = leapHand.palm.orientation.ToLeapQuaternion();
			hand.Direction = leapHand.palm.direction.ToLeapVector();
			hand.WristPosition = hand.Arm.NextJoint;
			hand.Fingers[0].CopyFrom(leapHand.thumb, Finger.FingerType.TYPE_THUMB, hand.Id, hand.TimeVisible);
			hand.Fingers[1].CopyFrom(leapHand.index, Finger.FingerType.TYPE_INDEX, hand.Id, hand.TimeVisible);
			hand.Fingers[2].CopyFrom(leapHand.middle, Finger.FingerType.TYPE_MIDDLE, hand.Id, hand.TimeVisible);
			hand.Fingers[3].CopyFrom(leapHand.ring, Finger.FingerType.TYPE_RING, hand.Id, hand.TimeVisible);
			hand.Fingers[4].CopyFrom(leapHand.pinky, Finger.FingerType.TYPE_PINKY, hand.Id, hand.TimeVisible);
			return hand;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000D615C File Offset: 0x000D455C
		public static Finger CopyFrom(this Finger finger, LEAP_DIGIT leapBone, Finger.FingerType type, int handId, float timeVisible)
		{
			finger.Id = handId * 10 + leapBone.finger_id;
			finger.HandId = handId;
			finger.TimeVisible = timeVisible;
			Bone bone = finger.bones[0];
			Bone bone2 = finger.bones[1];
			Bone bone3 = finger.bones[2];
			Bone bone4 = finger.bones[3];
			bone.CopyFrom(leapBone.metacarpal, Bone.BoneType.TYPE_METACARPAL);
			bone2.CopyFrom(leapBone.proximal, Bone.BoneType.TYPE_PROXIMAL);
			bone3.CopyFrom(leapBone.intermediate, Bone.BoneType.TYPE_INTERMEDIATE);
			bone4.CopyFrom(leapBone.distal, Bone.BoneType.TYPE_DISTAL);
			finger.TipPosition = bone4.NextJoint;
			finger.Direction = bone3.Direction;
			finger.Width = bone3.Width;
			finger.Length = ((leapBone.finger_id != 0) ? (0.5f * bone2.Length) : 0f) + bone3.Length + 0.77f * bone4.Length;
			finger.IsExtended = (leapBone.is_extended != 0);
			finger.Type = type;
			return finger;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000D6268 File Offset: 0x000D4668
		public static Bone CopyFrom(this Bone bone, LEAP_BONE leapBone, Bone.BoneType type)
		{
			bone.Type = type;
			bone.PrevJoint = leapBone.prev_joint.ToLeapVector();
			bone.NextJoint = leapBone.next_joint.ToLeapVector();
			bone.Direction = bone.NextJoint - bone.PrevJoint;
			bone.Length = bone.Direction.Magnitude;
			if (bone.Length < 1E-45f)
			{
				bone.Direction = Vector.Zero;
			}
			else
			{
				bone.Direction /= bone.Length;
			}
			bone.Center = (bone.PrevJoint + bone.NextJoint) / 2f;
			bone.Rotation = leapBone.rotation.ToLeapQuaternion();
			bone.Width = leapBone.width;
			return bone;
		}
	}
}
