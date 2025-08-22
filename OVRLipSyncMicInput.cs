using System;
using UnityEngine;

// Token: 0x020007DC RID: 2012
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncMicInput : MonoBehaviour
{
	// Token: 0x060032E8 RID: 13032 RVA: 0x0010885D File Offset: 0x00106C5D
	public OVRLipSyncMicInput()
	{
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x060032E9 RID: 13033 RVA: 0x00108894 File Offset: 0x00106C94
	// (set) Token: 0x060032EA RID: 13034 RVA: 0x0010889C File Offset: 0x00106C9C
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

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x060032EB RID: 13035 RVA: 0x001088B4 File Offset: 0x00106CB4
	// (set) Token: 0x060032EC RID: 13036 RVA: 0x001088BC File Offset: 0x00106CBC
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

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x060032ED RID: 13037 RVA: 0x001088D4 File Offset: 0x00106CD4
	// (set) Token: 0x060032EE RID: 13038 RVA: 0x001088DD File Offset: 0x00106CDD
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

	// Token: 0x060032EF RID: 13039 RVA: 0x001088F7 File Offset: 0x00106CF7
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

	// Token: 0x060032F0 RID: 13040 RVA: 0x00108928 File Offset: 0x00106D28
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

	// Token: 0x060032F1 RID: 13041 RVA: 0x00108978 File Offset: 0x00106D78
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
		if (this.micControl == OVRLipSyncMicInput.micActivation.HoldToSpeak)
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
		if (this.micControl == OVRLipSyncMicInput.micActivation.PushToSpeak && Input.GetKeyDown(KeyCode.Space))
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
		if (this.micControl == OVRLipSyncMicInput.micActivation.ConstantSpeak && !Microphone.IsRecording(this.selectedDevice))
		{
			this.StartMicrophone();
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			this.micSelected = false;
		}
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x00108ABB File Offset: 0x00106EBB
	private void OnApplicationFocus(bool focus)
	{
		this.focused = focus;
		if (!this.focused)
		{
			this.StopMicrophone();
		}
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x00108AD5 File Offset: 0x00106ED5
	private void OnApplicationPause(bool focus)
	{
		this.focused = focus;
		if (!this.focused)
		{
			this.StopMicrophone();
		}
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x00108AEF File Offset: 0x00106EEF
	private void OnDisable()
	{
		this.StopMicrophone();
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x00108AF7 File Offset: 0x00106EF7
	private void OnGUI()
	{
		this.MicDeviceGUI((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 75), 300f, 50f, 10f, -300f);
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x00108B2C File Offset: 0x00106F2C
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

	// Token: 0x060032F7 RID: 13047 RVA: 0x00108BD0 File Offset: 0x00106FD0
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

	// Token: 0x060032F8 RID: 13048 RVA: 0x00108C54 File Offset: 0x00107054
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

	// Token: 0x060032F9 RID: 13049 RVA: 0x00108CAC File Offset: 0x001070AC
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

	// Token: 0x060032FA RID: 13050 RVA: 0x00108D21 File Offset: 0x00107121
	private float GetAveragedVolume()
	{
		return 0f;
	}

	// Token: 0x040026E8 RID: 9960
	public AudioSource audioSource;

	// Token: 0x040026E9 RID: 9961
	public bool GuiSelectDevice = true;

	// Token: 0x040026EA RID: 9962
	[SerializeField]
	private float sensitivity = 100f;

	// Token: 0x040026EB RID: 9963
	[SerializeField]
	private float sourceVolume = 100f;

	// Token: 0x040026EC RID: 9964
	[SerializeField]
	private int micFrequency = 16000;

	// Token: 0x040026ED RID: 9965
	public OVRLipSyncMicInput.micActivation micControl;

	// Token: 0x040026EE RID: 9966
	public string selectedDevice;

	// Token: 0x040026EF RID: 9967
	public float loudness;

	// Token: 0x040026F0 RID: 9968
	private bool micSelected;

	// Token: 0x040026F1 RID: 9969
	private int minFreq;

	// Token: 0x040026F2 RID: 9970
	private int maxFreq;

	// Token: 0x040026F3 RID: 9971
	private bool focused = true;

	// Token: 0x020007DD RID: 2013
	public enum micActivation
	{
		// Token: 0x040026F5 RID: 9973
		HoldToSpeak,
		// Token: 0x040026F6 RID: 9974
		PushToSpeak,
		// Token: 0x040026F7 RID: 9975
		ConstantSpeak
	}
}
