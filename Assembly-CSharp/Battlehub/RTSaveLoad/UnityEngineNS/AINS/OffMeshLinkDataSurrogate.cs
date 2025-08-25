using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine.AI;

namespace Battlehub.RTSaveLoad.UnityEngineNS.AINS
{
	// Token: 0x020001FE RID: 510
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class OffMeshLinkDataSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A38 RID: 2616 RVA: 0x0003E627 File Offset: 0x0003CA27
		public OffMeshLinkDataSurrogate()
		{
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0003E630 File Offset: 0x0003CA30
		public static implicit operator OffMeshLinkData(OffMeshLinkDataSurrogate v)
		{
			return default(OffMeshLinkData);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0003E646 File Offset: 0x0003CA46
		public static implicit operator OffMeshLinkDataSurrogate(OffMeshLinkData v)
		{
			return new OffMeshLinkDataSurrogate();
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0003E64D File Offset: 0x0003CA4D
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0003E64F File Offset: 0x0003CA4F
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return obj;
		}
	}
}
