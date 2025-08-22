using System;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C8 RID: 712
	internal struct TankEvent
	{
		// Token: 0x06001071 RID: 4209 RVA: 0x0005C54F File Offset: 0x0005A94F
		public TankEvent(TankEventType type, float value = 0f)
		{
			this.eventType = type;
			this.eventValue = value;
		}

		// Token: 0x04000EA1 RID: 3745
		public TankEventType eventType;

		// Token: 0x04000EA2 RID: 3746
		public float eventValue;
	}
}
