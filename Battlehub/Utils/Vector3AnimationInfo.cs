using System;
using UnityEngine;

namespace Battlehub.Utils
{
	// Token: 0x020002A7 RID: 679
	public class Vector3AnimationInfo : AnimationInfo<object, Vector3>
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0005B5A9 File Offset: 0x000599A9
		public Vector3AnimationInfo(Vector3 from, Vector3 to, float duration, Func<float, float> easingFunction, AnimationCallback<object, Vector3> callback, object target = null) : base(from, to, duration, easingFunction, callback, target)
		{
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0005B5BA File Offset: 0x000599BA
		protected override Vector3 Lerp(Vector3 from, Vector3 to, float t)
		{
			return Vector3.Lerp(from, to, t);
		}
	}
}
