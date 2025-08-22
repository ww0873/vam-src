using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000135 RID: 309
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioHighPassFilter : PersistentBehaviour
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x00031959 File Offset: 0x0002FD59
		public PersistentAudioHighPassFilter()
		{
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00031964 File Offset: 0x0002FD64
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioHighPassFilter audioHighPassFilter = (AudioHighPassFilter)obj;
			audioHighPassFilter.cutoffFrequency = this.cutoffFrequency;
			audioHighPassFilter.highpassResonanceQ = this.highpassResonanceQ;
			return audioHighPassFilter;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000319A4 File Offset: 0x0002FDA4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioHighPassFilter audioHighPassFilter = (AudioHighPassFilter)obj;
			this.cutoffFrequency = audioHighPassFilter.cutoffFrequency;
			this.highpassResonanceQ = audioHighPassFilter.highpassResonanceQ;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000319DE File Offset: 0x0002FDDE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000794 RID: 1940
		public float cutoffFrequency;

		// Token: 0x04000795 RID: 1941
		public float highpassResonanceQ;
	}
}
