using System;
using GPUTools.Common.Scripts.PL.Tools;

namespace GPUTools.Physics.Scripts.Types.Joints
{
	// Token: 0x02000A7D RID: 2685
	public struct GPDistanceJoint : IGroupItem
	{
		// Token: 0x060045B3 RID: 17843 RVA: 0x0013F6C8 File Offset: 0x0013DAC8
		public GPDistanceJoint(int body1Id, int body2Id, float distance, float elasticity)
		{
			this.Body1Id = body1Id;
			this.Body2Id = body2Id;
			this.Distance = distance;
			this.Elasticity = elasticity;
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x0013F6E7 File Offset: 0x0013DAE7
		public static int Size()
		{
			return 16;
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x0013F6EC File Offset: 0x0013DAEC
		public bool HasConflict(IGroupItem item)
		{
			GPDistanceJoint gpdistanceJoint = (GPDistanceJoint)item;
			return gpdistanceJoint.Body1Id == this.Body1Id || gpdistanceJoint.Body2Id == this.Body1Id || gpdistanceJoint.Body1Id == this.Body2Id || gpdistanceJoint.Body2Id == this.Body2Id;
		}

		// Token: 0x0400335D RID: 13149
		public int Body1Id;

		// Token: 0x0400335E RID: 13150
		public int Body2Id;

		// Token: 0x0400335F RID: 13151
		public float Distance;

		// Token: 0x04003360 RID: 13152
		public float Elasticity;
	}
}
