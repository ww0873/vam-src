using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000C4 RID: 196
	public class Test : MonoBehaviour
	{
		// Token: 0x0600037F RID: 895 RVA: 0x00015BE7 File Offset: 0x00013FE7
		public Test()
		{
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00015BEF File Offset: 0x00013FEF
		private void Awake()
		{
			RuntimeSelection.SelectionChanged += this.OnSelectionChanged;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00015C02 File Offset: 0x00014002
		private void OnDestroy()
		{
			RuntimeSelection.SelectionChanged -= this.OnSelectionChanged;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00015C18 File Offset: 0x00014018
		private void OnSelectionChanged(UnityEngine.Object[] unselectedObjects)
		{
			UnityEngine.Object[] objects = RuntimeSelection.objects;
		}
	}
}
