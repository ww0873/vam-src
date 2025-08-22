using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003E5 RID: 997
	[RequireComponent(typeof(Animator))]
	[DisallowMultipleComponent]
	public class ObiAnimatorController : MonoBehaviour
	{
		// Token: 0x06001935 RID: 6453 RVA: 0x0008B91F File Offset: 0x00089D1F
		public ObiAnimatorController()
		{
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0008B927 File Offset: 0x00089D27
		public void Awake()
		{
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0008B938 File Offset: 0x00089D38
		public void UpdateAnimation()
		{
			if (this.animator != null && this.animator.enabled && !this.updatedThisStep)
			{
				this.animator.playableGraph.Stop();
				this.animator.Update(Time.fixedDeltaTime);
				this.updatedThisStep = true;
			}
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0008B99B File Offset: 0x00089D9B
		public void ResetUpdateFlag()
		{
			this.updatedThisStep = false;
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0008B9A4 File Offset: 0x00089DA4
		public void ResumeAutonomousUpdate()
		{
			if (this.animator != null && this.animator.enabled)
			{
				this.animator.playableGraph.Play();
			}
		}

		// Token: 0x04001494 RID: 5268
		private bool updatedThisStep;

		// Token: 0x04001495 RID: 5269
		private Animator animator;
	}
}
