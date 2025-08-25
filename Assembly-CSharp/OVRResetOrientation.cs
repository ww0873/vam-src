using System;
using UnityEngine;

// Token: 0x02000975 RID: 2421
public class OVRResetOrientation : MonoBehaviour
{
	// Token: 0x06003C6B RID: 15467 RVA: 0x00124B44 File Offset: 0x00122F44
	public OVRResetOrientation()
	{
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x00124B57 File Offset: 0x00122F57
	private void Update()
	{
		if (OVRInput.GetDown(this.resetButton, OVRInput.Controller.Active))
		{
			OVRManager.display.RecenterPose();
		}
	}

	// Token: 0x04002E50 RID: 11856
	public OVRInput.RawButton resetButton = OVRInput.RawButton.Y;
}
