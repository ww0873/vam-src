using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
[RequireComponent(typeof(AudioSource))]
public class SpeechBlendMicrophoneInput : MonoBehaviour
{
	// Token: 0x06001B1B RID: 6939 RVA: 0x00096BD7 File Offset: 0x00094FD7
	public SpeechBlendMicrophoneInput()
	{
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x00096BE0 File Offset: 0x00094FE0
	private void Start()
	{
		string deviceName = Microphone.devices[0];
		this.source = base.GetComponent<AudioSource>();
		this.source.clip = Microphone.Start(deviceName, true, 5, 44100);
		this.source.loop = true;
		while (Microphone.GetPosition(null) == 0)
		{
		}
		this.source.Play();
	}

	// Token: 0x040016C8 RID: 5832
	private AudioSource source;
}
