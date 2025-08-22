using System;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000214 RID: 532
	[ProtoContract]
	[ProtoInclude(101, typeof(PrimitiveContract<bool>))]
	[ProtoInclude(102, typeof(PrimitiveContract<char>))]
	[ProtoInclude(103, typeof(PrimitiveContract<byte>))]
	[ProtoInclude(104, typeof(PrimitiveContract<short>))]
	[ProtoInclude(105, typeof(PrimitiveContract<int>))]
	[ProtoInclude(106, typeof(PrimitiveContract<long>))]
	[ProtoInclude(107, typeof(PrimitiveContract<ushort>))]
	[ProtoInclude(108, typeof(PrimitiveContract<uint>))]
	[ProtoInclude(110, typeof(PrimitiveContract<ulong>))]
	[ProtoInclude(111, typeof(PrimitiveContract<string>))]
	[ProtoInclude(112, typeof(PrimitiveContract<float>))]
	[ProtoInclude(113, typeof(PrimitiveContract<double>))]
	[ProtoInclude(114, typeof(PrimitiveContract<decimal>))]
	[ProtoInclude(115, typeof(PrimitiveContract<bool[]>))]
	[ProtoInclude(116, typeof(PrimitiveContract<char[]>))]
	[ProtoInclude(117, typeof(PrimitiveContract<byte[]>))]
	[ProtoInclude(118, typeof(PrimitiveContract<short[]>))]
	[ProtoInclude(119, typeof(PrimitiveContract<int[]>))]
	[ProtoInclude(120, typeof(PrimitiveContract<long[]>))]
	[ProtoInclude(121, typeof(PrimitiveContract<ushort[]>))]
	[ProtoInclude(122, typeof(PrimitiveContract<uint[]>))]
	[ProtoInclude(123, typeof(PrimitiveContract<ulong[]>))]
	[ProtoInclude(124, typeof(PrimitiveContract<string[]>))]
	[ProtoInclude(125, typeof(PrimitiveContract<float[]>))]
	[ProtoInclude(126, typeof(PrimitiveContract<double[]>))]
	[ProtoInclude(127, typeof(PrimitiveContract<decimal[]>))]
	[ProtoInclude(128, typeof(PrimitiveContract<Color>))]
	[ProtoInclude(129, typeof(PrimitiveContract<Color[]>))]
	[ProtoInclude(130, typeof(PrimitiveContract<Vector3>))]
	[ProtoInclude(131, typeof(PrimitiveContract<Vector3[]>))]
	[ProtoInclude(132, typeof(PrimitiveContract<Vector4>))]
	[ProtoInclude(133, typeof(PrimitiveContract<Vector4[]>))]
	public abstract class PrimitiveContract
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x00042195 File Offset: 0x00040595
		protected PrimitiveContract()
		{
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0004219D File Offset: 0x0004059D
		public static PrimitiveContract<T> Create<T>(T value)
		{
			return new PrimitiveContract<T>(value);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000421A8 File Offset: 0x000405A8
		public static PrimitiveContract Create(Type type)
		{
			Type typeFromHandle = typeof(PrimitiveContract<>);
			Type type2 = typeFromHandle.MakeGenericType(new Type[]
			{
				type
			});
			return (PrimitiveContract)Activator.CreateInstance(type2);
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x000421DC File Offset: 0x000405DC
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x000421E4 File Offset: 0x000405E4
		public object ValueBase
		{
			get
			{
				return this.ValueImpl;
			}
			set
			{
				this.ValueImpl = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000AB4 RID: 2740
		// (set) Token: 0x06000AB5 RID: 2741
		protected abstract object ValueImpl { get; set; }
	}
}
