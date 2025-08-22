using System;

namespace GPUTools.Common.Scripts.PL.Attributes
{
	// Token: 0x020009B4 RID: 2484
	[AttributeUsage(AttributeTargets.Property)]
	public class GpuData : Attribute
	{
		// Token: 0x06003EE9 RID: 16105 RVA: 0x0012E27A File Offset: 0x0012C67A
		public GpuData(string name)
		{
			this.Name = name;
		}

		// Token: 0x04002FE2 RID: 12258
		public string Name;
	}
}
