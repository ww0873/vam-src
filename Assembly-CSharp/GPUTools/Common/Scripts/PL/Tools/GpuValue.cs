using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GPUTools.Common.Scripts.PL.Tools
{
	// Token: 0x020009BA RID: 2490
	public class GpuValue<T>
	{
		// Token: 0x06003F01 RID: 16129 RVA: 0x0012E661 File Offset: 0x0012CA61
		public GpuValue(T value = default(T))
		{
			this.Value = value;
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06003F03 RID: 16131 RVA: 0x0012E679 File Offset: 0x0012CA79
		// (set) Token: 0x06003F02 RID: 16130 RVA: 0x0012E670 File Offset: 0x0012CA70
		public T Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x04002FE9 RID: 12265
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T <Value>k__BackingField;
	}
}
