using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001DF RID: 479
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class RectOffsetSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009A1 RID: 2465 RVA: 0x0003B637 File Offset: 0x00039A37
		public RectOffsetSurrogate()
		{
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0003B640 File Offset: 0x00039A40
		public static implicit operator RectOffset(RectOffsetSurrogate v)
		{
			return new RectOffset
			{
				left = v.left,
				right = v.right,
				top = v.top,
				bottom = v.bottom
			};
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0003B684 File Offset: 0x00039A84
		public static implicit operator RectOffsetSurrogate(RectOffset v)
		{
			return new RectOffsetSurrogate
			{
				left = v.left,
				right = v.right,
				top = v.top,
				bottom = v.bottom
			};
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0003B6C8 File Offset: 0x00039AC8
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			RectOffset rectOffset = (RectOffset)obj;
			info.AddValue("left", rectOffset.left);
			info.AddValue("right", rectOffset.right);
			info.AddValue("top", rectOffset.top);
			info.AddValue("bottom", rectOffset.bottom);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0003B720 File Offset: 0x00039B20
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			RectOffset rectOffset = (RectOffset)obj;
			rectOffset.left = (int)info.GetValue("left", typeof(int));
			rectOffset.right = (int)info.GetValue("right", typeof(int));
			rectOffset.top = (int)info.GetValue("top", typeof(int));
			rectOffset.bottom = (int)info.GetValue("bottom", typeof(int));
			return rectOffset;
		}

		// Token: 0x04000AD7 RID: 2775
		public int left;

		// Token: 0x04000AD8 RID: 2776
		public int right;

		// Token: 0x04000AD9 RID: 2777
		public int top;

		// Token: 0x04000ADA RID: 2778
		public int bottom;
	}
}
