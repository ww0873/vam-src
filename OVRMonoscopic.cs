using System;
using UnityEngine;

// Token: 0x02000969 RID: 2409
public class OVRMonoscopic : MonoBehaviour
{
	// Token: 0x06003C23 RID: 15395 RVA: 0x00122E79 File Offset: 0x00121279
	public OVRMonoscopic()
	{
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x00122E88 File Offset: 0x00121288
	private void Update()
	{
		if (OVRInput.GetDown(this.toggleButton, OVRInput.Controller.Active))
		{
			this.monoscopic = !this.monoscopic;
			OVRManager.instance.monoscopic = this.monoscopic;
		}
	}

	// Token: 0x04002E10 RID: 11792
	public OVRInput.RawButton toggleButton = OVRInput.RawButton.B;

	// Token: 0x04002E11 RID: 11793
	private bool monoscopic;
}
