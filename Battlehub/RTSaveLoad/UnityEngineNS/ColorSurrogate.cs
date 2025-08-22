using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001E8 RID: 488
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class ColorSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009CE RID: 2510 RVA: 0x0003C861 File Offset: 0x0003AC61
		public ColorSurrogate()
		{
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0003C86C File Offset: 0x0003AC6C
		public static implicit operator Color(ColorSurrogate v)
		{
			return new Color
			{
				r = v.r,
				g = v.g,
				b = v.b,
				a = v.a
			};
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0003C8B8 File Offset: 0x0003ACB8
		public static implicit operator ColorSurrogate(Color v)
		{
			return new ColorSurrogate
			{
				r = v.r,
				g = v.g,
				b = v.b,
				a = v.a
			};
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0003C900 File Offset: 0x0003AD00
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Color color = (Color)obj;
			info.AddValue("r", color.r);
			info.AddValue("g", color.g);
			info.AddValue("b", color.b);
			info.AddValue("a", color.a);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0003C95C File Offset: 0x0003AD5C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Color color = (Color)obj;
			color.r = (float)info.GetValue("r", typeof(float));
			color.g = (float)info.GetValue("g", typeof(float));
			color.b = (float)info.GetValue("b", typeof(float));
			color.a = (float)info.GetValue("a", typeof(float));
			return color;
		}

		// Token: 0x04000B08 RID: 2824
		public float r;

		// Token: 0x04000B09 RID: 2825
		public float g;

		// Token: 0x04000B0A RID: 2826
		public float b;

		// Token: 0x04000B0B RID: 2827
		public float a;
	}
}
