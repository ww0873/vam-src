using System;
using MK.Glow;
using UnityEngine;

// Token: 0x02000C2F RID: 3119
public class MKGlowCamera : MonoBehaviour
{
	// Token: 0x06005AAF RID: 23215 RVA: 0x00214112 File Offset: 0x00212512
	public MKGlowCamera()
	{
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x0021411A File Offset: 0x0021251A
	private void OnEnable()
	{
		this.glow = base.GetComponent<MKGlow>();
		if (this.glow != null && UserPreferences.singleton != null)
		{
			UserPreferences.singleton.RegisterGlowCamera(this.glow);
		}
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x00214159 File Offset: 0x00212559
	private void OnDisable()
	{
		if (this.glow != null && UserPreferences.singleton != null)
		{
			UserPreferences.singleton.DeregisterGlowCamera(this.glow);
		}
	}

	// Token: 0x04004AE0 RID: 19168
	private MKGlow glow;
}
