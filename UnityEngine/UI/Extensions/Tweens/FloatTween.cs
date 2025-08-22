using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x020004A6 RID: 1190
	public struct FloatTween : ITweenValue
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x000AC049 File Offset: 0x000AA449
		// (set) Token: 0x06001E05 RID: 7685 RVA: 0x000AC051 File Offset: 0x000AA451
		public float startFloat
		{
			get
			{
				return this.m_StartFloat;
			}
			set
			{
				this.m_StartFloat = value;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x000AC05A File Offset: 0x000AA45A
		// (set) Token: 0x06001E07 RID: 7687 RVA: 0x000AC062 File Offset: 0x000AA462
		public float targetFloat
		{
			get
			{
				return this.m_TargetFloat;
			}
			set
			{
				this.m_TargetFloat = value;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x000AC06B File Offset: 0x000AA46B
		// (set) Token: 0x06001E09 RID: 7689 RVA: 0x000AC073 File Offset: 0x000AA473
		public float duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.m_Duration = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x000AC07C File Offset: 0x000AA47C
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x000AC084 File Offset: 0x000AA484
		public bool ignoreTimeScale
		{
			get
			{
				return this.m_IgnoreTimeScale;
			}
			set
			{
				this.m_IgnoreTimeScale = value;
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x000AC08D File Offset: 0x000AA48D
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			this.m_Target.Invoke(Mathf.Lerp(this.m_StartFloat, this.m_TargetFloat, floatPercentage));
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000AC0B8 File Offset: 0x000AA4B8
		public void AddOnChangedCallback(UnityAction<float> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new FloatTween.FloatTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000AC0DC File Offset: 0x000AA4DC
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (this.m_Finish == null)
			{
				this.m_Finish = new FloatTween.FloatFinishCallback();
			}
			this.m_Finish.AddListener(callback);
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000AC100 File Offset: 0x000AA500
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x000AC108 File Offset: 0x000AA508
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000AC110 File Offset: 0x000AA510
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000AC11E File Offset: 0x000AA51E
		public void Finished()
		{
			if (this.m_Finish != null)
			{
				this.m_Finish.Invoke();
			}
		}

		// Token: 0x04001970 RID: 6512
		private float m_StartFloat;

		// Token: 0x04001971 RID: 6513
		private float m_TargetFloat;

		// Token: 0x04001972 RID: 6514
		private float m_Duration;

		// Token: 0x04001973 RID: 6515
		private bool m_IgnoreTimeScale;

		// Token: 0x04001974 RID: 6516
		private FloatTween.FloatTweenCallback m_Target;

		// Token: 0x04001975 RID: 6517
		private FloatTween.FloatFinishCallback m_Finish;

		// Token: 0x020004A7 RID: 1191
		public class FloatTweenCallback : UnityEvent<float>
		{
			// Token: 0x06001E13 RID: 7699 RVA: 0x000AC136 File Offset: 0x000AA536
			public FloatTweenCallback()
			{
			}
		}

		// Token: 0x020004A8 RID: 1192
		public class FloatFinishCallback : UnityEvent
		{
			// Token: 0x06001E14 RID: 7700 RVA: 0x000AC13E File Offset: 0x000AA53E
			public FloatFinishCallback()
			{
			}
		}
	}
}
