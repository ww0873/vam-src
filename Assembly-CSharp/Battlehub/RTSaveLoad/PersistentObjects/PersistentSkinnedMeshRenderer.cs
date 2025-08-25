using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B6 RID: 438
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSkinnedMeshRenderer : PersistentRenderer
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x00038A3D File Offset: 0x00036E3D
		public PersistentSkinnedMeshRenderer()
		{
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00038A48 File Offset: 0x00036E48
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)obj;
			skinnedMeshRenderer.bones = base.Resolve<Transform, UnityEngine.Object>(this.bones, objects);
			skinnedMeshRenderer.rootBone = (Transform)objects.Get(this.rootBone);
			skinnedMeshRenderer.quality = (SkinQuality)this.quality;
			skinnedMeshRenderer.sharedMesh = (Mesh)objects.Get(this.sharedMesh);
			skinnedMeshRenderer.skinnedMotionVectors = this.skinnedMotionVectors;
			skinnedMeshRenderer.localBounds = this.localBounds;
			return skinnedMeshRenderer;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00038AD4 File Offset: 0x00036ED4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)obj;
			this.bones = skinnedMeshRenderer.bones.GetMappedInstanceID();
			this.rootBone = skinnedMeshRenderer.rootBone.GetMappedInstanceID();
			this.quality = (uint)skinnedMeshRenderer.quality;
			this.sharedMesh = skinnedMeshRenderer.sharedMesh.GetMappedInstanceID();
			this.updateWhenOffscreen = skinnedMeshRenderer.updateWhenOffscreen;
			this.skinnedMotionVectors = skinnedMeshRenderer.skinnedMotionVectors;
			this.localBounds = skinnedMeshRenderer.localBounds;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00038B59 File Offset: 0x00036F59
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependencies<T>(this.bones, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.rootBone, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sharedMesh, dependencies, objects, allowNulls);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00038B94 File Offset: 0x00036F94
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)obj;
			base.AddDependencies(skinnedMeshRenderer.bones, dependencies);
			base.AddDependency(skinnedMeshRenderer.rootBone, dependencies);
			base.AddDependency(skinnedMeshRenderer.sharedMesh, dependencies);
		}

		// Token: 0x04000A11 RID: 2577
		public long[] bones;

		// Token: 0x04000A12 RID: 2578
		public long rootBone;

		// Token: 0x04000A13 RID: 2579
		public uint quality;

		// Token: 0x04000A14 RID: 2580
		public long sharedMesh;

		// Token: 0x04000A15 RID: 2581
		public bool updateWhenOffscreen;

		// Token: 0x04000A16 RID: 2582
		public bool skinnedMotionVectors;

		// Token: 0x04000A17 RID: 2583
		public Bounds localBounds;
	}
}
