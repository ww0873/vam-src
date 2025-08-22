using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001AE RID: 430
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRenderTexture : PersistentTexture
	{
		// Token: 0x060008F3 RID: 2291 RVA: 0x000383B5 File Offset: 0x000367B5
		public PersistentRenderTexture()
		{
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000383C0 File Offset: 0x000367C0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			RenderTexture renderTexture = (RenderTexture)obj;
			renderTexture.depth = this.depth;
			renderTexture.isPowerOfTwo = this.isPowerOfTwo;
			renderTexture.format = (RenderTextureFormat)this.format;
			renderTexture.useMipMap = this.useMipMap;
			renderTexture.autoGenerateMips = this.autoGenerateMips;
			renderTexture.volumeDepth = this.volumeDepth;
			renderTexture.antiAliasing = this.antiAliasing;
			renderTexture.enableRandomWrite = this.enableRandomWrite;
			return renderTexture;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00038448 File Offset: 0x00036848
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			RenderTexture renderTexture = (RenderTexture)obj;
			this.depth = renderTexture.depth;
			this.isPowerOfTwo = renderTexture.isPowerOfTwo;
			this.format = (uint)renderTexture.format;
			this.useMipMap = renderTexture.useMipMap;
			this.autoGenerateMips = renderTexture.autoGenerateMips;
			this.volumeDepth = renderTexture.volumeDepth;
			this.antiAliasing = renderTexture.antiAliasing;
			this.enableRandomWrite = renderTexture.enableRandomWrite;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000384CA File Offset: 0x000368CA
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040009DC RID: 2524
		public int depth;

		// Token: 0x040009DD RID: 2525
		public bool isPowerOfTwo;

		// Token: 0x040009DE RID: 2526
		public uint format;

		// Token: 0x040009DF RID: 2527
		public bool useMipMap;

		// Token: 0x040009E0 RID: 2528
		public bool autoGenerateMips;

		// Token: 0x040009E1 RID: 2529
		public int volumeDepth;

		// Token: 0x040009E2 RID: 2530
		public int antiAliasing;

		// Token: 0x040009E3 RID: 2531
		public bool enableRandomWrite;
	}
}
