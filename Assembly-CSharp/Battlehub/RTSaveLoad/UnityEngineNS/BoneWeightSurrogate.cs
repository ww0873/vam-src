using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001E4 RID: 484
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class BoneWeightSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009BA RID: 2490 RVA: 0x0003BCB0 File Offset: 0x0003A0B0
		public BoneWeightSurrogate()
		{
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0003BCB8 File Offset: 0x0003A0B8
		public static implicit operator BoneWeight(BoneWeightSurrogate v)
		{
			return new BoneWeight
			{
				weight0 = v.weight0,
				weight1 = v.weight1,
				weight2 = v.weight2,
				weight3 = v.weight3,
				boneIndex0 = v.boneIndex0,
				boneIndex1 = v.boneIndex1,
				boneIndex2 = v.boneIndex2,
				boneIndex3 = v.boneIndex3
			};
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0003BD38 File Offset: 0x0003A138
		public static implicit operator BoneWeightSurrogate(BoneWeight v)
		{
			return new BoneWeightSurrogate
			{
				weight0 = v.weight0,
				weight1 = v.weight1,
				weight2 = v.weight2,
				weight3 = v.weight3,
				boneIndex0 = v.boneIndex0,
				boneIndex1 = v.boneIndex1,
				boneIndex2 = v.boneIndex2,
				boneIndex3 = v.boneIndex3
			};
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0003BDB4 File Offset: 0x0003A1B4
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			BoneWeight boneWeight = (BoneWeight)obj;
			info.AddValue("weight0", boneWeight.weight0);
			info.AddValue("weight1", boneWeight.weight1);
			info.AddValue("weight2", boneWeight.weight2);
			info.AddValue("weight3", boneWeight.weight3);
			info.AddValue("boneIndex0", boneWeight.boneIndex0);
			info.AddValue("boneIndex1", boneWeight.boneIndex1);
			info.AddValue("boneIndex2", boneWeight.boneIndex2);
			info.AddValue("boneIndex3", boneWeight.boneIndex3);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0003BE58 File Offset: 0x0003A258
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			BoneWeight boneWeight = (BoneWeight)obj;
			boneWeight.weight0 = (float)info.GetValue("weight0", typeof(float));
			boneWeight.weight1 = (float)info.GetValue("weight1", typeof(float));
			boneWeight.weight2 = (float)info.GetValue("weight2", typeof(float));
			boneWeight.weight3 = (float)info.GetValue("weight3", typeof(float));
			boneWeight.boneIndex0 = (int)info.GetValue("boneIndex0", typeof(int));
			boneWeight.boneIndex1 = (int)info.GetValue("boneIndex1", typeof(int));
			boneWeight.boneIndex2 = (int)info.GetValue("boneIndex2", typeof(int));
			boneWeight.boneIndex3 = (int)info.GetValue("boneIndex3", typeof(int));
			return boneWeight;
		}

		// Token: 0x04000AE7 RID: 2791
		public float weight0;

		// Token: 0x04000AE8 RID: 2792
		public float weight1;

		// Token: 0x04000AE9 RID: 2793
		public float weight2;

		// Token: 0x04000AEA RID: 2794
		public float weight3;

		// Token: 0x04000AEB RID: 2795
		public int boneIndex0;

		// Token: 0x04000AEC RID: 2796
		public int boneIndex1;

		// Token: 0x04000AED RID: 2797
		public int boneIndex2;

		// Token: 0x04000AEE RID: 2798
		public int boneIndex3;
	}
}
