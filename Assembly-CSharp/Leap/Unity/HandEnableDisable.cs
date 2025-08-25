using System;

namespace Leap.Unity
{
	// Token: 0x020006E5 RID: 1765
	public class HandEnableDisable : HandTransitionBehavior
	{
		// Token: 0x06002AAE RID: 10926 RVA: 0x000E6C95 File Offset: 0x000E5095
		public HandEnableDisable()
		{
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000E6C9D File Offset: 0x000E509D
		protected override void Awake()
		{
			base.Awake();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000E6CB1 File Offset: 0x000E50B1
		protected override void HandReset()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x000E6CBF File Offset: 0x000E50BF
		protected override void HandFinish()
		{
			base.gameObject.SetActive(false);
		}
	}
}
