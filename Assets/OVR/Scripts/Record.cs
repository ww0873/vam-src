using System;

namespace Assets.OVR.Scripts
{
	// Token: 0x02000971 RID: 2417
	public class Record
	{
		// Token: 0x06003C64 RID: 15460 RVA: 0x00124AE5 File Offset: 0x00122EE5
		public Record(string cat, string msg)
		{
			this.category = cat;
			this.message = msg;
		}

		// Token: 0x04002E47 RID: 11847
		public string category;

		// Token: 0x04002E48 RID: 11848
		public string message;
	}
}
