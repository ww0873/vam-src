using System;
using UnityEngine;

// Token: 0x020007D7 RID: 2007
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncContext : OVRLipSyncContextBase
{
	// Token: 0x060032CD RID: 13005 RVA: 0x00107F6E File Offset: 0x0010636E
	public OVRLipSyncContext()
	{
	}

	// Token: 0x060032CE RID: 13006 RVA: 0x00107FAE File Offset: 0x001063AE
	private void Start()
	{
		OVRMessenger.AddListener<OVRTouchpad.TouchEvent>("Touchpad", new OVRCallback<OVRTouchpad.TouchEvent>(this.LocalTouchEventCallback));
	}

	// Token: 0x060032CF RID: 13007 RVA: 0x00107FC8 File Offset: 0x001063C8
	private void Update()
	{
		if (Input.GetKeyDown(this.loopback))
		{
			this.audioMute = !this.audioMute;
			OVRLipSyncDebugConsole.Clear();
			OVRLipSyncDebugConsole.ClearTimeout(1.5f);
			if (this.audioMute)
			{
				OVRLipSyncDebugConsole.Log("LOOPBACK MODE: ENABLED");
			}
			else
			{
				OVRLipSyncDebugConsole.Log("LOOPBACK MODE: DISABLED");
			}
		}
		else if (Input.GetKeyDown(this.debugVisemes))
		{
			this.showVisemes = !this.showVisemes;
			if (this.showVisemes)
			{
				Debug.Log("DEBUG SHOW VISEMES: ENABLED");
			}
			else
			{
				OVRLipSyncDebugConsole.Clear();
				Debug.Log("DEBUG SHOW VISEMES: DISABLED");
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.gain -= 1f;
			if (this.gain < 1f)
			{
				this.gain = 1f;
			}
			string text = "LINEAR GAIN: ";
			text += this.gain;
			OVRLipSyncDebugConsole.Clear();
			OVRLipSyncDebugConsole.Log(text);
			OVRLipSyncDebugConsole.ClearTimeout(1.5f);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.gain += 1f;
			if (this.gain > 15f)
			{
				this.gain = 15f;
			}
			string text2 = "LINEAR GAIN: ";
			text2 += this.gain;
			OVRLipSyncDebugConsole.Clear();
			OVRLipSyncDebugConsole.Log(text2);
			OVRLipSyncDebugConsole.ClearTimeout(1.5f);
		}
		this.DebugShowVisemes();
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x00108158 File Offset: 0x00106558
	private void OnAudioFilterRead(float[] data, int channels)
	{
		if (OVRLipSync.IsInitialized() != OVRLipSync.Result.Success || this.audioSource == null)
		{
			return;
		}
		for (int i = 0; i < data.Length; i++)
		{
			data[i] *= this.gain;
		}
		lock (this)
		{
			if (base.Context != 0U)
			{
				OVRLipSync.Flags flags = OVRLipSync.Flags.None;
				if (this.delayCompensate)
				{
					flags |= OVRLipSync.Flags.DelayCompensateAudio;
				}
				OVRLipSync.Frame frame = base.Frame;
				OVRLipSync.ProcessFrameInterleaved(base.Context, data, flags, frame);
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

	// Token: 0x060032D1 RID: 13009 RVA: 0x0010822C File Offset: 0x0010662C
	private void DebugShowVisemes()
	{
		if (!this.showVisemes)
		{
			return;
		}
		this.debugFrameTimer -= Time.deltaTime;
		if (this.debugFrameTimer < 0f)
		{
			this.debugFrameTimer += this.debugFrameTimeoutValue;
			this.debugFrame.CopyInput(base.Frame);
		}
		string text = string.Empty;
		for (int i = 0; i < this.debugFrame.Visemes.Length; i++)
		{
			if (i < 10)
			{
				text += "0";
			}
			text += i;
			text += ":";
			int num = (int)(50f * this.debugFrame.Visemes[i]);
			for (int j = 0; j < num; j++)
			{
				text += "*";
			}
			text += "\n";
		}
		OVRLipSyncDebugConsole.Clear();
		OVRLipSyncDebugConsole.Log(text);
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x00108328 File Offset: 0x00106728
	private void LocalTouchEventCallback(OVRTouchpad.TouchEvent touchEvent)
	{
		string text = "LINEAR GAIN: ";
		if (touchEvent != OVRTouchpad.TouchEvent.SingleTap)
		{
			if (touchEvent != OVRTouchpad.TouchEvent.Up)
			{
				if (touchEvent == OVRTouchpad.TouchEvent.Down)
				{
					this.gain -= 1f;
					if (this.gain < 1f)
					{
						this.gain = 1f;
					}
					text += this.gain;
					OVRLipSyncDebugConsole.Clear();
					OVRLipSyncDebugConsole.Log(text);
					OVRLipSyncDebugConsole.ClearTimeout(1.5f);
				}
			}
			else
			{
				this.gain += 1f;
				if (this.gain > 15f)
				{
					this.gain = 15f;
				}
				text += this.gain;
				OVRLipSyncDebugConsole.Clear();
				OVRLipSyncDebugConsole.Log(text);
				OVRLipSyncDebugConsole.ClearTimeout(1.5f);
			}
		}
		else
		{
			this.audioMute = !this.audioMute;
			OVRLipSyncDebugConsole.Clear();
			OVRLipSyncDebugConsole.ClearTimeout(1.5f);
			if (this.audioMute)
			{
				OVRLipSyncDebugConsole.Log("LOOPBACK MODE: ENABLED");
			}
			else
			{
				OVRLipSyncDebugConsole.Log("LOOPBACK MODE: DISABLED");
			}
		}
	}

	// Token: 0x040026CF RID: 9935
	public float gain = 1f;

	// Token: 0x040026D0 RID: 9936
	public bool audioMute = true;

	// Token: 0x040026D1 RID: 9937
	public KeyCode loopback = KeyCode.L;

	// Token: 0x040026D2 RID: 9938
	public KeyCode debugVisemes = KeyCode.D;

	// Token: 0x040026D3 RID: 9939
	public bool showVisemes;

	// Token: 0x040026D4 RID: 9940
	public bool delayCompensate;

	// Token: 0x040026D5 RID: 9941
	private OVRLipSync.Frame debugFrame = new OVRLipSync.Frame();

	// Token: 0x040026D6 RID: 9942
	private float debugFrameTimer;

	// Token: 0x040026D7 RID: 9943
	private float debugFrameTimeoutValue = 0.1f;
}
