using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000216 RID: 534
	[ProtoContract(AsReferenceDefault = true)]
	public class DataContract
	{
		// Token: 0x06000ABC RID: 2748 RVA: 0x00042230 File Offset: 0x00040630
		public DataContract()
		{
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00042238 File Offset: 0x00040638
		public DataContract(PersistentData data)
		{
			this.Data = data;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00042247 File Offset: 0x00040647
		public DataContract(PrimitiveContract primitive)
		{
			this.Data = primitive;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00042256 File Offset: 0x00040656
		public DataContract(PersistentUnityEventBase data)
		{
			this.Data = data;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00042265 File Offset: 0x00040665
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x0004226D File Offset: 0x0004066D
		[ProtoMember(1, DynamicType = true)]
		public object Data
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00042276 File Offset: 0x00040676
		public PrimitiveContract AsPrimitive
		{
			get
			{
				return this.Data as PrimitiveContract;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00042283 File Offset: 0x00040683
		public PersistentUnityEventBase AsUnityEvent
		{
			get
			{
				return this.Data as PersistentUnityEventBase;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00042290 File Offset: 0x00040690
		public PersistentData AsPersistentData
		{
			get
			{
				return this.Data as PersistentData;
			}
		}

		// Token: 0x04000BF8 RID: 3064
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object <Data>k__BackingField;
	}
}
