using System;
using UnityEngine;

// Token: 0x020007D8 RID: 2008
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncContextBase : MonoBehaviour
{
	// Token: 0x060032D3 RID: 13011 RVA: 0x00107E3C File Offset: 0x0010623C
	public OVRLipSyncContextBase()
	{
	}

	// Token: 0x170005F5 RID: 1525
	// (set) Token: 0x060032D4 RID: 13012 RVA: 0x00107E4F File Offset: 0x0010624F
	public int Smoothing
	{
		set
		{
			OVRLipSync.SendSignal(this.context, OVRLipSync.Signals.VisemeSmoothing, value, 0);
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x060032D5 RID: 13013 RVA: 0x00107E60 File Offset: 0x00106260
	public uint Context
	{
		get
		{
			return this.context;
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x060032D6 RID: 13014 RVA: 0x00107E68 File Offset: 0x00106268
	protected OVRLipSync.Frame Frame
	{
		get
		{
			return this.frame;
		}
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x00107E70 File Offset: 0x00106270
	private void Awake()
	{
		if (!this.audioSource)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		lock (this)
		{
			if (this.context == 0U && OVRLipSync.CreateContext(ref this.context, this.provider) != OVRLipSync.Result.Success)
			{
				Debug.Log("OVRPhonemeContext.Start ERROR: Could not create Phoneme context.");
			}
		}
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x00107EF0 File Offset: 0x001062F0
	private void OnDestroy()
	{
		lock (this)
		{
			if (this.context != 0U && OVRLipSync.DestroyContext(this.context) != OVRLipSync.Result.Success)
			{
				Debug.Log("OVRPhonemeContext.OnDestroy ERROR: Could not delete Phoneme context.");
			}
		}
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x00107F48 File Offset: 0x00106348
	public OVRLipSync.Frame GetCurrentPhonemeFrame()
	{
		return this.frame;
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x00107F50 File Offset: 0x00106350
	public void SetVisemeBlend(int viseme, int amount)
	{
		OVRLipSync.SendSignal(this.context, OVRLipSync.Signals.VisemeAmount, viseme, amount);
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x00107F61 File Offset: 0x00106361
	public OVRLipSync.Result ResetContext()
	{
		return OVRLipSync.ResetContext(this.context);
	}

	// Token: 0x040026D8 RID: 9944
	public AudioSource audioSource;

	// Token: 0x040026D9 RID: 9945
	public OVRLipSync.ContextProviders provider;

	// Token: 0x040026DA RID: 9946
	private OVRLipSync.Frame frame = new OVRLipSync.Frame();

	// Token: 0x040026DB RID: 9947
	private uint context;
}
