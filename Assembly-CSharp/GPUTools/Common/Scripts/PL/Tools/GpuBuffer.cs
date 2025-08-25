using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
	// Token: 0x020009B8 RID: 2488
	public class GpuBuffer<T> : IBufferWrapper where T : struct
	{
		// Token: 0x06003EF2 RID: 16114 RVA: 0x0012E3C7 File Offset: 0x0012C7C7
		public GpuBuffer(ComputeBuffer computeBuffer)
		{
			this.ComputeBuffer = computeBuffer;
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0012E3D6 File Offset: 0x0012C7D6
		public GpuBuffer(int count, int stride)
		{
			this.Data = new T[count];
			this.ComputeBuffer = new ComputeBuffer(count, stride);
			this.ComputeBuffer.SetData(this.Data);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0012E408 File Offset: 0x0012C808
		public GpuBuffer(T[] data, int stride)
		{
			this.Data = data;
			this.ComputeBuffer = new ComputeBuffer(data.Length, stride);
			this.ComputeBuffer.SetData(data);
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x0012E43B File Offset: 0x0012C83B
		// (set) Token: 0x06003EF5 RID: 16117 RVA: 0x0012E432 File Offset: 0x0012C832
		public ComputeBuffer ComputeBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<ComputeBuffer>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ComputeBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06003EF8 RID: 16120 RVA: 0x0012E44C File Offset: 0x0012C84C
		// (set) Token: 0x06003EF7 RID: 16119 RVA: 0x0012E443 File Offset: 0x0012C843
		public T[] Data
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

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0012E454 File Offset: 0x0012C854
		public void PushData()
		{
			this.ComputeBuffer.SetData(this.Data);
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0012E467 File Offset: 0x0012C867
		public void PullData()
		{
			this.ComputeBuffer.GetData(this.Data);
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x0012E47A File Offset: 0x0012C87A
		public void Dispose()
		{
			this.ComputeBuffer.Dispose();
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06003EFC RID: 16124 RVA: 0x0012E487 File Offset: 0x0012C887
		public int Count
		{
			get
			{
				return this.ComputeBuffer.count;
			}
		}

		// Token: 0x04002FE5 RID: 12261
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ComputeBuffer <ComputeBuffer>k__BackingField;

		// Token: 0x04002FE6 RID: 12262
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T[] <Data>k__BackingField;
	}
}
