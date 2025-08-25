using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001EF RID: 495
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class NetworkPlayerSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009EF RID: 2543 RVA: 0x0003D4A7 File Offset: 0x0003B8A7
		public NetworkPlayerSurrogate()
		{
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0003D4AF File Offset: 0x0003B8AF
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0003D4B1 File Offset: 0x0003B8B1
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return obj;
		}
	}
}
