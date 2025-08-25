using System;

namespace Battlehub.RTCommon
{
	// Token: 0x020000B9 RID: 185
	public class LockObject
	{
		// Token: 0x06000306 RID: 774 RVA: 0x000140FE File Offset: 0x000124FE
		public LockObject()
		{
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00014106 File Offset: 0x00012506
		public bool IsPositionLocked
		{
			get
			{
				return this.PositionX && this.PositionY && this.PositionZ;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00014127 File Offset: 0x00012527
		public bool IsRotationLocked
		{
			get
			{
				return this.RotationX && this.RotationY && this.RotationZ && this.RotationScreen;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00014153 File Offset: 0x00012553
		public bool IsScaleLocked
		{
			get
			{
				return this.ScaleX && this.ScaleY && this.ScaleZ;
			}
		}

		// Token: 0x040003AF RID: 943
		public bool PositionX;

		// Token: 0x040003B0 RID: 944
		public bool PositionY;

		// Token: 0x040003B1 RID: 945
		public bool PositionZ;

		// Token: 0x040003B2 RID: 946
		public bool RotationX;

		// Token: 0x040003B3 RID: 947
		public bool RotationY;

		// Token: 0x040003B4 RID: 948
		public bool RotationZ;

		// Token: 0x040003B5 RID: 949
		public bool RotationScreen;

		// Token: 0x040003B6 RID: 950
		public bool ScaleX;

		// Token: 0x040003B7 RID: 951
		public bool ScaleY;

		// Token: 0x040003B8 RID: 952
		public bool ScaleZ;
	}
}
