using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F2 RID: 498
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class QuaternionSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x0003D5E9 File Offset: 0x0003B9E9
		public QuaternionSurrogate()
		{
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0003D5F4 File Offset: 0x0003B9F4
		public static implicit operator Quaternion(QuaternionSurrogate v)
		{
			return new Quaternion
			{
				x = v.x,
				y = v.y,
				z = v.z,
				w = v.w,
				eulerAngles = v.eulerAngles
			};
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0003D64C File Offset: 0x0003BA4C
		public static implicit operator QuaternionSurrogate(Quaternion v)
		{
			return new QuaternionSurrogate
			{
				x = v.x,
				y = v.y,
				z = v.z,
				w = v.w,
				eulerAngles = v.eulerAngles
			};
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0003D6A4 File Offset: 0x0003BAA4
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Quaternion quaternion = (Quaternion)obj;
			info.AddValue("x", quaternion.x);
			info.AddValue("y", quaternion.y);
			info.AddValue("z", quaternion.z);
			info.AddValue("w", quaternion.w);
			info.AddValue("eulerAngles", quaternion.eulerAngles);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0003D718 File Offset: 0x0003BB18
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Quaternion quaternion = (Quaternion)obj;
			quaternion.x = (float)info.GetValue("x", typeof(float));
			quaternion.y = (float)info.GetValue("y", typeof(float));
			quaternion.z = (float)info.GetValue("z", typeof(float));
			quaternion.w = (float)info.GetValue("w", typeof(float));
			quaternion.eulerAngles = (Vector3)info.GetValue("eulerAngles", typeof(Vector3));
			return quaternion;
		}

		// Token: 0x04000B2B RID: 2859
		public float x;

		// Token: 0x04000B2C RID: 2860
		public float y;

		// Token: 0x04000B2D RID: 2861
		public float z;

		// Token: 0x04000B2E RID: 2862
		public float w;

		// Token: 0x04000B2F RID: 2863
		public Vector3 eulerAngles;
	}
}
