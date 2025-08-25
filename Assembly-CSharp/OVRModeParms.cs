using System;
using UnityEngine;

// Token: 0x02000968 RID: 2408
public class OVRModeParms : MonoBehaviour
{
	// Token: 0x06003C1F RID: 15391 RVA: 0x00122E04 File Offset: 0x00121204
	public OVRModeParms()
	{
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x00122E17 File Offset: 0x00121217
	private void Start()
	{
		if (!OVRManager.isHmdPresent)
		{
			base.enabled = false;
			return;
		}
		base.InvokeRepeating("TestPowerStateMode", 10f, 10f);
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x00122E40 File Offset: 0x00121240
	private void Update()
	{
		if (OVRInput.GetDown(this.resetButton, OVRInput.Controller.Active))
		{
			OVRPlugin.cpuLevel = 0;
			OVRPlugin.gpuLevel = 1;
		}
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x00122E63 File Offset: 0x00121263
	private void TestPowerStateMode()
	{
		if (OVRPlugin.powerSaving)
		{
			Debug.Log("POWER SAVE MODE ACTIVATED");
		}
	}

	// Token: 0x04002E0F RID: 11791
	public OVRInput.RawButton resetButton = OVRInput.RawButton.X;
}
