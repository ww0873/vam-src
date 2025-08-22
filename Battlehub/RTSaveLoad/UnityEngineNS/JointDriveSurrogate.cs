using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F6 RID: 502
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointDriveSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x0003DC5D File Offset: 0x0003C05D
		public JointDriveSurrogate()
		{
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0003DC68 File Offset: 0x0003C068
		public static implicit operator JointDrive(JointDriveSurrogate v)
		{
			return new JointDrive
			{
				positionSpring = v.positionSpring,
				positionDamper = v.positionDamper,
				maximumForce = v.maximumForce
			};
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0003DCA8 File Offset: 0x0003C0A8
		public static implicit operator JointDriveSurrogate(JointDrive v)
		{
			return new JointDriveSurrogate
			{
				positionSpring = v.positionSpring,
				positionDamper = v.positionDamper,
				maximumForce = v.maximumForce
			};
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0003DCE4 File Offset: 0x0003C0E4
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointDrive jointDrive = (JointDrive)obj;
			info.AddValue("positionSpring", jointDrive.positionSpring);
			info.AddValue("positionDamper", jointDrive.positionDamper);
			info.AddValue("maximumForce", jointDrive.maximumForce);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0003DD30 File Offset: 0x0003C130
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointDrive jointDrive = (JointDrive)obj;
			jointDrive.positionSpring = (float)info.GetValue("positionSpring", typeof(float));
			jointDrive.positionDamper = (float)info.GetValue("positionDamper", typeof(float));
			jointDrive.maximumForce = (float)info.GetValue("maximumForce", typeof(float));
			return jointDrive;
		}

		// Token: 0x04000B3B RID: 2875
		public float positionSpring;

		// Token: 0x04000B3C RID: 2876
		public float positionDamper;

		// Token: 0x04000B3D RID: 2877
		public float maximumForce;
	}
}
