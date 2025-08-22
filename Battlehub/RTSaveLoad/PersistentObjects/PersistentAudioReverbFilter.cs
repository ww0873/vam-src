using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200013B RID: 315
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioReverbFilter : PersistentBehaviour
	{
		// Token: 0x06000747 RID: 1863 RVA: 0x00031C58 File Offset: 0x00030058
		public PersistentAudioReverbFilter()
		{
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00031C60 File Offset: 0x00030060
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioReverbFilter audioReverbFilter = (AudioReverbFilter)obj;
			audioReverbFilter.reverbPreset = (AudioReverbPreset)this.reverbPreset;
			audioReverbFilter.dryLevel = this.dryLevel;
			audioReverbFilter.room = this.room;
			audioReverbFilter.roomHF = this.roomHF;
			audioReverbFilter.decayTime = this.decayTime;
			audioReverbFilter.decayHFRatio = this.decayHFRatio;
			audioReverbFilter.reflectionsLevel = this.reflectionsLevel;
			audioReverbFilter.reflectionsDelay = this.reflectionsDelay;
			audioReverbFilter.reverbLevel = this.reverbLevel;
			audioReverbFilter.reverbDelay = this.reverbDelay;
			audioReverbFilter.diffusion = this.diffusion;
			audioReverbFilter.density = this.density;
			audioReverbFilter.hfReference = this.hfReference;
			audioReverbFilter.roomLF = this.roomLF;
			audioReverbFilter.lfReference = this.lfReference;
			return audioReverbFilter;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00031D3C File Offset: 0x0003013C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioReverbFilter audioReverbFilter = (AudioReverbFilter)obj;
			this.reverbPreset = (uint)audioReverbFilter.reverbPreset;
			this.dryLevel = audioReverbFilter.dryLevel;
			this.room = audioReverbFilter.room;
			this.roomHF = audioReverbFilter.roomHF;
			this.decayTime = audioReverbFilter.decayTime;
			this.decayHFRatio = audioReverbFilter.decayHFRatio;
			this.reflectionsLevel = audioReverbFilter.reflectionsLevel;
			this.reflectionsDelay = audioReverbFilter.reflectionsDelay;
			this.reverbLevel = audioReverbFilter.reverbLevel;
			this.reverbDelay = audioReverbFilter.reverbDelay;
			this.diffusion = audioReverbFilter.diffusion;
			this.density = audioReverbFilter.density;
			this.hfReference = audioReverbFilter.hfReference;
			this.roomLF = audioReverbFilter.roomLF;
			this.lfReference = audioReverbFilter.lfReference;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00031E12 File Offset: 0x00030212
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400079C RID: 1948
		public uint reverbPreset;

		// Token: 0x0400079D RID: 1949
		public float dryLevel;

		// Token: 0x0400079E RID: 1950
		public float room;

		// Token: 0x0400079F RID: 1951
		public float roomHF;

		// Token: 0x040007A0 RID: 1952
		public float decayTime;

		// Token: 0x040007A1 RID: 1953
		public float decayHFRatio;

		// Token: 0x040007A2 RID: 1954
		public float reflectionsLevel;

		// Token: 0x040007A3 RID: 1955
		public float reflectionsDelay;

		// Token: 0x040007A4 RID: 1956
		public float reverbLevel;

		// Token: 0x040007A5 RID: 1957
		public float reverbDelay;

		// Token: 0x040007A6 RID: 1958
		public float diffusion;

		// Token: 0x040007A7 RID: 1959
		public float density;

		// Token: 0x040007A8 RID: 1960
		public float hfReference;

		// Token: 0x040007A9 RID: 1961
		public float roomLF;

		// Token: 0x040007AA RID: 1962
		public float lfReference;
	}
}
