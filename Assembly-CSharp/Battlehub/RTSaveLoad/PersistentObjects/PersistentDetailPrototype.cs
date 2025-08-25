using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000160 RID: 352
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentDetailPrototype : PersistentData
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x000342D1 File Offset: 0x000326D1
		public PersistentDetailPrototype()
		{
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000342DC File Offset: 0x000326DC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			DetailPrototype detailPrototype = (DetailPrototype)obj;
			detailPrototype.prototype = (GameObject)objects.Get(this.prototype);
			detailPrototype.prototypeTexture = (Texture2D)objects.Get(this.prototypeTexture);
			detailPrototype.minWidth = this.minWidth;
			detailPrototype.maxWidth = this.maxWidth;
			detailPrototype.minHeight = this.minHeight;
			detailPrototype.maxHeight = this.maxHeight;
			detailPrototype.noiseSpread = this.noiseSpread;
			detailPrototype.bendFactor = this.bendFactor;
			detailPrototype.healthyColor = this.healthyColor;
			detailPrototype.dryColor = this.dryColor;
			detailPrototype.renderMode = (DetailRenderMode)this.renderMode;
			detailPrototype.usePrototypeMesh = this.usePrototypeMesh;
			return detailPrototype;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000343AC File Offset: 0x000327AC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			DetailPrototype detailPrototype = (DetailPrototype)obj;
			this.prototype = detailPrototype.prototype.GetMappedInstanceID();
			this.prototypeTexture = detailPrototype.prototypeTexture.GetMappedInstanceID();
			this.minWidth = detailPrototype.minWidth;
			this.maxWidth = detailPrototype.maxWidth;
			this.minHeight = detailPrototype.minHeight;
			this.maxHeight = detailPrototype.maxHeight;
			this.noiseSpread = detailPrototype.noiseSpread;
			this.bendFactor = detailPrototype.bendFactor;
			this.healthyColor = detailPrototype.healthyColor;
			this.dryColor = detailPrototype.dryColor;
			this.renderMode = (uint)detailPrototype.renderMode;
			this.usePrototypeMesh = detailPrototype.usePrototypeMesh;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00034468 File Offset: 0x00032868
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.prototype, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.prototypeTexture, dependencies, objects, allowNulls);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00034494 File Offset: 0x00032894
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			DetailPrototype detailPrototype = (DetailPrototype)obj;
			base.AddDependency(detailPrototype.prototype, dependencies);
			base.AddDependency(detailPrototype.prototypeTexture, dependencies);
		}

		// Token: 0x04000880 RID: 2176
		public long prototype;

		// Token: 0x04000881 RID: 2177
		public long prototypeTexture;

		// Token: 0x04000882 RID: 2178
		public float minWidth;

		// Token: 0x04000883 RID: 2179
		public float maxWidth;

		// Token: 0x04000884 RID: 2180
		public float minHeight;

		// Token: 0x04000885 RID: 2181
		public float maxHeight;

		// Token: 0x04000886 RID: 2182
		public float noiseSpread;

		// Token: 0x04000887 RID: 2183
		public float bendFactor;

		// Token: 0x04000888 RID: 2184
		public Color healthyColor;

		// Token: 0x04000889 RID: 2185
		public Color dryColor;

		// Token: 0x0400088A RID: 2186
		public uint renderMode;

		// Token: 0x0400088B RID: 2187
		public bool usePrototypeMesh;
	}
}
