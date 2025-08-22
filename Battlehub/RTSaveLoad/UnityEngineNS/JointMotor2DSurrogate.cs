using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F9 RID: 505
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointMotor2DSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A1F RID: 2591 RVA: 0x0003DFF9 File Offset: 0x0003C3F9
		public JointMotor2DSurrogate()
		{
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0003E004 File Offset: 0x0003C404
		public static implicit operator JointMotor2D(JointMotor2DSurrogate v)
		{
			return new JointMotor2D
			{
				motorSpeed = v.motorSpeed,
				maxMotorTorque = v.maxMotorTorque
			};
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0003E034 File Offset: 0x0003C434
		public static implicit operator JointMotor2DSurrogate(JointMotor2D v)
		{
			return new JointMotor2DSurrogate
			{
				motorSpeed = v.motorSpeed,
				maxMotorTorque = v.maxMotorTorque
			};
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0003E064 File Offset: 0x0003C464
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointMotor2D jointMotor2D = (JointMotor2D)obj;
			info.AddValue("motorSpeed", jointMotor2D.motorSpeed);
			info.AddValue("maxMotorTorque", jointMotor2D.maxMotorTorque);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0003E09C File Offset: 0x0003C49C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointMotor2D jointMotor2D = (JointMotor2D)obj;
			jointMotor2D.motorSpeed = (float)info.GetValue("motorSpeed", typeof(float));
			jointMotor2D.maxMotorTorque = (float)info.GetValue("maxMotorTorque", typeof(float));
			return jointMotor2D;
		}

		// Token: 0x04000B43 RID: 2883
		public float motorSpeed;

		// Token: 0x04000B44 RID: 2884
		public float maxMotorTorque;
	}
}
