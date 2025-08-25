using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001E7 RID: 487
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class Vector3Surrogate : ISerializationSurrogate
	{
		// Token: 0x060009C9 RID: 2505 RVA: 0x0003C711 File Offset: 0x0003AB11
		public Vector3Surrogate()
		{
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0003C71C File Offset: 0x0003AB1C
		public static implicit operator Vector3(Vector3Surrogate v)
		{
			return new Vector3
			{
				x = v.x,
				y = v.y,
				z = v.z
			};
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0003C75C File Offset: 0x0003AB5C
		public static implicit operator Vector3Surrogate(Vector3 v)
		{
			return new Vector3Surrogate
			{
				x = v.x,
				y = v.y,
				z = v.z
			};
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0003C798 File Offset: 0x0003AB98
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Vector3 vector = (Vector3)obj;
			info.AddValue("x", vector.x);
			info.AddValue("y", vector.y);
			info.AddValue("z", vector.z);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0003C7E4 File Offset: 0x0003ABE4
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Vector3 vector = (Vector3)obj;
			vector.x = (float)info.GetValue("x", typeof(float));
			vector.y = (float)info.GetValue("y", typeof(float));
			vector.z = (float)info.GetValue("z", typeof(float));
			return vector;
		}

		// Token: 0x04000B05 RID: 2821
		public float x;

		// Token: 0x04000B06 RID: 2822
		public float y;

		// Token: 0x04000B07 RID: 2823
		public float z;
	}
}
