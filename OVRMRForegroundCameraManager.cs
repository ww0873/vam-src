using System;
using UnityEngine;

// Token: 0x020008C2 RID: 2242
internal class OVRMRForegroundCameraManager : MonoBehaviour
{
	// Token: 0x06003866 RID: 14438 RVA: 0x0011277C File Offset: 0x00110B7C
	public OVRMRForegroundCameraManager()
	{
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x00112784 File Offset: 0x00110B84
	private void OnPreRender()
	{
		if (this.clipPlaneGameObj)
		{
			if (this.clipPlaneMaterial == null)
			{
				this.clipPlaneMaterial = this.clipPlaneGameObj.GetComponent<MeshRenderer>().material;
			}
			this.clipPlaneGameObj.GetComponent<MeshRenderer>().material.SetFloat("_Visible", 1f);
		}
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x001127E7 File Offset: 0x00110BE7
	private void OnPostRender()
	{
		if (this.clipPlaneGameObj)
		{
			this.clipPlaneGameObj.GetComponent<MeshRenderer>().material.SetFloat("_Visible", 0f);
		}
	}

	// Token: 0x040029AB RID: 10667
	public GameObject clipPlaneGameObj;

	// Token: 0x040029AC RID: 10668
	private Material clipPlaneMaterial;
}
