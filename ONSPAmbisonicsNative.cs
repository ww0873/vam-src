using System;
using UnityEngine;

// Token: 0x0200097E RID: 2430
public class ONSPAmbisonicsNative : MonoBehaviour
{
	// Token: 0x06003CA7 RID: 15527 RVA: 0x001260FB File Offset: 0x001244FB
	public ONSPAmbisonicsNative()
	{
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x0012610A File Offset: 0x0012450A
	// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x00126112 File Offset: 0x00124512
	public bool UseVirtualSpeakers
	{
		get
		{
			return this.useVirtualSpeakers;
		}
		set
		{
			this.useVirtualSpeakers = value;
		}
	}

	// Token: 0x06003CAA RID: 15530 RVA: 0x0012611C File Offset: 0x0012451C
	private void OnEnable()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		this.currentStatus = ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus.Uninitialized;
		if (component == null)
		{
			Debug.Log("Ambisonic ERROR: AudioSource does not exist.");
		}
		else
		{
			if (component.spatialize)
			{
				Debug.Log("Ambisonic WARNING: Turning spatialize field off for Ambisonic sources.");
				component.spatialize = false;
			}
			if (component.clip == null)
			{
				Debug.Log("Ambisonic ERROR: AudioSource does not contain an audio clip.");
			}
			else if (component.clip.channels != ONSPAmbisonicsNative.numFOAChannels)
			{
				Debug.Log("Ambisonic ERROR: AudioSource clip does not have correct number of channels.");
			}
		}
	}

	// Token: 0x06003CAB RID: 15531 RVA: 0x001261B0 File Offset: 0x001245B0
	private void Update()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		if (component == null)
		{
			return;
		}
		if (this.useVirtualSpeakers)
		{
			component.SetAmbisonicDecoderFloat(ONSPAmbisonicsNative.paramVSpeakerMode, 1f);
		}
		else
		{
			component.SetAmbisonicDecoderFloat(ONSPAmbisonicsNative.paramVSpeakerMode, 0f);
		}
		float num = 0f;
		component.GetAmbisonicDecoderFloat(ONSPAmbisonicsNative.paramAmbiStat, out num);
		ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus ovrAmbisonicsNativeStatus = (ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus)num;
		if (ovrAmbisonicsNativeStatus != this.currentStatus)
		{
			switch (ovrAmbisonicsNativeStatus + 1)
			{
			case ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus.NotEnabled:
				Debug.Log("Ambisonic Native: Stream uninitialized");
				break;
			case ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus.Success:
				Debug.Log("Ambisonic Native: Ambisonic not enabled on clip. Check clip field and turn it on");
				break;
			case ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus.StreamError:
				Debug.Log("Ambisonic Native: Stream successfully initialized and playing/playable");
				break;
			case ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus.ProcessError:
				Debug.Log("Ambisonic Native WARNING: Stream error (bad input format?)");
				break;
			case ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus.MaxStatValue:
				Debug.Log("Ambisonic Native WARNING: Stream process error (check default speaker setup)");
				break;
			}
		}
		this.currentStatus = ovrAmbisonicsNativeStatus;
	}

	// Token: 0x06003CAC RID: 15532 RVA: 0x0012629E File Offset: 0x0012469E
	// Note: this type is marked as 'beforefieldinit'.
	static ONSPAmbisonicsNative()
	{
	}

	// Token: 0x04002E87 RID: 11911
	private static int numFOAChannels = 4;

	// Token: 0x04002E88 RID: 11912
	private static int paramVSpeakerMode = 6;

	// Token: 0x04002E89 RID: 11913
	private static int paramAmbiStat = 7;

	// Token: 0x04002E8A RID: 11914
	private ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus currentStatus = ONSPAmbisonicsNative.ovrAmbisonicsNativeStatus.Uninitialized;

	// Token: 0x04002E8B RID: 11915
	[SerializeField]
	private bool useVirtualSpeakers;

	// Token: 0x0200097F RID: 2431
	public enum ovrAmbisonicsNativeStatus
	{
		// Token: 0x04002E8D RID: 11917
		Uninitialized = -1,
		// Token: 0x04002E8E RID: 11918
		NotEnabled,
		// Token: 0x04002E8F RID: 11919
		Success,
		// Token: 0x04002E90 RID: 11920
		StreamError,
		// Token: 0x04002E91 RID: 11921
		ProcessError,
		// Token: 0x04002E92 RID: 11922
		MaxStatValue
	}
}
