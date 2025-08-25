using System;
using UnityEngine;

// Token: 0x020008B2 RID: 2226
public class VoiceModDemo_Logic : MonoBehaviour
{
	// Token: 0x060037FA RID: 14330 RVA: 0x0010F037 File Offset: 0x0010D437
	public VoiceModDemo_Logic()
	{
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x0010F064 File Offset: 0x0010D464
	private void Start()
	{
		OVRMessenger.AddListener<OVRTouchpad.TouchEvent>("Touchpad", new OVRCallback<OVRTouchpad.TouchEvent>(this.LocalTouchEventCallback));
		this.targetSet = 0;
		this.SwitchTarget.SetActive(0);
		if (this.material != null)
		{
			this.material.SetColor("_Color", Color.grey);
		}
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x0010F0C4 File Offset: 0x0010D4C4
	private void Update()
	{
		int num = -1;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			num = 0;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			num = 1;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			num = 2;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			num = 3;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			num = 4;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			num = 5;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			num = 6;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			num = 7;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			num = 8;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			num = 9;
		}
		if (num != -1)
		{
			Color value = Color.black;
			for (int i = 0; i < this.contexts.Length; i++)
			{
				if (this.contexts[i].SetPreset(num))
				{
					value = this.contexts[i].GetPresetColor(num);
				}
			}
			if (this.material != null)
			{
				this.material.SetColor("_Color", value);
			}
		}
		this.UpdateModelScale();
		if (Input.GetKeyDown(KeyCode.Z))
		{
			this.targetSet = 0;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.X))
		{
			this.targetSet = 1;
			this.SetCurrentTarget();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x0010F248 File Offset: 0x0010D648
	private void SetCurrentTarget()
	{
		int num = this.targetSet;
		if (num != 0)
		{
			if (num == 1)
			{
				this.SwitchTarget.SetActive(1);
				OVRDebugConsole.Clear();
				OVRDebugConsole.Log("SAMPLE INPUT");
				OVRDebugConsole.ClearTimeout(1.5f);
			}
		}
		else
		{
			this.SwitchTarget.SetActive(0);
			OVRDebugConsole.Clear();
			OVRDebugConsole.Log("MICROPHONE INPUT");
			OVRDebugConsole.ClearTimeout(1.5f);
		}
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x0010F2C4 File Offset: 0x0010D6C4
	private void LocalTouchEventCallback(OVRTouchpad.TouchEvent touchEvent)
	{
		switch (touchEvent)
		{
		case OVRTouchpad.TouchEvent.Left:
			this.targetSet--;
			if (this.targetSet < 0)
			{
				this.targetSet = 1;
			}
			this.SetCurrentTarget();
			break;
		case OVRTouchpad.TouchEvent.Right:
			this.targetSet++;
			if (this.targetSet > 1)
			{
				this.targetSet = 0;
			}
			this.SetCurrentTarget();
			break;
		case OVRTouchpad.TouchEvent.Up:
			if (this.contexts.Length != 0)
			{
				if (this.contexts[0].GetNumPresets() == 0)
				{
					OVRDebugConsole.Clear();
					OVRDebugConsole.Log("NO PRESETS!");
					OVRDebugConsole.ClearTimeout(1.5f);
				}
				else
				{
					this.currentPreset++;
					if (this.currentPreset >= this.contexts[0].GetNumPresets())
					{
						this.currentPreset = 0;
					}
					Color value = Color.black;
					for (int i = 0; i < this.contexts.Length; i++)
					{
						if (this.contexts[i].SetPreset(this.currentPreset))
						{
							value = this.contexts[i].GetPresetColor(this.currentPreset);
						}
					}
					if (this.material != null)
					{
						this.material.SetColor("_Color", value);
					}
				}
			}
			break;
		case OVRTouchpad.TouchEvent.Down:
			if (this.contexts.Length != 0)
			{
				if (this.contexts[0].GetNumPresets() == 0)
				{
					OVRDebugConsole.Clear();
					OVRDebugConsole.Log("NO PRESETS!");
					OVRDebugConsole.ClearTimeout(1.5f);
				}
				else
				{
					this.currentPreset--;
					if (this.currentPreset < 0)
					{
						this.currentPreset = this.contexts[0].GetNumPresets() - 1;
					}
					Color value2 = Color.black;
					for (int j = 0; j < this.contexts.Length; j++)
					{
						if (this.contexts[j].SetPreset(this.currentPreset))
						{
							value2 = this.contexts[j].GetPresetColor(this.currentPreset);
						}
					}
					if (this.material != null)
					{
						this.material.SetColor("_Color", value2);
					}
				}
			}
			break;
		}
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x0010F500 File Offset: 0x0010D900
	private void UpdateModelScale()
	{
		for (int i = 0; i < this.xfrms.Length; i++)
		{
			if (i < this.contexts.Length)
			{
				this.xfrms[i].localScale = this.scale * (1f + this.contexts[i].GetAverageAbsVolume() * this.scaleMax);
			}
		}
	}

	// Token: 0x04002924 RID: 10532
	public OVRVoiceModContext[] contexts;

	// Token: 0x04002925 RID: 10533
	public Material material;

	// Token: 0x04002926 RID: 10534
	public Transform[] xfrms;

	// Token: 0x04002927 RID: 10535
	public EnableSwitch SwitchTarget;

	// Token: 0x04002928 RID: 10536
	private int targetSet;

	// Token: 0x04002929 RID: 10537
	private Vector3 scale = new Vector3(3f, 3f, 3f);

	// Token: 0x0400292A RID: 10538
	private float scaleMax = 10f;

	// Token: 0x0400292B RID: 10539
	private int currentPreset;
}
