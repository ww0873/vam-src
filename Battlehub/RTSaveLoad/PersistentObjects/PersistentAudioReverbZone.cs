using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200013C RID: 316
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioReverbZone : PersistentBehaviour
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x00031E1D File Offset: 0x0003021D
		public PersistentAudioReverbZone()
		{
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00031E28 File Offset: 0x00030228
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioReverbZone audioReverbZone = (AudioReverbZone)obj;
			audioReverbZone.minDistance = this.minDistance;
			audioReverbZone.maxDistance = this.maxDistance;
			audioReverbZone.reverbPreset = (AudioReverbPreset)this.reverbPreset;
			audioReverbZone.room = this.room;
			audioReverbZone.roomHF = this.roomHF;
			audioReverbZone.roomLF = this.roomLF;
			audioReverbZone.decayTime = this.decayTime;
			audioReverbZone.decayHFRatio = this.decayHFRatio;
			audioReverbZone.reflections = this.reflections;
			audioReverbZone.reflectionsDelay = this.reflectionsDelay;
			audioReverbZone.reverb = this.reverb;
			audioReverbZone.reverbDelay = this.reverbDelay;
			audioReverbZone.HFReference = this.HFReference;
			audioReverbZone.LFReference = this.LFReference;
			audioReverbZone.diffusion = this.diffusion;
			audioReverbZone.density = this.density;
			return audioReverbZone;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00031F10 File Offset: 0x00030310
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioReverbZone audioReverbZone = (AudioReverbZone)obj;
			this.minDistance = audioReverbZone.minDistance;
			this.maxDistance = audioReverbZone.maxDistance;
			this.reverbPreset = (uint)audioReverbZone.reverbPreset;
			this.room = audioReverbZone.room;
			this.roomHF = audioReverbZone.roomHF;
			this.roomLF = audioReverbZone.roomLF;
			this.decayTime = audioReverbZone.decayTime;
			this.decayHFRatio = audioReverbZone.decayHFRatio;
			this.reflections = audioReverbZone.reflections;
			this.reflectionsDelay = audioReverbZone.reflectionsDelay;
			this.reverb = audioReverbZone.reverb;
			this.reverbDelay = audioReverbZone.reverbDelay;
			this.HFReference = audioReverbZone.HFReference;
			this.LFReference = audioReverbZone.LFReference;
			this.diffusion = audioReverbZone.diffusion;
			this.density = audioReverbZone.density;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00031FF2 File Offset: 0x000303F2
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040007AB RID: 1963
		public float minDistance;

		// Token: 0x040007AC RID: 1964
		public float maxDistance;

		// Token: 0x040007AD RID: 1965
		public uint reverbPreset;

		// Token: 0x040007AE RID: 1966
		public int room;

		// Token: 0x040007AF RID: 1967
		public int roomHF;

		// Token: 0x040007B0 RID: 1968
		public int roomLF;

		// Token: 0x040007B1 RID: 1969
		public float decayTime;

		// Token: 0x040007B2 RID: 1970
		public float decayHFRatio;

		// Token: 0x040007B3 RID: 1971
		public int reflections;

		// Token: 0x040007B4 RID: 1972
		public float reflectionsDelay;

		// Token: 0x040007B5 RID: 1973
		public int reverb;

		// Token: 0x040007B6 RID: 1974
		public float reverbDelay;

		// Token: 0x040007B7 RID: 1975
		public float HFReference;

		// Token: 0x040007B8 RID: 1976
		public float LFReference;

		// Token: 0x040007B9 RID: 1977
		public float diffusion;

		// Token: 0x040007BA RID: 1978
		public float density;
	}
}
