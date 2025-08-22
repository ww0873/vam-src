using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200017F RID: 383
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLensFlare : PersistentBehaviour
	{
		// Token: 0x06000857 RID: 2135 RVA: 0x0003610D File Offset: 0x0003450D
		public PersistentLensFlare()
		{
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00036118 File Offset: 0x00034518
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			LensFlare lensFlare = (LensFlare)obj;
			lensFlare.flare = (Flare)objects.Get(this.flare);
			lensFlare.brightness = this.brightness;
			lensFlare.fadeSpeed = this.fadeSpeed;
			lensFlare.color = this.color;
			return lensFlare;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0003617C File Offset: 0x0003457C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			LensFlare lensFlare = (LensFlare)obj;
			this.flare = lensFlare.flare.GetMappedInstanceID();
			this.brightness = lensFlare.brightness;
			this.fadeSpeed = lensFlare.fadeSpeed;
			this.color = lensFlare.color;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000361D3 File Offset: 0x000345D3
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.flare, dependencies, objects, allowNulls);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x000361F0 File Offset: 0x000345F0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			LensFlare lensFlare = (LensFlare)obj;
			base.AddDependency(lensFlare.flare, dependencies);
		}

		// Token: 0x0400090D RID: 2317
		public long flare;

		// Token: 0x0400090E RID: 2318
		public float brightness;

		// Token: 0x0400090F RID: 2319
		public float fadeSpeed;

		// Token: 0x04000910 RID: 2320
		public Color color;
	}
}
