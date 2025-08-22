using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000133 RID: 307
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioDistortionFilter : PersistentBehaviour
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x00031821 File Offset: 0x0002FC21
		public PersistentAudioDistortionFilter()
		{
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0003182C File Offset: 0x0002FC2C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioDistortionFilter audioDistortionFilter = (AudioDistortionFilter)obj;
			audioDistortionFilter.distortionLevel = this.distortionLevel;
			return audioDistortionFilter;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00031860 File Offset: 0x0002FC60
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioDistortionFilter audioDistortionFilter = (AudioDistortionFilter)obj;
			this.distortionLevel = audioDistortionFilter.distortionLevel;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0003188E File Offset: 0x0002FC8E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400078F RID: 1935
		public float distortionLevel;
	}
}
