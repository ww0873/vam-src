using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000124 RID: 292
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTextureSheetAnimationModule : PersistentData
	{
		// Token: 0x060006F5 RID: 1781 RVA: 0x000303CE File Offset: 0x0002E7CE
		public PersistentTextureSheetAnimationModule()
		{
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000303D8 File Offset: 0x0002E7D8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = (ParticleSystem.TextureSheetAnimationModule)obj;
			textureSheetAnimationModule.enabled = this.enabled;
			textureSheetAnimationModule.numTilesX = this.numTilesX;
			textureSheetAnimationModule.numTilesY = this.numTilesY;
			textureSheetAnimationModule.animation = (ParticleSystemAnimationType)this.animation;
			textureSheetAnimationModule.useRandomRow = this.useRandomRow;
			textureSheetAnimationModule.frameOverTime = base.Write<ParticleSystem.MinMaxCurve>(textureSheetAnimationModule.frameOverTime, this.frameOverTime, objects);
			textureSheetAnimationModule.frameOverTimeMultiplier = this.frameOverTimeMultiplier;
			textureSheetAnimationModule.startFrame = base.Write<ParticleSystem.MinMaxCurve>(textureSheetAnimationModule.startFrame, this.startFrame, objects);
			textureSheetAnimationModule.startFrameMultiplier = this.startFrameMultiplier;
			textureSheetAnimationModule.cycleCount = this.cycleCount;
			textureSheetAnimationModule.rowIndex = this.rowIndex;
			textureSheetAnimationModule.uvChannelMask = (UVChannelFlags)this.uvChannelMask;
			textureSheetAnimationModule.flipU = this.flipU;
			textureSheetAnimationModule.flipV = this.flipV;
			return textureSheetAnimationModule;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000304D8 File Offset: 0x0002E8D8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = (ParticleSystem.TextureSheetAnimationModule)obj;
			this.enabled = textureSheetAnimationModule.enabled;
			this.numTilesX = textureSheetAnimationModule.numTilesX;
			this.numTilesY = textureSheetAnimationModule.numTilesY;
			this.animation = (uint)textureSheetAnimationModule.animation;
			this.useRandomRow = textureSheetAnimationModule.useRandomRow;
			this.frameOverTime = base.Read<PersistentMinMaxCurve>(this.frameOverTime, textureSheetAnimationModule.frameOverTime);
			this.frameOverTimeMultiplier = textureSheetAnimationModule.frameOverTimeMultiplier;
			this.startFrame = base.Read<PersistentMinMaxCurve>(this.startFrame, textureSheetAnimationModule.startFrame);
			this.startFrameMultiplier = textureSheetAnimationModule.startFrameMultiplier;
			this.cycleCount = textureSheetAnimationModule.cycleCount;
			this.rowIndex = textureSheetAnimationModule.rowIndex;
			this.uvChannelMask = (uint)textureSheetAnimationModule.uvChannelMask;
			this.flipU = textureSheetAnimationModule.flipU;
			this.flipV = textureSheetAnimationModule.flipV;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000305D2 File Offset: 0x0002E9D2
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.frameOverTime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.startFrame, dependencies, objects, allowNulls);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000305FC File Offset: 0x0002E9FC
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = (ParticleSystem.TextureSheetAnimationModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.frameOverTime, textureSheetAnimationModule.frameOverTime, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.startFrame, textureSheetAnimationModule.startFrame, dependencies);
		}

		// Token: 0x0400073C RID: 1852
		public bool enabled;

		// Token: 0x0400073D RID: 1853
		public int numTilesX;

		// Token: 0x0400073E RID: 1854
		public int numTilesY;

		// Token: 0x0400073F RID: 1855
		public uint animation;

		// Token: 0x04000740 RID: 1856
		public bool useRandomRow;

		// Token: 0x04000741 RID: 1857
		public PersistentMinMaxCurve frameOverTime;

		// Token: 0x04000742 RID: 1858
		public float frameOverTimeMultiplier;

		// Token: 0x04000743 RID: 1859
		public PersistentMinMaxCurve startFrame;

		// Token: 0x04000744 RID: 1860
		public float startFrameMultiplier;

		// Token: 0x04000745 RID: 1861
		public int cycleCount;

		// Token: 0x04000746 RID: 1862
		public int rowIndex;

		// Token: 0x04000747 RID: 1863
		public uint uvChannelMask;

		// Token: 0x04000748 RID: 1864
		public float flipU;

		// Token: 0x04000749 RID: 1865
		public float flipV;
	}
}
