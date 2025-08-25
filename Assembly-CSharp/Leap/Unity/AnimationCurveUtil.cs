using System;
using System.Runtime.CompilerServices;
using Leap.Unity.Query;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200071E RID: 1822
	public static class AnimationCurveUtil
	{
		// Token: 0x06002C68 RID: 11368 RVA: 0x000EE068 File Offset: 0x000EC468
		public static bool IsConstant(this AnimationCurve curve)
		{
			Keyframe[] keys = curve.keys;
			Keyframe keyframe = keys[0];
			foreach (Keyframe keyframe2 in keys)
			{
				if (!Mathf.Approximately(keyframe.value, keyframe2.value))
				{
					return false;
				}
				if (!Mathf.Approximately(keyframe2.inTangent, 0f) && !float.IsInfinity(keyframe2.inTangent))
				{
					return false;
				}
				if (!Mathf.Approximately(keyframe2.outTangent, 0f) && !float.IsInfinity(keyframe2.outTangent))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000EE118 File Offset: 0x000EC518
		public static bool ContainsKeyAtTime(this AnimationCurve curve, float time, float tolerance = 1E-07f)
		{
			AnimationCurveUtil.<ContainsKeyAtTime>c__AnonStorey0 <ContainsKeyAtTime>c__AnonStorey = new AnimationCurveUtil.<ContainsKeyAtTime>c__AnonStorey0();
			<ContainsKeyAtTime>c__AnonStorey.time = time;
			<ContainsKeyAtTime>c__AnonStorey.tolerance = tolerance;
			return curve.keys.Query<Keyframe>().Any(new Func<Keyframe, bool>(<ContainsKeyAtTime>c__AnonStorey.<>m__0));
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000EE158 File Offset: 0x000EC558
		public static AnimationCurve GetCropped(this AnimationCurve curve, float start, float end, bool slideToStart = true)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			Keyframe? keyframe = null;
			foreach (Keyframe value in curve.keys)
			{
				if (value.time >= start)
				{
					break;
				}
				keyframe = new Keyframe?(value);
			}
			Keyframe[] keys;
			int num = keys.Length;
			while (num-- != 0)
			{
				if (keys[num].time < start || keys[num].time > end)
				{
					curve.RemoveKey(num);
				}
			}
			bool flag = false;
			foreach (Keyframe key in keys)
			{
				if (key.time >= start && key.time <= end)
				{
					if (slideToStart)
					{
						key.time -= start;
					}
					if (Mathf.Approximately(key.time, 0f))
					{
						flag = true;
					}
					animationCurve.AddKey(key);
				}
			}
			if (keyframe != null && !flag)
			{
				Keyframe value2 = keyframe.Value;
				value2.time = 0f;
				animationCurve.AddKey(value2);
			}
			return animationCurve;
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000EE2B0 File Offset: 0x000EC6B0
		public static void AddBooleanKey(this AnimationCurve curve, float time, bool value)
		{
			Keyframe key = new Keyframe
			{
				time = time,
				value = (float)((!value) ? 0 : 1)
			};
			curve.AddKey(key);
		}

		// Token: 0x02000FAC RID: 4012
		[CompilerGenerated]
		private sealed class <ContainsKeyAtTime>c__AnonStorey0
		{
			// Token: 0x060074C2 RID: 29890 RVA: 0x000EE2EC File Offset: 0x000EC6EC
			public <ContainsKeyAtTime>c__AnonStorey0()
			{
			}

			// Token: 0x060074C3 RID: 29891 RVA: 0x000EE2F4 File Offset: 0x000EC6F4
			internal bool <>m__0(Keyframe k)
			{
				return Mathf.Abs(k.time - this.time) < this.tolerance;
			}

			// Token: 0x040068D6 RID: 26838
			internal float time;

			// Token: 0x040068D7 RID: 26839
			internal float tolerance;
		}
	}
}
