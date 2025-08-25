using System;
using UnityEngine;

// Token: 0x020007C2 RID: 1986
public class LipSyncDemo_SetCurrentTarget : MonoBehaviour
{
	// Token: 0x0600326A RID: 12906 RVA: 0x00106E36 File Offset: 0x00105236
	public LipSyncDemo_SetCurrentTarget()
	{
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x00106E3E File Offset: 0x0010523E
	private void Start()
	{
		OVRMessenger.AddListener<OVRTouchpad.TouchEvent>("Touchpad", new OVRCallback<OVRTouchpad.TouchEvent>(this.LocalTouchEventCallback));
		this.targetSet = 0;
		this.SwitchTargets[0].SetActive(0);
		this.SwitchTargets[1].SetActive(0);
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x00106E7C File Offset: 0x0010527C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.targetSet = 0;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.targetSet = 1;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.targetSet = 2;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.targetSet = 3;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.targetSet = 4;
			this.SetCurrentTarget();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			this.targetSet = 5;
			this.SetCurrentTarget();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x00106F4C File Offset: 0x0010534C
	private void SetCurrentTarget()
	{
		switch (this.targetSet)
		{
		case 0:
			this.SwitchTargets[0].SetActive(0);
			this.SwitchTargets[1].SetActive(0);
			break;
		case 1:
			this.SwitchTargets[0].SetActive(0);
			this.SwitchTargets[1].SetActive(1);
			break;
		case 2:
			this.SwitchTargets[0].SetActive(1);
			this.SwitchTargets[1].SetActive(2);
			break;
		case 3:
			this.SwitchTargets[0].SetActive(1);
			this.SwitchTargets[1].SetActive(3);
			break;
		case 4:
			this.SwitchTargets[0].SetActive(2);
			this.SwitchTargets[1].SetActive(4);
			break;
		case 5:
			this.SwitchTargets[0].SetActive(2);
			this.SwitchTargets[1].SetActive(5);
			break;
		}
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x00107058 File Offset: 0x00105458
	private void LocalTouchEventCallback(OVRTouchpad.TouchEvent touchEvent)
	{
		if (touchEvent != OVRTouchpad.TouchEvent.Left)
		{
			if (touchEvent == OVRTouchpad.TouchEvent.Right)
			{
				this.targetSet++;
				if (this.targetSet > 3)
				{
					this.targetSet = 0;
				}
				this.SetCurrentTarget();
			}
		}
		else
		{
			this.targetSet--;
			if (this.targetSet < 0)
			{
				this.targetSet = 3;
			}
			this.SetCurrentTarget();
		}
	}

	// Token: 0x0400268D RID: 9869
	public EnableSwitch[] SwitchTargets;

	// Token: 0x0400268E RID: 9870
	private int targetSet;
}
