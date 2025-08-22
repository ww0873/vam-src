using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine.Playables;

namespace Battlehub.RTSaveLoad.UnityEngineNS.ExperimentalNS.DirectorNS
{
	// Token: 0x020001FF RID: 511
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class PlayableGraphSurrogate : ISerializationSurrogate
	{
		// Token: 0x06000A3D RID: 2621 RVA: 0x0003E652 File Offset: 0x0003CA52
		public PlayableGraphSurrogate()
		{
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0003E65C File Offset: 0x0003CA5C
		public static implicit operator PlayableGraph(PlayableGraphSurrogate v)
		{
			return default(PlayableGraph);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0003E672 File Offset: 0x0003CA72
		public static implicit operator PlayableGraphSurrogate(PlayableGraph v)
		{
			return new PlayableGraphSurrogate();
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0003E679 File Offset: 0x0003CA79
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0003E67B File Offset: 0x0003CA7B
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return obj;
		}
	}
}
