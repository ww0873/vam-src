using System;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F2 RID: 242
	public class GameFerryButton : MonoBehaviour
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x0001DA7A File Offset: 0x0001BE7A
		public GameFerryButton()
		{
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001DA84 File Offset: 0x0001BE84
		private void Start()
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				GameFerry componentInChildren = parent.GetComponentInChildren<GameFerry>();
				if (componentInChildren != null)
				{
					this.m_ferryAnimator = componentInChildren.GetComponent<Animator>();
				}
				parent = parent.parent;
			}
			this.m_buttonAnimator = base.GetComponent<Animator>();
			this.m_forwardBool = Animator.StringToHash("Forward");
			this.m_pushBool = Animator.StringToHash("Push");
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001DB00 File Offset: 0x0001BF00
		private void OnTriggerEnter(Collider c)
		{
			if (this.m_ferryAnimator != null)
			{
				this.m_ferryAnimator.SetBool(this.m_forwardBool, true);
			}
			if (this.m_buttonAnimator != null)
			{
				this.m_buttonAnimator.SetBool(this.m_pushBool, true);
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001DB54 File Offset: 0x0001BF54
		private void OnTriggerExit(Collider c)
		{
			if (this.m_ferryAnimator != null)
			{
				this.m_ferryAnimator.SetBool(this.m_forwardBool, false);
			}
			if (this.m_buttonAnimator != null)
			{
				this.m_buttonAnimator.SetBool(this.m_pushBool, false);
			}
		}

		// Token: 0x040004E6 RID: 1254
		private Animator m_ferryAnimator;

		// Token: 0x040004E7 RID: 1255
		private Animator m_buttonAnimator;

		// Token: 0x040004E8 RID: 1256
		private int m_forwardBool;

		// Token: 0x040004E9 RID: 1257
		private int m_pushBool;
	}
}
