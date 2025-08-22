using System;
using UnityEngine;

// Token: 0x02000978 RID: 2424
public class OVRTrackedRemote : MonoBehaviour
{
	// Token: 0x06003C81 RID: 15489 RVA: 0x0012530C File Offset: 0x0012370C
	public OVRTrackedRemote()
	{
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x00125314 File Offset: 0x00123714
	private void Start()
	{
		this.m_isOculusGo = (OVRPlugin.productName == "Oculus Go");
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x0012532C File Offset: 0x0012372C
	private void Update()
	{
		bool flag = OVRInput.IsControllerConnected(this.m_controller);
		if (flag != this.m_prevControllerConnected || !this.m_prevControllerConnectedCached)
		{
			this.m_modelOculusGoController.SetActive(flag && this.m_isOculusGo);
			this.m_modelGearVrController.SetActive(flag && !this.m_isOculusGo);
			this.m_prevControllerConnected = flag;
			this.m_prevControllerConnectedCached = true;
		}
		if (!flag)
		{
			return;
		}
	}

	// Token: 0x04002E63 RID: 11875
	public GameObject m_modelGearVrController;

	// Token: 0x04002E64 RID: 11876
	public GameObject m_modelOculusGoController;

	// Token: 0x04002E65 RID: 11877
	public OVRInput.Controller m_controller;

	// Token: 0x04002E66 RID: 11878
	private bool m_isOculusGo;

	// Token: 0x04002E67 RID: 11879
	private bool m_prevControllerConnected;

	// Token: 0x04002E68 RID: 11880
	private bool m_prevControllerConnectedCached;
}
