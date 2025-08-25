using System;

namespace Leap
{
	// Token: 0x0200062A RID: 1578
	public static class TransformExtensions
	{
		// Token: 0x060026B2 RID: 9906 RVA: 0x000D91A8 File Offset: 0x000D75A8
		public static Frame Transform(this Frame frame, LeapTransform transform)
		{
			int count = frame.Hands.Count;
			while (count-- != 0)
			{
				frame.Hands[count].Transform(transform);
			}
			return frame;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000D91E4 File Offset: 0x000D75E4
		public static Frame TransformedCopy(this Frame frame, LeapTransform transform)
		{
			return new Frame().CopyFrom(frame).Transform(transform);
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000D91F8 File Offset: 0x000D75F8
		public static Hand Transform(this Hand hand, LeapTransform transform)
		{
			hand.PalmPosition = transform.TransformPoint(hand.PalmPosition);
			hand.StabilizedPalmPosition = transform.TransformPoint(hand.StabilizedPalmPosition);
			hand.PalmVelocity = transform.TransformVelocity(hand.PalmVelocity);
			hand.PalmNormal = transform.TransformDirection(hand.PalmNormal);
			hand.Direction = transform.TransformDirection(hand.Direction);
			hand.WristPosition = transform.TransformPoint(hand.WristPosition);
			hand.PalmWidth *= Math.Abs(transform.scale.x);
			hand.Rotation = transform.TransformQuaternion(hand.Rotation);
			hand.Arm.Transform(transform);
			int index = 5;
			while (index-- != 0)
			{
				hand.Fingers[index].Transform(transform);
			}
			return hand;
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000D92DD File Offset: 0x000D76DD
		public static Hand TransformedCopy(this Hand hand, LeapTransform transform)
		{
			return new Hand().CopyFrom(hand).Transform(transform);
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000D92F0 File Offset: 0x000D76F0
		public static Finger Transform(this Finger finger, LeapTransform transform)
		{
			Bone bone = finger.bones[3];
			bone.NextJoint = transform.TransformPoint(bone.NextJoint);
			finger.TipPosition = bone.NextJoint;
			int num = 3;
			while (num-- != 0)
			{
				Bone bone2 = finger.bones[num];
				bone2.NextJoint = (bone.PrevJoint = transform.TransformPoint(bone2.NextJoint));
				bone.TransformGivenJoints(transform);
				bone = bone2;
			}
			bone.PrevJoint = transform.TransformPoint(bone.PrevJoint);
			bone.TransformGivenJoints(transform);
			finger.Direction = finger.bones[2].Direction;
			finger.Width *= Math.Abs(transform.scale.x);
			finger.Length *= Math.Abs(transform.scale.z);
			return finger;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000D93D5 File Offset: 0x000D77D5
		public static Finger TransformedCopy(this Finger finger, LeapTransform transform)
		{
			return new Finger().CopyFrom(finger).Transform(transform);
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000D93E8 File Offset: 0x000D77E8
		public static Bone Transform(this Bone bone, LeapTransform transform)
		{
			bone.PrevJoint = transform.TransformPoint(bone.PrevJoint);
			bone.NextJoint = transform.TransformPoint(bone.NextJoint);
			bone.TransformGivenJoints(transform);
			return bone;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000D9418 File Offset: 0x000D7818
		internal static void TransformGivenJoints(this Bone bone, LeapTransform transform)
		{
			bone.Length *= Math.Abs(transform.scale.z);
			bone.Center = (bone.PrevJoint + bone.NextJoint) / 2f;
			if (bone.Length < 1E-45f)
			{
				bone.Direction = Vector.Zero;
			}
			else
			{
				bone.Direction = (bone.NextJoint - bone.PrevJoint) / bone.Length;
			}
			bone.Width *= Math.Abs(transform.scale.x);
			bone.Rotation = transform.TransformQuaternion(bone.Rotation);
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000D94DD File Offset: 0x000D78DD
		public static Bone TransformedCopy(this Bone bone, LeapTransform transform)
		{
			return new Bone().CopyFrom(bone).Transform(transform);
		}
	}
}
