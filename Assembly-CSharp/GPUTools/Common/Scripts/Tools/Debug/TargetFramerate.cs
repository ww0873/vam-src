using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
	// Token: 0x020009C8 RID: 2504
	public class TargetFramerate : MonoBehaviour
	{
		// Token: 0x06003F37 RID: 16183 RVA: 0x0012EBC3 File Offset: 0x0012CFC3
		public TargetFramerate()
		{
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0012EBCB File Offset: 0x0012CFCB
		private void Start()
		{
			Application.targetFrameRate = this.targetFrameRate;
		}

		// Token: 0x04002FF7 RID: 12279
		[SerializeField]
		private int targetFrameRate;
	}
}
