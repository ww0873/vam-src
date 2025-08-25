using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F0 RID: 496
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class Vector2Surrogate : ISerializationSurrogate
	{
		// Token: 0x060009F2 RID: 2546 RVA: 0x0003D4B4 File Offset: 0x0003B8B4
		public Vector2Surrogate()
		{
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0003D4BC File Offset: 0x0003B8BC
		public static implicit operator Vector2(Vector2Surrogate v)
		{
			return new Vector2
			{
				x = v.x,
				y = v.y
			};
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0003D4EC File Offset: 0x0003B8EC
		public static implicit operator Vector2Surrogate(Vector2 v)
		{
			return new Vector2Surrogate
			{
				x = v.x,
				y = v.y
			};
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0003D51C File Offset: 0x0003B91C
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Vector2 vector = (Vector2)obj;
			info.AddValue("x", vector.x);
			info.AddValue("y", vector.y);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0003D554 File Offset: 0x0003B954
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Vector2 vector = (Vector2)obj;
			vector.x = (float)info.GetValue("x", typeof(float));
			vector.y = (float)info.GetValue("y", typeof(float));
			return vector;
		}

		// Token: 0x04000B29 RID: 2857
		public float x;

		// Token: 0x04000B2A RID: 2858
		public float y;
	}
}
