using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002A6 RID: 678
	public class Vector3SlerpAnimationInfo : AnimationInfo<object, Vector3>
	{
		// Token: 0x06000FF5 RID: 4085 RVA: 0x0005B58E File Offset: 0x0005998E
		public Vector3SlerpAnimationInfo(Vector3 from, Vector3 to, float duration, Func<float, float> easingFunction, AnimationCallback<object, Vector3> callback, object target = null) : base(from, to, duration, easingFunction, callback, target)
		{
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0005B59F File Offset: 0x0005999F
		protected override Vector3 Lerp(Vector3 from, Vector3 to, float t)
		{
			return Vector3.Slerp(from, to, t);
		}
	}
}
