using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200014D RID: 333
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCanvasRenderer : PersistentComponent
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x00033409 File Offset: 0x00031809
		public PersistentCanvasRenderer()
		{
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00033414 File Offset: 0x00031814
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CanvasRenderer canvasRenderer = (CanvasRenderer)obj;
			canvasRenderer.hasPopInstruction = this.hasPopInstruction;
			canvasRenderer.materialCount = this.materialCount;
			canvasRenderer.popMaterialCount = this.popMaterialCount;
			canvasRenderer.cull = this.cull;
			return canvasRenderer;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0003346C File Offset: 0x0003186C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CanvasRenderer canvasRenderer = (CanvasRenderer)obj;
			this.hasPopInstruction = canvasRenderer.hasPopInstruction;
			this.materialCount = canvasRenderer.materialCount;
			this.popMaterialCount = canvasRenderer.popMaterialCount;
			this.cull = canvasRenderer.cull;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000334BE File Offset: 0x000318BE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400081C RID: 2076
		public bool hasPopInstruction;

		// Token: 0x0400081D RID: 2077
		public int materialCount;

		// Token: 0x0400081E RID: 2078
		public int popMaterialCount;

		// Token: 0x0400081F RID: 2079
		public bool cull;
	}
}
