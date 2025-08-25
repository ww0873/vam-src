using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Audio;

namespace Battlehub.RTSaveLoad.PersistentObjects.Audio
{
	// Token: 0x02000138 RID: 312
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioMixer : PersistentObject
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x00031B66 File Offset: 0x0002FF66
		public PersistentAudioMixer()
		{
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00031B70 File Offset: 0x0002FF70
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioMixer audioMixer = (AudioMixer)obj;
			audioMixer.outputAudioMixerGroup = (AudioMixerGroup)objects.Get(this.outputAudioMixerGroup);
			audioMixer.updateMode = (AudioMixerUpdateMode)this.updateMode;
			return audioMixer;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00031BBC File Offset: 0x0002FFBC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioMixer audioMixer = (AudioMixer)obj;
			this.outputAudioMixerGroup = audioMixer.outputAudioMixerGroup.GetMappedInstanceID();
			this.updateMode = (uint)audioMixer.updateMode;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00031BFB File Offset: 0x0002FFFB
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.outputAudioMixerGroup, dependencies, objects, allowNulls);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00031C18 File Offset: 0x00030018
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			AudioMixer audioMixer = (AudioMixer)obj;
			base.AddDependency(audioMixer.outputAudioMixerGroup, dependencies);
		}

		// Token: 0x0400079A RID: 1946
		public long outputAudioMixerGroup;

		// Token: 0x0400079B RID: 1947
		public uint updateMode;
	}
}
