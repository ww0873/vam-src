using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000129 RID: 297
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAnimationClip : PersistentMotion
	{
		// Token: 0x0600070D RID: 1805 RVA: 0x00031064 File Offset: 0x0002F464
		public PersistentAnimationClip()
		{
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0003106C File Offset: 0x0002F46C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AnimationClip animationClip = (AnimationClip)obj;
			animationClip.frameRate = this.frameRate;
			animationClip.wrapMode = (WrapMode)this.wrapMode;
			animationClip.localBounds = this.localBounds;
			animationClip.legacy = this.legacy;
			return animationClip;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000310C4 File Offset: 0x0002F4C4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AnimationClip animationClip = (AnimationClip)obj;
			this.frameRate = animationClip.frameRate;
			this.wrapMode = (uint)animationClip.wrapMode;
			this.localBounds = animationClip.localBounds;
			this.legacy = animationClip.legacy;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00031116 File Offset: 0x0002F516
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400076A RID: 1898
		public float frameRate;

		// Token: 0x0400076B RID: 1899
		public uint wrapMode;

		// Token: 0x0400076C RID: 1900
		public Bounds localBounds;

		// Token: 0x0400076D RID: 1901
		public bool legacy;
	}
}
