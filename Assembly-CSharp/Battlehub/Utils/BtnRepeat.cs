using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Battlehub.Utils
{
	// Token: 0x0200029C RID: 668
	public class BtnRepeat : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x06000FD9 RID: 4057 RVA: 0x0005ACF1 File Offset: 0x000590F1
		public BtnRepeat()
		{
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0005AD04 File Offset: 0x00059104
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			this.m_timeElapsed = 0f;
			this.m_repeat = true;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0005AD18 File Offset: 0x00059118
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			this.m_timeElapsed = 0f;
			this.m_repeat = false;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0005AD2C File Offset: 0x0005912C
		private void Update()
		{
			if (!this.m_repeat)
			{
				return;
			}
			this.m_timeElapsed += Time.deltaTime;
			if (this.m_timeElapsed >= this.Interval)
			{
				this.RepeatClick.Invoke();
				this.m_timeElapsed = 0f;
			}
		}

		// Token: 0x04000E4F RID: 3663
		public float Interval = 0.1f;

		// Token: 0x04000E50 RID: 3664
		private bool m_repeat;

		// Token: 0x04000E51 RID: 3665
		private float m_timeElapsed;

		// Token: 0x04000E52 RID: 3666
		public UnityEvent RepeatClick;
	}
}
