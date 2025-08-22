using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001EE RID: 494
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class NetworkViewIDSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009EC RID: 2540 RVA: 0x0003D49A File Offset: 0x0003B89A
		public NetworkViewIDSurrogate()
		{
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0003D4A2 File Offset: 0x0003B8A2
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0003D4A4 File Offset: 0x0003B8A4
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return obj;
		}
	}
}
