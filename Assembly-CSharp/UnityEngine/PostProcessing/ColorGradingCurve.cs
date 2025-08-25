﻿using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000091 RID: 145
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x0600020F RID: 527 RVA: 0x0000FC30 File Offset: 0x0000E030
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000FC5C File Offset: 0x0000E05C
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe key = this.curve[length - 1];
			key.time -= this.m_Range;
			Keyframe key2 = this.curve[0];
			key2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(key);
			this.m_InternalLoopingCurve.AddKey(key2);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000FD14 File Offset: 0x0000E114
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x0400030B RID: 779
		public AnimationCurve curve;

		// Token: 0x0400030C RID: 780
		[SerializeField]
		private bool m_Loop;

		// Token: 0x0400030D RID: 781
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x0400030E RID: 782
		[SerializeField]
		private float m_Range;

		// Token: 0x0400030F RID: 783
		private AnimationCurve m_InternalLoopingCurve;
	}
}
