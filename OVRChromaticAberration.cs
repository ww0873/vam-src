using System;
using UnityEngine;

// Token: 0x0200095B RID: 2395
public class OVRChromaticAberration : MonoBehaviour
{
	// Token: 0x06003B69 RID: 15209 RVA: 0x0011E505 File Offset: 0x0011C905
	public OVRChromaticAberration()
	{
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x0011E518 File Offset: 0x0011C918
	private void Start()
	{
		OVRManager.instance.chromatic = this.chromatic;
	}

	// Token: 0x06003B6B RID: 15211 RVA: 0x0011E52A File Offset: 0x0011C92A
	private void Update()
	{
		if (OVRInput.GetDown(this.toggleButton, OVRInput.Controller.Active))
		{
			this.chromatic = !this.chromatic;
			OVRManager.instance.chromatic = this.chromatic;
		}
	}

	// Token: 0x04002D4F RID: 11599
	public OVRInput.RawButton toggleButton = OVRInput.RawButton.X;

	// Token: 0x04002D50 RID: 11600
	private bool chromatic;
}
