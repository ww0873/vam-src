using System;
using UnityEngine;

namespace MeshedVR
{
	// Token: 0x02000C77 RID: 3191
	internal class SuperControllerControl : MonoBehaviour
	{
		// Token: 0x06005F66 RID: 24422 RVA: 0x002408BB File Offset: 0x0023ECBB
		public SuperControllerControl()
		{
		}

		// Token: 0x06005F67 RID: 24423 RVA: 0x002408C3 File Offset: 0x0023ECC3
		public void SetToLastUI()
		{
			if (SuperController.singleton != null)
			{
				SuperController.singleton.SetToLastActiveUI();
			}
		}
	}
}
