using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F7 RID: 503
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class SoftJointLimitSpringSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A15 RID: 2581 RVA: 0x0003DDAD File Offset: 0x0003C1AD
		public SoftJointLimitSpringSurrogate()
		{
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0003DDB8 File Offset: 0x0003C1B8
		public static implicit operator SoftJointLimitSpring(SoftJointLimitSpringSurrogate v)
		{
			return new SoftJointLimitSpring
			{
				spring = v.spring,
				damper = v.damper
			};
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0003DDE8 File Offset: 0x0003C1E8
		public static implicit operator SoftJointLimitSpringSurrogate(SoftJointLimitSpring v)
		{
			return new SoftJointLimitSpringSurrogate
			{
				spring = v.spring,
				damper = v.damper
			};
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0003DE18 File Offset: 0x0003C218
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			SoftJointLimitSpring softJointLimitSpring = (SoftJointLimitSpring)obj;
			info.AddValue("spring", softJointLimitSpring.spring);
			info.AddValue("damper", softJointLimitSpring.damper);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0003DE50 File Offset: 0x0003C250
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			SoftJointLimitSpring softJointLimitSpring = (SoftJointLimitSpring)obj;
			softJointLimitSpring.spring = (float)info.GetValue("spring", typeof(float));
			softJointLimitSpring.damper = (float)info.GetValue("damper", typeof(float));
			return softJointLimitSpring;
		}

		// Token: 0x04000B3E RID: 2878
		public float spring;

		// Token: 0x04000B3F RID: 2879
		public float damper;
	}
}
