using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001CF RID: 463
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTrailRenderer : PersistentRenderer
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x0003A45D File Offset: 0x0003885D
		public PersistentTrailRenderer()
		{
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0003A468 File Offset: 0x00038868
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			TrailRenderer trailRenderer = (TrailRenderer)obj;
			trailRenderer.time = this.time;
			trailRenderer.startWidth = this.startWidth;
			trailRenderer.endWidth = this.endWidth;
			trailRenderer.widthCurve = base.Write<AnimationCurve>(trailRenderer.widthCurve, this.widthCurve, objects);
			trailRenderer.widthMultiplier = this.widthMultiplier;
			trailRenderer.startColor = this.startColor;
			trailRenderer.endColor = this.endColor;
			trailRenderer.colorGradient = base.Write<Gradient>(trailRenderer.colorGradient, this.colorGradient, objects);
			trailRenderer.autodestruct = this.autodestruct;
			trailRenderer.numCornerVertices = this.numCornerVertices;
			trailRenderer.numCapVertices = this.numCapVertices;
			trailRenderer.minVertexDistance = this.minVertexDistance;
			trailRenderer.textureMode = (LineTextureMode)this.textureMode;
			trailRenderer.alignment = (LineAlignment)this.alignment;
			return trailRenderer;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0003A554 File Offset: 0x00038954
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			TrailRenderer trailRenderer = (TrailRenderer)obj;
			this.time = trailRenderer.time;
			this.startWidth = trailRenderer.startWidth;
			this.endWidth = trailRenderer.endWidth;
			this.widthCurve = base.Read<PersistentAnimationCurve>(this.widthCurve, trailRenderer.widthCurve);
			this.widthMultiplier = trailRenderer.widthMultiplier;
			this.startColor = trailRenderer.startColor;
			this.endColor = trailRenderer.endColor;
			this.colorGradient = base.Read<PersistentGradient>(this.colorGradient, trailRenderer.colorGradient);
			this.autodestruct = trailRenderer.autodestruct;
			this.numCornerVertices = trailRenderer.numCornerVertices;
			this.numCapVertices = trailRenderer.numCapVertices;
			this.minVertexDistance = trailRenderer.minVertexDistance;
			this.textureMode = (uint)trailRenderer.textureMode;
			this.alignment = (uint)trailRenderer.alignment;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0003A636 File Offset: 0x00038A36
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentAnimationCurve>(this.widthCurve, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGradient>(this.colorGradient, dependencies, objects, allowNulls);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0003A660 File Offset: 0x00038A60
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			TrailRenderer trailRenderer = (TrailRenderer)obj;
			base.GetDependencies<PersistentAnimationCurve>(this.widthCurve, trailRenderer.widthCurve, dependencies);
			base.GetDependencies<PersistentGradient>(this.colorGradient, trailRenderer.colorGradient, dependencies);
		}

		// Token: 0x04000A92 RID: 2706
		public float time;

		// Token: 0x04000A93 RID: 2707
		public float startWidth;

		// Token: 0x04000A94 RID: 2708
		public float endWidth;

		// Token: 0x04000A95 RID: 2709
		public PersistentAnimationCurve widthCurve;

		// Token: 0x04000A96 RID: 2710
		public float widthMultiplier;

		// Token: 0x04000A97 RID: 2711
		public Color startColor;

		// Token: 0x04000A98 RID: 2712
		public Color endColor;

		// Token: 0x04000A99 RID: 2713
		public PersistentGradient colorGradient;

		// Token: 0x04000A9A RID: 2714
		public bool autodestruct;

		// Token: 0x04000A9B RID: 2715
		public int numCornerVertices;

		// Token: 0x04000A9C RID: 2716
		public int numCapVertices;

		// Token: 0x04000A9D RID: 2717
		public float minVertexDistance;

		// Token: 0x04000A9E RID: 2718
		public uint textureMode;

		// Token: 0x04000A9F RID: 2719
		public uint alignment;
	}
}
