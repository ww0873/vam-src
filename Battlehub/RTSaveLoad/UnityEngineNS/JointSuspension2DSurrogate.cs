using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001FC RID: 508
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointSuspension2DSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A2E RID: 2606 RVA: 0x0003E2F0 File Offset: 0x0003C6F0
		public JointSuspension2DSurrogate()
		{
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0003E2F8 File Offset: 0x0003C6F8
		public static implicit operator JointSuspension2D(JointSuspension2DSurrogate v)
		{
			return new JointSuspension2D
			{
				dampingRatio = v.dampingRatio,
				frequency = v.frequency,
				angle = v.angle
			};
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0003E338 File Offset: 0x0003C738
		public static implicit operator JointSuspension2DSurrogate(JointSuspension2D v)
		{
			return new JointSuspension2DSurrogate
			{
				dampingRatio = v.dampingRatio,
				frequency = v.frequency,
				angle = v.angle
			};
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0003E374 File Offset: 0x0003C774
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointSuspension2D jointSuspension2D = (JointSuspension2D)obj;
			info.AddValue("dampingRatio", jointSuspension2D.dampingRatio);
			info.AddValue("frequency", jointSuspension2D.frequency);
			info.AddValue("angle", jointSuspension2D.angle);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0003E3C0 File Offset: 0x0003C7C0
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointSuspension2D jointSuspension2D = (JointSuspension2D)obj;
			jointSuspension2D.dampingRatio = (float)info.GetValue("dampingRatio", typeof(float));
			jointSuspension2D.frequency = (float)info.GetValue("frequency", typeof(float));
			jointSuspension2D.angle = (float)info.GetValue("angle", typeof(float));
			return jointSuspension2D;
		}

		// Token: 0x04000B49 RID: 2889
		public float dampingRatio;

		// Token: 0x04000B4A RID: 2890
		public float frequency;

		// Token: 0x04000B4B RID: 2891
		public float angle;
	}
}
