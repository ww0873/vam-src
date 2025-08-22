using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002A5 RID: 677
	public class QuaternionAnimationInfo : AnimationInfo<object, Quaternion>
	{
		// Token: 0x06000FF3 RID: 4083 RVA: 0x0005B573 File Offset: 0x00059973
		public QuaternionAnimationInfo(Quaternion from, Quaternion to, float duration, Func<float, float> easingFunction, AnimationCallback<object, Quaternion> callback, object target = null) : base(from, to, duration, easingFunction, callback, target)
		{
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0005B584 File Offset: 0x00059984
		protected override Quaternion Lerp(Quaternion from, Quaternion to, float t)
		{
			return Quaternion.Lerp(from, to, t);
		}
	}
}
