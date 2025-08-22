using System;
using UnityEngine;

namespace MeshVR.Hands
{
	// Token: 0x02000E21 RID: 3617
	public class HandInputInitializer : MonoBehaviour
	{
		// Token: 0x06006F40 RID: 28480 RVA: 0x0029D151 File Offset: 0x0029B551
		public HandInputInitializer()
		{
		}

		// Token: 0x06006F41 RID: 28481 RVA: 0x0029D15C File Offset: 0x0029B55C
		protected virtual void Awake()
		{
			HandInput[] componentsInChildren = base.GetComponentsInChildren<HandInput>(true);
			foreach (HandInput handInput in componentsInChildren)
			{
				handInput.Init();
			}
		}
	}
}
