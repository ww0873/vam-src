using System;

namespace Battlehub.Utils
{
	// Token: 0x020002A8 RID: 680
	public class FloatAnimationInfo : AnimationInfo<object, float>
	{
		// Token: 0x06000FF9 RID: 4089 RVA: 0x0005B5C4 File Offset: 0x000599C4
		public FloatAnimationInfo(float from, float to, float duration, Func<float, float> easingFunction, AnimationCallback<object, float> callback, object target = null) : base(from, to, duration, easingFunction, callback, target)
		{
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0005B5D5 File Offset: 0x000599D5
		protected override float Lerp(float from, float to, float t)
		{
			return to * t + from * (1f - t);
		}
	}
}
