using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002A2 RID: 674
	public class PsAutoDestroy : MonoBehaviour
	{
		// Token: 0x06000FE9 RID: 4073 RVA: 0x0005B0F4 File Offset: 0x000594F4
		public PsAutoDestroy()
		{
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0005B0FC File Offset: 0x000594FC
		public void Start()
		{
			this.ps = base.GetComponent<ParticleSystem>();
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0005B10A File Offset: 0x0005950A
		public void Update()
		{
			if (this.ps && !this.ps.IsAlive())
			{
				UnityEngine.Object.Destroy(base.gameObject, 0f);
			}
		}

		// Token: 0x04000E55 RID: 3669
		private ParticleSystem ps;
	}
}
