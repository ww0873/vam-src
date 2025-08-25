using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005C1 RID: 1473
	public class DistortionData
	{
		// Token: 0x06002563 RID: 9571 RVA: 0x000D69F6 File Offset: 0x000D4DF6
		public DistortionData()
		{
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000D69FE File Offset: 0x000D4DFE
		public DistortionData(ulong version, float width, float height, float[] data)
		{
			this.Version = version;
			this.Width = width;
			this.Height = height;
			this.Data = data;
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06002565 RID: 9573 RVA: 0x000D6A23 File Offset: 0x000D4E23
		// (set) Token: 0x06002566 RID: 9574 RVA: 0x000D6A2B File Offset: 0x000D4E2B
		public ulong Version
		{
			[CompilerGenerated]
			get
			{
				return this.<Version>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Version>k__BackingField = value;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x000D6A34 File Offset: 0x000D4E34
		// (set) Token: 0x06002568 RID: 9576 RVA: 0x000D6A3C File Offset: 0x000D4E3C
		public float Width
		{
			[CompilerGenerated]
			get
			{
				return this.<Width>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Width>k__BackingField = value;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06002569 RID: 9577 RVA: 0x000D6A45 File Offset: 0x000D4E45
		// (set) Token: 0x0600256A RID: 9578 RVA: 0x000D6A4D File Offset: 0x000D4E4D
		public float Height
		{
			[CompilerGenerated]
			get
			{
				return this.<Height>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Height>k__BackingField = value;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600256B RID: 9579 RVA: 0x000D6A56 File Offset: 0x000D4E56
		// (set) Token: 0x0600256C RID: 9580 RVA: 0x000D6A5E File Offset: 0x000D4E5E
		public float[] Data
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

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600256D RID: 9581 RVA: 0x000D6A68 File Offset: 0x000D4E68
		public bool IsValid
		{
			get
			{
				return this.Data != null && this.Width == (float)LeapC.DistortionSize && this.Height == (float)LeapC.DistortionSize && (float)this.Data.Length == this.Width * this.Height * 2f;
			}
		}

		// Token: 0x04001F2B RID: 7979
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ulong <Version>k__BackingField;

		// Token: 0x04001F2C RID: 7980
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <Width>k__BackingField;

		// Token: 0x04001F2D RID: 7981
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <Height>k__BackingField;

		// Token: 0x04001F2E RID: 7982
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float[] <Data>k__BackingField;
	}
}
