using System;
using System.Collections.Generic;

namespace Leap
{
	// Token: 0x020005DD RID: 1501
	[Serializable]
	public class Hand : IEquatable<Hand>
	{
		// Token: 0x060025D1 RID: 9681 RVA: 0x000D7180 File Offset: 0x000D5580
		public Hand()
		{
			this.Arm = new Arm();
			this.Fingers = new List<Finger>(5);
			this.Fingers.Add(new Finger());
			this.Fingers.Add(new Finger());
			this.Fingers.Add(new Finger());
			this.Fingers.Add(new Finger());
			this.Fingers.Add(new Finger());
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000D71FC File Offset: 0x000D55FC
		public Hand(long frameID, int id, float confidence, float grabStrength, float grabAngle, float pinchStrength, float pinchDistance, float palmWidth, bool isLeft, float timeVisible, Arm arm, List<Finger> fingers, Vector palmPosition, Vector stabilizedPalmPosition, Vector palmVelocity, Vector palmNormal, LeapQuaternion palmOrientation, Vector direction, Vector wristPosition)
		{
			this.FrameId = frameID;
			this.Id = id;
			this.Confidence = confidence;
			this.GrabStrength = grabStrength;
			this.GrabAngle = grabAngle;
			this.PinchStrength = pinchStrength;
			this.PinchDistance = pinchDistance;
			this.PalmWidth = palmWidth;
			this.IsLeft = isLeft;
			this.TimeVisible = timeVisible;
			this.Arm = arm;
			this.Fingers = fingers;
			this.PalmPosition = palmPosition;
			this.StabilizedPalmPosition = stabilizedPalmPosition;
			this.PalmVelocity = palmVelocity;
			this.PalmNormal = palmNormal;
			this.Rotation = palmOrientation;
			this.Direction = direction;
			this.WristPosition = wristPosition;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000D72A4 File Offset: 0x000D56A4
		public Finger Finger(int id)
		{
			int count = this.Fingers.Count;
			while (count-- != 0)
			{
				if (this.Fingers[count].Id == id)
				{
					return this.Fingers[count];
				}
			}
			return null;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000D72F1 File Offset: 0x000D56F1
		public bool Equals(Hand other)
		{
			return this.Id == other.Id && this.FrameId == other.FrameId;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x000D7315 File Offset: 0x000D5715
		public override string ToString()
		{
			return string.Format("Hand {0} {1}.", this.Id, (!this.IsLeft) ? "right" : "left");
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060025D6 RID: 9686 RVA: 0x000D7346 File Offset: 0x000D5746
		public LeapTransform Basis
		{
			get
			{
				return new LeapTransform(this.PalmPosition, this.Rotation);
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060025D7 RID: 9687 RVA: 0x000D7359 File Offset: 0x000D5759
		public bool IsRight
		{
			get
			{
				return !this.IsLeft;
			}
		}

		// Token: 0x04001F82 RID: 8066
		public long FrameId;

		// Token: 0x04001F83 RID: 8067
		public int Id;

		// Token: 0x04001F84 RID: 8068
		public List<Finger> Fingers;

		// Token: 0x04001F85 RID: 8069
		public Vector PalmPosition;

		// Token: 0x04001F86 RID: 8070
		public Vector PalmVelocity;

		// Token: 0x04001F87 RID: 8071
		public Vector PalmNormal;

		// Token: 0x04001F88 RID: 8072
		public Vector Direction;

		// Token: 0x04001F89 RID: 8073
		public LeapQuaternion Rotation;

		// Token: 0x04001F8A RID: 8074
		public float GrabStrength;

		// Token: 0x04001F8B RID: 8075
		public float GrabAngle;

		// Token: 0x04001F8C RID: 8076
		public float PinchStrength;

		// Token: 0x04001F8D RID: 8077
		public float PinchDistance;

		// Token: 0x04001F8E RID: 8078
		public float PalmWidth;

		// Token: 0x04001F8F RID: 8079
		public Vector StabilizedPalmPosition;

		// Token: 0x04001F90 RID: 8080
		public Vector WristPosition;

		// Token: 0x04001F91 RID: 8081
		public float TimeVisible;

		// Token: 0x04001F92 RID: 8082
		public float Confidence;

		// Token: 0x04001F93 RID: 8083
		public bool IsLeft;

		// Token: 0x04001F94 RID: 8084
		public Arm Arm;
	}
}
