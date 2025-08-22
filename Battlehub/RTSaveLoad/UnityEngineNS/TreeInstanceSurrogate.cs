using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001E5 RID: 485
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class TreeInstanceSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009BF RID: 2495 RVA: 0x0003BF7A File Offset: 0x0003A37A
		public TreeInstanceSurrogate()
		{
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0003BF84 File Offset: 0x0003A384
		public static implicit operator TreeInstance(TreeInstanceSurrogate v)
		{
			return new TreeInstance
			{
				position = v.position,
				widthScale = v.widthScale,
				heightScale = v.heightScale,
				rotation = v.rotation,
				color = v.color,
				lightmapColor = v.lightmapColor,
				prototypeIndex = v.prototypeIndex
			};
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0003BFF8 File Offset: 0x0003A3F8
		public static implicit operator TreeInstanceSurrogate(TreeInstance v)
		{
			return new TreeInstanceSurrogate
			{
				position = v.position,
				widthScale = v.widthScale,
				heightScale = v.heightScale,
				rotation = v.rotation,
				color = v.color,
				lightmapColor = v.lightmapColor,
				prototypeIndex = v.prototypeIndex
			};
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0003C068 File Offset: 0x0003A468
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			TreeInstance treeInstance = (TreeInstance)obj;
			info.AddValue("position", treeInstance.position);
			info.AddValue("widthScale", treeInstance.widthScale);
			info.AddValue("heightScale", treeInstance.heightScale);
			info.AddValue("rotation", treeInstance.rotation);
			info.AddValue("color", treeInstance.color);
			info.AddValue("lightmapColor", treeInstance.lightmapColor);
			info.AddValue("prototypeIndex", treeInstance.prototypeIndex);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0003C10C File Offset: 0x0003A50C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			TreeInstance treeInstance = (TreeInstance)obj;
			treeInstance.position = (Vector3)info.GetValue("position", typeof(Vector3));
			treeInstance.widthScale = (float)info.GetValue("widthScale", typeof(float));
			treeInstance.heightScale = (float)info.GetValue("heightScale", typeof(float));
			treeInstance.rotation = (float)info.GetValue("rotation", typeof(float));
			treeInstance.color = (Color32)info.GetValue("color", typeof(Color32));
			treeInstance.lightmapColor = (Color32)info.GetValue("lightmapColor", typeof(Color32));
			treeInstance.prototypeIndex = (int)info.GetValue("prototypeIndex", typeof(int));
			return treeInstance;
		}

		// Token: 0x04000AEF RID: 2799
		public Vector3 position;

		// Token: 0x04000AF0 RID: 2800
		public float widthScale;

		// Token: 0x04000AF1 RID: 2801
		public float heightScale;

		// Token: 0x04000AF2 RID: 2802
		public float rotation;

		// Token: 0x04000AF3 RID: 2803
		public Color32 color;

		// Token: 0x04000AF4 RID: 2804
		public Color32 lightmapColor;

		// Token: 0x04000AF5 RID: 2805
		public int prototypeIndex;
	}
}
