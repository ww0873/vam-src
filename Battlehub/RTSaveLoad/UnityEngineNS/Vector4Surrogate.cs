using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001ED RID: 493
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class Vector4Surrogate : ISerializationSurrogate
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x0003D303 File Offset: 0x0003B703
		public Vector4Surrogate()
		{
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0003D30C File Offset: 0x0003B70C
		public static implicit operator Vector4(Vector4Surrogate v)
		{
			return new Vector4
			{
				x = v.x,
				y = v.y,
				z = v.z,
				w = v.w
			};
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0003D358 File Offset: 0x0003B758
		public static implicit operator Vector4Surrogate(Vector4 v)
		{
			return new Vector4Surrogate
			{
				x = v.x,
				y = v.y,
				z = v.z,
				w = v.w
			};
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0003D3A0 File Offset: 0x0003B7A0
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Vector4 vector = (Vector4)obj;
			info.AddValue("x", vector.x);
			info.AddValue("y", vector.y);
			info.AddValue("z", vector.z);
			info.AddValue("w", vector.w);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0003D3FC File Offset: 0x0003B7FC
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Vector4 vector = (Vector4)obj;
			vector.x = (float)info.GetValue("x", typeof(float));
			vector.y = (float)info.GetValue("y", typeof(float));
			vector.z = (float)info.GetValue("z", typeof(float));
			vector.w = (float)info.GetValue("w", typeof(float));
			return vector;
		}

		// Token: 0x04000B25 RID: 2853
		public float x;

		// Token: 0x04000B26 RID: 2854
		public float y;

		// Token: 0x04000B27 RID: 2855
		public float z;

		// Token: 0x04000B28 RID: 2856
		public float w;
	}
}
