using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001DE RID: 478
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class LayerMaskSurrogate : ISerializationSurrogate
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x0003B584 File Offset: 0x00039984
		public LayerMaskSurrogate()
		{
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0003B58C File Offset: 0x0003998C
		public static implicit operator LayerMask(LayerMaskSurrogate v)
		{
			return new LayerMask
			{
				value = v.value
			};
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0003B5B0 File Offset: 0x000399B0
		public static implicit operator LayerMaskSurrogate(LayerMask v)
		{
			return new LayerMaskSurrogate
			{
				value = v.value
			};
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0003B5D4 File Offset: 0x000399D4
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", ((LayerMask)obj).value);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0003B5FC File Offset: 0x000399FC
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			LayerMask layerMask = (LayerMask)obj;
			layerMask.value = (int)info.GetValue("value", typeof(int));
			return layerMask;
		}

		// Token: 0x04000AD6 RID: 2774
		public int value;
	}
}
