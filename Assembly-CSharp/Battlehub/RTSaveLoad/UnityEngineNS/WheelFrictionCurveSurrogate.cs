using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001FD RID: 509
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class WheelFrictionCurveSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x0003E43D File Offset: 0x0003C83D
		public WheelFrictionCurveSurrogate()
		{
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0003E448 File Offset: 0x0003C848
		public static implicit operator WheelFrictionCurve(WheelFrictionCurveSurrogate v)
		{
			return new WheelFrictionCurve
			{
				extremumSlip = v.extremumSlip,
				extremumValue = v.extremumValue,
				asymptoteSlip = v.asymptoteSlip,
				asymptoteValue = v.asymptoteValue,
				stiffness = v.stiffness
			};
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0003E4A0 File Offset: 0x0003C8A0
		public static implicit operator WheelFrictionCurveSurrogate(WheelFrictionCurve v)
		{
			return new WheelFrictionCurveSurrogate
			{
				extremumSlip = v.extremumSlip,
				extremumValue = v.extremumValue,
				asymptoteSlip = v.asymptoteSlip,
				asymptoteValue = v.asymptoteValue,
				stiffness = v.stiffness
			};
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0003E4F8 File Offset: 0x0003C8F8
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			WheelFrictionCurve wheelFrictionCurve = (WheelFrictionCurve)obj;
			info.AddValue("extremumSlip", wheelFrictionCurve.extremumSlip);
			info.AddValue("extremumValue", wheelFrictionCurve.extremumValue);
			info.AddValue("asymptoteSlip", wheelFrictionCurve.asymptoteSlip);
			info.AddValue("asymptoteValue", wheelFrictionCurve.asymptoteValue);
			info.AddValue("stiffness", wheelFrictionCurve.stiffness);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0003E568 File Offset: 0x0003C968
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			WheelFrictionCurve wheelFrictionCurve = (WheelFrictionCurve)obj;
			wheelFrictionCurve.extremumSlip = (float)info.GetValue("extremumSlip", typeof(float));
			wheelFrictionCurve.extremumValue = (float)info.GetValue("extremumValue", typeof(float));
			wheelFrictionCurve.asymptoteSlip = (float)info.GetValue("asymptoteSlip", typeof(float));
			wheelFrictionCurve.asymptoteValue = (float)info.GetValue("asymptoteValue", typeof(float));
			wheelFrictionCurve.stiffness = (float)info.GetValue("stiffness", typeof(float));
			return wheelFrictionCurve;
		}

		// Token: 0x04000B4C RID: 2892
		public float extremumSlip;

		// Token: 0x04000B4D RID: 2893
		public float extremumValue;

		// Token: 0x04000B4E RID: 2894
		public float asymptoteSlip;

		// Token: 0x04000B4F RID: 2895
		public float asymptoteValue;

		// Token: 0x04000B50 RID: 2896
		public float stiffness;
	}
}
