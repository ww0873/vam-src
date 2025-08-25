using System;
using UnityEngine;

// Token: 0x02000E33 RID: 3635
public class VRWebBrowserUIConnector : UIConnector
{
	// Token: 0x06007019 RID: 28697 RVA: 0x002A2C60 File Offset: 0x002A1060
	public VRWebBrowserUIConnector()
	{
	}

	// Token: 0x0600701A RID: 28698 RVA: 0x002A2C68 File Offset: 0x002A1068
	public override void Connect()
	{
		Debug.LogError("VRWebBrowserUIConnector obsolete but still in use");
	}

	// Token: 0x040061CC RID: 25036
	public VRWebBrowser webBrowser;
}
