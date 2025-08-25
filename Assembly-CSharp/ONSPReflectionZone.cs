using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200097B RID: 2427
public class ONSPReflectionZone : MonoBehaviour
{
	// Token: 0x06003C86 RID: 15494 RVA: 0x001253E7 File Offset: 0x001237E7
	public ONSPReflectionZone()
	{
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x001253EF File Offset: 0x001237EF
	private void Start()
	{
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x001253F1 File Offset: 0x001237F1
	private void Update()
	{
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x001253F3 File Offset: 0x001237F3
	private void OnTriggerEnter(Collider other)
	{
		if (this.CheckForAudioListener(other.gameObject))
		{
			this.PushCurrentMixerShapshot();
		}
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x0012540C File Offset: 0x0012380C
	private void OnTriggerExit(Collider other)
	{
		if (this.CheckForAudioListener(other.gameObject))
		{
			this.PopCurrentMixerSnapshot();
		}
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00125428 File Offset: 0x00123828
	private bool CheckForAudioListener(GameObject gameObject)
	{
		AudioListener componentInChildren = gameObject.GetComponentInChildren<AudioListener>();
		return componentInChildren != null;
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x0012544C File Offset: 0x0012384C
	private void PushCurrentMixerShapshot()
	{
		ReflectionSnapshot t = ONSPReflectionZone.currentSnapshot;
		ONSPReflectionZone.snapshotList.Push(t);
		this.SetReflectionValues();
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x00125470 File Offset: 0x00123870
	private void PopCurrentMixerSnapshot()
	{
		ReflectionSnapshot reflectionSnapshot = ONSPReflectionZone.snapshotList.Pop();
		this.SetReflectionValues(ref reflectionSnapshot);
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x00125490 File Offset: 0x00123890
	private void SetReflectionValues()
	{
		if (this.mixerSnapshot != null)
		{
			Debug.Log("Setting off snapshot " + this.mixerSnapshot.name);
			this.mixerSnapshot.TransitionTo(this.fadeTime);
			ONSPReflectionZone.currentSnapshot.mixerSnapshot = this.mixerSnapshot;
			ONSPReflectionZone.currentSnapshot.fadeTime = this.fadeTime;
		}
		else
		{
			Debug.Log("Mixer snapshot not set - Please ensure play area has at least one encompassing snapshot.");
		}
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00125508 File Offset: 0x00123908
	private void SetReflectionValues(ref ReflectionSnapshot mss)
	{
		if (mss.mixerSnapshot != null)
		{
			Debug.Log("Setting off snapshot " + mss.mixerSnapshot.name);
			mss.mixerSnapshot.TransitionTo(mss.fadeTime);
			ONSPReflectionZone.currentSnapshot.mixerSnapshot = mss.mixerSnapshot;
			ONSPReflectionZone.currentSnapshot.fadeTime = mss.fadeTime;
		}
		else
		{
			Debug.Log("Mixer snapshot not set - Please ensure play area has at least one encompassing snapshot.");
		}
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00125580 File Offset: 0x00123980
	// Note: this type is marked as 'beforefieldinit'.
	static ONSPReflectionZone()
	{
	}

	// Token: 0x04002E6C RID: 11884
	public AudioMixerSnapshot mixerSnapshot;

	// Token: 0x04002E6D RID: 11885
	public float fadeTime;

	// Token: 0x04002E6E RID: 11886
	private static Stack<ReflectionSnapshot> snapshotList = new Stack<ReflectionSnapshot>();

	// Token: 0x04002E6F RID: 11887
	private static ReflectionSnapshot currentSnapshot = default(ReflectionSnapshot);
}
