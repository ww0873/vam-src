using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200018D RID: 397
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMovieTexture : PersistentTexture
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x00036CD0 File Offset: 0x000350D0
		public PersistentMovieTexture()
		{
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00036CD8 File Offset: 0x000350D8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			MovieTexture movieTexture = (MovieTexture)obj;
			movieTexture.loop = this.loop;
			return movieTexture;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00036D0C File Offset: 0x0003510C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			MovieTexture movieTexture = (MovieTexture)obj;
			this.loop = movieTexture.loop;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00036D3A File Offset: 0x0003513A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400094B RID: 2379
		public bool loop;
	}
}
