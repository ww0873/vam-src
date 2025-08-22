using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000137 RID: 311
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioLowPassFilter : PersistentBehaviour
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x00031A61 File Offset: 0x0002FE61
		public PersistentAudioLowPassFilter()
		{
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00031A6C File Offset: 0x0002FE6C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioLowPassFilter audioLowPassFilter = (AudioLowPassFilter)obj;
			audioLowPassFilter.cutoffFrequency = this.cutoffFrequency;
			audioLowPassFilter.customCutoffCurve = base.Write<AnimationCurve>(audioLowPassFilter.customCutoffCurve, this.customCutoffCurve, objects);
			audioLowPassFilter.lowpassResonanceQ = this.lowpassResonanceQ;
			return audioLowPassFilter;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00031AC4 File Offset: 0x0002FEC4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioLowPassFilter audioLowPassFilter = (AudioLowPassFilter)obj;
			this.cutoffFrequency = audioLowPassFilter.cutoffFrequency;
			this.customCutoffCurve = base.Read<PersistentAnimationCurve>(this.customCutoffCurve, audioLowPassFilter.customCutoffCurve);
			this.lowpassResonanceQ = audioLowPassFilter.lowpassResonanceQ;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00031B16 File Offset: 0x0002FF16
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentAnimationCurve>(this.customCutoffCurve, dependencies, objects, allowNulls);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00031B30 File Offset: 0x0002FF30
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			AudioLowPassFilter audioLowPassFilter = (AudioLowPassFilter)obj;
			base.GetDependencies<PersistentAnimationCurve>(this.customCutoffCurve, audioLowPassFilter.customCutoffCurve, dependencies);
		}

		// Token: 0x04000797 RID: 1943
		public float cutoffFrequency;

		// Token: 0x04000798 RID: 1944
		public PersistentAnimationCurve customCutoffCurve;

		// Token: 0x04000799 RID: 1945
		public float lowpassResonanceQ;
	}
}
