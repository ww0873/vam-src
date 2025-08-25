using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
	// Token: 0x020009D3 RID: 2515
	public class GPUVector3CopyPaster : KernelBase
	{
		// Token: 0x06003F7A RID: 16250 RVA: 0x0012F188 File Offset: 0x0012D588
		public GPUVector3CopyPaster(GpuBuffer<Vector3> vector3From, GpuBuffer<Vector3> vector3To) : base("Compute/Vector3CopyPaster", "CSVector3CopyPaster")
		{
			this.Vector3From = vector3From;
			this.Vector3To = vector3To;
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06003F7B RID: 16251 RVA: 0x0012F1A8 File Offset: 0x0012D5A8
		// (set) Token: 0x06003F7C RID: 16252 RVA: 0x0012F1B0 File Offset: 0x0012D5B0
		[GpuData("vector3From")]
		public GpuBuffer<Vector3> Vector3From
		{
			[CompilerGenerated]
			get
			{
				return this.<Vector3From>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Vector3From>k__BackingField = value;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06003F7D RID: 16253 RVA: 0x0012F1B9 File Offset: 0x0012D5B9
		// (set) Token: 0x06003F7E RID: 16254 RVA: 0x0012F1C1 File Offset: 0x0012D5C1
		[GpuData("vector3To")]
		public GpuBuffer<Vector3> Vector3To
		{
			[CompilerGenerated]
			get
			{
				return this.<Vector3To>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Vector3To>k__BackingField = value;
			}
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0012F1CA File Offset: 0x0012D5CA
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.Vector3From.Count / 256f);
		}

		// Token: 0x04003010 RID: 12304
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Vector3From>k__BackingField;

		// Token: 0x04003011 RID: 12305
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Vector3To>k__BackingField;
	}
}
