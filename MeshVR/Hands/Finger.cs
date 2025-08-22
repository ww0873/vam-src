using System;
using UnityEngine;

namespace MeshVR.Hands
{
	// Token: 0x02000E1B RID: 3611
	public class Finger : MonoBehaviour
	{
		// Token: 0x06006F32 RID: 28466 RVA: 0x0029BEC0 File Offset: 0x0029A2C0
		public Finger()
		{
		}

		// Token: 0x04006063 RID: 24675
		public Finger.Axis bendAxis = Finger.Axis.Z;

		// Token: 0x04006064 RID: 24676
		public Finger.Axis spreadAxis = Finger.Axis.Y;

		// Token: 0x04006065 RID: 24677
		public Finger.Axis twistAxis;

		// Token: 0x04006066 RID: 24678
		public bool bendEnabled = true;

		// Token: 0x04006067 RID: 24679
		public bool spreadEnabled;

		// Token: 0x04006068 RID: 24680
		public bool twistEnabled;

		// Token: 0x04006069 RID: 24681
		public float bendOffset;

		// Token: 0x0400606A RID: 24682
		public float spreadOffset;

		// Token: 0x0400606B RID: 24683
		public float twistOffset;

		// Token: 0x0400606C RID: 24684
		public float currentBend;

		// Token: 0x0400606D RID: 24685
		public float currentSpread;

		// Token: 0x0400606E RID: 24686
		public float currentTwist;

		// Token: 0x02000E1C RID: 3612
		public enum Axis
		{
			// Token: 0x04006070 RID: 24688
			X,
			// Token: 0x04006071 RID: 24689
			NegX,
			// Token: 0x04006072 RID: 24690
			Y,
			// Token: 0x04006073 RID: 24691
			NegY,
			// Token: 0x04006074 RID: 24692
			Z,
			// Token: 0x04006075 RID: 24693
			NegZ
		}
	}
}
