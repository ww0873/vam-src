using System;
using Leap.Unity;
using UnityEngine;

// Token: 0x02000C29 RID: 3113
public class LeapMotionProviderSwitcher : MonoBehaviour
{
	// Token: 0x06005A84 RID: 23172 RVA: 0x0021387E File Offset: 0x00211C7E
	public LeapMotionProviderSwitcher()
	{
	}

	// Token: 0x06005A85 RID: 23173 RVA: 0x00213888 File Offset: 0x00211C88
	private void OnEnable()
	{
		LeapXRServiceProvider component = base.GetComponent<LeapXRServiceProvider>();
		if (component != null && this.handModelManager != null)
		{
			this.handModelManager.leapProvider = component;
		}
	}

	// Token: 0x04004AC3 RID: 19139
	public HandModelManager handModelManager;
}
