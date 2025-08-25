using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F5 RID: 501
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointSpringSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A0B RID: 2571 RVA: 0x0003DB0F File Offset: 0x0003BF0F
		public JointSpringSurrogate()
		{
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0003DB18 File Offset: 0x0003BF18
		public static implicit operator JointSpring(JointSpringSurrogate v)
		{
			return new JointSpring
			{
				spring = v.spring,
				damper = v.damper,
				targetPosition = v.targetPosition
			};
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0003DB58 File Offset: 0x0003BF58
		public static implicit operator JointSpringSurrogate(JointSpring v)
		{
			return new JointSpringSurrogate
			{
				spring = v.spring,
				damper = v.damper,
				targetPosition = v.targetPosition
			};
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0003DB94 File Offset: 0x0003BF94
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointSpring jointSpring = (JointSpring)obj;
			info.AddValue("spring", jointSpring.spring);
			info.AddValue("damper", jointSpring.damper);
			info.AddValue("targetPosition", jointSpring.targetPosition);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0003DBE0 File Offset: 0x0003BFE0
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointSpring jointSpring = (JointSpring)obj;
			jointSpring.spring = (float)info.GetValue("spring", typeof(float));
			jointSpring.damper = (float)info.GetValue("damper", typeof(float));
			jointSpring.targetPosition = (float)info.GetValue("targetPosition", typeof(float));
			return jointSpring;
		}

		// Token: 0x04000B38 RID: 2872
		public float spring;

		// Token: 0x04000B39 RID: 2873
		public float damper;

		// Token: 0x04000B3A RID: 2874
		public float targetPosition;
	}
}
