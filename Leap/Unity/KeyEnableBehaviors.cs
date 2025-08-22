using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000735 RID: 1845
	public class KeyEnableBehaviors : MonoBehaviour
	{
		// Token: 0x06002CFC RID: 11516 RVA: 0x000F0AD1 File Offset: 0x000EEED1
		public KeyEnableBehaviors()
		{
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000F0AE4 File Offset: 0x000EEEE4
		private void Update()
		{
			if (this.unlockHold != KeyCode.None && !Input.GetKey(this.unlockHold))
			{
				return;
			}
			if (Input.GetKeyDown(this.toggle))
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					this.targets[i].enabled = !this.targets[i].enabled;
				}
			}
		}

		// Token: 0x040023B9 RID: 9145
		public List<Behaviour> targets;

		// Token: 0x040023BA RID: 9146
		[Header("Controls")]
		public KeyCode unlockHold;

		// Token: 0x040023BB RID: 9147
		public KeyCode toggle = KeyCode.Space;
	}
}
