using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001DD RID: 477
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class GradientColorKeySurrogate : ISerializationSurrogate
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x0003B480 File Offset: 0x00039880
		public GradientColorKeySurrogate()
		{
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0003B488 File Offset: 0x00039888
		public static implicit operator GradientColorKey(GradientColorKeySurrogate v)
		{
			return new GradientColorKey
			{
				color = v.color,
				time = v.time
			};
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0003B4B8 File Offset: 0x000398B8
		public static implicit operator GradientColorKeySurrogate(GradientColorKey v)
		{
			return new GradientColorKeySurrogate
			{
				color = v.color,
				time = v.time
			};
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0003B4E8 File Offset: 0x000398E8
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			GradientColorKey gradientColorKey = (GradientColorKey)obj;
			info.AddValue("color", gradientColorKey.color);
			info.AddValue("time", gradientColorKey.time);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0003B528 File Offset: 0x00039928
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			GradientColorKey gradientColorKey = (GradientColorKey)obj;
			gradientColorKey.color = (Color)info.GetValue("color", typeof(Color));
			gradientColorKey.time = (float)info.GetValue("time", typeof(float));
			return gradientColorKey;
		}

		// Token: 0x04000AD4 RID: 2772
		public Color color;

		// Token: 0x04000AD5 RID: 2773
		public float time;
	}
}
