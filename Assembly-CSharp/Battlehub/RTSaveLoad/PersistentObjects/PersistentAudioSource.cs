using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Audio;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200013D RID: 317
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAudioSource : PersistentBehaviour
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x00031FFD File Offset: 0x000303FD
		public PersistentAudioSource()
		{
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00032008 File Offset: 0x00030408
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AudioSource audioSource = (AudioSource)obj;
			audioSource.volume = this.volume;
			audioSource.pitch = this.pitch;
			audioSource.time = this.time;
			audioSource.timeSamples = this.timeSamples;
			audioSource.clip = (AudioClip)objects.Get(this.clip);
			audioSource.outputAudioMixerGroup = (AudioMixerGroup)objects.Get(this.outputAudioMixerGroup);
			audioSource.loop = this.loop;
			audioSource.ignoreListenerVolume = this.ignoreListenerVolume;
			audioSource.playOnAwake = this.playOnAwake;
			audioSource.ignoreListenerPause = this.ignoreListenerPause;
			audioSource.velocityUpdateMode = (AudioVelocityUpdateMode)this.velocityUpdateMode;
			audioSource.panStereo = this.panStereo;
			audioSource.spatialBlend = this.spatialBlend;
			audioSource.spatialize = this.spatialize;
			audioSource.spatializePostEffects = this.spatializePostEffects;
			audioSource.reverbZoneMix = this.reverbZoneMix;
			audioSource.bypassEffects = this.bypassEffects;
			audioSource.bypassListenerEffects = this.bypassListenerEffects;
			audioSource.bypassReverbZones = this.bypassReverbZones;
			audioSource.dopplerLevel = this.dopplerLevel;
			audioSource.spread = this.spread;
			audioSource.priority = this.priority;
			audioSource.mute = this.mute;
			audioSource.minDistance = this.minDistance;
			audioSource.maxDistance = this.maxDistance;
			audioSource.rolloffMode = (AudioRolloffMode)this.rolloffMode;
			return audioSource;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00032180 File Offset: 0x00030580
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AudioSource audioSource = (AudioSource)obj;
			this.volume = audioSource.volume;
			this.pitch = audioSource.pitch;
			this.time = audioSource.time;
			this.timeSamples = audioSource.timeSamples;
			this.clip = audioSource.clip.GetMappedInstanceID();
			this.outputAudioMixerGroup = audioSource.outputAudioMixerGroup.GetMappedInstanceID();
			this.loop = audioSource.loop;
			this.ignoreListenerVolume = audioSource.ignoreListenerVolume;
			this.playOnAwake = audioSource.playOnAwake;
			this.ignoreListenerPause = audioSource.ignoreListenerPause;
			this.velocityUpdateMode = (uint)audioSource.velocityUpdateMode;
			this.panStereo = audioSource.panStereo;
			this.spatialBlend = audioSource.spatialBlend;
			this.spatialize = audioSource.spatialize;
			this.spatializePostEffects = audioSource.spatializePostEffects;
			this.reverbZoneMix = audioSource.reverbZoneMix;
			this.bypassEffects = audioSource.bypassEffects;
			this.bypassListenerEffects = audioSource.bypassListenerEffects;
			this.bypassReverbZones = audioSource.bypassReverbZones;
			this.dopplerLevel = audioSource.dopplerLevel;
			this.spread = audioSource.spread;
			this.priority = audioSource.priority;
			this.mute = audioSource.mute;
			this.minDistance = audioSource.minDistance;
			this.maxDistance = audioSource.maxDistance;
			this.rolloffMode = (uint)audioSource.rolloffMode;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000322E4 File Offset: 0x000306E4
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.clip, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.outputAudioMixerGroup, dependencies, objects, allowNulls);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00032310 File Offset: 0x00030710
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			AudioSource audioSource = (AudioSource)obj;
			base.AddDependency(audioSource.clip, dependencies);
			base.AddDependency(audioSource.outputAudioMixerGroup, dependencies);
		}

		// Token: 0x040007BB RID: 1979
		public float volume;

		// Token: 0x040007BC RID: 1980
		public float pitch;

		// Token: 0x040007BD RID: 1981
		public float time;

		// Token: 0x040007BE RID: 1982
		public int timeSamples;

		// Token: 0x040007BF RID: 1983
		public long clip;

		// Token: 0x040007C0 RID: 1984
		public long outputAudioMixerGroup;

		// Token: 0x040007C1 RID: 1985
		public bool loop;

		// Token: 0x040007C2 RID: 1986
		public bool ignoreListenerVolume;

		// Token: 0x040007C3 RID: 1987
		public bool playOnAwake;

		// Token: 0x040007C4 RID: 1988
		public bool ignoreListenerPause;

		// Token: 0x040007C5 RID: 1989
		public uint velocityUpdateMode;

		// Token: 0x040007C6 RID: 1990
		public float panStereo;

		// Token: 0x040007C7 RID: 1991
		public float spatialBlend;

		// Token: 0x040007C8 RID: 1992
		public bool spatialize;

		// Token: 0x040007C9 RID: 1993
		public bool spatializePostEffects;

		// Token: 0x040007CA RID: 1994
		public float reverbZoneMix;

		// Token: 0x040007CB RID: 1995
		public bool bypassEffects;

		// Token: 0x040007CC RID: 1996
		public bool bypassListenerEffects;

		// Token: 0x040007CD RID: 1997
		public bool bypassReverbZones;

		// Token: 0x040007CE RID: 1998
		public float dopplerLevel;

		// Token: 0x040007CF RID: 1999
		public float spread;

		// Token: 0x040007D0 RID: 2000
		public int priority;

		// Token: 0x040007D1 RID: 2001
		public bool mute;

		// Token: 0x040007D2 RID: 2002
		public float minDistance;

		// Token: 0x040007D3 RID: 2003
		public float maxDistance;

		// Token: 0x040007D4 RID: 2004
		public uint rolloffMode;
	}
}
