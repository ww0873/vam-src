using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F8 RID: 504
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class SoftJointLimitSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x0003DEAC File Offset: 0x0003C2AC
		public SoftJointLimitSurrogate()
		{
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0003DEB4 File Offset: 0x0003C2B4
		public static implicit operator SoftJointLimit(SoftJointLimitSurrogate v)
		{
			return new SoftJointLimit
			{
				limit = v.limit,
				bounciness = v.bounciness,
				contactDistance = v.contactDistance
			};
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0003DEF4 File Offset: 0x0003C2F4
		public static implicit operator SoftJointLimitSurrogate(SoftJointLimit v)
		{
			return new SoftJointLimitSurrogate
			{
				limit = v.limit,
				bounciness = v.bounciness,
				contactDistance = v.contactDistance
			};
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0003DF30 File Offset: 0x0003C330
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			SoftJointLimit softJointLimit = (SoftJointLimit)obj;
			info.AddValue("limit", softJointLimit.limit);
			info.AddValue("bounciness", softJointLimit.bounciness);
			info.AddValue("contactDistance", softJointLimit.contactDistance);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0003DF7C File Offset: 0x0003C37C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			SoftJointLimit softJointLimit = (SoftJointLimit)obj;
			softJointLimit.limit = (float)info.GetValue("limit", typeof(float));
			softJointLimit.bounciness = (float)info.GetValue("bounciness", typeof(float));
			softJointLimit.contactDistance = (float)info.GetValue("contactDistance", typeof(float));
			return softJointLimit;
		}

		// Token: 0x04000B40 RID: 2880
		public float limit;

		// Token: 0x04000B41 RID: 2881
		public float bounciness;

		// Token: 0x04000B42 RID: 2882
		public float contactDistance;
	}
}
