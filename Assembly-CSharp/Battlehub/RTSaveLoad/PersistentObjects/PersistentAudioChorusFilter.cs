using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000131 RID: 305
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioChorusFilter : PersistentBehaviour
	{
		// Token: 0x06000726 RID: 1830 RVA: 0x00031711 File Offset: 0x0002FB11
		public PersistentAudioChorusFilter()
		{
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0003171C File Offset: 0x0002FB1C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioChorusFilter audioChorusFilter = (AudioChorusFilter)obj;
			audioChorusFilter.dryMix = this.dryMix;
			audioChorusFilter.wetMix1 = this.wetMix1;
			audioChorusFilter.wetMix2 = this.wetMix2;
			audioChorusFilter.wetMix3 = this.wetMix3;
			audioChorusFilter.delay = this.delay;
			audioChorusFilter.rate = this.rate;
			audioChorusFilter.depth = this.depth;
			return audioChorusFilter;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00031798 File Offset: 0x0002FB98
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioChorusFilter audioChorusFilter = (AudioChorusFilter)obj;
			this.dryMix = audioChorusFilter.dryMix;
			this.wetMix1 = audioChorusFilter.wetMix1;
			this.wetMix2 = audioChorusFilter.wetMix2;
			this.wetMix3 = audioChorusFilter.wetMix3;
			this.delay = audioChorusFilter.delay;
			this.rate = audioChorusFilter.rate;
			this.depth = audioChorusFilter.depth;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0003180E File Offset: 0x0002FC0E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000788 RID: 1928
		public float dryMix;

		// Token: 0x04000789 RID: 1929
		public float wetMix1;

		// Token: 0x0400078A RID: 1930
		public float wetMix2;

		// Token: 0x0400078B RID: 1931
		public float wetMix3;

		// Token: 0x0400078C RID: 1932
		public float delay;

		// Token: 0x0400078D RID: 1933
		public float rate;

		// Token: 0x0400078E RID: 1934
		public float depth;
	}
}
