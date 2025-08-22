using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001FB RID: 507
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class JointTranslationLimits2DSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A29 RID: 2601 RVA: 0x0003E1F4 File Offset: 0x0003C5F4
		public JointTranslationLimits2DSurrogate()
		{
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0003E1FC File Offset: 0x0003C5FC
		public static implicit operator JointTranslationLimits2D(JointTranslationLimits2DSurrogate v)
		{
			return new JointTranslationLimits2D
			{
				min = v.min,
				max = v.max
			};
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0003E22C File Offset: 0x0003C62C
		public static implicit operator JointTranslationLimits2DSurrogate(JointTranslationLimits2D v)
		{
			return new JointTranslationLimits2DSurrogate
			{
				min = v.min,
				max = v.max
			};
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0003E25C File Offset: 0x0003C65C
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			JointTranslationLimits2D jointTranslationLimits2D = (JointTranslationLimits2D)obj;
			info.AddValue("min", jointTranslationLimits2D.min);
			info.AddValue("max", jointTranslationLimits2D.max);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0003E294 File Offset: 0x0003C694
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			JointTranslationLimits2D jointTranslationLimits2D = (JointTranslationLimits2D)obj;
			jointTranslationLimits2D.min = (float)info.GetValue("min", typeof(float));
			jointTranslationLimits2D.max = (float)info.GetValue("max", typeof(float));
			return jointTranslationLimits2D;
		}

		// Token: 0x04000B47 RID: 2887
		public float min;

		// Token: 0x04000B48 RID: 2888
		public float max;
	}
}
