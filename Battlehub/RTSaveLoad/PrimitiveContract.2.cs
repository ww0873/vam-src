using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000215 RID: 533
	[ProtoContract]
	public class PrimitiveContract<T> : PrimitiveContract
	{
		// Token: 0x06000AB6 RID: 2742 RVA: 0x000421ED File Offset: 0x000405ED
		public PrimitiveContract()
		{
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000421F5 File Offset: 0x000405F5
		public PrimitiveContract(T value)
		{
			this.Value = value;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00042204 File Offset: 0x00040604
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0004220C File Offset: 0x0004060C
		[ProtoMember(1)]
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00042215 File Offset: 0x00040615
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x00042222 File Offset: 0x00040622
		protected override object ValueImpl
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = (T)((object)value);
			}
		}

		// Token: 0x04000BF7 RID: 3063
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T <Value>k__BackingField;
	}
}
