using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001FA RID: 506
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointAngleLimits2DSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A24 RID: 2596 RVA: 0x0003E0F8 File Offset: 0x0003C4F8
		public JointAngleLimits2DSurrogate()
		{
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0003E100 File Offset: 0x0003C500
		public static implicit operator JointAngleLimits2D(JointAngleLimits2DSurrogate v)
		{
			return new JointAngleLimits2D
			{
				min = v.min,
				max = v.max
			};
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0003E130 File Offset: 0x0003C530
		public static implicit operator JointAngleLimits2DSurrogate(JointAngleLimits2D v)
		{
			return new JointAngleLimits2DSurrogate
			{
				min = v.min,
				max = v.max
			};
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0003E160 File Offset: 0x0003C560
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointAngleLimits2D jointAngleLimits2D = (JointAngleLimits2D)obj;
			info.AddValue("min", jointAngleLimits2D.min);
			info.AddValue("max", jointAngleLimits2D.max);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0003E198 File Offset: 0x0003C598
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointAngleLimits2D jointAngleLimits2D = (JointAngleLimits2D)obj;
			jointAngleLimits2D.min = (float)info.GetValue("min", typeof(float));
			jointAngleLimits2D.max = (float)info.GetValue("max", typeof(float));
			return jointAngleLimits2D;
		}

		// Token: 0x04000B45 RID: 2885
		public float min;

		// Token: 0x04000B46 RID: 2886
		public float max;
	}
}
