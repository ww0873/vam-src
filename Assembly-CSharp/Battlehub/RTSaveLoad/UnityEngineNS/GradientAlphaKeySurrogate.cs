using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001DC RID: 476
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class GradientAlphaKeySurrogate : ISerializationSurrogate
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x0003B383 File Offset: 0x00039783
		public GradientAlphaKeySurrogate()
		{
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0003B38C File Offset: 0x0003978C
		public static implicit operator GradientAlphaKey(GradientAlphaKeySurrogate v)
		{
			return new GradientAlphaKey
			{
				alpha = v.alpha,
				time = v.time
			};
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0003B3BC File Offset: 0x000397BC
		public static implicit operator GradientAlphaKeySurrogate(GradientAlphaKey v)
		{
			return new GradientAlphaKeySurrogate
			{
				alpha = v.alpha,
				time = v.time
			};
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0003B3EC File Offset: 0x000397EC
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			GradientAlphaKey gradientAlphaKey = (GradientAlphaKey)obj;
			info.AddValue("alpha", gradientAlphaKey.alpha);
			info.AddValue("time", gradientAlphaKey.time);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0003B424 File Offset: 0x00039824
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			GradientAlphaKey gradientAlphaKey = (GradientAlphaKey)obj;
			gradientAlphaKey.alpha = (float)info.GetValue("alpha", typeof(float));
			gradientAlphaKey.time = (float)info.GetValue("time", typeof(float));
			return gradientAlphaKey;
		}

		// Token: 0x04000AD2 RID: 2770
		public float alpha;

		// Token: 0x04000AD3 RID: 2771
		public float time;
	}
}
