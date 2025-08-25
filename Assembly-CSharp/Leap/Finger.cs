using System;

namespace Leap
{
	// Token: 0x020005DA RID: 1498
	[Serializable]
	public class Finger
	{
		// Token: 0x060025C4 RID: 9668 RVA: 0x000D6EA8 File Offset: 0x000D52A8
		public Finger()
		{
			this.bones[0] = new Bone();
			this.bones[1] = new Bone();
			this.bones[2] = new Bone();
			this.bones[3] = new Bone();
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000D6EFC File Offset: 0x000D52FC
		public Finger(long frameId, int handId, int fingerId, float timeVisible, Vector tipPosition, Vector direction, float width, float length, bool isExtended, Finger.FingerType type, Bone metacarpal, Bone proximal, Bone intermediate, Bone distal)
		{
			this.Type = type;
			this.bones[0] = metacarpal;
			this.bones[1] = proximal;
			this.bones[2] = intermediate;
			this.bones[3] = distal;
			this.Id = handId * 10 + fingerId;
			this.HandId = handId;
			this.TipPosition = tipPosition;
			this.Direction = direction;
			this.Width = width;
			this.Length = length;
			this.IsExtended = isExtended;
			this.TimeVisible = timeVisible;
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000D6F8E File Offset: 0x000D538E
		public Bone Bone(Bone.BoneType boneIx)
		{
			return this.bones[(int)boneIx];
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000D6F98 File Offset: 0x000D5398
		public override string ToString()
		{
			return Enum.GetName(typeof(Finger.FingerType), this.Type) + " id:" + this.Id;
		}

		// Token: 0x04001F6C RID: 8044
		public Bone[] bones = new Bone[4];

		// Token: 0x04001F6D RID: 8045
		public Finger.FingerType Type;

		// Token: 0x04001F6E RID: 8046
		public int Id;

		// Token: 0x04001F6F RID: 8047
		public int HandId;

		// Token: 0x04001F70 RID: 8048
		public Vector TipPosition;

		// Token: 0x04001F71 RID: 8049
		public Vector Direction;

		// Token: 0x04001F72 RID: 8050
		public float Width;

		// Token: 0x04001F73 RID: 8051
		public float Length;

		// Token: 0x04001F74 RID: 8052
		public bool IsExtended;

		// Token: 0x04001F75 RID: 8053
		public float TimeVisible;

		// Token: 0x020005DB RID: 1499
		public enum FingerType
		{
			// Token: 0x04001F77 RID: 8055
			TYPE_THUMB,
			// Token: 0x04001F78 RID: 8056
			TYPE_INDEX,
			// Token: 0x04001F79 RID: 8057
			TYPE_MIDDLE,
			// Token: 0x04001F7A RID: 8058
			TYPE_RING,
			// Token: 0x04001F7B RID: 8059
			TYPE_PINKY,
			// Token: 0x04001F7C RID: 8060
			TYPE_UNKNOWN = -1
		}
	}
}
