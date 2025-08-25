using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001E9 RID: 489
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class RectSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x0003C9FA File Offset: 0x0003ADFA
		public RectSurrogate()
		{
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0003CA04 File Offset: 0x0003AE04
		public static implicit operator Rect(RectSurrogate v)
		{
			return new Rect
			{
				x = v.x,
				y = v.y,
				width = v.width,
				height = v.height
			};
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0003CA50 File Offset: 0x0003AE50
		public static implicit operator RectSurrogate(Rect v)
		{
			return new RectSurrogate
			{
				x = v.x,
				y = v.y,
				width = v.width,
				height = v.height
			};
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0003CA98 File Offset: 0x0003AE98
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Rect rect = (Rect)obj;
			info.AddValue("x", rect.x);
			info.AddValue("y", rect.y);
			info.AddValue("width", rect.width);
			info.AddValue("height", rect.height);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0003CAF4 File Offset: 0x0003AEF4
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Rect rect = (Rect)obj;
			rect.x = (float)info.GetValue("x", typeof(float));
			rect.y = (float)info.GetValue("y", typeof(float));
			rect.width = (float)info.GetValue("width", typeof(float));
			rect.height = (float)info.GetValue("height", typeof(float));
			return rect;
		}

		// Token: 0x04000B0C RID: 2828
		public float x;

		// Token: 0x04000B0D RID: 2829
		public float y;

		// Token: 0x04000B0E RID: 2830
		public float width;

		// Token: 0x04000B0F RID: 2831
		public float height;
	}
}
