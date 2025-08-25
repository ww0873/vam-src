using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
	// Token: 0x020009B9 RID: 2489
	public class GpuMatrix4x4
	{
		// Token: 0x06003EFD RID: 16125 RVA: 0x0012E494 File Offset: 0x0012C894
		public GpuMatrix4x4(Matrix4x4 data)
		{
			this.Data = data;
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06003EFE RID: 16126 RVA: 0x0012E4B0 File Offset: 0x0012C8B0
		// (set) Token: 0x06003EFF RID: 16127 RVA: 0x0012E4B8 File Offset: 0x0012C8B8
		public Matrix4x4 Data
		{
			[CompilerGenerated]
			get
			{
				return this.<Data>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Data>k__BackingField = value;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06003F00 RID: 16128 RVA: 0x0012E4C4 File Offset: 0x0012C8C4
		public float[] Values
		{
			get
			{
				this.values[0] = this.Data[0];
				this.values[1] = this.Data[1];
				this.values[2] = this.Data[2];
				this.values[3] = this.Data[3];
				this.values[4] = this.Data[4];
				this.values[5] = this.Data[5];
				this.values[6] = this.Data[6];
				this.values[7] = this.Data[7];
				this.values[8] = this.Data[8];
				this.values[9] = this.Data[9];
				this.values[10] = this.Data[10];
				this.values[11] = this.Data[11];
				this.values[12] = this.Data[12];
				this.values[13] = this.Data[13];
				this.values[14] = this.Data[14];
				this.values[15] = this.Data[15];
				return this.values;
			}
		}

		// Token: 0x04002FE7 RID: 12263
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Matrix4x4 <Data>k__BackingField;

		// Token: 0x04002FE8 RID: 12264
		private float[] values = new float[16];
	}
}
