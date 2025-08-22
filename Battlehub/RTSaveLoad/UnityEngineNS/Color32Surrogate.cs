using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x02000200 RID: 512
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class Color32Surrogate : ISerializationSurrogate
	{
		// Token: 0x06000A42 RID: 2626 RVA: 0x0003E67E File Offset: 0x0003CA7E
		public Color32Surrogate()
		{
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0003E688 File Offset: 0x0003CA88
		public static implicit operator Color32(Color32Surrogate v)
		{
			return new Color32
			{
				r = v.r,
				g = v.g,
				b = v.b,
				a = v.a
			};
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0003E6D4 File Offset: 0x0003CAD4
		public static implicit operator Color32Surrogate(Color32 v)
		{
			return new Color32Surrogate
			{
				r = v.r,
				g = v.g,
				b = v.b,
				a = v.a
			};
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0003E71C File Offset: 0x0003CB1C
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Color32 color = (Color32)obj;
			info.AddValue("r", color.r);
			info.AddValue("g", color.g);
			info.AddValue("b", color.b);
			info.AddValue("a", color.a);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0003E778 File Offset: 0x0003CB78
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Color32 color = (Color32)obj;
			color.r = (byte)info.GetValue("r", typeof(byte));
			color.g = (byte)info.GetValue("g", typeof(byte));
			color.b = (byte)info.GetValue("b", typeof(byte));
			color.a = (byte)info.GetValue("a", typeof(byte));
			return color;
		}

		// Token: 0x04000B51 RID: 2897
		public byte r;

		// Token: 0x04000B52 RID: 2898
		public byte g;

		// Token: 0x04000B53 RID: 2899
		public byte b;

		// Token: 0x04000B54 RID: 2900
		public byte a;
	}
}
