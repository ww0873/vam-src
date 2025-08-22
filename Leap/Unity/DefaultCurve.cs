using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200071D RID: 1821
	public static class DefaultCurve
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06002C61 RID: 11361 RVA: 0x000EDE34 File Offset: 0x000EC234
		public static AnimationCurve Zero
		{
			get
			{
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.AddKey(0f, 0f);
				animationCurve.AddKey(1f, 0f);
				return animationCurve;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x000EDE6C File Offset: 0x000EC26C
		public static AnimationCurve One
		{
			get
			{
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.AddKey(0f, 1f);
				animationCurve.AddKey(1f, 1f);
				return animationCurve;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06002C63 RID: 11363 RVA: 0x000EDEA4 File Offset: 0x000EC2A4
		public static AnimationCurve LinearUp
		{
			get
			{
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.AddKey(new Keyframe(0f, 0f, 1f, 1f));
				animationCurve.AddKey(new Keyframe(1f, 1f, 1f, 1f));
				return animationCurve;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06002C64 RID: 11364 RVA: 0x000EDEF8 File Offset: 0x000EC2F8
		public static AnimationCurve LinearDown
		{
			get
			{
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.AddKey(new Keyframe(0f, 1f, -1f, -1f));
				animationCurve.AddKey(new Keyframe(1f, 0f, -1f, -1f));
				return animationCurve;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06002C65 RID: 11365 RVA: 0x000EDF4C File Offset: 0x000EC34C
		public static AnimationCurve SigmoidUp
		{
			get
			{
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.AddKey(new Keyframe(0f, 0f, 0f, 0f));
				animationCurve.AddKey(new Keyframe(1f, 1f, 0f, 0f));
				return animationCurve;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06002C66 RID: 11366 RVA: 0x000EDFA0 File Offset: 0x000EC3A0
		public static AnimationCurve SigmoidDown
		{
			get
			{
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.AddKey(new Keyframe(0f, 1f, 0f, 0f));
				animationCurve.AddKey(new Keyframe(1f, 0f, 0f, 0f));
				return animationCurve;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002C67 RID: 11367 RVA: 0x000EDFF4 File Offset: 0x000EC3F4
		public static AnimationCurve SigmoidUpDown
		{
			get
			{
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.AddKey(new Keyframe(0f, 0f, 0f, 0f));
				animationCurve.AddKey(new Keyframe(0.5f, 1f, 0f, 0f));
				animationCurve.AddKey(new Keyframe(1f, 0f, 0f, 0f));
				return animationCurve;
			}
		}
	}
}
