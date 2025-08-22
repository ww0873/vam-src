using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F4 RID: 500
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointLimitsSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x0003D925 File Offset: 0x0003BD25
		public JointLimitsSurrogate()
		{
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0003D930 File Offset: 0x0003BD30
		public static implicit operator JointLimits(JointLimitsSurrogate v)
		{
			return new JointLimits
			{
				min = v.min,
				max = v.max,
				bounciness = v.bounciness,
				bounceMinVelocity = v.bounceMinVelocity,
				contactDistance = v.contactDistance
			};
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0003D988 File Offset: 0x0003BD88
		public static implicit operator JointLimitsSurrogate(JointLimits v)
		{
			return new JointLimitsSurrogate
			{
				min = v.min,
				max = v.max,
				bounciness = v.bounciness,
				bounceMinVelocity = v.bounceMinVelocity,
				contactDistance = v.contactDistance
			};
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0003D9E0 File Offset: 0x0003BDE0
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointLimits jointLimits = (JointLimits)obj;
			info.AddValue("min", jointLimits.min);
			info.AddValue("max", jointLimits.max);
			info.AddValue("bounciness", jointLimits.bounciness);
			info.AddValue("bounceMinVelocity", jointLimits.bounceMinVelocity);
			info.AddValue("contactDistance", jointLimits.contactDistance);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0003DA50 File Offset: 0x0003BE50
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointLimits jointLimits = (JointLimits)obj;
			jointLimits.min = (float)info.GetValue("min", typeof(float));
			jointLimits.max = (float)info.GetValue("max", typeof(float));
			jointLimits.bounciness = (float)info.GetValue("bounciness", typeof(float));
			jointLimits.bounceMinVelocity = (float)info.GetValue("bounceMinVelocity", typeof(float));
			jointLimits.contactDistance = (float)info.GetValue("contactDistance", typeof(float));
			return jointLimits;
		}

		// Token: 0x04000B33 RID: 2867
		public float min;

		// Token: 0x04000B34 RID: 2868
		public float max;

		// Token: 0x04000B35 RID: 2869
		public float bounciness;

		// Token: 0x04000B36 RID: 2870
		public float bounceMinVelocity;

		// Token: 0x04000B37 RID: 2871
		public float contactDistance;
	}
}
