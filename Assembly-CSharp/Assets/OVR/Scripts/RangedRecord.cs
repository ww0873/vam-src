using System;

namespace Assets.OVR.Scripts
{
	// Token: 0x02000972 RID: 2418
	public class RangedRecord : Record
	{
		// Token: 0x06003C65 RID: 15461 RVA: 0x00124AFB File Offset: 0x00122EFB
		public RangedRecord(string cat, string msg, float val, float minVal, float maxVal) : base(cat, msg)
		{
			this.value = val;
			this.min = minVal;
			this.max = maxVal;
		}

		// Token: 0x04002E49 RID: 11849
		public float value;

		// Token: 0x04002E4A RID: 11850
		public float min;

		// Token: 0x04002E4B RID: 11851
		public float max;
	}
}
