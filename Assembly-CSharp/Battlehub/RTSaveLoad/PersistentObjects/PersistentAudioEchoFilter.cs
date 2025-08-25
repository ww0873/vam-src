using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000134 RID: 308
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioEchoFilter : PersistentBehaviour
	{
		// Token: 0x0600072F RID: 1839 RVA: 0x00031899 File Offset: 0x0002FC99
		public PersistentAudioEchoFilter()
		{
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000318A4 File Offset: 0x0002FCA4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioEchoFilter audioEchoFilter = (AudioEchoFilter)obj;
			audioEchoFilter.delay = this.delay;
			audioEchoFilter.decayRatio = this.decayRatio;
			audioEchoFilter.dryMix = this.dryMix;
			audioEchoFilter.wetMix = this.wetMix;
			return audioEchoFilter;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000318FC File Offset: 0x0002FCFC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioEchoFilter audioEchoFilter = (AudioEchoFilter)obj;
			this.delay = audioEchoFilter.delay;
			this.decayRatio = audioEchoFilter.decayRatio;
			this.dryMix = audioEchoFilter.dryMix;
			this.wetMix = audioEchoFilter.wetMix;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0003194E File Offset: 0x0002FD4E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000790 RID: 1936
		public float delay;

		// Token: 0x04000791 RID: 1937
		public float decayRatio;

		// Token: 0x04000792 RID: 1938
		public float dryMix;

		// Token: 0x04000793 RID: 1939
		public float wetMix;
	}
}
