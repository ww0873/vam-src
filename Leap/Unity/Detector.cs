using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leap.Unity
{
	// Token: 0x020006CD RID: 1741
	public class Detector : MonoBehaviour
	{
		// Token: 0x060029F6 RID: 10742 RVA: 0x000E269D File Offset: 0x000E0A9D
		public Detector()
		{
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000E26DC File Offset: 0x000E0ADC
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x000E26E4 File Offset: 0x000E0AE4
		public virtual void Activate()
		{
			if (!this.IsActive)
			{
				this._isActive = true;
				this.OnActivate.Invoke();
			}
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x000E2703 File Offset: 0x000E0B03
		public virtual void Deactivate()
		{
			if (this.IsActive)
			{
				this._isActive = false;
				this.OnDeactivate.Invoke();
			}
		}

		// Token: 0x0400221C RID: 8732
		private bool _isActive;

		// Token: 0x0400221D RID: 8733
		[Tooltip("Dispatched when condition is detected.")]
		public UnityEvent OnActivate;

		// Token: 0x0400221E RID: 8734
		[Tooltip("Dispatched when condition is no longer detected.")]
		public UnityEvent OnDeactivate;

		// Token: 0x0400221F RID: 8735
		protected Color OnColor = Color.green;

		// Token: 0x04002220 RID: 8736
		protected Color OffColor = Color.red;

		// Token: 0x04002221 RID: 8737
		protected Color LimitColor = Color.blue;

		// Token: 0x04002222 RID: 8738
		protected Color DirectionColor = Color.white;

		// Token: 0x04002223 RID: 8739
		protected Color NormalColor = Color.gray;
	}
}
