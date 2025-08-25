using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine.AI;

namespace Battlehub.RTSaveLoad.UnityEngineNS.AINS
{
	// Token: 0x020001E2 RID: 482
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class NavMeshPathSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009B0 RID: 2480 RVA: 0x0003BB7C File Offset: 0x00039F7C
		public NavMeshPathSurrogate()
		{
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0003BB84 File Offset: 0x00039F84
		public static implicit operator NavMeshPath(NavMeshPathSurrogate v)
		{
			return new NavMeshPath();
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0003BB98 File Offset: 0x00039F98
		public static implicit operator NavMeshPathSurrogate(NavMeshPath v)
		{
			return new NavMeshPathSurrogate();
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0003BBAC File Offset: 0x00039FAC
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0003BBAE File Offset: 0x00039FAE
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return obj;
		}
	}
}
