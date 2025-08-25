using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002AA RID: 682
	public abstract class AnimationInfo<TObj, TValue> : IAnimationInfo
	{
		// Token: 0x06000FFF RID: 4095 RVA: 0x0005B218 File Offset: 0x00059618
		public AnimationInfo(TValue from, TValue to, float duration, Func<float, float> easingFunction, AnimationCallback<TObj, TValue> callback, TObj target)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (easingFunction == null)
			{
				throw new ArgumentNullException("easingFunction");
			}
			this.m_target = target;
			this.m_from = from;
			this.m_to = to;
			this.m_duration = duration;
			this.m_callback = callback;
			this.m_easingFunction = easingFunction;
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0005B27C File Offset: 0x0005967C
		public static float EaseLinear(float t)
		{
			return t;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0005B27F File Offset: 0x0005967F
		public static float EaseInQuad(float t)
		{
			return t * t;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0005B284 File Offset: 0x00059684
		public static float EaseOutQuad(float t)
		{
			return t * (2f - t);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0005B28F File Offset: 0x0005968F
		public static float EaseInOutQuad(float t)
		{
			return ((double)t >= 0.5) ? (-1f + (4f - 2f * t) * t) : (2f * t * t);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0005B2C4 File Offset: 0x000596C4
		public static float EaseInCubic(float t)
		{
			return t * t * t;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0005B2CB File Offset: 0x000596CB
		public static float EaseOutCubic(float t)
		{
			return (t -= 1f) * t * t + 1f;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0005B2E4 File Offset: 0x000596E4
		public static float EaseInOutCubic(float t)
		{
			return ((double)t >= 0.5) ? ((t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f) : (4f * t * t * t);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0005B33A File Offset: 0x0005973A
		public static float EaseInQuart(float t)
		{
			return t * t * t * t;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0005B343 File Offset: 0x00059743
		public static float EaseOutQuart(float t)
		{
			return 1f - (t -= 1f) * t * t * t;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0005B35B File Offset: 0x0005975B
		public static float EaseInOutQuart(float t)
		{
			return ((double)t >= 0.5) ? (1f - 8f * (t -= 1f) * t * t * t) : (8f * t * t * t * t);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0005B39B File Offset: 0x0005979B
		public static float ElasticEaseIn(float t)
		{
			return Mathf.Sin(81.68141f * t) * Mathf.Pow(2f, 10f * (t - 1f));
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0005B3C1 File Offset: 0x000597C1
		public static float ElasticEaseOut(float t)
		{
			return Mathf.Sin(-81.68141f * (t + 1f)) * Mathf.Pow(2f, -10f * t) + 1f;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0005B3F0 File Offset: 0x000597F0
		public static float ElasticEaseInOut(float t)
		{
			if ((double)t < 0.5)
			{
				return 0.5f * Mathf.Sin(81.68141f * (2f * t)) * Mathf.Pow(2f, 10f * (2f * t - 1f));
			}
			return 0.5f * (Mathf.Sin(-81.68141f * (2f * t - 1f + 1f)) * Mathf.Pow(2f, -10f * (2f * t - 1f)) + 2f);
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x0005B48C File Offset: 0x0005988C
		float IAnimationInfo.Duration
		{
			get
			{
				return this.m_duration;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x0005B494 File Offset: 0x00059894
		// (set) Token: 0x0600100F RID: 4111 RVA: 0x0005B49C File Offset: 0x0005989C
		float IAnimationInfo.T
		{
			get
			{
				return this.m_t;
			}
			set
			{
				this.m_t = value;
				if (this.m_t < 0f)
				{
					this.m_t = 0f;
				}
				if (!float.IsNaN(this.m_t))
				{
					bool flag = this.m_t >= this.m_duration;
					float t = (!flag) ? this.m_easingFunction(this.m_t / this.m_duration) : 1f;
					TValue value2 = this.Lerp(this.m_from, this.m_to, t);
					this.m_callback(this.m_target, value2, this.m_t, flag);
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x0005B543 File Offset: 0x00059943
		public bool InProgress
		{
			get
			{
				return this.m_t > 0f && this.m_t < this.m_duration;
			}
		}

		// Token: 0x06001011 RID: 4113
		protected abstract TValue Lerp(TValue from, TValue to, float t);

		// Token: 0x06001012 RID: 4114 RVA: 0x0005B566 File Offset: 0x00059966
		public void Abort()
		{
			this.m_t = float.NaN;
		}

		// Token: 0x04000E58 RID: 3672
		private float m_duration;

		// Token: 0x04000E59 RID: 3673
		private float m_t;

		// Token: 0x04000E5A RID: 3674
		private TObj m_target;

		// Token: 0x04000E5B RID: 3675
		private TValue m_from;

		// Token: 0x04000E5C RID: 3676
		private TValue m_to;

		// Token: 0x04000E5D RID: 3677
		private AnimationCallback<TObj, TValue> m_callback;

		// Token: 0x04000E5E RID: 3678
		private Func<float, float> m_easingFunction;
	}
}
