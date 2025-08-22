using System;
using System.Runtime.CompilerServices;
using Leap.Unity.Query;
using UnityEngine;

namespace Leap.Unity.Examples
{
	// Token: 0x020005B0 RID: 1456
	public class InertiaPostProcessProvider : PostProcessProvider
	{
		// Token: 0x06002489 RID: 9353 RVA: 0x000D3685 File Offset: 0x000D1A85
		public InertiaPostProcessProvider()
		{
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000D36A4 File Offset: 0x000D1AA4
		public override void ProcessFrame(ref Frame inputFrame)
		{
			Query<Hand> query = inputFrame.Hands.Query<Hand>();
			if (InertiaPostProcessProvider.<>f__am$cache0 == null)
			{
				InertiaPostProcessProvider.<>f__am$cache0 = new Func<Hand, bool>(InertiaPostProcessProvider.<ProcessFrame>m__0);
			}
			Hand hand = query.FirstOrDefault(InertiaPostProcessProvider.<>f__am$cache0);
			Query<Hand> query2 = inputFrame.Hands.Query<Hand>();
			if (InertiaPostProcessProvider.<>f__am$cache1 == null)
			{
				InertiaPostProcessProvider.<>f__am$cache1 = new Func<Hand, bool>(InertiaPostProcessProvider.<ProcessFrame>m__1);
			}
			Hand hand2 = query2.FirstOrDefault(InertiaPostProcessProvider.<>f__am$cache1);
			if (Time.inFixedTimeStep)
			{
				this.processHand(hand, ref this._fixedLeftPose, ref this._fixedPreviousLeftPose, ref this._fixedLeftAge);
				this.processHand(hand2, ref this._fixedRightPose, ref this._fixedPreviousRightPose, ref this._fixedRightAge);
			}
			else
			{
				this.processHand(hand, ref this._leftPose, ref this._previousLeftPose, ref this._leftAge);
				this.processHand(hand2, ref this._rightPose, ref this._previousRightPose, ref this._rightAge);
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000D3784 File Offset: 0x000D1B84
		private void processHand(Hand hand, ref Pose? maybeCurPose, ref Pose? maybePrevPose, ref float handAge)
		{
			if (hand == null)
			{
				maybeCurPose = null;
				maybePrevPose = null;
				handAge = 0f;
			}
			else
			{
				Pose palmPose = hand.GetPalmPose();
				if (maybeCurPose == null)
				{
					maybePrevPose = null;
					maybeCurPose = new Pose?(palmPose);
				}
				else if (maybePrevPose == null)
				{
					maybePrevPose = maybeCurPose;
					maybeCurPose = new Pose?(palmPose);
				}
				else
				{
					float num = hand.TimeVisible - handAge;
					if (num > 0f)
					{
						handAge = hand.TimeVisible;
						Pose value = maybeCurPose.Value;
						Pose value2 = maybePrevPose.Value;
						this.integratePose(ref value, ref value2, palmPose, num);
						hand.SetPalmPose(value);
						maybeCurPose = new Pose?(value);
						maybePrevPose = new Pose?(value2);
					}
				}
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000D3878 File Offset: 0x000D1C78
		private void integratePose(ref Pose curPose, ref Pose prevPose, Pose targetPose, float deltaTime)
		{
			Pose pose = curPose.inverse * prevPose;
			pose = new Pose(-pose.position, Quaternion.Inverse(pose.rotation));
			pose = Pose.Lerp(pose, Pose.identity, this.damping * deltaTime);
			Pose pose2 = curPose;
			curPose *= pose;
			prevPose = pose2;
			curPose = Pose.Lerp(curPose, targetPose, this.stiffness * deltaTime);
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000D3906 File Offset: 0x000D1D06
		[CompilerGenerated]
		private static bool <ProcessFrame>m__0(Hand h)
		{
			return h.IsLeft;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000D390E File Offset: 0x000D1D0E
		[CompilerGenerated]
		private static bool <ProcessFrame>m__1(Hand h)
		{
			return !h.IsLeft;
		}

		// Token: 0x04001EB1 RID: 7857
		[Header("Inertia")]
		[Tooltip("Higher stiffness will keep the bouncy hand closer to the tracked hand data.")]
		[Range(0f, 10f)]
		public float stiffness = 2f;

		// Token: 0x04001EB2 RID: 7858
		[Tooltip("Higher damping will suppress more motion and reduce oscillation.")]
		[Range(0f, 10f)]
		public float damping = 2f;

		// Token: 0x04001EB3 RID: 7859
		private Pose? _leftPose;

		// Token: 0x04001EB4 RID: 7860
		private Pose? _previousLeftPose;

		// Token: 0x04001EB5 RID: 7861
		private float _leftAge;

		// Token: 0x04001EB6 RID: 7862
		private Pose? _rightPose;

		// Token: 0x04001EB7 RID: 7863
		private Pose? _previousRightPose;

		// Token: 0x04001EB8 RID: 7864
		private float _rightAge;

		// Token: 0x04001EB9 RID: 7865
		private Pose? _fixedLeftPose;

		// Token: 0x04001EBA RID: 7866
		private Pose? _fixedPreviousLeftPose;

		// Token: 0x04001EBB RID: 7867
		private float _fixedLeftAge;

		// Token: 0x04001EBC RID: 7868
		private Pose? _fixedRightPose;

		// Token: 0x04001EBD RID: 7869
		private Pose? _fixedPreviousRightPose;

		// Token: 0x04001EBE RID: 7870
		private float _fixedRightAge;

		// Token: 0x04001EBF RID: 7871
		[CompilerGenerated]
		private static Func<Hand, bool> <>f__am$cache0;

		// Token: 0x04001EC0 RID: 7872
		[CompilerGenerated]
		private static Func<Hand, bool> <>f__am$cache1;
	}
}
