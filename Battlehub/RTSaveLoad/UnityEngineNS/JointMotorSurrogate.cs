using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F3 RID: 499
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointMotorSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A01 RID: 2561 RVA: 0x0003D7D7 File Offset: 0x0003BBD7
		public JointMotorSurrogate()
		{
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0003D7E0 File Offset: 0x0003BBE0
		public static implicit operator JointMotor(JointMotorSurrogate v)
		{
			return new JointMotor
			{
				targetVelocity = v.targetVelocity,
				force = v.force,
				freeSpin = v.freeSpin
			};
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0003D820 File Offset: 0x0003BC20
		public static implicit operator JointMotorSurrogate(JointMotor v)
		{
			return new JointMotorSurrogate
			{
				targetVelocity = v.targetVelocity,
				force = v.force,
				freeSpin = v.freeSpin
			};
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0003D85C File Offset: 0x0003BC5C
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointMotor jointMotor = (JointMotor)obj;
			info.AddValue("targetVelocity", jointMotor.targetVelocity);
			info.AddValue("force", jointMotor.force);
			info.AddValue("freeSpin", jointMotor.freeSpin);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0003D8A8 File Offset: 0x0003BCA8
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointMotor jointMotor = (JointMotor)obj;
			jointMotor.targetVelocity = (float)info.GetValue("targetVelocity", typeof(float));
			jointMotor.force = (float)info.GetValue("force", typeof(float));
			jointMotor.freeSpin = (bool)info.GetValue("freeSpin", typeof(bool));
			return jointMotor;
		}

		// Token: 0x04000B30 RID: 2864
		public float targetVelocity;

		// Token: 0x04000B31 RID: 2865
		public float force;

		// Token: 0x04000B32 RID: 2866
		public bool freeSpin;
	}
}
