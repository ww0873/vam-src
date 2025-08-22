using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000736 RID: 1846
	public class KeyEnableGameObjects : MonoBehaviour
	{
		// Token: 0x06002CFE RID: 11518 RVA: 0x000F0B5E File Offset: 0x000EEF5E
		public KeyEnableGameObjects()
		{
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000F0B7C File Offset: 0x000EEF7C
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
					this.targets[i].SetActive(!this.targets[i].activeSelf);
				}
			}
		}

		// Token: 0x040023BC RID: 9148
		public List<GameObject> targets;

		// Token: 0x040023BD RID: 9149
		[Header("Controls")]
		public KeyCode unlockHold = KeyCode.RightShift;

		// Token: 0x040023BE RID: 9150
		public KeyCode toggle = KeyCode.T;
	}
}
