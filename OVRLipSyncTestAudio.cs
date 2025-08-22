using System;
using UnityEngine;

// Token: 0x020007C4 RID: 1988
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncTestAudio : MonoBehaviour
{
	// Token: 0x0600327E RID: 12926 RVA: 0x00107384 File Offset: 0x00105784
	public OVRLipSyncTestAudio()
	{
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x0010738C File Offset: 0x0010578C
	private void Start()
	{
		if (!this.audioSource)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (!this.audioSource)
		{
			return;
		}
		string text = Application.dataPath;
		text += "/../";
		text += "TestViseme.wav";
		WWW www = new WWW("file:///" + text);
		while (!www.isDone)
		{
			Debug.Log(www.progress);
		}
		if (www.GetAudioClip() != null)
		{
			this.audioSource.clip = www.GetAudioClip();
			this.audioSource.loop = true;
			this.audioSource.mute = false;
			this.audioSource.Play();
		}
	}

	// Token: 0x04002695 RID: 9877
	public AudioSource audioSource;
}
