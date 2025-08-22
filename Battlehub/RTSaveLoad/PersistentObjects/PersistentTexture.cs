using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000218 RID: 536
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1074, typeof(PersistentProceduralTexture))]
	[ProtoInclude(1075, typeof(PersistentTexture2D))]
	[ProtoInclude(1076, typeof(PersistentCubemap))]
	[ProtoInclude(1077, typeof(PersistentTexture3D))]
	[ProtoInclude(1078, typeof(PersistentTexture2DArray))]
	[ProtoInclude(1079, typeof(PersistentCubemapArray))]
	[ProtoInclude(1080, typeof(PersistentSparseTexture))]
	[ProtoInclude(1081, typeof(PersistentRenderTexture))]
	[ProtoInclude(1082, typeof(PersistentMovieTexture))]
	[ProtoInclude(1083, typeof(PersistentWebCamTexture))]
	[Serializable]
	public class PersistentTexture : PersistentObject
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x00034201 File Offset: 0x00032601
		public PersistentTexture()
		{
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0003420C File Offset: 0x0003260C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Texture texture = (Texture)obj;
			texture.filterMode = this.filterMode;
			texture.anisoLevel = this.anisoLevel;
			texture.wrapMode = this.wrapMode;
			texture.mipMapBias = this.mipMapBias;
			return texture;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00034264 File Offset: 0x00032664
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Texture texture = (Texture)obj;
			this.filterMode = texture.filterMode;
			this.anisoLevel = texture.anisoLevel;
			this.wrapMode = texture.wrapMode;
			this.mipMapBias = texture.mipMapBias;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x000342B6 File Offset: 0x000326B6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000BFC RID: 3068
		public int width;

		// Token: 0x04000BFD RID: 3069
		public int height;

		// Token: 0x04000BFE RID: 3070
		public TextureDimension dimension;

		// Token: 0x04000BFF RID: 3071
		public FilterMode filterMode;

		// Token: 0x04000C00 RID: 3072
		public int anisoLevel;

		// Token: 0x04000C01 RID: 3073
		public TextureWrapMode wrapMode;

		// Token: 0x04000C02 RID: 3074
		public float mipMapBias;
	}
}
