using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001E3 RID: 483
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class ClothSkinningCoefficientSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009B5 RID: 2485 RVA: 0x0003BBB1 File Offset: 0x00039FB1
		public ClothSkinningCoefficientSurrogate()
		{
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0003BBBC File Offset: 0x00039FBC
		public static implicit operator ClothSkinningCoefficient(ClothSkinningCoefficientSurrogate v)
		{
			return new ClothSkinningCoefficient
			{
				maxDistance = v.maxDistance,
				collisionSphereDistance = v.collisionSphereDistance
			};
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0003BBEC File Offset: 0x00039FEC
		public static implicit operator ClothSkinningCoefficientSurrogate(ClothSkinningCoefficient v)
		{
			return new ClothSkinningCoefficientSurrogate
			{
				maxDistance = v.maxDistance,
				collisionSphereDistance = v.collisionSphereDistance
			};
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0003BC1C File Offset: 0x0003A01C
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			ClothSkinningCoefficient clothSkinningCoefficient = (ClothSkinningCoefficient)obj;
			info.AddValue("maxDistance", clothSkinningCoefficient.maxDistance);
			info.AddValue("collisionSphereDistance", clothSkinningCoefficient.collisionSphereDistance);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0003BC54 File Offset: 0x0003A054
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			ClothSkinningCoefficient clothSkinningCoefficient = (ClothSkinningCoefficient)obj;
			clothSkinningCoefficient.maxDistance = (float)info.GetValue("maxDistance", typeof(float));
			clothSkinningCoefficient.collisionSphereDistance = (float)info.GetValue("collisionSphereDistance", typeof(float));
			return clothSkinningCoefficient;
		}

		// Token: 0x04000AE5 RID: 2789
		public float maxDistance;

		// Token: 0x04000AE6 RID: 2790
		public float collisionSphereDistance;
	}
}
