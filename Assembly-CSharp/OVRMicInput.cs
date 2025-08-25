using System;
using UnityEngine;

// Token: 0x020008B4 RID: 2228
[RequireComponent(typeof(AudioSource))]
public class OVRMicInput : MonoBehaviour
{
	// Token: 0x0600380F RID: 14351 RVA: 0x0010F81C File Offset: 0x0010DC1C
	public OVRMicInput()
	{
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x06003810 RID: 14352 RVA: 0x0010F853 File Offset: 0x0010DC53
	// (set) Token: 0x06003811 RID: 14353 RVA: 0x0010F85B File Offset: 0x0010DC5B
	public float Sensitivity
	{
		get
		{
			return this.sensitivity;
		}
		set
		{
			this.sensitivity = Mathf.Clamp(value, 0f, 100f);
		}
	}

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06003812 RID: 14354 RVA: 0x0010F873 File Offset: 0x0010DC73
	// (set) Token: 0x06003813 RID: 14355 RVA: 0x0010F87B File Offset: 0x0010DC7B
	public float SourceVolume
	{
		get
		{
			return this.sourceVolume;
		}
		set
		{
			this.sourceVolume = Mathf.Clamp(value, 0f, 100f);
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06003814 RID: 14356 RVA: 0x0010F893 File Offset: 0x0010DC93
	// (set) Token: 0x06003815 RID: 14357 RVA: 0x0010F89C File Offset: 0x0010DC9C
	public float MicFrequency
	{
		get
		{
			return (float)this.micFrequency;
		}
		set
		{
			this.micFrequency = (int)Mathf.Clamp(value, 0f, 96000f);
		}
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x0010F8B6 File Offset: 0x0010DCB6
	private void Awake()
	{
		if (!this.audioSource)
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		if (!this.audioSource)
		{
			return;
		}
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x0010F8E8 File Offset: 0x0010DCE8
	private void Start()
	{
		this.audioSource.loop = true;
		this.audioSource.mute = false;
		if (Microphone.devices.Length != 0)
		{
			this.selectedDevice = Microphone.devices[0].ToString();
			this.micSelected = true;
			this.GetMicCaps();
		}
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x0010F938 File Offset: 0x0010DD38
	private void Update()
	{
		if (!this.focused)
		{
			this.StopMicrophone();
		}
		if (!Application.isPlaying)
		{
			this.StopMicrophone();
		}
		this.audioSource.volume = this.sourceVolume / 100f;
		this.loudness = Mathf.Clamp(this.GetAveragedVolume() * this.sensitivity * (this.sourceVolume / 10f), 0f, 100f);
		if (this.micControl == OVRMicInput.micActivation.HoldToSpeak)
		{
			if (Microphone.IsRecording(this.selectedDevice) && !Input.GetKey(KeyCode.Space))
			{
				this.StopMicrophone();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.StartMicrophone();
			}
			if (Input.GetKeyUp(KeyCode.Space))
			{
				this.StopMicrophone();
			}
		}
		if (this.micControl == OVRMicInput.micActivation.PushToSpeak && Input.GetKeyDown(KeyCode.Space))
		{
			if (Microphone.IsRecording(this.selectedDevice))
			{
				this.StopMicrophone();
			}
			else if (!Microphone.IsRecording(this.selectedDevice))
			{
				this.StartMicrophone();
			}
		}
		if (this.micControl == OVRMicInput.micActivation.ConstantSpeak && !Microphone.IsRecording(this.selectedDevice))
		{
			this.StartMicrophone();
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			this.micSelected = false;
		}
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x0010FA7B File Offset: 0x0010DE7B
	private void OnApplicationFocus(bool focus)
	{
		this.focused = focus;
		if (!this.focused)
		{
			this.StopMicrophone();
		}
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x0010FA95 File Offset: 0x0010DE95
	private void OnApplicationPause(bool focus)
	{
		this.focused = focus;
		if (!this.focused)
		{
			this.StopMicrophone();
		}
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x0010FAAF File Offset: 0x0010DEAF
	private void OnDisable()
	{
		this.StopMicrophone();
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x0010FAB7 File Offset: 0x0010DEB7
	private void OnGUI()
	{
		this.MicDeviceGUI((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 75), 300f, 50f, 10f, -300f);
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x0010FAEC File Offset: 0x0010DEEC
	public void MicDeviceGUI(float left, float top, float width, float height, float buttonSpaceTop, float buttonSpaceLeft)
	{
		if (Microphone.devices.Length >= 1 && this.GuiSelectDevice && !this.micSelected)
		{
			for (int i = 0; i < Microphone.devices.Length; i++)
			{
				if (GUI.Button(new Rect(left + (width + buttonSpaceLeft) * (float)i, top + (height + buttonSpaceTop) * (float)i, width, height), Microphone.devices[i].ToString()))
				{
					this.StopMicrophone();
					this.selectedDevice = Microphone.devices[i].ToString();
					this.micSelected = true;
					this.GetMicCaps();
					this.StartMicrophone();
				}
			}
		}
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x0010FB90 File Offset: 0x0010DF90
	public void GetMicCaps()
	{
		if (!this.micSelected)
		{
			return;
		}
		Microphone.GetDeviceCaps(this.selectedDevice, out this.minFreq, out this.maxFreq);
		if (this.minFreq == 0 && this.maxFreq == 0)
		{
			Debug.LogWarning("GetMicCaps warning:: min and max frequencies are 0");
			this.minFreq = 44100;
			this.maxFreq = 44100;
		}
		if (this.micFrequency > this.maxFreq)
		{
			this.micFrequency = this.maxFreq;
		}
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x0010FC14 File Offset: 0x0010E014
	public void StartMicrophone()
	{
		if (!this.micSelected)
		{
			return;
		}
		this.audioSource.clip = Microphone.Start(this.selectedDevice, true, 1, this.micFrequency);
		while (Microphone.GetPosition(this.selectedDevice) <= 0)
		{
		}
		this.audioSource.Play();
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x0010FC6C File Offset: 0x0010E06C
	public void StopMicrophone()
	{
		if (!this.micSelected)
		{
			return;
		}
		if (this.audioSource != null && this.audioSource.clip != null && this.audioSource.clip.name == "Microphone")
		{
			this.audioSource.Stop();
		}
		Microphone.End(this.selectedDevice);
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x0010FCE1 File Offset: 0x0010E0E1
	private float GetAveragedVolume()
	{
		return 0f;
	}

	// Token: 0x04002932 RID: 10546
	public AudioSource audioSource;

	// Token: 0x04002933 RID: 10547
	public bool GuiSelectDevice = true;

	// Token: 0x04002934 RID: 10548
	[SerializeField]
	private float sensitivity = 100f;

	// Token: 0x04002935 RID: 10549
	[SerializeField]
	private float sourceVolume = 100f;

	// Token: 0x04002936 RID: 10550
	[SerializeField]
	private int micFrequency = 16000;

	// Token: 0x04002937 RID: 10551
	public OVRMicInput.micActivation micControl;

	// Token: 0x04002938 RID: 10552
	public string selectedDevice;

	// Token: 0x04002939 RID: 10553
	public float loudness;

	// Token: 0x0400293A RID: 10554
	private bool micSelected;

	// Token: 0x0400293B RID: 10555
	private int minFreq;

	// Token: 0x0400293C RID: 10556
	private int maxFreq;

	// Token: 0x0400293D RID: 10557
	private bool focused = true;

	// Token: 0x020008B5 RID: 2229
	public enum micActivation
	{
		// Token: 0x0400293F RID: 10559
		HoldToSpeak,
		// Token: 0x04002940 RID: 10560
		PushToSpeak,
		// Token: 0x04002941 RID: 10561
		ConstantSpeak
	}
}
