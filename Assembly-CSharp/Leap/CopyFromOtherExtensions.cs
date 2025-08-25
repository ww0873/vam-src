using System;

namespace Leap
{
	// Token: 0x020005BC RID: 1468
	public static class CopyFromOtherExtensions
	{
		// Token: 0x0600253B RID: 9531 RVA: 0x000D6340 File Offset: 0x000D4740
		public static Frame CopyFrom(this Frame frame, Frame source)
		{
			frame.Id = source.Id;
			frame.Timestamp = source.Timestamp;
			frame.CurrentFramesPerSecond = source.CurrentFramesPerSecond;
			frame.ResizeHandList(source.Hands.Count);
			int count = frame.Hands.Count;
			while (count-- != 0)
			{
				frame.Hands[count].CopyFrom(source.Hands[count]);
			}
			return frame;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000D63BC File Offset: 0x000D47BC
		public static Hand CopyFrom(this Hand hand, Hand source)
		{
			hand.Id = source.Id;
			hand.Confidence = source.Confidence;
			hand.GrabStrength = source.GrabStrength;
			hand.GrabAngle = source.GrabAngle;
			hand.Rotation = source.Rotation;
			hand.PinchStrength = source.PinchStrength;
			hand.PinchDistance = source.PinchDistance;
			hand.PalmWidth = source.PalmWidth;
			hand.IsLeft = source.IsLeft;
			hand.TimeVisible = source.TimeVisible;
			hand.PalmPosition = source.PalmPosition;
			hand.StabilizedPalmPosition = source.StabilizedPalmPosition;
			hand.PalmVelocity = source.PalmVelocity;
			hand.PalmNormal = source.PalmNormal;
			hand.Direction = source.Direction;
			hand.WristPosition = source.WristPosition;
			hand.Arm.CopyFrom(source.Arm);
			int index = 5;
			while (index-- != 0)
			{
				hand.Fingers[index].CopyFrom(source.Fingers[index]);
			}
			return hand;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000D64CC File Offset: 0x000D48CC
		public static Finger CopyFrom(this Finger finger, Finger source)
		{
			int num = 4;
			while (num-- != 0)
			{
				finger.bones[num].CopyFrom(source.bones[num]);
			}
			finger.Id = source.Id;
			finger.HandId = source.HandId;
			finger.TimeVisible = source.TimeVisible;
			finger.TipPosition = source.TipPosition;
			finger.Direction = source.Direction;
			finger.Width = source.Width;
			finger.Length = source.Length;
			finger.IsExtended = source.IsExtended;
			finger.Type = source.Type;
			return finger;
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000D6570 File Offset: 0x000D4970
		public static Bone CopyFrom(this Bone bone, Bone source)
		{
			bone.PrevJoint = source.PrevJoint;
			bone.NextJoint = source.NextJoint;
			bone.Direction = source.Direction;
			bone.Center = source.Center;
			bone.Length = source.Length;
			bone.Width = source.Width;
			bone.Rotation = source.Rotation;
			bone.Type = source.Type;
			return bone;
		}
	}
}
