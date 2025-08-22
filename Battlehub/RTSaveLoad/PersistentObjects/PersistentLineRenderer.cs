using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000185 RID: 389
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLineRenderer : PersistentRenderer
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x00036691 File Offset: 0x00034A91
		public PersistentLineRenderer()
		{
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0003669C File Offset: 0x00034A9C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			LineRenderer lineRenderer = (LineRenderer)obj;
			lineRenderer.startWidth = this.startWidth;
			lineRenderer.endWidth = this.endWidth;
			lineRenderer.widthCurve = base.Write<AnimationCurve>(lineRenderer.widthCurve, this.widthCurve, objects);
			lineRenderer.widthMultiplier = this.widthMultiplier;
			lineRenderer.startColor = this.startColor;
			lineRenderer.endColor = this.endColor;
			lineRenderer.colorGradient = base.Write<Gradient>(lineRenderer.colorGradient, this.colorGradient, objects);
			lineRenderer.positionCount = this.positionCount;
			lineRenderer.useWorldSpace = this.useWorldSpace;
			lineRenderer.loop = this.loop;
			lineRenderer.numCornerVertices = this.numCornerVertices;
			lineRenderer.numCapVertices = this.numCapVertices;
			lineRenderer.textureMode = (LineTextureMode)this.textureMode;
			lineRenderer.alignment = (LineAlignment)this.alignment;
			return lineRenderer;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00036788 File Offset: 0x00034B88
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			LineRenderer lineRenderer = (LineRenderer)obj;
			this.startWidth = lineRenderer.startWidth;
			this.endWidth = lineRenderer.endWidth;
			this.widthCurve = base.Read<PersistentAnimationCurve>(this.widthCurve, lineRenderer.widthCurve);
			this.widthMultiplier = lineRenderer.widthMultiplier;
			this.startColor = lineRenderer.startColor;
			this.endColor = lineRenderer.endColor;
			this.colorGradient = base.Read<PersistentGradient>(this.colorGradient, lineRenderer.colorGradient);
			this.positionCount = lineRenderer.positionCount;
			this.useWorldSpace = lineRenderer.useWorldSpace;
			this.loop = lineRenderer.loop;
			this.numCornerVertices = lineRenderer.numCornerVertices;
			this.numCapVertices = lineRenderer.numCapVertices;
			this.textureMode = (uint)lineRenderer.textureMode;
			this.alignment = (uint)lineRenderer.alignment;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0003686A File Offset: 0x00034C6A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentAnimationCurve>(this.widthCurve, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGradient>(this.colorGradient, dependencies, objects, allowNulls);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00036894 File Offset: 0x00034C94
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			LineRenderer lineRenderer = (LineRenderer)obj;
			base.GetDependencies<PersistentAnimationCurve>(this.widthCurve, lineRenderer.widthCurve, dependencies);
			base.GetDependencies<PersistentGradient>(this.colorGradient, lineRenderer.colorGradient, dependencies);
		}

		// Token: 0x04000930 RID: 2352
		public float startWidth;

		// Token: 0x04000931 RID: 2353
		public float endWidth;

		// Token: 0x04000932 RID: 2354
		public PersistentAnimationCurve widthCurve;

		// Token: 0x04000933 RID: 2355
		public float widthMultiplier;

		// Token: 0x04000934 RID: 2356
		public Color startColor;

		// Token: 0x04000935 RID: 2357
		public Color endColor;

		// Token: 0x04000936 RID: 2358
		public PersistentGradient colorGradient;

		// Token: 0x04000937 RID: 2359
		public int positionCount;

		// Token: 0x04000938 RID: 2360
		public bool useWorldSpace;

		// Token: 0x04000939 RID: 2361
		public bool loop;

		// Token: 0x0400093A RID: 2362
		public int numCornerVertices;

		// Token: 0x0400093B RID: 2363
		public int numCapVertices;

		// Token: 0x0400093C RID: 2364
		public uint textureMode;

		// Token: 0x0400093D RID: 2365
		public uint alignment;
	}
}
