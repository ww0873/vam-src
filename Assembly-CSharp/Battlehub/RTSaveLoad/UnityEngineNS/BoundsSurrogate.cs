using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001EC RID: 492
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class BoundsSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009E2 RID: 2530 RVA: 0x0003D101 File Offset: 0x0003B501
		public BoundsSurrogate()
		{
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0003D10C File Offset: 0x0003B50C
		public static implicit operator Bounds(BoundsSurrogate v)
		{
			return new Bounds
			{
				center = v.center,
				size = v.size,
				extents = v.extents,
				min = v.min,
				max = v.max
			};
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0003D164 File Offset: 0x0003B564
		public static implicit operator BoundsSurrogate(Bounds v)
		{
			return new BoundsSurrogate
			{
				center = v.center,
				size = v.size,
				extents = v.extents,
				min = v.min,
				max = v.max
			};
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0003D1BC File Offset: 0x0003B5BC
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Bounds bounds = (Bounds)obj;
			info.AddValue("center", bounds.center);
			info.AddValue("size", bounds.size);
			info.AddValue("extents", bounds.extents);
			info.AddValue("min", bounds.min);
			info.AddValue("max", bounds.max);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0003D244 File Offset: 0x0003B644
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Bounds bounds = (Bounds)obj;
			bounds.center = (Vector3)info.GetValue("center", typeof(Vector3));
			bounds.size = (Vector3)info.GetValue("size", typeof(Vector3));
			bounds.extents = (Vector3)info.GetValue("extents", typeof(Vector3));
			bounds.min = (Vector3)info.GetValue("min", typeof(Vector3));
			bounds.max = (Vector3)info.GetValue("max", typeof(Vector3));
			return bounds;
		}

		// Token: 0x04000B20 RID: 2848
		public Vector3 center;

		// Token: 0x04000B21 RID: 2849
		public Vector3 size;

		// Token: 0x04000B22 RID: 2850
		public Vector3 extents;

		// Token: 0x04000B23 RID: 2851
		public Vector3 min;

		// Token: 0x04000B24 RID: 2852
		public Vector3 max;
	}
}
