using System;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F1 RID: 241
	public class GameFerryBehavior : MonoBehaviour
	{
		// Token: 0x0600054F RID: 1359 RVA: 0x0001D9B5 File Offset: 0x0001BDB5
		public GameFerryBehavior()
		{
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001D9C0 File Offset: 0x0001BDC0
		private void Start()
		{
			this.m_animator = base.GetComponent<Animator>();
			this.m_shortNameHash = this.m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
			this.m_ferry = this.m_animator.GetComponent<GameFerry>();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001DA04 File Offset: 0x0001BE04
		private void Update()
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(0);
			if (this.m_shortNameHash != currentAnimatorStateInfo.shortNameHash)
			{
				if (!currentAnimatorStateInfo.IsName("IdleForward") && !currentAnimatorStateInfo.IsName("IdleBackward"))
				{
					this.m_ferry.Lock();
				}
				else
				{
					this.m_ferry.Unlock();
				}
				this.m_shortNameHash = currentAnimatorStateInfo.shortNameHash;
			}
		}

		// Token: 0x040004E3 RID: 1251
		private Animator m_animator;

		// Token: 0x040004E4 RID: 1252
		private GameFerry m_ferry;

		// Token: 0x040004E5 RID: 1253
		private int m_shortNameHash;
	}
}
