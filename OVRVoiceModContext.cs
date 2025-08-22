using System;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
[RequireComponent(typeof(AudioSource))]
public class OVRVoiceModContext : MonoBehaviour
{
	// Token: 0x06003838 RID: 14392 RVA: 0x0010FE70 File Offset: 0x0010E270
	public OVRVoiceModContext()
	{
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x0011083D File Offset: 0x0010EC3D
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

	// Token: 0x0600383A RID: 14394 RVA: 0x0011086C File Offset: 0x0010EC6C
	private void Start()
	{
		lock (this)
		{
			if (this.context == 0U && OVRVoiceMod.CreateContext(ref this.context) != 0)
			{
				Debug.Log("OVRVoiceModContext.Start ERROR: Could not create VoiceMod context.");
				return;
			}
		}
		OVRMessenger.AddListener<OVRTouchpad.TouchEvent>("Touchpad", new OVRCallback<OVRTouchpad.TouchEvent>(this.LocalTouchEventCallback));
		this.SendVoiceModUpdate();
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x001108E4 File Offset: 0x0010ECE4
	private void Update()
	{
		if (Input.GetKeyDown(this.loopback))
		{
			this.audioMute = !this.audioMute;
			OVRDebugConsole.Clear();
			OVRDebugConsole.ClearTimeout(1.5f);
			if (!this.audioMute)
			{
				OVRDebugConsole.Log("LOOPBACK MODE: ENABLED");
			}
			else
			{
				OVRDebugConsole.Log("LOOPBACK MODE: DISABLED");
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.gain -= 0.1f;
			if (this.gain < 0.5f)
			{
				this.gain = 0.5f;
			}
			string text = "LINEAR GAIN: ";
			text += this.gain;
			OVRDebugConsole.Clear();
			OVRDebugConsole.Log(text);
			OVRDebugConsole.ClearTimeout(1.5f);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.gain += 0.1f;
			if (this.gain > 3f)
			{
				this.gain = 3f;
			}
			string text2 = "LINEAR GAIN: ";
			text2 += this.gain;
			OVRDebugConsole.Clear();
			OVRDebugConsole.Log(text2);
			OVRDebugConsole.ClearTimeout(1.5f);
		}
		this.UpdateVoiceModUpdate();
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x00110A28 File Offset: 0x0010EE28
	private void OnDestroy()
	{
		lock (this)
		{
			if (this.context != 0U && OVRVoiceMod.DestroyContext(this.context) != 0)
			{
				Debug.Log("OVRVoiceModContext.OnDestroy ERROR: Could not delete VoiceMod context.");
			}
		}
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x00110A80 File Offset: 0x0010EE80
	private void OnAudioFilterRead(float[] data, int channels)
	{
		if (OVRVoiceMod.IsInitialized() != 0 || this.audioSource == null)
		{
			return;
		}
		for (int i = 0; i < data.Length; i++)
		{
			data[i] *= this.gain;
		}
		lock (this)
		{
			if (this.context != 0U)
			{
				OVRVoiceMod.ProcessFrameInterleaved(this.context, data);
			}
		}
		if (this.audioMute)
		{
			for (int j = 0; j < data.Length; j++)
			{
				data[j] *= 0f;
			}
		}
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x00110B34 File Offset: 0x0010EF34
	public int SendParameter(OVRVoiceModContext.ovrVoiceModParams parameter, int value)
	{
		if (OVRVoiceMod.IsInitialized() != 0)
		{
			return -2250;
		}
		return OVRVoiceMod.SendParameter(this.context, (int)parameter, value);
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x00110B54 File Offset: 0x0010EF54
	public bool SetPreset(int preset)
	{
		if (preset < 0 || preset >= this.VMPresets.Length)
		{
			return false;
		}
		this.VM_MixAudio = this.VMPresets[preset].mix;
		this.VM_Pitch = this.VMPresets[preset].pitch;
		this.VM_Bands = this.VMPresets[preset].bands;
		this.VM_FormantCorrect = this.VMPresets[preset].formant;
		this.VM_C1_TrackPitch = this.VMPresets[preset].c1PTrack;
		this.VM_C1_Type = this.VMPresets[preset].c1Type;
		this.VM_C1_Gain = this.VMPresets[preset].c1Gain;
		this.VM_C1_Freq = this.VMPresets[preset].c1Freq;
		this.VM_C1_Note = this.VMPresets[preset].c1Note;
		this.VM_C1_PulseWidth = this.VMPresets[preset].c1PW;
		this.VM_C1_CycledNoiseSize = this.VMPresets[preset].c1CNS;
		this.VM_C2_TrackPitch = this.VMPresets[preset].c2PTrack;
		this.VM_C2_Type = this.VMPresets[preset].c2Type;
		this.VM_C2_Gain = this.VMPresets[preset].c2Gain;
		this.VM_C2_Freq = this.VMPresets[preset].c2Freq;
		this.VM_C2_Note = this.VMPresets[preset].c2Note;
		this.VM_C2_PulseWidth = this.VMPresets[preset].c2PW;
		this.VM_C2_CycledNoiseSize = this.VMPresets[preset].c2CNS;
		this.SendVoiceModUpdate();
		OVRDebugConsole.Clear();
		OVRDebugConsole.Log(this.VMPresets[preset].info);
		OVRDebugConsole.ClearTimeout(5f);
		return true;
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x00110D42 File Offset: 0x0010F142
	public int GetNumPresets()
	{
		return this.VMPresets.Length;
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x00110D4C File Offset: 0x0010F14C
	public Color GetPresetColor(int preset)
	{
		if (preset < 0 || preset >= this.VMPresets.Length)
		{
			return Color.black;
		}
		return this.VMPresets[preset].color;
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x00110D7C File Offset: 0x0010F17C
	public float GetAverageAbsVolume()
	{
		if (this.context == 0U)
		{
			return 0f;
		}
		float result = this.prevVol * 0.8f + OVRVoiceMod.GetAverageAbsVolume(this.context) * 0.2f;
		this.prevVol = result;
		return result;
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x00110DC4 File Offset: 0x0010F1C4
	private void LocalTouchEventCallback(OVRTouchpad.TouchEvent touchEvent)
	{
		if (touchEvent == OVRTouchpad.TouchEvent.SingleTap)
		{
			this.audioMute = !this.audioMute;
			OVRDebugConsole.Clear();
			OVRDebugConsole.ClearTimeout(1.5f);
			if (!this.audioMute)
			{
				OVRDebugConsole.Log("LOOPBACK MODE: ENABLED");
			}
			else
			{
				OVRDebugConsole.Log("LOOPBACK MODE: DISABLED");
			}
		}
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x00110E23 File Offset: 0x0010F223
	private void UpdateVoiceModUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.SendVoiceModUpdate();
			OVRDebugConsole.Clear();
			OVRDebugConsole.Log("UPDATED VOICE MOD FROM INSPECTOR");
			OVRDebugConsole.ClearTimeout(1f);
		}
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x00110E50 File Offset: 0x0010F250
	private void SendVoiceModUpdate()
	{
		this.VM_MixAudio = Mathf.Clamp(this.VM_MixAudio, 0f, 1f);
		this.VM_Pitch = Mathf.Clamp(this.VM_Pitch, 0.5f, 2f);
		this.VM_Bands = Mathf.Clamp(this.VM_Bands, 1, 128);
		this.VM_FormantCorrect = Mathf.Clamp(this.VM_FormantCorrect, 0, 1);
		this.VM_C1_TrackPitch = Mathf.Clamp(this.VM_C1_TrackPitch, 0, 1);
		this.VM_C1_Type = Mathf.Clamp(this.VM_C1_Type, 0, 3);
		this.VM_C1_Gain = Mathf.Clamp(this.VM_C1_Gain, 0f, 1f);
		this.VM_C1_Freq = Mathf.Clamp(this.VM_C1_Freq, 0f, 96000f);
		this.VM_C1_Note = Mathf.Clamp(this.VM_C1_Note, -1, 127);
		this.VM_C1_PulseWidth = Mathf.Clamp(this.VM_C1_PulseWidth, 0f, 1f);
		this.VM_C1_CycledNoiseSize = Mathf.Clamp(this.VM_C1_CycledNoiseSize, 0, 1024);
		this.VM_C2_TrackPitch = Mathf.Clamp(this.VM_C2_TrackPitch, 0, 1);
		this.VM_C2_Type = Mathf.Clamp(this.VM_C2_Type, 0, 3);
		this.VM_C2_Gain = Mathf.Clamp(this.VM_C2_Gain, 0f, 1f);
		this.VM_C2_Freq = Mathf.Clamp(this.VM_C2_Freq, 0f, 96000f);
		this.VM_C2_Note = Mathf.Clamp(this.VM_C2_Note, -1, 127);
		this.VM_C2_PulseWidth = Mathf.Clamp(this.VM_C2_PulseWidth, 0f, 1f);
		this.VM_C2_CycledNoiseSize = Mathf.Clamp(this.VM_C2_CycledNoiseSize, 0, 1024);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.MixInputAudio, (int)(100f * this.VM_MixAudio));
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.PitchInputAudio, (int)(100f * this.VM_Pitch));
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.SetBands, this.VM_Bands);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.FormantCorrection, this.VM_FormantCorrect);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier1_TrackPitch, this.VM_C1_TrackPitch);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier1_Type, this.VM_C1_Type);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier1_Gain, (int)(100f * this.VM_C1_Gain));
		if (this.VM_C1_Note == -1)
		{
			this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier1_Frequency, (int)(100f * this.VM_C1_Freq));
		}
		else
		{
			this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier1_Note, this.VM_C1_Note);
		}
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier1_PulseWidth, (int)(100f * this.VM_C1_PulseWidth));
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier1_CycledNoiseSize, this.VM_C1_CycledNoiseSize);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier2_TrackPitch, this.VM_C2_TrackPitch);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier2_Type, this.VM_C2_Type);
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier2_Gain, (int)(100f * this.VM_C2_Gain));
		if (this.VM_C2_Note == -1)
		{
			this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier2_Frequency, (int)(100f * this.VM_C2_Freq));
		}
		else
		{
			this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier2_Note, this.VM_C2_Note);
		}
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier2_PulseWidth, (int)(100f * this.VM_C2_PulseWidth));
		this.SendParameter(OVRVoiceModContext.ovrVoiceModParams.Carrier2_CycledNoiseSize, this.VM_C1_CycledNoiseSize);
	}

	// Token: 0x04002950 RID: 10576
	public AudioSource audioSource;

	// Token: 0x04002951 RID: 10577
	public float gain = 1f;

	// Token: 0x04002952 RID: 10578
	public bool audioMute = true;

	// Token: 0x04002953 RID: 10579
	public KeyCode loopback = KeyCode.L;

	// Token: 0x04002954 RID: 10580
	private OVRVoiceModContext.VMPreset[] VMPresets = new OVRVoiceModContext.VMPreset[]
	{
		new OVRVoiceModContext.VMPreset
		{
			info = "-INIT-\nNo pitch shift, no vocode",
			color = Color.gray,
			mix = 1f,
			pitch = 1f,
			bands = 32,
			formant = 0,
			c1PTrack = 0,
			c1Type = 0,
			c1Gain = 0f,
			c1Freq = 440f,
			c1Note = -1,
			c1PW = 0.5f,
			c1CNS = 512,
			c2PTrack = 0,
			c2Type = 0,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = -1,
			c2PW = 0.5f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "FULL VOCODE\nCarrier 1: Full noise",
			color = Color.white,
			mix = 0f,
			pitch = 1f,
			bands = 32,
			formant = 0,
			c1PTrack = 0,
			c1Type = 0,
			c1Gain = 1f,
			c1Freq = 440f,
			c1Note = -1,
			c1PW = 0.5f,
			c1CNS = 512,
			c2PTrack = 0,
			c2Type = 0,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = -1,
			c2PW = 0.5f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "FULL VOCODE\nCarrier 1: Cycled noise 512",
			color = Color.blue,
			mix = 0f,
			pitch = 1f,
			bands = 32,
			formant = 0,
			c1PTrack = 0,
			c1Type = 1,
			c1Gain = 1f,
			c1Freq = 440f,
			c1Note = -1,
			c1PW = 0.5f,
			c1CNS = 512,
			c2PTrack = 0,
			c2Type = 0,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = -1,
			c2PW = 0.5f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "FULL VOCODE\nCarrier 1: Saw Up, Freq 220",
			color = Color.magenta,
			mix = 0f,
			pitch = 1f,
			bands = 32,
			formant = 0,
			c1PTrack = 0,
			c1Type = 2,
			c1Gain = 1f,
			c1Freq = 220f,
			c1Note = -1,
			c1PW = 0.5f,
			c1CNS = 512,
			c2PTrack = 0,
			c2Type = 0,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = -1,
			c2PW = 0.5f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "FULL VOCODE\nCarrier 1: Saw Up, Pitch tracked\n",
			color = Color.cyan,
			mix = 0f,
			pitch = 1f,
			bands = 32,
			formant = 0,
			c1PTrack = 1,
			c1Type = 2,
			c1Gain = 0.34f,
			c1Freq = 440f,
			c1Note = -1,
			c1PW = 0.1f,
			c1CNS = 512,
			c2PTrack = 0,
			c2Type = 0,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = -1,
			c2PW = 0.5f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "INPUT PLUS VOCODE\nInput 50%, Vocode 50%\nPitch 1.0\nCarrier 1: Full Noise,\nCarrier 2: Cycled Noise 512",
			color = Color.green,
			mix = 0.5f,
			pitch = 1f,
			bands = 32,
			formant = 0,
			c1PTrack = 0,
			c1Type = 0,
			c1Gain = 0.5f,
			c1Freq = 440f,
			c1Note = 57,
			c1PW = 0.5f,
			c1CNS = 512,
			c2PTrack = 0,
			c2Type = 1,
			c2Gain = 0.5f,
			c2Freq = 440f,
			c2Note = 45,
			c2PW = 0.25f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "INPUT PLUS VOCODE PLUS PITCH DOWN\nInput 50%, Vocode 50%\nPitch 0.75\nCarrier 1: Cycled Noise 512\nCarrier 2: Cycled Noise 768",
			color = Color.red,
			mix = 0.5f,
			pitch = 0.75f,
			bands = 32,
			formant = 0,
			c1PTrack = 0,
			c1Type = 1,
			c1Gain = 0.6f,
			c1Freq = 440f,
			c1Note = 57,
			c1PW = 0.5f,
			c1CNS = 512,
			c2PTrack = 0,
			c2Type = 3,
			c2Gain = 0.2f,
			c2Freq = 440f,
			c2Note = 40,
			c2PW = 0.25f,
			c2CNS = 768
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "PITCH ONLY\nPitch 1.25 (Formant correction)",
			color = Color.blue,
			mix = 1f,
			pitch = 1.25f,
			bands = 32,
			formant = 1,
			c1PTrack = 0,
			c1Type = 1,
			c1Gain = 1f,
			c1Freq = 440f,
			c1Note = 57,
			c1PW = 0.5f,
			c1CNS = 400,
			c2PTrack = 0,
			c2Type = 3,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = 52,
			c2PW = 0.5f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "PITCH ONLY\nPitch 0.5 (Formant correction)",
			color = Color.green,
			mix = 1f,
			pitch = 0.5f,
			bands = 32,
			formant = 1,
			c1PTrack = 0,
			c1Type = 1,
			c1Gain = 1f,
			c1Freq = 440f,
			c1Note = 57,
			c1PW = 0.5f,
			c1CNS = 400,
			c2PTrack = 0,
			c2Type = 3,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = 52,
			c2PW = 0.5f,
			c2CNS = 512
		},
		new OVRVoiceModContext.VMPreset
		{
			info = "PITCH ONLY\nPitch 2.0 (Formant correction)",
			color = Color.yellow,
			mix = 1f,
			pitch = 2f,
			bands = 32,
			formant = 1,
			c1PTrack = 0,
			c1Type = 1,
			c1Gain = 1f,
			c1Freq = 440f,
			c1Note = 57,
			c1PW = 0.5f,
			c1CNS = 400,
			c2PTrack = 0,
			c2Type = 3,
			c2Gain = 0f,
			c2Freq = 440f,
			c2Note = 52,
			c2PW = 0.5f,
			c2CNS = 512
		}
	};

	// Token: 0x04002955 RID: 10581
	public float VM_MixAudio = 1f;

	// Token: 0x04002956 RID: 10582
	public float VM_Pitch = 1f;

	// Token: 0x04002957 RID: 10583
	public int VM_Bands = 32;

	// Token: 0x04002958 RID: 10584
	public int VM_FormantCorrect;

	// Token: 0x04002959 RID: 10585
	public int VM_C1_TrackPitch;

	// Token: 0x0400295A RID: 10586
	public int VM_C1_Type;

	// Token: 0x0400295B RID: 10587
	public float VM_C1_Gain = 0.5f;

	// Token: 0x0400295C RID: 10588
	public float VM_C1_Freq = 440f;

	// Token: 0x0400295D RID: 10589
	public int VM_C1_Note = 67;

	// Token: 0x0400295E RID: 10590
	public float VM_C1_PulseWidth = 0.5f;

	// Token: 0x0400295F RID: 10591
	public int VM_C1_CycledNoiseSize = 512;

	// Token: 0x04002960 RID: 10592
	public int VM_C2_TrackPitch;

	// Token: 0x04002961 RID: 10593
	public int VM_C2_Type;

	// Token: 0x04002962 RID: 10594
	public float VM_C2_Gain = 0.5f;

	// Token: 0x04002963 RID: 10595
	public float VM_C2_Freq = 440f;

	// Token: 0x04002964 RID: 10596
	public int VM_C2_Note = 67;

	// Token: 0x04002965 RID: 10597
	public float VM_C2_PulseWidth = 0.5f;

	// Token: 0x04002966 RID: 10598
	public int VM_C2_CycledNoiseSize = 512;

	// Token: 0x04002967 RID: 10599
	private uint context;

	// Token: 0x04002968 RID: 10600
	private float prevVol;

	// Token: 0x020008BA RID: 2234
	public enum ovrVoiceModParams
	{
		// Token: 0x0400296A RID: 10602
		MixInputAudio,
		// Token: 0x0400296B RID: 10603
		PitchInputAudio,
		// Token: 0x0400296C RID: 10604
		SetBands,
		// Token: 0x0400296D RID: 10605
		FormantCorrection,
		// Token: 0x0400296E RID: 10606
		Carrier1_TrackPitch,
		// Token: 0x0400296F RID: 10607
		Carrier1_Type,
		// Token: 0x04002970 RID: 10608
		Carrier1_Gain,
		// Token: 0x04002971 RID: 10609
		Carrier1_Frequency,
		// Token: 0x04002972 RID: 10610
		Carrier1_Note,
		// Token: 0x04002973 RID: 10611
		Carrier1_PulseWidth,
		// Token: 0x04002974 RID: 10612
		Carrier1_CycledNoiseSize,
		// Token: 0x04002975 RID: 10613
		Carrier2_TrackPitch,
		// Token: 0x04002976 RID: 10614
		Carrier2_Type,
		// Token: 0x04002977 RID: 10615
		Carrier2_Gain,
		// Token: 0x04002978 RID: 10616
		Carrier2_Frequency,
		// Token: 0x04002979 RID: 10617
		Carrier2_Note,
		// Token: 0x0400297A RID: 10618
		Carrier2_PulseWidth,
		// Token: 0x0400297B RID: 10619
		Carrier2_CycledNoiseSize,
		// Token: 0x0400297C RID: 10620
		Count
	}

	// Token: 0x020008BB RID: 2235
	public struct VMPreset
	{
		// Token: 0x0400297D RID: 10621
		public string info;

		// Token: 0x0400297E RID: 10622
		public Color color;

		// Token: 0x0400297F RID: 10623
		public float mix;

		// Token: 0x04002980 RID: 10624
		public float pitch;

		// Token: 0x04002981 RID: 10625
		public int bands;

		// Token: 0x04002982 RID: 10626
		public int formant;

		// Token: 0x04002983 RID: 10627
		public int c1PTrack;

		// Token: 0x04002984 RID: 10628
		public int c1Type;

		// Token: 0x04002985 RID: 10629
		public float c1Gain;

		// Token: 0x04002986 RID: 10630
		public float c1Freq;

		// Token: 0x04002987 RID: 10631
		public int c1Note;

		// Token: 0x04002988 RID: 10632
		public float c1PW;

		// Token: 0x04002989 RID: 10633
		public int c1CNS;

		// Token: 0x0400298A RID: 10634
		public int c2PTrack;

		// Token: 0x0400298B RID: 10635
		public int c2Type;

		// Token: 0x0400298C RID: 10636
		public float c2Gain;

		// Token: 0x0400298D RID: 10637
		public float c2Freq;

		// Token: 0x0400298E RID: 10638
		public int c2Note;

		// Token: 0x0400298F RID: 10639
		public float c2PW;

		// Token: 0x04002990 RID: 10640
		public int c2CNS;
	}
}
