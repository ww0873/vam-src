using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000234 RID: 564
	public class Serializer : ISerializer
	{
		// Token: 0x06000BC7 RID: 3015 RVA: 0x0004A3F6 File Offset: 0x000487F6
		public Serializer()
		{
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0004A3FE File Offset: 0x000487FE
		public byte[] Serialize<TData>(TData data)
		{
			return ProtobufSerializer.Serialize<TData>(data);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0004A406 File Offset: 0x00048806
		public TData Deserialize<TData>(byte[] data)
		{
			return ProtobufSerializer.Deserialize<TData>(data);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0004A40E File Offset: 0x0004880E
		public TData DeepClone<TData>(TData data)
		{
			return ProtobufSerializer.DeepClone<TData>(data);
		}
	}
}
