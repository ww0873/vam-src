using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001F1 RID: 497
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class RenderBufferSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009F7 RID: 2551 RVA: 0x0003D5B0 File Offset: 0x0003B9B0
		public RenderBufferSurrogate()
		{
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003D5B8 File Offset: 0x0003B9B8
		public static implicit operator RenderBuffer(RenderBufferSurrogate v)
		{
			return default(RenderBuffer);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0003D5D0 File Offset: 0x0003B9D0
		public static implicit operator RenderBufferSurrogate(RenderBuffer v)
		{
			return new RenderBufferSurrogate();
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0003D5E4 File Offset: 0x0003B9E4
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0003D5E6 File Offset: 0x0003B9E6
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return obj;
		}
	}
}
